Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZHideFile)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From ezhidefile " +
                "Where hidefileid=@hidefileid and Isdeleted=0"
            param = New SqlParameter("@hidefileid", objRead.HideFileId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.HideFileId = GetInteger(sqlRdr("HideFileId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.ItemId = GetInteger(sqlRdr("ItemId"))
                objRead.HideAlways = GetInteger(sqlRdr("HideAlways"))
                objRead.FromDate = sqlRdr("FromDate").ToString
                objRead.ToDate = sqlRdr("ToDate").ToString
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

    Public Function CreateeZHideFile(objEmp As eZHideFile) As eZHideFile
        Dim newObject As eZHideFile = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezhidefile(templateid,itemid,hidealways,fromdate,todate,CreatedBy,CreatedOn) VALUES" +
                "(@templateid,@itemid,@hidealways,@fromdate,@todate,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@templateid", objEmp.TemplateId)
            objParam(2) = param
            param = New SqlParameter("@itemid", objEmp.ItemId)
            objParam(3) = param
            param = New SqlParameter("@hidealways", objEmp.HideAlways)
            objParam(4) = param
            param = New SqlParameter("@fromdate", objEmp.FromDate)
            objParam(5) = param
            param = New SqlParameter("@todate", objEmp.ToDate)
            objParam(6) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            'obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZHideFile(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZHideFile)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezhidefile Set templateid=@templateid,itemid=@itemid,fromdate=@fromdate,todate=@todate,hidealways=@hidealways," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where hidefileid=@hidefileid"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@templateid", objToUpdate.TemplateId)
        objParam(0) = param
        param = New SqlParameter("@itemid", objToUpdate.ItemId)
        objParam(1) = param
        param = New SqlParameter("@fromdate", objToUpdate.FromDate)
        objParam(2) = param
        param = New SqlParameter("@todate", objToUpdate.ToDate)
        objParam(3) = param
        param = New SqlParameter("@hidealways", objToUpdate.HideAlways)
        objParam(4) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(5) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(6) = param
        param = New SqlParameter("@hidefileid", objToUpdate.HideFileId)
        objParam(7) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZHideFile)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezhidefile set Isdeleted=1 where hidefileid=@hidefileid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@hidefileid", objToDelete.HideFileId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region

    Public Function ReadAlleZHideFile() As System.Collections.Generic.List(Of IeZHideFile)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZHideFile)()
        Dim objItem As IeZHideFile
        Try
            Dim strQry As String = ""
            strQry = "Select hidefileid From ezhidefile where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide Files")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZHideFile(GetInteger(sqlRdr("hidefileid")))
                objItem.HideFileId = GetInteger(sqlRdr("hidefileid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZHideFile(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZHideFile)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZHideFile)()
        Dim objItem As IeZHideFile

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select hidefileid From ezhidefile where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by templateid, itemid"
            Else
                strQry = "Select hidefileid From ezhidefile where Isdeleted=0 order by templateid, itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZHideFile(GetInteger(sqlRdr("hidefileid")))
                objItem.HideFileId = GetInteger(sqlRdr("hidefileid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZHideFile(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZHideFile)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZHideFile)()
        Dim objItem As IeZHideFile

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select hidefileid From ezhidefile where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by templateid, itemid"
            Else
                strQry = "Select hidefileid From ezhidefile where Isdeleted=0 order by templateid, itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZHideFile(GetInteger(sqlRdr("hidefileid")))
                objItem.HideFileId = GetInteger(sqlRdr("hidefileid"))
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
