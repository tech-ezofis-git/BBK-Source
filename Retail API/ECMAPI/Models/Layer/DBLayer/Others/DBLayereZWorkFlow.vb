Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateWorkFlow(objEmp As eZWorkFlow) As IeZWorkFlow
        Dim newObject As IeZWorkFlow = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZWorkFlow(WorkFlowName,WorkFlowPath,CreatedOn,CreatedBy) VALUES(@WorkFlowName,@WorkFlowPath,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@WorkFlowName", objEmp.WorkFlowName)
            objParam(0) = param
            param = New SqlParameter("@WorkFlowPath", objEmp.WorkFlowPath)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZWorkFlow(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZWorkFlow)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZWorkFlow Where WorkFlowId=@WorkFlowId and Isdeleted=0"
            param = New SqlParameter("@WorkFlowId", objRead.WorkFlowId)
            objParam(0) = param


            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid WorkFlow.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.WorkFlowId = GetInteger(sqlRdr("WorkFlowId"))
                objRead.WorkFlowName = sqlRdr("WorkFlowName").ToString()
                objRead.WorkFlowPath = sqlRdr("WorkFlowPath").ToString()
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
    Public Function ReadAllWorkFlow() As System.Collections.Generic.List(Of IeZWorkFlow)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkFlow)()
        Dim objItem As IeZWorkFlow

        Try
            Dim strQry As String = ""
            strQry = "Select WorkFlowId From eZWorkFlow where Isdeleted=0 order by WorkFlowId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Work Flow.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkFlow(GetInteger(sqlRdr("WorkFlowId")))
                objItem.WorkFlowId = GetInteger(sqlRdr("WorkFlowId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedeZWorkFlow(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZWorkFlow)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkFlow)()
        Dim objItem As IeZWorkFlow
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select WorkFlowId From eZWorkFlow where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by WorkFlowId"
            Else
                strQry = "Select WorkFlowId From eZWorkFlow where Isdeleted=0 order by WorkFlowId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid WorkFlow.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkFlow(GetSmallInterger(sqlRdr("WorkFlowId")))
                objItem.WorkFlowId = GetSmallInterger(sqlRdr("WorkFlowId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZWorkFlow)
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
        strQry = "Update eZWorkFlow Set WorkFlowName=@WorkFlowName,WorkFlowPath=@WorkFlowPath,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where WorkFlowId=@WorkFlowID"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@WorkFlowId", objToUpdate.WorkFlowId)
        objParam(0) = param
        param = New SqlParameter("@WorkFlowName", objToUpdate.WorkFlowName)
        objParam(1) = param
        param = New SqlParameter("@WorkFlowPath", objToUpdate.WorkFlowPath)
        objParam(2) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(3) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(4) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZWorkFlow)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZWorkFlow set Isdeleted=1 where WorkFlowId=@WorkFlowId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@WorkFlowId", objToDelete.WorkFlowId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class