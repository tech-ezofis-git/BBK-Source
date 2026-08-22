Imports System.Collections.Generic
Imports System.Text
Public Interface IeZAlternateField
    Inherits IDatabaseItems
    Property AlternateId() As Integer
    Property FieldId() As Integer
    Property FieldName() As String
    Property AlternateFieldId() As Integer
    Property AlternateFieldName() As String
    Property FieldValue() As String
    Property TemplateID() As Integer
    Property TemplateName() As String
    Property AlternateValue() As String
    Property LastNo() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
