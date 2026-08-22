Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZTrialLicense
    Inherits IDatabaseCommonItems
    Implements IeZTrialLicense
    Protected _TrialId As Integer
    Protected _LicenseId As Integer
    Protected _LicenseClientId As Integer
    Protected _TrialKey As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpTrialId As Integer)
        Me._TrialId = tmpTrialId
    End Sub
    Public Sub New()
    End Sub

    Public Property TrialId() As Integer Implements IeZTrialLicense.TrialId
        Get
            If _TrialId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _TrialId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _TrialId <> 0 AndAlso _TrialId <> value Then
                Throw New MemberAccessException()
            End If
            _TrialId = value
        End Set
    End Property

    Public Property TrialKey() As String Implements IeZTrialLicense.TrialKey
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TrialKey
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TrialKey = value Then
                Return
            End If
            _TrialKey = value
            IsModified = True
        End Set
    End Property

    Public Property LicenseId() As Integer Implements IeZTrialLicense.LicenseId
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
    Public Property LicenseClientId() As Integer Implements IeZTrialLicense.LicenseClientId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LicenseClientId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LicenseClientId = value Then
                Return
            End If

            _LicenseClientId = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZTrialLicense.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZTrialLicense.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZTrialLicense.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZTrialLicense.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZTrialLicense.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZTrialLicense.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZTrialLicense.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

End Class
