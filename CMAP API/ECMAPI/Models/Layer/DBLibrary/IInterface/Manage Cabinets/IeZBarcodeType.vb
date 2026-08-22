Imports System.Collections.Generic
Imports System.Text

Public Interface IeZBarcodeType
    Inherits IDatabaseItems
    Property BarcodeTypeId() As Integer
    Property BarcodeType() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsBarcodeTypeExist() As Boolean
End Interface
