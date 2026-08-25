Imports Leadtools.ImageProcessing
Imports Leadtools.Codecs
Imports Leadtools
Imports System.Windows.Media.Animation
Imports Leadtools.WinForms
Imports Leadtools.Forms
Imports Leadtools.Ocr
Imports Leadtools.Document
Imports Leadtools.Document.Viewer
Imports Leadtools.Controls
Imports System.Text
Imports System.IO
Imports Leadtools.Caching
Imports ezofis.UserControl.DocumentViewerDemo
Imports Leadtools.Annotations.Automation
Imports System.Windows.Forms
Imports Tulpep.NotificationWindow
Imports ezofis.UserControl.PublicVariable


Public Class ECMViewer
#Region "Variables"

    Public Shared _documentViewer As Global.Leadtools.Document.Viewer.DocumentViewer
    Public Shared Viewer As New RasterImageViewer
    Public pageNumber As Integer = 1
    Public pageCount As Integer
    Private CurrentFileName As String
    Public _bitsPerPixel As Integer
    Public _fileFormat As RasterImageFormat
    Private _zones As New List(Of OcrZone)
    Private _Zone As New OcrZone
    Dim Index As Integer = Nothing
    Dim formName As New Window

    Dim Constr = "DefaultEndpointsProtocol=https;AccountName=ezofisdiag733;AccountKey=/PwucpqwjuJduHuKkhbQQvYvfC35rgamMnr27EvDkwRCvRh3B6vkU1UN/FNE3rhHziiqyPDLnxM9lZlDV9N2Dw==;EndpointSuffix=core.windows.net"


#End Region
    Public Sub New()
        InitializeComponent()
        InitDocumentViewer()
    End Sub
    Private Function GetPageCount(ByVal fileName As String) As Integer
        Try
            'RasterCodecs.Startup()
            _codecs = New RasterCodecs()
            info1 = _codecs.GetInformation(fileName, True)
            Return info1.TotalPages
        Catch ex As Exception
        End Try
        Return 0
    End Function

    Public Sub FreeHandling()
        Try

            If _documentViewer.Commands.CanRun(DocumentViewerCommands.InteractiveSelectText, pageNumber) Then
                _documentViewer.Commands.Run(DocumentViewerCommands.InteractiveSelectText, pageNumber)
            End If
            '   gettext = True
        Catch ex As Exception
        End Try
    End Sub

    Public Sub ClearDocument()
        Try
            If _documentViewer Is Nothing Then
                Return
            End If
            Dim oldDoc As LEADDocument = _documentViewer.Document
            _documentViewer.SetDocument(Nothing)
            If oldDoc IsNot Nothing Then
                oldDoc.Dispose()
            End If
            pageCount = 0
            pageNumber = 1
            CurrentFileName = ""
        Catch ex As Exception
        End Try
    End Sub

    Public Sub SaveTifPage(ByVal FileName As String, ByVal fileFormat As RasterImageFormat, ByVal bitsPerPixel As Integer)
        Dim info As CodecsImageInfo
        '_fileFormat as RasterImageFormat ="Tifjpeg(24)"
        'bitsPerPixel = 24
        Try
            _codecs = New RasterCodecs()
            If PageChanges.Count > 0 Then
                CurrentFileName = _documentViewer.Document.GetDocumentFileName
                _fileFormat = fileFormat
                _bitsPerPixel = bitsPerPixel
                Dim tempPath = IO.Path.Combine(IO.Path.GetDirectoryName(CurrentFileName), "temp")
                If Not IO.Directory.Exists(tempPath) Then
                    IO.Directory.CreateDirectory(tempPath)
                End If
                Dim tempfile = IO.Path.Combine(tempPath, IO.Path.GetFileName(CurrentFileName))
                IO.File.Copy(CurrentFileName, tempfile, True)

                Dim loader As New ImageFileLoader()
                info = _codecs.GetInformation(tempfile, True)
                For i As Integer = 1 To info.TotalPages
                    If PageChanges.ContainsKey(i) Then
                        loader = New ImageFileLoader()
                        _codecs = New RasterCodecs()
                        If (loader.Load(own, _codecs, True, i, i, tempfile)) Then
                            If Not info Is Nothing Then loader.Image.MakeRegionEmpty()
                            Try
                                Dim command As RasterCommand = CType(Activator.CreateInstance(GetType(RotateCommand)), RasterCommand)
                                Dim cmd As RotateCommand = CType(command, RotateCommand)
                                Dim eighth As Integer = CType(loader.Image.Width / 8, Integer)
                                cmd.Angle = PageChanges.Item(i) 'RotateAngle '-9000
                                Dim a As RasterImageChangedFlags = cmd.Run(loader.Image)
                            Catch ex As Exception

                            End Try
                            If i = 1 Then
                                _codecs.Save(loader.Image, CurrentFileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                            Else
                                _codecs.Save(loader.Image, CurrentFileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                            End If

                        End If
                    Else
                        loader = New ImageFileLoader()
                        _codecs = New RasterCodecs()
                        If (loader.Load(own, _codecs, True, i, i, tempfile)) Then
                            If Not info Is Nothing Then loader.Image.MakeRegionEmpty()
                            If i = 1 Then
                                _codecs.Save(loader.Image, CurrentFileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                            Else
                                _codecs.Save(loader.Image, CurrentFileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                            End If
                        End If
                    End If
                    _codecs.Dispose()
                    loader.Image.Dispose()
                Next
                If IO.File.Exists(tempfile) Then
                    Kill(tempfile)
                End If
                PageChanges = New Dictionary(Of Integer, Integer)
                ' MsgBox("Page Saved Successfully")
            End If

        Catch ex As Exception
            'MsgBox("SaveTifPage: " + ex.Message)
            Dim obkwind As New PopupNotifier
            Dim savepath = System.Reflection.Assembly.GetEntryAssembly().Location
            savepath = IO.Path.GetDirectoryName(savepath)
            Dim source = savepath + "\err.png"
            obkwind.Image = System.Drawing.Image.FromFile(source)
            obkwind.ContentFont = New System.Drawing.Font("Tahoma", 10.0F)
            obkwind.Size = New System.Drawing.Size(400, 200)
            obkwind.ShowGrip = False
            obkwind.AnimationDuration = 5000
            obkwind.HeaderHeight = 20
            obkwind.Scroll = True
            obkwind.TitleFont = New System.Drawing.Font("Tahoma", 12.0F)
            obkwind.TitleText = "EZOFIS CAPTURE : NOTIFICATION"
            obkwind.ContentText = "SaveTifPage :" + ex.Message.ToString()
            obkwind.Popup()
        Finally
            'info.Dispose()
            '   viewer1.Dispose()
            _codecs.Dispose()
        End Try

    End Sub
    Public Sub DeleteTifPage(ByVal FileName As String, ByVal fileFormat As RasterImageFormat, ByVal bitsPerPixel As Integer)
        'Try
        '    CurrentFileName = FileName
        '    _fileFormat = fileFormat
        '    _bitsPerPixel = bitsPerPixel
        '    'RasterCodecs.Startup()
        '    _codecs = New RasterCodecs()
        '    pageCount = GetPageCount(CurrentFileName)
        '    Dim viewer1 As New Leadtools.WinForms.RasterImageViewer
        '    Dim loader As New ImageFileLoader()
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
        '    MsgBox("DeleteTifPage: " + ex.Message)
        'End Try
        Dim info As CodecsImageInfo
        Try
            CurrentFileName = FileName
            _fileFormat = fileFormat
            _bitsPerPixel = bitsPerPixel
            'RasterCodecs.Startup()
            _codecs = New RasterCodecs()
            Dim tempPath = IO.Path.Combine(IO.Path.GetDirectoryName(FileName), "temp")
            If Not IO.Directory.Exists(tempPath) Then
                IO.Directory.CreateDirectory(tempPath)
            End If
            Dim tempfile = IO.Path.Combine(tempPath, IO.Path.GetFileName(FileName))
            IO.File.Copy(FileName, tempfile, True)

            Dim loader As New ImageFileLoader()
            info = _codecs.GetInformation(tempfile, True)

            For i As Integer = 1 To pageCount
                If pageNumber <> i Then
                    loader = New ImageFileLoader()
                    _codecs = New RasterCodecs()
                    If (loader.Load(formName, _codecs, True, i, i, tempfile)) Then
                        loader.Image.MakeRegionEmpty()
                        If pageNumber = 1 Then
                            If i = 2 Then
                                _codecs.Save(loader.Image, CurrentFileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                            Else
                                _codecs.Save(loader.Image, CurrentFileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                            End If
                        Else
                            If i = 1 Then
                                _codecs.Save(loader.Image, CurrentFileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Overwrite)
                            Else
                                _codecs.Save(loader.Image, CurrentFileName, _fileFormat, _bitsPerPixel, 1, 1, 1, CodecsSavePageMode.Append)
                            End If
                        End If
                        _codecs.Dispose()
                        loader.Image.Dispose()
                    End If

                End If
            Next
            If (pageNumber < pageCount) Then
                pageNumber = pageNumber - 1
                pageCount = pageCount - 1
            End If
            If IO.File.Exists(tempfile) Then
                Kill(tempfile)
            End If



        Catch ex As Exception
            'MsgBox("DeleteTifPage: " + ex.Message)
            Dim obkwind As New PopupNotifier
            Dim savepath = System.Reflection.Assembly.GetEntryAssembly().Location
            savepath = IO.Path.GetDirectoryName(savepath)
            Dim source = savepath + "\err.png"
            obkwind.Image = System.Drawing.Image.FromFile(source)
            obkwind.ContentFont = New System.Drawing.Font("Tahoma", 10.0F)
            obkwind.Size = New System.Drawing.Size(400, 200)
            obkwind.ShowGrip = False
            obkwind.AnimationDuration = 5000
            obkwind.HeaderHeight = 20
            obkwind.Scroll = True
            obkwind.TitleFont = New System.Drawing.Font("Tahoma", 12.0F)
            obkwind.TitleText = "EZOFIS CAPTURE : NOTIFICATION"
            obkwind.ContentText = "DeleteTifPage:" + ex.Message.ToString()
            obkwind.Popup()
        Finally
            info.Dispose()
        End Try

    End Sub


    Public Sub FirstPage(ByVal fileName As String)
        Try

            pageNumber = 1

            _documentViewer.Commands.Run(DocumentViewerCommands.PageGoto, 1)
        Catch ex As Exception
        End Try
    End Sub
    Public Sub LastPage(ByVal fileName As String)
        Try
            pageNumber = pageCount
            _documentViewer.Commands.Run(DocumentViewerCommands.PageGoto, pageCount)
        Catch ex As Exception
        End Try
    End Sub
    Public Sub NextPage(ByVal fileName As String)
        Try

            pageNumber = pageNumber + 1
            'LoadSinglePage(fileName)
            If pageNumber <= pageCount Then
                _documentViewer.Commands.Run(DocumentViewerCommands.PageGoto, pageNumber)
            Else
                pageNumber = pageNumber - 1
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub PreviousPage(ByVal fileName As String)
        Try

            pageNumber = pageNumber - 1

            If pageNumber <= pageCount AndAlso pageNumber <> 0 Then
                _documentViewer.Commands.Run(DocumentViewerCommands.PageGoto, pageNumber)
            Else
                If pageNumber = 0 Then
                    pageNumber = 1
                Else
                    pageNumber = pageNumber + 1
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub


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
    Dim own As New Window
    Public Sub RotateLeft(_fileFormat As RasterImageFormat, _bitsPerPixel As Integer)
        Try
            Dim cpage As Integer = _documentViewer.CurrentPageNumber
            Dim ns = New Integer() {cpage}
            _documentViewer.RotatePages({}, 90)
            _documentViewer.Commands.Run(DocumentViewerCommands.PageRotateCounterClockwise, ns)

            If Not PageChanges.ContainsKey(cpage) Then
                PageChanges.Add(cpage, -9000)
            Else
                PageChanges.Item(cpage) = PageChanges.Item(cpage) + (-9000)
            End If

            ' MsgBox("RotateLeft: " + _documentViewer.CurrentPageNumber.ToString)
        Catch ex As Exception
        End Try

        'Try
        '    CurrentFileName = _documentViewer.Document.GetDocumentFileName
        '    _codecs = New RasterCodecs()
        '    Dim image As RasterImage = _codecs.Load(CurrentFileName)

        '    ' Rotate the image by 45 degrees 
        '    'Dim command As RotateCommand = New RotateCommand()
        '    'command.Angle = 90 * 100
        '    'command.FillColor = New RasterColor(255, 255, 255)
        '    ' command.Flags = RotateCommandFlags.None

        '    Dim command As RotateFlipCommand = New RotateFlipCommand(RotateFlipType.Rotate90FlipXY)
        '    command.Run(image)

        '    ' Save it to disk 
        '    _codecs.Save(image, CurrentFileName, RasterImageFormat.Tif, 24)

        '    ' Clean Up 
        '    image.Dispose()
        'Catch ex As Exception
        'End Try

        'Try
        '    CurrentFileName = _documentViewer.Document.GetDocumentFileName
        '    ' MsgBox("Source File: " + CurrentFileName)
        '    _codecs = New RasterCodecs()
        '    Dim viewer1 As RasterImage
        '    Dim info As CodecsImageInfo
        '    Dim loader As New ImageFileLoader()
        '    info = _codecs.GetInformation(CurrentFileName, True)
        '    For i As Integer = 1 To info.TotalPages
        '        loader = New ImageFileLoader()
        '        _codecs = New RasterCodecs()
        '        If (loader.Load(own, _codecs, True, i, i, CurrentFileName)) Then
        '            If Not info Is Nothing Then
        '                loader.Image.MakeRegionEmpty()
        '                If pageNumber = i Then
        '                    Dim command As RasterCommand = CType(Activator.CreateInstance(GetType(RotateCommand)), RasterCommand)
        '                    Dim cmd As RotateCommand = CType(command, RotateCommand)
        '                    Dim eighth As Integer = CType(loader.Image.Width / 8, Integer)
        '                    cmd.Angle = -9000
        '                    Dim a As RasterImageChangedFlags = cmd.Run(loader.Image)
        '                    If Not IsNothing(viewer1) Then
        '                        viewer1.AddPage(loader.Image)
        '                    Else
        '                        viewer1 = loader.Image
        '                    End If
        '                Else
        '                    If Not IsNothing(viewer1) Then
        '                        viewer1.AddPage(loader.Image)
        '                    Else
        '                        viewer1 = loader.Image
        '                    End If
        '                End If
        '            End If
        '            _codecs.Dispose()
        '        End If
        '    Next
        '    _codecs = New RasterCodecs()
        '    _codecs.Save(viewer1, CurrentFileName, _fileFormat, _bitsPerPixel, 1, pageCount, 1, CodecsSavePageMode.Overwrite)
        'Catch ex As Exception

        'End Try

    End Sub
    Public Sub RotateRight(_fileFormat As RasterImageFormat, _bitsPerPixel As Integer)
        Try
            Dim cpage As Integer = _documentViewer.CurrentPageNumber
            Dim ns = New Integer() {cpage}
            _documentViewer.Commands.Run(DocumentViewerCommands.PageRotateClockwise, ns)
            _documentViewer.GotoPage(cpage)
            If Not PageChanges.ContainsKey(cpage) Then
                PageChanges.Add(cpage, 9000)
            Else
                PageChanges.Item(cpage) = PageChanges.Item(cpage) + 9000
            End If
        Catch ex As Exception
        End Try

        'Try
        '    CurrentFileName = _documentViewer.Document.GetDocumentFileName
        '    _codecs = New RasterCodecs()
        '    Dim options As New LoadDocumentOptions()
        '    Using document As Leadtools.Document.LEADDocument = _documentViewer.Document
        '        Dim page As DocumentPage
        '        If document.Pages.Count >= 1 Then
        '            Dim image As RasterImage = _codecs.Load(CurrentFileName, 24, CodecsLoadByteOrder.Rgb, 1, document.Pages.Count)
        '            Dim command As RotateFlipCommand = New RotateFlipCommand(RotateFlipType.Rotate90FlipNone)

        '            command.Run(image)

        '            ' Save it to disk 
        '            _codecs.Save(image, CurrentFileName, RasterImageFormat.Tif, 24)

        '            ' Clean Up 
        '            image.Dispose()
        '        End If
        '    End Using
        'Catch ex As Exception

        'End Try

        'Try

        '    CurrentFileName = _documentViewer.Document.GetDocumentFileName
        '    _codecs = New RasterCodecs()
        '    Dim viewer1 As RasterImage
        '    Dim info As CodecsImageInfo
        '    Dim loader As New ImageFileLoader()
        '    info = _codecs.GetInformation(CurrentFileName, True)
        '    For i As Integer = 1 To info.TotalPages
        '        loader = New ImageFileLoader()
        '        _codecs = New RasterCodecs()
        '        If (loader.Load(own, _codecs, True, i, i, CurrentFileName)) Then
        '            If Not info Is Nothing Then
        '                loader.Image.MakeRegionEmpty()
        '                If pageNumber = i Then
        '                    Dim command As RasterCommand = CType(Activator.CreateInstance(GetType(RotateCommand)), RasterCommand)
        '                    Dim cmd As RotateCommand = CType(command, RotateCommand)
        '                    Dim eighth As Integer = CType(loader.Image.Width / 8, Integer)
        '                    cmd.Angle = 9000
        '                    Dim a As RasterImageChangedFlags = cmd.Run(loader.Image)
        '                    If Not IsNothing(viewer1) Then
        '                        viewer1.AddPage(loader.Image)
        '                    Else
        '                        viewer1 = loader.Image
        '                    End If
        '                Else
        '                    If Not IsNothing(viewer1) Then
        '                        viewer1.AddPage(loader.Image)
        '                    Else
        '                        viewer1 = loader.Image
        '                    End If
        '                End If
        '            End If
        '            _codecs.Dispose()
        '        End If
        '    Next
        '    _codecs = New RasterCodecs()
        '    _codecs.Save(viewer1, CurrentFileName, _fileFormat, _bitsPerPixel, 1, pageCount, 1, CodecsSavePageMode.Overwrite)
        'Catch ex As Exception
        'End Try
    End Sub

    Public Sub ZoomIn()
        Try
            _documentViewer.Commands.Run(DocumentViewerCommands.ViewZoomIn, Nothing)
        Catch ex As Exception
        End Try
    End Sub
    Public Sub ZoomOut()
        Try
            _documentViewer.Commands.Run(DocumentViewerCommands.ViewZoomOut, Nothing)
        Catch ex As Exception
        End Try
    End Sub
    Public Sub Stretch()
        Try

            _documentViewer.Commands.Run(DocumentViewerCommands.ViewFitPage, Nothing)
            gettext = False
        Catch ex As Exception
        End Try
    End Sub
    Public Sub FitAlways()
        Try

            _documentViewer.Commands.Run(DocumentViewerCommands.ViewActualSize, Nothing)
            gettext = False
        Catch ex As Exception
        End Try
    End Sub
    Public Sub FitWidth()
        Try

            _documentViewer.Commands.Run(DocumentViewerCommands.ViewFitWidth, Nothing)
            gettext = False
        Catch ex As Exception
        End Try
    End Sub
    Public Sub MagnifyGlass()
        Try

            _documentViewer.Commands.Run(DocumentViewerCommands.InteractiveMagnifyGlass, Nothing)
            gettext = False
        Catch ex As Exception
        End Try
    End Sub
    Public Sub Pan()
        Try

            _documentViewer.Commands.Run(DocumentViewerCommands.InteractivePan, Nothing)
            gettext = False
        Catch ex As Exception
        End Try
    End Sub
    Public Sub ZoomTo()
        Try


            _documentViewer.Commands.Run(DocumentViewerCommands.InteractiveZoomTo, Nothing)
            gettext = False
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

#Region "Document Viewer"
    Private Sub InitDocumentViewer()
        Dim createOptions As DocumentViewerCreateOptions = New DocumentViewerCreateOptions()

        ' Set the UI part where the main view is displayed
        createOptions.ViewContainer = MainGrid
        ' Set the UI part where the thumbnails are displayed
        'createOptions.ThumbnailsContainer = _thumbnailsTabPageGrid
        ' Set the UI part where the bookmarks are displayed
        'createOptions.BookmarksContainer = _bookmarksTabPageGrid
        createOptions.UseAnnotations = True

        ' Now create the viewer
        _documentViewer = DocumentViewerFactory.CreateDocumentViewer(createOptions)
        ' Set the user name
        _documentViewer.UserName = Environment.UserName
        ' We prefer SVG viewing (if supported)
        _documentViewer.View.PreferredItemType = DocumentViewerItemType.Svg

        Dim imageViewer As ImageViewer = _documentViewer.View.ImageViewer
        imageViewer.Background = SystemColors.AppWorkspaceBrush

        ' Helps with debugging of there was a rendering error
        AddHandler imageViewer.RenderError, Sub(sender, e)
                                                Dim message As String = String.Format("Error during render item {0} part {1}: {2}", If(e.Item IsNot Nothing, imageViewer.Items.IndexOf(e.Item), -1), e.Part, e.[Error].Message)
                                                '_outputWindow.AddTextLine(message, UI.OutputWindow.LineStyle.[Error])

                                            End Sub

        _documentViewer.Text.AutoGetText = True
        '_documentViewer.Commands.Run(DocumentViewerCommands.InteractiveAutoPan)
        '' Comment this to use the default SelectText interactive mode
        '_documentViewer.Commands.Run(DocumentViewerCommands.InteractivePanZoom)
        ''Single Layout of Viewer
        '_documentViewer.Commands.Run(DocumentViewerCommands.LayoutSingle)

        _documentViewer.Commands.Run(DocumentViewerCommands.ViewFitWidth, Nothing)
        ' Set enable tooltip default value
        '_documentViewer.Annotations.AutomationManager.EnableToolTip = False

        ' See if we need to enable inertia scroll
        'If _preferences.EnableInertiaScroll Then
        '    ToggleInertiaScroll(True)
        'End If

        AddHandler _documentViewer.Operation, AddressOf _documentViewer_Operation

        AddHandler _documentViewer.View.ImageViewer.PostRender, AddressOf ImageViewer_PostRender
        If _documentViewer.Thumbnails IsNot Nothing Then
            AddHandler _documentViewer.Thumbnails.ImageViewer.PostRender, AddressOf ImageViewer_PostRender
        End If
    End Sub
    Private Sub _documentViewer_Operation(sender As Object, e As DocumentViewerOperationEventArgs)
        Dim updater As Action(Of DocumentViewerOperationEventArgs) = Sub(args As DocumentViewerOperationEventArgs)
                                                                         ' If we have an error, show it
                                                                         If args.[Error] IsNot Nothing Then
                                                                             Dim message As String = String.Format("Error in {0}{1} operation. {2}", If(args.IsPostOperation, "Post-", "Pre-"), args.Operation, args.[Error].Message)
                                                                             '_outputWindow.AddTextLine(message, UI.OutputWindow.LineStyle.[Error])
                                                                         End If

                                                                         '
                                                                         '* Updating the UI state is expensive - we check UI elements for their ability
                                                                         '* to run commands based on the state of the DocumentViewer.
                                                                         '* So we must approve each operation that we want to update the UI for.
                                                                         '
                                                                         Dim updateUIState As Boolean = False
                                                                         '
                                                                         '* Some operations don't need to be logged, either
                                                                         '
                                                                         Dim logOperationsInfo As Boolean = False

                                                                         Select Case args.Operation
                                                                             Case DocumentViewerOperation.GetPage, DocumentViewerOperation.GotoPage, DocumentViewerOperation.GetAnnotations, DocumentViewerOperation.RenderItemPlaceholder, DocumentViewerOperation.AutomationStateChanged
                                                                                 updateUIState = True

                                                                             Case DocumentViewerOperation.RenderSelectedText
                                                                                 If (args.IsPostOperation) Then
                                                                                     updateUIState = True
                                                                                 End If

                                                                             Case Else
                                                                         End Select

                                                                         Dim sb As StringBuilder = New StringBuilder()
                                                                         Dim documentViewer As Global.Leadtools.Document.Viewer.DocumentViewer = TryCast(sender, Global.Leadtools.Document.Viewer.DocumentViewer)
                                                                         Dim document As Leadtools.Document.LEADDocument = If((documentViewer IsNot Nothing), documentViewer.Document, Nothing)
                                                                         If (Not documentViewer Is Nothing) Then
                                                                             document = documentViewer.Document
                                                                         Else
                                                                             document = Nothing
                                                                         End If

                                                                         If True Then
                                                                             sb.AppendFormat("{0}{1} operation", If(args.IsPostOperation, "Post-", "Pre-"), args.Operation)
                                                                         End If

                                                                         Select Case args.Operation
                                                                             Case DocumentViewerOperation.RunCommand
                                                                                 updateUIState = True
                                                                                 logOperationsInfo = True
                                                                                 If True Then
                                                                                     Dim command As DocumentViewerCommand = TryCast(args.Data1, DocumentViewerCommand)
                                                                                     sb.AppendFormat(" Command:{0}", command.Name)

                                                                                     If args.IsPostOperation AndAlso command.Name = DocumentViewerCommands.InteractiveSelectText Then
                                                                                         ' Check if we have any text
                                                                                         CanPerformTextOperation("Cannot select text", True)
                                                                                     End If
                                                                                 End If
                                                                                 Exit Select

                                                                             Case DocumentViewerOperation.LoadingThumbnails, DocumentViewerOperation.LoadingAnnotations, DocumentViewerOperation.LoadingBookmarks
                                                                                 Exit Select

                                                                             Case DocumentViewerOperation.PagesAdded, DocumentViewerOperation.PagesRemoved
                                                                                 updateUIState = True
                                                                                 logOperationsInfo = True
                                                                                 If e.IsPostOperation Then
                                                                                     If _documentViewer.Annotations IsNot Nothing Then
                                                                                         'HandleContainersAddedOrRemoved()
                                                                                     End If
                                                                                     'UpdateDocumentSetUIState()
                                                                                 End If
                                                                                 Exit Select

                                                                             Case DocumentViewerOperation.GetText

                                                                                 ' Have to wait for it to finish. So show the busy dialog
                                                                                 If Not args.IsPostOperation Then
                                                                                     If _isInsideBusyOperation Then
                                                                                         ShowBusyDialog(True, String.Format("Getting text for page {0}", args.PageNumber))
                                                                                     Else
                                                                                         ' This was not requested by us, cancel it and start to get the text ourselves
                                                                                         args.Abort = True
                                                                                         Me.Dispatcher.BeginInvoke(CType(Sub() GetPagesText(args.PageNumber), Action))
                                                                                         '   _documentViewer.Commands.Run(DocumentViewerCommands.TextGet, args.PageNumber)
                                                                                     End If
                                                                                 Else
                                                                                     updateUIState = True
                                                                                     logOperationsInfo = True

                                                                                     ' When we are done, invalidate the items
                                                                                     If args.PageNumber <> 0 Then
                                                                                         _documentViewer.View.ImageViewer.InvalidateItemByIndex(args.PageNumber - 1)
                                                                                     Else
                                                                                         _documentViewer.View.ImageViewer.InvalidateVisual()
                                                                                     End If

                                                                                     If _documentViewer.Thumbnails IsNot Nothing Then
                                                                                         If args.PageNumber <> 0 Then
                                                                                             _documentViewer.Thumbnails.ImageViewer.InvalidateItemByIndex(args.PageNumber - 1)
                                                                                         Else
                                                                                             _documentViewer.Thumbnails.ImageViewer.InvalidateVisual()
                                                                                         End If
                                                                                     End If
                                                                                 End If

                                                                                 Exit Select
                                                                             Case DocumentViewerOperation.TextSelectionChanged
                                                                                 Try
                                                                                     Dim tst = _documentViewer.Text.GetSelectedText(args.PageNumber)
                                                                                     If tst <> "" Then
                                                                                         ' MessageBox.Show("Text Selection Changed : " + tst)
                                                                                         ' tst = ""
                                                                                         'UI.Helper.ShowInformation(Me, tst)
                                                                                     End If

                                                                                 Catch ex As Exception

                                                                                 End Try
                                                                                 Exit Select
                                                                             Case DocumentViewerOperation.CreateAutomation
                                                                                 ' After the document viewer creates the automation object, we need to hook to some events
                                                                                 If args.IsPostOperation Then
                                                                                     HandleCreateAutomation()
                                                                                 End If
                                                                                 Exit Select

                                                                             Case DocumentViewerOperation.DestroyAutomation
                                                                                 ' Before the document viewer destroys the automation object, we need to unhook from the events
                                                                                 If Not args.IsPostOperation Then
                                                                                     HandleDestroyAutomation()
                                                                                 End If
                                                                                 Exit Select

                                                                             Case DocumentViewerOperation.RunLink
                                                                                 updateUIState = True
                                                                                 logOperationsInfo = True
                                                                                 If args.IsPostOperation AndAlso args.[Error] Is Nothing Then
                                                                                     ' Get the link and check if its an external one
                                                                                     Dim link As Leadtools.Document.DocumentLink = CType(args.Data1, Leadtools.Document.DocumentLink)
                                                                                     If link.LinkType = Leadtools.Document.DocumentLinkType.Value AndAlso Not String.IsNullOrEmpty(link.Value) Then
                                                                                         '    sb.AppendFormat(" Running link value:" + link.Value)
                                                                                         RunValueLink(link.Value)
                                                                                     End If
                                                                                 End If
                                                                                 Exit Select

                                                                             Case DocumentViewerOperation.HoverLink
                                                                                 updateUIState = True
                                                                                 logOperationsInfo = True
                                                                                 If args.IsPostOperation Then
                                                                                     If args.Data1 IsNot Nothing Then
                                                                                         ' We are hovered over a new link, can show a tooltip for example
                                                                                         ' This demo will just dump the link info

                                                                                         Dim link As Leadtools.Document.DocumentLink = CType(args.Data1, Leadtools.Document.DocumentLink)

                                                                                         If link.LinkType = Leadtools.Document.DocumentLinkType.TargetPage Then
                                                                                             sb.AppendFormat(" Link with target page {0}", link.Target.PageNumber)
                                                                                         Else
                                                                                             sb.AppendFormat(" Link with value {0}", link.Value)
                                                                                         End If
                                                                                     Else
                                                                                         ' We are not hovering over any links any more, can hide the tooltip for example
                                                                                         sb.Append(" No link")
                                                                                     End If
                                                                                 End If
                                                                                 Exit Select

                                                                             Case DocumentViewerOperation.PagesDisabledEnabled
                                                                                 updateUIState = True
                                                                                 logOperationsInfo = True
                                                                                 If args.IsPostOperation Then
                                                                                     'HandleAnnotationsPagesDisabledEnabled()
                                                                                 End If
                                                                                 Exit Select

                                                                             Case Else

                                                                                 Exit Select
                                                                         End Select

                                                                         If True Then
                                                                             '_outputWindow.AddTextLine(sb.ToString())
                                                                         End If

                                                                         If args.IsPostOperation And updateUIState Then
                                                                             'Me.UpdateUIState()
                                                                         End If

                                                                     End Sub

        ' Try to abort before doing anything
        If _isInsideBusyOperation Then
            If IsBusyDialogCancelled Then
                e.Abort = True
            End If
        End If

        If Not Dispatcher.CheckAccess() Then
            Me.Dispatcher.BeginInvoke(CType(updater, Action(Of DocumentViewerOperationEventArgs)), New Object() {e})
        Else
            updater(e)
        End If
    End Sub



    Private ReadOnly Property IsBusyDialogCancelled() As Boolean
        Get
            Return _busyDialog IsNot Nothing AndAlso _busyDialog.IsCancelled
        End Get
    End Property
    Private Sub ShowBusyDialog(enableCancellation As Boolean, description As String)
        If _busyDialog Is Nothing Then
            _busyDialog = New UI.BusyDialog()
            _busyDialog.Title = "ezofis Capture"
            _busyDialog.EnableCancellation = enableCancellation

            _busyDialog.Show()
        End If

        _busyDialog.UpdateDescription(description)
    End Sub

    Private Sub ImageViewer_PostRender(sender As Object, e As ImageViewerRenderEventArgs)
        If e.Context Is Nothing Then
            Return
        End If

        Dim imageViewer As ImageViewer = TryCast(sender, ImageViewer)

        For Each item As ImageViewerItem In imageViewer.Items
            If Not imageViewer.IsItemVisible(item, ImageViewerItemPart.Item) Then
                Continue For
            End If

            Dim document As Leadtools.Document.LEADDocument = _documentViewer.Document
            If document Is Nothing Then
                Return
            End If

            Dim isView As Boolean = imageViewer Is _documentViewer.View.ImageViewer

            Dim showTextIndicators As Boolean = True
            Dim pageNumber As Integer = imageViewer.Items.IndexOf(item) + 1
            Dim page As Leadtools.Document.DocumentPage = document.Pages(pageNumber - 1)

            Dim isDisabled As Boolean = page.IsDeleted

            If (Not showTextIndicators) AndAlso (Not isDisabled) Then
                Return
            End If

            Dim context As DrawingContext = e.Context
            Dim transform As LeadMatrix = imageViewer.GetItemImageTransform(item)
            Dim imageSize As LeadSize = item.ImageSize
            Dim bounds As LeadRectD = LeadRectD.Create(0, 0, imageSize.Width, imageSize.Height)
            Dim corners As LeadPointD() = {LeadPointD.Create(bounds.Left, bounds.Top), LeadPointD.Create(bounds.Right, bounds.Top), LeadPointD.Create(bounds.Right, bounds.Bottom), LeadPointD.Create(bounds.Left, bounds.Bottom)}

            transform.TransformPoints(corners)

            If (showTextIndicators) Then
                ' render a small T at the top-right corner
                Dim hasText As Boolean = _documentViewer.Text.HasDocumentPageText(pageNumber)

                ' Get the top-right point
                Dim topRight As LeadPointD = corners(0)
                For i As Integer = 1 To corners.Length - 1
                    If corners(i).X > topRight.X Then
                        topRight.X = corners(i).X
                    End If
                    If corners(i).Y < topRight.Y Then
                        topRight.Y = corners(i).Y
                    End If
                Next

                Dim textSize As LeadSizeD = LeadSizeD.Create(pageHasTextFormattedText.Width, pageHasTextFormattedText.Height)
                Dim rect As Rect = New Rect(CInt(topRight.X - textSize.Width - 4.0), CInt(topRight.Y), CInt(textSize.Width + 0.5), CInt(textSize.Height + 0.5))

                context.DrawRectangle(_alphaBrush, Nothing, rect)
                context.DrawText(If((hasText), pageHasTextFormattedText, pageHasNoTextFormattedText), New Point(rect.X, rect.Y))
            End If

            If isDisabled Then
                Dim size As Double
                If isView Then
                    size = 30
                Else
                    size = 20
                End If
                Dim transformedBounds As LeadRectD = GeometryTools.GetBoundingRect(corners)
                If isView Then
                    ' Keep the size reasonable when the page scales
                    size = GetScaledRender(bounds, 0.2, size)
                End If

                ' Get the top-left point
                Dim topLeft As LeadPointD = corners(0)
                Dim i As Integer = 1
                Do While i < corners.Length
                    If corners(i).X < topLeft.X Then
                        topLeft.X = corners(i).X
                    End If
                    If corners(i).Y < topLeft.Y Then
                        topLeft.Y = corners(i).Y
                    End If
                    i += 1
                Loop

                Dim triangleLength As Double = (size * 1.8)

                ' This code draws filled triangle with DarkRed color on the Top-Left corner of the viewer
                Dim start As New Point(topLeft.X, topLeft.Y)
                Dim segments As LineSegment() = New LineSegment() {New LineSegment(New Point(topLeft.X + triangleLength, topLeft.Y), True), New LineSegment(New Point(topLeft.X, topLeft.Y + triangleLength), True)}

                Dim triangleFigure As New PathFigure(start, segments, True)
                Dim geometry As New PathGeometry(New PathFigure() {triangleFigure})
                context.DrawGeometry(Brushes.DarkRed, Nothing, geometry)

                ' This code draws the diabled image on the Top-Left corner of the viewer
                Dim disabledPageBitmap As BitmapImage = New BitmapImage(New Uri("pack://application:,,,/" & System.Reflection.Assembly.GetExecutingAssembly().GetName().Name & ";component/" & "Resources/DisabledPage.png", UriKind.Absolute))
                context.DrawImage(disabledPageBitmap, New Rect(topLeft.X, topLeft.Y, triangleLength / 2.0F, triangleLength / 2.0F))
                context.DrawRectangle(_alphaBrush, Nothing, New Rect(transformedBounds.X, transformedBounds.Y, transformedBounds.Width, transformedBounds.Height))
            End If
        Next
    End Sub
    Public Function CanPerformTextOperation(operation As String, noPages As Boolean) As Boolean
        If Not _documentViewer.Text.AutoGetText AndAlso Not _documentViewer.Text.HasAnyDocumentPageText() Then
            ' This means auto-get text is off and we never got any text, warn the user
            'Dim message As String = Helper.AddTextNote(operation, noPages)
            'Helper.ShowInformation(Me, message)
            Return False
        End If

        Return True
    End Function
    Private Sub GetPagesText(pageNumber As Integer)
        ' This could take some time, so run it as a busy operation
        Me.BeginBusyOperation()

        Dim thisOperation As DocumentViewerAsyncOperation = New DocumentViewerAsyncOperation() With {
           .[Error] = Sub(operation As DocumentViewerAsyncOperation, [error] As Exception)
                          MessageBox.Show([error].ToString())
                      End Sub,
           .Always = Sub(operation As DocumentViewerAsyncOperation)
                         Me.EndBusyOperation()

                     End Sub
         }

        _documentViewer.Commands.RunAsync(thisOperation, DocumentViewerCommands.TextGet, pageNumber)
    End Sub
    Public Sub BeginBusyOperation()
        ' Get ready ...
        _isInsideBusyOperation = True
        IsEnabled = False
    End Sub
    Public Sub EndBusyOperation()
        If Not Dispatcher.CheckAccess() Then
            Dispatcher.BeginInvoke(New Action(AddressOf EndBusyOperation))
            Return
        End If

        If _isInsideBusyOperation Then
            _isInsideBusyOperation = False

            IsEnabled = True

            HideBusyDialog()
        End If
    End Sub

    Private Sub HideBusyDialog()
        If _busyDialog Is Nothing Then
            Return
        End If

        _busyDialog.IsWorking = False
        _busyDialog.Close()
        _busyDialog = Nothing
    End Sub

    Private Sub RunValueLink(linkValue As String)
        ' Check if this is an email address
        'If Not linkValue.ToLower().StartsWith("mailto:") AndAlso _emailRegex.IsMatch(linkValue) Then
        '    ' Yes,
        '    linkValue = Convert.ToString("mailto:") & linkValue
        'End If

        'Dim hasRun As Boolean = False

        'Try
        '    ' Try default
        '    Process.Start(linkValue)
        '    hasRun = True
        'Catch
        'End Try

        'If Not hasRun Then
        '    ' Error, ask use what to do with this, application
        '    'Using dlg As LinkValueDialog = New LinkValueDialog()
        '    '    dlg.LinkValue = linkValue
        '    '    dlg.ShowDialog(Me)
        '    'End Using
        'End If
    End Sub
    Public Sub LoadDocumentFromazureFile(documentFileName As String)
        'Dim options As Document.LoadDocumentOptions = New Document.LoadDocumentOptions()
        'options.Cache = _cache
        'options.UseCache = _cache IsNot Nothing
        'options.AnnotationsUri = Nothing
        'options.LoadEmbeddedAnnotations = False
        'Dim doc As Document.LEADDocument
        'Dim srcpath = System.IO.Path.Combine("Settings", "Monitor", g_cabinet, documentFileName)
        'storageAccount = CloudStorageAccount.Parse(Constr)
        'clientFile = storageAccount.CreateCloudFileClient
        'ShareFile = clientFile.GetShareReference("ezindex")
        'Dim SAS = "?sv=2018-03-28&ss=bfqt&srt=sco&sp=rwdlacup&se=2020-10-11T15:41:23Z&st=2019-10-11T07:41:23Z&spr=https,http&sig=MmBlL77wpIYcijJHnppulMI7ONnCukntF43mgE3IIf0%3D"
        'Dim Path As New Uri("https://ezofisdiag733.file.core.windows.net/ezindex/" + srcpath + SAS)
        'If ShareFile.Exists() Then
        '    FoldArchive = ShareFile.GetRootDirectoryReference()
        '    FoldArchive = FoldArchive.GetDirectoryReference(IO.Path.GetDirectoryName(srcpath))
        '    cCloudFile = FoldArchive.GetFileReference(IO.Path.GetFileName(srcpath))
        '    If cCloudFile.Exists Then
        '        doc = Document.DocumentFactory.LoadFromUri(Path, options)
        '        '  doc = Document.DocumentFactory.LoadFromFile(documentFileName, options)
        '        If (doc IsNot Nothing) Then
        '            SetDocument(doc)
        '            pageCount = doc.Pages.OriginalPageCount
        '            pageNumber = 1
        '        End If
        '    Else
        '        'MessageBox.Show("This file is not exists on Azure")
        '        Dim obkwind As New PopupNotifier
        '        Dim savepath = System.Reflection.Assembly.GetEntryAssembly().Location
        '        savepath = IO.Path.GetDirectoryName(savepath)
        '        Dim source = savepath + "\err.png"
        '        obkwind.Image = System.Drawing.Image.FromFile(source)
        '        obkwind.ContentFont = New System.Drawing.Font("Tahoma", 10.0F)
        '        obkwind.Size = New System.Drawing.Size(600, 200)
        '        obkwind.ShowGrip = False
        '        obkwind.AnimationDuration = 8000
        '        obkwind.HeaderHeight = 20
        '        obkwind.Scroll = True
        '        obkwind.TitleFont = New System.Drawing.Font("Tahoma", 12.0F)
        '        obkwind.TitleText = "EZOFIS CAPTURE : NOTIFICATION"
        '        obkwind.ContentText = "This File is Not exists on Azure"
        '        obkwind.Popup()
        '    End If
        'End If
    End Sub

    Public Sub LoadDocumentFromFile(documentFileName As String)
        Try
            ClearDocument()
            Dim options As Document.LoadDocumentOptions = New Document.LoadDocumentOptions()
            options.Cache = _cache
            options.UseCache = _cache IsNot Nothing
            options.AnnotationsUri = Nothing
            options.LoadEmbeddedAnnotations = False
            Dim doc As Document.LEADDocument
            doc = Document.DocumentFactory.LoadFromFile(documentFileName, options)
            If (doc IsNot Nothing) Then
                SetDocument(doc)
                pageCount = doc.Pages.OriginalPageCount
                pageNumber = 1
            End If
        Catch ex As Exception
            MsgBox("Exception in LoadDocumentFromFile " & ex.Message)
        End Try
    End Sub


    Public Sub LoadDocumentFromFileWithPageNumber(documentFileName As String)
        Try
            ClearDocument()

            Dim options As Document.LoadDocumentOptions
            options = New Document.LoadDocumentOptions()
            options.Cache = _cache
            options.UseCache = _cache IsNot Nothing
            options.AnnotationsUri = Nothing
            options.LoadEmbeddedAnnotations = False

            Dim doc As Document.LEADDocument
            doc = Document.DocumentFactory.LoadFromFile(documentFileName, options)

            If (doc IsNot Nothing) Then
                SetDocument(doc)

                pageCount = doc.Pages.OriginalPageCount
                ' pageNumber = 1
                _documentViewer.Commands.Run(DocumentViewerCommands.PageGoto, pageNumber)
            End If
        Catch ex As Exception
            MsgBox("Exception in LoadDocumentFromFileWithPageNumber " & ex.Message)
        End Try
    End Sub

    Private Sub SetDocument(document As Document.LEADDocument)
        Try

            If document.IsEncrypted AndAlso Not document.IsDecrypted Then
                ' This document requires a password
                'DecryptDocument(document)

                ' If still, then dispose it and dont set it
                If document.IsEncrypted AndAlso Not document.IsDecrypted Then
                    document.Dispose()
                    Return
                End If
            End If

            FinishSetDocument(document)
        Catch ex As Exception
            MsgBox("Exception in SetDocument " & ex.Message)
        End Try
    End Sub


    Private Shared Function GetScaledRender(ByVal bounds As LeadRectD, ByVal maxSizeRatio As Double, ByVal original As Double) As Double
        Dim shortSide As Double = Math.Min(bounds.Width, bounds.Height)
        Dim sizeRatio As Double = Math.Min(maxSizeRatio, original / shortSide)
        original = sizeRatio * shortSide
        Return original
    End Function
    Private Sub FinishSetDocument(document As Document.LEADDocument)
        'document.Text.OcrEngine = ECMRightPane._ocrEngine
        _documentViewer.SetDocument(document)
    End Sub
    'Private Sub DecryptDocument(document As Document.LEADDocument)
    '    Dim done As Boolean = False

    '    While Not done
    '        Dim dlg As DocumentViewerDemo.UI.InputDialog = New DocumentViewerDemo.UI.InputDialog()
    '        dlg.Title = "Enter Password"
    '        dlg.ValueTitle = Nothing
    '        dlg.ValueDescription1 = "This document is encrypted. Enter the password to decrypt it"
    '        dlg.IsPassword = True
    '        If dlg.ShowDialog() = True Then
    '            Try
    '                ' Try the password
    '                If document.Decrypt(dlg.Value) Then
    '                    done = True
    '                Else
    '                    MsgBox("Invalid password")
    '                End If
    '            Catch ex As Exception
    '                MsgBox("From Decrypt : " + ex.ToString)
    '                done = True
    '            End Try
    '        Else
    '            ' Use canceled, so they dont want to load it, return
    '            done = True
    '        End If
    '    End While
    'End Sub
    Private Sub HandleCreateAutomation()
        Dim automation As AnnAutomation = _documentViewer.Annotations.Automation
        If automation Is Nothing Then
            Return
        End If
        AddHandler automation.OnShowContextMenu, AddressOf automation_OnShowContextMenu
    End Sub


    Private Sub automation_OnShowContextMenu(sender As Object, e As AnnAutomationEventArgs)
        Dim automation As AnnAutomation = _documentViewer.Annotations.Automation
        If automation Is Nothing Then
            Return
        End If

        Dim imageViewer As Global.Leadtools.Controls.ImageViewer = _documentViewer.View.ImageViewer

        If e IsNot Nothing AndAlso e.[Object] IsNot Nothing Then
            automation.Invalidate(LeadRectD.Empty)

        Else
            'ECMRightPane.ConMenu.PlacementTarget = imageViewer
            'ECMRightPane.ConMenu.IsOpen = True
            'ECMRightPane.ConMenu.Placement = Primitives.PlacementMode.MousePoint
        End If
    End Sub

    Private Sub DoSelectAllText()
        ' Check if we have any text or can get it automatically
        If Not CanPerformTextOperation("No text to select", True) Then
            Return
        End If

        'if (!_documentViewer.Text.HasDocumentPageText(0) && !
        'message = Helper.AddTextNote(message);

        Dim isSlow As Boolean = _documentViewer.Commands.IsSlow(DocumentViewerCommands.TextSelectAll, 0)

        If isSlow Then
            Me.BeginBusyOperation()
        End If

        Dim thisOperation As DocumentViewerAsyncOperation = New DocumentViewerAsyncOperation() With {
           .[Error] = Sub(operation As DocumentViewerAsyncOperation, [error] As Exception)
                          MessageBox.Show([error].ToString())

                      End Sub,
           .Always = Sub(operation As DocumentViewerAsyncOperation)
                         If isSlow Then
                             Me.EndBusyOperation()
                         End If

                     End Sub
         }

        _documentViewer.Commands.RunAsync(thisOperation, DocumentViewerCommands.TextSelectAll, 0)
    End Sub
    Private Sub HandleDestroyAutomation()
        If Not _documentViewer.HasDocument Then
            Return
        End If

        'RemoveAutomationTextBox(True)

        ' Get the automation object from the document viewer
        Dim automation As AnnAutomation = _documentViewer.Annotations.Automation
        If automation Is Nothing Then
            Return
        End If

        ' Remove it to the objects list
        'If _automationObjectsList IsNot Nothing Then
        '    _automationObjectsList.Automation = Nothing
        '    _automationObjectsList.ImageViewer = Nothing
        'End If

        Dim imageViewer As Global.Leadtools.Controls.ImageViewer = _documentViewer.View.ImageViewer
        ' Unhook from the events
        'RemoveHandler automationControl.AutomationTransformChanged, AddressOf automation_TransformChanged
        'RemoveHandler automation.SetCursor, AddressOf automation_SetCursor
        'RemoveHandler automation.RestoreCursor, AddressOf automation_RestoreCursor
        'RemoveHandler automation.ToolTip, AddressOf automation_ToolTip
        'RemoveHandler automation.OnShowObjectProperties, AddressOf automation_OnShowObjectProperties
        RemoveHandler automation.OnShowContextMenu, AddressOf automation_OnShowContextMenu
        'RemoveHandler automation.EditText, AddressOf automation_EditText
        'RemoveHandler automation.EditContent, AddressOf automation_EditContent
        'RemoveHandler automation.LockObject, AddressOf automation_LockObject
        'RemoveHandler automation.UnlockObject, AddressOf automation_UnlockObject
        'RemoveHandler automation.DeserializeObjectError, AddressOf automation_DeserializeObjectError
    End Sub
    'Private Sub automation_OnShowContextMenu(sender As Object, e As AnnAutomationEventArgs)
    '    Dim automation As AnnAutomation = _documentViewer.Annotations.Automation
    '    If automation Is Nothing Then
    '        Return
    '    End If

    '    Dim imageViewer As Global.Leadtools.Controls.ImageViewer = _documentViewer.View.ImageViewer

    '    If e IsNot Nothing AndAlso e.[Object] IsNot Nothing Then
    '        automation.Invalidate(LeadRectD.Empty)

    '        Dim annAutomationObject As AnnAutomationObject = e.[Object]
    '        Dim contextMenu As Annotations.Wpf.AnnObjectContextMenu = TryCast(annAutomationObject.ContextMenu, Annotations.Wpf.AnnObjectContextMenu)
    '        If annAutomationObject IsNot Nothing AndAlso contextMenu IsNot Nothing Then
    '            contextMenu.Automation = TryCast(sender, AnnAutomation)
    '            contextMenu.IsOpen = True
    '            contextMenu.PlacementTarget = imageViewer
    '        End If
    '    Else
    '        If _viewContextMenu Is Nothing Then
    '            _viewContextMenu = New UI.ViewContextMenu(_documentViewer, Nothing)
    '        End If

    '        _viewContextMenu.PlacementTarget = imageViewer
    '        _viewContextMenu.IsOpen = True
    '        _viewContextMenu.Placement = Primitives.PlacementMode.MousePoint
    '    End If
    'End Sub
    Private _viewPageRendersByIndex As SortedDictionary(Of Integer, Integer)
    Private _thumbnailsPageRendersByIndex As SortedDictionary(Of Integer, Integer)
    Private _countPageRenders As Boolean = False
    Private _cache As ObjectCache
    Private _alphaBrush As Brush = New SolidColorBrush(System.Windows.Media.Color.FromArgb(128, Colors.White.R, Colors.White.G, Colors.White.B))
    Private Shared _hasTextTypeface As New Typeface(New FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal)
    Private Shared _noTextTypeface As New Typeface(New FontFamily("Arial"), FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
    Private Shared pageHasTextFormattedText As New FormattedText("T", Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, _hasTextTypeface, 16, Brushes.Blue)
    Private Shared pageHasNoTextFormattedText As New FormattedText("T", Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, _noTextTypeface, 16, Brushes.Gray)
    Dim gettext As Boolean = False
    Private _busyDialog As UI.BusyDialog
    Private _isInsideBusyOperation As Boolean


#End Region
End Class
