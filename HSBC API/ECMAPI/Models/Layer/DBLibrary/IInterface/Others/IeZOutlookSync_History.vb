Imports System.Collections.Generic
Imports System.Text


Public Interface IeZOutlookSync_History
    Inherits IDatabaseItems
    Property Outlooksync_historyid() As Integer
    Property OutlookSyncId() As Integer
    Property SyncStatus() As String
    Property SyncDate() As String
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property updatedby() As Integer
    Property Createdby1() As String
    Property updatedby1() As String
    ReadOnly Property isdeleted() As Integer
End Interface
