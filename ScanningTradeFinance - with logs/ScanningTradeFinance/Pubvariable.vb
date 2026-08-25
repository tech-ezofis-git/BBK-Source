
Imports System.Globalization
Imports System.Resources
Imports System.Data
Imports Leadtools.Codecs
Imports Leadtools

Public Module Pubvariable
    Public ConnectString As String = ""
    Public SBusername As String = ""
    Public SBuserRole As String = ""
    Public sociallogin As Boolean = False
    Public SBuseremail As String = ""
    Public SBTenantid As String = ""
    Public TenantId As Integer = 0
    Public SBuserlanguage As String = ""
    Public SBuserlanguageId As Integer
    Public imaging As String = ""
    Public imagingAutoProcessing As String = ""
    Public imagingIndexed As String = ""

    Public PatternSplitkeyvalue As String = ""
    Public PatternBoundName As String = ""
    Public PatternBounds As LeadRect
    'Public rm As New ResourceManager("ezofis.UserControl.Main", GetType(ECMRightPane).Assembly)
    Public Fld_dataset As New DataSet

    Public Sess As String = ""
    Public _recognitionResults As String
    Public BarCodeTypeFromCmb As String
    Public StrZonalFileName As String
    Public vers As String
    Public Oldfilenames As String
    Public itemid As String
    Public AdvanceIndexing As String
    Public setanno As Integer = 0
    Public enablefreehand As Boolean = False
    Public outfile As String
    Public pdfilename As String
    Public g_cabinet As String
    Public sPdfSignature As String
    Public Volume As String
    Public errstr As String
    Public BarcodeStartsWith As String
    Public BarcodeEndsWith As String
    Public BarcodeType As String
    Public barcodecount As Integer
    Public cabinet As String
    Public usrdataset As DataSet
    Public pageCount As Integer = 0
    Public HadImages As Boolean
    Public info1 As CodecsImageInfo
    Public _codecs As RasterCodecs



End Module
