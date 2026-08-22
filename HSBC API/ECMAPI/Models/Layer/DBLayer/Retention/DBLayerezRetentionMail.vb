Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IezRetentionMail)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From ezRetentionMail ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.RetMailId=@RetMailId and ez.Isdeleted=0"
            param = New SqlParameter("@RetMailId", objRead.RetMailId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezRetentionMail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.RetMailId = GetInteger(sqlRdr("RetMailId"))
                objRead.RetentionId = GetInteger(sqlRdr("RetentionId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.MailTo = sqlRdr("MailTo").ToString
                objRead.ItemId = GetInteger(sqlRdr("ItemId"))
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
    Public Function CreateezRetentionMail(objEmp As ezRetentionMail) As ezRetentionMail
        Dim newObject As ezRetentionMail = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezRetentionMail (RetentionId,ItemId,TemplateId,MailTo,CreatedBy,CreatedOn) " +
                "VALUES (@RetentionId,@ItemId,@TemplateId,@MailTo,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@RetentionId", objEmp.RetentionId)
            objParam(0) = param
            param = New SqlParameter("@ItemId", objEmp.ItemId)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(2) = param
            param = New SqlParameter("@MailTo", objEmp.MailTo)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(4) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(5) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezRetentionMail(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IezRetentionMail)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezRetentionMail Set " +
           "RetentionId=@RetentionId,ItemId=@ItemId,TemplateId=@TemplateId,MailTo=@MailTo,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where RetMailId=@RetMailId"
        objParam = New SqlParameter(6) {}
        param = New SqlParameter("@RetentionId", objToUpdate.RetentionId)
        objParam(0) = param
        param = New SqlParameter("@ItemId", objToUpdate.ItemId)
        objParam(1) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(2) = param
        param = New SqlParameter("@MailTo", objToUpdate.MailTo)
        objParam(3) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(4) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(5) = param
        param = New SqlParameter("@RetMailId", objToUpdate.RetMailId)
        objParam(6) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IezRetentionMail)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezRetentionMail set Isdeleted=1,updatedby=@updatedby,updatedon=@updatedon where RetMailId=@RetMailId"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@RetMailId", objToDelete.RetMailId)
        objParam(0) = param
        param = New SqlParameter("@updatedby", objToDelete.Updatedby)
        objParam(1) = param
        param = New SqlParameter("@updatedon", objToDelete.Updatedon)
        objParam(2) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllezRetentionMail() As System.Collections.Generic.List(Of IezRetentionMail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezRetentionMail)()
        Dim objItem As IezRetentionMail
        Try
            Dim strQry As String = ""
            strQry = "Select RetMailId From ezRetentionMail where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezRetentionMail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezRetentionMail(GetInteger(sqlRdr("RetMailId")))
                objItem.RetMailId = GetInteger(sqlRdr("RetMailId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredezRetentionMail(Criteria As String, Value As String) As List(Of IezRetentionMail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezRetentionMail)()
        Dim objItem As IezRetentionMail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select RetMailId From ezRetentionMail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by RetMailId"
            Else
                strQry = "Select RetMailId From ezRetentionMail where Isdeleted=0 order by RetMailId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezRetentionMail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezRetentionMail(GetInteger(sqlRdr("RetMailId")))
                objItem.RetMailId = GetInteger(sqlRdr("RetMailId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedezRetentionMail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IezRetentionMail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezRetentionMail)()
        Dim objItem As IezRetentionMail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select RetMailId From ezRetentionMail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by RetMailId"
            Else
                strQry = "Select RetMailId From ezRetentionMail where Isdeleted=0 order by RetMailId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezRetentionMail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezRetentionMail(GetInteger(sqlRdr("RetMailId")))
                objItem.RetMailId = GetInteger(sqlRdr("RetMailId"))
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
