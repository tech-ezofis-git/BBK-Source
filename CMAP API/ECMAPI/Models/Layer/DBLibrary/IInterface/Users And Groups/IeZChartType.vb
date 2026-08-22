Imports System.Collections.Generic
Imports System.Text

Public Interface IeZChartType
    Inherits IDatabaseItems
    Property ChartTypeId() As Integer
    Property ChartType() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsChartTypeExist() As Boolean
End Interface
