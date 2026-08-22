Imports System.Collections.Generic
Imports System.Text

Public Interface IeZTempDatatype
    Inherits IDatabaseItems
    Property TempDatatypeId() As Integer
    Property TempDatatype() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsTempDatatypeExist() As Boolean
End Interface
