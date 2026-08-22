Imports System.Collections.Generic
Imports System.Text
Public Interface IeZTask
    Inherits IDatabaseItems
    Property TaskId() As Integer
    Property Task() As String
    Property TaskStatus() As Integer
    Property StartTime() As String
    Property EndTime() As String
    Property Templateid() As Integer
    Property itemid() As Integer
    Property TaskPriority() As Integer
    Property Typeid() As Integer
    Property Notification() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZTaskExist() As Boolean
End Interface
