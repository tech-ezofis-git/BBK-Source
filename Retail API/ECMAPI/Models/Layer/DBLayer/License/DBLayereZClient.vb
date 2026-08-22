Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZClient)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZClient ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.ClientId=@ClientId and ez.Isdeleted=0"
            param = New SqlParameter("@ClientId", objRead.ClientId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZClient")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ClientId = GetInteger(sqlRdr("ClientId"))
                objRead.ClientName = sqlRdr("ClientName").ToString
                objRead.Address = sqlRdr("Address").ToString
                objRead.City = sqlRdr("City").ToString
                objRead.Country = sqlRdr("Country").ToString
                objRead.ContactPerson = sqlRdr("ContactPerson").ToString
                objRead.ContactNo = sqlRdr("ContactNo").ToString
                objRead.EmailId = sqlRdr("EmailId").ToString
                objRead.ReferenceFrom = sqlRdr("Reference From").ToString
                objRead.InstalledDate = sqlRdr("Installed Date").ToString
                objRead.LastAMC = sqlRdr("Last AMC").ToString
                objRead.AMCDate = sqlRdr("AMC Date").ToString
                objRead.Logo = sqlRdr("Logo").ToString
                objRead.LicenseType = sqlRdr("LicenseType").ToString
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
    Public Function CreateeZClient(objEmp As eZClient) As eZClient
        Dim newObject As eZClient = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZClient(ClientName,Address,City,Country,ContactPerson,ContactNo,EmailId,[Reference From],[Installed Date],[Last AMC]," +
                "[AMC Date],logo,LicenseType,CreatedBy,CreatedOn) VALUES (@ClientName,@Address,@City,@Country,@ContactPerson,@ContactNo,@EmailId,@ReferenceFrom," +
                "@InstalledDate,@LastAMC,@AMCDate,@logo,@LicenseType,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(14) {}
            param = New SqlParameter("@ClientName", objEmp.ClientName)
            objParam(0) = param
            param = New SqlParameter("@Address", objEmp.Address)
            objParam(1) = param
            param = New SqlParameter("@City", objEmp.City)
            objParam(2) = param
            param = New SqlParameter("@Country", objEmp.Country)
            objParam(3) = param
            param = New SqlParameter("@ContactPerson", objEmp.ContactPerson)
            objParam(4) = param
            param = New SqlParameter("@ContactNo", objEmp.ContactNo)
            objParam(5) = param
            param = New SqlParameter("@EmailId", objEmp.EmailId)
            objParam(6) = param
            param = New SqlParameter("@ReferenceFrom", objEmp.ReferenceFrom)
            objParam(7) = param
            param = New SqlParameter("@InstalledDate", objEmp.InstalledDate)
            objParam(8) = param
            param = New SqlParameter("@LastAMC", objEmp.LastAMC)
            objParam(9) = param
            param = New SqlParameter("@AMCDate", objEmp.AMCDate)
            objParam(10) = param
            param = New SqlParameter("@logo", objEmp.Logo)
            objParam(11) = param
            param = New SqlParameter("@LicenseType", objEmp.LicenseType)
            objParam(12) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(13) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(14) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZClient(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZClient)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZClient Set ClientName=@ClientName,Address=@Address,City=@City,Country=@Country,ContactPerson=@ContactPerson,ContactNo=@ContactNo," +
            "EmailId=@EmailId,[Reference From]=@ReferenceFrom,[Installed Date]=@InstalledDate,[Last AMC]=@LastAMC,[AMC Date]=@AMCDate,logo=@logo," +
            "LicenseType=@LicenseType,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where ClientId=@ClientId"
        objParam = New SqlParameter(15) {}
        param = New SqlParameter("@ClientName", objToUpdate.ClientName)
        objParam(0) = param
        param = New SqlParameter("@Address", objToUpdate.Address)
        objParam(1) = param
        param = New SqlParameter("@City", objToUpdate.City)
        objParam(2) = param
        param = New SqlParameter("@Country", objToUpdate.Country)
        objParam(3) = param
        param = New SqlParameter("@ContactPerson", objToUpdate.ContactPerson)
        objParam(4) = param
        param = New SqlParameter("@ContactNo", objToUpdate.ContactNo)
        objParam(5) = param
        param = New SqlParameter("@EmailId", objToUpdate.EmailId)
        objParam(6) = param
        param = New SqlParameter("@ReferenceFrom", objToUpdate.ReferenceFrom)
        objParam(7) = param
        param = New SqlParameter("@InstalledDate", objToUpdate.InstalledDate)
        objParam(8) = param
        param = New SqlParameter("@LastAMC", objToUpdate.LastAMC)
        objParam(9) = param
        param = New SqlParameter("@AMCDate", objToUpdate.AMCDate)
        objParam(10) = param
        param = New SqlParameter("@logo", objToUpdate.Logo)
        objParam(11) = param
        param = New SqlParameter("@LicenseType", objToUpdate.LicenseType)
        objParam(12) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(13) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(14) = param
        param = New SqlParameter("@ClientId", objToUpdate.ClientId)
        objParam(15) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZClient)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZClient set Isdeleted=1 where ClientId=@ClientId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ClientId", objToDelete.ClientId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZClient() As System.Collections.Generic.List(Of IeZClient)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZClient)()
        Dim objItem As IeZClient
        Try
            Dim strQry As String = ""
            strQry = "Select ClientId From eZClient where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZClient")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZClient(GetInteger(sqlRdr("ClientId")))
                objItem.ClientId = GetInteger(sqlRdr("ClientId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZClient(Criteria As String, Value As String) As List(Of IeZClient)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZClient)()
        Dim objItem As IeZClient
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ClientId From eZClient where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ClientId"
            Else
                strQry = "Select ClientId From eZClient where Isdeleted=0 order by ClientId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZClient")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZClient(GetInteger(sqlRdr("ClientId")))
                objItem.ClientId = GetInteger(sqlRdr("ClientId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZClient(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZClient)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZClient)()
        Dim objItem As IeZClient
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ClientId From eZClient where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ClientId"
            Else
                strQry = "Select ClientId From eZClient where Isdeleted=0 order by ClientId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZClient")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZClient(GetInteger(sqlRdr("ClientId")))
                objItem.ClientId = GetInteger(sqlRdr("ClientId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
End Class
