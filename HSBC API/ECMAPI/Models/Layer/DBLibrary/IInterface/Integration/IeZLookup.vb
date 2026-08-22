Imports System.Collections.Generic
Imports System.Text
Public Interface IeZLookup
    Inherits IDatabaseItems
    Property LookupId() As Integer
    Property LookupTypeId() As Integer
    Property LookupServerTypeId() As Integer
    Property LookupConnStrId() As Integer
    Property LookupType() As String
    Property ConnectionString() As String
    Property schedule() As Integer
    Property Scheduletime() As String
    Property LookupValue() As String
    Property lookupname() As String
    Property Lookupconnectionname() As String
    Property TemplateId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZLookup() As Boolean
End Interface
