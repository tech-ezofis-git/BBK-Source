Imports System.Collections.Generic
Imports System.Text

Public Interface IeZPrivateFolders
    Inherits IDatabaseItems

    Property Privatefolderid() As Integer
    Property Nodeid() As Integer
    Property userid() As Integer
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Createdby1() As String
    Property Updatedby1() As String
    Property Updatedby() As Integer
    ReadOnly Property isdeleted() As Integer
End Interface
