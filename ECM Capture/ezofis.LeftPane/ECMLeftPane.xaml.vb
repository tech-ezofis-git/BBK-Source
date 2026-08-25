Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Media.Animation
Imports System.IO
Imports System.Collections.ObjectModel
Imports Leadtools.WinForms
Imports Leadtools.Codecs
Imports Leadtools
Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data
Imports System.Globalization
Imports System.Resources
Imports ezofis.UserControl.CAC
Public Class ECMLeftPane
#Region "Variables"
    Public Shared ListOfTifFiles As New ObservableCollection(Of ListViewItem)
    Public Shared ListOfOtherFiles As New ObservableCollection(Of ListViewItem)
    Dim ImageBrowse As New System.ComponentModel.BackgroundWorker()
    Dim SplitMerge As New System.ComponentModel.BackgroundWorker()
    Dim filelist As New List(Of IndexingField)
    Public Shared Imaging As String
    Public Shared CreateOnId As String
    Public _bitsPerPixel As Integer
    Public _fileFormat As RasterImageFormat
    Shared Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("Database"), NameValueCollection)
    Dim WorkerProcess As String = ""
    Dim fnme As String = ""
    Dim _currentFileName As String = ""
    Dim Frmpagenumber As Integer
    Shared CAC As New CACserviceClient
    Public Shared leftpaneselectioncall As Boolean = True
    Dim reminderAnim As Storyboard
    Shared SettingPath As String = ""
    Shared ezTempField = New List(Of eZTemplateField)
    Private Delegate Sub OtherUpdateMyDelegatedelegate(ByVal item As String)
    Private Delegate Sub TifUpdateMyDelegatedelegate(ByVal item As String)
    Public Event Tif_SelectionChanged As RoutedEventHandler
    Public Event Other_SelectionChanged As RoutedEventHandler
    Public Event Tif_unchecked As RoutedEventHandler
    Private startPoint As Point
    Dim FileLst As List(Of String)
    Dim own As New Window
    Dim barcode As New barcoderead
#End Region



    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
    End Sub
    Private Sub otherselectall()
        If otherselect.IsChecked.HasValue And otherselect.IsChecked.Value Then
            lstOther.SelectAll()
        Else
            lstOther.UnselectAll()
        End If
    End Sub
    Private Sub OnSelectAllChanged()
        If selectAll.IsChecked.HasValue And selectAll.IsChecked.Value Then
            lstTif.SelectAll()
        Else
            lstTif.UnselectAll()
        End If
    End Sub
    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try
            Dim eventhan As New ComponentModel.DoWorkEventHandler(AddressOf SplitMerge_DoWork)
            RemoveHandler SplitMerge.DoWork, eventhan
            AddHandler SplitMerge.DoWork, eventhan
            Dim runeve As New ComponentModel.RunWorkerCompletedEventHandler(AddressOf SplitMerge_RunWorkerCompleted)
            RemoveHandler SplitMerge.RunWorkerCompleted, runeve
            AddHandler SplitMerge.RunWorkerCompleted, runeve
            Dim mous As New MouseButtonEventHandler(AddressOf MouseRightButtonDown_Click)
            RemoveHandler Me.MouseRightButtonDown, mous
            AddHandler Me.MouseRightButtonDown, mous
            lstTif.ItemsSource = ListOfTifFiles
            'shankar
            'For i = 0 To ListOfTifFiles.Count - 1
            '    Dim newitem = New ListViewItem()
            '    newitem.Content = ListOfTifFiles(i).ToString
            '    lstTif.Items.Add(newitem)
            'Next
            lstOther.ItemsSource = ListOfOtherFiles
            Dim imgeventhan As New ComponentModel.DoWorkEventHandler(AddressOf ImageBrowse_DoWork)
            RemoveHandler ImageBrowse.DoWork, imgeventhan
            AddHandler ImageBrowse.DoWork, imgeventhan
            Dim imgruneve As New ComponentModel.RunWorkerCompletedEventHandler(AddressOf ImageBrowse_RunWorkerCompleted)
            RemoveHandler ImageBrowse.RunWorkerCompleted, imgruneve
            AddHandler ImageBrowse.RunWorkerCompleted, imgruneve
            Refresh()
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Function GetERSPath() As Boolean
        Try
            Dim CAC As New CACserviceClient
            Dim host As String = ""
            Dim LocalHostaddress As String = ""
            Try
                Dim strHostName As String = System.Net.Dns.GetHostName()
                Dim iphe As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(strHostName)
                For Each ipheal As System.Net.IPAddress In iphe.AddressList
                    If ipheal.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
                        LocalHostaddress = ipheal.ToString()
                    End If
                Next
                'LocalHostaddress = "192.168.001.055"
            Catch ex As Exception
                MessageBox.Show("Ip Error")
                Return False
            End Try
            'LocalHostaddress = "192.168.001.001"
            If LocalHostaddress <> "" Then
                Dim obj As New List(Of eZERSInfo)
                Dim cabinfo = CAC.SelectedeZCabinetList("cabinetid", ECMRightPane.cabinetid.ToString())
                obj = CAC.SelectedeZERSInfoList("ERSId", cabinfo(0).ERSId.ToString)
                If obj.Count <> 0 Then
                    SettingPath = obj(0).SettingPath
                    Return True
                Else
                    Return False
                End If
            Else
                MessageBox.Show("Ip Error")
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Sub createfolder(ByVal outfile As String)
        If IO.Directory.Exists(outfile) = False Then
            IO.Directory.CreateDirectory(outfile)
        End If
    End Sub
    Public Shared Sub SendToExport(tiffiles As List(Of String), otherfiles As List(Of String))
        Try
            '  Dim fileinfo As System.IO.FileInfo
            Dim dirinfo As System.IO.DirectoryInfo
            Dim Batch As String = DateTime.Now.ToString("MMddyyyyhhmmss")
            Dim DestinationPath As String = ""
            Dim NoOfSync As Integer = 0
            Dim totofsync As Integer = 0
            dirinfo = New System.IO.DirectoryInfo(Imaging)
            Dim flag As Integer = 0
            '  For Each fileinfo In dirinfo.GetFiles("*.*").OrderBy(Function(i) i.Name)
            For i As Int16 = 0 To tiffiles.Count - 1
                Dim fileinfo As FileInfo = New FileInfo(Imaging + "\" + tiffiles(i))
                If LCase(fileinfo.Extension) = ".tif" Or LCase(fileinfo.Extension) = ".tiff" Then
                    totofsync += 1
                    pdfilename = ""
                    If CheckIndexing(fileinfo.Name, ECMRightPane.templateid.ToString()) Then
                        flag = 1
                        If pdfilename = "" Then
                            pdfilename = fileinfo.Name
                        Else
                            pdfilename = pdfilename + Path.GetExtension(fileinfo.Name)
                        End If
                        Dim SharedPath As String = Appcon("IndexedFiles")
                        If SharedPath <> "" Then
                            DestinationPath = SharedPath + "\" + Environment.UserName + "\" + Batch + "\" + ECMRightPane.templateid.ToString()
                        Else
                            DestinationPath = Imaging.Replace("\imaging", "\Indexedimaging\" + ECMRightPane.templateid.ToString())
                        End If
                        DestinationPath = DestinationPath + "\" + pdfilename
                        createfolder(Path.GetDirectoryName(DestinationPath))
                        pdfilename = Path.GetFileName(DestinationPath)
                        If File.Exists(DestinationPath) Then
                            File.Delete(DestinationPath)
                        End If
                        '    MsgBox(Imaging + "\" + fileinfo.Name + "," + Imaging.Replace("\imaging", "\Indexedimaging") + "\" + fileinfo.Name)
                        File.Copy(Imaging + "\" + fileinfo.Name, DestinationPath)
                        '  MsgBox("File Copied")
                        If File.Exists(DestinationPath) Then
                            Dim tblname = "eZCA_" + ECMRightPane.cabinetid.ToString() + "_" + ECMRightPane.templateid.ToString() + "_stage"
                            Dim sqlQuery As String = "update " + tblname + " set ifilepath= Replace(ifilepath,'\imaging','\Indexedimaging\" + ECMRightPane.templateid.ToString() + "')," +
"ifilename= Replace(ifilename,'\imaging\" + fileinfo.Name.Replace("'", "''") + "','\Indexedimaging\" + ECMRightPane.templateid.ToString() + "\" + pdfilename.Replace("'", "''") + "')" +
" where ifilename='" + Imaging.Replace("'", "''") + "\" + fileinfo.Name.Replace("'", "''") + "'"
                            If CAC.InsertAndUpdate(sqlQuery) = 1 Then '
                                Dim query As String = "select * from ezScannedImg where Ifilepath like  '%" & pdfilename.Replace("'", "''") & "' "
                                Dim ds As DataSet = CAC.GetDatasetByQuery(query)
                                If ds.Tables.Count <> 0 Then
                                    If ds.Tables(0).Rows.Count <> 0 Then
                                        Dim qu As String = "update [ezScannedImg] set [Status]='3',[UpdatedOn]='" + CAC.DateDateTimeToString(DateTime.Now, 1) + "'," +
"Ifilepath='" + pdfilename.Replace("'", "''") + "' where Ifilepath like  '%" & pdfilename.Replace("'", "''") & "' "
                                        CAC.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qu)
                                        ds.Dispose()
                                    Else
                                        Dim qu As String = "INSERT INTO [ezScannedImg] ([TemplateId],[pcname],[appname],[Ifilepath],[Status],[nopages],[CreatedOn]," +
"[CreatedBy]) VALUES('" + ECMRightPane.templateid.ToString() + "','" + Environment.UserName + "','','" & pdfilename.Replace("'", "''") & "'," +
"1,0,'" + CAC.DateDateTimeToString(DateTime.Now, 1) + "','" + ECMRightPane.CreateOnId + "')"
                                        CAC.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qu)
                                    End If
                                End If
                                File.Delete(Imaging + "\" + fileinfo.Name)
                                NoOfSync = NoOfSync + 1
                            Else
                                File.Delete(DestinationPath)
                            End If
                        End If
                    Else
                    End If
                    pdfilename = ""
                    DestinationPath = ""
                End If
            Next
            ' For Each fileinfo In dirinfo.GetFiles("*.*").OrderBy(Function(i) i.Name)
            For i As Int16 = 0 To otherfiles.Count - 1
                Dim fileinfo As FileInfo = New FileInfo(Imaging + "\" + otherfiles(i))
                If LCase(fileinfo.Extension) <> ".tif" Then
                    If LCase(fileinfo.Extension) <> ".tiff" Then
                        If LCase(fileinfo.Extension) <> ".db" Then
                            totofsync += 1
                            pdfilename = ""
                            If CheckIndexing(fileinfo.Name, ECMRightPane.templateid.ToString()) Then
                                flag = 1
                                If pdfilename = "" Then
                                    pdfilename = fileinfo.Name
                                Else
                                    pdfilename = pdfilename + Path.GetExtension(fileinfo.Name)
                                End If
                                Dim SharedPath As String = Appcon("IndexedFiles")
                                If SharedPath <> "" Then
                                    DestinationPath = SharedPath + "\" + Environment.UserName + "\" + Batch + "\" + ECMRightPane.templateid.ToString()
                                Else
                                    DestinationPath = Imaging.Replace("\imaging", "\Indexedimaging\" + ECMRightPane.templateid.ToString())
                                End If
                                createfolder(DestinationPath)
                                DestinationPath = DestinationPath + "\" + pdfilename
                                If File.Exists(DestinationPath) Then
                                    File.Delete(DestinationPath)
                                End If
                                File.Copy(Imaging + "\" + fileinfo.Name, DestinationPath)
                                If File.Exists(DestinationPath) Then
                                    Dim tblname = "eZCA_" + ECMRightPane.cabinetid.ToString() + "_" + ECMRightPane.templateid.ToString() + "_stage"
                                    Dim sqlQuery As String = "update " + tblname + " set ifilepath= Replace(ifilepath,'\imaging','\Indexedimaging\" + ECMRightPane.templateid.ToString() + "'),ifilename= Replace(ifilename,'\imaging\" + fileinfo.Name + "','\Indexedimaging\" + ECMRightPane.templateid.ToString() + "\" + pdfilename + "') where ifilename='" + Imaging + "\" + fileinfo.Name + "'"
                                    If CAC.InsertAndUpdate(sqlQuery) = 1 Then
                                        Dim query As String = "select * from ezScannedImg where Ifilepath like  '%" & pdfilename & "' "
                                        Dim ds As DataSet = CAC.GetDatasetByQuery(query)
                                        If ds.Tables.Count <> 0 Then
                                            If ds.Tables(0).Rows.Count <> 0 Then
                                                Dim qu As String = "update [ezScannedImg] set [Status]='3',[UpdatedOn]='" + CAC.DateDateTimeToString(DateTime.Now, 1) + "',Ifilepath='" + pdfilename + "' where Ifilepath like  '%" & pdfilename & "' "
                                                CAC.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qu)
                                                ds.Dispose()
                                            Else
                                                Dim qu As String = "INSERT INTO [ezScannedImg] ([TemplateId],[pcname],[appname],[Ifilepath],[Status],[nopages],[CreatedOn],[CreatedBy]) VALUES('" + ECMRightPane.templateid.ToString() + "','" + Environment.UserName + "','','" & pdfilename & "',1,0,'" + CAC.DateDateTimeToString(DateTime.Now, 1) + "','" + ECMRightPane.CreateOnId + "')"
                                                CAC.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qu)
                                            End If
                                        End If
                                        File.Delete(Imaging + "\" + fileinfo.Name)
                                        NoOfSync = NoOfSync + 1
                                    Else
                                        File.Delete(DestinationPath)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                pdfilename = ""
                DestinationPath = ""
            Next
            Try
                If flag = 1 Then
                    Dim qu As String = "INSERT INTO [eZScanBatch] ([Batch],[Status],[NoOfDocumet],[CreatedOn],[CreatedAt],[TemplateId],[CreatedBy]) VALUES('" + Batch + "',1," + NoOfSync.ToString() + ",'" + DateTime.Now.ToString() + "','" + Environment.MachineName + "','" + ECMRightPane.templateid.ToString() + "','" + ECMRightPane.CreateOnId + "')"
                    '  MessageBox.Show(qu)
                    CAC.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qu)
                End If
            Catch ex1 As Exception
                '   MessageBox.Show(ex1.Message.ToString())
            End Try
            Dim msg = "Total No. Of File(s) Indexed : " + totofsync.ToString + Environment.NewLine +
"Number Of File(s) Send To Export : " + NoOfSync.ToString
            If NoOfSync <> totofsync Then
                msg = msg + Environment.NewLine + "Mandatory Field(s) are not Indexed Properly..."
            End If
            MsgBox(msg)
            'MsgBox("File(b) send to Export")
            ' Refresh()
        Catch ex As Exception
            MessageBox.Show("From Send To Export : " + ex.Message)
        End Try
    End Sub
    Public Shared Function CheckIndexing(ByVal Filename As String, ByVal templateid As Integer) As Boolean
        Try
            Dim sdataset As DataSet
            Dim fieldlst = New List(Of eZTemplateField)
            ezTempField = New List(Of eZTemplateField)
            fieldlst = CAC.SelectedeZTemplateFieldList("TemplateId", templateid.ToString())
            sdataset = CAC.SelectedeZUserDefinedList(1, templateid, "*", "ifilename", Imaging + "\" + Filename)
            ezTempField = CAC.SelectedeZTemplateFieldListForPdfCreation("TemplateId", templateid.ToString())
            If sdataset.Tables(0).Rows.Count > 0 Then
                For i As Int16 = 0 To fieldlst.Count - 1
                    If sdataset.Tables(0).Rows(0).Item(fieldlst(i).FieldName.Trim()).ToString() = "" And fieldlst(i).Mandatory Then
                        Return False
                    End If
                Next
                If ezTempField.Count <> 0 Then
                    filldb(Imaging + "\" + Filename)
                Else
                    pdfilename = Filename
                End If
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function RmvSplChar(ByVal value As String) As String
        Dim res = ""
        Try
            res = value.Trim.Replace("/", "-").Replace(":", "-").Replace("\", "-").Replace("*", "-").
Replace("<", "-").Replace(">", "-").Replace("?", "-").Replace("|", "-").Trim(" ")
        Catch ex As Exception
            res = value
        End Try
        Return res
    End Function
    Public Shared Function filldb(ByVal filename As String) As Boolean
        Try
            Volume = ""
            Dim Check1 As Integer = 0
            Dim Check2 As Integer = 0
            Dim CAC As New CACserviceClient
            Dim sdataset As DataSet
            Dim mergestr = ""
            sdataset = CAC.SelectedeZUserDefinedList(1, ECMRightPane.templateid, "*", "ifilename", filename)
            If sdataset.Tables(0).Rows.Count > 0 Then
                If ezTempField.Count >= 2 Then
                    For i As Integer = 0 To ezTempField.Count - 2
                        If Not IsDBNull(sdataset.Tables(0).Rows(0).Item(ezTempField(i).FieldName.Trim().ToString())) Then
                            If Trim(sdataset.Tables(0).Rows(0).Item(ezTempField(i).FieldName.Trim().ToString())) <> "" Then
                                If ezTempField(i).DataTypeId = 5 Then
                                    Dim daTime = CDate(sdataset.Tables(0).Rows(0).Item(ezTempField(i).FieldName.Trim())).Date
                                    Dim stdt As String = CAC.DateStringToString(daTime, 0, System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern)
                                    If Volume = "" Then
                                        Volume = stdt
                                    Else
                                        Volume = Volume & "-" & stdt
                                    End If
                                    Check1 += 1
                                Else
                                    If Volume = "" Then
                                        Volume = RmvSplChar(sdataset.Tables(0).Rows(0).Item(ezTempField(i).FieldName.Trim()).ToString.Trim.Replace("/", "-").Trim(" ").Replace(":", "-").Trim(" ").Replace("\", "-").Trim(" ").Replace("*", "-").Trim(" ").Replace("<", "-").Trim(" ").Replace(">", "-"))
                                    Else
                                        Volume = Volume & "-" & RmvSplChar(sdataset.Tables(0).Rows(0).Item(ezTempField(i).FieldName.Trim()).ToString.Trim.Replace("/", "-").Trim(" ").Replace(":", "-").Trim(" ").Replace("\", "-").Trim(" ").Replace("*", "-").Trim(" ").Replace("<", "-").Trim(" ").Replace(">", "-"))
                                    End If
                                    Check1 += 1
                                End If
                            End If
                        End If
                    Next
                End If
                Dim stamp = Date.Now.ToString("yyyymmddhhmmssfff")
                pdfilename = RmvSplChar(stamp & "-" & sdataset.Tables(0).Rows(0).Item(ezTempField(ezTempField.Count - 1).FieldName.Trim()))
            End If
            Return True
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Function
    Public Shared Sub Refresh()
        Try
            Dim dinosaurs As New List(Of String)
            leftpaneselectioncall = False
            ListOfOtherFiles.Clear()

            ListOfTifFiles.Clear()
            Dim fileinfo As System.IO.FileInfo
            Dim dirinfo As System.IO.DirectoryInfo
            dirinfo = New System.IO.DirectoryInfo(Imaging)
            dinosaurs.Clear()
            For Each fileinfo In dirinfo.GetFiles("*.*").OrderBy(Function(i) i.Name)
                If LCase(fileinfo.Extension) = ".tif" Or LCase(fileinfo.Extension) = ".tiff" Then
                    dinosaurs.Add(fileinfo.Name)
                End If
            Next
            For i = 0 To dinosaurs.Count - 1
                Dim items As New ListViewItem
                items.Content = dinosaurs.Item(i).ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
                'If CheckIndexing(items.Content.ToString(), ECMRightPane.templateid.ToString()) Then
                '    items.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                'Else
                ''items.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                '  End If
                'items. = dinosaurs.Item(i).ToString().Replace("System.Windows.Controls.ListViewItem: ", "")

                ' ListOfTifFiles.Add(dinosaurs.Item(i).ToString)
                'Dim item As Object = Nothing
                'If Not ECMRightPane.FileChkDic.TryGetValue(dinosaurs.Item(i).ToString, item) Then
                '    ECMRightPane.FileChkDic.Add(dinosaurs.Item(i).ToString, False)
                'End If
                '===============================================================================

                If CheckIndexing(items.Content.ToString(), ECMRightPane.templateid.ToString()) Then
                    items.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                Else
                    items.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                End If
                ListOfTifFiles.Add(items)
            Next
            dinosaurs.Clear()
            For Each fileinfo In dirinfo.GetFiles("*.*").OrderBy(Function(i) i.Name)
                If LCase(fileinfo.Extension) <> ".tif" Then
                    If LCase(fileinfo.Extension) <> ".tiff" Then
                        If LCase(fileinfo.Extension).ToLower <> ".txt" Then
                            If LCase(fileinfo.Extension) <> ".db" Then
                                dinosaurs.Add(fileinfo.Name)
                            End If
                        End If
                    End If
                End If
            Next
            For i = 0 To dinosaurs.Count - 1
                Dim items As New ListViewItem
                items.Content = dinosaurs.Item(i).ToString.Replace("System.Windows.Controls.ListViewItem: ", "")
                ''If CheckIndexing(items.Content.ToString(), ECMRightPane.templateid.ToString()) Then
                ''    items.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                ''Else
                'items.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                '' End If
                'ListOfOtherFiles.Add(items)
                ''Dim item As Object = Nothing
                ''If Not ECMRightPane.FileChkDic.TryGetValue(dinosaurs.Item(i).ToString, item) Then
                ''    ECMRightPane.FileChkDic.Add(dinosaurs.Item(i).ToString, False)
                ''End If

                If CheckIndexing(items.Content.ToString(), ECMRightPane.templateid.ToString()) Then
                    items.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                Else
                    items.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                End If
                ListOfOtherFiles.Add(items)
                'Dim item As Object = Nothing
                'If Not ECMRightPane.FileChkDic.TryGetValue(dinosaurs.Item(i).ToString, item) Then
                '    ECMRightPane.FileChkDic.Add(dinosaurs.Item(i).ToString, False)
                'End If

            Next
            dinosaurs.Clear()
        Catch ex As Exception
        Finally
            leftpaneselectioncall = True
        End Try
    End Sub
    Public Sub FilesLoad(ByVal lst As List(Of IndexingField))
        Try
            If Not ImageBrowse.IsBusy Then
                If Not lst.Count = 0 Then
                    filelist.Clear()
                    filelist = lst
                    reminderAnim = TryCast(Me.Resources("BeginProgress"), Storyboard)
                    reminderAnim.Begin()
                    ImageBrowse.RunWorkerAsync()
                End If
            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub
    Private Sub OtherUpdateMyDelegateLabel(ByVal item As String)
        Dim items As New ListViewItem
        items.Content = item
        ListOfOtherFiles.Add(items)
    End Sub
    Private Sub TifUpdateMyDelegateLabel(ByVal item As String)
        'Changed By Musthanzir purpose
        'ListOfTifFiles.Add(item)
    End Sub
    'Private Shared Function CompareDinosByLength(ByVal x As String, ByVal y As String) As Integer
    '    If x Is Nothing Then
    '        If y Is Nothing Then
    '            ' If x is Nothing and y is Nothing, they're
    '            ' equal.
    '            Return 0
    '        Else
    '            ' If x is Nothing and y is not Nothing, y
    '            ' is greater.
    '            Return -1
    '        End If
    '    Else
    '        ' If x is not Nothing...
    '        '
    '        If y Is Nothing Then
    '            ' ...and y is Nothing, x is greater.
    '            Return 1
    '        Else
    '            '...and y is not Nothing, compare the
    '            'lengths of the two strings.
    '            'Dim retval As Integer = x.CompareTo(y)
    '            Dim retval As Integer = x.Length.CompareTo(y.Length)
    '            If retval <> 0 Then
    '                'If the strings are not of equal length,
    '                'the longer string is greater.
    '                Return retval
    '            Else
    '                'If the strings are of equal length,
    '                'sort them with ordinary string comparison.
    '                Return x.CompareTo(y)
    '            End If
    '        End If
    '    End If
    'End Function
    Public Movedfiles = 0
    Private Sub ImageBrowse_DoWork(ByVal Sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Try
            Dim dirinfo = ""
            Movedfiles = 0
            ' For i As Integer = 0 To filelist.Count - 1 filelist(i)
            For Each filels In filelist
                dirinfo = Path.GetDirectoryName(filels.FieldName)
                Dim ext As String = String.Empty
                If (Path.HasExtension(filels.FieldName)) Then
                    ext = Path.GetExtension(filels.FieldName)
                End If
                Dim newtiffilepath As String = String.Format("{0}\\{1}{2}", Imaging, Path.GetFileNameWithoutExtension(filels.FieldName), ext)
                Dim newtiffilepath1 As String = String.Format("{0}{1}", Path.GetFileNameWithoutExtension(filels.FieldName), ext)
AfterLoop:
                If File.Exists(newtiffilepath) Then
                    newtiffilepath = Path.GetFileNameWithoutExtension(newtiffilepath) + "_A" + ext
                    newtiffilepath1 = newtiffilepath
                    newtiffilepath = Imaging + "\" + newtiffilepath
                    GoTo AfterLoop
                Else
                    ' File.Copy(filelist(i), newtiffilepath)
                    Try
                        File.Copy(filels.FieldName, newtiffilepath)
                    Catch ex As Exception
                        '   MessageBox.Show(ex.Message)
                    End Try
                    Movedfiles = Movedfiles + 1

                    If Not File.Exists(newtiffilepath) Then
                        MessageBox.Show("The file Not imported :" + newtiffilepath)
                        If CAC.InsertAndUpdate("update ezBatchProcessing set Status=2,ImportedAt='" + Environment.MachineName.ToString() + "',[ImportedOn]='" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + "',Importedby='" + ECMRightPane.CreateOnId.ToString() + "' where BatchId='" + filels.BatchId + "'") > 0 Then
                        End If
                    End If

                    'Dim dirinfo As DirectoryInfo = New DirectoryInfo(Path.GetDirectoryName(filelist(i)))
                    'If dirinfo.Exists Then
                    '    If dirinfo.GetFiles().Count = 0 Then
                    '        dirinfo.Delete(True)
                    '    End If
                    'End If
                End If
                If ext = ".tif" Or ext = ".tiff" Or ext = ".TIF" Or ext = ".TIFF" Then
                    Dim UpdateMyDelegate As New TifUpdateMyDelegatedelegate(AddressOf TifUpdateMyDelegateLabel)
                    lstTif.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, UpdateMyDelegate, newtiffilepath1)
                Else
                    Dim UpdateMyDelegate As New OtherUpdateMyDelegatedelegate(AddressOf OtherUpdateMyDelegateLabel)
                    lstOther.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, UpdateMyDelegate, newtiffilepath1)
                    'Dim newfilename As String
                    'Dim TiffFileName As String
                    'newfilename = filelist(i).ToString
                    'TiffFileName = Imaging + "\Temp\" & newtiffilepath1.Replace(Path.GetExtension(newfilename), ".tiff")
                    'If newfilename.Substring(newfilename.Length - 3).ToLower = "pdf" Then
                    '    pdfThumbnailview(newtiffilepath, TiffFileName.Replace(".tiff", ""))
                    'ElseIf newfilename.Substring(newfilename.Length - 4).ToLower = "docx" Then
                    '    docconverter(newtiffilepath, newtiffilepath1, "docx")
                    '    pdfThumbnailview(Imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                    'ElseIf newfilename.Substring(newfilename.Length - 3).ToLower = "doc" Then
                    '    docconverter(newtiffilepath, newtiffilepath1, "doc")
                    '    pdfThumbnailview(Imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                    'ElseIf newfilename.Substring(newfilename.Length - 3).ToLower = "xls" Then
                    '    docconverter(newtiffilepath, newtiffilepath1, "xls")
                    '    pdfThumbnailview(Imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                    'ElseIf newfilename.Substring(newfilename.Length - 4).ToLower = "xlsx" Then
                    '    docconverter(newtiffilepath, newtiffilepath1, "xlsx")
                    '    pdfThumbnailview(Imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                    'ElseIf newfilename.Substring(newfilename.Length - 3).ToLower = "ppt" Then
                    '    docconverter(newtiffilepath, newtiffilepath1, "ppt")
                    '    pdfThumbnailview(Imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                    'ElseIf newfilename.Substring(newfilename.Length - 4).ToLower = "pptx" Then
                    '    docconverter(newtiffilepath, newtiffilepath1, "pptx")
                    '    pdfThumbnailview(Imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                    'End If
                    If IO.File.Exists(Imaging + "\Convert_Pdf\Imaging.pdf") = True Then System.IO.File.Delete(Imaging + "\Convert_Pdf\Imaging.pdf")
                    'If Not newfilename(i) = String.Empty Then
                    '    If Path.GetExtension(newfilename.ToString).ToLower <> ".tif" Or Path.GetExtension(newfilename.ToString).ToLower <> ".tiff" Then
                    '    End If
                    'End If
                End If
            Next
            Dim Importdirinfo As New DirectoryInfo(dirinfo)
            If Importdirinfo.EnumerateFiles().Count = 0 Then
                Importdirinfo.Delete()
            End If
            filelist.Clear()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ImageBrowse_RunWorkerCompleted(ByVal Sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        reminderAnim.Stop()
        Refresh()
    End Sub
    Private Sub SplitMerge_DoWork(ByVal Sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Try
            Dim infodd As CodecsImageInfo
            If WorkerProcess <> "Merge" AndAlso WorkerProcess <> "SplitBarcode" Then
                _codecs = New RasterCodecs
                infodd = _codecs.GetInformation(_currentFileName, True)
            End If
            If WorkerProcess = "Merge" Then
                MergeFiles()
            ElseIf WorkerProcess = "SplitCurrent" Then
                SplitWithCurrentPage(_currentFileName, infodd, infodd.TotalPages, Frmpagenumber, _fileFormat, _bitsPerPixel)
            ElseIf WorkerProcess = "SplitSingle" Then
                SplitWithSinglePage(_currentFileName, infodd, infodd.TotalPages, _fileFormat, _bitsPerPixel)
            ElseIf WorkerProcess = "SplitBlack" Then
                'SplitWithBlackPage(_currentFileName, infodd, infodd.TotalPages, _fileFormat, _bitsPerPixel)
                'mar18 by raja
                SplitWithWhitePage(_currentFileName, infodd, infodd.TotalPages, _fileFormat, _bitsPerPixel)
            ElseIf WorkerProcess = "SplitBlank" Then
                SplitWithBlankPage(_currentFileName, infodd, infodd.TotalPages, _fileFormat, _bitsPerPixel)
            ElseIf WorkerProcess = "SplitBarcode" Then
                SplitWithBarcodePage()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Function SplitWithBarcodePage()
        Try
            Dim obj As New List(Of eZTempBarcode)
            obj = CAC.SelectedeZTempBarcodeList("TemplateId", ECMRightPane.templateid)
            If obj.Count <> 0 Then
                BarcodeStartsWith = obj(0).StartsWith.ToString
                BarcodeEndsWith = obj(0).EndWith.ToString
                BarcodeType = obj(0).BarcodeType.ToString
            End If
            barcodecount = 0
            'Application.Current.Dispatcher.Invoke(DirectCast(Function()
            Try
                For i As Int16 = 0 To FileLst.Count - 1
                    _currentFileName = Imaging + "\" + FileLst(i).ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
                    ' Dim barcode As New barcoderead
                    Dim dt = barcode.barcodee(_currentFileName, Imaging, _fileFormat, _bitsPerPixel)



                    'Dim dt As New DataTable
                    'dt = barcode.barcodee(barcodeTiflst(i).ToString, Imaging, _fileFormat, _bitsPerPixel)
                    'If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    '    CurrentFnInRightPane = barcodeTiflst(i).ToString
                    '    SetIndexingControlValues(obj(0).BarcodeField, dt.Rows(0).Item(0))
                    '    Btn_Click(Nothing, New System.Windows.RoutedEventArgs)
                    '    SaveRecords(barcodeTiflst(i).ToString)
                    '    Result = "Success"
                    'End If
                Next





                'SaveBarcodeValues(dt)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            'End Function, Action))
        Catch ex As Exception

        End Try
    End Function



    'Public Function SaveBarcodeValues(ByVal dt As DataTable)
    '    Try
    '        If dt IsNot Nothing Then
    '            For Each rw As DataRow In dt.Rows
    '                Dim val = rw("barcodevalue").ToString.Split("-")
    '                If val.Count = 3 Then
    '                    If ECMRightPane.templateid = 5 Then
    '                        Dim filname As String = Imaging + "\" + rw("filename").ToString
    '                        If filname <> "" Then
    '                            Dim sdataset = CAC.SelectedeZUserDefinedList(1, ECMRightPane.templateid, "*", "ifilename", filname)
    '                            If sdataset.Tables.Count > 0 And sdataset.Tables(0).Rows.Count > 0 Then
    '                                Dim qry = "update ezca_" + ECMRightPane.cabinetid.ToString + "_" + ECMRightPane.templateid.ToString + "_stage set "
    '                                qry += "[Document Type]='" + val(1).ToString + "',[Document Number]='" + val(2).ToString + "',"
    '                                qry += "[Customer Id]='" + val(0).ToString + "' where itemid='" + sdataset.Tables(0).Rows(0).Item(0).ToString + "'"
    '                                If Not CAC.InsertAndUpdateAndDeleteeZUserDefined(qry) = 1 Then
    '                                    MsgBox(qry)
    '                                End If
    '                            Else
    '                                GetERSPath()
    '                                Dim sqlstring As String = ""
    '                                sqlstring += "Insert into ezca_" + ECMRightPane.cabinetid.ToString + "_" + ECMRightPane.templateid.ToString + "_stage ("
    '                                sqlstring += "[Customer Id],[Document Type],[Document Number],"
    '                                sqlstring += "templateid,ifilepath,ifilename ,ifiletype,version ,createdby,updatedby,dtitle,dauthor,dsubject,dkeywords,checkout,checkoutpath,checkoutby,dstatus,dsize,nopages,CreatedOn,UpdatedOn,Isdeleted,ersid,ezfrom) values("
    '                                sqlstring += "'" + val(0).ToString + "','" + val(1).ToString + "','" + val(2).ToString + "',"
    '                                sqlstring += "'" + ECMRightPane.templateid.ToString() + "', '" + Imaging + "', '" + filname + "', 'tif', '0' ,'" + ECMRightPane.CreateOnId + "', '" + ECMRightPane.CreateOnId + "', '', '', '', '', '', '', '0', '', '', '', '" + CAC.DateDateTimeToString(DateTime.Now, 1) + "', '" + CAC.DateDateTimeToString(DateTime.Now, 1) + "', '0'," & Ersid & ",'ECM-CAPTURE(" + Environment.MachineName + ")')"
    '                                If Not CAC.InsertAndUpdateAndDeleteeZUserDefined(sqlstring) = 1 Then
    '                                    MsgBox(sqlstring)
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception
    '        MsgBox("Error While Saving Barcode Values : " + ex.ToString)
    '    End Try
    'End Function
    Private Sub SplitMerge_RunWorkerCompleted(ByVal Sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        reminderAnim.Stop()
        Refresh()
    End Sub
    Public Sub LastRecord()
        Try
            If lstTif.SelectedItems.Count <> 0 Then
                If lstTif.Items.Count <> 0 Then
                    If lstTif.SelectedIndex <> lstTif.Items.Count - 1 Then
                        lstTif.SelectedIndex = lstTif.Items.Count - 1
                    End If
                End If
            ElseIf lstOther.SelectedItems.Count <> 0 Then
                If lstOther.Items.Count <> 0 Then
                    If lstOther.SelectedIndex <> lstOther.Items.Count - 1 Then
                        lstOther.SelectedIndex = lstOther.Items.Count - 1
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub PreviousRecord()
        Try
            If lstOther.SelectedItems.Count <> 0 Then
                If lstOther.Items.Count <> 0 Then
                    If lstOther.SelectedIndex <> 0 Then
                        lstOther.SelectedIndex = lstOther.SelectedIndex - 1
                    End If
                End If
            ElseIf lstTif.SelectedItems.Count <> 0 Then
                If lstTif.Items.Count <> 0 Then
                    If lstTif.SelectedIndex <> 0 Then
                        lstTif.SelectedIndex = lstTif.SelectedIndex - 1
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub NextRecord()
        Try
            If lstTif.SelectedItems.Count <> 0 Then
                If lstTif.Items.Count <> 0 Then
                    If lstTif.SelectedIndex <> lstTif.Items.Count - 1 Then
                        lstTif.SelectedIndex = lstTif.SelectedIndex + 1
                    End If
                End If
            ElseIf lstOther.SelectedItems.Count <> 0 Then
                If lstOther.Items.Count <> 0 Then
                    If lstOther.SelectedIndex <> lstOther.Items.Count - 1 Then
                        lstOther.SelectedIndex = lstOther.SelectedIndex + 1
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub FirstRecord()
        Try
            If lstOther.SelectedItems.Count <> 0 Then
                If (lstOther.SelectedIndex <> 0) Then
                    If lstOther.Items.Count <> 0 Then
                        lstOther.SelectedIndex = 0
                    End If
                End If
            ElseIf lstTif.SelectedItems.Count <> 0 Then
                If (lstTif.SelectedIndex <> 0) Then
                    If lstTif.Items.Count <> 0 Then
                        lstTif.SelectedIndex = 0
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub lstTif_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        RaiseEvent Tif_SelectionChanged(sender, e)
        'selectedindex = lstTif.SelectedIndex
        'Try
        '    Dim filname As String = lstTif.SelectedItems(selectedindex).ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
        '    Dim list = DirectCast(lstTif.SelectedItems(selectedindex), ListViewItem)
        '    If CheckIndexing(filname, ECMRightPane.templateid.ToString()) Then
        '        list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
        '    Else
        '        list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
        '    End If
        'Catch ex As Exception
        'End Try
    End Sub
    Private Sub lstOther_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        RaiseEvent Other_SelectionChanged(sender, e)
    End Sub
    'Public Sub pdfThumbnailview(ByVal outputFilePath As String, ByVal filename As String)
    '    Try
    '        Dim f As New SautinSoft.PdfFocus()
    '        Dim pdfPath As String = outputFilePath
    '        Dim imagePath As String = filename & ".tiff"
    '        If IO.Directory.Exists(Imaging + "\Temp") = False Then IO.Directory.CreateDirectory(Imaging + "\Temp")
    '        f.OpenPdf(pdfPath)
    '        If f.PageCount > 0 Then
    '            f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Tiff
    '            f.ImageOptions.Dpi = 120
    '            If f.ToImage(imagePath, 1) = 0 Then
    '                f.ClosePdf()
    '            End If
    '        End If
    '    Catch ex As Exception
    '    End Try
    'End Sub

    Public Sub pdfThumbnailview(ByVal outputFilePath As String, ByVal filename As String)
        Try
            Dim f As New SautinSoft.PdfFocus()
            Dim pdfPath As String = outputFilePath
            Dim imageDirectory As String = IO.Path.Combine(Imaging, "Temp")
            Dim imageFormat As System.Drawing.Imaging.ImageFormat = System.Drawing.Imaging.ImageFormat.Tiff

            ' Ensure the output directory exists
            If Not IO.Directory.Exists(imageDirectory) Then
                IO.Directory.CreateDirectory(imageDirectory)
            End If

            ' Open the PDF document
            f.OpenPdf(pdfPath)

            ' Check if the PDF has pages
            If f.PageCount > 0 Then
                ' Loop through each page and save as an image
                For pageIndex As Integer = 1 To f.PageCount
                    Dim imagePath As String = IO.Path.Combine(imageDirectory, $"{filename}_Page{pageIndex}.tiff")

                    ' Set the image options
                    f.ImageOptions.ImageFormat = imageFormat
                    f.ImageOptions.Dpi = 120

                    ' Save the current page as an image
                    If f.ToImage(imagePath, pageIndex) <> 0 Then
                        Throw New Exception("Error converting page to image.")
                    End If
                Next
            End If

            ' Close the PDF document
            f.ClosePdf()

        Catch ex As Exception
            ' Handle exceptions as needed (e.g., log the error, display a message)
            Console.WriteLine("An error occurred: " & ex.Message)
        End Try
    End Sub

    Public Sub docconverter(ByVal filepath As String, ByVal targetpath As String, ByVal Ext As String)
        Try
            Dim u As New SautinSoft.UseOffice
            Dim inputFilePath As String = filepath
            Dim outputFilePath As String = Imaging + "\Convert_Pdf\Imaging.pdf"
            If IO.Directory.Exists(Imaging + "\Convert_Pdf") = False Then IO.Directory.CreateDirectory(Imaging + "\Convert_Pdf")
            Dim ret As Integer = u.InitWord()
            If ret = 1 Then
                Return
            End If
            If Not File.Exists(outputFilePath) Then
                If Ext = "doc" Then
                    ret = u.ConvertFile(inputFilePath, outputFilePath, SautinSoft.UseOffice.eDirection.DOC_to_PDF)
                    u.CloseWord()
                ElseIf Ext = "docx" Then
                    ret = u.ConvertFile(inputFilePath, outputFilePath, SautinSoft.UseOffice.eDirection.DOCX_to_PDF)
                    u.CloseWord()
                ElseIf Ext = "xls" Then
                    ret = u.ConvertFile(inputFilePath, outputFilePath, SautinSoft.UseOffice.eDirection.XLS_to_PDF)
                    u.CloseExcel()
                ElseIf Ext = "xlsx" Then
                    ret = u.ConvertFile(inputFilePath, outputFilePath, SautinSoft.UseOffice.eDirection.XLSX_to_PDF)
                    u.CloseExcel()
                ElseIf Ext = "ppt" Then
                    ret = u.ConvertFile(inputFilePath, outputFilePath, SautinSoft.UseOffice.eDirection.PPT_to_PDF)
                    u.ClosePowerPoint()
                ElseIf Ext = "pptx" Then
                    ret = u.ConvertFile(inputFilePath, outputFilePath, SautinSoft.UseOffice.eDirection.PPTX_to_PDF)
                    u.ClosePowerPoint()
                End If
            End If
            u.CloseWord()
        Catch ex As Exception
        End Try
    End Sub
    Public WriteOnly Property BGColor() As String
        Set(ByVal value As String)
            gr.Background = TryCast(New BrushConverter().ConvertFromString(value), SolidColorBrush)
        End Set
    End Property
    Public WriteOnly Property LabelColor() As String
        Set(ByVal value As String)
            selectAll.Foreground = TryCast(New BrushConverter().ConvertFromString(value), SolidColorBrush)
            lb1.Foreground = TryCast(New BrushConverter().ConvertFromString(value), SolidColorBrush)
            otherselect.Foreground = TryCast(New BrushConverter().ConvertFromString(value), SolidColorBrush)
            lb2.Foreground = TryCast(New BrushConverter().ConvertFromString(value), SolidColorBrush)
        End Set
    End Property
    Private Shared Function FindAnchestor(Of T As DependencyObject)(ByVal current As DependencyObject) As T
        Do
            If TypeOf current Is T Then
                Return DirectCast(current, T)
            End If
            current = VisualTreeHelper.GetParent(current)
        Loop While current IsNot Nothing
        Return Nothing
    End Function
    Private Sub lstTif_PreviewMouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles lstTif.PreviewMouseLeftButtonDown
        startPoint = e.GetPosition(Nothing)
    End Sub
    Private Sub lstTif_PreviewMouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles lstTif.PreviewMouseMove
        Try
            Dim mousePos As Point = e.GetPosition(Nothing)
            Dim diff As Vector = startPoint - mousePos
            If e.LeftButton = MouseButtonState.Pressed AndAlso Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance AndAlso Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance Then
                If lstTif.SelectedItems.Count = 0 Then
                    Return
                End If
                Dim files As List(Of String) = GetSelection()
                Dim dataFormat As String = DataFormats.FileDrop
                Dim dataObject As New DataObject(dataFormat, files)
                DragDrop.DoDragDrop(lstTif, dataObject, DragDropEffects.Copy)
            End If
        Catch ex As Exception
        End Try
    End Sub
    'Private Shared Function FindAncestor(Of T As DependencyObject)(ByVal current As DependencyObject) As T
    '    Do
    '        If TypeOf current Is T Then
    '            Return DirectCast(current, T)
    '        End If
    '        current = VisualTreeHelper.GetParent(current)
    '    Loop While current IsNot Nothing
    '    Return Nothing
    'End Function
    Private Sub lstTif_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles lstTif.Drop
        If e.Data.GetDataPresent("myFormat") Then
            Dim contact As String = TryCast(e.Data.GetData("myFormat"), String)
            Dim listView As ListView = TryCast(sender, ListView)
            listView.Items.Add(contact)
        End If
    End Sub
    Private Sub lstTif_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles lstTif.DragEnter
        If Not e.Data.GetDataPresent("myFormat") OrElse sender = e.Source Then
            e.Effects = DragDropEffects.None
        End If
    End Sub
    Public Function GetSelection() As List(Of String)
        Dim res As New List(Of String)
        If (lstTif.SelectedIndex <> -1) Then
            For i As Int16 = 0 To lstTif.SelectedItems.Count - 1
                res.Add(lstTif.SelectedItems(i).ToString.Replace("System.Windows.Controls.ListViewItem: ", ""))
            Next
        End If
        Return res
    End Function
    Private Sub gr_SizeChanged(ByVal sender As System.Object, ByVal e As System.Windows.SizeChangedEventArgs) Handles gr.SizeChanged
        'Try
        '    tifgrid.Width = gr.Width
        '    othersgrid.Width = gr.Width
        'Catch ex As Exception
        'End Try
    End Sub

    Private Function MergeFiles() As Boolean
        Dim Result As Boolean
        Try
            If FileLst.Count = 0 Then
                Result = False
            ElseIf FileLst.Count = 1 Then
                Result = True
            Else
                Dim loader As New ImageFileLoader()
                Dim MergeViewer As New RasterImageViewer
                RasterCodecs.Startup()
                Dim name As String = Imaging + "\" + FileLst(0).Replace("System.Windows.Controls.ListViewItem: ", "")
                _codecs = New RasterCodecs
                For i As Int16 = 0 To FileLst.Count - 1
                    Dim info As CodecsImageInfo = _codecs.GetInformation(Imaging + "\" + FileLst(i), True)
                    If (loader.Load(own, _codecs, True, 1, info.TotalPages, Imaging + "\" + FileLst(i))) Then
                        If (IsNothing(MergeViewer.Image)) Then
                            loader.Image.MakeRegionEmpty()
                            MergeViewer.Image = loader.Image
                        Else
                            MergeViewer.Image.AddPages(loader.Image, 1, loader.Image.PageCount)
                        End If
                    End If
                    Kill(Imaging + "\" + FileLst(i))
                Next
                _codecs.Save(MergeViewer.Image, name, _fileFormat, _bitsPerPixel, 1, MergeViewer.Image.PageCount, 1, CodecsSavePageMode.Overwrite)
                Result = True
                FileLst.Clear()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
        End Try
        Return Result
    End Function
    Public Function LoadImages(ByVal FormName As Window, ByVal fileName As String, ByVal frmpage As Integer, ByVal ToPage As Integer) As ImageInformation
        Dim loader As ImageFileLoader = New ImageFileLoader()
        RasterCodecs.Startup()
        _codecs = New RasterCodecs()
        Try
            pageCount = GetPageCount(fileName)
            If pageCount <> 0 Then
                loader.ShowLoadPagesDialog = False
                If loader.Load(FormName, _codecs, True, frmpage, ToPage, fileName) Then
                    HadImages = True
                    loader.Image.MakeRegionEmpty()
                    Return New ImageInformation(loader.Image, loader.FileName)
                Else
                    HadImages = False
                End If
            Else
                HadImages = False
            End If
        Catch ex As Exception
            'Messager.ShowFileOpenError(FormName, loader.FileName, ex)
        End Try
        Return Nothing
    End Function
    Private Function GetPageCount(ByVal fileName As String) As Integer
        Try
            RasterCodecs.Startup()
            _codecs = New RasterCodecs()
            info1 = _codecs.GetInformation(fileName, True)
            Return info1.TotalPages
        Catch ex As Exception
        End Try
        Return 0
    End Function
    Public Function SplitWithSinglePage(ByVal sfilename As String, ByVal info As CodecsImageInfo, ByVal pageCount As Integer, ByVal _fileFormat As RasterImageFormat, ByVal _bitsPerPixel As Integer) As Boolean
        Dim Result As Boolean
        Dim loader As New ImageFileLoader()
        'Try
        '    'Application.EnableVisualStyles()
        '    'Application.DoEvents()
        '    RasterCodecs.Startup()
        '    _codecs = New RasterCodecs()
        '    Dim Spliter As New RasterImageViewer
        '    If (loader.Load(own, _codecs, True, 1, info.TotalPages, _currentFileName)) Then
        '        If Not info Is Nothing Then
        '            loader.Image.MakeRegionEmpty()
        '            Spliter.Image = loader.Image
        '        End If
        '    End If
        '    Dim len As String = ""
        '    For i As Int16 = 1 To pageCount
        '        len = i
        '        If len.Length >= 2 Then
        '            _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#" & i & ".TIF", _fileFormat, _bitsPerPixel, i, i, 1, CodecsSavePageMode.Overwrite)
        '        Else
        '            _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#0" & i & ".TIF", _fileFormat, _bitsPerPixel, i, i, 1, CodecsSavePageMode.Overwrite)
        '        End If
        '    Next
        '    _codecs = Nothing
        '    If System.IO.File.Exists(sfilename) Then
        '        Kill(sfilename)
        '        DeleteRecords(sfilename)
        '    End If
        '    Result = True
        'Catch ex As Exception
        '    Result = False
        '    MsgBox(ex.Message.ToString)
        'Finally
        'End Try


        Try
            RasterCodecs.Startup()
            Dim extension As String = Path.GetExtension(_currentFileName)
            For i As Integer = 1 To info.TotalPages
                Dim Spliter As New RasterImageViewer
                loader = New ImageFileLoader()
                _codecs = New RasterCodecs()
                If (loader.Load(own, _codecs, True, i, i, _currentFileName)) Then
                    If Not info Is Nothing Then
                        loader.Image.MakeRegionEmpty()
                        Spliter.Image = loader.Image
                    End If
                    Dim len As String = ""
                    len = i
                    If len.Length >= 2 Then
                        Try
                            _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#" & i & extension, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                        Catch ex As Exception
                            MessageBox.Show(ex.Message)
                        End Try
                    Else
                        Try
                            _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#0" & i & extension, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                        Catch ex As Exception
                            MessageBox.Show(ex.Message)
                        End Try
                    End If
                    Spliter.Dispose()
                    _codecs.Dispose()
                End If
            Next
            If System.IO.File.Exists(sfilename) Then
                Kill(sfilename)
                DeleteRecords(sfilename)
            End If
            Result = True
        Catch ex As Exception
            Result = False
            MsgBox(ex.Message.ToString)
        End Try
        Return Result
    End Function

    Public Function SplitWithCurrentPage(ByVal sfilename As String, ByVal info As CodecsImageInfo, ByVal pageCount As Integer, ByVal CurrenPage As Integer, ByVal _fileFormat As RasterImageFormat, ByVal _bitsPerPixel As Integer) As Boolean
        Dim Result As Boolean
        Dim loader As New ImageFileLoader()
        ' Try
        'RasterCodecs.Startup()
        '_codecs = New RasterCodecs()
        ' Dim Spliter As New RasterImageViewer
        'If (loader.Load(own, _codecs, True, 1, info.TotalPages, _currentFileName)) Then
        '    If Not info Is Nothing Then
        '        loader.Image.MakeRegionEmpty()
        '        Spliter.Image = loader.Image
        '    End If
        'End If
        'If CurrenPage <> 1 Then
        '    _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & Format(DateTime.Now, "yyMMddmmss") & ".TIF", _fileFormat, _bitsPerPixel, CurrenPage, pageCount, 1, CodecsSavePageMode.Overwrite)
        '    _codecs.Save(Spliter.Image, sfilename, _fileFormat, _bitsPerPixel, 1, CurrenPage - 1, 1, CodecsSavePageMode.Overwrite)
        '    _codecs = Nothing
        'End If


        'Result = True
        ' Catch ex As Exception
        'Result = False
        'MsgBox(ex.Message.ToString)
        ' Finally
        ' End Try
        'info =As CodecsImageInfo
        Try
            RasterCodecs.Startup()
            If CurrenPage <> 1 Then

                Dim tempPath = IO.Path.Combine(IO.Path.GetDirectoryName(_currentFileName), "temp")
                If Not IO.Directory.Exists(tempPath) Then
                    IO.Directory.CreateDirectory(tempPath)
                End If
                Dim tempfile = IO.Path.Combine(tempPath, IO.Path.GetFileName(_currentFileName))
                IO.File.Copy(_currentFileName, tempfile, True)
                ' Dim MainFile As RasterImage
                ' Dim Newfile As RasterImage
                Dim Newfilename = ""
                For i As Integer = 1 To info.TotalPages
                    loader = New ImageFileLoader()
                    _codecs = New RasterCodecs()
                    If (loader.Load(own, _codecs, True, i, i, tempfile)) Then
                        If i >= CurrenPage Then
                            If Not info Is Nothing Then
                                loader.Image.MakeRegionEmpty()
                                'If Not IsNothing(Newfile) Then
                                '    Newfile.AddPage(loader.Image)
                                'Else
                                '    Newfile = loader.Image
                                'End If

                                'Dim len As String = ""
                                'len = i
                                'If len.Length >= 2 Then
                                If Newfilename = "" Then
                                    Newfilename = Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#C" & i.ToString() & ".TIF"
                                End If

                                'Else
                                '    Newfilename = Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#C0" & i & ".TIF"
                                'End If

                                Try
                                    If i = CurrenPage Then
                                        _codecs.Save(loader.Image, Newfilename, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                                    Else
                                        _codecs.Save(loader.Image, Newfilename, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show(ex.Message)
                                Finally
                                    ' Newfile.Dispose()
                                End Try
                            End If

                        Else
                            If Not info Is Nothing Then
                                loader.Image.MakeRegionEmpty()
                                'If Not IsNothing(MainFile) Then
                                '    MainFile.AddPage(loader.Image)
                                'Else
                                '    MainFile = loader.Image
                                'End If

                                Try
                                    If i = 1 Then
                                        _codecs.Save(loader.Image, sfilename, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                                    Else
                                        _codecs.Save(loader.Image, sfilename, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                                    End If

                                Catch ex As Exception
                                    MessageBox.Show(ex.Message)
                                Finally
                                    ' MainFile.Dispose()
                                End Try
                            End If
                        End If
                        _codecs.Dispose()
                        loader.Image.Dispose()
                    End If
                Next
                If IO.File.Exists(tempfile) Then
                    Kill(tempfile)
                End If
                '  _codecs = New RasterCodecs()


            End If
            Result = True
        Catch ex As Exception
            Result = False
            MsgBox(ex.Message.ToString)
        Finally
            _codecs.Dispose()
            RasterCodecs.Shutdown()
        End Try

        Return Result
    End Function
    Public Sub SplitCurrentPage(ByVal ToPage As Integer)
        Try
            Frmpagenumber = ToPage
            If Not SplitMerge.IsBusy Then
                WorkerProcess = "SplitCurrent"
                fnme = lstTif.SelectedItem().ToString
                _currentFileName = Imaging + "\" + fnme.Replace("System.Windows.Controls.ListViewItem: ", "")
                FileLst = New List(Of String)()
                For i As Int16 = 0 To lstTif.SelectedItems.Count - 1
                    FileLst.Add(lstTif.SelectedItems(i).ToString.Replace("System.Windows.Controls.ListViewItem: ", ""))
                Next
                reminderAnim = TryCast(Me.Resources("BeginProgress"), Storyboard)
                reminderAnim.Begin()
                SplitMerge.RunWorkerAsync()
            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub
    Public Sub SplitBlackPage()
        Try
            If Not SplitMerge.IsBusy Then
                WorkerProcess = "SplitBlack"
                fnme = lstTif.SelectedItem().ToString
                _currentFileName = Imaging + "\" + fnme.Replace("System.Windows.Controls.ListViewItem: ", "")
                FileLst = New List(Of String)()
                For i As Int16 = 0 To lstTif.SelectedItems.Count - 1
                    FileLst.Add(lstTif.SelectedItems(i).ToString.Replace("System.Windows.Controls.ListViewItem: ", ""))
                Next
                reminderAnim = TryCast(Me.Resources("BeginProgress"), Storyboard)
                reminderAnim.Begin()
                SplitMerge.RunWorkerAsync()
            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub
    'mar18 by raja
    Public Function SplitWithWhitePage(ByVal sfilename As String, ByVal info As CodecsImageInfo, ByVal pageCount As Integer, ByVal _fileFormat As RasterImageFormat, ByVal _bitsPerPixel As Integer) As Boolean
        Dim Result As Boolean
        Dim loader As New ImageFileLoader()
        Try
            'Application.DoEvents()
            RasterCodecs.Startup()
            Dim lowerColor As RasterColor = New RasterColor(200, 200, 200)
            Dim upperColor As RasterColor = New RasterColor(255, 255, 255)
            _codecs = New RasterCodecs()
            Dim Spliter As New RasterImageViewer
            If (loader.Load(own, _codecs, True, 1, info.TotalPages, _currentFileName)) Then
                If Not info Is Nothing Then
                    loader.Image.MakeRegionEmpty()
                    Spliter.Image = loader.Image
                End If
            End If
            Dim split As Boolean = False
            Dim prevpage As Integer
            Dim isplit As Integer = 0
            For i As Integer = 1 To pageCount
                Spliter.Image.Page = i
                Spliter.Image.AddColorRgbRangeToRegion(lowerColor, upperColor, RasterRegionCombineMode.SetNot)
                With Spliter.Image
                    Dim val As Double = (.CalculateRegionArea * 100) / (.ImageWidth * .ImageHeight)
                    If val = 0.0 Then
                        If prevpage <> i - 1 Then
                            isplit = isplit + 1
                            Dim cnt As String
                            cnt = isplit.ToString
                            If cnt.Length >= 2 Then
                                _codecs.Save(Spliter.Image, Replace(sfilename, ".TIF", "") & "#" & isplit & ".TIF", _fileFormat, _bitsPerPixel, prevpage + 1, i - 1, 1, CodecsSavePageMode.Overwrite)
                            Else
                                _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#0" & isplit & ".TIF", _fileFormat, _bitsPerPixel, prevpage + 1, i - 1, 1, CodecsSavePageMode.Overwrite)
                            End If
                            split = True
                            prevpage = i
                        Else
                            prevpage = i
                        End If
                    End If
                End With
                If prevpage > 0 And i = pageCount Then
                    _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#" & isplit + 1 & ".TIF", _fileFormat, _bitsPerPixel, prevpage + 1, pageCount, 1, CodecsSavePageMode.Overwrite)
                End If
                'Application.DoEvents()
            Next
            If split = False Then
                _codecs.Save(Spliter.Image, sfilename, _fileFormat, _bitsPerPixel, 1, pageCount, 1, CodecsSavePageMode.Overwrite)
                _codecs = Nothing
            Else
                Kill(sfilename)
                DeleteRecords(sfilename)
            End If
            Result = True
        Catch ex As Exception
            Result = False
            MsgBox(ex.Message.ToString)
        Finally
        End Try
        Return Result
    End Function
    Public Sub SplitWithBarcode()
        Try
            If Not SplitMerge.IsBusy Then
                WorkerProcess = "SplitBarcode"
                'fnme = lstTif.SelectedItem().ToString
                '_currentFileName = Imaging + "\" + fnme.Replace("System.Windows.Controls.ListViewItem: ", "")


                FileLst = New List(Of String)()
                    For i As Int16 = 0 To lstTif.SelectedItems.Count - 1
                        FileLst.Add(lstTif.SelectedItems(i).ToString.Replace("System.Windows.Controls.ListViewItem: ", ""))
                    Next
                    'If lstTif.SelectedItem.ToString() <> "" Then
                    '    _currentFileName = Imaging + "\" + lstTif.SelectedItem.ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
                    'End If
                    reminderAnim = TryCast(Me.Resources("BeginProgress"), Storyboard)
                    reminderAnim.Begin()
                    SplitMerge.RunWorkerAsync()
                End If
        Catch ex As Exception
        End Try
    End Sub




    Public Sub SplitBlankPage()
        Try
            If Not SplitMerge.IsBusy Then
                WorkerProcess = "SplitBlank"
                fnme = lstTif.SelectedItem().ToString
                _currentFileName = Imaging + "\" + fnme.Replace("System.Windows.Controls.ListViewItem: ", "")
                FileLst = New List(Of String)()
                For i As Int16 = 0 To lstTif.SelectedItems.Count - 1
                    FileLst.Add(lstTif.SelectedItems(i).ToString.Replace("System.Windows.Controls.ListViewItem: ", ""))
                Next
                reminderAnim = TryCast(Me.Resources("BeginProgress"), Storyboard)
                reminderAnim.Begin()
                SplitMerge.RunWorkerAsync()
            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub

    Public Sub MergeSelectedFiles()
        Try
            If Not SplitMerge.IsBusy Then
                WorkerProcess = "Merge"
                FileLst = New List(Of String)()
                For i As Int16 = 0 To lstTif.SelectedItems.Count - 1
                    FileLst.Add(lstTif.SelectedItems(i).ToString.Replace("System.Windows.Controls.ListViewItem: ", ""))
                Next
                reminderAnim = TryCast(Me.Resources("BeginProgress"), Storyboard)
                reminderAnim.Begin()
                SplitMerge.RunWorkerAsync()
            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub

    Public Sub SplitSinglePage()
        Try
            If Not SplitMerge.IsBusy Then
                WorkerProcess = "SplitSingle"
                fnme = lstTif.SelectedItem().ToString
                _currentFileName = Imaging + "\" + fnme.Replace("System.Windows.Controls.ListViewItem: ", "")
                FileLst = New List(Of String)()
                For i As Int16 = 0 To lstTif.SelectedItems.Count - 1
                    FileLst.Add(lstTif.SelectedItems(i).ToString.Replace("System.Windows.Controls.ListViewItem: ", ""))
                Next
                reminderAnim = TryCast(Me.Resources("BeginProgress"), Storyboard)
                reminderAnim.Begin()
                SplitMerge.RunWorkerAsync()
            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub
    Private Sub MouseRightButtonDown_Click(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
        Dim Cont As New ContextMenu
        Cont.IsOpen = False
    End Sub
    Public Sub SetCulture(ByVal Lang As String)
        Dim culture = CultureInfo.CreateSpecificCulture(Lang)
        Dim rm As New ResourceManager("ezofis.UserControl.Main", GetType(ECMLeftPane).Assembly)
        Me.selectAll.Content = rm.GetString("Select All / Unselect All", culture)
        Me.lb1.Content = rm.GetString("Tif Formats", culture)
        Me.lb2.Content = rm.GetString("Other Formats", culture)
        Me.otherselect.Content = rm.GetString("Select All / Unselect All", culture)
    End Sub
    Public Function SplitWithBlackPage(ByVal sfilename As String, ByVal info As CodecsImageInfo, ByVal pageCount As Integer, ByVal _fileFormat As RasterImageFormat, ByVal _bitsPerPixel As Integer) As Boolean
        Dim Result As Boolean
        Dim loader As New ImageFileLoader()
        Try
            'Application.DoEvents()
            RasterCodecs.Startup()
            Dim lowerColor As RasterColor = New RasterColor(200, 200, 200)
            Dim upperColor As RasterColor = New RasterColor(255, 255, 255)
            _codecs = New RasterCodecs()
            Dim Spliter As New RasterImageViewer
            If (loader.Load(own, _codecs, True, 1, info.TotalPages, _currentFileName)) Then
                If Not info Is Nothing Then
                    loader.Image.MakeRegionEmpty()
                    Spliter.Image = loader.Image
                End If
            End If
            Dim split As Boolean = False
            Dim prevpage As Integer
            Dim isplit As Integer = 0
            For i As Integer = 1 To pageCount
                Spliter.Image.Page = i
                Spliter.Image.AddColorRgbRangeToRegion(lowerColor, upperColor, RasterRegionCombineMode.SetNot)
                With Spliter.Image
                    Dim val As Double = (.CalculateRegionArea * 100) / (.ImageWidth * .ImageHeight)
                    If val > 86 Then
                        If prevpage <> i - 1 Then
                            isplit = isplit + 1
                            Dim cnt As String
                            cnt = isplit.ToString
                            If cnt.Length >= 2 Then
                                _codecs.Save(Spliter.Image, Replace(sfilename, ".TIF", "") & "#" & isplit & ".TIF", _fileFormat, _bitsPerPixel, prevpage + 1, i - 1, 1, CodecsSavePageMode.Overwrite)
                            Else
                                _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#0" & isplit & ".TIF", _fileFormat, _bitsPerPixel, prevpage + 1, i - 1, 1, CodecsSavePageMode.Overwrite)
                            End If
                            split = True
                            prevpage = i
                        Else
                            prevpage = i
                        End If
                    End If
                End With
                If prevpage > 0 And i = pageCount Then
                    _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#" & isplit + 1 & ".TIF", _fileFormat, _bitsPerPixel, prevpage + 1, pageCount, 1, CodecsSavePageMode.Overwrite)
                End If
                'Application.DoEvents()
            Next
            If split = False Then
                _codecs.Save(Spliter.Image, sfilename, _fileFormat, _bitsPerPixel, 1, pageCount, 1, CodecsSavePageMode.Overwrite)
                _codecs = Nothing
            Else
                Kill(sfilename)
                DeleteRecords(sfilename)
            End If
            Result = True
        Catch ex As Exception
            Result = False
            MsgBox(ex.Message.ToString)
        Finally
        End Try
        Return Result
    End Function
    'sep-19-2016 shankar

    Public Function SplitWithBlankPage(ByVal sfilename As String, ByVal info As CodecsImageInfo, ByVal pageCount As Integer, ByVal _fileFormat As RasterImageFormat, ByVal _bitsPerPixel As Integer) As Boolean
        Dim Result As Boolean
        Dim loader As New ImageFileLoader()
        Try
            RasterCodecs.Startup()
            Dim lowerColor As RasterColor = New RasterColor(200, 200, 200)
            Dim upperColor As RasterColor = New RasterColor(255, 255, 255)
            _codecs = New RasterCodecs()
            Dim Spliter As New RasterImageViewer
            If (loader.Load(own, _codecs, True, 1, info.TotalPages, _currentFileName)) Then
                If Not info Is Nothing Then
                    loader.Image.MakeRegionEmpty()
                    Spliter.Image = loader.Image
                End If
            End If
            Dim split As Boolean = False
            Dim prevpage As Integer
            Dim isplit As Integer = 0
            For i As Integer = 1 To pageCount
                Spliter.Image.Page = i
                Spliter.Image.AddColorRgbRangeToRegion(lowerColor, upperColor, RasterRegionCombineMode.SetNot)
                With Spliter.Image
                    Dim val As Double = (.CalculateRegionArea * 100) / (.ImageWidth * .ImageHeight)
                    If val = 0.0 Or val > 86.0 Then
                        If prevpage <> i - 1 Then
                            isplit = isplit + 1
                            Dim cnt As String
                            cnt = isplit.ToString
                            If cnt.Length >= 2 Then
                                _codecs.Save(Spliter.Image, Replace(sfilename, ".TIF", "") & "#" & isplit & ".TIF", _fileFormat, _bitsPerPixel, prevpage + 1, i - 1, 1, CodecsSavePageMode.Overwrite)
                            Else
                                _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#0" & isplit & ".TIF", _fileFormat, _bitsPerPixel, prevpage + 1, i - 1, 1, CodecsSavePageMode.Overwrite)
                            End If
                            split = True
                            prevpage = i
                        Else
                            prevpage = i
                        End If
                    End If
                End With
                If prevpage > 0 And i = pageCount Then
                    _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#" & isplit + 1 & ".TIF", _fileFormat, _bitsPerPixel, prevpage + 1, pageCount, 1, CodecsSavePageMode.Overwrite)
                End If
            Next
            If split = False Then
                _codecs.Save(Spliter.Image, sfilename, _fileFormat, _bitsPerPixel, 1, pageCount, 1, CodecsSavePageMode.Overwrite)
                _codecs = Nothing
            Else
                Kill(sfilename)
                DeleteRecords(sfilename)
            End If
            Result = True
        Catch ex As Exception
            Result = False
            MsgBox(ex.Message.ToString)
        Finally
        End Try
        Return Result
    End Function
    Public Function DeleteRecords(ByVal _currentFileName As String) As Boolean
        Try
            Dim CAC As New CACserviceClient
            If ECMRightPane.templateid <> 0 Then
                Dim tblname = "eZCA_" + ECMRightPane.cabinetid.ToString() + "_" + ECMRightPane.templateid.ToString() + "_stage"
                Dim sdataset As New DataSet
                sdataset = CAC.SelectedeZUserDefinedList(1, ECMRightPane.templateid, "*", "ifilename", _currentFileName)
                If sdataset.Tables(0).Rows.Count > 0 Then
                    If CAC.InsertAndUpdateAndDeleteeZUserDefined("delete from " + tblname + " where itemid= " & sdataset.Tables(0).Rows(0).Item(0).ToString) = 1 Then
                    Else
                        MsgBox("Due to some Error while Delete Records")
                    End If
                End If
            End If
            Return True
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Message.ToString)
            Return False
        End Try
    End Function
    Dim selectedindex As Integer = 0
    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        Try
            If (lstTif.SelectedIndex <> -1) Then
                Dim filname As String = lstTif.SelectedItem.ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
                Dim list = DirectCast(lstTif.SelectedItem, ListViewItem)
                If CheckIndexing(filname, ECMRightPane.templateid.ToString()) Then
                    list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                Else
                    list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub CheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        RaiseEvent Tif_unchecked(sender, e)
        'If (lstTif.SelectedIndex <> -1) Then
        '    Dim filname As String = lstTif.SelectedItem.ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
        '    ECMRightPane.SaveRecords(_currentFileName)
        '    Dim list = DirectCast(lstTif.SelectedItem, ListViewItem)
        '    If CheckIndexing(filname, ECMRightPane.templateid.ToString()) Then
        '        list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
        '    Else
        '        list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
        '    End If
        'End If
    End Sub
End Class
