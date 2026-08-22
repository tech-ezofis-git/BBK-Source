Public Interface IezEscalation
    Inherits IDatabaseItems

    Property EscalationId() As Integer
    Property WorkflowId() As Integer
    Property ActivityId() As String
    Property ActivityName() As String
    Property ResponseTime() As String
    Property ResponseType() As String
    Property User() As List(Of ezEscalationUser)
    Property Notification() As Boolean
    Property ActionFlow() As Boolean
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer

End Interface
