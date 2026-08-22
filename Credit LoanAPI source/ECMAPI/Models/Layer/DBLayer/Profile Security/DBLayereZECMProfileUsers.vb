Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZECMProfileUsers)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZECMProfileUsers ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.ECMProfileUsersId=@ECMProfileUsersId and ez.Isdeleted=0"
            param = New SqlParameter("@ECMProfileUsersId", objRead.ECMProfileUsersId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ECMProfileUsersId = GetInteger(sqlRdr("ECMProfileUsersId"))
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                objRead.ECMProfileId = GetInteger(sqlRdr("EcmProfileId"))
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
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
    Public Function CreateeZECMProfileUsers(objEmp As eZECMProfileUsers) As eZECMProfileUsers
        Dim newObject As eZECMProfileUsers = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZECMProfileUsers(ECMLoginId,EcmProfileId,CreatedBy,CreatedOn) VALUES" +
                "(@ECMLoginId,@EcmProfileId,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@EcmProfileId", objEmp.EcmProfileId)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZECMProfileUsers(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZECMProfileUsers)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMProfileUsers Set ECMLoginId=@ECMLoginId,EcmProfileId=@EcmProfileId,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn" +
            " where ECMProfileUsersId=@ECMProfileUsersId"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        objParam(0) = param
        param = New SqlParameter("@EcmProfileId", objToUpdate.ECMProfileId)
        objParam(1) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(2) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(3) = param
        param = New SqlParameter("@ECMProfileUsersId", objToUpdate.ECMProfileUsersId)
        objParam(4) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMProfileUsers)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMProfileUsers set Isdeleted=1 where ECMProfileUsersId=@ECMProfileUsersId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ECMProfileUsersId", objToDelete.ECMProfileUsersId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZECMProfileUsers() As System.Collections.Generic.List(Of IeZECMProfileUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMProfileUsers)()
        Dim objItem As IeZECMProfileUsers
        Try
            Dim strQry As String = ""
            strQry = "Select ECMProfileUsersId From eZECMProfileUsers where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Monitor Files")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMProfileUsers(GetInteger(sqlRdr("ECMProfileUsersId")))
                objItem.ECMProfileUsersId = GetInteger(sqlRdr("ECMProfileUsersId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZECMProfileUsers(Criteria As String, Value As String) As List(Of IeZECMProfileUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMProfileUsers)()
        Dim objItem As IeZECMProfileUsers
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMProfileUsersId From eZECMProfileUsers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMProfileUsersId"
            Else
                strQry = "Select ECMProfileUsersId From eZECMProfileUsers where Isdeleted=0 order by ECMProfileUsersId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Monitor File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMProfileUsers(GetInteger(sqlRdr("ECMProfileUsersId")))
                objItem.ECMProfileUsersId = GetInteger(sqlRdr("ECMProfileUsersId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMProfileUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMProfileUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMProfileUsers)()
        Dim objItem As IeZECMProfileUsers
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMProfileUsersId From eZECMProfileUsers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMProfileUsersId"
            Else
                strQry = "Select ECMProfileUsersId From eZECMProfileUsers where Isdeleted=0 order by ECMProfileUsersId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Monitor File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMProfileUsers(GetInteger(sqlRdr("ECMProfileUsersId")))
                objItem.ECMProfileUsersId = GetInteger(sqlRdr("ECMProfileUsersId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
End Class
