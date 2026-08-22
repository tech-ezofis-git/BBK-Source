Public Interface IeZFieldAlertTemp
    Inherits IDatabaseItems


    Property Id() As Integer
    Property ToAdd() As String
    Property BodyMessage() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
