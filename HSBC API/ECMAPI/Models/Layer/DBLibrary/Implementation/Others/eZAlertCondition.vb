Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZAlertCondition
    Inherits IDatabaseCommonItems
    Implements IeZAlertCondition
    Protected _AlertConditionId As Integer
    Protected _AlertCondition As String
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

    Public Sub New(tmpAlertConditionId As Integer)
        Me._AlertConditionId = tmpAlertConditionId
    End Sub
    Public Sub New(tmpAlertCondition As String)
        Me._AlertCondition = tmpAlertCondition
    End Sub

    Public Sub New()
    End Sub
    Public Property AlertConditionId() As Integer Implements IeZAlertCondition.AlertConditionId
        Get
            If _AlertConditionId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _AlertConditionId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _AlertConditionId <> 0 AndAlso _AlertConditionId <> value Then
                Throw New MemberAccessException()
            End If
            _AlertConditionId = value
        End Set
    End Property

    Public Property AlertCondition() As String Implements IeZAlertCondition.AlertCondition
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _AlertCondition
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _AlertCondition = value Then
                Return
            End If
            _AlertCondition = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZAlertCondition.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZAlertCondition.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZAlertCondition.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZAlertCondition.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZAlertCondition.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZAlertCondition.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZAlertCondition.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsAlertConditionExist() As Boolean Implements IeZAlertCondition.IsAlertConditionExist
        Get
            Return (AlertConditionId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
