Imports System.Windows.Media

Public Class CustomMessageBoxControl
    Public Function showCustomMessageBox(ByVal msgtype As String, ByVal msg As String)
        Dim r As Integer = 0
        Dim g As Integer = 0
        Dim b As Integer = 0
        Dim textalignment As String = ""
        If (msgtype = "error") Then
            '150, 255, 0, 0
            r = 205
            g = 92
            b = 92
            textalignment = Windows.TextAlignment.Left
        Else
            r = 107
            g = 142
            b = 35
            textalignment = Windows.TextAlignment.Center
            ' rgb = rgb(r, g, b)
        End If

        Dim mw As New MessageWin(msg)
        If (msg = "Process Initiated Successfully!") Then
            mw.lblLog.Visibility = Windows.Visibility.Visible
        Else
            mw.lblLog.Visibility = Windows.Visibility.Hidden
        End If

        mw.lblmsg.Text = msg
        mw.lblmsg.TextAlignment = textalignment
        mw.winborder.BorderBrush = New SolidColorBrush(Color.FromArgb(255, r, g, b))
        mw.titlepanel.Background = New SolidColorBrush(Color.FromArgb(255, r, g, b))
        ''mw.titlewinborder.BorderBrush = New SolidColorBrush(Color.FromArgb(255, r, g, b))
        '' mw.Btn_Ok.Background = New SolidColorBrush(Color.FromArgb(255,r, g, b))
        ''rgb50  205	50
        ''rgb107	142	35
        mw.ShowDialog()
    End Function

    Public Function showCustomMessageBox(ByVal msgtype As String, ByVal msg As String, ByVal opt As String)
        Dim r As Integer = 0
        Dim g As Integer = 0
        Dim b As Integer = 0

        Dim textalignment As String = ""
        If (msgtype = "error") Then
            '150, 255, 0, 0
            r = 205
            g = 92
            b = 92
            textalignment = Windows.TextAlignment.Left
        Else
            r = 107
            g = 142
            b = 35
            textalignment = Windows.TextAlignment.Center
            ' rgb = rgb(r, g, b)
        End If
        Dim mw As New MessageWin(msg)
        If (opt = "yesno") Then
            mw.Btn_Ok.Content = "Yes"
            mw.Btn_No.Visibility = Windows.Visibility.Visible
            mw.lblmsg.TextAlignment = textalignment
        ElseIf (opt = "login") Then
            mw.Width = 650
            mw.Height = 250
            mw.winborder.Width = 500
            mw.winborder.Height = 200
        End If


        mw.lblmsg.Text = msg
        mw.lblmsg.TextAlignment = textalignment
        mw.winborder.BorderBrush = New SolidColorBrush(Color.FromArgb(255, r, g, b))
        mw.titlepanel.Background = New SolidColorBrush(Color.FromArgb(255, r, g, b))
        ''mw.titlewinborder.BorderBrush = New SolidColorBrush(Color.FromArgb(255, r, g, b))
        '' mw.Btn_Ok.Background = New SolidColorBrush(Color.FromArgb(255,r, g, b))
        ''rgb50  205	50
        ''rgb107	142	35
        mw.ShowDialog()
    End Function
End Class
