
Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZLicenseMaintenance
    Inherits IDatabaseCommonItems
    Implements IeZLicenseMaintenance

    Protected I_Maintenance_Id As Integer
    Protected I_Client_Name As String
    Protected I_License_Key As String
    Protected I_Keytype As String
    Protected I_Created_On As String = ""
    Protected I_Created_by As Integer = 0
    Protected I_Createdby1 As Integer
    Protected I_Updated_On As String = ""
    Protected I_Updated_by As Integer = 0
    Protected I_updatedby1 As Integer
    Private I_isdeleted As Integer = 0

    Public Sub New(tmpMaintenance_Id As Integer)
        Me.I_Maintenance_Id = Maintenance_Id
    End Sub


    Public Sub New()
    End Sub

    Public Property Client_Name As String Implements IeZLicenseMaintenance.Client_Name
        Get
            DBLayer.DBLInstance.Read(Me)
            Return I_Client_Name
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If I_Client_Name = value Then
                Return
            End If

            I_Client_Name = value
            IsModified = True
        End Set
    End Property

    Public Property Created_by As Integer Implements IeZLicenseMaintenance.Created_by
        Get
            DBLayer.DBLInstance.Read(Me)
            Return I_Created_by
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If I_Created_by = value Then
                Return
            End If
            I_Created_by = value
            IsModified = True
        End Set
    End Property
    Public Property Created_On As String Implements IeZLicenseMaintenance.Created_On
        Get
            DBLayer.DBLInstance.Read(Me)
            Return I_Created_On
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If I_Created_On = value Then
                Return
            End If

            I_Created_On = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements IeZLicenseMaintenance.isdeleted
        Get
            Return I_isdeleted
        End Get
    End Property

    Public Property Keytype As String Implements IeZLicenseMaintenance.Keytype
        Get
            DBLayer.DBLInstance.Read(Me)
            Return I_Keytype
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If I_Keytype = value Then
                Return
            End If

            I_Keytype = value
            IsModified = True
        End Set
    End Property

    Public Property License_Key As String Implements IeZLicenseMaintenance.License_Key
        Get
            DBLayer.DBLInstance.Read(Me)
            Return I_License_Key
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If I_License_Key = value Then
                Return
            End If

            I_License_Key = value
            IsModified = True
        End Set
    End Property

    Public Property Maintenance_Id As Integer Implements IeZLicenseMaintenance.Maintenance_Id
        Get
            If I_Maintenance_Id = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return I_Maintenance_Id
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If I_Maintenance_Id <> 0 AndAlso I_Maintenance_Id <> value Then
                Throw New MemberAccessException()
            End If
            I_Maintenance_Id = value
        End Set
    End Property
    Public Property createdby1 As Integer Implements IeZLicenseMaintenance.createdby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return I_Createdby1
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If I_Createdby1 = value Then
                Return
            End If
            I_Createdby1 = value
            IsModified = True
        End Set
    End Property
    Public Property updatedby1 As Integer Implements IeZLicenseMaintenance.updatedby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return I_updatedby1
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If I_updatedby1 = value Then
                Return
            End If
            I_updatedby1 = value
            IsModified = True
        End Set
    End Property
    Public Property Updated_by As Integer Implements IeZLicenseMaintenance.Updated_by
        Get
            DBLayer.DBLInstance.Read(Me)
            Return I_Updated_by
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If I_Updated_by = value Then
                Return
            End If
            I_Updated_by = value
            IsModified = True
        End Set
    End Property
    Public Property Updated_On As String Implements IeZLicenseMaintenance.Updated_On
        Get
            DBLayer.DBLInstance.Read(Me)
            Return I_Updated_On
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If I_Updated_On = value Then
                Return
            End If

            I_Updated_On = value
            IsModified = True
        End Set
    End Property
End Class
