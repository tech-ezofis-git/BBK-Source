Imports System.Collections.Generic
Imports System.Text


Public Interface IeZFormControlDetail
    Inherits IDatabaseItems
    Property ControlId() As Integer
    Property ControlName() As String
    Property FormId() As Integer
    Property OrderId() As Double
    Property ControlTypeId() As Integer
    Property DataType() As Integer
    Property style() As String
    Property TableTagType() As String
    Property ValidationId() As Integer
    Property TabIndex() As Integer
    Property GridRow() As Integer
    Property GridColumn() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsControlNameExist() As Boolean
End Interface
