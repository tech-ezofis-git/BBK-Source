Imports System.Threading
Imports System.Data
Imports ezofis.UserControl.CAC
Public Module Pvariable
    Public errstr As String = ""
    Public startcount As Integer
    Public ecmlogin As eZECMLogin
    '**************** Configuration variables declarations******************
    Public dbpath As String
    Public Volume As String
    Public XMLPath As String
    Public Imgpath As String
    Public sPdfSignature As String
    Public dirinfo As String
    Public outfile As String
    Public pdfilename As String
    Public msgboxflag As String
    Public tagAuthor As String
    Public tagTitle As String
    Public tagSubject As String
    Public tagRemarks As String
    Public nopages As Integer
    Public Conn As String
    Public imaging As String
    Public locIP As String
    Public barcodecount As Integer = 0
    Public tablename1 As String
    Public dstemplatehier As DataSet
    Public BarcodeEndsWith As String
    Public BarcodeStartsWith As String
    Public BarcodeType As String
    Public BarcodeLength As Integer
    Public docsize As Integer
    Public ConnectString As String
    Public Login As Boolean = False
    Public loggedfromname As String
    '********** Thread********
    Public thread1 As Thread '(AddressOf folderwatch)
    Public thrdcnt As Integer
    Public Fileformet As String
    Public Fld_dataset As New DataSet
    Public cabinet_path As String
    Public form_status As String
    Public cabinet As String
    Public g_cabinet As String
    Public template As String
    Public g_template As String
    Public cabinetid As Integer
    Public templateid As Integer
    Public username As String
    Public usrgroup As String
    Public usrdataset As New DataSet
    Public uusername As String
    Public ucabinet As String
    Public userid As String
    Public ocrvalue As String
    Public pcname As String
    Public _bitsPerPixel As Integer = 1
    Public MergeImage As String = String.Empty
    Public MergeClicked As Boolean = False



End Module
