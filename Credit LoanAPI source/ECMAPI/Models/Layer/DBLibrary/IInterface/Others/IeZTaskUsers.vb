Imports System.Collections.Generic
Imports System.Text
Public Interface IeZTaskUsers
    Inherits IDatabaseItems
    Property TaskUsersId() As Integer
    Property ECMLoginId() As Integer
    Property LoginName() As String
    Property OwnerId() As Integer
    Property OwnerName() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZTaskUsersExist() As Boolean
End Interface
