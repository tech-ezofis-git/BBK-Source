Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Windows
Imports System.Windows.Input
Imports System.Configuration
Imports System.Data
Imports System.Windows.Media
Imports System.ServiceProcess
Imports System.Collections.Specialized
Imports System.IO
Imports Repository_File.Pubvar

Public Class Repos
    Public FilePath As String = ""
    Dim Dirr As String
    'Service name  read
    Dim Appcon As NameValueCollection = DirectCast(System.Configuration.ConfigurationManager.GetSection("Database"), NameValueCollection)
    Dim servicename = Appcon("ServiceName").ToString()
    Dim impersonateMethod = Appcon("impersonate")
    'Imporsanate Method
    Dim Appconimporsanate As NameValueCollection = DirectCast(System.Configuration.ConfigurationManager.GetSection("impersonateMethod"), NameValueCollection)

    Dim imporsanateUsername = Appconimporsanate("Username")
    Dim imporsanatePassword = Appconimporsanate("Password")
    Dim imporsanateDomainname = Appconimporsanate("Domainname")

    Dim AppconUNC As NameValueCollection = DirectCast(System.Configuration.ConfigurationManager.GetSection("UNCMethod"), NameValueCollection)
    Dim UNCUsername = AppconUNC("Username")
    Dim UNCPath = AppconUNC("PathUNC")
    Dim UNCPassword = AppconUNC("Password")
    Dim Domainname = AppconUNC("Domainname")

    Dim sc As ServiceController = New ServiceController(servicename, Environment.MachineName)
    Private scServices() As ServiceController
    Dim procstat As New List(Of String)
    Dim servstat As New List(Of String)
    Dim proclist As String()
    Dim servlist As String()

    Dim custommsgbox As New CustomMessageBoxControl

    Public Sub New()
        'InitializeComponent()
        Try
            Me.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight
            scServices = ServiceController.GetServices()
            Dim ctl As ServiceController = ServiceController.GetServices().FirstOrDefault(Function(s) s.ServiceName = servicename)
            If ctl Is Nothing Then
                custommsgbox.showCustomMessageBox("Info", "Standalone Service is NOT INSTALLED." & vbCrLf & "Please install the Standalone Service..!")
                ' MsgBox("Standalone Service is NOT INSTALLED.Please install the Standalone Service..!", vbOKOnly, "STANDALONE EXPLORER:Notification")
                Form1.Close()
                Me.Close()
            ElseIf ctl.Status = ServiceControllerStatus.Stopped Then
                custommsgbox.showCustomMessageBox("Info", "Standalone Service is NOT RUNNING." & vbCrLf & "Please START the service..!")
                'MsgBox("Standalone Service is NOT RUNNING.Please START the service..!", vbOKOnly, "STANDALONE EXPLORER:Notification")
                Form1.Close()
                Me.Close()
            ElseIf ctl.Status = ServiceControllerStatus.Running Then
                InitializeComponent()
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub Btn_choosefolder_Click(sender As Object, e As Windows.RoutedEventArgs) Handles Btn_choosefolder.Click
        Dim open As New FolderBrowserDialog()
        Try
            If (open.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
                Repname.Text = open.SelectedPath
                Dirr = Repname.Text.ToString()
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub Btn_getcon_Click(sender As Object, e As Windows.RoutedEventArgs) Handles Btn_getcon.Click

        Try
            ' Me.Hide()
            scServices = ServiceController.GetServices()
            Dim UNCs As ConnectUNCWithCredentials = New ConnectUNCWithCredentials()
            If Dirr = "" Then
                Dirr = Repname.Text
            End If
            FilePath = Dirr.Replace("""", "")
            If (imporsanateUsername = "") Then
                imporsanateUsername = username1.Text.ToString()
                imporsanatePassword = password.Password.ToString()
                imporsanateDomainname = txtdomain.Text.ToString
            End If
            If impersonateMethod = "True" Then
                Try
                    Dim acct As AliasAccount
                    Dim impersonate As Boolean = False
                    acct = New AliasAccount(Appcon("Username"), Appcon("Password"), Appcon("Domain"))
                    'acct = New AliasAccount(imporsanateUsername, imporsanatePassword, imporsanateDomainname)
                    Try
                        acct.BeginImpersonation()
                        impersonate = True
                    Catch ex As Exception
                    End Try
                    If impersonate Then
                        custommsgbox.showCustomMessageBox("Info", "Connection Succeeded." & vbCrLf & "Press 'OK' to Continue")
                        'MsgBox("Impersonate Successufully", vbOKOnly, "STANDALONE EXPLORER:Notification")
                        If FilePath <> "" Then
                            'If (Directory.GetFiles(Repname.Text + "\", "*.ezo", IO.SearchOption.AllDirectories).Count > 0) Then
                            Me.Hide()
                            Dim frm As Form2 = New Form2()
                            frm.repopath = FilePath
                            Dim bytKey As Byte()
                            Dim bytIV As Byte()
                            bytKey = CreateKey(imporsanatePassword)
                            bytIV = CreateIV(imporsanatePassword)
                            frm.key = imporsanatePassword
                            If frm.ShowDialog() = False Then
                                frm.Close()
                            End If
                            'Else
                            '    MsgBox("Folder Location is Empty", vbOKOnly, "STANDALONE EXPLORER:Notification")
                            'End If
                        End If
                        Form1.Close()
                    Else
                        'MsgBox("impersonate Not Connected", vbOKOnly, "STANDALONE EXPLORER:Notification")
                        custommsgbox.showCustomMessageBox("Info", "Impersonate Not Connected")
                    End If
                    If impersonate Then
                        acct.EndImpersonation()
                    End If
                Catch ex As Exception
                    custommsgbox.showCustomMessageBox("error", "Invalid Location")
                    ' MsgBox("Invalid Location", vbOKOnly, "STANDALONE EXPLORER:Notification")
                End Try
            ElseIf impersonateMethod = "False" Then
                Try
                    If UNCUsername = "" Then
                        UNCPassword = password.Password.ToString()
                        UNCUsername = username1.Text.ToString()
                        Domainname = txtdomain.Text.ToString()
                        UNCPath = Repname.Text.ToString()
                    End If
                    If UNCUsername <> "" And UNCPath <> "" And Domainname <> "" And UNCPassword <> "" Then
                        Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials()
                            If unc.NetUseWithCredentials(UNCPath, UNCUsername, Domainname, UNCPassword) Then
                                custommsgbox.showCustomMessageBox("Info", "Connection Succeeded." & vbCrLf & "Press 'OK' to Continue")
                                'MsgBox("ConnectUNCWithCredentials Successufully", vbOKOnly, "STANDALONE EXPLORER:Notification")
                                If FilePath <> "" Then
                                    '    If (Directory.GetFiles(Repname.Text + "\", "*.ezo", IO.SearchOption.AllDirectories).Count > 0) Then
                                    Me.Hide()
                                    Dim frm As Form2 = New Form2()
                                    frm.repopath = FilePath
                                    Dim bytKey As Byte()
                                    Dim bytIV As Byte()
                                    bytKey = CreateKey(UNCPassword)
                                    bytIV = CreateIV(UNCPassword)
                                    frm.key = UNCPassword
                                    If frm.ShowDialog() = False Then
                                        frm.Close()
                                    End If
                                    'Else
                                    '    MsgBox("Folder Location is Empty", vbOKOnly, "STANDALONE EXPLORER:Notification")
                                    'End If
                                End If
                                Form1.Close()
                            Else
                                'MsgBox("ConnectUNCWithCredentials Not Connected", vbOKOnly, "STANDALONE EXPLORER:Notification")
                                custommsgbox.showCustomMessageBox("Info", "Connection Failed...")
                                unc.writetxtfle("Failed to connect to UNC Credentials " & UNCPath & vbCrLf & "LastError = " + unc.LastError.ToString)
                            End If
                        End Using
                    Else
                        custommsgbox.showCustomMessageBox("Info", "Username is empty")
                        'MsgBox("Username is empty", vbOKOnly, "STANDALONE EXPLORER : Notification")
                        UNCs.writetxtfle("UNC CREDENTIALS:  Username is EMPTY")
                    End If
                Catch ex As Exception
                    'Dim mw As New MessageWin()
                    'mw.Show()
                    custommsgbox.showCustomMessageBox("error", "Invalid Location" & vbCrLf & ex.Message)
                    'MsgBox("Invalid Location ", vbOKOnly, "STANDALONE EXPLORER:Notification")
                Finally
                End Try
            Else
                Try

                    Dim passkey = password.Password.ToString()
                    Dim username = username1.Text.ToString()
                    Dim domain = txtdomain.Text.ToString()

                    If FilePath <> "" Then
                        If (Directory.Exists(FilePath)) Then
                            'If (Directory.GetFiles(Repname.Text + "\", "*.ezo", IO.SearchOption.AllDirectories).Count > 0) Then
                            Me.Hide()
                            Dim frm As Form2 = New Form2()
                            frm.repopath = FilePath
                            Dim bytKey As Byte()
                            Dim bytIV As Byte()
                            bytKey = CreateKey(passkey)
                            bytIV = CreateIV(passkey)
                            frm.key = passkey
                            If frm.ShowDialog() = False Then
                                frm.Close()
                            End If
                            'Else
                            '    MsgBox("Folder Location is Empty", vbOKOnly, "STANDALONE EXPLORER:Notification")
                            'End If
                        Else
                            custommsgbox.showCustomMessageBox("Info", "Please Select Correct Location")
                            ' MsgBox("Please Select Correct Location", vbOKOnly, "STANDALONE EXPLORER:Notification")
                        End If
                        'shiva
                    Else
                        'MsgBox("Please Enter the location", vbOKOnly, "STANDALONE EXPLORER:Notification")
                        custommsgbox.showCustomMessageBox("error", "Please Enter the Location", "login")
                    End If
                    'Form1.Close()
                Catch ex As Exception

                    'MsgBox("Invalid Location", vbOKOnly, "STANDALONE EXPLORER:Notification")
                    custommsgbox.showCustomMessageBox("error", "Invalid Location")

                Finally
                End Try
            End If

        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception in btn_getcon " & vbCrLf & ex.Message)

        End Try

    End Sub


    Private Function CreateKey(ByVal strPassword As String) As Byte()
        'Convert strPassword to an array and store in chrData.
        Dim chrData() As Char = strPassword.ToCharArray
        'Use intLength to get strPassword size.
        Dim intLength As Integer = chrData.GetUpperBound(0)
        'Declare bytDataToHash and make it the same size as chrData.
        Dim bytDataToHash(intLength) As Byte

        'Use For Next to convert and store chrData into bytDataToHash.
        For i As Integer = 0 To chrData.GetUpperBound(0)
            bytDataToHash(i) = CByte(Asc(chrData(i)))
        Next

        'Declare what hash to use.
        Dim SHA512 As New System.Security.Cryptography.SHA512Managed
        'Declare bytResult, Hash bytDataToHash and store it in bytResult.
        Dim bytResult As Byte() = SHA512.ComputeHash(bytDataToHash)
        'Declare bytKey(31).  It will hold 256 bits.
        Dim bytKey(31) As Byte

        'Use For Next to put a specific size (256 bits) of 
        'bytResult into bytKey. The 0 To 31 will put the first 256 bits
        'of 512 bits into bytKey.
        For i As Integer = 0 To 31
            bytKey(i) = bytResult(i)
        Next

        Return bytKey 'Return the key.
    End Function


    Private Function CreateIV(ByVal strPassword As String) As Byte()
        'Convert strPassword to an array and store in chrData.
        Dim chrData() As Char = strPassword.ToCharArray
        'Use intLength to get strPassword size.
        Dim intLength As Integer = chrData.GetUpperBound(0)
        'Declare bytDataToHash and make it the same size as chrData.
        Dim bytDataToHash(intLength) As Byte

        'Use For Next to convert and store chrData into bytDataToHash.
        For i As Integer = 0 To chrData.GetUpperBound(0)
            bytDataToHash(i) = CByte(Asc(chrData(i)))
        Next

        'Declare what hash to use.
        Dim SHA512 As New System.Security.Cryptography.SHA512Managed
        'Declare bytResult, Hash bytDataToHash and store it in bytResult.
        Dim bytResult As Byte() = SHA512.ComputeHash(bytDataToHash)
        'Declare bytIV(15).  It will hold 128 bits.
        Dim bytIV(15) As Byte

        'Use For Next to put a specific size (128 bits) of 
        'bytResult into bytIV. The 0 To 30 for bytKey used the first 256 bits.
        'of the hashed password. The 32 To 47 will put the next 128 bits into bytIV.
        For i As Integer = 32 To 47
            bytIV(i - 32) = bytResult(i)
        Next

        Return bytIV 'return the IV
    End Function


    Private Sub Btn_cancel_Click(sender As Object, e As RoutedEventArgs) Handles Btn_cancel.Click
        Me.Close()
    End Sub

    Private Sub Password_GotFocus(sender As Object, e As RoutedEventArgs) Handles password.GotFocus
        Me.password.Password = ""
        Me.password.Foreground = New SolidColorBrush(Colors.Gray)
    End Sub

    Private Sub Repname_TextChanged(sender As Object, e As Controls.TextChangedEventArgs) Handles Repname.TextChanged

    End Sub

    Private Sub Repname_KeyDown(sender As Object, e As Input.KeyEventArgs) Handles Repname.KeyDown
        'If e.keycode = Keys.Enter Then

        'End If
    End Sub
End Class


