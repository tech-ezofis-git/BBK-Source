Imports System.Collections.Generic
Imports System.Text

Public Interface IeZWorkFlowProcess
    Inherits IDatabaseItems
    Property ProcessId() As Integer
    Property WorkFlowId() As Integer
    Property Stage() As String
    Property InitiatedOn() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
