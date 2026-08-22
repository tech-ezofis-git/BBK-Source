Imports System.Collections.Generic
Imports System.Text
Public Interface IeZTemplateField
    Inherits IDatabaseItems
    Property TemplateID() As Integer
    Property TemplateName() As String
    'Property BarcodeTypeId() As Integer
    Property DataTypeId() As Integer
    Property DataType() As String
    'Property BarcodeType() As String
    Property TableName As String
    Property DT() As String
    Property FieldId() As Integer
    Property FieldName() As String
    Property FieldLevel() As Integer
    Property Mandatory() As Boolean
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsTemplateFieldExist() As Boolean
    Property IsEditable() As Boolean
End Interface

