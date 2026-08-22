Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "DB Creation"
    Public Function ReadAllDB() As List(Of String)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New List(Of String)
        Try
            Dim strQry As String = ""
            strQry = "select name from master..sysdatabases"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid DB Name.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                lstItems.Add((sqlRdr("name").ToString()))
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
        Return lstItems
    End Function

    Public Function chkCS(ByVal CS As String) As String
        Dim Result As String
        Try
            Dim provider As String = "System.Data.SqlClient"
            Dim factory As DbProviderFactory = DbProviderFactories.GetFactory(provider)
            Using conn As DbConnection = factory.CreateConnection()
                conn.ConnectionString = CS
                conn.Open()
                Result = "Connection Successed"
            End Using
        Catch ex As Exception
            Result = "Connection Failed"
        End Try
        Return Result
    End Function
    Public Function chkCS4Oracle(ByVal CS As String) As String
        Dim Result As String
        Try

            Dim provider As String = "System.Data.OracleClient"
            Dim factory As DbProviderFactory = DbProviderFactories.GetFactory(provider)
            Using conn As DbConnection = factory.CreateConnection()
                conn.ConnectionString = CS
                conn.Open()
                Result = "Connection Successed"
            End Using
        Catch ex As Exception
            Result = "Connection Failed"
        End Try
        Return Result
    End Function
    Public Function chkCSODBBC(ByVal CS As String) As String
        Dim Result As String
        Try
            Dim provider As String = "System.Data.ODBC"
            Dim factory As DbProviderFactory = DbProviderFactories.GetFactory(provider)
            Using conn As DbConnection = factory.CreateConnection()
                conn.ConnectionString = CS
                conn.Open()
                Result = "Connection Successed"
            End Using
        Catch ex As Exception
            Result = "Connection Failed"
        End Try
        Return Result
    End Function
    Public Function CreateDB(ByVal strDBName As String) As Integer
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "select name from master..sysdatabases Where name = @strDBName"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@strDBName", strDBName)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception(strDBName + " DataBase already exist!")
            End If
            strQry = "CREATE DATABASE " + strDBName
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@strDBName", strDBName)
            objParam(0) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return 0
            Else
                Return 1
            End If
        Catch e As Exception
            Throw New Exception(e.Message)
            Return 1
        End Try

    End Function
  
    Public Function CreateDefaltTable(ByVal SqltxtPath As String) As Integer
        Dim flag As Integer = 0
        Dim connection As SqlConnection = New SqlConnection(ConnectionStr)
        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = connection
        connection.Open()
        Dim transaction As SqlTransaction = connection.BeginTransaction()
        Try

            SqltxtPath = SqltxtPath.Replace("GO", ";")
            cmd.Transaction = transaction
            cmd.CommandText = SqltxtPath
            cmd.ExecuteNonQuery()
            transaction.Commit()
        Catch ex As Exception
            flag = 1
            transaction.Rollback()
        Finally

            connection.Close()
        End Try
        Return flag
    End Function
    Public Function CreateDefaltTable1(ByVal SqltxtPath As String) As Integer
        Dim flag As Integer = 0
        Dim connection As SqlConnection = New SqlConnection(ConnectionStr)
        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = connection
        connection.Open()
        Dim transaction As SqlTransaction = connection.BeginTransaction()
        Try
            'SqltxtPath = SqltxtPath.Replace("GO", ";")
            cmd.Transaction = transaction
            cmd.CommandText = SqltxtPath
            cmd.ExecuteNonQuery()
            transaction.Commit()
        Catch ex As Exception
            flag = 1
            transaction.Rollback()
        Finally

            connection.Close()
        End Try
        Return flag
    End Function
    Public Function deleteDB(ByVal strDBName As String) As Integer
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "select name from master..sysdatabases Where name = @strDBName"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@strDBName", strDBName)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                strQry = "Drop DATABASE " + strDBName
                objParam = New SqlParameter(0) {}
                param = New SqlParameter("@strDBName", strDBName)
                objParam(0) = param
                obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
                If obj Is Nothing Then
                    Return 0
                Else
                    Return 1
                End If
            Else
                Return 1
            End If
        Catch e As Exception
            Throw New Exception(e.Message)
            Return 1
        End Try
    End Function
#End Region

End Class
