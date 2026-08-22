
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer

    Public Function createoutlooksync(objtemp As eZOutlooksync) As IeZOutlooksync
        Dim newobject As IeZOutlooksync = Nothing
        Try
            Dim strqry = ""
            ' Dim obj As Object
            Dim objparam As SqlParameter()
            Dim param As SqlParameter
            strqry = "INSERT INTO eZOutlooksync(Scheduleid,Syncname,Syncrule,SyncMail,Createdon,Createdby,updatedon) values(@Scheduleid,@Syncname,@Syncrule,@SyncMail,@Createdon,@Createdby,@updatedon); SELECT SCOPE_IDENTITY()"
            objparam = New SqlParameter(6) {}
            param = New SqlParameter("@Scheduleid", objtemp.Scheduleid)
            objparam(0) = param
            param = New SqlParameter("@Syncname", objtemp.Syncname)
            objparam(1) = param
            param = New SqlParameter("@Syncrule", objtemp.Syncrule)
            objparam(2) = param
            param = New SqlParameter("@SyncMail", objtemp.SyncMail)
            objparam(3) = param
            param = New SqlParameter("@Createdon", objtemp.Createdon)
            objparam(4) = param
            param = New SqlParameter("@Createdby", objtemp.Createdby)
            objparam(5) = param
            param = New SqlParameter("@updatedon", objtemp.updatedon)
            objparam(6) = param

            Dim obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strqry.ToString(), objparam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newobject = GlobalInstance.eZOutlooksync(obj)
            Read(newobject)
            Return newobject

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Sub read(objread As IeZOutlooksync)
        If objread.IsReadFromDB Then
            Return
        End If
        If objread.IsModified Then
            Throw New InvalidOperationException()
        End If
        Dim sqlRdr As SqlDataReader = Nothing
        objread.IsReadFromDB = True
        Try
            Dim strqry = ""
            Dim objparam As SqlParameter()
            Dim param As SqlParameter

            strqry = "select O.*,S.ScheduleTypeId,S.ForSchedule,S.Id,S.WeekDay,S.Mont,S.Day,S.EachDay,S.OnceDate,S.Time FROM eZOutlookSync as O join ezschedule as S ON O.Scheduleid=S.Scheduleid WHERE S.isdeleted=0 and O.isdeleted=0 and  O.Outlooksyncid = @Outlooksyncid"
            objparam = New SqlParameter(0) {}
            param = New SqlParameter("@Outlooksyncid", objread.Outlooksyncid)
            objparam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString(), objparam)
            If obj Is Nothing Then
                Throw New Exception("")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objread.Outlooksyncid = sqlRdr("Outlooksyncid")
                objread.Scheduleid = GetInteger(sqlRdr("Scheduleid"))
                objread.SyncMail = sqlRdr("SyncMail")
                objread.Syncname = sqlRdr("Syncname")
                objread.Syncrule = sqlRdr("Syncrule")
                objread.ScheduleTypeId = GetInteger(sqlRdr("ScheduleTypeId"))
                objread.ForSchedule = GetInteger(sqlRdr("ForSchedule"))
                objread.Id = GetInteger(sqlRdr("Id"))
                objread.WeekDay = GetInteger(sqlRdr("WeekDay"))
                objread.Mont = GetInteger(sqlRdr("Mont"))
                objread.Day = GetInteger(sqlRdr("Day"))
                objread.EachDay = GetInteger(sqlRdr("EachDay"))
                objread.OnceDate = sqlRdr("OnceDate")
                objread.Time = sqlRdr("Time")
                objread.Createdby = sqlRdr("Createdby")
                objread.Createdon = sqlRdr("Createdon")
                objread.updatedby = sqlRdr("updatedby")
                objread.updatedon = sqlRdr("updatedon")
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


    Public Function ReadAlleZoutlooksync() As System.Collections.Generic.List(Of IeZOutlooksync)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZOutlooksync)()
        Dim objItem As IeZOutlooksync
        Try
            Dim strQry As String = ""
            strQry = "Select Outlooksyncid From eZOutlooksync where Isdeleted=0 order by Outlooksyncid"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Outlooksyncid.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZOutlooksync(GetSmallInterger(sqlRdr("Outlooksyncid")))
                objItem.Outlooksyncid = GetSmallInterger(sqlRdr("Outlooksyncid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function


    Public Function ReadSelectedeZoutlooksync(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZOutlooksync)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZOutlooksync)()
        Dim objItem As IeZOutlooksync
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Outlooksyncid From ezoutlooksync where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Outlooksyncid"
            Else
                strQry = "Select Outlooksyncid From ezoutlooksync where Isdeleted=0 order by Outlooksyncid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Outlooksync.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZOutlooksync(GetSmallInterger(sqlRdr("Outlooksyncid")))
                objItem.Outlooksyncid = GetSmallInterger(sqlRdr("Outlooksyncid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZOutlooksync)
      
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZOutlookSync Set Scheduleid=@Scheduleid,Syncname=@Syncname,Syncrule=@Syncrule,SyncMail=@SyncMail,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where Outlooksyncid=@Outlooksyncid"
            objParam = New SqlParameter(6) {}
        param = New SqlParameter("@Syncname", objToUpdate.Syncname)
            objParam(0) = param
        param = New SqlParameter("@Scheduleid", objToUpdate.Scheduleid)
            objParam(1) = param
        param = New SqlParameter("@Syncrule", objToUpdate.Syncrule)
            objParam(2) = param
        param = New SqlParameter("@SyncMail", objToUpdate.SyncMail)
            objParam(3) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(4) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(5) = param
        param = New SqlParameter("@Outlooksyncid", objToUpdate.Outlooksyncid)
            objParam(6) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")

            End If

        objToUpdate.IsModified = False
    End Sub

End Class
