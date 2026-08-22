Imports System.Collections.Generic
Imports System.Text
Public Interface IeZECMUserInfo
    Inherits IDatabaseItems
    Property UserId() As Integer
    Property ECMLoginId() As String
    Property Mobile() As String
    Property EmailAddress() As String
    Property FirstName() As String
    Property Department() As String
    Property Manager() As Integer
    Property ManagerName() As String
    Property Designation() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsEmployeeExist() As Boolean
End Interface
