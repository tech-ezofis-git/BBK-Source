Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "eZLookupSPparameters Details"


    Public Function CreateeZLookupSPparameters(objtemp As eZLookupSPparameters) As IeZLookupSPparameters
        Dim newObject As IeZLookupSPparameters = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select LookupSPparamId From eZLookupSPparameters Where  LookupId=@LookupId and ECMField=@ECMField  and Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@LookupId", objtemp.LookupId)
            objParam(0) = param
            param = New SqlParameter("@ECMField", objtemp.ECMField)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZLookupSPparameters Code already exist!")
            End If
            strQry = "INSERT INTO eZLookupSPparameters(LookupId,ECMField,ParameterName,IsOutputParameterDirection,VariableDataType,CreatedOn,CreatedBy) VALUES(@LookupId,@ECMField,@ParameterName,@IsOutputParameterDirection,@VariableDataType,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@LookupId", objtemp.LookupId)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@ParameterName", objtemp.ParameterName)
            objParam(3) = param
            param = New SqlParameter("@ECMField", objtemp.ECMField)
            objParam(4) = param
            param = New SqlParameter("@IsOutputParameterDirection", objtemp.IsOutputParameterDirection)
            objParam(5) = param
            param = New SqlParameter("@VariableDataType", objtemp.VariableDataType)
            objParam(6) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZLookupSPparameters(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZLookupSPparameters)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZLookupSPparameters Where Isdeleted=0 and LookupSPparamId=@LookupSPparamId"
            param = New SqlParameter("@LookupSPparamId", objRead.LookupSPparamId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupSPparameters.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.LookupSPparamId = GetInteger(sqlRdr("LookupSPparamId"))
                objRead.ECMField = sqlRdr("ECMField").ToString
                objRead.LookupId = GetInteger(sqlRdr("LookupId"))
                objRead.ParameterName = sqlRdr("ParameterName").ToString
                objRead.VariableDataType = sqlRdr("VariableDataType").ToString
                objRead.IsOutputParameterDirection = GetSmallInterger(sqlRdr("IsOutputParameterDirection").ToString)
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZLookupSPparameters.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZLookupSPparameters() As System.Collections.Generic.List(Of IeZLookupSPparameters)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupSPparameters)()
        Dim objItem As IeZLookupSPparameters
        Try
            Dim strQry As String = ""
            strQry = "Select LookupSPparamId From eZLookupSPparameters where Isdeleted=0 order by LookupSPparamId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupSPparameters.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupSPparameters(GetSmallInterger(sqlRdr("LookupSPparamId")))
                objItem.LookupSPparamId = GetSmallInterger(sqlRdr("LookupSPparamId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZLookupSPparameters(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLookupSPparameters)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupSPparameters)()
        Dim objItem As IeZLookupSPparameters
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupSPparamId From eZLookupSPparameters where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like '%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by LookupSPparamId"
            Else
                strQry = "Select LookupSPparamId From eZLookupSPparameters where Isdeleted=0 order by LookupSPparamId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupSPparameters.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupSPparameters(GetSmallInterger(sqlRdr("LookupSPparamId")))
                objItem.LookupSPparamId = GetSmallInterger(sqlRdr("LookupSPparamId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZLookupSPparameters(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLookupSPparameters)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupSPparameters)()
        Dim objItem As IeZLookupSPparameters
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupSPparamId From eZLookupSPparameters where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by LookupSPparamId"
            Else
                strQry = "Select LookupSPparamId From eZLookupSPparameters where Isdeleted=0 order by LookupSPparamId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupSPparameters.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupSPparameters(GetSmallInterger(sqlRdr("LookupSPparamId")))
                objItem.LookupSPparamId = GetSmallInterger(sqlRdr("LookupSPparamId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZLookupSPparameters)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        'strQry = "Select LookupSPparamId From eZLookupSPparameters Where LookupId = @LookupId and LookupSPparamId <> @LookupSPparamId and Isdeleted=0"
        'objParam = New SqlParameter(1) {}
        'param = New SqlParameter("@LookupId", objToUpdate.LookupId)
        'objParam(0) = param
        'param = New SqlParameter("@LookupSPparamId", objToUpdate.LookupSPparamId)
        'objParam(1) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("eZLookupSPparameters Code already exist!")
        'Else
        strQry = "Update eZLookupSPparameters Set VariableDataType=@VariableDataType,LookupId=@LookupId,IsOutputParameterDirection=@IsOutputParameterDirection,ECMField=@ECMField,ParameterName=@ParameterName,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where LookupSPparamId=@LookupSPparamId"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@LookupId", objToUpdate.LookupId)
        objParam(0) = param
        param = New SqlParameter("@IsOutputParameterDirection", objToUpdate.IsOutputParameterDirection)
        objParam(1) = param
        param = New SqlParameter("@ECMField", objToUpdate.ECMField)
        objParam(2) = param
        param = New SqlParameter("@ParameterName", objToUpdate.ParameterName)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@LookupSPparamId", objToUpdate.LookupSPparamId)
        objParam(6) = param
        param = New SqlParameter("@VariableDataType", objToUpdate.VariableDataType)
        objParam(7) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")

        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZLookupSPparameters)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLookupSPparameters set Isdeleted=1 where LookupSPparamId=@LookupSPparamId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LookupSPparamId", objToDelete.LookupSPparamId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

