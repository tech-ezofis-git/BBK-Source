Public Interface IeZProcessItems
    Inherits IDatabaseItems
    Property ProcessItemsId() As Integer
    Property ProcessId() As Integer
    Property ItemId() As Integer
    Property Workflowid() As Integer
    Property TemplateId() As Integer
    Property FormEntryId() As Integer
    Property FormId() As Integer
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property Createdby1() As String
    Property Updatedby1() As String
    ReadOnly Property isdeleted() As Integer
End Interface
