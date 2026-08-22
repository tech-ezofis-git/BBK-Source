Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "eZLookupFields Details"


    Public Function CreateeZLookupFields(objtemp As eZLookupFields) As IeZLookupFields
        Dim newObject As IeZLookupFields = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select LookupFieldId From eZLookupFields Where  LookupId=@LookupId and ECMField=@ECMField  and Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@LookupId", objtemp.LookupId)
            objParam(0) = param
            param = New SqlParameter("@ECMField", objtemp.ECMField)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZLookupFields Code already exist!")
            End If
            strQry = "INSERT INTO eZLookupFields(LookupId,ECMField,ClientField,IsSyncField,parameterorder,CreatedOn,CreatedBy) VALUES(@LookupId,@ECMField,@ClientField,@IsSyncField,@parameterorder,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@LookupId", objtemp.LookupId)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@ClientField", objtemp.ClientField)
            objParam(3) = param
            param = New SqlParameter("@ECMField", objtemp.ECMField)
            objParam(4) = param
            param = New SqlParameter("@IsSyncField", objtemp.IsSyncField)
            objParam(5) = param
            param = New SqlParameter("@parameterorder", objtemp.ParameterOrder)
            objParam(6) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZLookupFields(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZLookupFields)
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
            strQry = "Select *,dbo.UDF_Getcabinetidbylookupid(lookupid) as cabinetid,dbo.UDF_Gettemplateidbylookupid(lookupid) as Templateid ,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZLookupFields Where Isdeleted=0 and LookupFieldId=@LookupFieldId"
            param = New SqlParameter("@LookupFieldId", objRead.LookupFieldId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupFields.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.LookupFieldId = GetInteger(sqlRdr("LookupFieldId"))
                objRead.ECMField = sqlRdr("ECMField").ToString
                objRead.LookupId = GetInteger(sqlRdr("LookupId"))
                objRead.ClientField = sqlRdr("ClientField").ToString
                objRead.Cabinetid = sqlRdr("cabinetid").ToString()
                objRead.Templateid = sqlRdr("Templateid").ToString()

                If sqlRdr("IsSyncField").ToString = "True" Then
                    objRead.IsSyncField = True
                Else
                    objRead.IsSyncField = False
                End If

                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                objRead.ParameterOrder = GetInteger(sqlRdr("ParameterOrder"))
            Else
                'throw new Exception("Attempt to read Invalid eZLookupFields.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZLookupFields() As System.Collections.Generic.List(Of IeZLookupFields)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupFields)()
        Dim objItem As IeZLookupFields
        Try
            Dim strQry As String = ""
            strQry = "Select LookupFieldId From eZLookupFields where Isdeleted=0 order by LookupFieldId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupFields.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupFields(GetSmallInterger(sqlRdr("LookupFieldId")))
                objItem.LookupFieldId = GetSmallInterger(sqlRdr("LookupFieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZLookupFields(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLookupFields)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupFields)()
        Dim objItem As IeZLookupFields
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupFieldId From eZLookupFields where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by LookupFieldId"
            Else
                strQry = "Select LookupFieldId From eZLookupFields where Isdeleted=0 order by LookupFieldId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupFields.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupFields(GetSmallInterger(sqlRdr("LookupFieldId")))
                objItem.LookupFieldId = GetSmallInterger(sqlRdr("LookupFieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZLookupFields(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLookupFields)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupFields)()
        Dim objItem As IeZLookupFields
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupFieldId From eZLookupFields where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(200)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by LookupFieldId"
            Else
                strQry = "Select LookupFieldId From eZLookupFields where Isdeleted=0 order by LookupFieldId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupFields.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupFields(GetSmallInterger(sqlRdr("LookupFieldId")))
                objItem.LookupFieldId = GetSmallInterger(sqlRdr("LookupFieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZLookupFieldsWithLookupId(Criteria As String, Value As String, LookupId As String) As System.Collections.Generic.List(Of IeZLookupFields)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupFields)()
        Dim objItem As IeZLookupFields
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupFieldId From eZLookupFields where Isdeleted=0  and LookupId=" + LookupId + " and "
                strQry = strQry & "Convert(nvarchar(200)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by LookupFieldId"
            Else
                strQry = "Select LookupFieldId From eZLookupFields where Isdeleted=0 order by LookupFieldId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupFields.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupFields(GetSmallInterger(sqlRdr("LookupFieldId")))
                objItem.LookupFieldId = GetSmallInterger(sqlRdr("LookupFieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZLookupFields)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        'strQry = "Select LookupFieldId From eZLookupFields Where LookupId = @LookupId and LookupFieldId <> @LookupFieldId and Isdeleted=0"
        'objParam = New SqlParameter(1) {}
        'param = New SqlParameter("@LookupId", objToUpdate.LookupId)
        'objParam(0) = param
        'param = New SqlParameter("@LookupFieldId", objToUpdate.LookupFieldId)
        'objParam(1) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("eZLookupFields Code already exist!")
        'Else
        strQry = "Update eZLookupFields Set LookupId=@LookupId,IsSyncField=@IsSyncField,ECMField=@ECMField,ClientField=@ClientField,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy,parameterorder=@parameterorder where LookupFieldId=@LookupFieldId"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@LookupId", objToUpdate.LookupId)
        objParam(0) = param
        param = New SqlParameter("@IsSyncField", objToUpdate.IsSyncField)
        objParam(1) = param
        param = New SqlParameter("@ECMField", objToUpdate.ECMField)
        objParam(2) = param
        param = New SqlParameter("@ClientField", objToUpdate.ClientField)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@LookupFieldId", objToUpdate.LookupFieldId)
        objParam(6) = param
        param = New SqlParameter("@parameterorder", objToUpdate.ParameterOrder)
        objParam(7) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")

        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZLookupFields)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLookupFields set Isdeleted=1 where LookupFieldId=@LookupFieldId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LookupFieldId", objToDelete.LookupFieldId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

