Imports ECMAPI

Public Class ezSupportFiles
    Inherits IDatabaseCommonItems
    Implements IezSupportFiles

    Protected _Attachmentid As Integer
    Protected _ersid As Integer
    Protected _itemid As Integer
    Protected _templateid As Integer
    Protected _ifilepath As String = ""
    Protected _ifiletype As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(Attachmentid As Integer)
        Me._Attachmentid = Attachmentid
    End Sub
    Public Property Attachmentid As Integer Implements IezSupportFiles.Attachmentid
        Get
            If _Attachmentid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Attachmentid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Attachmentid <> 0 AndAlso _Attachmentid <> value Then
                Throw New MemberAccessException()
            End If
            _Attachmentid = value
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IezSupportFiles.CreatedBy
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

    Public Property CreatedBy1 As String Implements IezSupportFiles.CreatedBy1
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

    Public Property CreatedOn As String Implements IezSupportFiles.CreatedOn
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

    Public Property ersid As Integer Implements IezSupportFiles.ersid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ersid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ersid = value Then
                Return
            End If
            _ersid = value
            IsModified = True
        End Set
    End Property

    Public Property ifilepath As String Implements IezSupportFiles.ifilepath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ifilepath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ifilepath = value Then
                Return
            End If
            _ifilepath = value
            IsModified = True
        End Set
    End Property

    Public Property ifiletype As String Implements IezSupportFiles.ifiletype
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ifiletype
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ifiletype = value Then
                Return
            End If
            _ifiletype = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IezSupportFiles.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property itemid As Integer Implements IezSupportFiles.itemid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _itemid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _itemid = value Then
                Return
            End If
            _itemid = value
            IsModified = True
        End Set
    End Property

    Public Property templateid As Integer Implements IezSupportFiles.templateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _templateid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _templateid = value Then
                Return
            End If
            _templateid = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IezSupportFiles.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IezSupportFiles.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IezSupportFiles.UpdatedOn
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
