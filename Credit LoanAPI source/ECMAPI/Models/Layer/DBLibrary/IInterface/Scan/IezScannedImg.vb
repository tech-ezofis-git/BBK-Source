Public Interface IezScannedImg
    Inherits IDatabaseItems


    Property ScannedImgId() As Integer
    Property Ifilepath() As String
    Property pcname() As String
    Property appname() As String
    Property TemplateId() As Integer
    Property Status() As Integer
    Property nopages() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
