Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateeZFormDetails(objEmp As eZFormDetails) As IeZFormDetails
        Dim newObject As IeZFormDetails = Nothing
        If String.IsNullOrEmpty(objEmp.FormName) Then
            Return Nothing
        End If
        objEmp.FormName = objEmp.FormName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select FormId From eZFormDetails Where FormName = @FormName And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@FormName", objEmp.FormName)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("FormName Code already exist!")
            End If
            strQry = "INSERT INTO eZFormDetails(FormName,FormTableName,Status) VALUES(@FormName,@FormTableName,@Status);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@FormName", objEmp.FormName)
            objParam(0) = param
            param = New SqlParameter("@FormTableName", objEmp.FormTableName)
            objParam(1) = param
            param = New SqlParameter("@Status", objEmp.Status)
            objParam(2) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZFormDetails(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFormDetails)
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
            If objRead.FormName Is Nothing Then

                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFormDetails Where FormId=@FormName_ID and Isdeleted=0"
                param = New SqlParameter("@FormName_ID", objRead.FormId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFormDetails Where FormName=@FormName and Isdeleted=0"
                param = New SqlParameter("@FormName", objRead.FormName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FormName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.FormId = GetInteger(sqlRdr("FormId"))
                objRead.FormName = sqlRdr("FormName").ToString()
                objRead.FormTableName = sqlRdr("FormTableName").ToString()
                objRead.Status = sqlRdr("Status").ToString()
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
    Public Function ReadAllForm() As System.Collections.Generic.List(Of IeZFormDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormDetails)()
        Dim objItem As IeZFormDetails

        Try
            Dim strQry As String = ""
            strQry = "Select FormId From eZFormDetails where Isdeleted=0 order by FormName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FormName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormDetails(GetInteger(sqlRdr("FormId")))
                objItem.FormId = GetInteger(sqlRdr("FormId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function

    Public Function ReadFilteredForm(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFormDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormDetails)()
        Dim objItem As IeZFormDetails
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FormId From eZFormDetails where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like '%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by FormName"
            Else
                strQry = "Select FormId From eZFormDetails where Isdeleted=0 order by FormName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFormDetails.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormDetails(GetInteger(sqlRdr("FormId")))
                objItem.FormId = GetInteger(sqlRdr("FormId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedForm(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFormDetails)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormDetails)()
        Dim objItem As IeZFormDetails
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FormId From eZFormDetails where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FormName"
            Else
                strQry = "Select FormId From eZFormDetails where Isdeleted=0 order by FormName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFormDetails.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormDetails(GetInteger(sqlRdr("FormId")))
                objItem.FormId = GetInteger(sqlRdr("FormId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
   
    Public Sub Update(objToUpdate As IeZFormDetails)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select FormId From eZFormDetails Where FormName = @FormName and FormId <> @FormId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@FormName", objToUpdate.FormName)
        objParam(0) = param
        param = New SqlParameter("@FormId", objToUpdate.FormId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("FormName Code already exist!")
        Else
            strQry = "Update eZFormDetails Set FormName=@FormName,FormTableName=@FormTableName,Status=@Status where FormId=@FormName_ID"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@FormName", objToUpdate.FormName)
            objParam(0) = param
            param = New SqlParameter("@FormName_ID", objToUpdate.FormId)
            objParam(1) = param
            param = New SqlParameter("@FormTableName", objToUpdate.FormTableName)
            objParam(2) = param
            param = New SqlParameter("@Status", objToUpdate.Status)
            objParam(3) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFormDetails)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update FormName set Isdeleted=1 where FormId=@FormName_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@FormName_ID", objToDelete.FormId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class