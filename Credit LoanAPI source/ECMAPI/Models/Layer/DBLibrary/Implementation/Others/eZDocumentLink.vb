Imports System.Data
Imports System.Configuration
Imports System.Web


Public Class eZDocumentLink
    Inherits IDatabaseCommonItems
    Implements IeZDocumentLink
    Protected _LinkId As Integer
    Protected _LinkedItemId As Integer
    Protected _TemplateId As Integer
    Protected _LinkedTemplateId As Integer
    Protected _itemid As Integer
    Protected _LinkBy As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(LinkId As Integer)
        Me._LinkId = LinkId
    End Sub
    Public Sub New()
    End Sub


    Public Property itemid() As Integer Implements IeZDocumentLink.itemid
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

    Public Property LinkBy() As Integer Implements IeZDocumentLink.LinkBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LinkBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LinkBy = value Then
                Return
            End If
            _LinkBy = value
            IsModified = True
        End Set
    End Property
    Public Property LinkedItemId() As Integer Implements IeZDocumentLink.LinkedItemId
        Get
            If _LinkedItemId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LinkedItemId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LinkedItemId <> 0 AndAlso _LinkedItemId <> value Then
                Throw New MemberAccessException()
            End If
            _LinkedItemId = value
        End Set
    End Property
    Public Property LinkedTemplateId() As Integer Implements IeZDocumentLink.LinkedTemplateId
        Get
            If _LinkedTemplateId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LinkedTemplateId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LinkedTemplateId <> 0 AndAlso _LinkedTemplateId <> value Then
                Throw New MemberAccessException()
            End If
            _LinkedTemplateId = value
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZDocumentLink.TemplateId
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
    Public Property LinkId() As Integer Implements IeZDocumentLink.LinkId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LinkId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LinkId = value Then
                Return
            End If
            _LinkId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZDocumentLink.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZDocumentLink.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZDocumentLink.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZDocumentLink.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZDocumentLink.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZDocumentLink.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZDocumentLink.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsDocumentLink() As Boolean Implements IeZDocumentLink.IsDocumentLink
        Get
            Return (_LinkId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

   

  
End Class
