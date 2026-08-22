Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateTrialLicense(objEmp As eZTrialLicense) As IeZTrialLicense
        Dim newObject As IeZTrialLicense = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter

            strQry = "Select TrialId From eZTrialLicense Where LicenseId = @LicenseId And LicenseClientId = @LicenseClientId And TrialKey=@TrialKey And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@LicenseId", objEmp.LicenseId)
            objParam(0) = param
            param = New SqlParameter("@LicenseClientId", objEmp.LicenseClientId)
            objParam(1) = param
            param = New SqlParameter("@TrialKey", objEmp.TrialKey)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("Trial Key already exist!")
            End If

            strQry = "INSERT INTO eZTrialLicense(LicenseClientId,LicenseId,TrialKey,CreatedOn,CreatedBy) VALUES(@LicenseClientId,@LicenseId,@TrialKey,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@LicenseClientId", objEmp.LicenseClientId)
            objParam(0) = param
            param = New SqlParameter("@LicenseId", objEmp.LicenseId)
            objParam(1) = param

            param = New SqlParameter("@TrialKey", objEmp.TrialKey)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZTrialLicense(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZTrialLicense)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZTrialLicense Where TrialId=@TrialId and Isdeleted=0"
            param = New SqlParameter("@TrialId", objRead.TrialId)
            objParam(0) = param


            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Trial License.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.TrialId = GetInteger(sqlRdr("TrialId"))
                objRead.LicenseClientId = GetInteger(sqlRdr("LicenseClientId"))
                objRead.Licenseid = GetInteger(sqlRdr("LicenseId"))
                objRead.TrialKey = sqlRdr("TrialKey")
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
    Public Function ReadAllTrialLicense() As System.Collections.Generic.List(Of IeZTrialLicense)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTrialLicense)()
        Dim objItem As IeZTrialLicense

        Try
            Dim strQry As String = ""
            strQry = "Select TrialId From eZTrialLicense where Isdeleted=0 order by TrialId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Trial License.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTrialLicense(GetInteger(sqlRdr("TrialId")))
                objItem.TrialId = GetInteger(sqlRdr("TrialId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedeZTrialLicense(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTrialLicense)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTrialLicense)()
        Dim objItem As IeZTrialLicense
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TrialId From eZTrialLicense where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by TrialId"
            Else
                strQry = "Select TrialId From eZTrialLicense where Isdeleted=0 order by TrialId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Trial License.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTrialLicense(GetSmallInterger(sqlRdr("TrialId")))
                objItem.LicenseId = GetSmallInterger(sqlRdr("TrialId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZTrialLicense)
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
        strQry = "Update eZTrialLicense Set LicenseId=@LicenseId,LicenseClientId=@LicenseClientId,TrialKey=@TrialKey,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where TrialId=@TrialId"
        objParam = New SqlParameter(5) {}
        param = New SqlParameter("@TrialId", objToUpdate.TrialId)
        objParam(0) = param
        param = New SqlParameter("@LicenseId", objToUpdate.LicenseId)
        objParam(1) = param
        param = New SqlParameter("@LicenseClientId", objToUpdate.LicenseClientId)
        objParam(2) = param
        param = New SqlParameter("@TrialKey", objToUpdate.TrialKey)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZTrialLicense)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZTrialLicense set Isdeleted=1 where TrialId=@TrialId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@TrialId", objToDelete.TrialId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class