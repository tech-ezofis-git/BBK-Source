Imports System.Text.RegularExpressions

Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.Configuration
Imports System.Collections.Specialized
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Data

Public Class eZLicense


    Private Function decryptkey(ByVal key As String) As DataSet
        Try
            Dim pool As String = "abcdefghijklmnopqrstuvwxyz"
            Dim datepool As String = " !zcewgfir@lm#spq&htnvdajyubxo$k"
            Dim pool1 As String = pool.ToUpper
            Dim appnamewithcount As String = ""
            Dim poolindex As Int16 = 0
            Dim expiredate As String = ""
            Dim trialdays As String = ""
            Dim splitstr As String()
            Dim keytype As String = ""
            Dim rgx As New Regex("[^A-Z\d]")
            Dim rgx1 As New Regex("[^0-9]")
            Dim rgx2 As New Regex("[^A-Z]")
            Dim appnamewithcountarray As String() = Nothing
            Dim dt As New DataTable
            Dim ds As New DataSet
            dt.Columns.Add("Appname")
            dt.Columns.Add("Appcount")
            dt.Columns.Add("expiredate")
            dt.Columns.Add("Keytype")
            splitstr = key.Split("-")
            If splitstr(0).Length < 4 And splitstr.Length < 3 Then
                Throw New FaultException("Invalid Key!")
                'ElseIf splitstr(1).Length < 4 Then
                '  Throw New FaultException("Invalid Key!")
            End If
            keytype = splitstr(0).Substring(0, 1)
            If Char.IsLetter(keytype) Then
                poolindex = pool.ToString.IndexOf(keytype)
                If poolindex < 0 Then
                    poolindex = pool1.ToString.IndexOf(keytype)
                End If
                If poolindex <= 8 Then
                    keytype = "K1"
                    trialdays = splitstr(0).Substring(0, 1) + splitstr(0).Substring(3, 1)
                    dt.Columns.Add("Trialdays")
                ElseIf poolindex >= 17 Then
                    keytype = "K3"
                Else
                    keytype = "K2"
                    trialdays = splitstr(0).Substring(0, 1) + splitstr(0).Substring(3, 1)
                    dt.Columns.Add("Trialdays")
                End If
            Else
                keytype = splitstr(0).Substring(1, 1)
                poolindex = pool.IndexOf(keytype)
                If poolindex < 0 Then
                    poolindex = pool1.IndexOf(keytype)
                End If
                If poolindex <= 8 Then
                    keytype = "K1"
                    trialdays = splitstr(0).Substring(0, 1) + splitstr(0).Substring(3, 1)
                    dt.Columns.Add("Trialdays")
                ElseIf poolindex >= 17 Then
                    keytype = "K3"
                Else
                    keytype = "K2"
                    trialdays = splitstr(0).Substring(0, 1) + splitstr(0).Substring(3, 1)
                    dt.Columns.Add("Trialdays")
                End If
            End If
            'If Regex.Match(splitstr(1), "\d").Success Then
            'expiredate = splitstr(1).ToString()
            'expiredate = (splitstr(1).Substring(0, 1)).ToString + (pool.IndexOf(splitstr(1).Substring(1, 1)) + 1).ToString + (splitstr(1).Substring(2, 1)).ToString + (pool.IndexOf(splitstr(1).Substring(3, 1)) + 1).ToString
            expiredate = (datepool.IndexOf(splitstr(1).Substring(2, 1))).ToString + (datepool.IndexOf(splitstr(1).Substring(3, 1))).ToString + "-" +
               (datepool.IndexOf(splitstr(1).Substring(1, 1))).ToString + "-" + (datepool.IndexOf(splitstr(1).Substring(0, 1))).ToString
            'Else
            'expiredate = "1"
            'End If
            For i As Integer = 2 To splitstr.Length - 1
                appnamewithcount = appnamewithcount + splitstr(i).Trim()
            Next
            If appnamewithcount <> "" Then
                appnamewithcountarray = rgx.Replace(appnamewithcount, ",").Replace(",,", ",").Split(",")
            End If
            If appnamewithcountarray.Length <> 0 Then
                For i As Integer = 0 To appnamewithcountarray.Length - 1
                    Dim dr As DataRow = dt.NewRow
                    ' Dim appname = Findappnamebyappcode(rgx2.Replace(appnamewithcountarray(i), ""))
                    Dim appname = "ezofis Standalone App"
                    If appname = "" Then
                        Throw New FaultException("Invalid Key!")
                    Else
                        dr("Appname") = appname
                    End If
                    Dim Appcount = rgx1.Replace(appnamewithcountarray(i), "")
                    If Appcount = "" Then
                        Throw New FaultException("Invalid Key!")
                    Else
                        dr("Appcount") = Appcount
                    End If
                    dr("expiredate") = expiredate
                    dr("Keytype") = keytype.ToString()
                    If keytype = "K1" Or keytype = "K2" Then
                        dr("Trialdays") = trialdays
                    End If
                    dt.Rows.Add(dr)
                Next
            End If
            ds.Tables.Add(dt)
            Return ds
        Catch ex As Exception
            Throw New FaultException(ex.ToString())
        End Try
    End Function

    'Public Function ActivateLicense(ByVal key As String) As String
    '    Dim Result As String = ""
    '    Try
    '        Dim ResLicense = decryptkey(key)
    '        If Not ResLicense Is Nothing AndAlso ResLicense.Tables.Count > 0 AndAlso ResLicense.Tables(0).Rows.Count > 0 Then
    '            Dim currentDate = DateTime.Now.ToShortDateString
    '            Dim Appcon As NameValueCollection = CType(ConfigurationSettings.GetConfig("appSettings"), NameValueCollection)
    '            Dim cc = ResLicense.Tables(0).Rows(0)("expiredate").ToString

    '            'Dim lastYear = cc.Substring(0, 2)
    '            'Dim lastmonth = Convert.ToDateTime(cc).Month.ToString
    '            'Dim lastDate = String.Join("-", cc.Split("-").Skip(2).ToArray())
    '            Dim expiredate = String.Join("-", cc.Split("-").Skip(2).ToArray()) + "/" + Convert.ToDateTime(cc).Month.ToString + "/" + cc.Substring(0, 2)
    '            Dim da = Convert.ToDateTime(expiredate).Date
    '            If Convert.ToDateTime(expiredate) > DateTime.Now.Date Then
    '                Dim Enc_ArchivedDate = Encrypt("ArchivedDate", "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
    '                updateinconfig(Enc_ArchivedDate, Encrypt("Licensed_" + DateTime.Now.AddYears(5).ToShortDateString, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192))
    '                'Appcon = DirectCast(System.Configuration.ConfigurationManager.GetSection("appSettings"), NameValueCollection)
    '                'ArchivedDate = Appcon(Enc_ArchivedDate)
    '                Result = "Success"
    '                '  custommsgbox.showCustomMessageBox("Key Activated Alert", "License Activated Successfully.")
    '            Else
    '                Result = "Failed"
    '                ' custommsgbox.showCustomMessageBox("Key expired Alert", "key expired.")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw New FaultException(ex.ToString())
    '    End Try
    '    Return Result
    'End Function
    Public Function ActivateLicense(ByVal key As String) As String
        Dim Result As String = ""
        Try
            Dim ResLicense = decryptkey(key)
            If Not ResLicense Is Nothing AndAlso ResLicense.Tables.Count > 0 AndAlso ResLicense.Tables(0).Rows.Count > 0 Then
                Dim currentDate = DateTime.Now.ToShortDateString
                Dim Appcon As NameValueCollection = CType(ConfigurationSettings.GetConfig("appSettings"), NameValueCollection)
                Dim cc = ResLicense.Tables(0).Rows(0)("expiredate").ToString

                Dim expiredate = Convert.ToDateTime(String.Join("-", cc.Split("-").Skip(2).ToArray()) + "/" + Convert.ToDateTime(cc).Month.ToString + "/" + cc.Substring(0, 2)).Date

                If Convert.ToDateTime(expiredate) > DateTime.Now.Date AndAlso ResLicense.Tables(0).Rows(0)("Appname").ToString = "ezofis Standalone App" Then
                    Dim Enc_ArchivedDate = Encrypt("ArchivedDate", "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                    updateinconfig(Enc_ArchivedDate, Encrypt("Licensed_" + DateTime.Now.AddYears(5).ToShortDateString, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192))
                    'Appcon = DirectCast(System.Configuration.ConfigurationManager.GetSection("appSettings"), NameValueCollection)
                    'ArchivedDate = Appcon(Enc_ArchivedDate)
                    Result = "Success"
                    'custommsgbox.showCustomMessageBox("Key Activated Alert", "License Activated Successfully.")
                Else
                    Result = "Failed"
                    'custommsgbox.showCustomMessageBox("Key expired Alert", "key expired.")
                End If
            End If
        Catch ex As Exception
            Throw New FaultException(ex.ToString())
        End Try
        Return Result
    End Function
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
            Throw ex

        End Try
    End Function
#Region "Encrypt and Decrypt"
    Public Function Encrypt(plainText As String, passPhrase As String, saltValue As String, hashAlgorithm As String, passwordIterations As Integer, initVector As String,
 keySize As Integer) As String
        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)
        Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
        Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
        Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)
        Dim symmetricKey As New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Dim encryptor As ICryptoTransform = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)
        Dim memoryStream As New MemoryStream()
        Dim cryptoStream As New CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)
        cryptoStream.FlushFinalBlock()
        Dim cipherTextBytes As Byte() = memoryStream.ToArray()
        memoryStream.Close()
        cryptoStream.Close()
        Dim cipherText As String = Convert.ToBase64String(cipherTextBytes)
        Return cipherText
    End Function
    Public Function Decrypt(cipherText As String, passPhrase As String, saltValue As String, hashAlgorithm As String, passwordIterations As Integer, initVector As String,
     keySize As Integer) As String
        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)
        Dim cipherTextBytes As Byte() = Convert.FromBase64String(cipherText)
        Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
        Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)
        Dim symmetricKey As New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Dim decryptor As ICryptoTransform = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)
        Dim memoryStream As New MemoryStream(cipherTextBytes)
        Dim cryptoStream As New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)
        Dim plainTextBytes As Byte() = New Byte(cipherTextBytes.Length - 1) {}
        Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)
        memoryStream.Close()
        cryptoStream.Close()
        Dim plainText As String = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)
        Return plainText
    End Function
#End Region
End Class
