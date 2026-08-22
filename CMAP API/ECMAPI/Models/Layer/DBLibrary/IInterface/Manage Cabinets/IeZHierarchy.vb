Public Interface IeZHierarchy
    Inherits IDatabaseItems

    Property TemplateId() As Integer
    Property FromLevelId() As Integer
    Property ToLevelId() As Integer
    Property Hierarchy_id() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
