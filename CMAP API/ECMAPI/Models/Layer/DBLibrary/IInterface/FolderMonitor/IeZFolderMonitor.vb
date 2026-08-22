Public Interface IeZFolderMonitor
    Inherits IDatabaseItems
    Property Monitorid() As Integer
    Property TemplateId() As Integer
    Property MonitorPath() As String
    Property Monitortype() As String
    Property MonitorTypeId() As Integer
    Property FileType() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    Property IsActive() As Boolean
    Property Schedule() As Boolean
    ReadOnly Property Isdeleted() As Integer
End Interface
