Public Interface IeZMailArchiveValue
    Inherits IDatabaseItems


    Property MailArchiveValueId() As Integer
    Property MailArchiveId() As Integer
    Property MailArchiveValue() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
