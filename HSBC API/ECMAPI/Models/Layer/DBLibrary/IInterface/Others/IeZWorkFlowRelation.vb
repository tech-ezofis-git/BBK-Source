Imports System.Collections.Generic
Imports System.Text

Public Interface IeZWorkFlowRelation
    Inherits IDatabaseItems
    Property RelationId() As Integer
    Property FormId() As Integer
    Property WorkFlowId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
