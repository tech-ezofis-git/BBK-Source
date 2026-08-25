Option Strict On
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.ComponentModel
Imports System.Threading
Imports System.Web.Script.Serialization
Imports System.Collections.Specialized
Imports Newtonsoft.Json
Imports System.IO
Imports System.Windows
Imports System.Net
Imports Repository_File.Pubvar
Public Class Form2
    Public Enum HRESULT As Integer
        S_OK = 0
        S_FALSE = 1
        E_NOINTERFACE = &H80004002
        E_NOTIMPL = &H80004001
        E_FAIL = &H80004005
        E_UNEXPECTED = &H8000FFFF
    End Enum

    Public repopath As String
    Public key As String
    Dim allfiles As Integer
    Dim filecount As Integer
    Dim folderfilecount As Integer
    Dim folderDfilecount As Integer
    Dim movefilecount As String
    Dim mcount As String
    Dim strFileToDecrypt As String
    Dim strOutputEncrypt As String
    Dim strOutputDecrypt As String
    Dim fsInput As System.IO.FileStream
    Dim fsOutput As System.IO.FileStream
    Dim mousepath As String = ""
    Dim filedestination As String = "C:\Archive"
    Dim worker As New System.ComponentModel.BackgroundWorker()
    Dim fcount As Integer = 0
    Public notifiy As New Notification
    Dim file1 As IO.StreamWriter

    Dim notifications As Notification = New Notification()
    Dim ser As JavaScriptSerializer = New JavaScriptSerializer()
    Dim Appcon As NameValueCollection = DirectCast(System.Configuration.ConfigurationManager.GetSection("Database"), NameValueCollection)
    Dim jspath As String = Appcon("Jsonpath")
    Dim dwnpath As String = Appcon("Downloadjpath")
    Dim POLLTIMEOUTMINUTES As String = Appcon("POLLTIMEOUTMINUTES")
    Dim filesearch As Boolean = False

    Dim custommsgbox As New CustomMessageBoxControl

    <ComImport>
    <Guid("dfd3b6b5-c10c-4be9-85f6-a66969f402f6")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IExplorerBrowser
        Function Initialize(hwndParent As IntPtr, ByRef prc As RECT, ByRef pfs As FOLDERSETTINGS) As HRESULT
        Function Destroy() As HRESULT
        Function SetRect(phdwp As IntPtr, ByRef rcBrowser As RECT) As HRESULT
        Function SetPropertyBag(pszPropertyBag As String) As HRESULT
        Function SetEmptyText(pszEmptyText As String) As HRESULT
        Function SetFolderSettings(ByRef pfs As FOLDERSETTINGS) As HRESULT

        'IExplorerBrowserEvents *psbe
        Function Advise(psbe As IntPtr, ByRef pdwCookie As Integer) As HRESULT
        Function Unadvise(dwCookie As Integer) As HRESULT
        Function SetOptions(dwFlag As EXPLORER_BROWSER_OPTIONS) As HRESULT
        Function GetOptions(ByRef pdwFlag As EXPLORER_BROWSER_OPTIONS) As HRESULT
        Function BrowseToIDList(pidl As IntPtr, uFlags As UInteger) As HRESULT

        'IUnknown *punk,
        Function BrowseToObject(punk As IntPtr, uFlags As UInteger) As HRESULT
        Function FillFromObject(punk As IntPtr, dwFlags As EXPLORER_BROWSER_FILL_FLAGS) As HRESULT
        Function RemoveAll() As HRESULT
        Function GetCurrentView(ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
    End Interface

    Public Const SBSP_ABSOLUTE = &H0

    Public Enum EXPLORER_BROWSER_OPTIONS As Integer
        EBO_NONE = 0
        EBO_NAVIGATEONCE = &H1
        EBO_SHOWFRAMES = &H2
        EBO_ALWAYSNAVIGATE = &H4
        EBO_NOTRAVELLOG = &H8
        EBO_NOWRAPPERWINDOW = &H10
        EBO_HTMLSHAREPOINTVIEW = &H20
        EBO_NOBORDER = &H40
        EBO_NOPERSISTVIEWSTATE = &H80
    End Enum

    Public Enum EXPLORER_BROWSER_FILL_FLAGS As Integer
        EBF_NONE = 0
        EBF_SELECTFROMDATAOBJECT = &H100
        EBF_NODROPTARGET = &H200
    End Enum

    Public Enum FOLDERVIEWMODE As Integer
        FVM_AUTO = -1
        FVM_FIRST = 1
        FVM_ICON = 1
        FVM_SMALLICON = 2
        FVM_LIST = 3
        FVM_DETAILS = 4
        FVM_THUMBNAIL = 5
        FVM_TILE = 6
        FVM_THUMBSTRIP = 7
        FVM_CONTENT = 8
        FVM_LAST = 8
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure FOLDERSETTINGS
        Public ViewMode As Integer
        Public fFlags As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
        Public Sub New(left As Integer, top As Integer, right As Integer, bottom As Integer)
            Me.left = left
            Me.top = top
            Me.right = right
            Me.bottom = bottom
        End Sub
    End Structure

    <DllImport("User32.dll", SetLastError:=True)>
    Public Shared Function GetClientRect(hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    End Function

    <DllImport("User32.dll", SetLastError:=True)>
    Public Shared Function OffsetRect(ByRef lprc As RECT, dx As Integer, dy As Integer) As Boolean
    End Function

    <DllImport("User32.dll", SetLastError:=True)>
    Public Shared Function InflateRect(ByRef lprc As RECT, dx As Integer, dy As Integer) As Boolean
    End Function

    <DllImport("User32.dll", SetLastError:=True)>
    Private Shared Function MoveWindow(hWnd As IntPtr, X As Integer, Y As Integer, nWidth As Integer, nHeight As Integer, bRepaint As Boolean) As Boolean
    End Function

    <DllImport("User32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Shared Function FindWindowEx(ByVal hWndParent As IntPtr, ByVal hWndChildAfter As IntPtr, ByVal lpszClass As String, ByVal lpszWindow As String) As IntPtr
    End Function

    <DllImport("Shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Shared Function SHILCreateFromPath(<MarshalAs(UnmanagedType.LPWStr)> pszPath As String, ByRef ppIdl As IntPtr, ByRef rgflnOut As UInteger) As HRESULT
    End Function


    <ComImport>
    <Guid("a0ffbc28-5482-4366-be27-3e81e78e06c2")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface ISearchFolderItemFactory
        Function SetDisplayName(pszDisplayName As String) As HRESULT
        Function SetFolderTypeID(ByRef ftid As Guid) As HRESULT
        Function SetFolderLogicalViewMode(flvm As FOLDERLOGICALVIEWMODE) As HRESULT
        Function SetIconSize(iIconSize As Integer) As HRESULT
        Function SetVisibleColumns(cVisibleColumns As UInteger, rgKey As PROPERTYKEY) As HRESULT
        Function SetSortColumns(cSortColumns As UInteger, rgSortColumns As SORTCOLUMN) As HRESULT
        Function SetGroupColumn(keyGroup As PROPERTYKEY) As HRESULT
        Function SetStacks(cStackKeys As UInteger, rgStackKeys As PROPERTYKEY) As HRESULT
        Function SetScope(psiaScope As IShellItemArray) As HRESULT
        Function SetCondition(pCondition As ICondition) As HRESULT
        Function GetShellItem(ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Function GetIDList(ByRef ppidl As IntPtr) As HRESULT
    End Interface

    <StructLayout(LayoutKind.Sequential, Pack:=4)>
    Public Structure PROPERTYKEY
        Private fmtid As Guid
        Private pid As Integer
        Public ReadOnly Property FormatId() As Guid
            Get
                Return Me.fmtid
            End Get
        End Property
        Public ReadOnly Property PropertyId() As Integer
            Get
                Return Me.pid
            End Get
        End Property
        Public Sub New(ByVal formatId As Guid, ByVal propertyId As Integer)
            Me.fmtid = formatId
            Me.pid = propertyId
        End Sub
        Public Shared ReadOnly PKEY_DateCreated As PROPERTYKEY = New PROPERTYKEY(New Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC"), 15)
    End Structure

    Public Enum FOLDERLOGICALVIEWMODE
        FLVM_UNSPECIFIED = -1
        FLVM_FIRST = 1
        FLVM_DETAILS = 1
        FLVM_TILES = 2
        FLVM_ICONS = 3
        FLVM_LIST = 4
        FLVM_CONTENT = 5
        FLVM_LAST = 5
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure SORTCOLUMN
        Public propkey As PROPERTYKEY
        Public direction As SORTDIRECTION
    End Structure

    Public Enum SORTDIRECTION
        SORT_DESCENDING = -1
        SORT_ASCENDING = 1
    End Enum

    <ComImport()>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("b63ea76d-1f85-456f-a19c-48159efa858b")>
    Public Interface IShellItemArray
        'Function BindToHandler(pbc As IBindCtx, ByRef bhid As Guid, ByRef riid As Guid, ByRef ppvOut As IntPtr) As HRESULT
        Function BindToHandler(pbc As IntPtr, ByRef bhid As Guid, ByRef riid As Guid, ByRef ppvOut As IntPtr) As HRESULT
        Function GetPropertyStore(flags As GETPROPERTYSTOREFLAGS, ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Function GetPropertyDescriptionList(keyType As PROPERTYKEY, ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        'Function GetAttributes(AttribFlags As SIATTRIBFLAGS, sfgaoMask As SFGAOF, ByRef psfgaoAttribs As SFGAOF) As HRESULT
        Function GetAttributes(AttribFlags As SIATTRIBFLAGS, sfgaoMask As Integer, ByRef psfgaoAttribs As Integer) As HRESULT
        Function GetCount(ByRef pdwNumItems As Integer) As HRESULT
        Function GetItemAt(dwIndex As Integer, ByRef ppsi As IShellItem) As HRESULT
        'Function EnumItems(ByRef ppenumShellItems As IEnumShellItems) As HRESULT
        Function EnumItems(ByRef ppenumShellItems As IntPtr) As HRESULT
    End Interface

    <ComImport()>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")>
    Public Interface IShellItem
        <PreserveSig()>
        Function BindToHandler(ByVal pbc As IntPtr, ByRef bhid As Guid, ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Function GetParent(ByRef ppsi As IShellItem) As HRESULT
        Function GetDisplayName(ByVal sigdnName As SIGDN, ByRef ppszName As System.Text.StringBuilder) As HRESULT
        Function GetAttributes(ByVal sfgaoMask As UInteger, ByRef psfgaoAttribs As UInteger) As HRESULT
        Function Compare(ByVal psi As IShellItem, ByVal hint As UInteger, ByRef piOrder As Integer) As HRESULT
    End Interface

    Public Enum SIGDN As Integer
        SIGDN_NORMALDISPLAY = &H0
        SIGDN_PARENTRELATIVEPARSING = &H80018001
        SIGDN_DESKTOPABSOLUTEPARSING = &H80028000
        SIGDN_PARENTRELATIVEEDITING = &H80031001
        SIGDN_DESKTOPABSOLUTEEDITING = &H8004C000
        SIGDN_FILESYSPATH = &H80058000
        SIGDN_URL = &H80068000
        SIGDN_PARENTRELATIVEFORADDRESSBAR = &H8007C001
        SIGDN_PARENTRELATIVE = &H80080001
    End Enum

    Public Enum GETPROPERTYSTOREFLAGS
        GPS_DEFAULT = 0
        GPS_HANDLERPROPERTIESONLY = &H1
        GPS_READWRITE = &H2
        GPS_TEMPORARY = &H4
        GPS_FASTPROPERTIESONLY = &H8
        GPS_OPENSLOWITEM = &H10
        GPS_DELAYCREATION = &H20
        GPS_BESTEFFORT = &H40
        GPS_NO_OPLOCK = &H80
        GPS_PREFERQUERYPROPERTIES = &H100
        GPS_EXTRINSICPROPERTIES = &H200
        GPS_EXTRINSICPROPERTIESONLY = &H400
        GPS_MASK_VALID = &H7FF
    End Enum

    Public Enum SIATTRIBFLAGS
        SIATTRIBFLAGS_AND = &H1
        SIATTRIBFLAGS_OR = &H2
        SIATTRIBFLAGS_APPCOMPAT = &H3
        SIATTRIBFLAGS_MASK = &H3
        SIATTRIBFLAGS_ALLITEMS = &H4000
    End Enum

    <ComImport>
    <Guid("A5EFE073-B16F-474f-9F3E-9F8B497A3E08")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IConditionFactory
        Function MakeNot(pcSub As ICondition, fSimplify As Boolean, ByRef ppcResult As ICondition) As HRESULT
        'Function MakeAndOr(ct As CONDITION_TYPE, peuSubs As IEnumUnknown, fSimplify As Boolean, ByRef ppcResult As ICondition) As HRESULT
        Function MakeAndOr(ct As CONDITION_TYPE, peuSubs As IntPtr, fSimplify As Boolean, ByRef ppcResult As ICondition) As HRESULT
        'Function MakeLeaf(pszPropertyName As String, cop As CONDITION_OPERATION, pszValueType As String,
        '        ppropvar As PROPVARIANT,
        '       pPropertyNameTerm As IRichChunk,
        '       pOperationTerm As IRichChunk,
        '       pValueTerm As IRichChunk,
        '      fExpand As Boolean, ByRef pcResult As ICondition) As HRESULT
        Function MakeLeaf(pszPropertyName As String, cop As CONDITION_OPERATION, pszValueType As String,
           ByRef ppropvar As PROPVARIANT,
           pPropertyNameTerm As IntPtr,
           pOperationTerm As IntPtr,
           pValueTerm As IntPtr,
          fExpand As Boolean, ByRef pcResult As ICondition) As HRESULT
        Function Resolve(pc As ICondition, sqro As STRUCTURED_QUERY_RESOLVE_OPTION, pstReferenceTime As SYSTEMTIME, ByRef ppcResolved As ICondition) As HRESULT
    End Interface

    Public Enum STRUCTURED_QUERY_RESOLVE_OPTION
        SQRO_DEFAULT = 0
        SQRO_DONT_RESOLVE_DATETIME = &H1
        SQRO_ALWAYS_ONE_INTERVAL = &H2
        SQRO_DONT_SIMPLIFY_CONDITION_TREES = &H4
        SQRO_DONT_MAP_RELATIONS = &H8
        SQRO_DONT_RESOLVE_RANGES = &H10
        SQRO_DONT_REMOVE_UNRESTRICTED_KEYWORDS = &H20
        SQRO_DONT_SPLIT_WORDS = &H40
        SQRO_IGNORE_PHRASE_ORDER = &H80
        SQRO_ADD_VALUE_TYPE_FOR_PLAIN_VALUES = &H100
        SQRO_ADD_ROBUST_ITEM_NAME = &H200
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure SYSTEMTIME
        Public wYear As Short
        Public wMonth As Short
        Public wDayOfWeek As Short
        Public wDay As Short
        Public wHour As Short
        Public wMinute As Short
        Public wSecond As Short
        Public wMilliseconds As Short
    End Structure

    <ComImport>
    <Guid("0FC988D4-C935-4b97-A973-46282EA175C8")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface ICondition
        Inherits IPersistStream
#Region "IPersistStream"
        Overloads Function GetClassID(ByRef pClassID As Guid) As HRESULT
        Overloads Function IsDirty() As HRESULT
        Overloads Function Load(ByVal pstm As System.Runtime.InteropServices.ComTypes.IStream) As HRESULT
        Overloads Function Save(ByVal pstm As System.Runtime.InteropServices.ComTypes.IStream, ByVal fClearDirty As Boolean) As HRESULT
        Overloads Function GetSizeMax() As HRESULT
#End Region
        Function GetConditionType(ByRef pNodeType As CONDITION_TYPE) As HRESULT
        Function GetSubConditions(ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Function GetComparisonInfo(ByRef ppszPropertyName As String, ByRef pcop As CONDITION_OPERATION, ByRef ppropvar As PROPVARIANT) As HRESULT
        Function GetValueType(ByRef ppszValueTypeName As String) As HRESULT
        Function GetValueNormalization(ByRef ppszNormalization As String) As HRESULT
        'Function GetInputTerms(ByRef ppPropertyTerm As IRichChunk,
        '     ByRef pOperationTerm As IRichChunk,
        '   ByRef ppValueTerm As IRichChunk) As HRESULT
        Function GetInputTerms(ByRef ppPropertyTerm As IntPtr,
             ByRef pOperationTerm As IntPtr,
           ByRef ppValueTerm As IntPtr) As HRESULT
        Function Clone(ByRef ppc As ICondition) As HRESULT
    End Interface

    <StructLayout(LayoutKind.Sequential)>
    Public Structure PROPARRAY
        Public cElems As UInt32
        Public pElems As IntPtr
    End Structure

    <StructLayout(LayoutKind.Explicit, Pack:=1)>
    Public Structure PROPVARIANT
        <FieldOffset(0)>
        Public varType As UShort
        <FieldOffset(2)>
        Public wReserved1 As UShort
        <FieldOffset(4)>
        Public wReserved2 As UShort
        <FieldOffset(6)>
        Public wReserved3 As UShort
        <FieldOffset(8)>
        Public bVal As Byte
        <FieldOffset(8)>
        Public cVal As SByte
        <FieldOffset(8)>
        Public uiVal As UShort
        <FieldOffset(8)>
        Public iVal As Short
        <FieldOffset(8)>
        Public uintVal As UInt32
        <FieldOffset(8)>
        Public intVal As Int32
        <FieldOffset(8)>
        Public ulVal As UInt64
        <FieldOffset(8)>
        Public lVal As Int64
        <FieldOffset(8)>
        Public fltVal As Single
        <FieldOffset(8)>
        Public dblVal As Double
        <FieldOffset(8)>
        Public boolVal As Short
        <FieldOffset(8)>
        Public pclsidVal As IntPtr
        <FieldOffset(8)>
        Public pszVal As IntPtr
        <FieldOffset(8)>
        Public pwszVal As IntPtr
        <FieldOffset(8)>
        Public punkVal As IntPtr
        <FieldOffset(8)>
        Public ca As PROPARRAY
        <FieldOffset(8)>
        Public filetime As System.Runtime.InteropServices.ComTypes.FILETIME
    End Structure

    Public Enum VT As Short
        VT_EMPTY = 0
        VT_NULL = 1
        VT_I2 = 2
        VT_I4 = 3
        VT_R4 = 4
        VT_R8 = 5
        VT_CY = 6
        VT_DATE = 7
        VT_BSTR = 8
        VT_DISPATCH = 9
        VT_ERROR = 10
        VT_BOOL = 11
        VT_VARIANT = 12
        VT_UNKNOWN = 13
        VT_DECIMAL = 14
        VT_I1 = 16
        VT_UI1 = 17
        VT_UI2 = 18
        VT_UI4 = 19
        VT_I8 = 20
        VT_UI8 = 21
        VT_INT = 22
        VT_UINT = 23
        VT_VOID = 24
        VT_HRESULT = 25
        VT_PTR = 26
        VT_SAFEARRAY = 27
        VT_CARRAY = 28
        VT_USERDEFINED = 29
        VT_LPSTR = 30
        VT_LPWSTR = 31
        VT_RECORD = 36
        VT_FILETIME = 64
        VT_BLOB = 65
        VT_STREAM = 66
        VT_STORAGE = 67
        VT_STREAMED_OBJECT = 68
        VT_STORED_OBJECT = 69
        VT_BLOB_OBJECT = 70
        VT_CF = 71
        VT_CLSID = 72
        VT_BSTR_BLOB = 4095
        VT_VECTOR = 4096
        VT_ARRAY = 8192
        VT_BYREF = 16384
        VT_RESERVED = &H8000S
        VT_ILLEGAL = &HFFFFS
        VT_ILLEGALMASKED = 4095
        VT_TYPEMASK = 4095
    End Enum

    Public Enum CONDITION_TYPE
        CT_AND_CONDITION = 0
        CT_OR_CONDITION = (CT_AND_CONDITION + 1)
        CT_NOT_CONDITION = (CT_OR_CONDITION + 1)
        CT_LEAF_CONDITION = (CT_NOT_CONDITION + 1)
    End Enum

    Public Enum CONDITION_OPERATION
        COP_IMPLICIT = 0
        COP_EQUAL = (COP_IMPLICIT + 1)
        COP_NOTEQUAL = (COP_EQUAL + 1)
        COP_LESSTHAN = (COP_NOTEQUAL + 1)
        COP_GREATERTHAN = (COP_LESSTHAN + 1)
        COP_LESSTHANOREQUAL = (COP_GREATERTHAN + 1)
        COP_GREATERTHANOREQUAL = (COP_LESSTHANOREQUAL + 1)
        COP_VALUE_STARTSWITH = (COP_GREATERTHANOREQUAL + 1)
        COP_VALUE_ENDSWITH = (COP_VALUE_STARTSWITH + 1)
        COP_VALUE_CONTAINS = (COP_VALUE_ENDSWITH + 1)
        COP_VALUE_NOTCONTAINS = (COP_VALUE_CONTAINS + 1)
        COP_DOSWILDCARDS = (COP_VALUE_NOTCONTAINS + 1)
        COP_WORD_EQUAL = (COP_DOSWILDCARDS + 1)
        COP_WORD_STARTSWITH = (COP_WORD_EQUAL + 1)
        COP_APPLICATION_SPECIFIC = (COP_WORD_STARTSWITH + 1)
    End Enum

    <ComImport(),
    Guid("7FD52380-4E07-101B-AE2D-08002B2EC713"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IPersistStream
        Inherits IPersist
#Region "IPersist"
        Overloads Function GetClassID(ByRef pClassID As Guid) As HRESULT
#End Region
        Function IsDirty() As HRESULT
        Function Load(ByVal pstm As System.Runtime.InteropServices.ComTypes.IStream) As HRESULT
        Function Save(ByVal pstm As System.Runtime.InteropServices.ComTypes.IStream, ByVal fClearDirty As Boolean) As HRESULT
        Function GetSizeMax() As HRESULT
    End Interface

    <ComImport(),
    Guid("0000010c-0000-0000-C000-000000000046"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IPersist
        Function GetClassID(ByRef pClassID As Guid) As HRESULT
    End Interface

    <ComImport>
    <Guid("cde725b0-ccc9-4519-917e-325d72fab4ce")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IFolderView
        Function GetCurrentViewMode(ByRef pViewMode As UInteger) As HRESULT
        Function SetCurrentViewMode(ViewMode As UInteger) As HRESULT
        Function GetFolder(ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Function Item(iItemIndex As Integer, ByRef ppidl As IntPtr) As HRESULT
        Function ItemCount(uFlags As UInteger, ByRef pcItems As Integer) As HRESULT
        Function Items(uFlags As UInteger, ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Function GetSelectionMarkedItem(ByRef piItem As Integer) As HRESULT
        Function GetFocusedItem(ByRef piItem As Integer) As HRESULT
        Function GetItemPosition(pidl As IntPtr, ByRef ppt As POINT) As HRESULT
        Function GetSpacing(ByRef ppt As POINT) As HRESULT
        Function GetDefaultSpacing(ByRef ppt As POINT) As HRESULT
        Function GetAutoArrange() As HRESULT
        Function SelectItem(iItem As Integer, dwFlags As Integer) As HRESULT
        Function SelectAndPositionItems(cidl As UInteger, apidl As IntPtr, apt As POINT, dwFlags As Integer) As HRESULT
    End Interface

    <StructLayout(LayoutKind.Sequential)>
    Public Structure POINT
        Public x As Integer
        Public y As Integer
    End Structure

    <ComImport>
    <Guid("2EBDEE67-3505-43f8-9946-EA44ABC8E5B0")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IQueryParser
        'Function Parse(pszInputString As String, pCustomProperties As IEnumUnknown, ByRef ppSolution As IQuerySolution) As HRESULT
        Function Parse(pszInputString As String, pCustomProperties As IntPtr, ByRef ppSolution As IQuerySolution) As HRESULT
        Function SetOption([option] As STRUCTURED_QUERY_SINGLE_OPTION, pOptionValue As PROPVARIANT) As HRESULT
        Function GetOption([option] As STRUCTURED_QUERY_SINGLE_OPTION, ByRef pOptionValue As PROPVARIANT) As HRESULT
        Function SetMultiOption([option] As STRUCTURED_QUERY_MULTIOPTION, pszOptionKey As String, ByRef pOptionValue As PROPVARIANT) As HRESULT
        'Function GetSchemaProvider(ByRef ppSchemaProvider As ISchemaProvider) As HRESULT
        Function GetSchemaProvider(ByRef ppSchemaProvider As IntPtr) As HRESULT
        Function RestateToString(pCondition As ICondition, fUseEnglish As Boolean, ByRef ppszQueryString As String) As HRESULT
        Function ParsePropertyValue(pszPropertyName As String, pszInputString As String, ByRef ppSolution As IQuerySolution) As HRESULT
        Function RestatePropertyValueToString(pCondition As ICondition, fUseEnglish As Boolean, ByRef ppszPropertyName As String, ByRef ppszQueryString As String) As HRESULT
    End Interface

    Public Enum STRUCTURED_QUERY_SINGLE_OPTION
        SQSO_SCHEMA = 0
        SQSO_LOCALE_WORD_BREAKING = (SQSO_SCHEMA + 1)
        SQSO_WORD_BREAKER = (SQSO_LOCALE_WORD_BREAKING + 1)
        SQSO_NATURAL_SYNTAX = (SQSO_WORD_BREAKER + 1)
        SQSO_AUTOMATIC_WILDCARD = (SQSO_NATURAL_SYNTAX + 1)
        SQSO_TRACE_LEVEL = (SQSO_AUTOMATIC_WILDCARD + 1)
        SQSO_LANGUAGE_KEYWORDS = (SQSO_TRACE_LEVEL + 1)
        SQSO_SYNTAX = (SQSO_LANGUAGE_KEYWORDS + 1)
        SQSO_TIME_ZONE = (SQSO_SYNTAX + 1)
        SQSO_IMPLICIT_CONNECTOR = (SQSO_TIME_ZONE + 1)
        SQSO_CONNECTOR_CASE = (SQSO_IMPLICIT_CONNECTOR + 1)
    End Enum

    Public Enum STRUCTURED_QUERY_MULTIOPTION
        SQMO_VIRTUAL_PROPERTY = 0
        SQMO_DEFAULT_PROPERTY = (SQMO_VIRTUAL_PROPERTY + 1)
        SQMO_GENERATOR_FOR_TYPE = (SQMO_DEFAULT_PROPERTY + 1)
        SQMO_MAP_PROPERTY = (SQMO_GENERATOR_FOR_TYPE + 1)
    End Enum

    <ComImport>
    <Guid("D6EBC66B-8921-4193-AFDD-A1789FB7FF57")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IQuerySolution
        Inherits IConditionFactory
#Region "IConditionFactory"
        Overloads Function MakeNot(pcSub As ICondition, fSimplify As Boolean, ByRef ppcResult As ICondition) As HRESULT
        Overloads Function MakeAndOr(ct As CONDITION_TYPE, peuSubs As IntPtr, fSimplify As Boolean, ByRef ppcResult As ICondition) As HRESULT
        Overloads Function MakeLeaf(pszPropertyName As String, cop As CONDITION_OPERATION, pszValueType As String,
                                    ByRef ppropvar As PROPVARIANT, pPropertyNameTerm As IntPtr,
                                    pOperationTerm As IntPtr, pValueTerm As IntPtr,
                                    fExpand As Boolean, ByRef pcResult As ICondition) As HRESULT
        Overloads Function Resolve(pc As ICondition, sqro As STRUCTURED_QUERY_RESOLVE_OPTION, ByRef pstReferenceTime As SYSTEMTIME, ByRef ppcResolved As ICondition) As HRESULT
#End Region
        'Function GetQuery(ByRef ppQueryNode As ICondition, ByRef ppMainType As IEntity) As HRESULT
        Function GetQuery(ByRef ppQueryNode As ICondition, ByRef ppMainType As IntPtr) As HRESULT
        Function GetErrors(ByRef riid As Guid, ByRef ppParseErrors As IntPtr) As HRESULT
        'Function GetLexicalData(ByRef ppszInputString As String, ByRef ppTokens As ITokenCollection, ByRef plcid As Integer, ByRef ppWordBreaker As IUnknown) As HRESULT
        Function GetLexicalData(ByRef ppszInputString As String, ByRef ppTokens As IntPtr, ByRef plcid As Integer, ByRef ppWordBreaker As IntPtr) As HRESULT
    End Interface

    <ComImport>
    <Guid("A879E3C4-AF77-44fb-8F37-EBD1487CF920")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IQueryParserManager
        Function CreateLoadedParser(pszCatalog As String, langidForKeywords As Short, ByRef riid As Guid, ByRef ppQueryParser As IntPtr) As HRESULT
        Function InitializeOptions(fUnderstandNQS As Boolean, fAutoWildCard As Boolean, pQueryParser As IQueryParser) As HRESULT
        Function SetOption([option] As QUERY_PARSER_MANAGER_OPTION, ByRef pOptionValue As PROPVARIANT) As HRESULT
    End Interface

    Public Enum QUERY_PARSER_MANAGER_OPTION
        QPMO_SCHEMA_BINARY_NAME = 0
        QPMO_PRELOCALIZED_SCHEMA_BINARY_PATH = (QPMO_SCHEMA_BINARY_NAME + 1)
        QPMO_UNLOCALIZED_SCHEMA_BINARY_PATH = (QPMO_PRELOCALIZED_SCHEMA_BINARY_PATH + 1)
        QPMO_LOCALIZED_SCHEMA_BINARY_PATH = (QPMO_UNLOCALIZED_SCHEMA_BINARY_PATH + 1)
        QPMO_APPEND_LCID_TO_LOCALIZED_PATH = (QPMO_LOCALIZED_SCHEMA_BINARY_PATH + 1)
        QPMO_LOCALIZER_SUPPORT = (QPMO_APPEND_LCID_TO_LOCALIZED_PATH + 1)
    End Enum

    Public Const LOCALE_USER_DEFAULT = &H400


    <ComImport>
    <Guid("1af3a467-214f-4298-908e-06b03e0b39f9")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IFolderView2
        Inherits IFolderView
#Region "IFolderView"
        Overloads Function GetCurrentViewMode(ByRef pViewMode As UInteger) As HRESULT
        Overloads Function SetCurrentViewMode(ViewMode As UInteger) As HRESULT
        Overloads Function GetFolder(ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Overloads Function Item(iItemIndex As Integer, ByRef ppidl As IntPtr) As HRESULT
        Overloads Function ItemCount(uFlags As UInteger, ByRef pcItems As Integer) As HRESULT
        Overloads Function Items(uFlags As UInteger, ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Overloads Function GetSelectionMarkedItem(ByRef piItem As Integer) As HRESULT
        Overloads Function GetFocusedItem(ByRef piItem As Integer) As HRESULT
        Overloads Function GetItemPosition(pidl As IntPtr, ByRef ppt As POINT) As HRESULT
        Overloads Function GetSpacing(ByRef ppt As POINT) As HRESULT
        Overloads Function GetDefaultSpacing(ByRef ppt As POINT) As HRESULT
        Overloads Function GetAutoArrange() As HRESULT
        Overloads Function SelectItem(iItem As Integer, dwFlags As Integer) As HRESULT
        Overloads Function SelectAndPositionItems(cidl As UInteger, apidl As IntPtr, apt As POINT, dwFlags As Integer) As HRESULT
#End Region
        Function SetGroupBy(key As PROPERTYKEY, fAscending As Boolean) As HRESULT

        Function GetGroupBy(ByRef pkey As PROPERTYKEY, ByRef pfAscending As Boolean) As HRESULT

        'DEPRECATED
        Function SetViewProperty(pidl As IntPtr, propkey As PROPERTYKEY, propvar As PROPVARIANT) As HRESULT
        'DEPRECATED
        Function GetViewProperty(pidl As IntPtr, propkey As PROPERTYKEY, ByRef ppropvar As PROPVARIANT) As HRESULT
        'DEPRECATED
        Function SetTileViewProperties(pidl As IntPtr, pszPropList As String) As HRESULT
        'DEPRECATED
        Function SetExtendedTileViewProperties(pidl As IntPtr, pszPropList As String) As HRESULT

        Function SetText(iType As FVTEXTTYPE, pwszText As String) As HRESULT
        Function SetCurrentFolderFlags(dwMask As Integer, dwFlags As Integer) As HRESULT
        Function GetCurrentFolderFlags(ByRef pdwFlags As Integer) As HRESULT
        Function GetSortColumnCount(ByRef pcColumns As Integer) As HRESULT
        Function SetSortColumns(rgSortColumns As SORTCOLUMN, cColumns As Integer) As HRESULT
        Function GetSortColumns(ByRef rgSortColumns As SORTCOLUMN, cColumns As Integer) As HRESULT
        Function GetItem(iItem As Integer, ByRef riid As Guid, ByRef ppv As IntPtr) As HRESULT
        Function GetVisibleItem(iStart As Integer, fPrevious As Boolean, ByRef piItem As Integer) As HRESULT
        Function GetSelectedItem(iStart As Integer, ByRef piItem As Integer) As HRESULT
        Function GetSelection(fNoneImpliesFolder As Boolean, ByRef ppsia As IShellItemArray) As HRESULT
        Function GetSelectionState(pidl As IntPtr, ByRef pdwFlags As Integer) As HRESULT
        Function InvokeVerbOnSelection(pszVerb As String) As HRESULT
        Function SetViewModeAndIconSize(uViewMode As FOLDERVIEWMODE, iImageSize As Integer) As HRESULT
        Function GetViewModeAndIconSize(ByRef puViewMode As FOLDERVIEWMODE, ByRef piImageSize As Integer) As HRESULT
        Function SetGroupSubsetCount(cVisibleRows As UInteger) As HRESULT
        Function GetGroupSubsetCount(ByRef pcVisibleRows As UInteger) As HRESULT
        Function SetRedraw(fRedrawOn As Boolean) As HRESULT
        Function IsMoveInSameFolder() As HRESULT
        Function DoRename() As HRESULT
    End Interface

    Public Enum FVTEXTTYPE
        FVST_EMPTYTEXT = 0
    End Enum

    Public Enum FOLDERFLAGS
        FWF_NONE = 0
        FWF_AUTOARRANGE = &H1
        FWF_ABBREVIATEDNAMES = &H2
        FWF_SNAPTOGRID = &H4
        FWF_OWNERDATA = &H8
        FWF_BESTFITWINDOW = &H10
        FWF_DESKTOP = &H20
        FWF_SINGLESEL = &H40
        FWF_NOSUBFOLDERS = &H80
        FWF_TRANSPARENT = &H100
        FWF_NOCLIENTEDGE = &H200
        FWF_NOSCROLL = &H400
        FWF_ALIGNLEFT = &H800
        FWF_NOICONS = &H1000
        FWF_SHOWSELALWAYS = &H2000
        FWF_NOVISIBLE = &H4000
        FWF_SINGLECLICKACTIVATE = &H8000
        FWF_NOWEBVIEW = &H10000
        FWF_HIDEFILENAMES = &H20000
        FWF_CHECKSELECT = &H40000
        FWF_NOENUMREFRESH = &H80000
        FWF_NOGROUPING = &H100000
        FWF_FULLROWSELECT = &H200000
        FWF_NOFILTERS = &H400000
        FWF_NOCOLUMNHEADER = &H800000
        FWF_NOHEADERINALLVIEWS = &H1000000
        FWF_EXTENDEDTILES = &H2000000
        FWF_TRICHECKSELECT = &H4000000
        FWF_AUTOCHECKSELECT = &H8000000
        FWF_NOBROWSERVIEWSTATE = &H10000000
        FWF_SUBSETGROUPS = &H20000000
        FWF_USESEARCHFOLDER = &H40000000
        FWF_ALLOWRTLREADING = &H80000000
    End Enum

    <DllImport("Shell32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Public Shared Function SHCreateShellItemArrayFromShellItem(psi As IShellItem, <[In], MarshalAs(UnmanagedType.LPStruct)> ByVal riid As Guid, <Out> ByRef ppv As IShellItemArray) As HRESULT
    End Function

    <DllImport("Shell32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Public Shared Function SHCreateItemFromParsingName(pszPath As String, pbc As IntPtr, <[In], MarshalAs(UnmanagedType.LPStruct)> ByVal riid As Guid, <Out> ByRef ppv As IntPtr) As HRESULT
    End Function

    <DllImport("Kernel32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Public Shared Sub GetLocalTime(ByRef lpSystemTime As SYSTEMTIME)
    End Sub

    Dim selectpath As String
    Friend WithEvents TextBox1, TextBox2, TextBox3 As TextBox
    Friend WithEvents Butdecrypt, Butdownload, Butback As Button
    Friend WithEvents Label1, Label2 As Label
    'added by sara for progress bar
    Friend pnlDownloadProgress As Panel
    Friend pbDownload As ProgressBar
    Friend lblDownloadStatus As Label

    ' added by sara Download progress monitor
    Private downloadPollTimer As Windows.Forms.Timer
    Private pendingBatchIds As New List(Of String)
    Private downloadTotalFileCount As Integer
    Private downloadPollStart As DateTime
    Private Const DOWNLOAD_POLL_INTERVAL_MS As Integer = 2000
    Private DOWNLOAD_POLL_TIMEOUT_MINUTES As Integer = CInt(POLLTIMEOUTMINUTES)


    Dim pExplorerBrowser As IExplorerBrowser = Nothing
    Dim nTopExplorerBrowser As Integer = 40
    Dim pFolderViewPtr As IntPtr = IntPtr.Zero
    Dim pQueryParser As IQueryParser

    Dim sGenericProperties() As String = {"System.Generic.String", "System.Generic.Integer", "System.Generic.DateTime", "System.Generic.Boolean", "System.Generic.FloatingPoint"}
    Dim sGenericSemanticTypes() As String = {"System.StructuredQueryType.String", "System.StructuredQueryType.Integer", "System.StructuredQueryType.DateTime", "System.StructuredQueryType.Boolean", "System.StructuredQueryType.FloatingPoint"}

    Public thisTimer As System.Timers.Timer
    'Dim processthr() As Thread
    'Dim index As Integer = 0
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'SuspendLayout()
        Try

            ClientSize = New System.Drawing.Size(1080, 680)
            Name = "Form2"
            MaximizeBox = False
            MinimizeBox = True

            Dim CLSID_ExplorerBrowser As New Guid("71f96385-ddd6-48d3-a0c1-ae06e8b055fb")
            Dim ExplorerBrowserType As Type = Type.GetTypeFromCLSID(CLSID_ExplorerBrowser, True)
            Dim ExplorerBrowser As Object = Activator.CreateInstance(ExplorerBrowserType)
            pExplorerBrowser = DirectCast(ExplorerBrowser, IExplorerBrowser)

            Dim rc As RECT
            GetClientRect(Me.Handle, rc)
            InflateRect(rc, -10, -nTopExplorerBrowser)
            rc.bottom += (nTopExplorerBrowser - 70)
            Dim hr As HRESULT = pExplorerBrowser.Initialize(Me.Handle, rc, Nothing)

            If (hr = HRESULT.S_OK) Then

                pExplorerBrowser.SetOptions(
                    EXPLORER_BROWSER_OPTIONS.EBO_ALWAYSNAVIGATE Or
                    EXPLORER_BROWSER_OPTIONS.EBO_NOTRAVELLOG Or
                    EXPLORER_BROWSER_OPTIONS.EBO_NOWRAPPERWINDOW Or
                    EXPLORER_BROWSER_OPTIONS.EBO_HTMLSHAREPOINTVIEW Or
                    EXPLORER_BROWSER_OPTIONS.EBO_NOBORDER Or
                    EXPLORER_BROWSER_OPTIONS.EBO_NOPERSISTVIEWSTATE)

                Dim pidlFull As IntPtr = IntPtr.Zero
                hr = SHILCreateFromPath(repopath, pidlFull, Nothing)



                If (hr = HRESULT.S_OK) Then
                    pExplorerBrowser.BrowseToIDList(pidlFull, SBSP_ABSOLUTE)
                    'Dim pfs As FOLDERSETTINGS = New FOLDERSETTINGS()
                    'pfs.ViewMode = FOLDERVIEWMODE.FVM_ICON
                    'hr = pExplorerBrowser.SetFolderSettings(pfs)
                    hr = CreateQueryParser(pQueryParser)
                End If
            End If

            'Search Text box
            TextBox1 = New System.Windows.Forms.TextBox()
            TextBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            TextBox1.Location = New System.Drawing.Point(ClientSize.Width - 300 - 10, 10)
            TextBox1.Name = "TextBox1"
            TextBox1.Size = New System.Drawing.Size(150, 20)
            TextBox1.TabIndex = 0

            'Addressbar Text box
            TextBox2 = New System.Windows.Forms.TextBox()
            TextBox2.Text = repopath
            selectpath = TextBox2.Text
            TextBox2.Location = New System.Drawing.Point(80, 10)
            TextBox2.Name = "TextBox2"
            TextBox2.Size = New System.Drawing.Size(ClientSize.Width - 500 - 10 - 10 - 10, 20)
            TextBox2.TabIndex = 1
            'shiva
            TextBox2.Enabled = False

            Label1 = New System.Windows.Forms.Label()
            Label1.Font = New Font("Roboto", 9)
            Label1.Location = New System.Drawing.Point(10, 10)
            Label1.ForeColor = Color.DarkGray
            Label1.Text = "Repository"

            Label2 = New System.Windows.Forms.Label()
            Label2.Font = New Font("Roboto", 9)
            Label2.Location = New System.Drawing.Point(720, 10)
            Label2.ForeColor = Color.DarkGray
            Label2.Text = "Search"

            Butdecrypt = New System.Windows.Forms.Button()
            Butdecrypt.Location = New System.Drawing.Point(ClientSize.Width - 400, 620)
            Butdecrypt.Height = 35
            Butdecrypt.Width = 190
            Butdecrypt.BackColor = Color.BlueViolet
            Butdecrypt.ForeColor = Color.White
            Butdecrypt.Text = "Decrypt on same Location"
            Butdecrypt.TextAlign = ContentAlignment.MiddleCenter
            Butdecrypt.FlatStyle = FlatStyle.Flat
            Butdecrypt.Font = New Font("Roboto", 10)
            AddHandler Butdecrypt.Click, AddressOf Butdecrypt_Click


            Butdownload = New System.Windows.Forms.Button()
            Butdownload.Location = New System.Drawing.Point(ClientSize.Width - 200, 620)
            Butdownload.Height = 35
            Butdownload.Width = 170
            Butdownload.BackColor = Color.DeepSkyBlue
            Butdownload.ForeColor = Color.White
            Butdownload.FlatStyle = FlatStyle.Flat
            Butdownload.Text = "Download on Local"
            Butdownload.Font = New Font("Roboto", 10)
            Butdownload.TextAlign = ContentAlignment.MiddleCenter
            AddHandler Butdownload.Click, AddressOf Butdownload_Click



            'Butback = New System.Windows.Forms.Button()
            'Butback.Location = New System.Drawing.Point(ClientSize.Width - 400 - 10, 0)
            'Butback.Height = 37
            'Butback.Width = 43
            'Butback.BackColor = Color.White
            'Butback.ForeColor = Color.BlueViolet
            'Butback.Text = "Up"
            ''Butback.BackgroundImage = ""
            'Butback.TextAlign = ContentAlignment.MiddleCenter
            'Butback.FlatStyle = FlatStyle.Flat
            'Butback.Font = New Font("Roboto", 10)
            'AddHandler Butback.Click, AddressOf Butback_Click

            TextBox3 = New System.Windows.Forms.TextBox()
            TextBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            TextBox3.Location = New System.Drawing.Point(ClientSize.Width - 300 - 10, 10)
            TextBox3.Name = "HiddenTextBox3"
            TextBox3.Size = New System.Drawing.Size(150, 20)
            TextBox3.TabIndex = 0
            TextBox3.Text = TextBox2.Text

            Controls.Add(Me.Butdecrypt)
            Controls.Add(Me.Butdownload)
            ' added by sara for progress 
            pnlDownloadProgress = New Panel()
            pnlDownloadProgress.Name = "pnlDownloadProgress"
            pnlDownloadProgress.Location = New System.Drawing.Point(110, 620)
            pnlDownloadProgress.Size = New System.Drawing.Size(ClientSize.Width - 130, 42)
            pnlDownloadProgress.Visible = False
            pnlDownloadProgress.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
            pbDownload = New ProgressBar()
            pbDownload.Name = "pbDownload"
            pbDownload.Location = New System.Drawing.Point(0, 0)
            pbDownload.Size = New System.Drawing.Size(pnlDownloadProgress.Width - 4, 22)
            pbDownload.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            pbDownload.Minimum = 0
            pbDownload.Maximum = 100
            pbDownload.Value = 0
            pbDownload.Style = ProgressBarStyle.Continuous
            lblDownloadStatus = New Label()
            lblDownloadStatus.Name = "lblDownloadStatus"
            lblDownloadStatus.Location = New System.Drawing.Point(0, 24)
            lblDownloadStatus.AutoSize = True
            lblDownloadStatus.Font = New Font("Roboto", 9)
            lblDownloadStatus.ForeColor = Color.DarkGray
            lblDownloadStatus.Text = ""
            pnlDownloadProgress.Controls.Add(pbDownload)
            pnlDownloadProgress.Controls.Add(lblDownloadStatus)
            Controls.Add(pnlDownloadProgress)

            Controls.Add(Me.TextBox1)
            'TextBox1.Visible = False
            Controls.Add(Me.TextBox2)
            Controls.Add(Me.Label1)
            Controls.Add(Me.Label2)
            Label2.Visible = False
            Controls.Add(Me.Butback)
            Controls.Add(Me.TextBox1)
            Controls.Add(Me.TextBox3)
            TextBox3.Visible = False

            'added by sara 
            downloadPollTimer = New Windows.Forms.Timer()
            downloadPollTimer.Interval = DOWNLOAD_POLL_INTERVAL_MS
            AddHandler downloadPollTimer.Tick, AddressOf DownloadPollTimer_Tick

            'ResumeLayout(False)
            MinimumSize = New System.Drawing.Size(800, 600)
            thisTimer = New System.Timers.Timer()
            thisTimer.Enabled = True
            thisTimer.Interval = 1000
            thisTimer.AutoReset = True
            AddHandler thisTimer.Elapsed, AddressOf thisTimer_Tick
            thisTimer.Start()

            CenterToScreen()
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception :" & vbCrLf & ex.Message)
        Finally
        End Try
    End Sub

    Private Sub thisTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.Invoke(Sub()
                          thisTimer.Enabled = True
                          GetCurrentPaths()
                      End Sub)
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception :" & vbCrLf & ex.Message)
        End Try
    End Sub
    'added by sara for progress bar
    Private Sub StartDownloadProgressMonitor()
        pnlDownloadProgress.Visible = True
        pnlDownloadProgress.BringToFront()
        pbDownload.Value = 0
        lblDownloadStatus.Text = "Waiting for service..."
        downloadPollStart = DateTime.Now
        downloadPollTimer.Start()
    End Sub
    Private Sub DownloadPollTimer_Tick(sender As Object, e As EventArgs)
        Try
            Dim savepaths = System.Reflection.Assembly.GetEntryAssembly().Location
            savepaths = Path.GetDirectoryName(savepaths)
            Dim queuePath = Path.Combine(savepaths, "downloadjson", "Downloaddata.txt")
            If pendingBatchIds.Count = 0 Then
                StopDownloadProgressMonitor()
                Return
            End If
            If DateTime.Now.Subtract(downloadPollStart).TotalMinutes > DOWNLOAD_POLL_TIMEOUT_MINUTES Then
                ' lblDownloadStatus.Text = "Download timed out."
                ' StopDownloadProgressMonitor()
                ' Return
            End If
            If Not IO.File.Exists(queuePath) Then
                lblDownloadStatus.Text = "Waiting for queue file..."
                Return
            End If

            '    Dim content As String = IO.File.ReadAllText(queuePath).Trim()
            Dim content As String = ReadQueueText(queuePath).Trim()
            If String.IsNullOrEmpty(content) Then Return
            Dim jobs As List(Of downloadfile) = JsonConvert.DeserializeObject(Of List(Of downloadfile))(content)
            If jobs Is Nothing Then Return
            Dim totalFiles As Integer = 0
            Dim processedFiles As Integer = 0
            Dim allCompleted As Boolean = True
            Dim statusText As String = ""
            For Each batchId As String In pendingBatchIds
                Dim job = jobs.FirstOrDefault(Function(j) j.batchid = batchId)
                If job Is Nothing Then
                    allCompleted = False
                    statusText = "Queued..."
                    Continue For
                End If
                allCompleted = False
                processedFiles += job.NooffilesProcessed
                If downloadTotalFileCount > 0 Then
                    statusText = job.status & " (" & job.NooffilesProcessed.ToString() &
                 " files processed) out of " & downloadTotalFileCount.ToString()
                Else
                    statusText = job.status & " (" & job.NooffilesProcessed.ToString() & " files processed)"
                End If

                'Dim jobTotal = Math.Max(job.Nooffiles, 1)
                'totalFiles += jobTotal
                'processedFiles += Math.Min(job.NooffilesProcessed, jobTotal)
                'statusText = job.status & " (" & job.NooffilesProcessed & "/" & job.Nooffiles & ")"
            Next
            If totalFiles > 1 Then
                pbDownload.Style = ProgressBarStyle.Continuous
                pbDownload.Value = CInt(Math.Min(100, (processedFiles * 100) \ totalFiles))
            Else
                ' Bulk Retail: total unknown — show marquee or steady bar, count in label only
                pbDownload.Style = ProgressBarStyle.Marquee
            End If
            lblDownloadStatus.Text = statusText
            ' Re-check completion (all pending batches found and Completed)
            Dim done As Boolean = True
            For Each batchId As String In pendingBatchIds
                Dim job = jobs.FirstOrDefault(Function(j) j.batchid = batchId)
                If job Is Nothing OrElse job.status <> "Completed" Then
                    done = False
                    Exit For
                End If
                ' Still working
                If job.status = "New" OrElse job.status = "Initializing..." OrElse job.status = "Processing..." Then
                    done = False
                    Exit For
                End If

                ' Completed only when counts match (or total was 0/unknown and service set final Nooffiles)
                If job.status = "Completed" Then
                    If job.Nooffiles > 0 AndAlso job.NooffilesProcessed < job.Nooffiles Then
                        done = False   ' premature Completed — keep bar open
                        Exit For
                    End If
                Else
                    done = False
                    Exit For
                End If
            Next
            If done Then
                pbDownload.Value = 100
                lblDownloadStatus.Text = "Completed"
                StopDownloadProgressMonitor()
            End If
        Catch ex As Exception
            lblDownloadStatus.Text = "Reading progress..."
        End Try
    End Sub
    Public Shared Function ReadQueueText(queuePath As String) As String
        For attempt As Integer = 1 To 10
            Try
                Using fs As New FileStream(queuePath, FileMode.Open, FileAccess.Read,
                                      FileShare.ReadWrite Or FileShare.Delete)
                    Using sr As New StreamReader(fs)
                        Return sr.ReadToEnd()
                    End Using
                End Using
            Catch ex As IOException
                If attempt = 10 Then Throw
                Thread.Sleep(50 * attempt)
            End Try
        Next
        Return String.Empty
    End Function
    Public Shared Sub WriteQueueText(queuePath As String, content As String)
        Dim dir = Path.GetDirectoryName(queuePath)
        If Not Directory.Exists(dir) Then Directory.CreateDirectory(dir)
        Dim tempPath = queuePath & ".tmp"

        File.WriteAllText(tempPath, content, New UTF8Encoding(False))

        For attempt As Integer = 1 To 10
            Try
                If File.Exists(queuePath) Then
                    File.Replace(tempPath, queuePath, Nothing)
                Else
                    File.Move(tempPath, queuePath)
                End If
                Return
            Catch ex As IOException
                If attempt = 10 Then Throw
                Thread.Sleep(100 * attempt)
            End Try
        Next
    End Sub


    Private Sub StopDownloadProgressMonitor()
        downloadPollTimer.Stop()
        pendingBatchIds.Clear()
        pnlDownloadProgress.Visible = False
        pbDownload.Value = 0
        lblDownloadStatus.Text = ""
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
        Property pass As String
        Property foldersize As String
        Property Nooffiles As Integer
        Property batchid As String
        Property datime As String
        Property status As String
        Property NooffilesProcessed As Integer
        Property NooffilesUnprocessed As Integer
        Property KeepBothFiles As String
    End Class


    Public Class downloadfile
        Property foldername As String
        Property movepath As String
        Property passwordd As String
        Property dfoldersize As String
        Property Nooffiles As Integer
        Property extension As String
        Property batchid As String
        Property datime As String
        Property status As String
        Property NooffilesProcessed As Integer
        Property NooffilesUnprocessed As Integer
        '  Property KeepBothFiles As String
    End Class


    Public Class downloadobj
        Public Property info2 As List(Of downloadfile)
    End Class

    'Public Sub Butback_Click(sender As Object, e As EventArgs)
    '    MsgBox("back")
    'End Sub
    Public Sub Butdownload_Click(sender As Object, e As EventArgs)
        Try
            Dim Settings As New JsonSerializerSettings
            Settings.Formatting = Formatting.Indented
            Settings.NullValueHandling = NullValueHandling.Ignore
            Dim downpathinfo As New DirectoryInfo(selectpath)
            'shiva

            'downpathinfo.GetFiles("*.ezo")
            Dim info2 As New List(Of downloadfile)
            Dim check As Boolean = True
            'Dim downloadclick = "download"
            'frmbatch.clickdownload = downloadclick
            Dim frmdes As Popup = New Popup()
            frmdes.DecryptGrid.Visibility = Visibility.Collapsed
            frmdes.DownloadGrid.Visibility = Visibility.Visible

            frmdes.Labelpass.Content = "Download Password"
            frmdes.btnchsefolder.Visibility = Visibility.Visible
            frmdes.wrongpassDownload.Visibility = Visibility.Collapsed
            '  MsgBox(frmdes.ShowDialog().Value.ToString)

            frmdes.DecryptGrid.Visibility = Visibility.Collapsed
            frmdes.DownloadGrid.Visibility = Visibility.Visible
            frmdes.decryptpath.Content = selectpath
            'this line added by sara
            Popup.SearchPaths = GetSelectedFolderPath()
            frmdes.ShowDialog()
            'this line added by sara 
            Popup.SearchPaths = Nothing
            If CustomMessageBoxResult = 0 Then
                Exit Sub
                Me.Close()
            End If
            'If (Popup.ClickedButton = "Btn_cancel") Then
            '    Exit Sub
            '    Me.Close()
            'End If
            'shiva
            '//MsgBox(((frmdes)this.Owner).Text)
            ' MsgBox(frmdes.ShowDialog().GetValueOrDefault.ToString)
            Dim movepath = Popup.Destdir
            Dim password = Popup.passworkw
            Dim filepath = selectpath
            Dim folderpath() = GetSelectedFolderPath()
            ' Dim keepbothfiles = Popup.keepbothfiles

            For Each Downloadpath As String In folderpath
                info2.Clear()
                If Downloadpath <> "" Then
                    Dim split As String() = Downloadpath.Split(CType("\", Char()))
                    Dim parentFolder As String = split(split.Length - 2)
                    Dim DBatchid = Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss") + "-" + parentFolder
                    DBatchid = DBatchid.Replace("/", "")
                    DBatchid = DBatchid.Replace(":", "")
                    DBatchid = DBatchid.Replace(" ", "")
                    Dim ddtime = Format(DateTime.Now, "dd/MMM/yyyy hh:mm:ss tt") '.Replace("/", "").Replace(":", "").Replace(" ", "")

                    'folderDfilecount = Directory.GetFiles(Downloadpath + "\", "*.ezo", IO.SearchOption.AllDirectories).Count
                    folderDfilecount = 0
                    Dim filesizedpath As New DirectoryInfo(Downloadpath)
                    ' Dim lTotalFileSizes As Long = GetDirectoryFileSize(filesizedpath)
                    Dim lTotalFileSizes As Long = 0
                    If password <> "" Then
                        Dim b As New downloadfile
                        b.foldername = Downloadpath
                        b.movepath = movepath
                        b.passwordd = password
                        'shiva
                        'b.dfoldersize = Format(lTotalFileSizes / 1024 / 1024, "###,0.00") & " MB"
                        b.dfoldersize = Format(lTotalFileSizes, "###,0.00") & " MB"

                        b.Nooffiles = folderDfilecount
                        b.batchid = DBatchid
                        b.datime = ddtime
                        b.status = "New"
                        b.NooffilesProcessed = 0
                        b.NooffilesUnprocessed = 0
                        ' b.KeepBothFiles = keepbothfiles
                        info2.Add(b)
                        'added by sara for progrss bar visible 
                        pendingBatchIds.Add(DBatchid)
                        Dim pathToCount As String = Downloadpath
                        downloadTotalFileCount = 0
                        Task.Run(Sub()
                                     Try
                                         Dim count As Integer = 0
                                         For Each f As String In Directory.EnumerateFiles(pathToCount, "*.ezo", SearchOption.AllDirectories)
                                             count += 1
                                         Next
                                         Me.BeginInvoke(New Action(Sub()
                                                                       downloadTotalFileCount = count
                                                                   End Sub))
                                     Catch ex As Exception

                                     End Try
                                 End Sub)
                    End If

                    Dim client = New WebClient()
                    client.Headers("Content-Type") = "application/json"
                    client.Encoding = System.Text.Encoding.UTF8
                    Dim bool2 As Boolean = IO.File.Exists(dwnpath)
                    If bool2 = True Then
                        ' Dim uristring1 = File.ReadAllText(dwnpath)
                        Dim uristring1 = ReadQueueText(dwnpath)
                        Dim fileinfoss As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(uristring1)
                        If uristring1 <> "" Then
                            Dim jpaths = uristring1.Substring(0, uristring1.Length - 2)
                            Dim jsond As String = JsonConvert.SerializeObject(info2, Settings)
                            Dim djson As String = jsond.Substring(1)
                            Dim savejsond = jpaths + "," + djson '.Replace(" ", "")
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
                            ' Dim content = File.ReadAllText(filelocation).Trim()
                            Dim content = ReadQueueText(filelocation).Trim()
                            Dim existing As New List(Of downloadfile)
                            If Not String.IsNullOrEmpty(content) Then
                                Dim loaded = JsonConvert.DeserializeObject(Of List(Of downloadfile))(content)
                                If loaded IsNot Nothing Then
                                    existing = loaded
                                End If
                            End If
                            If info2 IsNot Nothing AndAlso info2.Count > 0 Then
                                existing.AddRange(info2)
                                ' File.WriteAllText(filelocation, JsonConvert.SerializeObject(existing, Settings))
                                WriteQueueText(filelocation, JsonConvert.SerializeObject(existing, Settings))
                                'Else
                                '    Using sw As StreamWriter = New StreamWriter(filelocation, True)
                                '        sw.Write(jsond)
                                '    End Using
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
                        ' Dim content = File.ReadAllText(filelocation).Trim()
                        Dim content = ReadQueueText(filelocation).Trim()
                        Dim existing As New List(Of downloadfile)
                        If Not String.IsNullOrEmpty(content) Then
                            Dim loaded = JsonConvert.DeserializeObject(Of List(Of downloadfile))(content)
                            If loaded IsNot Nothing Then
                                existing = loaded
                            End If
                        End If
                        If info2 IsNot Nothing AndAlso info2.Count > 0 Then
                            existing.AddRange(info2)
                            ' File.WriteAllText(filelocation, JsonConvert.SerializeObject(existing, Settings))
                            WriteQueueText(filelocation, JsonConvert.SerializeObject(existing, Settings))
                            'Else
                            '    Using sw As StreamWriter = New StreamWriter(filelocation, True)
                            '        sw.Write(jsond)
                            '    End Using
                        End If
                    End If
                Else
                    frmdes.wrongpassDownload.Content = "Enter Password"
                    frmdes.wrongpassDownload.Visibility = Visibility.Visible
                End If
            Next
            'added by sara
            If pendingBatchIds.Count > 0 Then
                StartDownloadProgressMonitor()
            End If
            custommsgbox.showCustomMessageBox("Info", "Process Initiated Successfully!")
            'MsgBox("File Download Process Initiated Successfully", vbOKOnly, "STANDALONE EXPLORER:Notification")
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception :" & vbCrLf & ex.Message)
        End Try
    End Sub


    Public Sub Butdecrypt_Click(sender As Object, e As EventArgs)
        Try
            Dim Settings As New JsonSerializerSettings
            Settings.Formatting = Formatting.Indented
            Settings.NullValueHandling = NullValueHandling.Ignore
            Dim spathinfo As New DirectoryInfo(selectpath)
            Dim check As Boolean = True
            'Dim decryptclick = "Decrypt"
            'frmbatch.clickdecrypy = decryptclick
            Dim frmpass As Popup = New Popup()
            frmpass.Labelpass.Content = "Decrypt Password"
            frmpass.Labeldes.Visibility = Visibility.Hidden
            frmpass.Destfolder.Visibility = Visibility.Hidden
            frmpass.btnchsefolder.Visibility = Visibility.Hidden
            frmpass.wrongpassDecrypt.Visibility = Visibility.Hidden

            frmpass.DownloadGrid.Visibility = Visibility.Collapsed
            frmpass.DecryptGrid.Visibility = Visibility.Visible
            frmpass.decryptpath.Content = selectpath
            'this line added by sara
            Popup.SearchPaths = GetSelectedFolderPath()

            frmpass.ShowDialog()
            'this line added by sara
            Popup.SearchPaths = Nothing

            If CustomMessageBoxResult = 0 Then
                Exit Sub
                Me.Close()
            End If
            'If (Popup.ClickedButton = "Btn_cancel") Then
            '    Exit Sub
            '    Me.Close()
            'End If

            'If frmpass.ShowDialog() Then
            'End If
            Dim keypasse = Popup.passworkw
            Dim Rootobject As New Rootobject
            Dim info As New List(Of folderinfo)
            Dim folderpath() = GetSelectedFolderPath()
            Dim keepbothfiles = Popup.keepbothfiles

            If keypasse <> "" Then
                For Each path As String In folderpath
                    If path <> "" Then
                        Dim split As String() = path.Split(CType("\", Char()))
                        Dim parentFolder As String = split(split.Length - 2)
                        Dim Batchid = Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss") + "-" + parentFolder
                        Batchid = Batchid.Replace("/", "")
                        Batchid = Batchid.Replace(":", "")
                        Batchid = Batchid.Replace(" ", "")
                        Dim dtime = Format(DateTime.Now, "dd/MMM/yyyy hh:mm:ss tt") '.Replace("/", "").Replace(":", "").Replace(" ", "")

                        'folderfilecount = Directory.GetFiles(path + "\", "*.ezo", IO.SearchOption.AllDirectories).Count
                        folderfilecount = 0
                        Dim directoryinfos As New DirectoryInfo(path)
                        'Dim lTotalFileSize As Long = GetDirectoryFileSize(directoryinfos)
                        Dim lTotalFileSize As Long = 0
                        If path <> "" Then
                            Dim a As New folderinfo
                            fcount += 1
                            a.foldername = path
                            a.pass = keypasse
                            'a.foldersize = Format(lTotalFileSize / 1024 / 1024, "###,0.00") & " MB"
                            a.foldersize = Format(lTotalFileSize, "###,0.00") & " MB"
                            a.Nooffiles = folderfilecount
                            a.batchid = Batchid
                            a.datime = dtime
                            a.status = "New"
                            a.NooffilesProcessed = 0
                            a.NooffilesUnprocessed = 0
                            a.KeepBothFiles = keepbothfiles
                            info.Add(a)
                        End If
                    Else
                        Dim split As String() = selectpath.Split(CType("\", Char()))
                        Dim parentFolder As String = split(split.Length - 2)
                        Dim Batchid = Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss") + "-" + parentFolder
                        Batchid = Batchid.Replace("/", "")
                        Batchid = Batchid.Replace(":", "")
                        Batchid = Batchid.Replace(" ", "")
                        Dim dtime = Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss").Replace("/", "").Replace(":", "").Replace(" ", "")

                        'folderfilecount = Directory.GetFiles(selectpath + "\", "*.ezo", IO.SearchOption.AllDirectories).Count
                        folderfilecount = 0
                        Dim directoryinfos As New DirectoryInfo(path)
                        'Dim lTotalFileSize As Long = GetDirectoryFileSize(spathinfo)
                        Dim lTotalFileSize As Long = 0
                        If selectpath <> "" Then
                            Dim a As New folderinfo
                            'Dim fname As String = IO.Path.GetFileName(filename)
                            'a.extension = System.IO.Path.GetExtension(filename).Replace(".", "")
                            fcount += 1
                            a.foldername = selectpath
                            a.pass = keypasse
                            'a.foldersize = Format(lTotalFileSize / 1024 / 1024, "###,0.00") & " MB"
                            a.foldersize = Format(lTotalFileSize, "###,0.00") & " MB"
                            a.Nooffiles = folderfilecount
                            a.batchid = Batchid
                            a.datime = dtime
                            a.status = "New"
                            a.NooffilesProcessed = 0
                            a.NooffilesUnprocessed = 0
                            a.KeepBothFiles = keepbothfiles
                            info.Add(a)
                        End If
                    End If
                Next
            Else
                frmpass.wrongpassDecrypt.Content = "Enter Password"
                frmpass.wrongpassDecrypt.Visibility = Visibility.Visible
            End If
            If keypasse <> "" Then
                Dim client = New WebClient()
                client.Headers("Content-Type") = "application/json"
                client.Encoding = System.Text.Encoding.UTF8
                Dim bool1 As Boolean = IO.File.Exists(jspath)
                If bool1 = True Then
                    'Dim uristring = File.ReadAllText(jspath)
                    Dim uristring = ReadQueueText(jspath)
                    Dim fileinfos As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring)
                    If uristring <> "" Then
                        Dim jpath = uristring.Substring(0, uristring.Length - 2)
                        Dim json As String = JsonConvert.SerializeObject(info, Settings)
                        Dim rjson As String = json.Substring(1)
                        Dim savejson = jpath + "," + rjson '.Replace(" ", "")
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
                            'shiva

                            custommsgbox.showCustomMessageBox("Info", "Process Initiated Successfully!")
                            'MsgBox("File Decrypt Process Initiated Successfully", vbOKOnly, "STANDALONE EXPLORER:Notification")
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
                            'shiva
                            custommsgbox.showCustomMessageBox("Info", "Process Initiated Successfully!")
                            'MsgBox("File Decrypt Process Initiated Successfully", vbOKOnly, "STANDALONE EXPLORER:Notification")
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
                        'shiva
                        custommsgbox.showCustomMessageBox("Info", "Process Initiated Successfully!")
                        'MsgBox("File Decrypt Process Initiated Successfully", vbOKOnly, "STANDALONE EXPLORER:Notification")
                    End Using
                End If
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception :" & vbCrLf & ex.Message)
        End Try
    End Sub


    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Dim hWndBrowserControl As IntPtr = FindWindowEx(Me.Handle, IntPtr.Zero, "ExplorerBrowserControl", Nothing)
        If (hWndBrowserControl <> IntPtr.Zero) Then
            'Dim rcBrowser As RECT = New RECT()
            'rcBrowser.left = 10
            'rcBrowser.top = nTopExplorerBrowser
            'rcBrowser.right = Me.ClientSize.Width - 10
            'rcBrowser.bottom = Me.ClientSize.Height - nTopExplorerBrowser - 10
            'pExplorerBrowser.SetRect(IntPtr.Zero, rcBrowser)
            MoveWindow(hWndBrowserControl, 10, nTopExplorerBrowser, Me.ClientSize.Width - 10 * 2, Me.ClientSize.Height - nTopExplorerBrowser - 5, True)
        End If
        If (TextBox2 IsNot Nothing) Then
            'MoveWindow(TextBox2.Handle, 10, 10, ClientSize.Width - 100 - 10 - 10 - 10, 20, True)
        End If
    End Sub

    Public Function GetSelectedFolderPath() As String()
        Dim selectedcount = 0
        Dim selectedfolderpath() As String
        Try
            Dim hr As HRESULT = HRESULT.E_FAIL
            Dim pFolderViewSearch As IFolderView2 = Nothing
            Dim pFolderViewSearchPtr As IntPtr
            Dim IID_IFolderView2 As New Guid("1af3a467-214f-4298-908e-06b03e0b39f9")
            Dim bContinue As Boolean = True
            hr = pExplorerBrowser.GetCurrentView(IID_IFolderView2, pFolderViewSearchPtr)
            If (hr = HRESULT.S_OK) Then
                pFolderViewSearch = DirectCast(Marshal.GetObjectForIUnknown(pFolderViewSearchPtr), IFolderView2)
                Dim pShellItemPtr As IntPtr
                Dim IID_IShellItem As New Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")
                hr = pFolderViewSearch.GetFolder(IID_IShellItem, pShellItemPtr)
                Dim pShellItem As IShellItem = DirectCast(Marshal.GetObjectForIUnknown(pShellItemPtr), IShellItem)
                Dim sbItemName As StringBuilder = New StringBuilder(260)
                hr = pShellItem.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, sbItemName)
                Dim selecteditem As IShellItemArray = Nothing
                hr = pFolderViewSearch.GetSelection(True, selecteditem)
                If (hr = HRESULT.S_OK) Then
                    hr = selecteditem.GetCount(selectedcount)
                    ReDim selectedfolderpath(selectedcount - 1)
                    For item = 0 To selectedcount - 1
                        Dim folderpath As StringBuilder = New StringBuilder(260)
                        hr = selecteditem.GetItemAt(item, pShellItem)
                        hr = pShellItem.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, folderpath)
                        'shiva
                        'MsgBox(folderpath.ToString.Substring(0, folderpath.ToString.LastIndexOf("\")))


                        If (Path.HasExtension(folderpath.ToString)) Then
                            selectedfolderpath(item) = folderpath.ToString.Substring(0, folderpath.ToString.LastIndexOf("\"))
                        Else
                            selectedfolderpath(item) = folderpath.ToString
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception in GetSelectedFolderPath : " & vbCrLf & ex.Message)
            'MsgBox(" GetSelectedFolderPath : " & ex.Message, vbOKOnly, "STANDALONE EXPLORER:Notification")
        End Try
        Return selectedfolderpath
    End Function

    ' Functions adapted From SDK :
    ' https://github.com/microsoft/Windows-classic-samples/tree/master/Samples/Win7Samples/winui/shell/appplatform/ExplorerBrowserSearch
    Public Function AddCustomCondition(psfif As ISearchFolderItemFactory) As HRESULT
        Dim hr As HRESULT = HRESULT.E_FAIL
        Dim pConditionFactory As IConditionFactory = Nothing
        Dim CLSID_ConditionFactory As New Guid("E03E85B0-7BE3-4000-BA98-6C13DE9FA486")
        Dim ConditionFactoryType As Type = Type.GetTypeFromCLSID(CLSID_ConditionFactory, True)
        Dim ConditionFactory As Object = Activator.CreateInstance(ConditionFactoryType)
        pConditionFactory = DirectCast(ConditionFactory, IConditionFactory)
        If (pConditionFactory IsNot Nothing) Then
            Dim pv As PROPVARIANT = New PROPVARIANT()
            pv.varType = CUShort(VT.VT_LPWSTR)
            pv.pwszVal = Marshal.StringToHGlobalUni(TextBox1.Text)
            Dim pCondition As ICondition = Nothing
            hr = pConditionFactory.MakeLeaf("System.FileName", CONDITION_OPERATION.COP_DOSWILDCARDS, Nothing, pv, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, False, pCondition)
            If (hr = HRESULT.S_OK) Then
                hr = psfif.SetCondition(pCondition)
                Marshal.ReleaseComObject(pCondition)
            End If
            Marshal.ReleaseComObject(pConditionFactory)
        End If
        Return hr
    End Function

    Public Function CreateQueryParser(ByRef pQueryParser As IQueryParser) As HRESULT
        Dim hr As HRESULT = HRESULT.E_FAIL
        Dim pQueryParserManager As IQueryParserManager = Nothing
        Dim CLSID_QueryParserManager As New Guid("5088B39A-29B4-4d9d-8245-4EE289222F66")
        Dim QueryParserManagerType As Type = Type.GetTypeFromCLSID(CLSID_QueryParserManager, True)
        Dim QueryParserManager As Object = Activator.CreateInstance(QueryParserManagerType)
        pQueryParserManager = DirectCast(QueryParserManager, IQueryParserManager)
        If (pQueryParserManager IsNot Nothing) Then
            Dim pQueryParserPtr As IntPtr
            Dim IID_IQueryParser As New Guid("{2EBDEE67-3505-43F8-9946-EA44ABC8E5B0}")
            hr = pQueryParserManager.CreateLoadedParser("SystemIndex", LOCALE_USER_DEFAULT, IID_IQueryParser, pQueryParserPtr)
            If (hr = HRESULT.S_OK) Then
                pQueryParser = DirectCast(Marshal.GetObjectForIUnknown(pQueryParserPtr), IQueryParser)
                hr = pQueryParserManager.InitializeOptions(False, True, pQueryParser)
                If (hr = HRESULT.S_OK) Then
                    For i As Integer = 0 To sGenericProperties.Length - 1
                        Dim pv As PROPVARIANT = New PROPVARIANT()
                        pv.varType = CUShort(VT.VT_LPWSTR)
                        pv.pwszVal = Marshal.StringToHGlobalUni(sGenericProperties(i))
                        hr = pQueryParser.SetMultiOption(STRUCTURED_QUERY_MULTIOPTION.SQMO_DEFAULT_PROPERTY, sGenericSemanticTypes(i), pv)
                    Next
                End If
            End If
            Marshal.ReleaseComObject(pQueryParserManager)
        End If
        Return hr
    End Function

    Public Function AddStructuredQueryCondition(psfif As ISearchFolderItemFactory, pqp As IQueryParser, pszQuery As String) As HRESULT
        Dim pc As ICondition = Nothing
        Dim hr As HRESULT = ParseStructuredQuery(pszQuery, pqp, pc)
        If (hr = HRESULT.S_OK) Then
            hr = psfif.SetCondition(pc)
            Marshal.ReleaseComObject(pc)
        End If
        Return hr
    End Function

    Private Sub ButUp_Click(sender As Object, e As EventArgs) Handles ButUp.Click

        Try
            TextBox1.Text = ""

            Dim splitpath As String() = selectpath.Split({"\"}, StringSplitOptions.RemoveEmptyEntries)
            If (selectpath = TextBox3.Text) Then
                ButUp.Enabled = False
                custommsgbox.showCustomMessageBox("Info", "Couldnt Move Up Further")
                Exit Sub
            End If
            If (splitpath.Length > 1) Then

                If (splitpath.Length = 2) Then
                    If (selectpath.Contains("\\")) Then
                        ButUp.Enabled = False
                        custommsgbox.showCustomMessageBox("Info", "Couldnt Move Up Further")
                        'MsgBox("Couldnt Up Further", vbOKOnly, "STANDALONE EXPLORER:Notification")
                    Else
                        repopath = Directory.GetParent(selectpath).ToString()

                        Try
                            Dim pidlFull As IntPtr = IntPtr.Zero
                            Dim hr = SHILCreateFromPath(repopath, pidlFull, Nothing)
                            If (hr = HRESULT.S_OK) Then
                                pExplorerBrowser.BrowseToIDList(pidlFull, SBSP_ABSOLUTE)
                                Dim pfs As FOLDERSETTINGS = New FOLDERSETTINGS()
                                pfs.ViewMode = FOLDERVIEWMODE.FVM_LIST
                                hr = pExplorerBrowser.SetFolderSettings(pfs)
                                hr = CreateQueryParser(pQueryParser)
                                GetCurrentPaths()
                            End If
                        Catch ex As Exception
                            custommsgbox.showCustomMessageBox("error", "Exception in ButUp :" & vbCrLf & ex.Message)
                            'MsgBox("ButUp" & ex.Message, vbOKOnly, "STANDALONE EXPLORER:Notification")
                        End Try
                    End If
                Else
                    ButUp.Enabled = True

                    repopath = Directory.GetParent(selectpath).ToString()

                    Try
                        Dim pidlFull As IntPtr = IntPtr.Zero
                        Dim hr = SHILCreateFromPath(repopath, pidlFull, Nothing)
                        If (hr = HRESULT.S_OK) Then
                            pExplorerBrowser.BrowseToIDList(pidlFull, SBSP_ABSOLUTE)
                            Dim pfs As FOLDERSETTINGS = New FOLDERSETTINGS()
                            pfs.ViewMode = FOLDERVIEWMODE.FVM_LIST
                            hr = pExplorerBrowser.SetFolderSettings(pfs)
                            hr = CreateQueryParser(pQueryParser)
                            GetCurrentPaths()
                            'repopath = TextBox2.Text
                        End If
                    Catch ex As Exception
                        custommsgbox.showCustomMessageBox("error", "Exception in ButUp :" & vbCrLf & ex.Message)
                        'MsgBox("ButUp" & ex.Message, vbOKOnly, "STANDALONE EXPLORER:Notification")
                    End Try
                End If
                'Else
                '    ButUp.Enabled = False

                '    repopath = Directory.GetParent(selectpath).ToString()

                '    Try
                '        Dim pidlFull As IntPtr = IntPtr.Zero
                '        Dim hr = SHILCreateFromPath(repopath, pidlFull, Nothing)
                '        If (hr = HRESULT.S_OK) Then
                '            pExplorerBrowser.BrowseToIDList(pidlFull, SBSP_ABSOLUTE)
                '            Dim pfs As FOLDERSETTINGS = New FOLDERSETTINGS()
                '            pfs.ViewMode = FOLDERVIEWMODE.FVM_ICON
                '            hr = pExplorerBrowser.SetFolderSettings(pfs)
                '            hr = CreateQueryParser(pQueryParser)
                '            GetCurrentPaths()
                '            'repopath = TextBox2.Text
                '        End If
                '    Catch ex As Exception
                '        MsgBox("ButUp" & ex.Message)
                '    End Try

            End If



            'End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception in ButUp :" & vbCrLf & ex.Message)
            'MsgBox("ButUp " & ex.Message)
        End Try

    End Sub

    Private Sub ButHome_Click(sender As Object, e As EventArgs) Handles ButHome.Click
        ' Dim frm As Form2 = New Form2()
        Try
            Me.repopath = TextBox3.Text
            Dim pidlFull As IntPtr = IntPtr.Zero

            'me.ShowDialog()
            'TextBox2.Text = TextBox3.Text
            Dim hr = SHILCreateFromPath(repopath, pidlFull, Nothing)
            If (hr = HRESULT.S_OK) Then
                pExplorerBrowser.BrowseToIDList(pidlFull, SBSP_ABSOLUTE)
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception in ButHome_Click :" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub BtnNotify_Click(sender As Object, e As EventArgs) Handles BtnNotify.Click
        Try
            Dim frmbatch As Batching = New Batching()
            If frmbatch.ShowDialog() Then
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception in BtnNotify_Click :" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Function ParseStructuredQuery(pszString As String, pqp As IQueryParser, ByRef ppc As ICondition) As HRESULT
        ppc = Nothing
        Dim pqs As IQuerySolution = Nothing
        Dim hr As HRESULT = pqp.Parse(pszString, IntPtr.Zero, pqs)
        If (hr = HRESULT.S_OK) Then
            Dim pc As ICondition = Nothing
            hr = pqs.GetQuery(pc, IntPtr.Zero)
            If (hr = HRESULT.S_OK) Then
                Dim st As SYSTEMTIME
                GetLocalTime(st)
                hr = pqs.Resolve(pc, STRUCTURED_QUERY_RESOLVE_OPTION.SQRO_DONT_SPLIT_WORDS, st, ppc)
                Marshal.ReleaseComObject(pc)
            End If
        End If
        Return hr
    End Function



    Public Function GetCurrentPaths() As String
        Try
            If (TextBox1.Text = "" AndAlso Not filesearch) Then
                Dim pFolderViewPtr As IntPtr
                Dim IID_IFolderView As New Guid("cde725b0-ccc9-4519-917e-325d72fab4ce")
                Dim hr = pExplorerBrowser.GetCurrentView(IID_IFolderView, pFolderViewPtr)
                If (hr = HRESULT.S_OK) Then
                    Dim pFolderView As IFolderView = DirectCast(Marshal.GetObjectForIUnknown(pFolderViewPtr), IFolderView)
                    Dim pShellItemPtr As IntPtr
                    Dim IID_IShellItem As New Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")
                    hr = pFolderView.GetFolder(IID_IShellItem, pShellItemPtr)
                    If (hr = HRESULT.S_OK) Then
                        hr = pFolderView.GetFolder(IID_IShellItem, pShellItemPtr)
                        If (hr = HRESULT.S_OK) Then
                            Dim pShellItem As IShellItem = DirectCast(Marshal.GetObjectForIUnknown(pShellItemPtr), IShellItem)
                            Dim sbItemName As StringBuilder = New StringBuilder(260)
                            hr = pShellItem.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, sbItemName)
                            Dim sItemName As String = sbItemName.ToString()
                            If TextBox2.Focused Then
                                Return ""
                            End If

                            TextBox2.Text = sItemName
                            selectpath = TextBox2.Text

                            Dim split As String() = selectpath.Split({"\"}, StringSplitOptions.RemoveEmptyEntries)

                            If (split.Length > 1) Then
                                '    If (split.Length = 2) Then
                                '        If (selectpath.Contains("\\")) Then
                                '            ButUp.Enabled = False
                                '        End If
                                '    End If
                                ButUp.Enabled = True

                            Else
                                ButUp.Enabled = False

                            End If

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception in GetCurrentPaths :" & vbCrLf & ex.Message)
        End Try
        Return ""
    End Function

    Private Sub ButSearch_Click(sender As Object, e As EventArgs) Handles ButSearch.Click
        Try

            If (TextBox1.Text <> "") Then
                filesearch = True
                Dim hr As HRESULT = HRESULT.E_FAIL
                ' If e.KeyCode = Keys.Enter Then
                Dim pSearchFolderItemFactory As ISearchFolderItemFactory = Nothing
                    Dim CLSID_SearchFolderItemFactory As New Guid("14010e02-bbbd-41f0-88e3-eda371216584")
                    Dim SearchFolderItemFactoryType As Type = Type.GetTypeFromCLSID(CLSID_SearchFolderItemFactory, True)
                    Dim SearchFolderItemFactory As Object = Activator.CreateInstance(SearchFolderItemFactoryType)
                    pSearchFolderItemFactory = DirectCast(SearchFolderItemFactory, ISearchFolderItemFactory)
                    If (pSearchFolderItemFactory IsNot Nothing) Then
                        hr = pSearchFolderItemFactory.SetDisplayName(TextBox1.Text)
                        Dim pFolderViewPtr As IntPtr
                        Dim IID_IFolderView As New Guid("cde725b0-ccc9-4519-917e-325d72fab4ce")
                        hr = pExplorerBrowser.GetCurrentView(IID_IFolderView, pFolderViewPtr)
                        If (hr = HRESULT.S_OK) Then
                            Dim pFolderView As IFolderView = DirectCast(Marshal.GetObjectForIUnknown(pFolderViewPtr), IFolderView)
                            Dim pShellItemPtr As IntPtr
                            Dim IID_IShellItem As New Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")
                            hr = pFolderView.GetFolder(IID_IShellItem, pShellItemPtr)
                            If (hr = HRESULT.S_OK) Then
                                Dim pShellItem As IShellItem = DirectCast(Marshal.GetObjectForIUnknown(pShellItemPtr), IShellItem)
                                Dim sbItemName As StringBuilder = New StringBuilder(260)
                                'hr = pShellItem.GetDisplayName(SIGDN.SIGDN_NORMALDISPLAY, sbItemName)
                                hr = pShellItem.GetDisplayName(SIGDN.SIGDN_PARENTRELATIVEFORADDRESSBAR, sbItemName)
                                'Test if the current view is the result of a search
                                'https://docs.microsoft.com/en-us/windows/desktop/search/-search-3x-wds-qryidx-searchms
                                Dim sItemName As String = sbItemName.ToString()
                                If (sItemName.Contains("search-ms")) Then
                                    Dim nLocationPos As Integer = sItemName.IndexOf("crumb=location:")
                                    Dim sLocation As String = sbItemName.ToString().Substring(nLocationPos + Len("crumb=location:"))
                                    Dim pShellItemLocationPtr As IntPtr
                                    Dim sDecodedLocation As String = Uri.UnescapeDataString(sLocation)
                                    hr = SHCreateItemFromParsingName(sDecodedLocation, IntPtr.Zero, IID_IShellItem, pShellItemLocationPtr)
                                    pShellItem = DirectCast(Marshal.GetObjectForIUnknown(pShellItemLocationPtr), IShellItem)
                                End If
                                '{search-ms:displayname=*.jpg&crumb=nomdefichier%3A~"*.jpg"&crumb=location:E%3A%5CToto}
                                Dim pShellItemArray As IShellItemArray = Nothing
                                Dim IID_IShellItemArray As New Guid("{B63EA76D-1F85-456F-A19C-48159EFA858B}")
                                hr = SHCreateShellItemArrayFromShellItem(pShellItem, IID_IShellItemArray, pShellItemArray)
                                If (hr = HRESULT.S_OK) Then
                                    hr = pSearchFolderItemFactory.SetScope(pShellItemArray)
                                    If (hr = HRESULT.S_OK) Then
                                        'hr = AddCustomCondition(pSearchFolderItemFactory)
                                        hr = AddStructuredQueryCondition(pSearchFolderItemFactory, pQueryParser, TextBox1.Text)
                                        If (hr = HRESULT.S_OK) Then
                                            Dim pShellItemPtr2 As IntPtr
                                            hr = pSearchFolderItemFactory.GetShellItem(IID_IShellItem, pShellItemPtr2)
                                            If (hr = HRESULT.S_OK) Then
                                                hr = pExplorerBrowser.BrowseToObject(pShellItemPtr2, 0)
                                                Dim pShellItem2 As IShellItem = DirectCast(Marshal.GetObjectForIUnknown(pShellItemPtr), IShellItem)
                                                Dim sbItemNamea As StringBuilder = New StringBuilder(260)
                                                hr = pShellItem2.GetDisplayName(SIGDN.SIGDN_NORMALDISPLAY, sbItemName)
                                                'hr = pShellItem2.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, sbItemName)
                                                'Test if the current view is the result of a search
                                                'https://docs.microsoft.com/en-us/windows/desktop/search/-search-3x-wds-qryidx-searchms
                                                Dim sItemNamea As String = sbItemName.ToString()
                                                'TextBox2.Text = sItemNamea
                                                ' Remove columns
                                                Dim t As New System.Threading.Thread(AddressOf ThreadProc)
                                                '  t.Start()

                                                'Dim pidlFull As IntPtr = IntPtr.Zero
                                                'hr = SHILCreateFromPath(TextBox2.Text, pidlFull, Nothing)
                                                'If (hr = HRESULT.S_OK) Then
                                                '    hr = pExplorerBrowser.BrowseToIDList(pidlFull, SBSP_ABSOLUTE)
                                                '    Dim pfs As FOLDERSETTINGS = New FOLDERSETTINGS()
                                                '    pfs.ViewMode = FOLDERVIEWMODE.FVM_LIST
                                                '    hr = pExplorerBrowser.SetFolderSettings(pfs)
                                                '    hr = CreateQueryParser(pQueryParser)
                                                '    GetCurrentPaths()
                                                'End If

                                                Marshal.ReleaseComObject(pShellItem2)

                                                'Marshal.Release(pShellItemPtr2)
                                            End If
                                        End If
                                    End If
                                    Marshal.ReleaseComObject(pShellItemArray)
                                End If
                                Marshal.ReleaseComObject(pShellItem)
                            End If
                            Marshal.ReleaseComObject(pFolderView)
                        End If
                        Marshal.ReleaseComObject(pSearchFolderItemFactory)
                    End If

                    'e.Handled = True
                    '  e.SuppressKeyPress = True
                    ' End If
                Else
                Dim pidlFull As IntPtr = IntPtr.Zero
                Dim hr = SHILCreateFromPath(TextBox2.Text, pidlFull, Nothing)
                If (hr = HRESULT.S_OK) Then
                    hr = pExplorerBrowser.BrowseToIDList(pidlFull, SBSP_ABSOLUTE)
                    Dim pfs As FOLDERSETTINGS = New FOLDERSETTINGS()
                    pfs.ViewMode = FOLDERVIEWMODE.FVM_LIST
                    hr = pExplorerBrowser.SetFolderSettings(pfs)
                    hr = CreateQueryParser(pQueryParser)
                End If
                filesearch = False
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception in TextBox1_KeyDown : " & vbCrLf & ex.Message)
            'MsgBox("TextBox1_KeyDown : " + ex.Message, vbOKOnly, "STANDALONE EXPLORER:Notification")
        End Try
    End Sub



    ' To check current view and remove columns for search result view
    Public Sub ThreadProc()
        Dim hr As HRESULT = HRESULT.E_FAIL
        Dim pFolderViewSearch As IFolderView2 = Nothing
        Dim pFolderViewSearchPtr As IntPtr
        Dim IID_IFolderView2 As New Guid("1af3a467-214f-4298-908e-06b03e0b39f9")
        Dim bContinue As Boolean = True
        While (bContinue)
            hr = pExplorerBrowser.GetCurrentView(IID_IFolderView2, pFolderViewSearchPtr)
            If (hr = HRESULT.S_OK) Then
                pFolderViewSearch = DirectCast(Marshal.GetObjectForIUnknown(pFolderViewSearchPtr), IFolderView2)
                Dim pShellItemPtr As IntPtr
                Dim IID_IShellItem As New Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")
                hr = pFolderViewSearch.GetFolder(IID_IShellItem, pShellItemPtr)
                If (hr = HRESULT.S_OK) Then
                    Dim pShellItem As IShellItem = DirectCast(Marshal.GetObjectForIUnknown(pShellItemPtr), IShellItem)
                    Dim sbItemName As StringBuilder = New StringBuilder(260)
                    hr = pShellItem.GetDisplayName(SIGDN.SIGDN_PARENTRELATIVEFORADDRESSBAR, sbItemName)
                    Dim sItemName As String = sbItemName.ToString()
                    'TextBox2.Text = sItemName
                    If (sItemName.Contains("search-ms") = False) Then
                        bContinue = True
                        Marshal.ReleaseComObject(pFolderViewSearch)
                    Else
                        bContinue = False
                    End If
                    Marshal.ReleaseComObject(pShellItem)
                End If
            End If
            System.Threading.Thread.Sleep(100)
        End While
        hr = pFolderViewSearch.SetCurrentFolderFlags(FOLDERFLAGS.FWF_NOCOLUMNHEADER, FOLDERFLAGS.FWF_NOCOLUMNHEADER)
        Marshal.ReleaseComObject(pFolderViewSearch)
    End Sub

    Private Sub Form2_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        'thisTimer.Dispose()
        If (thisTimer.Enabled) Then
            thisTimer.Stop()
        End If
        'added by sara for progress bar 
        If downloadPollTimer IsNot Nothing Then
            downloadPollTimer.Stop()
            downloadPollTimer.Dispose()
        End If

    End Sub

    Private Sub ButHome_ContextMenuChanged(sender As Object, e As EventArgs) Handles ButHome.ContextMenuChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp

    End Sub



End Class