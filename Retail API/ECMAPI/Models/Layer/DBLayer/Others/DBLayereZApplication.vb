Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateApplication(objEmp As eZApplication) As IeZApplication
        Dim newObject As IeZApplication = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            'strQry = "Select Applicationid From ezapplication Where Applicationname = @ApplicationName and AppVersion=@AppVersion  And Isdeleted=0"
            'objParam = New SqlParameter(1) {}
            'param = New SqlParameter("@ApplicationName", objEmp.ApplicationName)
            'objParam(0) = param
            'param = New SqlParameter("@AppVersion", objEmp.AppVersion)
            'objParam(1) = param
            'Dim obj1 As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            'If obj1 IsNot Nothing Then
            '    Throw New Exception("Application already exist!")
            'End If
            strQry = "INSERT INTO eZApplication(ApplicationName,AppVersion,CreatedOn,CreatedBy) VALUES(@ApplicationName,@AppVersion,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@ApplicationName", objEmp.ApplicationName)
            objParam(0) = param
            param = New SqlParameter("@AppVersion", objEmp.Appversion)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZApplication(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZApplication)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZApplication Where ApplicationId=@ApplicationId and Isdeleted=0"
            param = New SqlParameter("@ApplicationId", objRead.ApplicationId)
            objParam(0) = param


            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Application.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ApplicationId = GetInteger(sqlRdr("ApplicationId"))
                objRead.ApplicationName = sqlRdr("ApplicationName").ToString()
                objRead.AppVersion = sqlRdr("AppVersion").ToString()
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
    Public Function ReadAllApplication() As System.Collections.Generic.List(Of IeZApplication)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZApplication)()
        Dim objItem As IeZApplication

        Try
            Dim strQry As String = ""
            strQry = "Select ApplicationId From eZApplication where Isdeleted=0 order by ApplicationId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Application.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZApplication(GetInteger(sqlRdr("ApplicationId")))
                objItem.ApplicationId = GetInteger(sqlRdr("ApplicationId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedeZApplication(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZApplication)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZApplication)()
        Dim objItem As IeZApplication
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ApplicationId From eZApplication where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ApplicationId"
            Else
                strQry = "Select ApplicationId From eZApplication where Isdeleted=0 order by ApplicationId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Application.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZApplication(GetSmallInterger(sqlRdr("ApplicationId")))
                objItem.ApplicationId = GetSmallInterger(sqlRdr("ApplicationId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZApplicationbyapplicationnameandversion(Applicationname As String, version As String) As System.Collections.Generic.List(Of IeZApplication)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZApplication)()
        Dim objItem As IeZApplication
        Try
            Dim strQry As String = ""
            ' If Criteria <> "All" Then
            strQry = "Select ApplicationId From eZApplication where Isdeleted=0 and Applicationname='" + Applicationname.ToString() + "' and Appversion='" + version.ToString() + "'"
            strQry = strQry & " order by ApplicationId"
            '  Else
            ' strQry = "Select ApplicationId From eZApplication where Isdeleted=0 order by ApplicationId"
            ' End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Application.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZApplication(GetSmallInterger(sqlRdr("ApplicationId")))
                objItem.ApplicationId = GetSmallInterger(sqlRdr("ApplicationId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZApplication)
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
        strQry = "Update eZApplication Set ApplicationName=@ApplicationName,AppVersion=@AppVersion,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ApplicationId=@ApplicationId"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@ApplicationId", objToUpdate.ApplicationId)
        objParam(0) = param
        param = New SqlParameter("@ApplicationName", objToUpdate.ApplicationName)
        objParam(1) = param
        param = New SqlParameter("@AppVersion", objToUpdate.AppVersion)
        objParam(2) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(3) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(4) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZApplication)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZApplication set Isdeleted=1 where ApplicationId=@ApplicationId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ApplicationId", objToDelete.ApplicationId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class