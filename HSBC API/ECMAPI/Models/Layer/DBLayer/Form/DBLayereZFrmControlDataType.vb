Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateControlDataType(objEmp As eZFrmControlDataType) As IeZFrmControlDataType
        Dim newObject As IeZFrmControlDataType = Nothing
        If String.IsNullOrEmpty(objEmp.ControlDataType) Then
            Return Nothing
        End If
        objEmp.ControlDataType = objEmp.ControlDataType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ControlDataTypeId From eZFrmControlDataType Where ControlDataType = @ControlDataType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ControlDataType", objEmp.ControlDataType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ControlDataType Code already exist!")
            End If
            strQry = "INSERT INTO eZFrmControlDataType(ControlDataType) VALUES(@ControlDataType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ControlDataType", objEmp.ControlDataType)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZFrmControlDataType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFrmControlDataType)
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
            If objRead.ControlDataType Is Nothing Then

                strQry = "Select * From eZFrmControlDataType Where ControlDataTypeId=@ControlDataType_ID and Isdeleted=0"
                param = New SqlParameter("@ControlDataType_ID", objRead.ControlDataTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZFrmControlDataType Where ControlDataType=@ControlDataType and Isdeleted=0"
                param = New SqlParameter("@ControlDataType", objRead.ControlDataType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ControlDataType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ControlDataTypeId = GetInteger(sqlRdr("ControlDataTypeId"))
                objRead.ControlDataType = sqlRdr("ControlDataType").ToString()
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
    Public Function ReadAllControlDataType() As System.Collections.Generic.List(Of IeZFrmControlDataType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFrmControlDataType)()
        Dim objItem As IeZFrmControlDataType

        Try
            Dim strQry As String = ""
            strQry = "Select ControlDataTypeId From eZFrmControlDataType where Isdeleted=0 order by ControlDataType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ControlDataType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFrmControlDataType(GetInteger(sqlRdr("ControlDataTypeId")))
                objItem.ControlDataTypeId = GetInteger(sqlRdr("ControlDataTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
  
    Public Sub Update(objToUpdate As IeZFrmControlDataType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ControlDataTypeId From eZFrmControlDataType Where ControlDataType = @ControlDataType and ControlDataTypeId <> @ControlDataTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ControlDataType", objToUpdate.ControlDataType)
        objParam(0) = param
        param = New SqlParameter("@ControlDataTypeId", objToUpdate.ControlDataTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ControlDataType Code already exist!")
        Else
            strQry = "Update eZFrmControlDataType Set ControlDataType=@ControlDataType where ControlDataTypeId=@ControlDataType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ControlDataType", objToUpdate.ControlDataType)
            objParam(0) = param
            param = New SqlParameter("@ControlDataType_ID", objToUpdate.ControlDataTypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFrmControlDataType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ControlDataType set Isdeleted=1 where ControlDataTypeId=@ControlDataType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ControlDataType_ID", objToDelete.ControlDataTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class