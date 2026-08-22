Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer

#Region "User ECMProfiles"
    Public Function CreateECMProfile(objEmp As eZECMProfile) As IeZECMProfile
        Dim newObject As IeZECMProfile = Nothing
        If String.IsNullOrEmpty(objEmp.ECMProfile) Then
            Return Nothing
        End If
        objEmp.ECMProfile = objEmp.ECMProfile.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ECMProfileId From eZECMProfile Where ECMProfile = @ECMProfile And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ECMProfile", objEmp.ECMProfile)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ECMProfile Code already exist!")
            End If
            strQry = "INSERT INTO eZECMProfile(ECMProfile,Description,Createdon,Createdby) " +
                "VALUES(@ECMProfile,@Description,@Createdon,@Createdby);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@ECMProfile", objEmp.ECMProfile)
            objParam(0) = param
            param = New SqlParameter("@Description", objEmp.Description)
            objParam(1) = param
            param = New SqlParameter("@Createdon", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@Createdby", objEmp.CreatedBy)
            objParam(3) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZECMProfile(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.ToString)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZECMProfile)
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
            If objRead.ECMProfile Is Nothing Then
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 " +
                    "From eZECMProfile Where ECMProfileId=@ECMProfile_ID and Isdeleted=0"
                param = New SqlParameter("@ECMProfile_ID", objRead.ECMProfileId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 " +
                    "From eZECMProfile Where ECMProfile=@ECMProfile and Isdeleted=0"
                param = New SqlParameter("@ECMProfile", objRead.ECMProfile)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMProfile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ECMProfileId = GetInteger(sqlRdr("ECMProfileId"))
                objRead.ECMProfile = sqlRdr("ECMProfile").ToString()
                objRead.Description = sqlRdr("Description").ToString()
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
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
    Public Function ReadAllECMProfile() As System.Collections.Generic.List(Of IeZECMProfile)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMProfile)()
        Dim objItem As IeZECMProfile
        Dim obj As Object
        Try
            Dim strQry As String = ""
            Try
                strQry = "Select ECMProfileId From eZECMProfile where Isdeleted=0 order by ECMProfile"
                obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            Catch ex As Exception
            End Try
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMProfile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMProfile(GetInteger(sqlRdr("ECMProfileId")))
                objItem.ECMProfileId = GetInteger(sqlRdr("ECMProfileId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZECMProfile)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ECMProfileId From eZECMProfile Where ECMProfile = @ECMProfile and ECMProfileId <> @ECMProfileId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ECMProfile", objToUpdate.ECMProfile)
        objParam(0) = param
        param = New SqlParameter("@ECMProfileId", objToUpdate.ECMProfileId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ECMProfile Code already exist!")
        Else
            strQry = "Update eZECMProfile Set ECMProfile=@ECMProfile,Description=@Description,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn " +
                "where ECMProfileId=@ECMProfile_ID"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@ECMProfile", objToUpdate.ECMProfile)
            objParam(0) = param
            param = New SqlParameter("@ECMProfile_ID", objToUpdate.ECMProfileId)
            objParam(1) = param
            param = New SqlParameter("@Description", objToUpdate.Description)
            objParam(2) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(3) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(4) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry, objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMProfile)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ECMProfile set Isdeleted=1 where ECMProfileId=@ECMProfile_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ECMProfile_ID", objToDelete.ECMProfileId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry, objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Function ReadFilteredeZECMProfile(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMProfile)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMProfile)()
        Dim objItem As IeZECMProfile
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMProfileId From eZECMProfile where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMProfile"
            Else
                strQry = "Select ECMProfileId From eZECMProfile where Isdeleted=0 order by ECMProfile"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMProfile(GetInteger(sqlRdr("ECMProfileId")))
                objItem.ECMProfileId = GetInteger(sqlRdr("ECMProfileId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMProfile(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMProfile)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMProfile)()
        Dim objItem As IeZECMProfile
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMProfileId From eZECMProfile where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMProfile"
            Else
                strQry = "Select ECMProfileId From eZECMProfile where Isdeleted=0 order by ECMProfile"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMProfile(GetInteger(sqlRdr("ECMProfileId")))
                objItem.ECMProfileId = GetInteger(sqlRdr("ECMProfileId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
#End Region
End Class
