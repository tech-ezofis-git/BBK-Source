Imports System.Collections.Generic
Imports System.Text

Public Interface IeZAlertCondition
    Inherits IDatabaseItems
    Property AlertConditionId() As Integer
    Property AlertCondition() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsAlertConditionExist() As Boolean
End Interface
