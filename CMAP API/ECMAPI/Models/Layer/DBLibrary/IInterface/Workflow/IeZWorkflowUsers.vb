Public Interface IeZWorkflowUsers
    Inherits IDatabaseItems
    Property WorkflowUsersId() As Integer
    Property WorkflowId() As Integer
    Property ECMLoginId() As Integer
    Property ECMGroupId() As Integer
    Property AssignedFrom() As String
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property Createdby1() As String
    Property Updatedby1() As String
    Property UserType() As String
    Property FormId() As Integer
    ReadOnly Property isdeleted() As Integer
End Interface
