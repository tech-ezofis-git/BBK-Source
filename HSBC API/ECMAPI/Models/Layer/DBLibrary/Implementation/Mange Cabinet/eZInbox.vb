Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZInbox
    Inherits IDatabaseCommonItems
    Implements IeZInbox
    Protected _LoginId As Integer
    Protected _LoginName As String
    Protected _NodeId As Integer

    Protected _LevelId As Integer
    Protected _NodeName As String
    Protected _PathId As String
    Protected _ParentNodeId As Integer
    
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer
    Public Sub New(tempNodeId As Integer)
        Me._NodeId = tempNodeId
    End Sub
    Public Sub New(tmpNodeName As String)
        Me._NodeName = tmpNodeName.Trim()
    End Sub
    Public Sub New()
    End Sub
    Public Property NodeId() As Integer Implements IeZInbox.NodeId
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
    Public Property NodeName() As String Implements IeZInbox.NodeName
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
    Public Property PathId() As String Implements IeZInbox.PathId
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
    Public Property ParentNodeId() As Integer Implements IeZInbox.ParentNodeId
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
  
    Public Property LoginId() As Integer Implements IeZInbox.LoginId
        Get
            If _LoginId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LoginId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LoginId <> 0 AndAlso _LoginId <> value Then
                Throw New MemberAccessException()
            End If
            _LoginId = value
        End Set
    End Property

    Public Property LevelId() As Integer Implements IeZInbox.LevelId
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
    
    Public Property LoginName() As String Implements IeZInbox.LoginName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LoginName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LoginName = value Then
                Return
            End If
            _LoginName = value
            IsModified = True
        End Set
    End Property
    
    Public Property UpdatedBy1() As String Implements IeZInbox.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZInbox.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZInbox.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZInbox.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZInbox.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZInbox.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZInbox.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZInboxExist() As Boolean Implements IeZInbox.IseZInboxExist
        Get
            Return (_NodeId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
