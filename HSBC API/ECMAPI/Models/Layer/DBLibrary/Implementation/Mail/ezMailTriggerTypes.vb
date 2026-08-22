Imports ECMAPI

Public Class ezMailTriggerTypes
    Inherits IDatabaseCommonItems
    Implements IezMailTriggerTypes


    Protected _TriggerTypeId As Integer
    Protected _TriggerType As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Public Sub New(TriggerTypeId As Integer)
        Me._TriggerTypeId = TriggerTypeId
    End Sub
    Public Sub New()
    End Sub
    Public Property CreatedBy As Integer Implements IezMailTriggerTypes.CreatedBy
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

    Public Property CreatedBy1 As String Implements IezMailTriggerTypes.CreatedBy1
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

    Public Property CreatedOn As String Implements IezMailTriggerTypes.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IezMailTriggerTypes.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property TriggerType As String Implements IezMailTriggerTypes.TriggerType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TriggerType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TriggerType = value Then
                Return
            End If
            _TriggerType = value
            IsModified = True
        End Set
    End Property

    Public Property TriggerTypeId As Integer Implements IezMailTriggerTypes.TriggerTypeId
        Get
            If _TriggerTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _TriggerTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _TriggerTypeId <> 0 AndAlso _TriggerTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _TriggerTypeId = value
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IezMailTriggerTypes.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IezMailTriggerTypes.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IezMailTriggerTypes.UpdatedOn
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

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
