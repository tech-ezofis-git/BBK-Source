Imports System.Collections.Generic
Imports System.Text

Public Interface IeZLookupType
    Inherits IDatabaseItems
    Property LookupTypeId() As Integer
    Property LookupType() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsLookupTypeExist() As Boolean
End Interface
