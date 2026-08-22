Public Interface IeZFieldAlertDoc
    Inherits IDatabaseItems

    Property FieldAlertDocId() As Integer
    Property FieldAlertDetailId() As Integer
    Property Filename() As String
    Property ToMail() As String
    Property TemplateId() As Integer
    Property itemid() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
