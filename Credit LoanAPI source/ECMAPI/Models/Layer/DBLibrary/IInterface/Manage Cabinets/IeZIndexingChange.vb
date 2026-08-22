Imports System.Collections.Generic
Imports System.Text

Public Interface IeZIndexingChange
    Inherits IDatabaseItems
    Property Indexingchangeid() As Integer
    Property Templateid() As Integer
    Property Nodeid() As Integer
    Property oldvalue() As String
    Property Newvalue() As String
    Property Parentid() As Integer
    Property del() As Integer
    Property Levelid() As Integer
    Property itemid() As Integer
    Property Fieldid() As Integer
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer
End Interface
