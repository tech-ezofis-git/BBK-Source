Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZECMProfile
    Inherits IDatabaseCommonItems
    Implements IeZECMProfile
    Protected _ECMProfileId As Integer
    Protected _ECMProfile As String
    Protected _Description As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._ECMProfileId = DeptId
    End Sub
    Public Sub New(ECMProfileName As String)
        Me._ECMProfile = ECMProfileName.Trim()
    End Sub
    Public Sub New()
    End Sub

    Public Property ECMProfileId() As Integer Implements IeZECMProfile.ECMProfileId
        Get
            If _ECMProfileId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ECMProfileId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ECMProfileId <> 0 AndAlso _ECMProfileId <> value Then
                Throw New MemberAccessException()
            End If
            _ECMProfileId = value
        End Set
    End Property
    Public Property ECMProfile() As String Implements IeZECMProfile.ECMProfile
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMProfile
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ECMProfile = value Then
                Return
            End If
            _ECMProfile = value
            IsModified = True
        End Set
    End Property
    Public Property Description() As String Implements IeZECMProfile.Description
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Description
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Description = value Then
                Return
            End If
            _Description = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZECMProfile.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZECMProfile.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZECMProfile.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZECMProfile.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZECMProfile.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZECMProfile.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZECMProfile.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZECMProfiletExist() As Boolean Implements IeZECMProfile.IseZECMProfileExist
        Get
            Return (_ECMProfileId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
