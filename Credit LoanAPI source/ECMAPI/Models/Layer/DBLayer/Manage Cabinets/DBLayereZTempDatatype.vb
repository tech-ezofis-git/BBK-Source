Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateTempDatatype(objEmp As eZTempDatatype) As IeZTempDatatype
        Dim newObject As IeZTempDatatype = Nothing
        If String.IsNullOrEmpty(objEmp.TempDatatype) Then
            Return Nothing
        End If
        objEmp.TempDatatype = objEmp.TempDatatype.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select TempDatatypeId From eZTempDatatype Where TempDatatype = @TempDatatype And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@TempDatatype", objEmp.TempDatatype)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("TempDatatype Code already exist!")
            End If
            strQry = "INSERT INTO eZTempDatatype(TempDatatype) VALUES(@TempDatatype);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@TempDatatype", objEmp.TempDatatype)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZTempDatatype(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZTempDatatype)
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
            If objRead.TempDatatype Is Nothing Then

                strQry = "Select * From eZTempDatatype Where TempDatatypeId=@TempDatatype_ID and Isdeleted=0"
                param = New SqlParameter("@TempDatatype_ID", objRead.TempDatatypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZTempDatatype Where TempDatatype=@TempDatatype and Isdeleted=0"
                param = New SqlParameter("@TempDatatype", objRead.TempDatatype)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid TempDatatype.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.TempDatatypeId = GetInteger(sqlRdr("TempDatatypeId"))
                objRead.TempDatatype = sqlRdr("TempDatatype").ToString()
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
    Public Function ReadAllTempDatatype() As System.Collections.Generic.List(Of IeZTempDatatype)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTempDatatype)()
        Dim objItem As IeZTempDatatype

        Try
            Dim strQry As String = ""
            strQry = "Select TempDatatypeId From eZTempDatatype where Isdeleted=0 order by TempDatatype"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid TempDatatype.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTempDatatype(GetInteger(sqlRdr("TempDatatypeId")))
                objItem.TempDatatypeId = GetInteger(sqlRdr("TempDatatypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadAllTempDatatypeExceptBarcode() As System.Collections.Generic.List(Of IeZTempDatatype)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTempDatatype)()
        Dim objItem As IeZTempDatatype

        Try
            Dim strQry As String = ""
            strQry = "Select TempDatatypeId From eZTempDatatype where Isdeleted=0 and TempDatatype<>N'Barcode' order by TempDatatype"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid TempDatatype.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTempDatatype(GetInteger(sqlRdr("TempDatatypeId")))
                objItem.TempDatatypeId = GetInteger(sqlRdr("TempDatatypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZTempDatatype)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select TempDatatypeId From eZTempDatatype Where TempDatatype = @TempDatatype and TempDatatypeId <> @TempDatatypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@TempDatatype", objToUpdate.TempDatatype)
        objParam(0) = param
        param = New SqlParameter("@TempDatatypeId", objToUpdate.TempDatatypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("TempDatatype Code already exist!")
        Else
            strQry = "Update eZTempDatatype Set TempDatatype=@TempDatatype where TempDatatypeId=@TempDatatype_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@TempDatatype", objToUpdate.TempDatatype)
            objParam(0) = param
            param = New SqlParameter("@TempDatatype_ID", objToUpdate.TempDatatypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZTempDatatype)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update TempDatatype set Isdeleted=1 where TempDatatypeId=@TempDatatype_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@TempDatatype_ID", objToDelete.TempDatatypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class