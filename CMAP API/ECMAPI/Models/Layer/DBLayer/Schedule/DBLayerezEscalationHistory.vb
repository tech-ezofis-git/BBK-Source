Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IezEscalationHistory)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From ezEscalation_History ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.EscalationHistoryId=@EscalationHistoryId and ez.Isdeleted=0"
            param = New SqlParameter("@EscalationHistoryId", objRead.EscalationHistoryId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezEscalationHistory")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.EscalationHistoryId = GetInteger(sqlRdr("EscalationHistoryId"))
                objRead.EscalationId = GetInteger(sqlRdr("EscalationId"))
                objRead.WorkflowId = GetInteger(sqlRdr("WorkflowId"))
                objRead.ActivityId = sqlRdr("ActivityId").ToString()
                objRead.ResponseTime = sqlRdr("ResponseTime").ToString
                objRead.ResponseType = sqlRdr("ResponseType").ToString
                objRead.Notification = GetBoolean(sqlRdr("Notification"))
                objRead.ActionFlow = GetBoolean(sqlRdr("ActionFlow"))
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.ActivityName = sqlRdr("ActivityName").ToString()
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
    Public Function CreateezEscalationHistory(objEmp As ezEscalationHistory) As ezEscalationHistory
        Dim newObject As ezEscalationHistory = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezEscalation_History(WorkflowId,ActivityId,ResponseTime,ResponseType,Notification,ActionFlow,CreatedBy,CreatedOn" +
                ",ActivityName,EscalationId) VALUES (@WorkflowId,@ActivityId,@ResponseTime,@ResponseType,@Notification,@ActionFlow,@CreatedBy" +
                ",@CreatedOn,@ActivityName,@EscalationId);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(9) {}
            param = New SqlParameter("@WorkflowId", objEmp.WorkflowId)
            objParam(0) = param
            param = New SqlParameter("@ActivityId", objEmp.ActivityId)
            objParam(1) = param
            param = New SqlParameter("@ResponseTime", objEmp.ResponseTime)
            objParam(2) = param
            param = New SqlParameter("@ResponseType", objEmp.ResponseType)
            objParam(3) = param
            param = New SqlParameter("@Notification", objEmp.Notification)
            objParam(4) = param
            param = New SqlParameter("@ActionFlow", objEmp.ActionFlow)
            objParam(5) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(6) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(7) = param
            param = New SqlParameter("@ActivityName", objEmp.ActivityName)
            objParam(8) = param
            param = New SqlParameter("@EscalationId", objEmp.EscalationId)
            objParam(9) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezEscalationHistory(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IezEscalationHistory)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezEscalation_History Set WorkflowId=@WorkflowId,ActivityId=@ActivityId,ResponseTime=@ResponseTime,ResponseType=@ResponseType," +
            "Notification=@Notification,ActionFlow=@ActionFlow,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,ActivityName=@ActivityName,EscalationId=@EscalationId" +
            " where EscalationHistoryId=@EscalationHistoryId"
        objParam = New SqlParameter(10) {}
        param = New SqlParameter("@WorkflowId", objToUpdate.WorkflowId)
        objParam(0) = param
        param = New SqlParameter("@ActivityId", objToUpdate.ActivityId)
        objParam(1) = param
        param = New SqlParameter("@ResponseTime", objToUpdate.ResponseTime)
        objParam(2) = param
        param = New SqlParameter("@ResponseType", objToUpdate.ResponseType)
        objParam(3) = param
        param = New SqlParameter("@Notification", objToUpdate.Notification)
        objParam(4) = param
        param = New SqlParameter("@ActionFlow", objToUpdate.ActionFlow)
        objParam(5) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(6) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(7) = param
        param = New SqlParameter("@ActivityName", objToUpdate.ActivityName)
        objParam(8) = param
        param = New SqlParameter("@EscalationId", objToUpdate.EscalationId)
        objParam(9) = param
        param = New SqlParameter("@EscalationHistoryId", objToUpdate.EscalationHistoryId)
        objParam(10) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IezEscalationHistory)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezEscalation_History set Isdeleted=1 where EscalationHistoryId=@EscalationHistoryId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@EscalationHistoryId", objToDelete.EscalationHistoryId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllezEscalationHistory() As System.Collections.Generic.List(Of IezEscalationHistory)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezEscalationHistory)()
        Dim objItem As IezEscalationHistory
        Try
            Dim strQry As String = ""
            strQry = "Select EscalationHistoryId From ezEscalation_History where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezEscalationHistory")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezEscalationHistory(GetInteger(sqlRdr("EscalationHistoryId")))
                objItem.EscalationHistoryId = GetInteger(sqlRdr("EscalationHistoryId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredezEscalationHistory(Criteria As String, Value As String) As List(Of IezEscalationHistory)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezEscalationHistory)()
        Dim objItem As IezEscalationHistory
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select EscalationHistoryId From ezEscalation_History where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by EscalationHistoryId"
            Else
                strQry = "Select EscalationHistoryId From ezEscalationHistory where Isdeleted=0 order by EscalationHistoryId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezEscalationHistory")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezEscalationHistory(GetInteger(sqlRdr("EscalationHistoryId")))
                objItem.EscalationHistoryId = GetInteger(sqlRdr("EscalationHistoryId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedezEscalationHistory(Criteria As String, Value As String) As System.Collections.Generic.List(Of IezEscalationHistory)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezEscalationHistory)()
        Dim objItem As IezEscalationHistory
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select EscalationHistoryId From ezEscalation_History where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by EscalationHistoryId"
            Else
                strQry = "Select EscalationHistoryId From ezEscalationHistory where Isdeleted=0 order by EscalationHistoryId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezEscalationHistory")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezEscalationHistory(GetInteger(sqlRdr("EscalationHistoryId")))
                objItem.EscalationHistoryId = GetInteger(sqlRdr("EscalationHistoryId"))
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
