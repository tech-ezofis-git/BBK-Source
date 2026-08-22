Public Interface IezFoldersByUser
    Inherits IDatabaseItems

    Property NodeId() As Integer
    Property NodeName() As String
    Property ParentNodeId() As Integer
    Property TemplateId() As Integer
    Property LevelId() As Integer
    Property PathId() As Integer
    Property UserId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
