Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZWorkflowUsers)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From ezworkflowusers " +
                "Where workflowusersid=@workflowusersid and Isdeleted=0"
            param = New SqlParameter("@workflowusersid", objRead.WorkflowUsersId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Workflow Users")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.WorkflowUsersId = GetInteger(sqlRdr("workflowusersid"))
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                objRead.ECMGroupId = GetInteger(sqlRdr("ECMGroupId"))
                objRead.WorkflowId = GetInteger(sqlRdr("workflowid"))
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.AssignedFrom = sqlRdr("AssignedFrom").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
                objRead.FormId = GetInteger(sqlRdr("FormId"))
                objRead.UserType = sqlRdr("UserType").ToString
                objRead.Createdby1 = sqlRdr("CreatedBy1").ToString()
                objRead.Updatedby1 = sqlRdr("UpdatedBy1").ToString()
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

    Public Function CreateeZWorkflowUsers(objEmp As eZWorkflowUsers) As eZWorkflowUsers
        Dim newObject As eZWorkflowUsers = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZWorkflowUsers(ecmloginid,Workflowid,CreatedBy,CreatedOn,AssignedFrom,ECMGroupId,FormId,UserType) VALUES" +
                "(@ecmloginid,@Workflowid,@CreatedBy,@CreatedOn,@AssignedFrom,@ECMGroupId,@FormId,@UserType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(1) = param
            param = New SqlParameter("@ecmloginid", objEmp.ECMLoginId)
            objParam(2) = param
            param = New SqlParameter("@Workflowid", objEmp.WorkflowId)
            objParam(3) = param
            param = New SqlParameter("@AssignedFrom", objEmp.AssignedFrom)
            objParam(4) = param
            param = New SqlParameter("@ECMGroupId", objEmp.ECMGroupId)
            objParam(5) = param
            param = New SqlParameter("@FormId", objEmp.FormId)
            objParam(6) = param
            param = New SqlParameter("@UserType", objEmp.UserType)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZWorkflowUsers(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZWorkflowUsers)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezworkflowusers Set ecmloginid=@ecmloginid,Workflowid=@Workflowid,AssignedFrom=@AssignedFrom," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,ECMGroupId=@ECMGroupId,FormId=@FormId,UserType=@UserType where workflowusersid=@workflowusersid"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@Updatedby", objToUpdate.Updatedby)
        objParam(0) = param
        param = New SqlParameter("@Updatedon", objToUpdate.Updatedon)
        objParam(1) = param
        param = New SqlParameter("@Workflowid", objToUpdate.WorkflowId)
        objParam(2) = param
        param = New SqlParameter("@ecmloginid", objToUpdate.ECMLoginId)
        objParam(3) = param
        param = New SqlParameter("@workflowusersid", objToUpdate.WorkflowUsersId)
        objParam(4) = param
        param = New SqlParameter("@ECMGroupId", objToUpdate.ECMGroupId)
        objParam(5) = param
        param = New SqlParameter("@AssignedFrom", objToUpdate.AssignedFrom)
        objParam(6) = param
        param = New SqlParameter("@FormId", objToUpdate.FormId)
        objParam(7) = param
        param = New SqlParameter("@UserType", objToUpdate.UserType)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZWorkflowUsers)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezworkflowusers set Isdeleted=1 where workflowusersid=@workflowusersid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@workflowusersid", objToDelete.WorkflowUsersId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region

    Public Function ReadAlleZWorkflowUsers() As System.Collections.Generic.List(Of IeZWorkflowUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkflowUsers)()
        Dim objItem As IeZWorkflowUsers
        Try
            Dim strQry As String = ""
            strQry = "Select workflowusersid From eZWorkflowUsers where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Workflow Users")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkflowUsers(GetInteger(sqlRdr("workflowusersid")))
                objItem.WorkflowUsersId = GetInteger(sqlRdr("workflowusersid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZWorkflowUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZWorkflowUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkflowUsers)()
        Dim objItem As IeZWorkflowUsers
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select workflowusersid From eZWorkflowUsers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by workflowusersid"
            Else
                strQry = "Select workflowusersid From eZWorkflowUsers where Isdeleted=0 order by workflowusersid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Workflow Users")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkflowUsers(GetInteger(sqlRdr("workflowusersid")))
                objItem.WorkflowUsersId = GetInteger(sqlRdr("workflowusersid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZWorkflowUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZWorkflowUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkflowUsers)()
        Dim objItem As IeZWorkflowUsers
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select workflowusersid From eZWorkflowUsers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by workflowusersid"
            Else
                strQry = "Select workflowusersid From eZWorkflowUsers where Isdeleted=0 order by workflowusersid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Workflow Users")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkflowUsers(GetInteger(sqlRdr("workflowusersid")))
                objItem.WorkflowUsersId = GetInteger(sqlRdr("workflowusersid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadeZWorkflowUsersByCondition(condition As String) As System.Collections.Generic.List(Of IeZWorkflowUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWorkflowUsers)()
        Dim objItem As IeZWorkflowUsers
        Try
            Dim strQry As String = ""
            If condition <> "" Then
                strQry = "Select workflowusersid From eZWorkflowUsers where Isdeleted=0 and " + condition + " order by workflowusersid"
            Else
                strQry = "Select workflowusersid From eZWorkflowUsers where Isdeleted=0 order by workflowusersid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Workflow Users")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWorkflowUsers(GetInteger(sqlRdr("workflowusersid")))
                objItem.WorkflowUsersId = GetInteger(sqlRdr("workflowusersid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function DeleteeZWorkflowUsers(objToDelete As IeZWorkflowUsers)
        If objToDelete Is Nothing Then
            Return Nothing
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezworkflowusers set Isdeleted=1,Updatedon=@Updatedon,Updatedby=@Updatedby where workflowusersid=@workflowusersid "
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@workflowusersid", objToDelete.WorkflowUsersId)
        objParam(0) = param
        param = New SqlParameter("@Updatedon", objToDelete.Updatedon)
        objParam(1) = param
        param = New SqlParameter("@Updatedby", objToDelete.Updatedby)
        objParam(2) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Function
End Class
