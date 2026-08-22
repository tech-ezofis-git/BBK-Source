Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports System.Security.Cryptography

Partial Public Class DBLayer
    Dim Dateformat As String = "dd-MMM-yyyy"
    Dim DateformatWithTime As String = "dd-MMM-yyyy hh:mm:ss tt"
    Public Function CreateLicenseClients(objEmp As IeZLicenseClients) As IeZLicenseClients
        Dim newObject As IeZLicenseClients = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter

            strQry = "INSERT INTO eZLicenseClients(isactive,LicenseId,ClientName,LicenseKey,MachineCode,MacInfo,InstallOn,TrialDays,ExpiredOn,CreatedOn,CreatedBy) VALUES(@isactive,@LicenseId,@ClientName,@LicenseKey,@MachineCode,@MacInfo,@InstallOn,@TrialDays,@ExpiredOn,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(10) {}
            param = New SqlParameter("@LicenseId", objEmp.LicenseId)
            objParam(0) = param
            param = New SqlParameter("@ClientName", objEmp.ClientName)
            objParam(1) = param
            param = New SqlParameter("@LicenseKey", objEmp.LicenseKey)
            objParam(2) = param
            param = New SqlParameter("@MachineCode", objEmp.MachineCode)
            objParam(3) = param
            param = New SqlParameter("@MacInfo", objEmp.MacInfo)
            objParam(4) = param
            param = New SqlParameter("@InstallOn", objEmp.InstallOn)
            objParam(5) = param
            param = New SqlParameter("@TrialDays", objEmp.TrialDays)
            objParam(6) = param
            param = New SqlParameter("@ExpiredOn", objEmp.ExpiredOn)
            objParam(7) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(8) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(9) = param
            param = New SqlParameter("@isactive", objEmp.IsActive)
            objParam(10) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZLicenseClients(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZLicenseClients)
        If objRead.IsReadFromDB Then
            Return
        End If
        If objRead.IsModified Then
            Throw New InvalidOperationException()
        End If
        Dim sqlRdr As SqlDataReader = Nothing
        objRead.IsReadFromDB = True
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            objParam = New SqlParameter(0) {}

            objParam = New SqlParameter(0) {}
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZLicenseClients Where LicenseClientId=@LicenseClientId and Isdeleted=0"
            param = New SqlParameter("@LicenseClientId", objRead.LicenseClientId)
            objParam(0) = param


            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid License.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.LicenseClientId = GetInteger(sqlRdr("LicenseClientId"))
                objRead.LicenseId = GetInteger(sqlRdr("LicenseId"))
                objRead.ApplicationName = sqlRdr("LicenseKey").ToString
                objRead.ClientName = sqlRdr("ClientName").ToString
                objRead.LicenseKey = sqlRdr("LicenseKey").ToString
                objRead.MachineCode = sqlRdr("MachineCode").ToString
                objRead.MacInfo = sqlRdr("MacInfo").ToString
                objRead.InstallOn = sqlRdr("InstallOn").ToString()
                objRead.TrialDays = GetInteger(sqlRdr("TrialDays"))
                If sqlRdr("IsActive") = True Then
                    objRead.Status = "Licensed"
                    objRead.IsActive = 1
                ElseIf sqlRdr("InstallOn").ToString <> "" Then
                    objRead.Status = "Trial Period"
                    objRead.IsActive = 0
                Else
                    objRead.Status = "Not Yet Install"
                End If
                objRead.ExpiredOn = sqlRdr("ExpiredOn").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If

            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAllLicenseClients() As System.Collections.Generic.List(Of IeZLicenseClients)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLicenseClients)()
        Dim objItem As IeZLicenseClients

        Try
            Dim strQry As String = ""
            strQry = "Select LicenseClientId From eZLicenseClients where Isdeleted=0 order by LicenseClientId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid License.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLicenseClients(GetInteger(sqlRdr("LicenseClientId")))
                objItem.LicenseClientId = GetInteger(sqlRdr("LicenseClientId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZLicenseClients(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLicenseClients)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLicenseClients)()
        Dim objItem As IeZLicenseClients
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LicenseClientId From eZLicenseClients where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by LicenseClientId"
            Else
                strQry = "Select LicenseClientId From eZLicenseClients where Isdeleted=0 order by LicenseClientId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid LicenseClient.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLicenseClients(GetSmallInterger(sqlRdr("LicenseClientId")))
                objItem.LicenseClientId = GetSmallInterger(sqlRdr("LicenseClientId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZLicenseClients)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        Dim ran As New Random
        Dim key As Long = ran.Next(0, 60466393)
        objToUpdate.MacInfo = AES_Encrypt(objToUpdate.MacInfo, "ezofis")
        objToUpdate.MachineCode = AES_Encrypt(objToUpdate.MachineCode.ToString + "-" + key.ToString, "ezofis")
        'strQry = "Select ScheduleId From eZSchedule Where ScheduleId <> @ScheduleId and Isdeleted=0"
        'objParam = New SqlParameter(0) {}
        'param = New SqlParameter("@ScheduleId", objToUpdate.ScheduleId)
        'objParam(0) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("Schedule Code already exist!")
        'Else
        strQry = "Update eZLicenseClients Set LicenseId=@LicenseId,ClientName=@ClientName,LicenseKey=@LicenseKey,MachineCode=@MachineCode,MacInfo=@MacInfo,InstallOn=@InstallOn,TrialDays=@TrialDays,ExpiredOn=@ExpiredOn,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where LicenseClientId=@LicenseClientId"
        objParam = New SqlParameter(10) {}
        param = New SqlParameter("@LicenseClientId", objToUpdate.LicenseClientId)
        objParam(0) = param
        param = New SqlParameter("@LicenseId", objToUpdate.LicenseId)
        objParam(1) = param
        param = New SqlParameter("@ClientName", objToUpdate.ClientName)
        objParam(2) = param
        param = New SqlParameter("@LicenseKey", objToUpdate.LicenseKey)
        objParam(3) = param
        param = New SqlParameter("@MachineCode", objToUpdate.MachineCode)
        objParam(4) = param
        param = New SqlParameter("@MacInfo", objToUpdate.MacInfo)
        objParam(5) = param
        param = New SqlParameter("@InstallOn", objToUpdate.InstallOn)
        objParam(6) = param
        param = New SqlParameter("@TrialDays", objToUpdate.TrialDays)
        objParam(7) = param
        param = New SqlParameter("@ExpiredOn", objToUpdate.ExpiredOn)
        objParam(8) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(9) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(10) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZLicenseClients)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLicenseClients set Isdeleted=1 where LicenseClientId=@LicenseClientId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LicenseClientId", objToDelete.LicenseClientId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    'Public Sub updatetrialdays1(ByVal LicenseClientId As String, ByVal TrialKey As String, ByVal LoginId As Integer)
    '    Try
    '        Dim dskey As New DataSet
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Public Sub UpdateTrialDays(ByVal LicenseClientId As String, ByVal TrialKey As String, ByVal LoginId As Integer)
        Dim strkey As String() = AES_Decrypt(TrialKey, "ezofis").ToString.Split("_")
        Dim apna As String() = strkey(1).ToString().Split(",")
        Dim triday As String() = strkey(2).ToString().Split(",")
        If apna.Count <> 0 Then
            For i As Integer = 0 To apna.Count - 1

                Dim strQry As String = ""
                Dim objParam As SqlParameter()
                Dim param As SqlParameter
                Dim Obj As New List(Of IeZLicenseClients)
                Obj = ReadSelectedeZLicenseClients("LicenseClientId", LicenseClientId.ToString)
                ' Dim InstallDate As DateTime = Convert.ToDateTime(Obj(0).InstallOn.ToString)
                Dim expiredDate As DateTime = Convert.ToDateTime(Obj(0).ExpiredOn.ToString)
                Dim StrTrialKey As String() = AES_Decrypt(TrialKey, "ezofis").ToString.Split("_")
                ' Dim ExpiredOn As DateTime = InstallDate.AddDays(CInt(StrTrialKey(2)) + Obj(0).TrialDays)
                Dim ExpiredOn As DateTime = expiredDate.AddDays(CInt(StrTrialKey(2)))

                Dim objEmp As eZTrialLicense = Nothing
                Dim ObjeZTrialLicense As New eZTrialLicense
                ObjeZTrialLicense.LicenseId = Obj(0).LicenseId

                Dim ObjeZLicense As List(Of IeZLicense) = ReadSelectedeZLicense("LicenseId", Obj(0).LicenseId.ToString)

                '  If ObjeZLicense(0).ApplicationName = StrTrialKey(1) Then

                ObjeZTrialLicense.LicenseClientId = Obj(0).LicenseClientId
                ObjeZTrialLicense.TrialKey = TrialKey
                ObjeZTrialLicense.CreatedOn = DateDateTimeToString(Today.Date, False)
                ObjeZTrialLicense.CreatedBy = LoginId
                objEmp = DBLayer.DBLInstance.CreateTrialLicense(ObjeZTrialLicense)
                If objEmp IsNot Nothing Then
                    'strQry = "Update eZLicenseClients set UpdatedBy='" + LoginId.ToString() + "',TrialDays='" + (CInt(StrTrialKey(2)) + Obj(0).TrialDays).ToString() + "',ExpiredOn'" + DateDateTimeToString(ExpiredOn, False) + "' where LicenseClientId in ('" + LicenseClientId.ToString() + "')"
                    'DBLayer.DBLInstance.InsertAndUpdate(strQry.ToString())
                    strQry = "Update eZLicenseClients set UpdatedBy=@UpdatedBy,TrialDays=@TrialDays,ExpiredOn=@ExpiredOn where LicenseClientId=@LicenseClientId"
                    objParam = New SqlParameter(4) {}
                    param = New SqlParameter("@LicenseClientId", LicenseClientId)
                    objParam(0) = param
                    param = New SqlParameter("@TrialDays", (CInt(StrTrialKey(2)) + Obj(0).TrialDays))
                    objParam(1) = param
                    param = New SqlParameter("@UpdatedBy", LoginId)
                    objParam(2) = param
                    param = New SqlParameter("@UpdatedOn", DateDateTimeToString(Today.Date, False))
                    objParam(3) = param
                    param = New SqlParameter("@ExpiredOn", DateDateTimeToString(ExpiredOn, False))
                    objParam(4) = param
                    If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                        Throw New Exception("Record Not deleted due to some error")
                    End If
                Else
                    Throw New Exception("Trial Key Not Updated")
                End If
            Next
        End If
        'Else
        'Throw New Exception("Trial Key Not Updated")
        'End If
    End Sub

    Public Function UpdateLicenseKey(ByVal LicenseClientId As Integer, ByVal LicenseKey As String, ByVal LoginId As Integer)

        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        Dim Obj As New List(Of IeZLicenseClients)
        Dim LicenseKeyValidate As Integer = LicenseKeyValidationByLicenseClientId(LicenseClientId, LicenseKey)
        If LicenseKeyValidate = 1 Then
            LicenseKey = AES_Encrypt(LicenseKey, "ezofis")
            strQry = "Update eZLicenseClients set UpdatedBy=@UpdatedBy,LicenseKey=@LicenseKey,TrialDays=0,IsActive=1 where LicenseClientId=@LicenseClientId"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@LicenseClientId", LicenseClientId)
            objParam(0) = param
            param = New SqlParameter("@LicenseKey", LicenseKey)
            objParam(1) = param
            param = New SqlParameter("@UpdatedBy", LoginId)
            objParam(2) = param
            param = New SqlParameter("@UpdatedOn", DateDateTimeToString(Today.Date, False))
            objParam(3) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not deleted due to some error")
            Else
                Return 1
            End If
        Else
            Return 0
        End If

    End Function

    Public Function DateDateTimeToString(ByVal dt As DateTime, ByVal WithTime As Boolean) As String
        Try
            Dim dateValue As String
            If WithTime Then
                dateValue = dt.ToString(DateformatWithTime)
            Else
                dateValue = dt.ToString(Dateformat)
            End If

            Return dateValue
        Catch ex As Exception
            Return dt
        End Try
    End Function
    Function HexToString(ByVal hex As String) As String
        Dim text As New System.Text.StringBuilder(hex.Length \ 2)
        For i As Integer = 0 To hex.Length - 2 Step 2
            text.Append(hex.Substring(i, 2).ToString() + "-")
        Next
        Return text.ToString.Substring(0, text.ToString().LastIndexOf("-"))
    End Function

    Public Function AES_Encrypt(ByVal input As String, ByVal pass As String) As String
        Dim AES As New System.Security.Cryptography.RijndaelManaged
        Dim Hash_AES As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim encrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = System.Security.Cryptography.CipherMode.ECB
            Dim DESEncrypter As System.Security.Cryptography.ICryptoTransform = AES.CreateEncryptor
            Dim Buffer As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(input)
            Dim a = DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length)
            encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            'Dim bytes As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(encrypted)
            'encrypted = BitConverter.ToString(bytes).Replace("-", "")
            'encrypted = encrypted.Replace("0", "G").Replace("1", "H").Replace("2", "I").Replace("3", "J").Replace("4", "K").Replace("5", "L").Replace("6", "M").Replace("7", "N").Replace("8", "O").Replace("9", "P")
            Return encrypted
        Catch ex As Exception
        End Try
    End Function

    Public Function AES_Decrypt(ByVal input As String, ByVal pass As String) As String
        Dim AES As New System.Security.Cryptography.RijndaelManaged
        Dim Hash_AES As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim decrypted As String = ""
        Try
            'input = input.Replace("G", "0").Replace("H", "1").Replace("I", "2").Replace("J", "3").Replace("K", "4").Replace("L", "5").Replace("M", "6").Replace("N", "7").Replace("O", "8").Replace("P", "9")
            'input = HexToString(input).ToString()
            'Dim bytes As Byte() = input.Split("-"c).Select(Function(n) Convert.ToByte(Convert.ToInt32(n, 16))).ToArray()
            'input = System.Text.ASCIIEncoding.ASCII.GetString(bytes)
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = Security.Cryptography.CipherMode.ECB
            Dim DESDecrypter As System.Security.Cryptography.ICryptoTransform = AES.CreateDecryptor
            Dim Buffer As Byte() = Convert.FromBase64String(input)
            decrypted = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return decrypted
        Catch ex As Exception
        End Try
    End Function

    '*******************************Same memorystream Encryption decryption*******************************************
    Public Function Encrypt1(clearText As String) As String
        Dim aes As New RijndaelManaged
        aes.KeySize = 256

        Using ms As New MemoryStream
            ms.WriteByte(aes.Key.Length)
            ms.Write(aes.Key, 0, aes.Key.Length)
            ms.WriteByte(aes.IV.Length)
            ms.Write(aes.IV, 0, aes.IV.Length)

            Using cs As New CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write)
                Dim bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(clearText)
                cs.Write(bytes, 0, bytes.Length)
            End Using

            Return Convert.ToBase64String(ms.ToArray())
        End Using
    End Function


    Private Function Decrypt1(cipherText As String) As String
        Dim ms As New MemoryStream(Convert.FromBase64String(cipherText))

        Dim keyLength As Byte = ms.ReadByte()
        Dim key(keyLength - 1) As Byte
        ms.Read(key, 0, keyLength)

        Dim ivLength As Byte = ms.ReadByte()
        Dim iv(ivLength - 1) As Byte
        ms.Read(iv, 0, ivLength)

        Dim dataOffset As Integer = ms.Position

        Dim aes As New RijndaelManaged()
        aes.Key = key
        aes.IV = iv

        Using cs As New CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read)
            Using sr As New StreamReader(cs, System.Text.Encoding.UTF8)
                Return sr.ReadToEnd()
            End Using
        End Using
    End Function
    '*******************************Same memorystream Encryption decryption*******************************************

    Public Function LicenseKeyValidation(ByVal MotherBoardId As String, ByVal ApplicationName As String) As Integer

        Dim MacInfo As String = AES_Encrypt(MotherBoardId, "ezofis")

        Dim eZApplication As List(Of IeZApplication) = ReadSelectedeZApplication("ApplicationName", ApplicationName)

        Dim eZLicense As List(Of IeZLicense) = ReadSelectedeZLicense("ApplicationId", eZApplication(0).ApplicationId)

        Dim eZLicenseClients As List(Of IeZLicenseClients) = ReadSelectedeZLicenseClientsWithCondition("MacInfo='" & MacInfo & "' And LicenseId=" & eZLicense(0).LicenseId)

        Dim StrMachineCode As String = AES_Decrypt(eZLicenseClients(0).MachineCode, "ezofis")

        Dim StrRandomNumber As String() = StrMachineCode.Split("_")

        Dim ActualMachineCode As String = GenerateSerial(StrRandomNumber(1))

        Dim DecryptedMachineCode = AES_Decrypt(eZLicenseClients(0).LicenseKey, "ezofis")

        If ActualMachineCode = DecryptedMachineCode Then
            Return 1
        Else
            Return 0
        End If

    End Function

    Public Function LicenseKeyValidationByLicenseClientId(ByVal LicenseClientId As Integer, ByVal LicenseKey As String) As Integer

        Dim eZLicenseClients As List(Of IeZLicenseClients) = ReadSelectedeZLicenseClients("LicenseClientId", LicenseClientId)

        Dim StrMachineCode As String = AES_Decrypt(eZLicenseClients(0).MachineCode, "ezofis")

        Dim StrRandomNumber As String() = StrMachineCode.Split("_")

        Dim ActualMachineCode As String = GenerateSerial(StrRandomNumber(1))

        Dim DecryptedMachineCode = ""

        If eZLicenseClients(0).LicenseKey <> "" Then
            DecryptedMachineCode = AES_Decrypt(eZLicenseClients(0).LicenseKey, "ezofis")
        ElseIf ActualMachineCode = LicenseKey Then
            Return 1
        Else
            Return 0
        End If

        If ActualMachineCode = DecryptedMachineCode Then
            Return 1
        Else
            Return 0
        End If

    End Function

    Private Function GenerateSerial(ByVal RandomNumber As String)
        ' Make the variable that holds the serial
        Dim serial As String

        '
        ' Make the random key (within base 36)
        '

        'Dim key As Long = ran.Next(0, 60466)

        Dim key As Long = CInt(RandomNumber)

        '
        ' Create the list that will contain the 'Char Arrays'
        '
        Dim lst As New List(Of List(Of String))

        ' ################################################################
        ' #  IMPORTANT!!!!                                               #
        ' #                                                              #
        ' #  Initialize the arrays used in generating a key.             #
        ' #  THIS IS YOUR MAIN WEAPON AGAINST HACKERS!!!                 #
        ' #      MAKE IT DIFFERENT!!!!!                                  #
        ' #                                                              #
        ' ################################################################

        Dim arr1() As String = {"A", "A", "B", "C", "C", "D", "E", "E", "F", "G", "G", "H", "I", "I", "J", "K", "K", "L", "M", "M", "N", "O", "O", "P", "Q", "Q", "R", "S", "S", "T", "U", "U", "V", "W", "W", "X", "Y", "Y", "Z", "0", "0", "1", "2", "2", "3", "4", "4", "5", "6", "6", "7", "8", "8", "9"}
        Dim arr2() As String = {"B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
        Dim arr3() As String = {"A", "E", "I", "O", "U", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
        Dim arr4() As String = {"0", "2", "4", "6", "8", "A", "C", "E", "G", "I", "K", "M", "O", "Q", "S", "U", "W", "Y"}
        Dim arr5() As String = {"0", "1", "2", "6", "7", "8", "A", "B", "C", "G", "H", "I", "M", "N", "O", "S", "T", "U", "Y", "Z"}
        Dim arr6() As String = {"L", "X", "M", "N", "T", "O", "P", "U", "V", "Q", "W", "A", "K", "0", "2", "4", "5", "6", "8", "9"}
        Dim arr7() As String = {"N", "T", "O", "S", "P", "R", "I", "E", "L", "X", "Q", "Z", "C", "B", "H", "8", "7", "2", "1", "6", "0", "3"}
        Dim arr8() As String = {"L", "S", "D", "M", "N", "O", "Q", "R", "S", "N", "V", "Q", "Y", "Q", "K", "X", "C", "A", "4", "5", "9", "2"}
        Dim arr9() As String = {"9", "7", "6", "4", "3", "1", "Q", "E", "T", "U", "O", "A", "S", "F", "H", "K", "Z", "C", "B", "M"}
        Dim arr10() As String = {"9", "8", "7", "3", "2", "1", "5", "5", "5", "W", "R", "Y", "I", "P", "S", "F", "H", "K", "Z", "C", "B", "M"}
        Dim arr11() As String = {"Q", "R", "I", "A", "F", "J", "Z", "V", "M", "E", "U", "P", "D", "H", "L", "C", "N", "0", "2", "3", "5", "7", "8"}
        Dim arr12() As String = {"Q", "A", "Z", "E", "D", "C", "T", "G", "B", "U", "J", "M", "O", "L", "7", "4", "5", "6", "3", "2"}
        Dim arr13() As String = {"Q", "A", "S", "E", "R", "F", "G", "H", "Y", "U", "J", "K", "I", "O", "L", "Z", "X", "S", "D", "F", "V", "B", "G", "H", "J", "M", "K", "9", "5", "1", "7", "5", "3"}
        Dim arr14() As String = {"Q", "W", "E", "T", "Y", "U", "O", "P", "A", "D", "F", "G", "J", "K", "L", "X", "C", "V", "N", "M", "1", "4", "6", "9"}
        Dim arr15() As String = {"Q", "A", "Z", "X", "C", "D", "E", "R", "T", "G", "B", "N", "M", "J", "U", "I", "O", "L", "1", "5", "4", "8", "6", "2", "3"}
        Dim arr16() As String = {"W", "E", "D", "C", "V", "B", "G", "T", "Y", "U", "J", "M", "K", "L", "O", "H", "F", "S", "1", "5", "4", "2", "3"}
        Dim arr17() As String = {"O", "I", "U", "T", "G", "B", "C", "X", "Z", "Q", "W", "E", "R", "F", "V", "B", "N", "M"}
        Dim arr18() As String = {"Q", "A", "Z", "X", "S", "D", "C", "V", "F", "R", "T", "G", "B", "N", "H", "J", "M", "K", "I", "O", "L", "P", "1", "3", "2", "5", "6", "7", "9"}
        Dim arr19() As String = {"L", "I", "A", "M", "I", "S", "C", "O", "O", "L", "I", "S", "N", "T", "H", "E", "3", "3", "3"}
        '   VVVVVVV
        Dim arr20() As String = {"L", "S", "D", "I", "S", "C", "O", "O", "L", "S", "O", "I", "S", "C", "O", "C", "A", "I", "N", "E", "A", "N", "D", "P", "O", "T", "A", "N", "D", "M", "E", "T", "H"}
        '   ^^^^^^^ This could explain a few things =]

        '
        ' Add arrays as lists
        '
        lst.Add(New List(Of String)(arr1))
        lst.Add(New List(Of String)(arr2))
        lst.Add(New List(Of String)(arr3))
        lst.Add(New List(Of String)(arr4))
        lst.Add(New List(Of String)(arr5))
        lst.Add(New List(Of String)(arr6))
        lst.Add(New List(Of String)(arr7))
        lst.Add(New List(Of String)(arr8))
        lst.Add(New List(Of String)(arr9))
        lst.Add(New List(Of String)(arr10))
        lst.Add(New List(Of String)(arr11))
        lst.Add(New List(Of String)(arr12))
        lst.Add(New List(Of String)(arr13))
        lst.Add(New List(Of String)(arr14))
        lst.Add(New List(Of String)(arr15))
        lst.Add(New List(Of String)(arr16))
        lst.Add(New List(Of String)(arr17))
        lst.Add(New List(Of String)(arr18))
        lst.Add(New List(Of String)(arr19))
        lst.Add(New List(Of String)(arr20))

        ' Convert the key to Base36 and prepend to the serial code

        serial &= ToBase36(key)
        ' Append extra 0's if the key isn't already five characters long
        Do Until serial.Length = 5
            serial = "0" + serial
        Loop

        '
        ' Initialize the random using Random(key)
        '
        Dim r1 As New Random(key)

        '
        ' Generate the key using the unique 'array' for each character.
        '

        Dim x As Integer
        Do Until serial.Length = 29
            x = serial.Length
            ' Use modulus to see if this is the time for a hyphen ("-")
            If x Mod 6 = 5 Then
                serial &= "-"
            Else
                serial &= lst.Item(x - (5 + (x + 1) \ 6)).Item(r1.Next(0, lst.Item(x - (5 + (x + 1) \ 6)).Count - 1))
            End If
        Loop

        ' Return the serial key
        Return serial
    End Function

    Public Function ToBase36(ByVal IBase36 As Double) As String
        Dim Base36() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}
        Dim v As String
        Dim i As Decimal
        Do Until IBase36 < 1
            i = IBase36 Mod 36
            v = Base36(i) & v
            IBase36 = Math.DivRem(Long.Parse(IBase36), 36, Nothing)
        Loop
        Return v

    End Function

    Public Function CheckNoOfClients(ByVal ApplicationName As String) As Integer
        Dim eZApplication As List(Of IeZApplication) = ReadSelectedeZApplication("ApplicationName", ApplicationName)

        If eZApplication.Count > 0 Then
            Dim eZLicense As List(Of IeZLicense) = ReadSelectedeZLicense("ApplicationId", eZApplication(0).ApplicationId)
            If eZLicense.Count > 0 Then
                Dim eZLicensClients As List(Of IeZLicenseClients) = ReadSelectedeZLicenseClients("LicenseId", eZLicense(0).LicenseId)
                If eZLicensClients.Count = eZLicense(0).NoOfLicense Then
                    Return 0
                Else
                    Return eZLicense(0).LicenseId
                End If
            End If
        End If
    End Function

    Public Function GetMachineCode(ByVal MotherboardId As String, ByVal ApplicationName As String) As String

        Dim MacInfo As String = AES_Encrypt(MotherboardId, "ezofis")

        Dim eZApplication As List(Of IeZApplication) = ReadSelectedeZApplication("ApplicationName", ApplicationName)

        If eZApplication.Count > 0 Then
            Dim eZLicense As List(Of IeZLicense) = ReadSelectedeZLicense("ApplicationId", eZApplication(0).ApplicationId)
            If eZLicense.Count > 0 Then
                Dim eZLicenseClients As List(Of IeZLicenseClients) = ReadSelectedeZLicenseClientsWithCondition("MacInfo='" & MacInfo & "' And LicenseId=" & eZLicense(0).LicenseId)
                If eZLicenseClients.Count > 0 Then
                    Dim StrMachineCode As String = AES_Decrypt(eZLicenseClients(0).MachineCode, "ezofis")
                    Return StrMachineCode
                End If
            End If
        End If

    End Function

    Public Function GetStatusOfClientMachine(ByVal MotherboardId As String, ByVal ApplicationName As String) As Integer

        Dim MacInfo As String = AES_Encrypt(MotherboardId, "ezofis")
        Dim encryptapplicationname As String = AES_Encrypt(ApplicationName, "ezofis")
        Dim eZApplication As List(Of IeZApplication) = ReadSelectedeZApplication("ApplicationName", ApplicationName)

        If eZApplication.Count > 0 Then
            Dim eZLicense As List(Of IeZLicense) = ReadSelectedeZLicense("ApplicationId", eZApplication(0).ApplicationId)
            If eZLicense.Count > 0 Then
                Dim eZLicenseClients As List(Of IeZLicenseClients) = ReadSelectedeZLicenseClientsWithCondition("MacInfo='" & MacInfo & "' And LicenseId=" & eZLicense(0).LicenseId)
                If eZLicenseClients.Count > 0 Then
                    If eZLicenseClients(0).TrialDays = 0 And eZLicenseClients(0).IsActive = 1 Then
                        Return 1
                    ElseIf eZLicenseClients(0).InstallOn = "" Then
                        Return 0
                    ElseIf CDate(DateDateTimeToString(eZLicenseClients(0).ExpiredOn, False)) < CDate(DateDateTimeToString(Today.Date, False)) Then
                        Return 2
                    ElseIf CDate(DateDateTimeToString(eZLicenseClients(0).ExpiredOn, False)) = CDate(DateDateTimeToString(Today.Date, False)) Then
                        Return 4
                    ElseIf eZLicenseClients(0).IsActive = 0 Then
                        Return 3
                    End If
                End If
            End If
        End If
    End Function

    Public Function ReadSelectedeZLicenseClientsWithCondition(ByVal Condition As String) As System.Collections.Generic.List(Of IeZLicenseClients)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLicenseClients)()
        Dim objItem As IeZLicenseClients
        Try
            Dim strQry As String = ""
            If Condition <> "All" Then
                strQry = "Select LicenseClientId From eZLicenseClients where Isdeleted=0 and " & Condition
                strQry = strQry & " order by LicenseClientId"
            Else
                strQry = "Select LicenseClientId From eZLicenseClients where Isdeleted=0 order by LicenseClientId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid LicenseClient.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLicenseClients(GetSmallInterger(sqlRdr("LicenseClientId")))
                objItem.LicenseClientId = GetSmallInterger(sqlRdr("LicenseClientId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

End Class