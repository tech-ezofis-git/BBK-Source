Public Class Versioning


    Dim versionfile As String
    Dim Rimnum As String
    Public Sub New(filename As String, Rimnumber As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        versionfile = filename
        Rimnum = Rimnumber
        cancontinue = 0
    End Sub

    Private Sub Versioning_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        'lblfilename.Content = "File : " + versionfile + " for the RIM Number(" + Rimnum + ") Exisiting on the FileRepository. "
        lblfilename.Content = versionfile
        lblrim.Content = Rimnum
        btnsave.IsEnabled = False
    End Sub

    Private Sub btnsave_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If btnskip.IsChecked Then
                cancontinue = 0
            ElseIf btnversion.IsChecked Then
                cancontinue = 1
            ElseIf btnreplace.IsChecked Then
                cancontinue = 2
            End If
            Me.Close()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Btnskip_Checked(sender As Object, e As RoutedEventArgs)
        Try
            btnsave.IsEnabled = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Btnreplace_Checked(sender As Object, e As RoutedEventArgs)
        Try
            btnsave.IsEnabled = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Btnversion_Checked(sender As Object, e As RoutedEventArgs)
        Try
            btnsave.IsEnabled = True
        Catch ex As Exception

        End Try
    End Sub
End Class
