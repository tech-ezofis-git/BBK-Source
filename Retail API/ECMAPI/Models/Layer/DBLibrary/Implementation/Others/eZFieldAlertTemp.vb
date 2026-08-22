Imports ECMAPI

Public Class eZFieldAlertTemp
    Inherits IDatabaseCommonItems
    Implements IeZFieldAlertTemp

    Protected _BodyMessage As String = ""
    Protected _ToAdd As String = ""
    Protected _Id As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer


    Public Sub New()
    End Sub
    Public Sub New(Id As Integer)
        Me._Id = Id
    End Sub
    Public Property BodyMessage As String Implements IeZFieldAlertTemp.BodyMessage
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _BodyMessage
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _BodyMessage = value Then
                Return
            End If
            _BodyMessage = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZFieldAlertTemp.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZFieldAlertTemp.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZFieldAlertTemp.CreatedOn
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

    Public Property Id As Integer Implements IeZFieldAlertTemp.Id
        Get
            If _Id = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Id
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Id <> 0 AndAlso _Id <> value Then
                Throw New MemberAccessException()
            End If
            _Id = value
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZFieldAlertTemp.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property ToAdd As String Implements IeZFieldAlertTemp.ToAdd
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToAdd
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ToAdd = value Then
                Return
            End If
            _ToAdd = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZFieldAlertTemp.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZFieldAlertTemp.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZFieldAlertTemp.UpdatedOn
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
