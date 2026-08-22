Imports ECMAPI

Public Class eZMapTemplate
    Inherits IDatabaseCommonItems
    Implements IeZMapTemplate


    Protected _LocationId As Integer
    Protected _CabinetId As Integer
    Protected _TemplateId As Integer
    Protected _MapTemplateId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(MapTemplateId As Integer)
        Me._MapTemplateId = MapTemplateId
    End Sub
    Public Property CabinetId As Integer Implements IeZMapTemplate.CabinetId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabinetId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CabinetId = value Then
                Return
            End If
            _CabinetId = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZMapTemplate.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZMapTemplate.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZMapTemplate.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IeZMapTemplate.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property LocationId As Integer Implements IeZMapTemplate.LocationId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LocationId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LocationId = value Then
                Return
            End If
            _LocationId = value
            IsModified = True
        End Set
    End Property

    Public Property MapTemplateId As Integer Implements IeZMapTemplate.MapTemplateId
        Get
            If _MapTemplateId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _MapTemplateId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _MapTemplateId <> 0 AndAlso _MapTemplateId <> value Then
                Throw New MemberAccessException()
            End If
            _MapTemplateId = value
        End Set
    End Property

    Public Property TemplateId As Integer Implements IeZMapTemplate.TemplateId
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

    Public Property UpdatedBy As Integer Implements IeZMapTemplate.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZMapTemplate.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZMapTemplate.UpdatedOn
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
