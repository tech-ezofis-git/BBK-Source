Imports ECMAPI

Public Class eZClient
    Inherits IDatabaseCommonItems
    Implements IeZClient

    Protected _ClientId As Integer
    Protected _ClientName As String = ""
    Protected _Address As String = ""
    Protected _City As String = ""
    Protected _Country As String = ""
    Protected _ContactPerson As String = ""
    Protected _ContactNo As String = ""
    Protected _EmailId As String = ""
    Protected _ReferenceFrom As String = ""
    Protected _InstalledDate As String = ""
    Protected _LastAMC As String = ""
    Protected _AMCDate As String = ""
    Protected _Logo As String = ""
    Protected _LicenseType As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer


    Public Sub New()
    End Sub
    Public Sub New(ClientId As Integer)
        Me._ClientId = ClientId
    End Sub
    Public Property Address As String Implements IeZClient.Address
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Address
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Address = value Then
                Return
            End If
            _Address = value
            IsModified = True
        End Set
    End Property

    Public Property AMCDate As String Implements IeZClient.AMCDate
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _AMCDate
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _AMCDate = value Then
                Return
            End If
            _AMCDate = value
            IsModified = True
        End Set
    End Property

    Public Property City As String Implements IeZClient.City
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _City
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _City = value Then
                Return
            End If
            _City = value
            IsModified = True
        End Set
    End Property

    Public Property ClientId As Integer Implements IeZClient.ClientId
        Get
            If _ClientId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ClientId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ClientId <> 0 AndAlso _ClientId <> value Then
                Throw New MemberAccessException()
            End If
            _ClientId = value
        End Set
    End Property

    Public Property ClientName As String Implements IeZClient.ClientName
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

    Public Property ContactNo As String Implements IeZClient.ContactNo
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ContactNo
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ContactNo = value Then
                Return
            End If
            _ContactNo = value
            IsModified = True
        End Set
    End Property

    Public Property ContactPerson As String Implements IeZClient.ContactPerson
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ContactPerson
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ContactPerson = value Then
                Return
            End If
            _ContactPerson = value
            IsModified = True
        End Set
    End Property

    Public Property Country As String Implements IeZClient.Country
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Country
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Country = value Then
                Return
            End If
            _Country = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZClient.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZClient.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZClient.CreatedOn
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

    Public Property EmailId As String Implements IeZClient.EmailId
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

    Public Property InstalledDate As String Implements IeZClient.InstalledDate
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _InstalledDate
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _InstalledDate = value Then
                Return
            End If
            _InstalledDate = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZClient.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property LastAMC As String Implements IeZClient.LastAMC
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LastAMC
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LastAMC = value Then
                Return
            End If
            _LastAMC = value
            IsModified = True
        End Set
    End Property

    Public Property LicenseType As String Implements IeZClient.LicenseType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LicenseType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LicenseType = value Then
                Return
            End If
            _LicenseType = value
            IsModified = True
        End Set
    End Property

    Public Property Logo As String Implements IeZClient.Logo
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Logo
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Logo = value Then
                Return
            End If
            _Logo = value
            IsModified = True
        End Set
    End Property

    Public Property ReferenceFrom As String Implements IeZClient.ReferenceFrom
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ReferenceFrom
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ReferenceFrom = value Then
                Return
            End If
            _ReferenceFrom = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZClient.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZClient.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZClient.UpdatedOn
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
