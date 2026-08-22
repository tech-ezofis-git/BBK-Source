Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "eZFilesCopyLink Details"


    Public Function CreateeZFilesCopyLink(objtemp As eZFilesCopyLink) As IeZFilesCopyLink
        Dim newObject As IeZFilesCopyLink = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select CopyLinkId From eZFilesCopyLink Where itemid = @itemid And TemplateId=@TemplateId And NodeId=@NodeId and Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@itemid", objtemp.itemid)
            objParam(0) = param
            param = New SqlParameter("@NodeId", objtemp.NodeId)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZFilesCopyLink Code already exist!")
            End If
            strQry = "INSERT INTO eZFilesCopyLink(NodeId,itemid,CopyBy,TemplateId,ISMoved,CreatedOn,CreatedBy) VALUES(@NodeId,@itemid,@CopyBy,@TemplateId,@ISMoved,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@NodeId", objtemp.NodeId)
            objParam(0) = param
            param = New SqlParameter("@itemid", objtemp.itemid)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@CopyBy", objtemp.CopyBy)
            objParam(4) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(5) = param
            param = New SqlParameter("@ISMoved", objtemp.ISMoved)
            objParam(6) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZFilesCopyLink(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFilesCopyLink)
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
            If objRead.CreatedOn Is Nothing Then
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZFilesCopyLink Where Isdeleted=0 and CopyLinkId=@CopyLinkId"
                param = New SqlParameter("@CopyLinkId", objRead.CopyLinkId)
                objParam(0) = param
            Else
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZFilesCopyLink Where Isdeleted=0 and CopyLinkId=@CopyLinkId"
                param = New SqlParameter("@CopyLinkId", objRead.CopyLinkId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFilesCopyLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.CopyLinkId = GetInteger(sqlRdr("CopyLinkId"))
                objRead.NodeId = GetInteger(sqlRdr("NodeId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.itemid = GetInteger(sqlRdr("itemid"))
                objRead.CopyBy = GetInteger(sqlRdr("CopyBy"))
                objRead.ISMoved = GetInteger(sqlRdr("ISMoved"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZFilesCopyLink.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZFilesCopyLink() As System.Collections.Generic.List(Of IeZFilesCopyLink)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFilesCopyLink)()
        Dim objItem As IeZFilesCopyLink
        Try
            Dim strQry As String = ""
            strQry = "Select CopyLinkId From eZFilesCopyLink where Isdeleted=0 order by itemid"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFilesCopyLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFilesCopyLink(GetSmallInterger(sqlRdr("CopyLinkId")))
                objItem.CopyLinkId = GetSmallInterger(sqlRdr("CopyLinkId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZFilesCopyLink(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFilesCopyLink)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFilesCopyLink)()
        Dim objItem As IeZFilesCopyLink
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select CopyLinkId From eZFilesCopyLink where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by itemid"
            Else
                strQry = "Select CopyLinkId From eZFilesCopyLink where Isdeleted=0 order by itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFilesCopyLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFilesCopyLink(GetSmallInterger(sqlRdr("CopyLinkId")))
                objItem.CopyLinkId = GetSmallInterger(sqlRdr("CopyLinkId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadCopyFilewithNodeid(nodeid As Integer, templateid As Integer) As System.Collections.Generic.List(Of String)
        Dim lstItems As New System.Collections.Generic.List(Of String)()
        ' Dim objItem As IeZFilesCopyLink
        Dim param() As String = {templateid, nodeid}

        Try
            Dim ds As DataSet = GetDatasetByStoredProcedureName("SP_CopyFile", param)
            For Each dttable As DataTable In ds.Tables
                Dim dt As DataTable = dttable
                For Each dtrow As DataRow In dt.Rows
                    lstItems.Add(dt.Rows(0).Item(0).ToString())
                Next
            Next
            'objItem = GlobalInstance.eZFilesCopyLink(GetSmallInterger(sqlRdr("CopyLinkId")))
            'objItem.CopyLinkId = GetSmallInterger(sqlRdr("CopyLinkId"))


            Return lstItems
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ReadSelectedeZFilesCopyLinkWithLogingID(Criteria As String, Value As String, ECMLoginId As String) As System.Collections.Generic.List(Of IeZFilesCopyLink)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFilesCopyLink)()
        Dim objItem As IeZFilesCopyLink
        ' Dim ds As New DataSet
        '  Dim param() As New String={}
        Try
            Dim strQry As String = ""
            Dim nodeid As Integer = Value
            If Criteria <> "All" Then
                ' ds = GetDatasetByStoredProcedureName("SP_CopyFile", )
                strQry = "Select CopyLinkId From eZFilesCopyLink where Isdeleted=0 and CreatedBy=" + ECMLoginId + "  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by itemid"
            Else
                strQry = "Select CopyLinkId From eZFilesCopyLink where Isdeleted=0 order by itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFilesCopyLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFilesCopyLink(GetSmallInterger(sqlRdr("CopyLinkId")))
                objItem.CopyLinkId = GetSmallInterger(sqlRdr("CopyLinkId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZFilesCopyLinkWithMovedFiles(Criteria As String, Value As String, ECMLoginId As String) As System.Collections.Generic.List(Of IeZFilesCopyLink)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFilesCopyLink)()
        Dim objItem As IeZFilesCopyLink
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select CopyLinkId From eZFilesCopyLink where Isdeleted=0  and CreatedBy=" + ECMLoginId + "  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by itemid"
            Else
                strQry = "Select CopyLinkId From eZFilesCopyLink where Isdeleted=0 order by itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFilesCopyLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFilesCopyLink(GetSmallInterger(sqlRdr("CopyLinkId")))
                objItem.CopyLinkId = GetSmallInterger(sqlRdr("CopyLinkId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZFilesCopyLink(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFilesCopyLink)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFilesCopyLink)()
        Dim objItem As IeZFilesCopyLink
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select CopyLinkId From eZFilesCopyLink where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by itemid"
            Else
                strQry = "Select CopyLinkId From eZFilesCopyLink where Isdeleted=0 order by itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFilesCopyLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFilesCopyLink(GetSmallInterger(sqlRdr("CopyLinkId")))
                objItem.CopyLinkId = GetSmallInterger(sqlRdr("CopyLinkId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function


    Public Sub Update(objToUpdate As IeZFilesCopyLink)
        'If Not objToUpdate.IsModified Then
        '    Return
        'End If
        'If Not objToUpdate.IsReadFromDB Then
        '    Return
        'End If
        'Dim strQry As String = ""
        'Dim objParam As SqlParameter()
        'Dim param As SqlParameter
        'strQry = "Select NodeId From eZFilesCopyLink Where TemplateName = @TemplateName and NodeId <> @NodeId and Isdeleted=0"
        'objParam = New SqlParameter(1) {}
        'param = New SqlParameter("@TemplateName", objToUpdate.TemplateName)
        'objParam(0) = param
        'param = New SqlParameter("@NodeId", objToUpdate.NodeId)
        'objParam(1) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("eZFilesCopyLink Code already exist!")
        'Else
        '    strQry = "Update eZFilesCopyLink Set TemplateName=@TemplateName,DuplicateTypeId=@DuplicateTypeId,Description=@Description,CabinetID=@CabinetID,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where NodeId=@NodeId"
        '    objParam = New SqlParameter(6) {}
        '    param = New SqlParameter("@TemplateName", objToUpdate.TemplateName)
        '    objParam(0) = param
        '    param = New SqlParameter("@CabinetID", objToUpdate.CabinetID)
        '    objParam(1) = param
        '    param = New SqlParameter("@Description", objToUpdate.Description)
        '    objParam(2) = param
        '    param = New SqlParameter("@NodeId", objToUpdate.NodeId)
        '    objParam(3) = param
        '    param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        '    objParam(4) = param
        '    param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        '    objParam(5) = param
        '    param = New SqlParameter("@DuplicateTypeId", objToUpdate.DuplicateTypeId)
        '    objParam(6) = param
        '    If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
        '        Throw New Exception("Record Not updated due to some error")

        '    End If
        'End If
        objToUpdate.IsModified = False
    End Sub

    'udaya
    Public Function Finditemidusingnodeid(ByVal Nodeid As Integer, ByVal templateid As Integer) As Integer

        Dim itemid As Integer = 0
        Dim levelid As Integer = 0
        Dim Nodename As String = ""
        Dim strqry As String = ""
        Dim Fieldname As String = ""
        'shankar
        'Dim obj As Object

        Try


        Catch ex As Exception

        End Try

    End Function


    'udaya

    Public Function eZMoveandCopy(ByVal templateid As Integer, ByVal NewNodeid As Integer, ByVal itemid As Integer, ByVal work As String, ByVal createdon As String, ByVal Createdby As Integer, ByVal indexingpath As String) As String
        'Dim strqry As String = ""
        'Dim tablename As String = ""
        'Dim nodename As String = ""
        'Dim Fieldname As String = ""
        'Dim Fldname As String = ""
        Dim nodeid As Integer = 0
        'Dim levelid As Integer = 0
        'Dim oldvalue As String = ""
        'Dim cabinetname As String = ""
        'Dim newvalue As String = ""
        'Dim Fieldvalue As String = ""
        'Dim path As String = ""
        Dim obj As Object=Nothing
        Dim param As String()
        '  Dim item As String
        Try
            'tablename = GetTableNameByTemplateId(templateid)
            ' If NewNodeid = 0 Then
            ''srini 04-dec-2017
            'If work = "Move" Then
            param = {templateid.ToString(), nodeid.ToString(), itemid.ToString(), "0", "0", work.ToString(), Createdby.ToString(), createdon.ToString(), indexingpath.ToString()}
            obj = InsertandUpdateStoredProcedure("SP_eZIndexingService", param)
            'Else
            '    param = {templateid.ToString(), nodeid.ToString(), itemid.ToString(), "0", "0", work.ToString(), Createdby.ToString(), createdon.ToString(), indexingpath.ToString()}
            '    obj = InsertandUpdateStoredProcedure("SP_eZIndexingService", param)
            'End If

            '  Else

            '                If work = "Move" Then

            '                    'strqry = "SELECT Nodename,Levelid From eZFolders WHERE Nodeid=" + NewNodeid.ToString() + ""
            '                    'obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
            '                    'If obj Is Nothing Then
            '                    '    Return Nothing
            '                    'Else
            '                    '    Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
            '                    '    If dr.Read() Then
            '                    '        newvalue = dr(0).ToString()
            '                    '        levelid = dr(1).ToString()
            '                    '    End If
            '                    'End If


            '                    'strqry = "SELECT FieldName FROM eZTemplatefield WHERE Templateid=" + templateid.ToString() + " and FieldLevel=" + levelid.ToString() + "-1"
            '                    'obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
            '                    'If obj Is Nothing Then
            '                    '    Return Nothing
            '                    'Else
            '                    '    Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
            '                    '    If dr.Read() Then
            '                    '        Fieldname = dr(0).ToString()

            '                    '    End If
            '                    'End If
            '                    'strqry = "SELECT " + Fieldname.ToString() + " From " + tablename.ToString() + " WHERE Itemid=" + itemid.ToString() + ""
            '                    'obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
            '                    'If obj Is Nothing Then
            '                    '    Return Nothing
            '                    'Else
            '                    '    Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
            '                    '    If dr.Read() Then
            '                    '        oldvalue = dr(0).ToString()
            '                    '    End If
            '                    'End If

            '                    'strqry = "INSERT INTO eZIndexingChange (nodeid,Templateid,oldvalue,Newvalue,itemid,Levelid,Createdon,Createdby) values(" + NewNodeid.ToString() + "," + templateid.ToString() + ",N'" + oldvalue.ToString() + "',N'" + newvalue.ToString() + "'," + itemid.ToString() + "," + levelid.ToString() + ",N'" + createdon.ToString() + "'," + Createdby.ToString() + ")"
            '                    'InsertAndUpdate(strqry)
            '                    'Dim levid As Integer = levelid
            '                    'strqry = "SELECT Hierarchy_Id FROM eZHierarchy WHERE Createdby=" + Createdby.ToString() + " and templateid=" + templateid.ToString() + " and isdeleted=0"
            '                    'Dim ds As New DataSet
            '                    'ds = GetDatasetByQuery(strqry)
            '                    'If ds.Tables().Count <> 0 Then
            '                    '    If ds.Tables(0).Rows.Count <> 0 Then

            '                    '    Else
            '                    '        While NewNodeid <> 0 And levid <> 1
            '                    '            'strqry = "SELECT nodename,Parentnodeid FROM eZFolders WHERE nodeid=" + Newparentnodeid.ToString() + ""
            '                    '            Dim strqryfieldname As String = "SELECT distinct(TF.Fieldname),F.Nodename,F.Parentnodeid,F.Levelid FROM eZTEmplateField as TF join eZFolders as F on TF.Templateid=F.Templateid WHERE TF.FieldLevel=" + levelid.ToString() + "-1 and TF.Templateid=" + templateid.ToString() + " and F.Nodeid=" + NewNodeid.ToString() + " and F.isdeleted=0 and TF.isdeleted=0"
            '                    '            Dim objField As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqryfieldname.ToString())
            '                    '            If objField Is Nothing Then
            '                    '                Return Nothing
            '                    '            Else
            '                    '                Dim sqlrdr As SqlDataReader = DirectCast(objField, SqlDataReader)
            '                    '                If sqlrdr.Read() Then
            '                    '                    NewNodeid = sqlrdr(2).ToString()
            '                    '                    nodename = sqlrdr(1).ToString()
            '                    '                    Fieldname = sqlrdr(0).ToString()
            '                    '                    levelid = sqlrdr(3).ToString()
            '                    '                    levid = levelid - 1
            '                    '                    levelid = levelid - 1
            '                    '                End If
            '                    '            End If
            '                    '            item = itemid.ToString()
            '                    '            Dim strqryupdate As String = "UPDATE " + tablename.ToString() + " SET " + Fieldname.ToString() + "=N'" + nodename.ToString() + "' WHERE itemid=" + itemid.ToString() + " "
            '                    '            obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqryupdate.ToString())
            '                    '            If obj Is Nothing Then
            '                    '                Throw New Exception("Error Occur in Updation")
            '                    '            End If
            '                    '        End While
            '                    '    End If
            '                    'End If


            '                Else
            '                    Dim fieldlevel As Integer
            '                    Dim filepath As String = ""
            '                    Dim filename As String = ""
            '                    Dim filetype As String = ""
            '                    Dim levid As Integer

            '                    Dim templatename As String = ""
            '                    tablename = GetTableNameByTemplateId(templateid.ToString())
            '                    strqry = "SELECT max(Fieldlevel) FROM eZTemplatefield WHERE Templateid=" + templateid.ToString() + " and isdeleted=0"
            '                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
            '                    Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
            '                    If dr.Read() Then
            '                        fieldlevel = dr(0).ToString()
            '                        fieldlevel = fieldlevel + 1
            '                    End If

            '                    strqry = "SELECT Levelid FROM eZFolders WHERE nodeid=" + NewNodeid.ToString() + " and isdeleted=0"
            '                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
            '                    dr = DirectCast(obj, SqlDataReader)
            '                    If dr.Read() Then
            '                        levelid = dr(0).ToString()
            '                        levid = dr(0).ToString()
            '                    End If

            '                    strqry = "SELECT FieldName FROM eZTemplatefield WHERE templateid=" + templateid.ToString() + " and FieldLevel<>0 and isdeleted=0 order by Fieldlevel desc"
            '                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
            '                    dr = DirectCast(obj, SqlDataReader)
            '                    While dr.Read()
            '                        Fieldname = "," + dr(0).ToString() + Fieldname
            '                    End While

            '                    nodeid = NewNodeid
            'repeatfieldvalue:
            '                    strqry = "SELECT nodename,Parentnodeid,levelid,dbo.udf_CabinetByTemplateId(" + templateid.ToString() + "),dbo.udf_Template(" + templateid.ToString() + ")  FROM eZFolders WHERE nodeid=" + nodeid.ToString() + ""
            '                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
            '                    Dim drFieldvalue As SqlDataReader = DirectCast(obj, SqlDataReader)
            '                    If drFieldvalue.Read() Then
            '                        nodename = drFieldvalue(0).ToString()
            '                        nodeid = drFieldvalue(1).ToString()
            '                        levelid = drFieldvalue(2).ToString()
            '                        cabinetname = drFieldvalue(3).ToString()
            '                        templatename = drFieldvalue(4).ToString()
            '                    End If
            '                    Fieldvalue = "," + "'" + nodename + "'" + Fieldvalue
            '                    path = "\" + nodename + path
            '                    If levelid <> 2 Then
            '                        GoTo repeatfieldvalue
            '                    End If
            'repeat:
            '                    strqry = "SELECT Fieldname,fieldlevel FROM eZTemplatefield WHERE Fieldlevel=" + levid.ToString() + " and templateid=" + templateid.ToString() + ""
            '                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
            '                    dr = DirectCast(obj, SqlDataReader)
            '                    If dr.Read() Then
            '                        Fldname = dr(0).ToString()
            '                        levelid = dr(1).ToString()
            '                        levid = levelid + 1
            '                    End If
            '                    Dim version As String = ""
            '                    Dim status As String = ""
            '                    Dim size As String = ""
            '                    Dim strqryvalue As String = "SELECT " + Fldname.ToString() + ",ifilepath,ifilename,ifiletype,version,dstatus,dsize FROM " + tablename.ToString() + " WHERE itemid=" + itemid.ToString() + " and isdeleted=0"
            '                    Dim objvalue As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqryvalue)
            '                    Dim drvalue As SqlDataReader = DirectCast(objvalue, SqlDataReader)
            '                    If drvalue.Read() Then
            '                        nodename = drvalue(0).ToString()
            '                        filepath = drvalue(1).ToString()
            '                        filename = drvalue(2).ToString()
            '                        filetype = drvalue(3).ToString()
            '                        version = drvalue(4).ToString()
            '                        status = drvalue(5).ToString()
            '                        size = drvalue(6).ToString()
            '                    End If
            '                    Fieldvalue = Fieldvalue + "," + "N'" + nodename + "'"

            '                    path = path + "\" + nodename

            '                    If fieldlevel <> levid Then
            '                        GoTo repeat
            '                    End If
            '                    path = cabinetname + "\" + templatename + path
            '                    path = path.Remove(path.LastIndexOf("\"))
            '                    Fldname = ""
            '                    strqry = "SELECT Fieldname From eZTemplatefield WHERE templateid=" + templateid.ToString() + " and isdeleted=0 and (Fieldlevel=0 or mandatory=0) "
            '                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
            '                    dr = DirectCast(obj, SqlDataReader)
            '                    While dr.Read()
            '                        Fldname = "," + dr(0).ToString() + Fldname
            '                        strqry = "SELECT " + dr(0).ToString() + " FROM " + tablename.ToString() + " WHERE Itemid=" + itemid.ToString() + " and isdeleted=0"
            '                        obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
            '                        drvalue = DirectCast(obj, SqlDataReader)
            '                        If drvalue.Read() Then
            '                            Fieldvalue = Fieldvalue + "," + "N'" + drvalue(0).ToString() + "'"
            '                        End If
            '                    End While





            '                    Fieldname = Fieldname + Fldname
            '                    strqry = "INSERT INTO " + tablename.ToString() + "(ERSid,Templateid,ifilepath,ifilename,ifiletype,Createdon" + Fieldname.ToString() + ",Createdby,isdeleted,checkoutby,UpdatedBy,eZFrom,version,dstatus,dsize) values(1," + templateid.ToString() + ",N'" + filepath.ToString() + "',N'" + filename.ToString() + "',N'" + filetype.ToString() + "',N'" + createdon.ToString() + "'" + Fieldvalue.ToString() + "," + Createdby.ToString() + ",0,0,0,N'Copy','" + version.ToString() + "','" + status.ToString() + "','" + size.ToString() + "') ;SELECT SCOPE_IDENTITY();"
            '                    obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)
            '                    tablename = tablename.Replace("items", "History")
            '                    strqry = "INSERT INTO " + tablename.ToString() + "(ERSid,Templateid,ifilepath,ifilename,ifiletype,Createdon" + Fieldname.ToString() + ",Createdby,isdeleted,checkoutby,UpdatedBy,eZFrom,version,dstatus,dsize) values(1," + templateid.ToString() + ",N'" + filepath.ToString() + "',N'" + filename.ToString() + "',N'" + filetype.ToString() + "',N'" + createdon.ToString() + "'" + Fieldvalue.ToString() + "," + Createdby.ToString() + ",0,0,0,N'Copy','" + version.ToString() + "','" + status.ToString() + "','" + size.ToString() + "') ;SELECT SCOPE_IDENTITY();"
            '                    SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry)

            '                    Dim rdr As SqlDataReader = DirectCast(obj, SqlDataReader)
            '                    If rdr.Read() Then
            '                        item = rdr(0).ToString()
            '                    End If

            '                End If
            '            End If
            '            param = {path.ToString(), item.ToString()}
            '            '  obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())

            If obj Is Nothing Then
                Return Nothing
            Else
                Return obj

            End If

        Catch ex As Exception
            Return Nothing
        End Try
    End Function



    'udaya
    Public Function eZDragandDrop(ByVal Oldnodeid As Integer, ByVal Templateid As Integer, ByVal createdby As Integer, ByVal createdon As String, ByVal oldParentnodeid As Integer, ByVal Newparentnodeid As Integer, ByVal work As String) As String
        Dim itemid As Integer = 0
        Dim tablename As String = ""
        Dim strqry As String = ""
        Dim Levelid As Integer = 0
        Dim Nodename As String = ""
        Dim Fieldname As String = ""
        Dim oldvalue As String = ""
        Dim NewValue As String = ""
        Dim output As String = ""
        Dim obj As Object
        'Dim objparam As SqlParameter()
        'Dim param As SqlParameter
        Try

            tablename = GetTableNameByTemplateId(Templateid)
            strqry = "SELECT Nodename FROM eZFolders WHERE nodeid=" + Newparentnodeid.ToString() + " and isdeleted=0"
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
            If obj Is Nothing Then
                Return Nothing
            Else
                Dim sqlrdrnewvalue As SqlDataReader = DirectCast(obj, SqlDataReader)
                If sqlrdrnewvalue.Read() Then
                    NewValue = sqlrdrnewvalue(0).ToString()
                End If
            End If

            strqry = "SELECT Levelid,Nodename FROM eZFolders WHERE nodeid=" + Oldnodeid.ToString() + " and isdeleted=0"
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
            If obj Is Nothing Then
                Return Nothing
            Else
                Dim rd As SqlDataReader = DirectCast(obj, SqlDataReader)
                If rd.Read() Then
                    Levelid = rd(0).ToString()
                    Nodename = rd(1).ToString()
                    oldvalue = rd(1).ToString()
                End If
            End If
            strqry = "SELECT Fieldname FROM eZTemplateField WHERE FieldLevel=" + Levelid - 1 + " and Templateid=" + Templateid.ToString() + " and isdeleted=0"
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
            If obj Is Nothing Then
                Return Nothing
            Else
                Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
                If dr.Read() Then
                    Fieldname = dr(0).ToString()
                End If
            End If

            strqry = "SELECT Itemid FROM " + tablename + " WHERE " + Fieldname.ToString() + "=N'" + Nodename.ToString() + "' and isdeleted=0"
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString())
            If obj Is Nothing Then
                Return Nothing
            Else
                Dim sqldr As SqlDataReader = DirectCast(obj, SqlDataReader)
                While sqldr.Read()
                    itemid = sqldr(0).ToString()
                    If work = "Move" Then
                        While Newparentnodeid <> 0
                            'strqry = "SELECT nodename,Parentnodeid FROM eZFolders WHERE nodeid=" + Newparentnodeid.ToString() + ""
                            Dim strqryfieldname As String = "SELECT distinct(TF.Fieldname),F.Nodename,F.Parentnodeid FROM eZTEmplateField as TF join eZFolders as F on TF.Templateid=F.Templateid WHERE TF.FieldLevel=F.Levelid-1 and TF.Templateid=" + Templateid.ToString() + " and F.Nodeid=" + Newparentnodeid.ToString() + " and isdeleted=0"
                            Dim objField As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqryfieldname.ToString())
                            If objField Is Nothing Then
                                Return Nothing
                            Else
                                Dim sqlrdr As SqlDataReader = DirectCast(objField, SqlDataReader)
                                If sqlrdr.Read() Then
                                    Newparentnodeid = sqlrdr(2).ToString()
                                    Nodename = sqlrdr(1).ToString()
                                    Fieldname = sqlrdr(0).ToString()
                                End If
                            End If

                            Dim strqryupdate As String = "UPDATE " + tablename.ToString() + " SET " + Fieldname.ToString() + "=N'" + Nodename.ToString() + "' WHERE itemid=" + itemid.ToString() + " "
                            obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqryupdate.ToString())
                            If obj Is Nothing Then
                                Throw New Exception("Error Occur in Updation")
                            End If
                        End While
                        strqry = "INSERT INTO eZIndexingChange(Templateid,Nodeid,oldvalue,Newvalue,itemid,Levelid,Createdon,Createdby) values(" + Templateid.ToString() + "," + oldParentnodeid.ToString() + ",N'" + oldvalue.ToString() + "',N'" + NewValue.ToString() + "'," + itemid.ToString() + "," + Levelid.ToString() + ",N'" + createdon.ToString() + "'," + createdby.ToString() + ")"


                    Else
                        strqry = "INSERT INTO eZFilesCopyLink(Nodeid,Itemid,Templateid,Createdon,Createdby,isdeleted) values(" + Newparentnodeid.ToString() + "," + itemid.ToString() + "," + Templateid.ToString() + ",N'" + createdon.ToString() + "'," + createdby.ToString() + ",0)"
                    End If

                    output = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString())

                End While
            End If

            If output Is Nothing Then
                Throw New Exception("Error Occured")
            Else
                output = "Changed"
            End If
            Return output

        Catch ex As Exception

        End Try

    End Function

    Public Sub Delete(objToDelete As IeZFilesCopyLink)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFilesCopyLink set Isdeleted=1 where CopyLinkId=@CopyLinkId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@CopyLinkId", objToDelete.CopyLinkId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region

End Class

