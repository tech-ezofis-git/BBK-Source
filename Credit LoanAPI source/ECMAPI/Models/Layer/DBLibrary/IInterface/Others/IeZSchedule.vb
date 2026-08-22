Imports System.Collections.Generic
Imports System.Text
Public Interface IeZSchedule
    Inherits IDatabaseItems
    Property ScheduleId() As Integer
    Property ForSchedule() As Integer
    Property Id() As Integer
    Property ScheduleTypeId() As Integer
    Property WeekDay() As Integer
    Property Mont() As Integer
    Property EachDay() As Integer
    Property Day() As Integer
    Property OnceDate() As DateTime
    Property Time() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZScheduleExist() As Boolean

End Interface



