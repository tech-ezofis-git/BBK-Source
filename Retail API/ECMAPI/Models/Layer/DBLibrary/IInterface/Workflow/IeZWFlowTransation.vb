Imports System.Collections.Generic
Imports System.Text
Imports ECMAPI.ParaVariables

Public Interface IeZWFlowTransation
    Inherits IDatabaseItems
    Property Transactionid() As Integer
    Property ProcessId() As Integer
    Property ActivityId() As String
    Property RuleId() As String
    Property DynamicProperty() As Dictionary(Of String, String)
    Property SplUsers() As List(Of Userslist)
    Property RequestNo() As String
    Property RaisedOn() As String
    Property RaisedBy() As String
    Property ActionBy() As List(Of Userslist)
    Property ActionGroupBy() As String
    Property Formid() As Integer
    Property FTemplateid() As Integer
    Property FormTableName() As String
    ' Property FormName() As String
    Property ItemTableName() As String

    Property LastActionStage() As String
    Property LastActedBy() As String
    Property LastActedOn() As String

    Property LastActionReview() As String
    Property DocCount() As String
    Property ActivityUserId() As Integer
    Property ActivityGroupId() As Integer
    Property Action() As String
    Property Review() As String
    Property TranPath() As String
    Property TransactionStatus() As Integer
    Property Templateid() As Integer
    Property Notification() As Boolean
    Property itemid() As Integer
    Property FileType() As String
    Property SkipTo() As String
    Property FromMail() As String
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer
    Property RequestType() As Boolean
    Property UserType() As String
    Property Attachment() As Integer
    Property DaysOpen() As String
    Property Month() As String
    Property Escalated() As Integer

End Interface
