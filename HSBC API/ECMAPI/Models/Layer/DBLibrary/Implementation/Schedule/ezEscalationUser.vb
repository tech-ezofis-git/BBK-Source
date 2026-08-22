Imports ECMAPI

Public Class ezEscalationUser
    Inherits IDatabaseCommonItems
    Implements IezEscalationUser

    Protected _EscalationId As Integer
    Protected _EscalationUserId As Integer
    Protected _ECMLoginid As Integer
    Protected _LoginName As String = ""
    Protected _Createdon As String
    Protected _Updatedon As String
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Protected _ResponseType As String = ""
    Private _isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(EscalationUserId As Integer)
        Me._EscalationUserId = EscalationUserId
    End Sub

    Public Property EscalationUserId As Integer Implements IezEscalationUser.EscalationUserId
        Get
            If _EscalationUserId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _EscalationUserId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _EscalationUserId <> 0 AndAlso _EscalationUserId <> value Then
                Throw New MemberAccessException()
            End If
            _EscalationUserId = value
        End Set
    End Property

    Public Property EscalationId As Integer Implements IezEscalationUser.EscalationId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EscalationId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _EscalationId = value Then
                Return
            End If
            _EscalationId = value
            IsModified = True
        End Set
    End Property

    Public Property ECMLoginid As Integer Implements IezEscalationUser.ECMLoginid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMLoginid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMLoginid = value Then
                Return
            End If
            _ECMLoginid = value
            IsModified = True
        End Set
    End Property

    Public Property LoginName As String Implements IezEscalationUser.LoginName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LoginName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LoginName = value Then
                Return
            End If
            _LoginName = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon As String Implements IezEscalationUser.Createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Createdon = value Then
                Return
            End If
            _Createdon = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedon As String Implements IezEscalationUser.Updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedon = value Then
                Return
            End If
            _Updatedon = value
            IsModified = True
        End Set
    End Property

    Public Property Createdby As Integer Implements IezEscalationUser.Createdby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Createdby = value Then
                Return
            End If
            _Createdby = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedby As Integer Implements IezEscalationUser.Updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedby = value Then
                Return
            End If
            _Updatedby = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IezEscalationUser.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Createdby1 = value Then
                Return
            End If
            _Createdby1 = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1 As String Implements IezEscalationUser.UpdatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedby1 = value Then
                Return
            End If
            _Updatedby1 = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements IezEscalationUser.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
