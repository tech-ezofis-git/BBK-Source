Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZECMProfileTemplate)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZECMProfileTemplate ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.ProfileTemplateId=@ProfileTemplateId and ez.Isdeleted=0"
            param = New SqlParameter("@ProfileTemplateId", objRead.ProfileTemplateId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ProfileTemplateId = GetInteger(sqlRdr("ProfileTemplateId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.EcmProfileId = GetInteger(sqlRdr("EcmProfileId"))
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
    Public Function CreateeZECMProfileTemplate(objEmp As eZECMProfileTemplate) As eZECMProfileTemplate
        Dim newObject As eZECMProfileTemplate = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZECMProfileTemplate(TemplateId,EcmProfileId,CreatedBy,CreatedOn) VALUES" +
                "(@TemplateId,@EcmProfileId,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(0) = param
            param = New SqlParameter("@EcmProfileId", objEmp.EcmProfileId)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZECMProfileTemplate(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZECMProfileTemplate)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMProfileTemplate Set TemplateId=@TemplateId,EcmProfileId=@EcmProfileId,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn" +
            " where ProfileTemplateId=@ProfileTemplateId"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(0) = param
        param = New SqlParameter("@EcmProfileId", objToUpdate.EcmProfileId)
        objParam(1) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(2) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(3) = param
        param = New SqlParameter("@ProfileTemplateId", objToUpdate.ProfileTemplateId)
        objParam(4) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMProfileTemplate)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMProfileTemplate set Isdeleted=1 where ProfileTemplateId=@ProfileTemplateId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ProfileTemplateId", objToDelete.ProfileTemplateId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZECMProfileTemplate() As System.Collections.Generic.List(Of IeZECMProfileTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMProfileTemplate)()
        Dim objItem As IeZECMProfileTemplate
        Try
            Dim strQry As String = ""
            strQry = "Select ProfileTemplateId From eZECMProfileTemplate where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Monitor Files")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMProfileTemplate(GetInteger(sqlRdr("ProfileTemplateId")))
                objItem.ProfileTemplateId = GetInteger(sqlRdr("ProfileTemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZECMProfileTemplate(Criteria As String, Value As String) As List(Of IeZECMProfileTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMProfileTemplate)()
        Dim objItem As IeZECMProfileTemplate
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ProfileTemplateId From eZECMProfileTemplate where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ProfileTemplateId"
            Else
                strQry = "Select ProfileTemplateId From eZECMProfileTemplate where Isdeleted=0 order by ProfileTemplateId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Monitor File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMProfileTemplate(GetInteger(sqlRdr("ProfileTemplateId")))
                objItem.ProfileTemplateId = GetInteger(sqlRdr("ProfileTemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMProfileTemplate(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMProfileTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMProfileTemplate)()
        Dim objItem As IeZECMProfileTemplate
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ProfileTemplateId From eZECMProfileTemplate where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ProfileTemplateId"
            Else
                strQry = "Select ProfileTemplateId From eZECMProfileTemplate where Isdeleted=0 order by ProfileTemplateId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Monitor File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMProfileTemplate(GetInteger(sqlRdr("ProfileTemplateId")))
                objItem.ProfileTemplateId = GetInteger(sqlRdr("ProfileTemplateId"))
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
