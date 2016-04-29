Class ircCommand

    Public Output As System.IO.TextWriter
    Public Sender As String
    Public MessageType As String
    Public Message As String
    Public Username As String
    Public Channel As String

    Public Sub New(ByVal out As System.IO.TextWriter, ByVal sndr As String, ByVal msgType As String, ByVal msg As String, ByVal usr As String, ByVal chnl As String)
        Output = out
        Sender = sndr
        Username = usr
        Channel = chnl
        MessageType = msgType
        Message = msg
    End Sub


End Class
