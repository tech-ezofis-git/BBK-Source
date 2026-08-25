Imports ezofis.UserControl.CAC
Imports System.Configuration
Imports System.Collections.Specialized

Public Class PaperSettings
    Dim CAC As New CACserviceClient
    Dim Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("Database"), NameValueCollection)
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try

            Dim screenWidth As Double = System.Windows.SystemParameters.PrimaryScreenWidth
            Dim screenHeight As Double = System.Windows.SystemParameters.PrimaryScreenHeight
            Dim windowWidth As Double = Me.Width
            Dim windowHeight As Double = Me.Height
            Me.Left = (screenWidth / 2) - (windowWidth / 2)
            Me.Top = (screenHeight / 2) - (windowHeight / 2)
            Dim FileFormet As String = ""
            FileFormet = Appcon("FileFormet")
            If FileFormet.ToUpper = "BW-CCITTGROUP4" Then
                RadioButton1.IsChecked = True
            ElseIf FileFormet.ToUpper = "C-TIFLZW" Then
                RadioButton2.IsChecked = True
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub BtnSaveZonal_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnSaveZonal.Click
       
        Try
            If RadioButton1.IsChecked Then
                ConfigSettings.WriteSetting("FileFormet", "BW-CCITTGROUP4")
                Fileformet = "BW-CCITTGROUP4"
                _bitsPerPixel = 1
            ElseIf RadioButton2.IsChecked Then
                'ConfigSettings.WriteSetting("FileFormet", "C-TIFLZW")
                Fileformet = "C-TIFLZW"
                _bitsPerPixel = 4
            End If
            Me.DialogResult = True
        Catch ex As Exception

        End Try

    End Sub
End Class

