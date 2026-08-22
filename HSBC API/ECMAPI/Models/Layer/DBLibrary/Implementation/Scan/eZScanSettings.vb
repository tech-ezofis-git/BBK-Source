Imports ECMAPI

Public Class eZScanSettings
    Inherits IDatabaseCommonItems
    Implements IeZScanSettings


    Protected _SettingId As Integer
    Protected _Dublex As Boolean
    Protected _Colour As Boolean
    Protected _FileName As String = ""
    Protected _Dpi As Integer
    Protected _LoginId As Integer
    Protected _FileNameType As Integer
    Protected _DupType As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(SettingId As Integer)
        Me._SettingId = SettingId
    End Sub

    Public Property Colour As Boolean Implements IeZScanSettings.Colour
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Colour
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _Colour = value Then
                Return
            End If
            _Colour = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZScanSettings.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZScanSettings.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZScanSettings.CreatedOn
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

    Public Property Dpi As Integer Implements IeZScanSettings.Dpi
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Dpi
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Dpi = value Then
                Return
            End If
            _Dpi = value
            IsModified = True
        End Set
    End Property

    Public Property Dublex As Boolean Implements IeZScanSettings.Dublex
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Dublex
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _Dublex = value Then
                Return
            End If
            _Dublex = value
            IsModified = True
        End Set
    End Property

    Public Property DupType As Integer Implements IeZScanSettings.DupType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DupType
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _DupType = value Then
                Return
            End If
            _DupType = value
            IsModified = True
        End Set
    End Property

    Public Property FileName As String Implements IeZScanSettings.FileName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FileName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FileName = value Then
                Return
            End If
            _FileName = value
            IsModified = True
        End Set
    End Property

    Public Property FileNameType As Integer Implements IeZScanSettings.FileNameType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FileNameType
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FileNameType = value Then
                Return
            End If
            _FileNameType = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZScanSettings.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property LoginId As Integer Implements IeZScanSettings.LoginId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LoginId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LoginId = value Then
                Return
            End If
            _LoginId = value
            IsModified = True
        End Set
    End Property

    Public Property SettingId As Integer Implements IeZScanSettings.SettingId
        Get
            If _SettingId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _SettingId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _SettingId <> 0 AndAlso _SettingId <> value Then
                Throw New MemberAccessException()
            End If
            _SettingId = value
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZScanSettings.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZScanSettings.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZScanSettings.UpdatedOn
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
