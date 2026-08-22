Public Interface IezRetentionMail
    Inherits IDatabaseItems

    Property RetMailId() As Integer
    Property RetentionId() As Integer
    Property ItemId() As Integer
    Property TemplateId() As Integer
    Property MailTo() As String
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer

End Interface
