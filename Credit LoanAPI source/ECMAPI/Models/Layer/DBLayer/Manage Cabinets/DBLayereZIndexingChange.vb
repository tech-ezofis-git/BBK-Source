Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary

Partial Public Class DBLayer
#Region "Indexing Change"
    Public Function CreateezIndexingchange(ByVal itemid As Integer, ByVal fieldname As String, ByVal Tempid As Integer, ByVal nodeid As Integer, ByVal newvalue As String, ByVal Createdby As Integer, ByVal Createdon As String, ByVal Condition As String) As String
        Dim strqry As String = ""
        Dim strqryupdate As String = ""
        Dim fieldid As Integer
        Dim Levelid As Integer
        Dim Parentnodeid As Integer
        Dim Oldvalue As String = ""
        Dim paramupdate As SqlParameter
        Dim objupdate As Object
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        Dim obj As Object
        Dim objfield As Object
        Dim objold As Object
        Dim strqryupdate1 As String = ""
        Dim path As String = ""
        Dim changeindex As Boolean = True
        Try
            Dim Tablename As String = GetTableNameByTemplateId(Tempid)
            If itemid <> 0 And Condition = "" Then
                Dim strqryold As String = "SELECT [" + fieldname + "]  FROM " + Tablename + " WHERE itemid=@itemid"
                Dim objparamold As SqlParameter() = New SqlParameter(0) {}
                Dim paramold As SqlParameter
                paramold = New SqlParameter("@itemid", itemid)
                objparamold(0) = paramold
                objold = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqryold, objparamold)
                If objold Is Nothing Then
                    Return Nothing
                Else
                    Dim rd As SqlDataReader = DirectCast(objold, SqlDataReader)
                    If rd.Read() Then
                        Oldvalue = rd(0).ToString()
                    End If
                End If
                Dim strqryfield As String = "SELECT fieldid,FieldLevel FROM eZTemplateField WHERE FieldName=N'" + fieldname + "' " +
                    "and Templateid=" + Tempid.ToString() + "  and isdeleted=0"
                objfield = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqryfield)
                If objfield Is Nothing Then
                    Return Nothing
                Else
                    Dim DR As SqlDataReader = DirectCast(objfield, SqlDataReader)
                    If DR.Read() Then
                        fieldid = DR(0).ToString()
                        Levelid = DR(1).ToString()
                        If Levelid = "0" Then
                            changeindex = False
                        End If
                        Levelid = Levelid + 1
                    End If
                End If
                newvalue = newvalue.Replace("'", "''")
                strqryupdate = "UPDATE " + Tablename + " SET [" + fieldname + "]=N'" + newvalue + "',updatedon=N'" + Createdon + "'," +
                    "UpdatedBy=" + Createdby.ToString() + " WHERE itemid=" + itemid.ToString() + " "
                strqryupdate1 = "UPDATE " + Tablename.Replace("items", "history") + " SET [" + fieldname + "]=N'" + newvalue + "'," +
                    "updatedon=N'" + Createdon + "',UpdatedBy=" + Createdby.ToString() + " WHERE itemid=" + itemid.ToString() + " "
                strqry = "INSERT INTO eZIndexingChange(Templateid,Newvalue,Fieldid,itemid,oldvalue,Createdby,Createdon,Levelid) " +
                    "VALUES(@templateid,@newvalue,@fieldid,@itemid,@oldvalue,@Createdby,@Createdon,@Levelid) "
                objParam = New SqlParameter(7) {}
                param = New SqlParameter("@itemid", itemid.ToString)
                objParam(0) = param
                param = New SqlParameter("@newvalue", newvalue)
                objParam(1) = param
                param = New SqlParameter("@fieldid", fieldid)
                objParam(2) = param
                param = New SqlParameter("@templateid", Tempid)
                objParam(3) = param
                param = New SqlParameter("@oldvalue", Oldvalue)
                objParam(4) = param
                param = New SqlParameter("@Createdby", Createdby)
                objParam(5) = param
                param = New SqlParameter("@Createdon", Createdon)
                objParam(6) = param
                param = New SqlParameter("@Levelid", Levelid)
                objParam(7) = param
                objupdate = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqryupdate)
                objupdate = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqryupdate1)
                If changeindex Then
                    obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry, objParam)
                Else
                    obj = "1"
                End If
                If obj AndAlso objupdate Is Nothing Then
                    Return Nothing
                Else
                    Return "Folder Name Changed Successfully"
                End If
            Else
                Dim strold As String = "SELECT Nodename,Levelid,Parentnodeid FROM eZFolders WHERE nodeid=@nodeid"
                objParam = New SqlParameter(0) {}
                param = New SqlParameter("@nodeid", nodeid)
                objParam(0) = param
                objold = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strold, objParam)
                If objold Is Nothing Then
                    Return Nothing
                Else
                    Dim dr As SqlDataReader = DirectCast(objold, SqlDataReader)
                    If dr.Read() Then
                        Oldvalue = dr(0).ToString()
                        Levelid = dr(1).ToString()
                        Parentnodeid = dr(2).ToString()
                    End If
                End If
                Dim strfield As String = "SELECT Fieldname FROM eZTemplateField WHERE FieldLevel=@Fieldlevel and TemplateId=@Tempid"
                objParam = New SqlParameter(1) {}
                param = New SqlParameter("@Fieldlevel", Levelid - 1)
                objParam(0) = param
                param = New SqlParameter("@Tempid", Tempid)
                objParam(1) = param
                objfield = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strfield, objParam)
                If objfield Is Nothing Then
                    Return Nothing
                Else
                    Dim Reader As SqlDataReader = DirectCast(objfield, SqlDataReader)
                    If Reader.Read() Then
                        fieldname = Reader(0).ToString()
                    End If
                End If
                newvalue = newvalue.Replace("'", "''")
                Oldvalue = Oldvalue.Replace("'", "''")
                path = GetFolderPath(nodeid, Tempid, Parentnodeid).Replace("'", "''")
                strqryupdate = "UPDATE " + Tablename + " SET [" + fieldname + "]=N'" + newvalue + "',updatedon=N'" + Createdon + "'," +
                    "UpdatedBy=" + Createdby.ToString() + " WHERE itemid in (SELECT Itemid FROM " + Tablename + " WHERE ifilepath like N'" + path + "%' ) " +
                    "and ezfrom <> 'copy' "
                strqryupdate1 = "UPDATE " + Tablename.Replace("items", "history") + " SET [" + fieldname + "]=N'" + newvalue + "'," +
                    "updatedon=N'" + Createdon + "',UpdatedBy=" + Createdby.ToString() + " WHERE itemid in " +
                    "(SELECT Itemid FROM " + Tablename + " WHERE ifilepath like N'" + path + "%' ) and ezfrom <> 'copy'"
                strqry = "INSERT INTO ezIndexingChange(nodeid,parentid,itemid,Templateid,Newvalue,oldvalue,Createdby,Createdon,Levelid) " +
                   "SELECT 0,0,itemid," + Tempid.ToString + ",'" + newvalue + "','" + Oldvalue + "','" + Createdby.ToString + "','" +
                   Createdon + "'," + (Levelid - 1).ToString + " from " + Tablename + " WHERE ifilepath like N'" + path + "%' and ezfrom <> 'copy'"
                obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry)
                objupdate = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqryupdate)
                objupdate = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqryupdate1)
                If obj AndAlso objupdate Is Nothing Then
                    Return Nothing
                Else
                    Return "Folder Name Changed Successfully"
                End If
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function eZInsertNewFolder(ByVal Parentid As Integer, ByVal tempid As Integer, ByVal value As String, ByVal Levelid As Integer, ByVal Createdby As Integer, ByVal Createdon As String) As String
        Dim objparam As SqlParameter()
        Dim param As SqlParameter
        Dim strqry As String = ""
        Dim strqryItems As String = ""
        Dim objparamItems As SqlParameter()
        Dim paramItems As SqlParameter
        Dim Fieldname As String = ""
        Dim objparamfolder As SqlParameter()
        Dim paramfolder As SqlParameter
        Dim strqryfolder As String = ""
        Dim objitems As Object
        Dim objfolders As Object
        Dim objindex As Object
        Try
            Dim Check As String = Createdon
            Check = Check.Replace("/", "")
            Check = Check.Replace(":", "")
            Check = Check.Replace(" ", "")
            Check = Check.Remove(Check.Length - 2, 2)
            objparam = New SqlParameter(5) {}
            param = New SqlParameter("@Tempid", tempid)
            objparam(0) = param
            param = New SqlParameter("@Parentid", Parentid)
            objparam(1) = param
            param = New SqlParameter("@Value", value)
            objparam(2) = param
            param = New SqlParameter("@Createdon", Createdon)
            objparam(3) = param
            param = New SqlParameter("@Createdby", Createdby)
            objparam(4) = param
            param = New SqlParameter("@Fieldlevel", Levelid)
            objparam(5) = param
            Dim strqryLevelname As String = "SELECT FieldName From eZTemplateField WHERE FieldLevel=@Fieldlevel and TemplateId=@Tempid"
            Dim objfieldname As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqryLevelname.ToString(), objparam)
            If objfieldname Is Nothing Then
                Return Nothing
            Else
                Dim Sqlrdr As SqlDataReader = DirectCast(objfieldname, SqlDataReader)
                If Sqlrdr.Read() Then
                    Fieldname = Sqlrdr(0).ToString()
                End If
            End If
            Dim levid As Integer
            Dim path As String = ""
            levid = Levelid
            strqry = "INSERT INTO eZIndexingChange(Templateid,Parentid,Newvalue,Createdon,Createdby) values(@Tempid,@Parentid,@Value,@Createdon,@Createdby)"
            While levid <> 0
                strqryfolder = "SELECT nodename,Levelid,parentnodeid from ezfolders WHERE nodeid=" + Parentid.ToString() + " and isdeleted=0"
                objfolders = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqryfolder.ToString())
                Dim dr As SqlDataReader = DirectCast(objfolders, SqlDataReader)
                If dr.Read() Then
                    path = dr(0).ToString() + "\" + path
                    levid = dr(1).ToString()
                    Parentid = dr(2).ToString()
                End If
            End While
            path = path + value
            objparamfolder = New SqlParameter(6) {}
            paramfolder = New SqlParameter("@NodeName", value)
            objparamfolder(0) = paramfolder
            paramfolder = New SqlParameter("@ParentNodeId", Parentid)
            objparamfolder(1) = paramfolder
            paramfolder = New SqlParameter("@TemplateId", tempid)
            objparamfolder(2) = paramfolder
            paramfolder = New SqlParameter("@LevelId", Levelid)
            objparamfolder(3) = paramfolder
            paramfolder = New SqlParameter("@PathId", Check)
            objparamfolder(4) = paramfolder
            paramfolder = New SqlParameter("@CreatedOn", Createdon)
            objparamfolder(5) = paramfolder
            paramfolder = New SqlParameter("@CreatedBy", Createdby)
            objparamfolder(6) = paramfolder
            Dim tablename As String = GetTableNameByTemplateId(tempid)
            strqryItems = "INSERT INTO " + tablename + " (TemplateId,[" + Fieldname + "],CreatedOn,CreatedBy,ERSid,Checkoutby,updatedby,isdeleted)values(@Tempid,@Nodename,@Createdon,@Createdby,0,0,0,0) "
            objparamItems = New SqlParameter(3) {}
            paramItems = New SqlParameter("@Tempid", tempid)
            objparamItems(0) = paramItems
            paramItems = New SqlParameter("@Nodename", value)
            objparamItems(1) = paramItems
            paramItems = New SqlParameter("@Createdon", Createdon)
            objparamItems(2) = paramItems
            paramItems = New SqlParameter("@Createdby", Createdby)
            objparamItems(3) = paramItems
            objindex = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString, objparam)
            objfolders = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqryfolder.ToString(), objparamfolder)
            objitems = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqryItems.ToString(), objparamItems)
            If objindex AndAlso objfolders AndAlso objitems Is Nothing Then
                Return Nothing
            Else
                Return path
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
#End Region
#Region "Core"
    Public Sub Read(objRead As IeZIndexingChange)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZIndexingChange ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Indexingchangeid=@Indexingchangeid and ez.Isdeleted=0"
            param = New SqlParameter("@Indexingchangeid", objRead.Indexingchangeid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZIndexingChange")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Indexingchangeid = GetInteger(sqlRdr("Indexingchangeid"))
                objRead.Nodeid = GetInteger(sqlRdr("Nodeid"))
                objRead.oldvalue = sqlRdr("oldvalue").ToString
                objRead.Newvalue = sqlRdr("Newvalue").ToString
                objRead.Parentid = GetInteger(sqlRdr("Parentid"))
                objRead.del = GetInteger(sqlRdr("del"))
                objRead.Levelid = GetInteger(sqlRdr("Levelid"))
                objRead.itemid = GetInteger(sqlRdr("itemid"))
                objRead.Fieldid = GetInteger(sqlRdr("Fieldid"))
                objRead.Templateid = GetInteger(sqlRdr("TemplateId"))
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
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
    Public Function CreateeZIndexingChange(objEmp As eZIndexingChange) As eZIndexingChange
        Dim newObject As eZIndexingChange = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZIndexingChange(TemplateId,Nodeid,oldvalue,Newvalue,Parentid,del,Levelid,itemid,Fieldid,CreatedBy,CreatedOn) VALUES " +
                "(@TemplateId,@Nodeid,@oldvalue,@Newvalue,@Parentid,@del,@Levelid,@itemid,@Fieldid,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(10) {}
            param = New SqlParameter("@TemplateId", objEmp.Templateid)
            objParam(0) = param
            param = New SqlParameter("@Nodeid", objEmp.Nodeid)
            objParam(1) = param
            param = New SqlParameter("@oldvalue", objEmp.oldvalue)
            objParam(2) = param
            param = New SqlParameter("@Newvalue", objEmp.Newvalue)
            objParam(3) = param
            param = New SqlParameter("@Parentid", objEmp.Parentid)
            objParam(4) = param
            param = New SqlParameter("@del", objEmp.del)
            objParam(5) = param
            param = New SqlParameter("@Levelid", objEmp.Levelid)
            objParam(6) = param
            param = New SqlParameter("@itemid", objEmp.itemid)
            objParam(7) = param
            param = New SqlParameter("@Fieldid", objEmp.Fieldid)
            objParam(8) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(9) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(10) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZIndexingChange(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZIndexingChange)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZIndexingChange Set TemplateId=@TemplateId,Nodeid=@Nodeid,oldvalue=@oldvalue,Newvalue=@Newvalue,Parentid=@Parentid," +
            "del=@del,Levelid=@Levelid,itemid=@itemid,Fieldid=@Fieldid,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where Indexingchangeid=@Indexingchangeid"
        objParam = New SqlParameter(11) {}
        param = New SqlParameter("@TemplateId", objToUpdate.Templateid)
        objParam(0) = param
        param = New SqlParameter("@Nodeid", objToUpdate.Nodeid)
        objParam(1) = param
        param = New SqlParameter("@oldvalue", objToUpdate.oldvalue)
        objParam(2) = param
        param = New SqlParameter("@Newvalue", objToUpdate.Newvalue)
        objParam(3) = param
        param = New SqlParameter("@Parentid", objToUpdate.Parentid)
        objParam(4) = param
        param = New SqlParameter("@del", objToUpdate.del)
        objParam(5) = param
        param = New SqlParameter("@Levelid", objToUpdate.Levelid)
        objParam(6) = param
        param = New SqlParameter("@itemid", objToUpdate.itemid)
        objParam(7) = param
        param = New SqlParameter("@Fieldid", objToUpdate.Fieldid)
        objParam(8) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(9) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(10) = param
        param = New SqlParameter("@Indexingchangeid", objToUpdate.Indexingchangeid)
        objParam(11) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZIndexingChange)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZIndexingChange set Isdeleted=1 where Indexingchangeid=@Indexingchangeid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Indexingchangeid", objToDelete.Indexingchangeid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZIndexingChange() As System.Collections.Generic.List(Of IeZIndexingChange)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZIndexingChange)()
        Dim objItem As IeZIndexingChange
        Try
            Dim strQry As String = ""
            strQry = "Select Indexingchangeid From eZIndexingChange where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZIndexingChange")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZIndexingChange(GetInteger(sqlRdr("Indexingchangeid")))
                objItem.Indexingchangeid = GetInteger(sqlRdr("Indexingchangeid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZIndexingChange(Criteria As String, Value As String) As List(Of IeZIndexingChange)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZIndexingChange)()
        Dim objItem As IeZIndexingChange
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Indexingchangeid From eZIndexingChange where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Indexingchangeid"
            Else
                strQry = "Select Indexingchangeid From eZIndexingChange where Isdeleted=0 order by Indexingchangeid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZIndexingChange")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZIndexingChange(GetInteger(sqlRdr("Indexingchangeid")))
                objItem.Indexingchangeid = GetInteger(sqlRdr("Indexingchangeid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZIndexingChange(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZIndexingChange)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZIndexingChange)()
        Dim objItem As IeZIndexingChange
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Indexingchangeid From eZIndexingChange where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Indexingchangeid"
            Else
                strQry = "Select Indexingchangeid From eZIndexingChange where Isdeleted=0 order by Indexingchangeid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZIndexingChange")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZIndexingChange(GetInteger(sqlRdr("Indexingchangeid")))
                objItem.Indexingchangeid = GetInteger(sqlRdr("Indexingchangeid"))
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
