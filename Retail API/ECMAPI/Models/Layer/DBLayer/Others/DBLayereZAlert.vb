Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateAlert(objEmp As eZAlert) As IeZAlert
        Dim newObject As IeZAlert = Nothing
        If objEmp.AlertConditionId = 0 Then
            Return Nothing
        End If
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select AlertId From eZAlert Where DocumentAlertId = @DocumentAlertId And AlertConditionId = @AlertConditionId  And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@AlertConditionId", objEmp.AlertConditionId)
            objParam(0) = param
            param = New SqlParameter("@DocumentAlertId", objEmp.DocumentAlertId)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("AlertConditionId Code already exist!")
            End If
            strQry = "INSERT INTO eZAlert(AlertConditionId,DocumentAlertId,CreatedOn,CreatedBy) VALUES(@AlertConditionId,@DocumentAlertId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@AlertConditionId", objEmp.AlertConditionId)
            objParam(0) = param
            param = New SqlParameter("@DocumentAlertId", objEmp.DocumentAlertId)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZAlert(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZAlert)
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
            If objRead.AlertConditionId = 0 Then

                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZAlert Where AlertId=@Alert_ID and Isdeleted=0"
                param = New SqlParameter("@Alert_ID", objRead.AlertId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZAlert Where AlertConditionId=@AlertConditionId and Isdeleted=0"
                param = New SqlParameter("@AlertConditionId", objRead.AlertConditionId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid AlertConditionId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.AlertId = GetInteger(sqlRdr("AlertId"))
                objRead.AlertConditionId = GetInteger(sqlRdr("AlertConditionId"))
                objRead.DocumentAlertId = GetInteger(sqlRdr("DocumentAlertId"))
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
    Public Function ReadAllAlert() As System.Collections.Generic.List(Of IeZAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAlert)()
        Dim objItem As IeZAlert

        Try
            Dim strQry As String = ""
            strQry = "Select AlertId From eZAlert where Isdeleted=0 order by AlertConditionId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid AlertConditionId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAlert(GetInteger(sqlRdr("AlertId")))
                objItem.AlertId = GetInteger(sqlRdr("AlertId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedAlert(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAlert)()
        Dim objItem As IeZAlert
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select AlertId From eZAlert where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by AlertConditionId"
            Else
                strQry = "Select AlertId From eZAlert where Isdeleted=0 order by AlertConditionId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZAlert.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAlert(GetSmallInterger(sqlRdr("AlertId")))
                objItem.AlertId = GetSmallInterger(sqlRdr("AlertId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedAlertWithDocumentAlertId(Criteria As String, Value As String, DocumentAlertId As String) As System.Collections.Generic.List(Of IeZAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAlert)()
        Dim objItem As IeZAlert
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select AlertId From eZAlert where Isdeleted=0 and DocumentAlertId='" + DocumentAlertId + "' and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by AlertConditionId"
            Else
                strQry = "Select AlertId From eZAlert where Isdeleted=0 order by AlertConditionId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZAlert.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAlert(GetSmallInterger(sqlRdr("AlertId")))
                objItem.AlertId = GetSmallInterger(sqlRdr("AlertId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZAlert)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select AlertId From eZAlert Where DocumentAlertId = @DocumentAlertId And AlertConditionId = @AlertConditionId and AlertId <> @AlertId and Isdeleted=0"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@AlertConditionId", objToUpdate.AlertConditionId)
        objParam(0) = param
        param = New SqlParameter("@AlertId", objToUpdate.AlertId)
        objParam(1) = param
        param = New SqlParameter("@DocumentAlertId", objToUpdate.DocumentAlertId)
        objParam(2) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("AlertConditionId Code already exist!")
        Else
            strQry = "Update eZAlert Set AlertConditionId=@AlertConditionId,DocumentAlertId=@DocumentAlertId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where AlertId=@AlertId"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(2) = param
            param = New SqlParameter("@AlertConditionId", objToUpdate.AlertConditionId)
            objParam(0) = param
            param = New SqlParameter("@DocumentAlertId", objToUpdate.DocumentAlertId)
            objParam(1) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(3) = param
            param = New SqlParameter("@AlertId", objToUpdate.AlertId)
            objParam(4) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZAlert)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZAlert set Isdeleted=1 where AlertId=@Alert_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Alert_ID", objToDelete.AlertId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class