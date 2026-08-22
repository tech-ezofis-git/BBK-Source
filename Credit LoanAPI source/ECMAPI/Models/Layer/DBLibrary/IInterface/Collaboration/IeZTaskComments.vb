Public Interface IeZTaskComments
    Inherits IDatabaseItems

    Property CommentsId() As Integer
    Property Taskid() As Integer
    Property Comments() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
