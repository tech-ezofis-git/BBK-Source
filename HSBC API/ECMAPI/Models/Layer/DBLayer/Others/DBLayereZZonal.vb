Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary

Partial Public Class DBLayer
    Public Function CreateZonal(objEmp As eZZonal) As IeZZonal
        Dim newObject As IeZZonal = Nothing
        If String.IsNullOrEmpty(objEmp.ZonalName) Then
            Return Nothing
        End If
        objEmp.ZonalName = objEmp.ZonalName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ZonalId From eZZonal Where ZonalName = @ZonalName And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ZonalName", objEmp.ZonalName)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("Zonal Name already exist!")
            End If
            strQry = "INSERT INTO eZZonal(ZonalName,CabinetId,TemplateId,CreatedOn,CreatedBy,ProcessName,CreatedFrom) " +
                "VALUES(@ZonalName,@CabinetId,@TemplateId,@CreatedOn,@CreatedBy,@ProcessName,@CreatedFrom);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@ZonalName", objEmp.ZonalName)
            objParam(0) = param
            param = New SqlParameter("@CabinetId", objEmp.CabinetId)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            param = New SqlParameter("@ProcessName", objEmp.ProcessName)
            objParam(5) = param
            param = New SqlParameter("@CreatedFrom", objEmp.CreatedFrom)
            objParam(6) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZZonal(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZZonal)
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
            If objRead.ZonalName Is Nothing Then
                strQry = "Select *,dbo.udf_Cabinet(CabinetId) as CabinetName,dbo.udf_Template(TemplateId) as TemplateName" +
                    " From eZZonal Where ZonalId=@Zonal_ID and Isdeleted=0"
                objParam = New SqlParameter(0) {}
                param = New SqlParameter("@Zonal_ID", objRead.ZonalId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(0) {}
                strQry = "Select *,dbo.udf_Cabinet(CabinetId) as CabinetName,dbo.udf_Template(TemplateId) as TemplateName " +
                    "From eZZonal Where ZonalName=@ZonalName and Isdeleted=0"
                param = New SqlParameter("@ZonalName", objRead.ZonalName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ZonalName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ZonalId = GetInteger(sqlRdr("ZonalId"))
                objRead.ZonalName = sqlRdr("ZonalName").ToString()
                objRead.TemplateName = sqlRdr("TemplateName").ToString()
                objRead.CabinetName = sqlRdr("CabinetName").ToString()
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.CabinetId = GetInteger(sqlRdr("CabinetId"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                objRead.ProcessName = sqlRdr("ProcessName").ToString
                objRead.CreatedFrom = sqlRdr("CreatedFrom").ToString
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
    Public Function ReadAllZonal() As System.Collections.Generic.List(Of IeZZonal)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZZonal)()
        Dim objItem As IeZZonal

        Try
            Dim strQry As String = ""
            strQry = "Select ZonalId From eZZonal where Isdeleted=0 order by ZonalName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ZonalName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZZonal(GetInteger(sqlRdr("ZonalId")))
                objItem.ZonalId = GetInteger(sqlRdr("ZonalId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedZonal(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZZonal)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZZonal)()
        Dim objItem As IeZZonal
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ZonalId From ezZonal where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select ZonalId From Zonal where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ZonalName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZZonal(GetSmallInterger(sqlRdr("ZonalId")))
                objItem.ZonalId = GetSmallInterger(sqlRdr("ZonalId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedZonalByTemplateId(Criteria As String) As System.Collections.Generic.List(Of IeZZonal)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZZonal)()
        Dim objItem As IeZZonal
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ZonalId,ZonalName,dbo.udf_Cabinet(CabinetId) as CabinetName,dbo.udf_Template(TemplateId) as TemplateName From ezZonal where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select ZonalId,ZonalName,dbo.udf_Cabinet(CabinetId) as CabinetName,dbo.udf_Template(TemplateId) as TemplateName From Zonal where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ZonalName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZZonal(GetSmallInterger(sqlRdr("ZonalId")))
                objItem.ZonalId = GetSmallInterger(sqlRdr("ZonalId"))
                objItem.ZonalName = sqlRdr("ZonalName").ToString
                objItem.CabinetName = sqlRdr("CabinetName").ToString
                objItem.TemplateName = sqlRdr("TemplateName").ToString
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZZonal)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ZonalID From eZZonal Where ZonalName = @ZonalName and Isdeleted=0"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ZonalName", objToUpdate.ZonalName)
        objParam(0) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj Is Nothing Then
            Throw New Exception("Zonal Name doesn't exist!")
        Else
            strQry = "Update eZZonal Set ZonalName=@ZonalName,CabinetId=@CabinetId,TemplateId=@TemplateId,CreatedFrom=@CreatedFrom," +
                "ProcessName=@ProcessName,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ZonalID=@Zonal_ID"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@ZonalName", objToUpdate.ZonalName)
            objParam(0) = param
            param = New SqlParameter("@Zonal_ID", objToUpdate.ZonalId)
            objParam(1) = param
            param = New SqlParameter("@CabinetId", objToUpdate.CabinetId)
            objParam(2) = param
            param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
            objParam(3) = param
            param = New SqlParameter("@CreatedFrom", objToUpdate.CreatedFrom)
            objParam(4) = param
            param = New SqlParameter("@ProcessName", objToUpdate.ProcessName)
            objParam(5) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(6) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(7) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZZonal)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZZonal set Isdeleted=1 where ZonalId=@Zonal_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Zonal_ID", objToDelete.ZonalId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class