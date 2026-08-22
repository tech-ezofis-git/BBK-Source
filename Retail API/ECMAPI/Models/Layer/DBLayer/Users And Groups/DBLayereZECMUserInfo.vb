Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "User Information"
    Public Function CreateeZECMUserInfo(objEmp As eZECMUserInfo) As IeZECMUserInfo
        Dim newObject As IeZECMUserInfo = Nothing
        If objEmp.ECMLoginId = 0 Then
            Return Nothing
        End If
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select UserId From eZECMUserInfo Where ECMLoginId = @ECMLoginId And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZECMUserInfo Code already exist!")
            End If
            strQry = "INSERT INTO eZECMUserInfo(ECMLoginId,FirstName,Mobile,EmailAddress,Department,Designation,CreatedBy,CreatedOn,updatedby,isdeleted,Manager) VALUES(@ECMLoginId,@FirstName,@Mobile,@EmailAddress,@Department,@Designation,@CreatedBy,@CreatedOn,@updatedby,@isdeleted,@Manager);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(10) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@FirstName", objEmp.FirstName)
            objParam(3) = param
            param = New SqlParameter("@Mobile", objEmp.Mobile)
            objParam(4) = param
            param = New SqlParameter("@EmailAddress", objEmp.EmailAddress)
            objParam(5) = param
            param = New SqlParameter("@Department", objEmp.Department)
            objParam(6) = param
            param = New SqlParameter("@Designation", objEmp.Designation)
            objParam(7) = param
            param = New SqlParameter("@updatedby", objEmp.UpdatedBy)
            objParam(8) = param
            param = New SqlParameter("@isdeleted", objEmp.Isdeleted)
            objParam(9) = param
            param = New SqlParameter("@Manager", objEmp.Manager)
            objParam(10) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZECMUserInfo(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZECMUserInfo)
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
            If objRead.ECMLoginId Is Nothing Then
                strQry = "Select inf.*,log.FirstName as managername From eZECMUserInfo inf left join eZECMUserInfo log on inf.manager=log.ecmloginid Where " +
                    "inf.UserId=@UserId and inf.Isdeleted=0"
                param = New SqlParameter("@UserId", objRead.UserId)
                objParam(0) = param
            Else
                strQry = "Select * From eZECMUserInfo Where eZECMUserInfo=@eZECMUserInfo and Isdeleted=0"
                param = New SqlParameter("@eZECMUserInfo", objRead.ECMLoginId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZECMUserInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.UserId = GetInteger(sqlRdr("UserId"))
                objRead.ECMLoginId = sqlRdr("ECMLoginId").ToString()
                objRead.FirstName = sqlRdr("FirstName").ToString()
                objRead.Mobile = sqlRdr("Mobile").ToString()
                objRead.EmailAddress = sqlRdr("EmailAddress")
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.Department = sqlRdr("Department").ToString()
                objRead.Manager = GetInteger(sqlRdr("Manager"))
                objRead.Designation = sqlRdr("Designation").ToString()
                objRead.ManagerName = sqlRdr("ManagerName").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZECMUserInfo.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub

    Public Function ReadAlleZECMUserInfo() As System.Collections.Generic.List(Of IeZECMUserInfo)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMUserInfo)()
        Dim objItem As IeZECMUserInfo
        Try
            Dim strQry As String = ""
            strQry = "Select UserId From eZECMUserInfo where Isdeleted=0 order by ECMLoginId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZECMUserInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMUserInfo(GetSmallInterger(sqlRdr("UserId")))
                objItem.UserId = GetSmallInterger(sqlRdr("UserId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZECMUserInfo(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMUserInfo)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMUserInfo)()
        Dim objItem As IeZECMUserInfo
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select UserId From eZECMUserInfo where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select UserId From eZECMUserInfo where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZECMUserInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMUserInfo(GetSmallInterger(sqlRdr("UserId")))
                objItem.UserId = GetSmallInterger(sqlRdr("UserId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMUserInfo(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMUserInfo)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMUserInfo)()
        Dim objItem As IeZECMUserInfo
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select UserId From eZECMUserInfo where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(1000)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select UserId From eZECMUserInfo where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZECMUserInfo.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMUserInfo(GetSmallInterger(sqlRdr("UserId")))
                objItem.UserId = GetSmallInterger(sqlRdr("UserId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
  
    Public Sub Update(objToUpdate As IeZECMUserInfo)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select UserId From eZECMUserInfo Where ECMLoginId = @ECMLoginId and UserId <> @UserId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        objParam(0) = param
        param = New SqlParameter("@UserId", objToUpdate.UserId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZECMUserInfo Code already exist!")
        Else
            strQry = "Update eZECMUserInfo Set ECMLoginId=@ECMLoginId,FirstName=@FirstName,Mobile=@Mobile,EmailAddress=@EmailAddress,Designation=@Designation" +
                ",Department=@Department,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,Manager=@Manager where UserId=@UserId"
            objParam = New SqlParameter(9) {}
            param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(1) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(2) = param
            param = New SqlParameter("@FirstName", objToUpdate.FirstName)
            objParam(3) = param
            param = New SqlParameter("@Mobile", objToUpdate.Mobile)
            objParam(4) = param
            param = New SqlParameter("@EmailAddress", objToUpdate.EmailAddress)
            objParam(5) = param
            param = New SqlParameter("@UserId", objToUpdate.UserId)
            objParam(6) = param
            param = New SqlParameter("@Designation", objToUpdate.Designation)
            objParam(7) = param
            param = New SqlParameter("@Department", objToUpdate.Department)
            objParam(8) = param
            param = New SqlParameter("@Manager", objToUpdate.Manager)
            objParam(9) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMUserInfo)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMUserInfo set Isdeleted=1 where UserId=@UserId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@UserId", objToDelete.UserId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub

#End Region

End Class