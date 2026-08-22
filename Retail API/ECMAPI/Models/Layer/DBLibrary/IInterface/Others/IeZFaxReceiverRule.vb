Imports System.Collections.Generic
Imports System.Text
Public Interface IeZFaxReceiverRule
    Inherits IDatabaseItems
    Property FaxReceiverRuleId() As Integer
    Property FaxReceiverRule() As String

    Property DisplayFrom() As Integer
    Property Hours() As String
    Property DisplayText() As String
    Property ValidityOfFax As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsFaxReceiverRule() As Boolean
End Interface
