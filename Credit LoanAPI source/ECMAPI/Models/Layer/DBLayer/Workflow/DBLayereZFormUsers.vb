Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZFormUsers)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From ezformusers " +
                "Where FormUsersId=@FormUsersId and Isdeleted=0"
            param = New SqlParameter("@FormUsersId", objRead.FormUsersId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Form Users")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.FormUsersId = GetInteger(sqlRdr("FormUsersId"))
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                objRead.FormId = GetInteger(sqlRdr("FormId"))
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
                objRead.Createdby1 = sqlRdr("CreatedBy1").ToString()
                objRead.Updatedby1 = sqlRdr("UpdatedBy1").ToString()
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

    Public Function CreateeZFormUsers(objEmp As eZFormUsers) As eZFormUsers
        Dim newObject As eZFormUsers = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezformusers(ecmloginid,formid,CreatedBy,CreatedOn) VALUES" +
                "(@ecmloginid,@formid,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(1) = param
            param = New SqlParameter("@ecmloginid", objEmp.ECMLoginId)
            objParam(2) = param
            param = New SqlParameter("@formid", objEmp.FormId)
            objParam(3) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZFormUsers(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZFormUsers)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezformusers Set ecmloginid=@ecmloginid,formid=@formid" +
            ",UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where formusersid=@formusersid"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@Updatedby", objToUpdate.Updatedby)
        objParam(0) = param
        param = New SqlParameter("@Updatedon", objToUpdate.Updatedon)
        objParam(1) = param
        param = New SqlParameter("@formid", objToUpdate.FormId)
        objParam(2) = param
        param = New SqlParameter("@ecmloginid", objToUpdate.ECMLoginId)
        objParam(3) = param
        param = New SqlParameter("@formusersid", objToUpdate.FormUsersId)
        objParam(4) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZFormUsers)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezformusers set Isdeleted=1 where formusersid=@formusersid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@formusersid", objToDelete.FormUsersId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region

    Public Function ReadAlleZFormUsers() As System.Collections.Generic.List(Of IeZFormUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormUsers)()
        Dim objItem As IeZFormUsers
        Try
            Dim strQry As String = ""
            strQry = "Select formusersid From eZformUsers where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Form Users")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormUsers(GetInteger(sqlRdr("formusersid")))
                objItem.FormUsersId = GetInteger(sqlRdr("formusersid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZFormUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFormUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormUsers)()
        Dim objItem As IeZFormUsers
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select formusersid From eZformUsers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by formusersid"
            Else
                strQry = "Select formusersid From eZformUsers where Isdeleted=0 order by formusersid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Form Users")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormUsers(GetInteger(sqlRdr("formusersid")))
                objItem.FormUsersId = GetInteger(sqlRdr("formusersid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZFormUsers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFormUsers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormUsers)()
        Dim objItem As IeZFormUsers
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select formusersid From eZformUsers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by formusersid"
            Else
                strQry = "Select formusersid From eZformUsers where Isdeleted=0 order by formusersid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Form Users")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormUsers(GetInteger(sqlRdr("formusersid")))
                objItem.FormUsersId = GetInteger(sqlRdr("formusersid"))
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
