Public Interface IeZVault
    Inherits IDatabaseItems
    Property eZVaultId() As Integer
    Property TemplateId() As Integer
    Property Condition() As String
    Property NodeId() As Integer
    Property Status() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
