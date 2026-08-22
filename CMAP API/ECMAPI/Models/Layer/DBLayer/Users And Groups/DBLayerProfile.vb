Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateProfile(objEmp As eZProfile) As IeZProfile
        Dim newObject As IeZProfile = Nothing
        If String.IsNullOrEmpty(objEmp.Profile) Then
            Return Nothing
        End If
        objEmp.Profile = objEmp.Profile.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ProfileId From eZProfile Where Profile = @Profile And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@Profile", objEmp.Profile)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("Profile Code already exist!")
            End If
            strQry = "INSERT INTO Profile(Profile) VALUES(@Profile);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@Profile", objEmp.Profile)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZProfile(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZProfile)
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
            If objRead.Profile Is Nothing Then

                strQry = "Select * From eZProfile Where ProfileId=@Profile_ID and Isdeleted=0"
                param = New SqlParameter("@Profile_ID", objRead.ProfileId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZProfile Where Profile=@Profile and Isdeleted=0"
                param = New SqlParameter("@Profile", objRead.Profile)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ProfileId = GetInteger(sqlRdr("ProfileId"))
                objRead.Profile = sqlRdr("Profile").ToString()
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
    Public Function ReadAllProfile() As System.Collections.Generic.List(Of IeZProfile)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZProfile)()
        Dim objItem As IeZProfile

        Try
            Dim strQry As String = ""
            strQry = "Select ProfileId From eZProfile where Isdeleted=0 order by Profile"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZProfile(GetInteger(sqlRdr("ProfileId")))
                objItem.ProfileId = GetInteger(sqlRdr("ProfileId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZProfile)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ProfileId From eZProfile Where Profile = @Profile and ProfileId <> @ProfileId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@Profile", objToUpdate.Profile)
        objParam(0) = param
        param = New SqlParameter("@ProfileId", objToUpdate.ProfileId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("Profile Code already exist!")
        Else
            strQry = "Update Profile Set Profile=@Profile where ProfileId=@Profile_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@Profile", objToUpdate.Profile)
            objParam(0) = param
            param = New SqlParameter("@Profile_ID", objToUpdate.ProfileId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZProfile)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update Profile set Isdeleted=1 where ProfileId=@Profile_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Profile_ID", objToDelete.ProfileId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class