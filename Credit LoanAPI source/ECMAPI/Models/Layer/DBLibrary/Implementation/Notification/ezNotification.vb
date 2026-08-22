Imports ECMAPI

Public Class ezNotification
    Inherits IDatabaseCommonItems
    Implements IezNotification


    Protected _NotificationId As Integer
    Protected _ecmloginid As Integer
    Protected _refid As Integer
    Protected _notificationfrom As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(NotificationId As Integer)
        Me._NotificationId = NotificationId
    End Sub
    Public Property CreatedBy As Integer Implements IezNotification.CreatedBy
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

    Public Property CreatedBy1 As String Implements IezNotification.CreatedBy1
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

    Public Property CreatedOn As String Implements IezNotification.CreatedOn
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

    Public Property ecmloginid As Integer Implements IezNotification.ecmloginid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ecmloginid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ecmloginid = value Then
                Return
            End If
            _ecmloginid = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IezNotification.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property notificationfrom As String Implements IezNotification.notificationfrom
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _notificationfrom
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _notificationfrom = value Then
                Return
            End If
            _notificationfrom = value
            IsModified = True
        End Set
    End Property

    Public Property NotificationId As Integer Implements IezNotification.NotificationId
        Get
            If _NotificationId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _NotificationId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _NotificationId <> 0 AndAlso _NotificationId <> value Then
                Throw New MemberAccessException()
            End If
            _NotificationId = value
        End Set
    End Property

    Public Property refid As Integer Implements IezNotification.refid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _refid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _refid = value Then
                Return
            End If
            _refid = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IezNotification.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IezNotification.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IezNotification.UpdatedOn
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
