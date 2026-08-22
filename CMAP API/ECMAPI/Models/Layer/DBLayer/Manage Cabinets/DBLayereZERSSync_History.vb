Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common


Partial Public Class DBLayer

    Public Function CreateeZERSsyncHistory(objtemp As eZERSSync_History) As IeZERSSync_History
        Dim newObject As IeZERSSync_History = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            Dim obj As Object

            strQry = "INSERT INTO eZERSSync_History(eZERSSyncid,Scheduleid,NO_OF_Files_Copied,Status,CreatedOn,CreatedBy) VALUES(@eZERSSyncid,@Scheduleid,@NO_OF_Files_Copied,@Status,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@Scheduleid", objtemp.Scheduleid)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objtemp.Createdon)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objtemp.Createdby)
            objParam(2) = param
            param = New SqlParameter("@Status", objtemp.Status)
            objParam(3) = param
            param = New SqlParameter("@NO_OF_Files_Copied", objtemp.NO_OF_Files_Copied)
            objParam(4) = param
            param = New SqlParameter("@eZERSSyncid", objtemp.eZERSSyncid)
            objParam(5) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.ezerssync_History(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub read(ByVal objread As IeZERSSync_History)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZERSSync_History Where Isdeleted=0 and ezerssync_historyid=@ezerssync_historyid"
            param = New SqlParameter("@ezerssync_historyid", objread.ezerssync_historyid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSSync.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read Then
                objread.ezerssync_historyid = GetInteger(sqlRdr("ezerssync_historyid"))
                objread.eZERSSyncid = GetInteger(sqlRdr("eZERSSyncid"))
                objread.Scheduleid = sqlRdr("eZERSSyncname").ToString()
                objread.NO_OF_Files_Copied = GetInteger(sqlRdr("NO_OF_Files_Copied").ToString())
                objread.Status = sqlRdr("Status").ToString()
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


    Public Function ReadAlleZERSSync_History() As System.Collections.Generic.List(Of IeZERSSync_History)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSSync_History)()
        Dim objItem As IeZERSSync_History
        Try
            Dim strQry As String = ""
            strQry = "Select eZERSSyncid From eZERSSync_History where Isdeleted=0 order by eZERSSyncid"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSSync_History.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezerssync_History(GetSmallInterger(sqlRdr("eZERSSyncid")))
                objItem.eZERSSyncid = GetSmallInterger(sqlRdr("eZERSSyncid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZERSSync_History(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZERSSync_History)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSSync_History)()
        Dim objItem As IeZERSSync_History
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select eZERSSyncid From eZERSSync_History where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by eZERSSyncid"
            Else
                strQry = "Select eZERSSyncid From eZERSSync_History where Isdeleted=0 order by eZERSSyncid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSSync_History.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezerssync_History(GetSmallInterger(sqlRdr("eZERSSyncid")))
                objItem.eZERSSyncid = GetSmallInterger(sqlRdr("eZERSSyncid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZERSSync_History(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZERSSync_History)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSSync_History)()
        Dim objItem As IeZERSSync_History
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select eZERSSyncid From ezerssync_History where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by eZERSSyncid"
            Else
                strQry = "Select eZERSSyncid From ezerssync_History where Isdeleted=0 order by eZERSSyncid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezerssync_History.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezerssync_History(GetSmallInterger(sqlRdr("eZERSSyncid")))
                objItem.eZERSSyncid = GetSmallInterger(sqlRdr("eZERSSyncid"))
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
