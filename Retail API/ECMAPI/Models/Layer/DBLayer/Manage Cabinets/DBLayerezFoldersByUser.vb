Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IezFoldersByUser)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From ezFoldersByUser ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.NodeId=@NodeId and ez.Isdeleted=0"
            param = New SqlParameter("@NodeId", objRead.NodeId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezFoldersByUser")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.NodeId = GetInteger(sqlRdr("NodeId"))
                objRead.NodeName = sqlRdr("NodeName").ToString
                objRead.UserId = GetInteger(sqlRdr("UserId"))
                objRead.ParentNodeId = GetInteger(sqlRdr("ParentNodeId"))
                objRead.PathId = GetInteger(sqlRdr("PathId"))
                objRead.LevelId = GetInteger(sqlRdr("LevelId"))
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
    Public Function CreateezFoldersByUsers(objEmp As ezFoldersByUser) As ezFoldersByUser
        Dim newObject As ezFoldersByUser = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezFoldersByUser(NodeName,PathId,ParentNodeId,LevelId,UserId,TemplateId,CreatedBy,CreatedOn) VALUES " +
                "(@NodeName,@PathId,@ParentNodeId,@LevelId,@UserId,@TemplateId,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@NodeName", objEmp.NodeName)
            objParam(0) = param
            param = New SqlParameter("@PathId", objEmp.PathId)
            objParam(1) = param
            param = New SqlParameter("@ParentNodeId", objEmp.ParentNodeId)
            objParam(2) = param
            param = New SqlParameter("@LevelId", objEmp.LevelId)
            objParam(3) = param
            param = New SqlParameter("@ToAdd", objEmp.UserId)
            objParam(4) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(5) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(6) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezFoldersByUser(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IezFoldersByUser)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezFoldersByUser Set NodeName=@NodeName,PathId=@PathId,ParentNodeId=@ParentNodeId,LevelId=@LevelId,UserId=@UserId," +
            "TemplateId=@TemplateId,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where NodeId=@NodeId"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@NodeName", objToUpdate.NodeName)
        objParam(0) = param
        param = New SqlParameter("@PathId", objToUpdate.PathId)
        objParam(1) = param
        param = New SqlParameter("@ParentNodeId", objToUpdate.ParentNodeId)
        objParam(2) = param
        param = New SqlParameter("@LevelId", objToUpdate.LevelId)
        objParam(3) = param
        param = New SqlParameter("@UserId", objToUpdate.UserId)
        objParam(4) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(5) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(6) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(7) = param
        param = New SqlParameter("@NodeId", objToUpdate.NodeId)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IezFoldersByUser)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezFoldersByUser set Isdeleted=1 where NodeId=@NodeId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@NodeId", objToDelete.NodeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllezFoldersByUser() As System.Collections.Generic.List(Of IezFoldersByUser)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezFoldersByUser)()
        Dim objItem As IezFoldersByUser
        Try
            Dim strQry As String = ""
            strQry = "Select NodeId From ezFoldersByUser where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezFoldersByUser")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezFoldersByUser(GetInteger(sqlRdr("NodeId")))
                objItem.NodeId = GetInteger(sqlRdr("NodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredezFoldersByUser(Criteria As String, Value As String) As List(Of IezFoldersByUser)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezFoldersByUser)()
        Dim objItem As IezFoldersByUser
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From ezFoldersByUser where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by NodeId"
            Else
                strQry = "Select NodeId From ezFoldersByUser where Isdeleted=0 order by NodeId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezFoldersByUser")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezFoldersByUser(GetInteger(sqlRdr("NodeId")))
                objItem.NodeId = GetInteger(sqlRdr("NodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedezFoldersByUser(Criteria As String, Value As String) As System.Collections.Generic.List(Of IezFoldersByUser)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezFoldersByUser)()
        Dim objItem As IezFoldersByUser
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From ezFoldersByUser where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeId"
            Else
                strQry = "Select NodeId From ezFoldersByUser where Isdeleted=0 order by NodeId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezFoldersByUser")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezFoldersByUser(GetInteger(sqlRdr("NodeId")))
                objItem.NodeId = GetInteger(sqlRdr("NodeId"))
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
