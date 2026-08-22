Imports System.Collections.Generic
Imports System.Text

Public Interface ieZtest
    Inherits IDatabaseItems

    Property Outlooksyncid() As Integer
    Property Scheduleid() As Integer
    Property Syncname() As String
    Property Syncrule() As String
    Property SyncMail() As String
    Property Createdon() As String
    Property updatedon() As String
    Property Createdby() As Integer
    Property updatedby() As Integer
    ReadOnly Property isdeleted() As Integer

End Interface
