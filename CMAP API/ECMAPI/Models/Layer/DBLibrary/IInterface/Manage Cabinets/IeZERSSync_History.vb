Imports System.Collections.Generic
Imports System.Text


Public Interface IeZERSSync_History
    Inherits IDatabaseItems

    Property ezerssync_historyid() As Integer
    Property eZERSSyncid() As Integer
    Property Scheduleid() As Integer
    Property NO_OF_Files_Copied As Integer
    Property Status() As String
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property updatedby() As Integer
    Property Createdby1() As String
    Property updatedby1() As String
    ReadOnly Property isdeleted() As Integer

End Interface
