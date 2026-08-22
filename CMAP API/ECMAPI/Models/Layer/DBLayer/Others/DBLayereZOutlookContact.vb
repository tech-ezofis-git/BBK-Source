Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "User OutlookContacts"
    Public Function CreateOutlookContact(objEmp As eZOutlookContact) As IeZOutlookContact
        Dim newObject As IeZOutlookContact = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter

            strQry = "INSERT INTO eZOutlookContact(Name,CompanyName,Email,MobileNumber,EntryId,CreatedOn,CreatedBy) " +
                "VALUES(@Name,@CompanyName,@Email,@MobileNumber,@EntryId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@Name", objEmp.Name)
            objParam(0) = param
            param = New SqlParameter("@CompanyName", objEmp.CompanyName)
            objParam(1) = param
            param = New SqlParameter("@Email", objEmp.Email)
            objParam(2) = param
            param = New SqlParameter("@MobileNumber", objEmp.MobileNumber)
            objParam(3) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(4) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(5) = param
            param = New SqlParameter("@EntryId", objEmp.EntryId)
            objParam(6) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZOutlookContact(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZOutlookContact)
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
            'If objRead.Name Is Nothing Then

            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 " +
                "From eZOutlookContact Where OutlookContactId=@OutlookContact_ID and Isdeleted=0"
            param = New SqlParameter("@OutlookContact_ID", objRead.OutlookContactId)
            objParam(0) = param
            'Else
            'objParam = New SqlParameter(1) {}
            'strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZOutlookContact Where Name=@Name and Isdeleted=0"
            'param = New SqlParameter("@Name", objRead.Name)
            'objParam(0) = param
            'End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Name.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.OutlookContactId = GetInteger(sqlRdr("OutlookContactId"))
                objRead.Name = sqlRdr("Name").ToString()
                objRead.CompanyName = sqlRdr("CompanyName").ToString()
                objRead.EntryId = sqlRdr("EntryId").ToString()
                objRead.Email = sqlRdr("Email").ToString()
                objRead.MobileNumber = sqlRdr("MobileNumber").ToString()
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
    Public Function ReadAllOutlookContact() As System.Collections.Generic.List(Of IeZOutlookContact)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZOutlookContact)()
        Dim objItem As IeZOutlookContact

        Try
            Dim strQry As String = ""
            strQry = "Select OutlookContactId From eZOutlookContact where Isdeleted=0 order by Name"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Name.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZOutlookContact(GetInteger(sqlRdr("OutlookContactId")))
                objItem.OutlookContactId = GetInteger(sqlRdr("OutlookContactId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZOutlookContact)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZOutlookContact Set Name=@Name,EntryId=@EntryId,CompanyName=@CompanyName,Email=@Email,UpdatedOn=@UpdatedOn," +
            "UpdatedBy=@UpdatedBy,MobileNumber=@MobileNumber where OutlookContactId=@OutlookContact_ID"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@Name", objToUpdate.Name)
        objParam(0) = param
        param = New SqlParameter("@OutlookContact_ID", objToUpdate.OutlookContactId)
        objParam(1) = param
        param = New SqlParameter("@CompanyName", objToUpdate.CompanyName)
        objParam(2) = param
        param = New SqlParameter("@Email", objToUpdate.Email)
        objParam(3) = param
        param = New SqlParameter("@MobileNumber", objToUpdate.MobileNumber)
        objParam(4) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(5) = param
        param = New SqlParameter("@CreatedBy", objToUpdate.UpdatedBy)
        objParam(6) = param
        param = New SqlParameter("@EntryId", objToUpdate.EntryId)
        objParam(7) = param
        param = New SqlParameter("@Updatedby", objToUpdate.UpdatedBy)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZOutlookContact)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZOutlookContact set Isdeleted=1 where OutlookContactId=@OutlookContact_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@OutlookContact_ID", objToDelete.OutlookContactId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub

    Public Function ReadFilteredeZOutlookContact(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZOutlookContact)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZOutlookContact)()
        Dim objItem As IeZOutlookContact

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select OutlookContactId From eZOutlookContact where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Name"
            Else
                strQry = "Select OutlookContactId From eZOutlookContact where Isdeleted=0 order by Name"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZOutlookContact(GetInteger(sqlRdr("OutlookContactId")))
                objItem.OutlookContactId = GetInteger(sqlRdr("OutlookContactId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZOutlookContact(Criteria As String, Value As String, entryid As Integer) As System.Collections.Generic.List(Of IeZOutlookContact)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZOutlookContact)()
        Dim objItem As IeZOutlookContact
        Dim obj As Object

        Try
            Dim strQry As String = ""
            If entryid = 0 Then
                If Criteria <> "All" Then
                    strQry = "Select OutlookContactId From eZOutlookContact where Isdeleted=0 and " + Criteria
                    'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                    strQry = strQry & " =N'"
                    strQry = strQry & Unquote(Value)
                    strQry = strQry & "' "
                    strQry = strQry & " order by Name"
                Else
                    strQry = "Select OutlookContactId From eZOutlookContact where Isdeleted=0 order by Name"
                End If

            Else
                strQry = "Select OutlookContactId From eZOutlookContact where Isdeleted=0 and Entryid=N'" + entryid.ToString + "' and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Name"
            End If
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If

            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZOutlookContact(GetInteger(sqlRdr("OutlookContactId")))
                objItem.OutlookContactId = GetInteger(sqlRdr("OutlookContactId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZOutlookContactSP(strQry As String) As System.Collections.Generic.List(Of IeZOutlookContact)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZOutlookContact)()
        Dim objItem As IeZOutlookContact

        Try
            'Dim strQry As String = ""
            'strQry = "Select OutlookContactId From eZOutlookContact where CreatedBy=" + ECMLoginId.ToString + " and Isdeleted=0 and "
            'strQry = strQry & " Name like '%" & Unquote(Value) & "%' or CompanyName like '%" & Unquote(Value) & "%' or Email like '%" & Unquote(Value)
            'strQry = strQry & "%' or MobileNumber like '%" & Unquote(Value) & "%' union all "
            'strQry = strQry & "Select OutlookContactId From eZOutlookContact where CreatedBy=" + ECMLoginId.ToString + " and Isdeleted=0 and "
            'strQry = strQry & " Name not like '%" & Unquote(Value) & "%' or CompanyName not like '%" & Unquote(Value) & "%' or Email not like '%" & Unquote(Value)
            'strQry = strQry & "%' or MobileNumber not like '%" & Unquote(Value) & "%' "
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZOutlookContact(GetInteger(sqlRdr("OutlookContactId")))
                objItem.OutlookContactId = GetInteger(sqlRdr("OutlookContactId"))
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
