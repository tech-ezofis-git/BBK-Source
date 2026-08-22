Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "eZLookup Details"
    Public Function CreateeZLookup(objtemp As eZLookup) As IeZLookup
        Dim newObject As IeZLookup = Nothing
        Try
            Dim strQry As String = ""

            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            Dim obj As Object
            strQry = "Select LookupId From eZLookup Where  Lookupname=@Lookupname and Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@Lookupname", objtemp.lookupname)
            objParam(0) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("Lookup name already exist!")
            End If
            strQry = "INSERT INTO eZLookup(TemplateId,Lookupname,LookupTypeId,LookupConnStrId,LookupValue,Schedule,CreatedOn,CreatedBy) " +
                "VALUES(@TemplateId,@Lookupname,@LookupTypeId,@LookupConnStrId,@LookupValue,@schedule,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@LookupConnStrId", objtemp.LookupConnStrId)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@LookupValue", objtemp.LookupValue)
            objParam(4) = param
            param = New SqlParameter("@LookupTypeId", objtemp.LookupTypeId)
            objParam(5) = param
            param = New SqlParameter("@Lookupname", objtemp.lookupname)
            objParam(6) = param
            param = New SqlParameter("@schedule", objtemp.schedule)
            objParam(7) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZLookup(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.ToString)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZLookup)
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
            strQry = "Select *,dbo.udf_Connectionnamebyconnectionstringid(LookupConnStrId) as lookupconnname," +
                "dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_ConnectionString(LookupConnStrId) as ConnectionString," +
                "dbo.udf_Scheduletimebylookupid(lookupid) as Scheduletime,dbo.udf_LookupType(LookupTypeId) as LookupType," +
                "dbo.udf_LookupServerTypeId(LookupConnStrId) as LookupServerTypeId,dbo.udf_UserName(CreatedBy) as CreatedBy1  " +
                "From eZLookup Where Isdeleted=0 and LookupId=@LookupId"
            param = New SqlParameter("@LookupId", objRead.LookupId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookup.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.LookupId = GetInteger(sqlRdr("LookupId"))
                objRead.LookupType = sqlRdr("LookupType").ToString
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.LookupServerTypeId = GetInteger(sqlRdr("LookupServerTypeId"))
                objRead.LookupTypeId = GetInteger(sqlRdr("LookupTypeId"))
                objRead.Scheduletime = sqlRdr("Scheduletime").ToString()
                objRead.LookupConnStrId = GetInteger(sqlRdr("LookupConnStrId"))
                objRead.ConnectionString = sqlRdr("ConnectionString").ToString
                objRead.LookupValue = sqlRdr("LookupValue").ToString
                objRead.schedule = Convert.ToInt32(Convert.ToBoolean(sqlRdr("Schedule").ToString()))
                objRead.lookupname = sqlRdr("Lookupname").ToString()
                objRead.Lookupconnectionname = sqlRdr("lookupconnname").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZLookup.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZLookup() As System.Collections.Generic.List(Of IeZLookup)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookup)()
        Dim objItem As IeZLookup
        Try
            Dim strQry As String = ""
            strQry = "Select LookupId From eZLookup where Isdeleted=0 order by LookupId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookup.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookup(GetSmallInterger(sqlRdr("LookupId")))
                objItem.LookupId = GetSmallInterger(sqlRdr("LookupId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZLookup(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLookup)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookup)()
        Dim objItem As IeZLookup
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupId From eZLookup where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like '%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by LookupId"
            Else
                strQry = "Select LookupId From eZLookup where Isdeleted=0 order by LookupId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookup.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookup(GetSmallInterger(sqlRdr("LookupId")))
                objItem.LookupId = GetSmallInterger(sqlRdr("LookupId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZLookup(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLookup)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookup)()
        Dim objItem As IeZLookup
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LookupId From eZLookup where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(200)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by LookupId"
            Else
                strQry = "Select LookupId From eZLookup where Isdeleted=0 order by LookupId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLookup.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookup(GetSmallInterger(sqlRdr("LookupId")))
                objItem.LookupId = GetSmallInterger(sqlRdr("LookupId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZLookup)
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
        'strQry = "Select LookupId From eZLookup Where TemplateId = @TemplateId and LookupId <> @LookupId and Isdeleted=0"
        'objParam = New SqlParameter(1) {}
        'param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        'objParam(0) = param
        'param = New SqlParameter("@LookupId", objToUpdate.LookupId)
        'objParam(1) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("eZLookup Code already exist!")
        'Else
        strQry = "Update eZLookup Set TemplateId=@TemplateId,Lookupname=@lookupname,schedule=@schedule,LookupConnStrId=@LookupConnStrId,LookupValue=@LookupValue,LookupTypeId=@LookupTypeId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where LookupId=@LookupId"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(0) = param
        param = New SqlParameter("@LookupConnStrId", objToUpdate.LookupConnStrId)
        objParam(1) = param
        param = New SqlParameter("@LookupTypeId", objToUpdate.LookupTypeId)
        objParam(2) = param
        param = New SqlParameter("@LookupValue", objToUpdate.LookupValue)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@LookupId", objToUpdate.LookupId)
        objParam(6) = param
        param = New SqlParameter("@schedule", objToUpdate.schedule)
        objParam(7) = param
        param = New SqlParameter("@Lookupname", objToUpdate.lookupname)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")

        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZLookup)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLookup set Isdeleted=1 where LookupId=@LookupId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LookupId", objToDelete.LookupId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

