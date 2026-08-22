Imports ECMAPI

Public Class eZLdapConnection
    Inherits IDatabaseCommonItems
    Implements IeZLdapConnection

    Protected _LdapConnId As Integer
    Protected _LdapServer As String = ""
    Protected _LdapDomain As String = ""
    Protected _Username As String = ""
    Protected _Pasword As String = ""
    Protected _Preferred As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _LdapPath As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub

    Public Sub New(LdapConnId As Integer)
        Me._LdapConnId = LdapConnId
    End Sub

    Public Property CreatedBy As Integer Implements IeZLdapConnection.CreatedBy
        Get
            If _CreatedBy = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _CreatedBy
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _CreatedBy <> 0 AndAlso _CreatedBy <> value Then
                Throw New MemberAccessException()
            End If
            _CreatedBy = value
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZLdapConnection.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZLdapConnection.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IeZLdapConnection.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property LdapConnId As Integer Implements IeZLdapConnection.LdapConnId
        Get
            If _LdapConnId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LdapConnId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LdapConnId <> 0 AndAlso _LdapConnId <> value Then
                Throw New MemberAccessException()
            End If
            _LdapConnId = value
        End Set
    End Property

    Public Property LdapDomain As String Implements IeZLdapConnection.LdapDomain
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LdapDomain
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LdapDomain = value Then
                Return
            End If
            _LdapDomain = value
            IsModified = True
        End Set
    End Property

    Public Property LdapServer As String Implements IeZLdapConnection.LdapServer
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LdapServer
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LdapServer = value Then
                Return
            End If
            _LdapServer = value
            IsModified = True
        End Set
    End Property

    Public Property Pasword As String Implements IeZLdapConnection.Pasword
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Pasword
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Pasword = value Then
                Return
            End If
            _Pasword = value
            IsModified = True
        End Set
    End Property

    Public Property Preferred As Integer Implements IeZLdapConnection.Preferred
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Preferred
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Preferred = value Then
                Return
            End If
            _Preferred = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZLdapConnection.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZLdapConnection.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZLdapConnection.UpdatedOn
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

    Public Property Username As String Implements IeZLdapConnection.Username
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

    Public Property LdapPath As String Implements IeZLdapConnection.LdapPath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LdapPath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LdapPath = value Then
                Return
            End If

            _LdapPath = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
