Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateFieldAlert(objEmp As eZFieldAlert) As IeZFieldAlert
        Dim newObject As IeZFieldAlert = Nothing
        If objEmp.FieldId = 0 Then
            Return Nothing
        End If
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select FieldAlertId From eZFieldAlert Where ConditionValue=@ConditionValue and ConditionId = @ConditionId And FieldId = @FieldId And FieldAlertDetailId = @FieldAlertDetailId And Isdeleted=0"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@FieldId", objEmp.FieldId)
            objParam(0) = param
            param = New SqlParameter("@ConditionId", objEmp.ConditionId)
            objParam(1) = param
            param = New SqlParameter("@FieldAlertDetailId", objEmp.FieldAlertDetailId)
            objParam(2) = param
            param = New SqlParameter("@ConditionValue", objEmp.ConditionValue)
            objParam(3) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("FieldId Code already exist!")
            End If
            strQry = "INSERT INTO eZFieldAlert(FieldAlertDetailId,FieldId,ConditionId,ConditionValue,TemplateId,CreatedOn,CreatedBy) VALUES(@FieldAlertDetailId,@FieldId,@ConditionId,@ConditionValue,@TemplateId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@FieldId", objEmp.FieldId)
            objParam(0) = param
            param = New SqlParameter("@ConditionId", objEmp.ConditionId)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@ConditionValue", objEmp.ConditionValue)
            objParam(4) = param
            param = New SqlParameter("@FieldAlertDetailId", objEmp.FieldAlertDetailId)
            objParam(5) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(6) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZFieldAlert(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFieldAlert)
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

                strQry = "Select *,dbo.udf_FieldAlertCondition(ConditionId) as Condition,dbo.udf_FieldAlertName(FieldAlertDetailId) as FieldAlertName,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFieldAlert Where FieldAlertId=@FieldAlert_ID and Isdeleted=0"
                param = New SqlParameter("@FieldAlert_ID", objRead.FieldAlertId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_FieldAlertCondition(ConditionId) as Condition,dbo.udf_FieldAlertName(FieldAlertDetailId) as FieldAlertName,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFieldAlert Where FieldId=@FieldId and Isdeleted=0"
                param = New SqlParameter("@FieldId", objRead.FieldId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FieldId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.FieldAlertId = GetInteger(sqlRdr("FieldAlertId"))
                objRead.FieldId = GetInteger(sqlRdr("FieldId"))
                objRead.FieldAlertDetailId = GetInteger(sqlRdr("FieldAlertDetailId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.ConditionValue = sqlRdr("ConditionValue").ToString()
                objRead.Condition = sqlRdr("Condition").ToString()
                objRead.FieldAlertName = sqlRdr("FieldAlertName").ToString()
                objRead.ConditionId = GetInteger(sqlRdr("ConditionId"))
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
    Public Function ReadAllFieldAlert() As System.Collections.Generic.List(Of IeZFieldAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlert)()
        Dim objItem As IeZFieldAlert

        Try
            Dim strQry As String = ""
            strQry = "Select FieldAlertId From eZFieldAlert where Isdeleted=0 order by FieldId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FieldId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlert(GetInteger(sqlRdr("FieldAlertId")))
                objItem.FieldAlertId = GetInteger(sqlRdr("FieldAlertId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedFieldAlert(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFieldAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlert)()
        Dim objItem As IeZFieldAlert
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldAlertId From eZFieldAlert where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FieldId"
            Else
                strQry = "Select FieldAlertId From eZFieldAlert where Isdeleted=0 order by FieldId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlert.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlert(GetSmallInterger(sqlRdr("FieldAlertId")))
                objItem.FieldAlertId = GetSmallInterger(sqlRdr("FieldAlertId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedFieldAlertWithConditionId(Criteria As String, Value As String, ConditionId As String) As System.Collections.Generic.List(Of IeZFieldAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlert)()
        Dim objItem As IeZFieldAlert
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldAlertId From eZFieldAlert where Isdeleted=0 and ConditionId='" + ConditionId + "' and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FieldId"
            Else
                strQry = "Select FieldAlertId From eZFieldAlert where Isdeleted=0 order by FieldId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlert.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlert(GetSmallInterger(sqlRdr("FieldAlertId")))
                objItem.FieldAlertId = GetSmallInterger(sqlRdr("FieldAlertId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZFieldAlert)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select FieldAlertId From eZFieldAlert Where FieldAlertDetailId=@FieldAlertDetailId and ConditionValue=@ConditionValue and ConditionId = @ConditionId And FieldId = @FieldId and FieldAlertId <> @FieldAlertId and Isdeleted=0"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@FieldId", objToUpdate.FieldId)
        objParam(0) = param
        param = New SqlParameter("@FieldAlertId", objToUpdate.FieldAlertId)
        objParam(1) = param
        param = New SqlParameter("@ConditionId", objToUpdate.ConditionId)
        objParam(2) = param
        param = New SqlParameter("@ConditionValue", objToUpdate.ConditionValue)
        objParam(3) = param
        param = New SqlParameter("@FieldAlertDetailId", objToUpdate.FieldAlertDetailId)
        objParam(4) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("FieldId Code already exist!")
        Else
            strQry = "Update eZFieldAlert Set TemplateId=@TemplateId,FieldAlertDetailId=@FieldAlertDetailId,ConditionValue=@ConditionValue,FieldId=@FieldId,ConditionId=@ConditionId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where FieldAlertId=@FieldAlertId"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@FieldId", objToUpdate.FieldId)
            objParam(0) = param
            param = New SqlParameter("@ConditionId", objToUpdate.ConditionId)
            objParam(1) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(2) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(3) = param
            param = New SqlParameter("@FieldAlertId", objToUpdate.FieldAlertId)
            objParam(4) = param
            param = New SqlParameter("@ConditionValue", objToUpdate.ConditionValue)
            objParam(5) = param
            param = New SqlParameter("@FieldAlertDetailId", objToUpdate.FieldAlertDetailId)
            objParam(6) = param
            param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
            objParam(7) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFieldAlert)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFieldAlert set Isdeleted=1 where FieldAlertId=@FieldAlert_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@FieldAlert_ID", objToDelete.FieldAlertId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class