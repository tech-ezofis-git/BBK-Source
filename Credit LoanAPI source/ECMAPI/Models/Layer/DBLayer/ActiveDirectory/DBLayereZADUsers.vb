Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZADUsers)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZADUsers" +
                " Where LdapUserId=@LdapUserId and Isdeleted=0"
            param = New SqlParameter("@LdapUserId", objRead.LdapUserId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Ldap User")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.LdapConnId = GetInteger(sqlRdr("LdapConnId"))
                objRead.LdapUserId = GetInteger(sqlRdr("LdapUserId"))
                objRead.Firstname = sqlRdr("Firstname").ToString
                objRead.Lastname = sqlRdr("Lastname").ToString
                objRead.Username = sqlRdr("Username").ToString
                objRead.Displayname = sqlRdr("Displayname").ToString
                objRead.Department = sqlRdr("Department").ToString
                objRead.Mail = sqlRdr("Mail").ToString
                objRead.Mobile = sqlRdr("Mobile").ToString
                objRead.Jobtitle = sqlRdr("Jobtitle").ToString
                objRead.Description = sqlRdr("Description").ToString
                objRead.State = sqlRdr("State").ToString
                objRead.City = sqlRdr("City").ToString
                objRead.Office = sqlRdr("Office").ToString
                objRead.TelephoneNumber = sqlRdr("TelephoneNumber").ToString
                objRead.Company = sqlRdr("Company").ToString
                objRead.HomePhone = sqlRdr("HomePhone").ToString
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.IsECMUser = Convert.ToInt32(Convert.ToBoolean(sqlRdr("IsECMUser")))
                objRead.sAMAccountName = sqlRdr("sAMAccountName").ToString
                objRead.Manager = sqlRdr("Manager").ToString
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

    Public Function CreateeZADUsers(objEmp As eZADUsers) As eZADUsers
        Dim newObject As eZADUsers = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZADUsers(LdapConnId,Firstname,Username,CreatedBy,CreatedOn,Lastname,Displayname,Department,Mail," +
                "Mobile,Jobtitle,Description,State,City,Office,TelephoneNumber,Company,HomePhone,IsECMUser,sAMAccountName,Manager) VALUES" +
                "(@LdapConnId,@Firstname,@Username,@CreatedBy,@CreatedOn,@Lastname,@Displayname,@Department,@Mail,@Mobile,@Jobtitle," +
                "@Description,@State,@City,@Office,@TelephoneNumber,@Company,@HomePhone,@IsECMUser,@sAMAccountName,@Manager);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(20) {}
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@LdapConnId", objEmp.LdapConnId)
            objParam(2) = param
            param = New SqlParameter("@Firstname", objEmp.Firstname)
            objParam(3) = param
            param = New SqlParameter("@Username", objEmp.Username)
            objParam(4) = param
            param = New SqlParameter("@Lastname", objEmp.Lastname)
            objParam(5) = param
            param = New SqlParameter("@Displayname", objEmp.Displayname)
            objParam(6) = param
            param = New SqlParameter("@Department", objEmp.Department)
            objParam(7) = param
            param = New SqlParameter("@Mail", objEmp.Mail)
            objParam(8) = param
            param = New SqlParameter("@Mobile", objEmp.Mobile)
            objParam(9) = param
            param = New SqlParameter("@Jobtitle", objEmp.Jobtitle)
            objParam(10) = param
            param = New SqlParameter("@Description", objEmp.Description)
            objParam(11) = param
            param = New SqlParameter("@State", objEmp.State)
            objParam(12) = param
            param = New SqlParameter("@City", objEmp.City)
            objParam(13) = param
            param = New SqlParameter("@Office", objEmp.Office)
            objParam(14) = param
            param = New SqlParameter("@TelephoneNumber", objEmp.TelephoneNumber)
            objParam(15) = param
            param = New SqlParameter("@Company", objEmp.Company)
            objParam(16) = param
            param = New SqlParameter("@HomePhone", objEmp.HomePhone)
            objParam(17) = param
            param = New SqlParameter("@IsECMUser", objEmp.IsECMUser)
            objParam(18) = param
            param = New SqlParameter("@sAMAccountName", objEmp.sAMAccountName)
            objParam(19) = param
            param = New SqlParameter("@Manager", objEmp.Manager)
            objParam(20) = param
            Dim obj As Object = Nothing
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception(SqlHelper.errstr)
                'Return Nothing
            End If
            newObject = GlobalInstance.eZADUsers(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.ToString)
            Return Nothing
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZADUsers)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZADUsers Set LdapConnId=@LdapConnId,Firstname=@Firstname,Username=@Username,Department=@Department,Mail=@Mail," +
            "Mobile=@Mobile,Jobtitle=@Jobtitle,Description=@Description,State=@State,City=@City,Office=@Office" +
            ",TelephoneNumber=@TelephoneNumber,Company=@Company,HomePhone=@HomePhone,IsECMUser=@IsECMUser," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,Lastname=@Lastname,Displayname=@Displayname,sAMAccountName=@sAMAccountName,Manager=@Manager where LdapUserId=@LdapUserId"
        objParam = New SqlParameter(21) {}
        param = New SqlParameter("@LdapUserId", objToUpdate.LdapUserId)
        objParam(18) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(0) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(1) = param
        param = New SqlParameter("@LdapConnId", objToUpdate.LdapConnId)
        objParam(2) = param
        param = New SqlParameter("@Firstname", objToUpdate.Firstname)
        objParam(3) = param
        param = New SqlParameter("@Username", objToUpdate.Username)
        objParam(4) = param
        param = New SqlParameter("@Lastname", objToUpdate.Lastname)
        objParam(5) = param
        param = New SqlParameter("@Displayname", objToUpdate.Displayname)
        objParam(6) = param
        param = New SqlParameter("@Department", objToUpdate.Department)
        objParam(7) = param
        param = New SqlParameter("@Mail", objToUpdate.Mail)
        objParam(8) = param
        param = New SqlParameter("@Mobile", objToUpdate.Mobile)
        objParam(9) = param
        param = New SqlParameter("@Jobtitle", objToUpdate.Jobtitle)
        objParam(10) = param
        param = New SqlParameter("@Description", objToUpdate.Description)
        objParam(11) = param
        param = New SqlParameter("@State", objToUpdate.State)
        objParam(12) = param
        param = New SqlParameter("@City", objToUpdate.City)
        objParam(13) = param
        param = New SqlParameter("@Office", objToUpdate.Office)
        objParam(14) = param
        param = New SqlParameter("@TelephoneNumber", objToUpdate.TelephoneNumber)
        objParam(15) = param
        param = New SqlParameter("@Company", objToUpdate.Company)
        objParam(16) = param
        param = New SqlParameter("@HomePhone", objToUpdate.HomePhone)
        objParam(17) = param
        param = New SqlParameter("@IsECMUser", objToUpdate.IsECMUser)
        objParam(19) = param
        param = New SqlParameter("@Manager", objToUpdate.Manager)
        objParam(20) = param
        param = New SqlParameter("@sAMAccountName", objToUpdate.sAMAccountName)
        objParam(21) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to : " + SqlHelper.errstr)
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZADUsers)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZADUsers set Isdeleted=1 where LdapUserId=@LdapUserId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LdapUserId", objToDelete.LdapUserId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZADUsers() As System.Collections.Generic.List(Of IeZADUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZADUsers)()
        Dim objItem As IeZADUsers
        Try
            Dim strQry As String = ""
            strQry = "Select LdapUserId From eZADUsers where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Ldap User.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZADUsers(GetInteger(sqlRdr("LdapUserId")))
                objItem.LdapUserId = GetInteger(sqlRdr("LdapUserId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZADUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZADUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZADUsers)()
        Dim objItem As IeZADUsers
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LdapUserId From eZADUsers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by LdapUserId"
            Else
                strQry = "Select LdapUserId From eZADUsers where Isdeleted=0 order by  LdapUserId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Ldap User.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZADUsers(GetInteger(sqlRdr("LdapUserId")))
                objItem.LdapUserId = GetInteger(sqlRdr("LdapUserId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZADUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZADUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZADUsers)()
        Dim objItem As IeZADUsers

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LdapUserId From eZADUsers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by  LdapUserId"
            Else
                strQry = "Select LdapUserId From eZADUsers where Isdeleted=0 order by LdapUserId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Ldap User.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZADUsers(GetInteger(sqlRdr("LdapUserId")))
                objItem.LdapUserId = GetInteger(sqlRdr("LdapUserId"))
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