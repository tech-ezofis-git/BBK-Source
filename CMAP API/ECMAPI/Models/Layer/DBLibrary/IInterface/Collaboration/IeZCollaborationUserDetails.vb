Public Interface IeZCollaborationUserDetails
    Inherits IDatabaseItems

    Property ID() As Integer
    Property CollId() As Integer
    Property UserId() As Integer
    Property Status() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
