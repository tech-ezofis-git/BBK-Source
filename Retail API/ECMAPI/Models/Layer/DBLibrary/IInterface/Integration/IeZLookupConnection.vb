Imports System.Collections.Generic
Imports System.Text
Public Interface IeZLookupConnection
    Inherits IDatabaseItems
    Property LookupConnStrId() As Integer
    Property LookupConnName() As String
    Property LookupServerTypeId() As Integer
    Property LookupServerType() As String
    Property ConnectionString() As String
    Property DataSource() As String
    Property UserId() As String
    Property Pasword() As String
    Property connectedlookup() As String
    Property provider() As String
    Property Databasename() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    Property conn() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZLookupConnectionExist() As Boolean
End Interface

