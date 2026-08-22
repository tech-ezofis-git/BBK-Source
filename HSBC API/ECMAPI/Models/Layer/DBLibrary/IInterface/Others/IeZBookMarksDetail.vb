Imports System.Collections.Generic
Imports System.Text
Public Interface IeZBookMarksDetail
    Inherits IDatabaseItems
    Property BookMarksDetailid() As Integer
    Property BookMarksId() As Integer
    Property ItemId() As Integer
    Property TemplateId() As Integer
    Property HitCount() As String
    Property DisplayName() As String
    Property DirectLink() As String
    Property Dates() As String
    Property Size() As String
    Property Synopsis() As String
    Property ifiletype() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZBookMarksDetailExist() As Boolean
End Interface