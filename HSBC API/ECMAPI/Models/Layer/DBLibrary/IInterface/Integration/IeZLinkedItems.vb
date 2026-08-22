Imports System.Collections.Generic
Imports System.Text

Public Interface IeZLinkedItems
    Inherits IDatabaseItems
    Property Linkedid() As Integer
    Property templateid() As Integer
    Property SourceFieldid() As Integer
    Property Linkedfieldid() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Boolean
End Interface
