Imports ECMAPI

Public Class ezEscalationHistory
    Inherits IDatabaseCommonItems
    Implements IezEscalationHistory

    Protected _EscalationHistoryId As Integer
    Protected _EscalationId As Integer
    Protected _WorkflowId As Integer
    Protected _ActivityId As String = ""
    Protected _ActivityName As String = ""
    Protected _ResponseTime As String = ""
    Protected _User As List(Of ezEscalationUser) = Nothing
    Protected _Notification As Boolean
    Protected _ActionFlow As Boolean
    Protected _Createdon As String
    Protected _Updatedon As String
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Protected _ResponseType As String = ""
    Private _isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(EscalationHistoryId As Integer)
        Me._EscalationHistoryId = EscalationHistoryId
    End Sub

    Public Property EscalationHistoryId As Integer Implements IezEscalationHistory.EscalationHistoryId
        Get
            If _EscalationHistoryId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _EscalationHistoryId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _EscalationHistoryId <> 0 AndAlso _EscalationHistoryId <> value Then
                Throw New MemberAccessException()
            End If
            _EscalationHistoryId = value
        End Set
    End Property

    Public Property EscalationId As Integer Implements IezEscalationHistory.EscalationId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EscalationId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _EscalationId = value Then
                Return
            End If
            _EscalationId = value
            IsModified = True
        End Set
    End Property

    Public Property WorkflowId As Integer Implements IezEscalationHistory.WorkflowId
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

    Public Property ActivityId As String Implements IezEscalationHistory.ActivityId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ActivityId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ActivityId = value Then
                Return
            End If
            _ActivityId = value
            IsModified = True
        End Set
    End Property

    Public Property ActivityName As String Implements IezEscalationHistory.ActivityName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ActivityName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ActivityName = value Then
                Return
            End If
            _ActivityName = value
            IsModified = True
        End Set
    End Property

    Public Property ResponseTime As String Implements IezEscalationHistory.ResponseTime
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ResponseTime
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ResponseTime = value Then
                Return
            End If
            _ResponseTime = value
            IsModified = True
        End Set
    End Property

    Public Property ResponseType As String Implements IezEscalationHistory.ResponseType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ResponseType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ResponseType = value Then
                Return
            End If
            _ResponseType = value
            IsModified = True
        End Set
    End Property

    Public Property User As List(Of ezEscalationUser) Implements IezEscalationHistory.User
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _User
        End Get
        Set(value As List(Of ezEscalationUser))
            DBLayer.DBLInstance.Read(Me)
            If _User.Count = value.Count Then
                Return
            End If
            _User = value
            IsModified = True
        End Set
    End Property

    Public Property Notification As Boolean Implements IezEscalationHistory.Notification
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Notification
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _Notification = value Then
                Return
            End If
            _Notification = value
            IsModified = True
        End Set
    End Property

    Public Property ActionFlow As Boolean Implements IezEscalationHistory.ActionFlow
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ActionFlow
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _ActionFlow = value Then
                Return
            End If
            _ActionFlow = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon As String Implements IezEscalationHistory.Createdon
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

    Public Property Updatedon As String Implements IezEscalationHistory.Updatedon
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

    Public Property Createdby As Integer Implements IezEscalationHistory.Createdby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Createdby = value Then
                Return
            End If
            _Createdby = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedby As Integer Implements IezEscalationHistory.Updatedby
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

    Public Property CreatedBy1 As String Implements IezEscalationHistory.CreatedBy1
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

    Public Property UpdatedBy1 As String Implements IezEscalationHistory.UpdatedBy1
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

    Public ReadOnly Property isdeleted As Integer Implements IezEscalationHistory.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
