Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateReminder(objEmp As eZReminder) As IeZReminder
        Dim newObject As IeZReminder = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZReminder(StartTime,EndTime,Subject,Reminder,DefaultId,CreatedOn,CreatedBy) VALUES(@StartTime,@EndTime,@Subject,@Reminder,@DefaultId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@StartTime", objEmp.StartTime)
            objParam(0) = param
            param = New SqlParameter("@EndTime", objEmp.EndTime)
            objParam(1) = param
            param = New SqlParameter("@Subject", objEmp.Subject)
            objParam(2) = param
            param = New SqlParameter("@Reminder", objEmp.Reminder)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            param = New SqlParameter("@DefaultId", objEmp.DefaultId)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(6) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZReminder(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZReminder)
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
            'If objRead.ReminderId = 0 Then
            '    strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZReminder Where DefaultId=@DefaultId and Isdeleted=0"
            '    param = New SqlParameter("@DefaultId", objRead.DefaultId)
            '    objParam(0) = param
            'Else
            objParam = New SqlParameter(0) {}
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZReminder Where ReminderId=@ReminderId and Isdeleted=0"
            param = New SqlParameter("@ReminderId", objRead.ReminderId)
            objParam(0) = param
            'End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Reminder.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ReminderId = GetInteger(sqlRdr("ReminderId"))
                objRead.StartTime = sqlRdr("StartTime")
                objRead.EndTime = sqlRdr("EndTime")
                objRead.Subject = sqlRdr("Subject")
                objRead.Reminder = sqlRdr("Reminder")
                objRead.DefaultId = sqlRdr("DefaultId")
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
    Public Function ReadAllReminder() As System.Collections.Generic.List(Of IeZReminder)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZReminder)()
        Dim objItem As IeZReminder
        Try
            Dim strQry As String = ""
            strQry = "Select ReminderId From eZReminder where Isdeleted=0 order by Reminder"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Reminder.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZReminder(GetInteger(sqlRdr("ReminderId")))
                objItem.ReminderId = GetInteger(sqlRdr("ReminderId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZReminder(Criteria As String, Value As String, Defaultid As Integer) As System.Collections.Generic.List(Of IeZReminder)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZReminder)()
        Dim objItem As IeZReminder
        Try
            Dim strQry As String = ""
            If Defaultid = 0 Then
                If Criteria <> "All" Then
                    strQry = "Select ReminderId From eZReminder where Isdeleted=0 and "
                    strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                    strQry = strQry & " =N'"
                    strQry = strQry & Unquote(Value)
                    strQry = strQry & "' "
                    strQry = strQry & " order by ReminderId"
                Else
                    strQry = "Select ReminderId From eZReminder where Isdeleted=0 order by ReminderId"
                End If
            Else
                strQry = "Select ReminderId From eZReminder where Isdeleted=0 and Defaultid='' and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ReminderId"
            End If

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZECMUserInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZReminder(GetSmallInterger(sqlRdr("ReminderId")))
                objItem.ReminderId = GetSmallInterger(sqlRdr("ReminderId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZReminderByfield(ByVal Subject As String, ByVal StartTime As String, ByVal EndTime As String) As System.Collections.Generic.List(Of IeZReminder)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZReminder)()
        Dim objItem As IeZReminder
        Try
            Dim strQry As String = ""
            strQry = "Select ReminderId From eZReminder where Isdeleted=0 and Subject=N'" + Subject + "' and StartTime=N'" + StartTime + "' and EndTime=N'" + EndTime + "' "
            strQry = strQry & " order by ReminderId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZECMUserInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZReminder(GetSmallInterger(sqlRdr("ReminderId")))
                objItem.ReminderId = GetSmallInterger(sqlRdr("ReminderId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZReminder)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        
        strQry = "Update eZReminder Set Reminder=@Reminder,StartTime=@StartTime,EndTime=@EndTime,Subject=@Subject,DefaultId=@DefaultId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ReminderId=@ReminderId"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@Reminder", objToUpdate.Reminder)
        objParam(0) = param
        param = New SqlParameter("@StartTime", objToUpdate.StartTime)
        objParam(1) = param
        param = New SqlParameter("@EndTime", objToUpdate.EndTime)
        objParam(2) = param
        param = New SqlParameter("@Subject", objToUpdate.Subject)
        objParam(3) = param
        param = New SqlParameter("@DefaultId", objToUpdate.DefaultId)
        objParam(4) = param
        param = New SqlParameter("@ReminderId", objToUpdate.ReminderId)
        objParam(5) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(6) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(7) = param
       
       
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZReminder)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZReminder set Isdeleted=1 where ReminderId=@Reminder_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Reminder_ID", objToDelete.ReminderId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class