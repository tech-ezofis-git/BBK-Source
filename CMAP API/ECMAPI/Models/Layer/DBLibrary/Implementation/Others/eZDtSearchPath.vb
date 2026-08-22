Imports ECMAPI

Public Class eZDtSearchPath
    Inherits IDatabaseCommonItems
    Implements IeZDtSearchPath

    Protected _indexinpath As Integer
    Protected _ERSId As Integer
    Protected _TemplateId As Integer
    Protected _IFilePath As String = ""
    Protected _Status As Boolean
    Protected _ifiletype As String = ""
    Protected _itemid As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(indexinpath As Integer)
        Me._indexinpath = indexinpath
    End Sub
    Public Property CreatedBy As Integer Implements IeZDtSearchPath.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZDtSearchPath.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZDtSearchPath.CreatedOn
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

    Public Property ERSId As Integer Implements IeZDtSearchPath.ERSId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ERSId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ERSId = value Then
                Return
            End If
            _ERSId = value
            IsModified = True
        End Set
    End Property

    Public Property IFilePath As String Implements IeZDtSearchPath.IFilePath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IFilePath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _IFilePath = value Then
                Return
            End If
            _IFilePath = value
            IsModified = True
        End Set
    End Property

    Public Property ifiletype As String Implements IeZDtSearchPath.ifiletype
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

    Public Property indexpathid As Integer Implements IeZDtSearchPath.indexpathid
        Get
            If _indexinpath = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _indexinpath
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _indexinpath <> 0 AndAlso _indexinpath <> value Then
                Throw New MemberAccessException()
            End If
            _indexinpath = value
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZDtSearchPath.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property itemid As Integer Implements IeZDtSearchPath.itemid
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

    Public Property Status As Boolean Implements IeZDtSearchPath.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property

    Public Property TemplateId As Integer Implements IeZDtSearchPath.TemplateId
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

    Public Property UpdatedBy As Integer Implements IeZDtSearchPath.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZDtSearchPath.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZDtSearchPath.UpdatedOn
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
