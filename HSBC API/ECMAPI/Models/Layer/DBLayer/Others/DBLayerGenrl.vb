Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "For ECM"

    Public Function GetDatasetByQuery(ByRef strQry As String) As DataSet
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New DataSet
        Try
            Dim obj As Object = SqlHelper.ExecuteDataset(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            lstItems = obj
            Return lstItems
        Catch ex As Exception
            strQry = ex.Message
        End Try

    End Function
    Public Function GetDatasetByQuerywithParameters(ByRef strQry As String, ByVal Parameters As SqlParameter()) As DataSet
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New DataSet
        Try
            Dim obj As Object = SqlHelper.ExecuteDataset(ConnectionStr, CommandType.Text, strQry.ToString(), Parameters)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            lstItems = obj
            Return lstItems
        Catch ex As Exception
            strQry = ex.Message
        Finally

        End Try

    End Function
    'udaya
    Public Function GetDatasetByStoredProcedureName(ByVal StoredProcedureName As String, ByVal Parameters As String()) As DataSet

        Dim lstItems As New DataSet
        Dim m_commandobj As SqlClient.SqlCommand
        Dim m_sqladapter As SqlClient.SqlDataAdapter
        Try
            m_commandobj = New SqlClient.SqlCommand
            m_commandobj.Connection = New SqlClient.SqlConnection(ConnectionStr) '("Data Source=itofis;Initial Catalog=switch;User ID=sa;Password=123@abc;")
            m_commandobj.Connection.Open()
            m_commandobj.CommandType = CommandType.StoredProcedure
            m_commandobj.CommandText = StoredProcedureName
            SqlClient.SqlCommandBuilder.DeriveParameters(m_commandobj)
            For i As Integer = 0 To Parameters.Count - 1
                m_commandobj.Parameters(i + 1).Value = Parameters(i)
            Next
            m_sqladapter = New SqlClient.SqlDataAdapter(m_commandobj)
            m_sqladapter.Fill(lstItems)
            m_commandobj.Connection.Close()
            Return lstItems

        Catch ex As Exception
            Throw New Exception(ex.ToString())
        Finally
            m_commandobj.Connection.Close()
        End Try
        Return lstItems
    End Function
    'udaya
    Public Function GetDatasetbySPwithoutparameter(ByVal storedprocedurename As String) As DataSet
        Dim ds As New DataSet
        Dim m_commandobj As SqlClient.SqlCommand
        Dim m_sqladapter As SqlClient.SqlDataAdapter
        Try

            m_commandobj = New SqlClient.SqlCommand
            m_commandobj.Connection = New SqlClient.SqlConnection(ConnectionStr) '("Data Source=itofis;Initial Catalog=switch;User ID=sa;Password=123@abc;")
            m_commandobj.Connection.Open()
            m_commandobj.CommandType = CommandType.StoredProcedure
            m_commandobj.CommandText = storedprocedurename
            SqlClient.SqlCommandBuilder.DeriveParameters(m_commandobj)
            'For i As Integer = 0 To Parameters.Count - 1

            '    m_commandobj.Parameters(i + 1).Value = Parameters(i)
            'Next
            m_sqladapter = New SqlClient.SqlDataAdapter(m_commandobj)
            m_sqladapter.Fill(ds)
            m_commandobj.Connection.Close()
            Return ds
        Catch ex As Exception
            Throw New Exception(ex.ToString())
        Finally
            m_commandobj.Connection.Close()
        End Try
    End Function
    'udaya

    Public Function InsertandUpdateStoredProcedure(ByRef StoredProcedurename As String, ByVal parameters As String()) As String
        Dim m_commandobj As SqlClient.SqlCommand
        Try
            m_commandobj = New SqlClient.SqlCommand
            m_commandobj.Connection = New SqlClient.SqlConnection(ConnectionStr) '("Data Source=itofis;Initial Catalog=switch;User ID=sa;Password=123@abc;")
            m_commandobj.Connection.Open()
            m_commandobj.CommandType = CommandType.StoredProcedure
            m_commandobj.CommandText = StoredProcedurename
            SqlClient.SqlCommandBuilder.DeriveParameters(m_commandobj)
            For i As Integer = 0 To parameters.Count - 1
                m_commandobj.Parameters(i + 1).Value = parameters(i)
            Next

            Dim result As String = m_commandobj.ExecuteNonQuery()
            Return result
        Catch ex As Exception
            StoredProcedurename = ex.ToString
            Return Nothing
        Finally
            m_commandobj.Connection.Close()
        End Try

    End Function
    'udaya
    Public Function GetdatasetbySPwithoutParam(ByVal SPname As String) As DataSet
        Dim ds As New DataSet
        Dim m_commandobj As SqlClient.SqlCommand
        Dim m_sqladapter As SqlClient.SqlDataAdapter
        Try
            m_commandobj = New SqlClient.SqlCommand
            m_commandobj.Connection = New SqlClient.SqlConnection(ConnectionStr) '("Data Source=itofis;Initial Catalog=switch;User ID=sa;Password=123@abc;")
            m_commandobj.Connection.Open()
            m_commandobj.CommandType = CommandType.StoredProcedure
            m_commandobj.CommandText = SPname
            m_sqladapter = New SqlClient.SqlDataAdapter(m_commandobj)
            m_sqladapter.Fill(ds)
            m_commandobj.Connection.Close()
        Catch ex As Exception
            Throw New Exception(ex.ToString())
        Finally
            m_commandobj.Connection.Close()
        End Try
        Return ds
    End Function


    Public Function GetDatasetByStoredProcedure(ByVal StoredProcedureName As String, ByVal LookupId As Integer) As DataSet
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New DataSet
        Try
            'Dim obj As Object = SqlHelper.ExecuteDataset(ConnectionStr, CommandType.Text, strQry.ToString())
            'If obj Is Nothing Then
            '    Throw New Exception("Attempt to read Invalid eZTemplate.")
            'End If
            'lstItems = obj
            Return lstItems
        Catch ex As Exception

        End Try
        Return lstItems
    End Function
    Public Function InsertAndUpdateWithScope(ByRef strQry As String) As Integer

        strQry = strQry + ";SELECT Scope_identity();"
        Try
            Using cn As New SqlConnection(ConnectionStr)
                cn.Open()
                If cn.State <> ConnectionState.Open Then
                    cn.Open()
                End If
                Dim cmd As New SqlCommand()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = strQry.ToString()
                cmd.Connection = cn
                Return cmd.ExecuteScalar()
                cn.Close()
            End Using
        Catch ex As Exception
            strQry = ex.Message.ToString()
            Return 0
        End Try
    End Function
    Public Function InsertAndUpdate(ByRef strQry As String) As Integer
        Try
            Return SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString())
        Catch ex As Exception
            ' Throw New Exception(ex.Message.ToString())
            strQry = ex.Message.ToString()
            Return 0
        End Try
    End Function
    Public Function Delete(ByVal strQry As String) As Integer
        Try
            Return SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString())
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function GetFolderPath(ByVal NodeId As String, ByVal TemplateId As Integer, ByVal ParentNodeId As Integer) As String
        Dim Path As String = ""
        Try
            Dim Lst1 As New List(Of IeZFolders)()
            If TemplateId = 1 Or TemplateId = 2 Or TemplateId = 3 Then
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZFoldersForTempWithTemplateIdAndParentNodeId("NodeId", NodeId, TemplateId, ParentNodeId)
            Else
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZFoldersWithTemplateIdAndParentNodeId("NodeId", NodeId, TemplateId, ParentNodeId)
            End If


            If Lst1.Count <> 0 Then
                Path = Lst1(0).NodeName + "\" + Path
repeat:
                If TemplateId = 1 Or TemplateId = 2 Or TemplateId = 3 Then
                    Lst1 = DBLayer.DBLInstance.ReadSelectedeZFoldersForTempWithTemplateId("NodeId", Lst1(0).ParentNodeId, TemplateId)
                Else
                    Lst1 = DBLayer.DBLInstance.ReadSelectedeZFoldersWithTemplateId("NodeId", Lst1(0).ParentNodeId, TemplateId)
                End If


                Path = Lst1(0).NodeName + "\" + Path
                If Lst1(0).ParentNodeId <> 0 Then
                    GoTo repeat
                End If
            End If
            Return Path
        Catch ex As Exception

        End Try

        Return Path

    End Function
    Public Function GetTableNameByTemplateId(ByVal TemplateId As Integer) As String
        Try

            Dim query As String = "select dbo.udf_TableName(" + TemplateId.ToString + ") as TableName"
            Dim ds As DataSet = DBLayer.DBLInstance.GetDatasetByQuery(query)
            If ds.Tables.Count <> 0 Then
                If ds.Tables(0).Rows.Count <> 0 Then
                    Return ds.Tables(0).Rows(0).Item(0).ToString()
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        Catch ex As Exception
            Return Nothing
        End Try

    End Function
    Public Function GetValueFromeZUserDefinedByField(ByVal tabletype As Integer, ByVal TemplateId As Integer, ByVal itemid As Integer, ByVal Fieldname As String) As String
        Try
            Dim tblname As String = GetTableNameByTemplateId(TemplateId)
            If tabletype = 1 Then
                tblname = tblname.Replace("items", "stage")
            ElseIf tabletype = 3 Then
                tblname = tblname.Replace("items", "history")
            End If
            Dim ds As New DataSet
            ds = DBLayer.DBLInstance.GetDatasetByQuery("select * from " + tblname + " where itemid=" + itemid.ToString())
            If Not ds.Tables.Count = 0 Then
                If Not ds.Tables(0).Rows.Count = 0 Then
                    Return ds.Tables(0).Rows(0).Item(Fieldname).ToString()
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Function GetdistinctValueFromeZUserDefinedByField(ByVal tabletype As Integer, ByVal TemplateId As Integer, ByVal Fieldname As String) As DataSet
        Try
            Dim tblname As String = GetTableNameByTemplateId(TemplateId)
            If tabletype = 1 Then
                tblname = tblname.Replace("items", "stage")
            ElseIf tabletype = 3 Then
                tblname = tblname.Replace("items", "history")
            End If
            Dim ds As New DataSet
            ds = DBLayer.DBLInstance.GetDatasetByQuery("select DISTINCT [" + Fieldname + "] from " + tblname)
            If Not ds.Tables.Count = 0 Then
                If Not ds.Tables(0).Rows.Count = 0 Then
                    Return ds
                Else
                    Return ds
                End If
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
#End Region
End Class
