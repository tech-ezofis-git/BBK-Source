Imports ECMAPI

Public Class eZWorkflowUsers
    Inherits IDatabaseCommonItems
    Implements IeZWorkflowUsers

    Protected _WorkflowUsersId As Integer
    Protected _WorkflowId As Integer
    Protected _ECMLoginId As Integer
    Protected _ECMGroupId As Integer
    Protected _AssignedFrom As String = ""
    Protected _Createdon As String = ""
    Protected _Updatedon As String = ""
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Protected _FormId As Integer
    Protected _UserType As String = ""
    Private _isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(workflowusersid As Integer)
        Me._WorkflowUsersId = workflowusersid
    End Sub

    Public Property Createdby() As Integer Implements IeZWorkflowUsers.Createdby
        Get
            If _Createdby = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Createdby
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Createdby <> 0 AndAlso _Createdby <> value Then
                Throw New MemberAccessException()
            End If
            _Createdby = value
        End Set
    End Property

    Public Property Createdby1() As String Implements IeZWorkflowUsers.Createdby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Createdby1 = value Then
                Return
            End If
            _Createdby1 = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon() As String Implements IeZWorkflowUsers.Createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Createdon = value Then
                Return
            End If
            _Createdon = value
            IsModified = True
        End Set
    End Property

    Public Property ECMLoginId() As Integer Implements IeZWorkflowUsers.ECMLoginId
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

    Public ReadOnly Property isdeleted() As Integer Implements IeZWorkflowUsers.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Property Updatedby() As Integer Implements IeZWorkflowUsers.Updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedby = value Then
                Return
            End If
            _Updatedby = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedby1() As String Implements IeZWorkflowUsers.Updatedby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedby1 = value Then
                Return
            End If
            _Updatedby1 = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedon() As String Implements IeZWorkflowUsers.Updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedon = value Then
                Return
            End If
            _Updatedon = value
            IsModified = True
        End Set
    End Property

    Public Property WorkflowId() As Integer Implements IeZWorkflowUsers.WorkflowId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _WorkflowId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _WorkflowId = value Then
                Return
            End If
            _WorkflowId = value
            IsModified = True
        End Set
    End Property

    Public Property WorkflowUsersId() As Integer Implements IeZWorkflowUsers.WorkflowUsersId
        Get
            If _WorkflowUsersId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _WorkflowUsersId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _WorkflowUsersId <> 0 AndAlso _WorkflowUsersId <> value Then
                Throw New MemberAccessException()
            End If
            _WorkflowUsersId = value
        End Set
    End Property

    Public Property ECMGroupId As Integer Implements IeZWorkflowUsers.ECMGroupId
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
            IsModified = True
        End Set
    End Property

    Public Property AssignedFrom As String Implements IeZWorkflowUsers.AssignedFrom
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _AssignedFrom
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _AssignedFrom = value Then
                Return
            End If
            _AssignedFrom = value
            IsModified = True
        End Set
    End Property

    Public Property UserType As String Implements IeZWorkflowUsers.UserType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UserType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UserType = value Then
                Return
            End If
            _UserType = value
            IsModified = True
        End Set
    End Property

    Public Property FormId As Integer Implements IeZWorkflowUsers.FormId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FormId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FormId = value Then
                Return
            End If
            _FormId = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
