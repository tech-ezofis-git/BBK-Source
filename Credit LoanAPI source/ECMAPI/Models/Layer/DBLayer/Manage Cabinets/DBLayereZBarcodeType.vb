Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateBarcodeType(objEmp As eZBarcodeType) As IeZBarcodeType
        Dim newObject As IeZBarcodeType = Nothing
        If String.IsNullOrEmpty(objEmp.BarcodeType) Then
            Return Nothing
        End If
        objEmp.BarcodeType = objEmp.BarcodeType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select BarcodeTypeId From eZBarcodeType Where BarcodeType = @BarcodeType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@BarcodeType", objEmp.BarcodeType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("BarcodeType Code already exist!")
            End If
            strQry = "INSERT INTO eZBarcodeType(BarcodeType) VALUES(@BarcodeType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@BarcodeType", objEmp.BarcodeType)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZBarcodeType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZBarcodeType)
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
            If objRead.BarcodeType Is Nothing Then

                strQry = "Select * From eZBarcodeType Where BarcodeTypeId=@BarcodeType_ID and Isdeleted=0"
                param = New SqlParameter("@BarcodeType_ID", objRead.BarcodeTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZBarcodeType Where BarcodeType=@BarcodeType and Isdeleted=0"
                param = New SqlParameter("@BarcodeType", objRead.BarcodeType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid BarcodeType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.BarcodeTypeId = GetInteger(sqlRdr("BarcodeTypeId"))
                objRead.BarcodeType = sqlRdr("BarcodeType").ToString()
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
    Public Function ReadAllBarcodeType() As System.Collections.Generic.List(Of IeZBarcodeType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBarcodeType)()
        Dim objItem As IeZBarcodeType

        Try
            Dim strQry As String = ""
            strQry = "Select BarcodeTypeId From eZBarcodeType where Isdeleted=0 order by BarcodeType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid BarcodeType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBarcodeType(GetInteger(sqlRdr("BarcodeTypeId")))
                objItem.BarcodeTypeId = GetInteger(sqlRdr("BarcodeTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZBarcodeType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select BarcodeTypeId From eZBarcodeType Where BarcodeType = @BarcodeType and BarcodeTypeId <> @BarcodeTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@BarcodeType", objToUpdate.BarcodeType)
        objParam(0) = param
        param = New SqlParameter("@BarcodeTypeId", objToUpdate.BarcodeTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("BarcodeType Code already exist!")
        Else
            strQry = "Update eZBarcodeType Set BarcodeType=@BarcodeType where BarcodeTypeId=@BarcodeType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@BarcodeType", objToUpdate.BarcodeType)
            objParam(0) = param
            param = New SqlParameter("@BarcodeType_ID", objToUpdate.BarcodeTypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZBarcodeType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update BarcodeType set Isdeleted=1 where BarcodeTypeId=@BarcodeType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@BarcodeType_ID", objToDelete.BarcodeTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class