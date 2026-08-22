Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateChartType(objEmp As eZChartType) As IeZChartType
        Dim newObject As IeZChartType = Nothing
        If String.IsNullOrEmpty(objEmp.ChartType) Then
            Return Nothing
        End If
        objEmp.ChartType = objEmp.ChartType.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ChartTypeId From eZChartType Where ChartType = @ChartType And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ChartType", objEmp.ChartType)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ChartType Code already exist!")
            End If
            strQry = "INSERT INTO eZChartType(ChartType) VALUES(@ChartType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ChartType", objEmp.ChartType)
            objParam(0) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZChartType(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZChartType)
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
            If objRead.ChartType Is Nothing Then

                strQry = "Select * From eZChartType Where ChartTypeId=@ChartType_ID and Isdeleted=0"
                param = New SqlParameter("@ChartType_ID", objRead.ChartTypeId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select * From eZChartType Where ChartType=@ChartType and Isdeleted=0"
                param = New SqlParameter("@ChartType", objRead.ChartType)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ChartType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ChartTypeId = GetInteger(sqlRdr("ChartTypeId"))
                objRead.ChartType = sqlRdr("ChartType").ToString()
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
    Public Function ReadAllChartType() As System.Collections.Generic.List(Of IeZChartType)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZChartType)()
        Dim objItem As IeZChartType

        Try
            Dim strQry As String = ""
            strQry = "Select ChartTypeId From eZChartType where Isdeleted=0 order by ChartType"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ChartType.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZChartType(GetInteger(sqlRdr("ChartTypeId")))
                objItem.ChartTypeId = GetInteger(sqlRdr("ChartTypeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZChartType)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ChartTypeId From eZChartType Where ChartType = @ChartType and ChartTypeId <> @ChartTypeId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ChartType", objToUpdate.ChartType)
        objParam(0) = param
        param = New SqlParameter("@ChartTypeId", objToUpdate.ChartTypeId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ChartType Code already exist!")
        Else
            strQry = "Update eZChartType Set ChartType=@ChartType where ChartTypeId=@ChartType_ID"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ChartType", objToUpdate.ChartType)
            objParam(0) = param
            param = New SqlParameter("@ChartType_ID", objToUpdate.ChartTypeId)
            objParam(1) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZChartType)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ChartType set Isdeleted=1 where ChartTypeId=@ChartType_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ChartType_ID", objToDelete.ChartTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class