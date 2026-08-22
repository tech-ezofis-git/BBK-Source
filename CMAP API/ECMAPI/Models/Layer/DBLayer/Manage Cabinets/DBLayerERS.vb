Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "ERS Details"


    Public Function CreateeZERSInfo(objtemp As eZERSInfo) As IeZERSInfo
        Dim newObject As IeZERSInfo = Nothing
        If String.IsNullOrEmpty(objtemp.ERSName) Then
            Return Nothing
        End If
        objtemp.ERSName = objtemp.ERSName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            'strQry = "Select ERSId From eZERSInfo Where ERSName = @ERSName And Isdeleted=0"
            'objParam = New SqlParameter(0) {}
            'param = New SqlParameter("@ERSName", objtemp.ERSName)
            'objParam(0) = param
            'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            'If obj IsNot Nothing Then
            '    Throw New Exception("ERS Name already exist!")
            'End If
            strQry = "Select ERSId From eZERSInfo Where ERSName = @ERSName And ERSServerName = @ERSServerName And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ERSName", objtemp.ERSName)
            objParam(0) = param
            param = New SqlParameter("@ERSServerName", objtemp.ERSServerName)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ERS Name and Server Name Code already exist!")
            End If
            strQry = "INSERT INTO eZERSInfo(ERSName,ERSServerName,ERSDirPath,SettingPath,ERSIndexinpath,CreatedOn,CreatedBy,IsMain) VALUES(@ERSName,@ERSServerName,@ERSDirPath,@SettingPath,@ERSIndexinpath,@CreatedOn,@CreatedBy,@IsMain);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@ERSName", objtemp.ERSName)
            objParam(0) = param
            param = New SqlParameter("@ERSServerName", objtemp.ERSServerName)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@ERSDirPath", objtemp.ERSDirPath)
            objParam(4) = param
            param = New SqlParameter("@SettingPath", objtemp.SettingPath)
            objParam(5) = param
            param = New SqlParameter("@IsMain", objtemp.IsMain)
            objParam(6) = param
            param = New SqlParameter("@ERSIndexinpath", objtemp.ERSIndexinpath)
            objParam(7) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZERSInfo(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZERSInfo)
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
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZERSInfo Where Isdeleted=0 and ERSId=@ERSId"
                param = New SqlParameter("@ERSId", objRead.ERSID)
                objParam(0) = param
            Else
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZERSInfo Where Isdeleted=0 and ERSName=@ERSName"
                param = New SqlParameter("@ERSName", objRead.ERSName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ERSID = GetInteger(sqlRdr("ERSId"))
                objRead.ERSName = sqlRdr("ERSName").ToString()
                objRead.ERSDirPath = sqlRdr("ERSDirPath").ToString()
                objRead.SettingPath = sqlRdr("SettingPath").ToString()
                objRead.ERSIndexinpath = sqlRdr("ERSIndexinpath").ToString()
                objRead.ERSServerName = sqlRdr("ERSServerName").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.IsMain = sqlRdr("IsMain").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZERSInfo.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZERSInfo() As System.Collections.Generic.List(Of IeZERSInfo)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSInfo)()
        Dim objItem As IeZERSInfo
        Try
            Dim strQry As String = ""
            strQry = "Select ERSId From eZERSInfo where Isdeleted=0 order by ERSName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZERSInfo(GetSmallInterger(sqlRdr("ERSId")))
                objItem.ERSID = GetSmallInterger(sqlRdr("ERSId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZERSInfo(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZERSInfo)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSInfo)()
        Dim objItem As IeZERSInfo
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ERSId From eZERSInfo where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ERSName"
            Else
                strQry = "Select ERSId From eZERSInfo where Isdeleted=0 order by ERSName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZERSInfo(GetSmallInterger(sqlRdr("ERSId")))
                objItem.ERSID = GetSmallInterger(sqlRdr("ERSId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZERSInfo(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZERSInfo)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSInfo)()
        Dim objItem As IeZERSInfo
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ERSId From eZERSInfo where Isdeleted=0 and "
                strQry = strQry & "Convert(nvarchar(max)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ERSName"
            Else
                strQry = "Select ERSId From eZERSInfo where Isdeleted=0 order by ERSName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZERSInfo(GetSmallInterger(sqlRdr("ERSId")))
                objItem.ERSID = GetSmallInterger(sqlRdr("ERSId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZERSInfoByIP(IP As String) As System.Collections.Generic.List(Of IeZERSInfo)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSInfo)()
        Dim objItem As IeZERSInfo
        Try
            Dim strQry As String = ""
            strQry = "Select ERSId from eZERSInfo where Isdeleted=0 And ERSId in (Select ERSId From eZERSIPs Where Convert(numeric,replace (N'" + IP + "','.','')) between Convert(numeric,replace (FromIP,'.','')) and Convert(numeric,replace (ToIP,'.',''))) order by ERSName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZERSInfo(GetSmallInterger(sqlRdr("ERSId")))
                objItem.ERSId = GetSmallInterger(sqlRdr("ERSId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZERSInfo)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ERSId From eZERSInfo Where ERSName = @ERSName and ERSId <> @ERSId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ERSName", objToUpdate.ERSName)
        objParam(0) = param
        param = New SqlParameter("@ERSId", objToUpdate.ERSID)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZERSInfo Code already exist!")
        Else
            strQry = "Update eZERSInfo Set ERSIndexinpath=@ERSIndexinpath,SettingPath=@SettingPath,IsMain=@IsMain,ERSName=@ERSName,ERSServerName=@ERSServerName,ERSDirPath=@ERSDirPath,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ERSId=@ERSId"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@ERSName", objToUpdate.ERSName)
            objParam(0) = param
            param = New SqlParameter("@ERSDirPath", objToUpdate.ERSDirPath)
            objParam(1) = param
            param = New SqlParameter("@ERSServerName", objToUpdate.ERSServerName)
            objParam(2) = param
            param = New SqlParameter("@ERSId", objToUpdate.ERSID)
            objParam(3) = param
            param = New SqlParameter("@UpdatedOn", Today.Date.ToString)
            objParam(4) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(5) = param
            param = New SqlParameter("@SettingPath", objToUpdate.SettingPath)
            objParam(6) = param
            param = New SqlParameter("@IsMain", objToUpdate.IsMain)
            objParam(7) = param
            param = New SqlParameter("@ERSIndexinpath", objToUpdate.ERSIndexinpath)
            objParam(8) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")

            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZERSInfo)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZERSInfo set Isdeleted=1 where ERSId=@ERSId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ERSId", objToDelete.ERSID)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

