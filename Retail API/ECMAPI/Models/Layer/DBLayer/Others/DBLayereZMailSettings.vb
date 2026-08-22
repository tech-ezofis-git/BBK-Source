Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZMailSettings)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From ezmailsettings " +
                "Where SettingId=@SettingId and Isdeleted=0"
            param = New SqlParameter("@SettingId", objRead.SettingId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Mail Settings")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.SettingId = GetInteger(sqlRdr("SettingId"))
                objRead.SettingName = sqlRdr("SettingName").ToString
                objRead.IncomingPort = GetInteger(sqlRdr("IncomingPort"))
                objRead.EmailId = sqlRdr("EmailId").ToString
                objRead.UserName = sqlRdr("UserName").ToString
                objRead.Password = DBLayer.Decrypt(sqlRdr("Password").ToString, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                objRead.IncomingServer = sqlRdr("IncomingServer").ToString
                objRead.EnableSSL = Convert.ToInt32(Convert.ToBoolean(sqlRdr("EnableSSL")))
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.Preference = GetInteger(sqlRdr("Preference"))
                objRead.OutgoingPort = GetInteger(sqlRdr("OutgoingPort"))
                objRead.OutgoingServer = sqlRdr("OutgoingServer").ToString
                objRead.LogoPath = sqlRdr("LogoPath").ToString
                objRead.Signature = sqlRdr("Signature").ToString
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

    Public Function CreateeZMailSettings(objEmp As eZMailSettings) As eZMailSettings
        Dim newObject As eZMailSettings = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            If objEmp.Preference = 1 Then
                strQry = "Update ezmailsettings set Preference=0 where Preference=1;"
            End If
            strQry += "INSERT INTO ezmailsettings(SettingName,IncomingServer,EmailId,UserName,Password,IncomingPort,EnableSSL,CreatedBy,CreatedOn," +
                "Preference,OutgoingServer,OutgoingPort) VALUES (@SettingName,@Host,@EmailId,@UserName,@Password,@Port,@EnableSSL,@CreatedBy,@CreatedOn," +
                "@Preference,@OutgoingServer,@OutgoingPort);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(11) {}
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@SettingName", objEmp.SettingName)
            objParam(2) = param
            param = New SqlParameter("@Host", objEmp.IncomingServer)
            objParam(3) = param
            param = New SqlParameter("@EmailId", objEmp.EmailId)
            objParam(4) = param
            param = New SqlParameter("@UserName", objEmp.UserName)
            objParam(5) = param
            param = New SqlParameter("@Password", DBLayer.Encrypt(objEmp.Password, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192))
            objParam(6) = param
            param = New SqlParameter("@Port", objEmp.IncomingPort)
            objParam(7) = param
            param = New SqlParameter("@EnableSSL", objEmp.EnableSSL)
            objParam(8) = param
            param = New SqlParameter("@Preference", objEmp.Preference)
            objParam(9) = param
            param = New SqlParameter("@OutgoingServer", objEmp.OutgoingServer)
            objParam(10) = param
            param = New SqlParameter("@OutgoingPort", objEmp.OutgoingPort)
            objParam(11) = param
            Dim obj As Object = Nothing
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZMailSettings(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZMailSettings)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailSettings Set IncomingServer=@IncomingServer,IncomingPort=@IncomingPort,EmailId=@EmailId,UserName=@UserName," +
            "Password=@Password,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,SettingName=@SettingName,EnableSSL=@EnableSSL,Preference=@Preference," +
            "OutgoingPort=@OutgoingPort,OutgoingServer=@OutgoingServer where SettingId=@SettingId"
        objParam = New SqlParameter(12) {}
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(0) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(1) = param
        param = New SqlParameter("@SettingName", objToUpdate.SettingName)
        objParam(2) = param
        param = New SqlParameter("@IncomingServer", objToUpdate.IncomingServer)
        objParam(3) = param
        param = New SqlParameter("@EmailId", objToUpdate.EmailId)
        objParam(4) = param
        param = New SqlParameter("@UserName", objToUpdate.UserName)
        objParam(5) = param
        param = New SqlParameter("@Password", DBLayer.Encrypt(objToUpdate.Password, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192))
        objParam(6) = param
        param = New SqlParameter("@IncomingPort", objToUpdate.IncomingPort)
        objParam(7) = param
        param = New SqlParameter("@EnableSSL", objToUpdate.EnableSSL)
        objParam(8) = param
        param = New SqlParameter("@SettingId", objToUpdate.SettingId)
        objParam(9) = param
        param = New SqlParameter("@Preference", objToUpdate.Preference)
        objParam(10) = param
        param = New SqlParameter("@OutgoingPort", objToUpdate.OutgoingPort)
        objParam(11) = param
        param = New SqlParameter("@OutgoingServer", objToUpdate.OutgoingServer)
        objParam(12) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZMailSettings)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezmailsettings set Isdeleted=1 where SettingId=@SettingId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@SettingId", objToDelete.SettingId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZMailSettings() As System.Collections.Generic.List(Of IeZMailSettings)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailSettings)()
        Dim objItem As IeZMailSettings
        Try
            Dim strQry As String = ""
            strQry = "Select SettingId From eZMailSettings where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailSettings.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailSettings(GetInteger(sqlRdr("SettingId")))
                objItem.SettingId = GetInteger(sqlRdr("SettingId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZMailSettings(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailSettings)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailSettings)()
        Dim objItem As IeZMailSettings
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select SettingId From eZMailSettings where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by SettingId"
            Else
                strQry = "Select SettingId From eZMailSettings where Isdeleted=0 order by SettingId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailSettings.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailSettings(GetInteger(sqlRdr("SettingId")))
                objItem.SettingId = GetInteger(sqlRdr("SettingId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZMailSettings(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailSettings)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailSettings)()
        Dim objItem As IeZMailSettings

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select SettingId From eZMailSettings where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by SettingId"
            Else
                strQry = "Select SettingId From eZMailSettings where Isdeleted=0 order by SettingId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailSettings.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailSettings(GetInteger(sqlRdr("SettingId")))
                objItem.SettingId = GetInteger(sqlRdr("SettingId"))
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
