Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateMailArchiveType(objEmp As eZMailArchiveType) As IeZMailArchiveType
        Dim newObject As IeZMailArchiveType = Nothing
        If String.IsNullOrEmpty(objEmp.MailArchiveType) Then
            Return Nothing
        End If
        objEmp.MailArchiveType = objEmp.MailArchiveType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select MailArchiveTypeId From eZMailArchiveType Where MailArchiveType = @MailArchiveType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@MailArchiveType", objEmp.MailArchiveType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("MailArchiveType Code already exist!")
            End If
            strQry = "INSERT INTO eZMailArchiveType(MailArchiveType) VALUES(@MailArchiveType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@MailArchiveType", objEmp.MailArchiveType)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZMailArchiveType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZMailArchiveType)
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
            If objRead.MailArchiveType Is Nothing Then

                strQry = "Select * From eZMailArchiveType Where MailArchiveTypeId=@MailArchiveType_ID and Isdeleted=0"
                param = New SqlParameter("@MailArchiveType_ID", objRead.MailArchiveTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZMailArchiveType Where MailArchiveType=@MailArchiveType and Isdeleted=0"
                param = New SqlParameter("@MailArchiveType", objRead.MailArchiveType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid MailArchiveType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.MailArchiveTypeId = GetInteger(sqlRdr("MailArchiveTypeId"))
                objRead.MailArchiveType = sqlRdr("MailArchiveType").ToString()
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
    Public Function ReadAllMailArchiveType() As System.Collections.Generic.List(Of IeZMailArchiveType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailArchiveType)()
        Dim objItem As IeZMailArchiveType

        Try
            Dim strQry As String = ""
            strQry = "Select MailArchiveTypeId From eZMailArchiveType where Isdeleted=0 order by MailArchiveType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid MailArchiveType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailArchiveType(GetInteger(sqlRdr("MailArchiveTypeId")))
                objItem.MailArchiveTypeId = GetInteger(sqlRdr("MailArchiveTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadFilteredeZMailArchiveType(Criteria As String, Value As String) As List(Of IeZMailArchiveType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailArchiveType)()
        Dim objItem As IeZMailArchiveType
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailArchiveTypeId From eZMailArchiveType where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by MailArchiveTypeId"
            Else
                strQry = "Select MailArchiveTypeId From eZMailArchiveType where Isdeleted=0 order by MailArchiveTypeId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailArchiveType")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailArchiveType(GetInteger(sqlRdr("MailArchiveTypeId")))
                objItem.MailArchiveTypeId = GetInteger(sqlRdr("MailArchiveTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMailArchiveType(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailArchiveType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailArchiveType)()
        Dim objItem As IeZMailArchiveType
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailArchiveTypeId From eZMailArchiveType where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by MailArchiveTypeId"
            Else
                strQry = "Select MailArchiveTypeId From eZMailArchiveType where Isdeleted=0 order by MailArchiveTypeId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZMailArchiveType")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailArchiveType(GetInteger(sqlRdr("MailArchiveTypeId")))
                objItem.MailArchiveTypeId = GetInteger(sqlRdr("MailArchiveTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZMailArchiveType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select MailArchiveTypeId From eZMailArchiveType Where MailArchiveType = @MailArchiveType and MailArchiveTypeId <> @MailArchiveTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@MailArchiveType", objToUpdate.MailArchiveType)
        objParam(0) = param
        param = New SqlParameter("@MailArchiveTypeId", objToUpdate.MailArchiveTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("MailArchiveType Code already exist!")
        Else
            strQry = "Update eZMailArchiveType Set MailArchiveType=@MailArchiveType where MailArchiveTypeId=@MailArchiveType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@MailArchiveType", objToUpdate.MailArchiveType)
            objParam(0) = param
            param = New SqlParameter("@MailArchiveType_ID", objToUpdate.MailArchiveTypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZMailArchiveType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update MailArchiveType set Isdeleted=1 where MailArchiveTypeId=@MailArchiveType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@MailArchiveType_ID", objToDelete.MailArchiveTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub

End Class