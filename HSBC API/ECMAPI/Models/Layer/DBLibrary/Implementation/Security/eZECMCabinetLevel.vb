Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI

Public Class eZECMCabinetLevel
    Inherits IDatabaseCommonItems
    Implements IeZECMCabinetLevel
    Protected _Encrypt As Integer
    Protected _ECMCabinetLevelId As Integer
    Protected _ECMLoginId As Integer
    Protected _LoginName As String
    Protected _TemplateId As Integer
    Protected _Template As String
    Protected _CabinetId As Integer
    Protected _ECMCabinet As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._ECMCabinetLevelId = DeptId
    End Sub
    Public Sub New(ECMCabinetLevelName As String)
        Me._ECMLoginId = ECMCabinetLevelName.Trim()
    End Sub
    Public Sub New()
    End Sub
    Public Property TemplateId() As Integer Implements IeZECMCabinetLevel.TemplateId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TemplateId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TemplateId = value Then
                Return
            End If
            _TemplateId = value
            IsModified = True
        End Set
    End Property

    Public Property Template() As String Implements IeZECMCabinetLevel.Template
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Template
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Template = value Then
                Return
            End If
            _Template = value
            IsModified = True
        End Set
    End Property

    Public Property ECMCabinetLevelId() As Integer Implements IeZECMCabinetLevel.ECMCabinetLevelId
        Get
            If _ECMCabinetLevelId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ECMCabinetLevelId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ECMCabinetLevelId <> 0 AndAlso _ECMCabinetLevelId <> value Then
                Throw New MemberAccessException()
            End If
            _ECMCabinetLevelId = value
        End Set
    End Property
    Public Property CabinetId() As Integer Implements IeZECMCabinetLevel.CabinetId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabinetId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CabinetId = value Then
                Return
            End If
            _CabinetId = value
            IsModified = True
        End Set
    End Property
    Public Property ECMLoginId() As Integer Implements IeZECMCabinetLevel.ECMLoginId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMLoginId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMLoginId = value Then
                Return
            End If
            _ECMLoginId = value
            IsModified = True
        End Set
    End Property
    Public Property Cabinet() As String Implements IeZECMCabinetLevel.Cabinet
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMCabinet
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ECMCabinet = value Then
                Return
            End If
            _ECMCabinet = value
            IsModified = True
        End Set
    End Property
    Public Property LoginName() As String Implements IeZECMCabinetLevel.LoginName
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
    Public Property UpdatedBy1() As String Implements IeZECMCabinetLevel.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZECMCabinetLevel.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZECMCabinetLevel.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZECMCabinetLevel.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZECMCabinetLevel.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZECMCabinetLevel.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZECMCabinetLevel.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZECMCabinetLeveltExist() As Boolean Implements IeZECMCabinetLevel.IseZECMCabinetLevelExist
        Get
            Return (_ECMCabinetLevelId > 0)
        End Get
    End Property

    Public Property Encrypt() As Integer Implements IeZECMCabinetLevel.Encrypt
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Encrypt
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Encrypt = value Then
                Return
            End If
            _Encrypt = value
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
