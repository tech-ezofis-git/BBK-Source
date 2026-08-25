Imports System.Windows.Forms.Integration
Imports System.Windows.Forms
Public Class Form1
    Dim custommsgbox As New CustomMessageBoxControl
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.Hide()
        Try
            Me.Hide()
            Dim repo As New Repos
            If repo.ShowDialog() Then
                Me.Hide()
            End If
            Me.Close()
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try

    End Sub
End Class
