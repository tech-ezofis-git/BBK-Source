Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZMailWatchingDetails)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZMailWatchingDetails ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.sendid=@sendid and ez.Isdeleted=0"
            param = New SqlParameter("@sendid", objRead.sendid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.sendid = GetInteger(sqlRdr("sendid"))
                objRead.Conditionid = GetInteger(sqlRdr("conditionid"))
                objRead.Mailwatchingid = GetInteger(sqlRdr("Mailwatchingid"))
                objRead.keyword = sqlRdr("keyword").ToString
                objRead.Tomail = sqlRdr("Tomail").ToString
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
    Public Function CreateeZMailWatchingDetails(objEmp As eZMailWatchingDetails) As eZMailWatchingDetails
        Dim newObject As eZMailWatchingDetails = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZMailWatchingDetails(conditionid,Mailwatchingid,keyword,Tomail,CreatedBy,CreatedOn) VALUES " +
                "(@conditionid,@Mailwatchingid,@keyword,@Tomail,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@conditionid", objEmp.Conditionid)
            objParam(0) = param
            param = New SqlParameter("@Mailwatchingid", objEmp.Mailwatchingid)
            objParam(1) = param
            param = New SqlParameter("@keyword", objEmp.keyword)
            objParam(2) = param
            param = New SqlParameter("@Tomail", objEmp.Tomail)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(5) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZMailWatchingDetails(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZMailWatchingDetails)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailWatchingDetails Set Conditionid=@Conditionid,Mailwatchingid=@Mailwatchingid,keyword=@keyword,Tomail=@Tomail" +
            ",UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where sendid=@sendid"
        objParam = New SqlParameter(6) {}
        param = New SqlParameter("@conditionid", objToUpdate.Conditionid)
        objParam(0) = param
        param = New SqlParameter("@Mailwatchingid", objToUpdate.Mailwatchingid)
        objParam(1) = param
        param = New SqlParameter("@keyword", objToUpdate.keyword)
        objParam(2) = param
        param = New SqlParameter("@Tomail", objToUpdate.Tomail)
        objParam(3) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(4) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(5) = param
        param = New SqlParameter("@sendid", objToUpdate.sendid)
        objParam(6) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZMailWatchingDetails)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailWatchingDetails set Isdeleted=1 where sendid=@sendid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@sendid", objToDelete.sendid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZMailWatchingDetails() As System.Collections.Generic.List(Of IeZMailWatchingDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatchingDetails)()
        Dim objItem As IeZMailWatchingDetails
        Try
            Dim strQry As String = ""
            strQry = "Select sendid From eZMailWatchingDetails where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatchingDetails(GetInteger(sqlRdr("sendid")))
                objItem.sendid = GetInteger(sqlRdr("sendid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZMailWatchingDetails(Criteria As String, Value As String) As List(Of IeZMailWatchingDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatchingDetails)()
        Dim objItem As IeZMailWatchingDetails
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select sendid From eZMailWatchingDetails where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by sendid"
            Else
                strQry = "Select sendid From eZMailWatchingDetails where Isdeleted=0 order by sendid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatchingDetails(GetInteger(sqlRdr("sendid")))
                objItem.sendid = GetInteger(sqlRdr("sendid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMailWatchingDetails(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailWatchingDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatchingDetails)()
        Dim objItem As IeZMailWatchingDetails
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select sendid From eZMailWatchingDetails where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by sendid"
            Else
                strQry = "Select sendid From eZMailWatchingDetails where Isdeleted=0 order by sendid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingDetails")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatchingDetails(GetInteger(sqlRdr("sendid")))
                objItem.sendid = GetInteger(sqlRdr("sendid"))
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
