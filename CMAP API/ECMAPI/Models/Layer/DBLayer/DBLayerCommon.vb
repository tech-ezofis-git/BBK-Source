Imports System
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Security.Cryptography
Imports ECMAPI.DBLibrary


Partial Public Class DBLayer
    Public Shared ReadOnly DBLInstance As New DBLayer()
    Private Shared _globalInstance As IInstance = Nothing

#Region "private variables"

    Private Shared _connectionStr As String = Nothing

#End Region
    ' Tells the database how to instantiate classes.
    Public Property GlobalInstance() As IInstance
        Get
            Return DBLayer._globalInstance
        End Get
        Set(value As IInstance)
            DBLayer._globalInstance = value
        End Set
    End Property
    ' Tells the database how to instantiate classes.
    Public Property ConnectionStr() As String
        Get
            Return DBLayer._connectionStr
        End Get
        Set(value As String)
            DBLayer._connectionStr = value
        End Set
    End Property
    ' Eliminate special ecape character.
    Private Function Unquote(strVal As String) As String
        If strVal Is Nothing Then
            Return ""
        End If
        Return strVal.Replace("'", "''")
    End Function
    ' Get Integer value from object.
    Private Function GetBoolean(objVal As Object) As Boolean
        Dim intValue As Boolean = 0
        If objVal Is Nothing Then
            Return 0
        End If
        If TypeOf objVal Is DBNull Then
            Return 0
        End If
        If TypeOf objVal Is Boolean Then
            Return CBool(objVal)
        End If
        Boolean.TryParse(objVal.ToString(), intValue)
        Return intValue
    End Function
    ' Get boolean value from object.
    Private Function GetInteger(objVal As Object) As Integer
        Dim intValue As Integer = 0
        If objVal Is Nothing Then
            Return 0
        End If
        If TypeOf objVal Is DBNull Then
            Return 0
        End If
        If TypeOf objVal Is Int32 Then
            Return CInt(objVal)
        End If
        Integer.TryParse(objVal.ToString(), intValue)
        Return intValue
    End Function
    ' Get Small Integer value from object.
    Private Function GetSmallInterger(objVal As Object) As Short
        Dim smallintValue As Short = 0
        If objVal Is Nothing Then
            Return 0
        End If
        If TypeOf objVal Is DBNull Then
            Return 0
        End If
        If TypeOf objVal Is Int16 Then
            Return CShort(objVal)
        End If
        Short.TryParse(objVal.ToString(), smallintValue)
        Return smallintValue
    End Function
    ' Get long value from object.
    Private Function GetLong(objVal As Object) As Long
        Dim longValue As Long = 0
        If objVal Is Nothing Then
            Return 0
        End If
        If TypeOf objVal Is DBNull Then
            Return 0
        End If
        If TypeOf objVal Is Int64 Then
            Return CLng(objVal)
        End If
        Long.TryParse(objVal.ToString(), longValue)
        Return longValue
    End Function
    Private Function GetDecimal(objVal As Object) As Decimal
        Dim decimalValue As Decimal = 0
        If objVal Is Nothing Then
            Return 0
        End If
        If TypeOf objVal Is DBNull Then
            Return 0
        End If
        If TypeOf objVal Is Decimal Then
            Return CDec(objVal)
        End If
        Decimal.TryParse(objVal.ToString(), decimalValue)
        Return decimalValue
    End Function
    Private Function GetDouble(objVal As Object) As [Double]
        Dim doubleValue As [Double] = 0
        If objVal Is Nothing Then
            Return 0
        End If
        If TypeOf objVal Is DBNull Then
            Return 0
        End If
        If TypeOf objVal Is [Double] Then
            Return DirectCast(objVal, [Double])
        End If
        [Double].TryParse(objVal.ToString(), doubleValue)
        Return doubleValue
    End Function
    ' Get Date type value from object.
    Private Function GetDate(objVal As Object) As DateTime
        If objVal Is Nothing Then
            Return DateTime.MinValue
        End If
        If TypeOf objVal Is DBNull Then
            Return DateTime.MinValue
        End If
        If TypeOf objVal Is String AndAlso String.IsNullOrEmpty(objVal.ToString()) Then
            Return DateTime.MinValue
        End If
        Return Convert.ToDateTime(objVal)
    End Function
    Public Function ReturnDataReader(SqlQuery As String, QueryType As CommandType, ParamArray ParameterList As SqlParameter()) As SqlDataReader
        Dim dr As SqlDataReader = Nothing
        If ParameterList.Length = 0 Then

            dr = SqlHelper.ExecuteReader(ConnectionStr, QueryType, SqlQuery.ToString())
        Else
            dr = SqlHelper.ExecuteReader(ConnectionStr, QueryType, SqlQuery.ToString(), ParameterList)
        End If
        Return dr
    End Function
    Public Shared Function Encrypt(plainText As String, passPhrase As String, saltValue As String, hashAlgorithm As String, passwordIterations As Integer, initVector As String, _
  keySize As Integer) As String
        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)
        Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
        Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
        Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)
        Dim symmetricKey As New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Dim encryptor As ICryptoTransform = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)
        Dim memoryStream As New MemoryStream()
        Dim cryptoStream As New CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)
        cryptoStream.FlushFinalBlock()
        Dim cipherTextBytes As Byte() = memoryStream.ToArray()
        memoryStream.Close()
        cryptoStream.Close()
        Dim cipherText As String = Convert.ToBase64String(cipherTextBytes)
        Return cipherText
    End Function
    Public Shared Function Decrypt(cipherText As String, passPhrase As String, saltValue As String, hashAlgorithm As String, passwordIterations As Integer, initVector As String, _
     keySize As Integer) As String
        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)
        Dim cipherTextBytes As Byte() = Convert.FromBase64String(cipherText)
        Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
        Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)
        Dim symmetricKey As New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC
        Dim decryptor As ICryptoTransform = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)
        Dim memoryStream As New MemoryStream(cipherTextBytes)
        Dim cryptoStream As New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)
        Dim plainTextBytes As Byte() = New Byte(cipherTextBytes.Length - 1) {}
        Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)
        memoryStream.Close()
        cryptoStream.Close()
        Dim plainText As String = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)
        Return plainText
    End Function
End Class

