Imports ECMAPI

Public Class eZHidePages
    Inherits IDatabaseCommonItems
    Implements IeZHidePages

    Protected _HideId As Integer
    Protected _ItemId As Integer
    Protected _TemplateId As Integer
    Protected _Pages As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Public Sub New()
    End Sub
    Public Sub New(hideid As Integer)
        Me._HideId = hideid
    End Sub
    Public Property CreatedBy() As Integer Implements IeZHidePages.CreatedBy
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

    Public Property CreatedBy1() As String Implements IeZHidePages.CreatedBy1
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

    Public Property CreatedOn() As String Implements IeZHidePages.CreatedOn
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

    Public Property HideId() As Integer Implements IeZHidePages.HideId
        Get
            If _HideId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _HideId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _HideId <> 0 AndAlso _HideId <> value Then
                Throw New MemberAccessException()
            End If
            _HideId = value
        End Set
    End Property

    Public ReadOnly Property Isdeleted() As Integer Implements IeZHidePages.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property ItemId() As Integer Implements IeZHidePages.ItemId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ItemId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ItemId = value Then
                Return
            End If
            _ItemId = value
            IsModified = True
        End Set
    End Property

    Public Property Pages() As String Implements IeZHidePages.Pages
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Pages
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Pages = value Then
                Return
            End If
            _Pages = value
            IsModified = True
        End Set
    End Property

    Public Property TemplateId() As Integer Implements IeZHidePages.TemplateId
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

    Public Property UpdatedBy() As Integer Implements IeZHidePages.UpdatedBy
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

    Public Property UpdatedBy1() As String Implements IeZHidePages.UpdatedBy1
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

    Public Property UpdatedOn() As String Implements IeZHidePages.UpdatedOn
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
