Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
#Region "Template Field Details"
    Public Function CreateeZTemplateField(objtemp As eZTemplateField) As IeZTemplateField
        Dim newObject As IeZTemplateField = Nothing
        If String.IsNullOrEmpty(objtemp.FieldName) Then
            Return Nothing
        End If
        objtemp.FieldName = objtemp.FieldName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select FieldId From eZTemplateField Where FieldName = @FieldName and TemplateId=@TemplateId And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@FieldName", objtemp.FieldName)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZTemplateField Code already exist!")
            End If
            strQry = "INSERT INTO eZTemplateField(FieldName,FieldLevel,TemplateId,DataTypeId,Mandatory,CreatedOn,CreatedBy,IsEditable) " +
                "VALUES(@FieldName,@FieldLevel,@TemplateId,@DataTypeId,@Mandatory,@CreatedOn,@CreatedBy,@IsEditable);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@FieldName", objtemp.FieldName)
            objParam(0) = param
            param = New SqlParameter("@FieldLevel", objtemp.FieldLevel)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(4) = param
            param = New SqlParameter("@DataTypeId", objtemp.DataTypeId)
            objParam(5) = param
            param = New SqlParameter("@Mandatory", objtemp.Mandatory)
            objParam(6) = param
            param = New SqlParameter("@IsEditable", objtemp.IsEditable)
            objParam(7) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZTemplateField(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZTemplateField)
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
            If objRead.FieldName Is Nothing Then
                strQry = "Select *,dbo.udf_TableName(TemplateId) as TableName,dbo.udf_UserName(UpdatedBy) as UpdatedBy1," +
                    "dbo.udf_Template(TemplateId) as TemplateName,dbo.udf_DataType(DataTypeId) as DataType,dbo.udf_DataTypeDT(DataTypeId) as DT," +
                    "dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZTemplateField Where Isdeleted=0 and FieldId=@FieldId"
                param = New SqlParameter("@FieldId", objRead.FieldId)
                objParam(0) = param
            Else
                strQry = "Select *,dbo.udf_TableName(TemplateId) as TableName,dbo.udf_UserName(UpdatedBy) as UpdatedBy1," +
                    "dbo.udf_Template(TemplateId) as TemplateName,dbo.udf_DataType(DataTypeId) as DataType,dbo.udf_DataTypeDT(DataTypeId) as DT," +
                    "dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZTemplateField Where Isdeleted=0 and FieldName=@FieldName"
                param = New SqlParameter("@FieldName", objRead.FieldName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.FieldId = GetInteger(sqlRdr("FieldId"))
                objRead.FieldName = sqlRdr("FieldName").ToString()
                objRead.DataType = sqlRdr("DataType").ToString()
                objRead.TableName = sqlRdr("TableName").ToString()
                objRead.DT = sqlRdr("DT").ToString()
                objRead.DataTypeId = sqlRdr("DataTypeId").ToString()
                objRead.TemplateName = sqlRdr("TemplateName").ToString()
                objRead.TemplateID = GetSmallInterger(sqlRdr("TemplateId"))
                objRead.FieldLevel = sqlRdr("FieldLevel").ToString()
                If sqlRdr("Mandatory").ToString = "True" Then
                    objRead.Mandatory = True
                Else
                    objRead.Mandatory = False
                End If
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                If sqlRdr("IsEditable").ToString = "True" Then
                    objRead.IsEditable = True
                Else
                    objRead.IsEditable = False
                End If
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
    Public Function ReadAlleZTemplateField() As System.Collections.Generic.List(Of IeZTemplateField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplateField)()
        Dim objItem As IeZTemplateField
        Try
            Dim strQry As String = ""
            strQry = "Select FieldId From eZTemplateField where Isdeleted=0 order by FieldName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplateField(GetSmallInterger(sqlRdr("FieldId")))
                objItem.FieldId = GetSmallInterger(sqlRdr("FieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZTemplateField(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTemplateField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplateField)()
        Dim objItem As IeZTemplateField
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldId From eZTemplateField where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by FieldName"
            Else
                strQry = "Select FieldId From eZTemplateField where Isdeleted=0 order by FieldName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplateField(GetSmallInterger(sqlRdr("FieldId")))
                objItem.FieldId = GetSmallInterger(sqlRdr("FieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadGethighestversion(ByVal tempid As String, ByVal itemid As String, ByVal loginid As String, ByVal tablename1 As String, ByVal tablename2 As String) As DataSet
        Try
            Dim strqry As String = "SELECT distinct H.itemid,H.ifilename,H.version,(case when H.ifilename=I.ifilename then N'true' else N'false' end) as versiontype,H.dsize,H.CreatedOn from " + tablename1 + " as I join " + tablename2 + " as H on H.itemid=I.itemid WHERE i.ifilename<>'' and I.Itemid=" + itemid + " and H.Createdby=" + loginid + " and I.templateid=" + tempid + ""
            Dim ds As DataSet = DBLayer.DBLInstance.GetDatasetByQuery(strqry)
            Return ds
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Public Function ReadSelectedeZTemplateField(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTemplateField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplateField)()
        Dim objItem As IeZTemplateField
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldId From eZTemplateField where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(100)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & "ORDER BY CASE WHEN Fieldlevel<>0 THEN 1 ELSE 2 END,Fieldlevel ASC,mandatory DESC"
            Else
                strQry = "Select FieldId From eZTemplateField where Isdeleted=0 ORDER BY CASE WHEN Fieldlevel<>0 THEN 1 ELSE 2 END,Fieldlevel ASC,mandatory DESC"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplateField(GetSmallInterger(sqlRdr("FieldId")))
                objItem.FieldId = GetSmallInterger(sqlRdr("FieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTemplateFieldForGCC(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTemplateField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplateField)()
        Dim objItem As IeZTemplateField
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldId From eZTemplateField where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(100)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by indexingorder"
            Else
                strQry = "Select FieldId From eZTemplateField where Isdeleted=0 order by indexingorder"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplateField(GetSmallInterger(sqlRdr("FieldId")))
                objItem.FieldId = GetSmallInterger(sqlRdr("FieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTemplateFieldWithTemplateId(Criteria As String, Value As String, TemplateId As String) As System.Collections.Generic.List(Of IeZTemplateField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplateField)()
        Dim objItem As IeZTemplateField
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldId From eZTemplateField where Isdeleted=0 and TemplateID=N'" + TemplateId + "' and "
                strQry = strQry & "Convert(varchar(100)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FieldLevel"
            Else
                strQry = "Select FieldId From eZTemplateField where Isdeleted=0 and TemplateID=N'" + TemplateId + "' order by FieldLevel"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplateField(GetSmallInterger(sqlRdr("FieldId")))
                objItem.FieldId = GetSmallInterger(sqlRdr("FieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTemplateFieldForPdfCreation(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTemplateField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplateField)()
        Dim objItem As IeZTemplateField
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldId From eZTemplateField where Isdeleted=0 and FieldLevel<>0 and "
                strQry = strQry & "Convert(varchar(100)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FieldLevel"
            Else
                strQry = "Select FieldId From eZTemplateField where Isdeleted=0 and FieldLevel<>0 order by FieldLevel"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplateField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplateField(GetSmallInterger(sqlRdr("FieldId")))
                objItem.FieldId = GetSmallInterger(sqlRdr("FieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZTemplateField)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZTemplateField Set Fieldname=@Fieldname,Mandatory=@Mandatory,DataTypeId=@DataTypeId,FieldLevel=@FieldLevel,TemplateId=@TemplateId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where FieldId=@FieldId"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@Mandatory", objToUpdate.Mandatory)
        objParam(0) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateID)
        objParam(1) = param
        param = New SqlParameter("@FieldLevel", objToUpdate.FieldLevel)
        objParam(2) = param
        param = New SqlParameter("@FieldId", objToUpdate.FieldId)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@DataTypeId", objToUpdate.DataTypeId)
        objParam(6) = param
        param = New SqlParameter("@Fieldname", objToUpdate.FieldName)
        objParam(7) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error1")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZTemplateField)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZTemplateField set Isdeleted=1 where FieldId=@FieldId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@FieldId", objToDelete.FieldId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region

End Class

