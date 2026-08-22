Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZUnProcessedFiles)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZUnProcessedFiles ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.UnprocessId=@UnprocessId and ez.Isdeleted=0"
            param = New SqlParameter("@UnprocessId", objRead.UnprocessId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Unprocess File")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.UnprocessId = GetInteger(sqlRdr("UnprocessId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.Status = GetInteger(sqlRdr("Status"))
                objRead.FilePath = sqlRdr("FilePath").ToString
                objRead.FileName = sqlRdr("FileName").ToString
                objRead.FileExtension = sqlRdr("FileExtension").ToString
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.Issue = sqlRdr("Issue").ToString
                objRead.ProcessedFrom = sqlRdr("ProcessedFrom").ToString
                objRead.ReprocessPath = sqlRdr("ReprocessPath").ToString
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
    Public Function CreateeZUnProcessedFiles(objEmp As eZUnProcessedFiles) As eZUnProcessedFiles
        Dim newObject As eZUnProcessedFiles = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZUnProcessedFiles(FilePath,FileName,FileExtension,Status,Issue,CreatedBy,CreatedOn,ProcessedFrom,ReprocessPath,TemplateId) VALUES" +
                "(@FilePath,@FileName,@FileExtension,@Status,@Issue,@CreatedBy,@CreatedOn,@ProcessedFrom,@ReprocessPath,@TemplateId);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(9) {}
            param = New SqlParameter("@FilePath", objEmp.FilePath)
            objParam(0) = param
            param = New SqlParameter("@FileName", objEmp.FileName)
            objParam(1) = param
            param = New SqlParameter("@FileExtension", objEmp.FileExtension)
            objParam(2) = param
            param = New SqlParameter("@Status", objEmp.Status)
            objParam(3) = param
            param = New SqlParameter("@Issue", objEmp.Issue)
            objParam(4) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(6) = param
            param = New SqlParameter("@ProcessedFrom", objEmp.ProcessedFrom)
            objParam(7) = param
            param = New SqlParameter("@ReprocessPath", objEmp.ReprocessPath)
            objParam(8) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(9) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZUnProcessedFiles(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZUnProcessedFiles)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZUnProcessedFiles Set FilePath=@FilePath,FileName=@FileName,FileExtension=@FileExtension,Status=@Status," +
            "Issue=@Issue,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,ProcessedFrom=@ProcessedFrom,ReprocessPath=@ReprocessPath," +
            "TemplateId=@TemplateId where UnprocessId=@UnprocessId"
        objParam = New SqlParameter(10) {}
        param = New SqlParameter("@FilePath", objToUpdate.FilePath)
        objParam(0) = param
        param = New SqlParameter("@FileName", objToUpdate.FileName)
        objParam(1) = param
        param = New SqlParameter("@FileExtension", objToUpdate.FileExtension)
        objParam(2) = param
        param = New SqlParameter("@Status", objToUpdate.Status)
        objParam(3) = param
        param = New SqlParameter("@Issue", objToUpdate.Issue)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(6) = param
        param = New SqlParameter("@ProcessedFrom", objToUpdate.ProcessedFrom)
        objParam(7) = param
        param = New SqlParameter("@ReprocessPath", objToUpdate.ReprocessPath)
        objParam(8) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(9) = param
        param = New SqlParameter("@UnprocessId", objToUpdate.UnprocessId)
        objParam(10) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZUnProcessedFiles)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZUnProcessedFiles set Isdeleted=1 where UnprocessId=@UnprocessId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@UnprocessId", objToDelete.UnprocessId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZUnProcessedFiles() As System.Collections.Generic.List(Of IeZUnProcessedFiles)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZUnProcessedFiles)()
        Dim objItem As IeZUnProcessedFiles
        Try
            Dim strQry As String = ""
            strQry = "Select UnprocessId From eZUnProcessedFiles where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Unprocess Files")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZUnProcessedFiles(GetInteger(sqlRdr("UnprocessId")))
                objItem.UnprocessId = GetInteger(sqlRdr("UnprocessId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZUnProcessedFiles(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZUnProcessedFiles)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZUnProcessedFiles)()
        Dim objItem As IeZUnProcessedFiles
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select UnprocessId From eZUnProcessedFiles where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by UnprocessId"
            Else
                strQry = "Select UnprocessId From eZUnProcessedFiles where Isdeleted=0 order by UnprocessId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Unprocess File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZUnProcessedFiles(GetInteger(sqlRdr("UnprocessId")))
                objItem.UnprocessId = GetInteger(sqlRdr("UnprocessId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZUnProcessedFiles(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZUnProcessedFiles)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZUnProcessedFiles)()
        Dim objItem As IeZUnProcessedFiles
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select UnprocessId From eZUnProcessedFiles where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by UnprocessId"
            Else
                strQry = "Select UnprocessId From eZUnProcessedFiles where Isdeleted=0 order by UnprocessId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Unprocess File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZUnProcessedFiles(GetInteger(sqlRdr("UnprocessId")))
                objItem.UnprocessId = GetInteger(sqlRdr("UnprocessId"))
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