Imports System.Collections.Generic
Imports System.Text

Public Interface IeZIntegrationDetail
    Inherits IDatabaseItems
    Property IntegrationId() As Integer
    Property IntegrationName() As String
    Property IGServerType() As String
    Property IGDataSource() As String
    Property IGUserId() As String
    Property IGPassword() As String
    Property IGeZURL() As String
    Property IGStatus() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
