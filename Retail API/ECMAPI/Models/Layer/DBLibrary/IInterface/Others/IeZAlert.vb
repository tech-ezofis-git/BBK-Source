Imports System.Collections.Generic
Imports System.Text

Public Interface IeZAlert
    Inherits IDatabaseItems
    Property AlertId() As Integer
    Property DocumentAlertId() As Integer

    Property AlertConditionId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsAlertExist() As Boolean
End Interface
