Imports System.Windows
Imports Repository_File.Pubvar

Public Class MessageWin
    Public Sub New(ByVal msg As String)
        Try
            InitializeComponent()
            lblmsg.Text = msg
        Catch ex As Exception
            MsgBox("Exception in MessageWin Costructor : " & ex.Message)
        End Try
    End Sub
    'Public Sub New()
    '    Try
    '        InitializeComponent()

    '    Catch ex As Exception
    '        MsgBox("Exception in MessageWin Costructor : " & ex.Message)
    '    End Try
    'End Sub

    Private Sub Btn_Ok_Click(sender As Object, e As RoutedEventArgs) Handles Btn_Ok.Click
        Try
            CustomMessageBoxResult = 1
            Me.Close()
        Catch ex As Exception
            MsgBox("Exception in Btn_Ok_Click : " & ex.Message)
        End Try

    End Sub

    Private Sub Btn_cancel_Click(sender As Object, e As RoutedEventArgs) Handles Btn_cancel.Click
        Try
            CustomMessageBoxResult = 0
            Me.Close()
        Catch ex As Exception
            MsgBox("Exception in Btn_cancel_Click : " & ex.Message)
        End Try

    End Sub

    Private Sub Btn_No_Click(sender As Object, e As RoutedEventArgs) Handles Btn_No.Click
        Try
            CustomMessageBoxResult = 0
            Me.Close()
        Catch ex As Exception
            MsgBox("Exception in Btn_No_Click : " & ex.Message)
        End Try
    End Sub
End Class
