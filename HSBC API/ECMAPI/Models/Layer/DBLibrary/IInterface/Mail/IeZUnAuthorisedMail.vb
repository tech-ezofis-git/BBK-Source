Public Interface IeZUnAllocatedMail
    Inherits IDatabaseItems

    Property MailRequestId() As Integer
    Property MailSubject() As String
    Property MailBody() As String
    Property MailFrom() As String
    Property MailSettingsId() As Integer
    Property WorkflowId() As Integer
    Property JunkMail() As Boolean
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer
    Property Workflow() As String
End Interface
