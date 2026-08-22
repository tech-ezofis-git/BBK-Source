Public Interface IeZHideFile
    Inherits IDatabaseItems
    Property HideFileId() As Integer
    Property ItemId() As Integer
    Property TemplateId() As Integer
    Property HideAlways() As Integer
    Property FromDate() As String
    Property ToDate() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
