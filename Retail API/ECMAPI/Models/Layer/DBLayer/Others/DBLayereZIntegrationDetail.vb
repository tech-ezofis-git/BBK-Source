Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateIntegrationDetail(objEmp As eZIntegrationDetail) As IeZIntegrationDetail
        Dim newObject As IeZIntegrationDetail = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZIntegrationDetail(IntegrationName,IGServerType,IGDatasource,IGUserId,IGPassword,IGeZURL,IGStatus,CreatedOn,CreatedBy) VALUES(@IntegrationName,@IGServerType,@IGDataSource,@IGUserId,@IGPassword,@IGeZURL,@IGStatus,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@IntegrationName", objEmp.IntegrationName)
            objParam(0) = param
            param = New SqlParameter("@IGServerType", objEmp.IGServerType)
            objParam(1) = param
            param = New SqlParameter("@IGDataSource", objEmp.IGDataSource)
            objParam(2) = param
            param = New SqlParameter("@IGUserId", objEmp.IGUserId)
            objParam(3) = param
            param = New SqlParameter("@IGPassword", objEmp.IGPassword)
            objParam(4) = param
            param = New SqlParameter("@IGeZURL", objEmp.IGeZURL)
            objParam(5) = param
            param = New SqlParameter("@IGStatus", objEmp.IGStatus)
            objParam(6) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(7) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(8) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZIntegrationDetail(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZIntegrationDetail)
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

            objParam = New SqlParameter(0) {}
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZIntegrationDetail Where IntegrationId=@IntegrationId and Isdeleted=0"
            param = New SqlParameter("@IntegrationId", objRead.IntegrationId)
            objParam(0) = param


            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Integration Detail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.IntegrationId = GetInteger(sqlRdr("IntegrationId"))
                objRead.IntegrationName = sqlRdr("IntegrationName").ToString()
                objRead.IGServerType = sqlRdr("IGServerType").ToString()
                objRead.IGDataSource = sqlRdr("IGDataSource").ToString()
                objRead.IGUserId = sqlRdr("IGUserId").ToString()
                objRead.IGPassword = sqlRdr("IGPassword").ToString()
                objRead.IGeZURL = sqlRdr("IGeZURL").ToString()
                objRead.IGStatus = GetInteger(sqlRdr("IGStatus"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
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
    Public Function ReadAllIntegrationDetail() As System.Collections.Generic.List(Of IeZIntegrationDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZIntegrationDetail)()
        Dim objItem As IeZIntegrationDetail

        Try
            Dim strQry As String = ""
            strQry = "Select IntegrationId From eZIntegrationDetail where Isdeleted=0 order by Integrationid"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Integration Detail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZIntegrationDetail(GetInteger(sqlRdr("IntegrationId")))
                objItem.IntegrationId = GetInteger(sqlRdr("IntegrationId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedeZIntegrationDetail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZIntegrationDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZIntegrationDetail)()
        Dim objItem As IeZIntegrationDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select IntegrationId From eZIntegrationDetail where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by IntegrationId"
            Else
                strQry = "Select IntegrationId From eZIntegrationDetail where Isdeleted=0 order by IntegrationId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZIntegrationDetail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZIntegrationDetail(GetSmallInterger(sqlRdr("IntegrationId")))
                objItem.IntegrationId = GetSmallInterger(sqlRdr("IntegrationId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZIntegrationDetail)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        'strQry = "Select ScheduleId From eZSchedule Where ScheduleId <> @ScheduleId and Isdeleted=0"
        'objParam = New SqlParameter(0) {}
        'param = New SqlParameter("@ScheduleId", objToUpdate.ScheduleId)
        'objParam(0) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("Schedule Code already exist!")
        'Else
        strQry = "Update eZIntegrationdetail Set IntegrationName=@IntegrationName,IGServerType=@IGServerType,IGDataSource=@IGDataSource,IGUserId=@IGUserId,IGPassword=@IGPassword,IGeZURL=@IGeZURL,IGStatus=@IGStatus,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where IntegrationId=@IntegrationId"
        objParam = New SqlParameter(9) {}
        param = New SqlParameter("@IntegrationName", objToUpdate.IntegrationName)
        objParam(0) = param
        param = New SqlParameter("@IntegrationID", objToUpdate.IntegrationId)
        objParam(1) = param
        param = New SqlParameter("@IGServerType", objToUpdate.IGServerType)
        objParam(2) = param
        param = New SqlParameter("@IGDataSource", objToUpdate.IGDataSource)
        objParam(3) = param
        param = New SqlParameter("@IGUserId", objToUpdate.IGUserId)
        objParam(4) = param
        param = New SqlParameter("@IGPassword", objToUpdate.IGPassword)
        objParam(5) = param
        param = New SqlParameter("@IGeZURL", objToUpdate.IGeZURL)
        objParam(6) = param
        param = New SqlParameter("@IGStatus", objToUpdate.IGStatus)
        objParam(7) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(8) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(9) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZIntegrationDetail)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZIntegrationDetail set Isdeleted=1 where IntegrationId=@IntegrationId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@IntegrationId", objToDelete.IntegrationId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class