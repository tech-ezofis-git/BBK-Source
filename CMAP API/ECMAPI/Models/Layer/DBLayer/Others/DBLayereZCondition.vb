Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateCondition(objEmp As eZCondition) As IeZCondition
        Dim newObject As IeZCondition = Nothing
        If String.IsNullOrEmpty(objEmp.Condition) Then
            Return Nothing
        End If
        objEmp.Condition = objEmp.Condition.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ConditionId From eZCondition Where Condition = @Condition And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@Condition", objEmp.Condition)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("Condition Code already exist!")
            End If
            strQry = "INSERT INTO eZCondition(Condition) VALUES(@Condition);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@Condition", objEmp.Condition)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZCondition(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZCondition)
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
            If objRead.Condition Is Nothing Then

                strQry = "Select * From eZCondition Where ConditionId=@Condition_ID and Isdeleted=0"
                param = New SqlParameter("@Condition_ID", objRead.ConditionId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(0) {}
                strQry = "Select * From eZCondition Where Condition=@Condition and Isdeleted=0"
                param = New SqlParameter("@Condition", objRead.Condition)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Condition.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ConditionId = GetInteger(sqlRdr("ConditionId"))
                objRead.Condition = sqlRdr("Condition").ToString()
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
    Public Function ReadAllCondition() As System.Collections.Generic.List(Of IeZCondition)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCondition)()
        Dim objItem As IeZCondition

        Try
            Dim strQry As String = ""
            strQry = "Select ConditionId From eZCondition where Isdeleted=0 order by Condition"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Condition.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCondition(GetInteger(sqlRdr("ConditionId")))
                objItem.ConditionId = GetInteger(sqlRdr("ConditionId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZCondition)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ConditionId From eZCondition Where Condition = @Condition and ConditionId <> @ConditionId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@Condition", objToUpdate.Condition)
        objParam(0) = param
        param = New SqlParameter("@ConditionId", objToUpdate.ConditionId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("Condition Code already exist!")
        Else
            strQry = "Update eZCondition Set Condition=@Condition where ConditionId=@Condition_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@Condition", objToUpdate.Condition)
            objParam(0) = param
            param = New SqlParameter("@Condition_ID", objToUpdate.ConditionId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZCondition)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update Condition set Isdeleted=1 where ConditionId=@Condition_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Condition_ID", objToDelete.ConditionId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class