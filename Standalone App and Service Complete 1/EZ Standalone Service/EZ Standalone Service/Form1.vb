Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO
Imports Standalone_Service


Public Class Form1

    Dim Engine As New Standalone_Service.EncrptyDecrypt
    Private Sub btnencrypt_Click(sender As Object, e As EventArgs) Handles btnencrypt.Click
        Try
            'Engine.Encrypt()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Btndecrypt_Click(sender As Object, e As EventArgs) Handles Btndecrypt.Click
        Try
            'Engine.Decrypt()
            Dim logfilename = DateTime.Now.ToString("yyyyMMddhhmmsstt")
            Engine.filelocation = Engine.dir() & "\" & logfilename & ".txt"
            Engine.encryptdecrypt()
        Catch ex As Exception

        End Try
    End Sub
End Class