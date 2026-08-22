Imports ECMAPI

Public Class eZHideFileUsers
    Inherits IDatabaseCommonItems
    Implements IeZHideFileUsers

    Protected _HideFileUsersId As Integer
    Protected _HideFileId As Integer
    Protected _Show As Integer
    Protected _Sno As Integer
    Protected _Userid As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub

    Public Sub New(hidefileuserid As Integer)
        Me._HideFileUsersId = hidefileuserid
    End Sub

    Public Property CreatedBy() As Integer Implements IeZHideFileUsers.CreatedBy
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

    Public Property CreatedBy1() As String Implements IeZHideFileUsers.CreatedBy1
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

    Public Property CreatedOn() As String Implements IeZHideFileUsers.CreatedOn
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

    Public Property HideFileId() As Integer Implements IeZHideFileUsers.HideFileId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _HideFileId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _HideFileId = value Then
                Return
            End If
            _HideFileId = value
            IsModified = True
        End Set
    End Property

    Public Property HideFileUsersId() As Integer Implements IeZHideFileUsers.HideFileUsersId
        Get
            If _HideFileUsersId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _HideFileUsersId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _HideFileUsersId <> 0 AndAlso _HideFileUsersId <> value Then
                Throw New MemberAccessException()
            End If
            _HideFileUsersId = value
        End Set
    End Property

    Public ReadOnly Property Isdeleted() As Integer Implements IeZHideFileUsers.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property Show() As Integer Implements IeZHideFileUsers.Show
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Show
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Show = value Then
                Return
            End If
            _Show = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy() As Integer Implements IeZHideFileUsers.UpdatedBy
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

    Public Property UpdatedBy1() As String Implements IeZHideFileUsers.UpdatedBy1
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

    Public Property UpdatedOn() As String Implements IeZHideFileUsers.UpdatedOn
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

    Public Property UserId() As Integer Implements IeZHideFileUsers.UserId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Userid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Userid = value Then
                Return
            End If
            _Userid = value
            IsModified = True
        End Set
    End Property

    Private Property Sno() As Integer Implements IeZHideFileUsers.Sno
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Sno
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Sno = value Then
                Return
            End If
            _Sno = value
            IsModified = True
        End Set
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
