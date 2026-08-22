Imports System.Collections.Generic
Imports System.Text
Public Interface IeZLookupClientField
    Inherits IDatabaseItems
    Property LookupClientFieldId() As Integer
    Property ClientField() As String
    Property LookupId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZLookupClientField() As Boolean
    Property ClientFieldValues() As String
End Interface
