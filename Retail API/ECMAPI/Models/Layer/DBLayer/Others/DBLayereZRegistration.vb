Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateeZRegistration(objEmp As eZRegistration) As IeZRegistration
        Dim newObject As IeZRegistration = Nothing

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZRegistration(CompanyName,StateName,Country,TypeOfIndustry,NoOfEmployees,EmpName,Phone,Designation,Email,Subscribe,AllowTeamToContact,CreatedOn,CreatedBy) VALUES(@CompanyName,@StateName,@Country,@TypeOfIndustry,@NoOfEmployees,@EmpName,@Phone,@Designation,@Email,@Subscribe,@AllowTeamToContact,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(12) {}
            param = New SqlParameter("@CompanyName", objEmp.CompanyName)
            objParam(0) = param
            param = New SqlParameter("@StateName", objEmp.StateName)
            objParam(1) = param
            param = New SqlParameter("@Country", objEmp.Country)
            objParam(2) = param
            param = New SqlParameter("@TypeOfIndustry", objEmp.TypeOfIndustry)
            objParam(3) = param
            param = New SqlParameter("@NoOfEmployees", objEmp.NoOfEmployees)
            objParam(4) = param
            param = New SqlParameter("@EmpName", objEmp.EmpName)
            objParam(5) = param
            param = New SqlParameter("@Phone", objEmp.Phone)
            objParam(6) = param
            param = New SqlParameter("@Designation", objEmp.Designation)
            objParam(7) = param
            param = New SqlParameter("@Email", objEmp.Email)
            objParam(8) = param
            param = New SqlParameter("@Subscribe", objEmp.Subscribe)
            objParam(9) = param
            param = New SqlParameter("@AllowTeamToContact", objEmp.AllowTeamToContact)
            objParam(10) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(11) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(12) = param
            
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZRegistration(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Read(objRead As IezRegistration)
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

            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZRegistration Where CompanyId=@CompanyId and  Isdeleted=0"
            param = New SqlParameter("@CompanyId", objRead.CompanyId)
            objParam(0) = param

            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid CompanyId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.CompanyId = GetInteger(sqlRdr("CompanyId"))
                objRead.CompanyName = sqlRdr("CompanyName").ToString()
                objRead.StateName = sqlRdr("StateName").ToString()
                objRead.Country = sqlRdr("Country").ToString()
                objRead.TypeOfIndustry = sqlRdr("TypeOfIndustry").ToString()
                objRead.NoOfEmployees = GetInteger(sqlRdr("NoOfEmployees"))
                objRead.EmpName = sqlRdr("EmpName").ToString()
                objRead.Phone = sqlRdr("Phone").ToString()
                objRead.Designation = sqlRdr("Designation").ToString()
                objRead.Email = sqlRdr("Email").ToString()
                objRead.Subscribe = GetInteger(sqlRdr("Subscribe"))
                objRead.AllowTeamToContact = GetInteger(sqlRdr("AllowTeamToContact"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString()
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

    Public Function ReadAllRegistration() As System.Collections.Generic.List(Of IezRegistration)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezRegistration)()
        Dim objItem As IezRegistration

        Try
            Dim strQry As String = ""
            strQry = "Select CompanyId From eZRegistration where Isdeleted=0 order by CompanyId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid CompanyId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZRegistration(GetInteger(sqlRdr("CompanyId")))
                objItem.CompanyId = GetInteger(sqlRdr("CompanyId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function

    Public Function ReadSelectedRegistration(Criteria As String, Value As String) As System.Collections.Generic.List(Of IezRegistration)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezRegistration)()
        Dim objItem As IezRegistration
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select CompanyId From eZRegistration where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select CompanyId From eZRegistration where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZRegistration.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZRegistration(GetSmallInterger(sqlRdr("CompanyId")))
                objItem.CompanyId = GetSmallInterger(sqlRdr("CompanyId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedRegistrationWithCondition(condition As String) As System.Collections.Generic.List(Of IezRegistration)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezRegistration)()
        Dim objItem As IezRegistration
        Try
            Dim strQry As String = ""
            strQry = "Select CompanyId From eZRegistration where Isdeleted=0 and " + condition.ToString

            strQry = strQry & " order by CreatedOn"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZRegistration.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZRegistration(GetSmallInterger(sqlRdr("CompanyId")))
                objItem.CompanyId = GetSmallInterger(sqlRdr("CompanyId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZRegistration)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZRegistration Set CompanyName=@CompanyName,StateName=@StateName,Country=@Country,TypeOfIndustry=@TypeOfIndustry,NoOfEmployees=@NoOfEmployees,EmpName=@EmpName,Phone=@Phone,Designation=@Designation,Email=@Email,Subscribe=@Subscribe,AllowTeamToContact=@AllowTeamToContact,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where CompanyId=@CompanyId"
        objParam = New SqlParameter(13) {}
        param = New SqlParameter("@CompanyId", objToUpdate.CompanyId)
        objParam(0) = param
        param = New SqlParameter("@CompanyName", objToUpdate.CompanyName)
        objParam(1) = param
        param = New SqlParameter("@StateName", objToUpdate.StateName)
        objParam(2) = param
        param = New SqlParameter("@Country", objToUpdate.Country)
        objParam(3) = param
        param = New SqlParameter("@TypeOfIndustry", objToUpdate.TypeOfIndustry)
        objParam(4) = param
        param = New SqlParameter("@NoOfEmployees", objToUpdate.NoOfEmployees)
        objParam(5) = param
        param = New SqlParameter("@EmpName", objToUpdate.EmpName)
        objParam(6) = param
        param = New SqlParameter("@Phone", objToUpdate.Phone)
        objParam(7) = param
        param = New SqlParameter("@Designation", objToUpdate.Designation)
        objParam(8) = param
        param = New SqlParameter("@Email", objToUpdate.Email)
        objParam(9) = param
        param = New SqlParameter("@Subscribe", objToUpdate.Subscribe)
        objParam(10) = param
        param = New SqlParameter("@AllowTeamToContact", objToUpdate.AllowTeamToContact)
        objParam(11) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(12) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(13) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IezRegistration)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZRegistration set Isdeleted=1 where CompanyId=@CompanyId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@CompanyId", objToDelete.CompanyId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class