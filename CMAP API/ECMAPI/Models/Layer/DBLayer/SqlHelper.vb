Imports System
Imports System.Data
Imports System.Xml
Imports System.Data.SqlClient
Imports System.Collections
Imports System.IO
Imports System.Web


Namespace DBLibrary
    Public NotInheritable Class SqlHelper
        Public Shared errstr As String = ""

#Region "private utility methods & constructors"

        'Since this class provides only static methods, make the default constructor private to prevent 
        'instances from being created with "new SqlHelper()".
        Private Sub New()
        End Sub



        ''' <summary>
        ''' This method is used to attach array of SqlParameters to a SqlCommand.
        ''' 
        ''' This method will assign a value of DbNull to any parameter with a direction of
        ''' InputOutput and a value of null.  
        ''' 
        ''' This behavior will prevent default values from being used, but
        ''' this will be the less common case than an intended pure output parameter (derived as InputOutput)
        ''' where the user provided no input value.
        ''' </summary>
        ''' <param name="command">The command to which the parameters will be added</param>
        ''' <param name="commandParameters">an array of SqlParameters tho be added to command</param>
        Private Shared Sub AttachParameters(command As SqlCommand, commandParameters As SqlParameter())
            For Each p As SqlParameter In commandParameters
                'check for derived output value with no value assigned
                If (p.Direction = ParameterDirection.InputOutput) AndAlso (p.Value Is Nothing) Then
                    p.Value = DBNull.Value
                End If

                command.Parameters.Add(p)
            Next
        End Sub

        ''' <summary>
        ''' This method assigns an array of values to an array of SqlParameters.
        ''' </summary>
        ''' <param name="commandParameters">array of SqlParameters to be assigned values</param>
        ''' <param name="parameterValues">array of objects holding the values to be assigned</param>
        Private Shared Sub AssignParameterValues(commandParameters As SqlParameter(), parameterValues As Object())
            If (commandParameters Is Nothing) OrElse (parameterValues Is Nothing) Then
                'do nothing if we get no data
                Return
            End If

            ' we must have the same number of values as we pave parameters to put them in
            If commandParameters.Length <> parameterValues.Length Then
                Throw New ArgumentException("Parameter count does not match Parameter Value count.")
            End If

            'iterate through the SqlParameters, assigning the values from the corresponding position in the 
            'value array
            Dim i As Integer = 0, j As Integer = commandParameters.Length
            While i < j
                commandParameters(i).Value = parameterValues(i)
                i += 1
            End While
        End Sub

        ''' <summary>
        ''' This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        ''' to the provided command.
        ''' </summary>
        ''' <param name="command">the SqlCommand to be prepared</param>
        ''' <param name="connection">a valid SqlConnection, on which to execute this command</param>
        ''' <param name="transaction">a valid SqlTransaction, or 'null'</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        Private Shared Sub PrepareCommand(command As SqlCommand, connection As SqlConnection, transaction As SqlTransaction, commandType As CommandType, commandText As String, commandParameters As SqlParameter())
            'if the provided connection is not open, we will open it
            If connection.State <> ConnectionState.Open Then
                connection.Open()
            End If
            'associate the connection with the command
            command.Connection = connection

            'set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText

            'if we were provided a transaction, assign it.
            If transaction IsNot Nothing Then
                command.Transaction = transaction
            End If

            'set the command type
            command.CommandType = commandType

            'attach the command parameters if they are provided
            If commandParameters IsNot Nothing Then
                AttachParameters(command, commandParameters)
            End If
           

            Return
        End Sub


#End Region

#Region "ExecuteNonQuery"

        ''' <summary>
        ''' Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
        ''' the connection string. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>an int representing the number of rows affected by the command</returns>
        Public Shared Function ExecuteNonQuery(connectionString As String, commandType As CommandType, commandText As String) As Integer
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteNonQuery(connectionString, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>an int representing the number of rows affected by the command</returns>
        Public Shared Function ExecuteNonQuery(connectionString As String, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As Integer
            'create & open a SqlConnection, and dispose of it after we are done.
            Using cn As New SqlConnection(connectionString)
                Try
                    cn.Open()
                    'call the overload that takes a connection in place of the connection string
                    Return ExecuteNonQuery(cn, commandType, commandText, commandParameters)
                Catch ex As Exception
                    errstr = ex.ToString
                Finally
                    cn.Dispose()
                End Try
            End Using
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
        ''' the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="spName">the name of the stored prcedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>an int representing the number of rows affected by the command</returns>
        Public Shared Function ExecuteNonQuery(connectionString As String, spName As String, ParamArray parameterValues As Object()) As Integer
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlConnection. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>an int representing the number of rows affected by the command</returns>
        Public Shared Function ExecuteNonQuery(connection As SqlConnection, commandType As CommandType, commandText As String) As Integer
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteNonQuery(connection, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>an int representing the number of rows affected by the command</returns>
        Public Shared Function ExecuteNonQuery(connection As SqlConnection, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As Integer
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()
            'If connection.State <> ConnectionState.Closed Then
            '    connection.Open()
            'End If
            PrepareCommand(cmd, connection, DirectCast(Nothing, SqlTransaction), commandType, commandText, commandParameters)

            'finally, execute the command.
            Dim retval As Integer = cmd.ExecuteNonQuery()

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()
            'connection.Close()

            Return retval
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
        ''' using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>an int representing the number of rows affected by the command</returns>
        Public Shared Function ExecuteNonQuery(connection As SqlConnection, spName As String, ParamArray parameterValues As Object()) As Integer
            'if we receive parameter values, we need to figure out where they go
            'If connection.State <> ConnectionState.Closed Then
            '    connection.Open()
            'End If
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName)
            End If
            'connection.Close()
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlTransaction. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>an int representing the number of rows affected by the command</returns>
        Public Shared Function ExecuteNonQuery(transaction As SqlTransaction, commandType As CommandType, commandText As String) As Integer
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteNonQuery(transaction, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns no resultset) against the specified SqlTransaction
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>an int representing the number of rows affected by the command</returns>
        Public Shared Function ExecuteNonQuery(transaction As SqlTransaction, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As Integer
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters)

            'finally, execute the command.
            Dim retval As Integer = cmd.ExecuteNonQuery()

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()


            Return retval
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified 
        ''' SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>an int representing the number of rows affected by the command</returns>
        Public Shared Function ExecuteNonQuery(transaction As SqlTransaction, spName As String, ParamArray parameterValues As Object()) As Integer
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function


#End Region

#Region "ExecuteDataSet"

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        ''' the connection string. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataset(connectionString As String, commandType As CommandType, commandText As String) As DataSet
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteDataset(connectionString, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataset(connectionString As String, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As DataSet
            'create & open a SqlConnection, and dispose of it after we are done.
            Using cn As New SqlConnection(connectionString)
                Try
                    cn.Open()

                    'call the overload that takes a connection in place of the connection string
                    Return ExecuteDataset(cn, commandType, commandText, commandParameters)
                Finally
                    cn.Dispose()
                End Try
            End Using
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        ''' the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataset(connectionString As String, spName As String, ParamArray parameterValues As Object()) As DataSet
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataset(connection As SqlConnection, commandType As CommandType, commandText As String) As DataSet
            'pass through the call providing null for the set of SqlParameters
            'If connection.State <> ConnectionState.Closed Then
            '    connection.Open()
            'End If
            Return ExecuteDataset(connection, commandType, commandText, DirectCast(Nothing, SqlParameter()))
            'connection.Close()
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataset(connection As SqlConnection, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As DataSet
            'create a command and prepare it for execution
            'udaya
            Dim cmd As New SqlCommand()
            'If connection.State <> ConnectionState.Open Then
            '    connection.Open()
            'End If
            PrepareCommand(cmd, connection, DirectCast(Nothing, SqlTransaction), commandType, commandText, commandParameters)

            'create the DataAdapter & DataSet
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet()

            'fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds)

            ' detach the SqlParameters from the command object, so they can be used again.			
            cmd.Parameters.Clear()

            'return the dataset
            'connection.Close()
            Return ds
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ''' using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataset(connection As SqlConnection, spName As String, ParamArray parameterValues As Object()) As DataSet
            'if we receive parameter values, we need to figure out where they go
            'udaya
            'If connection.State <> ConnectionState.Closed Then
            '    connection.Open()
            'End If
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteDataset(connection, CommandType.StoredProcedure, spName)
            End If
            'connection.Close()
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataset(transaction As SqlTransaction, commandType As CommandType, commandText As String) As DataSet
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteDataset(transaction, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataset(transaction As SqlTransaction, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As DataSet
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters)

            'create the DataAdapter & DataSet
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet()

            'fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds)

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()


            'return the dataset
            Return ds
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
        ''' SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataset(transaction As SqlTransaction, spName As String, ParamArray parameterValues As Object()) As DataSet
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteDataset(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function

#End Region

#Region "ExecuteDataTable"

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        ''' the connection string. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataTable(connectionString As String, commandType As CommandType, commandText As String) As DataTable
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteDataTable(connectionString, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataTable(connectionString As String, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As DataTable
            'create & open a SqlConnection, and dispose of it after we are done.
            Using cn As New SqlConnection(connectionString)
                Try
                    cn.Open()

                    'call the overload that takes a connection in place of the connection string
                    Return ExecuteDataTable(cn, commandType, commandText, commandParameters)
                Finally
                    cn.Dispose()

                End Try
            End Using
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        ''' the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataTable(connectionString As String, spName As String, ParamArray parameterValues As Object()) As DataTable
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteDataTable(connectionString, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteDataTable(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataTable(connection As SqlConnection, commandType As CommandType, commandText As String) As DataTable
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteDataTable(connection, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataTable(connection As SqlConnection, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As DataTable
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()

            PrepareCommand(cmd, connection, DirectCast(Nothing, SqlTransaction), commandType, commandText, commandParameters)

            'create the DataAdapter & DataSet
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()

            'fill the DataSet using default values for DataTable names, etc.
            da.Fill(dt)

            ' detach the SqlParameters from the command object, so they can be used again.			
            cmd.Parameters.Clear()

            'return the dataset
            connection.Close()
            Return dt
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ''' using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataTable(connection As SqlConnection, spName As String, ParamArray parameterValues As Object()) As DataTable
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteDataTable(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteDataTable(connection, CommandType.StoredProcedure, spName)
            End If
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataTable(transaction As SqlTransaction, commandType As CommandType, commandText As String) As DataTable
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteDataTable(transaction, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataTable(transaction As SqlTransaction, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As DataTable
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters)

            'create the DataAdapter & DataSet
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()

            'fill the DataSet using default values for DataTable names, etc.
            da.Fill(dt)

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()


            'return the dataset
            Return dt
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
        ''' SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteDataTable(transaction As SqlTransaction, spName As String, ParamArray parameterValues As Object()) As DataTable
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteDataTable(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteDataTable(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function

#End Region

#Region "ExecuteReader"

        ''' <summary>
        ''' this enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
        ''' we can set the appropriate CommandBehavior when calling ExecuteReader()
        ''' </summary>
        Private Enum SqlConnectionOwnership
            ''' <summary>Connection is owned and managed by SqlHelper</summary>
            Internal
            ''' <summary>Connection is owned and managed by the caller</summary>
            External
        End Enum

        ''' <summary>
        ''' Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
        ''' </summary>
        ''' <remarks>
        ''' If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        ''' 
        ''' If the caller provided the connection, we want to leave it to them to manage.
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection, on which to execute this command</param>
        ''' <param name="transaction">a valid SqlTransaction, or 'null'</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        ''' <param name="connectionOwnership">indicates whether the connection parameter was provided by the caller, or created by SqlHelper</param>
        ''' <returns>SqlDataReader containing the results of the command</returns>
        Private Shared Function ExecuteReader(connection As SqlConnection, transaction As SqlTransaction, commandType As CommandType, commandText As String, commandParameters As SqlParameter(), connectionOwnership As SqlConnectionOwnership) As SqlDataReader
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()
            'If connection.State <> ConnectionState.Closed Then
            'connection.Open()
            '  End If
            Try

           
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters)

            'create a reader
            Dim dr As SqlDataReader

            ' call ExecuteReader with the appropriate CommandBehavior
            If connectionOwnership = SqlConnectionOwnership.External Then
                ' If connection.State <> ConnectionState.Closed Then
                'connection.Open()
                'End If
                dr = cmd.ExecuteReader()
            Else
                'If connection.State <> ConnectionState.Closed Then
                'connection.Open()
                'End If
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            End If

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()
            ' connection.Close()
                Return dr
            Catch ex As Exception
            Finally
                '  connection.Close()
            End Try
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        ''' the connection string. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>a SqlDataReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteReader(connectionString As String, commandType As CommandType, commandText As String) As SqlDataReader
            'pass through the call providing null for the set of SqlParameters
            'udaya
            Dim con As New SqlConnection(connectionString)
            Try

            If con.State <> ConnectionState.Closed Then
                con.Open()
            End If
                Return ExecuteReader(connectionString, commandType, commandText, DirectCast(Nothing, SqlParameter()))
            Catch ex As Exception
            Finally
                '  con.Close()
            End Try
            'con.Close()
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>a SqlDataReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteReader(connectionString As String, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As SqlDataReader
            'create & open a SqlConnection
            Dim cn As New SqlConnection(connectionString)
            If cn.State <> ConnectionState.Closed Then
                cn.Open()
            End If

            Try
                'call the private overload that takes an internally owned connection in place of the connection string
                Return ExecuteReader(cn, Nothing, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal)

            Catch
                'if we fail to return the SqlDatReader, we need to close the connection ourselves

                Throw
            Finally
                ' cn.Close()
            End Try
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        ''' the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  SqlDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>a SqlDataReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteReader(connectionString As String, spName As String, ParamArray parameterValues As Object()) As SqlDataReader
            'if we receive parameter values, we need to figure out where they go
            'udaya
            'Dim con As New SqlConnection(connectionString)
            'If con.State <> ConnectionState.Closed Then
            '    con.Open()
            'End If
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteReader(connectionString, CommandType.StoredProcedure, spName)
            End If
            'con.Close()
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>a SqlDataReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteReader(connection As SqlConnection, commandType As CommandType, commandText As String) As SqlDataReader
            'pass through the call providing null for the set of SqlParameters
            'udaya
            'If connection.State <> ConnectionState.Closed Then
            '    connection.Open()
            'End If
            Return ExecuteReader(connection, commandType, commandText, DirectCast(Nothing, SqlParameter()))
            'connection.Close()
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>a SqlDataReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteReader(connection As SqlConnection, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As SqlDataReader
            'pass through the call to the private overload using a null transaction value and an externally owned connection
            'udaya
            'If connection.State <> ConnectionState.Closed Then
            '    connection.Open()
            'End If
            Return ExecuteReader(connection, DirectCast(Nothing, SqlTransaction), commandType, commandText, commandParameters, SqlConnectionOwnership.External)
            'connection.Close()
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ''' using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>a SqlDataReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteReader(connection As SqlConnection, spName As String, ParamArray parameterValues As Object()) As SqlDataReader
            'if we receive parameter values, we need to figure out where they go
            'If connection.State <> ConnectionState.Closed Then
            '    connection.Open()
            'End If
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName)

                AssignParameterValues(commandParameters, parameterValues)

                Return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteReader(connection, CommandType.StoredProcedure, spName)
            End If
            'connection.Close()
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>a SqlDataReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteReader(transaction As SqlTransaction, commandType As CommandType, commandText As String) As SqlDataReader
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteReader(transaction, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''   SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>a SqlDataReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteReader(transaction As SqlTransaction, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As SqlDataReader
            'pass through to private overload, indicating that the connection is owned by the caller
            Return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External)
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
        ''' SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>a SqlDataReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteReader(transaction As SqlTransaction, spName As String, ParamArray parameterValues As Object()) As SqlDataReader
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName)

                AssignParameterValues(commandParameters, parameterValues)

                Return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteReader(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function


#End Region

#Region "ExecuteScalar"

        ''' <summary>
        ''' Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
        ''' the connection string. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Shared Function ExecuteScalar(connectionString As String, commandType As CommandType, commandText As String) As Object
            'pass through the call providing null for the set of SqlParameters
            Try
                Return ExecuteScalar(connectionString, commandType, commandText, DirectCast(Nothing, SqlParameter()))
            Catch

           
            End Try

        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>ry 
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Shared Function ExecuteScalar(connectionString As String, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As Object
            'create & open a SqlConnection, and dispose of it after we are done.



            Using cn As New SqlConnection(connectionString)
                Try

                    cn.Open()

                    'call the overload that takes a connection in place of the connection string

                    Return ExecuteScalar(cn, commandType, commandText, commandParameters)

                Catch ex As Exception
                    errstr = ex.ToString
                Finally
                    cn.Close()
                End Try
            End Using

        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
        ''' the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Shared Function ExecuteScalar(connectionString As String, spName As String, ParamArray parameterValues As Object()) As Object
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName)
            End If
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlConnection. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Shared Function ExecuteScalar(connection As SqlConnection, commandType As CommandType, commandText As String) As Object
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteScalar(connection, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Shared Function ExecuteScalar(connection As SqlConnection, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As Object
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()
            PrepareCommand(cmd, connection, DirectCast(Nothing, SqlTransaction), commandType, commandText, commandParameters)

            'execute the command & return the results
            Dim retval As Object = cmd.ExecuteScalar()

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()
            connection.Close()

            Return retval

        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        ''' using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Shared Function ExecuteScalar(connection As SqlConnection, spName As String, ParamArray parameterValues As Object()) As Object
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteScalar(connection, CommandType.StoredProcedure, spName)
            End If
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlTransaction. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Shared Function ExecuteScalar(transaction As SqlTransaction, commandType As CommandType, commandText As String) As Object
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteScalar(transaction, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Shared Function ExecuteScalar(transaction As SqlTransaction, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As Object
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters)

            'execute the command & return the results
            Dim retval As Object = cmd.ExecuteScalar()

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()
            transaction.Connection.Close()
            Return retval
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified
        ''' SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        Public Shared Function ExecuteScalar(transaction As SqlTransaction, spName As String, ParamArray parameterValues As Object()) As Object
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteScalar(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function

#End Region

#Region "ExecuteXmlReader"

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        ''' <returns>an XmlReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteXmlReader(connection As SqlConnection, commandType As CommandType, commandText As String) As XmlReader
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteXmlReader(connection, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>an XmlReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteXmlReader(connection As SqlConnection, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As XmlReader
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()

            PrepareCommand(cmd, connection, DirectCast(Nothing, SqlTransaction), commandType, commandText, commandParameters)

            'create the DataAdapter & DataSet
            Dim retval As XmlReader = cmd.ExecuteXmlReader()

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()
            Return retval

        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        ''' using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="connection">a valid SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure using "FOR XML AUTO"</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>an XmlReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteXmlReader(connection As SqlConnection, spName As String, ParamArray parameterValues As Object()) As XmlReader
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName)
            End If
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        ''' <returns>an XmlReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteXmlReader(transaction As SqlTransaction, commandType As CommandType, commandText As String) As XmlReader
            'pass through the call providing null for the set of SqlParameters
            Return ExecuteXmlReader(transaction, commandType, commandText, DirectCast(Nothing, SqlParameter()))
        End Function

        ''' <summary>
        ''' Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        ''' using the provided parameters.
        ''' </summary>
        ''' <remarks>
        ''' e.g.:  
        '''  XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
        ''' <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        ''' <returns>an XmlReader containing the resultset generated by the command</returns>
        Public Shared Function ExecuteXmlReader(transaction As SqlTransaction, commandType As CommandType, commandText As String, ParamArray commandParameters As SqlParameter()) As XmlReader
            'create a command and prepare it for execution
            Dim cmd As New SqlCommand()

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters)

            'create the DataAdapter & DataSet
            Dim retval As XmlReader = cmd.ExecuteXmlReader()

            ' detach the SqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear()
            Return retval
        End Function

        ''' <summary>
        ''' Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
        ''' SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
        ''' stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        ''' </summary>
        ''' <remarks>
        ''' This method provides no access to output parameters or the stored procedure's return value parameter.
        ''' 
        ''' e.g.:  
        '''  XmlReader r = ExecuteXmlReader(trans, "GetOrders", 24, 36);
        ''' </remarks>
        ''' <param name="transaction">a valid SqlTransaction</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        ''' <returns>a dataset containing the resultset generated by the command</returns>
        Public Shared Function ExecuteXmlReader(transaction As SqlTransaction, spName As String, ParamArray parameterValues As Object()) As XmlReader
            'if we receive parameter values, we need to figure out where they go
            If (parameterValues IsNot Nothing) AndAlso (parameterValues.Length > 0) Then
                'pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                Dim commandParameters As SqlParameter() = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName)

                'assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues)

                'call the overload that takes an array of SqlParameters
                Return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters)
            Else
                'otherwise we can just call the SP without params
                Return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName)
            End If
        End Function


#End Region
    End Class

    Public NotInheritable Class SqlHelperParameterCache
#Region "private methods, variables, and constructors"

        'Since this class provides only static methods, make the default constructor private to prevent 
        'instances from being created with "new SqlHelperParameterCache()".
        Private Sub New()
        End Sub

        Private Shared paramCache As Hashtable = Hashtable.Synchronized(New Hashtable())

        ''' <summary>
        ''' resolve at run time the appropriate set of SqlParameters for a stored procedure
        ''' </summary>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
        ''' <returns></returns>
        Private Shared Function DiscoverSpParameterSet(connectionString As String, spName As String, includeReturnValueParameter As Boolean) As SqlParameter()
            Using cn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(spName, cn)
                    cn.Open()
                    cmd.CommandType = CommandType.StoredProcedure

                    SqlCommandBuilder.DeriveParameters(cmd)

                    If Not includeReturnValueParameter Then
                        cmd.Parameters.RemoveAt(0)
                    End If

                    Dim discoveredParameters As SqlParameter() = New SqlParameter(cmd.Parameters.Count - 1) {}



                    cmd.Parameters.CopyTo(discoveredParameters, 0)
                    cn.Close()

                    Return discoveredParameters
                End Using
            End Using
        End Function

        'deep copy of cached SqlParameter array
        Private Shared Function CloneParameters(originalParameters As SqlParameter()) As SqlParameter()
            Dim clonedParameters As SqlParameter() = New SqlParameter(originalParameters.Length - 1) {}

            Dim i As Integer = 0, j As Integer = originalParameters.Length
            While i < j
                clonedParameters(i) = DirectCast(DirectCast(originalParameters(i), ICloneable).Clone(), SqlParameter)
                i += 1
            End While

            Return clonedParameters
        End Function

#End Region

#Region "caching functions"

        ''' <summary>
        ''' add parameter array to the cache
        ''' </summary>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <param name="commandParameters">an array of SqlParamters to be cached</param>
        Public Shared Sub CacheParameterSet(connectionString As String, commandText As String, ParamArray commandParameters As SqlParameter())
            Dim hashKey As String = connectionString & ":" & commandText

            paramCache(hashKey) = commandParameters
        End Sub

        ''' <summary>
        ''' retrieve a parameter array from the cache
        ''' </summary>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="commandText">the stored procedure name or T-SQL command</param>
        ''' <returns>an array of SqlParamters</returns>
        Public Shared Function GetCachedParameterSet(connectionString As String, commandText As String) As SqlParameter()
            Dim hashKey As String = connectionString & ":" & commandText

            Dim cachedParameters As SqlParameter() = DirectCast(paramCache(hashKey), SqlParameter())

            If cachedParameters Is Nothing Then
                Return Nothing
            Else
                Return CloneParameters(cachedParameters)
            End If
        End Function

#End Region

#Region "Parameter Discovery Functions"

        ''' <summary>
        ''' Retrieves the set of SqlParameters appropriate for the stored procedure
        ''' </summary>
        ''' <remarks>
        ''' This method will query the database for this information, and then store it in a cache for future requests.
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <returns>an array of SqlParameters</returns>
        Public Shared Function GetSpParameterSet(connectionString As String, spName As String) As SqlParameter()
            Return GetSpParameterSet(connectionString, spName, False)
        End Function

        ''' <summary>
        ''' Retrieves the set of SqlParameters appropriate for the stored procedure
        ''' </summary>
        ''' <remarks>
        ''' This method will query the database for this information, and then store it in a cache for future requests.
        ''' </remarks>
        ''' <param name="connectionString">a valid connection string for a SqlConnection</param>
        ''' <param name="spName">the name of the stored procedure</param>
        ''' <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
        ''' <returns>an array of SqlParameters</returns>
        Public Shared Function GetSpParameterSet(connectionString As String, spName As String, includeReturnValueParameter As Boolean) As SqlParameter()
            Dim hashKey As String = connectionString & ":" & spName & (If(includeReturnValueParameter, ":include ReturnValue Parameter", ""))

            Dim cachedParameters As SqlParameter()

            cachedParameters = DirectCast(paramCache(hashKey), SqlParameter())

            If cachedParameters Is Nothing Then
                cachedParameters = DirectCast(InlineAssignHelper(paramCache(hashKey), DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter)), SqlParameter())
            End If

            Return CloneParameters(cachedParameters)
        End Function
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

#End Region

    End Class

End Namespace

