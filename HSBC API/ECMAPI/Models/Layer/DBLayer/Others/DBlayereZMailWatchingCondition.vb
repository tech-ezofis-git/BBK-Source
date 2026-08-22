Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZMailWatchingCondition)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZMailWatchingCondition ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.conditionid=@conditionid and ez.Isdeleted=0"
            param = New SqlParameter("@conditionid", objRead.conditionid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingCondition")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.conditionid = GetInteger(sqlRdr("conditionid"))
                objRead.condition = sqlRdr("condition").ToString
                objRead.createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.createdon = sqlRdr("CreatedOn").ToString
                objRead.updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.updatedon = sqlRdr("UpdatedOn").ToString
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
    Public Function CreateeZMailWatchingCondition(objEmp As eZMailWatchingCondition) As eZMailWatchingCondition
        Dim newObject As eZMailWatchingCondition = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZMailWatchingCondition(condition,CreatedBy,CreatedOn) VALUES " +
                "(@condition,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@condition", objEmp.condition)
            objParam(0) = param
            param = New SqlParameter("@CreatedBy", objEmp.createdby)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.createdon)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZMailWatchingCondition(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZMailWatchingCondition)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailWatchingCondition Set condition=@condition,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where conditionid=@conditionid"
        objParam = New SqlParameter(3) {}
        param = New SqlParameter("@condition", objToUpdate.condition)
        objParam(0) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.updatedby)
        objParam(1) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.updatedon)
        objParam(2) = param
        param = New SqlParameter("@conditionid", objToUpdate.conditionid)
        objParam(3) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZMailWatchingCondition)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailWatchingCondition set Isdeleted=1 where conditionid=@conditionid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@conditionid", objToDelete.conditionid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZMailWatchingCondition() As System.Collections.Generic.List(Of IeZMailWatchingCondition)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatchingCondition)()
        Dim objItem As IeZMailWatchingCondition
        Try
            Dim strQry As String = ""
            strQry = "Select conditionid From eZMailWatchingCondition where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingCondition")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatchingCondition(GetInteger(sqlRdr("conditionid")))
                objItem.conditionid = GetInteger(sqlRdr("conditionid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZMailWatchingCondition(Criteria As String, Value As String) As List(Of IeZMailWatchingCondition)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatchingCondition)()
        Dim objItem As IeZMailWatchingCondition
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select conditionid From eZMailWatchingCondition where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by conditionid"
            Else
                strQry = "Select conditionid From eZMailWatchingCondition where Isdeleted=0 order by conditionid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingCondition")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatchingCondition(GetInteger(sqlRdr("conditionid")))
                objItem.conditionid = GetInteger(sqlRdr("conditionid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMailWatchingCondition(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailWatchingCondition)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatchingCondition)()
        Dim objItem As IeZMailWatchingCondition
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select conditionid From eZMailWatchingCondition where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by conditionid"
            Else
                strQry = "Select conditionid From eZMailWatchingCondition where Isdeleted=0 order by conditionid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingCondition")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatchingCondition(GetInteger(sqlRdr("conditionid")))
                objItem.conditionid = GetInteger(sqlRdr("conditionid"))
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
