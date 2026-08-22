Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZRCContacts)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From ez_RC_Contacts" +
                " Where ezContactId=@ezContactId and Isdeleted=0"
            param = New SqlParameter("@ezContactId", objRead.ezContactId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Ldap User")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then

                objRead.ezContactId = GetInteger(sqlRdr("ezContactId"))

                objRead.CompanyName = sqlRdr("Company Name").ToString
                objRead.ContactName = sqlRdr("Contact Name").ToString
                objRead.LastName = sqlRdr("Last name").ToString
                objRead.Title = sqlRdr("Title").ToString

                objRead.Phone = sqlRdr("Phone").ToString
                objRead.Mobile = sqlRdr("Mobile").ToString
                objRead.AltNumber = sqlRdr("Alt Number").ToString
                objRead.Fax = sqlRdr("Fax").ToString
                objRead.Email = sqlRdr("Email").ToString
                objRead.WebPage = sqlRdr("WebPage").ToString

                objRead.Address = sqlRdr("Address").ToString
                objRead.City = sqlRdr("City").ToString
                objRead.Country = sqlRdr("Country").ToString

                objRead.SecondPhone = sqlRdr("Second Phone").ToString
                objRead.SecondMobile = sqlRdr("Second Mobile").ToString
                objRead.SecondAltNumber = sqlRdr("Second Alt Number").ToString
                objRead.SecondFax = sqlRdr("Second Fax").ToString
                objRead.SecondCity = sqlRdr("Second City").ToString
                objRead.Categories = sqlRdr("Categories").ToString

                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()

                objRead.POBox = sqlRdr("P.O.Box").ToString()
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

    Public Function CreateeZRCContacts(objEmp As eZRCContacts) As eZRCContacts
        Dim newObject As eZRCContacts = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZ_RC_Contacts([CreatedBy],[CreatedOn],[Company Name],[Contact Name],[Last Name],[Title],[Phone],[Mobile],[Alt Number],[Fax],[Email]," +
                "[Webpage],[Address],[City],[Country],[Second Phone],[Second Alt Number],[Second Fax],[Second City],[Second Mobile],[Categories],[P.O.Box]) VALUES" +
                "(@CreatedBy,@CreatedOn,@CompanyName,@ContactName,@LastName,@Title,@Phone,@Mobile,@AltNumber,@Fax,@Email,@WebPage,@Address,@City," +
                "@Country,@SecondPhone,@SecondAltNumber,@SecondFax,@SecondCity,@SecondMobile,@Categories,@POBox);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(21) {}
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(1) = param
            param = New SqlParameter("@CompanyName", objEmp.CompanyName)
            objParam(2) = param
            param = New SqlParameter("@ContactName", objEmp.ContactName)
            objParam(3) = param
            param = New SqlParameter("@LastName", objEmp.LastName)
            objParam(4) = param
            param = New SqlParameter("@Title", objEmp.Title)
            objParam(5) = param
            param = New SqlParameter("@Phone", objEmp.Phone)
            objParam(6) = param
            param = New SqlParameter("@Mobile", objEmp.Mobile)
            objParam(7) = param
            param = New SqlParameter("@AltNumber", objEmp.AltNumber)
            objParam(8) = param
            param = New SqlParameter("@Fax", objEmp.Fax)
            objParam(9) = param
            param = New SqlParameter("@Email", objEmp.Email)
            objParam(10) = param
            param = New SqlParameter("@WebPage", objEmp.WebPage)
            objParam(11) = param
            param = New SqlParameter("@Address", objEmp.Address)
            objParam(12) = param
            param = New SqlParameter("@City", objEmp.City)
            objParam(13) = param
            param = New SqlParameter("@Country", objEmp.Country)
            objParam(14) = param
            param = New SqlParameter("@SecondPhone", objEmp.SecondPhone)
            objParam(15) = param
            param = New SqlParameter("@SecondAltNumber", objEmp.SecondAltNumber)
            objParam(16) = param
            param = New SqlParameter("@SecondFax", objEmp.SecondFax)
            objParam(17) = param
            param = New SqlParameter("@SecondCity", objEmp.SecondCity)
            objParam(18) = param
            param = New SqlParameter("@SecondMobile", objEmp.SecondMobile)
            objParam(19) = param
            param = New SqlParameter("@Categories", objEmp.Categories)
            objParam(20) = param
            param = New SqlParameter("@POBox", objEmp.POBox)
            objParam(21) = param
            Dim obj As Object = Nothing
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception(SqlHelper.errstr)
                'Return Nothing
            End If
            newObject = GlobalInstance.eZRCContacts(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.ToString)
            Return Nothing
        End Try
    End Function

    Public Sub Update(objEmp As IeZRCContacts)
        If Not objEmp.IsModified Then
            Return
        End If
        If Not objEmp.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ez_RC_Contacts Set UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,[Company Name]=@CompanyName,[Contact Name]=@ContactName," +
            "[Last Name]=@LastName,[Title]=@Title,[Phone]=@Phone," +
            "[Mobile]=@Mobile,[Alt Number]=@AltNumber,[Fax]=@Fax,[Email]=@Email,[Webpage]=@WebPage,[Address]=@Address" +
            ",[City]=@City,[Country]=@Country,[Second Phone]=@SecondPhone,[Second Alt Number]=@SecondAltNumber," +
            "[Second Fax]=@SecondFax,[Second City]=@SecondCity,[Second Mobile]=@SecondMobile,[Categories]=@Categories,[P.O.Box]=@POBox where ezContactId=@ezContactId"
        objParam = New SqlParameter(22) {}
        param = New SqlParameter("@UpdatedBy", objEmp.UpdatedBy)
        objParam(0) = param
        param = New SqlParameter("@UpdatedOn", objEmp.UpdatedOn)
        objParam(1) = param
        param = New SqlParameter("@CompanyName", objEmp.CompanyName)
        objParam(2) = param
        param = New SqlParameter("@ContactName", objEmp.ContactName)
        objParam(3) = param
        param = New SqlParameter("@LastName", objEmp.LastName)
        objParam(4) = param
        param = New SqlParameter("@Title", objEmp.Title)
        objParam(5) = param
        param = New SqlParameter("@Phone", objEmp.Phone)
        objParam(6) = param
        param = New SqlParameter("@Mobile", objEmp.Mobile)
        objParam(7) = param
        param = New SqlParameter("@AltNumber", objEmp.AltNumber)
        objParam(8) = param
        param = New SqlParameter("@Fax", objEmp.Fax)
        objParam(9) = param
        param = New SqlParameter("@Email", objEmp.Email)
        objParam(10) = param
        param = New SqlParameter("@WebPage", objEmp.WebPage)
        objParam(11) = param
        param = New SqlParameter("@Address", objEmp.Address)
        objParam(12) = param
        param = New SqlParameter("@City", objEmp.City)
        objParam(13) = param
        param = New SqlParameter("@Country", objEmp.Country)
        objParam(14) = param
        param = New SqlParameter("@SecondPhone", objEmp.SecondPhone)
        objParam(15) = param
        param = New SqlParameter("@SecondAltNumber", objEmp.SecondAltNumber)
        objParam(16) = param
        param = New SqlParameter("@SecondFax", objEmp.SecondFax)
        objParam(17) = param
        param = New SqlParameter("@SecondCity", objEmp.SecondCity)
        objParam(18) = param
        param = New SqlParameter("@SecondMobile", objEmp.SecondMobile)
        objParam(19) = param
        param = New SqlParameter("@ezContactId", objEmp.ezContactId)
        objParam(20) = param
        param = New SqlParameter("@Categories", objEmp.Categories)
        objParam(21) = param
        param = New SqlParameter("@POBox", objEmp.POBox)
        objParam(22) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to : " + SqlHelper.errstr)
        End If
        objEmp.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZRCContacts)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ez_RC_Contacts set Isdeleted=1 where ezContactId=@ezContactId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ezContactId", objToDelete.ezContactId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region

    Public Function ReadAlleZRCContacts() As System.Collections.Generic.List(Of IeZRCContacts)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZRCContacts)()
        Dim objItem As IeZRCContacts
        Try
            Dim strQry As String = ""
            strQry = "Select ezContactId From ez_RC_Contacts where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid RC Conatct.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZRCContacts(GetInteger(sqlRdr("ezContactId")))
                objItem.ezContactId = GetInteger(sqlRdr("ezContactId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZRCContacts(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZRCContacts)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZRCContacts)()
        Dim objItem As IeZRCContacts
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ezContactId From eZ_RC_Contacts where Isdeleted=0 and "
                strQry = strQry & "[" + Criteria + "]"
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ezContactId"
            Else
                strQry = "Select ezContactId From eZ_RC_Contacts where Isdeleted=0 order by  ezContactId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid RC Contact.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZRCContacts(GetInteger(sqlRdr("ezContactId")))
                objItem.ezContactId = GetInteger(sqlRdr("ezContactId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZRCContacts(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZRCContacts)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZRCContacts)()
        Dim objItem As IeZRCContacts

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ezContactId From eZ_RC_Contacts where Isdeleted=0 and "
                strQry = strQry & "[" + Criteria + "]"
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by  ezContactId"
            Else
                strQry = "Select ezContactId From eZ_RC_Contacts where Isdeleted=0 order by ezContactId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid RC Contact.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZRCContacts(GetInteger(sqlRdr("ezContactId")))
                objItem.ezContactId = GetInteger(sqlRdr("ezContactId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZRCContact(strQry As String) As System.Collections.Generic.List(Of IeZRCContacts)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZRCContacts)()
        Dim objItem As IeZRCContacts
        Try
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid RC Contacts.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZRCContacts(GetInteger(sqlRdr("ezcontactid")))
                objItem.ezContactId = GetInteger(sqlRdr("ezcontactid"))
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
