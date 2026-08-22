Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "User MailArchives"

    Public Function CreateZMail(objEmp As eZMail) As IeZMail
        Dim newObject As IeZMail = Nothing

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZMail(ToAdd,Subject,Body,AttachmentsPaths,BodyHtmlTypeId,CreatedOn,CreatedBy,MailSettingId) VALUES(@ToAdd,@Subject,@Body,@AttachmentsPaths,@BodyHtmlTypeId,@CreatedOn,@CreatedBy,@MailSettingId);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@ToAdd", objEmp.ToAdd)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@Subject", objEmp.Subject)
            objParam(4) = param
            param = New SqlParameter("@Body", objEmp.Body)
            objParam(5) = param
            param = New SqlParameter("@AttachmentsPaths", objEmp.AttachmentsPaths)
            objParam(6) = param
            param = New SqlParameter("@BodyHtmlTypeId", objEmp.BodyHtmlTypeId)
            objParam(7) = param
            param = New SqlParameter("@MailSettingId", objEmp.MailSettingId)
            objParam(1) = param
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

    Public Sub Read(objRead As IeZMail)
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
            If objRead.ToAdd = 0 Then
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZMail Where MailId=@MailId and Isdeleted=0"
                param = New SqlParameter("@MailId", objRead.MailId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(0) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZMail Where Subject=@Subject and Isdeleted=0"
                param = New SqlParameter("@Subject", objRead.Subject)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Subject.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.MailId = GetInteger(sqlRdr("MailId"))
                objRead.MailStatus = GetInteger(sqlRdr("MailStatus"))
                objRead.Subject = sqlRdr("Subject").ToString
                objRead.BodyHtmlTypeId = GetInteger(sqlRdr("BodyHtmlTypeId"))
                objRead.ToAdd = sqlRdr("ToAdd").ToString
                objRead.AttachmentsPaths = sqlRdr("AttachmentsPaths").ToString()
                objRead.Body = sqlRdr("Body").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                objRead.MailSettingId = GetInteger(sqlRdr("MailSettingId"))
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
    Public Function UpdateMailStatus(MailId As Integer, MailStatus As Integer) As String
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMail Set MailStatus = @MailStatus where MailId=@MailId"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@MailId", MailId)
        objParam(0) = param
        param = New SqlParameter("@MailStatus", MailStatus)
        objParam(1) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Return "Record Not updated due to some error"
        Else
            Return "MailStatus Updated"
        End If
    End Function
    Public Function ReadAlleZMail() As System.Collections.Generic.List(Of IeZMail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMail)()
        Dim objItem As IeZMail

        Try
            Dim strQry As String = ""
            strQry = "Select MailId From eZMail where Isdeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMail(GetInteger(sqlRdr("MailId")))
                objItem.MailId = GetInteger(sqlRdr("MailId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZMail)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select MailId From eZMail Where MailId <> @MailId and Isdeleted=0"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@MailId", objToUpdate.MailId)
        objParam(0) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ezmail Code already exist!")
        Else
            strQry = "Update eZMail Set ToAdd=@ToAdd,BodyHtmlTypeId=@BodyHtmlTypeId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy,MailSettingId=@MailSettingId where MailId=@MailArchive_ID"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@MailArchive_ID", objToUpdate.MailId)
            objParam(1) = param
            param = New SqlParameter("@ToAdd", objToUpdate.ToAdd)
            objParam(2) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(3) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(4) = param
            param = New SqlParameter("@BodyHtmlTypeId", objToUpdate.BodyHtmlTypeId)
            objParam(5) = param
            param = New SqlParameter("@MailSettingId", objToUpdate.MailSettingId)
            objParam(0) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZMail)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMail set Isdeleted=1 where MailId=@MailArchive_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@MailArchive_ID", objToDelete.MailId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub

    Public Function ReadFilteredeZMail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMail)()
        Dim objItem As IeZMail

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailId From eZMail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
            Else
                strQry = "Select MailId From eZMail where Isdeleted=0"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMail(GetInteger(sqlRdr("MailId")))
                objItem.MailId = GetInteger(sqlRdr("MailId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMail)()
        Dim objItem As IeZMail

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailId From eZMail where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
            Else
                strQry = "Select MailId From eZMail where Isdeleted=0"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMail(GetInteger(sqlRdr("MailId")))
                objItem.MailId = GetInteger(sqlRdr("MailId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

#End Region

End Class
