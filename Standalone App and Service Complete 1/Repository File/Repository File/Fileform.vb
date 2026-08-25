Imports System.IO
Imports System.Windows
Imports System.Windows.Controls
Imports Newtonsoft.Json
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports ListViewItem = System.Windows.Controls.ListViewItem
Imports System.Security.Cryptography
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.Web.Script.Serialization
Imports System.Collections.Specialized
Imports System.Windows.Media

Public Class Fileform
    Dim Dirr As String
    Dim selectpath As String
    Dim nextpath As String
    Public key
    Dim allfiles As Integer
    Dim filecount As Integer
    Dim folderfilecount
    Dim folderDfilecount
    Dim movefilecount
    Dim mcount
    Dim strFileToDecrypt As String
    Dim strOutputEncrypt As String
    Dim strOutputDecrypt As String
    Dim fsInput As System.IO.FileStream
    Dim fsOutput As System.IO.FileStream
    Public repopath As String = ""
    Dim mousepath As String = ""
    Dim filedestination = "C:\Archive"
    Dim worker As New System.ComponentModel.BackgroundWorker()
    Dim fcount = 0
    Public notifiy As New Notification
    Dim file1 As IO.StreamWriter
    Dim frmbatch As Batching = New Batching()
    Dim notifications As Notification = New Notification()
    Dim ser As JavaScriptSerializer = New JavaScriptSerializer()
    Dim Appcon As NameValueCollection = DirectCast(System.Configuration.ConfigurationManager.GetSection("Database"), NameValueCollection)
    Dim jspath = Appcon("Jsonpath")
    Dim dwnpath = Appcon("Downloadjpath")

    Dim custommsgbox As New CustomMessageBoxControl
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Enum CryptoAction
        'Define the enumeration for CryptoAction.
        ActionEncrypt = 1
        ActionDecrypt = 2
    End Enum

    Private Sub Btn_browse_Click(sender As Object, e As EventArgs)
        Dim open As New FolderBrowserDialog()
        Try
            If (open.ShowDialog() = DialogResult.OK) Then
                'WebBrowser1.Url = New Uri(open.SelectedPath)
                Txtpath.Text = open.SelectedPath
                Dirr = Txtpath.Text.ToString()
            End If
        Catch ex As Exception

        End Try
    End Sub


    'method 1 AlaisAccount
    'If username <> "" Then
    '    acct = New AliasAccount(username, passkey, FilePath)
    '    Try
    '        acct.BeginImpersonation()
    '        impersonate = True
    '        If impersonate = True Then
    '            If FilePath <> "" Then
    '                'Process.Start("explorer.exe", String.Format("/n, /e, {0}", FilePath))
    '                Dim frm As Form2 = New Form2()
    '                frm.repopath = FilePath
    '                Dim bytKey As Byte()
    '                Dim bytIV As Byte()
    '                'Send the password to the CreateKey function.
    '                bytKey = CreateKey(passkey)
    '                'Send the password to the CreateIV function.    
    '                bytIV = CreateIV(passkey)
    '                frm.key = passkey
    '                If frm.ShowDialog() = False Then
    '                    frm.Close()
    '                End If
    '            End If
    '            Form1.Close()
    '        End If
    '    Catch ex As Exception
    '        UNCC.writetxtfle("Impersonate :" + ex.ToString())
    '    Finally
    '        If impersonate Then
    '            acct.EndImpersonation()
    '        End If
    '    End Try
    'End If

    ''or Normal explorer
    'Try
    '    If FilePath <> "" Then
    '        MsgBox("Local Connection is Open", vbOKOnly, "STANDALONE EXPLORER:Notification")
    '        'Process.Start("explorer.exe", String.Format("/n, /e, {0}", FilePath))
    '        Dim frm As Form2 = New Form2()
    '        frm.repopath = FilePath
    '        Dim bytKey As Byte()
    '        Dim bytIV As Byte()
    '        'Send the password to the CreateKey function.
    '        bytKey = CreateKey(passkey)
    '        'Send the password to the CreateIV function.    
    '        bytIV = CreateIV(passkey)
    '        frm.key = passkey
    '        If frm.ShowDialog() = False Then
    '            frm.Close()
    '        End If
    '    End If
    '    Form1.Close()
    '    'End If
    'Catch ex As Exception
    'End Try


    'Public Sub Navigation_Forward(ByVal sender As Object, ByVal Path As String)
    '    Dim LView As System.Windows.Forms.ListView = sender
    '    With CType(sender, System.Windows.Forms.ListView)
    '        Dim Directory_Item = New IO.DirectoryInfo(Path)
    '        LView.Items.Clear()
    '        For Each My_directory As IO.DirectoryInfo In Directory_Item.GetDirectories
    '            Dim Lvi As New ListViewItem
    '            Lvi.Tag = My_directory.FullName
    '            Lvi.ImageKey = CacheShellIcon(My_directory.FullName)
    '            Lvi.Text = My_directory.Name
    '            Lvi.Items.Add(Directory_Item)
    '        Next
    '        'Do same loop for files if desired
    '    End With
    'End Sub

    'Private Sub Btn_forward_Click(sender As Object, e As EventArgs)
    '    Try
    '        'Dim i As Integer = 0
    '        ''When we navigate forward Listview has items, so we need to loop their tags. If loop finds same string that we stored in  Navigation variable than this means we were allready in this directory
    '        'For i = ListView1.Items.Count - 1 To 0 Step -1
    '        '    For Each path As String In Navigation
    '        '        If ListView1.Items(i).Tag.ToString = path Then
    '        '            Navigation_Forward(ListView1, path)
    '        '            Exit Sub
    '        '        End If
    '        '    Next
    '        'Next
    '        'If (ListView1.GoForward) Then
    '        '    WebBrowser1.GoForward()
    '        'End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Private Sub Btn_back_Click(sender As Object, e As EventArgs)
    '    Try
    '        'If (ListView.) Then
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub Fileform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDrives()
        ListView1.SmallImageList = ImageList1
        ListView1.LargeImageList = ImageList1
       ' Label3.ForeColor = Color.Gray
        Label3.Font = New Font("Segoe UI", 9)
    End Sub

    Public Sub LoadDrives()
        Dim directoryinformation As New DirectoryInfo(repopath)
        For Each drive As DirectoryInfo In directoryinformation.GetDirectories
            Dim node As New TreeNode(drive.Name)
            With node
                .Tag = drive.Name
                .ImageKey = "drive"
                .ImageIndex = 0
                .SelectedImageIndex = 0
                .Nodes.Add("Empty")
            End With
            'loadchildrens(node, node.Tag)
            TreeView.Nodes.Add(node)
            'PopulateTreeView(repopath, node)
        Next

        TreeView.Nodes.Clear()
        ToolTip1.ShowAlways = True

        If repopath <> "" AndAlso Directory.Exists(repopath) Then
            LoadDirectory(repopath)
        Else

        End If
    End Sub

    Public Sub LoadDirectory(ByVal Dir As String)
        Dim di As DirectoryInfo = New DirectoryInfo(Dir)
        'progressBar1.Maximum = Directory.GetFiles(Dir, "*.*", SearchOption.AllDirectories).Length + Directory.GetDirectories(Dir, "**", SearchOption.AllDirectories).Length
        Dim tds As TreeNode = TreeView.Nodes.Add(di.Name)
        tds.Tag = di.FullName
        tds.ImageIndex = 1
        tds.SelectedImageIndex = 1
        LoadFiles(Dir, tds)
        Dim lbl = TreeView.Nodes.Count
        LoadSubDirectories(Dir, tds)
        Label3.Text = lbl & " " & " items"
    End Sub

    Private Sub LoadSubDirectories(ByVal dir As String, ByVal td As TreeNode)
        Dim subdirectoryEntries = Directory.GetDirectories(dir)

        For Each subdirectory As String In subdirectoryEntries
            Dim di As DirectoryInfo = New DirectoryInfo(subdirectory)
            Dim tds As TreeNode = td.Nodes.Add(di.Name)
            tds.SelectedImageIndex = 1
            tds.Tag = di.FullName
            tds.ImageIndex = 1
            LoadFiles(subdirectory, tds)
            LoadSubDirectories(subdirectory, tds)

            'UpdateProgress()
        Next
    End Sub

    Private Sub LoadFiles(ByVal dir As String, ByVal td As TreeNode)
        Dim Files As String() = Directory.GetFiles(dir, "*.*")

        For Each file As String In Files
            Dim fi As FileInfo = New FileInfo(file)
            Dim tds As TreeNode = td.Nodes.Add(fi.Name)
            tds.Tag = fi.FullName
            tds.ImageIndex = 2
            tds.SelectedImageIndex = 2
            'UpdateProgress()
        Next
    End Sub

    Public Sub loadchildrens(nd As TreeNode, dir As String)
        Dim directoryinformation As New DirectoryInfo(dir)
        Label4.Visible = Visibility.Hidden
        ListView1.Items.Clear()
        Dim Subitems() As Forms.ListViewItem.ListViewSubItem
        Dim items As Forms.ListViewItem = Nothing
        Try
            'load all sub folders in the node
            For Each d As DirectoryInfo In directoryinformation.GetDirectories
                If Not (d.Attributes And FileAttribute.Hidden) = FileAttribute.Hidden Then
                    Dim folder As New TreeNode(d.Name)
                    With folder
                        .Tag = d.FullName
                        .ImageKey = "folder.png"
                        .ImageIndex = 1
                        .SelectedImageIndex = 1
                        '.SelectedImageKey = d.FullName
                        .Nodes.Add("Empty")
                    End With
                    nd.Nodes.Add(folder)
                    Dim nlbl = nd.Nodes.Count
                    Label3.Text = nlbl & " " & " Items"
                    items = New Forms.ListViewItem(d.Name, 1)
                    Subitems = New Forms.ListViewItem.ListViewSubItem() {New Forms.ListViewItem.ListViewSubItem(items, "folder"), New Forms.ListViewItem.ListViewSubItem(items, d.LastAccessTime.ToShortDateString())}
                    items.SubItems.AddRange(Subitems)
                    ListView1.Items.Add(items)
                    Dim allfiles = d.GetFiles.Count
                End If
            Next
            'load all files
            For Each files As FileInfo In directoryinformation.GetFiles
                Dim filecount = directoryinformation.GetFiles.Count
                If Not (files.Attributes And FileAttribute.Hidden) = FileAttribute.Hidden Then
                    Dim fn As New TreeNode(files.Name)
                    With fn
                        .Tag = files.FullName
                        .ImageKey = "file.png"
                        .ImageIndex = 2
                        .SelectedImageIndex = 2
                    End With
                    nd.Nodes.Add(fn)
                    Dim nlbls = nd.Nodes.Count
                    Label3.Text = nlbls & " " & " Items"
                    Dim file_type As String = "file"

                    Select Case files.FullName.Split(".").LastOrDefault().ToLower()

                        Case "dll"
                            file_type = "Dynamic link library"

                        Case "sys"
                            file_type = "System File"

                        Case "exe"
                            file_type = "Executable"

                        Case "jar"
                            file_type = "Executable"

                        Case "dat"
                            file_type = "Date File"

                        Case "txt"
                            file_type = "Document"
                        Case "html"
                            file_type = "Document"
                        Case "css"
                            file_type = "Document"
                        Case "rtf"
                            file_type = "Document"
                        Case "text"
                            file_type = "Document"
                        Case "log"
                            file_type = "Document"
                        Case "yml"
                            file_type = "Document"
                        Case "xml"
                            file_type = "Document"
                            '
                        Case "Zip"
                            file_type = "Compressed File"
                        Case "rar"
                            file_type = "Compressed File"
                        Case "7z"
                            file_type = "Compressed File"
                        Case "pak"
                            file_type = "Compressed File"
                        Case "rpf"
                            file_type = "Compressed File"
                            '
                        Case "bin"
                            file_type = "System Image"
                        Case "iso"
                            file_type = "System Image"
                        Case "img"
                            file_type = "System Image"
                        Case "dmg"
                            file_type = "System Image"
                            '
                        Case "bmp"
                            file_type = "Image"
                        Case "png"
                            file_type = "Image"
                        Case "jpg"
                            file_type = "Image"
                        Case "gif"
                            file_type = "Image"
                        Case "tiff"
                            file_type = "Image"
                        Case "jpeg"
                            file_type = "Image"
                        Case "ico"
                            file_type = "Image"
                        Case "jfif"
                            file_type = "Image"
                            '
                        Case "mp4"
                            file_type = "Video"
                        Case "webm"
                            file_type = "Video"
                        Case "3gp"
                            file_type = "Video"
                        Case "m4v"
                            file_type = "Video"
                        Case "flv"
                            file_type = "Video"
                        Case "mpeg"
                            file_type = "Video"
                        Case "mpv"
                            file_type = "Video"
                        Case "mov"
                            file_type = "Video"
                        Case "swf"
                            file_type = "Video"
                        Case "wmv"
                            file_type = "Video"
                            '
                        Case "mp1"
                            file_type = "Music"
                        Case "mp2"
                            file_type = "Music"
                        Case "mp3"
                            file_type = "Music"
                        Case "mp4"
                            file_type = "Music"
                        Case "wav"
                            file_type = "Music"
                        Case "m4a"
                            file_type = "Music"
                        Case "flac"
                            file_type = "Music"
                        Case "wma"
                            file_type = "Music"
                        Case "ogg"
                            file_type = "Music"
                            '
                        Case "ttf"
                            file_type = "Font File"
                        Case "ufo"
                            file_type = "Font File"
                        Case "fnt"
                            file_type = "Font File"
                        Case Else
                            file_type = "File"
                    End Select
                    items = New Forms.ListViewItem(files.Name, 2)
                    Subitems = New Forms.ListViewItem.ListViewSubItem() {New Forms.ListViewItem.ListViewSubItem(items, "file"), New Forms.ListViewItem.ListViewSubItem(items, files.LastAccessTime.ToShortDateString())}
                    items.SubItems.AddRange(Subitems)
                    ListView1.Items.Add(items)
                End If
            Next
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try

    End Sub

    Private Sub TreeView_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles TreeView.BeforeExpand
        Dim isdriveread As Boolean = (From d As DriveInfo In DriveInfo.GetDrives Where d.Name = e.Node.ImageKey Select d.IsReady).FirstOrDefault()
        If (e.Node.Tag <> "Desktop" AndAlso Not e.Node.Tag.Contains(":\")) OrElse isdriveread OrElse Directory.Exists(e.Node.Tag) Then
            e.Node.Nodes.Clear()
            loadchildrens(e.Node, e.Node.Tag.ToString)
        ElseIf e.Node.ImageKey = "Desktop" Then
            e.Node.Nodes.Clear()
            Dim desktopfolder As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)
            Dim userdeskfolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

            loadchildrens(e.Node, userdeskfolder)
            loadchildrens(e.Node, desktopfolder)
        Else
            e.Cancel = True
            Windows.MessageBox.Show("Error drive is empty," + e.Node.ImageKey.ToString())

        End If
    End Sub

    'Private Sub PopulateTreeView(ByVal dir As String, ByVal parentNode As TreeNode)
    '    Dim folder As String = String.Empty

    '    Try
    '        Dim folders As String() = System.IO.Directory.GetDirectories(dir)

    '        If folders.Length <> 0 Then
    '            Dim childNode As TreeNode = Nothing

    '            For Each folder_loopVariable As String In folders
    '                folder = folder_loopVariable
    '                childNode = New TreeNode(folder)
    '                childNode.Nodes.Add("")
    '                parentNode.Nodes.Add(childNode)
    '            Next
    '        End If

    '        Dim files As String() = System.IO.Directory.GetFiles(dir)

    '        If files.Length <> 0 Then
    '            Dim childNode As TreeNode = Nothing

    '            For Each file As String In files
    '                childNode = New TreeNode(file)
    '                parentNode.Nodes.Add(childNode)
    '            Next
    '        End If

    '    Catch ex As UnauthorizedAccessException
    '        parentNode.Nodes.Add(folder & ": Access Denied")
    '    End Try
    'End Sub

    Sub loadFolders(ByVal Tnode As TreeNode, ByVal fPath As String)
        'Application.DoEvents()
        For Each folderNode As String In Directory.GetDirectories(fPath)
            Dim shortNode As String
            shortNode = getFolder(folderNode) 'shorten path   

            Dim subNode As TreeNode = Tnode.Nodes.Add(shortNode)
            subNode.Tag = folderNode
            subNode.Nodes.Add("temp")
        Next
        loadFiles(Tnode, fPath)
    End Sub

    Sub loadFiles(ByVal fnode As TreeNode, ByVal dir As String)
        'Application.DoEvents()
        For Each filesNode As String In Directory.GetFiles(dir)
            Dim subFileNode As TreeNode = fnode.Nodes.Add(filesNode)
            subFileNode.Tag = filesNode
            subFileNode.Text = Path.GetFileName(filesNode)
            'getIcons(subFileNode, filesNode)
        Next
    End Sub

    Public Function getFolder(ByVal str As String)
        Dim i As Integer
        Dim shrtPath As String
        i = str.LastIndexOf("\")
        shrtPath = str.Substring(i + 1)
        Return shrtPath
    End Function

    Private Sub TreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView.AfterSelect
        Txtpath.Text = e.Node.Tag.ToString()
        selectpath = Txtpath.Text
    End Sub

    Private Sub TreeView_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles TreeView.AfterCollapse
        e.Node.Nodes.Clear()
        e.Node.Nodes.Add("Empty")
    End Sub

    'Private Sub TreeView_DoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView.DoubleClick
    '    Dim directoryinformation As New DirectoryInfo(e.Node.Tag.ToString())

    '    ListView1.Items.Clear()
    '    Dim Subitems() As Forms.ListViewItem.ListViewSubItem
    '    Dim items As Forms.ListViewItem = Nothing

    '    Try
    '        'load all sub folders in the node
    '        For Each d As DirectoryInfo In directoryinformation.GetDirectories
    '            If Not (d.Attributes And FileAttribute.Hidden) = FileAttribute.Hidden Then
    '                items = New Forms.ListViewItem(d.Name, 0)
    '                Subitems = New Forms.ListViewItem.ListViewSubItem() {New Forms.ListViewItem.ListViewSubItem(items, "Directory"), New Forms.ListViewItem.ListViewSubItem(items, d.LastAccessTime.ToShortDateString())}
    '                items.SubItems.AddRange(Subitems)
    '                ListView1.Items.Add(items)
    '                allfiles = d.GetFiles.Count
    '            End If
    '        Next
    '        'load all files
    '        For Each files As FileInfo In directoryinformation.GetFiles
    '            filecount = directoryinformation.GetFiles.Count

    '            If Not (files.Attributes And FileAttribute.Hidden) = FileAttribute.Hidden Then

    '                Dim file_type As String = "file"

    '                Select Case files.FullName.Split(".").LastOrDefault().ToLower()

    '                    Case "dll"
    '                        file_type = "Dynamic link library"

    '                    Case "sys"
    '                        file_type = "System File"

    '                    Case "exe"
    '                        file_type = "Executable"

    '                    Case "jar"
    '                        file_type = "Executable"

    '                    Case "dat"
    '                        file_type = "Date File"

    '                    Case "txt"
    '                        file_type = "Document"
    '                    Case "html"
    '                        file_type = "Document"
    '                    Case "css"
    '                        file_type = "Document"
    '                    Case "rtf"
    '                        file_type = "Document"
    '                    Case "text"
    '                        file_type = "Document"
    '                    Case "log"
    '                        file_type = "Document"
    '                    Case "yml"
    '                        file_type = "Document"
    '                    Case "xml"
    '                        file_type = "Document"
    '                    Case "docx"
    '                        file_type = "Document"


    '                    Case "Zip"
    '                        file_type = "Compressed File"
    '                    Case "rar"
    '                        file_type = "Compressed File"
    '                    Case "7z"
    '                        file_type = "Compressed File"
    '                    Case "pak"
    '                        file_type = "Compressed File"
    '                    Case "rpf"
    '                        file_type = "Compressed File"
    '                        '
    '                    Case "bin"
    '                        file_type = "System Image"
    '                    Case "iso"
    '                        file_type = "System Image"
    '                    Case "img"
    '                        file_type = "System Image"
    '                    Case "dmg"
    '                        file_type = "System Image"
    '                        '
    '                    Case "bmp"
    '                        file_type = "Image"
    '                    Case "png"
    '                        file_type = "Image"
    '                    Case "jpg"
    '                        file_type = "Image"
    '                    Case "gif"
    '                        file_type = "Image"
    '                    Case "tiff"
    '                        file_type = "Image"
    '                    Case "jpeg"
    '                        file_type = "Image"
    '                    Case "ico"
    '                        file_type = "Image"
    '                    Case "jfif"
    '                        file_type = "Image"
    '                        '
    '                    Case "mp4"
    '                        file_type = "Video"
    '                    Case "webm"
    '                        file_type = "Video"
    '                    Case "3gp"
    '                        file_type = "Video"
    '                    Case "m4v"
    '                        file_type = "Video"
    '                    Case "flv"
    '                        file_type = "Video"
    '                    Case "mpeg"
    '                        file_type = "Video"
    '                    Case "mpv"
    '                        file_type = "Video"
    '                    Case "mov"
    '                        file_type = "Video"
    '                    Case "swf"
    '                        file_type = "Video"
    '                    Case "wmv"
    '                        file_type = "Video"
    '                        '
    '                    Case "mp1"
    '                        file_type = "Music"
    '                    Case "mp2"
    '                        file_type = "Music"
    '                    Case "mp3"
    '                        file_type = "Music"
    '                    Case "mp4"
    '                        file_type = "Music"
    '                    Case "wav"
    '                        file_type = "Music"
    '                    Case "m4a"
    '                        file_type = "Music"
    '                    Case "flac"
    '                        file_type = "Music"
    '                    Case "wma"
    '                        file_type = "Music"
    '                    Case "ogg"
    '                        file_type = "Music"
    '                        '
    '                    Case "ttf"
    '                        file_type = "Font File"
    '                    Case "ufo"
    '                        file_type = "Font File"
    '                    Case "fnt"
    '                        file_type = "Font File"
    '                    Case Else
    '                        file_type = "File"
    '                End Select
    '                items = New Forms.ListViewItem(files.Name, 1)
    '                Subitems = New Forms.ListViewItem.ListViewSubItem() {New Forms.ListViewItem.ListViewSubItem(items, "file"), New Forms.ListViewItem.ListViewSubItem(items, files.LastAccessTime.ToShortDateString())}
    '                items.SubItems.AddRange(Subitems)
    '                ListView1.Items.Add(items)
    '            End If
    '        Next
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub TreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView.NodeMouseClick
        Label4.Visible = Visibility.Hidden
        Dim directoryinformation As New DirectoryInfo(e.Node.Tag.ToString())
        Dim dirList As New ArrayList()
        Dim fileList As New ArrayList()
        Dim TotalItems As Integer
        ListView1.Items.Clear()
        Dim Subitems() As Forms.ListViewItem.ListViewSubItem
        Dim items As Forms.ListViewItem = Nothing

        Try
            'load all sub folders in the node
            For Each d As DirectoryInfo In directoryinformation.GetDirectories
                If Not (d.Attributes And FileAttribute.Hidden) = FileAttribute.Hidden Then
                    items = New Forms.ListViewItem(d.Name, 1)
                    Subitems = New Forms.ListViewItem.ListViewSubItem() {New Forms.ListViewItem.ListViewSubItem(items, "folder"), New Forms.ListViewItem.ListViewSubItem(items, d.LastAccessTime.ToShortDateString())}
                    items.SubItems.AddRange(Subitems)
                    'items.ImageKey = "folder"
                    items.ImageIndex = 1
                    ListView1.Items.Add(items)
                    allfiles = d.GetFiles.Count
                End If
            Next
            'load all files
            For Each files As FileInfo In directoryinformation.GetFiles
                filecount = directoryinformation.GetFiles.Count

                If Not (files.Attributes And FileAttribute.Hidden) = FileAttribute.Hidden Then

                    Dim file_type As String = "file"

                    Select Case files.FullName.Split(".").LastOrDefault().ToLower()

                        Case "dll"
                            file_type = "Dynamic link library"

                        Case "sys"
                            file_type = "System File"

                        Case "exe"
                            file_type = "Executable"

                        Case "jar"
                            file_type = "Executable"

                        Case "dat"
                            file_type = "Date File"

                        Case "txt"
                            file_type = "Document"
                        Case "html"
                            file_type = "Document"
                        Case "css"
                            file_type = "Document"
                        Case "rtf"
                            file_type = "Document"
                        Case "text"
                            file_type = "Document"
                        Case "log"
                            file_type = "Document"
                        Case "yml"
                            file_type = "Document"
                        Case "xml"
                            file_type = "Document"
                        Case "docx"
                            file_type = "Document"


                        Case "Zip"
                            file_type = "Compressed File"
                        Case "rar"
                            file_type = "Compressed File"
                        Case "7z"
                            file_type = "Compressed File"
                        Case "pak"
                            file_type = "Compressed File"
                        Case "rpf"
                            file_type = "Compressed File"
                            '
                        Case "bin"
                            file_type = "System Image"
                        Case "iso"
                            file_type = "System Image"
                        Case "img"
                            file_type = "System Image"
                        Case "dmg"
                            file_type = "System Image"
                            '
                        Case "bmp"
                            file_type = "Image"
                        Case "png"
                            file_type = "Image"
                        Case "jpg"
                            file_type = "Image"
                        Case "gif"
                            file_type = "Image"
                        Case "tiff"
                            file_type = "Image"
                        Case "jpeg"
                            file_type = "Image"
                        Case "ico"
                            file_type = "Image"
                        Case "jfif"
                            file_type = "Image"
                            '
                        Case "mp4"
                            file_type = "Video"
                        Case "webm"
                            file_type = "Video"
                        Case "3gp"
                            file_type = "Video"
                        Case "m4v"
                            file_type = "Video"
                        Case "flv"
                            file_type = "Video"
                        Case "mpeg"
                            file_type = "Video"
                        Case "mpv"
                            file_type = "Video"
                        Case "mov"
                            file_type = "Video"
                        Case "swf"
                            file_type = "Video"
                        Case "wmv"
                            file_type = "Video"
                            '
                        Case "mp1"
                            file_type = "Music"
                        Case "mp2"
                            file_type = "Music"
                        Case "mp3"
                            file_type = "Music"
                        Case "mp4"
                            file_type = "Music"
                        Case "wav"
                            file_type = "Music"
                        Case "m4a"
                            file_type = "Music"
                        Case "flac"
                            file_type = "Music"
                        Case "wma"
                            file_type = "Music"
                        Case "ogg"
                            file_type = "Music"
                            '
                        Case "ttf"
                            file_type = "Font File"
                        Case "ufo"
                            file_type = "Font File"
                        Case "fnt"
                            file_type = "Font File"
                        Case Else
                            file_type = "File"
                    End Select
                    items = New Forms.ListViewItem(files.Name, 2)
                    Subitems = New Forms.ListViewItem.ListViewSubItem() {New Forms.ListViewItem.ListViewSubItem(items, "file"), New Forms.ListViewItem.ListViewSubItem(items, files.LastAccessTime.ToShortDateString())}
                    'items.ImageKey = "file"
                    items.ImageIndex = 2
                    items.SubItems.AddRange(Subitems)
                    ListView1.Items.Add(items)
                End If
            Next
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub
    Private Navigation As New List(Of String)
    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        Try
            Label4.Visible = Visibility.Hidden
            Dim lslabl = ""
            Dim lsflabl = ""
            Dim fname = ListView1.SelectedItems(0).Text
            selectpath = Txtpath.Text + "\" + fname
            Dim directoryinformation As New DirectoryInfo(selectpath)
            ListView1.Items.Clear()
            Dim Subitems() As Forms.ListViewItem.ListViewSubItem
            Dim items As Forms.ListViewItem = Nothing

            For Each d As DirectoryInfo In directoryinformation.GetDirectories
                If Not (d.Attributes And FileAttribute.Hidden) = FileAttribute.Hidden Then
                    items = New Forms.ListViewItem(d.Name, 1)
                    Subitems = New Forms.ListViewItem.ListViewSubItem() {New Forms.ListViewItem.ListViewSubItem(items, "folder"), New Forms.ListViewItem.ListViewSubItem(items, d.LastAccessTime.ToShortDateString())}
                    items.SubItems.AddRange(Subitems)
                    items.ImageKey = "folder"
                    items.ImageIndex = 1
                    ListView1.Items.Add(items)
                    lslabl = ListView1.Items.Count
                    Txtpath.Text = selectpath
                    allfiles = d.GetFiles.Count
                End If
            Next
            Label3.Text = lslabl & " " & " Items"
            For Each files As FileInfo In directoryinformation.GetFiles
                Txtpath.Text = selectpath
                filecount = directoryinformation.GetFiles.Count
                If Not (files.Attributes And FileAttribute.Hidden) = FileAttribute.Hidden Then

                    Dim file_type As String = "file"

                    Select Case files.FullName.Split(".").LastOrDefault().ToLower()

                        Case "dll"
                            file_type = "Dynamic link library"

                        Case "sys"
                            file_type = "System File"

                        Case "exe"
                            file_type = "Executable"

                        Case "jar"
                            file_type = "Executable"

                        Case "dat"
                            file_type = "Date File"

                        Case "txt"
                            file_type = "Document"
                        Case "html"
                            file_type = "Document"
                        Case "css"
                            file_type = "Document"
                        Case "rtf"
                            file_type = "Document"
                        Case "text"
                            file_type = "Document"
                        Case "log"
                            file_type = "Document"
                        Case "yml"
                            file_type = "Document"
                        Case "xml"
                            file_type = "Document"
                            '
                        Case "Zip"
                            file_type = "Compressed File"
                        Case "rar"
                            file_type = "Compressed File"
                        Case "7z"
                            file_type = "Compressed File"
                        Case "pak"
                            file_type = "Compressed File"
                        Case "rpf"
                            file_type = "Compressed File"
                            '
                        Case "bin"
                            file_type = "System Image"
                        Case "iso"
                            file_type = "System Image"
                        Case "img"
                            file_type = "System Image"
                        Case "dmg"
                            file_type = "System Image"
                            '
                        Case "bmp"
                            file_type = "Image"
                        Case "png"
                            file_type = "Image"
                        Case "jpg"
                            file_type = "Image"
                        Case "gif"
                            file_type = "Image"
                        Case "tiff"
                            file_type = "Image"
                        Case "jpeg"
                            file_type = "Image"
                        Case "ico"
                            file_type = "Image"
                        Case "jfif"
                            file_type = "Image"
                            '
                        Case "mp4"
                            file_type = "Video"
                        Case "webm"
                            file_type = "Video"
                        Case "3gp"
                            file_type = "Video"
                        Case "m4v"
                            file_type = "Video"
                        Case "flv"
                            file_type = "Video"
                        Case "mpeg"
                            file_type = "Video"
                        Case "mpv"
                            file_type = "Video"
                        Case "mov"
                            file_type = "Video"
                        Case "swf"
                            file_type = "Video"
                        Case "wmv"
                            file_type = "Video"
                            '
                        Case "mp1"
                            file_type = "Music"
                        Case "mp2"
                            file_type = "Music"
                        Case "mp3"
                            file_type = "Music"
                        Case "mp4"
                            file_type = "Music"
                        Case "wav"
                            file_type = "Music"
                        Case "m4a"
                            file_type = "Music"
                        Case "flac"
                            file_type = "Music"
                        Case "wma"
                            file_type = "Music"
                        Case "ogg"
                            file_type = "Music"
                            '
                        Case "ttf"
                            file_type = "Font File"
                        Case "ufo"
                            file_type = "Font File"
                        Case "fnt"
                            file_type = "Font File"
                        Case Else
                            file_type = "File"
                    End Select
                    items = New Forms.ListViewItem(files.Name, 2)
                    Subitems = New Forms.ListViewItem.ListViewSubItem() {New Forms.ListViewItem.ListViewSubItem(items, "file"), New Forms.ListViewItem.ListViewSubItem(items, files.LastAccessTime.ToShortDateString())}
                    items.ImageKey = "file"
                    items.ImageIndex = 2
                    items.SubItems.AddRange(Subitems)
                    ListView1.Items.Add(items)
                    lsflabl = ListView1.Items.Count
                    Label3.Text = lsflabl & " " & " Items"
                End If
            Next

        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub PopulateTreeView(ByVal path As String)
        Dim rootNode As TreeNode

        If True Then
            Dim info As DirectoryInfo = New DirectoryInfo(path)

            If info.Exists Then
                TreeView.Nodes.Clear()
                rootNode = New TreeNode(info.Name)
                rootNode.Tag = info
                'GetDirectories(info.GetDirectories(), rootNode)
                TreeView.Nodes.Add(rootNode)
                LoadFiles(info.FullName, rootNode)
            Else

            End If
        End If
    End Sub

    Dim check As Boolean
    Dim decryptclick As String
    Dim downloadclick As String
    Private Sub btn_decrypt_Click(sender As Object, e As EventArgs) Handles btn_decrypt.Click
        Try
            Dim Settings As New JsonSerializerSettings
            Settings.Formatting = Formatting.Indented
            Settings.NullValueHandling = NullValueHandling.Ignore
            Dim spathinfo As New DirectoryInfo(selectpath)
            check = True
            decryptclick = "Decrypt"
            frmbatch.clickdecrypy = decryptclick
            Dim frmpass As Popup = New Popup()
            frmpass.Labelpass.Content = "Decrypt Password"
            frmpass.Labeldes.Visibility = Visibility.Hidden
            frmpass.Destfolder.Visibility = Visibility.Hidden
            frmpass.btnchsefolder.Visibility = Visibility.Hidden
            frmpass.wrongpassDecrypt.Visibility = Visibility.Hidden
            If frmpass.ShowDialog() Then
            End If
            Dim keypasse = Popup.passworkw
            Dim Rootobject As New Rootobject
            Dim info As New List(Of folderinfo)
            If keypasse <> "" Then
                Dim split As String() = selectpath.Split("\")
                Dim parentFolder As String = split(split.Length - 2)
                Dim Batchid = Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss") + "-" + parentFolder
                Batchid = Batchid.Replace("/", "")
                Batchid = Batchid.Replace(":", "")
                Batchid = Batchid.Replace(" ", "")
                Dim dtime = Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss").Replace("/", "").Replace(":", "").Replace(" ", "")
                folderfilecount = Directory.GetFiles(selectpath + "\", "*.ezo", IO.SearchOption.AllDirectories).Count
                Dim lTotalFileSize As Long = GetDirectoryFileSize(spathinfo)
                If selectpath <> "" Then
                    Dim a As New folderinfo
                    'Dim fname As String = IO.Path.GetFileName(filename)
                    'a.extension = System.IO.Path.GetExtension(filename).Replace(".", "")
                    fcount += 1
                    a.foldername = selectpath
                    a.pass = keypasse
                    a.foldersize = Format(lTotalFileSize / 1024 / 1024, "###,0.00") & " MB"
                    a.Nooffiles = folderfilecount
                    a.batchid = Batchid
                    a.datime = dtime
                    a.status = "New"
                    info.Add(a)
                End If
                'For Each filename As String In IO.Directory.GetFiles(selectpath + "\", "*", IO.SearchOption.AllDirectories)
                'Next
            Else
                frmpass.wrongpassDecrypt.Content = "Enter Password"
                frmpass.wrongpassDecrypt.Visibility = Visibility.Visible
            End If
            Dim client = New WebClient()
            client.Headers("Content-Type") = "application/json"
            client.Encoding = System.Text.Encoding.UTF8
            Dim bool1 As Boolean = IO.File.Exists(jspath)
            If bool1 = True Then
                Dim uristring = File.ReadAllText(jspath)
                Dim fileinfos As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring)
                If uristring <> "" Then
                    Dim jpath = uristring.Substring(0, uristring.Length - 2)
                    Dim json As String = JsonConvert.SerializeObject(info, Settings)
                    Dim rjson As String = json.Substring(1)
                    Dim savejson = jpath + "," + rjson.Replace(" ", "")
                    Dim savepath = System.Reflection.Assembly.GetEntryAssembly().Location
                    savepath = Path.GetDirectoryName(savepath)
                    System.IO.File.Delete(jspath)
                    Dim source = savepath + "\Json"
                    If Not Directory.Exists(source) Then
                        Directory.CreateDirectory(source)
                    End If
                    Dim filelocation = source & "\" & "data.txt"
                    Using sw As StreamWriter = New StreamWriter(filelocation, True)
                        sw.Write(savejson)
                        custommsgbox.showCustomMessageBox("Info", "File Decrypt Successfully")
                        ' MsgBox("File Decrypt Successfully", vbOKOnly, "STANDALONE EXPLORER:Notification")
                    End Using
                Else
                    Dim json As String = JsonConvert.SerializeObject(info, Settings)
                    Dim savepath = System.Reflection.Assembly.GetEntryAssembly().Location
                    savepath = Path.GetDirectoryName(savepath)
                    Dim source = savepath + "\Json"
                    System.IO.File.Delete(jspath)
                    If Not Directory.Exists(source) Then
                        Directory.CreateDirectory(source)
                    End If
                    Dim filelocation = source & "\" & "data.txt"
                    Using sw As StreamWriter = New StreamWriter(filelocation, True)
                        sw.Write(json)
                        custommsgbox.showCustomMessageBox("Info", "File Decrypt Successfully")
                        'MsgBox("File Decrypt Successfully", vbOKOnly, "STANDALONE EXPLORER:Notification")
                    End Using
                End If
            Else
                Dim json As String = JsonConvert.SerializeObject(info, Settings)
                Dim savepath = System.Reflection.Assembly.GetEntryAssembly().Location
                savepath = Path.GetDirectoryName(savepath)
                Dim source = savepath + "\Json"
                If Not Directory.Exists(source) Then
                    Directory.CreateDirectory(source)
                End If
                Dim filelocation = source & "\" & "data.txt"
                Using sw As StreamWriter = New StreamWriter(filelocation, True)
                    sw.Write(json)
                    'File.WriteAllLines(filelocation, File.ReadAllLines(json).Where(Function(s) s <> String.Empty))
                    custommsgbox.showCustomMessageBox("Info", "File Decrypt Successfully")
                    'MsgBox("File Decrypt Successfully", vbOKOnly, "STANDALONE EXPLORER:Notification")
                End Using
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Overloads Shared Function GetDirectoryFileSize(ByVal hDirectoryInfo As System.IO.DirectoryInfo) As Long
        Dim lTotalSize As Long
        For Each cFileInfo As System.IO.FileInfo In hDirectoryInfo.GetFiles()
            lTotalSize += cFileInfo.Length
        Next cFileInfo
        For Each hDirInfo As System.IO.DirectoryInfo In hDirectoryInfo.GetDirectories()
            lTotalSize += GetDirectoryFileSize(hDirInfo)
        Next hDirInfo
        Return lTotalSize
    End Function

    Public Class Rootobject
        Public Property info As List(Of folderinfo)
    End Class

    Public Class folderinfo
        Property foldername As String
        Property pass
        Property foldersize As String
        Property Nooffiles As Integer
        Property batchid As String
        Property datime As String
        Property status As String
    End Class


    Public Class downloadfile
        Property foldername As String
        Property passwordd As String
        Property dfoldersize As String
        Property Nooffiles As Integer
        Property extension As String
        Property batchid As String
        Property datime As String
        Property status As String
    End Class


    Public Class downloadobj
        Public Property info2 As List(Of downloadfile)
    End Class


    Private Sub BtnNotify_Click(sender As Object, e As EventArgs) Handles BtnNotify.Click
        Try
            Label4.Visible = Visibility.Hidden
            If frmbatch.ShowDialog() Then
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Btn_dwld_Click(sender As Object, e As EventArgs) Handles Btn_dwld.Click
        Try
            Dim Settings As New JsonSerializerSettings
            Settings.Formatting = Formatting.Indented
            Settings.NullValueHandling = NullValueHandling.Ignore
            Dim downpathinfo As New DirectoryInfo(selectpath)
            Dim info2 As New List(Of downloadfile)
            check = True
            downloadclick = "download"
            frmbatch.clickdownload = downloadclick
            Dim frmdes As Popup = New Popup()
            frmdes.Labelpass.Content = "Download Password"
            frmdes.btnchsefolder.Visibility = Visibility.Visible
            frmdes.wrongpassDownload.Visibility = Visibility.Collapsed
            If frmdes.ShowDialog() Then
            End If
            Dim movepath = Popup.Destdir
            Dim password = Popup.passworkw
            Dim filepath = selectpath
            Dim split As String() = selectpath.Split("\")
            Dim parentFolder As String = split(split.Length - 2)
            Dim DBatchid = Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss") + "-" + parentFolder
            DBatchid = DBatchid.Replace("/", "")
            DBatchid = DBatchid.Replace(":", "")
            DBatchid = DBatchid.Replace(" ", "")
            Dim ddtime = Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss").Replace("/", "").Replace(":", "").Replace(" ", "")
            folderDfilecount = Directory.GetFiles(selectpath + "\", "*.ezo", IO.SearchOption.AllDirectories).Count
            Dim lTotalFileSizes As Long = GetDirectoryFileSize(downpathinfo)
            If password <> "" Then
                Dim b As New downloadfile
                b.foldername = selectpath
                b.passwordd = password
                b.dfoldersize = Format(lTotalFileSizes / 1024 / 1024, "###,0.00") & " MB"
                b.Nooffiles = folderDfilecount
                b.batchid = DBatchid
                b.datime = ddtime
                b.status = "Success"
                info2.Add(b)
                Dim client = New WebClient()
                client.Headers("Content-Type") = "application/json"
                client.Encoding = System.Text.Encoding.UTF8
                Dim bool2 As Boolean = IO.File.Exists(dwnpath)
                If bool2 = True Then
                    Dim uristring1 = File.ReadAllText(dwnpath)
                    Dim fileinfoss As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(uristring1)
                    If uristring1 <> "" Then
                        Dim jpaths = uristring1.Substring(0, uristring1.Length - 2)
                        Dim jsond As String = JsonConvert.SerializeObject(info2, Settings)
                        Dim djson As String = jsond.Substring(1)
                        Dim savejsond = jpaths + "," + djson.Replace(" ", "")
                        Dim savepaths = System.Reflection.Assembly.GetEntryAssembly().Location
                        savepaths = Path.GetDirectoryName(savepaths)
                        System.IO.File.Delete(dwnpath)
                        Dim sources = savepaths + "\downloadjson"
                        If Not Directory.Exists(sources) Then
                            Directory.CreateDirectory(sources)
                        End If
                        Dim filelocations = sources & "\" & "Downloaddata.txt"
                        Using sw As StreamWriter = New StreamWriter(filelocations, True)
                            sw.Write(savejsond)
                        End Using
                        If movepath <> "" Then
                            MoveFiles(selectpath, movepath)
                        Else
                            frmdes.wrongpassDownload.Content = "Choose Destination Path"
                            frmdes.wrongpassDownload.Visibility = Visibility.Visible
                        End If
                    Else
                        Dim jsond As String = JsonConvert.SerializeObject(info2, Settings)
                        Dim savepaths = System.Reflection.Assembly.GetEntryAssembly().Location
                        savepaths = Path.GetDirectoryName(savepaths)
                        Dim sources = savepaths + "\downloadjson"
                        System.IO.File.Delete(dwnpath)
                        If Not Directory.Exists(sources) Then
                            Directory.CreateDirectory(sources)
                        End If
                        Dim filelocation = sources & "\" & "Downloaddata.txt"
                        Using sw As StreamWriter = New StreamWriter(filelocation, True)
                            sw.Write(jsond)
                        End Using
                        If movepath <> "" Then
                            MoveFiles(selectpath, movepath)
                        Else
                            frmdes.wrongpassDownload.Content = "Choose Destination Path"
                            frmdes.wrongpassDownload.Visibility = Visibility.Visible
                        End If
                    End If
                Else
                    Dim jsond As String = JsonConvert.SerializeObject(info2, Settings)
                    Dim savepaths = System.Reflection.Assembly.GetEntryAssembly().Location
                    savepaths = Path.GetDirectoryName(savepaths)
                    Dim sources = savepaths + "\downloadjson"
                    If Not Directory.Exists(sources) Then
                        Directory.CreateDirectory(sources)
                    End If
                    Dim filelocation = sources & "\" & "Downloaddata.txt"
                    Using sw As StreamWriter = New StreamWriter(filelocation, True)
                        sw.Write(jsond)
                    End Using
                    If movepath <> "" Then
                        MoveFiles(selectpath, movepath)
                    Else
                        frmdes.wrongpassDownload.Content = "Choose Destination Path"
                        frmdes.wrongpassDownload.Visibility = Visibility.Visible
                    End If
                End If
            Else
                frmdes.wrongpassDownload.Content = "Enter Password"
                frmdes.wrongpassDownload.Visibility = Visibility.Visible
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Sub MoveFiles(ByVal sourcePath As String, ByVal DestinationPath As String)
        Try
            Dim dcount = 0
            If (Directory.Exists(sourcePath)) Then
                For Each fName As String In Directory.GetFiles(sourcePath, "*", IO.SearchOption.AllDirectories)
                    If File.Exists(fName) Then
                        Dim dFile As String = ""
                        dFile = Path.GetFileName(fName)
                        Dim dFilePath As String = ""
                        'b.extension = System.IO.Path.GetExtension(fName).Replace(".", "")
                        dcount += 1
                        dFilePath = DestinationPath + "\" + dFile
                        If Not IO.File.Exists(dFilePath) Then
                            File.Copy(fName, dFilePath)
                        End If
                    End If
                Next
                custommsgbox.showCustomMessageBox("Info", "File Download Successfully")
                'MsgBox("File Download Successfully", vbOKOnly, "STANDALONE EXPLORER:Notification")
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub ListView1_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles ListView1.ItemCheck
        Try
            For i As Integer = 0 To ListView1.SelectedItems.Count - 1
                'RichTextBox1.AppendText(ListView1.Items(i).Text & " | " & ListView1.Items(i).SubItems(1).Text & vbNewLine)
            Next
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub ListView1_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseClick
        Try
            For i As Integer = 0 To ListView1.SelectedItems.Count - 1
                If ListView1.SelectedItems.Count = 0 Then
                    Label4.Visible = Visibility.Hidden
                Else
                    Label4.Text = ListView1.SelectedItems.Count & " " & " Items Selected"
                End If
            Next
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    'Private Sub ListView1_Click(sender As Object, e As EventArgs) Handles ListView1.Click
    '    Try
    '        For i As Integer = 0 To ListView1.SelectedItems.Count - 1
    '            'RichTextBox1.AppendText(ListView1.Items(i).Text & " | " & ListView1.Items(i).SubItems(1).Text & vbNewLine)
    '        Next
    '    Catch ex As Exception

    '    End Try
    'End Sub

End Class

