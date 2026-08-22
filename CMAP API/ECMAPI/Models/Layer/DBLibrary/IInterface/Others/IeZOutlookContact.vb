Imports System.Collections.Generic
Imports System.Text
Public Interface IeZOutlookContact
    Inherits IDatabaseItems
    Property OutlookContactId() As Integer
    Property Name() As String
    Property EntryId() As String
    Property CompanyName() As String
    Property Email() As String
    Property MobileNumber() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZOutlookContactExist() As Boolean
End Interface
