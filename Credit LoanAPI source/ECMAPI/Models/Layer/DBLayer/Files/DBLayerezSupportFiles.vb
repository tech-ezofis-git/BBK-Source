Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IezSupportFiles)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From ezSupportFiles ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Attachmentid=@Attachmentid and ez.Isdeleted=0"
            param = New SqlParameter("@Attachmentid", objRead.Attachmentid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezSupportFiles")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Attachmentid = GetInteger(sqlRdr("Attachmentid"))
                objRead.ersid = GetInteger(sqlRdr("ersid"))
                objRead.itemid = GetInteger(sqlRdr("itemid"))
                objRead.templateid = GetInteger(sqlRdr("templateid"))
                objRead.ifilepath = sqlRdr("ifilepath").ToString
                objRead.ifiletype = sqlRdr("ifiletype").ToString
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
    Public Function CreateezSupportFiles(objEmp As ezSupportFiles) As ezSupportFiles
        Dim newObject As ezSupportFiles = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezSupportFiles(ersid,itemid,templateid,ifiletype,ifilepath,CreatedBy,CreatedOn) VALUES " +
                "(@ersid,@itemid,@templateid,@ifiletype,@ifilepath,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@ersid", objEmp.ersid)
            objParam(0) = param
            param = New SqlParameter("@itemid", objEmp.itemid)
            objParam(1) = param
            param = New SqlParameter("@templateid", objEmp.templateid)
            objParam(2) = param
            param = New SqlParameter("@ifiletype", objEmp.ifiletype)
            objParam(3) = param
            param = New SqlParameter("@ifilepath", objEmp.ifilepath)
            objParam(4) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(6) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezSupportFiles(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IezSupportFiles)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezSupportFiles Set ersid=@ersid,itemid=@itemid,templateid=@templateid,ifiletype=@ifiletype,ifilepath=@ifilepath," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where Attachmentid=@Attachmentid"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@ersid", objToUpdate.ersid)
        objParam(0) = param
        param = New SqlParameter("@itemid", objToUpdate.itemid)
        objParam(1) = param
        param = New SqlParameter("@templateid", objToUpdate.templateid)
        objParam(2) = param
        param = New SqlParameter("@ifiletype", objToUpdate.ifiletype)
        objParam(3) = param
        param = New SqlParameter("@ifilepath", objToUpdate.ifilepath)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(6) = param
        param = New SqlParameter("@Attachmentid", objToUpdate.Attachmentid)
        objParam(7) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IezSupportFiles)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezSupportFiles set Isdeleted=1 where Attachmentid=@Attachmentid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Attachmentid", objToDelete.Attachmentid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllezSupportFiles() As System.Collections.Generic.List(Of IezSupportFiles)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezSupportFiles)()
        Dim objItem As IezSupportFiles
        Try
            Dim strQry As String = ""
            strQry = "Select Attachmentid From ezSupportFiles where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezSupportFiles")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezSupportFiles(GetInteger(sqlRdr("Attachmentid")))
                objItem.Attachmentid = GetInteger(sqlRdr("Attachmentid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredezSupportFiles(Criteria As String, Value As String) As List(Of IezSupportFiles)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezSupportFiles)()
        Dim objItem As IezSupportFiles
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Attachmentid From ezSupportFiles where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Attachmentid"
            Else
                strQry = "Select Attachmentid From ezSupportFiles where Isdeleted=0 order by Attachmentid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezSupportFiles")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezSupportFiles(GetInteger(sqlRdr("Attachmentid")))
                objItem.Attachmentid = GetInteger(sqlRdr("Attachmentid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedezSupportFiles(Criteria As String, Value As String) As System.Collections.Generic.List(Of IezSupportFiles)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezSupportFiles)()
        Dim objItem As IezSupportFiles
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Attachmentid From ezSupportFiles where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Attachmentid"
            Else
                strQry = "Select Attachmentid From ezSupportFiles where Isdeleted=0 order by Attachmentid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezSupportFiles")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezSupportFiles(GetInteger(sqlRdr("Attachmentid")))
                objItem.Attachmentid = GetInteger(sqlRdr("Attachmentid"))
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
