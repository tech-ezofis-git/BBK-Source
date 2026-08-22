Imports System.Collections.Generic
Imports System.Text
Public Interface IeZPdfProperties
    Inherits IDatabaseItems
    Property PdfId() As Integer
    Property TemplateID() As Integer
    Property TemplateName() As String
    Property Sync() As Integer
    Property Subject() As String
    Property Title() As String
    Property Author() As String
    Property Keyword() As String
    Property Signature() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZPdfPropertiesExist() As Boolean
End Interface

