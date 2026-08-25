Public Class Message
    Dim tot As String = ""
    Dim export As String = ""
    Dim filelocation As String = ""
    Public Sub New(ByVal totalcount As String, ByVal exportcount As String, ByVal logfilepath As String)
        ' This call is required by the designer.
        InitializeComponent()
        lbltot.Content = totalcount
        lblexported.Content = exportcount
        filelocation = logfilepath
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub ViewLog_Click(sender As Object, e As RoutedEventArgs)
        Try
            System.Diagnostics.Process.Start(filelocation)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Btnclose_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class
