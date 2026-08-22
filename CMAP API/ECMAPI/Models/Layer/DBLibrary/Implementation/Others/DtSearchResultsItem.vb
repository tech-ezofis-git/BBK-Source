
Public Class DtSearchResultsItem
    Implements IDtSearchResultsItem
    Protected _HitCount As String
    Protected _DisplayName As String
    Protected _DirectLink As String
    Protected _Dates As String
    Protected _Size As String
    Protected _Synopsis As String
    Protected _ifiletype As String
    Public Property DisplayName() As String Implements IDtSearchResultsItem.DisplayName
        Get

            Return _DisplayName
        End Get
        Set(value As String)

            If _DisplayName = value Then
                Return
            End If
            _DisplayName = value

        End Set
    End Property
    Public Property ifiletype() As String Implements IDtSearchResultsItem.ifiletype
        Get

            Return _ifiletype
        End Get
        Set(value As String)

            If _ifiletype = value Then
                Return
            End If
            _ifiletype = value

        End Set
    End Property
    Public Property HitCount() As String Implements IDtSearchResultsItem.HitCount
        Get

            Return _HitCount
        End Get
        Set(value As String)

            If _HitCount = value Then
                Return
            End If
            _HitCount = value

        End Set
    End Property
    Public Property Dates() As String Implements IDtSearchResultsItem.Dates
        Get

            Return _Dates
        End Get
        Set(value As String)

            If _Dates = value Then
                Return
            End If
            _Dates = value

        End Set
    End Property
    Public Property DirectLink() As String Implements IDtSearchResultsItem.DirectLink
        Get

            Return _DirectLink
        End Get
        Set(value As String)


            _DirectLink = value

        End Set
    End Property
    Public Property Size() As String Implements IDtSearchResultsItem.Size
        Get

            Return _Size
        End Get
        Set(value As String)

            If _Size = value Then
                Return
            End If
            _Size = value

        End Set
    End Property
    Public Property Synopsis() As String Implements IDtSearchResultsItem.Synopsis
        Get

            Return _Synopsis
        End Get
        Set(value As String)

            If _Synopsis = value Then
                Return
            End If
            _Synopsis = value

        End Set
    End Property

End Class