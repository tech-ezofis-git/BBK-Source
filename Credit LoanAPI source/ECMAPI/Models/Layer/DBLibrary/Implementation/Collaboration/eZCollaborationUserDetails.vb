Imports ECMAPI

Public Class eZCollaborationUserDetails
    Inherits IDatabaseCommonItems
    Implements IeZCollaborationUserDetails

    Protected _CollId As Integer
    Protected _ID As Integer
    Protected _UserId As Integer
    Protected _Status As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(ID As Integer)
        Me._ID = ID
    End Sub
    Public Property CollId As Integer Implements IeZCollaborationUserDetails.CollId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CollId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CollId = value Then
                Return
            End If
            _CollId = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZCollaborationUserDetails.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZCollaborationUserDetails.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZCollaborationUserDetails.CreatedOn
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

    Public Property ID As Integer Implements IeZCollaborationUserDetails.ID
        Get
            If _ID = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ID
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ID <> 0 AndAlso _ID <> value Then
                Throw New MemberAccessException()
            End If
            _ID = value
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZCollaborationUserDetails.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property Status As String Implements IeZCollaborationUserDetails.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZCollaborationUserDetails.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZCollaborationUserDetails.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZCollaborationUserDetails.UpdatedOn
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

    Public Property UserID As Integer Implements IeZCollaborationUserDetails.UserId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UserId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UserId = value Then
                Return
            End If
            _UserId = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
