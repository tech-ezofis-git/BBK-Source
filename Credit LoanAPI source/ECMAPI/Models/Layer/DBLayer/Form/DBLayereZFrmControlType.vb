Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateControlType(objEmp As eZFrmControlType) As IeZFrmControlType
        Dim newObject As IeZFrmControlType = Nothing
        If String.IsNullOrEmpty(objEmp.ControlType) Then
            Return Nothing
        End If
        objEmp.ControlType = objEmp.ControlType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ControlTypeId From eZFrmControlType Where ControlType = @ControlType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ControlType", objEmp.ControlType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ControlType Code already exist!")
            End If
            strQry = "INSERT INTO eZFrmControlType(ControlType) VALUES(@ControlType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ControlType", objEmp.ControlType)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZFrmControlType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFrmControlType)
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
            If objRead.ControlType Is Nothing Then

                strQry = "Select * From eZFrmControlType Where ControlTypeId=@ControlType_ID and Isdeleted=0"
                param = New SqlParameter("@ControlType_ID", objRead.ControlTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZFrmControlType Where ControlType=@ControlType and Isdeleted=0"
                param = New SqlParameter("@ControlType", objRead.ControlType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ControlType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ControlTypeId = GetInteger(sqlRdr("ControlTypeId"))
                objRead.ControlType = sqlRdr("ControlType").ToString()
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
    Public Function ReadAllControlType() As System.Collections.Generic.List(Of IeZFrmControlType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFrmControlType)()
        Dim objItem As IeZFrmControlType

        Try
            Dim strQry As String = ""
            strQry = "Select ControlTypeId From eZFrmControlType where Isdeleted=0 order by ControlType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ControlType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFrmControlType(GetInteger(sqlRdr("ControlTypeId")))
                objItem.ControlTypeId = GetInteger(sqlRdr("ControlTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
  
    Public Sub Update(objToUpdate As IeZFrmControlType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ControlTypeId From eZFrmControlType Where ControlType = @ControlType and ControlTypeId <> @ControlTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ControlType", objToUpdate.ControlType)
        objParam(0) = param
        param = New SqlParameter("@ControlTypeId", objToUpdate.ControlTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ControlType Code already exist!")
        Else
            strQry = "Update eZFrmControlType Set ControlType=@ControlType where ControlTypeId=@ControlType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ControlType", objToUpdate.ControlType)
            objParam(0) = param
            param = New SqlParameter("@ControlType_ID", objToUpdate.ControlTypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFrmControlType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ControlType set Isdeleted=1 where ControlTypeId=@ControlType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ControlType_ID", objToDelete.ControlTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class