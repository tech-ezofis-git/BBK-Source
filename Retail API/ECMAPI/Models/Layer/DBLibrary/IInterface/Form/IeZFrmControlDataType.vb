Imports System.Collections.Generic
Imports System.Text

Public Interface IeZFrmControlDataType
    Inherits IDatabaseItems
    Property ControlDataTypeId() As Integer
    Property ControlDataType() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsControlDataTypeExist() As Boolean
End Interface
