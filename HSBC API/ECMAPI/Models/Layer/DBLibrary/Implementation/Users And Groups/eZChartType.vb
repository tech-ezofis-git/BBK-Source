Imports System.Data
Imports System.Configuration
Imports System.Web
''' <summary>
''' Summary description for ChartTypeGroup
''' </summary>
Public Class eZChartType
    Inherits IDatabaseCommonItems
    Implements IeZChartType
    Protected _ChartTypeId As Integer
    Protected _ChartType As String

    Private _Isdeleted As Integer

    Public Sub New(tmpChartTypeId As Integer)
        Me._ChartTypeId = tmpChartTypeId
    End Sub
    Public Sub New(tmpChartType As String)
        Me._ChartType = tmpChartType
    End Sub

    Public Sub New()
    End Sub
    Public Property ChartTypeId() As Integer Implements IeZChartType.ChartTypeId
        Get
            If _ChartTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ChartTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ChartTypeId <> 0 AndAlso _ChartTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _ChartTypeId = value
        End Set
    End Property

    Public Property ChartType() As String Implements IeZChartType.ChartType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ChartType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ChartType = value Then
                Return
            End If
            _ChartType = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted() As Integer Implements IeZChartType.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsChartTypeExist() As Boolean Implements IeZChartType.IsChartTypeExist
        Get
            Return (ChartTypeId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
