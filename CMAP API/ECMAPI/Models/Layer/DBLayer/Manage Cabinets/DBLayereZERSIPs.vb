Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "ERS Details"

    Public Function CreateeZERSIPs(objtemp As eZERSIPs) As IeZERSIPs
        Dim newObject As IeZERSIPs = Nothing

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ERSIPID From eZERSIPs Where Isdeleted=0 And ERSId=@ERSId And (( Convert(numeric,replace (FromIP,'.','')) between Convert(numeric,replace (@FromIP,'.','')) and Convert(numeric,replace (@ToIP,'.',''))) or (Convert(numeric,replace (ToIP,'.','')) between Convert(numeric,replace (@FromIP,'.','')) and Convert(numeric,replace (@ToIP,'.',''))))"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@ERSId", objtemp.ERSId)
            objParam(0) = param
            param = New SqlParameter("@FromIP", objtemp.FromIP)
            objParam(1) = param
            param = New SqlParameter("@ToIP", objtemp.ToIP)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("IP already exist!")
            End If
            strQry = "INSERT INTO eZERSIPs(ERSId,FromIP,ToIP,CreatedOn,CreatedBy) VALUES(@ERSId,@FromIP,@ToIP,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@ERSId", objtemp.ERSId)
            objParam(0) = param
            param = New SqlParameter("@FromIP", objtemp.FromIP)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@ToIP", objtemp.ToIP)
            objParam(4) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZERSIPs(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZERSIPs)
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
            If objRead.ERSName Is Nothing Then
                strQry = "Select eZERSIPs.*,dbo.udf_UserName(eZERSIPs.UpdatedBy) as UpdatedBy1,dbo.udf_UserName(eZERSIPs.CreatedBy) as CreatedBy1,eZERSInfo.ERSServerName as ERSServerName,eZERSInfo.ERSName as ERSName,eZERSInfo.SettingPath as SettingPath,eZERSInfo.ERSDirPath as ERSDirPath From eZERSIPs,eZERSInfo Where eZERSInfo.ERSId= eZERSIPs.ERSId and eZERSInfo.Isdeleted=0 and eZERSIPs.Isdeleted=0 and ERSIPID=@ERSIPID"
                param = New SqlParameter("@ERSIPID", objRead.ERSIPID)
                objParam(0) = param
            Else
                strQry = "Select eZERSIPs.*,dbo.udf_UserName(eZERSIPs.UpdatedBy) as UpdatedBy1,dbo.udf_UserName(eZERSIPs.CreatedBy) as CreatedBy1,eZERSInfo.ERSServerName as ERSServerName,eZERSInfo.ERSName as ERSName,eZERSInfo.SettingPath as SettingPath,eZERSInfo.ERSDirPath as ERSDirPath From eZERSIPs,eZERSInfo Where eZERSInfo.ERSId= eZERSIPs.ERSId and eZERSInfo.Isdeleted=0 and eZERSIPs.Isdeleted=0 and ERSName=@ERSName"
                param = New SqlParameter("@ERSName", objRead.ERSName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSIPs.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ERSIPID = GetInteger(sqlRdr("ERSIPID"))
                objRead.FromIP = sqlRdr("FromIP").ToString()
                objRead.ToIP = sqlRdr("ToIP").ToString()
                objRead.ERSId = GetInteger(sqlRdr("ERSId"))
                objRead.ERSName = sqlRdr("ERSName").ToString()
                objRead.ERSDirPath = GetSmallInterger(sqlRdr("ERSDirPath"))
                objRead.SettingPath = GetSmallInterger(sqlRdr("SettingPath"))
                objRead.ERSServerName = sqlRdr("ERSServerName").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZERSIPs.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZERSIPs() As System.Collections.Generic.List(Of IeZERSIPs)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSIPs)()
        Dim objItem As IeZERSIPs
        Try
            Dim strQry As String = ""
            strQry = "Select ERSIPID From eZERSIPs where Isdeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSIPs.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZERSIPs(GetSmallInterger(sqlRdr("ERSIPID")))
                objItem.ERSIPID = GetSmallInterger(sqlRdr("ERSIPID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZERSIPs(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZERSIPs)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSIPs)()
        Dim objItem As IeZERSIPs
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ERSIPID From eZERSIPs where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "

            Else
                strQry = "Select ERSIPID From eZERSIPs where Isdeleted=0 "
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSIPs.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZERSIPs(GetSmallInterger(sqlRdr("ERSIPID")))
                objItem.ERSId = GetSmallInterger(sqlRdr("ERSIPID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZERSIPs(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZERSIPs)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSIPs)()
        Dim objItem As IeZERSIPs
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ERSIPID From eZERSIPs where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "

            Else
                strQry = "Select ERSIPID From eZERSIPs where Isdeleted=0 "
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSIPs.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZERSIPs(GetSmallInterger(sqlRdr("ERSIPID")))
                objItem.ERSId = GetSmallInterger(sqlRdr("ERSIPID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadERSByIP(ByVal IP As String) As DataSet
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New DataSet
        Try
            Dim strQry As String = ""
            strQry = "Select eZERSInfo.ERSName as ERSName,eZERSInfo.SettingPath as SettingPath,eZERSInfo.ERSDirPath as ERSDirPath From eZERSIPs," +
                "eZERSInfo Where eZERSInfo.ERSId= eZERSIPs.ERSId and eZERSInfo.Isdeleted=0 and eZERSIPs.Isdeleted=0 and " +
                "Convert(numeric,replace ('" + IP + "','.','')) between Convert(numeric,replace (eZERSIPs.FromIP,'.','')) and " +
                "Convert(numeric,replace (eZERSIPs.ToIP,'.',''))"
            Dim obj As Object = SqlHelper.ExecuteDataset(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            lstItems = obj
            Return lstItems
        Catch ex As Exception

        End Try
        Return lstItems
    End Function
    Public Sub Update(objToUpdate As IeZERSIPs)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ERSIPID From eZERSIPs Where ERSIPID <> @ERSIPID and Isdeleted=0"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ERSIPID", objToUpdate.ERSIPID)
        objParam(0) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZERSIPs Code already exist!")
        Else
            strQry = "Update eZERSIPs Set ERSId=@ERSId,FromIP=@FromIP,ToIP=@ToIP,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ERSIPID=@ERSIPID"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@ERSId", objToUpdate.ERSId)
            objParam(0) = param
            param = New SqlParameter("@FromIP", objToUpdate.FromIP)
            objParam(1) = param
            param = New SqlParameter("@ToIP", objToUpdate.ToIP)
            objParam(2) = param
            param = New SqlParameter("@ERSIPID", objToUpdate.ERSIPID)
            objParam(3) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(4) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(5) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")

            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZERSIPs)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZERSIPs set Isdeleted=1 where ERSIPID=@ERSIPID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ERSIPID", objToDelete.ERSIPID)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

