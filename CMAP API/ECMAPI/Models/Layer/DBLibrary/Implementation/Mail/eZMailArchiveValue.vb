Imports ECMAPI

Public Class eZMailArchiveValue
    Inherits IDatabaseCommonItems
    Implements IeZMailArchiveValue

    Protected _MailArchiveValueId As Integer
    Protected _MailArchiveValue As String = ""
    Protected _MailArchiveId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New(MailArchiveValueId As Integer)
        Me._MailArchiveValueId = MailArchiveValueId
    End Sub
    Public Sub New()
    End Sub
    Public Property CreatedBy As Integer Implements IeZMailArchiveValue.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZMailArchiveValue.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZMailArchiveValue.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IeZMailArchiveValue.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property MailArchiveId As Integer Implements IeZMailArchiveValue.MailArchiveId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailArchiveId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _MailArchiveId = value Then
                Return
            End If
            _MailArchiveId = value
            IsModified = True
        End Set
    End Property

    Public Property MailArchiveValue As String Implements IeZMailArchiveValue.MailArchiveValue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailArchiveValue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MailArchiveValue = value Then
                Return
            End If
            _MailArchiveValue = value
            IsModified = True
        End Set
    End Property

    Public Property MailArchiveValueId As Integer Implements IeZMailArchiveValue.MailArchiveValueId
        Get
            If _MailArchiveValueId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _MailArchiveValueId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _MailArchiveValueId <> 0 AndAlso _MailArchiveValueId <> value Then
                Throw New MemberAccessException()
            End If
            _MailArchiveValueId = value
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZMailArchiveValue.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZMailArchiveValue.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZMailArchiveValue.UpdatedOn
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
