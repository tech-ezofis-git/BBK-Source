Public Interface IErrorMessage
    Inherits IDatabaseItems
    Property CreatedOn() As String
    Property ErrorFrom() As String
    Property Message() As String
    Property Description() As String
    Property SysName() As String
End Interface
