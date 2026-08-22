Public Interface IeZMailTriggering
    Inherits IDatabaseItems

    Property MailTriggerid() As Integer
    Property Status() As Boolean
    Property TriggerTypeId() As Integer
    Property MailSettingId() As Integer
    Property Condition() As String
    Property TempWFId() As Integer
    Property UnallocatedMailUser() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
