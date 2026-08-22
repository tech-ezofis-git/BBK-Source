Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI

Public Class eZECMFieldLevel
    Inherits IDatabaseCommonItems
    Implements IeZECMFieldLevel

    Protected _ECMGroupId As Integer
    Protected _Visibility As Integer
    Protected _TemplateId As Integer
    Protected _ConditionId As Integer
    Protected _ECMFieldLevelId As Integer
    Protected _ECMLoginId As Integer
    Protected _FieldId As Integer
    Protected _FieldValue As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Protected _LoginName As String = ""
    Public Sub New(DeptId As Integer)
        Me._ECMFieldLevelId = DeptId
    End Sub
    Public Sub New(ECMFieldLevelName As String)
        Me._ECMLoginId = ECMFieldLevelName.Trim()
    End Sub
    Public Sub New()
    End Sub
    Public Property LoginName() As String Implements IeZECMFieldLevel.LoginName
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
    Public Property ECMFieldLevelId() As Integer Implements IeZECMFieldLevel.ECMFieldLevelId
        Get
            If _ECMFieldLevelId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ECMFieldLevelId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ECMFieldLevelId <> 0 AndAlso _ECMFieldLevelId <> value Then
                Throw New MemberAccessException()
            End If
            _ECMFieldLevelId = value
        End Set
    End Property
    Public Property FieldId() As Integer Implements IeZECMFieldLevel.FieldId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FieldId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FieldId = value Then
                Return
            End If
            _FieldId = value
            IsModified = True
        End Set
    End Property
    Public Property ECMLoginId() As Integer Implements IeZECMFieldLevel.ECMLoginId
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

    Public Property FieldValue() As String Implements IeZECMFieldLevel.FieldValue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FieldValue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FieldValue = value Then
                Return
            End If
            _FieldValue = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZECMFieldLevel.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZECMFieldLevel.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZECMFieldLevel.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZECMFieldLevel.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZECMFieldLevel.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZECMFieldLevel.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZECMFieldLevel.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property


    Public Property Visibility() As Integer Implements IeZECMFieldLevel.Visibility
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Visibility
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Visibility = value Then
                Return
            End If

            _Visibility = value
        End Set
    End Property

    Public Property TemplateId() As Integer Implements IeZECMFieldLevel.TemplateId
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
        End Set
    End Property

    Public Property ConditionId() As Integer Implements IeZECMFieldLevel.ConditionId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ConditionId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ConditionId = value Then
                Return
            End If

            _ConditionId = value
        End Set
    End Property

    Public Property ECMGroupId As Integer Implements IeZECMFieldLevel.ECMGroupId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMGroupId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMGroupId = value Then
                Return
            End If

            _ECMGroupId = value
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
