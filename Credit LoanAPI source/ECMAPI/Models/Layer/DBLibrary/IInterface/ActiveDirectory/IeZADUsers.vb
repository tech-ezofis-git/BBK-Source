Public Interface IeZADUsers
    Inherits IDatabaseItems
    Property LdapUserId() As Integer
    Property LdapConnId() As Integer
    Property Username() As String
    Property Firstname() As String
    Property Lastname() As String
    Property Displayname() As String
    Property Department() As String
    Property Mail() As String
    Property Mobile() As String
    Property Jobtitle() As String
    Property Description() As String
    Property State() As String
    Property City() As String
    Property Office() As String
    Property TelephoneNumber() As String
    Property Company() As String
    Property HomePhone() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    Property Manager() As String
    ReadOnly Property Isdeleted() As Integer
    Property IsECMUser() As Integer
    Property sAMAccountName() As String
End Interface
