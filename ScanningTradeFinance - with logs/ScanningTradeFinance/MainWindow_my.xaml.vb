
Imports System.Collections.ObjectModel
Imports System.Windows.Media.Animation
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports SautinSoft
Imports ScanningTradeFinance.publicvariables
Imports System.Configuration
Imports System.Collections.Specialized
Imports ScanningTradeFinance.CACServiceReference
Imports Microsoft.Win32
Imports EzofisScanInit
Imports Newtonsoft.Json
Imports Leadtools
Imports System.Threading
Imports Tulpep
Imports Tulpep.NotificationWindow
Imports Ezofis_Tif2Pdf_Converter
Imports System.ComponentModel
Imports Leadtools.Codecs

Public Class MainWindow

    Protected Shared sharedCAC As New CACserviceClient
    Public Shared externalAPIobj As New ApiFunctions
    Public Shared invitaAPIobj As New ApiFunctions
    Public ResAccBasicInfoObj As New DataSet

    Public Property WorkitemReference As String = "WIR"
    Dim w As New wirno
    Dim reminderAnim As Storyboard

    Dim feAccNo As String = ""
    Dim feWorkItemRefNo As String = ""
    Dim feRimNo As String = ""
    Dim feAccType As String = ""
    Dim feAccStatus As String = ""
    Dim feProduct As String = ""
    Dim fePhase As String = ""
    Dim feDocType As String = ""

    Dim feEximbillsModule As String = ""

    Public _bitsPerPixel As Integer
    Public _fileFormat As RasterImageFormat
    Dim worker As New System.ComponentModel.BackgroundWorker()

    Dim progfor As String
    Dim _currentFileName As String

    Dim acct As AliasAccount
    Dim impersonate As Boolean = False

    Public scannedfilepath As String = ""
    Public lastWorkItemNoFormat As String = ""
    Public dstbl As New DataSet
    Public cabinetname As String = ""
    Public cabinetid As String = ""
    Public templatename As String = ""
    Public ersdirpath As String = ""
    Public ersid As Integer = 0
    Public lastWorkItemNo As Integer = 0

    Public submittedWIGridObj As SubmittedWorkItemsGrid

    Public tablefieldnames As String = ""
    Public tablefieldvalues As String = ""

    Public submittedDocNos As String = ""
    Public gridSelectedCellDocumentTypeValue As String = ""
    Dim gridSelectedRowNumber As Integer = -1

    Public sessionid As String = ""

    Dim GridDispDatatable As New DataTable

    Dim DocTypeList As New List(Of DocumentTypeList)
    Dim DuplicateDocTypeList As New List(Of DocumentTypeList)
    Public MyClass1 As ObservableCollection(Of DocumentTypeList)

    Public Class DocumentTypeList
        Public Property FILEPATH As String
        Public Property SCAN As String
        Public Property RESCAN As String
        Public Property DOCUMENT_TYPE As String
        Public Property MANDATORY As String
        Public Property itemid As String
        Public Property STAGEITEMID As String

        Public Sub New(ByVal pDOCUMENT_TYPE As String, ByVal pMANDATORY As String, ByVal pitemid As String, ByVal pSCAN As String, ByVal pRESCAN As String, ByVal pFILEPATH As String, ByVal pStageItemId As String)
            DOCUMENT_TYPE = pDOCUMENT_TYPE
            MANDATORY = pMANDATORY
            itemid = pitemid
            SCAN = pSCAN
            RESCAN = pRESCAN
            FILEPATH = pFILEPATH
            STAGEITEMID = pStageItemId
        End Sub
    End Class
    Class wirno
        Public Property Workitemref As String

    End Class

    Public Sub New()
        Try

            '--------------------With Login Form----------------------------------------------
            Dim Login As New LoginForm()

            If Login.ShowDialog() Then
                Dim starttime = Now
                Support.SetLicense()
                InitializeComponent()
                BackGroundWorkerAssign()
                Dim endtime = Now
                ' MsgBox("starttime : " & starttime & vbCrLf & "endtime : " & endtime)
                ''Me.DataContext = Me
                '    ' txtusername.Focus()
            Else
                ''Application.Current.Shutdown()
                End
            End If

            ''----------------------------w/o login form--------------------------------------

            'Support.SetLicense()
            'InitializeComponent()

            'BackGroundWorkerAssign()

            ''---------------------------------------
            If (getCabinetDetails()) Then

            Else
                MsgBox("Failed:Please Check Settings/Api/Template")
                End
            End If
        Catch ex As Exception

        End Try




    End Sub
    ' Public Sub New()
    '  Support.SetLicense()
    '  InitializeComponent()
    'End Sub
    Public Sub BackGroundWorkerAssign()

        Dim workerdo As New ComponentModel.DoWorkEventHandler(AddressOf worker_DoWork)
        ' RemoveHandler worker.DoWork, workerdo
        AddHandler worker.DoWork, workerdo
        Dim workerrun As New ComponentModel.RunWorkerCompletedEventHandler(AddressOf worker_RunWorkerCompleted)
        'RemoveHandler worker.RunWorkerCompleted, workerrun
        AddHandler worker.RunWorkerCompleted, workerrun
        Dim workerprogress As New ComponentModel.ProgressChangedEventHandler(AddressOf worker_ProgressChanged)
        'RemoveHandler worker.ProgressChanged, workerprogress
        AddHandler worker.ProgressChanged, workerprogress

    End Sub

    Private Sub worker_DoWork(ByVal Sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)

        Try

            thisTimer1 = New System.Timers.Timer()
            thisTimer1.Enabled = True
            thisTimer1.Interval = 500
            thisTimer1.AutoReset = True
            AddHandler thisTimer1.Elapsed, AddressOf thisTimer1_Tick
            thisTimer1.Start()


            If progfor = "Export" Then

                ' btnINITIATESCANNING.IsEnabled = False
                'Thread.Sleep(1000)
                ExportToArchive(scannedfilepath, lastWorkItemNoFormat, dstbl, cabinetname, cabinetid, templatename, ersdirpath, ersid, lastWorkItemNo)
                'ExportToArchive()
            ElseIf progfor = "Verify" Then
                'Thread.Sleep(1000)
                clearAllvalues(2)
                verifyAccountNo()
            ElseIf progfor = "Submit" Then

                SaveFileInLocal()


            End If
            '  Thread.Sleep(3000)
        Catch ex As Exception

        Finally
            thisTimer1.Stop()
            ' worker.CancelAsync()
        End Try
    End Sub
    Public Sub SaveFileInLocal()
        Try

            Me.Dispatcher.Invoke(Sub()

                                     lblStatusbar.Text = "Processing...."

                                     If (txtAccountNo.Text <> "") Then
                                         feAccNo = txtAccountNo.Text
                                     Else
                                         feAccNo = ""
                                     End If
                                     If (cbProductList.SelectedValue <> "") Then
                                         feProduct = cbProductList.SelectedValue.ToString
                                     Else
                                         feProduct = ""
                                     End If
                                     If (cbPhase.SelectedValue <> "") Then
                                         fePhase = cbPhase.SelectedValue.ToString
                                     Else
                                         fePhase = ""
                                     End If

                                     Dim Details = feAccNo & "/" & feProduct & "/" & fePhase
                                     If (Details = "//") Then
                                         Details = ""
                                     End If
                                     Dim codecsforpagescount As New RasterCodecs()
                                     Dim info = codecsforpagescount.GetInformation(_currentFileName, True)
                                     Dim noofpages = info.TotalPages

                                     Dim ScannedDocDetObj As New ScannedDocumentDetails(feDocType, noofpages.ToString, Details)
                                     ScannedDocDetObj.ShowDialog()


                                     If (ScannedDocMsg = 1) Then
                                         GetCabinetFields()
                                         scannedfilepath = copyfiletoimagingfolder(ersdirpath, scannedfilepath)
                                         SaveRecordsIntoStage(scannedfilepath) ', lastWorkItemNoFormat, dstbl, cabinetname, cabinetid, templatename, ersdirpath, ersid, lastWorkItemNo)
                                         '  SaveIntoGrid()
                                     ElseIf (ScannedDocMsg = 2) Then
                                         InititateScanning()
                                     End If
                                 End Sub)
        Catch ex As Exception
            MsgBox("Exception in SaveFileInLocal " & ex.Message.ToString)
        End Try

    End Sub
    Public Sub SaveIntoGrid()
        Dim dt As New DataTable
        'For i = 0 To GridDispDatatable.Rows.Count - 1

        'Next
        dt = TryCast(GridDispDatatable, DataTable)
        'GridDispDatatable.
    End Sub
    Public Sub SaveRecordsIntoStage(ByVal _currentFileName As String)
        Try

            Dim CAC As New CACserviceClient

            If invitaAPIobj.TemplateId <> 0 Then
                Dim tblname = "eZCA_" + cabinetid.ToString() + "_" + invitaAPIobj.TemplateId.ToString() + "_stage"
                Dim sdataset As DataSet
                Dim sqlstring As New System.Text.StringBuilder
                Dim fl As Integer = 0
                If _currentFileName <> "" Then

                    'sdataset = CAC.SelectedeZUserDefinedList(1, invitaAPIobj.TemplateId, "*", "ifilepath+'\'+ifilename", _currentFileName)
                    Dim ezfrom = "CAPTURE(" & Environment.MachineName & ")"

                    Dim qrystagetbl = "insert into " & tblname & "(ersid,templateid," & tablefieldnames & "ifilepath,ifilename,ifiletype,[eZFrom],[version],[dtitle],[dauthor],[dsubject],[dkeywords],[checkout],[checkoutpath],[checkoutby],[dstatus],[dsize],[nopages], createdon, createdby, updatedon, updatedby, isdeleted) values (" & ersid & "," & invitaAPIobj.TemplateId & "," & tablefieldvalues & "'" & Path.GetDirectoryName(_currentFileName) & "\','" & _currentFileName & "','" & Path.GetExtension(_currentFileName).Replace(".", "") & "','" & ezfrom & "','','','','','','','',0,'Active','',0,'" & Format(Now(), "dd/MMM/yyyy hh:mm:ss tt") & "',1,'',0,0" & ")"

                    MsgBox("SaveRecordsIntoStage query:" + qrystagetbl.ToString())
                    Dim res = invitaAPIobj.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qrystagetbl.ToString)
                    If res <> 0 Then
                        Me.Dispatcher.Invoke(Sub()
                                                 'Dim nomsg1 As New MessageWin("Document Saved Locally....")
                                                 'nomsg1.btnyes.Visibility = Visibility.Hidden
                                                 'nomsg1.btnno.Content = "OK"
                                                 'nomsg1.ShowDialog()
                                                 'Dim Details = feAccNo & "/" & feProduct & "/" & fePhase
                                                 'Dim codecsforpagescount As New RasterCodecs()
                                                 'Dim info = codecsforpagescount.GetInformation(_currentFileName, True)
                                                 'Dim noofpages = info.TotalPages

                                                 'Dim ScannedDocDetObj As New ScannedDocumentDetails(feDocType, noofpages.ToString, Details)
                                                 'Dim resultdlg = ScannedDocDetObj.ShowDialog()

                                                 ECMViewer._documentViewer.SetDocument(Nothing)
                                                 ECMViewerToolbar.IsEnabled = False
                                                 LblFileName.Content = ""
                                                 SetStageItemIdInGrid(res.ToString)
                                                 btnSubmit.IsEnabled = False
                                                 btnRescan.IsEnabled = False
                                                 btnClearRightPane.IsEnabled = False
                                             End Sub)

                        'MsgBox("Locally Document Saved")
                        'btnFinalSubmit.IsEnabled = True
                    Else
                        MsgBox("Due to some Error while Save Records : " + sqlstring.ToString())
                    End If
                End If
            End If

        Catch ex As Exception
            MsgBox("Exception in SaveRecordsIntoStage " & ex.Message.ToString)
        End Try
    End Sub
    Private Sub worker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
        btnSpin.Visibility = Visibility.Visible
        lblStatusbar.Text = "Processing...."
    End Sub

    Public thisTimer1 As System.Timers.Timer
    Private Sub thisTimer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Application.Current.Dispatcher.Invoke(Sub()
                                                      If lblStatusbar.Text = "Processing...." Or lblStatusbar.Text = "Exporting...." Or lblStatusbar.Text = "Scanning...." Then
                                                          btnSpin.Visibility = Visibility.Visible

                                                          ' btnAccNoVerifySpin.Visibility = Visibility.Visible

                                                      Else
                                                          btnSpin.Visibility = Visibility.Collapsed
                                                          worker.CancelAsync()
                                                      End If
                                                  End Sub)
        Catch ex As Exception
        Finally
        End Try
    End Sub
    Private Sub worker_RunWorkerCompleted(ByVal Sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        If e.Cancelled = True Then
            Me.lblStatusbar.Text = "Canceled!"
        ElseIf e.Error IsNot Nothing Then
            Me.lblStatusbar.Text = "Error: " & e.Error.Message
        Else
            'Me.lblStatusbar.Text = "Export Completed"

            btnSpin.Visibility = Visibility.Collapsed

            ' btnINITIATESCANNING.IsEnabled = True
            'btnSubmit.IsEnabled = False
            'btnRescan.IsEnabled = False
            'worker.CancelAsync()
            reminderAnim.Stop()
        End If
    End Sub

    Public Sub SetStageItemIdInGrid(stageitemid As String)
        If (gridSelectedRowNumber <> -1) Then
            DocTypeList(gridSelectedRowNumber).STAGEITEMID = stageitemid
            DocTypeList(gridSelectedRowNumber).SCAN = "View"
            DocTypeList(gridSelectedRowNumber).RESCAN = "Rescan"
            gridSubmittedDocuments.Items.Refresh()
        End If
    End Sub

    Public Sub ClearFilepathIdInGrid()
        If (gridSelectedRowNumber <> -1) Then
            DocTypeList(gridSelectedRowNumber).FILEPATH = ""
            DocTypeList(gridSelectedRowNumber).SCAN = "Scan"
            DocTypeList(gridSelectedRowNumber).RESCAN = "-"
            gridSubmittedDocuments.Items.Refresh()

            ECMViewer._documentViewer.SetDocument(Nothing)
            ECMViewerToolbar.IsEnabled = False
            LblFileName.Content = ""
            btnSubmit.IsEnabled = False
            btnRescan.IsEnabled = False
            btnClearRightPane.IsEnabled = False
        End If
    End Sub
    Private Sub btnINITIATESCANNING_Click(sender As Object, e As RoutedEventArgs) Handles btnINITIATESCANNING.Click
        Try

            Dim Appcon1 As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
            If (Appcon1("impersonate") = "true") Then
                acct = New AliasAccount(Appcon1("Username"), Appcon1("Password"), Appcon1("Domain"))
                '  acct = New AliasAccount("invita\BbkTFWP", "U@tTFW$21", "invita")
                Try
                    acct.BeginImpersonation()
                    impersonate = True
                Catch ex As Exception
                    MsgBox("Exception in btnINITIATESCANNING_Click " + ex.Message)
                End Try
                If impersonate Then
                    Me.Dispatcher.Invoke(Sub()
                                             lblStatusbar.Text = "Processing...."
                                         End Sub)
                    archiveFile()
                    Me.Dispatcher.Invoke(Sub()
                                             lblStatusbar.Text = ""
                                         End Sub)
                Else
                    MsgBox("Failed to connect to File Server ")
                End If
            Else
                Me.Dispatcher.Invoke(Sub()
                                         lblStatusbar.Text = "Processing...."
                                     End Sub)
                archiveFile()
                Me.Dispatcher.Invoke(Sub()
                                         lblStatusbar.Text = ""
                                     End Sub)
            End If

        Catch ex As Exception
            MsgBox("Exception in btnINITIATESCANNING_Click " + ex.Message)
        Finally
            'reminderAnim.Stop()
            'thisTimer1.Stop()
            'border1.Visibility = Visibility.Hidden
        End Try
    End Sub
    Public Sub InititateScanning()
        Try

            Dim Appcon1 As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
            If (Appcon1("impersonate") = "true") Then
                acct = New AliasAccount(Appcon1("Username"), Appcon1("Password"), Appcon1("Domain"))
                '  acct = New AliasAccount("invita\BbkTFWP", "U@tTFW$21", "invita")
                Try
                    acct.BeginImpersonation()
                    impersonate = True
                Catch ex As Exception
                    MsgBox("Exception in btnINITIATESCANNING_Click " + ex.Message)
                End Try
                If impersonate Then
                    archiveFile()
                Else
                    MsgBox("Failed to connect to File Server ")
                End If
            Else
                archiveFile()
            End If

        Catch ex As Exception
            MsgBox("Exception in btnINITIATESCANNING_Click " + ex.Message)
        Finally
            'reminderAnim.Stop()
            'thisTimer1.Stop()
            'border1.Visibility = Visibility.Hidden
        End Try
    End Sub
    Public Function CheckWorkItemAlreadyexists(accno As String, product As String, phase As String, cabinetid As Integer) As Boolean
        Try
            Dim qrywigen = "select [Work Item Reference] from ezca_" & cabinetid & "_" & invitaAPIobj.TemplateId & "_items where [Account Number]='" & accno & "' and [Product]='" & product & "' and [Phase]='" & phase & "'"
            Dim lastWorkItemNoFormat As String = ""
            Dim dstbl1 = invitaAPIobj.GetDatasetByQuery(qrywigen)
            If Not IsNothing(dstbl1) AndAlso dstbl1.Tables.Count > 0 AndAlso dstbl1.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox("Exception in CheckWorkItemAlreadyexists" + ex.Message)
        End Try
    End Function
    ' Public Function archiveFile()

    Private Function getCabinetDetails() As Boolean
        Try
            Dim qrytbl = "select erstbl.ERSServerName,erstbl.ERSDirPath,r2.* from (select r1.*,fldtbl.FieldId,fldtbl.FieldName,fldtbl.Mandatory,fldtbl.FieldLevel,fldtbl.DataTypeId  from (select cabtbl.CabinetID,cabtbl.CabinetName,cabtbl.ERSId,temptbl.TemplateId,temptbl.TemplateName from eZTemplate as temptbl left join eZCabinet as cabtbl on temptbl.CabinetID=cabtbl.CabinetID where temptbl.TemplateId=" & invitaAPIobj.TemplateId & " and temptbl.Isdeleted=0) as r1 left join eZTemplateField as fldtbl on r1.TemplateId=fldtbl.TemplateId) as r2 left join ezersinfo as erstbl on erstbl.ERSId=r2.ERSId order by r2.FieldLevel;select * from ezWorkItemIdGen;"
            dstbl = invitaAPIobj.GetDatasetByQuery(qrytbl)

            If Not IsNothing(dstbl) AndAlso dstbl.Tables.Count > 0 AndAlso dstbl.Tables(0).Rows.Count > 0 Then
                ' archivefolderpath = ""
                cabinetname = dstbl.Tables(0).Rows(0).Item("CabinetName")
                cabinetid = dstbl.Tables(0).Rows(0).Item("CabinetID")
                templatename = dstbl.Tables(0).Rows(0).Item("TemplateName")
                ersdirpath = dstbl.Tables(0).Rows(0).Item("ERSDirPath")
                ersid = dstbl.Tables(0).Rows(0).Item("ERSId")
                Return True
            Else
                Return False
            End If
        Catch ex As Exception

        End Try
    End Function

    Public Function archiveFile()
        Try
            Me.Dispatcher.Invoke(Sub()
                                     lblStatusbar.Text = "Processing...."
                                 End Sub)

            Dim Appcon1 As NameValueCollection = CType(ConfigurationSettings.GetConfig("appSettings"), NameValueCollection)
            'Dim SelectedscannerName = 

            btnINITIATESCANNING.IsEnabled = False
            'Dim qrytbl As String
            Dim tempdstbl As New DataSet

            Dim sysdate As Date

            Dim qryWItbl As String

            Dim archivepath As String = ""


            'qrytbl = "select erstbl.ERSServerName,erstbl.ERSDirPath,r2.* from (select r1.*,fldtbl.FieldId,fldtbl.FieldName,fldtbl.Mandatory,fldtbl.FieldLevel,fldtbl.DataTypeId  from (select cabtbl.CabinetID,cabtbl.CabinetName,cabtbl.ERSId,temptbl.TemplateId,temptbl.TemplateName from eZTemplate as temptbl left join eZCabinet as cabtbl on temptbl.CabinetID=cabtbl.CabinetID where temptbl.TemplateId=" & invitaAPIobj.TemplateId & " and temptbl.Isdeleted=0) as r1 left join eZTemplateField as fldtbl on r1.TemplateId=fldtbl.TemplateId) as r2 left join ezersinfo as erstbl on erstbl.ERSId=r2.ERSId order by r2.FieldLevel;select * from ezWorkItemIdGen;"
            'dstbl = invitaAPIobj.GetDatasetByQuery(qrytbl)

            'If Not IsNothing(dstbl) AndAlso dstbl.Tables.Count > 0 AndAlso dstbl.Tables(0).Rows.Count > 0 Then
            '    ' archivefolderpath = ""
            '    cabinetname = dstbl.Tables(0).Rows(0).Item("CabinetName")
            '    cabinetid = dstbl.Tables(0).Rows(0).Item("CabinetID")
            '    templatename = dstbl.Tables(0).Rows(0).Item("TemplateName")
            '    ersdirpath = dstbl.Tables(0).Rows(0).Item("ERSDirPath")
            '    ersid = dstbl.Tables(0).Rows(0).Item("ERSId")



            Dim accno As String = txtAccountNo.Text
            Dim product As String = ""
            If (cbProductList.SelectedIndex <> -1) Then
                product = cbProductList.SelectedValue.ToString()
            End If
            Dim phase As String = ""
            If (cbProductList.SelectedIndex <> -1) Then
                phase = cbPhase.SelectedValue.ToString()
            End If

            'Dim scannedfilepath = "" 'Path.Combine(scannedpath, ScannedFilename)

            'If WorkitemReference = "" Then
            '    If (accno <> "") Then
            '        If (product <> "") Then
            '            If (phase <> "") Then
            '                Dim resWIR = CheckWorkItemAlreadyexists(accno, product, phase, cabinetid)
            '                If resWIR Then
            '                    '                            NewWorkItem = 0
            '                    '                            Dim nomsg1 As New MessageWin("Work Item Already Exist.
            '                    'For the same Product and Phase. Do you want create New Work Item?")
            '                    '                            nomsg1.ShowDialog()
            '                    If NewWorkItem = 1 Then
            '                        If Not IsNothing(dstbl) AndAlso dstbl.Tables.Count > 0 AndAlso dstbl.Tables(1).Rows.Count > 0 Then
            '                            lastWorkItemNo = dstbl.Tables(1).Rows(0).Item("LastWorkItemNo") + 1
            '                            Dim noofchars = dstbl.Tables(1).Rows(0).Item("Noofchars")
            '                            Dim prefix = dstbl.Tables(1).Rows(0).Item("Prefix")
            '                            updatedon = dstbl.Tables(1).Rows(0).Item("UpdatedOn")
            '                            lastWorkItemNoFormat = generateWorkItemNo(lastWorkItemNo, noofchars, prefix, updatedon)
            '                        End If
            '                    Else
            '                        Exit Function
            '                    End If
            '                Else
            '                    If Not IsNothing(dstbl) AndAlso dstbl.Tables.Count > 0 AndAlso dstbl.Tables(1).Rows.Count > 0 Then
            '                        lastWorkItemNo = dstbl.Tables(1).Rows(0).Item("LastWorkItemNo") + 1
            '                        Dim noofchars = dstbl.Tables(1).Rows(0).Item("Noofchars")
            '                        Dim prefix = dstbl.Tables(1).Rows(0).Item("Prefix")
            '                        updatedon = dstbl.Tables(1).Rows(0).Item("UpdatedOn")
            '                        lastWorkItemNoFormat = generateWorkItemNo(lastWorkItemNo, noofchars, prefix, updatedon)
            '                    End If
            '                End If
            '                WorkitemReference = lastWorkItemNoFormat
            '                txtwir.Text = WorkitemReference
            '            End If
            '            End If
            '        ElseIf txtWorkItemNo.Text <> "" Then
            '            WorkitemReference = txtWorkItemNo.Text
            '        txtwir.Text = WorkitemReference
            '    End If
            'End If
            If (invitaAPIobj.scanfrom = "0") Then
Line1:
                Me.Dispatcher.Invoke(Sub()
                                         lblStatusbar.Text = "Scanning...."
                                     End Sub)
                Dim PageCount As Integer
                Dim obj As New EzofisScanInit.EzScanner
                Dim selectedScaner As String = Appcon1("ScannerName").ToString()
                If (selectedScaner = "") Then
                    scannedfilepath = obj.StartScanner(PageCount, selectedScaner)
                    'System.Configuration.ConfigurationManager.AppSettings.Set("ScannerName", selectedScaner)
                    updateinconfig("ScannerName", selectedScaner)
                Else
                    scannedfilepath = obj.StartScanner(PageCount, selectedScaner)
                End If

                ' MsgBox("ScannedFileName=" + ScannedFileName)
                Me.Dispatcher.Invoke(Sub()
                                         lblStatusbar.Text = "Total Scanned Page: " + PageCount.ToString()
                                     End Sub)

                'Dim nomsg1 As New MessageWin("Total Scanned Page: " + PageCount.ToString() + " Do you want Rescan?")
                '    nomsg1.ShowDialog()
                '    If NewWorkItem = 1 Then
                '        GoTo Line1
                '    End If
                ScannedPageCount = PageCount
                Me.Dispatcher.Invoke(Sub()
                                         Dim nomsg1 As New ScannerNotify()
                                         nomsg1.ShowDialog()
                                     End Sub)
                If CanContinue = 1 Then
                    GoTo Line1
                End If

                If (File.Exists(scannedfilepath)) Then
                    _currentFileName = scannedfilepath
                    ' _fileFormat =
                    '_bitsPerPixel = 0
                    ColorChange("COLOR")
                    Me.Dispatcher.Invoke(Sub()

                                             ECMViewer.LoadDocumentFromFileWithPageNumber(scannedfilepath)
                                             ECMViewerToolbar.IsEnabled = True

                                             UpdateFileInfo()
                                             btnINITIATESCANNING.IsEnabled = False
                                             lblStatusbar.Text = ""
                                         End Sub)
                Else
                    Me.Dispatcher.Invoke(Sub()

                                             scannedfilepath = ""
                                             _currentFileName = ""
                                             Dim c As Color = CType(ColorConverter.ConvertFromString("#C24641"), Color)
                                             lblStatusbar.Foreground = New SolidColorBrush(c)
                                             lblStatusbar.Text = "File Not Found in Scanned Location"
                                         End Sub)
                End If

            Else
                Me.Dispatcher.Invoke(Sub()
                                         lblStatusbar.Text = "Scanning...."
                                     End Sub)

                Dim myDialog As New OpenFileDialog()

                myDialog.Filter = "Tif Files|*.tif;*.TIF;*.tiff;*.TIFF"
                myDialog.Multiselect = False
                Dim res = myDialog.ShowDialog()
                If res Then
                    scannedfilepath = myDialog.FileName
                    _currentFileName = scannedfilepath
                    ' _fileFormat =
                    '_bitsPerPixel = 0

                    ColorChange("COLOR")

                    Me.Dispatcher.Invoke(Sub()

                                             ECMViewer.LoadDocumentFromFileWithPageNumber(scannedfilepath)

                                             ECMViewerToolbar.IsEnabled = True

                                             UpdateFileInfo()

                                             btnINITIATESCANNING.IsEnabled = False
                                         End Sub)

                Else
                    scannedfilepath = ""
                    _currentFileName = ""

                End If
                Me.Dispatcher.Invoke(Sub()
                                         lblStatusbar.Text = ""
                                     End Sub)

            End If
            Me.Dispatcher.Invoke(Sub()
                                     If (scannedfilepath <> "") Then
                                         btnSubmit.IsEnabled = True
                                         btnRescan.IsEnabled = True
                                         btnClearRightPane.IsEnabled = True
                                         btnFinalSubmit.IsEnabled = True

                                         btnINITIATESCANNING.IsEnabled = False
                                         cbProductList.IsEnabled = False
                                         cbPhase.IsEnabled = False
                                         'cbDocumentType.IsEnabled = False
                                     End If

                                 End Sub)

            'ExportToArchive(scannedfilepath, lastWorkItemNoFormat, dstbl, archivepath, cabinetname, cabinetid, templatename, ersdirpath, ersid, lastWorkItemNo)
            ' End If


            '  End Using
        Catch ex As Exception
            MsgBox("Exception in archiveFile " + ex.Message)
        End Try
    End Function

    Public Function updateinconfig(Key As String, Value As String)
        Try
            Dim configFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            Dim settings = configFile.AppSettings.Settings

            If settings(Key) Is Nothing Then
                settings.Add(Key, Value)
            Else
                settings(Key).Value = Value
            End If

            configFile.Save(ConfigurationSaveMode.Modified)
            ConfigurationManager.RefreshSection("appsettings")
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private bw As BackgroundWorker = New BackgroundWorker
    Private submittedWIGridWin As Object

    Public Function getLocallyStoredFiles(qry As String) As List(Of String)
        Try

            Dim LocallyStoredFilesList As New List(Of String)
            Dim ResDs = invitaAPIobj.GetDatasetByQuery(qry)

            If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then
                For i = 0 To ResDs.Tables(0).Rows.Count - 1
                    If (ResDs.Tables(0).Rows(i).Item("ifilename") <> "") Then
                        If (File.Exists(ResDs.Tables(0).Rows(i).Item("ifilename"))) Then
                            LocallyStoredFilesList.Add(ResDs.Tables(0).Rows(i).Item("ifilename").ToString)
                        End If
                    End If
                Next
            End If
            Return LocallyStoredFilesList
        Catch ex As Exception
            MsgBox("Exception in getLocallyStoredFiles" & ex.Message)
        End Try
    End Function

    Public Function ExportToArchive(scannedfilepath As String, lastWorkItemNoFormat As String, ByRef dstbl As DataSet, cabinetname As String, cabinetid As String, templatename As String, ersdirpath As String, ersid As Integer, lastWorkItemNo As Integer)
        Try

            If (Directory.Exists(ersdirpath)) Then

                Me.Dispatcher.Invoke(Sub()
                                         lblStatusbar.Text = "Exporting...."
                                     End Sub)


                ' check the source file size is 0 KB
                feAccNo = ""
                feWorkItemRefNo = lastWorkItemNoFormat
                feRimNo = ""
                feAccType = ""
                feAccStatus = ""
                'Dim View As DataView = TryCast(gridAccountDetails.ItemsSource, DataView)
                'Dim ViewRow As DataRowView
                'Dim ColumnValue As Object
                'msgbox("alert1")
                If Not IsNothing(ResAccBasicInfoObj) AndAlso ResAccBasicInfoObj.Tables.Count > 0 AndAlso ResAccBasicInfoObj.Tables(0).Rows.Count > 0 Then
                    '     gridAccountDetails.SelectedIndex = 0
                    Dim ViewRow = ResAccBasicInfoObj.Tables(0).Rows(0)
                    feAccNo = ViewRow("AccountNo").ToString()
                    feRimNo = ViewRow("RIMNumber").ToString()
                    feAccType = ViewRow("AccountType").ToString()
                    feAccStatus = ViewRow("Status").ToString()
                    ' ColumnValue = ViewRow.Item("ID")  'or ViewRow.Item(0) for positional value.
                    'do something with ColumnValue here.
                End If

                ' Dim feProduct = "" ' = cbProductList.SelectedValue.ToString()
                'Dim fePhase = "" '= cbPhase.SelectedValue.ToString()
                ' Dim feDocType = "" '= cbDocumentType.SelectedValue.ToString()

                Me.Dispatcher.Invoke(Sub()
                                         If (cbProductList.SelectedIndex <> -1) Then
                                             feProduct = cbProductList.SelectedValue.ToString()
                                         Else
                                             feProduct = ""
                                         End If
                                         If (cbPhase.SelectedIndex <> -1) Then
                                             fePhase = cbPhase.SelectedValue.ToString()
                                         Else
                                             fePhase = ""
                                         End If
                                         'If (cbDocumentType.SelectedIndex <> -1) Then
                                         '    'feDocType = cbDocumentType.SelectedValue.ToString()
                                         '    feDocType = gridSelectedCellDocumentTypeValue.ToString()
                                         'Else
                                         '    feDocType = ""
                                         'End If
                                         If (gridSelectedCellDocumentTypeValue.ToString() = "") Then
                                             feDocType = ""
                                         Else
                                             ' feDocType = cbDocumentType.SelectedValue.ToString()
                                             feDocType = gridSelectedCellDocumentTypeValue.ToString
                                         End If
                                     End Sub)

                If (sessionid <> "") Then

                    Dim qry = "select * from ezca_" & cabinetid.ToString & "_" & invitaAPIobj.TemplateId & "_stage where [Account Number]='" & feAccNo & "' and Product='" & feProduct & "' and Phase='" & fePhase & "' and ifilepath like '%" & sessionid & "%' order by itemid"
                    'Dim LocallyStoredFilesList = getLocallyStoredFiles(qry)
                    Dim ResDs = invitaAPIobj.GetDatasetByQuery(qry)

                    Dim updatedon As Date


                    'FinalSubmissionPath = ""

                    If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then


                        If (feAccNo <> "") Then
                            If (feProduct <> "") Then
                                If (fePhase <> "") Then
                                    ' MsgBox("before generate  work item refernce no " & WorkitemReference.ToString)
                                    Dim qrytbl = "select * from ezWorkItemIdGen"
                                    Dim dstbl1 = invitaAPIobj.GetDatasetByQuery(qrytbl)

                                    If Not IsNothing(dstbl) AndAlso dstbl.Tables.Count > 0 AndAlso dstbl.Tables(0).Rows.Count > 0 Then
                                        ' archivefolderpath = ""
                                        cabinetname = dstbl.Tables(0).Rows(0).Item("CabinetName")
                                        cabinetid = dstbl.Tables(0).Rows(0).Item("CabinetID")
                                        templatename = dstbl.Tables(0).Rows(0).Item("TemplateName")
                                        ersdirpath = dstbl.Tables(0).Rows(0).Item("ERSDirPath")
                                        ersid = dstbl.Tables(0).Rows(0).Item("ERSId")
                                    End If
                                    If WorkitemReference = "" Then
                                        If Not IsNothing(dstbl1) AndAlso dstbl1.Tables.Count > 0 AndAlso dstbl1.Tables(0).Rows.Count > 0 Then
                                            '  MsgBox("within if generate  work item refernce no " & WorkitemReference.ToString)
                                            lastWorkItemNo = dstbl1.Tables(0).Rows(0).Item("LastWorkItemNo") + 1
                                            Dim noofchars = dstbl1.Tables(0).Rows(0).Item("Noofchars")
                                            Dim prefix = dstbl1.Tables(0).Rows(0).Item("Prefix")
                                            updatedon = dstbl1.Tables(0).Rows(0).Item("UpdatedOn")
                                            lastWorkItemNoFormat = generateWorkItemNo(lastWorkItemNo, noofchars, prefix, updatedon)
                                            ' MsgBox("after workitem generate " & lastWorkItemNoFormat.ToString)
                                        End If
                                    Else
                                        If Not IsNothing(dstbl1) AndAlso dstbl1.Tables.Count > 0 AndAlso dstbl1.Tables(0).Rows.Count > 0 Then
                                            '  MsgBox("else generate  work item refernce no " & WorkitemReference.ToString)
                                            lastWorkItemNo = dstbl1.Tables(0).Rows(0).Item("LastWorkItemNo")
                                            Dim noofchars = dstbl1.Tables(0).Rows(0).Item("Noofchars")
                                            Dim prefix = dstbl1.Tables(0).Rows(0).Item("Prefix")
                                            updatedon = dstbl1.Tables(0).Rows(0).Item("UpdatedOn")
                                            lastWorkItemNoFormat = generateWorkItemNo(lastWorkItemNo, noofchars, prefix, updatedon)
                                            ' MsgBox("after workitem generate " & lastWorkItemNoFormat.ToString)
                                        End If
                                    End If
                                    'Dim resWIR = CheckWorkItemAlreadyexists(feAccNo, feProduct, fePhase, cabinetid)
                                    '    MsgBox("work item already exists" & resWIR.ToString)
                                    '    If resWIR Then
                                    '    'NewWorkItem = 0
                                    '    'Dim nomsg1 As New MessageWin("Work Item Already Exist.
                                    '    'For the same Product and Phase. Do you want create New Work Item?")
                                    '    'nomsg1.ShowDialog()
                                    '    'If NewWorkItem = 1 Then

                                    '    'Else
                                    '    '    Exit Function
                                    '    'End If
                                    'Else

                                    'End If
                                    WorkitemReference = lastWorkItemNoFormat
                                    Me.Dispatcher.Invoke(Sub()
                                                             txtwir.Text = WorkitemReference
                                                         End Sub)

                                End If
                            End If
                        ElseIf txtWorkItemNo.Text <> "" Then
                            WorkitemReference = txtWorkItemNo.Text
                            Me.Dispatcher.Invoke(Sub()
                                                     txtwir.Text = WorkitemReference
                                                 End Sub)
                        End If

                        Dim fileconversionflag As Boolean = False
                        For ij = 0 To ResDs.Tables(0).Rows.Count - 1

                            Me.Dispatcher.Invoke(Sub()
                                                     lblStatusbar.Text = "Exporting...."
                                                 End Sub)

                            GetCabinetFieldsFromStage(ResDs.Tables(0).Rows(ij).Item("Document type").ToString)

                            Dim qrywigen As String = ""
                            Dim archivepath As String = ""
                            Dim archivefolderpath As String = ""
                            Dim sourcefilename As String = ""
                            Dim fieldvalues As String
                            Dim fieldname As String
                            Dim fieldlevel As Integer
                            Dim fielddatatypeid As Integer

                            If (File.Exists(ResDs.Tables(0).Rows(ij).Item("ifilename").ToString)) Then
                                Dim fi As New FileInfo(ResDs.Tables(0).Rows(ij).Item("ifilename").ToString)
                                Dim filesize = fi.Length

                                scannedfilepath = ResDs.Tables(0).Rows(ij).Item("ifilename").ToString

                                'msgbox("alert2")
                                'form archive path except the last row-begin
                                For m = 0 To dstbl.Tables(0).Rows.Count - 2
                                    If (dstbl.Tables(0).Rows(m).Item("FieldLevel") <> 0) Then
                                        If (dstbl.Tables(0).Rows(m).Item("FieldName").ToString() = "Product") Then
                                            archivepath = Path.Combine(archivepath, feProduct.ToString().Trim)
                                        ElseIf (dstbl.Tables(0).Rows(m).Item("FieldName").ToString() = "Phase") Then
                                            archivepath = Path.Combine(archivepath, fePhase.ToString().Trim)
                                        ElseIf (dstbl.Tables(0).Rows(m).Item("FieldName").ToString() = "Work Item Reference") Then
                                            archivepath = Path.Combine(archivepath, WorkitemReference.ToString().Trim)
                                        ElseIf (dstbl.Tables(0).Rows(m).Item("FieldName").ToString() = "Document Type") Then
                                            archivepath = Path.Combine(archivepath, feDocType.ToString().Trim)
                                        ElseIf (dstbl.Tables(0).Rows(m).Item("FieldName").ToString() = "RIM Number") Then
                                            archivepath = Path.Combine(archivepath, feRimNo.ToString().Trim)
                                        ElseIf (dstbl.Tables(0).Rows(m).Item("FieldName").ToString() = "Account Type") Then
                                            archivepath = Path.Combine(archivepath, feAccType.ToString().Trim)
                                        ElseIf (dstbl.Tables(0).Rows(m).Item("FieldName").ToString() = "Account Number") Then
                                            archivepath = Path.Combine(archivepath, feAccNo.ToString().Trim)
                                        ElseIf (dstbl.Tables(0).Rows(m).Item("FieldName").ToString() = "Account Status") Then
                                            archivepath = Path.Combine(archivepath, feAccStatus.ToString().Trim)
                                        End If
                                    End If
                                Next
                                'form archive path end
                                archivefolderpath = archivepath
                                archivefolderpath = Path.Combine(cabinetname, templatename, archivefolderpath)
                                FinalSubmissionPath = archivefolderpath
                                'msgbox("alert3")
                                ' form the filename begin
                                If (dstbl.Tables(0).Rows(dstbl.Tables(0).Rows.Count - 1).Item("FieldName").ToString() = "Document Type") Then
                                    'sourcefilename = feDocType.ToString()
                                    'sourcefilename = gridSelectedCellDocumentTypeValue.ToString
                                    sourcefilename = ResDs.Tables(0).Rows(ij).Item("Document Type").ToString.Replace("'", "") & Path.GetExtension(ResDs.Tables(0).Rows(ij).Item("ifilename").ToString)
                                ElseIf (dstbl.Tables(0).Rows(dstbl.Tables(0).Rows.Count - 1).Item("FieldName").ToString() = "Product") Then
                                    sourcefilename = feProduct.ToString()
                                ElseIf (dstbl.Tables(0).Rows(dstbl.Tables(0).Rows.Count - 1).Item("FieldName").ToString() = "Phase") Then
                                    sourcefilename = fePhase.ToString()
                                ElseIf (dstbl.Tables(0).Rows(dstbl.Tables(0).Rows.Count - 1).Item("FieldName").ToString() = "Work Item Reference") Then
                                    sourcefilename = WorkitemReference.ToString()
                                ElseIf (dstbl.Tables(0).Rows(dstbl.Tables(0).Rows.Count - 1).Item("FieldName").ToString() = "RIM Number") Then
                                    'ColumnValue = ViewRow.Item(2)
                                    sourcefilename = feRimNo
                                ElseIf (dstbl.Tables(0).Rows(dstbl.Tables(0).Rows.Count - 1).Item("FieldName").ToString() = "Account Type") Then
                                    ' ColumnValue = ViewRow.Item(1)
                                    sourcefilename = feAccType
                                ElseIf (dstbl.Tables(0).Rows(dstbl.Tables(0).Rows.Count - 1).Item("FieldName").ToString() = "Account Number") Then
                                    ' ColumnValue = ViewRow.Item(0)
                                    sourcefilename = feAccNo
                                ElseIf (dstbl.Tables(0).Rows(dstbl.Tables(0).Rows.Count - 1).Item("FieldName").ToString() = "Account Status") Then
                                    'ColumnValue = ViewRow.Item(3)
                                    sourcefilename = feAccStatus
                                End If

                                '------------------file convert pdf-----------------


                                'Dim infile As String = scannedfilepath
                                'Dim infile As String = LocallyStoredFilesList(ij)
                                Dim infile As String = ResDs.Tables(0).Rows(ij).Item("ifilename").ToString()
                                Dim volume As String = Path.Combine(ersdirpath, archivefolderpath)

                                Dim pdffilename As String = Path.GetFileNameWithoutExtension(sourcefilename)
                                Dim strtitle As String = ""
                                Dim strsubject As String = ""
                                Dim strauthor As String = ""
                                Dim strkeyword As String = ""
                                Dim strsignature As String = ""

                                Dim archivefolderqry = "SELECT Title,Author,Subject,Keyword,Signature FROM eZPdfProperties where TemplateId=" & invitaAPIobj.TemplateId & ";"
                                Dim tempdstbl = invitaAPIobj.GetDatasetByQuery(archivefolderqry)

                                'msgbox("alert6")
                                If Not IsNothing(tempdstbl) AndAlso tempdstbl.Tables.Count > 0 AndAlso tempdstbl.Tables(0).Rows.Count > 0 Then
                                    'shiva
                                    strtitle = tempdstbl.Tables(0).Rows(0).Item("Title").ToString.Trim
                                    strtitle = getTemplateFieldValue(strtitle)
                                    strsubject = tempdstbl.Tables(0).Rows(0).Item("Subject").ToString.Trim
                                    strsubject = getTemplateFieldValue(strsubject)
                                    'strauthor = tempdstbl.Tables(0).Rows(0).Item("Author")
                                    strauthor = WorkitemReference.ToString() 'getTemplateFieldValue(strauthor)
                                    strkeyword = tempdstbl.Tables(0).Rows(0).Item("Keyword").ToString.Trim
                                    strkeyword = getTemplateFieldValue(strkeyword)
                                    strsignature = tempdstbl.Tables(0).Rows(0).Item("Signature").ToString.Trim
                                    strsignature = getTemplateFieldValue(strsignature)
                                    'shiva
                                End If

                                Dim destinationFilepath = Path.Combine(volume, pdffilename & ".pdf")
                                Dim resversioninfo = isFileExistsInArchiveLocation(destinationFilepath, feProduct, fePhase, feAccNo, cabinetid, invitaAPIobj.TemplateId)
                                Dim fileversion As String = "1.0"

                                If Not IsNothing(resversioninfo) AndAlso resversioninfo.Count > 0 Then
                                    If (resversioninfo.Item(0) <> "1") Then
                                        'create a versioning file
                                        fileversion = resversioninfo.Item(0)
                                    End If
                                    If (resversioninfo.Item(1) <> "") Then
                                        'create a versioning file
                                        pdffilename = resversioninfo.Item(1)
                                    End If
                                End If
                                Try

                                    Dim pdfconvobj As New maincls()

                                    Dim respdffile
                                    'msgbox("alert7")
                                    If (strtitle <> "" And strsubject <> "" And strauthor <> "" And strkeyword <> "" And strsignature <> "") Then
                                        respdffile = pdfconvobj.PdfconvertByVeryWrite(infile, volume, pdffilename, strtitle, strsubject, strauthor, strkeyword, strsignature)
                                    Else
                                        If (strtitle = "") Then strtitle = "BBK Trade Finance"
                                        If strsubject = "" Then strsubject = "BBK Trade Finance"
                                        If (strauthor = "") Then strauthor = "BBK Trade Finance"
                                        If strkeyword = "" Then strkeyword = "BBK Trade Finance"
                                        If (strsignature = "") Then strsignature = "BBK Trade Finance"

                                        respdffile = pdfconvobj.PdfconvertByVeryWrite(infile, volume, pdffilename, strtitle, strsubject, strauthor, strkeyword, strsignature)
                                    End If


                                    '{"Filesize":83123,"InfilePages":0,"OutfilePages":1,"Status":"Success"}
                                    'Imports Newtonsoft.Json.Linq
                                    Dim jsonResult = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(respdffile)
                                    Dim OutFilesize As String = "0"
                                    Dim OutfileNoofPages As Integer = 0
                                    Dim Infilepages As Integer = 0
                                    If (Not IsNothing(jsonResult)) AndAlso jsonResult.Item("Status") = "Success" Then
                                        OutFilesize = jsonResult.Item("Filesize")
                                        OutfileNoofPages = Convert.ToInt32(jsonResult.Item("OutfilePages"))
                                        Infilepages = Convert.ToInt32(jsonResult.Item("InfilePages"))
                                        If (OutFilesize = "0" Or (OutfileNoofPages < Infilepages)) Then
                                            Me.Dispatcher.Invoke(Sub()
                                                                     fileconversionflag = True
                                                                     lblStatusbar.Text = "File Convertion Failed"
                                                                     btnRescan.IsEnabled = True
                                                                     btnClearRightPane.IsEnabled = True
                                                                     btnSubmit.IsEnabled = True
                                                                     btnFinalSubmit.IsEnabled = True
                                                                 End Sub)
                                        Else
                                            Dim itemid = archivefiletotable(cabinetid, cabinetname, invitaAPIobj.TemplateId, scannedfilepath, archivefolderpath, ersdirpath, ersid, tablefieldnames, tablefieldvalues, pdffilename + ".pdf", lastWorkItemNo, strtitle, strsubject, strauthor, strkeyword, OutFilesize, OutfileNoofPages, fileversion)
                                            ' MsgBox("Archivepath=" & archivefolderpath)
                                            sharedCAC.InsertFolders(archivefolderpath, "0")

                                            If Not IsNothing(itemid) AndAlso itemid <> 0 Then
                                                Dim resBBKQ = CheckWorkItemexistsInBBKTicketQ(WorkitemReference)

                                                If (resBBKQ = "") Then
                                                    Dim qryticketqueuetbl = "insert into BBK_TicketQueue ([Account Number],[Product],[Phase],[Work Item Reference],[CreatedOn],[UpdatedOn],[CreatedBy],[UpdatedBy],[Isdeleted]) values ('" & feAccNo & "','" & feProduct & "','" & fePhase & "','" & WorkitemReference & "','" & Format(Now(), "dd/MMM/yyyy hh:mm:ss tt") & "','',1,0,0" & ")"
                                                    Dim res2 = invitaAPIobj.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qryticketqueuetbl)
                                                    'msgbox("alert9")
                                                End If

                                                Me.Dispatcher.Invoke(Sub()
                                                                         lblStatusbar.Text = "Document Exported successfully for " + WorkitemReference
                                                                         ECMViewer._documentViewer.SetDocument(Nothing)
                                                                         ECMViewerToolbar.IsEnabled = False
                                                                         LblFileName.Content = ""
                                                                     End Sub)

                                            Else
                                                Me.Dispatcher.Invoke(Sub()
                                                                         lblStatusbar.Text = "Issue while Archiving, Please check the network access! "
                                                                     End Sub)
                                            End If


                                        End If
                                    Else
                                        Me.Dispatcher.Invoke(Sub()
                                                                 fileconversionflag = True
                                                                 lblStatusbar.Text = "File Convertion Failed"
                                                                 btnRescan.IsEnabled = True
                                                                 btnClearRightPane.IsEnabled = True
                                                                 btnSubmit.IsEnabled = True
                                                                 btnFinalSubmit.IsEnabled = True
                                                             End Sub)
                                    End If
                                Catch ex As Exception
                                    MsgBox("Exception in ExportToArchive-File" & ex.Message)
                                End Try


                            End If
                        Next


                        CanContinue = 0
                        WorkItmref = WorkitemReference
                        Try
                            Dim destinationpath = Path.Combine("c:\imaging", invitaAPIobj.TemplateId, feAccNo, sessionid)
                            If Directory.Exists(destinationpath) Then
                                For Each dirfile As String In Directory.GetFiles(destinationpath)
                                    File.Delete(dirfile)
                                Next
                                Directory.Delete(destinationpath)
                            End If
                        Catch ex As Exception
                            MsgBox("Exception in ExportToArchive - Please check The imaging Folder Access Permission " & ex.Message)
                        End Try


                        If (fileconversionflag = True) Then
                            Me.Dispatcher.Invoke(Sub()
                                                     lblStatusbar.Text = "File Convertion Failed"
                                                     btnRescan.IsEnabled = True
                                                     btnClearRightPane.IsEnabled = True
                                                     btnSubmit.IsEnabled = True
                                                     btnFinalSubmit.IsEnabled = True
                                                 End Sub)
                        Else
                            Me.Dispatcher.Invoke(Sub()
                                                         Dim notmsg As New notifywin(FinalSubmissionPath, feProduct, fePhase, feAccNo, cabinetid)
                                                         notmsg.FinalSubmissionPath = FinalSubmissionPath
                                                         Dim resultdlg = notmsg.ShowDialog()

                                                         If CanContinue = 1 Then
                                                             verifyWorkItemReference(WorkItmref, 1)
                                                         ElseIf CanContinue = 2 Then
                                                             WorkItmref = ""
                                                             WorkitemReference = ""
                                                             verifyWorkItemReference(WorkItmref, 2)
                                                             WorkItmref = ""
                                                             WorkitemReference = ""
                                                         End If
                                                     End Sub)

                        End If
                    End If

                    sessionid = ""
                Else
                    Me.Dispatcher.Invoke(Sub()
                                             Dim msg As New MessageWin("Scanned Docment Not Saved..." & vbCrLf & "Do you want to Save?")
                                             msg.ShowDialog()
                                             If (NewWorkItem = 1) Then
                                                 SaveFileInLocal()
                                             Else
                                                 ClearFilepathIdInGrid()
                                             End If
                                         End Sub)
                End If
            Else
                Me.Dispatcher.Invoke(Sub()
                                         lblStatusbar.Text = "Please Check The Archive Path"
                                     End Sub)
            End If

        Catch ex As Exception
            MsgBox("Exception in ExportToArchive" & ex.Message)
        End Try
    End Function


    'Public Sub UpdateFileInfo()
    '    Try
    '        LblFileName.Content = ""
    '        btnvisiblitytocollapsed()
    '        If _currentFileName <> "" Then
    '            LblFileName.Content = "    FileName : " + _currentFileName.Replace(Pubvariable.imaging + "\", "") + "    [Page " + ECMViewer.pageNumber.ToString() + " of " + ECMViewer.pageCount.ToString() + "]"
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Public Function isFileExistsInArchiveLocation(DestinationFilepath As String, Product As String, Phase As String, AccNo As String, cabinetid As Integer, templateid As Integer) As List(Of String)
        Try
            Dim resVersioninfo As New List(Of String)
            Dim resfilename As String = ""
            Dim resVersion As Integer = 0
            Dim qryver As String = ""
            Dim dstbl1 As New DataSet
            Dim dstbl2 As New DataSet


            If (File.Exists(DestinationFilepath)) Then
                qryver = "select DuplicateType from (select DuplicateTypeId from eZTemplate where cabinetid=" & cabinetid & " and TemplateId=" & templateid & ") as r1 left join eZDuplicateType as duptypetbl on r1.DuplicateTypeId=duptypetbl.DuplicateTypeId"
                dstbl1 = invitaAPIobj.GetDatasetByQuery(qryver)
                If Not IsNothing(dstbl1) AndAlso dstbl1.Tables.Count > 0 AndAlso dstbl1.Tables(0).Rows.Count > 0 Then
                    Dim filenamewoext = Path.GetFileNameWithoutExtension(DestinationFilepath)
                    'Dim filenameext = Path.GetExtension(DestinationFilepath)


                    filenamewoext.Replace("'", "''")
                    Product.Replace("'", "''")
                    Phase.Replace("'", "''")



                    If (dstbl1.Tables(0).Rows(0).Item("DuplicateType").ToString = "_1") Then
                        qryver = "select top 1 [ifilename],[version] from eZCA_" & cabinetid & "_" & invitaAPIobj.TemplateId & "_items where Product='" & Product & "' and Phase='" & Phase & "' and [Account Number]='" & AccNo & "' and ifilename like '" & filenamewoext & "%' order by itemid desc"
                        dstbl2 = invitaAPIobj.GetDatasetByQuery(qryver)

                        If Not IsNothing(dstbl2) AndAlso dstbl2.Tables.Count > 0 AndAlso dstbl2.Tables(0).Rows.Count > 0 Then
                            Dim tbllastversionno As String = dstbl2.Tables(0).Rows(0).Item("version").ToString
                            tbllastversionno = tbllastversionno.Replace(".0", "")
                            Dim newversionno As Integer = Convert.ToInt32(tbllastversionno) + 1
                            resVersion = newversionno

                            Dim tblifilename As String = dstbl2.Tables(0).Rows(0).Item("ifilename").ToString
                            Dim lastsufix As Integer = newversionno
                            resfilename = filenamewoext & "_" & newversionno




                            resVersioninfo.Add(resVersion.ToString)
                            resVersioninfo.Add(resfilename.ToString)
                            '  resVersioninfo(0) = resVersion.ToString
                            '  resVersioninfo(1) = resfilename
                            Return resVersioninfo
                        Else
                            Dim newversionno As Integer = 1
                            resVersion = newversionno
                            resfilename = filenamewoext & "_" & newversionno & ".0"
                            resVersioninfo.Add(resVersion.ToString)
                            resVersioninfo.Add(resfilename.ToString)
                            Return resVersioninfo
                        End If
                    Else
                        qryver = "select top 1 [ifilename],[version] from eZCA_" & cabinetid & "_" & invitaAPIobj.TemplateId & "_items where Product='" & Product & "' and Phase='" & Phase & "' and [Account Number]='" & AccNo & "' and ifilename like '" & filenamewoext & "%' order by itemid desc"
                        dstbl2 = invitaAPIobj.GetDatasetByQuery(qryver)



                        If Not IsNothing(dstbl2) AndAlso dstbl2.Tables.Count > 0 AndAlso dstbl2.Tables(0).Rows.Count > 0 Then
                            Dim tbllastversionno As String = dstbl2.Tables(0).Rows(0).Item("version").ToString
                            tbllastversionno = tbllastversionno.Replace(".0", "")
                            Dim newversionno As Integer = Convert.ToInt32(tbllastversionno) + 1
                            resVersion = newversionno



                            Dim tblifilename As String = dstbl2.Tables(0).Rows(0).Item("ifilename").ToString
                            Dim lastsufix As Integer = newversionno
                            ' resfilename(0) = filenamewoext & "_" & Format(Now(), "ddMMyyyyhhmmss") & filenameext
                            resfilename = filenamewoext & "_" & Format(Now(), "ddMMyyyyhhmmss")



                            resVersioninfo.Add(resVersion.ToString)
                            resVersioninfo.Add(resfilename.ToString)
                            Return resVersioninfo
                        Else
                            Dim newversionno As Integer = 1
                            resVersion = newversionno
                            resfilename = filenamewoext & "_" & Format(Now(), "ddMMyyyyhhmmss")
                            resVersioninfo.Add(resVersion.ToString)
                            resVersioninfo.Add(resfilename.ToString)
                            Return resVersioninfo
                        End If
                    End If
                    ' End If
                End If
            Else
                qryver = "select DuplicateType from (select DuplicateTypeId from eZTemplate where cabinetid=" & cabinetid & " and TemplateId=" & templateid & ") as r1 left join eZDuplicateType as duptypetbl on r1.DuplicateTypeId=duptypetbl.DuplicateTypeId"
                dstbl1 = invitaAPIobj.GetDatasetByQuery(qryver)
                If Not IsNothing(dstbl1) AndAlso dstbl1.Tables.Count > 0 AndAlso dstbl1.Tables(0).Rows.Count > 0 Then
                    Dim filenamewoext = Path.GetFileNameWithoutExtension(DestinationFilepath)
                    Dim filenameext = Path.GetExtension(DestinationFilepath)

                    If (dstbl1.Tables(0).Rows(0).Item("DuplicateType").ToString = "_1") Then
                        Dim newversionno As Integer = 1
                        resVersion = 1
                        resfilename = filenamewoext
                        resVersioninfo.Add(resVersion.ToString)
                        resVersioninfo.Add("")
                        Return resVersioninfo
                        'End If
                    Else
                        Dim newversionno As Integer = 1
                        resVersion = newversionno
                        resfilename = filenamewoext
                        resVersioninfo.Add(resVersion.ToString)
                        resVersioninfo.Add("")
                        Return resVersioninfo
                        ' End If
                    End If
                    ' End If
                End If



            End If
            Return resVersioninfo
        Catch ex As Exception
            MsgBox("Exception in CheckWorkItemAlreadyexists" + ex.Message)
        End Try
    End Function

    Public Function getTemplateFieldValue(parameter As String) As String
        Try
            Dim result As String
            Me.Dispatcher.Invoke(Sub()
                                     If (parameter = "Product") Then
                                         result = cbProductList.SelectedValue.ToString()
                                     ElseIf (parameter = "Phase") Then
                                         result = cbPhase.SelectedValue.ToString()
                                     ElseIf (parameter = "Work Item Reference") Then
                                         result = feWorkItemRefNo.ToString()
                                     ElseIf (parameter = "Document Type") Then
                                         result = cbDocumentType.SelectedValue.ToString()
                                     ElseIf (parameter = "RIM Number") Then
                                         result = feRimNo
                                     ElseIf (parameter = "Account Type") Then
                                         result = feAccType
                                     ElseIf (parameter = "Account Number") Then
                                         result = feAccNo
                                     ElseIf (parameter = "Account Status") Then
                                         result = feAccStatus
                                     Else
                                         result = ""
                                     End If
                                 End Sub)
            Return result
        Catch ex As Exception
            MsgBox("Exception in getTemplateFieldValue" + ex.Message)
        End Try
    End Function

    Public Function generateWorkItemNo(lastWorkItemNo As Integer, noofchars As Integer, prefix As String, updatedon As String) As String
        Try
            Dim qryWItbl As String = ""
            '  MsgBox("within generate lastWorkItemNo=" & lastWorkItemNo.ToString & "  updated on =" & updatedon.ToString)
            Dim lastWorkItemNoFormat As String = ""
            Dim sysdate As Date

            If (lastWorkItemNo.ToString.Length = noofchars) Then

                lastWorkItemNoFormat = prefix & Format(Today, "ddMMyyyy") & lastWorkItemNo.ToString()
                ' MsgBox("within if lastWorkItemNoFormat=" & lastWorkItemNoFormat.ToString)
            Else
                Dim lastWorkItemNolen = lastWorkItemNo.ToString.Length
                Dim difflastWorkItemNo As Integer = noofchars - lastWorkItemNolen
                Dim t = StrDup(difflastWorkItemNo, "0")
                lastWorkItemNoFormat = prefix & Format(Today, "ddMMyyyy") & t & lastWorkItemNo.ToString()
                '  MsgBox("within else lastWorkItemNoFormat=" & lastWorkItemNoFormat.ToString)
            End If

            Try
                sysdate = Format(Today, "dd/MMM/yyyy")
                ' MsgBox("sysdate=" & sysdate)
            Catch ex As Exception
                MsgBox("exception in sydate" & ex.Message)
            End Try

            Try

                Dim d1 = Format(Convert.ToDateTime(sysdate), "dd/MMM/yyyy")
                'MsgBox("d1=" & d1.ToString)
                Dim d2
                If (updatedon = "") Then
                    d2 = d1
                Else
                    d2 = Format(Convert.ToDateTime(updatedon), "dd/MMM/yyyy")
                End If
                'MsgBox("d2=" & d2.ToString)


                ' MsgBox("d1=" & d1.ToString & "d2=" & d2.to)

                If (d1 = d2) Then
                    qryWItbl = "Update ezWorkItemIdGen set LastWorkItemNo=" & lastWorkItemNo.ToString() & ",UpdatedOn='" + Format(Now(), "dd/MMM/yyyy hh:mm:ss tt") + "'"
                Else
                    lastWorkItemNo = 1
                    qryWItbl = "Update ezWorkItemIdGen set LastWorkItemNo=" & lastWorkItemNo.ToString() & ",UpdatedOn='" + Format(Now(), "dd/MMM/yyyy hh:mm:ss tt") + "'"
                    Dim lastWorkItemNolen = lastWorkItemNo.ToString.Length
                    Dim difflastWorkItemNo As Integer = noofchars - lastWorkItemNolen
                    Dim t = StrDup(difflastWorkItemNo, "0")
                    lastWorkItemNoFormat = prefix & Format(Today, "ddMMyyyy") & t & lastWorkItemNo.ToString()
                End If
            Catch ex As Exception
                MsgBox("exception in updatedon" & ex.Message)
            End Try

            Dim res = invitaAPIobj.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qryWItbl)


            Return lastWorkItemNoFormat

        Catch ex As Exception
            MsgBox("Exception in generate work item " & ex.Message)
        End Try
    End Function
    Public Function removeTempPgTxtfiles(pdffilename As String, ersdirpath As String, archivefolderpath As String)
        Dim removefile1 As String
        removefile1 = Path.GetFileNameWithoutExtension(pdffilename) & "_pg_0.txt"
        removefile1 = Path.Combine(ersdirpath, archivefolderpath, removefile1)
        If (File.Exists(removefile1)) Then
            File.Delete(removefile1)
        End If
        Dim removefile2 As String
        removefile2 = Path.GetFileNameWithoutExtension(pdffilename) & "_pg_1.txt"
        removefile2 = Path.Combine(ersdirpath, archivefolderpath, removefile2)
        If (File.Exists(removefile2)) Then
            File.Delete(removefile2)
        End If
    End Function


    Public Function CheckWorkItemexistsInBBKTicketQ(workitemrefno As String) As String
        Try
            Dim qrywigen = "select [Work Item Reference] from BBK_TicketQueue where [Work Item Reference]='" & workitemrefno & "'"
            Dim lastWorkItemNoFormat As String = ""
            Dim dstbl1 = invitaAPIobj.GetDatasetByQuery(qrywigen)



            If Not IsNothing(dstbl1) AndAlso dstbl1.Tables.Count > 0 AndAlso dstbl1.Tables(0).Rows.Count > 0 Then
                lastWorkItemNoFormat = dstbl1.Tables(0).Rows(0).Item("Work Item Reference")
                Return lastWorkItemNoFormat
            Else
                Return ""
            End If
        Catch ex As Exception
            MsgBox("Exception in CheckWorkItemAlreadyexists" + ex.Message)
        End Try
    End Function

    Public Function copyfiletoarchivefolder(ersdirpath As String, archivefolderpath As String, scannedfilepath As String, sourcefilename As String) As String

        'Dim archivefolder As String
        ' archivefolder = archivefolderpath.Substring(0, archivefolderpath.LastIndexOf("\"))
        ' Dim filename = scannedfilepath.Substring(scannedfilepath.LastIndexOf("\") + 1)
        Dim filename = sourcefilename & Path.GetExtension(scannedfilepath)
        'filename = filename & Path.GetExtension(ScannedFilename)


        Dim destinationpath = Path.Combine(ersdirpath, archivefolderpath)
        Dim destinationfilepath As String

        If (Not Directory.Exists(destinationpath)) Then
            Directory.CreateDirectory(destinationpath)
        End If
        destinationfilepath = Path.Combine(destinationpath, filename)

        If (Not File.Exists(destinationfilepath)) Then
            File.Copy(scannedfilepath, destinationfilepath, False)
        Else
            filename = sourcefilename 'scannedfilepath.Substring(scannedfilepath.LastIndexOf("\") + 1)
            filename = filename & Format(Now, "ddmmyyyyhhmmss") & Path.GetExtension(scannedfilepath)
            destinationfilepath = Path.Combine(destinationpath, filename)
            File.Copy(scannedfilepath, destinationfilepath)
        End If
        Return filename
    End Function
    Public Function GetCabinetFields()
        Try
            Dim fieldname As String = ""
            Dim fieldlevel As String = ""
            Dim fielddatatypeid As String = ""
            tablefieldnames = ""
            tablefieldvalues = ""

            Dim ViewRow = ResAccBasicInfoObj.Tables(0).Rows(0)
            feAccNo = ViewRow("AccountNo").ToString()
            feRimNo = ViewRow("RIMNumber").ToString()
            feAccType = ViewRow("AccountType").ToString()
            feAccStatus = ViewRow("Status").ToString()

            Me.Dispatcher.Invoke(Sub()
                                     If (txtwir.Text = "") Then
                                         WorkitemReference = ""
                                     Else
                                         WorkitemReference = txtwir.Text
                                     End If
                                 End Sub)

            feProduct = ""
            fePhase = ""
            feDocType = ""

            Me.Dispatcher.Invoke(Sub()
                                     If (cbProductList.SelectedIndex <> -1) Then
                                         feProduct = cbProductList.SelectedValue.ToString()
                                     Else
                                         feProduct = ""
                                     End If
                                     If (cbPhase.SelectedIndex <> -1) Then
                                         fePhase = cbPhase.SelectedValue.ToString()
                                     Else
                                         fePhase = ""
                                     End If
                                     'If (cbDocumentType.SelectedIndex <> -1) Then
                                     '    ' feDocType = cbDocumentType.SelectedValue.ToString()
                                     '    feDocType = gridSelectedCellDocumentTypeValue.ToString
                                     'Else
                                     '    feDocType = ""
                                     'End If
                                     If (gridSelectedCellDocumentTypeValue.ToString() = "") Then
                                         feDocType = ""
                                     Else
                                         ' feDocType = cbDocumentType.SelectedValue.ToString()
                                         feDocType = gridSelectedCellDocumentTypeValue.ToString
                                     End If
                                 End Sub)


            For l = 0 To dstbl.Tables(0).Rows.Count - 1
                fieldname = dstbl.Tables(0).Rows(l).Item("FieldName")
                fieldlevel = dstbl.Tables(0).Rows(l).Item("FieldLevel")
                fielddatatypeid = dstbl.Tables(0).Rows(l).Item("DataTypeId")

                tablefieldnames = tablefieldnames & "[" & fieldname & "],"

                If (fielddatatypeid = 2 Or fielddatatypeid = 4 Or fielddatatypeid = 5) Then
                    If (fieldname = "Product") Then
                        tablefieldvalues = tablefieldvalues & "'" & feProduct.ToString().Replace("'", "''") & "',"
                        'cbProductList.IsEnabled = True
                        'tablefieldvalues = tablefieldvalues & "'" & feProduct.ToString().Replace("'", "''") & "',"
                        'cbProductList.IsEnabled = False
                    ElseIf (fieldname = "Phase") Then
                        tablefieldvalues = tablefieldvalues & "'" & fePhase.ToString().Replace("'", "''") & "',"
                        'tablefieldvalues = tablefieldvalues & "'" & cbPhase.SelectedValue.ToString().Replace("'", "''") & "',"
                    ElseIf (fieldname = "Work Item Reference") Then
                        tablefieldvalues = tablefieldvalues & "'" & WorkitemReference.ToString().Replace("'", "''") & "',"
                    ElseIf (fieldname = "Document Type") Then
                        tablefieldvalues = tablefieldvalues & "'" & feDocType.ToString().Replace("'", "''") & "',"
                        'tablefieldvalues = tablefieldvalues & "'" & cbDocumentType.SelectedValue.ToString().Replace("'", "''") & "',"
                    ElseIf (fieldname = "RIM Number") Then
                        ' ColumnValue = ViewRow.Item(2)
                        tablefieldvalues = tablefieldvalues & "'" & feRimNo & "',"
                    ElseIf (fieldname = "Account Type") Then
                        ' ColumnValue = ViewRow.Item(1)
                        tablefieldvalues = tablefieldvalues & "'" & feAccType & "',"
                    ElseIf (fieldname = "Account Number") Then
                        '  ColumnValue = ViewRow.Item(0)
                        tablefieldvalues = tablefieldvalues & "'" & feAccNo & "',"
                    ElseIf (fieldname = "Account Status") Then
                        ' ColumnValue = ViewRow.Item(3)
                        tablefieldvalues = tablefieldvalues & "'" & feAccStatus & "',"
                    ElseIf (fieldname = "Eximbills Module") Then
                        tablefieldvalues = tablefieldvalues & "'" & feEximbillsModule & "',"
                    Else
                        tablefieldvalues = tablefieldvalues & "'',"
                    End If
                Else
                    If (fieldname = "Product") Then
                        tablefieldvalues = tablefieldvalues & "'" & feProduct.ToString().Replace("'", "''") & "',"
                        'tablefieldvalues = tablefieldvalues & "'" & cbProductList.SelectedValue.Replace("'", "''") & ","
                    ElseIf (fieldname = "Phase") Then
                        tablefieldvalues = tablefieldvalues & "'" & fePhase.ToString().Replace("'", "''") & "',"
                        'tablefieldvalues = tablefieldvalues & cbPhase.SelectedValue.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Work Item Reference") Then
                        tablefieldvalues = tablefieldvalues & WorkitemReference.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Document Type") Then
                        tablefieldvalues = tablefieldvalues & "'" & feDocType.ToString().Replace("'", "''") & "',"
                        'tablefieldvalues = tablefieldvalues & cbDocumentType.SelectedValue.Replace("'", "''") & ","
                    ElseIf (fieldname = "RIM Number") Then
                        ' ColumnValue = ViewRow.Item(2)
                        tablefieldvalues = tablefieldvalues & feRimNo.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Account Type") Then
                        '  ColumnValue = ViewRow.Item(1)
                        tablefieldvalues = tablefieldvalues & feAccType.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Account Number") Then
                        '  ColumnValue = ViewRow.Item(0)
                        tablefieldvalues = tablefieldvalues & feAccNo.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Account Status") Then
                        ' ColumnValue = ViewRow.Item(3)
                        tablefieldvalues = tablefieldvalues & feAccStatus.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Eximbills Module") Then
                        tablefieldvalues = tablefieldvalues & feEximbillsModule.ToString().Replace("'", "''") & ","
                    Else
                        tablefieldvalues = tablefieldvalues & "0,"
                    End If
                End If
            Next


        Catch ex As Exception
            MsgBox("Exception in GetCabinetFields " & ex.Message)
        End Try
    End Function
    Public Function GetCabinetFieldsFromStage(doctype As String)
        Try
            Dim fieldname As String = ""
            Dim fieldlevel As String = ""
            Dim fielddatatypeid As String = ""
            tablefieldnames = ""
            tablefieldvalues = ""

            Dim ViewRow = ResAccBasicInfoObj.Tables(0).Rows(0)
            feAccNo = ViewRow("AccountNo").ToString()
            feRimNo = ViewRow("RIMNumber").ToString()
            feAccType = ViewRow("AccountType").ToString()
            feAccStatus = ViewRow("Status").ToString()

            Me.Dispatcher.Invoke(Sub()
                                     If (txtwir.Text = "") Then
                                         WorkitemReference = ""
                                     Else
                                         WorkitemReference = txtwir.Text
                                     End If
                                 End Sub)

            feProduct = ""
            fePhase = ""
            feDocType = ""

            Me.Dispatcher.Invoke(Sub()
                                     If (cbProductList.SelectedIndex <> -1) Then
                                         feProduct = cbProductList.SelectedValue.ToString()
                                     Else
                                         feProduct = ""
                                     End If
                                     If (cbPhase.SelectedIndex <> -1) Then
                                         fePhase = cbPhase.SelectedValue.ToString()
                                     Else
                                         fePhase = ""
                                     End If
                                     'If (cbDocumentType.SelectedIndex <> -1) Then
                                     '    ' feDocType = cbDocumentType.SelectedValue.ToString()
                                     '    feDocType = gridSelectedCellDocumentTypeValue.ToString
                                     'Else
                                     '    feDocType = ""
                                     'End If
                                     If (doctype.ToString = "") Then
                                         feDocType = ""
                                     Else
                                         ' feDocType = cbDocumentType.SelectedValue.ToString()
                                         feDocType = doctype.ToString
                                     End If
                                 End Sub)


            For l = 0 To dstbl.Tables(0).Rows.Count - 1
                fieldname = dstbl.Tables(0).Rows(l).Item("FieldName")
                fieldlevel = dstbl.Tables(0).Rows(l).Item("FieldLevel")
                fielddatatypeid = dstbl.Tables(0).Rows(l).Item("DataTypeId")

                tablefieldnames = tablefieldnames & "[" & fieldname & "],"

                If (fielddatatypeid = 2 Or fielddatatypeid = 4 Or fielddatatypeid = 5) Then
                    If (fieldname = "Product") Then
                        tablefieldvalues = tablefieldvalues & "'" & feProduct.ToString().Replace("'", "''") & "',"
                        'cbProductList.IsEnabled = True
                        'tablefieldvalues = tablefieldvalues & "'" & feProduct.ToString().Replace("'", "''") & "',"
                        'cbProductList.IsEnabled = False
                    ElseIf (fieldname = "Phase") Then
                        tablefieldvalues = tablefieldvalues & "'" & fePhase.ToString().Replace("'", "''") & "',"
                        'tablefieldvalues = tablefieldvalues & "'" & cbPhase.SelectedValue.ToString().Replace("'", "''") & "',"
                    ElseIf (fieldname = "Work Item Reference") Then
                        tablefieldvalues = tablefieldvalues & "'" & WorkitemReference.ToString().Replace("'", "''") & "',"
                    ElseIf (fieldname = "Document Type") Then
                        tablefieldvalues = tablefieldvalues & "'" & feDocType.ToString().Replace("'", "''") & "',"
                        'tablefieldvalues = tablefieldvalues & "'" & cbDocumentType.SelectedValue.ToString().Replace("'", "''") & "',"
                    ElseIf (fieldname = "RIM Number") Then
                        ' ColumnValue = ViewRow.Item(2)
                        tablefieldvalues = tablefieldvalues & "'" & feRimNo & "',"
                    ElseIf (fieldname = "Account Type") Then
                        ' ColumnValue = ViewRow.Item(1)
                        tablefieldvalues = tablefieldvalues & "'" & feAccType & "',"
                    ElseIf (fieldname = "Account Number") Then
                        '  ColumnValue = ViewRow.Item(0)
                        tablefieldvalues = tablefieldvalues & "'" & feAccNo & "',"
                    ElseIf (fieldname = "Account Status") Then
                        ' ColumnValue = ViewRow.Item(3)
                        tablefieldvalues = tablefieldvalues & "'" & feAccStatus & "',"
                    ElseIf (fieldname = "Eximbills Module") Then
                        tablefieldvalues = tablefieldvalues & "'" & feEximbillsModule & "',"
                    Else
                        tablefieldvalues = tablefieldvalues & "'',"
                    End If
                Else
                    If (fieldname = "Product") Then
                        tablefieldvalues = tablefieldvalues & "'" & feProduct.ToString().Replace("'", "''") & "',"
                        'tablefieldvalues = tablefieldvalues & "'" & cbProductList.SelectedValue.Replace("'", "''") & ","
                    ElseIf (fieldname = "Phase") Then
                        tablefieldvalues = tablefieldvalues & "'" & fePhase.ToString().Replace("'", "''") & "',"
                        'tablefieldvalues = tablefieldvalues & cbPhase.SelectedValue.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Work Item Reference") Then
                        tablefieldvalues = tablefieldvalues & WorkitemReference.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Document Type") Then
                        tablefieldvalues = tablefieldvalues & "'" & feDocType.ToString().Replace("'", "''") & "',"
                        'tablefieldvalues = tablefieldvalues & cbDocumentType.SelectedValue.Replace("'", "''") & ","
                    ElseIf (fieldname = "RIM Number") Then
                        ' ColumnValue = ViewRow.Item(2)
                        tablefieldvalues = tablefieldvalues & feRimNo.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Account Type") Then
                        '  ColumnValue = ViewRow.Item(1)
                        tablefieldvalues = tablefieldvalues & feAccType.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Account Number") Then
                        '  ColumnValue = ViewRow.Item(0)
                        tablefieldvalues = tablefieldvalues & feAccNo.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Account Status") Then
                        ' ColumnValue = ViewRow.Item(3)
                        tablefieldvalues = tablefieldvalues & feAccStatus.ToString().Replace("'", "''") & ","
                    ElseIf (fieldname = "Eximbills Module") Then
                        tablefieldvalues = tablefieldvalues & feEximbillsModule.ToString().Replace("'", "''") & ","
                    Else
                        tablefieldvalues = tablefieldvalues & "0,"
                    End If
                End If
            Next


        Catch ex As Exception
            MsgBox("Exception in GetCabinetFieldsFromStage " & ex.Message)
        End Try
    End Function

    Public Function copyfiletoimagingfolder(ersdirpath As String, scannedfilepath As String) As String

        Dim sourcefilenamepath = scannedfilepath
        Dim scannedfilename = Path.GetFileName(scannedfilepath)
        Dim doctypefilename = gridSelectedCellDocumentTypeValue.ToString.Replace("'", "") & Path.GetExtension(scannedfilepath)

        'Dim destinationpath = Path.Combine(ersdirpath, "imaging", feAccNo, Format(Today, "ddMMyyyy"))
        If sessionid = "" Then
            sessionid = Format(Now, "ddMMyyyyhhmmss")
        End If
        'Dim destinationpath = Path.Combine(ersdirpath, "imaging", feAccNo, sessionid)
        Dim destinationpath = Path.Combine("c:\imaging", invitaAPIobj.TemplateId, feAccNo, sessionid)
        Dim destinationfilepath As String

        If (Not Directory.Exists(destinationpath)) Then
            Directory.CreateDirectory(destinationpath)
        End If
        destinationfilepath = Path.Combine(destinationpath, scannedfilename)
        'destinationfilepath = Path.Combine(destinationpath, doctypefilename)

        If (Not File.Exists(destinationfilepath)) Then
            File.Copy(sourcefilenamepath, destinationfilepath, False)
        Else
            destinationfilepath = Path.Combine(destinationpath, Path.GetFileNameWithoutExtension(scannedfilepath) & Format(Now, "ddmmyyyyhhmmss") & Path.GetExtension(scannedfilepath))
            'destinationfilepath = Path.Combine(destinationpath, doctypefilename & Format(Now, "ddmmyyyyhhmmss") & Path.GetExtension(scannedfilepath))
            File.Copy(sourcefilenamepath, destinationfilepath)
        End If
        Return destinationfilepath
    End Function

    Public Function archivefiletotable(cabinetid As Integer, cabinetname As String, templateid As Integer, scannedfilepath As String, archivefolderpath As String, ersdirpath As String, ersid As Integer, tablefieldnames As String, tablefieldvalues As String, filename As String, lastworkitemno As Integer, strtitle As String, strsubject As String, strauthor As String, strkeyword As String, OutFilesize As String, OutfileNoofPages As Integer, fileversion As String) As Integer
        'shiva
        Dim fi As New FileInfo(scannedfilepath)
        Dim scannedfilesize = fi.Length
        Dim qryitemtbl As String
        Dim archivefolder As String
        fileversion = fileversion.Replace(".0", "")
        'Dim version = fileversion
        If (fileversion <> "1.0") Then
            fileversion = fileversion.Replace(".0", "")
        End If
        If (fileversion = "1") Then
            fileversion = "1.0"
        End If

        archivefolder = archivefolderpath '.Substring(0, archivefolderpath.LastIndexOf("\"))

        If (filename.Contains("'")) Then
            filename = filename.Replace("'", "''")
        End If

        'qryitemtbl = "insert into ezca_" & cabinetid & "_" & templateid & "_items(ersid,templateid," & tablefieldnames & "ifilepath,ifilename,ifiletype,createdon,createdby,updatedon,updatedby,isdeleted) values (" & ersid & "," & templateid & "," & tablefieldvalues & "'" & archivefolderpath & "','" & filename & "','" & Path.GetExtension(filename).Replace(".", "") & "','" & Format(Now(), "dd/MMM/yyyy hh:mm:ss tt") & "',1,0,0,0" & ")"
        Dim ezfrom = "CAPTURE(" & Environment.MachineName & ")"



        qryitemtbl = "insert into ezca_" & cabinetid & "_" & templateid & "_items(ersid,templateid," & tablefieldnames & "ifilepath,ifilename,ifiletype,[eZFrom],[version],[dtitle],[dauthor],[dsubject],[dkeywords],[checkout],[checkoutpath],[checkoutby],[dstatus],[dsize],[nopages], createdon, createdby, updatedon, updatedby, isdeleted) values (" & ersid & "," & templateid & "," & tablefieldvalues & "'" & archivefolderpath & "\','" & filename & "','" & Path.GetExtension(filename).Replace(".", "") & "','" & ezfrom & "','" & fileversion & "','" & strtitle & "','" & strauthor & "','" & strsubject & "','" & strkeyword & "','','',0,'Active','" & OutFilesize & "'," & OutfileNoofPages & ",'" & Format(Now(), "dd/MMM/yyyy hh:mm:ss tt") & "',1,'',0,0" & ")"



        'MsgBox(qryitemtbl)
        Dim res1 = invitaAPIobj.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qryitemtbl)
        Dim qryhistorytbl = "insert into ezca_" & cabinetid & "_" & templateid & "_history(itemid,ersid,templateid," & tablefieldnames & "ifilepath,ifilename,ifiletype,[eZFrom],[version],[dtitle],[dauthor],[dsubject],[dkeywords],[checkout],[checkoutpath],[checkoutby],[dstatus],[dsize],[nopages], createdon, createdby, updatedon, updatedby, isdeleted) values (" & res1 & "," & ersid & "," & templateid & "," & tablefieldvalues & "'" & archivefolderpath & "\','" & filename & "','" & Path.GetExtension(filename).Replace(".", "") & "','" & ezfrom & "','" & fileversion & "','" & strtitle & "','" & strauthor & "','" & strsubject & "','" & strkeyword & "','','',0,'Active','" & OutFilesize & "'," & OutfileNoofPages & ",'" & Format(Now(), "dd/MMM/yyyy hh:mm:ss tt") & "',1,'',0,0" & ")"
        Dim res2 = invitaAPIobj.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qryhistorytbl)





        '  If res <> 0 Then
        ' MsgBox("data inserted successfully")
        ' End If
        Return res1
    End Function
    Public Sub verifyWorkItemReference(ByVal Workitemref As String, ByVal searchflag As Integer)
        Try
            Me.Dispatcher.Invoke(Sub()
                                     'btnWorkItemVerifySpin.Visibility = Visibility.Visible

                                     If searchflag = 1 Then 'continue on the same batch
                                         'btnVERIFY.IsEnabled = True

                                         ' gridAccountDetails.ItemsSource = Nothing
                                         gridSubmittedDocuments.ItemsSource = Nothing

                                         Dim ResDs = externalAPIobj.GetDatasetByQuery("SELECT top 1 [Product] as PRODUCT,[Phase] as PHASE,[Document Type] as SUBMITTEDFiles, [Account Number] as AccountNo,[Account Type] as AccountType, [RIM Number] as RIMNumber, [Account Status] as Status, [Work Item Reference] as WORKITEMNO, itemid ,createdon as CREATEDDATE  FROM [ezca_" & cabinetid.ToString & "_" & invitaAPIobj.TemplateId & "_items] where [Work Item reference]='" + Workitemref + "'")

                                         'select [Document Type] as SUBMITTEDFiles,nopages  as NoPage, eZFrom  as ScannedAt,dbo.udf_LoginName(CreatedBy ) as Scannedby,CreatedOn  as Scannedon from ezca_3_15_items order by itemid desc

                                         If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then

                                             WorkitemReference = Workitemref
                                             Me.Dispatcher.Invoke(Sub()
                                                                      fillProduct()
                                                                      cbProductList.IsEnabled = True
                                                                      'gridAccountDetails.ItemsSource = ResDs.Tables(0).DefaultView()
                                                                      '  gridSubmittedDocuments.ItemsSource = ResDs.Tables(0).DefaultView()
                                                                      lblStatusbar.Foreground = New SolidColorBrush(Colors.Green)
                                                                      lblStatusbar.Text = "WorkItem Saved Successfully"
                                                                      If Not IsNothing(cbProductList) Then
                                                                          cbProductList.SelectedValue = ResDs.Tables(0).Rows(0).Item("PRODUCT")
                                                                          fillPhase()
                                                                          cbPhase.SelectedValue = ResDs.Tables(0).Rows(0).Item("PHASE")
                                                                          fillsubmitteddocumentsgrid(Workitemref)
                                                                          gridSubmittedDocuments.Items.Refresh()
                                                                          'cbDocumentType.SelectedValue = ResDs.Tables(0).Rows(0).Item("SUBMITTEDFiles")
                                                                          cbProductList.IsEnabled = False
                                                                          cbPhase.IsEnabled = False
                                                                          btnSubmit.IsEnabled = False
                                                                          btnRescan.IsEnabled = False
                                                                          btnClearRightPane.IsEnabled = False
                                                                          btnFinalSubmit.IsEnabled = False
                                                                          '  btnINITIATESCANNING.IsEnabled = True
                                                                          ' txtAccountNo.Text = ""
                                                                          'txtWorkItemNo.IsEnabled = False
                                                                          '  cbDocumentType.IsEnabled = False
                                                                      End If
                                                                  End Sub)

                                         Else
                                             Me.Dispatcher.Invoke(Sub()
                                                                      gridAccountDetails.ItemsSource = Nothing
                                                                      gridSubmittedDocuments.ItemsSource = Nothing
                                                                      btnINITIATESCANNING.IsEnabled = False
                                                                      txtAccountNo.Text = ""
                                                                      txtWorkItemNo.Text = ""
                                                                      cbProductList.IsEnabled = True
                                                                      cbPhase.ItemsSource = Nothing
                                                                      'cbDocumentType.ItemsSource = Nothing
                                                                      '  cbProductList.SelectedValue = "Select Product"
                                                                      cbPhase.IsEnabled = True
                                                                      btnSubmit.IsEnabled = False
                                                                      btnRescan.IsEnabled = False
                                                                      btnClearRightPane.IsEnabled = False
                                                                      btnFinalSubmit.IsEnabled = False
                                                                  End Sub)

                                         End If

                                         Dim qry = "select *,
Case 
	when SCAN ='View' then 'Rescan'  
	End As RESCAN,'' FILEPATH,'' STAGEITEMID
from
(select [DOCUMENT TYPE], [MANDATORY],[itemid],
case when itemid Is NULL then 'Scan' 
Else 'View' 
End As SCAN 
from
(SELECT R1.[DOCUMENT TYPE], R1.[MANDATORY], itemid 
from (Select  distinct [Document Type] [DOCUMENT TYPE],
Case WHEN Mandatory = 'true' THEN 'Mandatory' 
Else '-' End As  [MANDATORY] 
FROM [ezfb_Product CheckList Master]  where [Product]='" & cbProductList.SelectedValue.ToString & "' and [Phase]='" & cbPhase.SelectedValue.ToString & "') as r1 Left Join ezca_" & cabinetid.ToString & "_" & invitaAPIobj.TemplateId & "_items as itemtbl on r1.[Document Type]=itemtbl.[Document Type] And [Work Item Reference]='" & Workitemref & "') as r2) AS R3 order by MANDATORY desc"

                                         '   Dim ResDs1 = externalAPIobj.GetDatasetByQuery("select [Work Item Reference] as WORKITEMNO,[Product] as PRODUCT,[Phase] as PHASE,[Document Type] as SUBMITTEDFiles,nopages  as NoPage, eZFrom  as ScannedAt,dbo.udf_LoginName(CreatedBy ) as Scannedby,CreatedOn  as Scannedon,itemid from [ezca_3_" & invitaAPIobj.TemplateId & "_items] where [Work Item reference]='" + Workitemref + "' order by itemid desc")

                                         Dim ResDs1 = invitaAPIobj.GetDatasetByQuery(qry)

                                         If ResDs1 IsNot Nothing AndAlso ResDs1.Tables.Count > 0 AndAlso ResDs1.Tables(0).Rows.Count > 0 Then
                                             'gridSubmittedDocuments.ItemsSource = ResDs1.Tables(0).DefaultView()
                                             ' DocTypeList.Clear()
                                             fillsubmitteddocumentsgrid(Workitemref)
                                             gridSubmittedDocuments.Items.Refresh()
                                         Else
                                             Me.Dispatcher.Invoke(Sub()
                                                                      gridAccountDetails.ItemsSource = Nothing
                                                                      gridSubmittedDocuments.ItemsSource = Nothing
                                                                      btnINITIATESCANNING.IsEnabled = False
                                                                      txtAccountNo.Text = ""
                                                                      txtWorkItemNo.Text = ""
                                                                      cbProductList.IsEnabled = True
                                                                      cbPhase.IsEnabled = True
                                                                      cbDocumentType.IsEnabled = True
                                                                      cbProductList.ItemsSource = Nothing
                                                                      cbPhase.ItemsSource = Nothing
                                                                      cbDocumentType.ItemsSource = Nothing
                                                                      btnSubmit.IsEnabled = False
                                                                      btnRescan.IsEnabled = False
                                                                      btnClearRightPane.IsEnabled = False
                                                                      btnFinalSubmit.IsEnabled = False
                                                                      '   cbProductList.SelectedValue = "Select Product"
                                                                  End Sub)

                                         End If
                                     ElseIf searchflag = 3 Then
                                         Me.Dispatcher.Invoke(Sub()
                                                                  btnVERIFY.IsEnabled = True

                                                                  gridAccountDetails.ItemsSource = Nothing
                                                                  gridSubmittedDocuments.ItemsSource = Nothing
                                                              End Sub)

                                         ResAccBasicInfoObj = externalAPIobj.GetDatasetByQuery("SELECT top 1 [Product] as PRODUCT,[Phase] as PHASE,[Document Type] as SUBMITTEDFiles, [Account Number] as AccountNo,[Account Type] as AccountType, [RIM Number] as RIMNumber, [Account Status] as Status, [Work Item Reference] as WORKITEMNO, itemid ,createdon as CREATEDDATE  FROM [ezca_" & cabinetid.ToString & "_" & invitaAPIobj.TemplateId & "_items] where [Work Item reference]='" + Workitemref + "'")

                                         'select [Document Type] as SUBMITTEDFiles,nopages  as NoPage, eZFrom  as ScannedAt,dbo.udf_LoginName(CreatedBy ) as Scannedby,CreatedOn  as Scannedon from ezca_3_15_items order by itemid desc

                                         If ResAccBasicInfoObj IsNot Nothing AndAlso ResAccBasicInfoObj.Tables.Count > 0 AndAlso ResAccBasicInfoObj.Tables(0).Rows.Count > 0 Then
                                             Me.Dispatcher.Invoke(Sub()
                                                                      cbProductList.IsEnabled = True
                                                                      fillProduct()
                                                                      gridAccountDetails.ItemsSource = ResAccBasicInfoObj.Tables(0).DefaultView()
                                                                      '  gridSubmittedDocuments.ItemsSource = ResDs.Tables(0).DefaultView()
                                                                      lblStatusbar.Foreground = New SolidColorBrush(Colors.Green)
                                                                      lblStatusbar.Text = "WorkItem Documents Exists"
                                                                      If Not IsNothing(cbProductList) Then
                                                                          cbProductList.SelectedValue = ResAccBasicInfoObj.Tables(0).Rows(0).Item("PRODUCT")
                                                                          fillPhase()
                                                                          cbPhase.SelectedValue = ResAccBasicInfoObj.Tables(0).Rows(0).Item("PHASE")
                                                                          'cbDocumentType.SelectedValue = ResAccBasicInfoObj.Tables(0).Rows(0).Item("SUBMITTEDFiles")
                                                                          'fillsubmitteddocumentsgrid(Workitemref)
                                                                          gridSubmittedDocuments.Items.Refresh()
                                                                          cbProductList.IsEnabled = False
                                                                          cbPhase.IsEnabled = False
                                                                          '    btnINITIATESCANNING.IsEnabled = True
                                                                          ' txtAccountNo.Text = ""
                                                                          'txtWorkItemNo.IsEnabled = False
                                                                          '  cbDocumentType.IsEnabled = False
                                                                          btnSubmit.IsEnabled = False
                                                                          btnRescan.IsEnabled = False
                                                                          btnClearRightPane.IsEnabled = False
                                                                          btnFinalSubmit.IsEnabled = False
                                                                      End If
                                                                  End Sub)

                                         Else
                                             Me.Dispatcher.Invoke(Sub()

                                                                      gridAccountDetails.ItemsSource = Nothing
                                                                      gridSubmittedDocuments.ItemsSource = Nothing
                                                                      btnINITIATESCANNING.IsEnabled = False
                                                                      txtAccountNo.Text = ""
                                                                      txtWorkItemNo.Text = ""
                                                                      cbProductList.IsEnabled = True
                                                                      cbProductList.ItemsSource = Nothing
                                                                      cbPhase.ItemsSource = Nothing
                                                                      cbDocumentType.ItemsSource = Nothing
                                                                      ' cbProductList.SelectedValue = "Select Product"
                                                                      cbPhase.IsEnabled = True
                                                                      cbDocumentType.IsEnabled = True
                                                                      btnSubmit.IsEnabled = False
                                                                      btnRescan.IsEnabled = False
                                                                      btnClearRightPane.IsEnabled = False
                                                                      btnFinalSubmit.IsEnabled = False
                                                                      Dim c As Color = CType(ColorConverter.ConvertFromString("#C24641"), Color)
                                                                      lblStatusbar.Foreground = New SolidColorBrush(c)
                                                                      lblStatusbar.Text = "Work Item Not Found"
                                                                  End Sub)

                                         End If
                                         Dim qry = "select *,
Case 
	when SCAN ='View' then 'Rescan'  
	End As RESCAN ,'' FILEPATH,'' STAGEITEMID
from
(select [DOCUMENT TYPE], [MANDATORY],[itemid],
case when itemid Is NULL then 'Scan' 
Else 'View' 
End As SCAN 
from
(SELECT R1.[DOCUMENT TYPE], R1.[MANDATORY], itemid 
from (Select  distinct [Document Type] [DOCUMENT TYPE],
Case WHEN Mandatory = 'true' THEN 'Mandatory' 
Else '-' End As  [MANDATORY] 
FROM [ezfb_Product CheckList Master]  where [Product]='" & cbProductList.SelectedValue.ToString & "' and [Phase]='" & cbPhase.SelectedValue.ToString & "') as r1 Left Join ezca_" & cabinetid.ToString & "_" & invitaAPIobj.TemplateId & "_items as itemtbl on r1.[Document Type]=itemtbl.[Document Type] And [Work Item Reference]='" & Workitemref & "') as r2) AS R3 order by MANDATORY desc"
                                         Dim ResDs1 = invitaAPIobj.GetDatasetByQuery(qry)

                                         'Dim ResDs1 = externalAPIobj.GetDatasetByQuery("select [Work Item Reference] as WORKITEMNO,[Product] as PRODUCT,[Phase] as PHASE,[Document Type] as SUBMITTEDFiles,nopages  as NoPage, eZFrom  as ScannedAt,dbo.udf_LoginName(CreatedBy ) as Scannedby,CreatedOn  as Scannedon,itemid  from [ezca_3_" & invitaAPIobj.TemplateId & "_items] where [Work Item reference]='" + Workitemref + "' order by itemid desc")
                                         If ResDs1 IsNot Nothing AndAlso ResDs1.Tables.Count > 0 AndAlso ResDs1.Tables(0).Rows.Count > 0 Then
                                             '  gridSubmittedDocuments.ItemsSource = ResDs1.Tables(0).DefaultView()
                                             DocTypeList.Clear()
                                             fillsubmitteddocumentsgrid(Workitemref)
                                             gridSubmittedDocuments.Items.Refresh()
                                         Else
                                             Me.Dispatcher.Invoke(Sub()
                                                                      gridAccountDetails.ItemsSource = Nothing
                                                                      gridSubmittedDocuments.ItemsSource = Nothing
                                                                      btnINITIATESCANNING.IsEnabled = False
                                                                      txtAccountNo.Text = ""
                                                                      txtWorkItemNo.Text = ""
                                                                      cbProductList.ItemsSource = Nothing
                                                                      cbProductList.IsEnabled = True
                                                                      ' cbProductList.SelectedValue = "Select Product"
                                                                      cbPhase.IsEnabled = True
                                                                      btnSubmit.IsEnabled = False
                                                                      btnRescan.IsEnabled = False
                                                                      btnClearRightPane.IsEnabled = False
                                                                      btnFinalSubmit.IsEnabled = False
                                                                      Dim c As Color = CType(ColorConverter.ConvertFromString("#C24641"), Color)
                                                                      lblStatusbar.Foreground = New SolidColorBrush(c)
                                                                      lblStatusbar.Text = "Work Item Not Found"
                                                                  End Sub)

                                         End If
                                     ElseIf searchflag = 2 Then 'continue with new work item
                                         'gridAccountDetails.ItemsSource = Nothing
                                         'gridSubmittedDocuments.ItemsSource = Nothing
                                         'btnINITIATESCANNING.IsEnabled = False
                                         'txtAccountNo.Text = ""
                                         'txtWorkItemNo.Text = ""
                                         'cbProductList.ItemsSource = Nothing
                                         'cbProductList.IsEnabled = True
                                         'cbPhase.IsEnabled = True
                                         'cbDocumentType.IsEnabled = True
                                         'cbPhase.ItemsSource = Nothing
                                         'cbDocumentType.ItemsSource = Nothing
                                         'txtwir.Text = ""
                                         clearAllvalues(1)
                                     End If

                                     'Thread.Sleep(5000)
                                     'btnWorkItemVerifySpin.Visibility = Visibility.Collapsed
                                 End Sub)
        Catch ex As Exception
            MsgBox("Exception in verifyWorkItemReference " & ex.Message)
        End Try
    End Sub

    Public Sub verifyAccountNo()
        Try
            Me.Dispatcher.Invoke(Sub()

                                     'btnAccNoVerifySpin.Visibility = Visibility.Visible
                                     lblStatusbar.Text = "Scanning...."


                                     If (txtAccountNo.Text <> "") Then
                                         gridAccountDetails.ItemsSource = Nothing
                                         gridSubmittedDocuments.ItemsSource = Nothing
                                         WorkitemReference = ""
                                         Dim AccBasicInfoObj = New AccountInfo()
                                         AccBasicInfoObj.acct_no = txtAccountNo.Text
                                         AccBasicInfoObj.url = invitaAPIobj.apiUrlExternal
                                         ' AccBasicInfoObj.url = invitaAPIobj.apiUrlExternal + "/v1/TradeFinance/tf_acct_basic_info"

                                         ResAccBasicInfoObj = invitaAPIobj.GetInfoFromAccountNo(AccBasicInfoObj)
                                         '/v1/Common/GetInfoFromAccountNo
                                         If (Not ResAccBasicInfoObj Is Nothing) Then
                                             If ResAccBasicInfoObj IsNot Nothing AndAlso ResAccBasicInfoObj.Tables.Count > 0 AndAlso ResAccBasicInfoObj.Tables(0).Rows.Count > 0 Then

                                                 Dim accstatus As String = ResAccBasicInfoObj.Tables(0).Rows(0).Item("Status")
                                                 'MsgBox(accstatus)

                                                 If (accstatus.ToUpper = "ACTIVE") Then

                                                     gridAccountDetails.ItemsSource = ResAccBasicInfoObj.Tables(0).DefaultView
                                                     lblStatusbar.Foreground = New SolidColorBrush(Colors.Green)
                                                     lblStatusbar.Text = "Account is Active"

                                                     Dim ResTicketQueue = invitaAPIobj.GetDatasetByQuery("SELECT [Work Item Reference] WORKITEMREFERNCE,[Account Number] ACCOUNTNUMBER,[Product] PRODUCT,[Phase] PHASE,CreatedOn [CREATEDON],UpdatedOn,Upper(dbo.udf_LoginName(CreatedBy)) as [CREATEDBY],UpdatedBy,[TicketQueueId],'Select' AS [CHOOSE] FROM BBK_TicketQueue where [Account Number]='" + txtAccountNo.Text + "' and ProcessId=0 order by [Work Item Reference] desc")
                                                     If ResTicketQueue IsNot Nothing AndAlso ResTicketQueue.Tables.Count > 0 AndAlso ResTicketQueue.Tables(0).Rows.Count > 0 Then
                                                         'gridSubmittedDocuments.ItemsSource = ResDs1.Tables(0).DefaultView()
                                                         submittedWIGridObj = New SubmittedWorkItemsGrid(ResTicketQueue)
                                                         'Dim totrec = Convert.ToInt32(submittedWIGridObj.TotalNoOfRec)
                                                         Dim totrec = ResTicketQueue.Tables(0).Rows.Count
                                                         'MsgBox(totrec)
                                                         'If (totrec > 1) Then
                                                         submittedWIGridObj.ShowDialog()
                                                         writeWIR(submittedWIGridObj.GRIDWIR)
                                                         If (SubmittedworkItemFlag = 1) Then
                                                             fillProduct()

                                                             cbProductList.SelectedValue = submittedWIGridObj.GRIDPRODUCT

                                                             fillPhase()
                                                             cbPhase.SelectedValue = submittedWIGridObj.GRIDPHASE

                                                             ' fillsubmitteddocumentsgrid(submittedWIGridObj.GRIDWIR)

                                                             txtSubmittedWorkItems.Text = "Submitted Documents (" & submittedWIGridObj.TotalNoOfRec.ToString & ")"
                                                             submittedDocNos = "Submitted Documents (" & submittedWIGridObj.TotalNoOfRec.ToString & ")"
                                                             txtAccountNo.IsEnabled = False
                                                             btnVERIFY.IsEnabled = False

                                                             cbProductList.IsEnabled = False
                                                             cbPhase.IsEnabled = False
                                                         Else
                                                             txtAccountNo.IsEnabled = False
                                                             btnVERIFY.IsEnabled = False

                                                             fillProduct()
                                                             cbProductList.IsEnabled = True
                                                             cbPhase.IsEnabled = True
                                                             txtWorkItemNo.Text = ""
                                                         End If

                                                     Else
                                                         txtAccountNo.IsEnabled = False
                                                         btnVERIFY.IsEnabled = False
                                                         fillProduct()
                                                         If Not IsNothing(cbProductList) Then
                                                             cbProductList.IsEnabled = True
                                                             cbPhase.IsEnabled = True
                                                             cbDocumentType.IsEnabled = True
                                                             txtWorkItemNo.Text = ""
                                                         End If
                                                     End If


                                                 Else
                                                     txtAccountNo.IsEnabled = True
                                                     btnVERIFY.IsEnabled = True
                                                     btnINITIATESCANNING.IsEnabled = False
                                                     'cbProductList.IsEnabled = False
                                                     Dim c As Color = CType(ColorConverter.ConvertFromString("#C24641"), Color)
                                                     lblStatusbar.Foreground = New SolidColorBrush(c)
                                                     'lblStatusbar.Foreground = New SolidColorBrush(Colors.DarkRed)
                                                     lblStatusbar.Text = "Account is not active" & vbCrLf & "Please review with your Releationship Manager or The branch at which you opened the account"
                                                     cbPhase.ItemsSource = Nothing
                                                     cbDocumentType.ItemsSource = Nothing
                                                 End If
                                             End If
                                         Else
                                             txtAccountNo.IsEnabled = True
                                             btnVERIFY.IsEnabled = True
                                             btnINITIATESCANNING.IsEnabled = False
                                             ' cbProductList.IsEnabled = False
                                             gridAccountDetails.ItemsSource = Nothing

                                             Dim c As Color = CType(ColorConverter.ConvertFromString("#C24641"), Color)
                                             lblStatusbar.Foreground = New SolidColorBrush(c)
                                             'lblStatusbar.Foreground = New System.Windows.Media.SolidColorBrush((Color) ColorConverter.ConvertFromString("#C24641"))
                                             'lblStatusbar.Foreground = New System.Windows.Media.SolidColorBrush((Color) RGB(194, 70, 65))
                                             lblStatusbar.Text = "Invalid Account No"
                                             ' MessageBox.Show("Invalid Account No", "TRADE FINANCE INFORMATION")
                                             cbProductList.ItemsSource = Nothing
                                             cbPhase.ItemsSource = Nothing
                                             cbDocumentType.ItemsSource = Nothing

                                         End If
                                     Else
                                         txtAccountNo.IsEnabled = True
                                         btnVERIFY.IsEnabled = True
                                         Dim c As Color = CType(ColorConverter.ConvertFromString("#C24641"), Color)
                                         lblStatusbar.Foreground = New SolidColorBrush(c)
                                         lblStatusbar.Text = "Please Enter Account No"
                                         cbProductList.ItemsSource = Nothing
                                         cbPhase.ItemsSource = Nothing
                                         cbDocumentType.ItemsSource = Nothing
                                     End If

                                     'Thread.Sleep(5000)
                                     ' btnAccNoVerifySpin.Visibility = Visibility.Collapsed
                                 End Sub)
        Catch ex As Exception
            MsgBox("Exception in verifyAccountNo " & ex.Message)
        End Try
    End Sub


    Private Sub writeWIR(gridwir As String)
        Try
            If (gridwir <> "") Then
                txtwir.Text = gridwir.ToString
                WorkitemReference = gridwir.ToString
            Else
                txtwir.Text = ""
                WorkitemReference = ""
            End If
        Catch ex As Exception
            MsgBox("Exception in writeWIR" & ex.Message)
        End Try
    End Sub
    Private Sub btnVERIFY_Click(sender As Object, e As RoutedEventArgs) Handles btnVERIFY.Click
        '        border1.Visibility = Visibility.Visible
        Try
            'ECMViewer._documentViewer.SetDocument(Nothing)
            btnAccNoVerifySpin.Visibility = Visibility.Visible

            progfor = "Verify"
            reminderAnim = TryCast(Me.Resources("PageProgress1"), Storyboard)
            reminderAnim.Begin()
            worker.RunWorkerAsync()

            btnAccNoVerifySpin.Visibility = Visibility.Collapsed
            'clearAllvalues(2)
            'verifyAccountNo()
        Catch ex As Exception
            MsgBox("Exception in btnVERIFY_Click" & ex.Message)
        Finally
            '      border1.Visibility = Visibility.Hidden
        End Try



    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Me.Width = System.Windows.SystemParameters.WorkArea.Width
        Me.Height = System.Windows.SystemParameters.WorkArea.Height
        Me.Left = 0
        Me.Top = 0
        ' border1.Visibility = Visibility.Visible
        btnSubmit.IsEnabled = False
        btnRescan.IsEnabled = False
        btnClearRightPane.IsEnabled = False
        btnFinalSubmit.IsEnabled = False

        ECMViewerToolbar.IsEnabled = False


        'Dim ResDs = externalAPIobj.GetDatasetByQuery("SELECT distinct Product FROM [ezfb_Product Master]")

        'If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then
        '    '  Dim defrow As New DataRow

        '    ' ResDs.Tables(0).Rows.Add("Select Product")
        '    ' Dim product = ResDs.Tables(0).Rows(0).Item("Product")
        '    ' MsgBox(product)
        '    cbProductList.DisplayMemberPath = ResDs.Tables(0).Columns("Product").ToString
        '    cbProductList.SelectedValuePath = ResDs.Tables(0).Columns("Product").ToString
        '    cbProductList.ItemsSource = ResDs.Tables(0).DefaultView
        '    ' cbProductList.SelectedValue = "Select Product"

        'End If
        'cbProductList.IsEnabled = False
        ' border1.Visibility = Visibility.Hidden
    End Sub
    Public Sub fillPhase()
        Try
            If (cbProductList.SelectedValue IsNot Nothing) Then
                Dim ResDs = externalAPIobj.GetDatasetByQuery("SELECT distinct Phase FROM [ezfb_Product Master] where Product='" & cbProductList.SelectedValue.ToString & "'")

                If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then
                    cbPhase.ItemsSource = ResDs.Tables(0).DefaultView
                    cbPhase.DisplayMemberPath = ResDs.Tables(0).Columns("Phase").ToString
                    cbPhase.SelectedValuePath = ResDs.Tables(0).Columns("Phase").ToString
                    cbDocumentType.ItemsSource = Nothing
                Else
                    If cbProductList.SelectedValue.ToString = "Select Product" Then
                        '  cbPhase.SelectedIndex = 0
                        '   cbPhase.SelectedValue = "Select Phase"
                    End If
                    cbPhase.ItemsSource = Nothing
                End If
            End If
        Catch ex As Exception
            MsgBox("Exception in fillPhase" & ex.Message)
        End Try

    End Sub
    Public Sub fillDocTypeList(wir As String)
        Try
            DocTypeList.Clear()

            If (cbPhase.SelectedValue IsNot Nothing And cbPhase.SelectedValue <> "") Then

                Dim qry = "select *,
Case 
	when SCAN ='View' then 'Rescan'  
	End As RESCAN,'' FILEPATH,'' STAGEITEMID 
from
(select [DOCUMENT TYPE], [MANDATORY],[itemid],
case when itemid Is NULL then 'Scan' 
Else 'View' 
End As SCAN 
from
(SELECT R1.[DOCUMENT TYPE], R1.[MANDATORY], itemid 
from (Select  distinct [Document Type] [DOCUMENT TYPE],
Case WHEN Mandatory = 'true' THEN 'Mandatory' 
Else '-' End As  [MANDATORY] 
FROM [ezfb_Product CheckList Master]  where [Product]='" & cbProductList.SelectedValue.ToString & "' and [Phase]='" & cbPhase.SelectedValue.ToString & "') as r1 Left Join ezca_" & cabinetid.ToString & "_" & invitaAPIobj.TemplateId & "_items as itemtbl on r1.[Document Type]=itemtbl.[Document Type] And [Work Item Reference]='" & wir & "') as r2) AS R3 order by MANDATORY desc"
                Dim ResDs = invitaAPIobj.GetDatasetByQuery(qry)

                If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then


                    gridSubmittedDocuments.ItemsSource = Nothing
                    gridSubmittedDocuments.Items.Clear()
                    '  gridSubmittedDocuments.ItemsSource = ResDs.Tables(0).DefaultView

                    'GridDispDatatable = TryCast(ResDs.Tables(0), DataTable)

                    'DocTypeList.Clear()
                    'If (DocTypeList.Count > 1) Then
                    '    For i = 0 To ResDs.Tables(0).Rows.Count - 1
                    '        If (ResDs.Tables(0).Rows(i).Item(4).ToString <> "") Then
                    '            tempfilesflag = True
                    '            Exit For
                    '        End If
                    '    Next
                    '    If (tempflag = flase) Then

                    '    End If
                    'Else
                    For i = 0 To ResDs.Tables(0).Rows.Count - 1
                        DocTypeList.Add(New DocumentTypeList(ResDs.Tables(0).Rows(i).Item(0).ToString, ResDs.Tables(0).Rows(i).Item(1).ToString, ResDs.Tables(0).Rows(i).Item(2).ToString, ResDs.Tables(0).Rows(i).Item(3).ToString, ResDs.Tables(0).Rows(i).Item(4).ToString, ResDs.Tables(0).Rows(i).Item(5).ToString, ResDs.Tables(0).Rows(i).Item(6).ToString))
                    Next

                    Dim AllMandatoryFilesRecievedFlag = True
                    For i = 0 To DocTypeList.Count - 1
                        If (DocTypeList(i).MANDATORY.ToString.ToUpper = "MANDATORY" And DocTypeList(i).itemid.ToString = "") Then
                            AllMandatoryFilesRecievedFlag = False
                            Exit For
                        End If
                    Next

                    If (AllMandatoryFilesRecievedFlag = False) Then
                    Else
                        For i = 0 To DocTypeList.Count - 1
                            If (DocTypeList(i).MANDATORY.ToString.ToUpper = "MANDATORY") Then
                                DocTypeList(i).SCAN = "View"
                                DocTypeList(i).RESCAN = "SCANNED"
                            End If
                        Next

                    End If
                    gridSubmittedDocuments.ItemsSource = DocTypeList
                    ' End If

                    gridSubmittedDocuments.Items.Refresh()
                End If
            End If
        Catch ex As Exception
            MsgBox("Exception in fillDocTypeList" & ex.Message)
        End Try
    End Sub
    Public Sub filleximbillsmodule()
        Try
            Dim ResDs = externalAPIobj.GetDatasetByQuery("SELECT [Eximbills Module] FROM [ezfb_Product Master] where Product='" & cbProductList.SelectedValue.ToString & "' and  Phase='" & cbPhase.SelectedValue.ToString & "'")
            If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then
                If (ResDs.Tables(0).Rows(0).Item("Eximbills Module").ToString <> "") Then
                    feEximbillsModule = ResDs.Tables(0).Rows(0).Item("Eximbills Module").ToString
                Else
                    feEximbillsModule = ""
                End If
            End If
        Catch ex As Exception
            MsgBox("Exception in filleximbillsmodule" & ex.Message)
        End Try

    End Sub
    Public Sub fillsubmitteddocumentsgrid(wir As String)
        Try
            If (cbPhase.SelectedValue IsNot Nothing And cbPhase.SelectedValue <> "") Then
                'gridSubmittedDocuments.Items.Clear()


                fillDocTypeList(wir)
                '                Dim ViewRow = ResAccBasicInfoObj.Tables(0).Rows(0)
                '                Dim feRimNo1 = ViewRow("RIMNumber").ToString()

                '                'Dim ResDs = externalAPIobj.GetDatasetByQuery("select *,Case when SCAN ='View' then 'Rescan' Else '-' End As RESCAN FROM (select [DOCUMENT TYPE], [MANDATORY], case when itemid Is NULL then 'Scan' Else 'View' End As SCAN from (SELECT R1.[DOCUMENT TYPE], R1.[MANDATORY], itemid from (Select  distinct [Document Type] [DOCUMENT TYPE],Case WHEN Mandatory = 'true' THEN 'Mandatory' Else '-' End As  [MANDATORY] FROM [ezfb_Product CheckList Master]  where [Product]='" & cbProductList.SelectedValue.ToString & "' and [Phase]='" & cbPhase.SelectedValue.ToString & "') as r1 Left Join ezca_" & cabinetid & "_" & invitaAPIobj.TemplateId & "_items as itemtbl on r1.[Document Type]=itemtbl.[Document Type] And [Work Item Reference]='" & wir & "') as r2) AS R3 order by MANDATORY desc")


                '                Dim qry = "select *,'' FILEPATH,
                'Case 
                '	when SCAN ='View' then 'Rescan'  
                '	End As RESCAN 
                'from
                '(select [DOCUMENT TYPE], [MANDATORY],[itemid],
                'case when itemid Is NULL then 'Scan' 
                'Else 'View' 
                'End As SCAN 
                'from
                '(SELECT R1.[DOCUMENT TYPE], R1.[MANDATORY], itemid 
                'from (Select  distinct [Document Type] [DOCUMENT TYPE],
                'Case WHEN Mandatory = 'true' THEN 'Mandatory' 
                'Else '-' End As  [MANDATORY] 
                'FROM [ezfb_Product CheckList Master]  where [Product]='" & cbProductList.SelectedValue.ToString & "' and [Phase]='" & cbPhase.SelectedValue.ToString & "') as r1 Left Join ezca_3_" & invitaAPIobj.TemplateId & "_items as itemtbl on r1.[Document Type]=itemtbl.[Document Type] And [Work Item Reference]='" & wir & "') as r2) AS R3 order by MANDATORY desc"
                '                Dim ResDs = invitaAPIobj.GetDatasetByQuery(qry)

                '                If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then


                '                    gridSubmittedDocuments.ItemsSource = Nothing
                '                    '  gridSubmittedDocuments.ItemsSource = ResDs.Tables(0).DefaultView

                '                    GridDispDatatable = TryCast(ResDs.Tables(0), DataTable)
                '                    DuplicateDocTypeList = DocTypeList
                '                    ' DocTypeList.Clear()

                '                    For i = 0 To ResDs.Tables(0).Rows.Count - 1
                '                        DocTypeList.Add(New DocumentTypeList(ResDs.Tables(0).Rows(i).Item(5).ToString, ResDs.Tables(0).Rows(i).Item(3).ToString, ResDs.Tables(0).Rows(i).Item(4).ToString, ResDs.Tables(0).Rows(i).Item(0).ToString, ResDs.Tables(0).Rows(i).Item(1).ToString, ResDs.Tables(0).Rows(i).Item(2).ToString))
                '                    Next
                '                    gridSubmittedDocuments.ItemsSource = DocTypeList
                '                    gridSubmittedDocuments.Items.Refresh()
                '                    ' Dim documenttype = ResDs.Tables(0).Rows(0).Item("Document Type")
                '                    'MsgBox(documenttype)
                '                    Dim items1(ResDs.Tables(0).Rows.Count) As ComboBoxItem
                '                    cbDocumentType.Items.Clear()
                '                    For j = 0 To ResDs.Tables(0).Rows.Count - 1
                '                        items1(j) = New ComboBoxItem
                '                        If (ResDs.Tables(0).Rows(j).Item("Mandatory").ToString = "true") Then
                '                            'items1(j).Background = Brushes.SpringGreen
                '                            items1(j).Foreground = Brushes.OrangeRed
                '                            items1(j).Content = ResDs.Tables(0).Rows(j).Item("Document Type").ToString
                '                        Else
                '                            items1(j).Content = ResDs.Tables(0).Rows(j).Item("Document Type").ToString
                '                        End If
                '                        cbDocumentType.Items.Add(items1(j))
                '                    Next

                '                Else
                '                    cbDocumentType.Items.Clear()
                '                    If cbPhase.SelectedValue.ToString = "Select Phase" Then

                '                    End If
                '                    cbDocumentType.ItemsSource = Nothing
                '                End If
            End If


        Catch ex As Exception
            MsgBox("Exception in fillsubmitteddocumentsgrid" & ex.Message)
        End Try

    End Sub

    Private Sub cbProductList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbProductList.SelectionChanged
        Try
            If (cbProductList.SelectedValue IsNot Nothing) Then


                Dim ResDs = externalAPIobj.GetDatasetByQuery("SELECT distinct Phase FROM [ezfb_Product Master] where Product='" & cbProductList.SelectedValue.ToString & "'")

                If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then
                    cbPhase.ItemsSource = ResDs.Tables(0).DefaultView
                    cbPhase.DisplayMemberPath = ResDs.Tables(0).Columns("Phase").ToString
                    cbPhase.SelectedValuePath = ResDs.Tables(0).Columns("Phase").ToString
                    'cbDocumentType.ItemsSource = Nothing

                Else
                    If cbProductList.SelectedValue.ToString = "Select Product" Then
                        '  cbPhase.SelectedIndex = 0
                        '   cbPhase.SelectedValue = "Select Phase"
                    End If
                    cbPhase.ItemsSource = Nothing
                End If
            End If
            ' border1.Visibility = Visibility.Hidden
        Catch ex As Exception
            MsgBox("Exception in cbProductList_SelectionChanged " & ex.Message)
        End Try
        ' border1.Visibility = Visibility.Visible

    End Sub

    Private Sub cbPhase_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbPhase.SelectionChanged
        Try
            If (cbProductList.SelectedValue IsNot Nothing AndAlso cbProductList.SelectedValue <> "") Then
                If (WorkitemReference = "") Then
                    filleximbillsmodule()
                    fillsubmitteddocumentsgrid("")
                Else
                    filleximbillsmodule()
                    fillsubmitteddocumentsgrid(WorkitemReference)
                End If

            End If

        Catch ex As Exception
            MsgBox("Exception in cbPhase_SelectionChanged" & ex.Message)
        End Try
    End Sub

    'Private Sub btnVERIFY_KeyDown(sender As Object, e As KeyEventArgs) Handles btnVERIFY.KeyDown

    'End Sub

    Private Sub txtAccountNo_KeyDown(sender As Object, e As KeyEventArgs)
        'MsgBox(e.Key)
        'If (e.Key = 3) Then
        '    border1.Visibility = Visibility.Visible

        '    'Dim progbar As New ProgressBar()
        '    'progbar.IsIndeterminate = False
        '    'progbar.Orientation = Orientation.Horizontal
        '    'progbar.Width = 150
        '    'progbar.Height = 15
        '    'Dim d As New Duration(TimeSpan.FromSeconds(100))
        '    'Dim doubleanimation As New DoubleAnimation(100.0, d)
        '    'progbar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation)


        '    Dim AccBasicInfoObj = New AccBasicInfo()
        '    AccBasicInfoObj.acct_no = txtAccountNo.Text

        '    Dim ResAccBasicInfoObj = externalAPIobj.tf_acct_basic_info(AccBasicInfoObj)

        '    If ResAccBasicInfoObj IsNot Nothing AndAlso ResAccBasicInfoObj.Tables.Count > 0 AndAlso ResAccBasicInfoObj.Tables(0).Rows.Count > 0 Then

        '        Dim accstatus = ResAccBasicInfoObj.Tables(0).Rows(0).Item("Status")
        '        'MsgBox(accstatus)

        '        If (accstatus = "ACTIVE") Then
        '            cbProductList.IsEnabled = True
        '            gridAccountDetails.ItemsSource = ResAccBasicInfoObj.Tables(0).DefaultView
        '            lblStatusbar.Foreground = New SolidColorBrush(Colors.Green)
        '            lblStatusbar.Text = "Account is Active"
        '        Else
        '            cbProductList.IsEnabled = False
        '            Dim c As Color = CType(ColorConverter.ConvertFromString("#C24641"), Color)
        '            lblStatusbar.Foreground = New SolidColorBrush(c)
        '            'lblStatusbar.Foreground = New SolidColorBrush(Colors.DarkRed)
        '            lblStatusbar.Text = "Account is not active" & vbCrLf & "Please review with your Releationship Manager or The branch at which you opened the account"
        '        End If
        '    Else
        '        cbProductList.IsEnabled = False
        '        gridAccountDetails.ItemsSource = Nothing

        '        Dim c As Color = CType(ColorConverter.ConvertFromString("#C24641"), Color)
        '        lblStatusbar.Foreground = New SolidColorBrush(c)
        '        'lblStatusbar.Foreground = New System.Windows.Media.SolidColorBrush((Color) ColorConverter.ConvertFromString("#C24641"))
        '        'lblStatusbar.Foreground = New System.Windows.Media.SolidColorBrush((Color) RGB(194, 70, 65))
        '        lblStatusbar.Text = "Invalid Account No"
        '        MessageBox.Show("Invalid Account No", "TRADE FINANCE INFORMATION")
        '    End If

        '    border1.Visibility = Visibility.Hidden
        'End If
    End Sub

    Private Sub new_pdfThumbnailview(ByVal inputfile As String, ByVal outputfile As String)
        'apiobj.writetxtfle("new_pdfThumbnailview Called")
        'apiobj.writetxtfle("inputfile=" & inputfile)
        'apiobj.writetxtfle("outputfile=" & outputfile)
        Try
            Dim f As New PdfFocus()
            f.Serial = "10014625953"
            f.OpenPdf(inputfile)
            If (f.PageCount > 0) Then
                f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg
                f.ImageOptions.Dpi = 120
                'f.ImageOptions.JpegQuality = 95
                f.ToImage(outputfile, 1)
            End If
        Catch ex As Exception

            'apiobj.writetxtfle("Error In new_pdfThumbnailview - " + outputfile)
        End Try
        ' apiobj.writetxtfle("new_pdfThumbnailview Completed")
    End Sub
    Private Sub new_officeThumbnailview(ByVal inputfile As String, ByVal outputfile As String)
        ' apiobj.writetxtfle("new_officeThumbnailview Called")
        Try
            ' apiobj.writetxtfle("office Thumbnailview Started")
            Dim f As New PdfFocus()
            f.Serial = "10014625953"
            Dim officepath = inputfile
            Dim pdfpath = Path.ChangeExtension(officepath, ".pdf")
            Dim result = -1
            Dim u As New SautinSoft.UseOffice
            If (u.InitWord() = 0) Then
                Select Case Path.GetExtension(officepath).ToLower
                    Case ".doc"
                        result = u.ConvertFile(officepath, pdfpath, UseOffice.eDirection.DOC_to_PDF)
                        u.CloseWord()
                    Case ".docx"
                        result = u.ConvertFile(officepath, pdfpath, UseOffice.eDirection.DOCX_to_PDF)
                        u.CloseWord()
                    Case ".xls"
                        result = u.ConvertFile(officepath, pdfpath, UseOffice.eDirection.XLS_to_PDF)
                        u.CloseExcel()
                    Case ".xlsx"
                        result = u.ConvertFile(officepath, pdfpath, UseOffice.eDirection.XLSX_to_PDF)
                        u.CloseExcel()
                    Case ".ppt"
                        result = u.ConvertFile(officepath, pdfpath, UseOffice.eDirection.PPT_to_PDF)
                        u.ClosePowerPoint()
                    Case ".pptx"
                        result = u.ConvertFile(officepath, pdfpath, UseOffice.eDirection.PPTX_to_PDF)
                        u.ClosePowerPoint()
                    Case ".rtf"
                        result = u.ConvertFile(officepath, pdfpath, UseOffice.eDirection.RTF_to_PDF)
                        u.CloseWord()
                    Case ".mdb"
                        result = u.ConvertFile(officepath, pdfpath, UseOffice.eDirection.ACCESS_to_PDF)
                        u.CloseAccess()
                    Case ".html"
                        result = u.ConvertFile(officepath, pdfpath, UseOffice.eDirection.HTML_to_PDF)
                        u.CloseWord()
                End Select
                u.CloseWord()
                'apiobj.writetxtfle("conversion result - " + result.ToString())
                If (IO.File.Exists(pdfpath)) AndAlso result = 0 Then
                    new_pdfThumbnailview(pdfpath, outputfile)
                    '  Kill(pdfpath)
                    ' Else
                    '  apiobj.writetxtfle(outputfile + "- Error on thumb creation")
                    ' apiobj.writetxtfle(outputfile + "this File was Exported SucessFully With Thumb")
                End If
            End If
        Catch ex As Exception
            ' apiobj.writetxtfle("Error In new_officeThumbnailview - " + ex.ToString())
        End Try
    End Sub

    '-----------------------------
    Public Sub CreateThumbsForAllLocal(ByVal filetype As String, ByVal Varpath As String, ByVal OldFileName As String, dirpath As String, ifilepath As String, itemid As String)
        'apiobj.writetxtfle("CreateThumbsForAllLocal Called")
        'apiobj.writetxtfle("Varpath=" & Varpath)
        'apiobj.writetxtfle("dirpath=" & dirpath)
        'apiobj.writetxtfle("ifilepath=" & ifilepath)
        'apiobj.writetxtfle("OldFileName=" & OldFileName)


        Try
            'Kill(Varpath + "\" + xmlfilename)
            filetype = filetype.ToString.ToLower
            Dim outpath = Path.Combine(dirpath, ifilepath, itemid)
            If filetype = "pdf" Or filetype = "tif" Then
                new_pdfThumbnailview(Varpath + "\" + OldFileName, outpath + ".jpg")
                ' apiobj.writetxtfle(outpath + "this File was Exported SucessFully With Thumb")
            ElseIf filetype <> "jpg" And filetype <> "jpeg" And filetype <> "png" And filetype <> "gif" And filetype <> "msg" Then
                new_officeThumbnailview(Varpath + "\" + OldFileName, outpath + ".jpg")
            End If
        Catch ex As Exception
            'apiobj.writetxtfle(ex.Message.ToString)
        Finally
            Try
                If Directory.Exists(Varpath + "\Temp") Then
                    Directory.Delete(Varpath + "\Temp", True)
                End If
                If Directory.Exists(Varpath + "\Convert_Pdf") Then
                    Directory.Delete(Varpath + "\Convert_Pdf", True)
                End If
                'Kill(Varpath + "\" + temp)
                '  Kill(Varpath + "\" + OldFileName)
            Catch ex As Exception
            End Try
        End Try
    End Sub

    Private Sub MinimizeButton_Click(sender As Object, e As RoutedEventArgs)
        Try
            Application.Current.MainWindow.WindowState = WindowState.Minimized
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
        Try
            End
            'Application.Current.Shutdown()
            'Application.Current.MainWindow.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mainhead_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Try
            Application.Current.MainWindow.DragMove()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtAccountNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles txtAccountNo.LostFocus
        'border1.Visibility = Visibility.Visible
        'Try
        '    If (txtAccountNo.Text <> "") Then
        '        verifyAccountNo()
        '    End If
        'Catch ex As Exception

        'Finally
        '    border1.Visibility = Visibility.Hidden
        'End Try
    End Sub

    Private Sub txtWorkItemNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles txtWorkItemNo.LostFocus
        'border1.Visibility = Visibility.Visible
        'Try
        '    verifyWorkItemReference(txtWorkItemNo.Text, 3)
        'Catch ex As Exception

        'Finally
        '    border1.Visibility = Visibility.Hidden
        'End Try
    End Sub

    Private Sub btnVERIFYWorkItem_Click(sender As Object, e As RoutedEventArgs) Handles btnVERIFYWorkItem.Click
        border1.Visibility = Visibility.Visible
        Try
            'btnWorkItemVerifySpin.Visibility = Visibility.Visible
            WorkitemReference = txtWorkItemNo.Text
            txtwir.Text = WorkitemReference.ToString
            verifyWorkItemReference(txtWorkItemNo.Text, 3)
        Catch ex As Exception
            MsgBox("Exception in btnVERIFYWorkItem_Click " + ex.Message)
        Finally
            border1.Visibility = Visibility.Hidden
        End Try
    End Sub

    Private Sub txtAccountNo_IsKeyboardFocusWithinChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles txtAccountNo.IsKeyboardFocusWithinChanged

    End Sub

    Private Sub btnclear_Click(sender As Object, e As RoutedEventArgs)
        Try
            clearAllvalues(1)
        Catch ex As Exception
            MsgBox("Exception in btnclear_Click " + ex.Message)
        End Try
    End Sub
    Public Sub clearAllvalues(clearoption As Integer)
        Try
            '     clearAllvalues()
            Me.Dispatcher.Invoke(Sub()
                                     If (clearoption = 1) Then

                                         txtAccountNo.IsEnabled = True
                                         btnVERIFY.IsEnabled = True
                                         txtAccountNo.Text = ""
                                         txtWorkItemNo.Text = ""
                                         lblStatusbar.Text = ""

                                     ElseIf (clearoption = 2) Then
                                         txtWorkItemNo.Text = ""
                                     End If

                                     gridAccountDetails.ItemsSource = Nothing
                                     gridSubmittedDocuments.ItemsSource = Nothing
                                     btnINITIATESCANNING.IsEnabled = False
                                     cbProductList.ItemsSource = Nothing
                                     cbProductList.IsEnabled = False
                                     'cbProductList.IsEnabled = False
                                     ' cbProductList.SelectedValue = "Select Product"
                                     cbPhase.ItemsSource = Nothing
                                     'cbDocumentType.Items.Clear()
                                     'cbDocumentType.ItemsSource = Nothing
                                     cbPhase.IsEnabled = False
                                     ECMViewer._documentViewer.SetDocument(Nothing)
                                     ECMViewerToolbar.IsEnabled = False

                                     LblFileName.Content = ""
                                     'ECMViewer.pageNumber = 0
                                     'ECMViewer.pageCount = 0
                                     'UpdateFileInfo()
                                     _currentFileName = ""
                                     txtwir.Text = ""
                                     btnSubmit.IsEnabled = False
                                     btnRescan.IsEnabled = False
                                     btnClearRightPane.IsEnabled = False
                                     btnFinalSubmit.IsEnabled = False

                                     submittedDocNos = ""
                                     txtSubmittedWorkItems.Text = ""
                                     WorkitemReference = ""
                                     WorkItmref = ""
                                 End Sub)

        Catch ex As Exception
            MsgBox("Exception in clearAllvalues " + ex.Message)
        End Try
    End Sub
    Public Function fillProduct()
        Me.Dispatcher.Invoke(Sub()
                                 Dim ResDs = externalAPIobj.GetDatasetByQuery("SELECT distinct Product FROM [ezfb_Product Master]")
                                 If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then
                                     '  Dim defrow As New DataRow

                                     ' ResDs.Tables(0).Rows.Add("Select Product")
                                     ' Dim product = ResDs.Tables(0).Rows(0).Item("Product")
                                     ' MsgBox(product)
                                     cbProductList.DisplayMemberPath = ResDs.Tables(0).Columns("Product").ToString
                                     cbProductList.SelectedValuePath = ResDs.Tables(0).Columns("Product").ToString
                                     cbProductList.ItemsSource = ResDs.Tables(0).DefaultView
                                     ' cbProductList.SelectedValue = "Select Product"
                                 End If
                             End Sub)

    End Function

    Private Sub btncok_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnlog_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnpop_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub cbDocumentType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            If (cbDocumentType.SelectedValue IsNot Nothing) Then
                btnINITIATESCANNING.IsEnabled = True
            Else
                btnINITIATESCANNING.IsEnabled = False
            End If
        Catch ex As Exception
            MsgBox("Exception in cbDocumentType_SelectionChanged " + ex.Message)
        End Try
    End Sub

    Private Sub BtnZoomOut_Click(sender As Object, e As RoutedEventArgs)
        Try
            ECMViewer.ZoomOut()
        Catch ex As Exception
        Finally
            UpdateFileInfo()
        End Try
    End Sub

    Private Sub BtnZoomIn_Click(sender As Object, e As RoutedEventArgs)
        Try
            ECMViewer.ZoomIn()
        Catch ex As Exception
        Finally
            UpdateFileInfo()
        End Try
    End Sub

    Private Sub BtnFitWithHeight_Click(sender As Object, e As RoutedEventArgs)
        Try
            ECMViewer.FitAlways()
        Catch ex As Exception
        Finally
            UpdateFileInfo()
        End Try
    End Sub

    Private Sub BtnFitwithWidth_Click(sender As Object, e As RoutedEventArgs)
        Try
            ECMViewer.FitWidth()
        Catch ex As Exception
        Finally
            UpdateFileInfo()
        End Try
    End Sub

    Private Sub BtnFit_Click(sender As Object, e As RoutedEventArgs)
        Try
            ECMViewer.Stretch()
        Catch ex As Exception
        Finally
            UpdateFileInfo()
        End Try
    End Sub

    Private Sub BtnPageDelete_Click(sender As Object, e As RoutedEventArgs)
        Try

            Dim res = MessageBox.Show("Do you Want to Delete the Scanned page?", "Scan & Index", MessageBoxButton.YesNo, MessageBoxImage.Question)
            If res = MessageBoxResult.Yes Then
                progfor = "DeletePage"
                ' ECMViewer._documentViewer.SetDocument(Nothing)
                ECMViewer.LoadDocumentFromFileWithPageNumber(_currentFileName)
                ECMViewer.DeleteTifPage(_currentFileName, _fileFormat, _bitsPerPixel)
                ECMViewer.SaveTifPage(_currentFileName, _fileFormat, _bitsPerPixel)
                ECMViewer.LoadDocumentFromFileWithPageNumber(_currentFileName)
                ECMViewerToolbar.IsEnabled = True
                UpdateFileInfo()
                'reminderAnim = TryCast(Me.Resources("PageProgress"), Storyboard)
                'reminderAnim.Begin()
                'worker.RunWorkerAsync()
            End If
        Catch ex As Exception
        Finally
            UpdateFileInfo()
        End Try
    End Sub
    Private Sub BtnPageSave_Click(sender As Object, e As RoutedEventArgs)
        Try
            progfor = "SavePage"
            'ECMViewer._documentViewer.SetDocument(Nothing)
            ECMViewer.LoadDocumentFromFileWithPageNumber(_currentFileName)
            ECMViewerToolbar.IsEnabled = True
            ECMViewer.SaveTifPage(_currentFileName, _fileFormat, _bitsPerPixel)
            UpdateFileInfo()

            'reminderAnim = TryCast(Me.Resources("PageProgress"), Storyboard)
            'reminderAnim.Begin()
            'worker.RunWorkerAsync()
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            UpdateFileInfo()
        End Try
    End Sub

    Private Sub BtnRotateRight_Click(sender As Object, e As RoutedEventArgs)
        Try
            ECMViewer.RotateRight(_fileFormat, _bitsPerPixel)
        Catch ex As Exception
        Finally
            UpdateFileInfo()

        End Try
    End Sub

    Private Sub BtnRotateLeft_Click(sender As Object, e As RoutedEventArgs)
        Try
            ECMViewer.RotateLeft(_fileFormat, _bitsPerPixel)
        Catch ex As Exception
        Finally
            UpdateFileInfo()

        End Try
    End Sub

    Public Sub UpdateFileInfo()
        Try
            'MsgBox("alert4 ecmviewer load")
            ' progBar.Visibility = Visibility.Collapsed
            LblFileName.Content = ""
            If _currentFileName <> "" Then
                'LblFileName.Content = _currentFileName.Replace(_currentFileName.Substring(0, _currentFileName.LastIndexOf("\") + 1), "") + " Page[" + ECMViewer.pageNumber.ToString() + " / " + ECMViewer.pageCount.ToString() + "]"
                LblFileName.Content = " Page[ " + ECMViewer.pageNumber.ToString() + " / " + ECMViewer.pageCount.ToString() + " ]"
                If (ECMViewer.pageCount <= 1) Then
                    BtnPageDelete.IsEnabled = False
                    BtnMoveFirst.IsEnabled = False
                    BtnMoveLast.IsEnabled = False
                    BtnMoveLeft.IsEnabled = False
                    BtnMoveRight.IsEnabled = False
                Else
                    'MsgBox("alert5 ecmviewer load")
                    BtnPageDelete.IsEnabled = True
                    BtnMoveFirst.IsEnabled = True
                    BtnMoveLast.IsEnabled = True
                    BtnMoveLeft.IsEnabled = True
                    BtnMoveRight.IsEnabled = True
                End If
            Else
                LblFileName.Content = ""
            End If

        Catch ex As Exception
            MsgBox("Exception in UpdateFileInfo " & ex.Message)
        End Try
    End Sub




    Public Function ColorChange(Filefrmt As String)
        Try
            If Filefrmt.ToUpper = "BW" Then
                _fileFormat = RasterImageFormat.CcittGroup4
                _bitsPerPixel = 1
            ElseIf Filefrmt.ToUpper = "COLOR" Then
                _fileFormat = RasterImageFormat.TifJpeg422
                _bitsPerPixel = 24
            End If
        Catch ex As Exception
            MsgBox("Exception in ColorChange " & ex.ToString)
        End Try
    End Function
    'shiva
    Private Sub BtnMoveFirst_Click(sender As Object, e As RoutedEventArgs)
        Try
            ECMViewer.FirstPage(_currentFileName)

        Catch ex As Exception
        Finally
            UpdateFileInfo()
        End Try
    End Sub

    Private Sub BtnMoveLeft_Click(sender As Object, e As RoutedEventArgs)
        Try

            ECMViewer.PreviousPage(_currentFileName)
        Catch ex As Exception
        Finally
            UpdateFileInfo()
        End Try
    End Sub

    Private Sub BtnMoveRight_Click(sender As Object, e As RoutedEventArgs)
        Try
            ECMViewer.NextPage(_currentFileName)
        Catch ex As Exception
        Finally
            UpdateFileInfo()
        End Try
    End Sub

    Private Sub BtnMoveLast_Click(sender As Object, e As RoutedEventArgs)
        Try
            ECMViewer.LastPage(_currentFileName)
        Catch ex As Exception
        Finally
            UpdateFileInfo()
        End Try
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As RoutedEventArgs) Handles btnSubmit.Click
        Try
            '  border1.Visibility = Visibility.Visible
            If Not worker.IsBusy Then
                progfor = "Submit"
                btnINITIATESCANNING.IsEnabled = False
                reminderAnim = TryCast(Me.Resources("PageProgress1"), Storyboard)
                reminderAnim.Begin()
                worker.RunWorkerAsync()

            End If

        Catch ex As Exception
            MsgBox("Exception in btnSubmit_Click" & ex.Message)
        Finally
            'thisTimer1.Stop()
            'reminderAnim.Stop()
            'worker.CancelAsync()

            ' border1.Visibility = Visibility.Hidden
        End Try

    End Sub
    Private Sub btnFinalSubmit_Click(sender As Object, e As RoutedEventArgs) Handles btnFinalSubmit.Click
        Try
            '  border1.Visibility = Visibility.Visible
            Dim MendatoryDocPendingFlag = False
            For i = 0 To DocTypeList.Count - 1
                If (DocTypeList(i).MANDATORY.ToString.ToUpper = "MANDATORY") Then
                    If (DocTypeList(i).FILEPATH = "" And DocTypeList(i).itemid = "") Then
                        MendatoryDocPendingFlag = True
                        Exit For
                    End If
                End If
            Next

            If (MendatoryDocPendingFlag = True) Then
                Dim msg As New MessageWin("Some Mandatory Documents are Not Scanned." & vbCrLf & "Do you want to Submit the Document?")
                msg.ShowDialog()

                If NewWorkItem = 1 Then
                    If Not worker.IsBusy Then
                        progfor = "Export"
                        btnINITIATESCANNING.IsEnabled = False
                        reminderAnim = TryCast(Me.Resources("PageProgress1"), Storyboard)
                        reminderAnim.Begin()
                        worker.RunWorkerAsync()
                    End If
                End If
            Else
                If Not worker.IsBusy Then
                    progfor = "Export"
                    btnINITIATESCANNING.IsEnabled = False
                    reminderAnim = TryCast(Me.Resources("PageProgress1"), Storyboard)
                    reminderAnim.Begin()
                    worker.RunWorkerAsync()
                End If
            End If

        Catch ex As Exception
            MsgBox("Exception in btnFinalSubmit_Click" & ex.Message)
        Finally
            'thisTimer1.Stop()
            'reminderAnim.Stop()
            'worker.CancelAsync()

            ' border1.Visibility = Visibility.Hidden
        End Try

    End Sub

    Private Sub Window_SizeChanged(sender As Object, e As SizeChangedEventArgs)

    End Sub



    Private Sub btnRescan_Click(sender As Object, e As RoutedEventArgs)
        Try
            reminderAnim = TryCast(Me.Resources("PageProgress1"), Storyboard)
            reminderAnim.Begin()
            InititateScanning()
            ' Me.InvalidateVisual()
            'btnINITIATESCANNING.InvalidateVisual()
            'archiveFile()
            'Dim Appcon1 As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
            'If (Appcon1("impersonate") = "true") Then
            '    acct = New AliasAccount(Appcon1("Username"), Appcon1("Password"), Appcon1("Domain"))
            '    '  acct = New AliasAccount("invita\BbkTFWP", "U@tTFW$21", "invita")
            '    Try
            '        acct.BeginImpersonation()
            '        impersonate = True
            '    Catch ex As Exception
            '        MsgBox("Exception in btnRescan_Click " + ex.Message)
            '    End Try
            '    If impersonate Then
            '        archiveFile()
            '    Else
            '        MsgBox("Failed to connect to File Server ")
            '    End If
            'Else
            '    archiveFile()
            'End If

        Catch ex As Exception
            MsgBox("Exception in btnRescan_Click " + ex.Message)
        Finally
            reminderAnim.Stop()
        End Try
    End Sub

    'Private Sub cbPhase_PreviewMouseMove(sender As Object, e As MouseEventArgs) Handles cbPhase.PreviewMouseMove

    'End Sub
    'Public Class dataMember

    '    Public Property submitteddocuments As String
    '    Public Property noofpages As String
    '    ' Public Property submitteddocuments AsString  
    'End Class
    Private Sub gridSubmittedDocuments_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles gridSubmittedDocuments.SelectionChanged
        'Try

        '    Dim Appcon2 As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
        '    Dim fileviewerip = Appcon2("fileviewerip")
        '    If (fileviewerip <> "") Then
        '        If (gridSubmittedDocuments.SelectedItems.Count > 0) Then
        '            Dim DG As DataGrid = sender
        '            Dim DGRow As System.Data.DataRowView = gridSubmittedDocuments.SelectedItem

        '            Dim itemid = DGRow.Item("itemid").ToString
        '            ' MsgBox(itemid)
        '            Process.Start(fileviewerip & "/pdf.js_dev/web/viewer.html?tmpid=" & invitaAPIobj.TemplateId & "&itemid=" & itemid)

        '        End If
        '    End If

        'Catch ex As Exception
        '    MsgBox("Exception in gridSubmittedDocuments_SelectionChanged " + ex.Message)
        'End Try
        'Try
        '    Dim Appcon2 As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
        '    Dim fileviewerip = Appcon2("fileviewerip")
        '    If (fileviewerip <> "") Then
        '        If (gridSubmittedDocuments.SelectedItems.Count > 0) Then
        '            Dim DG As DataGrid = sender
        '            Dim DGRow As System.Data.DataRowView = gridSubmittedDocuments.SelectedItem

        '            Dim gridSelectedCellValue = DGRow.Item("SCAN").ToString
        '            If (gridSelectedCellValue.ToString.ToUpper = "SCAN") Then

        '            Else
        '                Dim itemid = DGRow.Item("ITEMID").ToString
        '                Process.Start(fileviewerip & "/pdf.js_dev/web/viewer.html?tmpid=" & invitaAPIobj.TemplateId & "&itemid=" & itemid)
        '            End If
        '        End If
        '    End If
        'Catch ex As Exception
        '    MsgBox("Exception in gridSubmittedDocuments_MouseUp " + ex.Message)
        'End Try
    End Sub


    Private Sub gridSubmittedDocuments_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles gridSubmittedDocuments.MouseUp

        'Try
        '    Dim Appcon2 As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
        '    Dim fileviewerip = Appcon2("fileviewerip")
        '    If (fileviewerip <> "") Then
        '        If (gridSubmittedDocuments.SelectedItems.Count > 0) Then
        '            Dim DG As DataGrid = sender
        '            Dim DGRow As System.Data.DataRowView = gridSubmittedDocuments.SelectedItem

        '            Dim gridSelectedCellValue = DGRow.Item("SCAN").ToString
        '            If (gridSelectedCellValue.ToString.ToUpper = "SCAN") Then

        '            Else
        '                Dim itemid = DGRow.Item("itemid").ToString
        '                Process.Start(fileviewerip & "/pdf.js_dev/web/viewer.html?tmpid=" & invitaAPIobj.TemplateId & "&itemid=" & itemid)

        '            End If

        '        End If
        '        End If
        'Catch ex As Exception
        '    MsgBox("Exception in gridSubmittedDocuments_MouseUp " + ex.Message)
        'End Try

    End Sub

    Private Sub Hyperlink_RequestNavigate(sender As Object, e As RequestNavigateEventArgs)

        Try


            Dim ResTicketQueue = invitaAPIobj.GetDatasetByQuery("SELECT [Work Item Reference] WORKITEMREFERNCE,[Account Number] ACCOUNTNUMBER,[Product] PRODUCT,[Phase] PHASE,CreatedOn [CREATEDON],UpdatedOn,CreatedBy,UpdatedBy,[TicketQueueId],'Select' AS [CHOOSE] FROM BBK_TicketQueue where [Account Number]='" + txtAccountNo.Text + "' and ProcessId=0 order by [Work Item Reference] desc")
            If ResTicketQueue IsNot Nothing AndAlso ResTicketQueue.Tables.Count > 0 AndAlso ResTicketQueue.Tables(0).Rows.Count > 0 Then
                submittedWIGridObj = New SubmittedWorkItemsGrid(ResTicketQueue)
                Dim totrec = ResTicketQueue.Tables(0).Rows.Count
                submittedWIGridObj.ShowDialog()
            End If

        Catch ex As Exception
            MsgBox("Exception in Hyperlink_RequestNavigate " + ex.Message)
        End Try
    End Sub

    Public Sub OnCellHyperlinkScanOrViewClick(sender As Object, e As RoutedEventArgs)
        Dim gridSelectedCellValue = ""
        'MsgBox("WORK ITEM REF" & id)

        'Dim selectedFile As DataRowView = gridSubmittedDocuments.SelectedItem
        'GRIDWIR = Convert.ToString(selectedFile.Row.ItemArray(0))
        'GRIDPRODUCT = Convert.ToString(selectedFile.Row.ItemArray(2))
        'GRIDPHASE = Convert.ToString(selectedFile.Row.ItemArray(3))

        'MsgBox("PRODUCT : " & PRODUCT & "  ,   PHASE : " & PHASE)
        Try

            If (LblFileName.Content = "") Then
                Dim Appcon2 As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
                Dim fileviewerip = Appcon2("fileviewerip")
                If (fileviewerip <> "") Then
                    If (gridSubmittedDocuments.SelectedItems.Count > 0) Then
                        If (gridSubmittedDocuments.SelectedIndex <> -1) Then
                            gridSelectedCellValue = DocTypeList(gridSubmittedDocuments.SelectedIndex).SCAN.ToString
                            gridSelectedCellDocumentTypeValue = DocTypeList(gridSubmittedDocuments.SelectedIndex).DOCUMENT_TYPE.ToString
                            gridSelectedRowNumber = gridSubmittedDocuments.SelectedIndex
                            ' MsgBox(gridSelectedRowNumber)
                        End If
                        'txtUsrname.Text = userlist(LVuserphoto.SelectedIndex).Name.ToString

                        ' Dim SelectedRowDetails As DataRowView = gridSubmittedDocuments.SelectedItem

                        ' Dim gridSelectedCellValue = SelectedRowDetails.Item("SCAN").ToString
                        'gridSelectedCellDocumentTypeValue = SelectedRowDetails.Item("DOCUMENT TYPE").ToString
                        'gridSelectedRowNumber = gridSubmittedDocuments.Items.IndexOf(gridSubmittedDocuments.SelectedItem)

                        feDocType = gridSelectedCellDocumentTypeValue
                        'gridSelectedCellDocumentTypeValue =
                        If (gridSelectedCellValue.ToString.ToUpper = "SCAN") Then
                            InititateScanning()
                            If (scannedfilepath <> "") Then
                                'DocTypeList.Item(gridSelectedRowNumber).FILEPATH = scannedfilepath
                                ' fillsubmitteddocumentsgrid(txtwir.Text)
                                DocTypeList(gridSelectedRowNumber).FILEPATH = scannedfilepath
                                DocTypeList(gridSelectedRowNumber).SCAN = "View"
                                DocTypeList(gridSelectedRowNumber).RESCAN = "Rescan"
                                gridSubmittedDocuments.Items.Refresh()
                            End If
                        Else
                            If (DocTypeList(gridSelectedRowNumber).FILEPATH = "") Then
                                'Dim itemid = Convert.ToString(SelectedRowDetails.Row.ItemArray(2))
                                Dim itemid = DocTypeList(gridSubmittedDocuments.SelectedIndex).itemid.ToString
                                Process.Start(fileviewerip & "/pdf.js_dev/web/viewer.html?tmpid=" & invitaAPIobj.TemplateId & "&itemid=" & itemid)
                            Else
                                ECMViewer.LoadDocumentFromFileWithPageNumber(DocTypeList(gridSelectedRowNumber).FILEPATH.ToString)
                                ECMViewerToolbar.IsEnabled = True
                            End If
                            btnClearRightPane.IsEnabled = True
                            btnRescan.IsEnabled = True
                        End If
                    End If
                End If
            Else
                'MsgBox("Please Submit the document First")
                Dim msg As New MessageWin("Scanned Docment Not Saved..." & vbCrLf & "Do you want to Save?")
                msg.ShowDialog()
                If (NewWorkItem = 1) Then
                    SaveFileInLocal()
                Else
                    ClearFilepathIdInGrid()
                End If

                'gridSubmittedDocuments.row(gridSelectedRowNumber).Selected = True
            End If

        Catch ex As Exception
            MsgBox("Exception in OnCellHyperlinkScanOrViewClick " + ex.Message)
        End Try
    End Sub
    Public Sub OnCellHyperlinkReScanClick(sender As Object, e As RoutedEventArgs)
        'OnCellHyperlinkReScanClick
        Try
            Dim gridSelectedCellValue = ""
            Dim Appcon2 As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
            If (gridSubmittedDocuments.SelectedItems.Count > 0) Then
                If (gridSubmittedDocuments.SelectedIndex <> -1) Then
                    gridSelectedCellValue = DocTypeList(gridSubmittedDocuments.SelectedIndex).RESCAN.ToString
                    gridSelectedCellDocumentTypeValue = DocTypeList(gridSubmittedDocuments.SelectedIndex).DOCUMENT_TYPE.ToString
                    gridSelectedRowNumber = gridSubmittedDocuments.SelectedIndex
                    If (gridSelectedCellValue.ToString.ToUpper = "RESCAN") Then
                        feDocType = gridSelectedCellDocumentTypeValue
                        InititateScanning()
                        If (scannedfilepath <> "") Then
                            'DocTypeList.Item(gridSelectedRowNumber).FILEPATH = scannedfilepath
                            ' fillsubmitteddocumentsgrid(txtwir.Text)
                            DocTypeList(gridSelectedRowNumber).FILEPATH = scannedfilepath
                            DocTypeList(gridSelectedRowNumber).SCAN = "View"
                            DocTypeList(gridSelectedRowNumber).RESCAN = "Rescan"
                            gridSubmittedDocuments.Items.Refresh()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox("Exception in OnCellHyperlinkReScanClick " + ex.Message)
        End Try
    End Sub
    Private Sub Hyperlink_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim ResTicketQueue = invitaAPIobj.GetDatasetByQuery("SELECT [Work Item Reference] WORKITEMREFERNCE,[Account Number] ACCOUNTNUMBER,[Product] PRODUCT,[Phase] PHASE,CreatedOn [CREATEDON],UpdatedOn,CreatedBy,UpdatedBy,[TicketQueueId],'Select' AS [CHOOSE] FROM BBK_TicketQueue where [Account Number]='" + txtAccountNo.Text + "' and ProcessId=0 order by [Work Item Reference] desc")
            If ResTicketQueue IsNot Nothing AndAlso ResTicketQueue.Tables.Count > 0 AndAlso ResTicketQueue.Tables(0).Rows.Count > 0 Then
                submittedWIGridObj = New SubmittedWorkItemsGrid(ResTicketQueue)
                Dim totrec = ResTicketQueue.Tables(0).Rows.Count
                submittedWIGridObj.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox("Exception in Hyperlink_Click " + ex.Message)
        End Try
    End Sub

    Private Sub CheckBox_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim ResTicketQueue = invitaAPIobj.GetDatasetByQuery("SELECT [Work Item Reference] WORKITEMREFERNCE,[Account Number] ACCOUNTNUMBER,[Product] PRODUCT,[Phase] PHASE,CreatedOn [CREATEDON],UpdatedOn,CreatedBy,UpdatedBy,[TicketQueueId],'Select' AS [CHOOSE] FROM BBK_TicketQueue where [Account Number]='" + txtAccountNo.Text + "'  and ProcessId=0 order by [Work Item Reference] desc")
            If ResTicketQueue IsNot Nothing AndAlso ResTicketQueue.Tables.Count > 0 AndAlso ResTicketQueue.Tables(0).Rows.Count > 0 Then
                submittedWIGridObj = New SubmittedWorkItemsGrid(ResTicketQueue)
                Dim totrec = ResTicketQueue.Tables(0).Rows.Count
                submittedWIGridObj.ShowDialog()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub chkboxShowLocally_Checked(sender As Object, e As RoutedEventArgs)
        Try
            If (cbPhase.SelectedValue IsNot Nothing) Then

                Dim ViewRow = ResAccBasicInfoObj.Tables(0).Rows(0)
                Dim feRimNo1 = ViewRow("RIMNumber").ToString()

                'Dim ResDs = externalAPIobj.GetDatasetByQuery("select *,Case when SCAN ='View' then 'Rescan' Else '-' End As RESCAN FROM (select [DOCUMENT TYPE], [MANDATORY], case when itemid Is NULL then 'Scan' Else 'View' End As SCAN from (SELECT R1.[DOCUMENT TYPE], R1.[MANDATORY], itemid from (Select  distinct [Document Type] [DOCUMENT TYPE],Case WHEN Mandatory = 'true' THEN 'Mandatory' Else '-' End As  [MANDATORY] FROM [ezfb_Product CheckList Master]  where [Product]='" & cbProductList.SelectedValue.ToString & "' and [Phase]='" & cbPhase.SelectedValue.ToString & "') as r1 Left Join ezca_" & cabinetid & "_" & invitaAPIobj.TemplateId & "_items as itemtbl on r1.[Document Type]=itemtbl.[Document Type] And [Work Item Reference]='" & wir & "') as r2) AS R3 order by MANDATORY desc")


                Dim ResDs = invitaAPIobj.GetDatasetByQuery("select *,
Case 
	when SCAN ='View' then 'Rescan'  
	End As RESCAN 
from
(select [DOCUMENT TYPE], [MANDATORY],[itemid],
case when itemid Is NULL then 'Scan' 
Else 'View' 
End As SCAN 
from
(SELECT R1.[DOCUMENT TYPE], R1.[MANDATORY], itemid 
from (Select  distinct [Document Type] [DOCUMENT TYPE],
Case WHEN Mandatory = 'true' THEN 'Mandatory' 
Else '-' End As  [MANDATORY] 
FROM [ezfb_Product CheckList Master]  where [Product]='" & cbProductList.SelectedValue.ToString & "' and [Phase]='" & cbPhase.SelectedValue.ToString & "') as r1 Left Join ezca_" & cabinetid.ToString & "_" & invitaAPIobj.TemplateId & "_items as itemtbl on r1.[Document Type]=itemtbl.[Document Type] And [Work Item Reference]='" & submittedWIGridObj.GRIDWIR & "') as r2) AS R3 order by MANDATORY desc")

                If ResDs IsNot Nothing AndAlso ResDs.Tables.Count > 0 AndAlso ResDs.Tables(0).Rows.Count > 0 Then


                    gridSubmittedDocuments.ItemsSource = Nothing
                    gridSubmittedDocuments.ItemsSource = ResDs.Tables(0).DefaultView
                    ' Dim documenttype = ResDs.Tables(0).Rows(0).Item("Document Type")
                    'MsgBox(documenttype)
                    Dim items1(ResDs.Tables(0).Rows.Count) As ComboBoxItem
                    cbDocumentType.Items.Clear()
                    For j = 0 To ResDs.Tables(0).Rows.Count - 1
                        items1(j) = New ComboBoxItem
                        If (ResDs.Tables(0).Rows(j).Item("Mandatory").ToString = "true") Then
                            'items1(j).Background = Brushes.SpringGreen
                            items1(j).Foreground = Brushes.OrangeRed
                            items1(j).Content = ResDs.Tables(0).Rows(j).Item("Document Type").ToString
                        Else
                            items1(j).Content = ResDs.Tables(0).Rows(j).Item("Document Type").ToString
                        End If
                        cbDocumentType.Items.Add(items1(j))
                    Next

                Else
                    cbDocumentType.Items.Clear()
                    If cbPhase.SelectedValue.ToString = "Select Phase" Then

                    End If
                    cbDocumentType.ItemsSource = Nothing
                End If
            End If


        Catch ex As Exception
            MsgBox("Exception in fillsubmitteddocumentsgrid" & ex.Message)
        End Try
    End Sub

    Private Sub chkboxShowLocally_Unchecked(sender As Object, e As RoutedEventArgs)
        fillsubmitteddocumentsgrid(submittedWIGridObj.GRIDWIR)
    End Sub

    Private Sub btnclearRightPane_Click(sender As Object, e As RoutedEventArgs)
        clearCurrentLoadedFile()
    End Sub
    Public Sub clearCurrentLoadedFile()
        Try
            '     clearAllvalues()
            Me.Dispatcher.Invoke(Sub()
                                     If (DocTypeList.Count > 0) Then
                                         Dim nomsg1 As New MessageWin("Do you want to delete the scanned Documents?")
                                         nomsg1.ShowDialog()

                                         Dim qry = ""
                                         Dim res = 0
                                         If NewWorkItem = 1 Then
                                             If (DocTypeList.Count > 1) Then
                                                 'For i = 0 To DocTypeList.Count - 1
                                                 If (DocTypeList.Item(gridSelectedRowNumber).FILEPATH <> "" And sessionid <> "") Then
                                                     qry = "delete from ezca_" & cabinetid.ToString & "_" & invitaAPIobj.TemplateId & "_stage where ifilepath like '%" & sessionid.ToString & "%' and ifilename='" & DocTypeList.Item(gridSelectedRowNumber).FILEPATH.ToString & "'"
                                                     res = invitaAPIobj.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qry)
                                                     DocTypeList(gridSelectedRowNumber).FILEPATH = ""
                                                     DocTypeList(gridSelectedRowNumber).SCAN = "Scan"
                                                     DocTypeList(gridSelectedRowNumber).RESCAN = ""
                                                     gridSubmittedDocuments.Items.Refresh()
                                                     ECMViewer._documentViewer.SetDocument(Nothing)
                                                     LblFileName.Content = ""
                                                     ECMViewerToolbar.IsEnabled = False
                                                     btnClearRightPane.IsEnabled = False
                                                     btnRescan.IsEnabled = False
                                                     btnSubmit.IsEnabled = False
                                                 Else
                                                     DocTypeList(gridSelectedRowNumber).FILEPATH = ""
                                                     DocTypeList(gridSelectedRowNumber).SCAN = "Scan"
                                                     DocTypeList(gridSelectedRowNumber).RESCAN = ""
                                                     gridSubmittedDocuments.Items.Refresh()
                                                     ECMViewer._documentViewer.SetDocument(Nothing)
                                                     LblFileName.Content = ""
                                                     ECMViewerToolbar.IsEnabled = False
                                                     btnClearRightPane.IsEnabled = False
                                                     btnRescan.IsEnabled = False
                                                     btnSubmit.IsEnabled = False
                                                 End If
                                                 'Next
                                             End If


                                         End If
                                     End If

                                 End Sub)

        Catch ex As Exception
            MsgBox("Exception in clearAllvalues " + ex.Message)
        End Try
    End Sub
    Public Sub clearAllvaluesIncludingLocalFile()
        Try
            '     clearAllvalues()
            Me.Dispatcher.Invoke(Sub()
                                     If (DocTypeList.Count > 0) Then
                                         Dim nomsg1 As New MessageWin("Do you want to delete the scanned Documents?")
                                         nomsg1.ShowDialog()

                                         Dim qry = ""
                                         Dim res = 0
                                         If NewWorkItem = 1 Then
                                             If (DocTypeList.Count > 1) Then
                                                 For i = 0 To DocTypeList.Count - 1
                                                     If (DocTypeList.Item(i).FILEPATH <> "") Then
                                                         qry = "delete from ezca_" & cabinetid.ToString & "_" & invitaAPIobj.TemplateId & "_stage where ifilepath like '%" & sessionid.ToString & "%' and ifilename='" & DocTypeList.Item(i).FILEPATH.ToString & "'"
                                                         res = invitaAPIobj.InsertAndUpdateAndDeleteeZUserDefinedWithScope(qry)
                                                         If (File.Exists(DocTypeList.Item(i).FILEPATH.ToString)) Then
                                                             File.Delete(DocTypeList.Item(i).FILEPATH.ToString)
                                                         End If
                                                         DocTypeList(gridSelectedRowNumber).FILEPATH = ""
                                                         DocTypeList(gridSelectedRowNumber).SCAN = "Scan"
                                                         DocTypeList(gridSelectedRowNumber).RESCAN = "Rescan"
                                                         gridSubmittedDocuments.Items.Refresh()
                                                         ECMViewer._documentViewer.SetDocument(Nothing)
                                                         ECMViewerToolbar.IsEnabled = False
                                                     End If
                                                 Next
                                             End If


                                             'txtAccountNo.IsEnabled = True
                                             'txtAccountNo.Text = ""
                                             'txtWorkItemNo.Text = ""
                                             'lblStatusbar.Text = ""
                                             'txtWorkItemNo.Text = ""
                                             'gridAccountDetails.ItemsSource = Nothing
                                             'gridSubmittedDocuments.ItemsSource = Nothing
                                             'btnINITIATESCANNING.IsEnabled = False
                                             'cbProductList.ItemsSource = Nothing
                                             'cbProductList.IsEnabled = False
                                             ''cbProductList.IsEnabled = False
                                             '' cbProductList.SelectedValue = "Select Product"
                                             'cbPhase.ItemsSource = Nothing
                                             ''cbDocumentType.Items.Clear()
                                             ''cbDocumentType.ItemsSource = Nothing
                                             'cbPhase.IsEnabled = False
                                             'ECMViewer._documentViewer.SetDocument(Nothing)
                                             'ECMViewerToolbar.IsEnabled = False

                                             'LblFileName.Content = ""

                                             '_currentFileName = ""
                                             'txtwir.Text = ""
                                             'btnSubmit.IsEnabled = False
                                             'btnRescan.IsEnabled = False
                                             'btnClearRightPane.IsEnabled = False
                                             'btnFinalSubmit.IsEnabled = False

                                             'submittedDocNos = ""
                                             'txtSubmittedWorkItems.Text = ""

                                             'DocTypeList.Clear()
                                             'sessionid = ""
                                         End If
                                     End If

                                 End Sub)

        Catch ex As Exception
            MsgBox("Exception in clearAllvalues " + ex.Message)
        End Try
    End Sub


End Class
