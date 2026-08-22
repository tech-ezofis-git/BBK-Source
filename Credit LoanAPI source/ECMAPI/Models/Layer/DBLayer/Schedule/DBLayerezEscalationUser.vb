Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IezEscalationUser)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1,ezus.firstname as Loginname From ezEscalationUser ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "left join ezecmuserinfo ezus on ez.ecmloginid=ezus.ecmloginid Where ez.EscalationUserId=@EscalationUserId and ez.Isdeleted=0"
            param = New SqlParameter("@EscalationUserId", objRead.EscalationUserId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezEscalationUser")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.EscalationUserId = GetInteger(sqlRdr("EscalationUserId"))
                objRead.EscalationId = GetInteger(sqlRdr("EscalationId"))
                objRead.ECMLoginid = GetInteger(sqlRdr("ECMLoginid"))
                objRead.LoginName = sqlRdr("Loginname").ToString
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
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
    Public Function CreateezEscalationUser(objEmp As ezEscalationUser) As ezEscalationUser
        Dim newObject As ezEscalationUser = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezEscalationUser(EscalationId,ECMLoginid,CreatedBy,CreatedOn) VALUES " +
                "(@EscalationId,@ECMLoginid,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@EscalationId", objEmp.EscalationId)
            objParam(0) = param
            param = New SqlParameter("@ECMLoginid", objEmp.ECMLoginid)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(3) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezEscalationUser(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IezEscalationUser)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezEscalationUser Set EscalationId=@EscalationId,ECMLoginid=@ECMLoginid,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn " +
            "where EscalationUserId=@EscalationUserId"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@EscalationId", objToUpdate.EscalationId)
        objParam(0) = param
        param = New SqlParameter("@ECMLoginid", objToUpdate.ECMLoginid)
        objParam(1) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(2) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(3) = param
        param = New SqlParameter("@EscalationUserId", objToUpdate.EscalationUserId)
        objParam(4) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IezEscalationUser)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezEscalationUser set Isdeleted=1 where EscalationUserId=@EscalationUserId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@EscalationUserId", objToDelete.EscalationUserId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllezEscalationUser() As System.Collections.Generic.List(Of IezEscalationUser)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezEscalationUser)()
        Dim objItem As IezEscalationUser
        Try
            Dim strQry As String = ""
            strQry = "Select EscalationUserId From ezEscalationUser where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezEscalationUser")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezEscalationUser(GetInteger(sqlRdr("EscalationUserId")))
                objItem.EscalationUserId = GetInteger(sqlRdr("EscalationUserId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredezEscalationUser(Criteria As String, Value As String) As List(Of IezEscalationUser)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezEscalationUser)()
        Dim objItem As IezEscalationUser
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select EscalationUserId From ezEscalationUser where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by EscalationUserId"
            Else
                strQry = "Select EscalationUserId From ezEscalationUser where Isdeleted=0 order by EscalationUserId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezEscalationUser")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezEscalationUser(GetInteger(sqlRdr("EscalationUserId")))
                objItem.EscalationUserId = GetInteger(sqlRdr("EscalationUserId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedezEscalationUser(Criteria As String, Value As String) As System.Collections.Generic.List(Of IezEscalationUser)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezEscalationUser)()
        Dim objItem As IezEscalationUser
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select EscalationUserId From ezEscalationUser where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by EscalationUserId"
            Else
                strQry = "Select EscalationUserId From ezEscalationUser where Isdeleted=0 order by EscalationUserId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezEscalationUser")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezEscalationUser(GetInteger(sqlRdr("EscalationUserId")))
                objItem.EscalationUserId = GetInteger(sqlRdr("EscalationUserId"))
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
