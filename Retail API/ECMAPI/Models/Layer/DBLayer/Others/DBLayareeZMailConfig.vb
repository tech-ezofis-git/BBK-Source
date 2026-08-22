
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports ECMAPI.DBLibrary

Partial Public Class DBLayer

    Public Function CreateZMailConfig(objEmp As eZMailConfig) As IeZMailConfig
        Dim newObject As IeZMailConfig = Nothing

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZMailconfig([Host],[Port],[Mailid],[UserName],[Password],[EnableSSL],[CreatedOn],[CreatedBy]) VALUES(@Host,@Port,@Mailid,@UserName,@Password,@EnableSSL,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@Host", objEmp.Host)
            objParam(0) = param
            param = New SqlParameter("@Port", objEmp.Port)
            objParam(1) = param
            param = New SqlParameter("@Mailid", objEmp.Mailid)
            objParam(2) = param
            param = New SqlParameter("@UserName", objEmp.UserName)
            objParam(3) = param
            param = New SqlParameter("@Password", objEmp.Password)
            objParam(4) = param
            param = New SqlParameter("@EnableSSL", objEmp.EnableSSL)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(6) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZMail(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Read(objRead As IeZMailConfig)
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

            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZMailConfig Where MailConfigId=@MailConfigId and Isdeleted=0"
            param = New SqlParameter("@MailConfigId", objRead.MailConfigId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ID.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.MailConfigId = GetInteger(sqlRdr("MailConfigId"))
                objRead.Mailid = GetInteger(sqlRdr("MailId"))
                objRead.Host = sqlRdr("Host").ToString()
                objRead.Port = sqlRdr("Port").ToString()
                objRead.UserName = sqlRdr("UserName").ToString()
                objRead.Password = sqlRdr("Password").ToString()
                ' objRead.EnableSSL = GetInteger(sqlRdr("EnableSSL"))
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

End Class
