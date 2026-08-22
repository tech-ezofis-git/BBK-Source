Imports System.Collections.Generic
Imports System.Text

Public Interface IeZFax
    Inherits IDatabaseItems
    Property FaxId() As Integer
    Property FaxReceiverRuleId() As Integer
    Property FaxName() As String
    Property FaxNumber() As String
    Property FaxType() As Integer
    Property FaxTypeValue() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsFaxExist() As Boolean
End Interface
