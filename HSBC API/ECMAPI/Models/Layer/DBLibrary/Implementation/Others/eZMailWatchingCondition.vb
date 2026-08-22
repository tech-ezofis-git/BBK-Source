
Imports ECMAPI

Public Class eZMailWatchingCondition
    Inherits IDatabaseCommonItems
    Implements IeZMailWatchingCondition

    Protected IM_conditionid As Integer
    Protected IM_Condition As Integer
    Protected IM_Createdon As String
    Protected IM_Updatedon As String
    Protected IM_Createdby As Integer
    Protected IM_Updatedby As Integer
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Public Sub New(conditionid As Integer)
        Me.IM_conditionid = conditionid
    End Sub
    Public Sub New()

    End Sub
    Public Property conditionid As Integer Implements IeZMailWatchingCondition.conditionid
        Get
            If conditionid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return IM_conditionid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If IM_conditionid <> 0 AndAlso IM_conditionid <> value Then
                Throw New MemberAccessException()
            End If
            IM_conditionid = value
        End Set
    End Property
    Public Property condition As String Implements IeZMailWatchingCondition.condition

        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Condition
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_Condition = value Then
                Return
            End If
            IM_Condition = value
            IsModified = True
        End Set
    End Property

    Public Property createdby As Integer Implements IeZMailWatchingCondition.createdby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Createdby = value Then
                Return
            End If
            IM_Createdby = value
            IsModified = True
        End Set
    End Property

    Public Property createdon As String Implements IeZMailWatchingCondition.createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_Createdon = value Then
                Return
            End If
            IM_Createdon = value
            IsModified = True

        End Set
    End Property


    Public Property updatedby As Integer Implements IeZMailWatchingCondition.updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Updatedby = value Then
                Return
            End If
            IM_Updatedby = value
            IsModified = True

        End Set
    End Property

    Public Property updatedon As String Implements IeZMailWatchingCondition.updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_Updatedon = value Then
                Return
            End If
            IM_Updatedon = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZMailWatchingCondition.CreatedBy1
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

    Public Property UpdatedBy1 As String Implements IeZMailWatchingCondition.UpdatedBy1
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

    Public ReadOnly Property isdeleted As Integer Implements IeZMailWatchingCondition.isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
