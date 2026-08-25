Imports Microsoft.VisualBasic
Imports System
Partial Public Class PdfEngineDialog
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (Not components Is Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me._gbOptions = New System.Windows.Forms.GroupBox()
        Me._rbContinue = New System.Windows.Forms.RadioButton()
        Me._rbCancel = New System.Windows.Forms.RadioButton()
        Me._lblLine1 = New System.Windows.Forms.Label()
        Me._lbEngine = New System.Windows.Forms.LinkLabel()
        Me._lblLine2 = New System.Windows.Forms.Label()
        Me._btnOk = New System.Windows.Forms.Button()
        Me._gbOptions.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' _gbOptions
        ' 
        Me._gbOptions.Controls.Add(Me._rbContinue)
        Me._gbOptions.Controls.Add(Me._rbCancel)
        Me._gbOptions.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me._gbOptions.Location = New System.Drawing.Point(12, 106)
        Me._gbOptions.Name = "_gbOptions"
        Me._gbOptions.Size = New System.Drawing.Size(403, 92)
        Me._gbOptions.TabIndex = 9
        Me._gbOptions.TabStop = False
        Me._gbOptions.Text = "What do you want to do now:"
        ' 
        ' _rbContinue
        ' 
        Me._rbContinue.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me._rbContinue.Location = New System.Drawing.Point(19, 55)
        Me._rbContinue.Name = "_rbContinue"
        Me._rbContinue.Size = New System.Drawing.Size(221, 28)
        Me._rbContinue.TabIndex = 1
        Me._rbContinue.Text = "Try to load the image anyway"
        ' 
        ' _rbCancel
        ' 
        Me._rbCancel.Checked = True
        Me._rbCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me._rbCancel.Location = New System.Drawing.Point(19, 18)
        Me._rbCancel.Name = "_rbCancel"
        Me._rbCancel.Size = New System.Drawing.Size(192, 28)
        Me._rbCancel.TabIndex = 0
        Me._rbCancel.TabStop = True
        Me._rbCancel.Text = "Cancel loading the image"
        ' 
        ' _lblLine1
        ' 
        Me._lblLine1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me._lblLine1.Location = New System.Drawing.Point(12, 13)
        Me._lblLine1.Name = "_lblLine1"
        Me._lblLine1.Size = New System.Drawing.Size(259, 27)
        Me._lblLine1.TabIndex = 6
        Me._lblLine1.Text = "The LEADTOOLS PDF engine is missing."
        Me._lblLine1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        ' 
        ' _lbEngine
        ' 
        Me._lbEngine.Location = New System.Drawing.Point(12, 69)
        Me._lbEngine.Name = "_lbEngine"
        Me._lbEngine.Size = New System.Drawing.Size(518, 26)
        Me._lbEngine.TabIndex = 8
        Me._lbEngine.TabStop = True
        Me._lbEngine.Text = "http://www.leadtools.com/ReleaseDownloads/v14/LEADTOOLSPDFRuntime.exe"
        Me._lbEngine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '		 Me._lbEngine.LinkClicked += New System.Windows.Forms.LinkLabelLinkClickedEventHandler(Me._lbEngine_LinkClicked);
        ' 
        ' _lblLine2
        ' 
        Me._lblLine2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me._lblLine2.Location = New System.Drawing.Point(12, 41)
        Me._lblLine2.Name = "_lblLine2"
        Me._lblLine2.Size = New System.Drawing.Size(537, 26)
        Me._lblLine2.TabIndex = 7
        Me._lblLine2.Text = "Please download and install the LEADTOOLS PDF engine from the following address:"
        Me._lblLine2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        ' 
        ' _btnOk
        ' 
        Me._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me._btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me._btnOk.Location = New System.Drawing.Point(453, 124)
        Me._btnOk.Name = "_btnOk"
        Me._btnOk.Size = New System.Drawing.Size(90, 27)
        Me._btnOk.TabIndex = 5
        Me._btnOk.Text = "OK"
        '		 Me._btnOk.Click += New System.EventHandler(Me._btnOk_Click);
        ' 
        ' PdfEngineDialog
        ' 
        Me.AcceptButton = Me._btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0F, 16.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me._btnOk
        Me.ClientSize = New System.Drawing.Size(555, 212)
        Me.Controls.Add(Me._gbOptions)
        Me.Controls.Add(Me._lblLine1)
        Me.Controls.Add(Me._lbEngine)
        Me.Controls.Add(Me._lblLine2)
        Me.Controls.Add(Me._btnOk)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PdfEngineDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "LEADTOOLS PDF Engine Warning"
        Me._gbOptions.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _gbOptions As System.Windows.Forms.GroupBox
    Private _rbContinue As System.Windows.Forms.RadioButton
    Private _rbCancel As System.Windows.Forms.RadioButton
    Private _lblLine1 As System.Windows.Forms.Label
    Private WithEvents _lbEngine As System.Windows.Forms.LinkLabel
    Private _lblLine2 As System.Windows.Forms.Label
    Private WithEvents _btnOk As System.Windows.Forms.Button
End Class
