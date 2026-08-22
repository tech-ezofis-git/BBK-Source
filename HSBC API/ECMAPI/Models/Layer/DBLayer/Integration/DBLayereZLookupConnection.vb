Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "Template Details"

  
    Public Function CreateeZLookupConnection(objtemp As eZLookupConnection) As IeZLookupConnection
        Dim newObject As IeZLookupConnection = Nothing
        If String.IsNullOrEmpty(objtemp.LookupConnName) Then
            Return Nothing
        End If
        objtemp.LookupConnName = objtemp.LookupConnName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            'error in creating multiple lookup so hide by raja 

            'strQry = "Select LookupConnStrId From eZLookupConnection Where datasource = @datasource And provider=@provider and Isdeleted=0"
            'objParam = New SqlParameter(1) {}
            'param = New SqlParameter("@datasource", objtemp.DataSource)
            'objParam(0) = param
            'param = New SqlParameter("@provider", objtemp.provider)
            'objParam(1) = param
            'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            'If obj IsNot Nothing Then
            '    Throw New Exception("LookupConnection Code already exist!")
            'End If
            strQry = "INSERT INTO eZLookupConnection(LookupConnName,provider,UserId,LookupServerTypeId,DataSource,Pasword,databasename,CreatedOn,CreatedBy) VALUES(@LookupConnName,@provider,@UserId,@LookupServerTypeId,@DataSource,@Pasword,@databasename,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@LookupConnName", objtemp.LookupConnName)
            objParam(0) = param
            param = New SqlParameter("@UserId", objtemp.UserId)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@LookupServerTypeId", objtemp.LookupServerTypeId)
            objParam(4) = param
            param = New SqlParameter("@DataSource", objtemp.DataSource)
            objParam(5) = param
            param = New SqlParameter("@Pasword", objtemp.Pasword)
            objParam(6) = param
            param = New SqlParameter("@provider", objtemp.provider)
            objParam(7) = param
            param = New SqlParameter("@databasename", objtemp.databasename)
            objParam(8) = param
            Dim obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZLookupConnection(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZLookupConnection)
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
            If objRead.LookupConnName Is Nothing Then
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_ConnectionString(LookupConnStrId) as ConnectionString,dbo.UDF_Lookupcountbyconnstrid (LookupConnStrId) as connectedlookup,dbo.udf_LookupServerType(LookupServerTypeId) as LookupServerType,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZLookupConnection Where Isdeleted=0 and LookupConnStrId=@LookupConnStrId"
                param = New SqlParameter("@LookupConnStrId", objRead.LookupConnStrId)
                objParam(0) = param
            Else
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_ConnectionString(LookupConnStrId) as ConnectionString,dbo.UDF_Lookupcountbyconnstrid (LookupConnStrId) as connectedlookup,dbo.udf_LookupServerType(LookupServerTypeId) as LookupServerType,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZLookupConnection Where Isdeleted=0 and LookupConnName=@LookupConnName"
                param = New SqlParameter("@LookupConnName", objRead.LookupConnName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupConnection.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.LookupConnStrId = GetInteger(sqlRdr("LookupConnStrId"))
                objRead.LookupConnName = sqlRdr("LookupConnName").ToString()
                objRead.Pasword = sqlRdr("Pasword").ToString()
                objRead.DataSource = sqlRdr("DataSource").ToString()
                objRead.ConnectionString = sqlRdr("ConnectionString").ToString
                objRead.connectedlookup = sqlRdr("connectedlookup").ToString
                objRead.conn = sqlRdr("conn").ToString
                objRead.LookupServerType = sqlRdr("LookupServerType").ToString()
                objRead.LookupServerTypeId = GetSmallInterger(sqlRdr("LookupServerTypeId"))
                objRead.provider = sqlRdr("provider").ToString()
                objRead.Databasename = sqlRdr("Databasename").ToString()
                objRead.UserId = sqlRdr("UserId").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZLookupConnection.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZLookupConnection() As System.Collections.Generic.List(Of IeZLookupConnection)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupConnection)()
        Dim objItem As IeZLookupConnection
        Try
            Dim strQry As String = ""
            strQry = "Select LookupConnStrId From eZLookupConnection where Isdeleted=0 order by LookupConnName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupConnection.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupConnection(GetSmallInterger(sqlRdr("LookupConnStrId")))
                objItem.LookupConnStrId = GetSmallInterger(sqlRdr("LookupConnStrId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZLookupConnection(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLookupConnection)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupConnection)()
        Dim objItem As IeZLookupConnection
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupConnStrId From eZLookupConnection where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like '%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by LookupConnName"
            Else
                strQry = "Select LookupConnStrId From eZLookupConnection where Isdeleted=0 order by LookupConnName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupConnection.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupConnection(GetSmallInterger(sqlRdr("LookupConnStrId")))
                objItem.LookupConnStrId = GetSmallInterger(sqlRdr("LookupConnStrId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZLookupConnection(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLookupConnection)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupConnection)()
        Dim objItem As IeZLookupConnection
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupConnStrId From eZLookupConnection where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(200)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by LookupConnName"
            Else
                strQry = "Select LookupConnStrId From eZLookupConnection where Isdeleted=0 order by LookupConnName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookupConnection.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupConnection(GetSmallInterger(sqlRdr("LookupConnStrId")))
                objItem.LookupConnStrId = GetSmallInterger(sqlRdr("LookupConnStrId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
  
    Public Sub Update(objToUpdate As IeZLookupConnection)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        'shankar
        'Dim obj As Object
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        ' strQry = "Select LookupConnStrId From eZLookupConnection Where LookupConnName = @LookupConnName and LookupConnStrId <> @LookupConnStrId and Isdeleted=0"
        ' objParam = New SqlParameter(1) {}
        ' param = New SqlParameter("@LookupConnName", objToUpdate.LookupConnName)
        ' objParam(0) = param
        ' param = New SqlParameter("@LookupConnStrId", objToUpdate.LookupConnStrId)
        ' objParam(1) = param
        '= SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        ' If obj IsNot Nothing Then
        '     Throw New Exception("eZLookupConnection Code already exist!")
        ' Else
        strQry = "Update eZLookupConnection Set LookupConnName=@LookupConnName,Pasword=@Pasword,DataSource=@DataSource,provider=@provider,databasename=@databasename,UserId=@UserId,LookupServerTypeId=@LookupServerTypeId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where LookupConnStrId=@LookupConnStrId"
        objParam = New SqlParameter(9) {}
        param = New SqlParameter("@Pasword", objToUpdate.Pasword)
        objParam(0) = param
        param = New SqlParameter("@LookupServerTypeId", objToUpdate.LookupServerTypeId)
        objParam(1) = param
        param = New SqlParameter("@UserId", objToUpdate.UserId)
        objParam(2) = param
        param = New SqlParameter("@LookupConnStrId", objToUpdate.LookupConnStrId)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@DataSource", objToUpdate.DataSource)
        objParam(6) = param
        param = New SqlParameter("@provider", objToUpdate.provider)
        objParam(7) = param
        param = New SqlParameter("@databasename", objToUpdate.Databasename)
        objParam(8) = param
        param = New SqlParameter("@LookupConnName", objToUpdate.LookupConnName)
        objParam(9) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")


        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZLookupConnection)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLookupConnection set Isdeleted=1 where LookupConnStrId=@LookupConnStrId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LookupConnStrId", objToDelete.LookupConnStrId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

