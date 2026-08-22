Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZAllottedTask
    Inherits IDatabaseCommonItems
    Implements IeZAllottedTask
    Protected _AllottedTaskId As Integer
    Protected _ECMLoginId As Integer
    Protected _TaskId As Integer
    Protected _Task As String
    Protected _LoginName As String
    Protected _Status As String
    Protected _Notification As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._AllottedTaskId = DeptId
    End Sub
    Public Sub New(AllottedTaskName As String)
        Me._ECMLoginId = AllottedTaskName.Trim()
    End Sub
    Public Sub New()
    End Sub
  
    Public Property AllottedTaskId() As Integer Implements IeZAllottedTask.AllottedTaskId
        Get
            If _AllottedTaskId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _AllottedTaskId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _AllottedTaskId <> 0 AndAlso _AllottedTaskId <> value Then
                Throw New MemberAccessException()
            End If
            _AllottedTaskId = value
        End Set
    End Property
   
    Public Property TaskId() As Integer Implements IeZAllottedTask.TaskId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TaskId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TaskId = value Then
                Return
            End If
            _TaskId = value
            IsModified = True
        End Set
    End Property
    Public Property ECMLoginId() As Integer Implements IeZAllottedTask.ECMLoginId
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
    Public Property Notification() As Integer Implements IeZAllottedTask.Notification
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Notification
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Notification = value Then
                Return
            End If
            _Notification = value
            IsModified = True
        End Set
    End Property
    Public Property LoginName() As String Implements IeZAllottedTask.LoginName
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
    Public Property status() As String Implements IeZAllottedTask.status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property
    Public Property Task() As String Implements IeZAllottedTask.Task
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Task
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Task = value Then
                Return
            End If
            _Task = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZAllottedTask.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZAllottedTask.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZAllottedTask.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZAllottedTask.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZAllottedTask.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZAllottedTask.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZAllottedTask.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZAllottedTasktExist() As Boolean Implements IeZAllottedTask.IseZAllottedTaskExist
        Get
            Return (_AllottedTaskId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
