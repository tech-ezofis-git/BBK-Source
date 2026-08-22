Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateLanguage(objEmp As eZLanguage) As IeZLanguage
        Dim newObject As IeZLanguage = Nothing
        If String.IsNullOrEmpty(objEmp.Language) Then
            Return Nothing
        End If
        objEmp.Language = objEmp.Language.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select LanguageId From eZLanguage Where Language = @Language And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@Language", objEmp.Language)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("Language Code already exist!")
            End If
            strQry = "INSERT INTO eZLanguage(Language) VALUES(@Language);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@Language", objEmp.Language)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZLanguage(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZLanguage)
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
            If objRead.Language Is Nothing Then

                strQry = "Select * From eZLanguage Where LanguageId=@Language_ID and Isdeleted=0"
                param = New SqlParameter("@Language_ID", objRead.LanguageId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZLanguage Where Language=@Language and Isdeleted=0"
                param = New SqlParameter("@Language", objRead.Language)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Language.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.LanguageId = GetInteger(sqlRdr("LanguageId"))
                objRead.Language = sqlRdr("Language").ToString()
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
    Public Function ReadAllLanguage() As System.Collections.Generic.List(Of IeZLanguage)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLanguage)()
        Dim objItem As IeZLanguage

        Try
            Dim strQry As String = ""
            strQry = "Select LanguageId From eZLanguage where Isdeleted=0 order by Language"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Language.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZLanguage(GetInteger(sqlRdr("LanguageId")))
                objItem.LanguageId = GetInteger(sqlRdr("LanguageId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZLanguage)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select LanguageId From eZLanguage Where Language = @Language and LanguageId <> @LanguageId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@Language", objToUpdate.Language)
        objParam(0) = param
        param = New SqlParameter("@LanguageId", objToUpdate.LanguageId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("Language Code already exist!")
        Else
            strQry = "Update eZLanguage Set Language=@Language where LanguageId=@Language_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@Language", objToUpdate.Language)
            objParam(0) = param
            param = New SqlParameter("@Language_ID", objToUpdate.LanguageId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZLanguage)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update Language set Isdeleted=1 where LanguageId=@Language_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Language_ID", objToDelete.LanguageId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class