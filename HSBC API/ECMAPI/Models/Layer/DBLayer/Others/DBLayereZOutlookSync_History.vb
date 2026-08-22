Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
    Public Function CreateeZoutlooksyncHistory(objtemp As eZOutlookSync_History) As IeZOutlookSync_History
        Dim newObject As IeZOutlookSync_History = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            Dim obj As Object
            strQry = "INSERT INTO ezoutlooksync_history(outlooksyncid,CreatedOn,CreatedBy,SyncStatus,syncdate) VALUES(@OutlookSyncId,@CreatedOn,@CreatedBy,@SyncStatus,@SyncDate);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@OutlookSyncId", objtemp.OutlookSyncId)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objtemp.Createdon)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objtemp.Createdby)
            objParam(2) = param
            param = New SqlParameter("@SyncStatus", objtemp.SyncStatus)
            objParam(3) = param
            param = New SqlParameter("@SyncDate", objtemp.SyncDate)
            objParam(4) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZOutlooksync_histroy(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub read(ByVal objread As IeZOutlookSync_History)
        If objread.IsReadFromDB Then
            Return
        End If
        If objread.IsModified Then
            Throw New InvalidOperationException
        End If
        Dim sqlRdr As SqlDataReader = Nothing
        objread.IsReadFromDB = True
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            objParam = New SqlParameter(0) {}
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From ezoutlooksync_history Where Isdeleted=0 and outlooksync_historyid=@outlooksync_historyid"
            param = New SqlParameter("@outlooksync_historyid", objread.Outlooksync_historyid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezoutlooksync_history.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read Then
                objread.Outlooksync_historyid = GetInteger(sqlRdr("Outlooksync_historyid"))
                objread.OutlookSyncId = GetInteger(sqlRdr("OutlookSyncId"))
                objread.SyncDate = sqlRdr("SyncDate").ToString()
                objread.SyncStatus = sqlRdr("SyncStatus").ToString()
                objread.Createdon = sqlRdr("Createdon").ToString()
                objread.Updatedon = sqlRdr("Updatedon").ToString()
                objread.Createdby = sqlRdr("Createdby").ToString()
                objread.updatedby = sqlRdr("updatedby").ToString()
                objread.Createdby1 = sqlRdr("Createdby1").ToString()
                objread.updatedby1 = sqlRdr("updatedby1").ToString()
            Else
                Return
            End If

        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objread.IsModified = False

        End Try
    End Sub
    'Public Function ReadAlleZERSSync_History() As System.Collections.Generic.List(Of IeZOutlookSync_History)
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    Dim lstItems As New System.Collections.Generic.List(Of IeZOutlookSync_History)()
    '    Dim objItem As IeZOutlookSync_History
    '    Try
    '        Dim strQry As String = ""
    '        strQry = "Select eZERSSyncid From eZERSSync_History where Isdeleted=0 order by eZERSSyncid"
    '        Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid eZERSSync_History.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        While sqlRdr.Read()
    '            objItem = GlobalInstance.ezerssync_History(GetSmallInterger(sqlRdr("eZERSSyncid")))
    '            objItem.eZERSSyncid = GetSmallInterger(sqlRdr("eZERSSyncid"))
    '            lstItems.Add(objItem)
    '        End While
    '        Return lstItems
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '    End Try
    'End Function
    'Public Function ReadFilteredeZERSSync_History(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZOutlookSync_History)
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    Dim lstItems As New System.Collections.Generic.List(Of IeZOutlookSync_History)()
    '    Dim objItem As IeZOutlookSync_History
    '    Try
    '        Dim strQry As String = ""
    '        If Criteria <> "All" Then
    '            strQry = "Select eZERSSyncid From eZERSSync_History where Isdeleted=0 and "
    '            strQry = strQry & Criteria
    '            strQry = strQry & " like N'%"
    '            strQry = strQry & Unquote(Value)
    '            strQry = strQry & "%' "
    '            strQry = strQry & " order by eZERSSyncid"
    '        Else
    '            strQry = "Select eZERSSyncid From eZERSSync_History where Isdeleted=0 order by eZERSSyncid"
    '        End If
    '        Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid eZERSSync_History.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        While sqlRdr.Read()
    '            objItem = GlobalInstance.ezerssync_History(GetSmallInterger(sqlRdr("eZERSSyncid")))
    '            objItem.eZERSSyncid = GetSmallInterger(sqlRdr("eZERSSyncid"))
    '            lstItems.Add(objItem)
    '        End While
    '        Return lstItems
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '    End Try
    'End Function
    'Public Function ReadSelectedeZERSSync_History(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZOutlookSync_History)
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    Dim lstItems As New System.Collections.Generic.List(Of IeZOutlookSync_History)()
    '    Dim objItem As IeZOutlookSync_History
    '    Try
    '        Dim strQry As String = ""
    '        If Criteria <> "All" Then
    '            strQry = "Select eZERSSyncid From ezerssync_History where Isdeleted=0 and "
    '            strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
    '            strQry = strQry & " =N'"
    '            strQry = strQry & Unquote(Value)
    '            strQry = strQry & "' "
    '            strQry = strQry & " order by eZERSSyncid"
    '        Else
    '            strQry = "Select eZERSSyncid From ezerssync_History where Isdeleted=0 order by eZERSSyncid"
    '        End If
    '        Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid ezerssync_History.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        While sqlRdr.Read()
    '            objItem = GlobalInstance.ezerssync_History(GetSmallInterger(sqlRdr("eZERSSyncid")))
    '            objItem.eZERSSyncid = GetSmallInterger(sqlRdr("eZERSSyncid"))
    '            lstItems.Add(objItem)
    '        End While
    '        Return lstItems
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '    End Try
    'End Function
End Class
