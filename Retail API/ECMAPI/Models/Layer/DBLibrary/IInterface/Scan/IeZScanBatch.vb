Public Interface IeZScanBatch
    Inherits IDatabaseItems

    Property BatchId() As Integer
    Property Batch() As String
    Property Status() As Integer
    Property NoOfDocument() As Integer
    Property TemplateId() As Integer
    Property CreatedAt() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
