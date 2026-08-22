Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateBodyHtmlType(objEmp As eZBodyHtmlType) As IeZBodyHtmlType
        Dim newObject As IeZBodyHtmlType = Nothing
        If String.IsNullOrEmpty(objEmp.BodyHtmlType) Then
            Return Nothing
        End If
        objEmp.BodyHtmlType = objEmp.BodyHtmlType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select BodyHtmlTypeId From eZBodyHtmlType Where BodyHtmlType = @BodyHtmlType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@BodyHtmlType", objEmp.BodyHtmlType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("BodyHtmlType Code already exist!")
            End If
            strQry = "INSERT INTO eZBodyHtmlType(BodyHtmlType,NoOfParameter,HtmlNamewithPath,CreatedOn,CreatedBy) VALUES(@BodyHtmlType,@NoOfParameter,@HtmlNamewithPath,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@BodyHtmlType", objEmp.BodyHtmlType)
            objParam(0) = param
            param = New SqlParameter("@NoOfParameter", objEmp.NoOfParameter)
            objParam(1) = param
            param = New SqlParameter("@HtmlNamewithPath", objEmp.HtmlNamewithPath)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZBodyHtmlType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZBodyHtmlType)
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
            If objRead.BodyHtmlType Is Nothing Then

                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZBodyHtmlType Where BodyHtmlTypeId=@BodyHtmlType_ID and Isdeleted=0"
                param = New SqlParameter("@BodyHtmlType_ID", objRead.BodyHtmlTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZBodyHtmlType Where BodyHtmlType=@BodyHtmlType and Isdeleted=0"
                param = New SqlParameter("@BodyHtmlType", objRead.BodyHtmlType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid BodyHtmlType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.BodyHtmlTypeId = GetInteger(sqlRdr("BodyHtmlTypeId"))
                objRead.BodyHtmlType = sqlRdr("BodyHtmlType").ToString()
                objRead.NoOfParameter = sqlRdr("NoOfParameter").ToString()
                objRead.HtmlNamewithPath = sqlRdr("HtmlNamewithPath").ToString()
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
    Public Function ReadAllBodyHtmlType() As System.Collections.Generic.List(Of IeZBodyHtmlType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBodyHtmlType)()
        Dim objItem As IeZBodyHtmlType

        Try
            Dim strQry As String = ""
            strQry = "Select BodyHtmlTypeId From eZBodyHtmlType where Isdeleted=0 order by BodyHtmlType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid BodyHtmlType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBodyHtmlType(GetInteger(sqlRdr("BodyHtmlTypeId")))
                objItem.BodyHtmlTypeId = GetInteger(sqlRdr("BodyHtmlTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedBodyHtmlType(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZBodyHtmlType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBodyHtmlType)()
        Dim objItem As IeZBodyHtmlType
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select BodyHtmlTypeId From eZBodyHtmlType where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select BodyHtmlTypeId From eZBodyHtmlType where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBodyHtmlType(GetSmallInterger(sqlRdr("BodyHtmlTypeId")))
                objItem.BodyHtmlTypeId = GetSmallInterger(sqlRdr("BodyHtmlTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZBodyHtmlType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select BodyHtmlTypeId From eZBodyHtmlType Where BodyHtmlType = @BodyHtmlType and BodyHtmlTypeId <> @BodyHtmlTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@BodyHtmlType", objToUpdate.BodyHtmlType)
        objParam(0) = param
        param = New SqlParameter("@BodyHtmlTypeId", objToUpdate.BodyHtmlTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("BodyHtmlType Code already exist!")
        Else
            strQry = "Update eZBodyHtmlType Set HtmlNamewithPath=@HtmlNamewithPath,NoOfParameter=@NoOfParameter,BodyHtmlType=@BodyHtmlType where BodyHtmlTypeId=@BodyHtmlType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@BodyHtmlType", objToUpdate.BodyHtmlType)
            objParam(0) = param
            param = New SqlParameter("@BodyHtmlType_ID", objToUpdate.BodyHtmlTypeId)
            objParam(1) = param
            param = New SqlParameter("@NoOfParameter", objToUpdate.NoOfParameter)
            objParam(2) = param
            param = New SqlParameter("@HtmlNamewithPath", objToUpdate.HtmlNamewithPath)
            objParam(3) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZBodyHtmlType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update BodyHtmlType set Isdeleted=1 where BodyHtmlTypeId=@BodyHtmlType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@BodyHtmlType_ID", objToDelete.BodyHtmlTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class