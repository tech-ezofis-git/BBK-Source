Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZUnAllocatedMail)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1,ewf.WorkflowName as Workflow From eZUnAllocatedMail ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "left join ezworkflowdetails ewf on ez.workflowid=ewf.WorkflowId Where ez.MailRequestId=@MailRequestId and ez.Isdeleted=0"
            param = New SqlParameter("@MailRequestId", objRead.MailRequestId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZUnAllocatedMail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.MailRequestId = GetInteger(sqlRdr("MailRequestId"))
                objRead.MailSettingsId = GetInteger(sqlRdr("MailSettingsId"))
                objRead.WorkflowId = GetInteger(sqlRdr("WorkflowId"))
                objRead.JunkMail = GetBoolean(sqlRdr("JunkMail"))
                objRead.MailSubject = sqlRdr("MailSubject").ToString
                objRead.MailBody = sqlRdr("MailBody").ToString
                objRead.MailFrom = sqlRdr("MailFrom").ToString
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.Workflow = sqlRdr("Workflow").ToString()
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
    Public Function CreateeZUnAllocatedMail(objEmp As eZUnAllocatedMail) As eZUnAllocatedMail
        Dim newObject As eZUnAllocatedMail = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZUnAllocatedMail(MailSubject,MailBody,MailFrom,MailSettingsId,WorkflowId,JunkMail,CreatedBy,CreatedOn) VALUES " +
                "(@MailSubject,@MailBody,@MailFrom,@MailSettingsId,@WorkflowId,@JunkMail,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@MailSubject", objEmp.MailSubject)
            objParam(0) = param
            param = New SqlParameter("@MailBody", objEmp.MailBody)
            objParam(1) = param
            param = New SqlParameter("@MailFrom", objEmp.MailFrom)
            objParam(2) = param
            param = New SqlParameter("@MailSettingsId", objEmp.MailSettingsId)
            objParam(3) = param
            param = New SqlParameter("@WorkflowId", objEmp.WorkflowId)
            objParam(4) = param
            param = New SqlParameter("@JunkMail", objEmp.JunkMail)
            objParam(5) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(6) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZUnAllocatedMail(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZUnAllocatedMail)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZUnAllocatedMail Set MailSubject=@MailSubject,MailBody=@MailBody,MailFrom=@MailFrom,MailSettingsId=@MailSettingsId," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,WorkflowId=@WorkflowId,JunkMail=@JunkMail where MailRequestId=@MailRequestId"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@MailSubject", objToUpdate.MailSubject)
        objParam(0) = param
        param = New SqlParameter("@MailBody", objToUpdate.MailBody)
        objParam(1) = param
        param = New SqlParameter("@MailFrom", objToUpdate.MailFrom)
        objParam(2) = param
        param = New SqlParameter("@MailSettingsId", objToUpdate.MailSettingsId)
        objParam(3) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(4) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(5) = param
        param = New SqlParameter("@MailRequestId", objToUpdate.MailRequestId)
        objParam(6) = param
        param = New SqlParameter("@WorkflowId", objToUpdate.WorkflowId)
        objParam(7) = param
        param = New SqlParameter("@JunkMail", objToUpdate.JunkMail)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZUnAllocatedMail)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZUnAllocatedMail set Isdeleted=1 where MailRequestId=@MailRequestId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@MailRequestId", objToDelete.MailRequestId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZUnAllocatedMail() As System.Collections.Generic.List(Of IeZUnAllocatedMail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZUnAllocatedMail)()
        Dim objItem As IeZUnAllocatedMail
        Try
            Dim strQry As String = ""
            strQry = "Select MailRequestId From eZUnAllocatedMail where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZUnAllocatedMail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZUnAllocatedMail(GetInteger(sqlRdr("MailRequestId")))
                objItem.MailRequestId = GetInteger(sqlRdr("MailRequestId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZUnAllocatedMail(Criteria As String, Value As String) As List(Of IeZUnAllocatedMail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZUnAllocatedMail)()
        Dim objItem As IeZUnAllocatedMail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailRequestId From eZUnAllocatedMail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by MailRequestId"
            Else
                strQry = "Select MailRequestId From eZUnAllocatedMail where Isdeleted=0 order by MailRequestId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZUnAllocatedMail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZUnAllocatedMail(GetInteger(sqlRdr("MailRequestId")))
                objItem.MailRequestId = GetInteger(sqlRdr("MailRequestId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZUnAllocatedMail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZUnAllocatedMail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZUnAllocatedMail)()
        Dim objItem As IeZUnAllocatedMail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailRequestId From eZUnAllocatedMail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by MailRequestId"
            Else
                strQry = "Select MailRequestId From eZUnAllocatedMail where Isdeleted=0 order by MailRequestId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZUnAllocatedMail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZUnAllocatedMail(GetInteger(sqlRdr("MailRequestId")))
                objItem.MailRequestId = GetInteger(sqlRdr("MailRequestId"))
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
