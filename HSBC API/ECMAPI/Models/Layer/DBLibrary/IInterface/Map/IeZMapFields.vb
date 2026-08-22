Public Interface IeZMapFields
    Inherits IDatabaseItems

    Property Mapfieldsid() As Integer
    Property Cabinetid() As Integer
    Property Templateid() As Integer
    Property LocationField() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
