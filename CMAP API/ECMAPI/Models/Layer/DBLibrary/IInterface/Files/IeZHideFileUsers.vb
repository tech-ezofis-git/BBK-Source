Public Interface IeZHideFileUsers
    Inherits IDatabaseItems
    Property HideFileUsersId As Integer
    Property HideFileId() As Integer
    Property Sno() As Integer
    Property UserId() As Integer
    Property Show() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
