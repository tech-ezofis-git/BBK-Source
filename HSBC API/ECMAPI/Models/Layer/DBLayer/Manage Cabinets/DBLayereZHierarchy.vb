Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZHierarchy)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZHierarchy ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Hierarchy_id=@Hierarchy_id and ez.Isdeleted=0"
            param = New SqlParameter("@Hierarchy_id", objRead.Hierarchy_id)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZHierarchy")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Hierarchy_id = GetInteger(sqlRdr("Hierarchy_id"))
                objRead.ToLevelId = GetInteger(sqlRdr("ToLevelId"))
                objRead.FromLevelId = GetInteger(sqlRdr("FromLevelId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
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
    Public Function CreateeZHierarchy(objEmp As eZHierarchy) As eZHierarchy
        Dim newObject As eZHierarchy = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZHierarchy(TemplateId,FromLevelId,ToLevelId,CreatedBy,CreatedOn) VALUES " +
                "(@TemplateId,@FromLevelId,@ToLevelId,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(0) = param
            param = New SqlParameter("@FromLevelId", objEmp.FromLevelId)
            objParam(1) = param
            param = New SqlParameter("@ToLevelId", objEmp.ToLevelId)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(4) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZHierarchy(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZHierarchy)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZHierarchy Set TemplateId=@TemplateId,FromLevelId=@FromLevelId,ToLevelId=@ToLevelId," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where Hierarchy_id=@Hierarchy_id"
        objParam = New SqlParameter(5) {}
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(0) = param
        param = New SqlParameter("@FromLevelId", objToUpdate.FromLevelId)
        objParam(1) = param
        param = New SqlParameter("@ToLevelId", objToUpdate.ToLevelId)
        objParam(2) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@Hierarchy_id", objToUpdate.Hierarchy_id)
        objParam(5) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZHierarchy)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZHierarchy set Isdeleted=1 where Hierarchy_id=@Hierarchy_id "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Hierarchy_id", objToDelete.Hierarchy_id)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZHierarchy() As System.Collections.Generic.List(Of IeZHierarchy)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZHierarchy)()
        Dim objItem As IeZHierarchy
        Try
            Dim strQry As String = ""
            strQry = "Select Hierarchy_id From eZHierarchy where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZHierarchy")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZHierarchy(GetInteger(sqlRdr("Hierarchy_id")))
                objItem.Hierarchy_id = GetInteger(sqlRdr("Hierarchy_id"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZHierarchy(Criteria As String, Value As String) As List(Of IeZHierarchy)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZHierarchy)()
        Dim objItem As IeZHierarchy
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Hierarchy_id From eZHierarchy where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Hierarchy_id"
            Else
                strQry = "Select Hierarchy_id From eZHierarchy where Isdeleted=0 order by Hierarchy_id"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZHierarchy")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZHierarchy(GetInteger(sqlRdr("Hierarchy_id")))
                objItem.Hierarchy_id = GetInteger(sqlRdr("Hierarchy_id"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZHierarchy(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZHierarchy)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZHierarchy)()
        Dim objItem As IeZHierarchy
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Hierarchy_id From eZHierarchy where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Hierarchy_id"
            Else
                strQry = "Select Hierarchy_id From eZHierarchy where Isdeleted=0 order by Hierarchy_id"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZHierarchy")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZHierarchy(GetInteger(sqlRdr("Hierarchy_id")))
                objItem.Hierarchy_id = GetInteger(sqlRdr("Hierarchy_id"))
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
