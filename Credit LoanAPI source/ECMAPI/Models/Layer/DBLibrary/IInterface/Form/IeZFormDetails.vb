Imports System.Collections.Generic
Imports System.Text

Public Interface IeZFormDetails
    Inherits IDatabaseItems
    Property FormId() As Integer
    Property FormName() As String
    Property FormTableName() As String
    Property Status() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZFormDetailsExist() As Boolean
End Interface
