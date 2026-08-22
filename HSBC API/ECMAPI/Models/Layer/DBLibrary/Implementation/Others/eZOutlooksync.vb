
Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZOutlooksync
    Inherits IDatabaseCommonItems
    Implements IeZOutlooksync

    Protected D_Outlooksyncid As Integer
    Protected D_Scheduleid As Integer = 0
    Protected D_Syncname As String = ""
    Protected D_Syncrule As String = ""
    Protected D_SyncMail As String = ""
    Protected D_ScheduleTypeId As Integer
    Protected D_ForSchedule As Integer
    Protected D_Id As Integer
    Protected D_WeekDay As Integer
    Protected D_Mont As Integer
    Protected D_Day As Integer
    Protected D_EachDay As Integer
    Protected D_OnceDate As String
    Protected D_Time As String
    Protected D_Createdon As String = ""
    Protected D_updatedon As String = ""
    Protected D_Createdby As Integer = 0
    Protected D_updatedby As Integer
    Private D_isdeleted As Integer = 0


    Public Sub New(ByVal tempoutlookid As Integer)
        Me.D_Outlooksyncid = tempoutlookid
    End Sub

    Public Sub New()

    End Sub


    Public Property Createdby As Integer Implements IeZOutlooksync.Createdby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdby = value Then
                Return
            End If
            D_Createdby = value

        End Set
    End Property
    Public Property Createdon As String Implements IeZOutlooksync.Createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdon = value Then
                Return
            End If
            D_Createdon = value

        End Set
    End Property
    Public Property OnceDate As String Implements IeZOutlooksync.OnceDate
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_OnceDate
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_OnceDate = value Then
                Return
            End If
            D_OnceDate = value

        End Set
    End Property
    Public Property Time As String Implements IeZOutlooksync.Time
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Time
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Time = value Then
                Return
            End If
            D_Time = value

        End Set
    End Property
    Public Property ScheduleTypeId As Integer Implements IeZOutlooksync.ScheduleTypeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ScheduleTypeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_ScheduleTypeId = value Then
                Return
            End If
            D_ScheduleTypeId = value

        End Set
    End Property
    Public Property ForSchedule As Integer Implements IeZOutlooksync.ForSchedule
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ForSchedule
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_ForSchedule = value Then
                Return
            End If
            D_ForSchedule = value

        End Set
    End Property
    Public Property Id As Integer Implements IeZOutlooksync.Id
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Id
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Id = value Then
                Return
            End If
            D_Createdon = value

        End Set
    End Property
    Public Property WeekDay As Integer Implements IeZOutlooksync.WeekDay
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_WeekDay
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_WeekDay = value Then
                Return
            End If
            D_WeekDay = value

        End Set
    End Property
    Public Property Mont As Integer Implements IeZOutlooksync.Mont
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Mont
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Mont = value Then
                Return
            End If
            D_Mont = value

        End Set
    End Property
    Public Property Day As Integer Implements IeZOutlooksync.Day
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Day
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Day = value Then
                Return
            End If
            D_Day = value

        End Set
    End Property

    Public Property EachDay As Integer Implements IeZOutlooksync.EachDay
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_EachDay
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_EachDay = value Then
                Return
            End If
            D_EachDay = value

        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements IeZOutlooksync.isdeleted
        Get
            Return D_isdeleted
        End Get
    End Property

    Public Property Outlooksyncid As Integer Implements IeZOutlooksync.Outlooksyncid
        Get
            If D_Outlooksyncid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return D_Outlooksyncid
        End Get
        Set(value As Integer)
            If Not IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_Outlooksyncid <> 0 AndAlso D_Outlooksyncid <> value Then
                Throw New MemberAccessException()
            End If
            D_Outlooksyncid = value

        End Set
    End Property

    Public Property Scheduleid As Integer Implements IeZOutlooksync.Scheduleid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Scheduleid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Scheduleid = value Then
                Return
            End If
            D_Scheduleid = value
            IsModified = True
        End Set
    End Property

    Public Property SyncMail As String Implements IeZOutlooksync.SyncMail
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_SyncMail
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_SyncMail = value Then
                Return
            End If
            D_SyncMail = value

        End Set
    End Property

    Public Property Syncname As String Implements IeZOutlooksync.Syncname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Syncname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Syncname = value Then
                Return
            End If
            D_Syncname = value

        End Set
    End Property

    Public Property Syncrule As String Implements IeZOutlooksync.Syncrule
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Syncrule
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Syncrule = value Then
                Return
            End If
            D_Syncrule = value

        End Set
    End Property

    Public Property updatedby As Integer Implements IeZOutlooksync.updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_updatedby = value Then
                Return
            End If
            D_updatedby = value

        End Set
    End Property

    Public Property updatedon As String Implements IeZOutlooksync.updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_updatedon = value Then
                Return
            End If
            D_updatedon = value

        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
