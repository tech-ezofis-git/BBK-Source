Imports ScanningTradeFinance.publicvariables

Public Class ScannerNotify

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        lblworkitem.Content = ScannedPageCount.ToString()
        ' lblpags.Content = Pagescou.ToString() + " Pages"
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub btnNewWorkItem_Click(sender As Object, e As RoutedEventArgs)
        Try
            CanContinue = 2
            Me.DialogResult = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnContinue_Click(sender As Object, e As RoutedEventArgs)
        Try
            CanContinue = 1
            Me.DialogResult = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
        Try
            CanContinue = 1
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
