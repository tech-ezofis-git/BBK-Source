Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZMapFields)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZMapFields ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Mapfieldsid=@Mapfieldsid and ez.Isdeleted=0"
            param = New SqlParameter("@Mapfieldsid", objRead.Mapfieldsid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMapFields")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Cabinetid = GetInteger(sqlRdr("Cabinetid"))
                objRead.Mapfieldsid = GetInteger(sqlRdr("Mapfieldsid"))
                objRead.Templateid = GetInteger(sqlRdr("TemplateId"))
                objRead.LocationField = sqlRdr("LocationField").ToString
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
    Public Function CreateeZMapFields(objEmp As eZMapFields) As eZMapFields
        Dim newObject As eZMapFields = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZMapFields(Cabinetid,TemplateId,LocationField,CreatedBy,CreatedOn) VALUES " +
                "(@Cabinetid,@TemplateId,@LocationField,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@Cabinetid", objEmp.Cabinetid)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objEmp.Templateid)
            objParam(1) = param
            param = New SqlParameter("@LocationField", objEmp.LocationField)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(4) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZMapFields(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZMapFields)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMapFields Set Cabinetid=@Cabinetid,TemplateId=@TemplateId,LocationField=@LocationField,UpdatedBy=@UpdatedBy," +
            "UpdatedOn=@UpdatedOn where Mapfieldsid=@Mapfieldsid"
        objParam = New SqlParameter(5) {}
        param = New SqlParameter("@Cabinetid", objToUpdate.Cabinetid)
        objParam(0) = param
        param = New SqlParameter("@TemplateId", objToUpdate.Templateid)
        objParam(1) = param
        param = New SqlParameter("@LocationField", objToUpdate.LocationField)
        objParam(2) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@Mapfieldsid", objToUpdate.Mapfieldsid)
        objParam(5) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZMapFields)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMapFields set Isdeleted=1 where Mapfieldsid=@Mapfieldsid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Mapfieldsid", objToDelete.Mapfieldsid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZMapFields() As System.Collections.Generic.List(Of IeZMapFields)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMapFields)()
        Dim objItem As IeZMapFields
        Try
            Dim strQry As String = ""
            strQry = "Select Mapfieldsid From eZMapFields where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMapFields")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMapFields(GetInteger(sqlRdr("Mapfieldsid")))
                objItem.Mapfieldsid = GetInteger(sqlRdr("Mapfieldsid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZMapFields(Criteria As String, Value As String) As List(Of IeZMapFields)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMapFields)()
        Dim objItem As IeZMapFields
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Mapfieldsid From eZMapFields where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Mapfieldsid"
            Else
                strQry = "Select Mapfieldsid From eZMapFields where Isdeleted=0 order by Mapfieldsid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMapFields")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMapFields(GetInteger(sqlRdr("Mapfieldsid")))
                objItem.Mapfieldsid = GetInteger(sqlRdr("Mapfieldsid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMapFields(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMapFields)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMapFields)()
        Dim objItem As IeZMapFields
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Mapfieldsid From eZMapFields where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Mapfieldsid"
            Else
                strQry = "Select Mapfieldsid From eZMapFields where Isdeleted=0 order by Mapfieldsid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMapFields")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMapFields(GetInteger(sqlRdr("Mapfieldsid")))
                objItem.Mapfieldsid = GetInteger(sqlRdr("Mapfieldsid"))
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
