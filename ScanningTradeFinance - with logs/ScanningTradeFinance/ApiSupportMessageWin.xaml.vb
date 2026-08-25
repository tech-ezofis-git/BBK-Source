Imports System.Windows
Imports System.Windows.Input
Imports System.Windows.Media

Public Class ApiSupportMessageWin

    Public Sub New(teamHint As String, functionName As String, calledFrom As String, swaggerUrl As String, detail As String)
        InitializeComponent()

        Dim summary = If(teamHint, "").Trim()
        lblSummary.Text = summary
        lblFunction.Text = If(functionName, "")
        lblCalledFrom.Text = If(calledFrom, "")
        lblSwaggerUrl.Text = If(swaggerUrl, "")
        lblDetail.Text = If(detail, "")

        ApplyTeamStyle(summary)
    End Sub

    Private Sub ApplyTeamStyle(summary As String)
        Try
            Dim upper = summary.ToUpperInvariant()
            If upper.Contains("BBK") Then
                lblTeamBadge.Text = "BBK Team"
                badgeTeam.Background = New SolidColorBrush(Color.FromRgb(&HFE, &HF3, &HC7))
                lblTeamBadge.Foreground = New SolidColorBrush(Color.FromRgb(&H92, &H40, &H0))
            ElseIf upper.Contains("EZOFIS") Then
                lblTeamBadge.Text = "Ezofis Team"
                badgeTeam.Background = New SolidColorBrush(Color.FromRgb(&HE0, &HE7, &HFF))
                lblTeamBadge.Foreground = New SolidColorBrush(Color.FromRgb(&H37, &H30, &HA3))
            Else
                lblTeamBadge.Text = "Support"
            End If
        Catch
        End Try
    End Sub

    Private Sub btnOk_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.DialogResult = True
        Catch
            Me.Close()
        End Try
    End Sub

    Private Sub Header_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If e.ChangedButton = MouseButton.Left Then
                Me.DragMove()
            End If
        Catch
        End Try
    End Sub

End Class
