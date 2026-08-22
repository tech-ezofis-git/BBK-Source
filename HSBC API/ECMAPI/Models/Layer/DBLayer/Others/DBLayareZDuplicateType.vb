Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateDuplicateType(objEmp As eZDuplicateType) As IeZDuplicateType
        Dim newObject As IeZDuplicateType = Nothing
        If String.IsNullOrEmpty(objEmp.DuplicateType) Then
            Return Nothing
        End If
        objEmp.DuplicateType = objEmp.DuplicateType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select DuplicateTypeId From eZDuplicateType Where DuplicateType = @DuplicateType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@DuplicateType", objEmp.DuplicateType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("DuplicateType Code already exist!")
            End If
            strQry = "INSERT INTO eZDuplicateType(DuplicateType) VALUES(@DuplicateType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@DuplicateType", objEmp.DuplicateType)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZDuplicateType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZDuplicateType)
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
            If objRead.DuplicateType Is Nothing Then

                strQry = "Select * From eZDuplicateType Where DuplicateTypeId=@DuplicateType_ID and Isdeleted=0"
                param = New SqlParameter("@DuplicateType_ID", objRead.DuplicateTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZDuplicateType Where DuplicateType=@DuplicateType and Isdeleted=0"
                param = New SqlParameter("@DuplicateType", objRead.DuplicateType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid DuplicateType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.DuplicateTypeId = GetInteger(sqlRdr("DuplicateTypeId"))
                objRead.DuplicateType = sqlRdr("DuplicateType").ToString()
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
    Public Function ReadAllDuplicateType() As System.Collections.Generic.List(Of IeZDuplicateType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDuplicateType)()
        Dim objItem As IeZDuplicateType

        Try
            Dim strQry As String = ""
            strQry = "Select DuplicateTypeId From eZDuplicateType where Isdeleted=0 order by DuplicateType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid DuplicateType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZDuplicateType(GetInteger(sqlRdr("DuplicateTypeId")))
                objItem.DuplicateTypeId = GetInteger(sqlRdr("DuplicateTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZDuplicateType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select DuplicateTypeId From eZDuplicateType Where DuplicateType = @DuplicateType and DuplicateTypeId <> @DuplicateTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@DuplicateType", objToUpdate.DuplicateType)
        objParam(0) = param
        param = New SqlParameter("@DuplicateTypeId", objToUpdate.DuplicateTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("DuplicateType Code already exist!")
        Else
            strQry = "Update eZDuplicateType Set DuplicateType=@DuplicateType where DuplicateTypeId=@DuplicateType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@DuplicateType", objToUpdate.DuplicateType)
            objParam(0) = param
            param = New SqlParameter("@DuplicateType_ID", objToUpdate.DuplicateTypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZDuplicateType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update DuplicateType set Isdeleted=1 where DuplicateTypeId=@DuplicateType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@DuplicateType_ID", objToDelete.DuplicateTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class