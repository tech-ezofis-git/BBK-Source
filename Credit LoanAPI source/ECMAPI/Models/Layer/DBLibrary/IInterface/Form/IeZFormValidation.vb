Imports System.Collections.Generic
Imports System.Text

Public Interface IeZFormValidation
    Inherits IDatabaseItems
    Property ValidationId() As Integer
    Property ValidationName() As String
    Property FunctionName() As String
    Property OnEvent() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
