Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZScanBatch)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZScanBatch ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.BatchId=@BatchId and ez.Isdeleted=0"
            param = New SqlParameter("@BatchId", objRead.BatchId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScanBatch")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Status = GetInteger(sqlRdr("Status"))
                objRead.BatchId = GetInteger(sqlRdr("BatchId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.NoOfDocument = GetInteger(sqlRdr("NoOfDocument"))
                objRead.Batch = sqlRdr("Batch").ToString
                objRead.CreatedAt = sqlRdr("CreatedAt").ToString
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
    Public Function CreateeZScanBatch(objEmp As eZScanBatch) As eZScanBatch
        Dim newObject As eZScanBatch = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZScanBatch(Batch,TemplateId,Status,NoOfDocument,CreatedAt,CreatedBy,CreatedOn) VALUES " +
                "(@Batch,@TemplateId,@Status,@NoOfDocument,@CreatedAt,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@Batch", objEmp.Batch)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@Status", objEmp.Status)
            objParam(2) = param
            param = New SqlParameter("@NoOfDocument", objEmp.NoOfDocument)
            objParam(3) = param
            param = New SqlParameter("@CreatedAt", objEmp.CreatedAt)
            objParam(4) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(6) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZScanBatch(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZScanBatch)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZScanBatch Set Batch=@Batch,TemplateId=@TemplateId,Status=@Status,NoOfDocument=@NoOfDocument," +
            "CreatedAt=@CreatedAt,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where BatchId=@BatchId"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@Batch", objToUpdate.Batch)
        objParam(0) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(1) = param
        param = New SqlParameter("@Status", objToUpdate.Status)
        objParam(2) = param
        param = New SqlParameter("@NoOfDocument", objToUpdate.NoOfDocument)
        objParam(3) = param
        param = New SqlParameter("@CreatedAt", objToUpdate.CreatedAt)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(6) = param
        param = New SqlParameter("@BatchId", objToUpdate.BatchId)
        objParam(7) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZScanBatch)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZScanBatch set Isdeleted=1 where BatchId=@BatchId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@BatchId", objToDelete.BatchId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZScanBatch() As System.Collections.Generic.List(Of IeZScanBatch)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScanBatch)()
        Dim objItem As IeZScanBatch
        Try
            Dim strQry As String = ""
            strQry = "Select BatchId From eZScanBatch where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScanBatch")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScanBatch(GetInteger(sqlRdr("BatchId")))
                objItem.BatchId = GetInteger(sqlRdr("BatchId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZScanBatch(Criteria As String, Value As String) As List(Of IeZScanBatch)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScanBatch)()
        Dim objItem As IeZScanBatch
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select BatchId From eZScanBatch where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by BatchId"
            Else
                strQry = "Select BatchId From eZScanBatch where Isdeleted=0 order by BatchId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScanBatch")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScanBatch(GetInteger(sqlRdr("BatchId")))
                objItem.BatchId = GetInteger(sqlRdr("BatchId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZScanBatch(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZScanBatch)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScanBatch)()
        Dim objItem As IeZScanBatch
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select BatchId From eZScanBatch where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by BatchId"
            Else
                strQry = "Select BatchId From eZScanBatch where Isdeleted=0 order by BatchId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScanBatch")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScanBatch(GetInteger(sqlRdr("BatchId")))
                objItem.BatchId = GetInteger(sqlRdr("BatchId"))
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
