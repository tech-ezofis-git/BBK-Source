Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZMailTriggering)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZMailTriggering ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.MailTriggerid=@MailTriggerid and ez.Isdeleted=0"
            param = New SqlParameter("@MailTriggerid", objRead.MailTriggerid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailTriggering")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.MailTriggerid = GetInteger(sqlRdr("MailTriggerid"))
                objRead.TriggerTypeId = GetInteger(sqlRdr("TriggerTypeId"))
                objRead.Condition = sqlRdr("Condition").ToString
                objRead.Status = GetBoolean(sqlRdr("Status"))
                objRead.MailSettingId = GetInteger(sqlRdr("MailSettingId"))
                objRead.TempWFId = GetInteger(sqlRdr("TempWFId"))
                objRead.UnallocatedMailUser = GetInteger(sqlRdr("UnallocatedMailUser"))
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
    Public Function CreateeZMailTriggering(objEmp As eZMailTriggering) As eZMailTriggering
        Dim newObject As eZMailTriggering = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZMailTriggering(Status,TriggerTypeId,MailSettingId,Condition,TempWFId,UnallocatedMailUser,CreatedBy,CreatedOn) VALUES " +
                "(@Status,@TriggerTypeId,@MailSettingId,@Condition,@TempWFId,@UnallocatedMailUser,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@Status", objEmp.Status)
            objParam(0) = param
            param = New SqlParameter("@TriggerTypeId", objEmp.TriggerTypeId)
            objParam(1) = param
            param = New SqlParameter("@MailSettingId", objEmp.MailSettingId)
            objParam(2) = param
            param = New SqlParameter("@Condition", objEmp.Condition)
            objParam(3) = param
            param = New SqlParameter("@TempWFId", objEmp.TempWFId)
            objParam(4) = param
            param = New SqlParameter("@UnallocatedMailUser", objEmp.UnallocatedMailUser)
            objParam(5) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(6) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZMailTriggering(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZMailTriggering)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailTriggering Set Status=@Status,TriggerTypeId=@TriggerTypeId,MailSettingId=@MailSettingId,Condition=@Condition," +
            "TempWFId=@TempWFId,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,UnallocatedMailUser=@UnallocatedMailUser where MailTriggerid=@MailTriggerid"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@Status", objToUpdate.Status)
        objParam(0) = param
        param = New SqlParameter("@TriggerTypeId", objToUpdate.TriggerTypeId)
        objParam(1) = param
        param = New SqlParameter("@MailSettingId", objToUpdate.MailSettingId)
        objParam(2) = param
        param = New SqlParameter("@Condition", objToUpdate.Condition)
        objParam(3) = param
        param = New SqlParameter("@TempWFId", objToUpdate.TempWFId)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(6) = param
        param = New SqlParameter("@UnallocatedMailUser", objToUpdate.UnallocatedMailUser)
        objParam(7) = param
        param = New SqlParameter("@MailTriggerid", objToUpdate.MailTriggerid)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZMailTriggering)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailTriggering set Isdeleted=1 where MailTriggerid=@MailTriggerid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@MailTriggerid", objToDelete.MailTriggerid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZMailTriggering() As System.Collections.Generic.List(Of IeZMailTriggering)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailTriggering)()
        Dim objItem As IeZMailTriggering
        Try
            Dim strQry As String = ""
            strQry = "Select MailTriggerid From eZMailTriggering where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailTriggering")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailTriggering(GetInteger(sqlRdr("MailTriggerid")))
                objItem.MailTriggerid = GetInteger(sqlRdr("MailTriggerid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZMailTriggering(Criteria As String, Value As String) As List(Of IeZMailTriggering)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailTriggering)()
        Dim objItem As IeZMailTriggering
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailTriggerid From eZMailTriggering where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by MailTriggerid"
            Else
                strQry = "Select MailTriggerid From eZMailTriggering where Isdeleted=0 order by MailTriggerid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailTriggering")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailTriggering(GetInteger(sqlRdr("MailTriggerid")))
                objItem.MailTriggerid = GetInteger(sqlRdr("MailTriggerid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMailTriggering(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailTriggering)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailTriggering)()
        Dim objItem As IeZMailTriggering
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailTriggerid From eZMailTriggering where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by MailTriggerid"
            Else
                strQry = "Select MailTriggerid From eZMailTriggering where Isdeleted=0 order by MailTriggerid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailTriggering")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailTriggering(GetInteger(sqlRdr("MailTriggerid")))
                objItem.MailTriggerid = GetInteger(sqlRdr("MailTriggerid"))
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
