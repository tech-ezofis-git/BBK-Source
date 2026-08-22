Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateAlternateField(objEmp As eZAlternateField) As IeZAlternateField
        Dim newObject As IeZAlternateField = Nothing
        If String.IsNullOrEmpty(objEmp.AlternateId) Then
            Return Nothing
        End If
        objEmp.AlternateId = objEmp.AlternateId
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select AlternateId From eZAlternateField Where FieldId = @AlternateFieldid and Alternatefieldid=@FieldId And TemplateId=@TemplateId And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@FieldId", objEmp.FieldId)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@AlternateFieldId", objEmp.AlternateFieldId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("Alternateid already exist!")
            End If
            strQry = "Select AlternateId From eZAlternateField Where FieldId = @FieldId and Alternatefieldid=@AlternateFieldid and FieldValue=@FieldValue And TemplateId=@TemplateId And Isdeleted=0"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@FieldId", objEmp.FieldId)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@FieldValue", objEmp.FieldValue)
            objParam(2) = param
            param = New SqlParameter("@AlternateFieldId", objEmp.AlternateFieldId)
            objParam(3) = param
            Dim obj1 As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj1 IsNot Nothing Then
                Throw New Exception("Alternateid already exist!")
            End If
            strQry = "INSERT INTO eZAlternateField(FieldId,AlternateFieldId,FieldValue,AlternateValue,TemplateId,LastNo,CreatedOn,CreatedBy) VALUES(@FieldId,@AlternateFieldId,@FieldValue,@AlternateValue,@TemplateID,@LastNo,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@FieldId", objEmp.FieldId)
            objParam(0) = param
            param = New SqlParameter("@AlternateFieldId", objEmp.AlternateFieldId)
            objParam(1) = param
            param = New SqlParameter("@FieldValue", objEmp.FieldValue)
            objParam(2) = param
            param = New SqlParameter("@AlternateValue", objEmp.AlternateValue)
            objParam(3) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(4) = param
            param = New SqlParameter("@LastNo", objEmp.LastNo)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(6) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(7) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZAlternateField(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZAlternateField)
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
            If objRead.FieldId = 0 Then
                strQry = "Select *,dbo.udf_FieldName(FieldId) as FieldName,dbo.udf_FieldName(AlternateFieldId) as AlternateFieldName,dbo.udf_Template(TemplateID) as TemplateName From eZAlternateField Where AlternateId=@Alternate_ID And Isdeleted=0"
                objParam = New SqlParameter(1) {}
                param = New SqlParameter("@Alternate_ID", objRead.AlternateId)
                objParam(0) = param
                param = New SqlParameter("@LastNo", objRead.LastNo)
                objParam(1) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_FieldName(FieldId) as FieldName,dbo.udf_FieldName(AlternateFieldId) as AlternateFieldName,dbo.udf_Template(TemplateID) as TemplateName From eZAlternateField Where AlternateId=@AlternateId And Isdeleted=0"
                param = New SqlParameter("@AlternateId", objRead.AlternateId)
                objParam(0) = param
                param = New SqlParameter("@LastNo", objRead.LastNo)
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FieldName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.AlternateId = GetInteger(sqlRdr("AlternateId"))
                objRead.FieldId = GetInteger(sqlRdr("FieldId"))
                objRead.AlternateFieldId = GetInteger(sqlRdr("AlternateFieldId"))
                objRead.TemplateName = sqlRdr("TemplateName").ToString()
                objRead.FieldName = sqlRdr("FieldName").ToString()
                objRead.AlternateFieldName = sqlRdr("AlternateFieldName").ToString()
                objRead.FieldValue = sqlRdr("FieldValue").ToString()
                objRead.AlternateValue = sqlRdr("AlternateValue").ToString()
                objRead.TemplateID = sqlRdr("TemplateID").ToString()
                objRead.LastNo = sqlRdr("LastNo").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
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
    Public Function ReadAllAlternateField() As System.Collections.Generic.List(Of IeZAlternateField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAlternateField)()
        Dim objItem As IeZAlternateField

        Try
            Dim strQry As String = ""
            strQry = "Select AlternateId From eZAlternateField where Isdeleted=0 order by FieldId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FieldId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAlternateField(GetInteger(sqlRdr("AlternateId")))
                objItem.AlternateId = GetInteger(sqlRdr("AlternateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedAlterNateField(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZAlternateField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAlternateField)()
        Dim objItem As IeZAlternateField
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select * From ezAlternateField where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select * From ezAlternateField where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FieldName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAlternateField(GetSmallInterger(sqlRdr("AlternateId")))
                objItem.AlternateId = GetSmallInterger(sqlRdr("AlternateId"))
                objItem.LastNo = sqlRdr("LastNo")
                objItem.FieldId = GetSmallInterger(sqlRdr("FieldId"))
                objItem.AlternateFieldId = GetSmallInterger(sqlRdr("AlternateFieldId"))
                objItem.FieldValue = sqlRdr("FieldValue")
                objItem.AlternateValue = sqlRdr("AlternateValue")
                objItem.TemplateID = sqlRdr("TemplateID")
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedAlterNateFieldByCondition(Condition As String) As System.Collections.Generic.List(Of IeZAlternateField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAlternateField)()
        Dim objItem As IeZAlternateField
        Try
            Dim strQry As String = ""
            If Condition <> "" Then
                strQry = "Select * From ezAlternateField where Isdeleted=0 and "
                strQry = strQry & Condition
                strQry = strQry & " order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FieldName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAlternateField(GetSmallInterger(sqlRdr("AlternateId")))
                objItem.AlternateId = GetSmallInterger(sqlRdr("AlternateId"))
                objItem.FieldId = GetSmallInterger(sqlRdr("FieldId"))
                objItem.AlternateFieldId = GetSmallInterger(sqlRdr("AlternateFieldId"))
                objItem.LastNo = sqlRdr("LastNo")
                objItem.FieldValue = sqlRdr("FieldValue")
                objItem.AlternateValue = sqlRdr("AlternateValue")
                objItem.TemplateID = sqlRdr("TemplateID")
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZAlternateField)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select AlternateId From eZAlternateField Where FieldId = @FieldId and FieldValue = @FieldValue and AlternateId <> @AlternateId and Isdeleted=0"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@FieldId", objToUpdate.FieldId)
        objParam(0) = param
        param = New SqlParameter("@AlternateId", objToUpdate.AlternateId)
        objParam(1) = param
        param = New SqlParameter("@AlternateFieldId", objToUpdate.AlternateFieldId)
        objParam(2) = param
        param = New SqlParameter("@FieldValue", objToUpdate.FieldValue)
        objParam(3) = param
        param = New SqlParameter("@AlternateValue", objToUpdate.AlternateValue)
        objParam(4) = param
        param = New SqlParameter("@TemplateID", objToUpdate.TemplateID)
        objParam(5) = param
        param = New SqlParameter("@LastNo", objToUpdate.LastNo)
        objParam(6) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(7) = param
        param = New SqlParameter("@Updatedby", objToUpdate.UpdatedBy)
        objParam(8) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("Field Name already exist!")
        Else
            strQry = "Update eZAlternateField Set FieldId=@FieldId,AlternateFieldId=@AlternateFieldId,FieldValue=@FieldValue,AlternateValue=@AlternateValue,TemplateId=@TemplateId,LastNo=@LastNo,Updatedon=@Updatedon,UpdatedBy=@UpdatedBy where AlternateID=@Alternateid"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@FieldId", objToUpdate.FieldId)
            objParam(0) = param
            param = New SqlParameter("@AlternateID", objToUpdate.AlternateId)
            objParam(1) = param
            param = New SqlParameter("@AlternateFieldID", objToUpdate.AlternateFieldId)
            objParam(2) = param
            param = New SqlParameter("@FieldValue", objToUpdate.FieldValue)
            objParam(3) = param
            param = New SqlParameter("@AlternateValue", objToUpdate.AlternateValue)
            objParam(4) = param
            param = New SqlParameter("@TemplateID", objToUpdate.TemplateID)
            objParam(5) = param
            param = New SqlParameter("@LastNo", objToUpdate.LastNo)
            objParam(6) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(7) = param
            param = New SqlParameter("@Updatedby", objToUpdate.UpdatedBy)
            objParam(8) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(AlternateId As Integer)
        If AlternateId = 0 Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezAlternateField set Isdeleted=1 where AlternateId=@AlternateID "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Alternateid", AlternateId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class
