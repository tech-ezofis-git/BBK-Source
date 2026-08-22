Imports ECMAPI

Public Class eZMailType
    Inherits IDatabaseCommonItems
    Implements IeZMailType


    Protected _MailTypeId As Integer
    Protected _MailType As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Public Sub New(MailTypeId As Integer)
        Me._MailTypeId = MailTypeId
    End Sub
    Public Sub New()
    End Sub
    Public Property CreatedBy As Integer Implements IeZMailType.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZMailType.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZMailType.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IeZMailType.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property MailType As String Implements IeZMailType.MailType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MailType = value Then
                Return
            End If
            _MailType = value
            IsModified = True
        End Set
    End Property

    Public Property MailTypeId As Integer Implements IeZMailType.MailTypeId
        Get
            If _MailTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _MailTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _MailTypeId <> 0 AndAlso _MailTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _MailTypeId = value
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZMailType.UpdatedBy
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
        End Set
    End Property

    Public Property UpdatedBy1 As String Implements IeZMailType.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZMailType.UpdatedOn
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
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
