Public Interface IezImpersonation
    Inherits IDatabaseItems

    Property ImpersonateId() As Integer
    Property ImpersonationFor() As String
    Property Domain() As String
    Property Username() As String
    Property Password() As String
    Property ERSid() As Integer
    Property TemplateId() As Integer
    Property Description() As String
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer

End Interface
