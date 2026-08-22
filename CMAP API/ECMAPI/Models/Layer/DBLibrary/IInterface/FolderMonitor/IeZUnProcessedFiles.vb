Public Interface IeZUnProcessedFiles
    Inherits IDatabaseItems

    Property UnprocessId() As Integer
    Property FilePath() As String
    Property FileName() As String
    Property FileExtension() As String
    Property Status() As Integer
    Property Issue() As String
    Property ProcessedFrom() As String
    Property ReprocessPath() As String
    Property TemplateId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
