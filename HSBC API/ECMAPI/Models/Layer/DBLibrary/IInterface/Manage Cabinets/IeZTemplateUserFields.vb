Public Interface IeZTemplateUserFields
    Inherits IDatabaseItems

    Property UserFieldId() As Integer
    Property ECMLoginId() As Integer
    Property FieldId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
