<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Fileform
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Fileform))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Txtpath = New System.Windows.Forms.TextBox()
        Me.TreeView = New System.Windows.Forms.TreeView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtSearch = New System.Windows.Forms.TextBox()
        Me.BtnNotify = New System.Windows.Forms.Button()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.Nameheader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TypeHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LastModifiedHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Btn_dwld = New System.Windows.Forms.Button()
        Me.btn_decrypt = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Panel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Lucida Sans", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Gray
        Me.Label1.Location = New System.Drawing.Point(5, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Repository"
        '
        'Txtpath
        '
        Me.Txtpath.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.Txtpath.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Txtpath.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.Txtpath.Location = New System.Drawing.Point(86, 19)
        Me.Txtpath.Multiline = True
        Me.Txtpath.Name = "Txtpath"
        Me.Txtpath.Size = New System.Drawing.Size(533, 24)
        Me.Txtpath.TabIndex = 3
        '
        'TreeView
        '
        Me.TreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TreeView.ImageIndex = 0
        Me.TreeView.ImageList = Me.ImageList1
        Me.TreeView.LineColor = System.Drawing.Color.White
        Me.TreeView.Location = New System.Drawing.Point(0, 0)
        Me.TreeView.Name = "TreeView"
        Me.TreeView.SelectedImageIndex = 0
        Me.TreeView.Size = New System.Drawing.Size(227, 364)
        Me.TreeView.TabIndex = 5
        Me.TreeView.Visible = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "drive.png")
        Me.ImageList1.Images.SetKeyName(1, "folder.png")
        Me.ImageList1.Images.SetKeyName(2, "file.png")
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.TxtSearch)
        Me.Panel1.Controls.Add(Me.BtnNotify)
        Me.Panel1.Controls.Add(Me.WebBrowser1)
        Me.Panel1.Controls.Add(Me.Txtpath)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.ForeColor = System.Drawing.Color.Gray
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(961, 63)
        Me.Panel1.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Lucida Sans", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Gray
        Me.Label2.Location = New System.Drawing.Point(640, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 15)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Search"
        '
        'TxtSearch
        '
        Me.TxtSearch.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.TxtSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSearch.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.TxtSearch.Location = New System.Drawing.Point(694, 20)
        Me.TxtSearch.Multiline = True
        Me.TxtSearch.Name = "TxtSearch"
        Me.TxtSearch.Size = New System.Drawing.Size(168, 24)
        Me.TxtSearch.TabIndex = 6
        '
        'BtnNotify
        '
        Me.BtnNotify.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.BtnNotify.BackgroundImage = CType(resources.GetObject("BtnNotify.BackgroundImage"), System.Drawing.Image)
        Me.BtnNotify.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnNotify.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnNotify.ForeColor = System.Drawing.Color.White
        Me.BtnNotify.Location = New System.Drawing.Point(906, 12)
        Me.BtnNotify.Name = "BtnNotify"
        Me.BtnNotify.Size = New System.Drawing.Size(43, 37)
        Me.BtnNotify.TabIndex = 5
        Me.BtnNotify.UseVisualStyleBackColor = False
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Location = New System.Drawing.Point(287, 95)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(645, 200)
        Me.WebBrowser1.TabIndex = 4
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Top
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 63)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TreeView)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ListView1)
        Me.SplitContainer1.Size = New System.Drawing.Size(961, 364)
        Me.SplitContainer1.SplitterDistance = 227
        Me.SplitContainer1.TabIndex = 7
        '
        'ListView1
        '
        Me.ListView1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Nameheader, Me.TypeHeader, Me.LastModifiedHeader})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(0, 0)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(730, 364)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'Nameheader
        '
        Me.Nameheader.Text = "Name"
        Me.Nameheader.Width = 149
        '
        'TypeHeader
        '
        Me.TypeHeader.Text = "Type"
        Me.TypeHeader.Width = 190
        '
        'LastModifiedHeader
        '
        Me.LastModifiedHeader.Text = "Last  Modified"
        Me.LastModifiedHeader.Width = 510
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Btn_dwld)
        Me.Panel2.Controls.Add(Me.btn_decrypt)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 430)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(961, 43)
        Me.Panel2.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Gray
        Me.Label4.Location = New System.Drawing.Point(77, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(0, 15)
        Me.Label4.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(27, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 13)
        Me.Label3.TabIndex = 2
        '
        'Btn_dwld
        '
        Me.Btn_dwld.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.Btn_dwld.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Btn_dwld.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn_dwld.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Btn_dwld.Location = New System.Drawing.Point(816, 6)
        Me.Btn_dwld.Name = "Btn_dwld"
        Me.Btn_dwld.Size = New System.Drawing.Size(101, 25)
        Me.Btn_dwld.TabIndex = 1
        Me.Btn_dwld.Text = "Download"
        Me.Btn_dwld.UseVisualStyleBackColor = False
        '
        'btn_decrypt
        '
        Me.btn_decrypt.BackColor = System.Drawing.Color.BlueViolet
        Me.btn_decrypt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_decrypt.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_decrypt.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btn_decrypt.Location = New System.Drawing.Point(673, 6)
        Me.btn_decrypt.Name = "btn_decrypt"
        Me.btn_decrypt.Size = New System.Drawing.Size(101, 25)
        Me.btn_decrypt.TabIndex = 0
        Me.btn_decrypt.Text = "Decrypt"
        Me.btn_decrypt.UseVisualStyleBackColor = False
        '
        'Fileform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.ClientSize = New System.Drawing.Size(961, 473)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Fileform"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "STANDALONE EXPLORER"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As Label
    Friend WithEvents Txtpath As TextBox
    Friend WithEvents TreeView As TreeView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents ListView1 As ListView
    Friend WithEvents Nameheader As ColumnHeader
    Friend WithEvents TypeHeader As ColumnHeader
    Friend WithEvents LastModifiedHeader As ColumnHeader
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Btn_dwld As Button
    Friend WithEvents btn_decrypt As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents WebBrowser1 As WebBrowser
    Friend WithEvents BtnNotify As Button
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label2 As Label
    Friend WithEvents TxtSearch As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
End Class
