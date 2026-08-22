Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateWorkFlowProcess(objEmp As eZWorkFlowProcess) As IeZWorkFlowProcess
        Dim newObject As IeZWorkFlowProcess = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZWorkFlowProcess(ProcessId,WorkFlowId,Stage,InitiatedOn,CreatedOn,CreatedBy) VALUES(@ProcessId,@WorkFlowId,@Stage,@InitiatedOn,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@WorkFlowId", objEmp.WorkFlowId)
            objParam(0) = param
            param = New SqlParameter("@Stage", objEmp.Stage)
            objParam(1) = param
            param = New SqlParameter("@InitiatedOn", objEmp.InitiatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZWorkFlowProcess(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZWorkFlowProcess)
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

            objParam = New SqlParameter(0) {}
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZWorkFlowProcess Where ProcessId=@ProcessId and Isdeleted=0"
            param = New SqlParameter("@ProcessId", objRead.ProcessId)
            objParam(0) = param


            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid WorkFlow Process.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ProcessId = GetInteger(sqlRdr("ProcessId"))
                objRead.WorkFlowId = GetInteger(sqlRdr("WorkFlowId"))
                objRead.Stage = sqlRdr("Stage").ToString
                objRead.InitiatedOn = sqlRdr("InitiatedOn").ToString
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
    Public Function ReadAllWorkFlowProcess() As System.Collections.Generic.List(Of IeZWorkFlowProcess)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkFlowProcess)()
        Dim objItem As IeZWorkFlowProcess

        Try
            Dim strQry As String = ""
            strQry = "Select ProcessId From eZWorkFlowProcess where Isdeleted=0 order by ProcessId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid WorkFlow Process.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkFlowProcess(GetInteger(sqlRdr("ProcessId")))
                objItem.ProcessId = GetInteger(sqlRdr("ProcessId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedeZWorkFlowProcess(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZWorkFlowProcess)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkFlowProcess)()
        Dim objItem As IeZWorkFlowProcess
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ProcessId From eZWorkFlowProcess where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ProcessId"
            Else
                strQry = "Select ProcessId From eZWorkFlowProcess where Isdeleted=0 order by ProcessId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid WorkFlow Process.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkFlowProcess(GetSmallInterger(sqlRdr("ProcessId")))
                objItem.ProcessId = GetSmallInterger(sqlRdr("ProcessId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZWorkFlowProcess)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        'strQry = "Select ScheduleId From eZSchedule Where ScheduleId <> @ScheduleId and Isdeleted=0"
        'objParam = New SqlParameter(0) {}
        'param = New SqlParameter("@ScheduleId", objToUpdate.ScheduleId)
        'objParam(0) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("Schedule Code already exist!")
        'Else
        strQry = "Update eZWorkFlowProcess Set Stage=@Stage,WorkFlowId=@WorkFlowId,InitiatedOn=@InitiatedOn,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ProcessId=@ProcessID"
        objParam = New SqlParameter(5) {}
        param = New SqlParameter("@ProcessId", objToUpdate.ProcessId)
        objParam(0) = param
        param = New SqlParameter("@Stage", objToUpdate.Stage)
        objParam(1) = param
        param = New SqlParameter("@WorkFlowId", objToUpdate.WorkFlowId)
        objParam(2) = param
        param = New SqlParameter("@InitiatedOn", objToUpdate.InitiatedOn)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZWorkFlowProcess)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZWorkFlowProcess set Isdeleted=1 where ProcessId=@ProcessId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ProcessId", objToDelete.ProcessId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class