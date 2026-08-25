Option Strict Off
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Text
Imports System.Threading
Imports Leadtools
Imports Leadtools.Codecs
Imports Leadtools.WinForms
Imports Leadtools.Barcode
Imports Leadtools.ImageProcessing
Imports System.Data
Imports System.Collections.Specialized
Imports System.Configuration

Public Class barcoderead
    'File Declarations
    Dim fileinfo As System.IO.FileInfo
    Dim FOLDER As System.IO.Directory
    Dim folderinfo As System.IO.DirectoryInfo
    Dim spfolinfo As DirectoryInfo
    Dim spfileinfo As FileInfo

    '**************** PDF/A parameter declarations******************
    Dim sAuthor As String
    Dim sTitle As String
    Dim sSubject As String
    Dim sRemarks As String
    Public dirinfo1 As String
    '**************** Object declarations******************

    'Dim db As New Database
    Dim cabinet As String
    Dim stblname As String = "Project Documents"
    '**************** General variables declarations******************
    Dim filename As String
    Dim sfilename As String
    Dim ext As String                       'Extension of the file.tif
    Dim fieldlist As ArrayList              'Structure stores each PDF/A parameter value
    Dim pdfpath As String                   'Return value from pdfconvertor
    Dim dbvalidate As Boolean               'To see Metadataset table name exists in the database
    'Lead tools
    Private _codecs As RasterCodecs
    Private _twainAvailable As Boolean = False
    Private _scanCount As Integer = 0
    Private _barcodeEngine As BarcodeEngine

    ' Global barcode read options 
    Public unit As BarcodeUnit
    Public readArea As Rectangle
    Public readMaxBarcodesCount As Integer
    Public barcodeRead1d As Barcode1d
    Public barcodeReadPDF As BarcodeReadPdf
    Public barcodeReadColor As BarcodeColor
    Public barcodeReadFlags As BarcodeReadFlags
    Public readLinearBarcodes As Boolean
    Public readDMBarcodes As Boolean
    Public readPDFBarcodes As Boolean
    Public readQRBarcodes As Boolean
    Public readBarcodeTypes As BarcodeSearchTypeFlags
    Public useColorRead As Boolean
    Public useRegion As Boolean
    Dim child As ChildForm = New ChildForm
    'Dim Mainfrm As New Split
    Dim vi As New Viewer
    ' Global barcode write options 
    Public barcodeWrite1d As Barcode1d
    Public barcodeWriteDM As BarcodeWriteDatamatrix
    Public barcodeWritePDF As BarcodeWritePdf
    Public barcodeWriteQR As BarcodeWriteQr
    Public barcodeWriteColor As BarcodeColor
    Public writeLinearBarcodes As Boolean
    Public writeDMBarcodes As Boolean
    Public writePDFBarcodes As Boolean
    Public writeQRBarcodes As Boolean
    Public barcodeWriteData As BarcodeData
    Public barcodeWriteFlags As BarcodeWriteFlags
    Public enbleTranparent As Boolean
    Public useColorWrite As Boolean

    Private _readDialogData As ReadDialogData
    Private _writeDialogData As WriteDialogData
    Private pasteCounter As Integer = 0

    Dim icurpage As Integer
    'Dim iarrlist As ArrayList
    Private _saver As ImageFileSaver
    Dim iprevpage As Integer = 1
    Dim inextpage As Integer
    Dim ipbarcodevalue As String
    Dim ipbarcodevalue1 As String
    Dim firstpage As String = ""
    Dim iarrlist As ArrayList
    Dim own As New System.Windows.Window
    Public dat As New DataTable
    Public _bitsPerPixel As Integer
    Public _fileFormat As RasterImageFormat
    Dim Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("Database"), NameValueCollection)

    Structure ElementType
        Public Barcodevalue As String
        Public curpage As Integer
        Public nextpage As Integer
        Public Sub New(ByVal name As String, ByVal svalue As Integer, ByVal pvalue As Integer)
            Me.Barcodevalue = name
            Me.nextpage = pvalue
            Me.curpage = svalue
        End Sub
    End Structure
    'Public Function barcodee(ByVal filename As String, ByVal dir As String) As DataTable
    '    Dim Fileformet = Appcon("FileFormet")
    '    If Fileformet = "1" Then
    '        '_fileFormat = FileFormets.CcittGroup4
    '        '_bitsPerPixel = 1
    '        'LeftPane._fileFormat = FileFormets.CcittGroup4
    '        'LeftPane._bitsPerPixel = 1
    '        _fileFormat = RasterImageFormat.TifLzw
    '        _bitsPerPixel = 4
    '    ElseIf Fileformet = "2" Then
    '        _fileFormat = FileFormets.TifJpeg
    '        _bitsPerPixel = 24
    '    Else
    '        _fileFormat = FileFormets.CcittGroup4
    '        _bitsPerPixel = 1
    '    End If
    '    ipbarcodevalue = ""
    '    iarrlist = New ArrayList
    '    InitClass()
    '    firstpage = ""
    '    Dim loader As New ImageFileLoader()
    '    Dim prgcnt As Integer

    '    dirinfo1 = dir
    '    sfilename = Common.GetlBSval(filename)
    '    RasterCodecs.Startup()

    '    _codecs = New RasterCodecs()
    '    Try
    '        loader.LoadOnlyOnePage = False
    '        Dim info2 As CodecsImageInfo
    '        info1 = info2
    '        info1 = _codecs.GetInformation(filename, True)
    '        Application.DoEvents()
    '        Dim pgcnt As Integer = 0


    '        For i As Integer = 1 To info1.TotalPages
    '            If i > 95 Then
    '                pgcnt = 0
    '            Else
    '                pgcnt = pgcnt + 1
    '            End If

    '            icurpage = i
    '            If (loader.Load(own, _codecs, True, i, i, filename)) Then
    '                child.MdiParent = vi
    '                child.InsertImage(loader.Image, loader.FileName)
    '                child.Show()

    '                miActionsRead()
    '            Else
    '                child.MdiParent = vi
    '                child.InsertImage(loader.Image, loader.FileName)
    '                child.Show()

    '                miActionsRead()

    '            End If
    '            prgcnt = prgcnt + 1
    '        Next

    '        iarrlist.Add(New ElementType(ipbarcodevalue, iprevpage, info1.TotalPages + 1))

    '        Dim SplitTable As New DataTable

    '        SplitTable = split()

    '        iarrlist = Nothing

    '        Return SplitTable
    '    Catch ex As Exception
    '        If ex.Message = "Thread was being aborted." Then

    '        Else

    '            MsgBox(ex.Message.ToString)
    '        End If

    '    Finally

    '    End Try
    'End Function
    Public Function barcodee(ByVal filename As String, ByVal dir As String, ByVal fileformat As RasterImageFormat, ByVal bitsperpixel As Integer) As DataTable
        _fileFormat = fileformat
        _bitsPerPixel = bitsperpixel
        ipbarcodevalue = ""
        iarrlist = New ArrayList
        InitClass()
        firstpage = ""
        Dim loader As New ImageFileLoader()
        Dim prgcnt As Integer

        dirinfo1 = dir
        sfilename = Common.GetlBSval(filename)
        RasterCodecs.Startup()

        _codecs = New RasterCodecs()
        Try
            loader.LoadOnlyOnePage = False
            Dim info2 As CodecsImageInfo
            info1 = info2
            info1 = _codecs.GetInformation(filename, True)
            Application.DoEvents()
            Dim pgcnt As Integer = 0


            For i As Integer = 1 To info1.TotalPages
                If i > 95 Then
                    pgcnt = 0
                Else
                    pgcnt = pgcnt + 1
                End If

                icurpage = i
                child = New ChildForm
                If (loader.Load(own, _codecs, True, i, i, filename)) Then
                    child.MdiParent = vi
                    child.InsertImage(loader.Image, loader.FileName)
                    child.Show()

                    miActionsRead()
                Else
                    child.MdiParent = vi
                    child.InsertImage(loader.Image, loader.FileName)
                    child.Show()

                    miActionsRead()

                End If
                prgcnt = prgcnt + 1
                child.Dispose()
                child.Close()
            Next


            iarrlist.Add(New ElementType(ipbarcodevalue, iprevpage, info1.TotalPages + 1))

            Dim SplitTable As New DataTable

            SplitTable = split()

            iarrlist = Nothing

            Return SplitTable
        Catch ex As Exception
            If ex.Message = "Thread was being aborted." Then

            Else

                MsgBox(ex.Message.ToString)
            End If

        Finally

        End Try
    End Function
    Public Sub barcodeeSplitWithTemp(ByVal filename As String, ByVal dir As String)
        ipbarcodevalue = ""
        iarrlist = New ArrayList
        InitClass()
        firstpage = ""
        Dim loader As New ImageFileLoader()
        Dim prgcnt As Integer

        dirinfo1 = dir
        sfilename = Common.GetlBSval(filename)
        RasterCodecs.Startup()

        _codecs = New RasterCodecs()
        Try
            loader.LoadOnlyOnePage = False
            Dim info2 As CodecsImageInfo
            info1 = info2
            info1 = _codecs.GetInformation(filename, True)
            Application.DoEvents()
            Dim pgcnt As Integer = 0


            For i As Integer = 1 To info1.TotalPages
                If i > 95 Then
                    pgcnt = 0
                Else
                    pgcnt = pgcnt + 1
                End If

                icurpage = i
                If (loader.Load(own, _codecs, True, i, i, filename)) Then
                    child.MdiParent = vi
                    child.InsertImage(loader.Image, loader.FileName)
                    child.Show()

                    miActionsRead()
                Else
                    child.MdiParent = vi
                    child.InsertImage(loader.Image, loader.FileName)
                    child.Show()

                    miActionsRead()

                End If
                prgcnt = prgcnt + 1
            Next

            iarrlist.Add(New ElementType(ipbarcodevalue, iprevpage, info1.TotalPages + 1))

            split()

            iarrlist = Nothing

        Catch ex As Exception
            If ex.Message = "Thread was being aborted." Then

            Else

                MsgBox(ex.Message.ToString)
            End If

        Finally

        End Try

    End Sub
    Public Sub GetbarcodeeValues(ByVal filename As String, ByRef barcodeVal As String)
        ipbarcodevalue1 = barcodeVal
        iarrlist = New ArrayList
        InitClass()
        firstpage = ""
        Dim loader As New ImageFileLoader()
        Dim prgcnt As Integer

        RasterCodecs.Startup()

        _codecs = New RasterCodecs()
        Try
            loader.LoadOnlyOnePage = False
            info1 = New CodecsImageInfo
            info1 = _codecs.GetInformation(filename, True)
            Application.DoEvents()
            Dim pgcnt As Integer = 0
            For i As Integer = 1 To info1.TotalPages
                If i > 95 Then
                    pgcnt = 0
                Else
                    pgcnt = pgcnt + 1
                End If

                icurpage = i
                If (loader.Load(own, _codecs, True, i, i, filename)) Then
                    child.MdiParent = vi
                    child.InsertImage(loader.Image, loader.FileName)
                    child.Show()

                    miActionsRead1()
                Else
                    child.MdiParent = vi
                    child.InsertImage(loader.Image, loader.FileName)
                    child.Show()

                    miActionsRead1()

                End If
                prgcnt = prgcnt + 1
            Next
            barcodeVal = ipbarcodevalue1


        Catch ex As Exception
            If ex.Message = "Thread was being aborted." Then

            Else

                MsgBox(ex.Message.ToString)
            End If

        Finally

        End Try

    End Sub
    Public Function miActionsRead() As Integer
        Dim num As Integer
        Try
            Dim barcodevalue As String = ""
            'RasterSupport.Unlock(RasterSupportType.Barcodes1D, "GJyrkLtc")
            RasterSupport.Unlock(RasterSupportType.Barcodes1D, "Fia8np2veF")
            BarcodeEngine.Startup(BarcodeMajorTypeFlags.Barcodes1d Or _
                  BarcodeMajorTypeFlags.Barcodes2dRead Or _
                  BarcodeMajorTypeFlags.Barcodes2dWrite Or _
                  BarcodeMajorTypeFlags.BarcodesDatamatrixRead Or _
                  BarcodeMajorTypeFlags.BarcodesDatamatrixWrite Or _
                  BarcodeMajorTypeFlags.BarcodesPdfRead Or _
                  BarcodeMajorTypeFlags.BarcodesPdfWrite Or _
                  BarcodeMajorTypeFlags.BarcodesQrRead Or _
                  BarcodeMajorTypeFlags.BarcodesQrWrite)
            Dim childFrm As ChildForm = CType(child, ChildForm)
            Dim area As Rectangle = Rectangle.Empty
            If (useRegion) Then
                area = Rectangle.Empty
            Else
                If (readArea = Rectangle.Empty) Then
                    area = New Rectangle(0, 0, childFrm.Viewer.Image.Width, childFrm.Viewer.Image.Height)
                Else
                    area = readArea
                End If
            End If

            Dim btype As Leadtools.Barcode.BarcodeSearchTypeFlags = CType([Enum].Parse(GetType(Leadtools.Barcode.BarcodeSearchTypeFlags), UserControl.BarCodeTypeFromCmb), Leadtools.Barcode.BarcodeSearchTypeFlags)
            Dim barcodeData1 As RasterCollection(Of BarcodeData)
            'For Each btype In System.[Enum].GetValues(GetType(Leadtools.Barcode.BarcodeSearchTypeFlags))
            Try
                barcodeData1 = _barcodeEngine.Read(childFrm.Viewer.Image, area, btype, unit, Leadtools.Barcode.BarcodeReadFlags.UseColors, readMaxBarcodesCount, barcodeRead1d, barcodeReadPDF, barcodeReadColor)
                Dim num3 As Integer = (barcodeData1.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num3)
                    Dim data As BarcodeData = barcodeData1.Item(i)
                    If BarcodeData.ConvertToStringArray(data.Data)(0).StartsWith(BarcodeStartsWith) And BarcodeData.ConvertToStringArray(data.Data)(0).EndsWith(BarcodeEndsWith) Then
                        barcodevalue = BarcodeData.ConvertToStringArray(data.Data)(0)
                    End If
                    i += 1
                Loop
                If barcodevalue <> "" Then
                    If (Me.icurpage <> 1) Then
                        Me.inextpage = Me.icurpage
                        Dim type As New ElementType(Me.ipbarcodevalue, Me.iprevpage, Me.inextpage)
                        Me.iarrlist.Add(type)
                    Else
                        Me.firstpage = barcodevalue
                    End If
                    Me.iprevpage = Me.icurpage
                    Me.ipbarcodevalue = barcodevalue
                    If (barcodevalue Is Nothing) Then
                        barcodecount = barcodecount
                        Return num
                    End If
                    barcodecount += 1
                End If
                ' Exit For
            Catch ex As Exception

            End Try
            'Next btype


            'Dim barcodeData1 As RasterCollection(Of BarcodeData) = _barcodeEngine.Read(childFrm.Viewer.Image, area, btype, unit, Leadtools.Barcode.BarcodeReadFlags.UseColors, readMaxBarcodesCount, barcodeRead1d, barcodeReadPDF, barcodeReadColor)


            Return num


        Catch ex As Exception

            child.barcodeDataCollection = Nothing
            'ipbarcodevalue = Nothing
            child.Invalidate(True)
            'Messager.ShowError(Me, ex)
            child.readBarcodes = False


        End Try
        'End If
    End Function
    Public Function miActionsRead1() As Integer
        Dim num As Integer
        Try
            Dim barcodevalue As String = ""
            'RasterSupport.Unlock(RasterSupportType.Barcodes1D, "GJyrkLtc")
            RasterSupport.Unlock(RasterSupportType.Barcodes1D, "Fia8np2veF")
            BarcodeEngine.Startup(BarcodeMajorTypeFlags.Barcodes1d Or _
                  BarcodeMajorTypeFlags.Barcodes2dRead Or _
                  BarcodeMajorTypeFlags.Barcodes2dWrite Or _
                  BarcodeMajorTypeFlags.BarcodesDatamatrixRead Or _
                  BarcodeMajorTypeFlags.BarcodesDatamatrixWrite Or _
                  BarcodeMajorTypeFlags.BarcodesPdfRead Or _
                  BarcodeMajorTypeFlags.BarcodesPdfWrite Or _
                  BarcodeMajorTypeFlags.BarcodesQrRead Or _
                  BarcodeMajorTypeFlags.BarcodesQrWrite)
            Dim childFrm As ChildForm = CType(child, ChildForm)
            Dim area As Rectangle = Rectangle.Empty
            If (useRegion) Then
                area = Rectangle.Empty
            Else
                If (readArea = Rectangle.Empty) Then
                    area = New Rectangle(0, 0, childFrm.Viewer.Image.Width, childFrm.Viewer.Image.Height)
                Else
                    area = readArea
                End If
            End If
            Dim btype As Leadtools.Barcode.BarcodeSearchTypeFlags = CType([Enum].Parse(GetType(Leadtools.Barcode.BarcodeSearchTypeFlags), UserControl.BarCodeTypeFromCmb), Leadtools.Barcode.BarcodeSearchTypeFlags)

            Dim barcodeData1 As RasterCollection(Of BarcodeData) = _barcodeEngine.Read(childFrm.Viewer.Image, area, btype, unit, Leadtools.Barcode.BarcodeReadFlags.UseColors, readMaxBarcodesCount, barcodeRead1d, barcodeReadPDF, barcodeReadColor)
            Dim num3 As Integer = (barcodeData1.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num3)
                Dim data As BarcodeData = barcodeData1.Item(i)
                If BarcodeData.ConvertToStringArray(data.Data)(0).StartsWith(BarcodeStartsWith) And BarcodeData.ConvertToStringArray(data.Data)(0).EndsWith(BarcodeEndsWith) And BarcodeData.ConvertToStringArray(data.Data)(0).Length = BarcodeLength Then
                    barcodevalue = BarcodeData.ConvertToStringArray(data.Data)(0)
                End If
                i += 1
            Loop
            If barcodevalue <> "" Then
                If (Me.icurpage <> 1) Then
                    Me.inextpage = Me.icurpage
                    Dim type As New ElementType(Me.ipbarcodevalue1, Me.iprevpage, Me.inextpage)
                    Me.iarrlist.Add(type)
                Else
                    Me.firstpage = barcodevalue
                End If
                Me.iprevpage = Me.icurpage
                Me.ipbarcodevalue1 = barcodevalue
                If (barcodevalue Is Nothing) Then
                    barcodecount = barcodecount
                    Return num
                End If
                barcodecount += 1
            End If

            Return num


        Catch ex As Exception

            child.barcodeDataCollection = Nothing
            'ipbarcodevalue = Nothing
            child.Invalidate(True)
            'Messager.ShowError(Me, ex)
            child.readBarcodes = False


        End Try
        'End If
    End Function
    Public Function miActionsRead2() As Integer
        Dim num As Integer
        Try
            Dim barcodevalue As String = ""
            'RasterSupport.Unlock(RasterSupportType.Barcodes1D, "GJyrkLtc")
            RasterSupport.Unlock(RasterSupportType.Barcodes1D, "Fia8np2veF")
            BarcodeEngine.Startup(BarcodeMajorTypeFlags.Barcodes1d Or _
                  BarcodeMajorTypeFlags.Barcodes2dRead Or _
                  BarcodeMajorTypeFlags.Barcodes2dWrite Or _
                  BarcodeMajorTypeFlags.BarcodesDatamatrixRead Or _
                  BarcodeMajorTypeFlags.BarcodesDatamatrixWrite Or _
                  BarcodeMajorTypeFlags.BarcodesPdfRead Or _
                  BarcodeMajorTypeFlags.BarcodesPdfWrite Or _
                  BarcodeMajorTypeFlags.BarcodesQrRead Or _
                  BarcodeMajorTypeFlags.BarcodesQrWrite)
            Dim childFrm As ChildForm = CType(child, ChildForm)
            Dim area As Rectangle = Rectangle.Empty
            If (useRegion) Then
                area = Rectangle.Empty
            Else
                If (readArea = Rectangle.Empty) Then
                    area = New Rectangle(0, 0, childFrm.Viewer.Image.Width, childFrm.Viewer.Image.Height)
                Else
                    area = readArea
                End If
            End If

            Dim btype As Leadtools.Barcode.BarcodeSearchTypeFlags = CType([Enum].Parse(GetType(Leadtools.Barcode.BarcodeSearchTypeFlags), UserControl.BarCodeTypeFromCmb), Leadtools.Barcode.BarcodeSearchTypeFlags)
            Dim barcodeData1 As RasterCollection(Of BarcodeData)
            'For Each btype In System.[Enum].GetValues(GetType(Leadtools.Barcode.BarcodeSearchTypeFlags))
            Try
                barcodeData1 = _barcodeEngine.Read(childFrm.Viewer.Image, area, btype, unit, Leadtools.Barcode.BarcodeReadFlags.UseColors, readMaxBarcodesCount, barcodeRead1d, barcodeReadPDF, barcodeReadColor)
                Dim num3 As Integer = (barcodeData1.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num3)
                    Dim data As BarcodeData = barcodeData1.Item(i)
                    If BarcodeData.ConvertToStringArray(data.Data)(0).StartsWith(BarcodeStartsWith) And BarcodeData.ConvertToStringArray(data.Data)(0).EndsWith(BarcodeEndsWith) And BarcodeData.ConvertToStringArray(data.Data)(0).Length = BarcodeLength Then
                        barcodevalue = BarcodeData.ConvertToStringArray(data.Data)(0)
                    End If
                    i += 1
                Loop
                If barcodevalue <> "" Then
                    If (Me.icurpage <> 1) Then
                        Me.inextpage = Me.icurpage
                        Dim type As New ElementType(Me.ipbarcodevalue, Me.iprevpage, Me.inextpage)
                        Me.iarrlist.Add(type)
                    Else
                        Me.firstpage = barcodevalue
                    End If
                    Me.iprevpage = Me.icurpage
                    Me.ipbarcodevalue = barcodevalue
                    If (barcodevalue Is Nothing) Then
                        barcodecount = barcodecount
                        Return num
                    End If
                    barcodecount += 1
                End If
                'Exit For
            Catch ex As Exception

            End Try
            'Next btype


            'Dim barcodeData1 As RasterCollection(Of BarcodeData) = _barcodeEngine.Read(childFrm.Viewer.Image, area, btype, unit, Leadtools.Barcode.BarcodeReadFlags.UseColors, readMaxBarcodesCount, barcodeRead1d, barcodeReadPDF, barcodeReadColor)


            Return num


        Catch ex As Exception

            child.barcodeDataCollection = Nothing
            'ipbarcodevalue = Nothing
            child.Invalidate(True)
            'Messager.ShowError(Me, ex)
            child.readBarcodes = False


        End Try
        'End If
    End Function
    Private Sub CreateChildForm(ByVal img As RasterImage, ByVal caption As String)
        Dim child As ChildForm = New ChildForm
        child.MdiParent = Me


        child.InsertImage(img, caption)
        child.Show()
    End Sub
    Public ReadOnly Property ActiveViewerForm() As ChildForm
        Get
            Return DirectCast(ActiveMdiChild, ChildForm)
        End Get
    End Property
    Private Property ImageToRun() As RasterImage
        Get
            Dim ActiveForm As ChildForm = ActiveViewerForm
            Return ActiveForm.Viewer.Image
        End Get
        Set(ByVal value As RasterImage)
            If (Not IsNothing(value)) Then
                Dim ActiveForm As ChildForm = ActiveViewerForm
                ActiveForm.Viewer.Image = value
            End If
        End Set
    End Property
    Private Sub InitClass()
        ' Default values for Global Read Dialog
        useRegion = False
        useColorRead = True
        unit = BarcodeUnit.ScanlinesPerPixels
        readMaxBarcodesCount = 0
        readBarcodeTypes = BarcodeSearchTypeFlags.None

        readArea = New Rectangle(0, 0, 0, 0)

        barcodeRead1d = New Barcode1d()
        barcodeReadPDF = New BarcodeReadPdf()
        barcodeReadColor = New BarcodeColor()

        barcodeRead1d.MinimumLength = 3
        barcodeRead1d.Granularity = 9
        barcodeRead1d.WhiteLines = 3
        barcodeRead1d.StandardFlags = Barcode1dStandardFlags.Barcode1dMsiModulo10 Or _
                                       Barcode1dStandardFlags.Barcode1dFast Or _
                                       Barcode1dStandardFlags.Barcode1dCode11C

        barcodeReadColor.BarColor = Color.FromArgb(0, 0, 0)
        barcodeReadColor.SpaceColor = Color.FromArgb(255, 255, 255)

        ' Default values for Global Write Dialog
        barcodeWriteColor = New BarcodeColor()
        barcodeWriteData = New BarcodeData()
        barcodeWritePDF = New BarcodeWritePdf()
        barcodeWrite1d = New Barcode1d()
        barcodeWriteDM = New BarcodeWriteDatamatrix()
        barcodeWriteQR = New BarcodeWriteQr()

        barcodeWriteFlags = barcodeWriteFlags.None
        enbleTranparent = False

        barcodeWrite1d.Direction = BarcodeDirectionFlags.Horizontal

        barcodeReadPDF.Direction = BarcodeDirectionFlags.LeftToRight
        barcodeWriteColor.BarColor = Color.FromArgb(0, 0, 0)
        barcodeWriteColor.SpaceColor = Color.FromArgb(255, 255, 255)

        barcodeWriteData.SearchType = BarcodeSearchTypeFlags.Barcode1dEan13
        barcodeWriteData.Unit = BarcodeUnit.ScanlinesPerPixels

        barcodeWrite1d.OutShowText = True
        barcodeWrite1d.ErrorCheck = False
        barcodeWriteDM.XModule = 30
        barcodeWrite1d.StandardFlags = Barcode1dStandardFlags.Barcode1dMsiModulo10 Or _
                                        Barcode1dStandardFlags.Barcode1dFast Or _
                                        Barcode1dStandardFlags.Barcode1dCode11C

        barcodeWriteQR.GroupNumber = 0
        barcodeWriteQR.GroupTotal = 0
        barcodeWriteQR.EccLevel = BarcodeQrEccLevel.LevelL
        barcodeWriteQR.XModule = 30

        ' Read Dialog structure
        _readDialogData.bUseRgn = useRegion
        _readDialogData.bUseColors = True
        _readDialogData.crBar = barcodeReadColor.BarColor
        _readDialogData.crSpace = barcodeReadColor.SpaceColor
        ' Standard 1D
        _readDialogData.bSearchAllStd1D = True
        _readDialogData.ulSearchStd1DType = BarcodeSearchTypeFlags.None
        _readDialogData.uFlags_Std1DPg = barcodeReadFlags.None

        _readDialogData.StdBar1D = New Barcode1d()
        _readDialogData.StdBar1D.Direction = BarcodeDirectionFlags.Horizontal
        _readDialogData.StdBar1D.MinimumLength = 3
        _readDialogData.StdBar1D.Granularity = 9
        _readDialogData.StdBar1D.WhiteLines = 3
        _readDialogData.StdBar1D.StandardFlags = Barcode1dStandardFlags.Barcode1dMsiModulo10 Or _
                                                 Barcode1dStandardFlags.Barcode1dFast Or _
                                                 Barcode1dStandardFlags.Barcode1dCode11C
        barcodeRead1d = _readDialogData.StdBar1D
        ' Patch Code
        _readDialogData.PatchBar1D = New Barcode1d()
        _readDialogData.PatchBar1D.Direction = BarcodeDirectionFlags.Horizontal
        _readDialogData.PatchBar1D.Granularity = 9
        ' PostNet
        _readDialogData.PostNetBar1D = New Barcode1d()
        _readDialogData.PostNetBar1D.Direction = BarcodeDirectionFlags.Horizontal
        _readDialogData.PostNetBar1D.Granularity = 9
        _readDialogData.PostNetBar1D.WhiteLines = 3
        _readDialogData.ulSearchPostNetType = BarcodeSearchTypeFlags.Barcode1dPlanet Or BarcodeSearchTypeFlags.Barcode1dPostnet
        _readDialogData.uFlags_PostNetPg = barcodeReadFlags.None
        ' PDF417
        _readDialogData.BarRPDF = New BarcodeReadPdf()
        _readDialogData.BarRPDF.Direction = BarcodeDirectionFlags.Horizontal
        ' Data Matrix
        _readDialogData.bSearchAllDM = True
        _readDialogData.ulSearchDMType = BarcodeSearchTypeFlags.None
        ' RSS14 Stacked
        _readDialogData.ulSearchStackedType = BarcodeSearchTypeFlags.Barcode1dRss14Stacked Or BarcodeSearchTypeFlags.Barcode1dRss14ExpandedStacked
        _readDialogData.StackedBar1D = New Barcode1d()
        _readDialogData.StackedBar1D.Direction = BarcodeDirectionFlags.Horizontal
        _readDialogData.StackedBar1D.Granularity = 9
        _readDialogData.uFlags_StackedPg = barcodeReadFlags.None
        ' 4 State
        _readDialogData.ulSearchStateType = BarcodeSearchTypeFlags.Barcode1dAustralianPost Or BarcodeSearchTypeFlags.Barcode1dRm4scc
        _readDialogData.StateBar1D = New Barcode1d()
        _readDialogData.StateBar1D.AdvancedFlags = Barcode1dAdvancedFlags.Barcode1dAustralianCifC
        _readDialogData.StateBar1D.Direction = BarcodeDirectionFlags.Horizontal
        _readDialogData.StateBar1D.Granularity = 9
        _readDialogData.uFlags_StatePg = barcodeReadFlags.None

        ' Write Dialog structure
        barcodeWriteData.Location = New Rectangle(50, 50, 300, 250) ' TabIndex = 0 i.e. Standard 1D

        _writeDialogData = New WriteDialogData()
        _writeDialogData.bUseColors = True
        _writeDialogData.crBar = barcodeWriteColor.BarColor
        _writeDialogData.crSpace = barcodeWriteColor.SpaceColor
        ' Standard 1D
        _writeDialogData.nStd1DCurTypeIndex = 0
        _writeDialogData.uStd1DCurTypeData = BarcodeSearchTypeFlags.Barcode1dUpcVersionA
        _writeDialogData.Std1DBarcode1D = New Barcode1d()
        _writeDialogData.Std1DBarcode1D.ErrorCheck = True
        _writeDialogData.Std1DBarcode1D.OutShowText = True
        _writeDialogData.Std1DBarcode1D.StandardFlags = Barcode1dStandardFlags.Barcode1dMsiModulo10 Or _
                                                        Barcode1dStandardFlags.Barcode1dFast Or _
                                                        Barcode1dStandardFlags.Barcode1dCode11C
        _writeDialogData.Std1DBarData = New BarcodeData()
        _writeDialogData.Std1DBarData.Data = Nothing
        _writeDialogData.Std1DBarData.SearchType = BarcodeSearchTypeFlags.Barcode1dUpcVersionA
        _writeDialogData.nStd1DStringStatus = 1
        _writeDialogData.rcStd1D = New Rectangle(50, 50, 300, 250)
        ' Patch Code
        _writeDialogData.PatchBarData = New BarcodeData()
        _writeDialogData.nPatchCurTypeIndex = 0
        _writeDialogData.PatchBarData.Data = Nothing
        _writeDialogData.rcPatch = New Rectangle(50, 50, 200, 400)
        ' postnet
        _writeDialogData.nPostnetCurTypeIndex = 0
        _writeDialogData.uPostnetCurTypeData = BarcodeSearchTypeFlags.Barcode1dPostnet
        _writeDialogData.PostnetBarData = New BarcodeData()
        _writeDialogData.nPostnetStringStatus = 1
        _writeDialogData.PostnetBarData.Data = Nothing
        _writeDialogData.rcPostnet = New Rectangle(50, 50, 300, 200)
        ' PDF417
        _writeDialogData.nPDFStringStatus = 1
        _writeDialogData.PDFBarData = New BarcodeData()
        _writeDialogData.PDFBarData.Data = Nothing
        _writeDialogData.nPDFEccCurSelection = 0
        _writeDialogData.uFlags_PDFPg = barcodeWriteFlags.None
        _writeDialogData.PDFBarcode = New BarcodeWritePdf()
        _writeDialogData.PDFBarcode.EccPercentage = 0
        _writeDialogData.PDFBarcode.EccLevel = BarcodePdf417EccLevelFlags.Level0
        _writeDialogData.PDFBarcode.AspectHeight = 0
        _writeDialogData.PDFBarcode.AspectWidth = 0
        _writeDialogData.PDFBarcode.ModAspectRatio = 0
        _writeDialogData.PDFBarcode.Columns = 0
        _writeDialogData.PDFBarcode.Rows = 0
        _writeDialogData.PDFBarcode.Module = 0
        _writeDialogData.PDFBarcode.Justify = BarcodeJustifyFlags.None
        _writeDialogData.rcPDF = New Rectangle(50, 50, 0, 0)
        ' Data Matrix
        _writeDialogData.nDMStringStatus = 1
        _writeDialogData.DMBarData = New BarcodeData()
        _writeDialogData.DMBarData.Data = Nothing
        _writeDialogData.nDMTypeCurSelection = 0
        _writeDialogData.uFlags_DMPg = barcodeWriteFlags.None
        _writeDialogData.DMBarcode = New BarcodeWriteDatamatrix()
        _writeDialogData.DMBarcode.Justify = BarcodeJustifyFlags.None
        _writeDialogData.DMBarcode.FileIdLow = 0
        _writeDialogData.DMBarcode.FileIdHigh = 0
        _writeDialogData.DMBarcode.GroupNumber = 0
        _writeDialogData.DMBarcode.GroupTotal = 0
        _writeDialogData.DMBarcode.XModule = 30
        _writeDialogData.rcDM = New Rectangle(50, 50, 0, 0)
        ' QR
        _writeDialogData.nQRStringStatus = 1
        _writeDialogData.QRBarData = New BarcodeData()
        _writeDialogData.QRBarData.Data = Nothing
        _writeDialogData.nQRTypeCurSelection = 0
        _writeDialogData.uFlags_QRPg = barcodeWriteFlags.None
        _writeDialogData.QRBarcode = New BarcodeWriteQr()
        _writeDialogData.QRBarcode.Justify = BarcodeJustifyFlags.None
        _writeDialogData.QRBarcode.GroupNumber = 0
        _writeDialogData.QRBarcode.GroupTotal = 0
        _writeDialogData.QRBarcode.XModule = 30
        _writeDialogData.QRBarcode.EccLevel = BarcodeQrEccLevel.LevelL
        _writeDialogData.nQREccCurSelection = 0
        _writeDialogData.rcQR = New Rectangle(50, 50, 0, 0)
        ' RSS14 Stacked
        _writeDialogData.nStackCurTypeIndex = 0
        _writeDialogData.uStackCurTypeData = BarcodeSearchTypeFlags.Barcode1dRss14Stacked
        _writeDialogData.nStackCurRowsIndex = 1
        _writeDialogData.uStackCurRowsData = Barcode1dAdvancedFlags.Barcode1dRss14ExpandedStackedRows2
        _writeDialogData.StackBarcode1D = New Barcode1d()
        _writeDialogData.StackBarcode1D.AdvancedFlags = Barcode1dAdvancedFlags.Barcode1dRss14ExpandedStackedRows2
        _writeDialogData.nStackStringStatus = 1
        _writeDialogData.StackBarData = New BarcodeData()
        _writeDialogData.StackBarData.Data = Nothing
        _writeDialogData.rcStack = New Rectangle(50, 50, 300, 550)
        ' 4 State
        _writeDialogData.nStateCurTypeIndex = 0
        _writeDialogData.uStateCurTypeData = BarcodeSearchTypeFlags.Barcode1dAustralianPost
        _writeDialogData.nStateCurFCCIndex = 0
        _writeDialogData.StateBarcode1D = New Barcode1d()
        _writeDialogData.StateBarcode1D.AdvancedFlags = Barcode1dAdvancedFlags.Barcode1dAustralianCifC
        _writeDialogData.nStateAustStringStatus = 1
        _writeDialogData.nStateRoyalStringStatus = 1
        _writeDialogData.StateBarData = New BarcodeData()
        _writeDialogData.State_Tmp_Aust_BarData = New BarcodeData()
        _writeDialogData.State_Tmp_Royal_BarData = New BarcodeData()
        _writeDialogData.StateBarData.Data = Nothing
        _writeDialogData.State_Tmp_Aust_BarData.Data = Nothing
        _writeDialogData.State_Tmp_Royal_BarData.Data = Nothing
        _writeDialogData.rcState = New Rectangle(50, 50, 350, 350)
        _barcodeEngine = New BarcodeEngine()

        '  Counter to set a number next to the pasted window text to differ it from others...
        pasteCounter = 0

    End Sub
    Public Function split() As DataTable

        Dim dc1 As DataColumn = New DataColumn("barcodevalue")
        Dim dc2 As DataColumn = New DataColumn("filename")
        dat = New DataTable
        dat.Columns.Add(dc1)
        dat.Columns.Add(dc2)
        Try
            Dim loader As New ImageFileLoader()
            Dim isbarcode As Boolean = False
            RasterCodecs.Startup()
            _codecs = New RasterCodecs()
            'Dim info1 As CodecsImageInfo
            'info1 = _codecs.GetInformation(dirinfo1 & "\" & sfilename, True)

            'For i As Integer = 1 To info1.TotalPages
            '    If (loader.Load(own, _codecs, True, 1, info1.TotalPages, dirinfo1 & "\" & sfilename)) Then
            '        child.MdiParent = vi
            '        child.InsertImage(loader.Image, loader.FileName)
            '        child.Show()
            '    Else
            '        child.MdiParent = vi
            '        child.InsertImage(loader.Image, loader.FileName)
            '        child.Show()
            '    End If

            'Next


            Dim icnt As Integer = 0
            Dim sfilecompare As Boolean = False
            Dim LogFileName = Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss.fff ")
            LogFileName = LogFileName.Replace("/", "")
            LogFileName = LogFileName.Replace(":", "")
            LogFileName = LogFileName.Replace(".", "")
            LogFileName = LogFileName.Replace("-", "")
            LogFileName = LogFileName.Replace(" ", "")
            Dim M As Integer = 0
            Dim S As Integer = 0
            For Each obj As Object In iarrlist
                If obj.barcodevalue <> Nothing Then
                    If CType(obj.nextpage, Integer) <> CType(obj.curpage, Integer) Then
                        isbarcode = True

                        Dim newfilename = ""
                        If obj.barcodevalue.ToString.ToLower() = "ezinvita-main" Then
                            M = M + 1
                            S = 0
                            newfilename = dirinfo1 & "\" & Replace(CType(LogFileName + "-M" + M.ToString(), String), "|", "_") & ".tif"
                        ElseIf obj.barcodevalue.ToString.ToLower() = "ezinvita-sub" Then
                            S = S + 1
                            newfilename = dirinfo1 & "\" & Replace(CType(LogFileName + "-M" + M.ToString() + "-S" + S.ToString(), String), "|", "_") & ".tif"
                        Else
                            newfilename = dirinfo1 & "\" & Replace(CType(obj.barcodevalue, String), "|", "_") & ".tif"
                        End If

                        'If File.Exists(newfilename) Then
                        '    newfilename = dirinfo1 & "\" & Replace(CType(obj.barcodevalue + "-" + obj.curpage.ToString() + "-" + LogFileName, String), "|", "_") & ".tif"
                        'End If
                        child = New ChildForm
                        loader = New ImageFileLoader()
                        _codecs = New RasterCodecs()
                        If (loader.Load(own, _codecs, True, CType(obj.curpage, Integer), CType(obj.nextpage, Integer) - 1, dirinfo1 & "\" & sfilename)) Then
                            child.MdiParent = vi
                            child.InsertImage(loader.Image, loader.FileName)
                            '  child.Show()
                            Try
                                Dim endpage = CType(obj.nextpage, Integer) - CType(obj.curpage, Integer)
                                _codecs.Save(child.Viewer.Image, newfilename, _fileFormat, _bitsPerPixel, 1, endpage, 1, CodecsSavePageMode.Overwrite)
                            Catch ex As Exception
                                MessageBox.Show(ex.Message)
                            End Try

                            child.Dispose()
                            child.Close()

                            _codecs.Dispose()

                        End If

                        Dim dr As DataRow
                        dr = dat.NewRow
                        dr("barcodevalue") = Replace(CType(obj.barcodevalue, String), "|", "_").ToString
                        dr("filename") = Replace(CType(obj.barcodevalue, String), "|", "_") & ".tif"
                        dat.Rows.Add(dr)
                    End If
                    icnt = icnt + 1
                End If
                If LCase(Replace(obj.barcodevalue, "|", "_") & ".tif").ToString = LCase(sfilename) Then sfilecompare = True
            Next
            If sfilecompare = False Then
                If isbarcode = True Then Kill(dirinfo1 & "\" & sfilename)
            End If
            isbarcode = False
            Dim barcount As Integer = 0
            barcount = dat.Rows.Count - 1
            Return dat
        Catch ex1 As InvalidOperationException
            If ex1.Message = "Thread was being aborted." Then
            Else
                MsgBox(ex1.Message.ToString)
            End If
        Finally
            iarrlist = Nothing
        End Try
    End Function
    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub
    Public Function SetDataTable() As DataTable
        Dim SetDataTble As New DataTable
        SetDataTble = dat
        Return SetDataTble
    End Function
End Class