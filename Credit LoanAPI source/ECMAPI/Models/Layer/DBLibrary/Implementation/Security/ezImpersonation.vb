Imports ECMAPI

Public Class ezImpersonation
    Inherits IDatabaseCommonItems
    Implements IezImpersonation


    Protected _ImpersonateId As Integer
    Protected _ImpersonationFor As String = ""
    Protected _Domain As String = ""
    Protected _Username As String = ""
    Protected _Password As String = ""
    Protected _ERSid As Integer
    Protected _TemplateId As Integer
    Protected _Description As String = ""
    Protected _Createdon As String
    Protected _Updatedon As String
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Private _isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(ImpersonateId As Integer)
        Me._ImpersonateId = ImpersonateId
    End Sub

    Public Property ImpersonateId As Integer Implements IezImpersonation.ImpersonateId
        Get
            If _ImpersonateId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ImpersonateId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ImpersonateId <> 0 AndAlso _ImpersonateId <> value Then
                Throw New MemberAccessException()
            End If
            _ImpersonateId = value
        End Set
    End Property

    Public Property ImpersonationFor As String Implements IezImpersonation.ImpersonationFor
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ImpersonationFor
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ImpersonationFor = value Then
                Return
            End If
            _ImpersonationFor = value
            IsModified = True
        End Set
    End Property

    Public Property Domain As String Implements IezImpersonation.Domain
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Domain
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Domain = value Then
                Return
            End If
            _Domain = value
            IsModified = True
        End Set
    End Property

    Public Property Username As String Implements IezImpersonation.Username
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

    Public Property Password As String Implements IezImpersonation.Password
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Password
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Password = value Then
                Return
            End If
            _Password = value
            IsModified = True
        End Set
    End Property

    Public Property ERSid As Integer Implements IezImpersonation.ERSid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ERSid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ERSid = value Then
                Return
            End If
            _ERSid = value
            IsModified = True
        End Set
    End Property

    Public Property TemplateId As Integer Implements IezImpersonation.TemplateId
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

    Public Property Description As String Implements IezImpersonation.Description
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Description
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Description = value Then
                Return
            End If
            _Description = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon As String Implements IezImpersonation.Createdon
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

    Public Property Updatedon As String Implements IezImpersonation.Updatedon
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

    Public Property Createdby As Integer Implements IezImpersonation.Createdby
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

    Public Property Updatedby As Integer Implements IezImpersonation.Updatedby
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

    Public Property CreatedBy1 As String Implements IezImpersonation.CreatedBy1
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

    Public Property UpdatedBy1 As String Implements IezImpersonation.UpdatedBy1
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

    Public ReadOnly Property isdeleted As Integer Implements IezImpersonation.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
