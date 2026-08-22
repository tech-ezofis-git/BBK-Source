Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateFormValidation(objEmp As eZFormValidation) As IeZFormValidation
        Dim newObject As IeZFormValidation = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZFormValidation(ValidationName,OnEvent,FunctionName,CreatedOn,CreatedBy) VALUES(@ValidationName,@OnEvent,@FunctionName,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@ValidationName", objEmp.ValidationName)
            objParam(0) = param
            param = New SqlParameter("@OnEvent", objEmp.OnEvent)
            objParam(1) = param
            param = New SqlParameter("@FunctionName", objEmp.FunctionName)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZFormValidation(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFormValidation)
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

            objParam = New SqlParameter(0) {}
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFormValidation Where ValidationId=@ValidationId and Isdeleted=0"
            param = New SqlParameter("@ValidationId", objRead.ValidationId)
            objParam(0) = param


            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Validation.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ValidationId = GetInteger(sqlRdr("ValidationId"))
                objRead.ValidationName = sqlRdr("ValidationName").ToString()
                objRead.OnEvent = sqlRdr("OnEvent").ToString()
                objRead.FunctionName = sqlRdr("FunctionName").ToString()
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
    Public Function ReadAllFormValidation() As System.Collections.Generic.List(Of IeZFormValidation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormValidation)()
        Dim objItem As IeZFormValidation

        Try
            Dim strQry As String = ""
            strQry = "Select ValidationId From eZFormValidation where Isdeleted=0 order by ValidationId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Validation.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormValidation(GetInteger(sqlRdr("ValidationId")))
                objItem.ValidationId = GetInteger(sqlRdr("ValidationId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedeZFormValidation(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFormValidation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormValidation)()
        Dim objItem As IeZFormValidation
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ValidationId From eZFormValidation where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ValidationId"
            Else
                strQry = "Select ValidationId From eZFormValidation where Isdeleted=0 order by ValidationId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FormValidation.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormValidation(GetSmallInterger(sqlRdr("ValidationId")))
                objItem.ValidationId = GetSmallInterger(sqlRdr("ValidationId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZFormValidation)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZformValidation Set ValidationName=@ValidationName,FunctionName=@FunctionName,OnEvent=@OnEventUpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ValidationId=@ValidationID"
        objParam = New SqlParameter(5) {}
        param = New SqlParameter("@ValidationId", objToUpdate.ValidationId)
        objParam(0) = param
        param = New SqlParameter("@ValidationName", objToUpdate.ValidationName)
        objParam(1) = param
        param = New SqlParameter("@FunctionName", objToUpdate.FunctionName)
        objParam(2) = param
        param = New SqlParameter("@OnEvent", objToUpdate.OnEvent)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZformValidation)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFormValidation set Isdeleted=1 where ValidationId=@ValidationId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ValidationId", objToDelete.ValidationId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class