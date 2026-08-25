Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Configuration
Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Resources
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Media.Animation
Imports eZLicence
Imports ezofis
Imports ezofis.Scan
Imports ezofis.UserControl
Imports ezofis.UserControl.CAC
Imports Leadtools
Imports Leadtools.Codecs
Imports Leadtools.Forms
Imports Leadtools.Forms.DocumentWriters
Imports Leadtools.Forms.Ocr
Imports Leadtools.Twain
Imports Leadtools.WinForms
Imports Telerik.Windows
Imports Telerik.Windows.Controls


Partial Public Class MainWindow
#Region "Variables"
    Dim Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("Database"), NameValueCollection)
    Dim imaging As String
    Dim _currentFileName As String
    Dim fl As Integer
    Dim fns(20) As String
    Public _bitsPerPixel As Integer
    Public _fileFormat As RasterImageFormat
    Dim Tiflist As List(Of String)
    Dim Otherlist As List(Of String)
    Dim reminderAnim As Storyboard
    Dim worker As New System.ComponentModel.BackgroundWorker()
    Dim FieldWord() As String
    Dim StrFolderName As String = ""
    Dim CAC As New CACserviceClient
    Dim msg As New Licence()
    Dim own As New Window
    Dim colordict As New Dictionary(Of String, String)
    Dim selectpagelst As New List(Of String)
    Dim selectpageleftpaneindex As Integer
    Dim isMultiplePageDeleteAllowed As Boolean = False

#End Region
#Region "Culture"
    Private Sub setCulture(ByVal Lang As String)
        Try
            Dim culture As CultureInfo = CultureInfo.CreateSpecificCulture(Lang)
            Dim rm As New ResourceManager("Capture.Main", GetType(MainWindow).Assembly)
            Me.htab.Header = rm.GetString("Home", culture)
            Me.settab.Header = rm.GetString("Settings", culture)
            Me.RadRibGrpScan.Header = rm.GetString("Scan", culture)
            Me.btnsca.Text = rm.GetString("Scan", culture)
            Me.btnref.Text = rm.GetString("Refresh", culture)
            Me.btnimp.Text = rm.GetString("Import", culture)
            Me.RadRibGrpMerge.Header = rm.GetString("Merge", culture)
            Me.btnmer.Text = rm.GetString("Merge", culture)
            Me.RadRibGrpSplitpg.Header = rm.GetString("Split Page", culture)
            Me.btnsin.Text = rm.GetString("Single", culture)
            Me.btncur.Text = rm.GetString("Current", culture)
            Me.btnbla.Text = rm.GetString("Black", culture)
            Me.btnbar.Text = rm.GetString("Barcode", culture)
            Me.RadRibGrpPg.Header = rm.GetString("Page", culture)
            Me.btnpgo.Text = rm.GetString("Save", culture)
            Me.btnpgc.Text = rm.GetString("Delete", culture)
            Me.btnrol.Text = rm.GetString("Left", culture)
            Me.btnrl.Text = rm.GetString("Right", culture)
            Me.RadRibGrpView.Header = rm.GetString("View", culture)
            Me.btnzoi.Text = rm.GetString("ZoomIn", culture)
            Me.btnzoo.Text = rm.GetString("ZoomOut", culture)
            Me.btnpan.Text = rm.GetString("Pan", culture)
            Me.btnSelectZoom.Text = rm.GetString("Zoom", culture)
            Me.btnlzo.Text = rm.GetString("LoadZone", culture)
            Me.RadRibGrpIndex.Header = rm.GetString("EzIndex", culture)
            Me.btnmag.Text = rm.GetString("Magnifier", culture)
            Me.btnfre.Text = rm.GetString("Free", culture)
            Me.RadRibGrpRecord.Header = rm.GetString("Record", culture)
            Me.btnrec.Text = rm.GetString("Save", culture)
            Me.btnrcl.Text = rm.GetString("Delete", culture)
            Me.btnsan.Text = rm.GetString("Scan", culture)
            Me.btnezc.Text = rm.GetString("EzConfig", culture)
            Me.RadRibGrpSet.Header = rm.GetString("Settings", culture)
            Me.AboutUs_Back.Header = rm.GetString("About Us", culture)
            Me.Exit_BackStage.Header = rm.GetString("Exit", culture)
            Me.Title = rm.GetString("ECM - Capture", culture)
            Me.RadRibbonView1.ApplicationName = rm.GetString("ECM Capture", culture)
            Me.stalab.Text = rm.GetString("Page", culture)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub Arabiclang_Selected(ByVal sender As Object, ByVal e As EventArgs)
        Try
            setCulture("ar")
            RightPane.SetCulture("ar")
            LeftPane.SetCulture("ar")
            ddlang.IsOpen = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub Englishlang_Selected(ByVal sender As Object, ByVal e As EventArgs)
        setCulture("")
        RightPane.SetCulture("")
        LeftPane.SetCulture("")
        ddlang.IsOpen = False
    End Sub
    Private Sub Frenchlang_Selected(ByVal sender As Object, ByVal e As EventArgs)
        setCulture("fr")
        RightPane.SetCulture("fr")
        LeftPane.SetCulture("fr")
        ddlang.IsOpen = False
    End Sub
    Private Sub Japanlang_Selected(ByVal sender As Object, ByVal e As EventArgs)
        setCulture("ja")
        RightPane.SetCulture("ja")
        LeftPane.SetCulture("ja")
        ddlang.IsOpen = False
    End Sub
    Private Sub Russianlang_Selected(ByVal sender As Object, ByVal e As EventArgs)
        setCulture("ru")
        RightPane.SetCulture("ru")
        LeftPane.SetCulture("ru")
        ddlang.IsOpen = False
    End Sub
    Private Sub Spanishlang_Selected(ByVal sender As Object, ByVal e As EventArgs)
        setCulture("es")
        RightPane.SetCulture("es")
        LeftPane.SetCulture("es")
        ddlang.IsOpen = False
    End Sub
    Private Sub Germanlang_Selected(ByVal sender As Object, ByVal e As EventArgs)
        setCulture("de")
        RightPane.SetCulture("de")
        LeftPane.SetCulture("de")
        ddlang.IsOpen = False
    End Sub
    Private Sub polishlang_Selected(ByVal sender As Object, ByVal e As EventArgs)
        setCulture("pl")
        RightPane.SetCulture("pl")
        LeftPane.SetCulture("pl")
        ddlang.IsOpen = False
    End Sub
    Private Sub Italianlang_Selected(ByVal sender As Object, ByVal e As EventArgs)
        setCulture("it")
        RightPane.SetCulture("it")
        LeftPane.SetCulture("it")
        ddlang.IsOpen = False
    End Sub
#End Region
    Enum FileFormets
        CcittGroup4 = RasterImageFormat.CcittGroup4
        'TifJpeg = RasterImageFormat.TifJpeg
        TifJpeg = RasterImageFormat.TifJpeg422
    End Enum
    Public Sub New()
        Try
            Me.InitializeComponent()
            'pan.MinHeight = Appcon("panminheight")
            'pan.Height = Appcon("panminheight")
            Support.Unlock(False)
            If ScanDocument.StartEngine() = False Then
                Dim i As Integer = 0
            End If
            ScreenTips()
            Dim Login As New LoginForm()
            If Login.ShowDialog() Then
                RightPane.CreateOnId = ecmlogin.ECMLoginId

                RightPane.LoadCabinets()
            Else
                Application.Current.Shutdown()
            End If
        Catch e As Exception
            MsgBox(e.ToString)
        End Try
    End Sub
    Public Function ScreenTips()
        Dim tip1 As New ToolTip()
        ' tip1.CaptionLabel.Text = "Scan"
        tip1.Content = "To Scan the Documents"
        Me.btnsca.ToolTip = tip1
        Dim tip2 As New ToolTip()
        '  tip2.CaptionLabel.Text = "Refresh"
        tip2.Content = "To Refresh Scanned Documents"
        Me.btnref.ToolTip = tip2
        Dim tip3 As New ToolTip()
        '  tip3.CaptionLabel.Text = "Import"
        tip3.Content = "To Import Documents From The System"
        Me.btnimp.ToolTip = tip3
        Dim tip4 As New ToolTip()
        ' tip4.CaptionLabel.Text = "Merge"
        tip4.Content = "To Merge Checked Documents"
        Me.btnmer.ToolTip = tip4
        Dim tip5 As New ToolTip()
        '  tip5.CaptionLabel.Text = "Single Pages"
        tip5.Content = "To Split Merged Documents to Single Pages"
        Me.btnsin.ToolTip = tip5
        Dim tip6 As New ToolTip()
        ' tip6.CaptionLabel.Text = "Currnet Pages"
        tip6.Content = "To Split Current Page Only"
        Me.btncur.ToolTip = tip6
        Dim tip7 As New ToolTip()
        '  tip7.CaptionLabel.Text = "Black Page"
        'tip7.Content = "To Split Black Pages"
        'mar18 by raja
        tip7.Content = "To Split Blank Pages"
        Me.btnbla.ToolTip = tip7
        Dim tip8 As New ToolTip()
        '  tip8.CaptionLabel.Text = "Rotate Left"
        tip8.Content = "Rotates Current Page To Left"
        Me.btnrol.ToolTip = tip8
        Dim tip9 As New ToolTip()
        '  tip9.CaptionLabel.Text = "First Page"
        tip9.Content = "Moves To First Page"
        Me.btnbk.ToolTip = tip9
        Dim tip10 As New ToolTip()
        '  tip10.CaptionLabel.Text = "Previous Page"
        tip10.Content = "Moves To Previous Page (Ctrl+P)"
        Me.btnbk1.ToolTip = tip10
        Dim tip11 As New ToolTip()
        ' tip11.CaptionLabel.Text = "Next Page"
        tip11.Content = "Moves To Next Page (Ctrl+N)"
        Me.btnfr.ToolTip = tip11
        Dim tip12 As New ToolTip()
        '  tip12.CaptionLabel.Text = "Last Page"
        tip12.Content = "Moves To Last Page"
        Me.btnfr1.ToolTip = tip12
        Dim tip13 As New ToolTip()
        '  tip13.CaptionLabel.Text = "Save Page"
        tip13.Content = "Saves Current Page"
        Me.btnpgo.ToolTip = tip13
        Dim tip14 As New ToolTip()
        '  tip14.CaptionLabel.Text = "Delete Page"
        tip14.Content = "Deletes Current Page"
        Me.btnpgc.ToolTip = tip14
        Dim tip15 As New ToolTip()
        '  tip15.CaptionLabel.Text = "Rotate Right"
        tip15.Content = "Rotates Current Page To Right"
        Me.btnrl.ToolTip = tip15
        Dim tip16 As New ToolTip()
        '  tip16.CaptionLabel.Text = "Zoom In"
        tip16.Content = "Zoom In Current Page"
        Me.btnzoi.ToolTip = tip16
        Dim tip17 As New ToolTip()
        ' tip17.CaptionLabel.Text = "Fit To Height"
        tip17.Content = "Fits the Page Height With The Viewer"
        Me.btnhei.ToolTip = tip17
        Dim tip18 As New ToolTip()
        ' tip18.CaptionLabel.Text = "Fit To Width"
        tip18.Content = "Fits the Page Width With The Viewer"
        Me.btnwid.ToolTip = tip18
        Dim tip19 As New ToolTip()
        ' tip19.CaptionLabel.Text = "Fit"
        tip19.Content = "Fits the Page With The Viewer"
        Me.btnfit.ToolTip = tip19
        Dim tip30 As New ToolTip()
        '   tip30.CaptionLabel.Text = "Pan Mode"
        tip30.Content = "Enable Scrolling Image After Zooming"
        Me.btnpan.ToolTip = tip30
        Dim tip31 As New ToolTip()
        ' tip31.CaptionLabel.Text = "Zoom Mode"
        tip31.Content = "Enable Zoom To Particular Portion of the Image"
        Me.btnSelectZoom.ToolTip = tip31
        Dim tip20 As New ToolTip()
        ' tip20.CaptionLabel.Text = "Zoom Out"
        tip20.Content = "Zoom Out Current Page"
        Me.btnzoo.ToolTip = tip20
        Dim tip21 As New ToolTip()
        '   tip21.CaptionLabel.Text = "Magnify"
        tip21.Content = "Magnifies Along The Cursor"
        Me.btnmag.ToolTip = tip21
        Dim tip22 As New ToolTip()
        '   tip22.CaptionLabel.Text = "Free Hand"
        tip22.Content = "To Enable Free Handling"
        Me.btnfre.ToolTip = tip22
        Dim tip23 As New ToolTip()
        '  tip23.CaptionLabel.Text = "First Record"
        tip23.Content = "Moves To First Record of The List"
        Me.firstrec.ToolTip = tip23
        Dim tip24 As New ToolTip()
        '  tip24.CaptionLabel.Text = "Previous Record"
        tip24.Content = "Moves To Previous Record of the List (Alt+P)"
        Me.prerec.ToolTip = tip24
        Dim tip25 As New ToolTip()
        '  tip25.CaptionLabel.Text = "Next Record"
        tip25.Content = "Moves To Next Record of The List (Alt+N)"
        Me.nextrec.ToolTip = tip25
        Dim tip26 As New ToolTip()
        ' tip26.CaptionLabel.Text = "Last Record"
        tip26.Content = "Moves To Last Record of The List"
        Me.lastrec.ToolTip = tip26
        Dim tip27 As New ToolTip()
        ' tip27.CaptionLabel.Text = "Save Record"
        tip27.Content = "Saves Current Record"
        Me.btnrec.ToolTip = tip27
        Dim tip28 As New ToolTip()
        '  tip28.CaptionLabel.Text = "Delete Record"
        tip28.Content = "Deletes Current Record"
        Me.btnrcl.ToolTip = tip28
        Dim tip29 As New ToolTip()
        ' tip29.CaptionLabel.Text = "Export"
        tip29.Content = "Exports Documents which Are Indexed"
        Dim tip32 As New ToolTip()
        '  tip32.CaptionLabel.Text = "Scan Settings"
        tip32.Content = "Adjust Settings for Scan"
        Me.btnsan.ToolTip = tip32
        'Dim tip33 As New ToolTip()
        ''  tip33.CaptionLabel.Text = "New Template"
        'tip33.Content = "Select Scan Type"
        'Me.btnpapsav.ToolTip = tip33
        Dim tip34 As New ToolTip()
        ' tip34.CaptionLabel.Text = "DB Connect"
        tip34.Content = "Connects to ezofis"
        Me.btnezc.ToolTip = tip34
        Dim tip35 As New ToolTip()
        '  tip32.CaptionLabel.Text = "Scan Settings"
        tip35.Content = "Start QC Check"
        Dim tip36 As New ToolTip()
        '  tip33.CaptionLabel.Text = "New Template"
        tip36.Content = "Pause QC Checking"
        Dim tip37 As New ToolTip()
        ' tip34.CaptionLabel.Text = "DB Connect"
        tip37.Content = "Stop QC Checking"
        'Dim tip38 As New ToolTip()
        '' tip34.CaptionLabel.Text = "DB Connect"
        'tip38.Content = "Reject Document"
        'Me.btnexpStop.ToolTip = tip38
    End Function
    Public Sub isTif()
        If LeftPane.lstTif.SelectedItems.Count > 0 Then
            RadRibGrpMerge.IsEnabled = True
            RadRibGrpSplitpg.IsEnabled = True
            pagenavgrp.IsEnabled = True
            RadRibGrpPg.IsEnabled = True
            RadRibGrpView.IsEnabled = True
            RadRibGrpIndex.IsEnabled = True
            RecordNav.IsEnabled = True
        Else
            RadRibGrpMerge.IsEnabled = False
            RadRibGrpSplitpg.IsEnabled = False
        End If
    End Sub

    Public Sub isPdf()
        If LeftPane.lstOther.SelectedItems.Count > 0 Then
            RadRibGrpMerge.IsEnabled = False
            RadRibGrpSplitpg.IsEnabled = False
            pagenavgrp.IsEnabled = False
            RadRibGrpPg.IsEnabled = False
            RadRibGrpView.IsEnabled = False
            RadRibGrpIndex.IsEnabled = False
            RecordNav.IsEnabled = False
        Else
            RadRibGrpMerge.IsEnabled = True
            RadRibGrpSplitpg.IsEnabled = True
            pagenavgrp.IsEnabled = True
            RadRibGrpPg.IsEnabled = True
            RadRibGrpView.IsEnabled = True
            RadRibGrpIndex.IsEnabled = True
            RecordNav.IsEnabled = True
        End If
    End Sub
    Private Sub MainWindow_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Try
            'If _newtwainSession IsNot Nothing Then
            '    _newtwainSession.Shutdown()
            'End If
            If ScanDocument._twainSession IsNot Nothing Then
                ScanDocument._twainSession.Shutdown()
            End If
        Catch ex As Exception
        End Try
        Application.Current.Shutdown()
    End Sub
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try
            ' checklicense()
            ezofis.UserControl.BarCodeTypeFromCmb = Decrypt(Appcon(Encrypt("BarCodeType")))
            imaging = Appcon("Imagefiles").ToString
            Fileformet = Appcon("FileFormet").ToString
            btnindex.IsEnabled = False
            Dim ds As DataSet
            Dim qry As String = "select * from ezecmcontrollevel where ECMProfileId in (select ECMProfileId from eZECMLogin where ECMLoginId=" + ecmlogin.ECMLoginId.ToString() + ")  and ecmcontrolid in (select ecmcontrolid from eZECMControl where ECMControl='Add Field Values')"
            ds = CAC.GetDatasetByQuery(qry)
            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        btnindex.IsEnabled = True
                    End If
                End If
            End If
            If Fileformet.ToUpper = "BW-CCITTGROUP4" Then
                _fileFormat = RasterImageFormat.CcittGroup4
                _bitsPerPixel = 1
                LeftPane._fileFormat = RasterImageFormat.CcittGroup4
                LeftPane._bitsPerPixel = 1
                btnbla.Text = "B/W"
                btnimgfrmt.SmallImage = New BitmapImage(New Uri("pack://application:,,,/Images/blackpg.png", UriKind.Absolute))
                btnimgfrmt.LargeImage = New BitmapImage(New Uri("pack://application:,,,/Images/blackpg.png", UriKind.Absolute))
            ElseIf Fileformet.ToUpper = "C-TIFLZW" Then
                _fileFormat = RasterImageFormat.TifLzw
                _bitsPerPixel = Appcon("bpp").ToString
                LeftPane._fileFormat = RasterImageFormat.TifLzw
                LeftPane._bitsPerPixel = Appcon("bpp").ToString
                btnbla.Text = "B/W"
            Else
                _fileFormat = RasterImageFormat.TifJpeg422
                _bitsPerPixel = 24
                LeftPane._fileFormat = RasterImageFormat.TifJpeg422
                LeftPane._bitsPerPixel = 24
                btnbla.Text = "B/W"
                btnimgfrmt.SmallImage = New BitmapImage(New Uri("pack://application:,,,/Images/colorpg.png", UriKind.Absolute))
                btnimgfrmt.LargeImage = New BitmapImage(New Uri("pack://application:,,,/Images/colorpg.png", UriKind.Absolute))
            End If
            ECMLeftPane.Imaging = Appcon("Imagefiles").ToString
            RightPane.Imaging = Appcon("Imagefiles").ToString
            isTif()
            BackGroundWorkerAssign()
            AddLanguage()
            Me.radcolor.[AddHandler](RadMenuItem.ClickEvent, New RoutedEventHandler(AddressOf ColorChangeClick))
            keyevent()
        Catch ex As Exception
            MsgBox("Error From Load : " + ex.ToString)
        Finally
        End Try
    End Sub
    Public Sub checklicense()
        Try
            If Appcon(Encrypt("License")) = 1 Then
                If Lic.IsCanRunWithEncrypt() = True Then
                    If Lic.canrun <> 1 Then
                        msg.ShowDialog()
                    End If
                Else
                    If Lic.canrun = 4 Then
                        msg.ShowDialog()
                        Application.Current.Shutdown()
                    ElseIf Lic.canrun = 3 Then
                        msg.ShowDialog()
                        Application.Current.Shutdown()
                    Else
                        msg.ShowDialog()
                        Application.Current.Shutdown()
                    End If
                End If
            Else
                Dim status As String = ""
                Dim ecmlic As New WindowsApplication1.ECMLicense
                Dim licensed = ecmlic.CheckLicense("ezofis Scan and Index", status)
                If IsNumeric(licensed) Then
                    If licensed = "0" Or licensed = "2" Then
                        MsgBox(status)
                        Application.Current.Shutdown()
                    Else
                        If licensed <> "1" Then
                            MsgBox(status)
                        End If
                    End If
                Else
                    MsgBox("From License : " + status + " - " + licensed)
                    Application.Current.Shutdown()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            Application.Current.Shutdown()
        End Try
    End Sub
    Public Sub BackGroundWorkerAssign()
        'Dim savedo As New ComponentModel.DoWorkEventHandler(AddressOf SaveDelete_DoWork)
        'RemoveHandler SaveDelete.DoWork, savedo
        'AddHandler SaveDelete.DoWork, savedo
        'Dim saverun As New ComponentModel.RunWorkerCompletedEventHandler(AddressOf SaveDelete_RunWorkerCompleted)
        'RemoveHandler SaveDelete.RunWorkerCompleted, saverun
        'AddHandler SaveDelete.RunWorkerCompleted, saverun
        Dim workerdo As New ComponentModel.DoWorkEventHandler(AddressOf worker_DoWork)
        RemoveHandler worker.DoWork, workerdo
        AddHandler worker.DoWork, workerdo
        Dim workerrun As New ComponentModel.RunWorkerCompletedEventHandler(AddressOf worker_RunWorkerCompleted)
        RemoveHandler worker.RunWorkerCompleted, workerrun
        AddHandler worker.RunWorkerCompleted, workerrun
    End Sub
    Public Sub AddLanguage()
        Dim ListBox As New ListBox
        Dim Englishlang As New ListBoxItem()
        Englishlang.Content = "English"
        ListBox.Items.Add(Englishlang)
        Dim Arabiclang As New ListBoxItem()
        Arabiclang.Content = "Arabic"
        ListBox.Items.Add(Arabiclang)
        Dim Frenchlang As New ListBoxItem()
        Frenchlang.Content = "French"
        ListBox.Items.Add(Frenchlang)
        Dim Japanlang As New ListBoxItem()
        Japanlang.Content = "Japanese"
        ListBox.Items.Add(Japanlang)
        Dim Russianlang As New ListBoxItem()
        Russianlang.Content = "Russian"
        ListBox.Items.Add(Russianlang)
        Dim Spanishlang As New ListBoxItem()
        Spanishlang.Content = "Spanish"
        ListBox.Items.Add(Spanishlang)
        Dim Germanlang As New ListBoxItem()
        Germanlang.Content = "German"
        ListBox.Items.Add(Germanlang)
        Dim polishlang As New ListBoxItem()
        polishlang.Content = "Polish"
        ListBox.Items.Add(polishlang)
        Dim Italianlang As New ListBoxItem()
        Italianlang.Content = "Italian"
        ListBox.Items.Add(Italianlang)
        ddlang.DropDownContent = ListBox
        AddHandler Englishlang.Selected, AddressOf Englishlang_Selected
        AddHandler Arabiclang.Selected, AddressOf Arabiclang_Selected
        AddHandler Frenchlang.Selected, AddressOf Frenchlang_Selected
        AddHandler Japanlang.Selected, AddressOf Japanlang_Selected
        AddHandler Russianlang.Selected, AddressOf Russianlang_Selected
        AddHandler Spanishlang.Selected, AddressOf Spanishlang_Selected
        AddHandler Germanlang.Selected, AddressOf Germanlang_Selected
        AddHandler polishlang.Selected, AddressOf polishlang_Selected
        AddHandler Italianlang.Selected, AddressOf Italianlang_Selected
    End Sub
    Dim barcodesplit = False
    Private Sub btnbar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnbar.Click
        Try
            'Tiflist = New List(Of String)
            'For Each item As ListViewItem In LeftPane.lstTif.SelectedItems
            '    Tiflist.Add(imaging + "\" + item.ToString().Replace("System.Windows.Controls.ListViewItem: ", ""))
            'Next
            ''LeftPane.lstTif.SelectedItems.Clear()
            'RightPane.SplitWithBarcode(Tiflist, _fileFormat, _bitsPerPixel)
            'barcodesplit = True
            ''RightPane.SaveBarcodeValues(dt)
            '_currentFileName = ""
            'ECMViewer.Viewer.Image = Nothing
            'ECMLeftPane.Refresh()
            'RightPane.ClearFields()
            'barcodesplit = False
            ''Application.Current.Dispatcher.Invoke(DirectCast(Sub()
            ''                                                     _currentFileName = ""
            ''                                                     LeftPane.lstTif.SelectedItems.Clear()
            ''                                                     ECMViewer.Viewer.Image = Nothing
            ''                                                     ECMLeftPane.Refresh()
            ''                                                     RightPane.ClearFields()
            ''                                                 End Sub, Action))

            LeftPane.SplitWithBarcode()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub btnbartype_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnbartype.Click
        Try
            Dim BarMsg = New BarcodeTypeForm
            If BarMsg.ShowDialog() = True Then
                'LeftPane.SplitWithBarcode()
                'LeftPane.Refresh()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btnpgc_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnpgc.Click
        Try

            If Appcon("MultiplePageDeleteId") IsNot Nothing Then
                Dim templateIdforMultipageDel As String = Appcon("MultiplePageDeleteId").ToString()
                Dim templateId As String() = templateIdforMultipageDel.Split(","c).Select(Function(x) x.Trim()).ToArray()
                If templateId.Contains(ECMRightPane.templateid.ToString()) Then
                    isMultiplePageDeleteAllowed = True
                    'added by sara 
                    ' MessageBox.Show("Entered ismultipageDeleteallowed is true")
                End If
            End If
            If isMultiplePageDeleteAllowed Then
                progfor = "DeletePage"
                reminderAnim = TryCast(Me.Resources("PageProgress"), Storyboard)
                reminderAnim.Begin()
                ' MessageBox.Show("Entered set progfor is deletePage ismultiplagedeleteallowed is true")
                worker.RunWorkerAsync()
            Else
                Dim res = MessageBox.Show("Do you Want to Delete the Scanned page?", "Scan & Index", MessageBoxButton.YesNo, MessageBoxImage.Question)
                If res = MessageBoxResult.Yes Then
                    progfor = "DeletePage"
                    reminderAnim = TryCast(Me.Resources("PageProgress"), Storyboard)
                    reminderAnim.Begin()
                    worker.RunWorkerAsync()
                    'ECMViewer.DeleteTifPage(_currentFileName, _fileFormat, _bitsPerPixel)
                    'ECMViewer.NextPage(_currentFileName)
                    'ECMViewer.PreviousPage(_currentFileName)
                    'stalab.Text = "Page " + ECMViewer.pageNumber.ToString() + " of " + ECMViewer.pageCount.ToString()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnrol_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnrol.Click
        Try
            ECMViewer.RotateLeft()
        Catch
        End Try
    End Sub
    Private Sub btnrl_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnrl.Click
        Try
            ECMViewer.RotateRight()
        Catch
        End Try
    End Sub
    Private Sub btnzoi_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnzoi.Click
        Try
            ECMViewer.ZoomIn()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btnzoo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnzoo.Click
        Try
            ECMViewer.ZoomOut()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btnhei_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnhei.Click
        ECMViewer.Stretch()
    End Sub
    Private Sub btnfit_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnfit.Click
        ECMViewer.FitAlways()
    End Sub
    Private Sub btnwid_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnwid.Click
        Try
            ECMViewer.FitWidth()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub firstrec_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles firstrec.Click
        RightPane.SaveRecords(_currentFileName)
        Try
            If ECMRightPane.templateid <> 0 Then
                Dim list = DirectCast(LeftPane.lstTif.SelectedItem, ListViewItem)
                If CheckIndexing(_currentFileName, ECMRightPane.templateid.ToString()) Then
                    list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                Else
                    list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                End If
            End If
        Catch ex As Exception
        End Try
        LeftPane.FirstRecord()
    End Sub
    Private Sub prerec_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles prerec.Click
        RightPane.SaveRecords(_currentFileName)
        Try
            If ECMRightPane.templateid <> 0 Then
                Dim list = DirectCast(LeftPane.lstTif.SelectedItem, ListViewItem)
                If CheckIndexing(_currentFileName, ECMRightPane.templateid.ToString()) Then
                    list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                Else
                    list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                End If
            End If
        Catch ex As Exception
        End Try
        LeftPane.PreviousRecord()
    End Sub
    Private Sub nextrec_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles nextrec.Click
        Try
            RightPane.SaveRecords(_currentFileName)
            Try
                If ECMRightPane.templateid <> 0 Then
                    Dim list = DirectCast(LeftPane.lstTif.SelectedItem, ListViewItem)
                    If CheckIndexing(_currentFileName, ECMRightPane.templateid.ToString()) Then
                        list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                    Else
                        list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                    End If
                End If
            Catch ex As Exception
            End Try
            LeftPane.NextRecord()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub lastrec_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles lastrec.Click
        RightPane.SaveRecords(_currentFileName)
        Try
            If ECMRightPane.templateid <> 0 Then
                Dim list = DirectCast(LeftPane.lstTif.SelectedItem, ListViewItem)
                If CheckIndexing(_currentFileName, ECMRightPane.templateid.ToString()) Then
                    list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                Else
                    list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                End If
            End If
        Catch ex As Exception
        End Try
        LeftPane.LastRecord()
    End Sub
    Private Sub btnmag_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnmag.Click
        ECMViewer.MagnifyGlass()
    End Sub
    Private Sub btnbk_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnbk.Click
        Try
            ECMViewer.FirstPage(_currentFileName)
            stalab.Text = "Page " + ECMViewer.pageNumber.ToString() + " of " + ECMViewer.pageCount.ToString()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub btnbk1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnbk1.Click
        Try
            ECMViewer.PreviousPage(_currentFileName)
            stalab.Text = "Page " + ECMViewer.pageNumber.ToString() + " of " + ECMViewer.pageCount.ToString()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub btnfr_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnfr.Click
        Try
            ECMViewer.NextPage(_currentFileName)
            stalab.Text = "Page " + ECMViewer.pageNumber.ToString() + " of " + ECMViewer.pageCount.ToString()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnfr1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnfr1.Click
        Try
            ECMViewer.LastPage(_currentFileName)
            stalab.Text = "Page " + ECMViewer.pageNumber.ToString() + " of " + ECMViewer.pageCount.ToString()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub btnref_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnref.Click
        ECMLeftPane.Refresh()
    End Sub
    Private Shared Function CompareDinosByLength(ByVal x As String, ByVal y As String) As Integer
        If x Is Nothing Then
            If y Is Nothing Then
                ' If x is Nothing and y is Nothing, they're
                ' equal.
                Return 0
            Else
                ' If x is Nothing and y is not Nothing, y
                ' is greater.
                Return -1
            End If
        Else
            ' If x is not Nothing...
            '
            If y Is Nothing Then
                ' ...and y is Nothing, x is greater.
                Return 1
            Else
                '...and y is not Nothing, compare the
                'lengths of the two strings.
                'Dim retval As Integer = x.CompareTo(y)
                Dim retval As Integer = x.Length.CompareTo(y.Length)
                If retval <> 0 Then
                    'If the strings are not of equal length,
                    'the longer string is greater.
                    Return retval
                Else
                    'If the strings are of equal length,
                    'sort them with ordinary string comparison.
                    Return x.CompareTo(y)
                End If
            End If
        End If
    End Function

    Private Sub btnmer_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnmer.Click
        Try
            'LeftPane.FileFormet = Appcon("FileFormet")
            LeftPane.MergeSelectedFiles()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btnpgo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnpgo.Click
        Try
            'ECMViewer.Viewer.Image = Nothing
            ' btnfr.IsEnabled = False
            progfor = "SavePage"

            reminderAnim = TryCast(Me.Resources("PageProgress"), Storyboard)
            reminderAnim.Begin()
            worker.RunWorkerAsync()
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString())
        Finally
            '  btnfr.IsEnabled = True
        End Try

    End Sub
    Dim fOwn As System.Windows.Forms.Form


    Private Sub btnsin_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnsin.Click
        Try
            Dim res = MessageBox.Show("Do you Want to Split with single page?", "Scan & Index", MessageBoxButton.YesNo, MessageBoxImage.Question)
            If res = MessageBoxResult.Yes Then
                LeftPane.SplitSinglePage()
            End If

        Catch ex As Exception
        End Try
    End Sub
    Private Sub btncur_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btncur.Click
        Try
            LeftPane.SplitCurrentPage(ECMViewer.pageNumber)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btnbla_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnbla.Click
        LeftPane.SplitBlankPage()
    End Sub

#Region "LeftPane"
    Private Sub LeftPane_Tif_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If ECMLeftPane.leftpaneselectioncall Then
                LeftPane.lstOther.SelectedItems.Clear()
                isTif()
                Dim _currentHighlightRect As RectangleF
                Try
                    If ECMViewer.Viewer.Image Is Nothing Then
                        If (LeftPane.lstTif.SelectedIndex <> -1 AndAlso Not barcodesplit) Then
                            Dim fnme As String
                            fnme = LeftPane.lstTif.SelectedItem().ToString.Replace("System.Windows.Controls.ListViewItem: ", "")
                            _currentFileName = imaging + "\" + fnme
                            RightPane.Getrecords(_currentFileName)
                            selectpageleftpaneindex = LeftPane.lstTif.SelectedIndex

                        End If
                    End If
                    If (LeftPane.lstTif.SelectedIndex <> -1 AndAlso Not barcodesplit) Then
                        RightPane.SaveRecords(_currentFileName)
                        _currentHighlightRect = Nothing
                        Dim fnme As String
                        fnme = LeftPane.lstTif.SelectedItem().ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
                        If fnme <> "" Then
                            _currentFileName = imaging + "\" + fnme
                            RightPane.CurrentFnInRightPane = _currentFileName
                            ECMViewer.pageNumber = 1
                            ECMViewer.PageChanges = New Dictionary(Of Integer, Integer)
                            ECMViewer.LoadSinglePage(_currentFileName)
                            ECMViewer.Viewer.Image.MakeRegionEmpty()
                            stalab.Text = "Page " + ECMViewer.pageNumber.ToString() + " of " + ECMViewer.pageCount.ToString()
                            ECMViewer.Viewer.ContextMenuStrip = Nothing
                            ECMViewer.Viewer.ContextMenuStrip = RightPane.GetContextMenu(ECMViewer.Viewer.Image)
                            LeftPane.lstTif.ContextMenu = Nothing
                            RightPane.Getrecords(_currentFileName)
                            RightPane.SaveRecords(_currentFileName)
                            RightPane.IndexingFieldEnabled(True)
                        End If
                    Else

                        RightPane.SaveRecords(_currentFileName)
                        ECMViewer.Viewer.Image = Nothing
                        ECMViewer.ContextMenu = Nothing
                        RightPane.ClearFields()
                        _currentFileName = ""
                        RightPane.IndexingFieldEnabled(False)
                    End If
                Catch ex As Exception
                    MsgBox(ex.ToString)
                Finally
                    RightPane.Getrecords(_currentFileName)
                    'shankar
                    If _currentFileName <> "" Then
                        If ECMRightPane.templateid <> 0 Then
                            'For i As Int16 = 0 To LeftPane.lstTif.SelectedItems.Count - 1
                            '    If CheckIndexing(LeftPane.lstTif.SelectedItems(i).ToString, ECMRightPane.templateid.ToString) Then
                            '        Try
                            '            Dim list = DirectCast(LeftPane.lstTif.SelectedItems(i), ListViewItem)
                            '            list.Foreground = New LinearGradientBrush(Colors.LightGreen, Colors.SlateBlue, 90)
                            '        Catch ex As Exception
                            '            MsgBox(ex.ToString)
                            '        End Try
                            '    End If
                            'Next
                            Dim list = DirectCast(LeftPane.lstTif.SelectedItem, ListViewItem)
                            If CheckIndexing(_currentFileName, ECMRightPane.templateid.ToString()) Then
                                list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                            Else
                                list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                            End If
                            'For Each item As String In LeftPane.lstOther.SelectedItems
                            '    CheckIndexing(item, ECMRightPane.templateid.ToString)
                            'Next
                        End If
                    End If
                End Try
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub
    Private Sub LeftPane_Tif_checkedChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            '  RightPane.SaveRecords(_currentFileName)
            If ECMRightPane.templateid <> 0 Then
                Dim list = DirectCast(LeftPane.lstTif.SelectedItem, ListViewItem)
                If list IsNot Nothing Then
                    If CheckIndexing(_currentFileName, ECMRightPane.templateid.ToString()) Then
                        list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                    Else
                        list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub LeftPane_Other_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        If ECMLeftPane.leftpaneselectioncall Then
            LeftPane.lstTif.SelectedItems.Clear()
            isPdf()

            Dim _currentHighlightRect As RectangleF
            Try
                If ECMViewer.Viewer.Image Is Nothing Then
                    If (LeftPane.lstTif.SelectedIndex <> -1) Then
                        Dim fnme As String
                        fnme = LeftPane.lstTif.SelectedItem().ToString.Replace("System.Windows.Controls.ListViewItem: ", "")
                        _currentFileName = imaging + "\" + fnme
                        RightPane.Getrecords(_currentFileName)
                    End If
                End If
                RightPane.SaveRecords(_currentFileName)
                If (LeftPane.lstOther.SelectedIndex <> -1) Then
                    _currentHighlightRect = Nothing
                    _currentFileName = imaging + "\" + LeftPane.lstOther.SelectedItem().ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
                    Dim TiffFileName As String = imaging + "\Temp\" & LeftPane.lstOther.SelectedItem().ToString().Replace("System.Windows.Controls.ListViewItem: ", "").ToString.Replace(Path.GetExtension(LeftPane.lstOther.SelectedItem().ToString.Replace("System.Windows.Controls.ListViewItem: ", "")), ".tiff")
                    ECMViewer.pageNumber = 1
                    ECMViewer.PageChanges = New Dictionary(Of Integer, Integer)
                    If _currentFileName.Substring(_currentFileName.Length - 3).ToLower = "pdf" Then
                        'If progfor <> "Export" Then

                        'End If
                        If LeftPane.lstOther.SelectedItems.Count = 1 Then

                            ECMViewer.LoadSinglePagepdf(_currentFileName)
                        End If


                        'If IO.File.Exists(TiffFileName) = True Then
                        '    ECMViewer.LoadSinglePage(TiffFileName)
                        'Else
                        '    LeftPane.pdfThumbnailview(_currentFileName, TiffFileName.Replace(".tiff", ""))
                        '    If IO.File.Exists(TiffFileName) = True Then
                        '        ECMViewer.LoadSinglePage(TiffFileName)
                        '    End If
                        'End If
                    ElseIf _currentFileName.Substring(_currentFileName.Length - 4).ToLower = "docx" Then
                        If IO.File.Exists(TiffFileName) = True Then
                            ECMViewer.LoadSinglePage(TiffFileName)
                        Else
                            LeftPane.docconverter(_currentFileName, LeftPane.lstOther.SelectedItem().ToString().Replace("System.Windows.Controls.ListViewItem: ", "").ToString(), "docx")
                            LeftPane.pdfThumbnailview(imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                            If IO.File.Exists(TiffFileName) = True Then
                                ECMViewer.LoadSinglePage(TiffFileName)
                            End If
                        End If
                    ElseIf _currentFileName.Substring(_currentFileName.Length - 3).ToLower = "doc" Then
                        If IO.File.Exists(TiffFileName) = True Then
                            ECMViewer.LoadSinglePage(TiffFileName)
                        Else
                            LeftPane.docconverter(_currentFileName, LeftPane.lstOther.SelectedItem().ToString().Replace("System.Windows.Controls.ListViewItem: ", "").ToString(), "doc")
                            LeftPane.pdfThumbnailview(imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                            If IO.File.Exists(TiffFileName) = True Then
                                ECMViewer.LoadSinglePage(TiffFileName)
                            End If
                        End If
                    ElseIf _currentFileName.Substring(_currentFileName.Length - 3).ToLower = "xls" Then
                        If IO.File.Exists(TiffFileName) = True Then
                        Else
                            LeftPane.docconverter(_currentFileName, LeftPane.lstOther.SelectedItem().ToString().Replace("System.Windows.Controls.ListViewItem: ", "").ToString(), "xls")
                            LeftPane.pdfThumbnailview(imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                            If IO.File.Exists(TiffFileName) = True Then
                                ECMViewer.LoadSinglePage(TiffFileName)
                            End If
                            ECMViewer.LoadSinglePage(TiffFileName)
                        End If
                    ElseIf _currentFileName.Substring(_currentFileName.Length - 4).ToLower = "xlsx" Then
                        If IO.File.Exists(TiffFileName) = True Then
                            ECMViewer.LoadSinglePage(TiffFileName)
                        Else
                            LeftPane.docconverter(_currentFileName, LeftPane.lstOther.SelectedItem().ToString().Replace("System.Windows.Controls.ListViewItem: ", "").ToString(), "xlsx")
                            LeftPane.pdfThumbnailview(imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                            If IO.File.Exists(TiffFileName) = True Then
                                ECMViewer.LoadSinglePage(TiffFileName)
                            End If
                        End If
                    ElseIf _currentFileName.Substring(_currentFileName.Length - 3).ToLower = "ppt" Then
                        If IO.File.Exists(TiffFileName) = True Then
                            ECMViewer.LoadSinglePage(TiffFileName)
                        Else
                            LeftPane.docconverter(_currentFileName, LeftPane.lstOther.SelectedItem().ToString().Replace("System.Windows.Controls.ListViewItem: ", "").ToString(), "ppt")
                            LeftPane.pdfThumbnailview(imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                            If IO.File.Exists(TiffFileName) = True Then
                                ECMViewer.LoadSinglePage(TiffFileName)
                            End If
                        End If
                    ElseIf _currentFileName.Substring(_currentFileName.Length - 3).ToLower = "jpg" Then
                        If IO.File.Exists(_currentFileName) = True Then
                            ECMViewer.LoadSinglePage(_currentFileName)
                            ECMViewer.Viewer.Image.MakeRegionEmpty()
                        Else
                        End If
                    ElseIf _currentFileName.Substring(_currentFileName.Length - 4).ToLower = "pptx" Then
                        If IO.File.Exists(TiffFileName) = True Then
                            ECMViewer.LoadSinglePage(TiffFileName)
                        Else
                            LeftPane.docconverter(_currentFileName, LeftPane.lstOther.SelectedItem().ToString().Replace("System.Windows.Controls.ListViewItem: ", "").ToString(), "pptx")
                            LeftPane.pdfThumbnailview(imaging + "\Convert_Pdf\Imaging.pdf", TiffFileName.Replace(".tiff", ""))
                            If IO.File.Exists(TiffFileName) = True Then
                                ECMViewer.LoadSinglePage(TiffFileName)
                            End If
                        End If
                    Else
                        If IO.File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\About\NoPreview.tif") = True Then
                            ECMViewer.LoadSinglePage(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\About\NoPreview.tif")
                        End If
                    End If
                    'If IO.File.Exists(imaging + "\Convert_Pdf\Imaging.pdf") = True Then System.IO.File.Delete(imaging + "\Convert_Pdf\Imaging.pdf")
                    'If Not _currentFileName = String.Empty Then
                    '    If Path.GetExtension(_currentFileName.ToString).ToLower <> ".tif" Or Path.GetExtension(_currentFileName.ToString).ToLower <> ".tiff" Then
                    '    End If
                    'End If
                    'stalab.Text = ""
                    stalab.Text = "Page " + ECMViewer.pageNumber.ToString() + " of " + ECMViewer.pageCount.ToString()
                    ECMViewer.Viewer.ContextMenuStrip = Nothing
                    ECMViewer.Viewer.ContextMenuStrip = RightPane.GetContextMenu(ECMViewer.Viewer.Image)
                    RightPane.Getrecords(_currentFileName)
                    RightPane.SaveRecords(_currentFileName)
                    RightPane.IndexingFieldEnabled(True)
                    'ECMViewer.DisposePdfViewerResources()
                    'ECMViewer.Viewer.Image.MakeRegionEmpty()

                Else
                    RightPane.SaveRecords(_currentFileName)
                    ECMViewer.Viewer.Image = Nothing
                    ECMViewer.Viewer.ContextMenuStrip = Nothing

                    isPdf()
                    'ECMViewer.DisposePdfViewerResources()
                    RightPane.ClearFields()
                    _currentFileName = ""
                    RightPane.IndexingFieldEnabled(False)
                End If


            Catch
            Finally
            End Try
            Try
                ' For changing the color 
                If ECMRightPane.templateid <> 0 Then
                    Dim list = DirectCast(LeftPane.lstOther.SelectedItem, ListViewItem)
                    If list IsNot Nothing Then
                        If CheckIndexing(_currentFileName, ECMRightPane.templateid.ToString()) Then
                            list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                        Else
                            list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub
    Public Function CheckIndexing(filename As String, templateid As String) As Boolean
        Try
            Dim sdataset As DataSet
            Dim fieldlst = New List(Of eZTemplateField)
            fieldlst = CAC.SelectedeZTemplateFieldList("TemplateId", templateid)
            sdataset = CAC.SelectedeZUserDefinedList(1, templateid, "*", "ifilename", filename)
            If sdataset IsNot Nothing Then
                If sdataset.Tables(0).Rows.Count > 0 Then
                    For i As Int16 = 0 To fieldlst.Count - 1
                        If sdataset.Tables(0).Rows(0).Item(fieldlst(i).FieldName.Trim()).ToString() = "" And fieldlst(i).Mandatory Then
                            Return False
                        End If
                    Next
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region
    Public Sub btnrec_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnrec.Click
        Try
            System.Windows.Forms.Application.EnableVisualStyles()
            System.Windows.Forms.Application.DoEvents()
            RightPane.SaveRecords(_currentFileName)
            Try
                If ECMRightPane.templateid <> 0 Then
                    Dim list = DirectCast(LeftPane.lstTif.SelectedItem, ListViewItem)
                    'If (LeftPane.lstTif.SelectedItem > 0) Then

                    If (LeftPane.lstTif.SelectedIndex <> -1) Then
                        If CheckIndexing(_currentFileName, ECMRightPane.templateid.ToString()) Then

                            list.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                        Else
                            list.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                        End If
                    End If

                    Dim otherfile = DirectCast(LeftPane.lstOther.SelectedItem, ListViewItem)
                    If (LeftPane.lstOther.SelectedIndex <> -1) Then
                        If CheckIndexing(_currentFileName, ECMRightPane.templateid.ToString()) Then
                            otherfile.Foreground = New LinearGradientBrush(Colors.OrangeRed, Colors.OrangeRed, 90)
                        Else
                            otherfile.Foreground = New LinearGradientBrush(Colors.Black, Colors.Black, 90)
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
            RightPane.Getrecords(_currentFileName)
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Message.ToString)

        End Try
    End Sub
    Private Sub btnrcl_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnrcl.Click
        Try
            Dim res = MessageBox.Show("Do you want to Delete the Scanned File?", "Scan & Index", MessageBoxButton.YesNo, MessageBoxImage.Question)
            If res = MessageBoxResult.Yes Then
                ' Ensure PdfViewer stops loading and clear the current document
                ECMViewer.DisposePdfViewerResources()
                ECMViewer.Viewer.Image = Nothing

                ' Delete files in LeftPane.lstTif
                For i As Int16 = 0 To LeftPane.lstTif.SelectedItems.Count - 1
                    Dim filePath = imaging + "\" + LeftPane.lstTif.SelectedItems(i).ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
                    If RightPane.DeleteRecords(filePath) Then
                        Kill(filePath)
                    End If
                Next

                ' Delete files in LeftPane.lstOther
                For i As Int16 = 0 To LeftPane.lstOther.SelectedItems.Count - 1
                    Dim filePath = imaging + "\" + LeftPane.lstOther.SelectedItems(i).ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
                    If RightPane.DeleteRecords(filePath) Then
                        Kill(filePath)
                    End If
                Next

                ECMLeftPane.Refresh()
                RightPane.loadcontrol()
            End If
        Catch ex As Exception
            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Dim progfor As String

    Public Sub CheckInvitaSplit(ByVal filename As String)
        Try
            If filename.Contains("#") Then
                Dim batchname = filename.Replace("DCMS11", "").Replace("DCMS12", "").Replace("DCMS12", "").Replace("DCMS6", "").Replace("DCMS7", "").Replace("DCMS8", "")
                Dim BatchId = batchname.Substring(0, 17).ToString()
                ' MessageBox.Show(BatchId)
                Dim ExportAtQuery = "select batchid from ezbatchfiles WITH (NOLOCK) where batchid like '%" + BatchId + "%'"
                Dim ScanedtYpeDS = CAC.GetDatasetByQuery(ExportAtQuery)
                If Not IsNothing(ScanedtYpeDS) AndAlso ScanedtYpeDS.Tables.Count > 0 AndAlso ScanedtYpeDS.Tables(0).Rows.Count > 0 Then
                    BatchId = ScanedtYpeDS.Tables(0).Rows(0)(0).ToString()
                    ' MessageBox.Show(BatchId)
                End If
                Dim QueryU = "insert into eZBatchFiles([Filename],[RIMNumber],[BatchId])values('" + filename + "','','" + BatchId + "')"
                If CAC.InsertAndUpdate(QueryU) > 0 Then

                End If
            Else
                Dim batchname = filename.Replace("DCMS11", "").Replace("DCMS12", "").Replace("DCMS12", "").Replace("DCMS6", "").Replace("DCMS7", "").Replace("DCMS8", "")
                Dim BatchId = batchname.Substring(0, 17).ToString()
                ' MessageBox.Show(BatchId)
                Dim ExportAtQuery = "select batchid from ezbatchfiles WITH (NOLOCK) where batchid like '%" + BatchId + "%'"
                Dim ScanedtYpeDS = CAC.GetDatasetByQuery(ExportAtQuery)
                If Not IsNothing(ScanedtYpeDS) AndAlso ScanedtYpeDS.Tables.Count > 0 AndAlso ScanedtYpeDS.Tables(0).Rows.Count > 0 Then
                    BatchId = ScanedtYpeDS.Tables(0).Rows(0)(0).ToString()
                    ExportAtQuery = "select batchid from ezbatchfiles WITH (NOLOCK) where batchid like '%" + BatchId + "%' and Filename='" + filename + "'"
                    ScanedtYpeDS = CAC.GetDatasetByQuery(ExportAtQuery)
                    If Not IsNothing(ScanedtYpeDS) AndAlso ScanedtYpeDS.Tables.Count > 0 AndAlso ScanedtYpeDS.Tables(0).Rows.Count = 0 Then
                        '  MessageBox.Show(BatchId)
                        Dim QueryU = "insert into eZBatchFiles([Filename],[RIMNumber],[BatchId])values('" + filename + "','','" + BatchId + "')"
                        If CAC.InsertAndUpdate(QueryU) > 0 Then

                        End If
                    End If

                End If
            End If

        Catch ex As Exception

        End Try
    End Sub




    Private Sub worker_DoWork(ByVal Sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Try
            Dim acct As AliasAccount
            Dim impersonate As Boolean = False
            Try
                If progfor = "Export" AndAlso Appcon("Username") <> "" Then
                    '  ECMViewer.DisposePdfViewerResources()
                    acct = New AliasAccount(Appcon("Username"), Appcon("Password"), Appcon("Domain"))
                    Try
                        acct.BeginImpersonation()
                        impersonate = True
                    Catch ex As Exception
                    End Try
                End If
                If progfor = "Export" Then
                    ' ECMViewer.DisposePdfViewerResources()
                    Dim count As Integer = 0
                    Dim notup As Integer = 0
                    Dim totcount As Integer = 0
                    Try
                        RightPane.LogFileName = Format(DateTime.Now, "MM-dd-yyyy hh-mm-ss")
                        RightPane.writetxtfle("Exporting Files Started  ", "")
                        RightPane.writetxtfle(Environment.NewLine, "")
                        Dim filelocation As String = RightPane.dir() & "\" & RightPane.LogFileName & ".csv"
                        'If Tiflist.Count > 0 Or Otherlist.Count > 0 Then
                        '    Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                        '        If unc.NetUseWithCredentials(Appcon("UNCpath"), Appcon("Username"), Appcon("Domain"), Appcon("Password")) Then
                        '            '  MessageBox.Show("Testing connection  ")
                        '        Else
                        '            MessageBox.Show("Please check the IP address, The Path " + Appcon("UNCpath") + " not Accessible now !")
                        '            Exit Sub
                        '        End If
                        '    End Using


                        'End If

                        For k As Integer = 0 To Tiflist.Count - 1
                            totcount = totcount + 1
                            LeftPane.lstTif.Dispatcher.Invoke(New Action(Sub() LeftPane.lstTif.SelectedItem = Tiflist(k).ToString()))
                            Dim str As String = Tiflist(k).ToString()
                            CheckInvitaSplit(str)

                            If RightPane.ExportToEngine(Tiflist(k).ToString()) = 1 Then
                                count = count + 1
                            Else
                                notup = notup + 1
                            End If
                            'If RightPane.ExportTifFiles(Tiflist(k).ToString()) = 1 Then
                            '    count = count + 1
                            'Else
                            '    notup = notup + 1
                            'End If
                        Next
                        ECMViewer.DisposePdfViewerResources()
                        For j As Integer = 0 To Otherlist.Count - 1
                            totcount = totcount + 1
                            '  ECMViewer.DisposePdfViewerResources()
                            If RightPane.ExportToEngine(Otherlist(j).ToString()) = 1 Then
                                count = count + 1
                            Else
                                notup = notup + 1
                            End If
                            'If RightPane.ExportOtherFiles(Otherlist(j).ToString()) = 1 Then
                            '    count = count + 1
                            'Else
                            '    notup = notup + 1
                            'End If
                        Next

                        RightPane.writetxtfle("Exporting Files Completed.", "")
                        RightPane.writetxtfle("Number of Files Exported : " & count.ToString() + " Out Of " + totcount.ToString, "")
                        Try
                            Dim txtnam As String = "_pg_.txt"
                            Dim filinf As System.IO.FileInfo
                            Dim dirinf As System.IO.DirectoryInfo
                            dirinf = New System.IO.DirectoryInfo(imaging + "\" + ECMRightPane.templateid.ToString())
                            For Each filinf In dirinf.GetFiles("*.txt")
                                'If filinf.Name.Contains("_pg_") Or filinf.Name.Contains("_pg_0.") Then
                                Kill(imaging + "\" + templateid.ToString() + "\" + filinf.Name)
                                'End If
                            Next
                            Dim qry = "delete from ezca_" + ECMRightPane.cabinetid.ToString + "_" + ECMRightPane.templateid.ToString + "_stage " +
                                "where ifilename in(" + RightPane.ifilenamelist.Substring(0, RightPane.ifilenamelist.Length - 1) + ")"
                            Dim res = CAC.InsertAndUpdateAndDeleteeZUserDefined(qry)
                        Catch ex As Exception

                        End Try
                        Application.Current.Dispatcher.Invoke(DirectCast(Sub()
                                                                             Dim msg As New Message(totcount, count, filelocation)
                                                                             msg.Show()
                                                                         End Sub, Action))
                        Application.Current.Dispatcher.Invoke(DirectCast(Sub()
                                                                             ECMViewer.Viewer.Image = Nothing
                                                                             ECMLeftPane.Refresh()
                                                                             RightPane.ClearFields()
                                                                         End Sub, Action))

                        'ECMLeftPane.Refresh()
                        'RightPane.ClearFields()
                    Catch ex As Exception
                    End Try
                ElseIf progfor = "Reject" Then
                    ' ECMLeftPane.SendToReject(Tiflist, Otherlist)
                ElseIf progfor = "Invoice" Then
                    'Dim query = "select * from ezzonal where templateid=" + ECMRightPane.templateid.ToString + " and createdfrom=N'" +
                    '    Environment.MachineName + "' and processname='ECM-Capture'"
                    'Dim ds = CAC.GetDatasetByQuery(query)
                    'If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    '    Dim eZERSInfoList As List(Of eZERSInfo) = CAC.SelectedeZERSInfoList("ERSId", "1")
                    '    Dim StrFile As String = ds.Tables(0).Rows(0)("zonalname").ToString
                    '    StrFolderName = eZERSInfoList(0).SettingPath & ECMRightPane.cabinetName & "\" & ECMRightPane.TemplateName & "\" + StrFile + ".ozf"
                    'Else
                    '    MsgBox("Please Select (Or) Create a Zonal.")
                    'End If
                    StrFolderName = ConfigSettings.loadConfigDocument().SelectSingleNode("//configuration//ZonalSettings//ZonalFilePath").Attributes("Value").Value.ToString.ToLower
                    If File.Exists(StrFolderName) Then
                        For k As Integer = 0 To Tiflist.Count - 1
                            Try
                                Dim str As String = Tiflist(k).ToString()
                                '_codecs = New RasterCodecs
                                'Dim infodd = _codecs.GetInformation(str, True)
                                'Dim singlepage As New List(Of String)
                                ''If infodd.TotalPages > 1 Then
                                ''    singlepage = SplitWithSinglePage(str, infodd, infodd.TotalPages, LeftPane._fileFormat, LeftPane._bitsPerPixel)
                                ''Else
                                'singlepage.Add(str)
                                'End If
                                FieldWord = Nothing
                                'For i As Integer = 0 To singlepage.Count - 1
                                _currentHighlightRect = Nothing
                                _currentFileName = str
                                'RightPane.CurrentFnInRightPane = _currentFileName
                                PageNumber = 1
                                ECMViewer.Viewer.BeginUpdate()
                                ECMViewer.LoadSinglePage(_currentFileName)
                                ECMViewer.Viewer.EndUpdate()
                                ECMViewer.Viewer.Invalidate()
                                ECMViewer.Viewer.Image.MakeRegionEmpty()
                                _OcrZonePage.Engine.DocumentManager.CreateDocument()
                                _OcrZonePage.Pages.Clear()
                                _OcrZonePage.Pages.AddPage(ECMViewer.Viewer.Image, Nothing)
                                _OcrZonePage.Pages(0).LoadZones(StrFolderName)
                                Application.Current.Dispatcher.Invoke(DirectCast(Sub()
                                                                                     ZonesUpdated(0)
                                                                                     RightPane.Btn_Click(Nothing, New System.Windows.RoutedEventArgs)
                                                                                     RightPane.SaveRecords(_currentFileName)
                                                                                     _currentFileName = ""
                                                                                     RightPane.ClearFields()
                                                                                 End Sub, Action))
                                'Next
                            Catch ex As Exception
                            End Try
                        Next
                        Application.Current.Dispatcher.Invoke(DirectCast(Sub()
                                                                             ECMViewer.Viewer.Image = Nothing
                                                                             ECMLeftPane.Refresh()
                                                                             _currentFileName = ""
                                                                             RightPane.ClearFields()
                                                                         End Sub, Action))


                    Else
                        MsgBox("Zonal File Not Found")
                    End If
                End If
                If progfor = "SavePage" Then
                    ECMViewer.SaveTifPage(_currentFileName, _fileFormat, _bitsPerPixel)
                End If
                If progfor = "DeletePage" Then
                    If isMultiplePageDeleteAllowed Then
                        If Appcon("DocumentviewerEXEPath") Is Nothing OrElse
           String.IsNullOrWhiteSpace(Appcon("DocumentviewerEXEPath").ToString()) Then
                            MessageBox.Show("DocumentviewerEXEPath is not configured in App.config.")
                            Return
                        End If
                        If Appcon("DocumentviewerEXEPath") IsNot Nothing Then
                            Dim viewerExe As String = Appcon("DocumentviewerEXEPath").ToString()
                            Dim documentPath As String = _currentFileName
                            If String.IsNullOrWhiteSpace(viewerExe) Then
                                MessageBox.Show("Document viewer EXE path is empty.")
                                Return
                            End If
                            If Not Path.IsPathRooted(viewerExe) Then
                                MessageBox.Show("Document viewer EXE path must be a full path:" & vbCrLf & viewerExe)
                                Return
                            End If
                            If Not File.Exists(viewerExe) Then
                                MessageBox.Show("documentViewer.exe not found at: " & viewerExe)
                                Return
                            End If
                            If String.IsNullOrWhiteSpace(documentPath) Then
                                MessageBox.Show("No document is currently loaded (_currentFileName is empty).")
                                Return
                            End If
                            If Not File.Exists(documentPath) Then
                                MessageBox.Show("Document file not found:" & vbCrLf & documentPath)
                                Return
                            End If
                            Try
                                Dim psi As New ProcessStartInfo With {
                                .FileName = viewerExe,
                                .Arguments = """" & documentPath & """",
                                .WorkingDirectory = Path.GetDirectoryName(viewerExe),
                                .UseShellExecute = True
                                }
                                Using p As Process = Process.Start(psi)
                                    If p Is Nothing Then
                                        MessageBox.Show("Process.Start returned Nothing." & vbCrLf & "EXE: " & viewerExe & vbCrLf & "Document: " & documentPath)
                                        Return
                                    End If
                                    p.WaitForExit()
                                    If p.ExitCode <> 0 Then
                                        MessageBox.Show("Document viewer closed with exitcode :" & p.ExitCode & "EXE" & vbCrLf & "Document: " & documentPath)
                                        Return
                                    End If
                                End Using
                                MessageBox.Show("Changes were applied successfully. The file list has been refreshed.")
                            Catch ex As Win32Exception
                                MessageBox.Show("Failed to start document viewer (Win32). " & vbCrLf & "NativeErrorCode: " & ex.NativeErrorCode & vbCrLf & "Message: " & ex.Message & vbCrLf & "EXE :" & viewerExe & vbCrLf & "Document: " & documentPath, "Error", MessageBoxButton.OK)
                            Catch ex As Exception
                                MessageBox.Show("Failed to Open document viewer" & vbCrLf & "Message: " & ex.Message & vbCrLf & "EXE: " & viewerExe & vbCrLf & "Document: " & documentPath, "Error", MessageBoxButton.OK)
                            End Try
                            '************old one *************************
                            '                        Using p = Process.Start(New ProcessStartInfo With {
                            '                            .FileName = viewerExe,
                            '                            .Arguments = $"""{documentPath}""",
                            '                            .UseShellExecute = True
                            '})
                            '                            p.WaitForExit() ' blocks until the user closes the viewer (Finish / X)
                            '                        End Using

                            '                        ' Runs only after DocumentViewer has exited
                            '                        MessageBox.Show("Changes were applied Successfully.The file list has been refreshed.")
                            '************************old one finishes*****************
                        Else
                            MessageBox.Show("The documentViewer.exe path is not configured.")
                        End If

                    Else
                        ECMViewer.DeleteTifPage(_currentFileName, _fileFormat, _bitsPerPixel)
                    End If

                    '  ECMViewer.DeletePage(_currentFileName, _fileFormat, _bitsPerPixel)
                    'ECMViewer.NextPage(_currentFileName)
                    'ECMViewer.PreviousPage(_currentFileName)
                    'stalab.Text = "Page " + ECMViewer.pageNumber.ToString() + " Of " + ECMViewer.pageCount.ToString()
                End If
                'ECMViewer.DisposePdfViewerResources()
            Finally
                If impersonate Then
                    acct.EndImpersonation()
                End If
            End Try
        Catch ex As Exception
            MessageBox.Show("Common Error  :" & ex.Message, "Error", MessageBoxButton.OK)
        End Try
    End Sub
    Private Sub worker_RunWorkerCompleted(ByVal Sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        reminderAnim.Stop()
        If progfor = "DeletePage" Then
            ' ECMViewer.DeleteTifPage(_currentFileName, _fileFormat, _bitsPerPixel)
            '  ECMViewer.DeletePage(_currentFileName, _fileFormat, _bitsPerPixel)
            ECMViewer.NextPage(_currentFileName)
            ECMViewer.PreviousPage(_currentFileName)
            stalab.Text = "Page " + ECMViewer.pageNumber.ToString() + " Of " + ECMViewer.pageCount.ToString()
        ElseIf progfor = "SavePage" Then
        Else
            LeftPane.Refresh()
        End If

    End Sub
    Private Sub btnezc_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnezc.Click
        Dim frm As New ServiceRef
        frm.Show()
    End Sub
    Private Sub btnpan_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnpan.Click
        ECMViewer.Pan()
    End Sub
    Private Sub btnSelectZoom_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSelectZoom.Click
        ECMViewer.ZoomTo()
    End Sub
    Private Sub btnfre_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnfre.Click
        ECMViewer.FreeHandling()
    End Sub
    Public Sub StartScan()
        Try
            SetTransferMode()
            ScanDocument.StartScan(imaging, _fileFormat, _bitsPerPixel)
            LeftPane.Refresh()
        Catch ex As Exception
        Finally
            btnsca.IsEnabled = True
            QuickScan.IsEnabled = True
        End Try
    End Sub
    Private Sub btnsca_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnsca.Click
        If _fileFormat <> RasterImageFormat.TifJpeg422 Then
            btnsca.IsEnabled = False
            QuickScan.IsEnabled = False
            StartScan()
            LeftPane.Refresh()
        Else
            MessageBox.Show("Color Settings Is choosed. Please Proceed With Scanner Button No.9 EZOFIS Color To Scan As Color." +
"(Or) Change the settings To B/W In Settings->Paper", "Scan Option", MessageBoxButton.OK, MessageBoxImage.Information)
        End If
    End Sub
    Private Sub btnsan_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnsan.Click
        'If _newtwainSession IsNot Nothing Then
        '    _newtwainSession.SelectSource(String.Empty)
        'End If
        If ScanDocument._twainSession IsNot Nothing Then
            ScanDocument._twainSession.SelectSource(String.Empty)
        End If
    End Sub
    Private Sub SetTransferMode()
        Dim twnCap As TwainCapability = New TwainCapability
        Dim _transferMode As TwainTransferMechanism = TwainTransferMechanism.Native
        Try
            twnCap.Information.Type = TwainCapabilityType.ImageTransferMechanism
            twnCap.Information.ContainerType = TwainContainerType.OneValue
            twnCap.OneValueCapability.ItemType = TwainItemType.Uint16
            twnCap.OneValueCapability.Value = CType(_transferMode, UInt16)
            ' Set the value of ICAP_XFERMECH (Image Transfer Mechanism) capability
            ScanDocument._twainSession.SetCapability(twnCap, TwainSetCapabilityMode.Set)
        Catch ex As Exception
            Dim errorMsg As String = String.Format("Error Set TwainCapabilityType.ImageTransferMechanism Is {0}", ex.Message)
            MsgBox(errorMsg)
        End Try
    End Sub
    Private Sub AboutUs_Back_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim filepath As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\About\about.pdf"
        Process.Start(filepath)
    End Sub
    Private Sub Exit_BackStage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.Close()
    End Sub
    Private Sub QuickRecord_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            System.Windows.Forms.Application.EnableVisualStyles()
            System.Windows.Forms.Application.DoEvents()
            RightPane.SaveRecords(_currentFileName)
            RightPane.Getrecords(_currentFileName)
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Private Sub QuickExport_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If Not worker.IsBusy Then
                RightPane.SaveRecords(_currentFileName)
                Tiflist = New List(Of String)
                Otherlist = New List(Of String)
                'LeftPane.lstTif.UnselectAll()
                'LeftPane.lstOther.UnselectAll()
                RightPane.ClearFields()
                For Each item As String In LeftPane.lstTif.SelectedItems
                    Tiflist.Add(item.Trim())
                Next
                For Each item As String In LeftPane.lstOther.SelectedItems
                    Otherlist.Add(item.Trim())
                Next
                RightPane.cabinetName = RightPane.ddlstcab.Text.ToString()
                RightPane.TemplateName = RightPane.ddlsttem.Text.ToString()
                reminderAnim = TryCast(Me.Resources("BeginProgress"), Storyboard)
                reminderAnim.Begin()
                worker.RunWorkerAsync()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub QuickScan_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        StartScan()
        LeftPane.Refresh()
    End Sub
    Public Sub SaveZones(ByVal StrZonalName As String)
        Try
            Dim eZERSInfoList As List(Of eZERSInfo) = CAC.SelectedeZERSInfoList("ERSId", "1")
            Dim StrFile As String = StrZonalName
            StrFolderName = eZERSInfoList(0).SettingPath & RightPane.ddlstcab.Text & "\" & RightPane.ddlsttem.Text & "\"
            If Not Directory.Exists(StrFolderName) Then
                Directory.CreateDirectory(StrFolderName)
            End If
            If File.Exists(StrFolderName & StrFile & ".ozf") Then
                StrFile = StrFile & Now.ToString.Replace("/", "-").Replace(":", "-").Replace("\", "-").Replace("|", "-")
                Dim ObjeZZonal As New eZZonal
                ObjeZZonal.CabinetId = ECMRightPane.cabinetid
                ObjeZZonal.TemplateId = ECMRightPane.templateid
                ObjeZZonal.ZonalName = StrFile
                ObjeZZonal.CreatedBy = ECMRightPane.CreateOnId
                ObjeZZonal.CreatedOn = CAC.DateDateTimeToString(DateTime.Now.ToString, 1)
                ObjeZZonal.ProcessName = "ECM-Capture"
                ObjeZZonal.CreatedFrom = Environment.MachineName
                Dim Result = CAC.InsertAndUpdateeZZonal(ObjeZZonal)
                If Result <> 0 Then
                    _OcrZonePage.Pages(0).SaveZones(StrFolderName & StrFile & ".ozf")
                    StrFolderName = StrFolderName & StrFile & ".ozf"
                    ConfigSettings.SaveZonalFilePath(StrFolderName)
                    MsgBox("Zone Saved Successfully..!", vbInformation)
                End If
            Else
                Dim ObjeZZonal As New eZZonal
                ObjeZZonal.CabinetId = ECMRightPane.cabinetid
                ObjeZZonal.TemplateId = ECMRightPane.templateid
                ObjeZZonal.ZonalName = StrFile
                ObjeZZonal.CreatedBy = ECMRightPane.CreateOnId
                ObjeZZonal.CreatedOn = CAC.DateDateTimeToString(DateTime.Now.ToString, 1)
                ObjeZZonal.ProcessName = "ECM-Capture"
                ObjeZZonal.CreatedFrom = Environment.MachineName
                Dim Result = CAC.InsertAndUpdateeZZonal(ObjeZZonal)
                If Result <> 0 Then
                    _OcrZonePage.Pages(0).SaveZones(StrFolderName & StrFile & ".ozf")
                    StrFolderName = StrFolderName & StrFile & ".ozf"
                    ConfigSettings.SaveZonalFilePath(StrFolderName)
                    MsgBox("Zone Saved Successfully..!", vbInformation)
                End If
            End If

        Catch ex As Exception
            MsgBox("Error From Save Zonal : " + ex.Message)
        End Try
    End Sub
    Private Sub btnpapsav_Click(sender As Object, e As RoutedEventArgs) Handles btnpapsav.Click
        Try
            Dim BarMsg = New PaperSettings
            If BarMsg.ShowDialog() = True Then
                If Fileformet = "BW-CCITTGROUP4" Then
                    _fileFormat = FileFormets.CcittGroup4
                    _bitsPerPixel = 1
                    LeftPane._fileFormat = FileFormets.CcittGroup4
                    LeftPane._bitsPerPixel = 1
                    btnbla.Text = "B/W"
                ElseIf Fileformet = "C-TIFLZW" Then
                    _fileFormat = RasterImageFormat.TifJpeg422
                    _bitsPerPixel = 24
                    LeftPane._fileFormat = RasterImageFormat.TifJpeg422
                    LeftPane._bitsPerPixel = 24
                    btnbla.Text = "B/W"
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub RadRibbonView1_MinimizedChanged(sender As Object, e As Telerik.Windows.RadRoutedEventArgs) Handles RadRibbonView1.MinimizedChanged
        Try
            If RadRibbonView1.IsMinimized = True Then
                pan.Height = Appcon("panheight4min")
            Else
                pan.Height = Appcon("panminheight")
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub CSVFileAppend(ByVal csvvalues As String)
        Try
            Dim loc = "C:\QCReport\Rejected"
            Dim filename As String = loc + "\" + DateTime.Now.ToString("ddMMMyyyy") + ".csv"
            If Not IO.Directory.Exists(loc) Then
                IO.Directory.CreateDirectory(loc)
            End If
            If Not IO.File.Exists(filename) Then
                Dim str(1) As String
                str(0) = "Barcode-Value,UserName,Time"
                File.WriteAllLines(filename, str)
            End If
            Dim exststr() As String = File.ReadAllLines(filename)
            Array.Resize(exststr, exststr.Length + 1)
            exststr(exststr.Length - 1) = csvvalues
            File.WriteAllLines(filename, exststr)
        Catch ex As Exception
        End Try
    End Sub
    Public Function Encrypt(plainText As String) As String
        '    Public Function Encrypt(plainText As String, passPhrase As String, saltValue As String, hashAlgorithm As String, passwordIterations As Integer, initVector As String, _
        'keySize As Integer) As String
        Dim passPhrase As String = "vairavaraj"
        Dim saltValue As String = "vairavaraj"
        Dim hashAlgorithm As String = "SHA1"
        Dim passwordIterations As Integer = 1
        Dim initVector As String = "@v#a5i%r&a7v&a#j"
        Dim keySize As Integer = 192
        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)
        Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
        Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
        Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)
        Dim symmetricKey As New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Dim encryptor As ICryptoTransform = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)
        Dim memoryStream As New MemoryStream()
        Dim cryptoStream As New CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)
        cryptoStream.FlushFinalBlock()
        Dim cipherTextBytes As Byte() = memoryStream.ToArray()
        memoryStream.Close()
        cryptoStream.Close()
        Dim cipherText As String = Convert.ToBase64String(cipherTextBytes)
        Return cipherText
    End Function
    Public Function Decrypt(cipherText As String) As String
        Dim passPhrase As String = "vairavaraj"
        Dim saltValue As String = "vairavaraj"
        Dim hashAlgorithm As String = "SHA1"
        Dim passwordIterations As Integer = 1
        Dim initVector As String = "@v#a5i%r&a7v&a#j"
        Dim keySize As Integer = 192
        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)
        Dim cipherTextBytes As Byte() = Convert.FromBase64String(cipherText)
        Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
        Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)
        Dim symmetricKey As New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Dim decryptor As ICryptoTransform = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)
        Dim memoryStream As New MemoryStream(cipherTextBytes)
        Dim cryptoStream As New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)
        Dim plainTextBytes As Byte() = New Byte(cipherTextBytes.Length - 1) {}
        Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)
        memoryStream.Close()
        cryptoStream.Close()
        Dim plainText As String = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)
        Return plainText
    End Function
    Private Sub btnexp_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not worker.IsBusy Then
                progfor = "Export"
                RightPane.SaveRecords(_currentFileName)
                Tiflist = New List(Of String)
                Otherlist = New List(Of String)

                RightPane.ClearFields()

                'Tiflist = New List(Of String)
                'For Each item As ListViewItem In LeftPane.lstTif.Items
                '    Tiflist.Add(item.ToString().Replace("System.Windows.Controls.ListViewItem: ", ""))
                'Next

                Tiflist = New List(Of String)
                For Each item As ListViewItem In LeftPane.lstTif.SelectedItems
                    Tiflist.Add(item.ToString().Replace("System.Windows.Controls.ListViewItem: ", ""))
                Next


                ' ECMViewer.DisposePdfViewerResources()
                Otherlist = New List(Of String)

                For Each item As ListViewItem In LeftPane.lstOther.Items
                    Otherlist.Add(item.ToString().Replace("System.Windows.Controls.ListViewItem: ", ""))

                Next


                'ECMViewer.DisposePdfViewerResources()

                ECMRightPane.cabinetName = RightPane.ddlstcab.Text.ToString()
                ECMRightPane.TemplateName = RightPane.ddlsttem.Text.ToString()
                reminderAnim = TryCast(Me.Resources("BeginProgress"), Storyboard)
                reminderAnim.Begin()
                worker.RunWorkerAsync()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub LoadPdfIntoViewer(filePath As String)
        ' Assuming LoadPdf is a method that loads the PDF document into the pdfPanel
        ECMViewer.LoadSinglePagepdf(filePath)
    End Sub

    'Private Sub LoadPdfIntoViewer(filePath As String)
    '    ' Assuming LoadPdf is a method that loads the PDF document into the pdfPanel
    '    ECMViewer.LoadPdf(filePath)
    'End Sub
    'Private Sub btnimp_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnimp.Click
    '    Dim filelist As New List(Of String)
    '    Dim open As New Microsoft.Win32.OpenFileDialog()
    '    open.Title = "Please Select a File"
    '    open.Filter = "All Files(*.*)|*.*"
    '    open.FileName = ""
    '    fl = open.FileName.Length
    '    fns = open.FileNames
    '    open.Multiselect = True
    '    open.InitialDirectory = Appcon("SplitFolder").ToString()
    '    Dim result As Nullable(Of Boolean) = open.ShowDialog()
    '    If (result = True) Then
    '        Try
    '            filelist = open.FileNames.ToList()
    '            filelist.Sort(AddressOf CompareDinosByLength)
    '            LeftPane.FilesLoad(filelist)
    '        Catch ex As Exception
    '            System.Windows.MessageBox.Show(ex.Message)
    '        Finally
    '        End Try
    '    End If
    'End Sub

    Private Sub btnimp1_Click(sender As Object, e As RoutedEventArgs) Handles btnimp1.Click
        Try
            Dim filelist As New List(Of String)
            Dim flst As New List(Of IndexingField)
            Dim open As New Microsoft.Win32.OpenFileDialog()
            open.Title = "Please Select a File"
            open.Filter = "All Files(*.*)|*.*"
            open.FileName = ""
            fl = open.FileName.Length
            fns = open.FileNames
            open.Multiselect = True
            open.InitialDirectory = Appcon("SplitFolder").ToString()

            Dim result As Nullable(Of Boolean) = open.ShowDialog()
            If (result = True) Then
                Try
                    filelist = open.FileNames.ToList()
                    filelist.Sort(AddressOf CompareDinosByLength)
                    For Each file As String In filelist
                        Dim dirname = file.Replace("\", "").Replace(":", "")
                        Dim rec As New IndexingField
                        rec.FieldName = file
                        rec.BatchId = dirname
                        flst.Add(rec)
                    Next
                    LeftPane.FilesLoad(flst)
                    'ECMLeftPane.leftpaneselectioncall = True
                Catch ex As Exception
                    System.Windows.MessageBox.Show(ex.Message)
                Finally
                    ECMLeftPane.Refresh()
                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub btnimp_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnimp.Click
        Try
            'Dim SharedPath As String = Appcon("IndexedFiles")
            'If SharedPath <> "" Then
            Dim filelist As New List(Of IndexingField)
            Dim open As New ImportFileForm
            Dim result As Nullable(Of Boolean) = open.ShowDialog()
            If (result = True) Then
                Try
                    filelist.AddRange(open.filelist)
                    '  filelist.Sort(AddressOf CompareDinosByLength)
                    LeftPane.FilesLoad(filelist)
                    If open.Selected <> open.imported Then

                        Dim msg As String = "No. of Batch Imported: " + open.selectedBatch.ToString() + Environment.NewLine + "No. of Files Imported :" + open.imported.ToString() + Environment.NewLine + "Other Batches are imported by Another QC User"
                        MessageBox.Show(msg, "Import Scanned Image", MessageBoxButton.OK, MessageBoxImage.Information)

                    Else
                        If open.Selected <> LeftPane.Movedfiles Then
                            Dim msg As String = "No. of Batch Imported: " + open.selectedBatch.ToString() + Environment.NewLine + "No. of Files Imported :" + open.imported.ToString() + " "
                            MessageBox.Show(msg, "Import Scanned Image", MessageBoxButton.OK, MessageBoxImage.Information)

                        Else
                            Dim msg As String = "No. of Batch Imported: " + open.selectedBatch.ToString() + Environment.NewLine + "No. of Files Imported :" + open.imported.ToString() + ""
                            MessageBox.Show(msg, "Import Scanned Image", MessageBoxButton.OK, MessageBoxImage.Information)
                        End If
                    End If
                Catch ex As Exception
                    System.Windows.MessageBox.Show(ex.Message)
                Finally
                End Try
                'If LeftPane.lstTif.SelectedItems.Count <> 0 Then
                'End If
            End If
            'Else
            '    LeftPane.Refresh()
            'End If
        Catch ex As Exception
            System.Windows.MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub btnlzo_Click(sender As Object, e As RoutedEventArgs) Handles btnlzo.Click
        Try
            StrFolderName = ConfigSettings.loadConfigDocument().SelectSingleNode("//configuration//ZonalSettings//ZonalFilePath").Attributes("Value").Value.ToString.ToLower
            'Dim query = "select * from ezzonal where templateid=" + ECMRightPane.templateid.ToString + " and createdfrom=N'" +
            '    Environment.MachineName + "' and processname='ECM-Capture'"
            'Dim ds = CAC.GetDatasetByQuery(query)
            'If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            '    Dim eZERSInfoList As List(Of eZERSInfo) = CAC.SelectedeZERSInfoList("ERSId", "1")
            '    Dim StrFile As String = ds.Tables(0).Rows(0)("zonalname").ToString
            '    StrFolderName = eZERSInfoList(0).SettingPath & RightPane.ddlstcab.Text & "\" & RightPane.ddlsttem.Text & "\" + StrFile + ".ozf"
            If File.Exists(StrFolderName) Then
                LoadZones()
            Else
                MsgBox("Please Select (Or) Create a Zonal.")
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub LoadZones()
        Try
            FieldWord = Nothing
            For i As Integer = 0 To LeftPane.lstTif.Items.Count - 1
                _currentHighlightRect = Nothing
                _currentFileName = imaging + "\" + LeftPane.lstTif.Items.Item(i).ToString().Replace("System.Windows.Controls.ListViewItem: ", "")
                PageNumber = 1
                ECMViewer.Viewer.BeginUpdate()
                ECMViewer.LoadSinglePage(_currentFileName)
                ECMViewer.Viewer.EndUpdate()
                ECMViewer.Viewer.Invalidate()
                ECMViewer.Viewer.Image.MakeRegionEmpty()
                _OcrZonePage.Engine.DocumentManager.CreateDocument()
                _OcrZonePage.Pages.AddPage(ECMViewer.Viewer.Image, Nothing)
                _OcrZonePage.Pages(i).LoadZones(StrFolderName)
                ZonesUpdated(i)
                RightPane.SaveRecords(_currentFileName)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
        End Try
    End Sub
    Public Sub ZonesUpdated(ByVal fileno As Integer)
        ECMViewer.Viewer.BeginUpdate()
        Call FindWord(fileno)
        For i As Integer = 0 To _OcrZonePage.Pages(fileno).Zones.Count - 1
            Dim zone As OcrZone = _OcrZonePage.Pages(fileno).Zones(i)
            If Not (String.IsNullOrEmpty(zone.Name)) Then
                Dim count = VisualTreeHelper.GetChildrenCount(RightPane.IndexingFieldPanel)
                If count = 0 Then
                    Exit Sub
                End If
                For j As Integer = 0 To count - 1
                    Dim child = VisualTreeHelper.GetChild(RightPane.IndexingFieldPanel, j)
                    If TypeOf child Is Grid Then
                        Dim targetItem As Grid = DirectCast(child, Grid)
                        For Each childcontrol As UIElement In targetItem.Children
                            If TypeOf childcontrol Is System.Windows.Controls.TextBox Then
                                Dim ctl = DirectCast(childcontrol, System.Windows.Controls.TextBox)
                                If ctl.Name = zone.Name Then
                                    ctl.Text = FieldWord(i).Replace("/", "-").Trim(" ").Replace(":", "-").Trim(" ").Replace("\", "-").Trim(" ").Replace("?", " ").Trim(" ").Replace("*", "-").Trim(" ").Replace("<", " -").Trim(" ").Replace(">", "-").Trim(" ").Replace("""", "-").Trim(" ").Replace("|", "-")
                                End If
                            ElseIf TypeOf childcontrol Is System.Windows.Controls.ComboBox Then
                                Dim ctl = DirectCast(childcontrol, System.Windows.Controls.ComboBox)
                                If ctl.Name = zone.Name Then
                                    ctl.Text = FieldWord(i).ToString.Replace("/", "-").Trim(" ").Replace(":", "-").Trim(" ").Replace("\", "-").Trim(" ").Replace("?", " ").Trim(" ").Replace("*", "-").Trim(" ").Replace("<", " -").Trim(" ").Replace(">", "-").Trim(" ").Replace("""", "-").Trim(" ").Replace("|", "-")
                                End If
                            ElseIf TypeOf childcontrol Is RadDatePicker Then
                                Dim ctl = DirectCast(childcontrol, RadDatePicker)
                                If ctl.Name = zone.Name Then
                                    ctl.DateTimeText = FieldWord(i).Replace("/", "-").Trim(" ").Replace(":", "-").Trim(" ").Replace("\", "-").Trim(" ").Replace("?", " ").Trim(" ").Replace("*", "-").Trim(" ").Replace("<", " -").Trim(" ").Replace(">", "-").Trim(" ").Replace("""", "-").Trim(" ").Replace("|", "-")
                                End If
                            End If
                        Next
                    End If
                Next
            End If
        Next
        ECMViewer.Viewer.EndUpdate()
        ECMViewer.Viewer.Invalidate()
        'Next
    End Sub
    Private Sub FindWord(ByVal pageno As Integer)
        Dim Count As Integer = 0
        Dim ocrpage As IOcrPage = _OcrZonePage.Pages(pageno)
        ReDim FieldWord(_OcrZonePage.Pages(pageno).Zones.Count)
        ocrpage.Recognize(Nothing)
        Dim pageCharacters As IOcrPageCharacters = ocrpage.GetRecognizedCharacters()
        If (IsNothing(pageCharacters)) Then
            Exit Sub
        End If
        FieldWord(Count) = ""
        For Each zoneCharacters As IOcrZoneCharacters In pageCharacters
            Dim words As ICollection(Of OcrWord) = zoneCharacters.GetWords(ocrpage.DpiX, ocrpage.DpiY, LogicalUnit.Pixel)
            For Each word As OcrWord In words
                If words.Count = 1 Then
                    FieldWord(Count) = word.Value
                ElseIf words.Count > 1 Then
                    If FieldWord(Count) = String.Empty Then
                        FieldWord(Count) = word.Value
                    Else
                        FieldWord(Count) = FieldWord(Count) & " " & word.Value
                    End If
                End If
            Next
            Count += 1
        Next
    End Sub
    Private Sub btnsav_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnsav.Click
        RightPane.SelectZoneIsClicked = False
        Dim BarMsg = New SelectZonalFile
        BarMsg.BtnSelectZonal.Visibility = System.Windows.Visibility.Hidden
        BarMsg.Label1.Visibility = System.Windows.Visibility.Hidden
        BarMsg.ComboBox1.Visibility = System.Windows.Visibility.Hidden
        BarMsg.BtnSaveZonal.Visibility = System.Windows.Visibility.Visible
        BarMsg.TxtZonalName.Visibility = System.Windows.Visibility.Visible
        BarMsg.Label2.Visibility = System.Windows.Visibility.Visible
        If BarMsg.ShowDialog() = True Then
            Call SaveZones(StrZonalFileName)
        End If
    End Sub

    Private Sub btnsel_Click(sender As Object, e As RoutedEventArgs) Handles btnsel.Click
        Try
            RightPane.SelectZoneIsClicked = True
            ECMViewer.FreeHandling()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub BtnSSelZonFile_Click(sender As Object, e As RoutedEventArgs) Handles BtnSSelZonFile.Click
        Try
            Dim BarMsg = New SelectZonalFile
            Dim strHostName As String = String.Empty
            Dim CabinetName As String = String.Empty
            Dim TemplateName As String = String.Empty
            Dim StrSelectedZonalFile As String = String.Empty
            BarMsg.BtnSelectZonal.Visibility = System.Windows.Visibility.Visible
            BarMsg.Label1.Visibility = System.Windows.Visibility.Visible
            BarMsg.ComboBox1.Visibility = System.Windows.Visibility.Visible
            BarMsg.BtnSaveZonal.Visibility = System.Windows.Visibility.Hidden
            BarMsg.TxtZonalName.Visibility = System.Windows.Visibility.Hidden
            BarMsg.Label2.Visibility = System.Windows.Visibility.Hidden
            cabinetid = RightPane.ddlstcab.SelectedValue
            templateid = RightPane.ddlsttem.SelectedValue
            If BarMsg.ShowDialog() = True Then
                Dim Cond As String = "ZonalName='" & StrZonalFileName & "' And CabinetId='" & ECMRightPane.cabinetid.ToString & "' And TemplateId='" & ECMRightPane.templateid.ToString & "'"
                Dim ObjeZZonalList As New List(Of eZZonal)
                ObjeZZonalList = CAC.SelectedeZZonalListByCabinetAndTemplateId(Cond)
                If ObjeZZonalList.Count = 0 Then
                    MsgBox("File Does Not Exists", vbInformation)
                Else
                    Dim eZERSInfoList As List(Of eZERSInfo) = CAC.SelectedeZERSInfoList("ERSId", "1")
                    CabinetName = ObjeZZonalList(0).CabinetName
                    TemplateName = ObjeZZonalList(0).TemaplateName
                    StrSelectedZonalFile = eZERSInfoList(0).SettingPath & CabinetName & "\" & TemplateName & "\" & ObjeZZonalList(0).ZonalName & ".ozf"
                    If File.Exists(StrSelectedZonalFile) = True Then
                        ConfigSettings.SaveZonalFilePath(StrSelectedZonalFile)
                    Else
                        MsgBox("File Does Not Exists", vbInformation)
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ColorChangeClick(sender As Object, args As RoutedEventArgs)
        ' Get the clicked context menu item '
        Try
            Dim menuItem As RadMenuItem = TryCast(DirectCast(args, RadRoutedEventArgs).OriginalSource, RadMenuItem)
            Dim header As String = TryCast(menuItem.Header, String)
            Select Case header
                Case "Color"
                    btnimgfrmt.SmallImage = New BitmapImage(New Uri("pack://application:,,,/Images/colorpg.png", UriKind.Absolute))
                    btnimgfrmt.LargeImage = New BitmapImage(New Uri("pack://application:,,,/Images/colorpg.png", UriKind.Absolute))
                    _fileFormat = RasterImageFormat.TifJpeg422
                    _bitsPerPixel = 24
                    LeftPane._fileFormat = RasterImageFormat.TifJpeg422
                    LeftPane._bitsPerPixel = 24
                    Exit Select
                Case "Black"
                    btnimgfrmt.SmallImage = New BitmapImage(New Uri("pack://application:,,,/Images/blackpg.png", UriKind.Absolute))
                    btnimgfrmt.LargeImage = New BitmapImage(New Uri("pack://application:,,,/Images/blackpg.png", UriKind.Absolute))
                    _fileFormat = FileFormets.CcittGroup4
                    _bitsPerPixel = 1
                    LeftPane._fileFormat = FileFormets.CcittGroup4
                    LeftPane._bitsPerPixel = 1
                    Exit Select
            End Select
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Window_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            'If e.Key = Key.LeftCtrl Then
            '    btnpgo_Click(Nothing, New RoutedEventArgs)
            'End If
        Catch ex As Exception
            MsgBox("Error From KeyDown : " + ex.ToString)
        End Try
    End Sub
    Public Sub keyevent()
        Try
            Dim newCmd As New RoutedCommand()
            'Next Page
            newCmd.InputGestures.Add(New KeyGesture(Key.N, ModifierKeys.Control))
            CommandBindings.Add(New CommandBinding(newCmd, AddressOf btnfr_Click))
            newCmd = New RoutedCommand()
            'Prev Page
            newCmd.InputGestures.Add(New KeyGesture(Key.P, ModifierKeys.Control))
            CommandBindings.Add(New CommandBinding(newCmd, AddressOf btnbk1_Click))
            'Next Record
            newCmd = New RoutedCommand()
            newCmd.InputGestures.Add(New KeyGesture(Key.N, ModifierKeys.Alt))
            CommandBindings.Add(New CommandBinding(newCmd, AddressOf nextrec_Click))
            newCmd = New RoutedCommand()
            'Prev Record
            newCmd.InputGestures.Add(New KeyGesture(Key.P, ModifierKeys.Alt))
            CommandBindings.Add(New CommandBinding(newCmd, AddressOf prerec_Click))
        Catch ex As Exception
            MsgBox("Error From KeyCombo : " + ex.ToString)
        End Try
    End Sub
    Private Sub btnindex_Click(sender As Object, e As RoutedEventArgs) Handles btnindex.Click
        'Try
        '    If ECMRightPane.cabinetid <> 0 Then
        '        Dim frm As New WinForm1(ECMRightPane.cabinetid, ECMRightPane.templateid)
        '        frm.ShowDialog()
        '        RightPane.loadcontrol()
        '        If _currentFileName <> "" Then
        '            RightPane.Getrecords(_currentFileName)
        '        End If
        '    Else
        '        MsgBox("Please Select Cabinet And Template...!")
        '    End If
        'Catch ex As Exception
        '    MsgBox("Error From Adding Index : " + ex.Message.ToString)
        'End Try
    End Sub
    'Private Sub btninvoice_Click(sender As Object, e As RoutedEventArgs) Handles btninvoice.Click
    '    Try
    '        If ECMRightPane.templateid <> 0 Then
    '            If Not worker.IsBusy Then
    '                progfor = "Invoice"
    '                RightPane.SaveRecords(_currentFileName)
    '                RightPane.ClearFields()
    '                _currentFileName = ""
    '                Tiflist = New List(Of String)
    '                For Each item As ListViewItem In LeftPane.lstTif.Items
    '                    Tiflist.Add(imaging + "\" + item.ToString().Replace("System.Windows.Controls.ListViewItem: ", ""))
    '                Next
    '                ECMRightPane.cabinetName = RightPane.ddlstcab.Text.ToString()
    '                ECMRightPane.TemplateName = RightPane.ddlsttem.Text.ToString()
    '                reminderAnim = TryCast(Me.Resources("BeginProgress"), Storyboard)
    '                reminderAnim.Begin()
    '                worker.RunWorkerAsync()
    '            End If
    '        Else
    '            MsgBox("Please Select Cabinet And Template...!")
    '        End If
    '    Catch ex As Exception
    '        MsgBox("Error From Invoice Processing: " + ex.Message.ToString)
    '    End Try
    'End Sub
    Public Function SplitWithSinglePage(ByVal sfilename As String, ByVal info As CodecsImageInfo, ByVal pageCount As Integer, ByVal _fileFormat As RasterImageFormat, ByVal _bitsPerPixel As Integer) As List(Of String)
        Dim loader As New ezofis.UserControl.ImageFileLoader()
        Dim splittedlist = New List(Of String)
        Try
            RasterCodecs.Startup()
            _codecs = New RasterCodecs()
            Dim Spliter As New RasterImageViewer
            If (loader.Load(own, _codecs, True, 1, info.TotalPages, sfilename)) Then
                If Not info Is Nothing Then
                    loader.Image.MakeRegionEmpty()
                    Spliter.Image = loader.Image
                End If
            End If
            Dim len As String = ""
            For i As Int16 = 1 To pageCount
                len = i
                If len.Length >= 2 Then
                    _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#" & i & ".TIF", _fileFormat, _bitsPerPixel, i, i, 1, CodecsSavePageMode.Overwrite)
                    splittedlist.Add(Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#" & i & ".TIF")
                Else
                    _codecs.Save(Spliter.Image, Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#0" & i & ".TIF", _fileFormat, _bitsPerPixel, i, i, 1, CodecsSavePageMode.Overwrite)
                    splittedlist.Add(Replace(Replace(sfilename, ".TIF", ""), ".tif", "") & "#0" & i & ".TIF")
                End If
            Next
            _codecs = Nothing
            If System.IO.File.Exists(sfilename) Then
                Kill(sfilename)
                LeftPane.DeleteRecords(sfilename)
            End If
        Catch ex As Exception
            'MsgBox(ex.Message.ToString)
        Finally
        End Try
        Return splittedlist
    End Function


End Class
