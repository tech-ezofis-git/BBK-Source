Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZHideFileUsers)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 " +
                "From ezhidefileusers Where hidefileusersid=@hidefileusersid and Isdeleted=0"
            param = New SqlParameter("@hidefileusersid", objRead.HideFileUsersId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File Users")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.HideFileId = GetInteger(sqlRdr("HideFileId"))
                objRead.HideFileUsersId = GetInteger(sqlRdr("HideFileUsersId"))
                objRead.Show = GetInteger(sqlRdr("Show"))
                objRead.Sno = GetInteger(sqlRdr("Sno"))
                objRead.UserId = GetInteger(sqlRdr("UserId"))
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

    Public Function CreateeZHideFileUsers(objEmp As eZHideFileUsers) As eZHideFileUsers
        Dim newObject As eZHideFileUsers = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezhidefileusers(hidefileid,show,sno,userid,CreatedBy,CreatedOn) VALUES" +
                "(@hidefileid,@show,@sno,@userid,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@hidefileid", objEmp.HideFileId)
            objParam(2) = param
            param = New SqlParameter("@show", objEmp.Show)
            objParam(3) = param
            param = New SqlParameter("@sno", objEmp.SNo)
            objParam(4) = param
            param = New SqlParameter("@userid", objEmp.UserId)
            objParam(5) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            'obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZHideFileUsers(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZHideFileUsers)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezhidefileusers Set hidefileid=@hidefileid,show=@show,sno=@sno,userid=@userid,UpdatedBy=@UpdatedBy," +
            "UpdatedOn=@UpdatedOn where hidefileusersid=@hidefileusersid"
        objParam = New SqlParameter(6) {}
        param = New SqlParameter("@hidefileid", objToUpdate.HideFileId)
        objParam(0) = param
        param = New SqlParameter("@show", objToUpdate.Show)
        objParam(1) = param
        param = New SqlParameter("@sno", objToUpdate.Sno)
        objParam(2) = param
        param = New SqlParameter("@userid", objToUpdate.UserId)
        objParam(3) = param
        param = New SqlParameter("@hidefileusersid", objToUpdate.HideFileUsersId)
        objParam(4) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(5) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(6) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZHideFileUsers)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezhidefileusers set Isdeleted=1 where hidefileusersid=@hidefileusersid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@hidefileusersid", objToDelete.HideFileUsersId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZHideFileUsers() As System.Collections.Generic.List(Of IeZHideFileUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZHideFileUsers)()
        Dim objItem As IeZHideFileUsers
        Try
            Dim strQry As String = ""
            strQry = "Select hidefileusersid From ezhidefileusers where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide Files")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZHideFileUsers(GetInteger(sqlRdr("hidefileusersid")))
                objItem.HideFileUsersId = GetInteger(sqlRdr("hidefileusersid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZHideFileUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZHideFileUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZHideFileUsers)()
        Dim objItem As IeZHideFileUsers

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select hidefileusersid From ezhidefileusers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by hidefileid, hidefileusersid"
            Else
                strQry = "Select hidefileusersid From ezhidefileusers where Isdeleted=0 order by hidefileid, hidefileusersid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File Users.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZHideFileUsers(GetInteger(sqlRdr("hidefileusersid")))
                objItem.HideFileUsersId = GetInteger(sqlRdr("hidefileusersid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZHideFileUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZHideFileUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZHideFileUsers)()
        Dim objItem As IeZHideFileUsers

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select hidefileusersid From ezhidefileusers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by hidefileid, hidefileusersid"
            Else
                strQry = "Select hidefileusersid From ezhidefileusers where Isdeleted=0 order by hidefileid, hidefileusersid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File Users.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZHideFileUsers(GetInteger(sqlRdr("hidefileusersid")))
                objItem.HideFileUsersId = GetInteger(sqlRdr("hidefileusersid"))
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
