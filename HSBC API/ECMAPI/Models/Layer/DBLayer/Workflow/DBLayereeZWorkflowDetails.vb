Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common


Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZWorkflowDetails)
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
            strQry = "Select ers.ERSDirPath+item.ifilepath+item.ifilename as ifilepath,ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZWorkflowDetails ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid left join eZCA_1_4_items item on ez.WorkflowItemId =item.itemid left join eZERSInfo ers on item.ERSId=ers.ERSId " +
                "Where ez.Workflowid=@Workflowid and ez.Isdeleted=0"
            param = New SqlParameter("@Workflowid", objRead.Workflowid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWorkflowDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Workflowid = GetInteger(sqlRdr("Workflowid"))
                objRead.Workflowitemid = GetInteger(sqlRdr("Workflowitemid"))
                objRead.Status = sqlRdr("Status").ToString()
                objRead.XMLDS = New System.IO.StreamReader(sqlRdr("ifilepath").ToString()).ReadToEnd()
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.MailSettingsId = GetInteger(sqlRdr("MailSettingsId"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.WorkflowName = sqlRdr("WorkflowName").ToString()

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
    Public Function CreateeZWorkflowDetails(objEmp As eZWorkflowDetails) As eZWorkflowDetails
        Dim newObject As eZWorkflowDetails = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZWorkflowDetails(Workflowitemid,Status,WorkflowName,MailSettingsId,CreatedBy,CreatedOn) VALUES " +
                "(@Workflowitemid,@Status,@WorkflowName,@MailSettingsId,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@Workflowitemid", objEmp.Workflowitemid)
            objParam(0) = param
            param = New SqlParameter("@Status", objEmp.Status)
            objParam(1) = param
            param = New SqlParameter("@WorkflowName", objEmp.WorkflowName)
            objParam(2) = param
            param = New SqlParameter("@MailSettingsId", objEmp.MailSettingsId)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(4) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(5) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZWorkflowDetails(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZWorkflowDetails)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZWorkflowDetails Set Workflowitemid=@Workflowitemid,Status=@Status,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn," +
            "WorkflowName=@WorkflowName,MailSettingsId=@MailSettingsId where Workflowid=@Workflowid"
        objParam = New SqlParameter(6) {}
        param = New SqlParameter("@Workflowitemid", objToUpdate.Workflowitemid)
        objParam(0) = param
        param = New SqlParameter("@Status", objToUpdate.Status)
        objParam(1) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(2) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(3) = param
        param = New SqlParameter("@WorkflowName", objToUpdate.WorkflowName)
        objParam(4) = param
        param = New SqlParameter("@MailSettingsId", objToUpdate.MailSettingsId)
        objParam(5) = param
        param = New SqlParameter("@Workflowid", objToUpdate.Workflowid)
        objParam(6) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZWorkflowDetails)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZWorkflowDetails set Isdeleted=1 where Workflowid=@Workflowid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Workflowid", objToDelete.Workflowid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZWorkflowDetails() As System.Collections.Generic.List(Of IeZWorkflowDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkflowDetails)()
        Dim objItem As IeZWorkflowDetails
        Try
            Dim strQry As String = ""
            strQry = "Select Workflowid From eZWorkflowDetails where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWorkflowDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkflowDetails(GetInteger(sqlRdr("Workflowid")))
                objItem.Workflowid = GetInteger(sqlRdr("Workflowid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZWorkflowDetails(Criteria As String, Value As String) As List(Of IeZWorkflowDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkflowDetails)()
        Dim objItem As IeZWorkflowDetails
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Workflowid From eZWorkflowDetails where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Workflowid"
            Else
                strQry = "Select Workflowid From eZWorkflowDetails where Isdeleted=0 order by Workflowid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWorkflowDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkflowDetails(GetInteger(sqlRdr("Workflowid")))
                objItem.Workflowid = GetInteger(sqlRdr("Workflowid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZWorkflowDetails(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZWorkflowDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkflowDetails)()
        Dim objItem As IeZWorkflowDetails
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Workflowid From eZWorkflowDetails where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Workflowid"
            Else
                strQry = "Select Workflowid From eZWorkflowDetails where Isdeleted=0 order by Workflowid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWorkflowDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkflowDetails(GetInteger(sqlRdr("Workflowid")))
                objItem.Workflowid = GetInteger(sqlRdr("Workflowid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadRunningWorkflowDetailsByLoginId(ecmloginid As String) As System.Collections.Generic.List(Of IeZWorkflowDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkflowDetails)()
        Dim objItem As IeZWorkflowDetails
        Try
            Dim strQry As String = ""
            strQry = "select a.workflowid from ezworkflowdetails a left join ezca_1_4_items b on a.workflowitemid=b.itemid where a.status='Running' and b.isdeleted=0 and a.workflowid in (select workflowid from ezworkflowusers where ecmloginid='" + ecmloginid + "' or ECMLoginId=0) order by workflowid desc"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWorkflowDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkflowDetails(GetInteger(sqlRdr("Workflowid")))
                objItem.Workflowid = GetInteger(sqlRdr("Workflowid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function InsertandUpdateeZWorkflowDetails(ByVal OBJ As eZWorkflowDetails) As String
        Try
            ' Dim objval As IeZWorkflowDetails
            Dim param As String()
            If String.IsNullOrEmpty(OBJ.Updatedon) Then
                OBJ.Updatedon = "0"
            End If
            If String.IsNullOrEmpty(OBJ.Createdon) Then
                OBJ.Createdon = "0"
            End If
            If String.IsNullOrEmpty(OBJ.Status) Then
                OBJ.Status = "0"
            End If
            param = {OBJ.Workflowid.ToString(), OBJ.Workflowitemid.ToString(), OBJ.Status.ToString(), OBJ.Createdon.ToString(), OBJ.Updatedon.ToString(), OBJ.Createdby.ToString(), OBJ.Updatedby.ToString()}
            Dim exc As String = DBLayer.DBLInstance.InsertandUpdateStoredProcedure("SP_InsertandUpdateeZWorkflowDetails", param)
            Return "1"
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
End Class
