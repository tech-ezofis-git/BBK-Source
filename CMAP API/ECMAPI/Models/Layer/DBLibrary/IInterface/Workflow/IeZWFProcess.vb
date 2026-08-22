Imports System.Collections.Generic
Imports System.Text
Imports ECMAPI.ParaVariables

Public Interface IeZWFProcess
    Inherits IDatabaseItems
    Property ProcessId() As Integer
    Property WorkflowId() As Integer
    Property FlowStatus() As String
    Property Workflowtypeid() As Integer
    Property Itemid() As Integer
    Property Templateid() As Integer

    Property DocCount() As String

    Property DaysOpen() As String
    Property Month() As String
    Property Escalated() As Integer
    Property ActionBy() As List(Of Userslist)

    Property Action() As String

    Property SplUsers() As List(Of Userslist)
    Property RaisedOn() As String
    Property RaisedBy() As String
    Property Formid() As String

    Property FormTableName() As String

    Property FTemplateid() As String
    Property ItemTableName() As String
    Property ifilepath() As String

    Property LastActedBy() As String
    Property LastActedOn() As String
    Property DynamicProperty() As Dictionary(Of String, String)

    Property LastActionStage() As String
    Property LastActionReview() As String

    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    Property RequestNo() As String
    ReadOnly Property isdeleted() As Integer

End Interface
