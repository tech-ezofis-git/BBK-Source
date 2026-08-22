Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "Folder Details"

    Public Function CreateeZInbox(objtemp As eZInbox) As IeZInbox

        'SqlDbType.Money()


        Dim newObject As IeZInbox = Nothing
        If String.IsNullOrEmpty(objtemp.NodeName) Then
            Return Nothing
        End If
        objtemp.NodeName = objtemp.NodeName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select NodeId From eZInbox Where NodeName = @NodeName And LoginId=@LoginId And ParentNodeId=@ParentNodeId And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@NodeName", objtemp.NodeName)
            objParam(0) = param
            param = New SqlParameter("@LoginId", objtemp.LoginId)
            objParam(1) = param
            param = New SqlParameter("@ParentNodeId", objtemp.ParentNodeId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                ''  Throw New Exception("eZInbox Code already exist!")
            Else
                strQry = "INSERT INTO eZInbox(NodeName,ParentNodeId,LoginId,LevelId,PathId,CreatedOn,CreatedBy) VALUES(@NodeName,@ParentNodeId,@LoginId,@LevelId,@PathId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
                objParam = New SqlParameter(6) {}
                param = New SqlParameter("@NodeName", objtemp.NodeName)
                objParam(0) = param
                param = New SqlParameter("@ParentNodeId", objtemp.ParentNodeId)
                objParam(1) = param
                param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
                objParam(2) = param
                param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
                objParam(3) = param
                param = New SqlParameter("@LoginId", objtemp.LoginId)
                objParam(4) = param
                param = New SqlParameter("@LevelId", objtemp.LevelId)
                objParam(5) = param
                param = New SqlParameter("@PathId", objtemp.PathId)
                objParam(6) = param
                obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            End If

            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZInbox(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Function CreateeZInboxByUser(objtemp As eZInbox) As IeZInbox
        Dim newObject As IeZInbox = Nothing
        If String.IsNullOrEmpty(objtemp.NodeName) Then
            Return Nothing
        End If
        objtemp.NodeName = objtemp.NodeName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select NodeId From eZInboxByUser Where NodeId = @NodeId And LoginId=@LoginId And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@NodeId", objtemp.NodeId)
            objParam(0) = param
            param = New SqlParameter("@LoginId", objtemp.LoginId)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                strQry = "Update eZInboxByUser Set NodeName=@NodeName,LevelId=@LevelId,PathId=@PathId,LoginId=@LoginId,ParentNodeId=@ParentNodeId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where NodeId=@NodeId"
                objParam = New SqlParameter(7) {}
                param = New SqlParameter("@NodeName", objtemp.NodeName)
                objParam(0) = param
                param = New SqlParameter("@LoginId", objtemp.LoginId)
                objParam(1) = param
                param = New SqlParameter("@ParentNodeId", objtemp.ParentNodeId)
                objParam(2) = param
                param = New SqlParameter("@NodeId", objtemp.NodeId)
                objParam(3) = param
                param = New SqlParameter("@UpdatedOn", objtemp.UpdatedOn)
                objParam(4) = param
                param = New SqlParameter("@UpdatedBy", objtemp.UpdatedBy)
                objParam(5) = param
                param = New SqlParameter("@LevelId", objtemp.LevelId)
                objParam(6) = param
                param = New SqlParameter("@PathId", objtemp.PathId)
                objParam(7) = param
                If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                    Return Nothing
                Else
                    newObject = GlobalInstance.eZInbox(objtemp.NodeId)
                    Read(newObject)
                    Return newObject
                End If
            Else
                strQry = "INSERT INTO eZInboxByUser(NodeId,NodeName,ParentNodeId,LoginId,LevelId,PathId,CreatedOn,CreatedBy,UserId) VALUES(@NodeId,@NodeName,@ParentNodeId,@LoginId,@LevelId,@PathId,@CreatedOn,@CreatedBy,@UserId);Select SCOPE_IDENTITY();"
                objParam = New SqlParameter(7) {}
                param = New SqlParameter("@NodeName", objtemp.NodeName)
                objParam(0) = param
                param = New SqlParameter("@ParentNodeId", objtemp.ParentNodeId)
                objParam(1) = param
                param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
                objParam(2) = param
                param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
                objParam(3) = param
                param = New SqlParameter("@LoginId", objtemp.LoginId)
                objParam(4) = param
                param = New SqlParameter("@NodeId", objtemp.NodeId)
                objParam(5) = param
                param = New SqlParameter("@PathId", objtemp.PathId)
                objParam(6) = param
                param = New SqlParameter("@LevelId", objtemp.LevelId)
                objParam(7) = param
               
                obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
                newObject = GlobalInstance.eZInbox(objtemp.NodeId)
                Read(newObject)
                Return newObject
            End If

            'If obj Is Nothing Then
            '    Return Nothing
            'End If


        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZInbox)
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
            If objRead.NodeName Is Nothing Then
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(LoginId) as LoginName,dbo.udf_UserName(CreatedBy)as CreatedBy1  From eZInbox  Where Isdeleted=0  and NodeId=@NodeId"
                param = New SqlParameter("@NodeId", objRead.NodeId)
                objParam(0) = param
            Else
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(LoginId) as LoginName,dbo.udf_UserName(CreatedBy)as CreatedBy1  From eZInbox  Where Isdeleted=0  and NodeName=@NodeName"
                param = New SqlParameter("@NodeName", objRead.NodeName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZInbox.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.NodeId = GetInteger(sqlRdr("NodeId"))
                objRead.NodeName = sqlRdr("NodeName").ToString()
                objRead.LoginName = sqlRdr("LoginName").ToString()
                objRead.LoginId = GetInteger(sqlRdr("LoginId"))
                objRead.LevelId = GetInteger(sqlRdr("LevelId"))
                objRead.PathId = sqlRdr("PathId").ToString()
                objRead.ParentNodeId = GetInteger(sqlRdr("ParentNodeId"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZInbox.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZInbox() As System.Collections.Generic.List(Of IeZInbox)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZInbox)()
        Dim objItem As IeZInbox
        Try
            Dim strQry As String = ""
            strQry = "Select NodeId From eZInbox where Isdeleted=0 order by NodeName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZInbox.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZInbox(GetSmallInterger(sqlRdr("NodeId")))
                objItem.NodeId = GetSmallInterger(sqlRdr("NodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZInbox(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZInbox)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZInbox)()
        Dim objItem As IeZInbox
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZInbox where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZInbox where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZInbox.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZInbox(GetSmallInterger(sqlRdr("NodeId")))
                objItem.NodeId = GetSmallInterger(sqlRdr("NodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZInbox(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZInbox)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZInbox)()
        Dim objItem As IeZInbox
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZInbox where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZInbox where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZInbox.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZInbox(GetSmallInterger(sqlRdr("NodeId")))
                objItem.NodeId = GetSmallInterger(sqlRdr("NodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZInboxWithLoginId(Criteria As String, Value As String, ByVal LoginId As Integer) As System.Collections.Generic.List(Of IeZInbox)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZInbox)()
        Dim objItem As IeZInbox
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZInbox where Isdeleted=0 and LoginId=" + LoginId.ToString() + " and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZInbox where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZInbox.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZInbox(GetSmallInterger(sqlRdr("NodeId")))
                objItem.NodeId = GetSmallInterger(sqlRdr("NodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZInboxWithPathAndLevel(PathId As String, LevelId As String, ByVal LoginId As Integer) As System.Collections.Generic.List(Of IeZInbox)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZInbox)()
        Dim objItem As IeZInbox
        Try
            Dim strQry As String = ""

            strQry = "Select NodeId From eZInbox where Isdeleted=0 and LoginId=" + LoginId.ToString() + " and "
            strQry = strQry & "PathId=N'" + PathId.ToString() + "' and "
            strQry = strQry & "LevelId=" + LevelId.ToString()
            strQry = strQry & " order by NodeName"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZInbox.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZInbox(GetSmallInterger(sqlRdr("NodeId")))
                objItem.NodeId = GetSmallInterger(sqlRdr("NodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZInboxWithLoginIdAndParentNodeId(Criteria As String, Value As String, ByVal LoginId As Integer, ByVal ParentNodeId As Integer) As System.Collections.Generic.List(Of IeZInbox)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZInbox)()
        Dim objItem As IeZInbox
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZInbox where Isdeleted=0 and LoginId=" + LoginId.ToString() + " and ParentNodeId=" + ParentNodeId.ToString() + " and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZInbox where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZInbox.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZInbox(GetSmallInterger(sqlRdr("NodeId")))
                objItem.NodeId = GetSmallInterger(sqlRdr("NodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZInbox)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select NodeId From eZInbox Where NodeName = @NodeName And LoginId=@LoginId And ParentNodeId=@ParentNodeId And Isdeleted=0 and NodeId <> @NodeId"
        objParam = New SqlParameter(3) {}
        param = New SqlParameter("@NodeName", objToUpdate.NodeName)
        objParam(0) = param
        param = New SqlParameter("@LoginId", objToUpdate.LoginId)
        objParam(1) = param
        param = New SqlParameter("@ParentNodeId", objToUpdate.ParentNodeId)
        objParam(2) = param
        param = New SqlParameter("@NodeId", objToUpdate.NodeId)
        objParam(3) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZInbox Code already exist!")
        Else
            strQry = "Update eZInbox Set NodeName=@NodeName,LevelId=@LevelId,PathId=@PathId,LoginId=@LoginId,ParentNodeId=@ParentNodeId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where NodeId=@NodeId"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@NodeName", objToUpdate.NodeName)
            objParam(0) = param
            param = New SqlParameter("@LoginId", objToUpdate.LoginId)
            objParam(1) = param
            param = New SqlParameter("@ParentNodeId", objToUpdate.ParentNodeId)
            objParam(2) = param
            param = New SqlParameter("@NodeId", objToUpdate.NodeId)
            objParam(3) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(4) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(5) = param
            param = New SqlParameter("@LevelId", objToUpdate.LevelId)
            objParam(6) = param
            param = New SqlParameter("@PathId", objToUpdate.PathId)
            objParam(7) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub

    'Public Function GetValForInboxTreeView(ByVal TempId As String, ByVal UserId As String) As System.Collections.Generic.List(Of IeZInbox)
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    Dim lstItems As New System.Collections.Generic.List(Of IeZInbox)()
    '    Dim objRead As IeZInbox
    '    Try
    '        Dim strQry As String = ""
    '        strQry = "select * from dbo.udf_TreeView(" + TempId + "," + UserId + ")"
    '        Dim obj As Object = ""
    '        obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid eZInbox.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        While sqlRdr.Read()
    '            objRead = GlobalInstance.eZInbox(GetSmallInterger(sqlRdr("NodeId")))
    '            objRead.NodeId = GetInteger(sqlRdr("NodeId"))
    '            objRead.NodeName = sqlRdr("NodeName").ToString()
    '            'objRead.PathId = sqlRdr("PathId").ToString()
    '            objRead.ParentNodeId = GetInteger(sqlRdr("ParentNodeId"))
    '            objRead.CreatedOn = sqlRdr("CreatedOn").ToString

    '            objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
    '            'objRead.LevelId = GetInteger(sqlRdr("LevelId"))
    '            lstItems.Add(objRead)
    '        End While
    '        Return lstItems
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '    End Try
    'End Function


    'Public Sub Delete(objToDelete As IeZInbox)
    '    If objToDelete Is Nothing Then
    '        Return
    '    End If
    '    Dim strQry As String = ""
    '    Dim objParam As SqlParameter()
    '    Dim param As SqlParameter
    '    strQry = "Update eZInbox set Isdeleted=1 where NodeId=@NodeId"
    '    objParam = New SqlParameter(0) {}
    '    param = New SqlParameter("@NodeId", objToDelete.NodeId)
    '    objParam(0) = param
    '    If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
    '        Throw New Exception("Record Not deleted due to some error")
    '    End If
    'End Sub


#End Region

End Class

