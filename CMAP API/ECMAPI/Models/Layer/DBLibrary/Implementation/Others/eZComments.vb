Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZComments
    Inherits IDatabaseCommonItems
    Implements IeZComments
    Protected _CommentsId As Integer
    Protected _TemplateId As Integer
    Protected _itemid As Integer
    Protected _CommentsBy As Integer
    Protected _Processid As Integer
    Protected _ExternalCommentsBy As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _Comments As String
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(CommentsId As Integer)
        Me._CommentsId = CommentsId
    End Sub
    Public Sub New()
    End Sub
    Public Property Comments() As String Implements IeZComments.Comments
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Comments
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Comments = value Then
                Return
            End If
            _Comments = value
            IsModified = True
        End Set
    End Property

    Public Property itemid() As Integer Implements IeZComments.itemid
        Get
            If _itemid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _itemid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _itemid <> 0 AndAlso _itemid <> value Then
                Throw New MemberAccessException()
            End If
            _itemid = value
        End Set
    End Property

    Public Property CommentsBy() As Integer Implements IeZComments.CommentsBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CommentsBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CommentsBy = value Then
                Return
            End If
            _CommentsBy = value
            IsModified = True
        End Set
    End Property
    Public Property Processid() As Integer Implements IeZComments.Processid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Processid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Processid = value Then
                Return
            End If
            _Processid = value
            IsModified = True
        End Set
    End Property
    Public Property ExternalCommentsBy() As String Implements IeZComments.ExternalCommentsBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ExternalCommentsBy
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ExternalCommentsBy = value Then
                Return
            End If
            _ExternalCommentsBy = value
            IsModified = True
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZComments.TemplateId
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

    Public Property CommentsId() As Integer Implements IeZComments.CommentsId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CommentsId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CommentsId = value Then
                Return
            End If
            _CommentsId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZComments.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZComments.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZComments.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZComments.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZComments.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZComments.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZComments.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsComments() As Boolean Implements IeZComments.IsComments
        Get
            Return (_CommentsId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub




End Class

