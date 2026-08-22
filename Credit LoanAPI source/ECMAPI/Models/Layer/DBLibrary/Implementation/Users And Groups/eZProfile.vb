Imports System.Data
Imports System.Configuration
Imports System.Web
''' <summary>
''' Summary description for ProfileGroup
''' </summary>
Public Class eZProfile
    Inherits IDatabaseCommonItems
    Implements IeZProfile
    Protected _ProfileId As Integer
    Protected _Profile As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CUserName As String
    Protected _CUserCode As String
    Protected _UUserName As String
    Protected _UUserCode As String
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpProfileId As Integer)
        Me._ProfileId = tmpProfileId
    End Sub
    Public Sub New(tmpProfile As String)
        Me._Profile = tmpProfile
    End Sub

    Public Sub New()
    End Sub
    Public Property ProfileId() As Integer Implements IeZProfile.ProfileId
        Get
            If _ProfileId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ProfileId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ProfileId <> 0 AndAlso _ProfileId <> value Then
                Throw New MemberAccessException()
            End If
            _ProfileId = value
        End Set
    End Property

    Public Property Profile() As String Implements IeZProfile.Profile
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Profile
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Profile = value Then
                Return
            End If
            _Profile = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZProfile.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZProfile.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZProfile.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZProfile.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZProfile.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZProfile.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZProfile.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsProfileExist() As Boolean Implements IeZProfile.IsProfileExist
        Get
            Return (ProfileId > 0)
        End Get
    End Property

    'Public Overrides Sub SaveChanges()
    '    DBLayer.DBLInstance.Update(Me, Me._LstProfile)
    'End Sub
End Class
