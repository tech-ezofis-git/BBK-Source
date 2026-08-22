Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZChart
    Inherits IDatabaseCommonItems
    Implements IeZChart
    Protected _ChartId As Integer
    Protected _Chart As String
    Protected _CreatedBy As Integer
    Protected _HtmlNamewithPath As String
    Protected _ChartTypeId As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CUserName As String
    Protected _CUserCode As String
    Protected _UUserName As String
    Protected _UUserCode As String
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpChartId As Integer)
        Me._ChartId = tmpChartId
    End Sub
    Public Sub New(tmpChart As String)
        Me._Chart = tmpChart
    End Sub

    Public Sub New()
    End Sub
    Public Property ChartId() As Integer Implements IeZChart.ChartId
        Get
            If _ChartId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ChartId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ChartId <> 0 AndAlso _ChartId <> value Then
                Throw New MemberAccessException()
            End If
            _ChartId = value
        End Set
    End Property

    Public Property Chart() As String Implements IeZChart.Chart
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Chart
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Chart = value Then
                Return
            End If
            _Chart = value
            IsModified = True
        End Set
    End Property
   
    Public Property UpdatedBy1() As String Implements IeZChart.UpdatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy1 = value Then
                Return
            End If
            _UpdatedBy1 = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedBy1() As String Implements IeZChart.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy1 = value Then
                Return
            End If
            _CreatedBy1 = value
            IsModified = True
        End Set
    End Property


    Public Property CreatedBy() As Integer Implements IeZChart.CreatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy = value Then
                Return
            End If

            _CreatedBy = value
            IsModified = True
        End Set
    End Property
    Public Property ChartTypeId() As Integer Implements IeZChart.ChartTypeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ChartTypeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ChartTypeId = value Then
                Return
            End If

            _ChartTypeId = value
            IsModified = True
        End Set
    End Property


    Public Property CreatedOn() As String Implements IeZChart.CreatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedOn = value Then
                Return
            End If

            _CreatedOn = value
            IsModified = True
        End Set
    End Property


    Public Property UpdatedBy() As Integer Implements IeZChart.UpdatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy = value Then
                Return
            End If

            _UpdatedBy = value
        End Set
    End Property

    Public Property UpdatedOn() As String Implements IeZChart.UpdatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedOn = value Then
                Return
            End If

            _UpdatedOn = value
        End Set
    End Property

    Public ReadOnly Property Isdeleted() As Integer Implements IeZChart.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsChartExist() As Boolean Implements IeZChart.IsChartExist
        Get
            Return (ChartId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
