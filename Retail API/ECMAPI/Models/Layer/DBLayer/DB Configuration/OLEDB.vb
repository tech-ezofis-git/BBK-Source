Imports MySql.Data.MySqlClient
Imports System.Data.OleDb
Imports System.Data.Odbc

Partial Public Class DBLayer
    Public Function getdsfromoledb(ByRef query As String, connectionstring As String)
        Dim ds As New DataSet
        Try
            Dim con As New OleDbConnection
            Dim cmd As New OleDbCommand
            Dim DA As New OleDbDataAdapter

            con.ConnectionString = connectionstring
            cmd.CommandText = query
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            DA.SelectCommand = cmd
            DA.Fill(ds)
            DA = Nothing
            con.Close()
            con = Nothing
        Catch ex As Exception
            query = ex.Message
        End Try
        Return ds
    End Function
    Public Function getdsfrommysql(ByRef query As String, connectionstring As String)
        Dim conn As New MySqlConnection(connectionstring)
        Dim ds As New DataSet
        Dim dt As New DataTable
        Try
            conn.Open()
            Dim cmd As New MySqlCommand(query, conn)
            Dim rdr = cmd.ExecuteReader()
            ds.Tables.Add(dt)
            'ds.EnforceConstraints = False
            dt.Load(rdr)
            rdr.Close()
        Catch ex As Exception
            query = ex.Message
        Finally
            conn.Close()
        End Try
        Return ds
    End Function
    Public Function getdsfromodbc(ByRef query As String, connectionstring As String)
        Dim ds As New DataSet
        Try
            Dim con As New OdbcConnection
            Dim cmd As New OdbcCommand
            Dim DA As New OdbcDataAdapter
            con.ConnectionString = connectionstring
            cmd.CommandText = query
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            DA.SelectCommand = cmd
            DA.Fill(ds)
            DA = Nothing
            con.Close()
            con = Nothing
        Catch ex As Exception
            query = ex.Message
        End Try
        Return ds
    End Function
End Class
