Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateLookupServerType(objEmp As eZLookupServerType) As IeZLookupServerType
        Dim newObject As IeZLookupServerType = Nothing
        If String.IsNullOrEmpty(objEmp.LookupServerType) Then
            Return Nothing
        End If
        objEmp.LookupServerType = objEmp.LookupServerType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select LookupServerTypeId From eZLookupServerType Where LookupServerType = @LookupServerType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@LookupServerType", objEmp.LookupServerType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("LookupServerType Code already exist!")
            End If
            strQry = "INSERT INTO eZLookupServerType(LookupServerType) VALUES(@LookupServerType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@LookupServerType", objEmp.LookupServerType)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZLookupServerType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZLookupServerType)
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
            If objRead.LookupServerType Is Nothing Then

                strQry = "Select * From eZLookupServerType Where LookupServerTypeId=@LookupServerType_ID and Isdeleted=0"
                param = New SqlParameter("@LookupServerType_ID", objRead.LookupServerTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZLookupServerType Where LookupServerType=@LookupServerType and Isdeleted=0"
                param = New SqlParameter("@LookupServerType", objRead.LookupServerType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid LookupServerType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.LookupServerTypeId = GetInteger(sqlRdr("LookupServerTypeId"))
                objRead.LookupServerType = sqlRdr("LookupServerType").ToString()
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
    Public Function ReadAllLookupServerType() As System.Collections.Generic.List(Of IeZLookupServerType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupServerType)()
        Dim objItem As IeZLookupServerType

        Try
            Dim strQry As String = ""
            strQry = "Select LookupServerTypeId From eZLookupServerType where Isdeleted=0 order by LookupServerType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid LookupServerType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupServerType(GetInteger(sqlRdr("LookupServerTypeId")))
                objItem.LookupServerTypeId = GetInteger(sqlRdr("LookupServerTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZLookupServerType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select LookupServerTypeId From eZLookupServerType Where LookupServerType = @LookupServerType and LookupServerTypeId <> @LookupServerTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@LookupServerType", objToUpdate.LookupServerType)
        objParam(0) = param
        param = New SqlParameter("@LookupServerTypeId", objToUpdate.LookupServerTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("LookupServerType Code already exist!")
        Else
            strQry = "Update eZLookupServerType Set LookupServerType=@LookupServerType where LookupServerTypeId=@LookupServerType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@LookupServerType", objToUpdate.LookupServerType)
            objParam(0) = param
            param = New SqlParameter("@LookupServerType_ID", objToUpdate.LookupServerTypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZLookupServerType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update LookupServerType set Isdeleted=1 where LookupServerTypeId=@LookupServerType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LookupServerType_ID", objToDelete.LookupServerTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class