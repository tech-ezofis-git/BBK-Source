<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ServiceRef
    Inherits Telerik.WinControls.UI.RadForm

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
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.txt_ServiceUrl = New Telerik.WinControls.UI.RadTextBox()
        Me.btn_save = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txt_ServiceUrl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btn_save, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadLabel2
        '
        Me.RadLabel2.Location = New System.Drawing.Point(9, 26)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(90, 18)
        Me.RadLabel2.TabIndex = 0
        Me.RadLabel2.Text = "Server Name \ IP"
        '
        'txt_ServiceUrl
        '
        Me.txt_ServiceUrl.Location = New System.Drawing.Point(104, 24)
        Me.txt_ServiceUrl.Name = "txt_ServiceUrl"
        Me.txt_ServiceUrl.Size = New System.Drawing.Size(257, 20)
        Me.txt_ServiceUrl.TabIndex = 1
        Me.txt_ServiceUrl.TabStop = False
        '
        'btn_save
        '
        Me.btn_save.Location = New System.Drawing.Point(231, 59)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(130, 24)
        Me.btn_save.TabIndex = 5
        Me.btn_save.Text = "Save"
        '
        'ServiceRef
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(380, 101)
        Me.Controls.Add(Me.btn_save)
        Me.Controls.Add(Me.txt_ServiceUrl)
        Me.Controls.Add(Me.RadLabel2)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "ServiceRef"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "eZConfig"
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txt_ServiceUrl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btn_save, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents txt_ServiceUrl As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents btn_save As Telerik.WinControls.UI.RadButton
End Class

