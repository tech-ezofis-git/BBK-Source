Imports System.Text
Imports System.Data.SqlClient
Imports System.Data.OracleClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "For Oracle "

    Public Function GetDatasetByQueryInOracle(ByRef strQry As String, ByVal strConnString As String) As DataSet
        Dim ds As New DataSet
        Try
            Dim objConn As New OracleConnection
            Dim objCmd As New OracleCommand
            Dim dtAdapter As New OracleDataAdapter
            objConn.ConnectionString = strConnString
            With objCmd
                .Connection = objConn
                .CommandText = strQry
                .CommandType = CommandType.Text
            End With
            dtAdapter.SelectCommand = objCmd
            dtAdapter.Fill(ds)
            dtAdapter = Nothing
            objConn.Close()
            objConn = Nothing
            Return ds
        Catch ex As Exception
            strQry = ex.Message
            Return Nothing
        End Try
    End Function
    Public Function GetDatasetByStoredProcedureInOracle(ByVal StoredProcedureName As String, ByVal LookupId As Integer, ByVal strConnString As String) As DataSet
        'Dim connection As New OracleConnection(strConnString)
        Dim ds As New DataSet()
        Try
            Dim connetionString As String
            Dim connection As SqlConnection
            Dim adapter As SqlDataAdapter
            Dim command As New SqlCommand
            Dim param As SqlParameter
            Dim i As Integer
            connetionString = "Data Source=servername;Initial Catalog=PUBS;User ID=sa;Password=yourpassword"
            connection = New SqlConnection(connetionString)
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "SPCOUNTRY"
            param = New SqlParameter("@COUNTRY", "Germany")
            param.Direction = ParameterDirection.Input
            param.DbType = DbType.String
            command.Parameters.Add(param)
            adapter = New SqlDataAdapter(command)
            adapter.Fill(ds)
            For i = 0 To ds.Tables(0).Rows.Count - 1
                MsgBox(ds.Tables(0).Rows(i).Item(0))
            Next
            connection.Close()
        Catch ex As Exception
        End Try
        'Try
        '    Dim command As New OracleCommand()
        '    connection.Open()
        '    command.Connection = connection
        '    command.CommandText = StoredProcedureName
        '    command.CommandType = CommandType.StoredProcedure
        '    Dim Lst1 As New List(Of IeZLookupSPparameters)()
        '    Lst1 = DBLayer.DBLInstance.ReadSelectedeZLookupSPparameters("LookupId", LookupId.ToString())
        '    For i As Integer = 0 To Lst1.Count - 1
        '        If Lst1(i).IsOutputParameterDirection Then
        '            command.Parameters.Add(New OracleParameter("INVOICE_CUR", OracleType.Cursor)).Direction = ParameterDirection.Output
        '        Else

        '        End If
        '    Next
        '    command.Parameters.Add(New OracleParameter("BARCODEID", OracleType.VarChar)).Value = BARCODEID
        '    command.Parameters.Add(New OracleParameter("INVOICE_CUR", OracleType.Cursor)).Direction = ParameterDirection.Output
        '    Dim param3 As New OracleParameter()
        '    param3.ParameterName = "RESULT"
        '    param3.OracleType = OracleType.Number
        '    param3.Direction = ParameterDirection.Output
        '    command.Parameters.Add(param3)
        '    Dim param4 As New OracleParameter()
        '    param4.ParameterName = "EXCEPSTR"
        '    param4.OracleType = OracleType.VarChar
        '    param4.Size = 4000
        '    param4.Direction = ParameterDirection.Output
        '    command.Parameters.Add(param4)
        '    Dim da As New OracleDataAdapter(command)
        '    da.Fill(ds)
        '    RESULT = Convert.ToByte(param3.Value)
        '    EXCEPSTR = param4.Value.ToString()
        'Catch ex As Exception
        'Finally
        '    connection.Close()
        'End Try
        Return ds
    End Function
    Public Function InsertAndUpdateDeleteInOracle(ByVal strQry As String, ByVal strConnString As String) As Integer
        Try
            Dim objConn As New OracleConnection
            Dim objCmd As New OracleCommand
            Dim dtAdapter As New OracleDataAdapter
            objConn.ConnectionString = strConnString
            With objCmd
                .Connection = objConn
                .CommandText = strQry
                .CommandType = CommandType.Text
            End With
           objCmd.ExecuteNonQuery()  
            Return 1
        Catch ex As Exception
            Return 0
        End Try
    End Function
#End Region

End Class
