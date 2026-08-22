Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IezImpersonation)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From ezImpersonation ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.ImpersonateId=@ImpersonateId and ez.Isdeleted=0"
            param = New SqlParameter("@ImpersonateId", objRead.ImpersonateId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezImpersonation")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ImpersonateId = GetInteger(sqlRdr("ImpersonateId"))
                objRead.ImpersonationFor = sqlRdr("ImpersonationFor").ToString
                objRead.Domain = sqlRdr("Domain").ToString()
                objRead.Username = sqlRdr("Username").ToString
                objRead.Password = sqlRdr("Password").ToString()
                objRead.ERSid = GetInteger(sqlRdr("ERSid"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.Description = sqlRdr("Description").ToString()
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
    Public Function CreateezImpersonation(objEmp As ezImpersonation) As ezImpersonation
        Dim newObject As ezImpersonation = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezImpersonation(ImpersonationFor,Domain,Username,Password,ERSid,TemplateId,Description,CreatedBy,CreatedOn) VALUES " +
                "(@ImpersonationFor,@Domain,@Username,@Password,@ERSid,@TemplateId,@Description,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@ImpersonationFor", objEmp.ImpersonationFor)
            objParam(0) = param
            param = New SqlParameter("@Domain", objEmp.Domain)
            objParam(1) = param
            param = New SqlParameter("@Username", objEmp.Username)
            objParam(2) = param
            param = New SqlParameter("@Password", objEmp.Password)
            objParam(3) = param
            param = New SqlParameter("@ERSid", objEmp.ERSid)
            objParam(4) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(5) = param
            param = New SqlParameter("@Description", objEmp.Description)
            objParam(6) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(7) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(8) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezImpersonation(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IezImpersonation)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezImpersonation Set ImpersonationFor=@ImpersonationFor,Domain=@Domain,Username=@Username,Password=@Password," +
            "ERSid=@ERSid,TemplateId=@TemplateId,Description=@Description,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where ImpersonateId=@ImpersonateId"
        objParam = New SqlParameter(9) {}
        param = New SqlParameter("@ImpersonationFor", objToUpdate.ImpersonationFor)
        objParam(0) = param
        param = New SqlParameter("@Domain", objToUpdate.Domain)
        objParam(1) = param
        param = New SqlParameter("@Username", objToUpdate.Username)
        objParam(2) = param
        param = New SqlParameter("@Password", objToUpdate.Password)
        objParam(3) = param
        param = New SqlParameter("@ERSid", objToUpdate.ERSid)
        objParam(4) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(5) = param
        param = New SqlParameter("@Description", objToUpdate.Description)
        objParam(6) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(7) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(8) = param
        param = New SqlParameter("@ImpersonateId", objToUpdate.ImpersonateId)
        objParam(9) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IezImpersonation)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezImpersonation set Isdeleted=1 where ImpersonateId=@ImpersonateId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ImpersonateId", objToDelete.ImpersonateId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllezImpersonation() As System.Collections.Generic.List(Of IezImpersonation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezImpersonation)()
        Dim objItem As IezImpersonation
        Try
            Dim strQry As String = ""
            strQry = "Select ImpersonateId From ezImpersonation where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezImpersonation")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezImpersonation(GetInteger(sqlRdr("ImpersonateId")))
                objItem.ImpersonateId = GetInteger(sqlRdr("ImpersonateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredezImpersonation(Criteria As String, Value As String) As List(Of IezImpersonation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezImpersonation)()
        Dim objItem As IezImpersonation
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ImpersonateId From ezImpersonation where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ImpersonateId"
            Else
                strQry = "Select ImpersonateId From ezImpersonation where Isdeleted=0 order by ImpersonateId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezImpersonation")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezImpersonation(GetInteger(sqlRdr("ImpersonateId")))
                objItem.ImpersonateId = GetInteger(sqlRdr("ImpersonateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedezImpersonation(Criteria As String, Value As String) As System.Collections.Generic.List(Of IezImpersonation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezImpersonation)()
        Dim objItem As IezImpersonation
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ImpersonateId From ezImpersonation where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ImpersonateId"
            Else
                strQry = "Select ImpersonateId From ezImpersonation where Isdeleted=0 order by ImpersonateId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezImpersonation")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezImpersonation(GetInteger(sqlRdr("ImpersonateId")))
                objItem.ImpersonateId = GetInteger(sqlRdr("ImpersonateId"))
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
