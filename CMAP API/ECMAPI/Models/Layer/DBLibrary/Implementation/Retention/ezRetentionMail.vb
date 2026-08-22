Imports ECMAPI

Public Class ezRetentionMail
    Inherits IDatabaseCommonItems
    Implements IezRetentionMail

    Protected _RetMailId As Integer
    Protected _RetentionId As Integer
    Protected _TemplateId As Integer
    Protected _ItemId As Integer
    Protected _MailTo As String = ""
    Protected _Createdon As String
    Protected _Updatedon As String
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Private _isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(RetMailId As Integer)
        Me._RetMailId = RetMailId
    End Sub

    Public Property RetMailId As Integer Implements IezRetentionMail.RetMailId
        Get
            If _RetMailId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _RetMailId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _RetMailId <> 0 AndAlso _RetMailId <> value Then
                Throw New MemberAccessException()
            End If
            _RetMailId = value
        End Set
    End Property

    Public Property ItemId As Integer Implements IezRetentionMail.ItemId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ItemId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ItemId = value Then
                Return
            End If
            _ItemId = value
            IsModified = True
        End Set
    End Property

    Public Property TemplateId As Integer Implements IezRetentionMail.TemplateId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TemplateId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TemplateId = value Then
                Return
            End If
            _TemplateId = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon As String Implements IezRetentionMail.Createdon
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

    Public Property Updatedon As String Implements IezRetentionMail.Updatedon
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

    Public Property Createdby As Integer Implements IezRetentionMail.Createdby
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

    Public Property Updatedby As Integer Implements IezRetentionMail.Updatedby
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

    Public Property CreatedBy1 As String Implements IezRetentionMail.CreatedBy1
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

    Public Property UpdatedBy1 As String Implements IezRetentionMail.UpdatedBy1
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

    Public ReadOnly Property isdeleted As Integer Implements IezRetentionMail.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Property MailTo As String Implements IezRetentionMail.MailTo
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailTo
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MailTo = value Then
                Return
            End If
            _MailTo = value
            IsModified = True
        End Set
    End Property

    Public Property RetentionId As Integer Implements IezRetentionMail.RetentionId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _RetentionId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _RetentionId = value Then
                Return
            End If
            _RetentionId = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
