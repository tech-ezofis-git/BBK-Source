Imports Leadtools.ImageProcessing
Imports Leadtools.ImageProcessing.Color
Imports Leadtools.ImageProcessing.Core
Imports Leadtools.ImageProcessing.Effects
Imports Leadtools.Codecs
Imports Leadtools
Imports System.Windows.Media.Animation
Imports Leadtools.WinForms
Imports Leadtools.Windows.Controls
Imports Leadtools.Forms.Ocr
Imports iTextSharp.text.pdf
Imports sautinsoftlocal.pdf
Imports System.IO
Imports PdfiumViewer
Imports System.Windows.Forms



Public Class ECMViewer
#Region "FreeHandling"

    Private _rubberBandingHelper As ViewerRubberBandingHelper
#End Region

    Public Shared Viewer As New Leadtools.WinForms.RasterImageViewer
    Dim WorkerProcess As String = ""
    Dim reminderAnim As Storyboard
    Public pageNumber As Integer = 1
    Public pageCount As Integer
    Private CurrentFileName As String
    Public V_bitsPerPixel As Integer
    Public V_fileFormat As RasterImageFormat
    Private _zones As New List(Of OcrZone)
    Private _Zone As New OcrZone
    Dim Index As Integer = Nothing
    Dim BgWork As New System.ComponentModel.BackgroundWorker()
    Public PageChanges As New Dictionary(Of Integer, Integer)
    Public PdfViewer As New PdfiumViewer.PdfViewer()
    Public pdfdoc As PdfiumViewer.PdfDocument

    'Private PdfiumViewer As New PdfiumViewer.PdfViewer()
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        Viewer = New Leadtools.WinForms.RasterImageViewer
        'Viewer.SizeMode = PaintSizeMode.Fit
        'Viewer.ScaleFactor = 1.0
        'Viewer.Top = 10
        'Viewer.Left = 10
        Viewer.HorizontalAlignMode = RasterPaintAlignMode.Center
        Viewer.VerticalAlignMode = RasterPaintAlignMode.Center
        Viewer.SizeMode = PageSizeMode.Normal
        Viewer.SizeMode = PaintSizeMode.Fit
        Viewer.Dock = System.Windows.Forms.DockStyle.Fill

        AddHandler Viewer.MouseWheel, AddressOf ECMViewer_previewmousewheel
        AddHandler Viewer.MouseEnter, AddressOf ECMViewer_mouseenter
        Host.Child = Viewer
        'pdfPanel.Controls.Add(PdfViewer)
        'PdfViewer = New DocumentViewer()

        '' Add PDF viewer to pdfPanel

        'pdfPanel.Controls.Add(PdfViewer);

        _rubberBandingHelper = New ViewerRubberBandingHelper()
        _rubberBandingHelper.Viewer = Viewer

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    'Private Function GetPageCount(ByVal fileName As String) As Integer
    '    Try
    '        RasterCodecs.Startup()
    '        _codecs = New RasterCodecs()
    '        info1 = _codecs.GetInformation(fileName, True)
    '        Return info1.TotalPages
    '    Catch ex As Exception
    '    Finally
    '        info1.Dispose()
    '        _codecs.Dispose()
    '        RasterCodecs.Shutdown()
    '    End Try
    '    Return 0
    'End Function


    Private Function GetPageCount(ByVal fileName As String) As Integer

        Try
            ' Check if the file is a PDF
            If fileName.ToLower().EndsWith(".pdf") Then
                ' Ensure the file exists
                'If Not File.Exists(fileName) Then
                '    Throw New FileNotFoundException("The file was not found.", fileName)
                'End If

                ' Use fully qualified name to avoid ambiguity
                Using reader As New iTextSharp.text.pdf.PdfReader(fileName)
                    Return reader.NumberOfPages
                End Using
            Else
                Try
                    RasterCodecs.Startup()
                    _codecs = New RasterCodecs()
                    info1 = _codecs.GetInformation(fileName, True)
                    Return info1.TotalPages
                Catch ex As Exception
                    MessageBox.Show("Error initializing RasterCodecs: " & ex.Message)
                    Return 0
                Finally
                    ' Dispose codecs if initialized
                    If _codecs IsNot Nothing Then
                        _codecs.Dispose()
                        _codecs = Nothing
                    End If
                    RasterCodecs.Shutdown()
                End Try
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return 0
        End Try
    End Function

    Private pdfStream As FileStream
    ' Private pdfDocument As PdfiumViewer.PdfDocument

    Public Sub LoadSinglePagepdf(ByVal fileName As String)
        Try
            DisposePdfViewerResources()
            ' Ensure visibility settings
            PdfwindowsFormsHost.Visibility = Visibility.Visible
            Host.Visibility = Visibility.Collapsed
            pageCount = GetPageCount(fileName)
            ' Store the current file name
            CurrentFileName = fileName
            pdfStream = File.OpenRead(CurrentFileName)
            pdfdoc = PdfiumViewer.PdfDocument.Load(pdfStream)
            ' Assuming you have a PdfViewer control named pdfViewer on your form
            PdfViewer.Document = pdfdoc

            PdfViewer.Dock = DockStyle.Fill
            pdfPanel.Controls.Add(PdfViewer)

            'pdfdoc.Dispose()

        Catch ex As Exception
            ' Handle exceptions
            'MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Finally
            ' Optionally, perform any cleanup or finalization
        End Try
    End Sub

    Function GetFileStream(filePath As String) As FileStream
        Using stream As FileStream = File.OpenRead(filePath)
            ' Some operations
            Return stream   ' This will not work as expected; stream is disposed after Using block
        End Using
    End Function

    Public Sub DisposePdfViewerResources()
        'If pdfPanel.Controls.Count > 0 Then
        '    Dim pdfViewer = TryCast(pdfPanel.Controls(0), PdfiumViewer.PdfViewer)
        '    If pdfViewer IsNot Nothing Then
        '        pdfViewer.Document.Dispose()
        '        '  pdfViewer.Dispose()
        '    End If
        'End If

        'pdfStream.Close()
        'pdfStream.Dispose()
        'pdfPanel.Controls.Clear()
        Try
            pdfdoc.Dispose()
            PdfViewer.Document.Dispose()
            pdfPanel.Controls.Clear()
            pdfStream.Close()
            pdfStream.Dispose()
        Catch ex As Exception
            '  MessageBox.Show(ex.Message)
        End Try

    End Sub
    Public Sub LoadSinglePage(ByVal fileName As String)

        Dim loader As ImageFileLoader = New ImageFileLoader()

        Try
            Host.Visibility = Visibility.Visible
            PdfwindowsFormsHost.Visibility = Visibility.Collapsed

            'border1.Opacity = 0
            'border2.Opacity = 0
            CurrentFileName = fileName

            pageCount = GetPageCount(fileName)
            RasterCodecs.Startup()
            _codecs = New RasterCodecs()
            If pageCount <> 0 Then
                If pageNumber <> 0 Then
                    If pageCount > pageNumber Then
                        'border2.Opacity = 1
                        If pageNumber <> 1 Then
                            'border1.Opacity = 1
                        End If
                    ElseIf pageCount = pageNumber Then
                        If pageCount > 1 Then
                            'border1.Opacity = 1
                        End If
                    Else
                        pageNumber = pageNumber - 1
                        'border1.Opacity = 1
                    End If
                Else
                    pageNumber = pageNumber + 1
                    'border1.Opacity = 1
                    'border2.Opacity = 1
                End If
                loader.ShowLoadPagesDialog = False
                If loader.Load(formName, _codecs, True, pageNumber, pageNumber, fileName) Then
                    HadImages = True
                    loader.Image.MakeRegionEmpty()
                    Viewer.Image = loader.Image
                Else
                    HadImages = False
                End If
            Else
                loader.ShowLoadPagesDialog = False
                If loader.Load(formName, _codecs, True, 1, 1, fileName) Then
                    HadImages = True
                    loader.Image.MakeRegionEmpty()
                    Viewer.Image = loader.Image

                    'Viewer.InteractiveMode = RasterViewerInteractiveMode.Region
                Else
                    HadImages = False
                End If
            End If

        Catch ex As Exception
        Finally

            _codecs.Dispose()
            '  loader.Image.Dispose()
            RasterCodecs.Shutdown()
        End Try

    End Sub

    Public Function LoadTifImage(ByVal fileName As String) As Leadtools.RasterImage
        Dim ZoneViewer As New Leadtools.WinForms.RasterImageViewer
        Dim loader As ImageFileLoader = New ImageFileLoader()
        RasterCodecs.Startup()
        _codecs = New RasterCodecs()
        Try

            CurrentFileName = fileName
            pageCount = GetPageCount(fileName)
            If pageCount <> 0 Then
                loader.ShowLoadPagesDialog = False
                If loader.Load(formName, _codecs, True, 1, pageCount, fileName) Then
                    loader.Image.MakeRegionEmpty()
                    ZoneViewer.Image = loader.Image
                    Return ZoneViewer.Image
                End If
            End If
        Catch ex As Exception
        Finally

        End Try
        Return Nothing
    End Function


    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        AddHandler _rubberBandingHelper.RubberBand, AddressOf _rubberBandingHelper_RubberBand

        'Viewer.ContextMenu.Show(Viewer, New System.Drawing.Point(20, 20))
        'Dim PtX As Integer = System.Windows.Forms.Control.MousePosition.X
        'Dim PtY As Integer = System.Windows.Forms.Control.MousePosition.Y
        'Dim Cont As New ContextMenu
        'Viewer.ContextMenu.Show(Viewer, New System.Drawing.Point(20, 20))


        'Dim eventhan As New ComponentModel.DoWorkEventHandler(AddressOf BgWork_DoWork)
        'RemoveHandler BgWork.DoWork, eventhan
        'AddHandler BgWork.DoWork, eventhan
        'Dim runeve As New ComponentModel.RunWorkerCompletedEventHandler(AddressOf BgWork_RunWorkerCompleted)
        'RemoveHandler BgWork.RunWorkerCompleted, runeve
        'AddHandler BgWork.RunWorkerCompleted, runeve
    End Sub
    Private Sub BgWork_RunWorkerCompleted(ByVal Sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        reminderAnim.Stop()

    End Sub
    Private Sub BgWork_DoWork(ByVal Sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Try
            If WorkerProcess = "DeletePage" Then
                DeleteTifPage(CurrentFileName, V_fileFormat, V_bitsPerPixel)
            ElseIf WorkerProcess = "SavePage" Then
                SaveTifPage(CurrentFileName, V_fileFormat, V_bitsPerPixel)
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub _rubberBandingHelper_RubberBand(ByVal sender As Object, ByVal e As ViewerRubberBandingHelperEventArgs)
        Try
            _recognitionResults = ""
            If _rubberBandingHelper.IsStarted Then
                _rubberBandingHelper.Stop()
            End If
            Try
                If Not Viewer.Image Is Nothing Then
                    _currentHighlightRect = e.Bounds
                    _document.Pages.Clear()
                    _document.Pages.AddPage(Viewer.Image, Nothing)
                    _document.Pages(0).Zones.Add(FreeHand(_currentHighlightRect))
                    _recognitionResults = _document.Pages(0).RecognizeText(Nothing)
                    If _recognitionResults = Constants.vbLf Or _recognitionResults = "" Then
                        Messager.ShowInformation(Me, "No text was recognized.")
                    Else
                        Dim Cont As New System.Windows.Forms.ContextMenuStrip
                        Cont = Viewer.ContextMenuStrip
                        Dim PtX As Integer = System.Windows.Forms.Control.MousePosition.X
                        Dim PtY As Integer = System.Windows.Forms.Control.MousePosition.Y
                        Cont.Show(New System.Drawing.Point(PtX, PtY))
                    End If
                End If
            Catch

            Finally

            End Try
        Catch ex As Exception
            Messager.ShowError(Me, ex)
        Finally

            System.Windows.Forms.Application.DoEvents()
            If (Not _rubberBandingHelper.IsStarted) Then
                _rubberBandingHelper.Start()
            End If

            'Viewer.ContextMenu.Show(sender, New System.Drawing.Point(20, 20))

        End Try
    End Sub
    Public Sub FreeHandling()
        Try
            If Viewer.InteractiveMode = BitmapSourceViewerInteractiveMode.MagnifyGlass Or Viewer.InteractiveMode = RasterViewerInteractiveMode.Pan Or Viewer.InteractiveMode = RasterViewerInteractiveMode.ZoomTo Then
                Viewer.InteractiveMode = BitmapSourceViewerInteractiveMode.None
            End If
            'Viewer.InteractiveMode = ViewerInteractiveMode.Pan
            _rubberBandingHelper.Start()
        Catch ex As Exception

        End Try
    End Sub
    Dim own As New Window
    Public Sub SaveTifPage(ByVal FileName As String, ByVal _fileFormat As RasterImageFormat, ByVal _bitsPerPixel As Integer)

        'Dim viewer1 As RasterImage
        'Dim info As CodecsImageInfo
        'Try
        '    RasterCodecs.Startup()
        '    _codecs = New RasterCodecs()
        '    If PageChanges.Count > 0 Then


        '        Dim loader As New ImageFileLoader()
        '        info = _codecs.GetInformation(FileName, True)
        '        For i As Integer = 1 To info.TotalPages
        '            If PageChanges.ContainsKey(i) Then
        '                loader = New ImageFileLoader()
        '                _codecs = New RasterCodecs()
        '                If (loader.Load(own, _codecs, True, i, i, FileName)) Then
        '                    If Not info Is Nothing Then
        '                        loader.Image.MakeRegionEmpty()
        '                        Try
        '                            Dim angle = PageChanges.Item(i)
        '                            Dim command1 As IRasterCommand = CType(Activator.CreateInstance(GetType(RotateCommand)), IRasterCommand)
        '                            Dim cmd1 As RotateCommand = CType(command1, RotateCommand)
        '                            cmd1.Angle = angle
        '                            cmd1.Run(loader.Image)
        '                        Catch ex As Exception

        '                        End Try
        '                        If Not IsNothing(viewer1) Then
        '                            viewer1.AddPage(loader.Image)
        '                        Else
        '                            viewer1 = loader.Image
        '                        End If
        '                    End If
        '                    _codecs.Dispose()
        '                End If
        '            Else
        '                loader = New ImageFileLoader()
        '                _codecs = New RasterCodecs()
        '                If (loader.Load(own, _codecs, True, i, i, FileName)) Then
        '                    If Not info Is Nothing Then
        '                        loader.Image.MakeRegionEmpty()
        '                        If Not IsNothing(viewer1) Then
        '                            viewer1.AddPage(loader.Image)
        '                        Else
        '                            viewer1 = loader.Image
        '                        End If
        '                    End If
        '                    _codecs.Dispose()
        '                End If
        '            End If
        '        Next


        '        _codecs = New RasterCodecs()
        '        _codecs.Save(viewer1, FileName, _fileFormat, _bitsPerPixel, 1, pageCount, 1, CodecsSavePageMode.Overwrite)
        '        PageChanges = New Dictionary(Of Integer, Integer)

        '        ' _codecs.Dispose()
        '    End If


        'Catch ex As Exception
        '    MessageBox.Show(ex.Message.ToString())
        'Finally
        '    info.Dispose()
        '    viewer1.Dispose()
        '    _codecs.Dispose()
        '    RasterCodecs.Shutdown()
        'End Try


        ' Dim viewer1 As RasterImage
        Dim info As CodecsImageInfo
        Try
            RasterCodecs.Startup()
            _codecs = New RasterCodecs()
            If PageChanges.Count > 0 Then
                Dim tempPath = IO.Path.Combine(IO.Path.GetDirectoryName(FileName), "temp")
                If Not IO.Directory.Exists(tempPath) Then
                    IO.Directory.CreateDirectory(tempPath)
                End If
                Dim tempfile = IO.Path.Combine(tempPath, IO.Path.GetFileName(FileName))
                IO.File.Copy(FileName, tempfile, True)
                Dim loader As New ImageFileLoader()
                info = _codecs.GetInformation(tempfile, True)
                For i As Integer = 1 To info.TotalPages
                    If PageChanges.ContainsKey(i) Then
                        loader = New ImageFileLoader()
                        _codecs = New RasterCodecs()
                        If (loader.Load(own, _codecs, True, i, i, tempfile)) Then
                            If Not info Is Nothing Then
                                loader.Image.MakeRegionEmpty()
                                Try
                                    Dim angle = PageChanges.Item(i)
                                    Dim command1 As IRasterCommand = CType(Activator.CreateInstance(GetType(RotateCommand)), IRasterCommand)
                                    Dim cmd1 As RotateCommand = CType(command1, RotateCommand)
                                    cmd1.Angle = angle
                                    cmd1.Run(loader.Image)
                                Catch ex As Exception

                                End Try
                                If i = 1 Then
                                    _codecs.Save(loader.Image, FileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                                Else
                                    _codecs.Save(loader.Image, FileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                                End If

                            End If
                            _codecs.Dispose()
                            loader.Image.Dispose()
                        End If
                    Else
                        loader = New ImageFileLoader()
                        _codecs = New RasterCodecs()
                        If (loader.Load(own, _codecs, True, i, i, tempfile)) Then
                            If Not info Is Nothing Then
                                loader.Image.MakeRegionEmpty()
                                If i = 1 Then
                                    _codecs.Save(loader.Image, FileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                                Else
                                    _codecs.Save(loader.Image, FileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                                End If
                            End If

                            _codecs.Dispose()
                            loader.Image.Dispose()
                        End If
                    End If
                Next

                If IO.File.Exists(tempfile) Then
                    Kill(tempfile)
                End If
                '  _codecs = New RasterCodecs()
                '_codecs.Save(viewer1, FileName, _fileFormat, _bitsPerPixel, 1, pageCount, 1, CodecsSavePageMode.Overwrite)
                PageChanges = New Dictionary(Of Integer, Integer)

                ' _codecs.Dispose()
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString())
        Finally
            info.Dispose()
            '   viewer1.Dispose()
            _codecs.Dispose()
            RasterCodecs.Shutdown()
        End Try

    End Sub
    Dim formName As New Window
    Public Sub DeleteTifPage(ByVal FileName As String, ByVal fileFormat As RasterImageFormat, ByVal bitsPerPixel As Integer)
        'Try


        '    RasterCodecs.Startup()
        '    _codecs = New RasterCodecs()
        '    pageCount = GetPageCount(CurrentFileName)
        '    Dim viewer1 As New Leadtools.WinForms.RasterImageViewer
        '    Dim loader As New ImageFileLoader()
        '    'Dim info As ImageInformation
        '    Dim info As CodecsImageInfo = _codecs.GetInformation(CurrentFileName, True)

        '    If (loader.Load(formName, _codecs, True, 1, info.TotalPages, CurrentFileName)) Then
        '        If (IsNothing(viewer1.Image)) Then
        '            loader.Image.MakeRegionEmpty()
        '            viewer1.Image = loader.Image
        '            viewer1.Image.RemovePageAt(pageNumber)

        '        End If
        '    End If

        '    _codecs.Save(viewer1.Image, CurrentFileName, _fileFormat, _bitsPerPixel, 1, pageCount - 1, 1, CodecsSavePageMode.Overwrite)




        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
        'Dim viewer1 As RasterImage
        'Dim info As CodecsImageInfo
        'Try
        '    CurrentFileName = FileName
        '    _fileFormat = fileFormat
        '    _bitsPerPixel = bitsPerPixel
        '    RasterCodecs.Startup()
        '    _codecs = New RasterCodecs()


        '    Dim loader As New ImageFileLoader()
        '    info = _codecs.GetInformation(FileName, True)
        '    For i As Integer = 1 To info.TotalPages
        '        If pageNumber <> i Then

        '            loader = New ImageFileLoader()
        '            _codecs = New RasterCodecs()
        '            If (loader.Load(own, _codecs, True, i, i, FileName)) Then
        '                If Not info Is Nothing Then
        '                    loader.Image.MakeRegionEmpty()
        '                    If Not IsNothing(viewer1) Then
        '                        viewer1.AddPage(loader.Image)
        '                    Else
        '                        viewer1 = loader.Image
        '                    End If
        '                End If
        '                _codecs.Dispose()
        '            End If
        '        End If
        '    Next
        '    _codecs = New RasterCodecs()
        '    _codecs.Save(viewer1, FileName, _fileFormat, _bitsPerPixel, 1, pageCount - 1, 1, CodecsSavePageMode.Overwrite)
        '    ' _codecs.Dispose()
        'Catch ex As Exception
        'Finally
        '    info.Dispose()
        '    viewer1.Dispose()
        '    _codecs.Dispose()
        '    RasterCodecs.Shutdown()
        'End Try

        '  Dim viewer1 As RasterImage
        Dim info As CodecsImageInfo
        Try
            CurrentFileName = FileName
            _fileFormat = fileFormat
            _bitsPerPixel = bitsPerPixel
            RasterCodecs.Startup()
            _codecs = New RasterCodecs()
            Dim tempPath = IO.Path.Combine(IO.Path.GetDirectoryName(FileName), "temp")
            If Not IO.Directory.Exists(tempPath) Then
                IO.Directory.CreateDirectory(tempPath)
            End If
            Dim tempfile = IO.Path.Combine(tempPath, IO.Path.GetFileName(FileName))
            IO.File.Copy(FileName, tempfile, True)

            Dim loader As New ImageFileLoader()
            info = _codecs.GetInformation(tempfile, True)
            For i As Integer = 1 To info.TotalPages
                If pageNumber <> i Then

                    loader = New ImageFileLoader()
                    _codecs = New RasterCodecs()
                    If (loader.Load(own, _codecs, True, i, i, tempfile)) Then
                        If Not info Is Nothing Then
                            loader.Image.MakeRegionEmpty()
                            'If Not IsNothing(viewer1) Then
                            '    viewer1.AddPage(loader.Image)
                            'Else
                            '    viewer1 = loader.Image
                            'End If
                            If pageNumber = 1 Then
                                If i = 2 Then
                                    _codecs.Save(loader.Image, FileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                                Else
                                    _codecs.Save(loader.Image, FileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                                End If
                            Else
                                If i = 1 Then
                                    _codecs.Save(loader.Image, FileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                                Else
                                    _codecs.Save(loader.Image, FileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                                End If
                            End If

                        End If
                        _codecs.Dispose()
                        loader.Image.Dispose()
                    End If
                End If
            Next
            If IO.File.Exists(tempfile) Then
                Kill(tempfile)
            End If
            ' _codecs = New RasterCodecs()
            '  _codecs.Save(viewer1, FileName, _fileFormat, _bitsPerPixel, 1, pageCount - 1, 1, CodecsSavePageMode.Overwrite)
            ' _codecs.Dispose()
        Catch ex As Exception
        Finally
            info.Dispose()
            '   viewer1.Dispose()
            '  _codecs.Dispose()
            RasterCodecs.Shutdown()

        End Try

    End Sub
    Public Sub Deleteselectedtifpage(ByVal FileName As String, ByVal fileFormat As RasterImageFormat, ByVal bitsPerPixel As Integer, ByVal pageNumbers As String)

        Dim info As CodecsImageInfo
        Try
            Dim pagesToSkip As New HashSet(Of Integer)(
            pageNumbers.Split(","c).
            Select(Function(x) Integer.Parse(x.Trim()))
            )
            CurrentFileName = FileName 'org filename 
            _fileFormat = fileFormat 'Leadtools.RasterImageFormat.CcittGroup4
            _bitsPerPixel = bitsPerPixel '1
            RasterCodecs.Startup()
            _codecs = New RasterCodecs()
            Dim tempPath = IO.Path.Combine(IO.Path.GetDirectoryName(FileName), "temp")
            If Not IO.Directory.Exists(tempPath) Then
                IO.Directory.CreateDirectory(tempPath)
            End If
            Dim tempfile = IO.Path.Combine(tempPath, IO.Path.GetFileName(FileName))
            IO.File.Copy(FileName, tempfile, True)
            Dim firstSavedPage As Boolean = True
            Dim enteredpagedeletion As Boolean = False
            Dim loader As New ImageFileLoader()
            info = _codecs.GetInformation(tempfile, True)
            For i As Integer = 1 To info.TotalPages
                If Not pagesToSkip.Contains(i) Then

                    loader = New ImageFileLoader()
                    _codecs = New RasterCodecs()
                    If (loader.Load(own, _codecs, True, i, i, tempfile)) Then
                        If Not info Is Nothing Then
                            loader.Image.MakeRegionEmpty()
                            'If Not IsNothing(viewer1) Then
                            '    viewer1.AddPage(loader.Image)
                            'Else
                            '    viewer1 = loader.Image
                            'End If
                            If firstSavedPage Then
                                _codecs.Save(loader.Image, FileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                                firstSavedPage = False
                            Else
                                _codecs.Save(loader.Image, FileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                            End If
                            enteredpagedeletion = True
                        End If
                    End If
                    _codecs.Dispose()
                    loader.Image.Dispose()
                End If
            Next
            If IO.File.Exists(tempfile) Then
                Kill(tempfile)
            End If
            ' _codecs = New RasterCodecs()
            '  _codecs.Save(viewer1, FileName, _fileFormat, _bitsPerPixel, 1, pageCount - 1, 1, CodecsSavePageMode.Overwrite)
            ' _codecs.Dispose()
            If enteredpagedeletion Then
                MessageBox.Show("Pages have been deleted", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
        Finally
            info.Dispose()
            '   viewer1.Dispose()
            '  _codecs.Dispose()
            RasterCodecs.Shutdown()

        End Try

    End Sub

    Public Sub FirstPage(ByVal fileName As String)
        Try
            Viewer.InteractiveMode = BitmapSourceViewerInteractiveMode.None
            pageNumber = 1
            LoadSinglePage(fileName)
        Catch ex As Exception

        End Try

    End Sub
    Public Sub LastPage(ByVal fileName As String)
        Try
            Viewer.InteractiveMode = BitmapSourceViewerInteractiveMode.None
            pageNumber = GetPageCount(fileName)
            LoadSinglePage(fileName)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub NextPage(ByVal fileName As String)
        Try
            Viewer.InteractiveMode = BitmapSourceViewerInteractiveMode.None
            pageNumber = pageNumber + 1
            LoadSinglePage(fileName)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub PreviousPage(ByVal fileName As String)
        Try
            Viewer.InteractiveMode = BitmapSourceViewerInteractiveMode.None
            pageNumber = pageNumber - 1
            LoadSinglePage(fileName)
        Catch ex As Exception

        End Try
    End Sub

    Public Function LoadMultiPages(ByVal fileName As String, ByVal frmpage As Integer, ByVal ToPage As Integer) As ImageInformation
        Dim loader As ImageFileLoader = New ImageFileLoader()
        RasterCodecs.Startup()
        _codecs = New RasterCodecs()
        Try
            Dim formName As New Window
            pageCount = GetPageCount(fileName)
            If pageCount <> 0 Then
                loader.ShowLoadPagesDialog = False
                If loader.Load(formName, _codecs, True, frmpage, ToPage, fileName) Then
                    loader.Image.MakeRegionEmpty()
                    Return New ImageInformation(loader.Image, loader.FileName)
                End If
            End If
        Catch ex As Exception

        End Try
        Return Nothing
    End Function


    Public Function MergeFiles(ByRef MergeException As String, ByVal imagingDirectory As String, ByVal FileLst As List(Of String)) As Boolean
        Dim Result As Boolean
        Try

            If FileLst.Count = 0 Then
                Result = False
                MergeException = "Imaging Folder is Empty"
            ElseIf FileLst.Count = 1 Then
                Result = True
            Else
                Dim viewer1 As RasterImage
                RasterCodecs.Startup()
                _codecs = New RasterCodecs()
                Dim name As String = imagingDirectory + "\" + FileLst(0)
                For i As Int16 = 0 To FileLst.Count - 1
                    Dim Spliter As RasterImage
                    pageCount = GetPageCount(imagingDirectory + "\" + FileLst(i))
                    Dim info As ImageInformation = LoadMultiPages(imagingDirectory + "\" + FileLst(i), 1, pageCount)
                    If Not info Is Nothing Then
                        Spliter = info.Image
                    End If
                    If (IsNothing(viewer1)) Then
                        viewer1 = Spliter
                    Else
                        viewer1.AddPages(Spliter, 1, pageCount)
                    End If
                    Kill(imagingDirectory + "\" + FileLst(i))
                Next
                _codecs.Save(viewer1, name, _fileFormat, _bitsPerPixel, 1, viewer1.PageCount, 1, CodecsSavePageMode.Overwrite)
                Result = True
            End If

        Catch ex As Exception
            MergeException = ex.Message
            Result = False
        Finally

        End Try
        Return Result
    End Function


    Private Sub BtnNext_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If GetPageCount(CurrentFileName) <> 0 Then
            NextPage(CurrentFileName)
        End If

    End Sub

    Private Sub BtnPrevious_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If GetPageCount(CurrentFileName) <> 0 Then
            PreviousPage(CurrentFileName)
        End If
    End Sub




    Public Sub RotateLeft()
        Try
            Viewer.InteractiveMode = BitmapSourceViewerInteractiveMode.None
            Dim command As IRasterCommand = CType(Activator.CreateInstance(GetType(RotateCommand)), IRasterCommand)
            Dim cmd As RotateCommand = CType(command, RotateCommand)
            Dim eighth As Integer = CType(Viewer.Image.Width / 8, Integer)
            cmd.Angle = -9000
            Dim a As RasterImageChangedFlags = cmd.Run(Viewer.Image)
            If Not PageChanges.ContainsKey(pageNumber) Then
                PageChanges.Add(pageNumber, -9000)
            Else
                PageChanges.Item(pageNumber) = PageChanges.Item(pageNumber) - 9000

            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub RotateRight()
        Try
            Viewer.InteractiveMode = BitmapSourceViewerInteractiveMode.None
            Dim command As IRasterCommand = CType(Activator.CreateInstance(GetType(RotateCommand)), IRasterCommand)
            Dim cmd As RotateCommand = CType(command, RotateCommand)
            Dim eighth As Integer = CType(Viewer.Image.Width / 8, Integer)
            cmd.Angle = 9000
            Dim a As RasterImageChangedFlags = cmd.Run(Viewer.Image)

            If Not PageChanges.ContainsKey(pageNumber) Then
                PageChanges.Add(pageNumber, 9000)
            Else
                PageChanges.Item(pageNumber) = PageChanges.Item(pageNumber) + 9000
            End If


        Catch ex As Exception

        End Try
    End Sub
    Public Sub ZoomIn()
        Try
            'If Viewer.SizeMode = RasterPaintSizeMode.Fit Or Viewer.SizeMode = RasterPaintSizeMode.FitWidth Or Viewer.SizeMode = RasterPaintSizeMode.Stretch Then
            '    Viewer.ScaleFactor = 1
            '    Viewer.SizeMode = RasterPaintSizeMode.Normal
            'End If
            Viewer.ScaleFactor *= 1.2F
        Catch ex As Exception

        End Try
    End Sub
    Public Sub ZoomOut()
        Try
            'If Viewer.SizeMode = RasterPaintSizeMode.Fit Or Viewer.SizeMode = RasterPaintSizeMode.FitWidth Or Viewer.SizeMode = RasterPaintSizeMode.Stretch Then
            '    Viewer.ScaleFactor = 0.48225300976769697
            '    Viewer.SizeMode = RasterPaintSizeMode.Normal
            'End If
            Viewer.ScaleFactor /= 1.2F
        Catch ex As Exception

        End Try
    End Sub
    Public Sub Stretch()
        Try
            Viewer.ScaleFactor = 1.0
            Viewer.InteractiveMode = RasterViewerInteractiveMode.None
            Viewer.SizeMode = RasterPaintSizeMode.Stretch
        Catch ex As Exception

        End Try
    End Sub
    Public Sub FitAlways()
        Try
            Viewer.ScaleFactor = 1.0
            Viewer.InteractiveMode = RasterViewerInteractiveMode.None
            Viewer.HorizontalAlignMode = RasterPaintAlignMode.Center
            Viewer.VerticalAlignMode = RasterPaintAlignMode.Center
            Viewer.SizeMode = PaintSizeMode.FitAlways
        Catch ex As Exception

        End Try
    End Sub
    Public Sub FitWidth()
        Try
            Viewer.ScaleFactor = 1.0
            Viewer.InteractiveMode = RasterViewerInteractiveMode.None
            Viewer.SizeMode = RasterPaintSizeMode.FitWidth
        Catch ex As Exception

        End Try
    End Sub
    Public Sub MagnifyGlass()
        Try
            _rubberBandingHelper.Stop()
            Viewer.InteractiveMode = BitmapSourceViewerInteractiveMode.MagnifyGlass
        Catch ex As Exception

        End Try
    End Sub

    Public Sub Pan()
        Try
            _rubberBandingHelper.Stop()
            Viewer.InteractiveMode = RasterViewerInteractiveMode.Pan
        Catch ex As Exception

        End Try
    End Sub
    Public Sub ZoomTo()
        Try
            _rubberBandingHelper.Stop()
            If Viewer.SizeMode = RasterPaintSizeMode.Fit Or Viewer.SizeMode = RasterPaintSizeMode.FitWidth Or Viewer.SizeMode = RasterPaintSizeMode.Stretch Then
                Viewer.ScaleFactor = 1.0
                Viewer.SizeMode = RasterPaintSizeMode.Normal
            End If
            Viewer.InteractiveMode = RasterViewerInteractiveMode.ZoomTo
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ECMViewer_previewmousewheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Try
            If (e.Delta > 0) Then
                ZoomIn()
            Else
                ZoomOut()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ECMViewer_mouseenter(ByVal sender As Object, ByVal e As EventArgs)
        If Viewer.InteractiveMode = RasterViewerInteractiveMode.Pan Then
            Viewer.Cursor = System.Windows.Forms.Cursors.Hand
        Else
            Viewer.Cursor = System.Windows.Forms.Cursors.Arrow
        End If

    End Sub



End Class
