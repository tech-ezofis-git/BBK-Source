
Imports System.Collections.Generic
Imports System.Text

Public Interface IeZOutlooksync
    Inherits IDatabaseItems

    Property Outlooksyncid() As Integer
    Property Scheduleid() As Integer
    Property Syncname() As String
    Property Syncrule() As String
    Property SyncMail() As String
    Property ScheduleTypeId() As Integer
    Property ForSchedule() As Integer
    Property Id() As Integer
    Property WeekDay() As Integer
    Property Mont() As Integer
    Property Day() As Integer
    Property EachDay() As Integer
    Property OnceDate() As String
    Property Time() As String
    Property Createdon() As String
    Property updatedon() As String
    Property Createdby() As Integer
    Property updatedby() As Integer
    ReadOnly Property isdeleted() As Integer

End Interface
