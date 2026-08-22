Public Interface IeZClient
    Inherits IDatabaseItems

    Property ClientId() As Integer
    Property ClientName() As String
    Property Address() As String
    Property City() As String
    Property Country() As String
    Property ContactPerson() As String
    Property ContactNo() As String
    Property EmailId() As String
    Property ReferenceFrom() As String
    Property InstalledDate() As String
    Property LastAMC() As String
    Property AMCDate() As String
    Property Logo() As String
    Property LicenseType() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
