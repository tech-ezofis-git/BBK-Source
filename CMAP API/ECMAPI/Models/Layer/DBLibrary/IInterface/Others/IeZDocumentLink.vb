Imports System.Collections.Generic
Imports System.Text
Public Interface IeZDocumentLink
    Inherits IDatabaseItems
    Property LinkId() As Integer
    Property itemid() As Integer
    Property LinkedItemId() As Integer
    Property LinkedTemplateId() As Integer
    Property TemplateId() As Integer
    Property LinkBy() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsDocumentLink() As Boolean
End Interface
