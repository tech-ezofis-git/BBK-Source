Imports System.Collections.Generic
Imports System.Text

Public Interface IeZLicenseClients
    Inherits IDatabaseItems
    Property LicenseClientId() As Integer
    Property LicenseId() As Integer
    Property ApplicationId() As Integer
    Property ApplicationName() As String
    Property Status() As String
    Property ClientName() As String
    Property LicenseKey() As String
    Property MachineCode() As String
    Property MacInfo() As String
    Property InstallOn() As String
    Property TrialDays() As Integer
    Property ExpiredOn() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    Property IsActive() As Integer
End Interface
