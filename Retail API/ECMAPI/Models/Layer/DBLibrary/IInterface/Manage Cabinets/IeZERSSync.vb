
Imports System.Collections.Generic
Imports System.Text

Public Interface IeZERSSync
    Inherits IDatabaseItems

    Property eZERSSyncid() As Integer
    Property eZERSSyncname() As String
    Property FromERS() As String
    Property ToERS() As String
    Property Status() As String
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property updatedby() As Integer
    Property Createdby1() As String
    Property updatedby1() As String
    ReadOnly Property isdeleted() As Integer

End Interface
