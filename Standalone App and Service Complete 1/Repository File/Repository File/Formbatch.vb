Imports System.Windows.Forms.Integration
Imports System.Windows.Forms
Public Class Formbatch
    Dim custommsgbox As New CustomMessageBoxControl
    Private Sub Formbatch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Try
            Dim host As New ElementHost()
            host.Dock = DockStyle.Fill
            ' Create the WPF UserControl. 
            Dim uc As New Batching
            ' Assign the WPF UserControl to the ElementHost control's 
            ' Child property.
            host.Child = uc
            ' Add the ElementHost control to the form's 
            ' collection of child controls. 
            Me.Controls.Add(host)
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception in btn_decrypt :" & ex.Message)
            'MsgBox("From Search :: " + ex.Message.ToString)
        End Try
    End Sub
End Class




