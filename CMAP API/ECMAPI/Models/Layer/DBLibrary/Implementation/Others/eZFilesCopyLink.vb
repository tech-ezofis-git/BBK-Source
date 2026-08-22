Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZFilesCopyLink
    Inherits IDatabaseCommonItems
    Implements IeZFilesCopyLink
    Protected _CopyLinkId As Integer
    Protected _NodeId As Integer
    Protected _TemplateId As Integer
    Protected _ISMoved As Integer
    Protected _itemid As Integer
    Protected _CopyBy As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(CopyLinkId As Integer)
        Me._CopyLinkId = CopyLinkId
    End Sub
    Public Sub New()
    End Sub


    Public Property itemid() As Integer Implements IeZFilesCopyLink.itemid
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

    Public Property CopyBy() As Integer Implements IeZFilesCopyLink.CopyBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CopyBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CopyBy = value Then
                Return
            End If
            _CopyBy = value
            IsModified = True
        End Set
    End Property
    Public Property ISMoved() As Boolean Implements IeZFilesCopyLink.ISMoved
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ISMoved
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _ISMoved = value Then
                Return
            End If
            _ISMoved = value
            IsModified = True
        End Set
    End Property
    Public Property NodeId() As Integer Implements IeZFilesCopyLink.NodeId
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

    Public Property TemplateId() As Integer Implements IeZFilesCopyLink.TemplateId
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
    Public Property CopyLinkId() As Integer Implements IeZFilesCopyLink.CopyLinkId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CopyLinkId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CopyLinkId = value Then
                Return
            End If
            _CopyLinkId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZFilesCopyLink.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZFilesCopyLink.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZFilesCopyLink.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZFilesCopyLink.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZFilesCopyLink.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZFilesCopyLink.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZFilesCopyLink.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsDocumentLink() As Boolean Implements IeZFilesCopyLink.IsDocumentLink
        Get
            Return (_CopyLinkId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub




End Class