Imports System.Collections.Generic
Imports System.Text
Public Interface IeZReminder
    Inherits IDatabaseItems
    Property ReminderId() As Integer
    Property StartTime() As String
    Property EndTime() As String
    Property Subject() As String
    Property Reminder() As String
    Property DefaultId() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZReminderExist() As Boolean

End Interface



