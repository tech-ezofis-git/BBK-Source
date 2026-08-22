Imports System.Collections.Generic
Imports System.Text

Public Interface IeZMailWatchingDetails
    Inherits IDatabaseItems

    Property sendid() As Integer
    Property Mailwatchingid() As Integer
    Property Conditionid() As Integer
    Property keyword() As String
    Property Tomail() As String
    Property CreatedOn() As String
    Property UpdatedOn() As String
    Property CreatedBy() As Integer
    Property UpdatedBy() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
