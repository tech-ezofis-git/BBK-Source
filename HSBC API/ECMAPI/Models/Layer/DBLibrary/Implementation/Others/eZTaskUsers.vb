Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZTaskUsers
    Inherits IDatabaseCommonItems
    Implements IeZTaskUsers
    Protected _TaskUsersId As Integer
    Protected _ECMLoginId As Integer
    Protected _OwnerId As Integer
    Protected _OwnerName As String
    Protected _LoginName As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._TaskUsersId = DeptId
    End Sub
    Public Sub New(TaskUsersName As String)
        Me._ECMLoginId = TaskUsersName.Trim()
    End Sub
    Public Sub New()
    End Sub

    Public Property TaskUsersId() As Integer Implements IeZTaskUsers.TaskUsersId
        Get
            If _TaskUsersId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _TaskUsersId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _TaskUsersId <> 0 AndAlso _TaskUsersId <> value Then
                Throw New MemberAccessException()
            End If
            _TaskUsersId = value
        End Set
    End Property
    Public Property OwnerId() As Integer Implements IeZTaskUsers.OwnerId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _OwnerId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _OwnerId = value Then
                Return
            End If
            _OwnerId = value
            IsModified = True
        End Set
    End Property
    Public Property ECMLoginId() As Integer Implements IeZTaskUsers.ECMLoginId
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
    Public Property LoginName() As String Implements IeZTaskUsers.LoginName
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
    Public Property OwnerName() As String Implements IeZTaskUsers.OwnerName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _OwnerName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _OwnerName = value Then
                Return
            End If
            _OwnerName = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZTaskUsers.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZTaskUsers.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZTaskUsers.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZTaskUsers.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZTaskUsers.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZTaskUsers.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZTaskUsers.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZTaskUserstExist() As Boolean Implements IeZTaskUsers.IseZTaskUsersExist
        Get
            Return (_TaskUsersId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
