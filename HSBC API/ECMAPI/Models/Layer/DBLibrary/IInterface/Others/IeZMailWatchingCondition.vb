Imports System.Collections.Generic
Imports System.Text

Public Interface IeZMailWatchingCondition
    Inherits IDatabaseItems

    Property conditionid() As Integer
    Property condition() As String
    Property createdon() As String
    Property updatedon() As String
    Property createdby() As Integer
    Property updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer
End Interface
