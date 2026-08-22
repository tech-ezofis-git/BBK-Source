Imports System.Runtime.Serialization
Imports System.ComponentModel
Public Class ParaVariables

    Public Shared csvexport As New CSVData
    Public Shared APIurl As String = ConfigurationManager.AppSettings("APIurl")
    Public Shared APICallId_Prefix As String = ConfigurationManager.AppSettings("APICallId_Prefix")
    Public Shared SaveAPICallInput As String = ConfigurationManager.AppSettings("SaveAPICallInput")
    Public Shared IUsername As String = ConfigurationManager.AppSettings("Username")
    Public Shared IPassword As String = ConfigurationManager.AppSettings("Password")
    Public Shared IDomain As String = ConfigurationManager.AppSettings("Domain")
    Public Shared IUNCpath As String = ConfigurationManager.AppSettings("UNCpath")
    Public Shared NewAPI As String = ConfigurationManager.AppSettings("CMAPAPI")


    Public Class UserLogin
        Public Property UserName As String
        Public Property Password As String

    End Class

    Public Class ByQuery
        Public Property StrQry As String

    End Class
    Public Class UniqueIdGen
        Public Property GenName As String

    End Class
    Public Class Templateids
        Public Property Templateid As Integer

    End Class

    Public Class Dates
        Public Property CurrentDate As DateTime
        Public Property WithTime As Boolean
    End Class
    Public Class ForAssetAttachments
        Public Property CabinetId As Integer
        Public Property TemplateId As Integer

        Public Property AssetId As Integer

        Public Property AssetNumber As String

    End Class

    Public Class ForAssetProcessHistory
        'Public Property CabinetId As Integer
        'Public Property TemplateId As Integer

        Public Property AssetId As Integer

        Public Property AssetNumber As String

    End Class

    Public Class ForTreeview
        'Public Property CabinetId As Integer
        'Public Property TemplateId As Integer

        Public Property Treelevel As Integer

        Public Property FieldName As String

        Public Property Value As String
    End Class

    Public Class ItemtableList
        Public Property TemplateId As Integer

        Public Property ReportFor As String

    End Class

    Public Class ForXMLCreation
        Public Property cabid As Integer
        Public Property cabname As String
        Public Property tmpid As Integer
        Public Property tmpname As String
        Public Property fields As String()
        Public Property fieldvalues As String()
        Public Property filename As String
        Public Property size As Integer
        Public Property xmlfilename As String
        Public Property loginid As String
        Public Property ipaddress As String
        Public Property ezfrom As String
        Public Property nopages As String
    End Class



    Public Class Byid
        Public Property id As Integer

    End Class

    Public Class ForWorkflowUserFields
        Public Property EcmLoginId As Integer

        Public Property Workflowid As Integer

    End Class
    Public Class Options
        Public Property ItemId As String
        Public Property Value As String
        Public Property ParentField As String

    End Class

    Public Class SearchRegistries
        Public Property Criteria As List(Of FilterCriteria)
        Public Property RowFrom As Integer
        Public Property RowCount As Integer

    End Class

    Public Class MailPara
        Public Property DSdata As DataSet
        Public Property Email As String

    End Class

    Public Class FilterCriteria
        Public Property Criteria As String

        Public Property DataTypeId As String
        Public Property Value1 As String
        Public Property Value2 As String

        Public Property RefTableName As String
    End Class

    Public Class ForSubLoc
        Public Property Country As String
        Public Property Location As String

    End Class

    Public Class ForAssetItem
        'Public Property Category As String
        'Public Property Asset_Type As String
        Public Property Asset_Item As String
        Public Property Asset_Item_Code As String


    End Class

    Public Class ForAssetItemByCategory
        Public Property Category As String
        Public Property Asset_Type As String
        Public Property Asset_Item As String
        Public Property Asset_Item_Code As String


    End Class

    Public Class ForAssetService
        Public Property AssetId As Integer
    End Class


    Public Class ForAssetetailByNumber
        Public Property AssetNumber As String
    End Class

    Public Class ForAssetetailByid
        Public Property AssetId As Integer
    End Class
    Public Class ForAsset_type
        Public Property Category As String
        Public Property Asset_Type As String

        Public Property Asset_Code As String

    End Class

    Public Class ForSupplier
        Public Property SupplierName As String
        Public Property SupplierCode As String



    End Class

    Public Class Userslist
        Public Property UserType As String
        Public Property Username As String

        Public Property UserRole As String

    End Class

    Public Class ECMLoginid
        Public Property ECMLoginid As String

        Public Property ECMGroupList As String = "0"

    End Class

    Public Class Criteria
        Public Property Criteria As String
        Public Property Value As String

    End Class

    Public Class ForERSPath
        Public Property CabinetID As String
        Public Property ERSDirPath As String

        Public Property SettingPath As String

    End Class

    Public Class ProcessInfo
        Public Property WorkflowId As String


        Public Property ECMLoginId As String
        Public Property ECMGroupList As String

        Public Property RowFrom As Integer = 0
        Public Property RowCount As Integer = 0


    End Class

    Public Class UpdatePassword
        Public Property ECMLoginId As String
        Public Property Password As String

    End Class
    Public Class Userinfo
        Public Property ECMLoginInfo As OldeZECMLogin
        Public Property ECMUserInfo As eZECMUserInfo

    End Class


    Public Class FlowInfo
        Public Property WorkflowId As String
        Public Property ECMLoginId As String

        Public Property ECMGroupList As String = "0"

    End Class

    Public Class FlowNodes
        Public Property NodeName As String
        Public Property ProcessCountInfo As String

    End Class
    Public Class query
        Public Property query As String
    End Class

    Public Class FlowGridLoadPara
        Public Property NodeName As String

        Public Property Rowfrom As String
        Public Property Rowto As String

        Public Property OrderBy As String

        Public Property WorkflowInfo As eZWorkflowDetails
        Public Property LoggedInfo As OldeZECMLogin



    End Class
    Public Class FormFieldsJson
        Public Property control As String
        Public Property controlid As Integer
        Public Property selected As Boolean
        Public Property text As String
        Public Property placeholder As String
        Public Property size As Integer
        Public Property textcolor As String
        Public Property width As Integer
        Public Property oldcolname As String
        Public Property deleted As Boolean
        Public Property dbcolname As String
        Public Property type As String
    End Class

    Public Class Forminfo
        Public Property Processid As String
        Public Property Transid As String

        Public Property Workflowid As String

    End Class
    Public Class InsAccNo
        Public Property acct_no As String
        Public Property url As String
    End Class
    Public Class InsRimNo
        Public Property rim_no As String
        Public Property url As String
    End Class

    Public Class acct_rim_info
        Public rim_no As String
        Public acct_title As String
        Public grade As String
        Public officername As String
        Public legalStatus As String
        Public docStatus As String
        Public marketvalue As String
        Public bankvalue As String
        Public expRevDate As String
        Public accounts As List(Of accountlist)
    End Class

    Public Class accountlist
        Public ac_type As String
        Public acct_type As String
        Public acct_no As String
        Public acct_type_desc As String
        Public sub_cmt_no As String
        Public rsm As String
        Public cur_bal As String
        Public iso_code As String
        Public limit_amt As String
        Public nbR_DR As String
        Public nbR_CR As String
        Public amT_DR As String
        Public amT_CR As String
        Public total_Float As String
        Public availBal As String
        Public availabilityDate As String
    End Class
    Public Class Condition
        'Public Property username As String
        'Public Property password As String

        Public Property cabinetName As String
        Public Property RIMNumber As String
    End Class
    Public Class Conditionforcommon
        Public Property RIMNumber As String
    End Class
    Public Class Data
        Public Property url As String
    End Class
    Public Class SessionDetailed
        Public Property sessionId As Integer

    End Class

    Public Class InsDelete
        Public Property Token As String
        Public Property CabinetName As String
        Public Property itemId As String
    End Class
    Public Class InsMasterEntry
        Public Property Token As String
        Public Property Fields As List(Of FieldWithValues)
    End Class
#Region "Upload"
    Public Class InsUpload
        Public Property Token As String
        Public Property CabinetName As String
        'Public Property fileBytes As Byte()
        'Public Property file As Byte() = Nothing
        Public Property file As String
        Public Property filetype As String
        Public Property Fields As List(Of FieldWithValues)
    End Class

    Public Class InsSearchandGetURL
        Public Property Token As String
        Public Property CabinetName As String
        Public Property Fields As List(Of FieldWithValues)
    End Class

    Public Class FieldWithValues
        Public FieldName As String
        Public FieldValue As String
    End Class

    Public Class InsGetToken
        Public Property LoginName As String
        Public Property Password As String

    End Class
    Public Class resmessage
        Public Property errorCode As Integer = 0
        Public Property value As String = ""

    End Class
    Public Class GetOptionsValue
        ' Public Property TemplateId As Integer
        Public Property Column As String()
    End Class
    Public Class ResFileReport
        ' Public Property Data1 As DataTable
        Public Property Data As List(Of ResFileReportA)
        Public Property RIMCount As Integer = 0
        Public Property FileCount As Integer = 0
        Public Property PageCount As Integer = 0
        Public Property TotalRows As Integer = 0
        Public Property NoofSucessfulAPI As Integer = 0
        Public Property NoofUnSucessfulAPI As Integer = 0
    End Class
    Public Class ResFileReportA
        Public Property CallHistoryId As Integer
        Public Property CabinetId As Integer
        Public Property TemplateId As Integer
        Public Property ItemId As Integer
        Public Property Status As String = ""
        Public Property RimNumber As String = ""
        Public Property TinNumber As String = ""
        Public Property InitiatedAT As String = ""
        Public Property CompletedAt As String = ""
        Public Property FileName As String = ""
        Public Property NoofPages As Integer
        Public Property CallDuration As Double
        Public Property Corporate As Boolean
        Public Property Retail As Boolean
    End Class
    Public Class ResSessionReport
        Public Property rowCount As Integer
        Public Property totalRow As Integer
        Public Property data As DataSet
    End Class
#End Region
#Region "TradefinanceReport"
    Public Class TATReport
        Public Property data As List(Of TATReportA)
        Public Property totalRow As Integer
    End Class
    Public Class TATReportA
        Public Property processId As Integer
        Public Property requestNo As String
        Public Property workflowId As Integer
        Public Property activityId As String
        Public Property scanDateandTime As String
        Public Property scannedBy As String
        Public Property transactionReference As String
        Public Property rim As String
        Public Property accountNo As String
        Public Property product As String
        Public Property phase As String
        Public Property rimNumber As String
        Public Property type As String
        Public Property stage As String
        Public Property claimedOn As String
        Public Property claimedBy As String
        Public Property submittedToApproval As String
        Public Property receivedBy As String
        Public Property completedBy As String
        Public Property totalTimeScanToComplete As String
        Public Property totalTimeClaimToApproval As String
        Public Property totalTimeReceivingTicketToApproval As String
        Public Property currentlyReceivingTime As String
    End Class
    Public Class AvailmentTicketReport
        Public Property data As List(Of AvailmentTicketReportA)
        Public Property totalRow As Integer
    End Class
    Public Class AvailmentTicketReportA
        Public Property processId As Integer
        Public Property requestNo As String
        Public Property workflowId As Integer
        Public Property activityId As String
        Public Property scanDateandTime As String
        Public Property transactionReference As String
        Public Property scannedBy As String
        Public Property rim As String
        Public Property accountNo As String
        Public Property product As String
        Public Property phase As String
        Public Property type As String
        Public Property claimedOn As String
        Public Property claimedBy As String
        Public Property submittedToApproval As String
        Public Property receivedBy As String
        Public Property submittedForRMApproval As String
        Public Property approvedAtReceived As String
        Public Property totalTimeFromSubmitingATUntilApproved As String
        Public Property comment As String
        Public Property currentlyReceivingTime As String
    End Class
    Public Class RejectionReport
        Public Property data As List(Of RejectionReportA)
        Public Property totalRow As Integer
    End Class
    Public Class RejectionReportA
        Public Property processId As Integer
        Public Property requestNo As String
        Public Property workflowId As Integer
        Public Property activityId As String
        Public Property scanDateandTime As String
        Public Property transactionReference As String
        Public Property scannedBy As String
        Public Property rim As String
        Public Property accountNo As String
        Public Property product As String
        Public Property phase As String
        Public Property type As String
        Public Property claimedOn As String
        Public Property claimedBy As String
        Public Property submittedToApproval As String
        Public Property receivedBy As String
        Public Property rejectForCorrection As String
        Public Property submittedForApproval As String
        Public Property completedBy As String
        Public Property totalNumberOfRejections As String
        Public Property comment As String
        Public Property currentlyReceivingTime As String
    End Class
    Public Class ProcessingTimeReport
        Public Property data As List(Of ProcessingTimeReportA)
        Public Property totalRow As Integer
    End Class
    Public Class ProcessingTimeReportA
        Public Property processId As Integer
        Public Property requestNo As String
        Public Property workflowId As Integer
        Public Property activityId As String
        Public Property transactionReference As String
        Public Property product As String
        Public Property phase As String
        Public Property type As String
        Public Property claimedOn As String
        Public Property claimedBy As String
        Public Property submittedToApproval As String
        Public Property receivedBy As String
        Public Property totalNumberOfRejections As String
        Public Property totalProcessingTime As String
        Public Property totalTransactionProcessed As String
        Public Property transactionRatio As String
        Public Property currentlyReceivingTime As String
    End Class
#End Region
End Class
