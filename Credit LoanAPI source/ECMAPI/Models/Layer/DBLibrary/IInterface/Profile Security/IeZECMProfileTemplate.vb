Public Interface IeZECMProfileTemplate
    Inherits IDatabaseItems
    Property ProfileTemplateId() As Integer
    Property EcmProfileId() As Integer
    Property TemplateId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
