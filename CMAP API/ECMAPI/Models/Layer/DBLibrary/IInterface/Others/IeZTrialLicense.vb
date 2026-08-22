Imports System.Collections.Generic
Imports System.Text

Public Interface IeZTrialLicense
    Inherits IDatabaseItems
    Property TrialId() As Integer
    Property LicenseClientId() As Integer
    Property LicenseId() As Integer
    Property TrialKey() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
