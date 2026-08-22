Imports System.Collections.Generic
Imports System.Text

Public Interface IeZBodyHtmlType
    Inherits IDatabaseItems
    Property BodyHtmlTypeId() As Integer
    Property BodyHtmlType() As String
    Property NoOfParameter() As Integer
    Property HtmlNamewithPath() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsBodyHtmlTypeExist() As Boolean
End Interface
