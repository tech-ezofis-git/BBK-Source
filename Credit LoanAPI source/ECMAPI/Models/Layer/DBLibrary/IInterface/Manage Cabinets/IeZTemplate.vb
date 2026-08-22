Imports System.Collections.Generic
Imports System.Text
Public Interface IeZTemplate
    Inherits IDatabaseItems
    Property CabinetID() As Integer
    Property CabinetName() As String
    Property DuplicateTypeId() As Integer
    Property TableName As String
    Property DocumentCount() As Integer
    Property DuplicateType() As String
    Property TempCurrentSize() As String
    Property TemplateId() As Integer
    Property TemplateName() As String
    Property Description() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    Property Encrypt() As Integer
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsTemplateExist() As Boolean
End Interface

