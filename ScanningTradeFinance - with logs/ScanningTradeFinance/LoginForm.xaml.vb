Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data
Imports System.Net
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports ScanningTradeFinance.CACServiceReference
Imports ScanningTradeFinance.publicvariables

Public Class LoginForm
    Protected Shared sharedCAC As New CACserviceClient
    'Public ecmlogin As eZECMLogin
    Dim ezLic As New eZLicense

    Dim Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
    Dim apiUrlInvita = Appcon("InvitaAPI").ToString()

    Public Sub New()

        ' This call is required by the designer.
        'CheckLicense()
        'txtusername.Focus()
        InitializeComponent()


        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Function updateinconfig(Key As String, Value As String)
        Try
            Dim configFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            Dim settings = configFile.AppSettings.Settings

            '  If settings(Key) Is Nothing Then
            'settings.Add(Key, Value)
            ' Else
            settings(Key).Value = Value
            'End If

            configFile.Save(ConfigurationSaveMode.Modified)
            ConfigurationManager.RefreshSection("appsettings")
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
        Catch ex As Exception
            AppLogger.Write("LoginForm.updateinconfig", ex.Message, ex)
            Throw ex

        End Try
    End Function
    Private Function CheckLicense()
        Try

            Dim Appcon As NameValueCollection = CType(ConfigurationSettings.GetConfig("appSettings"), NameValueCollection)

            Dim ArchivedDate As String = ""
            Dim IsTrail As Boolean = False
            Dim IsLicensed As Boolean = False
            Dim Enc_ArchivedDate As String = ""

            Dim chktrailorlic = "Trial"
            Try
                Enc_ArchivedDate = ezLic.Encrypt("ArchivedDate", "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                ArchivedDate = Appcon(Enc_ArchivedDate).ToString()
                Dim dd = ezLic.Decrypt("zLB9yMKdsSnV/IhEjfYiNQ==", "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                ArchivedDate = ezLic.Decrypt(ArchivedDate, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            Catch ex As Exception
                AppLogger.Write("LoginForm.CheckLicense.config", ex.Message, ex)
                MsgBox("Mismatch Configuration Alert!.Configuration failed..")
                ' MainWindow.Close()
                Me.Close()
                Exit Function
            End Try
            Dim currentDate = DateTime.Now.ToShortDateString
            If ArchivedDate = "" Then
                updateinconfig(Enc_ArchivedDate, ezLic.Encrypt("Trial_" + currentDate, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192))
                Appcon = DirectCast(System.Configuration.ConfigurationManager.GetSection("appSettings"), NameValueCollection)
                ArchivedDate = Appcon(Enc_ArchivedDate)
                IsTrail = True
            Else
                'Dim gh = String.Join("_", ArchivedDate.Split("_").Skip(1).ToArray())
                Dim trailDate = Convert.ToDateTime(String.Join("_", ArchivedDate.Split("_").Skip(1).ToArray()))
                If ArchivedDate.StartsWith("Trial") Then
                    If trailDate.AddDays(15) > DateTime.Now Then
                        IsTrail = True
                    Else
                        IsTrail = False
                    End If
                Else
                    If trailDate >= DateTime.Now Then
                        IsLicensed = True
                    Else
                        chktrailorlic = "License"
                    End If
                End If
            End If
            InitializeComponent()
            If IsTrail Or IsLicensed Then
                'InitializeComponent()

                If (IsTrail) Then
                    ShowControlsLicTrail()
                ElseIf IsLicensed Then
                    ShowControlsLicActivated()
                End If
            Else
                If chktrailorlic = "License" Then
                    MsgBox("License Expired.")
                    ShowLicenseExpiredCtrl()
                    'End
                Else
                    MsgBox("Trial Period Expired.")
                    ShowTrialExpiredCtrl()

                    'End
                End If
            End If

        Catch ex As Exception
            AppLogger.Write("LoginForm.CheckLicense", ex.Message, ex)
            MsgBox("Exception in CheckLicense " & ex.Message)
        End Try
    End Function
    Public Sub ShowControlsLicActivated()
        lblLicStatus.Visibility = Visibility.Visible
        lblLicStatus.Content = " "
        lblLicStatus.Foreground = Brushes.DarkGray
        Btn_Activate.Content = "Activated."
        Btn_Activate.IsEnabled = False
        Btn_Activate_License.Visibility = Visibility.Hidden
    End Sub
    Public Sub ShowControlsLicTrail()
        lblLicStatus.Visibility = Visibility.Visible
        lblLicStatus.Content = "Trial Version."
        lblLicStatus.Foreground = Brushes.OrangeRed
        Btn_Activate_License.Visibility = Visibility.Visible

    End Sub
    Public Sub ShowLicenseExpiredCtrl()
        Grid1.Visibility = Visibility.Collapsed
        LoginPanel.Visibility = Visibility.Collapsed
        LicensePanel.Visibility = Visibility.Visible

        txtusername.IsEnabled = False
        txtpassword.IsEnabled = False

        lblLicStatus.Visibility = Visibility.Visible
        lblLicStatus.Content = "License Expired."
        lblLicStatus.Foreground = Brushes.OrangeRed

    End Sub
    Public Sub ShowTrialExpiredCtrl()
        Grid1.Visibility = Visibility.Collapsed
        LoginPanel.Visibility = Visibility.Collapsed
        LicensePanel.Visibility = Visibility.Visible

        txtusername.IsEnabled = False
        txtpassword.IsEnabled = False

        lblLicStatus.Visibility = Visibility.Visible
        lblLicStatus.Content = "Trial Expired."
        lblLicStatus.Foreground = Brushes.OrangeRed

    End Sub
    Private Sub btnlogin_Click(sender As Object, e As RoutedEventArgs) Handles btnlogin.Click
        Try
            Dim invitaAPIobj As New ApiFunctions
            Dim Appcon2 As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)

            AppLogger.StartProcess("Login", "NA", True)
            AppLogger.LogStep("Login started | Username='" & txtusername.Text & "'")

            If (txtusername.Text <> "" And txtpassword.Password <> "") Then

                btnlogin.IsEnabled = False
                txtusername.IsEnabled = False
                txtpassword.IsEnabled = False

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                Dim starttime = Now
                If (lblresult.Text <> "") Then
                    lblresult.Text = ""
                End If
                ecmlogin = sharedCAC.UserLogin(txtusername.Text, txtpassword.Password, 1, "EZCapture", Environment.MachineName)

                Dim endtime = Now
                If Not ecmlogin Is Nothing Then
                    AppLogger.LogStep("WCF UserLogin succeeded | ECMLoginId=" & ecmlogin.ECMLoginId.ToString() & " | LoginName='" & ecmlogin.LoginName & "'")

                    Dim groupid = Appcon2("groupid")
                    If (groupid.ToString <> "") Then
                        Dim groupCheck = CheckUserInGroup(invitaAPIobj, ecmlogin.ECMLoginId.ToString(), groupid.ToString())
                        Dim apiInfo = GetPermissionApiDisplayInfo(Appcon2)
                        If groupCheck = GroupCheckResult.ApiFailed Then
                            AppLogger.LogStep("GetDatasetByQuery API failed after retries | endpoint=" & apiInfo.Endpoint & " | GroupId=" & groupid.ToString() & " | ECMLoginId=" & ecmlogin.ECMLoginId.ToString())
                            AppLogger.EndProcess("Login failed - GetDatasetByQuery API error")
                            lblresult.Text = BuildBbkLoginPermissionMessage(apiInfo)
                        ElseIf groupCheck = GroupCheckResult.InGroup Then
                            AppLogger.LogStep("Group permission check passed | GroupId=" & groupid.ToString())
                            lblusername.Content = ecmlogin.LoginName
                            lblusername1.Content = ecmlogin.LoginName
                            lblusername2.Content = ecmlogin.LoginName
                            If Passwordagevalidation(ecmlogin.ECMLoginId.ToString()) Then
                                AppLogger.LogStep("Password age validation passed")
                                AppLogger.EndProcess("Login success")
                                Me.DialogResult = True
                            Else
                                AppLogger.LogStep("Password age validation failed | ECMLoginId=" & ecmlogin.ECMLoginId.ToString())
                                AppLogger.EndProcess("Login failed - password age")
                            End If
                        Else
                            AppLogger.LogStep("Group permission check failed | GroupId=" & groupid.ToString() & " | ECMLoginId=" & ecmlogin.ECMLoginId.ToString() & " | endpoint=" & apiInfo.Endpoint)
                            AppLogger.EndProcess("Login failed - no permission")
                            lblresult.Text = BuildEzofisLoginPermissionMessage(apiInfo, txtusername.Text, Appcon2)
                        End If
                    Else
                        AppLogger.LogStep("GroupId not configured - allowing login")
                        AppLogger.EndProcess("Login success (no group check)")
                        Me.DialogResult = True
                    End If

                Else
                    AppLogger.LogStep("WCF UserLogin returned Nothing - invalid credentials")
                    AppLogger.EndProcess("Login failed - invalid login")
                    lblresult.Text = "Invalid Login"
                End If
            Else
                AppLogger.LogStep("Login aborted - username or password empty")
                AppLogger.EndProcess("Login failed - missing credentials")
                lblresult.Text = "Please Enter Login Information..."
            End If

        Catch ex As Exception
            LogCaughtException("btnlogin_Click", ex)
            AppLogger.EndProcess("Login failed - exception")
            lblresult.Text = "Invalid Login " + ex.Message
        Finally
            btnlogin.IsEnabled = True
            txtusername.IsEnabled = True
            txtpassword.IsEnabled = True
        End Try
    End Sub

    Private Enum GroupCheckResult
        InGroup
        NotInGroup
        ApiFailed
    End Enum

    Private Class PermissionApiDisplayInfo
        Public Domain As String = ""
        Public ApiName As String = "EZAPI"
        Public Endpoint As String = ""
        Public SwaggerUrl As String = ""
        Public HttpMethod As String = "POST"
        Public MethodName As String = "GetDatasetByQuery"
    End Class

    Private Function GetPermissionApiDisplayInfo(appcon As NameValueCollection) As PermissionApiDisplayInfo
        Dim info As New PermissionApiDisplayInfo()
        Try
            Dim baseUrl = ""
            If appcon IsNot Nothing AndAlso appcon("InvitaAPI") IsNot Nothing Then
                baseUrl = appcon("InvitaAPI").ToString().Trim().TrimEnd("/"c)
            End If
            info.Endpoint = baseUrl & "/v1/Common/GetDatasetByQuery"
            info.SwaggerUrl = If(String.IsNullOrWhiteSpace(baseUrl), "", baseUrl & "/swagger")

            Dim u As Uri = Nothing
            If Uri.TryCreate(baseUrl, UriKind.Absolute, u) Then
                info.Domain = u.Host
                Dim lastSeg = u.AbsolutePath.Trim("/"c)
                If lastSeg.Contains("/") Then
                    lastSeg = lastSeg.Substring(lastSeg.LastIndexOf("/"c) + 1)
                End If
                If Not String.IsNullOrWhiteSpace(lastSeg) Then
                    info.ApiName = lastSeg
                End If
            Else
                info.Domain = baseUrl
            End If
        Catch ex As Exception
            LogCaughtException("GetPermissionApiDisplayInfo", ex)
        End Try
        Return info
    End Function

    Private Function GetConfiguredGroupName(appcon As NameValueCollection) As String
        Try
            If appcon IsNot Nothing AndAlso appcon("ECMGroup") IsNot Nothing AndAlso
               Not String.IsNullOrWhiteSpace(appcon("ECMGroup").ToString()) Then
                Return appcon("ECMGroup").ToString().Trim()
            End If
            If appcon IsNot Nothing AndAlso appcon("groupid") IsNot Nothing Then
                Return "Group " & appcon("groupid").ToString().Trim()
            End If
        Catch ex As Exception
            LogCaughtException("GetConfiguredGroupName", ex)
        End Try
        Return "Group"
    End Function

    ''' <summary>
    ''' UI-facing Additional Info line: Domain + Swagger URL (not the API endpoint).
    ''' </summary>
    Private Function FormatDomainSwaggerInfo(info As PermissionApiDisplayInfo) As String
        Return "Domain: " & info.Domain & "   Swagger URL:  " & info.SwaggerUrl
    End Function

    Private Function BuildBbkLoginPermissionMessage(info As PermissionApiDisplayInfo) As String
        Return "Unable to complete the request." & Environment.NewLine &
               "We're unable to verify the required permissions at this time. This may be related to the server, API availability, or API permissions. Please try again later or contact the support team if the issue persists." & Environment.NewLine &
               "Reference: BBK Team (Additional Info: " & FormatDomainSwaggerInfo(info) & ")"
    End Function

    Private Function BuildEzofisLoginPermissionMessage(info As PermissionApiDisplayInfo, loginUser As String, appcon As NameValueCollection) As String
        Dim userName = If(String.IsNullOrWhiteSpace(loginUser), "", loginUser.Trim())
        Dim groupName = GetConfiguredGroupName(appcon)
        Return "Access Denied." & Environment.NewLine &
               "You do not have the required permissions to access this application. Please contact your administrator or support team to request access." & Environment.NewLine &
               "Reference: EZOFIS Team (Additional Info: Login User: " & userName & " | Not in Group: " & groupName & " | " & FormatDomainSwaggerInfo(info) & ")"
    End Function

    Private Sub LogCaughtException(context As String, ex As Exception)
        Try
            AppLogger.LogException("LoginForm." & context, ex)
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Retries on indeterminate API results (Nothing / 0 tables).
    ''' Only treats a valid 1-table 0-row response as definitive "not in group".
    ''' </summary>
    Private Function CheckUserInGroup(invitaAPIobj As ApiFunctions, ecmLoginId As String, groupid As String) As GroupCheckResult
        Try
            Dim sqlqry = "select * from eZECMGroupUsers where ECMLoginId='" & ecmLoginId & "' and ECMGroupId='" & groupid & "' and isdeleted=0"
            AppLogger.LogStep("Group membership check | GroupId=" & groupid & " | ECMLoginId=" & ecmLoginId)
            Dim ResEcmLogin = invitaAPIobj.GetDatasetByQuery(sqlqry, showSupportMessage:=False)

            If ResEcmLogin Is Nothing OrElse ResEcmLogin.Tables.Count = 0 Then
                AppLogger.LogStep("Group check API failed after retries (Nothing or 0 tables) | GroupId=" & groupid & " | ECMLoginId=" & ecmLoginId)
                Return GroupCheckResult.ApiFailed
            End If

            If ResEcmLogin.Tables(0).Rows.Count > 0 Then
                Return GroupCheckResult.InGroup
            End If

            AppLogger.LogStep("Group check definitive empty result (1+ tables, 0 rows) | GroupId=" & groupid & " | ECMLoginId=" & ecmLoginId)
            Return GroupCheckResult.NotInGroup
        Catch ex As Exception
            LogCaughtException("CheckUserInGroup", ex)
            Return GroupCheckResult.ApiFailed
        End Try
    End Function

    Private Sub btncancel_Click(sender As Object, e As RoutedEventArgs) Handles btncancel.Click
        Try
            ecmlogin = New eZECMLogin()
            Me.DialogResult = False
        Catch ex As Exception
            LogCaughtException("btncancel_Click", ex)
        End Try
    End Sub

    Private Sub EnterClicked(sender As Object, e As KeyEventArgs)
        If e.Key = Key.[Return] Then
            Try
                Dim invitaAPIobj As New ApiFunctions
                Dim Appcon2 As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)

                AppLogger.StartProcess("Login", "NA", True)
                AppLogger.LogStep("Login started via Enter key | Username='" & txtusername.Text & "'")

                Dim CAC As New CACserviceClient
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                ecmlogin = CAC.UserLogin(txtusername.Text, txtpassword.Password, 1, "ECM-Capture", Environment.MachineName)
                If Not ecmlogin Is Nothing Then
                    AppLogger.LogStep("WCF UserLogin succeeded | ECMLoginId=" & ecmlogin.ECMLoginId.ToString() & " | LoginName='" & ecmlogin.LoginName & "'")

                    Dim groupid = Appcon2("groupid")
                    If (groupid.ToString <> "") Then
                        Dim groupCheck = CheckUserInGroup(invitaAPIobj, ecmlogin.ECMLoginId.ToString(), groupid.ToString())
                        Dim apiInfo = GetPermissionApiDisplayInfo(Appcon2)
                        If groupCheck = GroupCheckResult.ApiFailed Then
                            AppLogger.LogStep("GetDatasetByQuery API failed after retries | endpoint=" & apiInfo.Endpoint & " | GroupId=" & groupid.ToString() & " | ECMLoginId=" & ecmlogin.ECMLoginId.ToString())
                            AppLogger.EndProcess("Login failed - GetDatasetByQuery API error")
                            lblresult.Text = BuildBbkLoginPermissionMessage(apiInfo)
                        ElseIf groupCheck = GroupCheckResult.InGroup Then
                            AppLogger.LogStep("Group permission check passed | GroupId=" & groupid.ToString())
                            If Passwordagevalidation(ecmlogin.ECMLoginId.ToString()) Then
                                AppLogger.LogStep("Password age validation passed")
                                AppLogger.EndProcess("Login success")
                                Me.DialogResult = True
                            Else
                                AppLogger.LogStep("Password age validation failed | ECMLoginId=" & ecmlogin.ECMLoginId.ToString())
                                AppLogger.EndProcess("Login failed - password age")
                            End If
                        Else
                            AppLogger.LogStep("Group permission check failed | GroupId=" & groupid.ToString() & " | ECMLoginId=" & ecmlogin.ECMLoginId.ToString() & " | endpoint=" & apiInfo.Endpoint)
                            AppLogger.EndProcess("Login failed - no permission")
                            lblresult.Text = BuildEzofisLoginPermissionMessage(apiInfo, txtusername.Text, Appcon2)
                        End If
                    Else
                        AppLogger.LogStep("GroupId not configured - allowing login")
                        AppLogger.EndProcess("Login success (no group check)")
                        Me.DialogResult = True
                    End If
                Else
                    AppLogger.LogStep("WCF UserLogin returned Nothing - invalid credentials")
                    AppLogger.EndProcess("Login failed - invalid login")
                    lblresult.Text = "Invalid Login"
                End If

            Catch ex As Exception
                LogCaughtException("EnterClicked", ex)
                AppLogger.EndProcess("Login failed - exception")
                lblresult.Text = "Invalid Login " + ex.Message
            End Try
            e.Handled = True
        End If
    End Sub

    Private Sub Btn_Activate_License_Click(sender As Object, e As RoutedEventArgs) Handles Btn_Activate_License.Click
        LicensePanel.Visibility = Visibility.Visible
        txtLicense.Text = ""
        LoginPanel.Visibility = Visibility.Collapsed
        Grid1.Visibility = Visibility.Collapsed
    End Sub

    Private Sub Btn_Activate_Click(sender As Object, e As RoutedEventArgs) Handles Btn_Activate.Click
        Try
            If (txtLicense.Text <> "") Then
                If (ezLic.ActivateLicense(txtLicense.Text) = "Success") Then
                    ShowControlsLicActivated()
                    MsgBox("License Key Activated Successfully")
                Else
                    MsgBox("Please Enter Valid License Key")
                End If
            Else
                MsgBox("Please Enter the License Key")
            End If
            ' Me.Close()
        Catch ex As Exception
            LogCaughtException("Btn_Activate_Click", ex)
            MsgBox("Please Enter Valid License Key")
        End Try
    End Sub

    Private Sub Btn_Activate_Cancel_Click(sender As Object, e As RoutedEventArgs) Handles Btn_Activate_Cancel.Click
        LicensePanel.Visibility = Visibility.Collapsed
        LoginPanel.Visibility = Visibility.Visible
        Grid1.Visibility = Visibility.Visible
    End Sub

    Public Function Passwordagevalidation(ByVal loginid As String) As Boolean
        Try
            Dim sqlquery = "select LastPasswordUpdate from eZECMLogin where ECMLoginId = " + loginid + " and Isdeleted=0"
            Dim ds As DataSet = sharedCAC.GetDatasetByQuery(sqlquery)
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
            LogCaughtException("Passwordagevalidation", ex)
            Return False
        End Try
    End Function

    Private Sub btnProceedlogin_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.DialogResult = True
        Catch ex As Exception
            LogCaughtException("btnProceedlogin_Click", ex)
        End Try
    End Sub

    Private Sub btnChangePW_Click(sender As Object, e As RoutedEventArgs)
        Try
            Grid2.Visibility = Visibility.Collapsed
            Grid3.Visibility = Visibility.Visible
        Catch ex As Exception
            LogCaughtException("btnChangePW_Click", ex)
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
                            Dim json = client.UploadString(apiUrlInvita + "/v1/User/UpdateUserPassword", "PATCH", inputJson)
                            sharedCAC.InsertAndUpdate("Update ezecmlogin set LastPasswordUpdate ='" + Date.Now + "' where ECMLoginId = " + ecmlogin.ECMLoginId.ToString() + " and Isdeleted=0")
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
            LogCaughtException("btnUpdate_Click", ex)
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

    Private Sub btnloin_Click(sender As Object, e As RoutedEventArgs) Handles btnloin.Click

        Try
            Me.DialogResult = True
        Catch ex As Exception
            LogCaughtException("btnloin_Click", ex)
        End Try

    End Sub

End Class
