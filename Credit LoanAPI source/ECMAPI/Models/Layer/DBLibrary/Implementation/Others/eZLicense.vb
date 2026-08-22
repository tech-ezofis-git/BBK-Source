Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZLicense
    Inherits IDatabaseCommonItems
    Implements IeZLicense
    Protected _LicenseId As Integer
    Protected _ApplicationId As Integer = 0
    Protected _ApplicationName As String
    Protected _Key As String
    Protected _NoOfLicense As Integer = 0
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpLicenseId As Integer)
        Me._LicenseId = tmpLicenseId
    End Sub
    Public Sub New()
    End Sub

    Public Property LicenseId() As Integer Implements IeZLicense.LicenseId
        Get
            If _LicenseId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LicenseId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LicenseId <> 0 AndAlso _LicenseId <> value Then
                Throw New MemberAccessException()
            End If
            _LicenseId = value
        End Set
    End Property

    Public Property ApplicationName() As String Implements IeZLicense.ApplicationName
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

    Public Property Key() As String Implements IeZLicense.Key
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Key
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Key = value Then
                Return
            End If
            _Key = value
            IsModified = True
        End Set
    End Property

    Public Property ApplicationId() As Integer Implements IeZLicense.ApplicationId
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
    Public Property NoOfLicense() As Integer Implements IeZLicense.NoOfLicense
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _NoOfLicense
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _NoOfLicense = value Then
                Return
            End If

            _NoOfLicense = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZLicense.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZLicense.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZLicense.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZLicense.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZLicense.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZLicense.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZLicense.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

End Class
