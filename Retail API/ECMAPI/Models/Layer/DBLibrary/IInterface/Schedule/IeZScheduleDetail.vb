Public Interface IeZScheduleDetail
    Inherits IDatabaseItems

    Property Detailid() As Integer
    Property Id() As Integer
    Property ForSchedule() As Integer
    Property ScheduleId() As Integer
    Property ScheduleDate() As String
    Property Status() As Boolean
    Property Result() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
