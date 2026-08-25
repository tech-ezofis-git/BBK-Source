Public Class publicvariables


    Public Shared CanContinue As Integer = 0
    Public Shared WorkItmref As String = ""
    Public Shared ScannedPageCount As Integer
    Public Shared Pagescou As Integer = 0
    Public Shared NewWorkItem As Integer = 0
    Public Shared ScannedDocMsg As Integer = 1
    Public Shared SubmittedworkItemFlag As Integer = 0
    Public Shared FinalSubmissionPath = ""
    Public Shared ecmlogin As CACServiceReference.eZECMLogin


    Public Class AccountInfo
        Public Property acct_no As String
        Public Property url As String

    End Class
    Public Class ByQuery
        Public Property StrQry As String
    End Class
    Public Class InsertFolder
        Public Property Nodes As String()
        Public Property Loginid As String
        Public Property TenantId As Integer
    End Class

    Public Class UserSession
        Public Property SessionId As Integer
        Public Property Id As Integer
        Public Property ActionId As Integer
        Public Property Cabname As String
        Public Property Action As String
        Public Property Itemid As Integer
        Public Property LinkId As Integer
        Public Property CabinetId As Integer
        Public Property Tenantid As Integer
        Public Property SearchBy As String
        Public Property CommentsId As Integer
        Public Property PanelId As Integer
        Public Property loggedat As String
        Public Property loggedFrom As String
        Public Property CreatedBy As Integer
        Public Property CreatedOn As String
        Public Property CreatedBy1 As String
        Public Property UpdatedBy As Integer
        Public Property UpdatedOn As String
        Public Property UpdatedBy1 As String
        Public Property Isdeleted As Boolean
        Public Property IsReadFromDB As Boolean
        Public Property IsModified As Boolean
        Public Property SNo As Integer
    End Class


End Class
