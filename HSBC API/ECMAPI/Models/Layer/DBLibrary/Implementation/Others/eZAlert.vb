Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZAlert
    Inherits IDatabaseCommonItems
    Implements IeZAlert
    Protected _AlertId As Integer
    Protected _AlertConditionId As Integer
    Protected _DocumentAlertId As Integer

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

    Public Sub New(tmpAlertId As Integer)
        Me._AlertId = tmpAlertId
    End Sub
  

    Public Sub New()
    End Sub
    Public Property AlertId() As Integer Implements IeZAlert.AlertId
        Get
            If _AlertId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _AlertId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _AlertId <> 0 AndAlso _AlertId <> value Then
                Throw New MemberAccessException()
            End If
            _AlertId = value
        End Set
    End Property
    Public Property AlertConditionId() As Integer Implements IeZAlert.AlertConditionId
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
    Public Property DocumentAlertId() As Integer Implements IeZAlert.DocumentAlertId
        Get
            If _DocumentAlertId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _DocumentAlertId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _DocumentAlertId <> 0 AndAlso _DocumentAlertId <> value Then
                Throw New MemberAccessException()
            End If
            _DocumentAlertId = value
        End Set
    End Property
   
    Public Property UpdatedBy1() As String Implements IeZAlert.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZAlert.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZAlert.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZAlert.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZAlert.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZAlert.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZAlert.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsAlertExist() As Boolean Implements IeZAlert.IsAlertExist
        Get
            Return (AlertId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
