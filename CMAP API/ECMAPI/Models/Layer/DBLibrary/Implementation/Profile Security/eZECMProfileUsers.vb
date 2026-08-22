Imports ECMAPI
Public Class eZECMProfileUsers
    Inherits IDatabaseCommonItems
    Implements IeZECMProfileUsers

    Protected _ECMProfileUsersId As Integer
    Protected _EcmProfileId As Integer
    Protected _ECMLoginId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(ECMProfileUsersId As Integer)
        Me._ECMProfileUsersId = ECMProfileUsersId
    End Sub

    Public Property CreatedBy As Integer Implements IeZECMProfileUsers.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZECMProfileUsers.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZECMProfileUsers.CreatedOn
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

    Public Property ECMLoginId As Integer Implements IeZECMProfileUsers.ECMLoginId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMLoginId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMLoginId = value Then
                Return
            End If
            _ECMLoginId = value
            IsModified = True
        End Set
    End Property

    Public Property EcmProfileId As Integer Implements IeZECMProfileUsers.ECMProfileId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EcmProfileId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _EcmProfileId = value Then
                Return
            End If
            _EcmProfileId = value
            IsModified = True
        End Set
    End Property

    Public Property ECMProfileUsersId As Integer Implements IeZECMProfileUsers.ECMProfileUsersId
        Get
            If _ECMProfileUsersId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ECMProfileUsersId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ECMProfileUsersId <> 0 AndAlso _ECMProfileUsersId <> value Then
                Throw New MemberAccessException()
            End If
            _ECMProfileUsersId = value
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZECMProfileUsers.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property UpdatedBy As Integer Implements IeZECMProfileUsers.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZECMProfileUsers.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZECMProfileUsers.UpdatedOn
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

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
