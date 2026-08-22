Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI
Imports ECMAPI.ParaVariables

Public Class ezFrmDetails

    Implements IezFrmDetails
    Protected _DynamicProperty As Dictionary(Of String, String)
    Protected _DynamicProp As Dictionary(Of String, String)
    Protected D_Proceedwith As String
    Public Property DynamicProperty As Dictionary(Of String, String) Implements IezFrmDetails.DynamicProperty
        Get
            Return _DynamicProperty
        End Get
        Set(value As Dictionary(Of String, String))
            _DynamicProperty = value
        End Set
    End Property
    Public Property DynamicProp As Dictionary(Of String, String) Implements IezFrmDetails.DynamicProp
        Get
            Return _DynamicProp
        End Get
        Set(value As Dictionary(Of String, String))
            _DynamicProp = value
        End Set
    End Property

    Public Property Proceedwith As String Implements IezFrmDetails.Proceedwith
        Get
            Return D_Proceedwith
        End Get
        Set(value As String)
            D_Proceedwith = value
        End Set
    End Property

End Class
