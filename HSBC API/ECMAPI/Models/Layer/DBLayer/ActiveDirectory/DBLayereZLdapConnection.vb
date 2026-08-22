Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZLdapConnection)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZLdapConnection" +
                " Where LdapConnId=@LdapConnId and Isdeleted=0"
            param = New SqlParameter("@LdapConnId", objRead.LdapConnId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Ldap Connection")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.LdapConnId = GetInteger(sqlRdr("LdapConnId"))
                objRead.Preferred = Convert.ToInt32(Convert.ToBoolean(sqlRdr("Preferred")))
                objRead.LdapDomain = sqlRdr("LdapDomain").ToString
                objRead.LdapServer = sqlRdr("LdapServer").ToString
                objRead.Username = sqlRdr("Username").ToString
                objRead.Pasword = DBLayer.Decrypt(sqlRdr("Pasword").ToString, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.LdapPath = sqlRdr("LdapPath").ToString()
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

    Public Function CreateeZLdapConnection(objEmp As eZLdapConnection) As eZLdapConnection
        Dim newObject As eZLdapConnection = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZLdapConnection(LdapServer,LdapDomain,Username,CreatedBy,CreatedOn,Pasword,Preferred,LdapPath) VALUES" +
                "(@LdapServer,@LdapDomain,@Username,@CreatedBy,@CreatedOn,@Pasword,@Preferred,@LdapPath);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@LdapServer", objEmp.LdapServer)
            objParam(2) = param
            param = New SqlParameter("@LdapDomain", objEmp.LdapDomain)
            objParam(3) = param
            param = New SqlParameter("@Username", objEmp.Username)
            objParam(4) = param
            param = New SqlParameter("@Pasword", DBLayer.Encrypt(objEmp.Pasword, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192))
            objParam(5) = param
            param = New SqlParameter("@Preferred", objEmp.Preferred)
            objParam(6) = param
            param = New SqlParameter("@LdapPath", objEmp.LdapPath)
            objParam(7) = param
            Dim obj As Object = Nothing
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZLdapConnection(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZLdapConnection)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLdapConnection Set LdapServer=@LdapServer,LdapDomain=@LdapDomain,Username=@Username," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,Pasword=@Pasword,Preferred=@Preferred,LdapPath=@LdapPath where LdapConnId=@LdapConnId"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(0) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(1) = param
        param = New SqlParameter("@LdapServer", objToUpdate.LdapServer)
        objParam(2) = param
        param = New SqlParameter("@LdapDomain", objToUpdate.LdapDomain)
        objParam(3) = param
        param = New SqlParameter("@Username", objToUpdate.Username)
        objParam(4) = param
        param = New SqlParameter("@Pasword", DBLayer.Encrypt(objToUpdate.Pasword, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192))
        objParam(5) = param
        param = New SqlParameter("@Preferred", objToUpdate.Preferred)
        objParam(6) = param
        param = New SqlParameter("@LdapPath", objToUpdate.LdapPath)
        objParam(7) = param
        param = New SqlParameter("@LdapConnId", objToUpdate.LdapConnId)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZLdapConnection)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLdapConnection set Isdeleted=1 where LdapConnId=@LdapConnId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LdapConnId", objToDelete.LdapConnId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZLdapConnection() As System.Collections.Generic.List(Of IeZLdapConnection)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLdapConnection)()
        Dim objItem As IeZLdapConnection
        Try
            Dim strQry As String = ""
            strQry = "Select LdapConnId From eZLdapConnection where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Vault.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLdapConnection(GetInteger(sqlRdr("LdapConnId")))
                objItem.LdapConnId = GetInteger(sqlRdr("LdapConnId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZLdapConnection(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLdapConnection)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLdapConnection)()
        Dim objItem As IeZLdapConnection
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LdapConnId From eZLdapConnection where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by LdapConnId"
            Else
                strQry = "Select LdapConnId From eZLdapConnection where Isdeleted=0 order by  LdapConnId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Ldap Connection.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLdapConnection(GetInteger(sqlRdr("LdapConnId")))
                objItem.LdapConnId = GetInteger(sqlRdr("LdapConnId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZLdapConnection(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLdapConnection)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLdapConnection)()
        Dim objItem As IeZLdapConnection

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LdapConnId From eZLdapConnection where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by  LdapConnId"
            Else
                strQry = "Select LdapConnId From eZLdapConnection where Isdeleted=0 order by LdapConnId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Ldap Connection.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLdapConnection(GetInteger(sqlRdr("LdapConnId")))
                objItem.LdapConnId = GetInteger(sqlRdr("LdapConnId"))
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
