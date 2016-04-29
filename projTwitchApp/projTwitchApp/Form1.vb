Imports System.ComponentModel
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices

Public Class frmMain

#Region "CueText"
    Private Const EM_SETCUEBANNER As Integer = &H1501
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal lParam As String) As Int32
    End Function
    Private Sub SetCueText(ByVal control As Control, ByVal text As String)
        SendMessage(control.Handle, EM_SETCUEBANNER, 0, text)
    End Sub
#End Region

#Region "Textbox"
    Private Delegate Sub AppendTextBoxTextDelegate(ByVal TextBox As TextBox, ByVal Text As String)
    Private Sub AppendTextBoxText(ByVal TextBox As TextBox, ByVal Text As String)
        If Me.InvokeRequired Then
            Me.Invoke(New AppendTextBoxTextDelegate(AddressOf AppendTextBoxText), TextBox, Text)
        Else
            TextBox.AppendText(String.Format("[{0}] - {1}{2}", DateTime.Now, Text, vbNewLine))
        End If
    End Sub
#End Region

#Region "IRC Message"
    Private Sub SendIrcMessage(ByVal outputStream As System.IO.TextWriter, ByVal Message As String)
        outputStream.WriteLine(Message)
        outputStream.Flush()
    End Sub
    Private Sub SendTwitchMessage(ByVal outputStream As System.IO.TextWriter, ByVal Username As String, ByVal Channel As String, ByVal Message As String)
        SendIrcMessage(outputStream, ":"c & Username & "!"c & Username & "@"c & Username & ".tmi.twitch.tv PRIVMSG " & Channel & " :" & Message)
    End Sub
    Private Sub SendTwitchWhisper(ByVal outputStream As System.IO.TextWriter, ByVal Username As String, ByVal Message As String)
        SendIrcMessage(outputStream, "PRIVMSG #jtv :/w " & Username & " "c & Message)
    End Sub
#End Region

#Region "CommandHandler"
    Dim CommandList As New List(Of ircCommand)
    Private Sub CommandWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles CommandWorker.DoWork
        While CommandList.Count > 0
            HandleCommand(CommandList(0))
            CommandList.RemoveAt(0)
            System.Threading.Thread.Sleep(1200)
        End While
    End Sub
    Private Sub CommandChecker()
        While True
            If Not CommandWorker.IsBusy AndAlso CommandList.Count > 0 Then
                CommandWorker.RunWorkerAsync()
            End If
            System.Threading.Thread.Sleep(1000)
        End While
    End Sub
    Private Sub HandleCommand(ByVal Command As ircCommand)
        Select Case Command.MessageType
            Case "PRIVMSG"
                SendTwitchMessage(Command.Output, Command.Username, Command.Channel, Command.Message)
            Case "WHISPER"
                SendTwitchWhisper(Command.Output, Command.Username, Command.Message)
        End Select
    End Sub
#End Region

#Region "Connection"
    Private Sub Connect(ByVal Whisper As Boolean, ByVal Username As String, ByVal OAuth As String, ByVal Channel As String)
        Dim Buffer As String = String.Empty
        Dim Socket As New System.Net.Sockets.TcpClient()
        Dim Input As System.IO.TextReader
        Dim Output As System.IO.TextWriter

        Select Case Whisper
            Case True
                Socket.Connect("irc.twitch.tv", 6667)
            Case False
                Socket.Connect("irc.chat.twitch.tv", 6667)
        End Select


        If Not Socket.Connected Then
            AppendTextBoxText(txtBuffer, "Failed to connect!")
            Return
        End If


        Input = New System.IO.StreamReader(Socket.GetStream())
        Output = New System.IO.StreamWriter(Socket.GetStream())

        Output.Write("PASS " & OAuth & vbCr & vbLf & "NICK " & Username & vbCr & vbLf)
        Output.Flush()


        Select Case Whisper
            Case True
                Output.Write("CAP REQ :twitch.tv/commands" & vbCr & vbLf)
                Output.Flush()
            Case False
                Output.Write("CAP REQ :twitch.tv/tags" & vbCr & vbLf) 'ENABLES mod=, sub=, etc.
                Output.Flush()
                Output.Write("CAP REQ :twitch.tv/membership" & vbCr & vbLf) 'ENABLES JOIN/PARTS
                Output.Flush()
        End Select



        While True

            Select Case Whisper
                Case True
                    If Regex.IsMatch(Buffer, ":tmi\.twitch\.tv 001 .*? :Welcome, GLHF!") Then
                        AppendTextBoxText(txtBuffer, "You have connected to irc.chat.twitch.tv!")
                    End If
                Case False
                    If Regex.IsMatch(Buffer, ":tmi\.twitch\.tv 001 .*? :Welcome, GLHF!") Then
                        AppendTextBoxText(txtBuffer, "You have connected to irc.twitch.tv!")
                    End If
            End Select



            Buffer = Input.ReadLine()


            Select Case Whisper
                Case True
                    HandleWhisper(Buffer)
                Case False
                    HandleMessage(Buffer)
            End Select



            If Not Whisper Then
                If Buffer.Contains("End of /NAMES list") Then
                    SendTwitchMessage(Output, Username, "#"c & Channel, "The_Wise_Old_BOT has loaded! Type: !commands to get a list of commands.")
                End If
                If Buffer.Split(" "c)(1) = "001" Then
                    Output.Write("MODE " & Username & " +B" & vbCr & vbLf & "JOIN #" & Channel & vbCr & vbLf)
                    Output.Flush()
                End If
            End If
            If Buffer.StartsWith("PING ") Then
                Output.Write(Buffer.Replace("PING", "PONG") & vbCr & vbLf)
                Output.Flush()
            End If


        End While
    End Sub
#End Region

#Region "HandleData"
    Private Sub HandleWhisper(ByVal Message As String)
        Dim R As New Regex(":(.*?)!(.*?)@.*?\.tmi\.twitch\.tv (.*?) (.*?) :(.*)")
        Dim M As MatchCollection = R.Matches(Message)
        For Each Match As Match In M
            Dim Username As String = Match.Groups(1).Value
            Dim Header As String = Match.Groups(3).Value
            Dim Footer As String = Match.Groups(5).Value
            AppendTextBoxText(txtBuffer, "[WHISPER] " & Username & ": " & Footer)
        Next
    End Sub

    Private Sub HandleMessage(ByVal Buffer As String)
        Dim R As New Regex("@badges=(.*?);color=(.*?);display-name=(.*?);emotes=(.*?);mod=(.*?);room-id=(.*?);subscriber=(.*?);turbo=(.*?);user-id=(.*?);user-type=(.*?) :(.*?)!.*?@.*?\.tmi\.twitch\.tv PRIVMSG #(.*?) :(.*)")
        Dim M As MatchCollection = R.Matches(Buffer)
        For Each Match As Match In M
            Dim DisplayName As String = Match.Groups(3).Value
            Dim Username As String = Match.Groups(11).Value
            Dim Message As String = Match.Groups(13).Value
            If Not DisplayName.Equals(String.Empty) Then
                Username = DisplayName
            End If
            AppendTextBoxText(txtBuffer, Username & ": " & Message)
            Exit Sub
        Next
    End Sub
#End Region

#Region "From Events"
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetCueText(txtUsername, "Enter Username")
        SetCueText(txtOAuth, "Enter Password")
        SetCueText(txtChannel, "Enter Channel")
        txtUsername.Text = My.Settings.Username
        txtOAuth.Text = My.Settings.OAuth
        txtChannel.Text = My.Settings.Channel
        Dim cmdCheck As New System.Threading.Thread(AddressOf CommandChecker)
        With cmdCheck
            .IsBackground = True
            .Start()
        End With
    End Sub
    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.Username = txtUsername.Text
        My.Settings.OAuth = txtOAuth.Text
        My.Settings.Channel = txtChannel.Text
        My.Settings.Save()
        End
    End Sub
    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Dim T As New System.Threading.Thread(Sub() Connect(False, txtUsername.Text, txtOAuth.Text, txtChannel.Text))
        With T
            .IsBackground = True
            .Start()
        End With
        Dim T2 As New System.Threading.Thread(Sub() Connect(True, txtUsername.Text, txtOAuth.Text, String.Empty))
        With T2
            .IsBackground = True
            .Start()
        End With
    End Sub
#End Region



End Class
