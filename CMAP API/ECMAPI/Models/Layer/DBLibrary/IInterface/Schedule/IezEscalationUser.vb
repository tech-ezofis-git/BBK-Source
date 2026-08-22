Public Interface IezEscalationUser
    Inherits IDatabaseItems

    Property EscalationUserId() As Integer
    Property EscalationId() As Integer
    Property ECMLoginid() As Integer
    Property LoginName() As String
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer

End Interface
