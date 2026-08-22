Imports System.Collections.Generic
Imports System.Text
Public Interface IeZMailArchive
    Inherits IDatabaseItems
    Property MailArchiveId() As Integer
    Property ScheduleId() As Integer
    Property MailArchiveTypeId() As Integer
    Property MailArchiveValueId() As Integer
    Property MailArchiveValue() As String
    Property MailArchiveType() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZMailArchiveExist() As Boolean
End Interface
