Imports System.Collections.Generic
Imports System.Text

Public Interface IeZScheduleType
    Inherits IDatabaseItems
    Property ScheduleTypeId() As Integer
    Property ScheduleType() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsScheduleTypeExist() As Boolean
End Interface
