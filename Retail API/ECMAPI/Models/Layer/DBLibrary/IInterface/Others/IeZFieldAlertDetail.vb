Imports System.Collections.Generic
Imports System.Text

Public Interface IeZFieldAlertDetail
    Inherits IDatabaseItems
    Property FieldAlertDetailId() As Integer
    Property FieldAlertName As String
    Property ToMail() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsFieldAlertDetailExist() As Boolean
End Interface
