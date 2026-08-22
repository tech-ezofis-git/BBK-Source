Public Interface IeZMapLocation
    Inherits IDatabaseItems


    Property LocationId() As Integer
    Property LocationName() As String
    Property Latitude() As String
    Property Longitude() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
