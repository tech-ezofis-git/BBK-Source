Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZMailArchiveValue)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZMailArchiveValue ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.MailArchiveValueId=@MailArchiveValueId and ez.Isdeleted=0"
            param = New SqlParameter("@MailArchiveValueId", objRead.MailArchiveValueId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailArchiveValue")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.MailArchiveValueId = GetInteger(sqlRdr("MailArchiveValueId"))
                objRead.MailArchiveId = GetInteger(sqlRdr("MailArchiveId"))
                objRead.MailArchiveValue = sqlRdr("MailArchiveValue").ToString
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
    Public Function CreateeZMailArchiveValue(objEmp As eZMailArchiveValue) As eZMailArchiveValue
        Dim newObject As eZMailArchiveValue = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZMailArchiveValue(MailArchiveId,MailArchiveValue,CreatedBy,CreatedOn) VALUES " +
                "(@MailArchiveId,@MailArchiveValue,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@MailArchiveId", objEmp.MailArchiveId)
            objParam(0) = param
            param = New SqlParameter("@MailArchiveValue", objEmp.MailArchiveValue)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZMailArchiveValue(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZMailArchiveValue)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailArchiveValue Set MailArchiveId=@MailArchiveId,MailArchiveValue=@MailArchiveValue," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where MailArchiveValueId=@MailArchiveValueId"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@MailArchiveId", objToUpdate.MailArchiveId)
        objParam(0) = param
        param = New SqlParameter("@MailArchiveValue", objToUpdate.MailArchiveValue)
        objParam(1) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(2) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(3) = param
        param = New SqlParameter("@MailArchiveValueId", objToUpdate.MailArchiveValueId)
        objParam(4) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZMailArchiveValue)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailArchiveValue set Isdeleted=1 where MailArchiveValueId=@MailArchiveValueId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@MailArchiveValueId", objToDelete.MailArchiveValueId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZMailArchiveValue() As System.Collections.Generic.List(Of IeZMailArchiveValue)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailArchiveValue)()
        Dim objItem As IeZMailArchiveValue
        Try
            Dim strQry As String = ""
            strQry = "Select MailArchiveValueId From eZMailArchiveValue where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailArchiveValue")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailArchiveValue(GetInteger(sqlRdr("MailArchiveValueId")))
                objItem.MailArchiveValueId = GetInteger(sqlRdr("MailArchiveValueId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZMailArchiveValue(Criteria As String, Value As String) As List(Of IeZMailArchiveValue)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailArchiveValue)()
        Dim objItem As IeZMailArchiveValue
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailArchiveValueId From eZMailArchiveValue where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by MailArchiveValueId"
            Else
                strQry = "Select MailArchiveValueId From eZMailArchiveValue where Isdeleted=0 order by MailArchiveValueId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailArchiveValue")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailArchiveValue(GetInteger(sqlRdr("MailArchiveValueId")))
                objItem.MailArchiveValueId = GetInteger(sqlRdr("MailArchiveValueId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMailArchiveValue(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailArchiveValue)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailArchiveValue)()
        Dim objItem As IeZMailArchiveValue
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailArchiveValueId From eZMailArchiveValue where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by MailArchiveValueId"
            Else
                strQry = "Select MailArchiveValueId From eZMailArchiveValue where Isdeleted=0 order by MailArchiveValueId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailArchiveValue")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailArchiveValue(GetInteger(sqlRdr("MailArchiveValueId")))
                objItem.MailArchiveValueId = GetInteger(sqlRdr("MailArchiveValueId"))
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
