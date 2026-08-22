Imports System.Collections.Generic
Imports System.Text

Public Interface IeZFaxTransaction
    Inherits IDatabaseItems
    Property FaxTransactionId() As Integer
    Property FaxReceiverRuleId() As Integer
    Property IsExpired() As Boolean
    Property ArchivedItemid() As Integer
    Property ArchivedTemplateId() As Integer
    Property Itemid() As Integer
    Property FromAdd() As Integer
    Property FAXNUMBER() As String
    Property Subject() As String
    Property DocType() As String
    Property FilePath() As String
    Property ToAdd() As Integer
    Property DisplayFrom() As Integer
    Property FromName() As String
    Property IsRead() As Boolean
    Property IsArchived() As Boolean
    Property ArchivedBy() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsFaxTransactionExist() As Boolean
End Interface
