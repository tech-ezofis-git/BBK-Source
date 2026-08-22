Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports ECMAPI
Partial Public Class DBLayer
#Region "BookMarks Details"
    Public Function CreateeZBookMarks(objtemp As eZBookMarks, ByVal Foldername As String) As IeZBookMarks
        Dim newObject As IeZBookMarks = Nothing

        Dim folderid As String = ""
        If String.IsNullOrEmpty(objtemp.BookMarksName) Then
            Return Nothing
        End If
        objtemp.BookMarksName = objtemp.BookMarksName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select BookMarksID,issavedsearch From eZBookMarks Where BookMarksName = @BookMarksName And Isdeleted=0 and issavedsearch=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@BookMarksName", objtemp.BookMarksName)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZBookMarks Code already exist!")
            End If
            Foldername = Foldername.Replace("'", "''")
            'udaya
            If objtemp.IsSavedSearch = False Then
                strQry = "SELECT Folderid FROM eZBookMarksFolder WHERE Foldername='" + Foldername + "' and createdby=" + objtemp.CreatedBy.ToString() + " and isdeleted=0"
                obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString())
                If obj Is Nothing Then
                    Dim strqryfolder As String = "INSERT INTO eZBookMarksFolder(Foldername,Templateid,Createdon,Createdby) values(@foldername,@Templateid,@Createdon,@Createdby)"
                    objParam = New SqlParameter(4) {}
                    param = New SqlParameter("@FolderName", Foldername)
                    objParam(0) = param
                    param = New SqlParameter("@SearchKeyWord", objtemp.SearchKeyWord)
                    objParam(1) = param
                    param = New SqlParameter("@TemplateId", objtemp.TemplateId)
                    objParam(2) = param
                    param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
                    objParam(3) = param
                    param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
                    objParam(4) = param

                    obj = SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqryfolder.ToString(), objParam)
                End If
                strQry = "SELECT Folderid FROM eZBookmarksfolder WHERE foldername=N'" + Foldername + "' and Createdby=" + objtemp.CreatedBy.ToString() + ""
                obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
                If obj Is Nothing Then
                    Return Nothing
                Else
                    Dim dr As SqlDataReader = DirectCast(obj, SqlDataReader)
                    If dr.Read() Then
                        folderid = dr(0).ToString()
                    End If
                End If
            Else
                folderid = 0
            End If
            'udaya
            strQry = "INSERT INTO eZBookMarks(BookMarksName,SearchKeyWord,TemplateId,IsContenSearch,IsSavedSearch,CreatedOn,CreatedBy,folderid) VALUES(@BookMarksName,@SearchKeyWord,@TemplateId,@IsContenSearch,@IsSavedSearch,@CreatedOn,@CreatedBy,@folderid);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@BookMarksName", objtemp.BookMarksName)
            objParam(0) = param
            param = New SqlParameter("@SearchKeyWord", objtemp.SearchKeyWord)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(4) = param
            param = New SqlParameter("@IsContenSearch", objtemp.IsContenSearch)
            objParam(5) = param
            param = New SqlParameter("@IsSavedSearch", objtemp.IsSavedSearch)
            objParam(6) = param
            param = New SqlParameter("@folderid", folderid)
            objParam(7) = param
            'param = New SqlParameter("@Foldername", Foldername)
            'objParam(8) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZBookMarks(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZBookMarks)
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
            If objRead.BookMarksName Is Nothing Then
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZBookMarks where Isdeleted=0 and  BookMarksID=@BookMarksID"
                param = New SqlParameter("@BookMarksID", objRead.BookMarksId)
                objParam(0) = param
            Else
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZBookMarks where Isdeleted=0 and  BookMarksName=@BookMarksName"
                param = New SqlParameter("@BookMarksName", objRead.BookMarksName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarks.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.BookMarksId = GetInteger(sqlRdr("BookMarksID"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.IsContenSearch = sqlRdr("IsContenSearch")
                objRead.IsSavedSearch = sqlRdr("IsSavedSearch")
                objRead.BookMarksName = sqlRdr("BookMarksName").ToString()
                objRead.folderid = sqlRdr("Folderid").ToString()
                objRead.SearchKeyWord = sqlRdr("SearchKeyWord").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                'udaya
                'objRead.folderid = sqlRdr("folder").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZBookMarks.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZBookMarks() As System.Collections.Generic.List(Of IeZBookMarks)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBookMarks)()
        Dim objItem As IeZBookMarks
        Try
            Dim strQry As String = ""
            strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0 order by BookMarksName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarks.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBookMarks(GetSmallInterger(sqlRdr("BookMarksID")))
                objItem.BookMarksId = GetSmallInterger(sqlRdr("BookMarksID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZBookMarks(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZBookMarks)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBookMarks)()
        Dim objItem As IeZBookMarks
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by BookMarksName"
            Else
                strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0  order by BookMarksName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarks.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBookMarks(GetSmallInterger(sqlRdr("BookMarksID")))
                objItem.BookMarksId = GetSmallInterger(sqlRdr("BookMarksID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZBookMarksWithSearch(ByVal BookMarksName As String, ByVal FromDate As String, ByVal ToDate As String, ByVal LoginId As String) As System.Collections.Generic.List(Of IeZBookMarks)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBookMarks)()
        Dim objItem As IeZBookMarks
        Try
            Dim strQry As String = ""
            If BookMarksName = "" And FromDate <> "" And ToDate <> "" Then
                strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0 and CreatedBy=" + LoginId.ToString() + " and "
                strQry = strQry & "  (CreatedOn Between N'" + FromDate + "' And N'" + ToDate + "') order by BookMarksName"
            ElseIf BookMarksName <> "" And FromDate = "" And ToDate = "" Then
                strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0 and CreatedBy=" + LoginId.ToString() + " and BookMarksName "
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(BookMarksName)
                strQry = strQry & "%' "
                strQry = strQry & " order by BookMarksName"
            ElseIf BookMarksName <> "" And FromDate <> "" And ToDate <> "" Then
                strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0 and CreatedBy=" + LoginId.ToString() + " and BookMarksName "
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(BookMarksName)
                strQry = strQry & "%' "
                strQry = strQry & " and (CreatedOn Between N'" + FromDate + "' And N'" + ToDate + "') order by BookMarksName"
            End If
            'strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0 and CreatedBy=" + LoginId.ToString() + " and BookMarksName "
            'strQry = strQry & " like '%"
            'strQry = strQry & Unquote(BookMarksName)
            'strQry = strQry & "%' "
            'strQry = strQry & " or (CreatedOn Between '" + FromDate + "' And '" + ToDate + "') order by BookMarksName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarks.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBookMarks(GetSmallInterger(sqlRdr("BookMarksID")))
                objItem.BookMarksId = GetSmallInterger(sqlRdr("BookMarksID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZBookMarks(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZBookMarks)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBookMarks)()
        Dim objItem As IeZBookMarks
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0 and  "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by BookMarksName"
            Else
                strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0 order by BookMarksName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarks.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBookMarks(GetSmallInterger(sqlRdr("BookMarksID")))
                objItem.BookMarksId = GetSmallInterger(sqlRdr("BookMarksID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZBookMarksByuser(Criteria As String, Value As String, LoginId As String) As System.Collections.Generic.List(Of IeZBookMarks)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBookMarks)()
        Dim objItem As IeZBookMarks
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0 and CreatedBy=" + LoginId + " and  "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by BookMarksName"
            Else
                strQry = "Select BookMarksID From eZBookMarks where Isdeleted=0 and CreatedBy=" + LoginId + " order by BookMarksName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarks.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBookMarks(GetSmallInterger(sqlRdr("BookMarksID")))
                objItem.BookMarksId = GetSmallInterger(sqlRdr("BookMarksID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    'udaya
    Public Function readselectezbookmarksdistinctfoldervalue(ByVal criteria As String, ByVal value As String, ByVal loginid As String) As DataSet
        Try
            Dim strqry As String = ""
            If criteria = "" Then
                strqry = "SELECT DISTINCT(Foldername),folderid FROM eZBookMarksFolder WHERE isdeleted=0 and CreatedBy=" + loginid + ""
            Else
                strqry = "SELECT DISTINCT(Foldername) FROM eZBookMarksfolder WHERE isdeleted=0 and CreatedBy=" + loginid + " and  "
                strqry = strqry & "Convert(varchar(20)," & criteria & ") "
                strqry = strqry & " =N'"
                strqry = strqry & Unquote(value)
                strqry = strqry & "' "
            End If
            Dim ds As DataSet = DBLayer.DBLInstance.GetDatasetByQuery(strqry)
            Return ds
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    'udaya
    Public Function ReadBookmarksnameitemdetailusingfolder(ByVal loginid As String, ByVal folderid As String) As List(Of IeZBookMarks)
        Dim STRQRY As String = ""
        'Dim ds As DataSet

        Dim sqlrdr As SqlDataReader = Nothing

        Dim objitem As eZBookMarks
        Dim lstitems As New System.Collections.Generic.List(Of IeZBookMarks)()
        Try
            STRQRY = "SELECT distinct(Templateid) FROM eZBookMarks WHERE isdeleted=0 and Createdby=N'" + loginid + "' and Folderid=N'" + folderid + "'"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, STRQRY.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarks.")
            End If
            sqlrdr = DirectCast(obj, SqlDataReader)
            While sqlrdr.Read()
                objitem = GlobalInstance.eZBookMarks(GetSmallInterger(sqlrdr("TemplateID")))
                objitem.BookMarksId = GetSmallInterger(sqlrdr("TemplateID"))
                lstitems.Add(objitem)
            End While

            Return lstitems
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    'udaya
    Public Function ReadbookmarksERDpath(ByVal loginid As String, ByVal folderid As String, ByVal tablename As String) As DataSet
        Try
            Dim ds As DataSet
            ds = GetDatasetByQuery("SELECT b.BookMarksId,B.bookmarksname,BD.Itemid,IT.TemplateId,IT.ifilepath,IT.ifilename,IT.ifiletype,IT.version,IT.dtitle,IT.dauthor,IT.dsubject,IT.dkeywords,IT.checkout,IT.checkoutpath,IT.checkoutby,IT.dstatus,IT.dsize,IT.nopages,IT.CreatedOn,IT.UpdatedOn,IT.CreatedBy,IT.UpdatedBy,IT.Isdeleted,IT.ERSid,IT.eZFrom,IT.Encrypt,IT.Password FROM eZBookMarks as b join eZbookmarksdetail as bd on B.BookMarksid=BD.BookMarksid join " + tablename + " as IT on BD.ItemId=IT.itemid WHERE IT.Ifilename<>'' and b.isdeleted=0 and b.Folderid=N'" + folderid + "' and b.Createdby=N'" + loginid + "' and BD.Templateid=IT.Templateid")
            Return ds
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZBookMarks)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select BookMarksID From eZBookMarks Where BookMarksName = @BookMarksName and BookMarksID <> @BookMarksID and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@BookMarksName", objToUpdate.BookMarksName)
        objParam(0) = param
        param = New SqlParameter("@BookMarksID", objToUpdate.BookMarksId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZBookMarks Code already exist!")
        Else
            strQry = "Update eZBookMarks Set IsSavedSearch=@IsSavedSearch,BookMarksName=@BookMarksName,SearchKeyWord=@SearchKeyWord,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy,Folderid=@Folderid where BookMarksID=@BookMarksID"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@BookMarksName", objToUpdate.BookMarksName)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@SearchKeyWord", objToUpdate.SearchKeyWord)
            objParam(2) = param
            param = New SqlParameter("@BookMarksID", objToUpdate.BookMarksId)
            objParam(3) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(4) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(5) = param
            param = New SqlParameter("@IsSavedSearch", objToUpdate.IsSavedSearch)
            objParam(6) = param
            param = New SqlParameter("@Folderid", objToUpdate.folderid)
            objParam(7) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZBookMarks)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim strqry2 As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZBookMarks set Isdeleted=1 where BookMarksID=@BookMarksID"
        strqry2 = "UPDATE eZBookmarksDetail SET isdeleted=1 WHERE BookmarksID=@BookMarksID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@BookMarksID", objToDelete.BookMarksId)
        objParam(0) = param
        SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry2.ToString(), objParam)
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    'udaya
    Public Function folderDelete(ByVal Folderid As String, ByVal Createdby As Integer) As String
        Dim strqry As String = ""
        Dim objparam As SqlParameter()
        Dim param As SqlParameter
        Dim SQLRDR As SqlDataReader
        objparam = New SqlParameter(1) {}
        param = New SqlParameter("@Folderid", Folderid)
        objparam(0) = param
        param = New SqlParameter("@Createdby", Createdby)
        objparam(1) = param
        Try
            Dim strqrybook As String = "SELECT Bookmarksid From eZBookMarks WHERE Folderid=@Folderid and Createdby=@Createdby"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqrybook.ToString(), objparam)
            If obj Is Nothing Then
                Throw New Exception("Invalid")
            End If
            SQLRDR = DirectCast(obj, SqlDataReader)
            While SQLRDR.Read()
                strqry = "Update eZBookMarks SET Isdeleted=1 WHERE Bookmarksid=N'" + SQLRDR(0).ToString() + "'"
                strqrybook = "Update eZBookmarksdetail SET isdeleted=1 WHERE BookMarksid=" + SQLRDR(0).ToString() + ""
                SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqrybook.ToString())
                If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString()) = 0 Then
                    Throw New Exception("")
                End If
            End While

            strqry = "Update eZBookMarksFolder set Isdeleted=1 where Folderid=@Folderid"

            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strqry.ToString(), objparam) = 0 Then
                Throw New Exception("")
            End If
            Return "Folder Deleted"
        Catch ex As Exception
            Return "Nothing"
        End Try
    End Function
#End Region
End Class

