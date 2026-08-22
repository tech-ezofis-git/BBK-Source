Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateECMUserType(objEmp As eZECMUserType) As IeZECMUserType
        Dim newObject As IeZECMUserType = Nothing
        If String.IsNullOrEmpty(objEmp.ECMUserType) Then
            Return Nothing
        End If
        objEmp.ECMUserType = objEmp.ECMUserType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ECMUserTypeId From eZECMUserType Where ECMUserType = @ECMUserType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ECMUserType", objEmp.ECMUserType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ECMUserType Code already exist!")
            End If
            strQry = "INSERT INTO eZECMUserType(ECMUserType) VALUES(@ECMUserType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ECMUserType", objEmp.ECMUserType)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZECMUserType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZECMUserType)
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
            If objRead.ECMUserType Is Nothing Then

                strQry = "Select * From eZECMUserType Where ECMUserTypeId=@ECMUserType_ID and Isdeleted=0"
                param = New SqlParameter("@ECMUserType_ID", objRead.ECMUserTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZECMUserType Where ECMUserType=@ECMUserType and Isdeleted=0"
                param = New SqlParameter("@ECMUserType", objRead.ECMUserType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMUserType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ECMUserTypeId = GetInteger(sqlRdr("ECMUserTypeId"))
                objRead.ECMUserType = sqlRdr("ECMUserType").ToString()
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
    Public Function ReadAllECMUserType() As System.Collections.Generic.List(Of IeZECMUserType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMUserType)()
        Dim objItem As IeZECMUserType

        Try
            Dim strQry As String = ""
            strQry = "Select ECMUserTypeId From eZECMUserType where Isdeleted=0 order by ECMUserType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMUserType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMUserType(GetInteger(sqlRdr("ECMUserTypeId")))
                objItem.ECMUserTypeId = GetInteger(sqlRdr("ECMUserTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZECMUserType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ECMUserTypeId From eZECMUserType Where ECMUserType = @ECMUserType and ECMUserTypeId <> @ECMUserTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ECMUserType", objToUpdate.ECMUserType)
        objParam(0) = param
        param = New SqlParameter("@ECMUserTypeId", objToUpdate.ECMUserTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ECMUserType Code already exist!")
        Else
            strQry = "Update eZECMUserType Set ECMUserType=@ECMUserType where ECMUserTypeId=@ECMUserType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ECMUserType", objToUpdate.ECMUserType)
            objParam(0) = param
            param = New SqlParameter("@ECMUserType_ID", objToUpdate.ECMUserTypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMUserType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ECMUserType set Isdeleted=1 where ECMUserTypeId=@ECMUserType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ECMUserType_ID", objToDelete.ECMUserTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class