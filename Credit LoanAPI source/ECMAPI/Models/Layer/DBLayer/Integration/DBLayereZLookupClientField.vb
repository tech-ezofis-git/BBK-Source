Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "eZLookupClientField Details"


    Public Function CreateeZLookupClientField(objtemp As eZLookupClientField) As IeZLookupClientField
        Dim newObject As IeZLookupClientField = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select LookupClientFieldId From eZLookupClientField Where  LookupId=@LookupId and ClientField=@ClientField  and Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@LookupId", objtemp.LookupId)
            objParam(0) = param
            param = New SqlParameter("@ClientField", objtemp.ClientField)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZLookupClientField Code already exist!")
            End If
            strQry = "INSERT INTO eZLookupClientField(LookupId,ClientField,CreatedOn,CreatedBy,ClientFieldValues) " +
                "VALUES(@LookupId,@ClientField,@CreatedOn,@CreatedBy,@ClientFieldValues);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@LookupId", objtemp.LookupId)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@ClientField", objtemp.ClientField)
            objParam(3) = param
            param = New SqlParameter("@ClientFieldValues", objtemp.ClientFieldValues)
            objParam(4) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZLookupClientField(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZLookupClientField)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  " +
                "From eZLookupClientField Where Isdeleted=0 and LookupClientFieldId=@LookupClientFieldId"
            param = New SqlParameter("@LookupClientFieldId", objRead.LookupClientFieldId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupClientField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.LookupClientFieldId = GetInteger(sqlRdr("LookupClientFieldId"))
                objRead.ClientFieldValues = sqlRdr("ClientFieldValues").ToString
                objRead.LookupId = GetInteger(sqlRdr("LookupId"))
                objRead.ClientField = sqlRdr("ClientField").ToString
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZLookupClientField.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZLookupClientField() As System.Collections.Generic.List(Of IeZLookupClientField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupClientField)()
        Dim objItem As IeZLookupClientField
        Try
            Dim strQry As String = ""
            strQry = "Select LookupClientFieldId From eZLookupClientField where Isdeleted=0 order by LookupClientFieldId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupClientField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupClientField(GetSmallInterger(sqlRdr("LookupClientFieldId")))
                objItem.LookupClientFieldId = GetSmallInterger(sqlRdr("LookupClientFieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZLookupClientField(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLookupClientField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupClientField)()
        Dim objItem As IeZLookupClientField
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupClientFieldId From eZLookupClientField where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like '%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by LookupClientFieldId"
            Else
                strQry = "Select LookupClientFieldId From eZLookupClientField where Isdeleted=0 order by LookupClientFieldId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupClientField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupClientField(GetSmallInterger(sqlRdr("LookupClientFieldId")))
                objItem.LookupClientFieldId = GetSmallInterger(sqlRdr("LookupClientFieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZLookupClientField(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLookupClientField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupClientField)()
        Dim objItem As IeZLookupClientField
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupClientFieldId From eZLookupClientField where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by LookupClientFieldId"
            Else
                strQry = "Select LookupClientFieldId From eZLookupClientField where Isdeleted=0 order by LookupClientFieldId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupClientField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupClientField(GetSmallInterger(sqlRdr("LookupClientFieldId")))
                objItem.LookupClientFieldId = GetSmallInterger(sqlRdr("LookupClientFieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZLookupClientFieldWithLookupId(Criteria As String, Value As String, LookupId As String) As System.Collections.Generic.List(Of IeZLookupClientField)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupClientField)()
        Dim objItem As IeZLookupClientField
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupClientFieldId From eZLookupClientField where Isdeleted=0  and LookupId=" + LookupId + " and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by LookupClientFieldId"
            Else
                strQry = "Select LookupClientFieldId From eZLookupClientField where Isdeleted=0 order by LookupClientFieldId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupClientField.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupClientField(GetSmallInterger(sqlRdr("LookupClientFieldId")))
                objItem.LookupClientFieldId = GetSmallInterger(sqlRdr("LookupClientFieldId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZLookupClientField)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        'strQry = "Select LookupClientFieldId From eZLookupClientField Where LookupId = @LookupId and LookupClientFieldId <> @LookupClientFieldId and Isdeleted=0"
        'objParam = New SqlParameter(1) {}
        'param = New SqlParameter("@LookupId", objToUpdate.LookupId)
        'objParam(0) = param
        'param = New SqlParameter("@LookupClientFieldId", objToUpdate.LookupClientFieldId)
        'objParam(1) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("eZLookupClientField Code already exist!")
        'Else
        strQry = "Update eZLookupClientField Set LookupId=@LookupId,ClientField=@ClientField,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy," +
            "ClientFieldValues=@ClientFieldValues where LookupClientFieldId=@LookupClientFieldId"
        objParam = New SqlParameter(5) {}
        param = New SqlParameter("@LookupId", objToUpdate.LookupId)
        objParam(0) = param
        param = New SqlParameter("@ClientField", objToUpdate.ClientField)
        objParam(1) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(2) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(3) = param
        param = New SqlParameter("@LookupClientFieldId", objToUpdate.LookupClientFieldId)
        objParam(4) = param
        param = New SqlParameter("@ClientFieldValues", objToUpdate.ClientFieldValues)
        objParam(5) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZLookupClientField)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLookupClientField set Isdeleted=1 where LookupClientFieldId=@LookupClientFieldId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LookupClientFieldId", objToDelete.LookupClientFieldId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

