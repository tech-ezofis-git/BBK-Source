Public Interface IeZScanSettings
    Inherits IDatabaseItems


    Property SettingId() As Integer
    Property Dublex() As Boolean
    Property Colour() As Boolean
    Property Dpi() As Integer
    Property LoginId() As Integer
    Property FileNameType() As Integer
    Property FileName() As String
    Property DupType() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
