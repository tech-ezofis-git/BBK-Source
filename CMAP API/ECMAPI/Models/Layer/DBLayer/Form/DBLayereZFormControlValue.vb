Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateeZFormControlValue(objEmp As eZFormControlValue) As IeZFormControlValue
        Dim newObject As IeZFormControlValue = Nothing
        If String.IsNullOrEmpty(objEmp.ControlValue) Then
            Return Nothing
        End If
        objEmp.ControlValue = objEmp.ControlValue.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ControlValueId From eZFormControlValue Where ControlValue = @ControlValue And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ControlValue", objEmp.ControlValue)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ControlValue Code already exist!")
            End If
            strQry = "INSERT INTO eZFormControlValue(ControlValue,FormControlId,RefControlId,RefControlValueId,CreatedOn,CreatedBy) VALUES(@ControlValue,@FormControlId,@RefControlId,@RefControlValueId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@ControlValue", objEmp.ControlValue)
            objParam(0) = param
            param = New SqlParameter("@FormControlId", objEmp.FormControlId)
            objParam(1) = param
            param = New SqlParameter("@RefControlId", objEmp.RefControlId)
            objParam(2) = param
            param = New SqlParameter("@RefControlValueId", objEmp.RefControlValueId)
            objParam(3) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(4) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(5) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZFormControlValue(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFormControlValue)
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
            If objRead.ControlValue Is Nothing Then

                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFormControlValue Where ControlValueId=@ControlValue_ID and Isdeleted=0"
                param = New SqlParameter("@ControlValue_ID", objRead.ControlValueId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFormControlValue Where ControlValue=@ControlValue and Isdeleted=0"
                param = New SqlParameter("@ControlValue", objRead.ControlValue)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ControlValue.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ControlValueId = GetInteger(sqlRdr("ControlValueId"))
                objRead.ControlValue = sqlRdr("ControlValue").ToString()
                objRead.FormControlId = GetInteger(sqlRdr("FormControlId"))
                objRead.RefControlId = GetInteger(sqlRdr("RefControlId"))
                objRead.RefControlValueId = GetInteger(sqlRdr("RefControlValueId"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
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
    Public Function ReadeZFormControlValue() As System.Collections.Generic.List(Of IeZFormControlValue)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormControlValue)()
        Dim objItem As IeZFormControlValue

        Try
            Dim strQry As String = ""
            strQry = "Select ControlValueId From eZFormControlValue where Isdeleted=0 order by ControlValue"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ControlValue.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormControlValue(GetInteger(sqlRdr("ControlValueId")))
                objItem.ControlValueId = GetInteger(sqlRdr("ControlValueId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function

    Public Function ReadFilteredeZFormControlValue(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFormControlValue)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormControlValue)()
        Dim objItem As IeZFormControlValue
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ControlValueId From eZFormControlValue where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like '%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ControlValue"
            Else
                strQry = "Select ControlValueId From eZFormControlValue where Isdeleted=0 order by ControlValue"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFormControlValue.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormControlValue(GetInteger(sqlRdr("ControlValueId")))
                objItem.ControlValueId = GetInteger(sqlRdr("ControlValueId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZFormControlValue(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFormControlValue)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormControlValue)()
        Dim objItem As IeZFormControlValue
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ControlValueId From eZFormControlValue where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ControlValue"
            Else
                strQry = "Select ControlValueId From eZFormControlValue where Isdeleted=0 order by ControlValue"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFormControlValue.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormControlValue(GetInteger(sqlRdr("ControlValueId")))
                objItem.ControlValueId = GetInteger(sqlRdr("ControlValueId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZFormControlValue)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ControlValueId From eZFormControlValue Where ControlValue = @ControlValue and ControlValueId <> @ControlValueId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ControlValue", objToUpdate.ControlValue)
        objParam(0) = param
        param = New SqlParameter("@ControlValueId", objToUpdate.ControlValueId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ControlValue Code already exist!")
        Else
            strQry = "Update eZFormControlValue Set UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,RefControlValueId=@RefControlValueId,RefControlId,@RefControlId,FormControlId=@FormControlId,ControlValue=@ControlValue where ControlValueId=@ControlValue_ID"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@ControlValue", objToUpdate.ControlValue)
            objParam(0) = param
            param = New SqlParameter("@ControlValue_ID", objToUpdate.ControlValueId)
            objParam(1) = param
            param = New SqlParameter("@FormControlId", objToUpdate.FormControlId)
            objParam(2) = param
            param = New SqlParameter("@RefControlId", objToUpdate.RefControlId)
            objParam(3) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(4) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(5) = param
            param = New SqlParameter("@RefControlValueId", objToUpdate.RefControlValueId)
            objParam(6) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFormControlValue)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ControlValue set Isdeleted=1 where ControlValueId=@ControlValue_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ControlValue_ID", objToDelete.ControlValueId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class