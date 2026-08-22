Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateLookupType(objEmp As eZLookupType) As IeZLookupType
        Dim newObject As IeZLookupType = Nothing
        If String.IsNullOrEmpty(objEmp.LookupType) Then
            Return Nothing
        End If
        objEmp.LookupType = objEmp.LookupType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select LookupTypeId From eZLookupType Where LookupType = @LookupType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@LookupType", objEmp.LookupType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("LookupType Code already exist!")
            End If
            strQry = "INSERT INTO eZLookupType(LookupType) VALUES(@LookupType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@LookupType", objEmp.LookupType)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZLookupType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZLookupType)
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
            If objRead.LookupType Is Nothing Then

                strQry = "Select * From eZLookupType Where LookupTypeId=@LookupType_ID and Isdeleted=0"
                param = New SqlParameter("@LookupType_ID", objRead.LookupTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZLookupType Where LookupType=@LookupType and Isdeleted=0"
                param = New SqlParameter("@LookupType", objRead.LookupType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid LookupType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.LookupTypeId = GetInteger(sqlRdr("LookupTypeId"))
                objRead.LookupType = sqlRdr("LookupType").ToString()
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
    Public Function ReadAllLookupType() As System.Collections.Generic.List(Of IeZLookupType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLookupType)()
        Dim objItem As IeZLookupType

        Try
            Dim strQry As String = ""
            strQry = "Select LookupTypeId From eZLookupType where Isdeleted=0 order by LookupType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid LookupType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLookupType(GetInteger(sqlRdr("LookupTypeId")))
                objItem.LookupTypeId = GetInteger(sqlRdr("LookupTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZLookupType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select LookupTypeId From eZLookupType Where LookupType = @LookupType and LookupTypeId <> @LookupTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@LookupType", objToUpdate.LookupType)
        objParam(0) = param
        param = New SqlParameter("@LookupTypeId", objToUpdate.LookupTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("LookupType Code already exist!")
        Else
            strQry = "Update eZLookupType Set LookupType=@LookupType where LookupTypeId=@LookupType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@LookupType", objToUpdate.LookupType)
            objParam(0) = param
            param = New SqlParameter("@LookupType_ID", objToUpdate.LookupTypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZLookupType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update LookupType set Isdeleted=1 where LookupTypeId=@LookupType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@LookupType_ID", objToDelete.LookupTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class