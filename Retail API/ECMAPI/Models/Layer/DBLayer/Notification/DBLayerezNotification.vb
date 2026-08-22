Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IezNotification)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From ezNotification ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.NotificationId=@NotificationId and ez.Isdeleted=0"
            param = New SqlParameter("@NotificationId", objRead.NotificationId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezNotification")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ecmloginid = GetInteger(sqlRdr("ecmloginid"))
                objRead.NotificationId = GetInteger(sqlRdr("NotificationId"))
                objRead.refid = GetInteger(sqlRdr("refid"))
                objRead.notificationfrom = sqlRdr("notificationfrom").ToString
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
    Public Function CreateezNotification(objEmp As ezNotification) As ezNotification
        Dim newObject As ezNotification = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezNotification(ecmloginid,refid,notificationfrom,CreatedBy,CreatedOn) VALUES " +
                "(@ecmloginid,@refid,@notificationfrom,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@ecmloginid", objEmp.ecmloginid)
            objParam(0) = param
            param = New SqlParameter("@refid", objEmp.refid)
            objParam(1) = param
            param = New SqlParameter("@notificationfrom", objEmp.notificationfrom)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(4) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezNotification(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IezNotification)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezNotification Set ecmloginid=@ecmloginid,refid=@refid,notificationfrom=@notificationfrom,UpdatedBy=@UpdatedBy," +
            "UpdatedOn=@UpdatedOn where NotificationId=@NotificationId"
        objParam = New SqlParameter(5) {}
        param = New SqlParameter("@ecmloginid", objToUpdate.ecmloginid)
        objParam(0) = param
        param = New SqlParameter("@refid", objToUpdate.refid)
        objParam(1) = param
        param = New SqlParameter("@notificationfrom", objToUpdate.notificationfrom)
        objParam(2) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@NotificationId", objToUpdate.NotificationId)
        objParam(5) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IezNotification)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezNotification set Isdeleted=1 where NotificationId=@NotificationId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@NotificationId", objToDelete.NotificationId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllezNotification() As System.Collections.Generic.List(Of IezNotification)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezNotification)()
        Dim objItem As IezNotification
        Try
            Dim strQry As String = ""
            strQry = "Select NotificationId From ezNotification where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezNotification")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezNotification(GetInteger(sqlRdr("NotificationId")))
                objItem.NotificationId = GetInteger(sqlRdr("NotificationId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredezNotification(Criteria As String, Value As String) As List(Of IezNotification)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezNotification)()
        Dim objItem As IezNotification
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NotificationId From ezNotification where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by NotificationId"
            Else
                strQry = "Select NotificationId From ezNotification where Isdeleted=0 order by NotificationId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezNotification")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezNotification(GetInteger(sqlRdr("NotificationId")))
                objItem.NotificationId = GetInteger(sqlRdr("NotificationId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedezNotification(Criteria As String, Value As String) As System.Collections.Generic.List(Of IezNotification)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezNotification)()
        Dim objItem As IezNotification
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select NotificationId From ezNotification where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by NotificationId"
            Else
                strQry = "Select NotificationId From ezNotification where Isdeleted=0 order by NotificationId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezNotification")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezNotification(GetInteger(sqlRdr("NotificationId")))
                objItem.NotificationId = GetInteger(sqlRdr("NotificationId"))
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
