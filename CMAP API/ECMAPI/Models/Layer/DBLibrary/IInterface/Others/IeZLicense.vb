Imports System.Collections.Generic
Imports System.Text

Public Interface IeZLicense
    Inherits IDatabaseItems
    Property LicenseId() As Integer
    Property ApplicationId() As Integer
    Property ApplicationName() As String
    Property NoOfLicense() As Integer
    Property Key() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
