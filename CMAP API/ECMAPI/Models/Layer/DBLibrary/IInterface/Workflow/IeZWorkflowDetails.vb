Imports System.Collections.Generic
Imports System.Text

Public Interface IeZWorkflowDetails
    Inherits IDatabaseItems
    Property Workflowid() As Integer
    Property Workflowitemid() As Integer

    ReadOnly Property ItemTableName() As String

    ReadOnly Property FormTableName() As String

    ReadOnly Property TemplateId() As String





    Property XMLDS() As String
    ReadOnly Property FormId() As String

    ReadOnly Property ProcessInfoColumns() As String

    Property Status() As String

    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer
    Property MailSettingsId() As Integer
    Property WorkflowName() As String
End Interface
