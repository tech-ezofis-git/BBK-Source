Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZMailWatchingStatus)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZMailWatchingStatus ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Mailwatchingid=@Mailwatchingid and ez.Isdeleted=0"
            param = New SqlParameter("@Mailwatchingid", objRead.Mailwatchingid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingStatus")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Mailwatchingid = GetInteger(sqlRdr("Mailwatchingid"))
                objRead.sendid = GetInteger(sqlRdr("sendid"))
                objRead.Keyword = sqlRdr("keyword").ToString
                objRead.receivedtime = sqlRdr("receivedtime").ToString
                objRead.ReceivedFrom = sqlRdr("ReceivedFrom").ToString
                objRead.MailsendStatus = sqlRdr("MailsendStatus").ToString
                objRead.MailsendTime = sqlRdr("MailsendTime").ToString
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
    Public Function CreateeZMailWatchingStatus(objEmp As eZMailWatchingStatus) As eZMailWatchingStatus
        Dim newObject As eZMailWatchingStatus = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZMailWatchingStatus(receivedtime,ReceivedFrom,keyword,sendid,MailsendTime,MailsendStatus,CreatedBy,CreatedOn) VALUES " +
                "(@receivedtime,@ReceivedFrom,@keyword,@sendid,@MailsendTime,@MailsendStatus,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@receivedtime", objEmp.receivedtime)
            objParam(0) = param
            param = New SqlParameter("@ReceivedFrom", objEmp.ReceivedFrom)
            objParam(1) = param
            param = New SqlParameter("@keyword", objEmp.Keyword)
            objParam(2) = param
            param = New SqlParameter("@sendid", objEmp.sendid)
            objParam(3) = param
            param = New SqlParameter("@MailsendTime", objEmp.MailsendTime)
            objParam(4) = param
            param = New SqlParameter("@MailsendStatus", objEmp.MailsendStatus)
            objParam(5) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(6) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZMailWatchingStatus(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZMailWatchingStatus)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailWatchingStatus Set receivedtime=@receivedtime,ReceivedFrom=@ReceivedFrom,keyword=@keyword,sendid=@sendid" +
            "MailsendTime=@MailsendTime,MailsendStatus=@MailsendStatus,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where Mailwatchingid=@Mailwatchingid"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@receivedtime", objToUpdate.receivedtime)
        objParam(0) = param
        param = New SqlParameter("@ReceivedFrom", objToUpdate.ReceivedFrom)
        objParam(1) = param
        param = New SqlParameter("@keyword", objToUpdate.Keyword)
        objParam(2) = param
        param = New SqlParameter("@sendid", objToUpdate.sendid)
        objParam(3) = param
        param = New SqlParameter("@MailsendTime", objToUpdate.MailsendTime)
        objParam(4) = param
        param = New SqlParameter("@MailsendStatus", objToUpdate.MailsendStatus)
        objParam(5) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(6) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(7) = param
        param = New SqlParameter("@Mailwatchingid", objToUpdate.Mailwatchingid)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZMailWatchingStatus)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailWatchingStatus set Isdeleted=1 where Mailwatchingid=@Mailwatchingid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Mailwatchingid", objToDelete.Mailwatchingid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZMailWatchingStatus() As System.Collections.Generic.List(Of IeZMailWatchingStatus)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatchingStatus)()
        Dim objItem As IeZMailWatchingStatus
        Try
            Dim strQry As String = ""
            strQry = "Select Mailwatchingid From eZMailWatchingStatus where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingStatus")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatchingStatus(GetInteger(sqlRdr("Mailwatchingid")))
                objItem.Mailwatchingid = GetInteger(sqlRdr("Mailwatchingid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZMailWatchingStatus(Criteria As String, Value As String) As List(Of IeZMailWatchingStatus)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatchingStatus)()
        Dim objItem As IeZMailWatchingStatus
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Mailwatchingid From eZMailWatchingStatus where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Mailwatchingid"
            Else
                strQry = "Select Mailwatchingid From eZMailWatchingStatus where Isdeleted=0 order by Mailwatchingid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingStatus")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatchingStatus(GetInteger(sqlRdr("Mailwatchingid")))
                objItem.Mailwatchingid = GetInteger(sqlRdr("Mailwatchingid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMailWatchingStatus(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailWatchingStatus)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailWatchingStatus)()
        Dim objItem As IeZMailWatchingStatus
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Mailwatchingid From eZMailWatchingStatus where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Mailwatchingid"
            Else
                strQry = "Select Mailwatchingid From eZMailWatchingStatus where Isdeleted=0 order by Mailwatchingid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailWatchingStatus")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailWatchingStatus(GetInteger(sqlRdr("Mailwatchingid")))
                objItem.Mailwatchingid = GetInteger(sqlRdr("Mailwatchingid"))
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
