Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IezScannedImg)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From ezScannedImg ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.ScannedImgId=@ScannedImgId and ez.Isdeleted=0"
            param = New SqlParameter("@ScannedImgId", objRead.ScannedImgId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezScannedImg")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Status = GetInteger(sqlRdr("Status"))
                objRead.ScannedImgId = GetInteger(sqlRdr("ScannedImgId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.nopages = GetInteger(sqlRdr("nopages"))
                objRead.Ifilepath = sqlRdr("Ifilepath").ToString
                objRead.appname = sqlRdr("appname").ToString
                objRead.pcname = sqlRdr("pcname").ToString
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
    Public Function CreateezScannedImg(objEmp As ezScannedImg) As ezScannedImg
        Dim newObject As ezScannedImg = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezScannedImg(Ifilepath,TemplateId,Status,pcname,appname,nopages,CreatedBy,CreatedOn) VALUES " +
                "(@Ifilepath,@TemplateId,@Status,@pcname,@appname,@nopages,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@Ifilepath", objEmp.Ifilepath)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@Status", objEmp.Status)
            objParam(2) = param
            param = New SqlParameter("@pcname", objEmp.pcname)
            objParam(3) = param
            param = New SqlParameter("@appname", objEmp.appname)
            objParam(4) = param
            param = New SqlParameter("@nopages", objEmp.nopages)
            objParam(5) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(6) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezScannedImg(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IezScannedImg)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezScannedImg Set Ifilepath=@Ifilepath,TemplateId=@TemplateId,Status=@Status,pcname=@pcname," +
            "appname=@appname,nopages=@nopages,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where ScannedImgId=@ScannedImgId"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@Ifilepath", objToUpdate.Ifilepath)
        objParam(0) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(1) = param
        param = New SqlParameter("@Status", objToUpdate.Status)
        objParam(2) = param
        param = New SqlParameter("@pcname", objToUpdate.pcname)
        objParam(3) = param
        param = New SqlParameter("@appname", objToUpdate.appname)
        objParam(4) = param
        param = New SqlParameter("@nopages", objToUpdate.nopages)
        objParam(5) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(6) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(7) = param
        param = New SqlParameter("@ScannedImgId", objToUpdate.ScannedImgId)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IezScannedImg)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezScannedImg set Isdeleted=1 where ScannedImgId=@ScannedImgId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ScannedImgId", objToDelete.ScannedImgId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllezScannedImg() As System.Collections.Generic.List(Of IezScannedImg)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezScannedImg)()
        Dim objItem As IezScannedImg
        Try
            Dim strQry As String = ""
            strQry = "Select ScannedImgId From ezScannedImg where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezScannedImg")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezScannedImg(GetInteger(sqlRdr("ScannedImgId")))
                objItem.ScannedImgId = GetInteger(sqlRdr("ScannedImgId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredezScannedImg(Criteria As String, Value As String) As List(Of IezScannedImg)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezScannedImg)()
        Dim objItem As IezScannedImg
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ScannedImgId From ezScannedImg where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ScannedImgId"
            Else
                strQry = "Select ScannedImgId From ezScannedImg where Isdeleted=0 order by ScannedImgId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezScannedImg")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezScannedImg(GetInteger(sqlRdr("ScannedImgId")))
                objItem.ScannedImgId = GetInteger(sqlRdr("ScannedImgId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedezScannedImg(Criteria As String, Value As String) As System.Collections.Generic.List(Of IezScannedImg)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezScannedImg)()
        Dim objItem As IezScannedImg
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ScannedImgId From ezScannedImg where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ScannedImgId"
            Else
                strQry = "Select ScannedImgId From ezScannedImg where Isdeleted=0 order by ScannedImgId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezScannedImg")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezScannedImg(GetInteger(sqlRdr("ScannedImgId")))
                objItem.ScannedImgId = GetInteger(sqlRdr("ScannedImgId"))
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
