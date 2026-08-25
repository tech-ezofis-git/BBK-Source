Imports ezofis.UserControl.CAC
Imports System.Configuration

Public Class SelectZonalFile
    Dim CAC As New CACserviceClient
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnSelectZonal.Click
        Try
            ezofis.UserControl.StrZonalFileName = ComboBox1.SelectedValue
            'Dim StrZonalFileName As String = ConfigSettings.loadConfigDocument().SelectSingleNode("//configuration//ZonalSettings//ZonalFilePath").Attributes("Value").Value.ToString.ToLower
            Me.DialogResult = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try
            ComboBox1.ItemsSource = CAC.SelectedeZZonalListByCabinetAndTemplateId("TemplateId='" & templateid & "' And CabinetId='" & cabinetid & "'")
            Dim screenWidth As Double = System.Windows.SystemParameters.PrimaryScreenWidth
            Dim screenHeight As Double = System.Windows.SystemParameters.PrimaryScreenHeight
            Dim windowWidth As Double = Me.Width
            Dim windowHeight As Double = Me.Height
            Me.Left = (screenWidth / 2) - (windowWidth / 2)
            Me.Top = (screenHeight / 2) - (windowHeight / 2)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BtnSaveZonal_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles BtnSaveZonal.Click
        If TxtZonalName.Text = String.Empty Then
            MsgBox("Please Enter The Zonal Name", vbInformation)
        Else
            ezofis.UserControl.StrZonalFileName = TxtZonalName.Text.ToString
            Me.DialogResult = True
        End If

    End Sub
End Class

