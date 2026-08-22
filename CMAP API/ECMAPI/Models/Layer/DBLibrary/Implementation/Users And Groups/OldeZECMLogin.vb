Imports System.Data
Imports System.Configuration
Imports System.Web

''' <summary>
''' Summary description for Login
''' </summary>
Public Class OldeZECMLogin
    Inherits IDatabaseCommonItems
    Implements IOldeZECMLogin
    Protected _ECMLoginId As Integer
    'Protected _ECMGroupId As Integer
    Protected _ECMUserTypeId As Integer
    Protected _IsFaxUser As Boolean
    Protected _LanguageId As Integer
    Protected _Chart1 As Integer
    Protected _Chart2 As Integer
    Protected _Chart3 As Integer
    Protected _IsADUser As Boolean
    Protected _LoginName As String
    'Protected _ECMGroup As String
    Protected _Signatureid As String = ""
    Protected _ECMProfileId As Integer
    Protected _ECMGroupList As String = "0"
    Protected _ECMProfile As String
    Protected _Pasword As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer = 0
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String = 0
    Protected _Isdeleted As Integer
    Public Sub New(tmpLogin_ID As Integer)
        Me._ECMLoginId = tmpLogin_ID
    End Sub
    Public Sub New()
    End Sub
    Public Sub New(tmpUserName As String, tmpPassword As String)
        Me._LoginName = tmpUserName.Trim()
        Me._Pasword = tmpPassword.Trim()
    End Sub
    Public Property ECMLoginId() As Integer Implements IOldeZECMLogin.ECMLoginId
        Get
            If _ECMLoginId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ECMLoginId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ECMLoginId <> 0 AndAlso _ECMLoginId <> value Then
                Throw New MemberAccessException()
            End If
            _ECMLoginId = value
        End Set
    End Property
    Public Property Signatureid() As String Implements IOldeZECMLogin.Signatureid
        Get

            DBLayer.DBLInstance.Read(Me)
            Return _Signatureid
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Signatureid = value Then
                Return
            End If
            _Signatureid = value
            IsModified = True
        End Set
    End Property
    Public Property ECMProfile() As String Implements IOldeZECMLogin.ECMProfile
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


    Public Property ECMGroupList() As String Implements IOldeZECMLogin.ECMGroupList
        Get

            DBLayer.DBLInstance.Read(Me)
            Return _ECMGroupList
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ECMGroupList = value Then
                Return
            End If
            _ECMGroupList = value
            IsModified = True
        End Set
    End Property

    Public Property ECMProfileId() As Integer Implements IOldeZECMLogin.ECMProfileId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMProfileId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMProfileId = value Then
                Return
            End If

            _ECMProfileId = value
            IsModified = True
        End Set
    End Property
    Public Property ECMUserTypeId() As Integer Implements IOldeZECMLogin.ECMUserTypeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMUserTypeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMUserTypeId = value Then
                Return
            End If

            _ECMUserTypeId = value
            IsModified = True
        End Set
    End Property
    Public Property Chart1() As Integer Implements IOldeZECMLogin.Chart1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Chart1
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Chart1 = value Then
                Return
            End If

            _Chart1 = value
            IsModified = True
        End Set
    End Property
    Public Property Chart2() As Integer Implements IOldeZECMLogin.Chart2
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Chart2
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Chart2 = value Then
                Return
            End If

            _Chart2 = value
            IsModified = True
        End Set
    End Property
    Public Property Chart3() As Integer Implements IOldeZECMLogin.Chart3
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Chart3
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Chart3 = value Then
                Return
            End If

            _Chart3 = value
            IsModified = True
        End Set
    End Property
    Public Property LanguageId() As Integer Implements IOldeZECMLogin.LanguageId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LanguageId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LanguageId = value Then
                Return
            End If

            _LanguageId = value
            IsModified = True
        End Set
    End Property
    Public Property IsADUser() As Boolean Implements IOldeZECMLogin.IsADUser
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsADUser
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsADUser = value Then
                Return
            End If
            _IsADUser = value
            IsModified = True
        End Set
    End Property
    Public Property IsFaxUser() As Boolean Implements IOldeZECMLogin.IsFaxUser
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsFaxUser
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsFaxUser = value Then
                Return
            End If
            _IsFaxUser = value
            IsModified = True
        End Set
    End Property
    'Public Property ECMGroup() As String Implements IOldeZECMLogin.ECMGroup
    '    Get
    '        DBLayer.DBLInstance.Read(Me)
    '        Return _ECMGroup
    '    End Get
    '    Set(value As String)
    '        DBLayer.DBLInstance.Read(Me)
    '        If _ECMGroup = value Then
    '            Return
    '        End If
    '        _ECMGroup = value
    '        IsModified = True
    '    End Set
    'End Property
    Public Property LoginName() As String Implements IOldeZECMLogin.LoginName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LoginName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LoginName = value Then
                Return
            End If

            _LoginName = value
            IsModified = True
        End Set
    End Property
    'Public Property ECMGroupId() As Integer Implements IOldeZECMLogin.ECMGroupId
    '    Get
    '        DBLayer.DBLInstance.Read(Me)
    '        Return _ECMGroupId
    '    End Get
    '    Set(value As Integer)
    '        DBLayer.DBLInstance.Read(Me)
    '        If _ECMGroupId = value Then
    '            Return
    '        End If

    '        _ECMGroupId = value
    '        IsModified = True
    '    End Set
    'End Property

    Public Property Pasword() As String Implements IOldeZECMLogin.Pasword
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Pasword
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Pasword = value Then
                Return
            End If

            _Pasword = value
            IsModified = True
        End Set
    End Property


    Public Property CreatedBy() As Integer Implements IOldeZECMLogin.CreatedBy
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
    Public Property CreatedOn() As String Implements IOldeZECMLogin.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IOldeZECMLogin.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IOldeZECMLogin.UpdatedOn
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
    Public Property UpdatedBy1() As String Implements IOldeZECMLogin.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IOldeZECMLogin.CreatedBy1
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
    Public ReadOnly Property Isdeleted() As Integer Implements IOldeZECMLogin.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsECMLoginExist() As Boolean Implements IOldeZECMLogin.IsECMLoginExist
        Get
            Return (ECMLoginId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

End Class
