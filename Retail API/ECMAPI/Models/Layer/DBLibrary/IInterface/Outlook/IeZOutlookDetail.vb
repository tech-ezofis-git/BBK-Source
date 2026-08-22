Public Interface IeZOutlookDetail
    Inherits IDatabaseItems

    Property Outlookdetailid() As Integer
    Property ConversationIndex() As String
    Property EntryId() As String
    Property itemid() As Integer
    Property templateid() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
