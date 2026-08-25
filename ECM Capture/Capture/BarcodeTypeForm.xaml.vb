Imports ezofis.UserControl.CAC
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO

Public Class BarcodeTypeForm
    Dim CAC As New CACserviceClient
    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Try
            ezofis.UserControl.BarCodeTypeFromCmb = ComboBox1.SelectedValue
            Me.DialogResult = True
            Dim encrypt1 = Encrypt(ComboBox1.SelectedValue.ToString)
            'ConfigurationSettings.AppSettings.Set("bjQRmrCJaKTQcBDLo92d6Q==", encrypt1)
            ConfigSettings.WriteSetting(Encrypt("BarCodeType"), encrypt1)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try
            Dim screenWidth As Double = System.Windows.SystemParameters.PrimaryScreenWidth
            Dim screenHeight As Double = System.Windows.SystemParameters.PrimaryScreenHeight
            Dim windowWidth As Double = Me.Width
            Dim windowHeight As Double = Me.Height
            Me.Left = (screenWidth / 2) - (windowWidth / 2)
            Me.Top = (screenHeight / 2) - (windowHeight / 2)
            Dim barcodelst = CAC.eZBarcodeTypeList
            ComboBox1.DataContext = barcodelst
            ComboBox1.SelectedIndex = barcodelst.FindIndex(Function(i) i.BarcodeType = ezofis.UserControl.BarCodeTypeFromCmb)
        Catch ex As Exception

        End Try
    End Sub
    Public Function Encrypt(plainText As String) As String
        '    Public Function Encrypt(plainText As String, passPhrase As String, saltValue As String, hashAlgorithm As String, passwordIterations As Integer, initVector As String, _
        'keySize As Integer) As String
        Dim passPhrase As String = "vairavaraj"
        Dim saltValue As String = "vairavaraj"
        Dim hashAlgorithm As String = "SHA1"
        Dim passwordIterations As Integer = 1
        Dim initVector As String = "@v#a5i%r&a7v&a#j"
        Dim keySize As Integer = 192
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
    Public Function Decrypt(cipherText As String) As String
        Dim passPhrase As String = "vairavaraj"
        Dim saltValue As String = "vairavaraj"
        Dim hashAlgorithm As String = "SHA1"
        Dim passwordIterations As Integer = 1
        Dim initVector As String = "@v#a5i%r&a7v&a#j"
        Dim keySize As Integer = 192
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

