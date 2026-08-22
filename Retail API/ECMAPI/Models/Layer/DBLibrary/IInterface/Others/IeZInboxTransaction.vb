Imports System.Collections.Generic
Imports System.Text

Public Interface IeZInboxTransaction
    Inherits IDatabaseItems
    Property InboxId() As Integer
    Property ItemId() As Integer
    Property TemplateId() As Integer
    Property ProcessId() As Integer
    Property FromUserId() As Integer
    Property ToUserId() As Integer
    Property FromUser() As String
    Property ToUser() As String
    Property URL() As String
    Property Status() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsInboxIdExist() As Boolean
End Interface
