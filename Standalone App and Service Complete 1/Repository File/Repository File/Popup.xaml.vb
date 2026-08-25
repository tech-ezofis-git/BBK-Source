Imports System.IO
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Forms.Timer
Imports System.Windows.Media
Imports System.Windows.Threading
Imports Repository_File.Pubvar
Imports System.Security.Cryptography


Public Class Popup
    Public Shared passworkw As String
    Public Shared Destdir As String
    'shiva
    Public Shared ClickedButton As String = ""
    Public Shared keepbothfiles As String = ""
    Public Shared SearchPaths As String() = Nothing
    Dim custommsgbox As New CustomMessageBoxControl

    Public Sub New()
        Try
            InitializeComponent()
            BtnDecryptInitProcess.IsEnabled = False

        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception :" & vbCrLf & ex.Message)
        End Try
    End Sub



    Private Sub Btnchsefolder_Click(sender As Object, e As RoutedEventArgs) Handles btnchsefolder.Click
        Dim open As New FolderBrowserDialog()
        Try
            If (open.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
                Destfolder.Text = open.SelectedPath
                Destdir = Destfolder.Text.ToString()
                If passwordboxDownload.Password.ToString() = "" Or passwordboxDownload.Password.ToString() = "Password" Then
                    wrongpassDownload.Content = "Enter Password"
                    wrongpassDecrypt.Visibility = Visibility.Visible
                ElseIf Destfolder.Text.ToString() = "" Then
                    wrongpassDecrypt.Content = "Choose Correct path"
                    wrongpassDecrypt.Visibility = Visibility.Visible
                End If
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception :" & vbCrLf & ex.Message)
        End Try
    End Sub

    'Private Sub Btnclose_Click(sender As Object, e As RoutedEventArgs) Handles btnclose.Click
    '    ClickedButton = "Close"
    '    Me.Close()
    'End Sub

    Private Sub Destfolder_GotFocus(sender As Object, e As RoutedEventArgs) Handles Destfolder.GotFocus
        'Me.Destfolder.Text = ""
        Me.Destfolder.Foreground = New SolidColorBrush(Colors.DeepSkyBlue)
    End Sub

    Private Sub Destfolder_TextChanged(sender As Object, e As Controls.TextChangedEventArgs) Handles Destfolder.TextChanged

    End Sub

    Private Sub Btn_cancel_Click(sender As Object, e As RoutedEventArgs) Handles Btn_cancel.Click
        CustomMessageBoxResult = 0
        Me.Close()
    End Sub

    Private Sub btnDecryptChkPwd_Click(sender As Object, e As RoutedEventArgs) Handles btnDecryptChkPwd.Click
        ValidatePasswordAsync("Decrypt")
    End Sub
    Private Function FindFirstEzoInFolder(folderPath As String) As String
        Try
            If String.IsNullOrWhiteSpace(folderPath) OrElse Not Directory.Exists(folderPath) Then
                Return Nothing
            End If
            Dim normalized = folderPath.TrimEnd("\"c, "/"c)
            Return Directory.EnumerateFiles(normalized, "*.ezo", SearchOption.TopDirectoryOnly).FirstOrDefault()
        Catch
            Return Nothing
        End Try
    End Function
    Private Function FindFirstEzoFile(rootPath As String) As String
        Try
            ' 1) Check current folder only (TopDirectoryOnly) added by sara
            Dim file = FindFirstEzoInFolder(rootPath)
            If Not String.IsNullOrEmpty(file) Then Return file
            ' 2) Check each selected folder from Form2 (TopDirectoryOnly only)
            If SearchPaths IsNot Nothing Then
                For Each selectedPath As String In SearchPaths
                    If String.IsNullOrWhiteSpace(selectedPath) Then Continue For
                    file = FindFirstEzoInFolder(selectedPath)
                    If Not String.IsNullOrEmpty(file) Then Return file
                    file = FindFirstEzoUnderRetailStructure(selectedPath)
                    If Not String.IsNullOrEmpty(file) Then Return file
                Next
            End If

            ' 3) No file found — do NOT scan AllDirectories
            Return FindFirstEzoUnderRetailStructure(rootPath)

            '**********************OLD ONE****************************
            'If String.IsNullOrWhiteSpace(rootPath) OrElse Not Directory.Exists(rootPath) Then
            '    Return Nothing
            'End If

            'Dim normalized = rootPath.TrimEnd("\"c, "/"c)

            'Dim topFile = Directory.EnumerateFiles(normalized, "*.ezo", SearchOption.TopDirectoryOnly).FirstOrDefault()
            'If Not String.IsNullOrEmpty(topFile) Then Return topFile

            'Return Directory.EnumerateFiles(normalized, "*.ezo", SearchOption.AllDirectories).FirstOrDefault()
            '**********************************************************
        Catch
            Return Nothing
        End Try
    End Function
    Private Function FindFirstEzoUnderRetailStructure(startPath As String) As String
        If String.IsNullOrWhiteSpace(startPath) OrElse Not Directory.Exists(startPath) Then
            Return Nothing
        End If

        Dim root = startPath.TrimEnd("\"c, "/"c)

        ' No config — walk customer folders in order; stop at first .ezo found
        ' Layout: {RIM Number}\{TIN Number}\Personal\*.ezo
        For Each RimNumberDir In Directory.EnumerateDirectories(root)
            For Each TinNumberDir In Directory.EnumerateDirectories(RimNumberDir)
                Dim personalDir = Path.Combine(TinNumberDir, "Personal")
                If Not Directory.Exists(personalDir) Then Continue For

                Dim file = Directory.EnumerateFiles(personalDir, "*.ezo", SearchOption.TopDirectoryOnly).FirstOrDefault()
                If Not String.IsNullOrEmpty(file) Then Return file   ' ONE sample only — stop here
            Next
        Next

        Return Nothing
    End Function
    Private Async Sub ValidatePasswordAsync(action As String)
        Dim formpassword As String = ""
        Dim checkButton As System.Windows.Controls.Button = Nothing

        Try
            If action = "Decrypt" Then
                formpassword = PasswordboxDecrypt.Password
                checkButton = btnDecryptChkPwd
            Else
                formpassword = passwordboxDownload.Password
                checkButton = btnDownloadChkPwd
            End If

            If formpassword = "" OrElse formpassword = "Password" Then
                custommsgbox.showCustomMessageBox("error", "Password Incorrect" & vbCrLf & "Please Check your Passowrd")
                Return
            End If

            Dim searchPath = decryptpath.Content.ToString()
            If checkButton IsNot Nothing Then checkButton.IsEnabled = False
            Me.Cursor = System.Windows.Input.Cursors.Wait

            Dim firstEzo As String = Await Task.Run(Function() FindFirstEzoFile(searchPath))

            If String.IsNullOrEmpty(firstEzo) Then
                custommsgbox.showCustomMessageBox("Info", "No File in Exists")
                Return
            End If

            Dim tempOutput = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() & ".pdf")
            Dim decryptResult As Integer = Await Task.Run(Function() DecryptFile(firstEzo, tempOutput, formpassword))

            If File.Exists(tempOutput) Then
                File.Delete(tempOutput)
            End If

            If decryptResult <> 0 Then
                custommsgbox.showCustomMessageBox("error", "Password Incorrect" & vbCrLf & "Please Check your Passowrd")
                PasswordboxDecrypt.Password = ""
                passwordboxDownload.Password = ""
                BtnDecryptInitProcess.IsEnabled = False
                BtnDownloadInitProcess.IsEnabled = False
            Else
                custommsgbox.showCustomMessageBox("Info", "Password Vaildated Successfully!")
                BtnDecryptInitProcess.IsEnabled = True
                BtnDownloadInitProcess.IsEnabled = True
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception in validatePassword :" & vbCrLf & ex.Message)
        Finally
            Me.Cursor = System.Windows.Input.Cursors.Arrow
            If checkButton IsNot Nothing Then checkButton.IsEnabled = True
        End Try
    End Sub

    ' Kept for callers that expect synchronous name; routes to async implementation.
    Public Sub validatePassword(action As String)
        ValidatePasswordAsync(action)
    End Sub

    Private Function DecryptFile(inputFile As String, outputFile As String, password As String) As Integer
        Dim fsInput As System.IO.FileStream
        Dim fsOutput As System.IO.FileStream
        Dim errorno As Integer = 0
        Try


            fsInput = New FileStream(inputFile, FileMode.Open, FileAccess.Read)
            fsOutput = New FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write)
            fsOutput.SetLength(0)

            '  Dim cryptFile As String = outputFile
            ' Dim fsCrypt As FileStream = New FileStream(cryptFile, FileMode.Create)
            Dim bytBuffer(4096) As Byte 'holds a block of bytes for processing
            Dim lngBytesProcessed As Long = 0 'running count of bytes processed
            Dim lngFileLength As Long = fsInput.Length 'the input file's length
            Dim intBytesInCurrentBlock As Integer 'current bytes being processed

            Dim RMCrypto As RijndaelManaged = New RijndaelManaged()
            'RMCrypto.Padding = PaddingMode.PKCS7

            Dim bytKey As Byte()
            Dim bytIV As Byte()

            'Send the password to the CreateKey function.
            bytKey = CreateKey(password)
            'Send the password to the CreateIV function.
            bytIV = CreateIV(password)


            Dim csCryptoStream As CryptoStream = New CryptoStream(fsOutput, RMCrypto.CreateDecryptor(bytKey, bytIV), CryptoStreamMode.Write) ' True)


            While lngBytesProcessed < lngFileLength

                'Read file with the input filestream.
                intBytesInCurrentBlock = fsInput.Read(bytBuffer, 0, 4096)

                'Write output file with the cryptostream.
                csCryptoStream.Write(bytBuffer, 0, intBytesInCurrentBlock)
                'Update lngBytesProcessed
                lngBytesProcessed = lngBytesProcessed + CLng(intBytesInCurrentBlock)

            End While

            'If (Not csCryptoStream.HasFlushedFinalBlock) Then
            '    csCryptoStream.FlushFinalBlock()
            'End If

            'csCryptoStream.FlushFinalBlock()
            csCryptoStream.Close()
            fsInput.Close()
            fsOutput.Close()

            Dim fileDelete As New System.IO.FileInfo(outputFile)
            fileDelete.Delete()
            'File.Delete(inputFile)
            Return errorno
            ''msgbox("File Decrepted...")
            'Catch When Err.Number = 53 'if file not found
            '    'successflag = False
            '   errorno = 53
            '  Return errorno
            '  '    'Catch all other errors. And delete partial files.
        Catch ex As Exception
            '    'successflag = False
            '    ' MsgBox(ex.Message)
            fsInput.Close()
            fsOutput.Close()

            Dim fileDelete As New System.IO.FileInfo(outputFile)
            fileDelete.Delete()
            errorno = 1
            Return errorno
            'Catch ex As Exception
        End Try

    End Function
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

    Private Sub BtnDecryptInitProcess_Click(sender As Object, e As RoutedEventArgs) Handles BtnDecryptInitProcess.Click
        Try
            If (RBKeepBothFilesDecrypt.IsChecked) Then
                CustomMessageBoxResult = 1
                DecryptFormValidate()
            Else
                custommsgbox.showCustomMessageBox("error", "Conform Again!" & vbCrLf & "Do you want to REPLACE the existing Encrypted File?", "yesno")
                DecryptFormValidate()
            End If


        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception :" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Sub DecryptFormValidate()
        wrongpassDecrypt.Visibility = Visibility.Hidden
        passworkw = PasswordboxDecrypt.Password.ToString


        If RBKeepBothFilesDecrypt.IsChecked Then
            keepbothfiles = "true"
        Else
            keepbothfiles = "false"
        End If


        If PasswordboxDecrypt.Password.ToString() = "" Or PasswordboxDecrypt.Password.ToString() = "Password" Then
            wrongpassDecrypt.Content = "Enter Password"
            wrongpassDecrypt.Visibility = Visibility.Visible
        Else
            Me.Hide()
        End If
    End Sub
    Public Sub DownloadFormValidate()
        wrongpassDecrypt.Visibility = Visibility.Hidden
        passworkw = passwordboxDownload.Password.ToString

        'If CBKeepBothFilesDownload.IsChecked Then
        '    keepbothfiles = "true"
        'Else
        '    keepbothfiles = "false"
        'End If

        If passwordboxDownload.Password.ToString() = "" Or passwordboxDownload.Password.ToString() = "Password" Then
            wrongpassDownload.Content = "Enter Password"
            wrongpassDownload.Visibility = Visibility.Visible
        ElseIf Destfolder.Visibility = Visibility.Hidden Then
            Me.Hide()
        ElseIf passwordboxDownload.Password.ToString() <> "" And Destfolder.Text.ToString() <> "" Then
            If (Directory.Exists(Destfolder.Text.ToString)) Then
                Me.Hide()
            Else
                wrongpassDownload.Content = "Choose Correct Path"
                wrongpassDownload.Visibility = Visibility.Visible
            End If

        ElseIf Destfolder.Visibility = Visibility.Visible Then
            If Destfolder.Text.ToString() = "" Then
                wrongpassDownload.Content = "Choose Correct Path"
                wrongpassDownload.Visibility = Visibility.Visible
            End If
        End If
    End Sub

    Private Sub PasswordboxDecrypt_GotFocus(sender As Object, e As RoutedEventArgs) Handles PasswordboxDecrypt.GotFocus
        Me.wrongpassDecrypt.Visibility = Visibility.Hidden
        Me.PasswordboxDecrypt.Password = ""
        Me.PasswordboxDecrypt.Foreground = New SolidColorBrush(Colors.DeepSkyBlue)
    End Sub

    Private Sub BtnDownloadInitProcess_Click(sender As Object, e As RoutedEventArgs) Handles BtnDownloadInitProcess.Click
        Try
            CustomMessageBoxResult = 1
            DownloadFormValidate()
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception :" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub btnDownloadChkPwd_Click(sender As Object, e As RoutedEventArgs) Handles btnDownloadChkPwd.Click
        ValidatePasswordAsync("Download")
    End Sub

    Private Sub passwordboxDownload_GotFocus(sender As Object, e As RoutedEventArgs) Handles passwordboxDownload.GotFocus
        Me.wrongpassDownload.Visibility = Visibility.Hidden
        Me.passwordboxDownload.Password = ""
        Me.passwordboxDownload.Foreground = New SolidColorBrush(Colors.DeepSkyBlue)
    End Sub
End Class
