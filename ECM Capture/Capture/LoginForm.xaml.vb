Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data
Imports System.Net
Imports System.Text.RegularExpressions
Imports ezofis.UserControl.CAC
Imports Newtonsoft.Json

Public Class LoginForm

    Shared CAC As New CACserviceClient
    Dim Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
    Dim apiUrl As String = Appcon("api").ToString
    Public Shared loggedfrom As String
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        txtusername.Focus()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnlogin_Click(sender As Object, e As RoutedEventArgs) Handles btnlogin.Click
        Try

            Dim CAC As New CACserviceClient
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            ecmlogin = CAC.UserLogin(txtusername.Text, txtpassword.Password, 1, "ECM-Capture", Environment.MachineName)
            If Not ecmlogin Is Nothing Then




                'If ecmlogin.ECMProfileId = 13 OrElse ecmlogin.ECMProfileId = 11 OrElse ecmlogin.ECMProfileId = 12 OrElse ecmlogin.ECMProfileId = 1 Then

                loggedfrom = ecmlogin.LoginName
                    lblusername.Content = ecmlogin.LoginName
                    lblusername1.Content = ecmlogin.LoginName
                    lblusername2.Content = ecmlogin.LoginName

                    If Passwordagevalidation(ecmlogin.ECMLoginId.ToString()) Then
                        Me.DialogResult = True
                    Else

                    End If
                'Else
                '    lblresult.Text = "You don't have permission to access this application"
                'End If

            Else
                lblresult.Text = "Invalid Login"
            End If


        Catch ex As Exception
            lblresult.Text = "Invalid Login " + ex.Message
        End Try
    End Sub

    Private Sub btncancel_Click(sender As Object, e As RoutedEventArgs) Handles btncancel.Click
        Try
            ecmlogin = New eZECMLogin()
            Me.DialogResult = False

        Catch ex As Exception

        End Try
    End Sub

    Private Sub EnterClicked(sender As Object, e As KeyEventArgs)
        If e.Key = Key.[Return] Then
            Try

                Dim CAC As New CACserviceClient
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                ecmlogin = CAC.UserLogin(txtusername.Text, txtpassword.Password, 1, "ECM-Capture", Environment.MachineName)
                If Not ecmlogin Is Nothing Then
                    'If ecmlogin.ECMProfileId = 13 OrElse ecmlogin.ECMProfileId = 11 OrElse ecmlogin.ECMProfileId = 12 OrElse ecmlogin.IsFaxUser = 1 Then
                    lblusername.Content = ecmlogin.LoginName
                    lblusername1.Content = ecmlogin.LoginName
                    lblusername2.Content = ecmlogin.LoginName
                    If Passwordagevalidation(ecmlogin.ECMLoginId.ToString()) Then
                        Me.DialogResult = True
                    Else

                    End If

                    'Else
                    '    lblresult.Text = "You don't have permission to access this application"
                    'End If
                Else
                    lblresult.Text = "Invalid Login"
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message)
                lblresult.Text = "Invalid Login " + ex.Message
            End Try
            e.Handled = True
        End If
    End Sub

    Public Function Passwordagevalidation(ByVal loginid As String) As Boolean
        Try
            Dim sqlquery = "select LastPasswordUpdate from eZECMLogin where ECMLoginId = " + loginid + " and Isdeleted=0"
            Dim ds As DataSet = CAC.GetDatasetByQuery(sqlquery)
            If Not IsNothing(ds) AndAlso ds.Tables.Count <> 0 AndAlso ds.Tables(0).Rows.Count <> 0 Then
                Dim LastPasswordUpdate = ds.Tables(0).Rows(0)("LastPasswordUpdate").ToString()
                Dim countdays = 0

                Dim startdate = Date.Now
                Dim enddate = Date.Parse(LastPasswordUpdate)
                Dim diff = startdate - enddate
                countdays = diff.Days

                If countdays < 85 Then
                    Return True
                ElseIf countdays >= 85 AndAlso countdays <= 89 Then
                    If countdays = 85 Then
                        lbldays.Content = "5"
                    ElseIf countdays = 86 Then
                        lbldays.Content = "4"
                    ElseIf countdays = 87 Then
                        lbldays.Content = "3"
                    ElseIf countdays = 88 Then
                        lbldays.Content = "2"
                    ElseIf countdays = 89 Then
                        lbldays.Content = "1"
                    End If
                    Grid1.Visibility = Visibility.Collapsed
                    Grid2.Visibility = Visibility.Visible
                    Return False
                Else
                    Grid1.Visibility = Visibility.Collapsed
                    Grid2.Visibility = Visibility.Collapsed
                    Grid3.Visibility = Visibility.Visible
                    Return False
                End If
            Else
                Return False
            End If

        Catch ex As Exception

        End Try
    End Function

    Private Sub btnProceedlogin_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.DialogResult = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnChangePW_Click(sender As Object, e As RoutedEventArgs)
        Try
            Grid2.Visibility = Visibility.Collapsed
            Grid3.Visibility = Visibility.Visible
        Catch ex As Exception

        End Try
    End Sub

    Public Class ByCriteria

        Public Property ECMLoginId As String
        Public Property Password As String

    End Class

    Private Sub btnUpdate_Click(sender As Object, e As RoutedEventArgs)
        Try
            If txtnewpw.Password <> "" Then
                If txConfirmpassword.Password <> "" Then
                    If ValidatePassword(txtnewpw.Password) Then
                        If txtnewpw.Password = txConfirmpassword.Password Then

                            Dim Input = New ByCriteria()
                            Input.ECMLoginId = ecmlogin.ECMLoginId.ToString()
                            Input.Password = txtnewpw.Password.ToString()
                            Dim client = New WebClient()
                            client.Headers("Content-Type") = "application/json"
                            client.Encoding = System.Text.Encoding.UTF8
                            Dim inputJson = JsonConvert.SerializeObject(Input)
                            Dim json = client.UploadString(apiUrl + "/v1/User/UpdateUserPassword", "PATCH", inputJson)
                            CAC.InsertAndUpdate("Update ezecmlogin set LastPasswordUpdate ='" + Date.Now + "' where ECMLoginId = " + ecmlogin.ECMLoginId.ToString() + " and Isdeleted=0")
                            Grid3.Visibility = Visibility.Collapsed
                            Grid4.Visibility = Visibility.Visible
                        Else
                            lblresult1.Text = "Confirmed password mismatch"
                        End If
                    Else
                        lblresult1.Text = "Password Requirement does not match"
                    End If
                Else
                    lblresult1.Text = "Please provide a confirm password"
                End If
            Else
                lblresult1.Text = "Please provide a valid new password"
            End If




        Catch ex As Exception

        End Try
    End Sub

    Function ValidatePassword(ByVal pwd As String,
        Optional ByVal minLength As Integer = 8,
        Optional ByVal numUpper As Integer = 1,
        Optional ByVal numLower As Integer = 1,
        Optional ByVal numNumbers As Integer = 1,
        Optional ByVal numSpecial As Integer = 1) As Boolean

        ' Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
        Dim upper As New System.Text.RegularExpressions.Regex("[A-Z]")
        Dim lower As New System.Text.RegularExpressions.Regex("[a-z]")
        Dim number As New System.Text.RegularExpressions.Regex("[0-9]")
        ' Special is "none of the above".
        Dim special As New System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]")

        ' Check the length.
        If Len(pwd) < minLength Then Return False
        ' Check for minimum number of occurrences.
        If upper.Matches(pwd).Count < numUpper Then Return False
        If lower.Matches(pwd).Count < numLower Then Return False
        If number.Matches(pwd).Count < numNumbers Then Return False
        If special.Matches(pwd).Count < numSpecial Then Return False

        ' Passed all checks.
        Return True
    End Function

    Private Sub btnloin_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.DialogResult = True
        Catch ex As Exception

        End Try
    End Sub
End Class
