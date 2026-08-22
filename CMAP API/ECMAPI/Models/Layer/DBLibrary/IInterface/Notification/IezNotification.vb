Public Interface IezNotification
    Inherits IDatabaseItems

    Property NotificationId() As Integer
    Property ecmloginid() As Integer
    Property refid() As Integer
    Property notificationfrom() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
