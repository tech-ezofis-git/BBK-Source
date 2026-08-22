Public Interface IeZLdapConnection
    Inherits IDatabaseItems
    Property LdapConnId() As Integer
    Property LdapServer() As String
    Property LdapDomain() As String
    Property Username() As String
    Property Pasword() As String
    Property LdapPath() As String
    Property Preferred() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
