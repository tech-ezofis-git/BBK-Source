Imports ECMAPI

Public Class ErrorMessage
    Inherits IDatabaseCommonItems
    Implements IErrorMessage

    Protected _CreatedOn As String = ""
    Protected _Description As String = ""
    Protected _ErrorFrom As String = ""
    Protected _Message As String = ""
    Protected _SysName As String = ""

    Public Sub New()
    End Sub
    Public Property CreatedOn As String Implements IErrorMessage.CreatedOn
        Get
            Return _CreatedOn
        End Get
        Set(value As String)
            _CreatedOn = value
        End Set
    End Property

    Public Property Description As String Implements IErrorMessage.Description
        Get
            Return _Description
        End Get
        Set(value As String)
            _Description = value
        End Set
    End Property

    Public Property ErrorFrom As String Implements IErrorMessage.ErrorFrom
        Get
            Return _ErrorFrom
        End Get
        Set(value As String)
            _ErrorFrom = value
        End Set
    End Property

    Public Property Message As String Implements IErrorMessage.Message
        Get
            Return _Message
        End Get
        Set(value As String)
            _Message = value
        End Set
    End Property

    Public Property SysName As String Implements IErrorMessage.SysName
        Get
            Return _SysName
        End Get
        Set(value As String)
            _SysName = value
        End Set
    End Property
End Class
