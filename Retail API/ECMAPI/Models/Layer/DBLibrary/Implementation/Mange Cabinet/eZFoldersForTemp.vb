Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZFoldersForTemp
    Inherits IDatabaseCommonItems
    Implements IeZFoldersForTemp
    Protected _TemplateId As Integer
    Protected _TemplateName As String = ""
    Protected _NodeId As Integer
    Protected _UserId As Integer
    Protected _LevelId As Integer
    Protected _NodeName As String = ""
    Protected _PathId As String = ""
    Protected _TableName As String = ""
    Protected _ParentNodeId As Integer
    Protected _CabinetName As String = ""
    Protected _CabinetID As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Public Sub New(tempNodeId As Integer)
        Me._NodeId = tempNodeId
    End Sub
    Public Sub New(tmpNodeName As String)
        Me._NodeName = tmpNodeName.Trim()
    End Sub
    Public Sub New()
    End Sub
    Public Property TableName() As String Implements IeZFoldersForTemp.TableName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TableName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TableName = value Then
                Return
            End If
            _TableName = value
            IsModified = True
        End Set
    End Property
    Public Property NodeId() As Integer Implements IeZFoldersForTemp.NodeId
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
    Public Property NodeName() As String Implements IeZFoldersForTemp.NodeName
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
    Public Property PathId() As String Implements IeZFoldersForTemp.PathId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _PathId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _PathId = value Then
                Return
            End If
            _PathId = value
            IsModified = True
        End Set
    End Property
    Public Property ParentNodeId() As Integer Implements IeZFoldersForTemp.ParentNodeId
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
    Public Property CabinetName() As String Implements IeZFoldersForTemp.CabinetName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabinetName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CabinetName = value Then
                Return
            End If
            _CabinetName = value
            IsModified = True
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZFoldersForTemp.TemplateId
        Get
            If _TemplateId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _TemplateId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _TemplateId <> 0 AndAlso _TemplateId <> value Then
                Throw New MemberAccessException()
            End If
            _TemplateId = value
        End Set
    End Property

    Public Property LevelId() As Integer Implements IeZFoldersForTemp.LevelId
        Get
            If _LevelId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LevelId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LevelId <> 0 AndAlso _LevelId <> value Then
                Throw New MemberAccessException()
            End If
            _LevelId = value
        End Set
    End Property
    Public Property UserId() As Integer Implements IeZFoldersForTemp.UserId
        Get
            If _UserId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _UserId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _UserId <> 0 AndAlso _UserId <> value Then
                Throw New MemberAccessException()
            End If
            _UserId = value
        End Set
    End Property
    Public Property TemplateName() As String Implements IeZFoldersForTemp.TemplateName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TemplateName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TemplateName = value Then
                Return
            End If
            _TemplateName = value
            IsModified = True
        End Set
    End Property
    Public Property CabinetID() As Integer Implements IeZFoldersForTemp.CabinetID
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabinetID
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CabinetID = value Then
                Return
            End If
            _CabinetID = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZFoldersForTemp.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZFoldersForTemp.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZFoldersForTemp.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZFoldersForTemp.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZFoldersForTemp.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZFoldersForTemp.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZFoldersForTemp.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZFoldersForTempExist() As Boolean Implements IeZFoldersForTemp.IseZFoldersForTempExist
        Get
            Return (_NodeId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
