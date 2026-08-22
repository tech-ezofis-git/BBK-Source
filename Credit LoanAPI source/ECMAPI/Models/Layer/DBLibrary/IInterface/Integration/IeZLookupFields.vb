Imports System.Collections.Generic
Imports System.Text
Public Interface IeZLookupFields
    Inherits IDatabaseItems
    Property LookupFieldId() As Integer
    Property ECMField() As String
    Property ClientField() As String
    Property ParameterOrder As Integer
    Property LookupId() As Integer
    Property IsSyncField() As Boolean
    Property Templateid() As String
    Property Cabinetid() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZLookupFields() As Boolean
End Interface
