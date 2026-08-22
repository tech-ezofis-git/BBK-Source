Imports System.Collections.Generic
Imports System.Text
Public Interface IeZCabinet
    Inherits IDatabaseItems
    Property CabinetID() As Integer
    Property CabinetName() As String
    Property Description() As String
    Property CabSize() As Integer
    Property CabCurrentSize() As String
    Property DocumentCount() As Integer
    Property CabExpiryDate() As DateTime
    Property UserId() As Integer
    Property CabOwnerID() As Integer
    Property CabOwnerName() As String
    Property ProfileId() As Integer
    Property Profile() As String
    Property ERSId() As Integer
    Property ERSName() As String
    Property ERSServerName() As String
    Property ERSDirPath() As String
    Property ERSIndexinpath() As String
    Property CabIcon() As Byte()
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsCabinetExist() As Boolean
End Interface

