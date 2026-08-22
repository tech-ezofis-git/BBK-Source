Imports System.Collections.Generic
Imports System.Text
Public Interface IeZCabOwners
    Inherits IDatabaseItems
    Property UserId() As Integer
    Property EmployeeName() As String
    Property EmpId() As String
    Property CabinetID() As Integer
    Property CabinetName() As String
    Property CabOwnerID() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsCabOwnersExist() As Boolean
End Interface
