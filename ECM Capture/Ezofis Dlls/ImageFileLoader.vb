Imports System
Imports System.Windows.Forms
Imports System.Text
Imports System.Drawing
Imports Leadtools
Imports Leadtools.Codecs
Imports Leadtools.WinForms.CommonDialogs.File

Public Class ImageFileLoader

    Private Shared _filterIndex As Integer = 1
    Private _fileName As String
    Private _filters() As RasterOpenDialogLoadFormat
    Private _image As RasterImage
    Private _loadOnlyOnePage As Boolean = False
    Private _firstPage As Integer
    Private _lastPage As Integer
    Private _showLoadPagesDialog As Boolean = False
    Private _showPdfOptions As Boolean = True


    Public Sub New()
    End Sub

    Public Property FileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal Value As String)
            _fileName = Value
        End Set
    End Property

    Public ReadOnly Property Image() As RasterImage
        Get
            Return _image
        End Get
    End Property

    Public Property ShowLoadPagesDialog() As Boolean
        Get
            Return _showLoadPagesDialog
        End Get
        Set(ByVal Value As Boolean)
            _showLoadPagesDialog = Value
        End Set
    End Property

    Public Property LoadOnlyOnePage() As Boolean
        Get
            Return _loadOnlyOnePage
        End Get
        Set(ByVal Value As Boolean)
            _loadOnlyOnePage = Value
        End Set
    End Property

    Public Shared Property FilterIndex() As Integer
        Get
            Return _filterIndex
        End Get
        Set(ByVal Value As Integer)
            _filterIndex = Value
        End Set
    End Property

    Public Property Filters() As RasterOpenDialogLoadFormat()
        Get
            Return _filters
        End Get
        Set(ByVal Value As RasterOpenDialogLoadFormat())
            _filters = Value
        End Set
    End Property
    Public ReadOnly Property FirstPage() As Integer
        Get
            Return _firstPage
        End Get
    End Property

    Public ReadOnly Property LastPage() As Integer
        Get
            Return _lastPage
        End Get
    End Property

    Public Property ShowPdfOptions() As Boolean
        Get
            Return _showPdfOptions
        End Get
        Set(ByVal Value As Boolean)
            _showPdfOptions = Value
        End Set
    End Property

    Public Function Load(ByVal owner As IWin32Window, ByVal codecs As RasterCodecs, ByVal autoLoad As Boolean) As Boolean
        Dim ofd As New RasterOpenDialog(codecs)

        ofd.DereferenceLinks = True
        ofd.CheckFileExists = False
        ofd.CheckPathExists = True
        ofd.EnableSizing = True
        ofd.Filter = Filters
        ofd.FilterIndex = _filterIndex
        ofd.LoadFileImage = False
        ofd.LoadOptions = False
        ofd.LoadRotated = True
        ofd.LoadCompressed = True
        ofd.Multiselect = False
        ofd.ShowGeneralOptions = True
        ofd.ShowLoadCompressed = True
        ofd.ShowLoadOptions = True
        ofd.ShowLoadRotated = True
        ofd.ShowMultipage = True
        ofd.ShowPdfOptions = ShowPdfOptions
        'ofd.ShowXpsOptions = ShowXpsOptions
        ofd.ShowPreview = True
        ofd.ShowProgressive = True
        ofd.ShowRasterOptions = True
        ofd.ShowTotalPages = True
        ofd.ShowDeletePage = True
        ofd.ShowFileInformation = True
        ofd.UseFileStamptoPreview = True
        ofd.PreviewWindowVisible = True
        ofd.Title = "SmartDocX Open Dialog"
        ofd.FileName = FileName

        Dim ok As Boolean = False

        If (ofd.ShowDialog(owner) = DialogResult.OK) Then
            Dim firstItem As RasterDialogFileData = ofd.OpenedFileData(0)
            FileName = firstItem.Name

            ok = True

            FilterIndex = ofd.FilterIndex

            Dim info As CodecsImageInfo

            Try
                info = codecs.GetInformation(FileName, True)
            Finally

            End Try

            If (info.Format = RasterImageFormat.RasPdf OrElse _
               info.Format = RasterImageFormat.RasPdfG31Dim OrElse _
               info.Format = RasterImageFormat.RasPdfG32Dim OrElse _
               info.Format = RasterImageFormat.RasPdfG4 OrElse _
               info.Format = RasterImageFormat.RasPdfJpeg OrElse _
               info.Format = RasterImageFormat.RasPdfJpeg422 OrElse _
               info.Format = RasterImageFormat.RasPdfJpeg411) Then
                If (Not codecs.Options.Pdf.IsEngineInstalled) Then
                    Dim dlg As New PdfEngineDialog
                    If (dlg.ShowDialog(owner) <> DialogResult.OK) Then
                        Return False
                    End If
                End If
            End If

            'Set the user Options
            codecs.Options.Load.Passes = firstItem.Passes
            codecs.Options.Load.Rotated = firstItem.LoadRotated
            codecs.Options.Load.Compressed = firstItem.LoadCompressed

            Select Case (firstItem.Options.FileType)
                Case RasterDialogFileOptionsType.Meta
                    'Set the user options
                    codecs.Options.Wmf.Load.XResolution = firstItem.Options.MetaOptions.XResolution
                    codecs.Options.Wmf.Load.YResolution = firstItem.Options.MetaOptions.XResolution

                Case RasterDialogFileOptionsType.Pdf
                    If (codecs.Options.Pdf.IsEngineInstalled) Then
                        'Set the user options
                        codecs.Options.Pdf.Load.DisplayDepth = firstItem.Options.PdfOptions.DisplayDepth
                        codecs.Options.Pdf.Load.GraphicsAlpha = firstItem.Options.PdfOptions.GraphicsAlpha

                        If (String.Empty <> firstItem.Options.PdfOptions.Password) Then
                            codecs.Options.Pdf.Load.Password = firstItem.Options.PdfOptions.Password
                        End If

                        codecs.Options.Pdf.Load.TextAlpha = firstItem.Options.PdfOptions.TextAlpha
                        codecs.Options.Pdf.Load.UseLibFonts = firstItem.Options.PdfOptions.UseLibFonts
                        codecs.Options.Pdf.Load.XResolution = firstItem.Options.PdfOptions.XResolution
                        codecs.Options.Pdf.Load.YResolution = firstItem.Options.PdfOptions.YResolution
                    End If
                Case RasterDialogFileOptionsType.Misc
                    Select Case (firstItem.FileInfo.Format)
                        Case RasterImageFormat.Jbig
                            'Set the user options
                            codecs.Options.Jbig.Load.Resolution = New Size( _
                               firstItem.Options.MiscOptions.XResolution, _
                               firstItem.Options.MiscOptions.YResolution)
                        Case RasterImageFormat.Cmw
                            'Set the user options
                            codecs.Options.Jpeg2000.Load.CmwResolution = New Size( _
                               firstItem.Options.MiscOptions.XResolution, _
                               firstItem.Options.MiscOptions.YResolution)

                        Case RasterImageFormat.Jp2
                            'Set the user options
                            codecs.Options.Jpeg2000.Load.Jp2Resolution = New Size( _
                               firstItem.Options.MiscOptions.XResolution, _
                               firstItem.Options.MiscOptions.YResolution)

                        Case RasterImageFormat.J2k
                            'Set the user options
                            codecs.Options.Jpeg2000.Load.J2kResolution = New Size( _
                               firstItem.Options.MiscOptions.XResolution, _
                               firstItem.Options.MiscOptions.YResolution)
                    End Select

            End Select

            Dim firstPage As Integer = 1
            Dim lastPage As Integer = 1

            If (ShowLoadPagesDialog) Then
                firstPage = 1
                lastPage = info.TotalPages

                If (firstPage <> lastPage) Then
                    Dim dlg As New ImageFileLoaderPagesDialog(info.TotalPages, LoadOnlyOnePage)
                    If (dlg.ShowDialog(owner) = DialogResult.OK) Then
                        If (dlg.AllPages) Then
                            lastPage = -1
                        Else
                            firstPage = dlg.FirstPage
                            lastPage = dlg.LastPage
                        End If
                    Else
                        ok = False
                    End If
                End If
            Else
                firstPage = firstItem.PageNumber
                lastPage = firstItem.PageNumber
            End If

            _firstPage = firstPage
            _lastPage = lastPage

            If (autoLoad AndAlso ok) Then

                Try
                    _image = codecs.Load(FileName, 0, CodecsLoadByteOrder.BgrOrGray, firstPage, lastPage)
                Finally

                End Try
            End If
        End If

        Return ok
    End Function
    Public Function Loadscanimage(ByVal owner As IWin32Window, ByVal codecs As RasterCodecs, ByVal autoLoad As Boolean, ByVal filename As String, ByVal totpage As Integer) As Boolean
        _image = codecs.Load(filename, 0, CodecsLoadByteOrder.BgrOrGray, 1, totpage)

        Return True
    End Function
    Public Function Load(ByVal owner As IWin32Window, ByVal codecs As RasterCodecs, ByVal autoLoad As Boolean, ByVal fpage As Integer, ByVal lpage As Integer, ByVal filename As String) As Boolean
        Dim ofd As New RasterOpenDialog(codecs)
        Dim ok As Boolean = False
        ok = True

        If (autoLoad AndAlso ok) Then

            Try
                _image = codecs.Load(filename, 0, CodecsLoadByteOrder.BgrOrGray, fpage, lpage)
            Finally

            End Try
        End If

        Return ok
    End Function
End Class
