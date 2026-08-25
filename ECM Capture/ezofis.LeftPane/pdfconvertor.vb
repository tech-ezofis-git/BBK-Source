Option Strict Off
Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Drawing.Imaging
Imports Leadtools.ImageProcessing
Imports Leadtools.Codecs
Imports System.Text
Imports Leadtools
Imports Leadtools.Forms.Ocr
Imports Leadtools.Forms.DocumentWriters
Imports System.Reflection
Imports System.Data
Imports ezofis.UserControl.CAC
Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Security.Cryptography

Public Class pdfconvertor
    Private Declare Function VeryRunCommand Lib "verywrite.dll" (ByVal szCommand As String, ByVal Para1 As Integer, ByVal Para2 As Integer, ByVal szPara3 As String, ByVal szPara4 As String) As Integer
    Dim folderin As System.IO.DirectoryInfo
    Dim filein As System.IO.FileInfo
    Dim CAC As New CACserviceClient
    Public Function ContainsUnicodeCharacter(ByVal input As String) As Boolean
        Dim ascii As Encoding = Encoding.ASCII
        Dim unicode As Encoding = Encoding.Unicode

        ' Convert the string into a byte array. 
        Dim unicodeBytes As Byte() = unicode.GetBytes(input)

        ' Perform the conversion from one encoding to the other. 
        Dim asciiBytes As Byte() = Encoding.Convert(unicode, ascii, unicodeBytes)

        ' Convert the new byte array into a char array and then into a string. 
        Dim asciiChars(ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length) - 1) As Char
        ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0)
        Dim asciiString As New String(asciiChars)
        If asciiString.Contains("?") Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function ConvertToPDF(ByVal volume As String, ByVal filename As String, ByVal inFile As String, ByVal strTitle As String, ByVal strSubject As String, ByVal strAuthor As String, ByVal strKeyWords As String, ByVal signature As String, ByVal outPUTfile As String) As Integer
        Dim ret As Integer
        outfile = ""
        Dim arbicname As String = ""
        Try
            createfolder(volume)
            If IO.File.Exists(volume & "\" & filename & ".pdf") Then
                Dim Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("Database"), NameValueCollection)
                'if can continue 0 means abort 1 means create version 2 means replace file
                If Appcon("version").ToString = "1" Then
                    Application.Current.Dispatcher.Invoke(DirectCast(Sub()
                                                                         Dim v As New Versioning(Path.GetFileName(inFile), "")
                                                                         v.ShowDialog()
                                                                     End Sub, Action))
                    If cancontinue = 1 Then
                        Dim values() = Function_VersionCreation(ECMRightPane.cabinetid.ToString(), ECMRightPane.templateid.ToString(), volume.Replace("'", "''"), filename.Replace("'", "''").Replace("/", "-") & ".pdf")
                        Dim Version As String = values(0)
                        Dim oldversion As String = values(1)
                        vers = Version
                        pdfilename = filename & "_" & Version
                        outfile = volume & "\" & pdfilename & ".pdf"
                    ElseIf cancontinue = 0 Then
                        ConvertToPDF = 2
                        Exit Function
                    ElseIf cancontinue = 2 Then
                        Try
                            File.Delete(volume & "\" & filename & ".pdf")
                            File.Delete(volume & "\" & filename & ".jpg")
                        Catch ex As Exception
                        End Try
                        pdfilename = filename
                        outfile = volume & "\" & filename.Replace("/", "-") & ".pdf"
                        vers = "1.0"
                    End If
                Else
                    Dim values() = Function_VersionCreation(ECMRightPane.cabinetid.ToString(), ECMRightPane.templateid.ToString(), volume.Replace("'", "''"), filename.Replace("'", "''").Replace("/", "-") & ".pdf")
                    Dim Version As String = values(0)
                    Dim oldversion As String = values(1)
                    vers = Version
                    pdfilename = filename & "_" & Version
                    outfile = volume & "\" & pdfilename & ".pdf"
                End If
            Else
                pdfilename = filename
                outfile = volume & "\" & filename.Replace("/", "-") & ".pdf"
                vers = "1.0"
            End If
            If Not IsNothing(strSubject) Then
                strSubject = strSubject.Replace("""", "")
                strSubject = strSubject.Replace("'", "")
                strSubject = strSubject.Replace("#", "")
                strSubject = strSubject.Replace("\", "")
                strSubject = strSubject.Replace("&", "")
            Else
                strSubject = Common.Rjunk(System.DateTime.Now)
            End If
            If Not IsNothing(strTitle) Then
                strTitle = strTitle.Replace("""", "")
                strTitle = strTitle.Replace("'", "")
                strTitle = strTitle.Replace("#", "")
                strTitle = strTitle.Replace("\", "")
                strTitle = strTitle.Replace("&", "")
            Else
                strTitle = "untitled"
            End If
            If Not IsNothing(strAuthor) Then
                strAuthor = strAuthor.Replace("""", "")
                strAuthor = strAuthor.Replace("'", "")
                strAuthor = strAuthor.Replace("#", "")
                strAuthor = strAuthor.Replace("\", "")
                strAuthor = strAuthor.Replace("&", "")
            End If
            If Not IsNothing(strKeyWords) Then
                strKeyWords = strKeyWords.Replace("""", "")
                strKeyWords = strKeyWords.Replace("'", "")
                strKeyWords = strKeyWords.Replace("#", "")
                strKeyWords = strKeyWords.Replace("\", "-")
                strKeyWords = strKeyWords.Replace("&", "")
            End If
            Dim boo As Boolean = ContainsUnicodeCharacter(outfile)
            If ContainsUnicodeCharacter(outfile) Then
                Dim temppath As String = inFile.Replace(IO.Path.GetFileName(inFile), "NonameForUnicode.pdf")
                Dim sttime As String = DateTime.Now()
                ret = VeryRunCommand("img2pdf", 0, 0, "-x 1 -b -1 -r -1 -u """ & signature & """ -d ""SMART CAPTURE SYSTEM"" -j """ & strSubject & """ -t """ & strTitle & """ -a """ & strAuthor & """ -k """ & strKeyWords & """ -o """ & temppath & """  """ & inFile & """", "")
                Dim entime As String = DateTime.Now()
                If IO.File.Exists(temppath) Then
                    IO.File.Move(temppath, outfile)
                End If
            Else
                ret = VeryRunCommand("img2pdf", 0, 0, "-x 1 -b -1 -r -1 -u """ & signature & """ -d ""SMART CAPTURE SYSTEM"" -j """ & strSubject & """ -t """ & strTitle & """ -a """ & strAuthor & """ -k """ & strKeyWords & """ -o """ & outfile & """  """ & inFile & """", "")
            End If
            If IO.File.Exists(outfile) Then
                Dim f As New IO.FileInfo(outfile)
                Pvariable.docsize = f.Length
                Pvariable.nopages = pageCountPDF(outfile)
                ConvertToPDF = ret
                If cancontinue = 2 Then
                    ConvertToPDF = 3
                End If
                If IO.File.Exists(inFile) Then
                    _codecs = New RasterCodecs
                    Dim tifftoJpg As Leadtools.RasterImage = _codecs.Load(inFile)
                    Dim destImg As Leadtools.RasterImage = New Leadtools.RasterImage(Leadtools.RasterMemoryFlags.Conventional, 600, 600, tifftoJpg.BitsPerPixel, tifftoJpg.Order, tifftoJpg.ViewPerspective, tifftoJpg.GetPalette(), IntPtr.Zero, 0)
                    Dim sizer As New ResizeCommand
                    sizer.DestinationImage = destImg
                    sizer.Flags = Leadtools.RasterSizeFlags.Resample
                    sizer.Run(tifftoJpg)
                    _codecs.Save(destImg, outfile.Replace(Path.GetExtension(outfile), ".jpg"), Leadtools.RasterImageFormat.Jpeg, 24)
                    'End If
                End If
            Else
                ConvertToPDF = 0
            End If
        Catch ex As Exception
            errstr = ex.Message.ToString
            ConvertToPDF = 0
        End Try
    End Function
    Private Function Function_VersionCreation(ByVal CabId As String, ByVal TempId As String, ByVal IfilePath As String, ByVal filename As String) As String()
        Dim NewVersion As String = String.Empty
        Dim DublicateType As String = ""
        Dim ExistVersion As String = String.Empty
        Dim TblName As String = "eZCA_" & CabId & "_" & TempId & "_Items"
        'Dim oj = obj.GetDBCONFIG()
        Dim StrSql As String = "Select DuplicateType From ezDuplicateType Where DuplicateTypeId = (Select DuplicateTypeID From ezTemplate Where TemplateId=" & TempId & ")"

        Dim Dt As DataSet = CAC.GetDatasetByQuery(StrSql)
        If Dt.Tables(0).Rows.Count > 0 Then
            DublicateType = Dt.Tables(0).Rows(0).Item("DuplicateType")

            'StrSql = "Select Version From " & TblName & " Where Isdeleted=0 and (ifileName Like N'" + filename + "' or " +
            '    "ifileName Like '" + filename.Insert(filename.LastIndexOf("."), "[_]%") + "') " +
            '    "And IfilePath=N'" & IfilePath & "' order by itemid desc"

            StrSql = "Select itemid,ifilename,Version From " & TblName & "  a left join eZERSInfo b on a.ERSId=b.ERSId Where a.Isdeleted=0 and (ifileName Like N'" + filename + "' or " +
              "ifileName Like '" + filename.Insert(filename.LastIndexOf("."), "[_]%") + "') " +
              "And b.ersdirpath+'\'+ IfilePath=N'" & IfilePath & "\' order by itemid desc"

            Dt = CAC.GetDatasetByQuery(StrSql)

            If Dt.Tables(0).Rows.Count > 0 Then

                ExistVersion = Dt.Tables(0).Rows(0).Item("Version")
                Oldfilenames = Dt.Tables(0).Rows(0).Item("ifilename")
                itemid = Dt.Tables(0).Rows(0).Item("itemid")
                'Else
                '    ExistVersion = "RenameOldFile"
                '    NewVersion = "RenameOldFile"
            End If
            If DublicateType = "_A" Then
                If ExistVersion = "1.0" Or ExistVersion = "1" Then
                    NewVersion = "A"
                ElseIf ExistVersion <> "Z" And ExistVersion.Count = 1 Then
                    NewVersion = Chr(Asc(ExistVersion) + 1)
                ElseIf ExistVersion = "Z" Then
                    NewVersion = "AA"
                ElseIf ExistVersion.Count = 2 And ExistVersion <> "ZZ" Then
                    If ExistVersion.Substring(1, 1) = "Z" Then
                        NewVersion = Chr(Asc(ExistVersion.Substring(0, 1)) + 1) & "A"
                    Else
                        NewVersion = Chr(Asc(ExistVersion.Substring(0, 1))) + Chr(Asc(ExistVersion.Substring(1, 1)) + 1)
                    End If
                ElseIf ExistVersion = "ZZ" Then
                    NewVersion = "AAA"
                ElseIf ExistVersion.Count = 3 And ExistVersion <> "ZZZ" Then
                    If ExistVersion.Substring(2, 1) = "Z" And ExistVersion.Substring(1, 1) <> "Z" Then
                        NewVersion = Chr(Asc(ExistVersion.Substring(0, 1))) & Chr(Asc(ExistVersion.Substring(1, 1)) + 1) & "A"
                    ElseIf ExistVersion.Substring(2, 1) = "Z" And ExistVersion.Substring(1, 1) = "Z" Then
                        NewVersion = Chr(Asc(ExistVersion.Substring(0, 1)) + 1) & "A" & "A"
                    Else
                        NewVersion = Chr(Asc(ExistVersion.Substring(0, 1))) & Chr(Asc(ExistVersion.Substring(1, 1))) & Chr(Asc(ExistVersion.Substring(2, 1)) + 1)
                    End If
                ElseIf ExistVersion = "ZZZ" Then
                    NewVersion = "AAAA"
                End If
            ElseIf DublicateType = "_1" Then
                If ExistVersion = "" Then
                    NewVersion = "1.0"
                ElseIf ExistVersion = "1.0" Or ExistVersion = "1" Then
                    NewVersion = "2"
                ElseIf ExistVersion <> "" Then
                    NewVersion = (Convert.ToInt32(ExistVersion) + 1).ToString
                End If
            ElseIf DublicateType = "DateTime" Then
                NewVersion = Replace(Replace((DateTime.Now), " ", ""), ":", "")
            Else
                NewVersion = DublicateType
            End If
        End If

        Return {NewVersion, ExistVersion}
    End Function
    Public Function pageCountPDF(ByVal fileName As String) As Integer
        Try
            Using sr As New StreamReader(File.OpenRead(fileName))
                Dim regex As New System.Text.RegularExpressions.Regex("/Type\s*/Page[^s]")
                Dim matches As MatchCollection = regex.Matches(sr.ReadToEnd())
                Return matches.Count
            End Using
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Function
    Public Function PageCountTiff(ByVal FileName As String) As Integer
        Try
            Dim info As CodecsImageInfo = _codecs.GetInformation(FileName, True)
            Return info.TotalPages
            'Dim img As Image = Bitmap.FromFile(FileName)
            'Dim numpages As Integer = img.GetFrameCount(FrameDimension.Page)
            'img.Dispose()
            'Return numpages
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Function
    Public Sub createfolder(ByVal outfile As String)
        If IO.Directory.Exists(outfile) = False Then
            IO.Directory.CreateDirectory(outfile)
        End If
    End Sub
    Public Function ConvertToOther(ByVal Ext As String, ByVal volume As String, ByVal Otherfilename As String, ByVal SourceFileName As String, ByVal outPUTfile As String) As Integer
        Dim ret As Integer
        outfile = ""
        Try
            Dim VolumeCorrect As String = volume.Replace("/", "-").Trim(" ")
            createfolder(VolumeCorrect)
            Otherfilename = Otherfilename.Replace("/", "-")
            If IO.File.Exists(VolumeCorrect & "\" & Otherfilename & "." & Ext) Then
                Dim Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("Database"), NameValueCollection)
                'if can continue 0 means abort 1 means create version 2 means replace file
                If Appcon("version").ToString = "1" Then
                    Application.Current.Dispatcher.Invoke(DirectCast(Sub()
                                                                         Dim v As New Versioning(Path.GetFileName(SourceFileName), "")
                                                                         v.ShowDialog()
                                                                     End Sub, Action))
                    If cancontinue = 1 Then
                        Dim values() = Function_VersionCreation(ECMRightPane.cabinetid.ToString(), ECMRightPane.templateid.ToString(), VolumeCorrect.Replace("'", "''"), Otherfilename.Replace("'", "''").Replace("/", "-") & "." + Ext)
                        Dim Version As String = values(0)
                        Dim oldversion As String = values(1)
                        vers = Version
                        pdfilename = Otherfilename & "_" & Version
                        outfile = volume & "\" & pdfilename & "." + Ext
                    ElseIf cancontinue = 0 Then
                        ConvertToOther = 2
                        Exit Function
                    ElseIf cancontinue = 2 Then
                        Try
                            File.Delete(VolumeCorrect & "\" & Otherfilename & "." & Ext)
                            File.Delete(VolumeCorrect & "\" & Otherfilename & ".jpg")
                        Catch ex As Exception
                        End Try
                        vers = "1.0"
                        pdfilename = Otherfilename
                        outfile = VolumeCorrect & "\" & pdfilename & "." & Ext
                    End If
                Else
                    Dim values() = Function_VersionCreation(ECMRightPane.cabinetid.ToString(), ECMRightPane.templateid.ToString(), VolumeCorrect.Replace("'", "''"), Otherfilename.Replace("'", "''").Replace("/", "-") & "." + Ext)
                    Dim Version As String = values(0)
                    Dim oldversion As String = values(1)
                    vers = Version
                    pdfilename = Otherfilename & "_" & Version
                    outfile = volume & "\" & pdfilename & "." + Ext
                End If
            Else
                vers = "1.0"
                pdfilename = Otherfilename
                outfile = VolumeCorrect & "\" & pdfilename & "." & Ext
            End If
            ret = 1
            ConvertToOther = ret
            If Not IO.File.Exists(outfile) Then

                File.Copy(SourceFileName, outfile)




                Dim f As New IO.FileInfo(outfile)
                Pvariable.docsize = f.Length
                Pvariable.nopages = pageCountPDF(outfile)
                ConvertToOther = 1
                If cancontinue = 2 Then
                    ConvertToOther = 3
                End If
                If IO.File.Exists(outPUTfile) Then
                    _codecs = New RasterCodecs
                    Dim tifftoJpg As Leadtools.RasterImage = _codecs.Load(outPUTfile)
                    Dim destImg As Leadtools.RasterImage = New Leadtools.RasterImage(Leadtools.RasterMemoryFlags.Conventional, 600, 600, tifftoJpg.BitsPerPixel, tifftoJpg.Order, tifftoJpg.ViewPerspective, tifftoJpg.GetPalette(), IntPtr.Zero, 0)
                    Dim sizer As New ResizeCommand
                    sizer.DestinationImage = destImg
                    sizer.Flags = Leadtools.RasterSizeFlags.Resample
                    sizer.Run(tifftoJpg)
                    _codecs.Save(destImg, outfile.Replace(Path.GetExtension(outfile), ".jpg"), Leadtools.RasterImageFormat.Jpeg, 24)
                End If

                Try
                    ' CAC.FileEncryptOrDecryptFile(cabinetid.ToString(), templateid.ToString, itemid.ToString, "3z0f1s$ecm", 1)
                    Dim strInputFile = IO.Path.Combine(outfile)
                    Dim strOutputFile = IO.Path.Combine(outfile.ToLower.Replace(".pdf", ".ezo"))
                    Dim bytKey As Byte()
                    Dim bytIV As Byte()
                    bytKey = CreateKey("3z0f1s$ecm")
                    bytIV = CreateIV("3z0f1s$ecm")
                    Dim result As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionEncrypt)
                    If result <> "Encryption Complete" Then
                        '  writetxtfle("File valut : Encryption Complete")
                        ' outfile = strOutputFile
                    End If
                Catch ex As Exception
                    ' writetxtfle("File valut : " + ex.Message)
                End Try
            End If
        Catch ex As Exception
            errstr = ex.Message.ToString
            ConvertToOther = 0
        End Try
    End Function

    Public Enum CryptoAction

        ActionEncrypt = 1
        ActionDecrypt = 2
    End Enum
    Private Function EncryptOrDecryptFile(ByVal strInputFile As String,
                                    ByVal strOutputFile As String,
                                    ByVal bytKey() As Byte,
                                    ByVal bytIV() As Byte,
                                    ByVal Direction As CryptoAction) As String

        Dim fsInput As System.IO.FileStream
        Dim fsOutput As System.IO.FileStream
        Try 'In case of errors.
            'Setup file streams to handle input and output.
            fsInput = New System.IO.FileStream(strInputFile, FileMode.Open,
                                               FileAccess.Read)
            fsOutput = New System.IO.FileStream(strOutputFile, FileMode.OpenOrCreate,
                                                FileAccess.Write)
            fsOutput.SetLength(0) 'make sure fsOutput is empty
            'Declare variables for encrypt/decrypt process.
            Dim bytBuffer(4096) As Byte 'holds a block of bytes for processing
            Dim lngBytesProcessed As Long = 0 'running count of bytes processed
            Dim lngFileLength As Long = fsInput.Length 'the input file's length
            Dim intBytesInCurrentBlock As Integer 'current bytes being processed
            Dim csCryptoStream As CryptoStream
            'Declare your CryptoServiceProvider.
            Dim cspRijndael As New System.Security.Cryptography.RijndaelManaged
            'Setup Progress Bar
            'Determine if ecryption or decryption and setup CryptoStream.
            Select Case Direction
                Case CryptoAction.ActionEncrypt
                    csCryptoStream = New CryptoStream(fsOutput,
                    cspRijndael.CreateEncryptor(bytKey, bytIV),
                    CryptoStreamMode.Write)
                Case CryptoAction.ActionDecrypt
                    csCryptoStream = New CryptoStream(fsOutput,
                    cspRijndael.CreateDecryptor(bytKey, bytIV),
                    CryptoStreamMode.Write)
            End Select
            'Use While to loop until all of the file is processed.
            While lngBytesProcessed < lngFileLength
                'Read file with the input filestream.
                intBytesInCurrentBlock = fsInput.Read(bytBuffer, 0, 4096)
                'Write output file with the cryptostream.
                csCryptoStream.Write(bytBuffer, 0, intBytesInCurrentBlock)
                'Update lngBytesProcessed
                lngBytesProcessed = lngBytesProcessed + CLng(intBytesInCurrentBlock)
                'Update Progress Bar
            End While
            'Close FileStreams and CryptoStream.
            csCryptoStream.Close()
            fsInput.Close()
            fsOutput.Close()
            'If encrypting then delete the original unencrypted file.
            If Direction = CryptoAction.ActionEncrypt Then
                Dim fileOriginal As New FileInfo(strInputFile)
                fileOriginal.Delete()
            End If
            'If decrypting then delete the encrypted file.
            If Direction = CryptoAction.ActionDecrypt Then
                Dim fileEncrypted As New FileInfo(strInputFile)
                fileEncrypted.Delete()
            End If
            'Update the user when the file is done.
            Dim Wrap As String = Chr(13) + Chr(10)
            If Direction = CryptoAction.ActionEncrypt Then
                Return "Encryption Complete"
                'MsgBox("Encryption Complete" + Wrap + Wrap + _
                '        "Total bytes processed = " + _
                '        lngBytesProcessed.ToString, _
                '        MsgBoxStyle.Information, "Done")
                'Update the progress bar and textboxes.
            Else
                'Update the user when the file is done.
                Return "Decryption Complete"
                'MsgBox("Decryption Complete" + Wrap + Wrap + _
                '       "Total bytes processed = " + _
                '        lngBytesProcessed.ToString, _
                '        MsgBoxStyle.Information, "Done")

                'Update the progress bar and textboxes.
            End If
            'Catch file not found error.
        Catch When Err.Number = 53 'if file not found
            Return "Please check to make sure the path and filename" +
                    "are correct and if the file exists."
            'MsgBox("Please check to make sure the path and filename" + _
            '        "are correct and if the file exists.", _
            '         MsgBoxStyle.Exclamation, "Invalid Path or Filename")
            'Catch all other errors. And delete partial files.
        Catch
            fsInput.Close()
            fsOutput.Close()
            If Direction = CryptoAction.ActionDecrypt Then
                Dim fileDelete As New FileInfo(strOutputFile)
                fileDelete.Delete()
                Return "Please check to make sure that you entered the correct" +
                        "password."
                'MsgBox("Please check to make sure that you entered the correct" + _
                '        "password.", MsgBoxStyle.Exclamation, "Invalid Password")
            Else
                Dim fileDelete As New FileInfo(strOutputFile)
                fileDelete.Delete()
                Return "This file cannot be encrypted."
                'MsgBox("This file cannot be encrypted.", _
                '        MsgBoxStyle.Exclamation, "Invalid File")
            End If
        End Try
    End Function


    Private Function CreateKey(ByVal strPassword As String) As Byte()
        Try
            'Convert strPassword to an array and store in chrData.
            Dim chrData() As Char = strPassword.ToCharArray
            'Use intLength to get strPassword size.
            Dim intLength As Integer = chrData.GetUpperBound(0)
            'Declare bytDataToHash and make it the same size as chrData.
            Dim bytDataToHash(intLength) As Byte

            'Use For Next to convert and store chrData into bytDataToHash.
            For i As Integer = 0 To chrData.GetUpperBound(0)
                bytDataToHash(i) = CByte(Asc(chrData(i)))
            Next

            'Declare what hash to use.
            Dim SHA512 As New System.Security.Cryptography.SHA512Managed
            'Declare bytResult, Hash bytDataToHash and store it in bytResult.
            Dim bytResult As Byte() = SHA512.ComputeHash(bytDataToHash)
            'Declare bytKey(31).  It will hold 256 bits.
            Dim bytKey(31) As Byte

            'Use For Next to put a specific size (256 bits) of 
            'bytResult into bytKey. The 0 To 31 will put the first 256 bits
            'of 512 bits into bytKey.
            For i As Integer = 0 To 31
                bytKey(i) = bytResult(i)
            Next

            Return bytKey 'Return the key.
        Catch ex As Exception
            Dim exc As String
            'writetxtfle("CreateKey : " + ex.ToString())
            ' Throw New FaultException(exc)
        End Try
    End Function

    Private Function CreateIV(ByVal strPassword As String) As Byte()
        'Convert strPassword to an array and store in chrData.
        Try
            Dim chrData() As Char = strPassword.ToCharArray
            'Use intLength to get strPassword size.
            Dim intLength As Integer = chrData.GetUpperBound(0)
            'Declare bytDataToHash and make it the same size as chrData.
            Dim bytDataToHash(intLength) As Byte

            'Use For Next to convert and store chrData into bytDataToHash.
            For i As Integer = 0 To chrData.GetUpperBound(0)
                bytDataToHash(i) = CByte(Asc(chrData(i)))
            Next

            'Declare what hash to use.
            Dim SHA512 As New System.Security.Cryptography.SHA512Managed
            'Declare bytResult, Hash bytDataToHash and store it in bytResult.
            Dim bytResult As Byte() = SHA512.ComputeHash(bytDataToHash)
            'Declare bytIV(15).  It will hold 128 bits.
            Dim bytIV(15) As Byte

            'Use For Next to put a specific size (128 bits) of 
            'bytResult into bytIV. The 0 To 30 for bytKey used the first 256 bits.
            'of the hashed password. The 32 To 47 will put the next 128 bits into bytIV.
            For i As Integer = 32 To 47
                bytIV(i - 32) = bytResult(i)
            Next

            Return bytIV 'return the IV
        Catch ex As Exception
            Dim exc As String
            ' exc = "ERROR CODE:WSR640F300 " + ex.ToString()
            'writetxtfle("CreateKey : " + ex.ToString())
        End Try
    End Function

End Class
Public NotInheritable Class LEAD_VARS
    Public Const ImagesDir As String = "C:\Users\Public\Documents\LEADTOOLS Images"
    Public Const OcrAdvantageRuntimeDir As String = "C:\LEADTOOLS 18\Bin\Common\OcrAdvantageRuntime"
End Class