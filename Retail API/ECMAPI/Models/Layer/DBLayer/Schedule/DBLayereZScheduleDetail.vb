Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZScheduleDetail)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZScheduleDetail ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Detailid=@Detailid and ez.Isdeleted=0"
            param = New SqlParameter("@Detailid", objRead.Detailid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScheduleDetail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Id = GetInteger(sqlRdr("Id"))
                objRead.ForSchedule = GetInteger(sqlRdr("ForSchedule"))
                objRead.ScheduleId = GetInteger(sqlRdr("ScheduleId"))
                objRead.Status = GetBoolean(sqlRdr("Status"))
                objRead.Detailid = GetInteger(sqlRdr("Detailid"))
                objRead.Result = sqlRdr("Result").ToString
                objRead.ScheduleDate = sqlRdr("ScheduleDate").ToString
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
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
    Public Function CreateeZScheduleDetail(objEmp As eZScheduleDetail) As eZScheduleDetail
        Dim newObject As eZScheduleDetail = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZScheduleDetail(Id,ForSchedule,ScheduleId,ScheduleDate,Status,Result,CreatedBy,CreatedOn) VALUES " +
                "(@Id,@ForSchedule,@ScheduleId,@ScheduleDate,@Status,@Result,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@Id", objEmp.Id)
            objParam(0) = param
            param = New SqlParameter("@ForSchedule", objEmp.ForSchedule)
            objParam(1) = param
            param = New SqlParameter("@ScheduleId", objEmp.ScheduleId)
            objParam(2) = param
            param = New SqlParameter("@ScheduleDate", objEmp.ScheduleDate)
            objParam(3) = param
            param = New SqlParameter("@Status", objEmp.Status)
            objParam(4) = param
            param = New SqlParameter("@Result", objEmp.Result)
            objParam(5) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(6) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZScheduleDetail(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZScheduleDetail)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZScheduleDetail Set Id=@Id,ForSchedule=@ForSchedule,ScheduleId=@ScheduleId,ScheduleDate=@ScheduleDate," +
            "Status=@Status,Result=@Result,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where Detailid=@Detailid"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@Id", objToUpdate.Id)
        objParam(0) = param
        param = New SqlParameter("@ForSchedule", objToUpdate.ForSchedule)
        objParam(1) = param
        param = New SqlParameter("@ScheduleId", objToUpdate.ScheduleId)
        objParam(2) = param
        param = New SqlParameter("@ScheduleDate", objToUpdate.ScheduleDate)
        objParam(3) = param
        param = New SqlParameter("@Status", objToUpdate.Status)
        objParam(4) = param
        param = New SqlParameter("@Result", objToUpdate.Result)
        objParam(5) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(6) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(7) = param
        param = New SqlParameter("@Detailid", objToUpdate.Detailid)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZScheduleDetail)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZScheduleDetail set Isdeleted=1 where Detailid=@Detailid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Detailid", objToDelete.Detailid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZScheduleDetail() As System.Collections.Generic.List(Of IeZScheduleDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScheduleDetail)()
        Dim objItem As IeZScheduleDetail
        Try
            Dim strQry As String = ""
            strQry = "Select Detailid From eZScheduleDetail where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScheduleDetail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScheduleDetail(GetInteger(sqlRdr("Detailid")))
                objItem.Detailid = GetInteger(sqlRdr("Detailid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZScheduleDetail(Criteria As String, Value As String) As List(Of IeZScheduleDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScheduleDetail)()
        Dim objItem As IeZScheduleDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Detailid From eZScheduleDetail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Detailid"
            Else
                strQry = "Select Detailid From eZScheduleDetail where Isdeleted=0 order by Detailid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScheduleDetail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScheduleDetail(GetInteger(sqlRdr("Detailid")))
                objItem.Detailid = GetInteger(sqlRdr("Detailid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZScheduleDetail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZScheduleDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScheduleDetail)()
        Dim objItem As IeZScheduleDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Detailid From eZScheduleDetail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Detailid"
            Else
                strQry = "Select Detailid From eZScheduleDetail where Isdeleted=0 order by Detailid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScheduleDetail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScheduleDetail(GetInteger(sqlRdr("Detailid")))
                objItem.Detailid = GetInteger(sqlRdr("Detailid"))
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
