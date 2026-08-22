Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZFieldAlert
    Inherits IDatabaseCommonItems
    Implements IeZFieldAlert
    Protected _FieldAlertId As Integer
    Protected _FieldAlertDetailId As Integer
    Protected _TemplateId As Integer
    Protected _ConditionId As Integer
    Protected _FieldId As Integer
    Protected _FieldAlertName As String
    Protected _ConditionValue As String
    Protected _Condition As String

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

    Public Sub New(tmpFieldAlertId As Integer)
        Me._FieldAlertId = tmpFieldAlertId
    End Sub
    Public Sub New(tmpFieldAlert As String)
        Me._ConditionValue = tmpFieldAlert
    End Sub
    Public Property FieldAlertName() As String Implements IeZFieldAlert.FieldAlertName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FieldAlertName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FieldAlertName = value Then
                Return
            End If
            _FieldAlertName = value
            IsModified = True
        End Set
    End Property
    Public Sub New()
    End Sub
    Public Property FieldAlertId() As Integer Implements IeZFieldAlert.FieldAlertId
        Get
            If _FieldAlertId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FieldAlertId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FieldAlertId <> 0 AndAlso _FieldAlertId <> value Then
                Throw New MemberAccessException()
            End If
            _FieldAlertId = value
        End Set
    End Property
    Public Property FieldAlertDetailId() As Integer Implements IeZFieldAlert.FieldAlertDetailId
        Get
            If _FieldAlertDetailId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FieldAlertDetailId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FieldAlertDetailId <> 0 AndAlso _FieldAlertDetailId <> value Then
                Throw New MemberAccessException()
            End If
            _FieldAlertDetailId = value
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZFieldAlert.TemplateId
        Get
            If _TemplateId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _TemplateId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _TemplateId <> 0 AndAlso _TemplateId <> value Then
                Throw New MemberAccessException()
            End If
            _TemplateId = value
        End Set
    End Property
    Public Property ConditionId() As Integer Implements IeZFieldAlert.ConditionId
        Get
            If _ConditionId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ConditionId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ConditionId <> 0 AndAlso _ConditionId <> value Then
                Throw New MemberAccessException()
            End If
            _ConditionId = value
        End Set
    End Property
    Public Property FieldId() As Integer Implements IeZFieldAlert.FieldId
        Get
            If _FieldId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FieldId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FieldId <> 0 AndAlso _FieldId <> value Then
                Throw New MemberAccessException()
            End If
            _FieldId = value
        End Set
    End Property
    Public Property ConditionValue() As String Implements IeZFieldAlert.ConditionValue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ConditionValue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ConditionValue = value Then
                Return
            End If
            _ConditionValue = value
            IsModified = True
        End Set
    End Property
    Public Property Condition() As String Implements IeZFieldAlert.Condition
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Condition
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Condition = value Then
                Return
            End If
            _Condition = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZFieldAlert.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZFieldAlert.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZFieldAlert.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZFieldAlert.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZFieldAlert.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZFieldAlert.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZFieldAlert.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsFieldAlertExist() As Boolean Implements IeZFieldAlert.IsFieldAlertExist
        Get
            Return (FieldAlertId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
