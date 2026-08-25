Imports ScanningTradeFinance.publicvariables
Public Class MessageWin

    Public Sub New(ByVal msg As String)

        ' This call is required by the designer.
        InitializeComponent()
        lblmsg.Text = msg
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnyes_Click(sender As Object, e As RoutedEventArgs)
        Try
            NewWorkItem = 1
            Me.DialogResult = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnno_Click(sender As Object, e As RoutedEventArgs)
        Try
            NewWorkItem = 2
            Me.DialogResult = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
        Try
            NewWorkItem = 0
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mainhead_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Try
            Application.Current.MainWindow.DragMove()
        Catch ex As Exception

        End Try
    End Sub
End Class
