Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateChart(objEmp As eZChart) As IeZChart
        Dim newObject As IeZChart = Nothing
        If String.IsNullOrEmpty(objEmp.Chart) Then
            Return Nothing
        End If
        objEmp.Chart = objEmp.Chart.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ChartId From eZChart Where Chart = @Chart And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@Chart", objEmp.Chart)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("Chart Code already exist!")
            End If
            strQry = "INSERT INTO eZChart(Chart,ChartTypeId,CreatedOn,CreatedBy) VALUES(@Chart,@ChartTypeId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@Chart", objEmp.Chart)
            objParam(0) = param
            param = New SqlParameter("@ChartTypeId", objEmp.ChartTypeId)
            
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZChart(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZChart)
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
            If objRead.Chart Is Nothing Then

                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZChart Where ChartId=@Chart_ID and Isdeleted=0"
                param = New SqlParameter("@Chart_ID", objRead.ChartId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZChart Where Chart=@Chart and Isdeleted=0"
                param = New SqlParameter("@Chart", objRead.Chart)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Chart.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ChartId = GetInteger(sqlRdr("ChartId"))
                objRead.Chart = sqlRdr("Chart").ToString()
                objRead.ChartTypeId = sqlRdr("ChartTypeId").ToString()

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
    Public Function ReadAllChart() As System.Collections.Generic.List(Of IeZChart)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZChart)()
        Dim objItem As IeZChart

        Try
            Dim strQry As String = ""
            strQry = "Select ChartId From eZChart where Isdeleted=0 order by Chart"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Chart.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZChart(GetInteger(sqlRdr("ChartId")))
                objItem.ChartId = GetInteger(sqlRdr("ChartId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedChart(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZChart)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZChart)()
        Dim objItem As IeZChart
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ChartId From eZChart where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select ChartId From eZChart where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZChart(GetSmallInterger(sqlRdr("ChartId")))
                objItem.ChartId = GetSmallInterger(sqlRdr("ChartId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZChart)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ChartId From eZChart Where Chart = @Chart and ChartId <> @ChartId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@Chart", objToUpdate.Chart)
        objParam(0) = param
        param = New SqlParameter("@ChartId", objToUpdate.ChartId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("Chart Code already exist!")
        Else
            strQry = "Update eZChart Set ChartTypeId=@ChartTypeId,Chart=@Chart where ChartId=@Chart_ID"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@Chart", objToUpdate.Chart)
            objParam(0) = param
            param = New SqlParameter("@Chart_ID", objToUpdate.ChartId)
            objParam(1) = param
            param = New SqlParameter("@ChartTypeId", objToUpdate.ChartTypeId)
            objParam(2) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZChart)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update Chart set Isdeleted=1 where ChartId=@Chart_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Chart_ID", objToDelete.ChartId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class