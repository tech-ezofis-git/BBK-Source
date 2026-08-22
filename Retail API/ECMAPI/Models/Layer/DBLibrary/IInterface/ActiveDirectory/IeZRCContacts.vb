Public Interface IeZRCContacts
    Inherits IDatabaseItems

    Property ezContactId() As Integer

    Property CompanyName() As String
    Property ContactName() As String
    Property LastName() As String
    Property Title() As String

    Property Phone() As String
    Property Mobile() As String
    Property AltNumber() As String
    Property Fax() As String
    Property Email() As String
    Property WebPage() As String

    Property Address() As String
    Property City() As String
    Property Country() As String

    Property SecondPhone() As String
    Property SecondMobile() As String
    Property SecondAltNumber() As String
    Property SecondFax() As String
    Property SecondCity() As String

    Property Categories() As String

    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

    Property POBox() As String
End Interface
