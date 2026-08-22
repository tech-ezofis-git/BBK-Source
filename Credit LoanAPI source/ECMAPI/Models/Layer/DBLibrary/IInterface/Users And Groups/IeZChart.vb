Imports System.Collections.Generic
Imports System.Text

Public Interface IeZChart
    Inherits IDatabaseItems
    Property ChartId() As Integer
    Property Chart() As String
    Property ChartTypeId() As Integer

    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsChartExist() As Boolean
End Interface
