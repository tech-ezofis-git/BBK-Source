Imports System.Collections.Generic
Imports System.Text

Public Interface IeZFieldAlert
    Inherits IDatabaseItems
    Property FieldAlertId() As Integer
    Property TemplateId() As Integer
    Property FieldAlertDetailId() As Integer
    Property FieldAlertName As String
    Property FieldId() As Integer
    Property Condition As String
    Property ConditionValue As String
    Property ConditionId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsFieldAlertExist() As Boolean
End Interface
