Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZVault)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From ezvault Where ezvaultid=@ezvaultid and Isdeleted=0"
            param = New SqlParameter("@ezvaultid", objRead.eZVaultId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.eZVaultId = GetInteger(sqlRdr("eZVaultId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.Condition = sqlRdr("Condition").ToString
                objRead.Status = GetInteger(sqlRdr("Status"))
                objRead.NodeId = GetInteger(sqlRdr("NodeId"))
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

    Public Function CreateeZVault(objEmp As eZVault) As eZVault
        Dim newObject As eZVault = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezvault(templateid,condition,Status,CreatedBy,CreatedOn,nodeid) VALUES" +
                "(@templateid,@condition,@Status,@CreatedBy,@CreatedOn,@nodeid);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@templateid", objEmp.TemplateId)
            objParam(2) = param
            param = New SqlParameter("@condition", objEmp.Condition)
            objParam(3) = param
            param = New SqlParameter("@Status", objEmp.Status)
            objParam(4) = param
            param = New SqlParameter("@nodeid", objEmp.NodeId)
            objParam(5) = param
            Dim obj As Object = Nothing
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZVault(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.ToString)
            Return Nothing
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZVault)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezvault Set templateid=@templateid,condition=@condition,Status=@Status," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,nodeid=@nodeid where ezvaultid=@ezvaultid"
        objParam = New SqlParameter(6) {}
        param = New SqlParameter("@templateid", objToUpdate.TemplateId)
        objParam(0) = param
        param = New SqlParameter("@condition", objToUpdate.Condition)
        objParam(1) = param
        param = New SqlParameter("@Status", objToUpdate.Status)
        objParam(2) = param
        param = New SqlParameter("@ezvaultid", objToUpdate.eZVaultId)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@nodeid", objToUpdate.NodeId)
        objParam(6) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZVault)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezvault set Isdeleted=1 where ezvaultid=@ezvaultid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ezvaultid", objToDelete.eZVaultId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZVault() As System.Collections.Generic.List(Of IeZVault)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZVault)()
        Dim objItem As IeZVault
        Try
            Dim strQry As String = ""
            strQry = "Select ezvaultid From ezvault where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Vault.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZVault(GetInteger(sqlRdr("ezvaultid")))
                objItem.eZVaultId = GetInteger(sqlRdr("ezvaultid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZVault(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZVault)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZVault)()
        Dim objItem As IeZVault
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ezvaultid From ezvault where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by templateid, ezvaultid"
            Else
                strQry = "Select ezvaultid From ezvault where Isdeleted=0 order by templateid, ezvaultid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Vault.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZVault(GetInteger(sqlRdr("ezvaultid")))
                objItem.eZVaultId = GetInteger(sqlRdr("ezvaultid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZVault(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZVault)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZVault)()
        Dim objItem As IeZVault

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ezvaultid From ezvault where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by templateid, ezvaultid"
            Else
                strQry = "Select ezvaultid From ezvault where Isdeleted=0 order by templateid, ezvaultid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZVault(GetInteger(sqlRdr("ezvaultid")))
                objItem.eZVaultId = GetInteger(sqlRdr("ezvaultid"))
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
