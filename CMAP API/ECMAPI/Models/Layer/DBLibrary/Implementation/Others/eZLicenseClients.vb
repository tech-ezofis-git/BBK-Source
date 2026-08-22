Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZLicenseClients
    Inherits IDatabaseCommonItems
    Implements IeZLicenseClients
    Protected _LicenseClientId As Integer
    Protected _LicenseId As Integer
    Protected _ApplicationId As Integer
    Protected _ApplicationName As String
    Protected _ClientName As String = ""
    Protected _MachineCode As String = ""
    Protected _MacInfo As String = ""
    Private _IsActive As Integer
    Protected _Status As String = ""
    Protected _LicenseKey As String = ""
    Protected _TrialDays As Integer
    Protected _InstallOn As String = ""
    Protected _ExpiredOn As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpLicenseClientId As Integer)
        Me._LicenseClientId = tmpLicenseClientId
    End Sub
    Public Sub New()
    End Sub

    Public Property LicenseClientId() As Integer Implements IeZLicenseClients.LicenseClientId
        Get
            If _LicenseClientId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LicenseClientId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LicenseClientId <> 0 AndAlso _LicenseClientId <> value Then
                Throw New MemberAccessException()
            End If
            _LicenseClientId = value
        End Set
    End Property

    Public Property ApplicationName() As String Implements IeZLicenseClients.ApplicationName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ApplicationName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ApplicationName = value Then
                Return
            End If
            _ApplicationName = value
            IsModified = True
        End Set
    End Property

    Public Property Status() As String Implements IeZLicenseClients.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property

    Public Property ClientName() As String Implements IeZLicenseClients.ClientName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ClientName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ClientName = value Then
                Return
            End If
            _ClientName = value
            IsModified = True
        End Set
    End Property

    Public Property MachineCode() As String Implements IeZLicenseClients.MachineCode
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MachineCode
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MachineCode = value Then
                Return
            End If
            _MachineCode = value
            IsModified = True
        End Set
    End Property

    Public Property MacInfo() As String Implements IeZLicenseClients.MacInfo
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MacInfo
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MacInfo = value Then
                Return
            End If
            _MacInfo = value
            IsModified = True
        End Set
    End Property

    Public Property LicenseKey() As String Implements IeZLicenseClients.LicenseKey
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LicenseKey
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LicenseKey = value Then
                Return
            End If
            _LicenseKey = value
            IsModified = True
        End Set
    End Property

    Public Property InstallOn() As String Implements IeZLicenseClients.InstallOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _InstallOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _InstallOn = value Then
                Return
            End If
            _InstallOn = value
            IsModified = True
        End Set
    End Property

    Public Property ExpiredOn() As String Implements IeZLicenseClients.ExpiredOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ExpiredOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ExpiredOn = value Then
                Return
            End If
            _ExpiredOn = value
            IsModified = True
        End Set
    End Property

    Public Property LicenseId() As Integer Implements IeZLicenseClients.LicenseId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LicenseId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LicenseId = value Then
                Return
            End If

            _LicenseId = value
            IsModified = True
        End Set
    End Property

    Public Property ApplicationId() As Integer Implements IeZLicenseClients.ApplicationId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ApplicationId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ApplicationId = value Then
                Return
            End If

            _ApplicationId = value
            IsModified = True
        End Set
    End Property
    Public Property TrialDays() As Integer Implements IeZLicenseClients.TrialDays
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TrialDays
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TrialDays = value Then
                Return
            End If

            _TrialDays = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZLicenseClients.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZLicenseClients.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZLicenseClients.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZLicenseClients.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZLicenseClients.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZLicenseClients.UpdatedOn
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

    Public Property IsActive() As Integer Implements IeZLicenseClients.IsActive
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsActive
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _IsActive = value Then
                Return
            End If

            _IsActive = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted() As Integer Implements IeZLicenseClients.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

End Class
