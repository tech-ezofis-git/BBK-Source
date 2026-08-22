Imports System.Collections.Generic
Imports System.Text
Public Interface IeZComments
    Inherits IDatabaseItems
    Property CommentsId() As Integer
    Property itemid() As Integer
    Property TemplateId() As Integer
    Property CommentsBy() As Integer
    Property Processid() As Integer
    Property ExternalCommentsBy() As String
    Property CreatedBy() As Integer
    Property Comments() As String
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsComments() As Boolean
End Interface
