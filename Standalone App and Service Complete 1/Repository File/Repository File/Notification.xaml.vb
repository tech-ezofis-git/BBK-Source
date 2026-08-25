Imports System.Windows


Public Class Notification
    Public Sub New()
        Try
            InitializeComponent()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub OKbtn_Click(sender As Object, e As RoutedEventArgs) Handles OKbtn.Click
        Me.Hide()
    End Sub

    Private Sub Btn_cancel_Click(sender As Object, e As RoutedEventArgs) Handles Btn_cancel.Click
        Me.Hide()
    End Sub

    'Private Sub Closebtn_Click(sender As Object, e As RoutedEventArgs) Handles closebtn.Click
    '    Me.Hide()
    'End Sub
End Class
