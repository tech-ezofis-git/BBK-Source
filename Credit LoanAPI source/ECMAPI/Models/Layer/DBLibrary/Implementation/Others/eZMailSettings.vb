
Imports ECMAPI

Public Class eZMailSettings
    Inherits IDatabaseCommonItems
    Implements IeZMailSettings

    Protected _SettingId As Integer
    Protected _SettingName As String = ""
    Protected _IncomingServer As String = ""
    Protected _EmailId As String = ""
    Protected _UserName As String = ""
    Protected _Password As String = ""
    Protected _IncomingPort As Integer
    Protected _EnableSSL As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Protected _Preference As Integer
    Protected _OutgoingServer As String = ""
    Protected _OutgoingPort As Integer
    Protected _LogoPath As String = ""
    Protected _Signature As String = ""

    Public Sub New(SettingId As Integer)
        Me._SettingId = SettingId
    End Sub
    Public Sub New()
    End Sub
    Public Property CreatedBy As Integer Implements IeZMailSettings.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZMailSettings.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZMailSettings.CreatedOn
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

    Public Property EmailId As String Implements IeZMailSettings.EmailId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EmailId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _EmailId = value Then
                Return
            End If

            _EmailId = value
            IsModified = True
        End Set
    End Property

    Public Property EnableSSL As Integer Implements IeZMailSettings.EnableSSL
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EnableSSL
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _EnableSSL = value Then
                Return
            End If

            _EnableSSL = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZMailSettings.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property Password As String Implements IeZMailSettings.Password
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Password
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Password = value Then
                Return
            End If

            _Password = value
            IsModified = True
        End Set
    End Property

    Public Property SettingId As Integer Implements IeZMailSettings.SettingId
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

    Public Property SettingName As String Implements IeZMailSettings.SettingName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SettingName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SettingName = value Then
                Return
            End If

            _SettingName = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZMailSettings.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZMailSettings.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZMailSettings.UpdatedOn
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

    Public Property UserName As String Implements IeZMailSettings.UserName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UserName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UserName = value Then
                Return
            End If

            _UserName = value
            IsModified = True
        End Set
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

    Public Property Preference As Integer Implements IeZMailSettings.Preference
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Preference
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Preference = value Then
                Return
            End If

            _Preference = value
            IsModified = True
        End Set
    End Property

    Public Property OutgoingServer As String Implements IeZMailSettings.OutgoingServer
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _OutgoingServer
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _OutgoingServer = value Then
                Return
            End If

            _OutgoingServer = value
            IsModified = True
        End Set
    End Property

    Public Property OutgoingPort As Integer Implements IeZMailSettings.OutgoingPort
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _OutgoingPort
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _OutgoingPort = value Then
                Return
            End If

            _OutgoingPort = value
            IsModified = True
        End Set
    End Property

    Public Property IncomingServer As String Implements IeZMailSettings.IncomingServer
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IncomingServer
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _IncomingServer = value Then
                Return
            End If

            _IncomingServer = value
            IsModified = True
        End Set
    End Property

    Public Property IncomingPort As Integer Implements IeZMailSettings.IncomingPort
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IncomingPort
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _IncomingPort = value Then
                Return
            End If

            _IncomingPort = value
            IsModified = True
        End Set
    End Property

    Public Property LogoPath As String Implements IeZMailSettings.LogoPath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LogoPath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LogoPath = value Then
                Return
            End If

            _LogoPath = value
            IsModified = True
        End Set
    End Property

    Public Property Signature As String Implements IeZMailSettings.Signature
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Signature
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Signature = value Then
                Return
            End If

            _Signature = value
            IsModified = True
        End Set
    End Property
End Class
