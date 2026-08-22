Imports System.Collections.Generic
Imports System.Text
Public Interface IeZTempBarcode
    Inherits IDatabaseItems
    Property BarcodeId() As Integer
    Property TemplateID() As Integer
    Property BarcodeTypeId() As Integer
    Property BarcodeType() As String
    Property TemplateName() As String
    Property StartsWith() As String
    Property EndWith() As String
    Property Length() As String

    Property BarcodeField() As String
    Property prefix() As String
    Property suffix() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZTempBarcodeExist() As Boolean
End Interface


