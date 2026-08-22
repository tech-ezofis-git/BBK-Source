Imports ECMAPI
Public Class eZECMProfileTemplate
    Inherits IDatabaseCommonItems
    Implements IeZECMProfileTemplate

    Protected _ProfileTemplateId As Integer
    Protected _EcmProfileId As Integer
    Protected _TemplateId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Public Sub New()
    End Sub
    Public Sub New(ProfileTemplateId As Integer)
        Me._ProfileTemplateId = ProfileTemplateId
    End Sub

    Public Property CreatedBy As Integer Implements IeZECMProfileTemplate.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZECMProfileTemplate.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZECMProfileTemplate.CreatedOn
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

    Public Property EcmProfileId As Integer Implements IeZECMProfileTemplate.EcmProfileId
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

    Public ReadOnly Property Isdeleted As Integer Implements IeZECMProfileTemplate.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property ProfileTemplateId As Integer Implements IeZECMProfileTemplate.ProfileTemplateId
        Get
            If _ProfileTemplateId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ProfileTemplateId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ProfileTemplateId <> 0 AndAlso _ProfileTemplateId <> value Then
                Throw New MemberAccessException()
            End If
            _ProfileTemplateId = value
        End Set
    End Property

    Public Property TemplateId As Integer Implements IeZECMProfileTemplate.TemplateId
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

    Public Property UpdatedBy As Integer Implements IeZECMProfileTemplate.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZECMProfileTemplate.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZECMProfileTemplate.UpdatedOn
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
