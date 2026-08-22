Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZTask
    Inherits IDatabaseCommonItems
    Implements IeZTask
    Protected _TaskId As Integer
    Protected _Task As String
    Protected _TaskStatus As Integer
    Protected _StartTime As String
    Protected _EndTime As String
    Protected _Templateid As Integer
    Protected _Itemid As Integer
    Protected _TaskPriority As Integer
    Protected _Typeid As Integer
    Protected _Notification As Integer
    Protected _Description As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._TaskId = DeptId
    End Sub
    Public Sub New(TaskName As String)
        Me._Task = TaskName.Trim()
    End Sub
    Public Sub New()
    End Sub
    Public Property StartTime() As String Implements IeZTask.StartTime
        Get

            DBLayer.DBLInstance.Read(Me)
            Return _StartTime
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)

            If _StartTime = value Then
                Return
            End If
            _StartTime = value
            IsModified = True
        End Set
    End Property
    Public Property TaskStatus() As Integer Implements IeZTask.TaskStatus
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TaskStatus
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TaskStatus = value Then
                Return
            End If
            _TaskStatus = value
            IsModified = True
        End Set
    End Property
    Public Property Notification() As Integer Implements IeZTask.Notification
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
    Public Property EndTime() As String Implements IeZTask.EndTime
        Get

            DBLayer.DBLInstance.Read(Me)
            Return _EndTime
        End Get
        Set(value As String)

            DBLayer.DBLInstance.Read(Me)
            If _EndTime = value Then
                Return
            End If
            _EndTime = value
            IsModified = True
        End Set
    End Property

    Public Property templateid() As Integer Implements IeZTask.Templateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Templateid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Templateid = value Then
                Return
            End If
            _Templateid = value
            IsModified = True
        End Set
    End Property
    Public Property itemid() As Integer Implements IeZTask.itemid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Itemid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If itemid = value Then
                Return
            End If
            _Itemid = value
            IsModified = True
        End Set
    End Property

    Public Property TaskPriority As Integer Implements IeZTask.TaskPriority
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TaskPriority
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If TaskPriority = value Then
                Return
            End If
            _TaskPriority = value
            IsModified = True
        End Set
    End Property
    Public Property Typeid As Integer Implements IeZTask.Typeid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Typeid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If Typeid = value Then
                Return
            End If
            _Typeid = value
            IsModified = True
        End Set
    End Property
    Public Property TaskId() As Integer Implements IeZTask.TaskId
        Get
            If _TaskId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _TaskId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _TaskId <> 0 AndAlso _TaskId <> value Then
                Throw New MemberAccessException()
            End If
            _TaskId = value
        End Set
    End Property
    Public Property Task() As String Implements IeZTask.Task
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

    Public Property UpdatedBy1() As String Implements IeZTask.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZTask.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZTask.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZTask.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZTask.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZTask.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZTask.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZTasktExist() As Boolean Implements IeZTask.IseZTaskExist
        Get
            Return (_TaskId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
