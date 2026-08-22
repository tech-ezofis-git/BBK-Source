Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer




#Region "Folder Details"

    Public Function CreateeZFolders(objtemp As eZFolders) As IeZFolders

        'SqlDbType.Money()


        Dim newObject As IeZFolders = Nothing
        If String.IsNullOrEmpty(objtemp.NodeName) Then
            Return Nothing
        End If
        objtemp.NodeName = objtemp.NodeName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select NodeId From eZFolders Where NodeName = @NodeName And TemplateId=@TemplateId And ParentNodeId=@ParentNodeId And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@NodeName", objtemp.NodeName)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@ParentNodeId", objtemp.ParentNodeId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                ''  Throw New Exception("eZFolders Code already exist!")
            Else
                strQry = "INSERT INTO eZFolders(NodeName,ParentNodeId,TemplateId,LevelId,PathId,CreatedOn,CreatedBy,Userid) " +
                    "VALUES(@NodeName,@ParentNodeId,@TemplateId,@LevelId,@PathId,@CreatedOn,@CreatedBy,@Userid);Select SCOPE_IDENTITY();"
                objParam = New SqlParameter(7) {}
                param = New SqlParameter("@NodeName", objtemp.NodeName)
                objParam(0) = param
                param = New SqlParameter("@ParentNodeId", objtemp.ParentNodeId)
                objParam(1) = param
                param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
                objParam(2) = param
                param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
                objParam(3) = param
                param = New SqlParameter("@TemplateId", objtemp.TemplateId)
                objParam(4) = param
                param = New SqlParameter("@LevelId", objtemp.LevelId)
                objParam(5) = param
                param = New SqlParameter("@PathId", objtemp.PathId)
                objParam(6) = param
                param = New SqlParameter("@Userid", objtemp.UserId)
                objParam(7) = param
                obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            End If

            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZFolders(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    'udaya
    Public Function InsertFolderprivate(ByVal Nodeid As Integer, ByVal Createdby As Integer, ByVal createdon As String) As String

        Dim strqry As String = ""
        Dim nodename As String = ""
        Dim parentnodeid As Integer = 0
        Dim Templateid As Integer = 0
        Dim Levelid As Integer = 0
        Dim pathid As String = 0
        Dim obj As Object

        Try
            strqry = "SELECT Nodename,Parentnodeid,Templateid,Levelid,Pathid FROM eZFolders WHERE Nodeid=" + Nodeid.ToString() + ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
            If obj Is Nothing Then
                Return Nothing
            End If
            Dim sqlrdr As SqlDataReader = DirectCast(obj, SqlDataReader)
            If sqlrdr.Read() Then
                nodename = sqlrdr(0).ToString()
                parentnodeid = sqlrdr(1).ToString()
                Templateid = sqlrdr(2).ToString()
                Levelid = sqlrdr(3).ToString()
                pathid = sqlrdr(4).ToString()
            End If
            strqry = "INSERT INTO eZFoldersbyuser(Nodeid,Nodename,Parentnodeid,Templateid,Levelid,Pathid,Createdon,Userid) " +
                "values(" + Nodeid.ToString() + ",N'" + nodename.ToString() + "'," + parentnodeid.ToString() + "," + Templateid.ToString() + "," + Levelid.ToString() + "," + pathid.ToString() + ",N'" + createdon.ToString() + "'," + Createdby.ToString() + ") "
            obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())
            If obj Is Nothing Then
                Return Nothing
            Else
                strqry = "UPDATE eZFolders SET userid=" + Createdby.ToString() + " where nodeid=" + Nodeid.ToString() + " and isdeleted=0"
                obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())
                If obj Is Nothing Then
                    Return Nothing
                Else
                    Return "Folder Changed As Private"
                End If
            End If


        Catch ex As Exception
            Return Nothing
        End Try

    End Function
    'udaya
    Public Function RemovePrivate(ByVal nodeid As Integer) As String

        Try
            Dim strqry As String = "UPDATE ezFoldersByUser SET isdeleted=1 WHERE nodeid=" + nodeid.ToString() + ""
            Dim obj As Object = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry)
            If obj Is Nothing Then
                Return Nothing
            Else
                strqry = "UPDATE ezFolders SET userid=0 WHERE nodeid=" + nodeid.ToString() + ""
                obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry)
                If obj Is Nothing Then
                    Return Nothing
                Else
                    Return "Folder Changed As Public"
                End If
            End If

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function CreateeZFoldersByUser(objtemp As eZFolders) As IeZFolders
        Dim newObject As IeZFolders = Nothing
        If String.IsNullOrEmpty(objtemp.NodeName) Then
            Return Nothing
        End If
        objtemp.NodeName = objtemp.NodeName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select NodeId From ezFoldersByUser Where NodeId = @NodeId And TemplateId=@TemplateId And UserId=@UserId And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@NodeId", objtemp.NodeId)
            objParam(0) = param
            param = New SqlParameter("@UserId", objtemp.UserId)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                strQry = "Update ezFoldersByUser Set NodeName=@NodeName,LevelId=@LevelId,PathId=@PathId,TemplateId=@TemplateId,ParentNodeId=@ParentNodeId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where NodeId=@NodeId"
                objParam = New SqlParameter(7) {}
                param = New SqlParameter("@NodeName", objtemp.NodeName)
                objParam(0) = param
                param = New SqlParameter("@TemplateId", objtemp.TemplateId)
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
                    newObject = GlobalInstance.eZFolders(objtemp.NodeId)
                    Read(newObject)
                    Return newObject
                End If
            Else
                strQry = "INSERT INTO ezFoldersByUser(NodeId,NodeName,ParentNodeId,TemplateId,LevelId,PathId,CreatedOn,CreatedBy,UserId) VALUES(@NodeId,@NodeName,@ParentNodeId,@TemplateId,@LevelId,@PathId,@CreatedOn,@CreatedBy,@UserId);Select SCOPE_IDENTITY();"
                objParam = New SqlParameter(8) {}
                param = New SqlParameter("@NodeName", objtemp.NodeName)
                objParam(0) = param
                param = New SqlParameter("@ParentNodeId", objtemp.ParentNodeId)
                objParam(1) = param
                param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
                objParam(2) = param
                param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
                objParam(3) = param
                param = New SqlParameter("@TemplateId", objtemp.TemplateId)
                objParam(4) = param
                param = New SqlParameter("@NodeId", objtemp.NodeId)
                objParam(5) = param
                param = New SqlParameter("@UserId", objtemp.UserId)
                objParam(6) = param
                param = New SqlParameter("@LevelId", objtemp.LevelId)
                objParam(7) = param
                param = New SqlParameter("@PathId", objtemp.PathId)
                objParam(8) = param
                obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
                newObject = GlobalInstance.eZFolders(objtemp.NodeId)
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

    Public Sub Read(objRead As IeZFolders)
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
                strQry = "Select ef.*,dbo.udf_TableName(ef.TemplateId) as TableName,dbo.udf_UserName(ef.UpdatedBy) as UpdatedBy1,dbo.udf_Cabinet(et.CabinetId) as CabinetName,dbo.udf_Template(ef.TemplateId) as TemplateName,dbo.udf_UserName(ef.CreatedBy) as CreatedBy1,et.CabinetID as CabinetID  From eZFolders ef,eZTemplate et Where ef.Isdeleted=0 and et.Isdeleted=0 and ef.TemplateId=et.TemplateId and ef.NodeId=@NodeId"
                param = New SqlParameter("@NodeId", objRead.NodeId)
                objParam(0) = param
            Else
                strQry = "Select ef.*,dbo.udf_TableName(ef.TemplateId) as TableName,dbo.udf_UserName(ef.UpdatedBy) as UpdatedBy1,dbo.udf_Cabinet(et.CabinetId) as CabinetName,dbo.udf_Template(ef.TemplateId) as TemplateName,dbo.udf_UserName(ef.CreatedBy) as CreatedBy1,et.CabinetID as CabinetID  From eZFolders ef,eZTemplate et Where ef.Isdeleted=0 and et.Isdeleted=0 and ef.TemplateId=et.TemplateId and ef.NodeName=@NodeName"
                param = New SqlParameter("@NodeName", objRead.NodeName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.NodeId = GetInteger(sqlRdr("NodeId"))
                objRead.NodeName = sqlRdr("NodeName").ToString()
                objRead.TemplateName = sqlRdr("TemplateName").ToString()
                objRead.TableName = sqlRdr("TableName").ToString()
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.LevelId = GetInteger(sqlRdr("LevelId"))
                objRead.CabinetName = sqlRdr("CabinetName").ToString()
                objRead.PathId = sqlRdr("PathId").ToString()
                objRead.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                objRead.ParentNodeId = GetInteger(sqlRdr("ParentNodeId"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZFolders.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZFolders() As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFolders
        Try
            Dim strQry As String = ""
            strQry = "Select NodeId From eZFolders where Isdeleted=0 order by NodeName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolders(GetSmallInterger(sqlRdr("NodeId")))
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
    Public Function ReadFilteredeZFolders(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFolders
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZFolders where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZFolders where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolders(GetSmallInterger(sqlRdr("NodeId")))
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
    Public Function ReadSelectedeZFolders(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFolders
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZFolders where Isdeleted=0 and ParentNodeId<>0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZFolders where Isdeleted=0 and ParentNodeId<>0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolders(GetSmallInterger(sqlRdr("NodeId")))
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
    Public Function ReadSelectezFoldersByUser(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFolders
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From ezFoldersByUser where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From ezFoldersByUser where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolders(GetSmallInterger(sqlRdr("NodeId")))
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
    Public Function ReadSelectedeZFoldersByUserWithUserId(Criteria As String, Value As String, ByVal UserId As Integer) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFolders
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From ezFoldersByUser where Isdeleted=0 and UserId=" + UserId.ToString() + " and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From ezFoldersByUser where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolders(GetSmallInterger(sqlRdr("NodeId")))
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

    Public Function ReadSelectedeZFoldersWithTemplateId(Criteria As String, Value As String, ByVal TemplateId As Integer) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFolders
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZFolders where Isdeleted=0 and TemplateId=" + TemplateId.ToString() + " and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZFolders where Isdeleted=0 and TemplateId=" + TemplateId.ToString() + " order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolders(GetSmallInterger(sqlRdr("NodeId")))
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

    Public Function ReadSelectedeZFoldersWithPathAndLevel(PathId As String, LevelId As String, ByVal TemplateId As Integer) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFolders
        Try
            Dim strQry As String = ""

            strQry = "Select NodeId From eZFolders where Isdeleted=0 and TemplateId=" + TemplateId.ToString() + " and "
            strQry = strQry & "PathId=N'" + PathId.ToString() + "' and "
            strQry = strQry & "LevelId=" + LevelId.ToString()
            strQry = strQry & " order by NodeName"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolders(GetSmallInterger(sqlRdr("NodeId")))
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
    Public Function ReadSelectedeZFoldersWithTemplateIdAndParentNodeId(Criteria As String, Value As String, ByVal TemplateId As Integer, ByVal ParentNodeId As Integer) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFolders
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZFolders where Isdeleted=0 and TemplateId=" + TemplateId.ToString() + " and ParentNodeId=" + ParentNodeId.ToString() + " and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZFolders where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolders(GetSmallInterger(sqlRdr("NodeId")))
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
    Public Sub Update(objToUpdate As IeZFolders)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select NodeId From eZFolders Where NodeName = @NodeName And TemplateId=@TemplateId And ParentNodeId=@ParentNodeId And Isdeleted=0 and NodeId <> @NodeId"
        objParam = New SqlParameter(3) {}
        param = New SqlParameter("@NodeName", objToUpdate.NodeName)
        objParam(0) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(1) = param
        param = New SqlParameter("@ParentNodeId", objToUpdate.ParentNodeId)
        objParam(2) = param
        param = New SqlParameter("@NodeId", objToUpdate.NodeId)
        objParam(3) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZFolders Code already exist!")
        Else
            strQry = "Update eZFolders Set NodeName=@NodeName,LevelId=@LevelId,PathId=@PathId,TemplateId=@TemplateId," +
                "ParentNodeId=@ParentNodeId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy,Userid=@UserId where NodeId=@NodeId"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@NodeName", objToUpdate.NodeName)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
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
            param = New SqlParameter("@UserId", objToUpdate.UserId)
            objParam(8) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub

    'udaya
    'Public Function GetValForHierarchyTreeView(ByVal TempId As String, ByVal UserId As String, ByVal Nodename As String, ByVal Levelid As Integer, ByVal nodeid As String) As DataSet
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
    '    ' Dim objRead As IeZFolders
    '    Try
    '        Dim strQry As String = ""

    '        Dim param As String() = {TempId, Nodename, Levelid, UserId, nodeid}
    '        Dim ds As DataSet = GetDatasetByStoredProcedureName("SP_GetFoldersbyLevelidandnodeid", param)
    '        Return ds
    '    Catch
    '        Return Nothing

    '    End Try
    'End Function
    'srini
    Public Function GetValForHierarchyTreeView(ByVal TempId As String, ByVal UserId As String, ByVal Nodename As String, ByVal Levelid As Integer, ByVal nodeid As String, ByVal hasaccess As Boolean, ByVal From As String) As DataSet
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        ' Dim objRead As IeZFolders
        Try
            Dim strQry As String = ""

            Dim param As String() = {TempId, Nodename, Levelid, UserId, nodeid, hasaccess, From}
            Dim ds As DataSet = GetDatasetByStoredProcedureName("SP_GetFoldersbyLevelidandnodeid", param)
            Return ds
        Catch
            Return Nothing

        End Try
    End Function


    Public Function GetValForTreeView(ByVal TempId As String, ByVal UserId As String) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objRead As IeZFolders
        Try
            Dim strQry As String = ""
            If UserId = 4 Then
                strQry = "select * from dbo.udf_HierarchyTreeView1(" + TempId + "," + UserId + ",2,4) WHERE isdeleted=0"
            Else
                strQry = "select * from dbo.udf_TreeView(" + TempId + "," + UserId + ") WHERE isdeleted=0"
            End If


            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objRead = GlobalInstance.eZFolders(GetSmallInterger(sqlRdr("NodeId")))
                objRead.NodeId = GetInteger(sqlRdr("NodeId"))
                objRead.NodeName = sqlRdr("NodeName").ToString()
                'objRead.PathId = sqlRdr("PathId").ToString()
                objRead.ParentNodeId = GetInteger(sqlRdr("ParentNodeId"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UserId = GetInteger(sqlRdr("Userid"))
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                'objRead.LevelId = GetInteger(sqlRdr("LevelId"))
                lstItems.Add(objRead)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    'Public Sub Delete(objToDelete As IeZFolders)
    '    If objToDelete Is Nothing Then
    '        Return
    '    End If
    '    Dim strQry As String = ""
    '    Dim objParam As SqlParameter()
    '    Dim param As SqlParameter
    '    strQry = "Update eZFolders set Isdeleted=1 where NodeId=@NodeId"
    '    objParam = New SqlParameter(0) {}
    '    param = New SqlParameter("@NodeId", objToDelete.NodeId)
    '    objParam(0) = param
    '    If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
    '        Throw New Exception("Record Not deleted due to some error")
    '    End If
    'End Sub
    Public Function FileLinkidentify(ByVal Nodeid As Integer, ByVal Itemid As Integer, ByVal templateid As Integer) As Integer
        Dim strqry As String = ""
        Dim tablename As String = ""
        Dim fieldname As String = ""
        Dim Nodename As String = ""
        Dim strqry2 As String = ""
        Dim obj2 As String = ""
        Dim obj As Object
        Dim path As String = ""
        Dim parentnodeid As Integer = 0
        Dim Lst1 As New List(Of IeZFolders)()
        Dim a As Integer = 0
        Dim b As Integer = 0
        Dim c As Integer
        Try
            tablename = GetTableNameByTemplateId(templateid)
            If Itemid = 0 Then
                Dim lstItems As New System.Collections.Generic.List(Of String)()
                ' Dim objItem As IeZFilesCopyLink
                Dim param0() As String = {templateid, Nodeid}
                Dim ds0 As DataSet = GetDatasetByStoredProcedureName("SP_CopyFile", param0)
                For Each dttable As DataTable In ds0.Tables
                    Dim dt As DataTable = dttable
                    For Each dtrow As DataRow In dt.Rows
                        lstItems.Add(dt.Rows(0).Item(0).ToString())
                    Next
                Next
                ' strqry = "SELECT Itemid FROM eZFilesCopyLink WHERE nodeid=" + Nodeid.ToString() + " and templateid=" + templateid.ToString() + " and isdeleted=0"
                'obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strqry.ToString())
                If lstItems.Count = 0 Then
                    a = 0
                Else
                    a = 2
                End If
                Lst1 = ReadSelectedeZFoldersWithTemplateId("Nodeid", Nodeid, templateid)
                If Lst1.Count <> 0 Then
                    path = Lst1(0).NodeName + "\"
                End If
repeat:
                Lst1 = ReadSelectedeZFoldersWithTemplateId("Nodeid", Lst1(0).ParentNodeId, templateid)
                path = Lst1(0).NodeName + "\" + path
                If Lst1(0).ParentNodeId <> 0 Then
                    GoTo repeat
                End If
                strqry = "SELECT Itemid FROM " + tablename.ToString() + " WHERE ifilepath like N'" + path + "%' and isdeleted=0"
                obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
                Dim sqlrdr As SqlDataReader = DirectCast(obj, SqlDataReader)
                Itemid = 0
                While sqlrdr.Read()
                    Itemid = sqlrdr(0).ToString()
                    strqry = "SELECT Nodeid FROM eZFilesCopyLink WHERE copyfrom=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + " and isdeleted=0"
                    Dim objfile As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
                    Dim rdrfile As SqlDataReader = DirectCast(objfile, SqlDataReader)
                    If rdrfile.Read() Then
                        b = 1
                    Else
                        b = 0
                    End If
                    If b = 1 Then
                        Exit While
                    End If
                End While
                If Itemid = 0 Then
                    c = 1
                    b = 4
                End If
            Else
                ' Dim fieldlevel As Integer
                strqry = "SELECT * FROM eZFilesCopyLink WHERE Itemid=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + " and isdeleted=0"
                obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
                Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
                If dr.Read() Then
                    a = 2
                    c = 1
                Else
                    a = 0
                End If
                If a = 0 Then
                    strqry = "SELECT Nodeid FROM eZFilesCopyLink WHERE CopyFrom=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + " and isdeleted=0"
                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
                    dr = DirectCast(obj, SqlDataReader)
                    If dr.Read() Then
                        b = 1
                    Else
                        b = 0
                    End If
                End If
            End If
            If a = 0 And b = 0 Then
                Return 0
            ElseIf a = 0 And b = 1 Then
                Return 1
            ElseIf a <> 0 And c = 1 Then
                Return 2
            ElseIf a <> 0 And b = 1 Then
                Return 3
            ElseIf a <> 0 And b = 0 Then
                Return 4
            ElseIf a = 0 And c = 1 Then
                Return 5
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function filelinkDelete(ByVal Nodeid As Integer, ByVal Itemid As Integer, ByVal templateid As Integer, ByVal delete As Integer, loginid As Integer) As String
        Dim strqry As String = ""
        Dim tablename As String = ""
        Dim fieldname As String = ""
        Dim Nodename As String = ""
        Dim strqry2 As String = ""
        Dim obj2 As Object = Nothing
        Dim obj As Object = Nothing
        Dim obj3 As Object = Nothing
        Dim path As String = ""
        Dim parentnodeid As Integer = 0
        Dim Lst1 As New List(Of IeZFolders)()
        Dim a As Integer = 0
        Dim b As Integer = 0
        Dim createdon As String = ""
        Try
            'Find Tabelname
            tablename = GetTableNameByTemplateId(templateid)
            createdon = DateDateTimeToString(Date.Now, True)
            'Find path for folder delete
            If Itemid = 0 Then
                Lst1 = ReadSelectedeZFoldersWithTemplateId("Nodeid", Nodeid, templateid)
                If Lst1.Count <> 0 Then
                    path = Lst1(0).NodeName + "\"
                End If
repeat:
                Lst1 = ReadSelectedeZFoldersWithTemplateId("Nodeid", Lst1(0).ParentNodeId, templateid)
                path = Lst1(0).NodeName + "\" + path
                If Lst1(0).ParentNodeId <> 0 Then
                    GoTo repeat
                End If
            End If
            Dim item As Integer
            If delete = 0 Then
                If Itemid = 0 Then
                    strqry = "SELECT Itemid FROM " + tablename.ToString() + " WHERE ifilepath like N'" + path + "%' and isdeleted=0"
                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
                    Dim sqlrdr As SqlDataReader = DirectCast(obj, SqlDataReader)
                    While sqlrdr.Read()
                        Itemid = sqlrdr(0).ToString()
                        strqry = "UPDATE " + tablename.ToString() + " SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + ""
                        obj = InsertAndUpdate(strqry)
                        Dim strqry3 As String = "UPDATE eZFilesCopyLink SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE Itemid=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + " and isdeleted=0"
                        obj3 = InsertAndUpdate(strqry3)
                    End While
                    strqry2 = "INSERT INTO eZIndexingChange(nodeid,templateid,del,createdby,Createdon) values(" + Nodeid.ToString() + "," + templateid.ToString() + ",1,'" + loginid.ToString() + "','" + createdon + "')"
                    obj2 = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry2.ToString())
                Else
                    strqry = "SELECT Itemid FROM eZFilesCopyLink WHERE CopyFrom=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + " and isdeleted=0"
                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
                    Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
                    While dr.Read()
                        item = dr(0).ToString()
                        Dim strqry3 As String = "UPDATE eZFilesCopyLink SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + item.ToString() + " and templateid=" + templateid.ToString() + " AND isdeleted=0 "
                        obj3 = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry3.ToString())
                        strqry = "UPDATE " + tablename.ToString() + " SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + item.ToString() + ""
                        obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())
                    End While
                    strqry = "UPDATE " + tablename.ToString() + " SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + ""
                    strqry2 = "INSERT INTO eZIndexingChange(itemid,templateid,del,Updatedby,updatedon) values(" + Itemid.ToString() + "," + templateid.ToString() + ",1,'" + loginid.ToString() + "','" + createdon + "')"
                    obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())
                    obj2 = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry2.ToString())
                End If
            End If
            If delete = 1 Then
                ' If Itemid = 0 Then
                'strqry = "SELECT Itemid,copyFrom,templateid FROM eZFilesCopyLink WHERE nodeid=" + Nodeid.ToString() + " and templateid=" + templateid.ToString() + " and isdeleted=0"
                'obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
                'Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
                'While dr.Read()
                '    Itemid = dr(1).ToString()
                '    Dim copyfrom As Integer = dr(0).ToString()
                '    tablename = GetTableNameByTemplateId(dr(2).ToString())
                '    '  strqry = "UPDATE " + tablename.ToString() + " SET isdeleted=1 WHERE itemid=" + copyfrom.ToString() + ""
                '    strqry2 = "UPDATE " + tablename.ToString() + " SET isdeleted=1 WHERE itemid=" + Itemid.ToString() + " or itemid=" + copyfrom.ToString() + ""
                '    Dim strqry3 As String = "INSERT INTO eZIndexingChange(Templateid,itemid,del) values(" + templateid.ToString() + "," + Itemid.ToString() + ",1) "
                '    obj = InsertAndUpdate(strqry2)
                '    obj = InsertAndUpdate(strqry3)
                '    ' obj = InsertAndUpdate(strqry)
                'End While
                'strqry = "UPDATE eZFilesCopyLink SET isdeleted=1 WHERE nodeid=" + Nodeid.ToString() + ""
                'obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())
                ' Else
                strqry = "SELECT CopyFrom FROM eZFilesCopyLink WHERE itemid=" + Itemid.ToString() + " and Templateid=" + templateid.ToString() + " and isdeleted=0"
                obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
                Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
                While dr.Read()
                    item = dr(0).ToString()
                    strqry2 = "UPDATE " + tablename.ToString() + " SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + " or itemid=" + item.ToString() + " and isdeleted=0"
                    Dim strqryupd As String = "INSERT INTO ezIndexingchange(itemid,templateid,del,updatedby,updatedon) values(" + item.ToString() + "," + templateid.ToString() + ",1,'" + loginid.ToString() + "','" + createdon + "')"
                    obj3 = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqryupd)
                    obj2 = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry2)
                End While
                strqry = "UPDATE eZFilesCopyLink SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + " or copyfrom=" + item.ToString() + " and templateid=" + templateid.ToString() + ""
                obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())
                ' End If
            End If
            If delete = 2 Then
                If Itemid = 0 Then
                    Dim lstItems As New System.Collections.Generic.List(Of String)()
                    ' Dim objItem As IeZFilesCopyLink
                    Dim param0() As String = {templateid, Nodeid}
                    Dim ds0 As DataSet = GetDatasetByStoredProcedureName("SP_CopyFile", param0)
                    For Each dttable As DataTable In ds0.Tables
                        Dim dt As DataTable = dttable
                        For Each dtrow As DataRow In dt.Rows
                            lstItems.Add(dt.Rows(0).Item(0).ToString())
                        Next
                    Next
                    Dim i As Integer = lstItems.Count
                    While i <> 0
                        i = i - 1
                        Itemid = lstItems(i).ToString()
                        strqry2 = "UPDATE " + tablename.ToString() + " SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + ""
                        obj = InsertAndUpdate(strqry2)
                        strqry = "UPDATE eZFilesCopyLink SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + ""
                        obj = InsertAndUpdate(strqry)
                    End While
                    strqry2 = "UPDATE ezfolders SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE nodeid=" + Nodeid.ToString() + ""
                    obj = InsertAndUpdate(strqry2)
                Else
                    strqry2 = "UPDATE " + tablename.ToString() + " SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + ""
                    strqry = "UPDATE eZFilesCopyLink SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + ""
                    obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())
                    InsertAndUpdate(strqry2)
                End If
            End If
            If delete = 3 Then
                If Itemid = 0 Then
                    strqry = "SELECT Itemid FROM " + tablename.ToString() + " WHERE ifilepath like N'" + path + "%' and isdeleted=0"
                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
                    Dim sqlrdr As SqlDataReader = DirectCast(obj, SqlDataReader)
                    While sqlrdr.Read()
                        Itemid = sqlrdr(0).ToString()
                        strqry = "UPDATE " + tablename.ToString() + " SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + ""
                        Dim strqry3 As String = "UPDATE eZFilesCopyLink SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE (CopyFrom=" + Itemid.ToString() + " or itemid=" + Itemid.ToString() + ") and templateid=" + templateid.ToString() + " "
                        obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())
                        obj3 = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry3.ToString())
                    End While
                    strqry2 = "INSERT INTO eZIndexingChange(nodeid,templateid,del,updatedby,updatedon) values(" + Nodeid.ToString() + "," + templateid.ToString() + ",1,'" + loginid.ToString() + "','" + createdon + "')"
                    obj2 = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry2.ToString())
                    Dim lstItems As New System.Collections.Generic.List(Of String)()
                    ' Dim objItem As IeZFilesCopyLink
                    Dim param0() As String = {templateid, Nodeid}
                    Dim ds0 As DataSet = GetDatasetByStoredProcedureName("SP_CopyFile", param0)
                    For Each dttable As DataTable In ds0.Tables
                        Dim dt As DataTable = dttable
                        For Each dtrow As DataRow In dt.Rows
                            lstItems.Add(dt.Rows(0).Item(0).ToString())
                        Next
                    Next
                    Dim i As Integer = lstItems.Count
                    While i <> 0
                        i = i - 1
                        Itemid = lstItems(i).ToString()
                        strqry2 = "UPDATE " + tablename.ToString() + " SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + ""
                        InsertAndUpdate(strqry2)
                        strqry = "UPDATE eZFilesCopyLink SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + ""
                        InsertAndUpdate(strqry)
                    End While
                Else
                    strqry = "UPDATE " + tablename.ToString() + " SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + ""
                    strqry2 = "INSERT INTO eZIndexingChange(itemid,templateid,del,updatedby,updatedon) values(" + Itemid.ToString() + "," + templateid.ToString() + ",1,'" + loginid.ToString() + "','" + createdon + "')"
                    Dim strqry3 As String = "UPDATE eZFilesCopyLink SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE copyfrom=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + " "
                    Dim strqry4 As String = "UPDATE eZFilesCopyLink SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + ""
                    obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())
                    obj2 = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry2.ToString())
                    obj3 = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry3.ToString())
                    Dim obj4 As Object = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry4.ToString())
                    strqry = "SELECT Itemid FROM eZFilesCopyLink WHERE copyfrom=" + Itemid.ToString() + " and templateid=" + templateid.ToString() + " and isdeleted=0"
                    Dim ds As New DataSet
                    ds = GetDatasetByQuery(strqry)
                    If ds.Tables.Count <> 0 Then
                        If ds.Tables(0).Rows.Count <> 0 Then
                            Dim i As Integer = ds.Tables(0).Rows.Count()
                            While i <> 0
                                i = i - 1
                                strqry4 = "UPDATE " + tablename.ToString() + " SET isdeleted=1,updatedby='" + loginid.ToString() + "',updatedon='" + createdon + "' WHERE itemid=" + ds.Tables(0).Rows(i).Item(0).ToString() + ""
                                InsertAndUpdate(strqry4)
                            End While

                        End If
                    End If
                End If
            End If
            If delete = 4 Then
                strqry = "INSERT INTO eZIndexingChange(nodeid,Templateid,del,updatedby,updatedon) values(" + Nodeid.ToString() + "," + templateid.ToString() + ",1,'" + loginid.ToString() + "','" + createdon + "')"
                obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry)
            End If
        Catch ex As Exception
            Return Nothing
        End Try
        If obj Is Nothing Then
            Return 0
        Else
            Return 1
        End If
    End Function
#End Region


#Region "Form Builder"

    Public Function FormBuilderGetxmlfilename() As List(Of String)
        Dim strqry As String = ""
        Dim filename As New List(Of String)

        Try
            strqry = "SELECT WorkFlowname FROM eZCA_1_4_items"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
            If obj Is Nothing Then
                Return Nothing
            Else
                Dim sqlrdr As SqlDataReader = DirectCast(obj, SqlDataReader)
                While sqlrdr.Read()
                    filename.Add(sqlrdr(0).ToString())
                End While
            End If
        Catch ex As Exception
            Return Nothing
        End Try
        Return filename
    End Function

#End Region


End Class

