
Imports ECMAPI

Public Class eZMailWatching
    Inherits IDatabaseCommonItems
    Implements IeZMailWatching


    Protected IM_mailwatchingid As Integer
    Protected IM_Watchingmail As String = ""
    Protected IM_WatchingMailPWD As String = ""
    Protected IM_Conditionid As Integer
    Protected IM_WatchingTime As String = ""
    Protected IM_WatchingStatus As String = ""
    Protected IM_port As String = ""
    Protected IM_SMTP As String = ""
    Protected IM_Createdon As String = ""
    Protected IM_Updatedon As String = ""
    Protected _createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Protected IM_createdby As Integer
    Protected IM_Updatedby As Integer
    Private IM_Isdeleted As Integer

    Public Sub New(Mailwatchingid As Integer)
        Me.IM_mailwatchingid = Mailwatchingid
    End Sub
    Public Sub New()

    End Sub

    Public Property mailwatchingid As Integer Implements IeZMailWatching.mailwatchingid
        Get
            If IM_mailwatchingid = 0 Then
                DBLayer.DBLInstance.read(Me)
            End If
            Return IM_mailwatchingid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.read(Me)
            End If
            If IM_mailwatchingid <> 0 AndAlso IM_mailwatchingid <> value Then
                Throw New MemberAccessException()
            End If
            IM_mailwatchingid = value
        End Set
    End Property
    Public Property Watchingmail As String Implements IeZMailWatching.Watchingmail
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_Watchingmail
        End Get
        Set(value As String)
            DBLayer.DBLInstance.read(Me)
            If IM_Watchingmail = value Then
                Return
            End If
            IM_Watchingmail = value
            IsModified = True
        End Set
    End Property
    Public Property WatchingMailPWD As String Implements IeZMailWatching.WatchingMailPWD
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_WatchingMailPWD
        End Get
        Set(value As String)
            DBLayer.DBLInstance.read(Me)
            If IM_WatchingMailPWD = value Then
                Return
            End If
            IM_WatchingMailPWD = value
            IsModified = True
        End Set
    End Property

    Public Property Conditionid As Integer Implements IeZMailWatching.Conditionid
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_Conditionid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.read(Me)
            If IM_Conditionid = value Then
                Return
            End If
            IM_Conditionid = value
            IsModified = True
        End Set
    End Property
    Public Property WatchingTime As String Implements IeZMailWatching.WatchingTime
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_WatchingTime
        End Get
        Set(value As String)
            DBLayer.DBLInstance.read(Me)
            If IM_WatchingTime = value Then
                Return
            End If
            IM_WatchingTime = value
            IsModified = True
        End Set
    End Property
    Public Property WatchingStatus As String Implements IeZMailWatching.WatchingStatus
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_WatchingStatus
        End Get
        Set(value As String)
            DBLayer.DBLInstance.read(Me)
            If IM_WatchingStatus = value Then
                Return
            End If
            IM_WatchingStatus = value
            IsModified = True
        End Set
    End Property
    Public Property port As String Implements IeZMailWatching.port
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_port
        End Get
        Set(value As String)
            DBLayer.DBLInstance.read(Me)
            If IM_port = value Then
                Return
            End If
            IM_port = value
            IsModified = True
        End Set
    End Property
    Public Property SMTP As String Implements IeZMailWatching.SMTP
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_SMTP
        End Get
        Set(value As String)
            DBLayer.DBLInstance.read(Me)
            If IM_SMTP = value Then
                Return
            End If
            IM_SMTP = value
            IsModified = True
        End Set
    End Property

    Public Property createdby As Integer Implements IeZMailWatching.createdby
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.read(Me)
            If IM_createdby = value Then
                Return
            End If
            IM_createdby = value
            IsModified = True
        End Set
    End Property
    Public Property updatedby As Integer Implements IeZMailWatching.updatedby
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_Updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.read(Me)
            If IM_Updatedby = value Then
                Return
            End If
            IM_Updatedby = value
            IsModified = True
        End Set
    End Property

    Public Property createdon As String Implements IeZMailWatching.createdon
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.read(Me)
            If IM_Createdon = value Then
                Return
            End If
            IM_Createdon = value
            IsModified = True
        End Set
    End Property
    Public Property updatedon As String Implements IeZMailWatching.updatedon
        Get
            DBLayer.DBLInstance.read(Me)
            Return IM_Updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.read(Me)
            If IM_Updatedon = value Then
                Return
            End If
            IM_Updatedon = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements IeZMailWatching.Isdeleted
        Get
            Return IM_Isdeleted
        End Get
    End Property

    Public Property CreatedBy1 As String Implements IeZMailWatching.CreatedBy1
        Get
            DBLayer.DBLInstance.read(Me)
            Return _CreatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.read(Me)
            If _CreatedBy1 = value Then
                Return
            End If
            _CreatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1 As String Implements IeZMailWatching.UpdatedBy1
        Get
            DBLayer.DBLInstance.read(Me)
            Return _UpdatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.read(Me)
            If _UpdatedBy1 = value Then
                Return
            End If
            _UpdatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
