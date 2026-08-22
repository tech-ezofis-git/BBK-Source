Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "User AllottedTasks"
    Public Function CreateAllottedTask(objEmp As eZAllottedTask) As IeZAllottedTask
        Dim newObject As IeZAllottedTask = Nothing

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select AllottedTaskId From eZAllottedTask Where ECMLoginId = @ECMLoginId and TaskId = @TaskId And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@TaskId", objEmp.TaskId)
            objParam(1) = param
          
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ECMLoginId Code already exist!")
            End If
            strQry = "INSERT INTO eZAllottedTask(ECMLoginId,TaskId,CreatedOn,CreatedBy) VALUES(@ECMLoginId,@TaskId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@TaskId", objEmp.TaskId)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZAllottedTask(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZAllottedTask)
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
            If objRead.ECMLoginId = 0 Then
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1,dbo.udf_Task(TaskId) as Task,dbo.udf_LoginName(ECMLoginId) as LoginName From eZAllottedTask Where AllottedTaskId=@AllottedTask_ID and Isdeleted=0"
                param = New SqlParameter("@AllottedTask_ID", objRead.AllottedTaskId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1,dbo.udf_Task(TaskId) as Task,dbo.udf_LoginName(ECMLoginId) as LoginName From eZAllottedTask Where ECMLoginId=@ECMLoginId and Isdeleted=0"
                param = New SqlParameter("@ECMLoginId", objRead.ECMLoginId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMLoginId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.AllottedTaskId = GetInteger(sqlRdr("AllottedTaskId"))
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                objRead.TaskId = GetInteger(sqlRdr("TaskId"))
                objRead.Task = sqlRdr("Task").ToString()
                objRead.Notification = GetInteger(sqlRdr("Notification"))
                objRead.LoginName = sqlRdr("LoginName").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                objRead.status = sqlRdr("Status").ToString()
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
    Public Function ReadAllAllottedTask() As System.Collections.Generic.List(Of IeZAllottedTask)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAllottedTask)()
        Dim objItem As IeZAllottedTask

        Try
            Dim strQry As String = ""
            strQry = "Select AllottedTaskId From eZAllottedTask where Isdeleted=0 order by ECMLoginId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMLoginId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAllottedTask(GetInteger(sqlRdr("AllottedTaskId")))
                objItem.AllottedTaskId = GetInteger(sqlRdr("AllottedTaskId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZAllottedTask)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select AllottedTaskId From eZAllottedTask Where TaskId = @TaskId and AllottedTaskId <> @AllottedTaskId  and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@TaskId", objToUpdate.TaskId)
        objParam(0) = param
        param = New SqlParameter("@AllottedTaskId", objToUpdate.AllottedTaskId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ECMLoginId Code already exist!")
        Else
            strQry = "Update eZAllottedTask Set ECMLoginId=@ECMLoginId,TaskId=@TaskId,Status=@Status,Notification=@Notification,Createdby=@Updatedby,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where AllottedTaskId=@AllottedTask_ID"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@AllottedTask_ID", objToUpdate.AllottedTaskId)
            objParam(1) = param
            param = New SqlParameter("@TaskId", objToUpdate.TaskId)
            objParam(2) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(3) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(4) = param
            param = New SqlParameter("@Status", objToUpdate.status)
            objParam(5) = param
            param = New SqlParameter("@Notification", objToUpdate.Notification)
            objParam(6) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZAllottedTask)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZAllottedTask set Isdeleted=1 where AllottedTaskId=@AllottedTask_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@AllottedTask_ID", objToDelete.AllottedTaskId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub

    Public Sub DeleteTask(TaskId As Integer)

        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZAllottedTask set Isdeleted=1 where TaskId=@TaskId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@TaskId", TaskId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Function ReadFilteredeZAllottedTask(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZAllottedTask)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAllottedTask)()
        Dim objItem As IeZAllottedTask

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select AllottedTaskId From eZAllottedTask where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select AllottedTaskId From eZAllottedTask where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAllottedTask(GetInteger(sqlRdr("AllottedTaskId")))
                objItem.AllottedTaskId = GetInteger(sqlRdr("AllottedTaskId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZAllottedTask(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZAllottedTask)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAllottedTask)()
        Dim objItem As IeZAllottedTask

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select AllottedTaskId From eZAllottedTask where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select AllottedTaskId From eZAllottedTask where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAllottedTask(GetInteger(sqlRdr("AllottedTaskId")))
                objItem.AllottedTaskId = GetInteger(sqlRdr("AllottedTaskId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZAllottedTaskWithLoginid(Criteria As String, Value As String, Loginid As String) As System.Collections.Generic.List(Of IeZAllottedTask)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAllottedTask)()
        Dim objItem As IeZAllottedTask

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select AllottedTaskId From eZAllottedTask where Isdeleted=0 and ECMLoginId=" + Loginid + " and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select AllottedTaskId From eZAllottedTask where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAllottedTask(GetInteger(sqlRdr("AllottedTaskId")))
                objItem.AllottedTaskId = GetInteger(sqlRdr("AllottedTaskId"))
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
