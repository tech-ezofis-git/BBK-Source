Public Interface IezMailTriggerTypes
    Inherits IDatabaseItems

    Property TriggerTypeId() As Integer
    Property TriggerType() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
