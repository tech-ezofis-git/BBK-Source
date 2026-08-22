Imports System.Collections.Generic
Imports System.Text

Public Interface IeZLookupServerType
    Inherits IDatabaseItems
    Property LookupServerTypeId() As Integer
    Property LookupServerType() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsLookupServerTypeExist() As Boolean
End Interface
