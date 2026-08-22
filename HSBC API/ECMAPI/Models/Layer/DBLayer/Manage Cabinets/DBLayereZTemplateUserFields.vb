Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZTemplateUserFields)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZTemplateUserFields ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.UserFieldId=@UserFieldId and ez.Isdeleted=0"
            param = New SqlParameter("@UserFieldId", objRead.UserFieldId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateUserFields")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.UserFieldId = GetInteger(sqlRdr("UserFieldId"))
                objRead.FieldId = GetInteger(sqlRdr("FieldId"))
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
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
    Public Function CreateeZTemplateUserFields(objEmp As eZTemplateUserFields) As eZTemplateUserFields
        Dim newObject As eZTemplateUserFields = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZTemplateUserFields(FieldId,ECMLoginId,CreatedBy,CreatedOn) VALUES " +
                "(@FieldId,@ECMLoginId,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@FieldId", objEmp.FieldId)
            objParam(0) = param
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZTemplateUserFields(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZTemplateUserFields)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZTemplateUserFields Set FieldId=@FieldId,ECMLoginId=@ECMLoginId," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where UserFieldId=@UserFieldId"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@FieldId", objToUpdate.FieldId)
        objParam(0) = param
        param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        objParam(1) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(2) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(3) = param
        param = New SqlParameter("@UserFieldId", objToUpdate.UserFieldId)
        objParam(4) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZTemplateUserFields)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZTemplateUserFields set Isdeleted=1 where UserFieldId=@UserFieldId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@UserFieldId", objToDelete.UserFieldId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZTemplateUserFields() As System.Collections.Generic.List(Of IeZTemplateUserFields)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplateUserFields)()
        Dim objItem As IeZTemplateUserFields
        Try
            Dim strQry As String = ""
            strQry = "Select UserFieldId From eZTemplateUserFields where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateUserFields")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplateUserFields(GetInteger(sqlRdr("UserFieldId")))
                objItem.UserFieldId = GetInteger(sqlRdr("UserFieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZTemplateUserFields(Criteria As String, Value As String) As List(Of IeZTemplateUserFields)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplateUserFields)()
        Dim objItem As IeZTemplateUserFields
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select UserFieldId From eZTemplateUserFields where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by UserFieldId"
            Else
                strQry = "Select UserFieldId From eZTemplateUserFields where Isdeleted=0 order by UserFieldId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateUserFields")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplateUserFields(GetInteger(sqlRdr("UserFieldId")))
                objItem.UserFieldId = GetInteger(sqlRdr("UserFieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTemplateUserFields(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTemplateUserFields)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplateUserFields)()
        Dim objItem As IeZTemplateUserFields
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select UserFieldId From eZTemplateUserFields where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by UserFieldId"
            Else
                strQry = "Select UserFieldId From eZTemplateUserFields where Isdeleted=0 order by UserFieldId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateUserFields")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplateUserFields(GetInteger(sqlRdr("UserFieldId")))
                objItem.UserFieldId = GetInteger(sqlRdr("UserFieldId"))
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
