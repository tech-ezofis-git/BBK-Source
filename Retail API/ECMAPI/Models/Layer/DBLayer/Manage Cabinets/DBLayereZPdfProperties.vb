Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "Pdf Properties Details"


    Public Function CreateeZPdfProperties(objtemp As eZPdfProperties) As IeZPdfProperties
        Dim newObject As IeZPdfProperties = Nothing
        If String.IsNullOrEmpty(objtemp.Subject) Then
            Return Nothing
        End If
        objtemp.Subject = objtemp.Subject.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZPdfProperties(Subject,Author,Title,TemplateId,Sync,Keyword,Signature,CreatedOn,CreatedBy) VALUES(@Subject,@Author,@Title,@TemplateId,@Sync,@Keyword,@Signature,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@Subject", objtemp.Subject)
            objParam(0) = param
            param = New SqlParameter("@Author", objtemp.Author)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(4) = param
            param = New SqlParameter("@Sync", objtemp.Sync)
            objParam(5) = param
            param = New SqlParameter("@Keyword", objtemp.Keyword)
            objParam(6) = param
            param = New SqlParameter("@Title", objtemp.Title)
            objParam(7) = param
            param = New SqlParameter("@Signature", objtemp.Signature)
            objParam(8) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZPdfProperties(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZPdfProperties)
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
            'If objRead.Subject Is Nothing Then
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_Template(TemplateId) as TemplateName,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZPdfProperties Where Isdeleted=0 and PdfId=@PdfId"
            param = New SqlParameter("@PdfId", objRead.PdfId)
            objParam(0) = param
            'Else
            'strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_Template(TemplateId) as TemplateName,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZPdfProperties Where Isdeleted=0 and Subject=@Subject"
            'param = New SqlParameter("@Subject", objRead.Subject)
            'objParam(0) = param
            'End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZPdfProperties.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.PdfId = GetInteger(sqlRdr("PdfId"))
                objRead.Subject = sqlRdr("Subject").ToString()
                objRead.Title = sqlRdr("Title").ToString()
                objRead.Sync = sqlRdr("Sync").ToString()
                objRead.TemplateName = sqlRdr("TemplateName").ToString()
                objRead.TemplateID = GetSmallInterger(sqlRdr("TemplateId"))
                objRead.Author = sqlRdr("Author").ToString()
                objRead.Keyword = sqlRdr("Keyword").ToString()
                objRead.Signature = sqlRdr("Signature").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZPdfProperties.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZPdfProperties() As System.Collections.Generic.List(Of IeZPdfProperties)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZPdfProperties)()
        Dim objItem As IeZPdfProperties
        Try
            Dim strQry As String = ""
            strQry = "Select PdfId From eZPdfProperties where Isdeleted=0 order by Subject"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZPdfProperties.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZPdfProperties(GetSmallInterger(sqlRdr("PdfId")))
                objItem.PdfId = GetSmallInterger(sqlRdr("PdfId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZPdfProperties(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZPdfProperties)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZPdfProperties)()
        Dim objItem As IeZPdfProperties
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select PdfId From eZPdfProperties where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Subject"
            Else
                strQry = "Select PdfId From eZPdfProperties where Isdeleted=0 order by Subject"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZPdfProperties.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZPdfProperties(GetSmallInterger(sqlRdr("PdfId")))
                objItem.PdfId = GetSmallInterger(sqlRdr("PdfId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZPdfProperties(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZPdfProperties)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZPdfProperties)()
        Dim objItem As IeZPdfProperties
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select PdfId From eZPdfProperties where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Subject"
            Else
                strQry = "Select PdfId From eZPdfProperties where Isdeleted=0 order by Subject"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZPdfProperties.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZPdfProperties(GetSmallInterger(sqlRdr("PdfId")))
                objItem.PdfId = GetSmallInterger(sqlRdr("PdfId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZPdfProperties)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        
        strQry = "Update eZPdfProperties Set Signature=@Signature,Subject=@Subject,Title=@Title,Keyword=@Keyword,Sync=@Sync,Author=@Author,TemplateId=@TemplateId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where PdfId=@PdfId"
        objParam = New SqlParameter(9) {}
            param = New SqlParameter("@Subject", objToUpdate.Subject)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objToUpdate.TemplateID)
            objParam(1) = param
            param = New SqlParameter("@Author", objToUpdate.Author)
            objParam(2) = param
            param = New SqlParameter("@PdfId", objToUpdate.PdfId)
            objParam(3) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(4) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(5) = param
            param = New SqlParameter("@Sync", objToUpdate.Sync)
            objParam(6) = param
            param = New SqlParameter("@Keyword", objToUpdate.Keyword)
            objParam(7) = param
            param = New SqlParameter("@Title", objToUpdate.Title)
        objParam(8) = param
        param = New SqlParameter("@Signature", objToUpdate.Signature)
        objParam(9) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")

        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZPdfProperties)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZPdfProperties set Isdeleted=1 where PdfId=@PdfId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@PdfId", objToDelete.PdfId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

