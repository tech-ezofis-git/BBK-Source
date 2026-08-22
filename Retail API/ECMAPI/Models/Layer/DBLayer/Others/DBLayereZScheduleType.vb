Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateScheduleType(objEmp As eZScheduleType) As IeZScheduleType
        Dim newObject As IeZScheduleType = Nothing
        If String.IsNullOrEmpty(objEmp.ScheduleType) Then
            Return Nothing
        End If
        objEmp.ScheduleType = objEmp.ScheduleType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ScheduleTypeId From eZScheduleType Where ScheduleType = @ScheduleType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ScheduleType", objEmp.ScheduleType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ScheduleType Code already exist!")
            End If
            strQry = "INSERT INTO eZScheduleType(ScheduleType) VALUES(@ScheduleType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ScheduleType", objEmp.ScheduleType)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZScheduleType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZScheduleType)
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
            If objRead.ScheduleType Is Nothing Then

                strQry = "Select * From eZScheduleType Where ScheduleTypeId=@ScheduleType_ID and Isdeleted=0"
                param = New SqlParameter("@ScheduleType_ID", objRead.ScheduleTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZScheduleType Where ScheduleType=@ScheduleType and Isdeleted=0"
                param = New SqlParameter("@ScheduleType", objRead.ScheduleType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ScheduleType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ScheduleTypeId = GetInteger(sqlRdr("ScheduleTypeId"))
                objRead.ScheduleType = sqlRdr("ScheduleType").ToString()
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
    Public Function ReadAllScheduleType() As System.Collections.Generic.List(Of IeZScheduleType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScheduleType)()
        Dim objItem As IeZScheduleType

        Try
            Dim strQry As String = ""
            strQry = "Select ScheduleTypeId From eZScheduleType where Isdeleted=0 order by ScheduleType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ScheduleType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScheduleType(GetInteger(sqlRdr("ScheduleTypeId")))
                objItem.ScheduleTypeId = GetInteger(sqlRdr("ScheduleTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZScheduleType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ScheduleTypeId From eZScheduleType Where ScheduleType = @ScheduleType and ScheduleTypeId <> @ScheduleTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ScheduleType", objToUpdate.ScheduleType)
        objParam(0) = param
        param = New SqlParameter("@ScheduleTypeId", objToUpdate.ScheduleTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ScheduleType Code already exist!")
        Else
            strQry = "Update eZScheduleType Set ScheduleType=@ScheduleType where ScheduleTypeId=@ScheduleType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ScheduleType", objToUpdate.ScheduleType)
            objParam(0) = param
            param = New SqlParameter("@ScheduleType_ID", objToUpdate.ScheduleTypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZScheduleType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ScheduleType set Isdeleted=1 where ScheduleTypeId=@ScheduleType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ScheduleType_ID", objToDelete.ScheduleTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class