Imports System.Collections.Generic
Imports System.Text

Public Interface IeZDocumentAlert
    Inherits IDatabaseItems
    Property DocumentAlertId() As Integer
    Property itemid() As Integer
    Property filename As String
    Property TableName As String
    Property TemplateId() As Integer
    Property ToMail() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsDocumentAlertExist() As Boolean
End Interface
