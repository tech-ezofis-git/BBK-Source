Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateLicense(objEmp As eZLicense) As IeZLicense
        Dim newObject As IeZLicense = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            Dim ApplicationName As String = ""
            strQry = "Select Licenseid From ezlicense Where [key] = @key And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@key", objEmp.Key)
            objParam(0) = param

            Dim Dt As DataSet = SqlHelper.ExecuteDataset(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If Dt IsNot Nothing Then
                If Dt.Tables.Count <> 0 Then
                    If Dt.Tables(0).Rows.Count <> 0 Then
                        Throw New Exception("Already installed!")
                    End If
                End If

                'ApplicationName = Dt.Tables(0).Rows(0).Item("already installed")
            Else
                Throw New Exception("Already installed!")
            End If

            'Dim StrKeySplit As String() = AES_Decrypt(objEmp.Key, "ezofis").ToString.Split("_")

            ' If ApplicationName = StrKeySplit(1).ToString Then
            'objEmp.NoOfLicense = StrKeySplit(2)
            strQry = "INSERT INTO eZLicense([Key],CreatedOn,CreatedBy) VALUES(@Key,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(2) {}
            'param = New SqlParameter("@ApplicationId", objEmp.ApplicationId)
            'objParam(0) = param
            'param = New SqlParameter("@NoOfLicense", objEmp.NoOfLicense)
            'objParam(1) = param
            param = New SqlParameter("@Key", objEmp.Key)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZLicense(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
            'Else
            'Return Nothing
            'End If

        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZLicense)
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
            strQry = "Select *,dbo.udf_ApplicationName(ApplicationId) as ApplicationName,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZLicense Where LicenseId=@LicenseId and Isdeleted=0"
            param = New SqlParameter("@LicenseId", objRead.LicenseId)
            objParam(0) = param


            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid License.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.LicenseId = GetInteger(sqlRdr("LicenseId"))
                objRead.ApplicationId = GetInteger(sqlRdr("ApplicationId"))
                ' objRead.ApplicationName = sqlRdr("ApplicationName")
                objRead.ApplicationName = ""
                objRead.NoOfLicense = GetInteger(sqlRdr("NoOfLicense"))
                objRead.Key = sqlRdr("Key")
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
    Public Function ReadAllLicense() As System.Collections.Generic.List(Of IeZLicense)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLicense)()
        Dim objItem As IeZLicense

        Try
            Dim strQry As String = ""
            strQry = "Select LicenseId From eZLicense where Isdeleted=0 order by LicenseId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid License.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLicense(GetInteger(sqlRdr("LicenseId")))
                objItem.LicenseId = GetInteger(sqlRdr("LicenseId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedeZLicense(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLicense)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLicense)()
        Dim objItem As IeZLicense
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LicenseId From eZLicense where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by LicenseId"
            Else
                strQry = "Select LicenseId From eZLicense where Isdeleted=0 order by LicenseId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid License.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLicense(GetSmallInterger(sqlRdr("LicenseId")))
                objItem.LicenseId = GetSmallInterger(sqlRdr("LicenseId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZLicense)
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
        strQry = "Update eZLicense Set ApplicationId=@ApplicationId,NoOfLicense=@NoOfLicense,Key=@Key,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where LicenseId=@LicenseId"
        objParam = New SqlParameter(5) {}
        param = New SqlParameter("@LicenseId", objToUpdate.LicenseId)
        objParam(0) = param
        param = New SqlParameter("@ApplicationId", objToUpdate.ApplicationId)
        objParam(1) = param
        param = New SqlParameter("@NoOfLicense", objToUpdate.NoOfLicense)
        objParam(2) = param
        param = New SqlParameter("@Key", objToUpdate.Key)
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
    Public Sub Delete(objToDelete As IeZLicense)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLicense set Isdeleted=1 where LicenseId=@LicenseId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LicenseId", objToDelete.LicenseId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class