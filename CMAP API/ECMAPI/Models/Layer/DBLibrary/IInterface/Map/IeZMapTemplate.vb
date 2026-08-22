Public Interface IeZMapTemplate
    Inherits IDatabaseItems


    Property LocationId() As Integer
    Property MapTemplateId() As Integer
    Property CabinetId() As Integer
    Property TemplateId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
