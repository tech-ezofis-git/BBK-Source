Imports System.Collections.Generic
Imports System.Text
Public Interface IeZFaxReceiver
    Inherits IDatabaseItems
    Property FaxReceiverId() As Integer
    Property IsPrimaryUser() As Boolean
    Property FaxReceiverRule() As String
    Property DisplayFrom() As Integer
    Property DisplayText() As String
    Property ECMLoginId() As Integer
    Property FaxReceiverRuleId() As Integer
    Property RuleName() As String
    Property SenderType() As String
    Property PrimaryUser() As String
    Property Hours() As String
    Property SecondaryUser() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsFaxReceiver() As Boolean
End Interface
