Imports ECMAPI

Public Class eZMailWatchingStatus
    Inherits IDatabaseCommonItems
    Implements IeZMailWatchingStatus

    Protected IM_Mailwatchingid As Integer
    Protected IM_receivedtime As Date
    Property IM_ReceivedFrom As String
    Protected IM_Keyword As String
    Protected IM_sendid As Integer
    Protected IM_MailsendStatus As String
    Protected IM_MailsendTime As Date
    Protected IM_CreatedOn As String
    Protected IM_UpdatedOn As String
    Protected IM_CreatedBy As Integer
    Protected IM_UpdatedBy As Integer
    Protected IM_CreatedBy1 As String
    Protected IM_UpdatedBy1 As String
    Private IM_isdeleted As Integer

    Public Sub New(Mailwatchingid As Integer)
        Me.IM_Mailwatchingid = Mailwatchingid
    End Sub
    Public Sub New()

    End Sub

    Public Property CreatedBy As Integer Implements IeZMailWatchingStatus.CreatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_CreatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_CreatedBy = value Then
                Return
            End If
            IM_CreatedBy = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedOn As String Implements IeZMailWatchingStatus.CreatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_CreatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_CreatedOn = value Then
                Return
            End If
            IM_CreatedOn = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements IeZMailWatchingStatus.isdeleted
        Get
            Return IM_isdeleted
        End Get
    End Property

    Public Property Keyword As String Implements IeZMailWatchingStatus.Keyword
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Keyword
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_Keyword = value Then
                Return
            End If
            IM_Keyword = value
            IsModified = True
        End Set
    End Property

    Public Property MailsendStatus As String Implements IeZMailWatchingStatus.MailsendStatus
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_MailsendStatus
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_MailsendStatus = value Then
                Return
            End If
            IM_MailsendStatus = value
            IsModified = True
        End Set
    End Property

    Public Property MailsendTime As Date Implements IeZMailWatchingStatus.MailsendTime
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_MailsendTime
        End Get
        Set(value As Date)
            DBLayer.DBLInstance.Read(Me)
            If IM_MailsendTime = value Then
                Return
            End If
            IM_MailsendTime = value
            IsModified = True
        End Set
    End Property

    Public Property Mailwatchingid As Integer Implements IeZMailWatchingStatus.Mailwatchingid
        Get
            If Mailwatchingid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return IM_Mailwatchingid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If IM_Mailwatchingid <> 0 AndAlso IM_Mailwatchingid <> value Then
                Throw New MemberAccessException()
            End If
            IM_Mailwatchingid = value
        End Set
    End Property

    Public Property ReceivedFrom As String Implements IeZMailWatchingStatus.ReceivedFrom
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_ReceivedFrom
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_ReceivedFrom = value Then
                Return
            End If
            IM_ReceivedFrom = value
            IsModified = True
        End Set
    End Property

    Public Property receivedtime As Date Implements IeZMailWatchingStatus.receivedtime
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_receivedtime
        End Get
        Set(value As Date)
            DBLayer.DBLInstance.Read(Me)
            If IM_receivedtime = value Then
                Return
            End If
            IM_receivedtime = value
            IsModified = True
        End Set
    End Property

    Public Property sendid As Integer Implements IeZMailWatchingStatus.sendid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_sendid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_sendid Then
                Return
            End If
            IM_sendid = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZMailWatchingStatus.UpdatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_UpdatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_UpdatedBy Then
                Return
            End If
            IM_UpdatedBy = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedOn As String Implements IeZMailWatchingStatus.UpdatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_UpdatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_UpdatedOn = value Then
                Return
            End If
            IM_UpdatedOn = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZMailWatchingStatus.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_CreatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_CreatedBy1 = value Then
                Return
            End If
            IM_CreatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1 As String Implements IeZMailWatchingStatus.UpdatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_UpdatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_UpdatedBy1 = value Then
                Return
            End If
            IM_UpdatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
