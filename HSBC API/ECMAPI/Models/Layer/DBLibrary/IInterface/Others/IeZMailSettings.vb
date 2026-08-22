Public Interface IeZMailSettings
    Inherits IDatabaseItems
    Property SettingId() As Integer
    Property SettingName() As String
    Property EmailId() As String
    Property UserName() As String
    Property Password() As String
    Property EnableSSL() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    Property Preference() As Integer
    Property OutgoingServer() As String
    Property OutgoingPort() As Integer
    Property IncomingServer() As String
    Property IncomingPort() As Integer
    Property LogoPath() As String
    Property Signature() As String
End Interface
