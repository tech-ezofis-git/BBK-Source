Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "Folder Details"

   
    Public Function CreateeZFoldersForTemp(objtemp As eZFoldersForTemp) As IeZFoldersForTemp

        'SqlDbType.Money()


        Dim newObject As IeZFoldersForTemp = Nothing
        If String.IsNullOrEmpty(objtemp.NodeName) Then
            Return Nothing
        End If
        objtemp.NodeName = objtemp.NodeName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select NodeId From eZFoldersForTemp Where NodeName = @NodeName And TemplateId=@TemplateId And ParentNodeId=@ParentNodeId And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@NodeName", objtemp.NodeName)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@ParentNodeId", objtemp.ParentNodeId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                ''  Throw New Exception("eZFoldersForTemp Code already exist!")
            Else
                strQry = "INSERT INTO eZFoldersForTemp(NodeName,ParentNodeId,TemplateId,LevelId,PathId,UserId,CreatedOn,CreatedBy) VALUES(@NodeName,@ParentNodeId,@TemplateId,@LevelId,@PathId,@UserId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
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
                param = New SqlParameter("@UserId", objtemp.UserId)
                objParam(7) = param
                obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            End If

            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZFoldersForTemp(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Read(objRead As IeZFoldersForTemp)
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
                strQry = "Select ef.*,dbo.udf_TableName(ef.TemplateId) as TableName,dbo.udf_UserName(ef.UpdatedBy) as UpdatedBy1,dbo.udf_Cabinet(et.CabinetId) as CabinetName,dbo.udf_Template(ef.TemplateId) as TemplateName,dbo.udf_UserName(ef.CreatedBy) as CreatedBy1,et.CabinetID as CabinetID  From eZFoldersForTemp ef,eZTemplate et Where ef.Isdeleted=0 and et.Isdeleted=0 and ef.TemplateId=et.TemplateId and ef.NodeId=@NodeId"
                param = New SqlParameter("@NodeId", objRead.NodeId)
                objParam(0) = param
            Else
                strQry = "Select ef.*,dbo.udf_TableName(ef.TemplateId) as TableName,dbo.udf_UserName(ef.UpdatedBy) as UpdatedBy1,dbo.udf_Cabinet(et.CabinetId) as CabinetName,dbo.udf_Template(ef.TemplateId) as TemplateName,dbo.udf_UserName(ef.CreatedBy) as CreatedBy1,et.CabinetID as CabinetID  From eZFoldersForTemp ef,eZTemplate et Where ef.Isdeleted=0 and et.Isdeleted=0 and ef.TemplateId=et.TemplateId and ef.NodeName=@NodeName"
                param = New SqlParameter("@NodeName", objRead.NodeName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFoldersForTemp.")
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
                'throw new Exception("Attempt to read Invalid eZFoldersForTemp.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZFoldersForTemp() As System.Collections.Generic.List(Of IeZFoldersForTemp)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFoldersForTemp)()
        Dim objItem As IeZFoldersForTemp
        Try
            Dim strQry As String = ""
            strQry = "Select NodeId From eZFoldersForTemp where Isdeleted=0 order by NodeName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFoldersForTemp.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFoldersForTemp(GetSmallInterger(sqlRdr("NodeId")))
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
    Public Function ReadFilteredeZFoldersForTemp(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFoldersForTemp)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFoldersForTemp)()
        Dim objItem As IeZFoldersForTemp
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZFoldersForTemp where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZFoldersForTemp where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFoldersForTemp.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFoldersForTemp(GetSmallInterger(sqlRdr("NodeId")))
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
    Public Function ReadSelectedeZFoldersForTemp(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFoldersForTemp
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZFoldersForTemp where Isdeleted=0 and ParentNodeId<>0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZFoldersForTemp where Isdeleted=0 and ParentNodeId<>0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFoldersForTemp.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFoldersForTemp(GetSmallInterger(sqlRdr("NodeId")))
                objItem.NodeId = GetSmallInterger(sqlRdr("NodeId"))
                Dim objItem1 As IeZFolders
                objItem1 = New eZFolders
                objItem1.NodeId = objItem.NodeId
                objItem1.NodeName = objItem.NodeName
                objItem1.ParentNodeId = objItem.ParentNodeId
                objItem1.CreatedOn = objItem.CreatedOn
                objItem1.CreatedBy = objItem.CreatedBy
                objItem1.PathId = objItem.PathId
                objItem1.LevelId = objItem.LevelId
                objItem1.TemplateName = objItem.LevelId
                objItem1.TemplateId = objItem.TemplateId
                objItem1.CabinetName = objItem.CabinetName
                objItem1.CabinetID = objItem.CabinetID
                lstItems.Add(objItem1)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function


    Public Function ReadSelectedeZFoldersForTempWithTemplateId(Criteria As String, Value As String, ByVal TemplateId As Integer) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFoldersForTemp
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZFoldersForTemp where Isdeleted=0 and TemplateId=" + TemplateId.ToString() + " and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZFoldersForTemp where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFoldersForTemp.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFoldersForTemp(GetSmallInterger(sqlRdr("NodeId")))
                objItem.NodeId = GetSmallInterger(sqlRdr("NodeId"))
                Dim objItem1 As IeZFolders
                objItem1 = New eZFolders
                objItem1.NodeId = objItem.NodeId
                objItem1.NodeName = objItem.NodeName
                objItem1.ParentNodeId = objItem.ParentNodeId
                objItem1.CreatedOn = objItem.CreatedOn
                objItem1.CreatedBy = objItem.CreatedBy
                objItem1.PathId = objItem.PathId
                objItem1.LevelId = objItem.LevelId
                objItem1.CabinetID = objItem.CabinetID
                objItem1.CabinetName = objItem.CabinetName
                objItem1.TemplateId = objItem.TemplateId
                objItem1.TemplateName = objItem.TemplateName
                objItem1.TableName = objItem.TableName
                objItem1.UserId = objItem.UserId

                lstItems.Add(objItem1)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZFoldersForTempWithPathAndLevel(PathId As String, LevelId As String, ByVal TemplateId As Integer) As System.Collections.Generic.List(Of IeZFoldersForTemp)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFoldersForTemp)()
        Dim objItem As IeZFoldersForTemp
        Try
            Dim strQry As String = ""

            strQry = "Select NodeId From eZFoldersForTemp where Isdeleted=0 and TemplateId=" + TemplateId.ToString() + " and "
            strQry = strQry & "PathId=N'" + PathId.ToString() + "' and "
            strQry = strQry & "LevelId=" + LevelId.ToString()
            strQry = strQry & " order by NodeName"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFoldersForTemp.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFoldersForTemp(GetSmallInterger(sqlRdr("NodeId")))
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
    Public Function ReadSelectedeZFoldersForTempWithTemplateIdAndParentNodeId(Criteria As String, Value As String, ByVal TemplateId As Integer, ByVal ParentNodeId As Integer) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objItem As IeZFoldersForTemp
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NodeId From eZFoldersForTemp where Isdeleted=0 and TemplateId=" + TemplateId.ToString() + " and ParentNodeId=" + ParentNodeId.ToString() + " and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NodeName"
            Else
                strQry = "Select NodeId From eZFoldersForTemp where Isdeleted=0 order by NodeName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFoldersForTemp.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFoldersForTemp(GetSmallInterger(sqlRdr("NodeId")))
                objItem.NodeId = GetSmallInterger(sqlRdr("NodeId"))
                Dim objItem1 As IeZFolders
                objItem1 = New eZFolders
                objItem1.NodeId = objItem.NodeId
                objItem1.NodeName = objItem.NodeName
                objItem1.ParentNodeId = objItem.ParentNodeId
                objItem1.CreatedOn = objItem.CreatedOn
                objItem1.CreatedBy = objItem.CreatedBy
                objItem1.PathId = objItem.PathId
                objItem1.LevelId = objItem.LevelId
                lstItems.Add(objItem1)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZFoldersForTemp)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select NodeId From eZFoldersForTemp Where NodeName = @NodeName And TemplateId=@TemplateId And ParentNodeId=@ParentNodeId And Isdeleted=0 and NodeId <> @NodeId"
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
            Throw New Exception("eZFoldersForTemp Code already exist!")
        Else
            strQry = "Update eZFoldersForTemp Set NodeName=@NodeName,LevelId=@LevelId,PathId=@PathId,TemplateId=@TemplateId,ParentNodeId=@ParentNodeId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where NodeId=@NodeId"
            objParam = New SqlParameter(7) {}
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
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub

    'udaya 22/7
    Public Function GetValForTreeViewForTemp(ByVal TempId As String, ByVal UserId As String, ByVal Levelid As String, ByVal nodeid As String) As System.Collections.Generic.List(Of IeZFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolders)()
        Dim objRead As IeZFoldersForTemp
        Try
            Dim strQry As String = ""
            If Levelid <> "1" Then
                strQry = "select * from eZFoldersForTemp where TemplateId =" + TempId + " and parentnodeid=" + nodeid.ToString() + " and isdeleted=0"
            Else
                strQry = "select * from eZFoldersForTemp where TemplateId =" + TempId + " and levelid=2 and isdeleted=0"
            End If

            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFoldersForTemp.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objRead = GlobalInstance.eZFoldersForTemp(GetSmallInterger(sqlRdr("NodeId")))
                objRead.NodeId = GetInteger(sqlRdr("NodeId"))
                objRead.NodeName = sqlRdr("NodeName").ToString()
                objRead.PathId = sqlRdr("PathId").ToString()
                objRead.ParentNodeId = GetInteger(sqlRdr("ParentNodeId"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.LevelId = GetInteger(sqlRdr("LevelId"))
                Dim objRead1 As IeZFolders
                objRead1 = New eZFolders
                objRead1.NodeId = objRead.NodeId
                objRead1.NodeName = objRead.NodeName
                objRead1.ParentNodeId = objRead.ParentNodeId
                objRead1.CreatedOn = objRead.CreatedOn
                objRead1.CreatedBy = objRead.CreatedBy
                objRead1.PathId = objRead.PathId
                objRead1.LevelId = objRead.LevelId
                lstItems.Add(objRead1)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function getparentnodeid(ByVal nodeid As Integer) As Integer
        Dim parentnodeid As Integer = 0
        Try

            Dim strqry As String = "SELECT Parentnodeid from ezfolders WHERE nodeid=" + nodeid.ToString()
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
            Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
            If dr.Read() Then
                parentnodeid = dr(0).ToString()
            End If
        Catch ex As Exception

        End Try
        Return parentnodeid
    End Function

    'Public Sub Delete(objToDelete As IeZFoldersForTemp)
    '    If objToDelete Is Nothing Then
    '        Return
    '    End If
    '    Dim strQry As String = ""
    '    Dim objParam As SqlParameter()
    '    Dim param As SqlParameter
    '    strQry = "Update eZFoldersForTemp set Isdeleted=1 where NodeId=@NodeId"
    '    objParam = New SqlParameter(0) {}
    '    param = New SqlParameter("@NodeId", objToDelete.NodeId)
    '    objParam(0) = param
    '    If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
    '        Throw New Exception("Record Not deleted due to some error")
    '    End If
    'End Sub


#End Region

End Class

