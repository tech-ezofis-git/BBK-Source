Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateFieldAlertDetail(objEmp As eZFieldAlertDetail) As IeZFieldAlertDetail
        Dim newObject As IeZFieldAlertDetail = Nothing
        If String.IsNullOrEmpty(objEmp.FieldAlertName) Then
            Return Nothing
        End If
       
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select FieldAlertDetailId From eZFieldAlertDetail Where FieldAlertName=@FieldAlertName and CreatedBy = @CreatedBy And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@FieldAlertName", objEmp.FieldAlertName)
            objParam(0) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("FieldAlertName Code already exist!")
            End If
            strQry = "INSERT INTO eZFieldAlertDetail(FieldAlertName,ToMail,CreatedOn,CreatedBy) VALUES(@FieldAlertName,@ToMail,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@FieldAlertName", objEmp.FieldAlertName)
            objParam(0) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@ToMail", objEmp.ToMail)
            objParam(3) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZFieldAlertDetail(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFieldAlertDetail)
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
            If objRead.FieldAlertName = 0 Then

                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFieldAlertDetail Where FieldAlertDetailId=@FieldAlertDetail_ID and Isdeleted=0"
                param = New SqlParameter("@FieldAlertDetail_ID", objRead.FieldAlertDetailId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFieldAlertDetail Where FieldAlertName=@FieldAlertName and Isdeleted=0"
                param = New SqlParameter("@FieldAlertName", objRead.FieldAlertName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FieldAlertName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.FieldAlertDetailId = GetInteger(sqlRdr("FieldAlertDetailId"))
                objRead.FieldAlertName = sqlRdr("FieldAlertName").ToString()
                objRead.ToMail = sqlRdr("ToMail").ToString()
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
    Public Function ReadAllFieldAlertDetail() As System.Collections.Generic.List(Of IeZFieldAlertDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlertDetail)()
        Dim objItem As IeZFieldAlertDetail

        Try
            Dim strQry As String = ""
            strQry = "Select FieldAlertDetailId From eZFieldAlertDetail where Isdeleted=0 order by FieldAlertName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FieldAlertName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlertDetail(GetInteger(sqlRdr("FieldAlertDetailId")))
                objItem.FieldAlertDetailId = GetInteger(sqlRdr("FieldAlertDetailId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedFieldAlertDetail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFieldAlertDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlertDetail)()
        Dim objItem As IeZFieldAlertDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldAlertDetailId From eZFieldAlertDetail where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FieldAlertName"
            Else
                strQry = "Select FieldAlertDetailId From eZFieldAlertDetail where Isdeleted=0 order by FieldAlertName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlertDetail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlertDetail(GetSmallInterger(sqlRdr("FieldAlertDetailId")))
                objItem.FieldAlertDetailId = GetSmallInterger(sqlRdr("FieldAlertDetailId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedFieldAlertDetailWithFieldAlertId(Criteria As String, Value As String, FieldAlertId As String) As System.Collections.Generic.List(Of IeZFieldAlertDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlertDetail)()
        Dim objItem As IeZFieldAlertDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldAlertDetailId From eZFieldAlertDetail where Isdeleted=0 and FieldAlertId=N'" + FieldAlertId + "' and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FieldAlertName"
            Else
                strQry = "Select FieldAlertDetailId From eZFieldAlertDetail where Isdeleted=0 order by FieldAlertName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlertDetail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlertDetail(GetSmallInterger(sqlRdr("FieldAlertDetailId")))
                objItem.FieldAlertDetailId = GetSmallInterger(sqlRdr("FieldAlertDetailId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZFieldAlertDetail)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select FieldAlertDetailId From eZFieldAlertDetail Where CreatedBy = @CreatedBy And FieldAlertName = @FieldAlertName and FieldAlertDetailId <> @FieldAlertDetailId and Isdeleted=0"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@FieldAlertName", objToUpdate.FieldAlertName)
        objParam(0) = param
        param = New SqlParameter("@FieldAlertDetailId", objToUpdate.FieldAlertDetailId)
        objParam(1) = param
        param = New SqlParameter("@CreatedBy", objToUpdate.CreatedBy)
        objParam(2) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("FieldAlertName Code already exist!")
        Else
            strQry = "Update eZFieldAlertDetail Set ToMail=@ToMail,FieldAlertName=@FieldAlertName,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where FieldAlertDetailId=@FieldAlertDetailId"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@FieldAlertName", objToUpdate.FieldAlertName)
            objParam(0) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(1) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(2) = param
            param = New SqlParameter("@FieldAlertDetailId", objToUpdate.FieldAlertDetailId)
            objParam(3) = param
            param = New SqlParameter("@ToMail", objToUpdate.ToMail)
            objParam(4) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFieldAlertDetail)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFieldAlertDetail set Isdeleted=1 where FieldAlertDetailId=@FieldAlertDetail_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@FieldAlertDetail_ID", objToDelete.FieldAlertDetailId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class