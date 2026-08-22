Public Interface IeZWFlowFormDetails
    Inherits IDatabaseItems

    Property FormDetailsId() As Integer
    Property formid() As Integer
    Property parentformid() As Integer
    Property workflowid() As Integer
    Property tablename() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
