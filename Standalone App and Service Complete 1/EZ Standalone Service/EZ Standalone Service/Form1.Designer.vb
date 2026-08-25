<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Btndecrypt = New System.Windows.Forms.Button()
        Me.btnencrypt = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Btndecrypt
        '
        Me.Btndecrypt.Location = New System.Drawing.Point(266, 51)
        Me.Btndecrypt.Name = "Btndecrypt"
        Me.Btndecrypt.Size = New System.Drawing.Size(75, 36)
        Me.Btndecrypt.TabIndex = 7
        Me.Btndecrypt.Text = "Decrypt"
        Me.Btndecrypt.UseVisualStyleBackColor = True
        '
        'btnencrypt
        '
        Me.btnencrypt.Location = New System.Drawing.Point(84, 51)
        Me.btnencrypt.Name = "btnencrypt"
        Me.btnencrypt.Size = New System.Drawing.Size(70, 36)
        Me.btnencrypt.TabIndex = 6
        Me.btnencrypt.Text = "Encrypt"
        Me.btnencrypt.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(425, 139)
        Me.Controls.Add(Me.Btndecrypt)
        Me.Controls.Add(Me.btnencrypt)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "STANDALONE SERVICE"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Btndecrypt As Windows.Forms.Button
    Friend WithEvents btnencrypt As Windows.Forms.Button
End Class
