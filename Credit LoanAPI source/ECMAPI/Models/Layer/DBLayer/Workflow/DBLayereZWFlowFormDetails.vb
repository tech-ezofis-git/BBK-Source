Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZWFlowFormDetails)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZWFlowFormDetails ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.FormDetailsId=@FormDetailsId and ez.Isdeleted=0"
            param = New SqlParameter("@FormDetailsId", objRead.FormDetailsId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowFormDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.FormDetailsId = GetInteger(sqlRdr("FormDetailsId"))
                objRead.workflowid = GetInteger(sqlRdr("workflowid"))
                objRead.parentformid = GetInteger(sqlRdr("parentformid"))
                objRead.formid = GetInteger(sqlRdr("formid"))
                objRead.tablename = sqlRdr("tablename").ToString
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
    Public Function CreateeZWFlowFormDetails(objEmp As eZWFlowFormDetails) As eZWFlowFormDetails
        Dim newObject As eZWFlowFormDetails = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZWFlowFormDetails(formid,parentformid,workflowid,tablename,CreatedBy,CreatedOn) VALUES " +
                "(@formid,@parentformid,@workflowid,@tablename,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@formid", objEmp.formid)
            objParam(0) = param
            param = New SqlParameter("@parentformid", objEmp.parentformid)
            objParam(1) = param
            param = New SqlParameter("@workflowid", objEmp.workflowid)
            objParam(2) = param
            param = New SqlParameter("@tablename", objEmp.tablename)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(5) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZWFlowFormDetails(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZWFlowFormDetails)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZWFlowFormDetails Set formid=@formid,parentformid=@parentformid,workflowid=@workflowid," +
            "tablename=@tablename,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where FormDetailsId=@FormDetailsId"
        objParam = New SqlParameter(6) {}
        param = New SqlParameter("@formid", objToUpdate.formid)
        objParam(0) = param
        param = New SqlParameter("@parentformid", objToUpdate.parentformid)
        objParam(1) = param
        param = New SqlParameter("@workflowid", objToUpdate.workflowid)
        objParam(2) = param
        param = New SqlParameter("@tablename", objToUpdate.tablename)
        objParam(3) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(4) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(5) = param
        param = New SqlParameter("@FormDetailsId", objToUpdate.FormDetailsId)
        objParam(6) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZWFlowFormDetails)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZWFlowFormDetails set Isdeleted=1 where FormDetailsId=@FormDetailsId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@FormDetailsId", objToDelete.FormDetailsId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZWFlowFormDetails() As System.Collections.Generic.List(Of IeZWFlowFormDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFlowFormDetails)()
        Dim objItem As IeZWFlowFormDetails
        Try
            Dim strQry As String = ""
            strQry = "Select FormDetailsId From eZWFlowFormDetails where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowFormDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFlowFormDetails(GetInteger(sqlRdr("FormDetailsId")))
                objItem.FormDetailsId = GetInteger(sqlRdr("FormDetailsId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZWFlowFormDetails(Criteria As String, Value As String) As List(Of IeZWFlowFormDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFlowFormDetails)()
        Dim objItem As IeZWFlowFormDetails
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FormDetailsId From eZWFlowFormDetails where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by FormDetailsId"
            Else
                strQry = "Select FormDetailsId From eZWFlowFormDetails where Isdeleted=0 order by FormDetailsId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowFormDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFlowFormDetails(GetInteger(sqlRdr("FormDetailsId")))
                objItem.FormDetailsId = GetInteger(sqlRdr("FormDetailsId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZWFlowFormDetails(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZWFlowFormDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFlowFormDetails)()
        Dim objItem As IeZWFlowFormDetails
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FormDetailsId From eZWFlowFormDetails where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FormDetailsId"
            Else
                strQry = "Select FormDetailsId From eZWFlowFormDetails where Isdeleted=0 order by FormDetailsId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowFormDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFlowFormDetails(GetInteger(sqlRdr("FormDetailsId")))
                objItem.FormDetailsId = GetInteger(sqlRdr("FormDetailsId"))
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
