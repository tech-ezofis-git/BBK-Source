Imports System.Collections.Generic
Imports System.Text
Public Interface IeZMailWatching
    Inherits IDatabaseItems
    Property mailwatchingid() As Integer
    Property Watchingmail() As String
    Property WatchingMailPWD() As String
    Property Conditionid() As Integer
    Property WatchingTime() As String
    Property WatchingStatus() As String
    Property port() As String
    Property SMTP() As String
    Property createdon() As String
    Property updatedon() As String
    Property createdby() As Integer
    Property updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer


End Interface
