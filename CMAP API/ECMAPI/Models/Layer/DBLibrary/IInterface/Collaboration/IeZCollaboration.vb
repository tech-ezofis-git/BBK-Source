Public Interface IeZCollaboration
    Inherits IDatabaseItems

    Property CollId() As Integer
    Property CollName() As String
    Property itemid() As Integer
    Property Templateid() As Integer
    Property XMLPath() As String
    Property XMLHistorypath() As String
    Property StartDateTime() As String
    Property EndDateTime() As String
    Property Status() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
