Imports System.Collections.Generic
Imports System.Text
Public Interface IeZAllottedTask
    Inherits IDatabaseItems
    Property AllottedTaskId() As Integer
    Property ECMLoginId() As Integer
    Property LoginName() As String
    Property TaskId() As Integer
    Property Task() As String
    Property status() As String
    Property Notification() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZAllottedTaskExist() As Boolean
End Interface
