Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateSchedule(objEmp As eZSchedule) As IeZSchedule
        Dim newObject As IeZSchedule = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZSchedule(ScheduleTypeId,ForSchedule,Id,WeekDay,Mont,Day,EachDay,Time,OnceDate,CreatedOn,CreatedBy) VALUES(@ScheduleTypeId,@ForSchedule,@Id,@WeekDay,@Mont,@Day,@EachDay,@Time,@OnceDate,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(10) {}
            param = New SqlParameter("@ScheduleTypeId", objEmp.ScheduleTypeId)
            objParam(0) = param
            param = New SqlParameter("@WeekDay", objEmp.WeekDay)
            objParam(1) = param
            param = New SqlParameter("@Mont", objEmp.Mont)
            objParam(2) = param
            param = New SqlParameter("@Day", objEmp.Day)
            objParam(3) = param
            param = New SqlParameter("@Time", objEmp.Time)
            objParam(4) = param
            param = New SqlParameter("@OnceDate", objEmp.OnceDate)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(6) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(7) = param
            param = New SqlParameter("@ForSchedule", objEmp.ForSchedule)
            objParam(8) = param
            param = New SqlParameter("@Id", objEmp.Id)
            objParam(9) = param
            param = New SqlParameter("@EachDay", objEmp.EachDay)
            objParam(10) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZSchedule(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZSchedule)
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
            'If objRead.ScheduleId = 0 Then
            '    strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZSchedule Where ScheduleTypeId=@ScheduleTypeId and Isdeleted=0"
            '    param = New SqlParameter("@ScheduleTypeId", objRead.ScheduleTypeId)
            '    objParam(0) = param
            'Else
            objParam = New SqlParameter(0) {}
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZSchedule Where ScheduleId=@ScheduleId and Isdeleted=0"
            param = New SqlParameter("@ScheduleId", objRead.ScheduleId)
            objParam(0) = param
            'End If

            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Schedule.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ScheduleId = GetInteger(sqlRdr("ScheduleId"))
                objRead.ScheduleTypeId = GetInteger(sqlRdr("ScheduleTypeId"))
                objRead.ForSchedule = GetInteger(sqlRdr("ForSchedule"))
                objRead.Id = GetInteger(sqlRdr("Id"))
                objRead.WeekDay = GetInteger(sqlRdr("WeekDay"))
                objRead.Mont = GetInteger(sqlRdr("Mont"))
                objRead.Day = sqlRdr("Day").ToString()
                objRead.EachDay = sqlRdr("EachDay").ToString()
                objRead.OnceDate = sqlRdr("OnceDate").ToString()
                objRead.Time = sqlRdr("Time").ToString()
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
    Public Function ReadAllSchedule() As System.Collections.Generic.List(Of IeZSchedule)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZSchedule)()
        Dim objItem As IeZSchedule

        Try
            Dim strQry As String = ""
            strQry = "Select ScheduleId From eZSchedule where Isdeleted=0 order by ScheduleTypeId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Schedule.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZSchedule(GetInteger(sqlRdr("ScheduleId")))
                objItem.ScheduleId = GetInteger(sqlRdr("ScheduleId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    'udaya
    Public Function ReadSelectedeZSchedulebyfunction(ByVal Forschedule As Integer) As System.Collections.Generic.List(Of IeZSchedule)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZSchedule)()
        Dim objItem As IeZSchedule
        Dim ds As New DataSet
        Dim param As String() = {Forschedule.ToString()}
        Try
            ds = DBLayer.DBLInstance.GetDatasetByStoredProcedureName("SP_ScheduleListByFunction", param)
            If ds.Tables.Count <> 0 Then
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1 Step 1
                    objItem = GlobalInstance.eZSchedule(GetSmallInterger(ds.Tables(0).Rows(i).Item(0).ToString()))
                    objItem.ScheduleId = GetSmallInterger(ds.Tables(0).Rows(i).Item(0).ToString())
                    lstItems.Add(objItem)
                Next
            End If
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZSchedule(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZSchedule)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZSchedule)()
        Dim objItem As IeZSchedule
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ScheduleId From eZSchedule where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ScheduleId"

            Else
                strQry = "Select ScheduleId From eZSchedule where Isdeleted=0 order by ScheduleId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZECMUserInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZSchedule(GetSmallInterger(sqlRdr("ScheduleId")))
                objItem.ScheduleId = GetSmallInterger(sqlRdr("ScheduleId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZSchedule)
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
        If objToUpdate.ScheduleTypeId = 5 Then
            strQry = "Update eZSchedule Set ScheduleTypeId=@ScheduleTypeId,ForSchedule=@ForSchedule,Id=@Id,EachDay=@EachDay,WeekDay=@WeekDay,Mont=@Mont,Day=@Day,Time=@Time,OnceDate=@OnceDate,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ScheduleId=@ScheduleId"
            objParam = New SqlParameter(11) {}
            param = New SqlParameter("@ScheduleTypeId", objToUpdate.ScheduleTypeId)
            objParam(0) = param
            param = New SqlParameter("@WeekDay", objToUpdate.WeekDay)
            objParam(1) = param
            param = New SqlParameter("@Mont", objToUpdate.Mont)
            objParam(2) = param
            param = New SqlParameter("@Day", objToUpdate.Day)
            objParam(3) = param
            param = New SqlParameter("@Time", objToUpdate.Time)
            objParam(4) = param
            param = New SqlParameter("@OnceDate", objToUpdate.OnceDate)
            objParam(5) = param
            param = New SqlParameter("@ScheduleId", objToUpdate.ScheduleId)
            objParam(6) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(7) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(8) = param
            param = New SqlParameter("@ForSchedule", objToUpdate.ForSchedule)
            objParam(9) = param
            param = New SqlParameter("@Id", objToUpdate.Id)
            objParam(10) = param
            param = New SqlParameter("@EachDay", objToUpdate.EachDay)
            objParam(11) = param
        Else
            strQry = "Update eZSchedule Set ScheduleTypeId=@ScheduleTypeId,ForSchedule=@ForSchedule,Id=@Id,EachDay=@EachDay,WeekDay=@WeekDay,Mont=@Mont,Day=@Day,Time=@Time,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ScheduleId=@ScheduleId"
            objParam = New SqlParameter(10) {}
            param = New SqlParameter("@ScheduleTypeId", objToUpdate.ScheduleTypeId)
            objParam(0) = param
            param = New SqlParameter("@WeekDay", objToUpdate.WeekDay)
            objParam(1) = param
            param = New SqlParameter("@Mont", objToUpdate.Mont)
            objParam(2) = param
            param = New SqlParameter("@Day", objToUpdate.Day)
            objParam(3) = param
            param = New SqlParameter("@Time", objToUpdate.Time)
            objParam(4) = param
         
            param = New SqlParameter("@ScheduleId", objToUpdate.ScheduleId)
            objParam(5) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(6) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(7) = param
            param = New SqlParameter("@ForSchedule", objToUpdate.ForSchedule)
            objParam(8) = param
            param = New SqlParameter("@Id", objToUpdate.Id)
            objParam(9) = param
            param = New SqlParameter("@EachDay", objToUpdate.EachDay)
            objParam(10) = param
        End If
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZSchedule)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZSchedule set Isdeleted=1 where ScheduleId=@Schedule_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Schedule_ID", objToDelete.ScheduleId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class