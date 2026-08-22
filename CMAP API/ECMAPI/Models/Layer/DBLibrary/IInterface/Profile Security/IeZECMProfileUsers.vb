Public Interface IeZECMProfileUsers
    Inherits IDatabaseItems
    Property ECMProfileUsersId() As Integer
    Property ECMProfileId() As Integer
    Property ECMLoginId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
