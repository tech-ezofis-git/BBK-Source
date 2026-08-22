Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As Iezlookupsynchistory)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1,eze.loginname as Loginname From ezlookupsynchistory ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "left join ezecmlogin eze on ez.ecmloginid=eze.ecmloginid Where ez.synchistoryid=@synchistoryid and ez.Isdeleted=0"
            param = New SqlParameter("@synchistoryid", objRead.synchistoryid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezlookupsynchistory")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.synchistoryid = GetInteger(sqlRdr("synchistoryid"))
                objRead.lookupid = GetInteger(sqlRdr("lookupid"))
                objRead.query = sqlRdr("query").ToString()
                objRead.application = sqlRdr("application").ToString
                objRead.ecmloginid = GetInteger(sqlRdr("ecmloginid"))
                objRead.result = sqlRdr("result").ToString()
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.Loginname = sqlRdr("Loginname").ToString()
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
    Public Function Createezlookupsynchistory(objEmp As ezlookupsynchistory) As ezlookupsynchistory
        Dim newObject As ezlookupsynchistory = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezlookupsynchistory(lookupid,query,application,ecmloginid,result,CreatedBy,CreatedOn) VALUES " +
                "(@lookupid,@query,@application,@ecmloginid,@result,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@lookupid", objEmp.lookupid)
            objParam(0) = param
            param = New SqlParameter("@query", objEmp.query)
            objParam(1) = param
            param = New SqlParameter("@application", objEmp.application)
            objParam(2) = param
            param = New SqlParameter("@ecmloginid", objEmp.ecmloginid)
            objParam(3) = param
            param = New SqlParameter("@result", objEmp.result)
            objParam(4) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(6) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezlookupsynchistory(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As Iezlookupsynchistory)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezlookupsynchistory Set lookupid=@lookupid,query=@query,application=@application,ecmloginid=@ecmloginid," +
            "result=@result,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where synchistoryid=@synchistoryid"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@lookupid", objToUpdate.lookupid)
        objParam(0) = param
        param = New SqlParameter("@query", objToUpdate.query)
        objParam(1) = param
        param = New SqlParameter("@application", objToUpdate.application)
        objParam(2) = param
        param = New SqlParameter("@ecmloginid", objToUpdate.ecmloginid)
        objParam(3) = param
        param = New SqlParameter("@result", objToUpdate.result)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(5) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(6) = param
        param = New SqlParameter("@synchistoryid", objToUpdate.synchistoryid)
        objParam(7) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As Iezlookupsynchistory)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezlookupsynchistory set Isdeleted=1 where synchistoryid=@synchistoryid"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@synchistoryid", objToDelete.synchistoryid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllezlookupsynchistory() As System.Collections.Generic.List(Of Iezlookupsynchistory)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of Iezlookupsynchistory)()
        Dim objItem As Iezlookupsynchistory
        Try
            Dim strQry As String = ""
            strQry = "Select synchistoryid From ezlookupsynchistory where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezlookupsynchistory")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezlookupsynchistory(GetInteger(sqlRdr("synchistoryid")))
                objItem.synchistoryid = GetInteger(sqlRdr("synchistoryid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredezlookupsynchistory(Criteria As String, Value As String) As List(Of Iezlookupsynchistory)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of Iezlookupsynchistory)()
        Dim objItem As Iezlookupsynchistory
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select synchistoryid From ezlookupsynchistory where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by synchistoryid"
            Else
                strQry = "Select synchistoryid From ezlookupsynchistory where Isdeleted=0 order by synchistoryid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezlookupsynchistory")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezlookupsynchistory(GetInteger(sqlRdr("synchistoryid")))
                objItem.synchistoryid = GetInteger(sqlRdr("synchistoryid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedezlookupsynchistory(Criteria As String, Value As String) As System.Collections.Generic.List(Of Iezlookupsynchistory)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of Iezlookupsynchistory)()
        Dim objItem As Iezlookupsynchistory
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select synchistoryid From ezlookupsynchistory where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by synchistoryid"
            Else
                strQry = "Select synchistoryid From ezlookupsynchistory where Isdeleted=0 order by synchistoryid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezlookupsynchistory")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezlookupsynchistory(GetInteger(sqlRdr("synchistoryid")))
                objItem.synchistoryid = GetInteger(sqlRdr("synchistoryid"))
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
