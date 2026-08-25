<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        Me.BtnNotify = New System.Windows.Forms.Button()
        Me.ButSearch = New System.Windows.Forms.Button()
        Me.ButHome = New System.Windows.Forms.Button()
        Me.ButUp = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'BtnNotify
        '
        Me.BtnNotify.BackColor = System.Drawing.Color.SandyBrown
        Me.BtnNotify.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnNotify.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnNotify.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnNotify.ForeColor = System.Drawing.Color.White
        Me.BtnNotify.Location = New System.Drawing.Point(20, 620)
        Me.BtnNotify.Margin = New System.Windows.Forms.Padding(100, 3, 3, 3)
        Me.BtnNotify.Name = "BtnNotify"
        Me.BtnNotify.Size = New System.Drawing.Size(80, 35)
        Me.BtnNotify.TabIndex = 7
        Me.BtnNotify.Text = "LOGS"
        Me.BtnNotify.UseVisualStyleBackColor = False
        '
        'ButSearch
        '
        Me.ButSearch.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ButSearch.BackgroundImage = CType(resources.GetObject("ButSearch.BackgroundImage"), System.Drawing.Image)
        Me.ButSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ButSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButSearch.ForeColor = System.Drawing.Color.White
        Me.ButSearch.Location = New System.Drawing.Point(930, 10)
        Me.ButSearch.Margin = New System.Windows.Forms.Padding(100, 3, 3, 3)
        Me.ButSearch.Name = "ButSearch"
        Me.ButSearch.Size = New System.Drawing.Size(28, 23)
        Me.ButSearch.TabIndex = 10
        Me.ButSearch.UseVisualStyleBackColor = False
        '
        'ButHome
        '
        Me.ButHome.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ButHome.BackgroundImage = CType(resources.GetObject("ButHome.BackgroundImage"), System.Drawing.Image)
        Me.ButHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ButHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButHome.ForeColor = System.Drawing.Color.White
        Me.ButHome.Location = New System.Drawing.Point(678, 10)
        Me.ButHome.Margin = New System.Windows.Forms.Padding(100, 3, 3, 3)
        Me.ButHome.Name = "ButHome"
        Me.ButHome.Size = New System.Drawing.Size(20, 25)
        Me.ButHome.TabIndex = 9
        Me.ButHome.UseVisualStyleBackColor = False
        '
        'ButUp
        '
        Me.ButUp.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ButUp.BackgroundImage = CType(resources.GetObject("ButUp.BackgroundImage"), System.Drawing.Image)
        Me.ButUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ButUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButUp.ForeColor = System.Drawing.Color.White
        Me.ButUp.Location = New System.Drawing.Point(633, 10)
        Me.ButUp.Margin = New System.Windows.Forms.Padding(100, 3, 3, 3)
        Me.ButUp.Name = "ButUp"
        Me.ButUp.Size = New System.Drawing.Size(20, 25)
        Me.ButUp.TabIndex = 8
        Me.ButUp.UseVisualStyleBackColor = False
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.ClientSize = New System.Drawing.Size(1222, 653)
        Me.Controls.Add(Me.ButSearch)
        Me.Controls.Add(Me.ButHome)
        Me.Controls.Add(Me.ButUp)
        Me.Controls.Add(Me.BtnNotify)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form2"
        Me.Text = "STANDALONE EXPLORER"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BtnNotify As Button
    Friend WithEvents ButUp As Button
    Friend WithEvents ButHome As Button
    Friend WithEvents ButSearch As Button
End Class
