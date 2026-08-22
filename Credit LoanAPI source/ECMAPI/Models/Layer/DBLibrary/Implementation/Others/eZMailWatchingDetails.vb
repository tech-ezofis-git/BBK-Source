Imports ECMAPI

Public Class eZMailWatchingDetails
    Inherits IDatabaseCommonItems
    Implements IeZMailWatchingDetails

    Protected IM_sendid As Integer
    Protected IM_Mailwatchingid As Integer
    Protected IM_Conditionid As Integer
    Protected IM_keyword As String
    Protected IM_Tomail As String
    Protected IM_CreatedOn As String
    Protected IM_UpdatedOn As String
    Protected IM_CreatedBy As Integer
    Protected IM_UpdatedBy As Integer
    Protected IM_CreatedBy1 As String
    Protected IM_UpdatedBy1 As String
    Private IM_Isdelete As Integer

    Public Sub New(sendid As Integer)
        Me.IM_sendid = sendid
    End Sub
    Public Sub New()

    End Sub
    Public Property sendid As Integer Implements IeZMailWatchingDetails.sendid
        Get
            If sendid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return IM_sendid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If IM_sendid <> 0 AndAlso IM_sendid <> value Then
                Throw New MemberAccessException()
            End If
            IM_sendid = value
        End Set
    End Property
    Public Property Conditionid As Integer Implements IeZMailWatchingDetails.Conditionid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Conditionid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Conditionid = value Then
                Return
            End If
            IM_Conditionid = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZMailWatchingDetails.CreatedBy
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

    Public Property CreatedOn As String Implements IeZMailWatchingDetails.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IeZMailWatchingDetails.Isdeleted

        Get
            Return Isdeleted
        End Get
    End Property

    Public Property keyword As String Implements IeZMailWatchingDetails.keyword
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_keyword
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_keyword = value Then
                Return
            End If
            IM_keyword = value
            IsModified = True
        End Set
    End Property

    Public Property Mailwatchingid As Integer Implements IeZMailWatchingDetails.Mailwatchingid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Mailwatchingid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Mailwatchingid = value Then
                Return
            End If
            IM_Mailwatchingid = value
            IsModified = True
        End Set
    End Property

    Public Property Tomail As String Implements IeZMailWatchingDetails.Tomail
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Tomail
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_Tomail = value Then
                Return
            End If
            IM_Tomail = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZMailWatchingDetails.UpdatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_UpdatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_UpdatedBy = value Then
                Return
            End If
            IM_UpdatedBy = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedOn As String Implements IeZMailWatchingDetails.UpdatedOn
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

    Public Property CreatedBy1 As String Implements IeZMailWatchingDetails.CreatedBy1
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

    Public Property UpdatedBy1 As String Implements IeZMailWatchingDetails.UpdatedBy1
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
