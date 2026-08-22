Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "eZECMLogin Details"
    Public Function CreateeZECMLogin(objEmp As OldeZECMLogin) As OldeZECMLogin
        Dim newObject As OldeZECMLogin = Nothing

        If String.IsNullOrEmpty(objEmp.LoginName) Then
            Return Nothing
        End If
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ECMLoginId From eZECMLogin Where LoginName = @LoginName And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@LoginName", objEmp.LoginName)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("Login Name already exist!")
            End If
            objEmp.Pasword = Encrypt(objEmp.Pasword, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            strQry = "INSERT INTO eZECMLogin(LoginName,Pasword,ECMProfileId,ECMUserTypeId,Signatureid,IsADUser,IsFaxUser,CreatedBy,CreatedOn) VALUES(@LoginName,@Pasword,@ECMProfileId,@ECMUserTypeId,@Signatureid,@IsADUser,@IsFaxUser,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            'param = New SqlParameter("@ECMGroupId", objEmp.ECMGroupId)
            'objParam(0) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@ECMProfileId", objEmp.ECMProfileId)
            objParam(3) = param
            param = New SqlParameter("@LoginName", objEmp.LoginName)
            objParam(4) = param
            param = New SqlParameter("@Pasword", objEmp.Pasword)
            objParam(5) = param
            param = New SqlParameter("@IsADUser", objEmp.IsADUser)
            objParam(6) = param
            param = New SqlParameter("@ECMUserTypeId", objEmp.ECMUserTypeId)
            objParam(7) = param
            param = New SqlParameter("@IsFaxUser", objEmp.IsFaxUser)
            objParam(8) = param
            param = New SqlParameter("@Signatureid", objEmp.Signatureid)
            objParam(0) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZECMLogin(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IOldeZECMLogin)
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
            If objRead.LoginName IsNot Nothing Then
                objParam = New SqlParameter(1) {}
                objRead.Pasword = Encrypt(objRead.Pasword, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                strQry = "Select eZECMLogin.*,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1,dbo.udf_ECMProfile(ECMProfileId) as ECMProfile From eZECMLogin Where LoginName=@LoginName and Pasword=@Pasword  and Isdeleted=0"
                param = New SqlParameter("@LoginName", objRead.LoginName)
                objParam(0) = param
                param = New SqlParameter("@Pasword", objRead.Pasword)
                objParam(1) = param

            Else
                objParam = New SqlParameter(0) {}
                strQry = "Select eZECMLogin.*,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1,dbo.udf_ECMProfile(ECMProfileId) as ECMProfile From eZECMLogin Where ECMLoginId=@ECMLoginId and Isdeleted=0"
                param = New SqlParameter("@ECMLoginId", objRead.ECMLoginId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Login ID.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))

                objRead.ECMUserTypeId = GetInteger(sqlRdr("ECMUserTypeId"))
                objRead.LanguageId = GetInteger(sqlRdr("LanguageId"))
                ' objRead.Signatureid = sqlRdr("Signatureid").ToString()
                objRead.Chart1 = GetInteger(sqlRdr("Chart1"))
                objRead.Chart2 = GetInteger(sqlRdr("Chart2"))
                objRead.Chart3 = GetInteger(sqlRdr("Chart3"))
                'objRead.ECMGroup = sqlRdr("ECMGroup").ToString()
                If sqlRdr("IsADUser").ToString = "True" Then
                    objRead.IsADUser = True
                Else
                    objRead.IsADUser = False
                End If
                If sqlRdr("IsFaxUser").ToString = "True" Then
                    objRead.IsFaxUser = True
                Else
                    objRead.IsFaxUser = False
                End If
                objRead.LoginName = sqlRdr("LoginName").ToString()
                objRead.Pasword = Decrypt(sqlRdr("Pasword").ToString(), "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                'objRead.Pasword = sqlRdr("Pasword").ToString()
                objRead.ECMProfileId = GetInteger(sqlRdr("ECMProfileId"))
                objRead.ECMProfile = sqlRdr("ECMProfile").ToString()
                objRead.Signatureid = sqlRdr("Signatureid").ToString()
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
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
    Public Function ReadAlleZECMLogin() As System.Collections.Generic.List(Of IOldeZECMLogin)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IOldeZECMLogin)()
        Dim objItem As IOldeZECMLogin
        Try
            Dim strQry As String = ""
            strQry = "Select * From eZECMLogin where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Login Name.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMLogin(GetInteger(sqlRdr("ECMLoginId")))
                objItem.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadAlleZECMLoginById(ECMLoginId As Integer) As System.Collections.Generic.List(Of IOldeZECMLogin)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IOldeZECMLogin)()
        Dim objItem As IOldeZECMLogin
        Try
            Dim strQry As String = ""
            strQry = "Select * From eZECMLogin where IsDeleted=0 and ECMLoginId=" & ECMLoginId & ""
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Login Name.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMLogin(GetInteger(sqlRdr("ECMLoginId")))
                objItem.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadFilteredeZECMLogin(Criteria As String, Value As String) As System.Collections.Generic.List(Of IOldeZECMLogin)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IOldeZECMLogin)()
        Dim objItem As IOldeZECMLogin

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMLoginId From eZECMLogin where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMProfileId,LoginName"
            Else
                strQry = "Select ECMLoginId From eZECMLogin where Isdeleted=0 order by ECMProfileId,LoginName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMLogin(GetInteger(sqlRdr("ECMLoginId")))
                objItem.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMLogin(Criteria As String, Value As String) As System.Collections.Generic.List(Of IOldeZECMLogin)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IOldeZECMLogin)()
        Dim objItem As IOldeZECMLogin

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMLoginId From eZECMLogin where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMProfileId,LoginName"
            Else
                strQry = "Select ECMLoginId From eZECMLogin where Isdeleted=0 order by ECMProfileId,LoginName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMLogin(GetInteger(sqlRdr("ECMLoginId")))
                objItem.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMLoginByGroup(Criteria As String, Value As String) As System.Collections.Generic.List(Of IOldeZECMLogin)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IOldeZECMLogin)()
        Dim objItem As IOldeZECMLogin

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMLoginId From eZECMLogin where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMProfileId,LoginName"
            Else
                strQry = "Select ECMLoginId From eZECMLogin where Isdeleted=0 order by ECMProfileId,LoginName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMLogin(GetInteger(sqlRdr("ECMLoginId")))
                objItem.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function UpdateeZECMLoginPassword(ECMLoginId As Integer, Password As String) As String
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        Password = Encrypt(Password, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j",
         192)
        Dim currentdate = DateDateTimeToString(Date.Now, False)
        strQry = "Update eZECMLogin Set Pasword = @Pasword,LastPasswordUpdate=@LastPasswordUpdate,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ECMLoginId=@ECMLoginId"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@ECMLoginId", ECMLoginId)
        objParam(0) = param
        param = New SqlParameter("@Pasword", Password)
        objParam(1) = param
        param = New SqlParameter("@LastPasswordUpdate", currentdate)
        objParam(2) = param
        param = New SqlParameter("@UpdatedOn", DateDateTimeToString(Date.Now, True))
        objParam(3) = param
        param = New SqlParameter("@UpdatedBy", ECMLoginId)
        objParam(4) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Return "Record Not updated due to some error"
        Else
            Return "Success"
        End If
    End Function
    Public Function UpdateeZLanguage(ECMLoginId As Integer, LanguageId As Integer) As String
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMLogin Set LanguageId = @LanguageId where ECMLoginId=@ECMLoginId"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ECMLoginId", ECMLoginId)
        objParam(0) = param
        param = New SqlParameter("@LanguageId", LanguageId)
        objParam(1) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Return "Record Not updated due to some error"
        Else
            Return "User Language Updated"
        End If
    End Function
    Public Sub Update(objToUpdate As IOldeZECMLogin)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ECMLoginId From eZECMLogin Where LoginName = @LoginName and ECMLoginId <> @ECMLoginId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@LoginName", objToUpdate.LoginName)
        objParam(0) = param
        param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZECMLogin already have acount!")
        Else
            strQry = "Update eZECMLogin Set Pasword=@Pasword, Signatureid=@Signatureid,IsADUser=@IsADUser,IsFaxUser=@IsFaxUser,Chart1=@Chart1," +
                "Chart2=@Chart2,Chart3=@Chart3,ECMUserTypeId=@ECMUserTypeId,ECMProfileId=@ECMProfileId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy " +
                "where ECMLoginId=@ECMLoginId"
            objParam = New SqlParameter(11) {}
            'param = New SqlParameter("@ECMGroupId", objToUpdate.ECMGroupId)
            'objParam(0) = param
            param = New SqlParameter("@ECMProfileId", objToUpdate.ECMProfileId)
            objParam(1) = param
            param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
            objParam(2) = param
            param = New SqlParameter("@IsADUser", objToUpdate.IsADUser)
            objParam(3) = param
            param = New SqlParameter("@ECMUserTypeId", objToUpdate.ECMUserTypeId)
            objParam(4) = param
            param = New SqlParameter("@Chart1", objToUpdate.Chart1)
            objParam(5) = param
            param = New SqlParameter("@Chart2", objToUpdate.Chart2)
            objParam(6) = param
            param = New SqlParameter("@Chart3", objToUpdate.Chart3)
            objParam(7) = param
            param = New SqlParameter("@IsFaxUser", objToUpdate.IsFaxUser)
            objParam(8) = param
            objToUpdate.Pasword = Encrypt(objToUpdate.Pasword, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            param = New SqlParameter("@Pasword", objToUpdate.Pasword)
            objParam(9) = param
            param = New SqlParameter("@Signatureid", objToUpdate.Signatureid)
            objParam(0) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(10) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(11) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IOldeZECMLogin)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMLogin set Isdeleted=1 where ECMLoginId=@ECMLoginId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ECMLoginId", objToDelete.ECMLoginId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region

End Class
