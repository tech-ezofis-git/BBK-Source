Imports ECMAPI

Public Class ezFoldersByUser
    Inherits IDatabaseCommonItems
    Implements IezFoldersByUser


    Protected _TemplateId As Integer
    Protected _NodeId As Integer
    Protected _UserId As Integer
    Protected _LevelId As Integer
    Protected _NodeName As String
    Protected _PathId As String
    Protected _ParentNodeId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New(NodeId As Integer)
        Me._NodeId = NodeId
    End Sub
    Public Sub New()
    End Sub
    Public Property CreatedBy As Integer Implements IezFoldersByUser.CreatedBy
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

    Public Property CreatedBy1 As String Implements IezFoldersByUser.CreatedBy1
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

    Public Property CreatedOn As String Implements IezFoldersByUser.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IezFoldersByUser.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property LevelId As Integer Implements IezFoldersByUser.LevelId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LevelId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LevelId = value Then
                Return
            End If
            _LevelId = value
            IsModified = True
        End Set
    End Property

    Public Property NodeId As Integer Implements IezFoldersByUser.NodeId
        Get
            If _NodeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _NodeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _NodeId <> 0 AndAlso _NodeId <> value Then
                Throw New MemberAccessException()
            End If
            _NodeId = value
        End Set
    End Property

    Public Property NodeName As String Implements IezFoldersByUser.NodeName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _NodeName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _NodeName = value Then
                Return
            End If
            _NodeName = value
            IsModified = True
        End Set
    End Property

    Public Property ParentNodeId As Integer Implements IezFoldersByUser.ParentNodeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ParentNodeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ParentNodeId = value Then
                Return
            End If
            _ParentNodeId = value
            IsModified = True
        End Set
    End Property

    Public Property PathId As Integer Implements IezFoldersByUser.PathId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _PathId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _PathId = value Then
                Return
            End If
            _PathId = value
            IsModified = True
        End Set
    End Property

    Public Property TemplateId As Integer Implements IezFoldersByUser.TemplateId
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

    Public Property UpdatedBy As Integer Implements IezFoldersByUser.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IezFoldersByUser.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IezFoldersByUser.UpdatedOn
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

    Public Property UserId As Integer Implements IezFoldersByUser.UserId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UserId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UserId = value Then
                Return
            End If
            _UserId = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
