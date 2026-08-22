Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateAlertCondition(objEmp As eZAlertCondition) As IeZAlertCondition
        Dim newObject As IeZAlertCondition = Nothing
        If String.IsNullOrEmpty(objEmp.AlertCondition) Then
            Return Nothing
        End If
        objEmp.AlertCondition = objEmp.AlertCondition.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select AlertConditionId From eZAlertCondition Where AlertCondition = @AlertCondition And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@AlertCondition", objEmp.AlertCondition)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("AlertCondition Code already exist!")
            End If
            strQry = "INSERT INTO eZAlertCondition(AlertCondition) VALUES(@AlertCondition);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@AlertCondition", objEmp.AlertCondition)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZAlertCondition(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZAlertCondition)
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
            If objRead.AlertCondition Is Nothing Then

                strQry = "Select * From eZAlertCondition Where AlertConditionId=@AlertCondition_ID and Isdeleted=0"
                param = New SqlParameter("@AlertCondition_ID", objRead.AlertConditionId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZAlertCondition Where AlertCondition=@AlertCondition and Isdeleted=0"
                param = New SqlParameter("@AlertCondition", objRead.AlertCondition)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid AlertCondition.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.AlertConditionId = GetInteger(sqlRdr("AlertConditionId"))
                objRead.AlertCondition = sqlRdr("AlertCondition").ToString()
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
    Public Function ReadAllAlertCondition() As System.Collections.Generic.List(Of IeZAlertCondition)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZAlertCondition)()
        Dim objItem As IeZAlertCondition

        Try
            Dim strQry As String = ""
            strQry = "Select AlertConditionId From eZAlertCondition where Isdeleted=0 order by AlertCondition"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid AlertCondition.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZAlertCondition(GetInteger(sqlRdr("AlertConditionId")))
                objItem.AlertConditionId = GetInteger(sqlRdr("AlertConditionId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZAlertCondition)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select AlertConditionId From eZAlertCondition Where AlertCondition = @AlertCondition and AlertConditionId <> @AlertConditionId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@AlertCondition", objToUpdate.AlertCondition)
        objParam(0) = param
        param = New SqlParameter("@AlertConditionId", objToUpdate.AlertConditionId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("AlertCondition Code already exist!")
        Else
            strQry = "Update eZAlertCondition Set AlertCondition=@AlertCondition where AlertConditionId=@AlertCondition_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@AlertCondition", objToUpdate.AlertCondition)
            objParam(0) = param
            param = New SqlParameter("@AlertCondition_ID", objToUpdate.AlertConditionId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZAlertCondition)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update AlertCondition set Isdeleted=1 where AlertConditionId=@AlertCondition_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@AlertCondition_ID", objToDelete.AlertConditionId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class