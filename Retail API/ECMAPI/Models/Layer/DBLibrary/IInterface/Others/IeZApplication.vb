Imports System.Collections.Generic
Imports System.Text

Public Interface IeZApplication
    Inherits IDatabaseItems
    Property ApplicationId() As Integer
    Property ApplicationName() As String
    Property AppVersion() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
