Imports System.Collections.Generic
Imports System.Text

Public Interface IeZECMUserType
    Inherits IDatabaseItems
    Property ECMUserTypeId() As Integer
    Property ECMUserType() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsECMUserTypeExist() As Boolean
End Interface
