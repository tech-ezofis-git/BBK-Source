Imports ECMAPI

Public Class eZHierarchy
    Inherits IDatabaseCommonItems
    Implements IeZHierarchy

    Protected _TemplateId As Integer
    Protected _Hierarchy_id As Integer
    Protected _ToLevelId As Integer
    Protected _FromLevelId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New(Hierarchy_id As Integer)
        Me._Hierarchy_id = Hierarchy_id
    End Sub
    Public Sub New()
    End Sub
    Public Property CreatedBy As Integer Implements IeZHierarchy.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZHierarchy.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZHierarchy.CreatedOn
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

    Public Property FromLevelId As Integer Implements IeZHierarchy.FromLevelId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FromLevelId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FromLevelId = value Then
                Return
            End If
            _FromLevelId = value
            IsModified = True
        End Set
    End Property

    Public Property Hierarchy_id As Integer Implements IeZHierarchy.Hierarchy_id
        Get
            If _Hierarchy_id = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Hierarchy_id
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Hierarchy_id <> 0 AndAlso _Hierarchy_id <> value Then
                Throw New MemberAccessException()
            End If
            _Hierarchy_id = value
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZHierarchy.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property TemplateId As Integer Implements IeZHierarchy.TemplateId
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

    Public Property ToLevelId As Integer Implements IeZHierarchy.ToLevelId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToLevelId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ToLevelId = value Then
                Return
            End If
            _ToLevelId = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZHierarchy.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZHierarchy.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZHierarchy.UpdatedOn
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
