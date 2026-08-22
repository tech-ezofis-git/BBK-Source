Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "User TaskUserss"
    Public Function CreateTaskUsers(objEmp As eZTaskUsers) As IeZTaskUsers
        Dim newObject As IeZTaskUsers = Nothing

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select TaskUsersId From eZTaskUsers Where ECMLoginId = @ECMLoginId and OwnerId = @OwnerId And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@OwnerId", objEmp.OwnerId)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ECMLoginId Code already exist!")
            End If
            strQry = "INSERT INTO eZTaskUsers(ECMLoginId,OwnerId) VALUES(@ECMLoginId,@OwnerId);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@OwnerId", objEmp.OwnerId)
            objParam(1) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZTaskUsers(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZTaskUsers)
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
            If objRead.ECMLoginId = 0 Then
                strQry = "Select *,dbo.udf_LoginName(OwnerId) as OwnerName,dbo.udf_LoginName(ECMLoginId) as LoginName From eZTaskUsers Where TaskUsersId=@TaskUsers_ID and Isdeleted=0"
                param = New SqlParameter("@TaskUsers_ID", objRead.TaskUsersId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_LoginName(OwnerId) as OwnerName,dbo.udf_LoginName(ECMLoginId) as LoginName From eZTaskUsers Where ECMLoginId=@ECMLoginId and Isdeleted=0"
                param = New SqlParameter("@ECMLoginId", objRead.ECMLoginId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMLoginId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.TaskUsersId = GetInteger(sqlRdr("TaskUsersId"))
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                objRead.OwnerId = GetInteger(sqlRdr("OwnerId"))
                objRead.OwnerName = sqlRdr("OwnerName").ToString()
                objRead.LoginName = sqlRdr("LoginName").ToString()
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
    Public Function ReadAllTaskUsers() As System.Collections.Generic.List(Of IeZTaskUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTaskUsers)()
        Dim objItem As IeZTaskUsers

        Try
            Dim strQry As String = ""
            strQry = "Select TaskUsersId From eZTaskUsers where Isdeleted=0 order by ECMLoginId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMLoginId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTaskUsers(GetInteger(sqlRdr("TaskUsersId")))
                objItem.TaskUsersId = GetInteger(sqlRdr("TaskUsersId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZTaskUsers)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select TaskUsersId From eZTaskUsers Where ECMLoginId = @ECMLoginId and TaskUsersId <> @TaskUsersId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        objParam(0) = param
        param = New SqlParameter("@TaskUsersId", objToUpdate.TaskUsersId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ECMLoginId Code already exist!")
        Else
            strQry = "Update eZTaskUsers Set ECMLoginId=@ECMLoginId,OwnerId=@OwnerId where TaskUsersId=@TaskUsers_ID"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@TaskUsers_ID", objToUpdate.TaskUsersId)
            objParam(1) = param
            param = New SqlParameter("@OwnerId", objToUpdate.OwnerId)
            objParam(2) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZTaskUsers)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZTaskUsers set Isdeleted=1 where TaskUsersId=@TaskUsers_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@TaskUsers_ID", objToDelete.TaskUsersId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Function ReadFilteredeZTaskUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTaskUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTaskUsers)()
        Dim objItem As IeZTaskUsers

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TaskUsersId From eZTaskUsers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select TaskUsersId From eZTaskUsers where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTaskUsers(GetInteger(sqlRdr("TaskUsersId")))
                objItem.TaskUsersId = GetInteger(sqlRdr("TaskUsersId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTaskUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTaskUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTaskUsers)()
        Dim objItem As IeZTaskUsers

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TaskUsersId From eZTaskUsers where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select TaskUsersId From eZTaskUsers where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTaskUsers(GetInteger(sqlRdr("TaskUsersId")))
                objItem.TaskUsersId = GetInteger(sqlRdr("TaskUsersId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTaskUsersWithOwnerId(Criteria As String, Value As String, OwnerId As String) As System.Collections.Generic.List(Of IeZTaskUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTaskUsers)()
        Dim objItem As IeZTaskUsers

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TaskUsersId From eZTaskUsers where Isdeleted=0 and ECMLoginId=" + OwnerId + " and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select TaskUsersId From eZTaskUsers where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTaskUsers(GetInteger(sqlRdr("TaskUsersId")))
                objItem.TaskUsersId = GetInteger(sqlRdr("TaskUsersId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

#End Region

End Class
