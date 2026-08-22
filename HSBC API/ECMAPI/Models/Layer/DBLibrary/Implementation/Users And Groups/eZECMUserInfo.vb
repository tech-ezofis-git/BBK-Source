Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI

Public Class eZECMUserInfo
    Inherits IDatabaseCommonItems
    Implements IeZECMUserInfo

    Protected _UserId As Integer
    Protected _ECMLoginId As String
    Protected _FirstName As String = ""
    Protected _Mobile As String = ""
    Protected _EmailAddress As String = ""
    Protected _Designation As String = ""
    Protected _Department As String = ""
    Protected _ManagerName As String = ""
    Protected _Manager As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer
    Public Sub New(tmpUserId As Integer)
        Me._UserId = tmpUserId
    End Sub
    Public Sub New()
    End Sub
    Public Property FirstName() As String Implements IeZECMUserInfo.FirstName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FirstName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FirstName = value Then
                Return
            End If
            _FirstName = value
            IsModified = True
        End Set
    End Property
    Public Property Designation() As String Implements IeZECMUserInfo.Designation
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Designation
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Designation = value Then
                Return
            End If
            _Designation = value
            IsModified = True
        End Set
    End Property
    Public Property Department() As String Implements IeZECMUserInfo.Department
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Department
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Department = value Then
                Return
            End If
            _Department = value
            IsModified = True
        End Set
    End Property

    Public Property UserId() As Integer Implements IeZECMUserInfo.UserId
        Get
            If _UserId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _UserId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _UserId <> 0 AndAlso _UserId <> value Then
                Throw New MemberAccessException()
            End If
            _UserId = value
        End Set
    End Property
    Public Property ECMLoginId() As String Implements IeZECMUserInfo.ECMLoginId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMLoginId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ECMLoginId = value Then
                Return
            End If
            _ECMLoginId = value
            IsModified = True
        End Set
    End Property


    Public Property Mobile() As String Implements IeZECMUserInfo.Mobile
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Mobile
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Mobile = value Then
                Return
            End If
            _Mobile = value
            IsModified = True
        End Set
    End Property
    Public Property EmailAddress() As String Implements IeZECMUserInfo.EmailAddress
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EmailAddress
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _EmailAddress = value Then
                Return
            End If
            _EmailAddress = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZECMUserInfo.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZECMUserInfo.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZECMUserInfo.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZECMUserInfo.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZECMUserInfo.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZECMUserInfo.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZECMUserInfo.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsEmployeeExist() As Boolean Implements IeZECMUserInfo.IsEmployeeExist
        Get
            Return (_UserId > 0)
        End Get
    End Property

    Public Property Manager As Integer Implements IeZECMUserInfo.Manager
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Manager
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Manager = value Then
                Return
            End If
            _Manager = value
            IsModified = True
        End Set
    End Property

    Public Property ManagerName As String Implements IeZECMUserInfo.ManagerName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ManagerName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ManagerName = value Then
                Return
            End If
            _ManagerName = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

End Class
