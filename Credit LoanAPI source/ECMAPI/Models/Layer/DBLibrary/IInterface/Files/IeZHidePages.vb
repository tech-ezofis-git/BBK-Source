Public Interface IeZHidePages
    Inherits IDatabaseItems
    Property HideId() As Integer
    Property ItemId() As Integer
    Property TemplateId() As Integer
    Property Pages() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
