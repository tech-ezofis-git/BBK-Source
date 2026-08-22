Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "User Tasks"
    Public Function CreateTask(objEmp As eZTask) As IeZTask
        Dim newObject As IeZTask = Nothing
        If String.IsNullOrEmpty(objEmp.Task) Then
            Return Nothing
        End If
        objEmp.Task = objEmp.Task.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select TaskId From eZTask Where Task = @Task And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@Task", objEmp.Task)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("Task Code already exist!")
            End If
            strQry = "INSERT INTO eZTask(Task,StartTime,EndTime,templateid,itemid,Taskpriority,Typeid,CreatedOn,CreatedBy) VALUES(@Task,@StartTime,@EndTime,@templateid,@itemid,@Taskpriority,@typeid,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@Task", objEmp.Task)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@StartTime", objEmp.StartTime)
            objParam(3) = param
            param = New SqlParameter("@EndTime", objEmp.EndTime)
            objParam(4) = param
            param = New SqlParameter("@templateid", objEmp.templateid)
            objParam(5) = param
            param = New SqlParameter("@itemid", objEmp.itemid)
            objParam(6) = param
            param = New SqlParameter("@Taskpriority", objEmp.TaskPriority)
            objParam(7) = param
            param = New SqlParameter("@Typeid", objEmp.Typeid)
            objParam(8) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZTask(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            'DBLayer.DBLInstance.ERRORCODEMessage("DBLayer", e.Message, "When New Task Insert")
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZTask)
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
            If objRead.Task Is Nothing Then

                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZTask Where TaskId=@Task_ID and Isdeleted=0"
                param = New SqlParameter("@Task_ID", objRead.TaskId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZTask Where Task=@Task and Isdeleted=0"
                param = New SqlParameter("@Task", objRead.Task)
                objParam(0) = param

            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Task.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.TaskId = GetInteger(sqlRdr("TaskId"))
                objRead.Task = sqlRdr("Task").ToString()
                objRead.TaskStatus = GetInteger(sqlRdr("TaskStatus"))
                objRead.StartTime = sqlRdr("StartTime")
                objRead.EndTime = sqlRdr("EndTime")
                objRead.Notification = GetInteger(sqlRdr("Notification"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                objRead.Templateid = GetInteger(sqlRdr("Templateid"))
                objRead.itemid = GetInteger(sqlRdr("Itemid"))
                objRead.TaskPriority = GetInteger(sqlRdr("TaskPriority"))
                objRead.Typeid = GetInteger(sqlRdr("Typeid"))
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
    Public Function ReadAllTask() As System.Collections.Generic.List(Of IeZTask)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTask)()
        Dim objItem As IeZTask

        Try
            Dim strQry As String = ""
            strQry = "Select TaskId From eZTask where Isdeleted=0 order by Task"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Task.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTask(GetInteger(sqlRdr("TaskId")))
                objItem.TaskId = GetInteger(sqlRdr("TaskId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZTask)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select TaskId From eZTask Where Task = @Task and TaskId <> @TaskId and CreatedBy=@UpdatedBy and Isdeleted=0"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@Task", objToUpdate.Task)
        objParam(0) = param
        param = New SqlParameter("@TaskId", objToUpdate.TaskId)
        objParam(1) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(2) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("Task Code already exist!")
        Else
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            strQry = "Update eZTask Set Task=@Task, TaskStatus=@TaskStatus,StartTime=@StartTime,Notification=@Notification,EndTime=@EndTime,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy,Templateid=@Templateid,itemid=@itemid,Taskpriority=@TaskPriority,Typeid=@Typeid where TaskId=@Task_ID"
            objParam = New SqlParameter(13) {}
            param = New SqlParameter("@Task", objToUpdate.Task)
            objParam(0) = param
            param = New SqlParameter("@Task_ID", objToUpdate.TaskId)
            objParam(1) = param
            param = New SqlParameter("@TaskStatus", objToUpdate.TaskStatus)
            objParam(2) = param
            param = New SqlParameter("@StartTime", objToUpdate.StartTime)
            objParam(3) = param
            param = New SqlParameter("@EndTime", objToUpdate.EndTime)
            objParam(4) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(5) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(6) = param
            param = New SqlParameter("@Createdon", objToUpdate.CreatedOn)
            objParam(7) = param
            param = New SqlParameter("@Createdby", objToUpdate.CreatedBy)
            objParam(8) = param
            param = New SqlParameter("@Templateid", objToUpdate.Templateid)
            objParam(9) = param
            param = New SqlParameter("@Itemid", objToUpdate.itemid)
            objParam(10) = param
            param = New SqlParameter("@TaskPriority", objToUpdate.TaskPriority)
            objParam(11) = param
            param = New SqlParameter("@Typeid", objToUpdate.Typeid)
            objParam(12) = param
            param = New SqlParameter("@Notification", objToUpdate.Notification)
            objParam(13) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZTask)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZTask set Isdeleted=1 where TaskId=@Task_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Task_ID", objToDelete.TaskId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub

    Public Sub UpdateTask(ByVal TaskId As Integer, ByVal TaskStatus As Integer, ByVal LoginId As Integer)

        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZTask set UpdatedBy=@LoginId,TaskStatus=@TaskStatus where TaskId=@TaskId"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@TaskId", TaskId)
        objParam(0) = param
        param = New SqlParameter("@TaskStatus", TaskStatus)
        objParam(1) = param
        param = New SqlParameter("@LoginId", LoginId)
        objParam(2) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


    Public Function ReadFilteredeZTask(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTask)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTask)()
        Dim objItem As IeZTask

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TaskId From eZTask where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Task"
            Else
                strQry = "Select TaskId From eZTask where Isdeleted=0 order by Task"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTask(GetInteger(sqlRdr("TaskId")))
                objItem.TaskId = GetInteger(sqlRdr("TaskId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTask(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTask)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTask)()
        Dim objItem As IeZTask

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TaskId From eZTask where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Task"
            Else
                strQry = "Select TaskId From eZTask where Isdeleted=0 order by Task"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTask(GetInteger(sqlRdr("TaskId")))
                objItem.TaskId = GetInteger(sqlRdr("TaskId"))
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
