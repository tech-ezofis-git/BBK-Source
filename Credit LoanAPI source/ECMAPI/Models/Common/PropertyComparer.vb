Imports System.Reflection

Public Class PropertyComparer(Of T)
    Implements IEqualityComparer(Of T)

    Private _PropertyInfo As PropertyInfo

    Public Sub New(ByVal propertyName As String)
        _PropertyInfo = GetType(T).GetProperty(propertyName, BindingFlags.GetProperty Or BindingFlags.Instance Or BindingFlags.[Public])

        If _PropertyInfo Is Nothing Then
            Throw New ArgumentException(String.Format("{0} is not a property of type {1}.", propertyName, GetType(T)))
        End If
    End Sub


    Private Function IEqualityComparer_Equals(x As T, y As T) As Boolean Implements IEqualityComparer(Of T).Equals
        Dim xValue As Object = _PropertyInfo.GetValue(x, Nothing)
        Dim yValue As Object = _PropertyInfo.GetValue(y, Nothing)
        If xValue Is Nothing Then Return yValue Is Nothing
        Return xValue.Equals(yValue)
    End Function

    Private Function IEqualityComparer_GetHashCode(obj As T) As Integer Implements IEqualityComparer(Of T).GetHashCode
        Dim propertyValue As Object = _PropertyInfo.GetValue(obj, Nothing)

        If propertyValue Is Nothing Then
            Return 0
        Else
            Return propertyValue.GetHashCode()
        End If
    End Function
End Class

