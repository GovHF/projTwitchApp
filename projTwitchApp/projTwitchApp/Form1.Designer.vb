<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.CommandWorker = New System.ComponentModel.BackgroundWorker()
        Me.txtBuffer = New System.Windows.Forms.TextBox()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtOAuth = New System.Windows.Forms.TextBox()
        Me.txtChannel = New System.Windows.Forms.TextBox()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'CommandWorker
        '
        '
        'txtBuffer
        '
        Me.txtBuffer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBuffer.Location = New System.Drawing.Point(12, 38)
        Me.txtBuffer.Multiline = True
        Me.txtBuffer.Name = "txtBuffer"
        Me.txtBuffer.Size = New System.Drawing.Size(804, 596)
        Me.txtBuffer.TabIndex = 0
        '
        'txtUsername
        '
        Me.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUsername.Location = New System.Drawing.Point(12, 9)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(196, 20)
        Me.txtUsername.TabIndex = 1
        '
        'txtOAuth
        '
        Me.txtOAuth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtOAuth.Location = New System.Drawing.Point(214, 9)
        Me.txtOAuth.Name = "txtOAuth"
        Me.txtOAuth.Size = New System.Drawing.Size(196, 20)
        Me.txtOAuth.TabIndex = 2
        '
        'txtChannel
        '
        Me.txtChannel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtChannel.Location = New System.Drawing.Point(416, 9)
        Me.txtChannel.Name = "txtChannel"
        Me.txtChannel.Size = New System.Drawing.Size(196, 20)
        Me.txtChannel.TabIndex = 3
        '
        'btnConnect
        '
        Me.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConnect.Location = New System.Drawing.Point(618, 6)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(198, 23)
        Me.btnConnect.TabIndex = 4
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(828, 646)
        Me.Controls.Add(Me.btnConnect)
        Me.Controls.Add(Me.txtChannel)
        Me.Controls.Add(Me.txtOAuth)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.txtBuffer)
        Me.Name = "frmMain"
        Me.Text = "Twitch Client"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CommandWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents txtBuffer As System.Windows.Forms.TextBox
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtOAuth As System.Windows.Forms.TextBox
    Friend WithEvents txtChannel As System.Windows.Forms.TextBox
    Friend WithEvents btnConnect As System.Windows.Forms.Button

End Class
