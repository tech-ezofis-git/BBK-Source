Imports ECMAPI

Public Class ChartItems
    Implements IChartItems
    Protected _Encrypt As Integer
    Protected _CabinetName As String
    Protected _CabinetID As String
    Protected _TemplateName As String
    Protected _TemplateId As String
    Protected _CabCurrentSize As String
    Protected _TotalSize As String
    Protected _Value1Dataset As DataSet
    Protected _value1 As String
    Protected _value2 As String
    Protected _value3 As String
    Protected _value4 As String
    Protected _value5 As String
    Protected _value6 As String
    Protected _value7 As String
    Protected _value8 As String
    Protected _value9 As String
    Protected _value10 As String
    Protected _lstValue1 As List(Of String)
    Protected _lstValue2 As List(Of String)
    Protected _lstValue3 As List(Of String)
    Protected _lstValue4 As List(Of String)
    Protected _lstValue5 As List(Of String)
    Protected _lstValue6 As List(Of String)
    Protected _lstValue7 As List(Of String)
    Protected _lstValue8 As List(Of String)
    Protected _lstValue9 As List(Of String)
    Protected _lstValue10 As List(Of String)
    Protected _HasAccess As Boolean = True
    Public Property CabinetID() As String Implements IChartItems.CabinetID
        Get
            Return _CabinetID
        End Get
        Set(value As String)

            If _CabinetID = value Then
                Return
            End If
            _CabinetID = value

        End Set
    End Property

    Public Property CabinetName() As String Implements IChartItems.CabinetName
        Get

            Return _CabinetName
        End Get
        Set(value As String)

            If _CabinetName = value Then
                Return
            End If
            _CabinetName = value

        End Set
    End Property
    Public Property TemplateId() As String Implements IChartItems.TemplateId
        Get

            Return _TemplateId
        End Get
        Set(value As String)

            If _TemplateId = value Then
                Return
            End If
            _TemplateId = value

        End Set
    End Property
    Public Property TemplateName() As String Implements IChartItems.TemplateName
        Get

            Return _TemplateName
        End Get
        Set(value As String)


            _TemplateName = value

        End Set
    End Property
    Public Property CabCurrentSize() As String Implements IChartItems.CabCurrentSize
        Get
            Return _CabCurrentSize
        End Get
        Set(value As String)
            _CabCurrentSize = value
        End Set
    End Property
    Public Property TotalSize() As String Implements IChartItems.TotalSize
        Get
            Return _TotalSize
        End Get
        Set(value As String)
            _TotalSize = value
        End Set
    End Property
    Public Property value1() As String Implements IChartItems.Value1
        Get
            Return _value1
        End Get
        Set(value As String)
            If _value1 = value Then
                Return
            End If
            _value1 = value
        End Set
    End Property
    Public Property Value1Dataset() As DataSet Implements IChartItems.Value1Dataset
        Get
            Return _Value1Dataset
        End Get
        Set(value As DataSet)

            _Value1Dataset = value
        End Set
    End Property
    Public Property value2() As String Implements IChartItems.Value2
        Get
            Return _value2
        End Get
        Set(value As String)
            If _value2 = value Then
                Return
            End If
            _value2 = value
        End Set
    End Property
    Public Property value3() As String Implements IChartItems.Value3
        Get

            Return _value3
        End Get
        Set(value As String)

            If _value3 = value Then
                Return
            End If
            _value3 = value

        End Set
    End Property
    Public Property value4() As String Implements IChartItems.Value4
        Get

            Return _value4
        End Get
        Set(value As String)

            If _value4 = value Then
                Return
            End If
            _value4 = value

        End Set
    End Property
    Public Property value5() As String Implements IChartItems.Value5
        Get

            Return _value5
        End Get
        Set(value As String)

            If _value5 = value Then
                Return
            End If
            _value5 = value

        End Set
    End Property
    Public Property value6() As String Implements IChartItems.Value6
        Get

            Return _value6
        End Get
        Set(value As String)

            If _value6 = value Then
                Return
            End If
            _value6 = value

        End Set
    End Property
    Public Property value7() As String Implements IChartItems.Value7
        Get

            Return _value7
        End Get
        Set(value As String)

            If _value7 = value Then
                Return
            End If
            _value7 = value

        End Set
    End Property
    Public Property value8() As String Implements IChartItems.Value8
        Get

            Return _value8
        End Get
        Set(value As String)

            If _value8 = value Then
                Return
            End If
            _value8 = value

        End Set
    End Property
    Public Property value9() As String Implements IChartItems.Value9
        Get

            Return _value9
        End Get
        Set(value As String)

            If _value9 = value Then
                Return
            End If
            _value9 = value

        End Set
    End Property
    Public Property value10() As String Implements IChartItems.Value10
        Get

            Return _value10
        End Get
        Set(value As String)

            If _value10 = value Then
                Return
            End If
            _value10 = value

        End Set
    End Property

    Public Property lstValue1() As List(Of String) Implements IChartItems.lstValue1
        Get

            Return _lstValue1
        End Get
        Set(lstValue As List(Of String))


            _lstValue1 = lstValue

        End Set
    End Property
    Public Property lstValue2() As List(Of String) Implements IChartItems.lstValue2
        Get

            Return _lstValue2
        End Get
        Set(lstValue As List(Of String))


            _lstValue2 = lstValue

        End Set
    End Property
    Public Property lstValue3() As List(Of String) Implements IChartItems.lstValue3
        Get

            Return _lstValue3
        End Get
        Set(lstValue As List(Of String))


            _lstValue3 = lstValue

        End Set
    End Property
    Public Property lstValue4() As List(Of String) Implements IChartItems.lstValue4
        Get

            Return _lstValue4
        End Get
        Set(lstValue As List(Of String))


            _lstValue4 = lstValue

        End Set
    End Property
    Public Property lstValue5() As List(Of String) Implements IChartItems.lstValue5
        Get

            Return _lstValue5
        End Get
        Set(lstValue As List(Of String))


            _lstValue5 = lstValue

        End Set
    End Property
    Public Property lstValue6() As List(Of String) Implements IChartItems.lstValue6
        Get

            Return _lstValue6
        End Get
        Set(lstValue As List(Of String))


            _lstValue6 = lstValue

        End Set
    End Property
    Public Property lstValue7() As List(Of String) Implements IChartItems.lstValue7
        Get

            Return _lstValue7
        End Get
        Set(lstValue As List(Of String))


            _lstValue7 = lstValue

        End Set
    End Property
    Public Property lstValue8() As List(Of String) Implements IChartItems.lstValue8
        Get

            Return _lstValue8
        End Get
        Set(lstValue As List(Of String))


            _lstValue8 = lstValue

        End Set
    End Property
    Public Property lstValue9() As List(Of String) Implements IChartItems.lstValue9
        Get

            Return _lstValue9
        End Get
        Set(lstValue As List(Of String))


            _lstValue9 = lstValue

        End Set
    End Property
    Public Property lstValue10() As List(Of String) Implements IChartItems.lstValue10
        Get

            Return _lstValue10
        End Get
        Set(lstValue As List(Of String))


            _lstValue10 = lstValue

        End Set

    End Property
    Public Property HasAccess() As Boolean Implements IChartItems.HasAccess
        Get
            Return _HasAccess
        End Get
        Set(lstValue As Boolean)
            _HasAccess = lstValue
        End Set
    End Property

    Public Property Encrypt() As Integer Implements IChartItems.Encrypt
        Get
            Return _Encrypt
        End Get
        Set(value As Integer)
            _Encrypt = value
        End Set
    End Property
End Class