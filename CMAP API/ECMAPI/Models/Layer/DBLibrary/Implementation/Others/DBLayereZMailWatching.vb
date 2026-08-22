Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZMailWatching)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZMailWatching ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.mailwatchingid=@mailwatchingid and ez.Isdeleted=0"
            param = New SqlParameter("@mailwatchingid", objRead.mailwatchingid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatching")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.mailwatchingid = GetInteger(sqlRdr("mailwatchingid"))
                objRead.Watchingmail = sqlRdr("Watchingmail").ToString()
                objRead.WatchingMailPWD = sqlRdr("WatchingMailPWD").ToString
                objRead.Conditionid = GetInteger("Conditionid").ToString()
                objRead.WatchingTime = sqlRdr("WatchingTime").ToString()
                objRead.WatchingStatus = sqlRdr("WatchingStatus ").ToString()
                objRead.port = sqlRdr("port").ToString()
                objRead.SMTP = sqlRdr("SMTP").ToString()
                objRead.createdon = sqlRdr("createdon").ToString()
                objRead.updatedon = sqlRdr("updatedon").ToString()
                objRead.createdby = GetInteger(sqlRdr("createdby"))
                objRead.updatedby = GetInteger(sqlRdr("updatedby"))
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
    Public Function CreateeZMailWatching(objEmp As eZMailWatching) As eZMailWatching
        Dim newObject As eZMailWatching = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZMailWatching(Watchingmail,WatchingMailPWD,Conditionid,WatchingTime,WatchingStatus,port,SMTP,createdby,createdon) VALUES " +
                "(@Watchingmail,@WatchingMailPWD,@Conditionid,@WatchingTime,@WatchingStatus,@port,@SMTP,@createdby,@createdon);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@Watchingmail", objEmp.Watchingmail)
            objParam(0) = param
            param = New SqlParameter("@WatchingMailPWD", objEmp.WatchingMailPWD)
            objParam(1) = param
            param = New SqlParameter("@Conditionid", objEmp.Conditionid)
            objParam(2) = param
            param = New SqlParameter("@WatchingTime", objEmp.WatchingTime)
            objParam(3) = param
            param = New SqlParameter("@WatchingStatus", objEmp.WatchingStatus)
            objParam(4) = param
            param = New SqlParameter("@port", objEmp.port)
            objParam(5) = param
            param = New SqlParameter("@SMTP", objEmp.SMTP)
            objParam(6) = param
            param = New SqlParameter("@createdby", objEmp.createdby)
            objParam(7) = param
            param = New SqlParameter("@createdon", objEmp.createdon)
            objParam(8) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZMailWatching(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZMailWatching)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailWatching Set Watchingmail=@Watchingmail,WatchingMailPWD=@WatchingMailPWD,Conditionid=@Conditionid,WatchingTime=@WatchingTime," +
            "WatchingStatus=@WatchingStatus,port=@port,SMTP=@SMTP,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where mailwatchingid=@mailwatchingid"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@Watchingmail", objToUpdate.Watchingmail)
        objParam(0) = param
        param = New SqlParameter("@WatchingMailPWD", objToUpdate.WatchingMailPWD)
        objParam(1) = param
        param = New SqlParameter("@Conditionid", objToUpdate.Conditionid)
        objParam(2) = param
        param = New SqlParameter("@WatchingTime", objToUpdate.WatchingTime)
        objParam(3) = param
        param = New SqlParameter("@WatchingStatus", objToUpdate.WatchingStatus)
        objParam(4) = param
        param = New SqlParameter("@port", objToUpdate.port)
        objParam(5) = param
        param = New SqlParameter("@SMTP", objToUpdate.SMTP)
        objParam(6) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.updatedby)
        objParam(7) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.updatedon)
        objParam(8) = param
        param = New SqlParameter("@mailwatchingid", objToUpdate.mailwatchingid)
        objParam(9) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZMailWatching)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailWatching set Isdeleted=1 where mailwatchingid=@mailwatchingid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@mailwatchingid", objToDelete.mailwatchingid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZMailWatching() As System.Collections.Generic.List(Of IeZMailWatching)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatching)()
        Dim objItem As IeZMailWatching
        Try
            Dim strQry As String = ""
            strQry = "Select mailwatchingid From eZMailWatching where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatching")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatching(GetInteger(sqlRdr("mailwatchingid")))
                objItem.mailwatchingid = GetInteger(sqlRdr("mailwatchingid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZMailWatching(Criteria As String, Value As String) As List(Of IeZMailWatching)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatching)()
        Dim objItem As IeZMailWatching
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select mailwatchingid From eZMailWatching where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by mailwatchingid"
            Else
                strQry = "Select mailwatchingid From eZMailWatching where Isdeleted=0 order by mailwatchingid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatching")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatching(GetInteger(sqlRdr("mailwatchingid")))
                objItem.mailwatchingid = GetInteger(sqlRdr("mailwatchingid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMailWatching(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailWatching)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatching)()
        Dim objItem As IeZMailWatching
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select mailwatchingid From eZMailWatching where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by mailwatchingid"
            Else
                strQry = "Select mailwatchingid From eZMailWatching where Isdeleted=0 order by mailwatchingid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatching")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatching(GetInteger(sqlRdr("mailwatchingid")))
                objItem.mailwatchingid = GetInteger(sqlRdr("mailwatchingid"))
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

