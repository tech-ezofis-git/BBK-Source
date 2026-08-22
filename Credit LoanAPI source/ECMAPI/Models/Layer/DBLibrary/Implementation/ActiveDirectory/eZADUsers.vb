Imports ECMAPI

Public Class eZADUsers
    Inherits IDatabaseCommonItems
    Implements IeZADUsers

    Protected _LdapUserId As Integer
    Protected _LdapConnId As Integer
    Protected _Username As String = ""
    Protected _Firstname As String = ""
    Protected _Lastname As String = ""
    Protected _Displayname As String = ""
    Protected _Department As String = ""
    Protected _Mail As String = ""
    Protected _Mobile As String = ""
    Protected _Jobtitle As String = ""
    Protected _Description As String = ""
    Protected _State As String = ""
    Protected _City As String = ""
    Protected _Office As String = ""
    Protected _TelephoneNumber As String = ""
    Protected _Company As String = ""
    Protected _HomePhone As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Protected _Manager As String = ""
    Private _Isdeleted As Integer
    Protected _IsECMUser As Integer
    Protected _sAMAccountName As String = ""

    Public Sub New()
    End Sub

    Public Sub New(LdapUserId As Integer)
        Me._LdapUserId = LdapUserId
    End Sub

    Public Property City As String Implements IeZADUsers.City
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _City
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _City = value Then
                Return
            End If
            _City = value
            IsModified = True
        End Set
    End Property

    Public Property Company As String Implements IeZADUsers.Company
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Company
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Company = value Then
                Return
            End If
            _Company = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZADUsers.CreatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy = value Then
                Return
            End If

            _CreatedBy = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZADUsers.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy1 = value Then
                Return
            End If
            _CreatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedOn As String Implements IeZADUsers.CreatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedOn = value Then
                Return
            End If

            _CreatedOn = value
            IsModified = True
        End Set
    End Property

    Public Property Department As String Implements IeZADUsers.Department
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Department
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Department = value Then
                Return
            End If
            _Department = value
            IsModified = True
        End Set
    End Property

    Public Property Description As String Implements IeZADUsers.Description
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Description
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Description = value Then
                Return
            End If
            _Description = value
            IsModified = True
        End Set
    End Property

    Public Property Displayname As String Implements IeZADUsers.Displayname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Displayname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Displayname = value Then
                Return
            End If
            _Displayname = value
            IsModified = True
        End Set
    End Property

    Public Property Firstname As String Implements IeZADUsers.Firstname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Firstname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Firstname = value Then
                Return
            End If
            _Firstname = value
            IsModified = True
        End Set
    End Property

    Public Property HomePhone As String Implements IeZADUsers.HomePhone
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _HomePhone
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _HomePhone = value Then
                Return
            End If
            _HomePhone = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZADUsers.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property Jobtitle As String Implements IeZADUsers.Jobtitle
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Jobtitle
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Jobtitle = value Then
                Return
            End If
            _Jobtitle = value
            IsModified = True
        End Set
    End Property

    Public Property Lastname As String Implements IeZADUsers.Lastname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Lastname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Lastname = value Then
                Return
            End If
            _Lastname = value
            IsModified = True
        End Set
    End Property

    Public Property LdapConnId As Integer Implements IeZADUsers.LdapConnId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LdapConnId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LdapConnId = value Then
                Return
            End If
            _LdapConnId = value
            IsModified = True
        End Set
    End Property

    Public Property LdapUserId As Integer Implements IeZADUsers.LdapUserId
        Get
            If _LdapUserId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LdapUserId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LdapUserId <> 0 AndAlso _LdapUserId <> value Then
                Throw New MemberAccessException()
            End If
            _LdapUserId = value
        End Set
    End Property

    Public Property Mail As String Implements IeZADUsers.Mail
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Mail
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Mail = value Then
                Return
            End If
            _Mail = value
            IsModified = True
        End Set
    End Property

    Public Property Mobile As String Implements IeZADUsers.Mobile
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Mobile
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Mobile = value Then
                Return
            End If
            _Mobile = value
            IsModified = True
        End Set
    End Property

    Public Property Office As String Implements IeZADUsers.Office
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Office
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Office = value Then
                Return
            End If
            _Office = value
            IsModified = True
        End Set
    End Property

    Public Property State As String Implements IeZADUsers.State
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _State
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _State = value Then
                Return
            End If
            _State = value
            IsModified = True
        End Set
    End Property

    Public Property TelephoneNumber As String Implements IeZADUsers.TelephoneNumber
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TelephoneNumber
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TelephoneNumber = value Then
                Return
            End If
            _TelephoneNumber = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZADUsers.UpdatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy = value Then
                Return
            End If
            _UpdatedBy = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1 As String Implements IeZADUsers.UpdatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy1 = value Then
                Return
            End If
            _UpdatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedOn As String Implements IeZADUsers.UpdatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedOn = value Then
                Return
            End If
            _UpdatedOn = value
            IsModified = True
        End Set
    End Property

    Public Property Username As String Implements IeZADUsers.Username
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Username
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Username = value Then
                Return
            End If
            _Username = value
            IsModified = True
        End Set
    End Property

    Public Property IsECMUser As Integer Implements IeZADUsers.IsECMUser
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsECMUser
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _IsECMUser = value Then
                Return
            End If
            _IsECMUser = value
            IsModified = True
        End Set
    End Property

    Public Property sAMAccountName As String Implements IeZADUsers.sAMAccountName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _sAMAccountName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _sAMAccountName = value Then
                Return
            End If
            _sAMAccountName = value
            IsModified = True
        End Set
    End Property

    Public Property Manager As String Implements IeZADUsers.Manager
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Manager
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Manager = value Then
                Return
            End If

            _Manager = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
