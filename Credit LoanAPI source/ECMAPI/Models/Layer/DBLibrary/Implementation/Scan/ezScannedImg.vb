Imports ECMAPI

Public Class ezScannedImg
    Inherits IDatabaseCommonItems
    Implements IezScannedImg

    Protected _ScannedImgId As Integer
    Protected _Ifilepath As String = ""
    Protected _pcname As String = ""
    Protected _appname As String = ""
    Protected _Status As Integer
    Protected _nopages As Integer
    Protected _TemplateId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(ScannedImgId As Integer)
        Me._ScannedImgId = ScannedImgId
    End Sub

    Public Property appname As String Implements IezScannedImg.appname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _appname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _appname = value Then
                Return
            End If
            _appname = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IezScannedImg.CreatedBy
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

    Public Property CreatedBy1 As String Implements IezScannedImg.CreatedBy1
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

    Public Property CreatedOn As String Implements IezScannedImg.CreatedOn
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

    Public Property Ifilepath As String Implements IezScannedImg.Ifilepath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Ifilepath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Ifilepath = value Then
                Return
            End If
            _Ifilepath = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IezScannedImg.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property nopages As Integer Implements IezScannedImg.nopages
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _nopages
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _nopages = value Then
                Return
            End If
            _nopages = value
            IsModified = True
        End Set
    End Property

    Public Property pcname As String Implements IezScannedImg.pcname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _pcname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _pcname = value Then
                Return
            End If
            _pcname = value
            IsModified = True
        End Set
    End Property

    Public Property ScannedImgId As Integer Implements IezScannedImg.ScannedImgId
        Get
            If _ScannedImgId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ScannedImgId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ScannedImgId <> 0 AndAlso _ScannedImgId <> value Then
                Throw New MemberAccessException()
            End If
            _ScannedImgId = value
        End Set
    End Property

    Public Property Status As Integer Implements IezScannedImg.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property

    Public Property TemplateId As Integer Implements IezScannedImg.TemplateId
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

    Public Property UpdatedBy As Integer Implements IezScannedImg.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IezScannedImg.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IezScannedImg.UpdatedOn
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
