Public Interface IeZDtSearchPath
    Inherits IDatabaseItems

    Property indexpathid() As Integer
    Property ERSId() As Integer
    Property TemplateId() As Integer
    Property IFilePath() As String
    Property Status() As Boolean
    Property ifiletype() As String
    Property itemid() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
