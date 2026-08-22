Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZOutlookSync_History
    Inherits IDatabaseCommonItems
    Implements IeZOutlookSync_History

    Protected _Outlooksync_historyid As Integer
    Protected _OutlookSyncId As Integer = 0
    Protected _SyncStatus As String = ""
    Protected _SyncDate As String = ""
    Protected _Createdon As String = ""
    Protected _Updatedon As String = ""
    Protected _Createdby As Integer = 0
    Protected _updatedby As Integer = 0
    Protected _Createdby1 As String = 0
    Protected _updatedby1 As String = 0
    Private _isdeleted As Integer = 0
    Public Sub New(ByVal Outlooksync_historyid As Integer)
        Me.Outlooksync_historyid = Outlooksync_historyid
    End Sub
    Public Sub New()
    End Sub
    Public Property Createdby As Integer Implements IeZOutlookSync_History.Createdby
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
    Public Property Createdby1 As String Implements IeZOutlookSync_History.Createdby1
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
    Public Property Createdon As String Implements IeZOutlookSync_History.Createdon
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
    Public ReadOnly Property isdeleted As Integer Implements IeZOutlookSync_History.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Property Outlooksync_historyid As Integer Implements IeZOutlookSync_History.Outlooksync_historyid
        Get
            If _Outlooksync_historyid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Outlooksync_historyid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Outlooksync_historyid <> 0 AndAlso _Outlooksync_historyid <> value Then
                Throw New MemberAccessException()
            End If
            _Outlooksync_historyid = value
        End Set
    End Property

    Public Property OutlookSyncId As Integer Implements IeZOutlookSync_History.OutlookSyncId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _OutlookSyncId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _OutlookSyncId = value Then
                Return
            End If
            _OutlookSyncId = value
            IsModified = True
        End Set
    End Property
    Public Property SyncDate As String Implements IeZOutlookSync_History.SyncDate
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SyncDate
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SyncDate = value Then
                Return
            End If
            _SyncDate = value
            IsModified = True
        End Set
    End Property

    Public Property SyncStatus As String Implements IeZOutlookSync_History.SyncStatus
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SyncStatus
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SyncStatus = value Then
                Return
            End If
            _SyncStatus = value
            IsModified = True
        End Set
    End Property

    Public Property updatedby As Integer Implements IeZOutlookSync_History.updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _updatedby = value Then
                Return
            End If
            _updatedby = value
            IsModified = True
        End Set
    End Property
    Public Property updatedby1 As String Implements IeZOutlookSync_History.updatedby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _updatedby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _updatedby1 = value Then
                Return
            End If
            _updatedby1 = value
            IsModified = True
        End Set
    End Property
    Public Property Updatedon As String Implements IeZOutlookSync_History.Updatedon
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
End Class
