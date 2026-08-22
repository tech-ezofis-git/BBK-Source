Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As ISyncTable)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From SyncTable ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Syncid=@Syncid and ez.Isdeleted=0"
            param = New SqlParameter("@Syncid", objRead.Syncid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid SyncTable")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Syncid = GetInteger(sqlRdr("Syncid"))
                objRead.FromERS = GetInteger(sqlRdr("FromERS"))
                objRead.Syncname = sqlRdr("Syncname").ToString()
                objRead.ToERS = GetInteger(sqlRdr("ToERS"))
                objRead.Syncdate = sqlRdr("Syncdate").ToString()
                objRead.Syncschedule = GetInteger(sqlRdr("Syncschedule"))
                objRead.Sync = sqlRdr("Sync").ToString()
                objRead.Synctime = sqlRdr("Synctime").ToString()
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
    Public Function CreateSyncTable(objEmp As SyncTable) As SyncTable
        Dim newObject As SyncTable = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO SyncTable(Syncname,FromERS,ToERS,Sync,Syncdate,Synctime,Syncschedule,CreatedBy,CreatedOn) VALUES " +
                "(@Syncname,@FromERS,@ToERS,@Sync,@Syncdate,@Synctime,@Syncschedule,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@Syncname", objEmp.Syncname)
            objParam(0) = param
            param = New SqlParameter("@FromERS", objEmp.FromERS)
            objParam(1) = param
            param = New SqlParameter("@ToERS", objEmp.ToERS)
            objParam(2) = param
            param = New SqlParameter("@Sync", objEmp.Sync)
            objParam(3) = param
            param = New SqlParameter("@Syncdate", objEmp.Syncdate)
            objParam(4) = param
            param = New SqlParameter("@Synctime", objEmp.Synctime)
            objParam(5) = param
            param = New SqlParameter("@Syncschedule", objEmp.Syncschedule)
            objParam(6) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(7) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(8) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.SyncTable(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As ISyncTable)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update SyncTable Set Syncname=@Syncname,FromERS=@FromERS,ToERS=@ToERS,Sync=@Sync,Syncdate=@Syncdate,Synctime=@Synctime," +
            "Syncschedule=@Syncschedule,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where Syncid=@Syncid"
        objParam = New SqlParameter(9) {}
        param = New SqlParameter("@Syncname", objToUpdate.Syncname)
        objParam(0) = param
        param = New SqlParameter("@FromERS", objToUpdate.FromERS)
        objParam(1) = param
        param = New SqlParameter("@ToERS", objToUpdate.ToERS)
        objParam(2) = param
        param = New SqlParameter("@Sync", objToUpdate.Sync)
        objParam(3) = param
        param = New SqlParameter("@Syncdate", objToUpdate.Syncdate)
        objParam(4) = param
        param = New SqlParameter("@Synctime", objToUpdate.Synctime)
        objParam(5) = param
        param = New SqlParameter("@Syncschedule", objToUpdate.Syncschedule)
        objParam(6) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(7) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(8) = param
        param = New SqlParameter("@Syncid", objToUpdate.Syncid)
        objParam(9) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As ISyncTable)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update SyncTable set Isdeleted=1 where Syncid=@Syncid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Syncid", objToDelete.Syncid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllSyncTable() As System.Collections.Generic.List(Of ISyncTable)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of ISyncTable)()
        Dim objItem As ISyncTable
        Try
            Dim strQry As String = ""
            strQry = "Select Syncid From SyncTable where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid SyncTable")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.SyncTable(GetInteger(sqlRdr("Syncid")))
                objItem.Syncid = GetInteger(sqlRdr("Syncid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredSyncTable(Criteria As String, Value As String) As List(Of ISyncTable)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of ISyncTable)()
        Dim objItem As ISyncTable
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Syncid From SyncTable where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Syncid"
            Else
                strQry = "Select Syncid From SyncTable where Isdeleted=0 order by Syncid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid SyncTable")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.SyncTable(GetInteger(sqlRdr("Syncid")))
                objItem.Syncid = GetInteger(sqlRdr("Syncid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedSyncTable(Criteria As String, Value As String) As System.Collections.Generic.List(Of ISyncTable)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of ISyncTable)()
        Dim objItem As ISyncTable
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Syncid From SyncTable where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Syncid"
            Else
                strQry = "Select Syncid From SyncTable where Isdeleted=0 order by Syncid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid SyncTable")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.SyncTable(GetInteger(sqlRdr("Syncid")))
                objItem.Syncid = GetInteger(sqlRdr("Syncid"))
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
