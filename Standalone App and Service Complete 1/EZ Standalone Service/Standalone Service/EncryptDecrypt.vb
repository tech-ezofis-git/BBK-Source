
Imports System
Imports System.IO
Imports System.Net
Imports System.Security
Imports System.Security.Cryptography
Imports System.Threading
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization
Imports System.Web
Imports Newtonsoft.Json.Linq
Imports System.Collections.Specialized
Imports System.Text
Imports Standalone_Service.EncrptyDecrypt

Public Class EncrptyDecrypt
#Region "Declaration"
    Dim strFileToEncrypt As String
    Dim strFileToDecrypt As String
    Dim strOutputEncrypt As String
    Dim strOutputDecrypt As String
    ' Dim fsInput As System.IO.FileStream
    'Dim fsOutput As System.IO.FileStream
    Public filelocation As String = ""
    Dim jsonpath As String
    Dim passwordjson As String
    Dim status As String
    Dim noffiles As Integer
    Dim fsize As String
    Dim namefile As String
    Dim exts As String
    Dim datetime
    Dim ezofisfile
    Dim Batchid As String
    Dim noffilesProcessed As Integer
    Dim noffilesNotdecrypted As Integer
    Dim keepbothfiles As String

    Dim ser As JavaScriptSerializer = New JavaScriptSerializer()
    ' Dim csCryptoStream As CryptoStream

    Dim totfilesdecrypted As Integer = 0
    Dim totfilesnotdecrypted As Integer = 0
    Dim totfilesscanned As Integer = 0
    Dim notdecryptedfiles As New List(Of String)

    Dim dtotfilesdecrypted As Integer = 0
    Dim dtotfilesnotdecrypted As Integer = 0
    Dim dtotfilesscanned As Integer = 0
    Dim dnotdecryptedfiles As New List(Of String)

    Dim statusflag As String

    'json data path reader
    Dim Appcon As NameValueCollection = DirectCast(System.Configuration.ConfigurationManager.GetSection("Database"), NameValueCollection)
    Dim jpath = Appcon("Jsonpath")
    Dim Dpath = Appcon("Downloadjpath")
    Dim impersonateMethod = Appcon("impersonate")
    ' UNC Cretantial Method
    Dim Appconimporsanate2 As NameValueCollection = DirectCast(System.Configuration.ConfigurationManager.GetSection("impersonateMethod"), NameValueCollection)
    Dim UNCUsername = Appconimporsanate2("Username")
    Dim UNCPath = Appconimporsanate2("UNCPath")
    Dim UNCPassword = Appconimporsanate2("UNCPassword")
    Dim Domainname = Appconimporsanate2("Domainname")
    Dim flushRaw = Appcon("DownloadQueueFlushEvery")
    Dim parallelRaw = Appcon("DownloadDecryptParallelism")
    'alaise method
    Dim alaisemethod As NameValueCollection = DirectCast(System.Configuration.ConfigurationManager.GetSection("AliasAccount"), NameValueCollection)
    Dim Username = alaisemethod("Usernames")
    Dim password = alaisemethod("Password")
    Dim alasiedomain = alaisemethod("Domainnames")
#End Region

    Dim Actionencrypt() As Thread
    Dim Actiondownload() As Thread
    Dim Actiondecrypt() As Thread

    Public Class efileinfo
        Property Filepath As String
        Property filesize As String
        Property Nooffiles As Integer
        Property status As String
    End Class

    Public Class folderinfo
        Property foldername As String
        Property pass
        Property foldersize As String
        Property Nooffiles As Integer
        Property batchid As String
        Property datime As String
        Property status As String
        Property NooffilesProcessed As Integer
        Property NooffilesUnprocessed As Integer
        Property KeepBothFiles As String
    End Class

    Public Class downloadfile
        Property foldername As String
        Property movepath As String
        Property passwordd As String
        Property dfoldersize As String
        Property Nooffiles As Integer
        Property extension As String
        Property batchid As String
        Property datime As String
        Property status As String
        Property NooffilesProcessed As Integer
        Property NooffilesUnprocessed As Integer

    End Class

    Dim dwnpath As String
    Dim movpath As String
    Dim dwnpass As String
    Dim dwnfoldersize As String
    Dim dwnnooffile As Integer
    Dim dwnextension As String
    Dim dwnbatchid As String
    Dim dwndatime As String
    Dim dwnstatus As String
    Dim nooffilesProcessed As Integer
    Private DOWNLOAD_QUEUE_FLUSH_EVERY As Integer = 50
    Private queueFlushLock As New Object()
    Private DOWNLOAD_DECRYPT_PARALLELISM As Integer = 4
    Private cachedDownloadKey As Byte()
    Private cachedDownloadIV As Byte()
    Private flushGate As Integer = 0   ' optional: single-flusher gate


    Private Enum CryptoAction
        'Define the enumeration for CryptoAction.
        ActionEncrypt = 1
        ActionDecrypt = 2
    End Enum

    Public Sub encryptdecrypt()
        Try
            Dim index As Integer = 0
            Dim index1 As Integer = 0

            If jpath <> "" And Dpath <> "" Then
                '  writetxtfle("Decrypt and Download path" + jpath + ":" + Dpath)
                If (File.Exists(jpath)) Then
                    Try
                        If Actiondecrypt Is Nothing Then
                            Array.Resize(Actiondecrypt, 1)
                            Actiondecrypt(index) = New Thread(New ParameterizedThreadStart(AddressOf Decrypt))
                        ElseIf index > Actiondecrypt.Length - 1 Then
                            Array.Resize(Actiondecrypt, Actiondecrypt.Length + 1)
                            Actiondecrypt(index) = New Thread(New ParameterizedThreadStart(AddressOf Decrypt))
                        End If
                        Select Case Actiondecrypt(index).ThreadState
                            Case ThreadState.Stopped
                                Actiondecrypt(index) = New Thread(New ParameterizedThreadStart(AddressOf Decrypt))
                                Actiondecrypt(index).Start()
                            Case ThreadState.Unstarted
                                Actiondecrypt(index) = New Thread(New ParameterizedThreadStart(AddressOf Decrypt))
                                Actiondecrypt(index).Start()
                                '    writetxtfle("Decrypt Start")
                        End Select
                        index = index + 1
                    Catch ex As Exception
                        writetxtfle("Decrypt thread:" + ex.Message)
                    End Try
                End If
                If (File.Exists(Dpath)) Then
                    Try
                        If Actiondownload Is Nothing Then
                            Array.Resize(Actiondownload, 1)
                            Actiondownload(index1) = New Thread(New ParameterizedThreadStart(AddressOf Download))
                        ElseIf index1 > Actiondownload.Length - 1 Then
                            Array.Resize(Actiondownload, Actiondownload.Length + 1)
                            Actiondownload(index1) = New Thread(New ParameterizedThreadStart(AddressOf Download))
                        End If
                        Select Case Actiondownload(index1).ThreadState
                            Case ThreadState.Stopped
                                Actiondownload(index1) = New Thread(New ParameterizedThreadStart(AddressOf Download))
                                Actiondownload(index1).Start()
                            Case ThreadState.Unstarted
                                Actiondownload(index1) = New Thread(New ParameterizedThreadStart(AddressOf Download))
                                Actiondownload(index1).Start()
                                '    writetxtfle("Download Start")
                        End Select
                        index1 = index1 + 1
                    Catch ex As Exception
                        writetxtfle("Download thread:" + ex.Message)
                    End Try
                End If
            End If
        Catch ex As Exception
            writetxtfle("Exception encryptdecrypt:" + ex.Message)
        End Try
    End Sub



    Private Sub Decrypt()
        dtotfilesdecrypted = 0
        dtotfilesscanned = 0
        dtotfilesnotdecrypted = 0
        Try
            If (File.Exists(jpath)) Then
                'writetxtfle("jpath found")
                Dim cspRijndael As New System.Security.Cryptography.RijndaelManaged
                Dim client = New WebClient()
                client.Headers("Content-Type") = "application/json"
                client.Encoding = System.Text.Encoding.UTF8
                Try
                    If impersonateMethod = "True" Then
                        Try
                            'writetxtfle("impoersonate true")
                            Dim acct As AliasAccount
                            Dim impersonate As Boolean = False
                            acct = New AliasAccount(Appcon("Usernames"), Appcon("Password"), Appcon("Domainnames"))
                            Try
                                acct.BeginImpersonation()
                                impersonate = True
                            Catch ex As Exception
                                writetxtfle("Exception in BeginImpersonation")
                            End Try
                            If impersonate Then
                                'decryptallfiles(jpath)
                                If jpath <> "" Then
                                    Dim uristring As String
                                    Dim fileinfos As List(Of folderinfo)

                                    Dim uristring1 = File.ReadAllText(jpath)
                                    If (uristring1.Length > 0) Then
                                        Dim fileinfos1 As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring1)
                                        'Dim table As DataTable = JsonConvert.DeserializeObject(Of DataTable)(uristring)
                                        If fileinfos1.Count > 0 Then
                                            For k = 0 To fileinfos1.Count - 1
                                                dtotfilesdecrypted = 0
                                                dtotfilesscanned = 0
                                                dtotfilesnotdecrypted = 0

                                                uristring = File.ReadAllText(jpath)
                                                ' fileinfos = ser.Deserialize(Of List(Of folderinfo))(uristring)

                                                noffiles = Directory.GetFiles(fileinfos1(k).foldername + "\", "*.ezo", IO.SearchOption.AllDirectories).Count

                                                Dim status = fileinfos1(k).status
                                                If (noffiles > fileinfos1(k).Nooffiles) Then
                                                    fileinfos1(k).Nooffiles = noffiles
                                                End If
                                                If status = "New" Then

                                                    fileinfos1(k).status = "Initializing..."
                                                    Dim Settings As New JsonSerializerSettings
                                                    Settings.Formatting = Formatting.Indented
                                                    Settings.NullValueHandling = NullValueHandling.Ignore
                                                    Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
                                                    File.WriteAllText(jpath, json)

                                                    If (noffiles > 0) Then
                                                        fileinfos1(k).Nooffiles = noffiles
                                                        fileinfos1(k).status = "Processing..."
                                                        'writetxtfle("Decrypt : Status Changed from Initialing... To Processing...")
                                                        Settings.Formatting = Formatting.Indented
                                                        Settings.NullValueHandling = NullValueHandling.Ignore
                                                        json = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
                                                        File.WriteAllText(jpath, json)
                                                    Else
                                                        fileinfos1(k).status = "Completed"
                                                        Settings.Formatting = Formatting.Indented
                                                        Settings.NullValueHandling = NullValueHandling.Ignore
                                                        json = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
                                                        File.WriteAllText(jpath, json)
                                                        'writetxtfle("No of files = " & noffiles & " .so status changed To Completed")
                                                    End If
                                                End If

                                                jsonpath = fileinfos1(k).foldername
                                                passwordjson = fileinfos1(k).pass
                                                status = fileinfos1(k).status
                                                noffiles = fileinfos1(k).Nooffiles
                                                fsize = fileinfos1(k).foldersize
                                                Batchid = fileinfos1(k).batchid
                                                datetime = fileinfos1(k).datime
                                                noffilesProcessed = fileinfos1(k).NooffilesProcessed
                                                noffilesNotdecrypted = fileinfos1(k).NooffilesUnprocessed
                                                keepbothfiles = fileinfos1(k).KeepBothFiles


                                                If (noffiles > 0) Then
                                                    For Each fname As String In IO.Directory.GetFiles(jsonpath + "\", "*.ezo", IO.SearchOption.AllDirectories)
                                                        Dim dFile As String = ""
                                                        dFile = Path.GetFileName(fname)
                                                        Dim Decryptfile = fname
                                                        Dim iPosition As Integer = 0
                                                        Dim i As Integer = 0
                                                        While Decryptfile.IndexOf("\"c, i) <> -1
                                                            iPosition = Decryptfile.IndexOf("\"c, i)
                                                            i = iPosition + 1
                                                        End While
                                                        strOutputDecrypt = Decryptfile.Substring(0, Decryptfile.Length - 4)
                                                        strOutputDecrypt = strOutputDecrypt + ".pdf"
                                                        Dim S As String = Decryptfile.Substring(0, iPosition + 1)
                                                        strOutputDecrypt = strOutputDecrypt.Substring((iPosition + 1))
                                                        'Dim strtext = S + strOutputDecrypt.Replace("_"c, "."c)
                                                        Dim destinationpath = S + strOutputDecrypt
                                                        '.Replace("_", ".")
                                                        If passwordjson <> "" And status = "Processing..." Then
                                                            Encryptordecryptfile(fname, destinationpath, passwordjson, CryptoAction.ActionDecrypt, keepbothfiles)
                                                        End If
                                                    Next
                                                End If
                                            Next
                                        End If
                                    End If
                                Else
                                    writetxtfle("Json Path is Empty,Please Make Some Decrypt File in Standalone Explorer")
                                End If

                            Else
                                writetxtfle("impersonate Not Connected")
                            End If
                        Catch ex As Exception
                            writetxtfle("Exception in impersonateMethod-TRUE")
                        End Try
                    ElseIf impersonateMethod = "False" Then
                        Try
                            'writetxtfle("impoersonate false")
                            If UNCUsername <> "" And UNCUsername <> "" And Domainname <> "" And UNCPassword <> "" Then
                                Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials()
                                    If unc.NetUseWithCredentials(UNCPath, UNCUsername, Domainname, UNCPassword) Then
                                        'writetxtfle("impersonatemethod false ok")
                                        'decryptallfiles(jpath)
                                        If jpath <> "" Then
                                            Dim uristring As String
                                            Dim fileinfos As List(Of folderinfo)
                                            Dim uristring1 = File.ReadAllText(jpath)
                                            If (uristring1.Length > 0) Then
                                                'writetxtfle("impersonatemethod false Read json is ok")
                                                Dim fileinfos1 As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring1)
                                                'Dim table As DataTable = JsonConvert.DeserializeObject(Of DataTable)(uristring)
                                                'writetxtfle("impersonatemethod false Read json deserialize ok")
                                                If fileinfos1.Count > 0 Then
                                                    For k = 0 To fileinfos1.Count - 1

                                                        dtotfilesdecrypted = 0
                                                        dtotfilesscanned = 0
                                                        dtotfilesnotdecrypted = 0

                                                        uristring = File.ReadAllText(jpath)
                                                        'writetxtfle("impersonatemethod false Read json file :" & uristring)
                                                        '  fileinfos = ser.Deserialize(Of List(Of folderinfo))(uristring)

                                                        noffiles = Directory.GetFiles(fileinfos1(k).foldername + "\", "*.ezo", IO.SearchOption.AllDirectories).Count

                                                        Dim status = fileinfos1(k).status
                                                        If (noffiles > fileinfos1(k).Nooffiles) Then
                                                            fileinfos1(k).Nooffiles = noffiles
                                                        End If
                                                        If status = "New" Then

                                                            fileinfos1(k).status = "Initializing..."
                                                            'writetxtfle("impersonatemethod false Status changed to Initializing")
                                                            Dim Settings As New JsonSerializerSettings
                                                            Settings.Formatting = Formatting.Indented
                                                            Settings.NullValueHandling = NullValueHandling.Ignore
                                                            Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
                                                            File.WriteAllText(jpath, json)

                                                            If (noffiles > 0) Then
                                                                fileinfos1(k).Nooffiles = noffiles
                                                                fileinfos1(k).status = "Processing..."
                                                                'writetxtfle("Decrypt : Status Changed from Initialing... To Processing...")
                                                                Settings.Formatting = Formatting.Indented
                                                                Settings.NullValueHandling = NullValueHandling.Ignore
                                                                json = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
                                                                File.WriteAllText(jpath, json)
                                                            Else
                                                                fileinfos1(k).status = "Completed"
                                                                Settings.Formatting = Formatting.Indented
                                                                Settings.NullValueHandling = NullValueHandling.Ignore
                                                                json = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
                                                                File.WriteAllText(jpath, json)
                                                                'writetxtfle("No of files = " & noffiles & " .so status changed To Completed")
                                                            End If
                                                        End If
                                                        dtotfilesdecrypted = 0
                                                        dtotfilesscanned = 0
                                                        dtotfilesnotdecrypted = 0

                                                        jsonpath = fileinfos1(k).foldername
                                                        passwordjson = fileinfos1(k).pass
                                                        status = fileinfos1(k).status
                                                        noffiles = fileinfos1(k).Nooffiles
                                                        fsize = fileinfos1(k).foldersize
                                                        Batchid = fileinfos1(k).batchid
                                                        datetime = fileinfos1(k).datime
                                                        noffilesProcessed = fileinfos1(k).NooffilesProcessed
                                                        noffilesNotdecrypted = fileinfos1(k).NooffilesUnprocessed
                                                        keepbothfiles = fileinfos1(k).KeepBothFiles

                                                        If (noffiles > 0) Then
                                                            For Each fname As String In IO.Directory.GetFiles(jsonpath + "\", "*.ezo", IO.SearchOption.AllDirectories)
                                                                'writetxtfle("impersonatemethod false For loop entered")
                                                                Dim dFile As String = ""
                                                                dFile = Path.GetFileName(fname)
                                                                Dim Decryptfile = fname
                                                                Dim iPosition As Integer = 0
                                                                Dim i As Integer = 0
                                                                While Decryptfile.IndexOf("\"c, i) <> -1
                                                                    iPosition = Decryptfile.IndexOf("\"c, i)
                                                                    i = iPosition + 1
                                                                End While
                                                                strOutputDecrypt = Decryptfile.Substring(0, Decryptfile.Length - 4)
                                                                strOutputDecrypt = strOutputDecrypt + ".pdf"
                                                                Dim S As String = Decryptfile.Substring(0, iPosition + 1)
                                                                strOutputDecrypt = strOutputDecrypt.Substring((iPosition + 1))
                                                                'Dim strtext = S + strOutputDecrypt.Replace("_"c, "."c)
                                                                Dim destinationpath = S + strOutputDecrypt
                                                                '.Replace("_", ".")
                                                                'writetxtfle("impersonatemethod false passwordjson" & passwordjson & " and status : " & status)
                                                                If passwordjson <> "" And status = "Processing..." Then
                                                                    'writetxtfle("impersonatemethod false Encryptordecryptfile function called")
                                                                    Encryptordecryptfile(fname, destinationpath, passwordjson, CryptoAction.ActionDecrypt, keepbothfiles)
                                                                    'writetxtfle("impersonatemethod false Encryptordecryptfile function completed")
                                                                End If
                                                            Next
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        Else
                                            writetxtfle("Json Path is Empty,Please Make Some Decrypt File in Standalone Explorer")
                                        End If
                                    Else
                                        writetxtfle("Failed to connect to UNC Credentials " & jpath & vbCrLf & "LastError = " + unc.LastError.ToString)
                                    End If
                                End Using
                            Else
                                writetxtfle("UNC Credentials Username is EMPTY")
                            End If
                        Catch ex As Exception
                            writetxtfle("Exception in impersonateMethod-FALSE")
                        End Try
                    Else
                        'writetxtfle("Decrypt-impersonateMethod is nothing")
                        ' decryptallfiles(jpath)
                        If jpath <> "" Then
                            Try
                                Dim uristring As String
                                Dim fileinfos As New List(Of folderinfo)
                                Dim uristring1 = File.ReadAllText(jpath)
                                If (uristring1.Length > 0) Then
                                    Dim fileinfos1 As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring1)
                                    'Dim table As DataTable = JsonConvert.DeserializeObject(Of DataTable)(uristring)
                                    If fileinfos1.Count > 0 Then
                                        For k = 0 To fileinfos1.Count - 1
                                            dtotfilesdecrypted = 0
                                            dtotfilesscanned = 0
                                            dtotfilesnotdecrypted = 0

                                            uristring = File.ReadAllText(jpath)
                                            ' fileinfos = ser.Deserialize(Of List(Of folderinfo))(uristring)

                                            noffiles = Directory.GetFiles(fileinfos1(k).foldername + "\", "*.ezo", IO.SearchOption.AllDirectories).Count
                                            'writetxtfle("Decrypt : Total No of files=" + noffiles.ToString())
                                            Dim status = fileinfos1(k).status
                                            If (noffiles > fileinfos1(k).Nooffiles) Then
                                                fileinfos1(k).Nooffiles = noffiles
                                            End If
                                            If status = "New" Then

                                                fileinfos1(k).status = "Initializing..."
                                                'writetxtfle("Decrypt : Status Changed from New To Initialing...")
                                                Dim Settings As New JsonSerializerSettings
                                                Settings.Formatting = Formatting.Indented
                                                Settings.NullValueHandling = NullValueHandling.Ignore
                                                Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
                                                File.WriteAllText(jpath, json)

                                                If (noffiles > 0) Then
                                                    fileinfos1(k).Nooffiles = noffiles
                                                    fileinfos1(k).status = "Processing..."
                                                    'writetxtfle("Decrypt : Status Changed from Initialing... To Processing...")
                                                    Settings.Formatting = Formatting.Indented
                                                    Settings.NullValueHandling = NullValueHandling.Ignore
                                                    json = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
                                                    File.WriteAllText(jpath, json)
                                                Else
                                                    fileinfos1(k).status = "Completed"
                                                    Settings.Formatting = Formatting.Indented
                                                    Settings.NullValueHandling = NullValueHandling.Ignore
                                                    json = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
                                                    File.WriteAllText(jpath, json)
                                                    'writetxtfle("No of files = " & noffiles & " .so status changed To Completed")
                                                End If

                                            End If

                                            dtotfilesdecrypted = 0
                                            dtotfilesscanned = 0
                                            dtotfilesnotdecrypted = 0
                                            jsonpath = fileinfos1(k).foldername
                                            passwordjson = fileinfos1(k).pass
                                            status = fileinfos1(k).status
                                            noffiles = fileinfos1(k).Nooffiles
                                            fsize = fileinfos1(k).foldersize
                                            Batchid = fileinfos1(k).batchid
                                            datetime = fileinfos1(k).datime
                                            noffilesProcessed = fileinfos1(k).NooffilesProcessed
                                            noffilesNotdecrypted = fileinfos1(k).NooffilesUnprocessed
                                            keepbothfiles = fileinfos1(k).KeepBothFiles

                                            If (noffiles > 0) Then
                                                For Each fname As String In IO.Directory.GetFiles(jsonpath + "\", "*.ezo", IO.SearchOption.AllDirectories)
                                                    Dim dFile As String = ""
                                                    dFile = Path.GetFileName(fname)
                                                    Dim Decryptfile = fname
                                                    Dim iPosition As Integer = 0
                                                    Dim i As Integer = 0
                                                    While Decryptfile.IndexOf("\"c, i) <> -1
                                                        iPosition = Decryptfile.IndexOf("\"c, i)
                                                        i = iPosition + 1
                                                    End While
                                                    strOutputDecrypt = Decryptfile.Substring(0, Decryptfile.Length - 4)
                                                    strOutputDecrypt = strOutputDecrypt + ".pdf"
                                                    Dim S As String = Decryptfile.Substring(0, iPosition + 1)
                                                    strOutputDecrypt = strOutputDecrypt.Substring((iPosition + 1))
                                                    'Dim strtext = S + strOutputDecrypt.Replace("_"c, "."c)
                                                    Dim destinationpath = S + strOutputDecrypt
                                                    '.Replace("_", ".")
                                                    If passwordjson <> "" And status = "Processing..." Then
                                                        Encryptordecryptfile(fname, destinationpath, passwordjson, CryptoAction.ActionDecrypt, keepbothfiles)
                                                    End If
                                                Next
                                            End If
                                        Next
                                    End If
                                End If
                            Catch ex As Exception
                                writetxtfle("Exception within local path " & ex.Message)
                            End Try
                        Else
                            writetxtfle("Json Path is Empty,Please Make Some Decrypt File in Standalone Explorer")
                        End If
                    End If
                Catch ex As Exception
                    writetxtfle("File Exists But Exception in Function Decrypt" & ex.Message)
                End Try
            Else
                writetxtfle(jpath & "File not exists")
            End If
        Catch ex As Exception
            writetxtfle("Exception in Function Decrypt" & ex.Message)
        Finally
            ' writetxtfle("Process Completed")
        End Try
    End Sub
    'Public Sub decryptallfiles(jpath)
    '    'writetxtfle("decryptallfiles called-JPath=" + jpath)
    '    If jpath <> "" Then
    '        Try

    '            Dim uristring As String
    '            Dim fileinfos As New List(Of folderinfo)
    '            Dim uristring1 = File.ReadAllText(jpath)
    '            If (uristring1.Length > 0) Then
    '                Dim fileinfos1 As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring1)
    '                'Dim table As DataTable = JsonConvert.DeserializeObject(Of DataTable)(uristring)
    '                'writetxtfle("Decryptallfiles-file count " & fileinfos1.Count)
    '                If fileinfos1.Count > 0 Then
    '                    For k = 0 To fileinfos1.Count - 1
    '                        dtotfilesdecrypted = 0
    '                        dtotfilesscanned = 0
    '                        dtotfilesnotdecrypted = 0

    '                        uristring = File.ReadAllText(jpath)
    '                        ' fileinfos = ser.Deserialize(Of List(Of folderinfo))(uristring)

    '                        noffiles = Directory.GetFiles(fileinfos1(k).foldername + "\", "*.ezo", IO.SearchOption.AllDirectories).Count

    '                        Dim status = fileinfos1(k).status
    '                        If (noffiles > fileinfos1(k).Nooffiles) Then
    '                            fileinfos1(k).Nooffiles = noffiles
    '                        End If
    '                        If status = "New" Then
    '                            fileinfos1(k).status = "Initializing..."
    '                            'writetxtfle("Decrypt : Status Changed from New To Initialing...")
    '                            Dim Settings As New JsonSerializerSettings
    '                            Settings.Formatting = Formatting.Indented
    '                            Settings.NullValueHandling = NullValueHandling.Ignore
    '                            Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
    '                            File.WriteAllText(jpath, json)


    '                            If (noffiles > 0) Then
    '                                fileinfos1(k).Nooffiles = noffiles
    '                                fileinfos1(k).status = "Processing..."
    '                                'writetxtfle("Decrypt : Status Changed from Initialing... To Processing...")
    '                                Settings.Formatting = Formatting.Indented
    '                                Settings.NullValueHandling = NullValueHandling.Ignore
    '                                json = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
    '                                File.WriteAllText(jpath, json)
    '                            Else
    '                                fileinfos1(k).status = "Completed"
    '                                Settings.Formatting = Formatting.Indented
    '                                Settings.NullValueHandling = NullValueHandling.Ignore
    '                                json = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos1, Settings)
    '                                File.WriteAllText(jpath, json)
    '                                'writetxtfle("No of files = " & noffiles & " .so status changed To Completed")
    '                            End If
    '                        End If



    '                        dtotfilesdecrypted = 0
    '                        dtotfilesscanned = 0
    '                        dtotfilesnotdecrypted = 0
    '                        jsonpath = fileinfos1(k).foldername
    '                        passwordjson = fileinfos1(k).pass
    '                        status = fileinfos1(k).status
    '                        noffiles = fileinfos1(k).Nooffiles
    '                        fsize = fileinfos1(k).foldersize
    '                        Batchid = fileinfos1(k).batchid
    '                        datetime = fileinfos1(k).datime
    '                        noffilesProcessed = fileinfos1(k).Nooffilesprocessed
    '                        noffilesNotdecrypted = fileinfos1(k).NooffilesUnprocessed

    '                        If (noffiles > 0) Then
    '                            For Each fname As String In IO.Directory.GetFiles(jsonpath + "\", "*.ezo", IO.SearchOption.AllDirectories)
    '                                'writetxtfle("Decryptallfiles-Take : " & fname)
    '                                Dim dFile As String = ""
    '                                dFile = Path.GetFileName(fname)
    '                                Dim Decryptfile = fname
    '                                Dim iPosition As Integer = 0
    '                                Dim i As Integer = 0
    '                                While Decryptfile.IndexOf("\"c, i) <> -1
    '                                    iPosition = Decryptfile.IndexOf("\"c, i)
    '                                    i = iPosition + 1
    '                                End While
    '                                strOutputDecrypt = Decryptfile.Substring(0, Decryptfile.Length - 4)
    '                                strOutputDecrypt = strOutputDecrypt + ".pdf"
    '                                Dim S As String = Decryptfile.Substring(0, iPosition + 1)
    '                                strOutputDecrypt = strOutputDecrypt.Substring((iPosition + 1))
    '                                'Dim strtext = S + strOutputDecrypt.Replace("_"c, "."c)
    '                                Dim destinationpath = S + strOutputDecrypt
    '                                '.Replace("_", ".")
    '                                If passwordjson <> "" And status = "Processing..." Then
    '                                    'writetxtfle("Decryptallfiles-passwordjson= " & passwordjson & ",status=" & status)
    '                                    Encryptordecryptfile(fname, destinationpath, passwordjson, CryptoAction.ActionDecrypt)
    '                                End If
    '                            Next
    '                        End If


    '                    Next
    '                End If
    '            End If
    '        Catch ex As Exception
    '            writetxtfle("Exception within local path " & ex.Message)
    '        Finally
    '            ' writetxtfle("Process Completed")
    '        End Try
    '    Else
    '        writetxtfle("Json Path is Empty,Please Make Some Decrypt File in Standalone Explorer")
    '    End If
    'End Sub
    Public Shared Function ReadQueueText(queuePath As String) As String
        For attempt As Integer = 1 To 10
            Try
                Using fs As New FileStream(queuePath, FileMode.Open, FileAccess.Read,
                                      FileShare.ReadWrite Or FileShare.Delete)
                    Using sr As New StreamReader(fs)
                        Return sr.ReadToEnd()
                    End Using
                End Using
            Catch ex As IOException
                If attempt = 10 Then Throw
                Thread.Sleep(50 * attempt)
            End Try
        Next
        Return String.Empty
    End Function
    Public Shared Sub WriteQueueText(queuePath As String, content As String)
        Dim dir = Path.GetDirectoryName(queuePath)
        If Not Directory.Exists(dir) Then Directory.CreateDirectory(dir)
        Dim tempPath = queuePath & ".tmp"

        File.WriteAllText(tempPath, content, New UTF8Encoding(False))

        For attempt As Integer = 1 To 10
            Try
                If File.Exists(queuePath) Then
                    File.Replace(tempPath, queuePath, Nothing)
                Else
                    File.Move(tempPath, queuePath)
                End If
                Return
            Catch ex As IOException
                If attempt = 10 Then Throw
                Thread.Sleep(100 * attempt)
            End Try
        Next
    End Sub
    Private Sub Download()
        Try
            'shiva
            totfilesdecrypted = 0
            totfilesscanned = 0
            totfilesnotdecrypted = 0
            Dim cspRijndael As New System.Security.Cryptography.RijndaelManaged
            Dim client = New WebClient()
            client.Headers("Content-Type") = "application/json"
            client.Encoding = System.Text.Encoding.UTF8
            ' writetxtfle("download-1")
            Try
                If (File.Exists(Dpath)) Then
                    '   writetxtfle("download-2")
                    If impersonateMethod = "True" Or impersonateMethod = "true" Then
                        Try
                            Dim acct As AliasAccount
                            Dim impersonate As Boolean = False
                            acct = New AliasAccount(Appcon("Usernames"), Appcon("Password"), Appcon("Domainnames"))
                            Try
                                acct.BeginImpersonation()
                                impersonate = True
                            Catch ex As Exception
                                writetxtfle("Exception in Download BeginImpersonation" & ex.Message)
                            End Try
                            If impersonate Then
                                'writetxtfle("download impersonate true")
                                'downloadallfiles(Dpath)
                                If Dpath <> "" Then
                                    Dim uristring As String
                                    Dim dfileinfos As List(Of downloadfile)
                                    Dim uristring1 = File.ReadAllText(Dpath)
                                    If (uristring1.Length > 0) Then
                                        Dim dfileinfos1 As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(uristring1)
                                        If dfileinfos1.Count > 0 Then
                                            For u = 0 To dfileinfos1.Count - 1
                                                totfilesdecrypted = 0
                                                totfilesscanned = 0
                                                totfilesnotdecrypted = 0

                                                uristring = File.ReadAllText(Dpath)
                                                ' dfileinfos = ser.Deserialize(Of List(Of downloadfile))(uristring)

                                                dwnnooffile = Directory.GetFiles(dfileinfos1(u).foldername + "\", "*.ezo", IO.SearchOption.AllDirectories).Count

                                                Dim status = dfileinfos1(u).status
                                                If (dwnnooffile > dfileinfos1(u).Nooffiles) Then
                                                    dfileinfos1(u).Nooffiles = dwnnooffile
                                                End If

                                                If status = "New" Then

                                                    dfileinfos1(u).status = "Initializing..."
                                                    'writetxtfle("Status Changed from New To Initialing...")
                                                    Dim Settings As New JsonSerializerSettings
                                                    Settings.Formatting = Formatting.Indented
                                                    Settings.NullValueHandling = NullValueHandling.Ignore
                                                    Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                                    File.WriteAllText(Dpath, json)

                                                    If (dwnnooffile > 0) Then
                                                        dfileinfos1(u).Nooffiles = dwnnooffile
                                                        dfileinfos1(u).status = "Processing..."
                                                        'writetxtfle("Status Changed from Initialing... To Processing...")
                                                        Settings.Formatting = Formatting.Indented
                                                        Settings.NullValueHandling = NullValueHandling.Ignore
                                                        json = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                                        File.WriteAllText(Dpath, json)
                                                    Else
                                                        dfileinfos1(u).status = "Completed"
                                                        Settings.Formatting = Formatting.Indented
                                                        Settings.NullValueHandling = NullValueHandling.Ignore
                                                        json = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                                        File.WriteAllText(Dpath, json)
                                                        'writetxtfle("No of files = " & dwnnooffile & " .so status changed To Completed")

                                                    End If

                                                End If

                                                dwnpath = dfileinfos1(u).foldername
                                                movpath = dfileinfos1(u).movepath
                                                dwnpass = dfileinfos1(u).passwordd
                                                dwnfoldersize = dfileinfos1(u).dfoldersize
                                                dwnnooffile = dfileinfos1(u).Nooffiles
                                                dwnextension = ".pdf"
                                                dwnbatchid = dfileinfos1(u).batchid
                                                dwndatime = dfileinfos1(u).datime
                                                dwnstatus = dfileinfos1(u).status
                                                nooffilesProcessed = dfileinfos1(u).NooffilesProcessed
                                                noffilesNotdecrypted = dfileinfos1(u).NooffilesUnprocessed

                                                If (dwnnooffile > 0) Then
                                                    If dwnstatus = "Processing..." Then
                                                        'writetxtfle("download status is processing... ReadAllFromFolder called")
                                                        Try
                                                            ReadAllFromFolder(dwnpath, movpath, dwnpass)
                                                        Catch ex As Exception

                                                        Finally
                                                            LogStatusUpdate(dwnpath, movpath)
                                                        End Try


                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                Else
                                    '  writetxtfle("Download Json Path is Empty,Please Make Some Decrypt File in Standalone Explorer")
                                End If
                            End If
                        Catch ex As Exception
                            writetxtfle("Exception in Download impersonateMethod=TRUE" & ex.Message)
                        End Try
                    ElseIf impersonateMethod = "False" Or impersonateMethod = "false" Then
                        ' writetxtfle("download impersonate False")
                        Try
                            If UNCUsername <> "" And UNCUsername <> "" And Domainname <> "" And UNCPassword <> "" Then
                                '      writetxtfle("all are having value")
                                Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials()
                                    '    writetxtfle("unc object created")
                                    If unc.NetUseWithCredentials(UNCPath, UNCUsername, Domainname, UNCPassword) Then
                                        '       writetxtfle("within download impersonate False Dpath=" + Dpath)
                                        'downloadallfiles(Dpath)
                                        If Dpath <> "" Then
                                            Dim uristring As String
                                            Dim dfileinfos As List(Of downloadfile)
                                            Dim uristring1 = File.ReadAllText(Dpath)
                                            If (uristring1.Length > 0) Then
                                                Dim dfileinfos1 As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(uristring1)
                                                If dfileinfos1.Count > 0 Then
                                                    For u = 0 To dfileinfos1.Count - 1
                                                        totfilesdecrypted = 0
                                                        totfilesscanned = 0
                                                        totfilesnotdecrypted = 0
                                                        uristring = File.ReadAllText(Dpath)
                                                        ' dfileinfos = ser.Deserialize(Of List(Of downloadfile))(uristring)

                                                        dwnnooffile = Directory.GetFiles(dfileinfos1(u).foldername + "\", "*.ezo", IO.SearchOption.AllDirectories).Count

                                                        Dim status = dfileinfos1(u).status
                                                        If (dwnnooffile > dfileinfos1(u).Nooffiles) Then
                                                            dfileinfos1(u).Nooffiles = dwnnooffile
                                                        End If
                                                        If status = "New" Then

                                                            dfileinfos1(u).status = "Initializing..."
                                                            'writetxtfle("Status Changed from New To Initialing...")
                                                            Dim Settings As New JsonSerializerSettings
                                                            Settings.Formatting = Formatting.Indented
                                                            Settings.NullValueHandling = NullValueHandling.Ignore
                                                            Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                                            File.WriteAllText(Dpath, json)

                                                            If (dwnnooffile > 0) Then
                                                                dfileinfos1(u).Nooffiles = dwnnooffile
                                                                dfileinfos1(u).status = "Processing..."
                                                                'writetxtfle("Status Changed from Initialing... To Processing...")
                                                                Settings.Formatting = Formatting.Indented
                                                                Settings.NullValueHandling = NullValueHandling.Ignore
                                                                json = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                                                File.WriteAllText(Dpath, json)
                                                            Else
                                                                dfileinfos1(u).status = "Completed"
                                                                Settings.Formatting = Formatting.Indented
                                                                Settings.NullValueHandling = NullValueHandling.Ignore
                                                                json = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                                                File.WriteAllText(Dpath, json)
                                                                'writetxtfle("No of files = " & dwnnooffile & " .so status changed To Completed")

                                                            End If
                                                        End If

                                                        dwnpath = dfileinfos1(u).foldername
                                                        movpath = dfileinfos1(u).movepath
                                                        dwnpass = dfileinfos1(u).passwordd
                                                        dwnfoldersize = dfileinfos1(u).dfoldersize
                                                        dwnnooffile = dfileinfos1(u).Nooffiles
                                                        dwnextension = ".pdf"
                                                        dwnbatchid = dfileinfos1(u).batchid
                                                        dwndatime = dfileinfos1(u).datime
                                                        dwnstatus = dfileinfos1(u).status
                                                        'writetxtfle("download status" & dwnstatus)
                                                        nooffilesProcessed = dfileinfos1(u).NooffilesProcessed
                                                        noffilesNotdecrypted = dfileinfos1(u).NooffilesUnprocessed

                                                        If (dwnnooffile > 0) Then
                                                            If dwnstatus = "Processing..." Then
                                                                'writetxtfle("download status is processing... ReadAllFromFolder called")
                                                                ReadAllFromFolder(dwnpath, movpath, dwnpass)
                                                                LogStatusUpdate(dwnpath, movpath)
                                                            End If
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        Else
                                            'writetxtfle("Download Json Path is Empty,Please Make Some Decrypt File in Standalone Explorer")
                                        End If
                                    Else
                                        writetxtfle("Failed to connect to UNC Credentials " & Dpath & vbCrLf & "LastError = " + unc.LastError.ToString)
                                    End If
                                End Using
                            End If
                        Catch ex As Exception
                            writetxtfle("Exception in Download impersonateMethod=FALSE" & ex.Message)
                        End Try
                    Else
                        'downloadallfiles(Dpath)
                        ' writetxtfle("download-3")
                        '  writetxtfle(Dpath)
                        If Dpath <> "" Then
                            ' writetxtfle("download-4")
                            Dim uristring As String
                            Dim dfileinfos As List(Of downloadfile)
                            'writetxtfle("download-5a")
                            ' Dim uristring1 = File.ReadAllText(Dpath)
                            Dim uristring1 = ReadQueueText(Dpath)
                            'writetxtfle("download-5b")
                            If (uristring1.Length > 0) Then
                                'writetxtfle("download-5c")
                                Dim dfileinfos1 As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(uristring1)
                                If dfileinfos1.Count > 0 Then
                                    '      writetxtfle("download-6")
                                    For u = 0 To dfileinfos1.Count - 1
                                        totfilesdecrypted = 0
                                        totfilesscanned = 0
                                        totfilesnotdecrypted = 0

                                        ' uristring = File.ReadAllText(Dpath)
                                        uristring = ReadQueueText(Dpath)
                                        'dfileinfos = ser.Deserialize(Of List(Of downloadfile))(uristring)

                                        'dwnnooffile = Directory.GetFiles(dfileinfos1(u).foldername + "\", "*.ezo", IO.SearchOption.AllDirectories).Count
                                        'writetxtfle("dwnnooffile: " & dwnnooffile)
                                        ' writetxtfle("download-6 folder: " & dfileinfos1(u).foldername)
                                        'writetxtfle("download-6 status: " & dfileinfos1(u).status)

                                        dwnnooffile = dfileinfos1(u).Nooffiles
                                        'If dwnnooffile <= 0 Then
                                        '    dwnnooffile = 1   ' Form2 sends 0; still allow Processing + ReadAllFromFolder
                                        'End If

                                        'writetxtfle("dwnnooffile: " & dwnnooffile)

                                        Dim status = dfileinfos1(u).status
                                        If (dwnnooffile > dfileinfos1(u).Nooffiles) Then
                                            dfileinfos1(u).Nooffiles = dwnnooffile
                                        End If
                                        If status = "New" Then

                                            dfileinfos1(u).status = "Initializing..."

                                            'writetxtfle("Status Changed from New To Initialing...")
                                            Dim Settings As New JsonSerializerSettings
                                            Settings.Formatting = Formatting.Indented
                                            Settings.NullValueHandling = NullValueHandling.Ignore
                                            Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                            ' File.WriteAllText(Dpath, json)
                                            ' WriteQueueText(Dpath, json)
                                            dfileinfos1(u).Nooffiles = 0
                                            dfileinfos1(u).status = "Processing..."
                                            dfileinfos1(u).NooffilesProcessed = 0
                                            'writetxtfle("Status Changed from Initialing... To Processing...")
                                            Settings.Formatting = Formatting.Indented
                                            Settings.NullValueHandling = NullValueHandling.Ignore
                                            json = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                            ' File.WriteAllText(Dpath, json)
                                            WriteQueueText(Dpath, json)
                                            If (dwnnooffile > 0) Then
                                                'dfileinfos1(u).Nooffiles = dwnnooffile
                                                'dfileinfos1(u).Nooffiles = 0
                                                'dfileinfos1(u).status = "Processing..."
                                                'dfileinfos1(u).Nooffilesprocessed = 0
                                                ''writetxtfle("Status Changed from Initialing... To Processing...")
                                                'Settings.Formatting = Formatting.Indented
                                                'Settings.NullValueHandling = NullValueHandling.Ignore
                                                'json = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                                '' File.WriteAllText(Dpath, json)
                                                'WriteQueueText(Dpath, json)
                                            Else
                                                'dfileinfos1(u).status = "Completed"
                                                'Settings.Formatting = Formatting.Indented
                                                'Settings.NullValueHandling = NullValueHandling.Ignore
                                                'json = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                                '' File.WriteAllText(Dpath, json)
                                                'WriteQueueText(status, json)
                                                ''writetxtfle("No of files = " & dwnnooffile & " .so status changed To Completed")

                                            End If
                                        End If

                                        dwnpath = dfileinfos1(u).foldername
                                        movpath = dfileinfos1(u).movepath
                                        dwnpass = dfileinfos1(u).passwordd
                                        dwnfoldersize = dfileinfos1(u).dfoldersize
                                        dwnnooffile = dfileinfos1(u).Nooffiles
                                        dwnextension = ".pdf"
                                        dwnbatchid = dfileinfos1(u).batchid
                                        dwndatime = dfileinfos1(u).datime
                                        dwnstatus = dfileinfos1(u).status
                                        nooffilesProcessed = dfileinfos1(u).NooffilesProcessed
                                        noffilesNotdecrypted = dfileinfos1(u).NooffilesUnprocessed

                                        'shiva & commented by sara 
                                        'If (dwnnooffile > 0) Then
                                        '    If dwnstatus = "Processing..." Then
                                        '        'writetxtfle("download status is processing... ReadAllFromFolder called")
                                        '        ReadAllFromFolder(dwnpath, movpath, dwnpass)
                                        '        LogStatusUpdate(dwnpath, movpath)
                                        '    End If
                                        'End If

                                        ' If (dwnnooffile > 0) Then
                                        If dwnstatus = "Processing..." Then
                                            'writetxtfle("download status is processing... ReadAllFromFolder called")
                                            Try
                                                'ReadAllFromFolder(dwnpath, movpath, dwnpass)
                                                newReadAllFromFolder(dwnpath, movpath, dwnpass)
                                            Catch ex As Exception
                                                Dim detail = ex.Message
                                                If TypeOf ex Is AggregateException Then
                                                    detail = String.Join(" | ", DirectCast(ex, AggregateException).Flatten().InnerExceptions.Select(Function(i) i.Message))
                                                ElseIf ex.InnerException IsNot Nothing Then
                                                    detail = ex.InnerException.Message
                                                End If
                                                writetxtfle("ReadAllFromFolder failed: " & detail)
                                            Finally
                                                LogStatusUpdate(dwnpath, movpath)
                                            End Try
                                        End If
                                        'End If

                                    Next
                                End If
                            Else
                                ' writetxtfle("Download Json Path is Empty,Please Make Some Decrypt File in Standalone Explorer")
                            End If
                        End If
                    End If
                Else
                    writetxtfle(Dpath & "File not exists")
                End If
            Catch ex As Exception
                writetxtfle("Exception in Download Function" & ex.Message)
            Finally
                '  writetxtfle("Process Completed")
            End Try
        Catch ex As Exception
            writetxtfle("Exception in Function Download" & ex.Message)
        End Try
    End Sub
    'shiva
    Public Sub downloadallfiles(dpath As String)
        Try
            'writetxtfle("downloadallfiles started-dpath=" + dpath)
            If dpath <> "" Then
                Dim uristring As String
                Dim dfileinfos As List(Of downloadfile)
                Dim uristring1 = File.ReadAllText(dpath)
                If (uristring1.Length > 0) Then
                    Dim dfileinfos1 As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(uristring1)
                    If dfileinfos1.Count > 0 Then
                        For u = 0 To dfileinfos1.Count - 1
                            totfilesdecrypted = 0
                            totfilesscanned = 0
                            totfilesnotdecrypted = 0

                            uristring = File.ReadAllText(dpath)
                            ' dfileinfos = ser.Deserialize(Of List(Of downloadfile))(uristring)

                            Dim status = dfileinfos1(u).status
                            If (dwnnooffile > dfileinfos1(u).Nooffiles) Then
                                dfileinfos1(u).Nooffiles = dwnnooffile
                            End If
                            If status = "New" Then

                                dfileinfos1(u).status = "Initializing..."
                                'writetxtfle("Status Changed from New To Initialing...")
                                Dim Settings As New JsonSerializerSettings
                                Settings.Formatting = Formatting.Indented
                                Settings.NullValueHandling = NullValueHandling.Ignore
                                Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                File.WriteAllText(dpath, json)

                                If (dwnnooffile > 0) Then
                                    dfileinfos1(u).Nooffiles = dwnnooffile
                                    dfileinfos1(u).status = "Processing..."
                                    'writetxtfle("Status Changed from Initialing... To Processing...")
                                    Settings.Formatting = Formatting.Indented
                                    Settings.NullValueHandling = NullValueHandling.Ignore
                                    json = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                    File.WriteAllText(dpath, json)
                                Else
                                    dfileinfos1(u).status = "Completed"
                                    Settings.Formatting = Formatting.Indented
                                    Settings.NullValueHandling = NullValueHandling.Ignore
                                    json = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos1, Settings)
                                    File.WriteAllText(dpath, json)
                                    'writetxtfle("No of files = " & dwnnooffile & " .so status changed To Completed")

                                End If
                            End If

                            dwnpath = dfileinfos1(u).foldername
                            movpath = dfileinfos1(u).movepath
                            dwnpass = dfileinfos1(u).passwordd
                            dwnfoldersize = dfileinfos1(u).dfoldersize
                            dwnnooffile = dfileinfos1(u).Nooffiles
                            dwnextension = ".pdf"
                            dwnbatchid = dfileinfos1(u).batchid
                            dwndatime = dfileinfos1(u).datime
                            dwnstatus = dfileinfos1(u).status
                            nooffilesProcessed = dfileinfos1(u).NooffilesProcessed
                            noffilesNotdecrypted = dfileinfos1(u).NooffilesUnprocessed

                            'shiva
                            If (dwnnooffile > 0) Then
                                If dwnstatus = "Processing..." Then
                                    ReadAllFromFolder(dwnpath, movpath, dwnpass)
                                    LogStatusUpdate(dwnpath, movpath)
                                End If
                            End If

                        Next
                    Else
                        writetxtfle("downloadallfiles-download json is 0")
                    End If
                End If
            Else
                writetxtfle("Download Json Path is Empty,Please Make Some Decrypt File in Standalone Explorer")
            End If
        Catch ex As Exception
            writetxtfle("Exception in Downloadallfiles Function" & ex.Message)
        Finally
            '  writetxtfle("Process Completed")
        End Try
    End Sub
    Private Function GetRelativePathFromArchive(uncPath As String) As String
        Dim p = uncPath.Replace("/"c, "\"c)
        Dim idx = p.IndexOf("\Archive\", StringComparison.OrdinalIgnoreCase)
        If idx >= 0 Then Return p.Substring(idx + 1)
        Return Path.GetFileName(p.TrimEnd("\"c))
    End Function
    Public Function newReadAllFromFolder(sourcepath As String, targetpath As String, downloadpass As String) As String
        Dim str As String = "DONE"
        Try
            Dim currentdir = sourcepath.Substring(sourcepath.LastIndexOf("") + 1)
            currentdir = Path.Combine(targetpath, GetRelativePathFromArchive(sourcepath))
            If Not (Directory.Exists(currentdir)) Then
                Directory.CreateDirectory(currentdir)
            End If
            If dwnstatus = "Processing..." Then
                Try
                    cachedDownloadKey = CreateKey(dwnpass)
                    cachedDownloadIV = CreateIV(dwnpass)
                    Dim files = Directory.GetFiles(sourcepath, "*.ezo")
                    If files.Length > 0 AndAlso downloadpass <> "" AndAlso dwnstatus = "Processing..." Then
                        Dim parallelOpts As New ParallelOptions With {
                            .MaxDegreeOfParallelism = DOWNLOAD_DECRYPT_PARALLELISM
                        }
                        Parallel.ForEach(files, parallelOpts,
                            Sub(filepath)
                                ' LOCAL dest — no shared strOutputDecrypt
                                Dim dest = Path.Combine(currentdir,
                                    Path.GetFileName(filepath).Replace(".ezo", ".pdf"))
                                DecryptOnefileSafe(filepath, dest, cachedDownloadKey, cachedDownloadIV)
                                ' Coordinator-only queue flush (not WriteQueueText in workers)
                                Dim total = totfilesscanned + totfilesnotdecrypted
                                If total Mod DOWNLOAD_QUEUE_FLUSH_EVERY = 0 Then
                                    If Interlocked.CompareExchange(flushGate, 1, 0) = 0 Then
                                        Try
                                            FlushDownloadQueueProgress()
                                        Finally
                                            Interlocked.Exchange(flushGate, 0)
                                        End Try
                                    End If
                                End If
                            End Sub)
                        ' Flush once per folder batch when parallel work finishes
                        FlushDownloadQueueProgress()
                    End If

                    For Each directorypath As String In IO.Directory.GetDirectories(sourcepath)
                        ' ReadAllFromFolder(directorypath, currentdir, downloadpass)
                        newReadAllFromFolder(directorypath, targetpath, downloadpass)
                    Next
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
        Return str
    End Function
    Public Function ReadAllFromFolder(sourcepath As String, targetpath As String, downloadpass As String)
        'writetxtfle("ReadAllFromFolder started ")
        Dim currentdir = sourcepath.Substring(sourcepath.LastIndexOf("\") + 1)
        currentdir = Path.Combine(targetpath, GetRelativePathFromArchive(sourcepath))
        If Not (Directory.Exists(currentdir)) Then
            Directory.CreateDirectory(currentdir)
        End If

        'writetxtfle("Total number of files in path" & Directory.GetFiles(sourcepath, "*.ezo").Count)
        For Each filepath As String In IO.Directory.GetFiles(sourcepath, "*.ezo")

            'writetxtfle("ReadAllFromFolder-take file-" & filepath)
            Dim dwFile As String = Path.GetFileName(filepath)
            strOutputDecrypt = dwFile.Replace(".ezo", ".pdf")
            Dim destinationpath = Path.Combine(currentdir, strOutputDecrypt)
            '                                      Processing...
            If downloadpass <> "" And dwnstatus = "Processing..." Then
                'writetxtfle("dwnstatus is success-decryptDownloadfile")
                decryptDownloadfile(filepath, destinationpath, downloadpass, CryptoAction.ActionDecrypt)
            End If
        Next

        For Each directorypath As String In IO.Directory.GetDirectories(sourcepath)
            ' ReadAllFromFolder(directorypath, currentdir, downloadpass)
            ReadAllFromFolder(directorypath, targetpath, downloadpass)
        Next
        'writetxtfle("ReadAllFromFolder end ")

    End Function
    Private Function DecryptOnefileSafe(inputPath As String, outputPath As String, bytKey As Byte(), bytIV As Byte()) As Boolean
        Try
            Using fsinput As New FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read)
                Using fsOutput As New FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None)
                    fsOutput.SetLength(0)
                    Using csp As New RijndaelManaged
                        Using cs As New CryptoStream(fsOutput, csp.CreateDecryptor(bytKey, bytIV), CryptoStreamMode.Write)
                            Dim buffer(4095) As Byte
                            Dim read As Integer
                            Do
                                read = fsinput.Read(buffer, 0, buffer.Length)
                                If read = 0 Then Exit Do
                                cs.Write(buffer, 0, read)
                            Loop
                        End Using
                    End Using
                End Using
            End Using
            Interlocked.Increment(totfilesscanned)
            Interlocked.Increment(totfilesdecrypted)
            Return True
        Catch ex As Exception
            Interlocked.Increment(totfilesscanned)
            SyncLock notdecryptedfiles
                notdecryptedfiles.Add(inputPath)
            End SyncLock
            Try
                If File.Exists(outputPath) Then
                    File.Delete(outputPath)
                End If
            Catch

            End Try
            Return False
        End Try
    End Function
    Private Sub FlushDownloadQueueProgress()
        'optional : only one flushed at a time 
        SyncLock queueFlushLock
            Dim duristring = ReadQueueText(Dpath)
            Dim dfileinfos = ser.Deserialize(Of List(Of downloadfile))(duristring)
            For k = 0 To dfileinfos.Count - 1
                If dwnpath = dfileinfos(k).foldername AndAlso dwnbatchid = dfileinfos(k).batchid Then
                    If dfileinfos(k).status = "Processing..." Then
                        dfileinfos(k).NooffilesProcessed = totfilesscanned      ' read after Interlocked
                        dfileinfos(k).NooffilesUnprocessed = totfilesnotdecrypted
                        Dim Settings As New JsonSerializerSettings
                        Settings.Formatting = Formatting.Indented
                        Settings.NullValueHandling = NullValueHandling.Ignore
                        Dim json = JsonConvert.SerializeObject(dfileinfos, settings)
                        WriteQueueText(Dpath, json)
                    End If
                End If
            Next
        End SyncLock
    End Sub
    Private Sub decryptDownloadfile(ByVal strInputFile As String, ByVal stroutputfile As String, ByVal password As String,
                                     ByVal Direction As CryptoAction)
        Dim fsInput As System.IO.FileStream
        Dim fsOutput As System.IO.FileStream

        Try
            'writetxtfle("decryptDownloadfile start")
            'Setup file streams to handle input and output.
            fsInput = New System.IO.FileStream(strInputFile, FileMode.Open,
                                            FileAccess.Read)
            fsOutput = New System.IO.FileStream(stroutputfile, FileMode.OpenOrCreate,
                                                FileAccess.Write)
            fsOutput.SetLength(0) 'make sure fsOutput is empty
            'Declare variables for encrypt/decrypt process.
            Dim bytBuffer(4096) As Byte 'holds a block of bytes for processing
            Dim lngBytesProcessed As Long = 0 'running count of bytes processed
            Dim lngFileLength As Long = fsInput.Length 'the input file's length
            Dim intBytesInCurrentBlock As Integer 'current bytes being processed
            'Declare your CryptoServiceProvider.
            Dim cspRijndael As New System.Security.Cryptography.RijndaelManaged

            Dim bytKey As Byte()
            Dim bytIV As Byte()

            'Send the password to the CreateKey function.
            bytKey = CreateKey(password)
            'Send the password to the CreateIV function.
            bytIV = CreateIV(password)

            Dim csCryptoStream As CryptoStream
            Select Case Direction
                Case CryptoAction.ActionEncrypt
                    csCryptoStream = New CryptoStream(fsOutput,
                    cspRijndael.CreateEncryptor(bytKey, bytIV),
                    CryptoStreamMode.Write)

                Case CryptoAction.ActionDecrypt
                    csCryptoStream = New CryptoStream(fsOutput,
                    cspRijndael.CreateDecryptor(bytKey, bytIV),
                    CryptoStreamMode.Write)
            End Select
            'writetxtfle("File length is " & lngFileLength)
            'Use While to loop until all of the file is processed.

            While lngBytesProcessed < lngFileLength
                'Read file with the input filestream.
                intBytesInCurrentBlock = fsInput.Read(bytBuffer, 0, 4096)
                'Write output file with the cryptostream.
                csCryptoStream.Write(bytBuffer, 0, intBytesInCurrentBlock)
                'Update lngBytesProcessed
                lngBytesProcessed = lngBytesProcessed + CLng(intBytesInCurrentBlock)
                'Update Progress Bar
                'writetxtfle(lngBytesProcessed)
            End While
            'shiva
            totfilesscanned = totfilesscanned + 1
            '  writetxtfle("totalfilesscanned: " + totfilesscanned.ToString())
            'Close FileStreams and CryptoStream.
            csCryptoStream.Close()
            'shiva
            totfilesdecrypted = totfilesdecrypted + 1
            'writetxtfle("totfilesdecrypted: " + totfilesdecrypted.ToString())
            fsOutput.Close()
            'If encrypting then delete the original unencrypted file.
            If Direction = CryptoAction.ActionEncrypt Then
                Dim fileOriginal As New FileInfo(strOutputDecrypt)
                fileOriginal.Delete()
            End If
            'If decrypting then delete the encrypted file.
            If Direction = CryptoAction.ActionDecrypt Then
                Dim fileEncrypted As New FileInfo(strOutputDecrypt)
                fileEncrypted.Delete()
            End If

            Dim Wrap As String = Chr(13) + Chr(10)
            If Direction = CryptoAction.ActionEncrypt Then
                writetxtfle("Encryption Complete" + Wrap + Wrap +
                        "Total bytes processed = " +
                        lngBytesProcessed.ToString + " Done. " + stroutputfile.ToString())

            Else
                'Dim duristring = File.ReadAllText(Dpath)
                If Not String.IsNullOrWhiteSpace(flushRaw) Then
                    Integer.TryParse(flushRaw, DOWNLOAD_QUEUE_FLUSH_EVERY)
                End If
                If DOWNLOAD_QUEUE_FLUSH_EVERY < 1 Then DOWNLOAD_QUEUE_FLUSH_EVERY = 50
                Dim totalHandled As Integer = totfilesscanned + totfilesnotdecrypted
                If totalHandled Mod DOWNLOAD_QUEUE_FLUSH_EVERY <> 0 Then
                    Return
                End If
                Dim duristring = ReadQueueText(Dpath)
                    Dim dfileinfos As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(duristring)
                    If dfileinfos.Count > 0 Then
                        For k = 0 To dfileinfos.Count - 1
                            If dwnpath = dfileinfos(k).foldername And dwnbatchid = dfileinfos(k).batchid Then
                                If dfileinfos(k).status = "Processing..." Then
                                    'dfileinfos(k).status = "Completed"
                                    dfileinfos(k).NooffilesProcessed = totfilesscanned
                                    dfileinfos(k).NooffilesUnprocessed = totfilesnotdecrypted
                                ' writetxtfle("NooffilesProcessed: " & totfilesscanned.ToString())
                                Dim Settings As New JsonSerializerSettings
                                    Settings.Formatting = Formatting.Indented
                                    Settings.NullValueHandling = NullValueHandling.Ignore
                                    Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos, Settings)
                                    'File.WriteAllText(Dpath, json)
                                    WriteQueueText(Dpath, json)
                                   ' writetxtfle("writeQueueText")
                                End If

                                Dim nooffilescount = dfileinfos(k).Nooffiles
                                'If nooffilescount = totfilesscanned Then
                                '    If nooffilescount = totfilesdecrypted Then
                                '        Dim status = dfileinfos(k).status
                                '        If status = "Processing..." Then
                                '            'dfileinfos(k).status = "Completed"
                                '            dfileinfos(k).NooffilesProcessed = totfilesscanned
                                '            dfileinfos(k).NooffilesUnprocessed = totfilesnotdecrypted

                                '            Dim Settings As New JsonSerializerSettings
                                '            Settings.Formatting = Formatting.Indented
                                '            Settings.NullValueHandling = NullValueHandling.Ignore
                                '            Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos, Settings)
                                '            'File.WriteAllText(Dpath, json)
                                '            WriteQueueText(Dpath, json)
                                '        End If
                                '    End If
                                'ElseIf nooffilescount > totfilesscanned Then
                                '    dfileinfos(k).NooffilesProcessed = totfilesscanned
                                '    dfileinfos(k).NooffilesUnprocessed = totfilesnotdecrypted

                                '    Dim Settings As New JsonSerializerSettings
                                '    Settings.Formatting = Formatting.Indented
                                '    Settings.NullValueHandling = NullValueHandling.Ignore
                                '    Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos, Settings)
                                '    'File.WriteAllText(Dpath, json)
                                '    WriteQueueText(Dpath, json)
                                'End If
                            Else
                            writetxtfle("elsepart came")
                        End If
                        Next
                    End If
                    'Update the user when the file is done.
                    writetxtfle("Download Decryption Completed" + Batchid + " Done. FileName : " + stroutputfile.ToString())
                End If

            'Catch file not found error.
        Catch When Err.Number = 53 'if file not found
            writetxtfle("Please check to make sure the path and filename" +
                    "are correct and if the file exists." + " Invalid Path or Filename")
        Catch ex As Exception
            totfilesnotdecrypted = totfilesnotdecrypted + 1
            fsInput.Close()
            fsOutput.Close()
            If Direction = CryptoAction.ActionDecrypt Then
                'writetxtfle("Unable to Download " + Batchid + "." + strInputFile.ToString() + "." + ex.Message)
                notdecryptedfiles.Add(strInputFile)

                Dim fileDelete As New FileInfo(stroutputfile)
                fileDelete.Delete()

                'Dim duristring = File.ReadAllText(Dpath)
                If Not String.IsNullOrWhiteSpace(flushRaw) Then
                    Integer.TryParse(flushRaw, DOWNLOAD_QUEUE_FLUSH_EVERY)
                End If
                If DOWNLOAD_QUEUE_FLUSH_EVERY < 1 Then DOWNLOAD_QUEUE_FLUSH_EVERY = 50
                Dim totalHandled As Integer = totfilesscanned + totfilesnotdecrypted
                If totalHandled Mod DOWNLOAD_QUEUE_FLUSH_EVERY <> 0 Then
                    Return
                End If
                Dim duristring = ReadQueueText(Dpath)
                Dim dfileinfos As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(duristring)
                If dfileinfos.Count > 0 Then
                    For k = 0 To dfileinfos.Count - 1
                        If dwnpath = dfileinfos(k).foldername And dwnbatchid = dfileinfos(k).batchid Then
                            Dim status = dfileinfos(k).status
                            If status = "Processing..." Then
                                dfileinfos(k).NooffilesProcessed = totfilesscanned
                                dfileinfos(k).NooffilesUnprocessed = totfilesnotdecrypted
                                Dim Settings As New JsonSerializerSettings
                                Settings.Formatting = Formatting.Indented
                                Settings.NullValueHandling = NullValueHandling.Ignore
                                Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos, Settings)
                                'File.WriteAllText(Dpath, json)
                                WriteQueueText(Dpath, json)

                                'Dim nooffilescount = dfileinfos(k).Nooffiles
                                'If nooffilescount = totfilesscanned Then
                                '    dfileinfos(k).NooffilesProcessed = totfilesscanned
                                '    dfileinfos(k).NooffilesUnprocessed = totfilesnotdecrypted
                                '    ' dfileinfos(k).status = "Completed"
                                'Else
                                '    dfileinfos(k).NooffilesProcessed = totfilesscanned
                                '    dfileinfos(k).NooffilesUnprocessed = totfilesnotdecrypted
                                'End If

                            End If
                        End If
                    Next
                End If
                writetxtfle("Download : Please check to make sure that you entered the correct" +
                       "password." + namefile + " Invalid Password.." + Batchid)
            Else
                'Dim fileDelete As New FileInfo(stroutputfile)
                'fileDelete.Delete()
                writetxtfle("This file cannot be encrypted." + Batchid + " ....Invalid File")
            End If
            writetxtfle("decryptDownloadfile Completed")
        End Try

    End Sub


    Private Sub Encryptordecryptfile(ByVal strInputFile As String, ByVal stroutputfile As String, ByVal password As String,
                                     ByVal Direction As CryptoAction, ByVal keepbothfiles As String)
        writetxtfle("File Taken :" & strInputFile)
        Dim fsInput As System.IO.FileStream
        Dim fsOutput As System.IO.FileStream

        Try
            'Setup file streams to handle input and output.
            fsInput = New System.IO.FileStream(strInputFile, FileMode.Open,
                                            FileAccess.Read)
            fsOutput = New System.IO.FileStream(stroutputfile, FileMode.OpenOrCreate,
                                                FileAccess.Write)
            fsOutput.SetLength(0) 'make sure fsOutput is empty
            'Declare variables for encrypt/decrypt process.
            Dim bytBuffer(4096) As Byte 'holds a block of bytes for processing
            Dim lngBytesProcessed As Long = 0 'running count of bytes processed
            Dim lngFileLength As Long = fsInput.Length 'the input file's length
            Dim intBytesInCurrentBlock As Integer 'current bytes being processed
            'Declare your CryptoServiceProvider.
            Dim cspRijndael As New System.Security.Cryptography.RijndaelManaged

            Dim bytKey As Byte()
            Dim bytIV As Byte()

            'Send the password to the CreateKey function.
            bytKey = CreateKey(password)
            'Send the password to the CreateIV function.
            bytIV = CreateIV(password)

            Dim csCryptoStream As CryptoStream

            Select Case Direction
                Case CryptoAction.ActionEncrypt
                    csCryptoStream = New CryptoStream(fsOutput,
                    cspRijndael.CreateEncryptor(bytKey, bytIV),
                    CryptoStreamMode.Write)

                Case CryptoAction.ActionDecrypt
                    csCryptoStream = New CryptoStream(fsOutput,
                    cspRijndael.CreateDecryptor(bytKey, bytIV),
                    CryptoStreamMode.Write)
            End Select

            'Use While to loop until all of the file is processed.
            While lngBytesProcessed < lngFileLength
                'Read file with the input filestream.
                intBytesInCurrentBlock = fsInput.Read(bytBuffer, 0, 4096)
                'Write output file with the cryptostream.
                csCryptoStream.Write(bytBuffer, 0, intBytesInCurrentBlock)
                'Update lngBytesProcessed
                lngBytesProcessed = lngBytesProcessed + CLng(intBytesInCurrentBlock)
                'Update Progress Bar
            End While
            dtotfilesscanned = dtotfilesscanned + 1

            'Close FileStreams and CryptoStream.
            csCryptoStream.Close()
            'writetxtfle("within Encryptordecryptfile function file converted")
            'shiva
            dtotfilesdecrypted = dtotfilesdecrypted + 1
            fsOutput.Close()
            fsInput.Close()
            'If encrypting then delete the original unencrypted file.
            If Direction = CryptoAction.ActionEncrypt Then
                If (keepbothfiles = "false") Then
                    Dim fileOriginal As New FileInfo(strInputFile)
                    fileOriginal.Delete()
                End If
            End If
            'If decrypting then delete the encrypted file.
            If Direction = CryptoAction.ActionDecrypt Then
                If (keepbothfiles = "false") Then
                    Dim fileEncrypted As New FileInfo(strInputFile)
                    fileEncrypted.Delete()
                End If
            End If

            Dim Wrap As String = Chr(13) + Chr(10)
            If Direction = CryptoAction.ActionEncrypt Then
                writetxtfle("Encryption Completed" + Wrap + Wrap +
                        "Total bytes processed = " +
                        lngBytesProcessed.ToString + " Done. " + stroutputfile.ToString())
            Else
                'writetxtfle("within Encryptordecryptfile function File ok")
                Dim uristring = File.ReadAllText(jpath)
                Dim fileinfos As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring)
                If fileinfos.Count > 0 Then
                    For k = 0 To fileinfos.Count - 1
                        If jsonpath = fileinfos(k).foldername And Batchid = fileinfos(k).batchid Then
                            Dim nooffilescount = fileinfos(k).Nooffiles
                            If nooffilescount = dtotfilesscanned Then
                                If nooffilescount = dtotfilesdecrypted Then
                                    Dim status = fileinfos(k).status
                                    If status = "Processing..." Then
                                        fileinfos(k).NooffilesUnprocessed = dtotfilesnotdecrypted
                                        fileinfos(k).NooffilesProcessed = dtotfilesscanned
                                        fileinfos(k).status = "Completed"
                                        Dim Settings As New JsonSerializerSettings
                                        Settings.Formatting = Formatting.Indented
                                        Settings.NullValueHandling = NullValueHandling.Ignore
                                        Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos, Settings)
                                        File.WriteAllText(jpath, json)
                                    End If
                                End If
                            ElseIf nooffilescount > dtotfilesscanned Then
                                fileinfos(k).NooffilesUnprocessed = dtotfilesnotdecrypted
                                fileinfos(k).NooffilesProcessed = dtotfilesscanned
                                Dim Settings As New JsonSerializerSettings
                                Settings.Formatting = Formatting.Indented
                                Settings.NullValueHandling = NullValueHandling.Ignore
                                Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos, Settings)
                                File.WriteAllText(jpath, json)
                            End If
                        End If
                    Next
                End If
                'Update the user when the file is done.
                'writetxtfle("Decryption Complete" + Batchid + " Done." + stroutputfile.ToString())
            End If
            'Catch file not found error.
        Catch When Err.Number = 53 'if file not found
            writetxtfle("Please check to make sure the path and filename" +
                    "are correct and if the file exists." + " Invalid Path or Filename")
        Catch
            'writetxtfle("within Encryptordecryptfile function file Exception")
            dtotfilesnotdecrypted = dtotfilesnotdecrypted + 1
            fsInput.Close()
            fsOutput.Close()
            If Direction = CryptoAction.ActionDecrypt Then
                Dim fileDelete As New FileInfo(stroutputfile)
                fileDelete.Delete()
                Dim uristring = File.ReadAllText(jpath)
                Dim fileinfos As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring)
                If fileinfos.Count > 0 Then
                    For k = 0 To fileinfos.Count - 1
                        'writetxtfle("within Encryptordecryptfile function Exception Total file count " & fileinfos.Count)
                        If jsonpath = fileinfos(k).foldername And Batchid = fileinfos(k).batchid Then
                            Dim status = fileinfos(k).status
                            If status = "Processing..." Then
                                'writetxtfle("within Encryptordecryptfile function Exception Enter into Processing")
                                Dim nooffilescount = fileinfos(k).Nooffiles
                                If nooffilescount = dtotfilesscanned Then
                                    fileinfos(k).NooffilesProcessed = dtotfilesscanned
                                    fileinfos(k).NooffilesUnprocessed = dtotfilesnotdecrypted
                                    fileinfos(k).status = "Completed"
                                Else

                                    fileinfos(k).NooffilesProcessed = dtotfilesscanned
                                    fileinfos(k).NooffilesUnprocessed = dtotfilesnotdecrypted
                                End If
                                Dim Settings As New JsonSerializerSettings
                                Settings.Formatting = Formatting.Indented
                                Settings.NullValueHandling = NullValueHandling.Ignore
                                Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(fileinfos, Settings)
                                'File.WriteAllText(jpath, json)
                                WriteQueueText(jpath, json)
                            End If

                        End If

                    Next
                End If
                writetxtfle("Please check to make sure that you entered the correct" +
                       "password." + namefile + " Invalid Password.." + Batchid)
            Else
                Dim fileDelete As New FileInfo(stroutputfile)
                fileDelete.Delete()

                writetxtfle("This file cannot be encrypted." + Batchid + " ....Invalid File")
            End If
        End Try
    End Sub
    Public Sub LogStatusUpdate(downloadpath As String, movpath As String)
        Dim duristring = ReadQueueText(Dpath)
        Dim dfileinfos = ser.Deserialize(Of List(Of downloadfile))(duristring)
        For k = 0 To dfileinfos.Count - 1
            If dwnpath = dfileinfos(k).foldername And dwnbatchid = dfileinfos(k).batchid Then
                dfileinfos(k).Nooffiles = totfilesscanned
                dfileinfos(k).NooffilesProcessed = totfilesscanned
                dfileinfos(k).NooffilesUnprocessed = totfilesnotdecrypted
                dfileinfos(k).status = "Completed"
                Dim Settings As New JsonSerializerSettings
                Settings.Formatting = Formatting.Indented
                Settings.NullValueHandling = NullValueHandling.Ignore
                Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(dfileinfos, Settings)
                WriteQueueText(Dpath, json)
            End If
        Next
        writetxtfle("======================================")
        writetxtfle("DOWLOADED FILE COMPLETED STATUS")
        writetxtfle("Downloaded From :" & downloadpath)
        writetxtfle("Downloaded To :" & movpath)
        writetxtfle("Total Processed Files :" & totfilesscanned)
        writetxtfle("Total Converted Files :" & totfilesdecrypted)
        If (notdecryptedfiles.Count > 0) Then
            'writetxtfle("Total Not Converted Files :" & notdecryptedfiles.Count)
            writetxtfle("Total Not Converted Files :" & totfilesnotdecrypted)
            writetxtfle("Not Converted Files List")
            For i = 0 To notdecryptedfiles.Count - 1
                writetxtfle(notdecryptedfiles.Item(i))
            Next
        Else
            writetxtfle("Not Converted Files List is Zero")
        End If
        writetxtfle("======================================")
        totfilesdecrypted = 0
        totfilesscanned = 0
        totfilesnotdecrypted = 0
        notdecryptedfiles.Clear()
    End Sub
    Public Function dir() As String
        Dim source As String = ""
        Dim apppath As String = ""
        Try
            apppath = System.Reflection.Assembly.GetEntryAssembly().Location
            apppath = Path.GetDirectoryName(apppath)
            source = apppath + "\log"
            If Not Directory.Exists(source) Then
                Directory.CreateDirectory(source)
            End If
        Catch ex As Exception
        End Try
        Return source
    End Function

    Public Sub writetxtfle(ByVal msg As String)
        Try
            Using sw As StreamWriter = New StreamWriter(filelocation, True)
                sw.WriteLine(Format(DateAndTime.Now, "MM/dd/yyyy hh:mm:ss") + " : " + msg)
            End Using
            'System.Windows.Forms.MessageBox.Show(msg)
        Catch ex As Exception
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

        Return bytIV
    End Function

End Class



'Public Function Encrypt()
'    Try
'        Dim filename = jsonpath
'        Dim iPosition As Integer = 0
'        Dim i As Integer = 0
'        'Get the position of the last "\" in the OpenFileDialog.FileName path.
'        '-1 is when the character your searching for is not there.
'        'IndexOf searches from left to right.
'        While filename.IndexOf("\", i) <> -1
'            iPosition = filename.IndexOf("\", i)
'            i = iPosition + 1
'        End While
'        'Assign strOutputFile to the position after the last "\" in the path.
'        'This position is the beginning of the file name.
'        strOutputEncrypt = filename.Substring(iPosition + 1)
'        'Assign S the entire path, ending at the last "\".
'        Dim S As String = filename.Substring(0, iPosition + 1)
'        'Replace the "." in the file extension with "_".
'        strOutputEncrypt = strOutputEncrypt.Replace(".", "_")
'        'The final file name.  XXXXX.encrypt
'        Dim despath = S + strOutputEncrypt + ".ezo"
'        'Encryptordecryptfile(strOutputEncrypt, despath, CryptoAction.ActionEncrypt)
'    Catch ex As Exception
'    End Try
'End Function