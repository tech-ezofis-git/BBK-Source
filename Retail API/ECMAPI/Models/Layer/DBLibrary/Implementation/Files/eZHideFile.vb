Imports ECMAPI

Public Class eZHideFile
    Inherits IDatabaseCommonItems
    Implements IeZHideFile

    Protected _HideFileId As Integer
    Protected _ItemId As Integer
    Protected _TemplateId As Integer
    Protected _HideAlways As Integer
    Protected _FromDate As String = ""
    Protected _ToDate As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(hidefileid As Integer)
        Me._HideFileId = hidefileid
    End Sub

    Public Property CreatedBy() As Integer Implements IeZHideFile.CreatedBy
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

    Public Property CreatedBy1() As String Implements IeZHideFile.CreatedBy1
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

    Public Property CreatedOn() As String Implements IeZHideFile.CreatedOn
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

    Public Property FromDate() As String Implements IeZHideFile.FromDate
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FromDate
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FromDate = value Then
                Return
            End If
            _FromDate = value
            IsModified = True
        End Set
    End Property

    Public Property HideAlways() As Integer Implements IeZHideFile.HideAlways
        Get
            If _HideAlways = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _HideAlways
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _HideAlways <> 0 AndAlso _HideAlways <> value Then
                Throw New MemberAccessException()
            End If
            _HideAlways = value
        End Set
    End Property

    Public Property HideFileId() As Integer Implements IeZHideFile.HideFileId
        Get
            If _HideFileId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _HideFileId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _HideFileId <> 0 AndAlso _HideFileId <> value Then
                Throw New MemberAccessException()
            End If
            _HideFileId = value
        End Set
    End Property

    Public ReadOnly Property Isdeleted() As Integer Implements IeZHideFile.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property ItemId() As Integer Implements IeZHideFile.ItemId
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

    Public Property TemplateId() As Integer Implements IeZHideFile.TemplateId
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

    Public Property ToDate() As String Implements IeZHideFile.ToDate
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToDate
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ToDate = value Then
                Return
            End If
            _ToDate = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy() As Integer Implements IeZHideFile.UpdatedBy
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

    Public Property UpdatedBy1() As String Implements IeZHideFile.UpdatedBy1
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

    Public Property UpdatedOn() As String Implements IeZHideFile.UpdatedOn
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
