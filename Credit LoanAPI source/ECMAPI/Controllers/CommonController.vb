Imports System.Net
Imports System.Web.Http
Imports ECMAPI.ParaVariables
Imports ECMAPI.SharedGetFunction
Imports Newtonsoft.Json
Imports System.Web
Imports System.Xml
Imports System.IO
Imports System.Net.Http
Imports System.Threading.Tasks
'for Merge 
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html.simpleparser
Imports System.Security.Permissions
Imports System.Security

Namespace Controllers
    Public Class CommonController
        Inherits ApiController


        <HttpPost>
        Public Function GetDatasetByQuery(para As ByQuery) As DataSet
            Try
                Return SharedGetFunction.GetDatasetByQuery(para.StrQry)
            Catch ex As Exception
                ' Return result
            End Try
        End Function

        <HttpPost>
        Public Function InsertAndUpdateAndDeleteeZUserDefined(para As ByQuery) As String
            Try
                Return SharedGetFunction.InsertAndUpdateAndDeleteeZUserDefined(para.StrQry)
            Catch ex As Exception
                ' Return result
            End Try
        End Function

        <HttpPost>
        Public Function InsertAndUpdateAndDeleteeZUserDefinedWithScope(para As ByQuery) As String
            Try
                Return SharedGetFunction.InsertAndUpdateAndDeleteeZUserDefinedWithScope(para.StrQry)
            Catch ex As Exception
                ' Return result
            End Try
        End Function

        <HttpPost>
        Public Function InsertAndUpdate(para As ByQuery) As String
            Try
                Return SharedGetFunction.InsertAndUpdate(para.StrQry)
            Catch ex As Exception
                ' Return result
            End Try
        End Function

        <HttpPost>
        Public Function GetDefaultDateFormet(para As Dates) As String
            Try
                Return DateDateTimeToString(para.CurrentDate, para.WithTime)

            Catch ex As Exception

            End Try
        End Function


        <HttpPost>
        Public Function GetTableName(para As Templateids) As String
            Try
                Return SharedGetFunction.GetTableName(para.Templateid)

            Catch ex As Exception

            End Try
        End Function


        <HttpPost>
        Public Function GetUniqId(para As UniqueIdGen) As String
            Try
                Return SharedGetFunction.GetUniqId(para.GenName)
            Catch ex As Exception
                ' Return result
            End Try
        End Function

        <HttpPost>
        Public Function GetInfoFromRimNo(para As InsRimNo) As Object
            Dim lstout As New Object
            Dim json = ""
            Try
                ServicePointManager.Expect100Continue = True
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                Dim Input1 = New InsRimNo
                Input1.rim_no = para.rim_no '"100000027062"
                Dim client = New WebClient()
                client.Headers("Content-Type") = "application/json"
                Dim inputJson = JsonConvert.SerializeObject(Input1)
                'json = client.UploadString("http://10.1.18.52:8080/uat/customer_positon_dac870/customer_positon_dac870", "POST", inputJson)
                json = client.UploadString(para.url, "POST", inputJson)
                lstout = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Object)(json)
            Catch ex As Exception
                json += ex.Message.ToString
            End Try
            Return lstout
        End Function

        <HttpPost>
        Public Function GetInfoFromAccountNo(para As InsAccNo) As Object
            Dim lstout As New Object
            Dim json = ""
            Try
                ServicePointManager.Expect100Continue = True
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                Dim Input1 = New InsAccNo
                Input1.acct_no = para.acct_no '"100000027062"
                Dim client = New WebClient()
                client.Headers("Content-Type") = "application/json"
                Dim inputJson = JsonConvert.SerializeObject(Input1)
                'json = client.UploadString("http://10.1.18.52:8080/uat/tf_acct_basic_info/tf_acct_basic_info", "POST", inputJson)
                json = client.UploadString(para.url, "POST", inputJson)
                lstout = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Object)(json)
            Catch ex As Exception
                json = ex.Message.ToString
            End Try
            Return lstout
        End Function

#Region "merge files"
        '<HttpPost>
        'Public Function RetailGetFilesFromRIMNumber(para As Condition) As HttpResponseMessage
        '    Dim response As HttpResponseMessage
        '    Dim result As New Data
        '    Dim paraqry As New ByQuery
        '    Dim UserId = ""
        '    Dim ext = "", pathlist = "", imglist = "", pdfpath = ""
        '    If para.username = "" Then
        '        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Username should not be Empty")
        '    ElseIf para.RIMNumber = "" Then
        '        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "RIM Number should not be Empty")
        '    Else
        '        Try
        '            paraqry.StrQry = "select EcmLoginid userid from eZECMLogin where LoginName='" + para.username + "'"
        '            ' strqry = "select userid from eZsbLogininfo where username='" + para.username + "'"
        '            Dim dsu = GetDatasetByQuery(paraqry)
        '            If Not dsu Is Nothing AndAlso dsu.Tables.Count > 0 AndAlso dsu.Tables(0).Rows.Count > 0 Then
        '                Dim password = DBLayer.Encrypt(para.password, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
        '                paraqry.StrQry = "select EcmLoginid userid from eZECMLogin where LoginName='" + para.username + "' and pasword='" + password + "'"
        '                Dim dsp = GetDatasetByQuery(paraqry)
        '                If Not dsp Is Nothing AndAlso dsp.Tables.Count > 0 AndAlso dsp.Tables(0).Rows.Count > 0 Then

        '                    UserId = dsp.Tables(0).Rows(0)("userid").ToString

        '                    paraqry.StrQry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype,[Tin Number],[Rim Type],[Document Type],[Rim Branch],[Rim Name],[File Location] from ezca_3_9_items i left join eZERSInfo e on e.ERSId=i.ERSId  where [rim number]='" + para.RIMNumber + "'"
        '                    'strqry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype from ezca_101_items i left join eZERSInfo e on e.ERSId=i.ERSId"
        '                    Dim ds = GetDatasetByQuery(paraqry)
        '                    If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

        '                        Dim TempFilepath = Hosting.HostingEnvironment.MapPath("~\Mergefiles\" + UserId.ToString + "")
        '                        If (Not System.IO.Directory.Exists(TempFilepath)) Then
        '                            System.IO.Directory.CreateDirectory(TempFilepath)
        '                        End If
        '                        Try
        '                            Dim txtList As String() = Directory.GetFiles(TempFilepath)
        '                            Dim F2 As Short = FreeFile()
        '                            For Each f As String In txtList
        '                                Try
        '                                    File.Delete(f)
        '                                Catch ex As Exception
        '                                End Try

        '                            Next
        '                        Catch ex As Exception

        '                        End Try
        '                        Dim clientD As New System.Net.WebClient()
        '                        Dim localfile = ""
        '                        For Each Row In ds.Tables(0).Rows
        '                            Dim Filename As String = Path.Combine(Row("DirPath").ToString, Row("ifilepath").ToString, Path.GetFileNameWithoutExtension(Row("ifilename").ToString) + ".ezo")

        '                            If File.Exists(Filename) Then

        '                                localfile = System.IO.Path.Combine(TempFilepath, Path.GetFileNameWithoutExtension(Row("ifilename").ToString) + ".ezo")
        '                                If File.Exists(localfile) Then
        '                                    localfile = System.IO.Path.Combine(TempFilepath, Path.GetFileNameWithoutExtension(Row("ifilename").ToString) + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + ".ezo")
        '                                End If
        '                                clientD.DownloadFile(Filename, localfile)
        '                                clientD.Dispose()

        '                                If File.Exists(localfile) Then
        '                                    Dim strInputFile = localfile
        '                                    Dim strOutputFile = localfile.Replace(".ezo", ".pdf")
        '                                    Dim bytKey As Byte()
        '                                    Dim bytIV As Byte()
        '                                    bytKey = CreateKey("3z0f1s$ecm")
        '                                    bytIV = CreateIV("3z0f1s$ecm")
        '                                    Dim resulte As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionDecrypt)
        '                                    If resulte <> "Decryption Complete" Then
        '                                        'writetxtfle("File valut : Encryption Complete")
        '                                    End If
        '                                    ext = Path.GetExtension(strOutputFile).ToLower().Replace(".", "")
        '                                    If ext = "pdf" Then
        '                                        pathlist += strOutputFile + ","
        '                                    ElseIf ext = "jpg" Or ext = "jpeg" Or ext = "png" Or ext = "bmp" Or ext = "gif" Or ext.ToLower = "tif" Or ext.ToLower = "tiff" Then
        '                                        imglist += strOutputFile + ","
        '                                    End If
        '                                End If
        '                            End If
        '                        Next
        '                        Dim pdfname As String = ""
        '                        If pathlist.Length <> 0 Then
        '                            pathlist = pathlist.Substring(0, pathlist.Length - 1)
        '                        End If
        '                        If imglist.Length <> 0 Then
        '                            imglist = imglist.Substring(0, imglist.Length - 1)
        '                        End If

        '                        pdfname = UserId + DateTime.Now.ToString("dd/MM/yyyyhh:mm:ss").Replace("/", "").Replace(":", "").Replace("-", "")
        '                        pdfpath = TempFilepath + "\" + pdfname + ".pdf"
        '                        If pathlist <> "" Or imglist <> "" Then
        '                            Dim res = MergeFiles(pdfpath, pathlist.Split(","), imglist.Split(","))
        '                            If res = "" Then
        '                                Try
        '                                    For Each DelFileName In System.IO.Directory.GetFiles(TempFilepath)
        '                                        Dim FilesToExclude As String() = {pdfname + ".pdf"}
        '                                        If Not Array.Exists(FilesToExclude, Function(element) element = Path.GetFileName(DelFileName)) Then
        '                                            Try
        '                                                System.IO.File.Delete(DelFileName)
        '                                            Catch ex As Exception
        '                                            End Try

        '                                        End If
        '                                    Next
        '                                Catch ex As Exception
        '                                    ' response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString)
        '                                End Try
        '                                result.url = "https://dcms.invita.com.bh/EZAPI/v1/Common/DownloadFiles?userid=" + UserId + "&filename=" + pdfname + ""
        '                                response = Request.CreateResponse(HttpStatusCode.OK, result)
        '                            Else
        '                                response = Request.CreateErrorResponse(HttpStatusCode.NoContent, res)
        '                            End If
        '                        Else
        '                            response = Request.CreateErrorResponse(HttpStatusCode.NoContent, "Files not found")
        '                        End If
        '                    Else
        '                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid RIM NUmber")
        '                    End If
        '                Else
        '                    response = Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Password")
        '                End If
        '            Else
        '                response = Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid username")
        '            End If
        '        Catch ex As Exception
        '            response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString)
        '        End Try
        '    End If
        '    Return response
        'End Function

        <HttpPost>
        Public Function RetailGetFilesFromRIMNumber(para As Conditionforcommon ) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Dim result As New Data
            Dim paraqry As New ByQuery
            Dim pathid = ""
            Dim ext = "", pathlist = "", imglist = "", pdfpath = ""
            Try
                pathid = para.RIMNumber '+ "_" + DateTime.Now.ToString("yyyyMMddhhmmssffftt")

                paraqry.StrQry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype,[Tin Number],[Rim Type],[Document Type],[Rim Branch],[Rim Name],[File Location] from ezca_3_9_items i left join eZERSInfo e on e.ERSId=i.ERSId  where [rim number]='" + para.RIMNumber + "' and i.isdeleted=0"
                'strqry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype from ezca_101_items i left join eZERSInfo e on e.ERSId=i.ERSId"
                Dim ds = GetDatasetByQuery(paraqry)
                If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                    Dim TempFilepath = Hosting.HostingEnvironment.MapPath("~\Mergefiles\" + pathid.ToString + "")
                    If (Not System.IO.Directory.Exists(TempFilepath)) Then
                        System.IO.Directory.CreateDirectory(TempFilepath)
                    End If
                    Try
                        Dim txtList As String() = Directory.GetFiles(TempFilepath)
                        Dim F2 As Short = FreeFile()
                        For Each f As String In txtList
                            Try
                                File.Delete(f)
                            Catch ex As Exception
                                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "E1_" + ex.Message)
                            End Try

                        Next
                    Catch ex As Exception
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "E2_" + ex.Message)
                    End Try
                    Dim clientD As New System.Net.WebClient()
                    Dim localfile = ""
                    For Each Row In ds.Tables(0).Rows
                        Dim Filename As String = Path.Combine(Row("DirPath").ToString, Row("ifilepath").ToString, Path.GetFileNameWithoutExtension(Row("ifilename").ToString) + ".ezo")

                        ' Dim writePermission As FileIOPermission = New FileIOPermission(FileIOPermissionAccess.AllAccess, Filename)

                        If IUsername <> "" AndAlso IUNCpath <> "" Then
                            Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                                Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
                                If uncval Then
                                    If File.Exists(Filename) Then
                                        localfile = System.IO.Path.Combine(TempFilepath, Path.GetFileNameWithoutExtension(Row("ifilename").ToString) + ".ezo")
                                        If File.Exists(localfile) Then
                                            localfile = System.IO.Path.Combine(TempFilepath, Path.GetFileNameWithoutExtension(Row("ifilename").ToString) + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + ".ezo")
                                        End If
                                        clientD.DownloadFile(Filename, localfile)
                                        clientD.Dispose()

                                        If File.Exists(localfile) Then
                                            Dim strInputFile = localfile
                                            Dim strOutputFile = localfile.Replace(".ezo", ".pdf")
                                            Dim bytKey As Byte()
                                            Dim bytIV As Byte()
                                            bytKey = CreateKey("3z0f1s$ecm")
                                            bytIV = CreateIV("3z0f1s$ecm")
                                            Dim resulte As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionDecrypt)
                                            If resulte <> "Decryption Complete" Then
                                                'writetxtfle("File valut : Encryption Complete")
                                            End If
                                            ext = Path.GetExtension(strOutputFile).ToLower().Replace(".", "")
                                            If ext = "pdf" Then
                                                pathlist += strOutputFile + ","
                                            ElseIf ext = "jpg" Or ext = "jpeg" Or ext = "png" Or ext = "bmp" Or ext = "gif" Or ext.ToLower = "tif" Or ext.ToLower = "tiff" Then
                                                imglist += strOutputFile + ","
                                            End If
                                        Else
                                            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "localFiles not exist in " + localfile + "")
                                            Return response
                                        End If
                                    Else
                                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Files not exist in " + Filename + "")
                                        Return response
                                    End If
                                End If
                            End Using
                        End If
                    Next
                    Dim pdfname As String = ""
                    If pathlist.Length <> 0 Then
                        pathlist = pathlist.Substring(0, pathlist.Length - 1)
                    End If
                    If imglist.Length <> 0 Then
                        imglist = imglist.Substring(0, imglist.Length - 1)
                    End If

                    pdfname = DateTime.Now.ToString("dd/MM/yyyyhh:mm:ss").Replace("/", "").Replace(":", "").Replace("-", "")
                    pdfpath = TempFilepath + "\" + pdfname + ".pdf"
                    If pathlist <> "" Or imglist <> "" Then
                        Dim res = MergeFiles(pdfpath, pathlist.Split(","), imglist.Split(","))
                        If res = "" Then
                            Try
                                For Each DelFileName In System.IO.Directory.GetFiles(TempFilepath)
                                    Dim FilesToExclude As String() = {pdfname + ".pdf"}
                                    If Not Array.Exists(FilesToExclude, Function(element) element = Path.GetFileName(DelFileName)) Then
                                        Try
                                            System.IO.File.Delete(DelFileName)
                                        Catch ex As Exception
                                            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "E3_" + ex.Message)
                                        End Try

                                    End If
                                Next
                            Catch ex As Exception
                                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "E4_" + ex.Message)
                            End Try
                            result.url = APIurl + "/v1/Common/DownloadFiles?pathid=" + pathid + "&filename=" + pdfname + ""
                            'result.url = "http://52.172.32.88/InvitaAPI/v1/Common/DownloadFiles?userid=" + UserId + "&filename=" + pdfname + ""
                            response = Request.CreateResponse(HttpStatusCode.OK, result)
                        Else
                            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, res)
                        End If
                    Else
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Files not found .. path list: " + pathlist + "  .. Imagelist: " + imglist)
                    End If
                Else
                    response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid RIM NUmber")
                End If

            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "E5_" + ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function CheckFilePermission(fullfilepath As String) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Dim res = ""
            Try
                Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                    Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
                    If uncval Then
                        If File.Exists(fullfilepath) Then
                            res = "Success"
                        Else
                            res = "file not found"
                        End If
                        response = Request.CreateResponse(HttpStatusCode.OK, res)
                    Else
                        response = Request.CreateErrorResponse(HttpStatusCode.OK, "unc incorrect" + fullfilepath)
                    End If
                End Using

                'Dim writePermission As FileIOPermission = New FileIOPermission(FileIOPermissionAccess.AllAccess, fullfilepath)

                'If SecurityManager.IsGranted(writePermission) Then
                '    If File.Exists(fullfilepath) Then
                '        res = "Success"
                '    Else
                '        res = "file not found"
                '    End If
                'Else
                '    res = "Permission missing"
                'End If

            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "E5_" + ex.Message)
            End Try
            Return response

        End Function
        Public Shared Function MergeFiles(destpath As String, filelist As String(), imglist As String()) As String
            Dim msg = ""
            msg = msg + " MergeFiles strated.."
            Dim document = New Document(PageSize.A4, 20, 20, 20, 20)
            Try
                Dim writer = PdfWriter.GetInstance(document, New FileStream(destpath, FileMode.Create))
                msg = msg + " file created.."
                document.Open()
                If filelist(0) <> "" Then
                    Dim reader = New PdfReader(filelist(0))
                    Dim n = reader.NumberOfPages
                    Dim cb = writer.DirectContent
                    Dim f = 0
                    Dim rotation As Integer
                    Dim page As PdfImportedPage
                    While (f < filelist.Length)
                        Dim i = 0
                        While (i < n)
                            i = i + 1
                            document.SetPageSize(reader.GetPageSizeWithRotation(i))
                            document.NewPage()
                            page = writer.GetImportedPage(reader, i)
                            rotation = reader.GetPageRotation(i)
                            If (rotation = 90 Or rotation = 270) Then
                                cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                            Else
                                cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
                            End If
                        End While
                        f = f + 1
                        If (f < filelist.Length) Then
                            reader = New PdfReader(filelist(f))
                            n = reader.NumberOfPages
                        End If
                    End While
                End If
                If imglist(0) <> "" Then
                    For i = 0 To imglist.Length - 1
                        document.NewPage()
                        Dim img = iTextSharp.text.Image.GetInstance(imglist(i))
                        Dim percentage = 0.0F

                        If (img.Height > img.Width) Then
                            percentage = 700 / img.Height
                        Else
                            percentage = 540 / img.Width
                        End If
                        img.ScalePercent(percentage * 100)
                        document.Add(img)
                    Next
                End If
                document.Dispose()
                writer.Dispose()
                document.Close()
                msg = ""
            Catch ex As Exception
                msg = ex.Message
                document.Dispose()
                document.Close()
            End Try
            Return msg
        End Function

        <HttpGet>
        Public Function DownloadFiles(ByVal pathid As Integer, ByVal filename As String)

            If IUsername <> "" AndAlso IUNCpath <> "" Then
                Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                    Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
                    If uncval Then
                        Try
                            Dim Noimagepath As String = Hosting.HostingEnvironment.MapPath("~\images\Nopreview.jpg")
                            Dim Path As String = ""
                            Dim TempFilepath = Hosting.HostingEnvironment.MapPath("~\Mergefiles\" + pathid.ToString + "")
                            Path = TempFilepath + "\" + filename + ".pdf"
                            Try
                                'Dim filename As String = System.IO.Path.GetFileName(Path)
                                If (System.IO.File.Exists(Path)) Then
                                    HttpContext.Current.Response.ContentType = "application/pdf"
                                    Dim attach = "attachment;filename=" + System.IO.Path.GetFileName(Path)
                                    'HttpContext.Current.Response.Headers.Add("Content-Disposition", attach)
                                    HttpContext.Current.Response.Headers.Add("Content-Disposition", "inline;filename=" + filename)
                                Else
                                    If Noimagepath.EndsWith("jpg") Then
                                        HttpContext.Current.Response.ContentType = "image/jpeg"
                                        Path = Noimagepath
                                    End If
                                End If
                            Catch ex As Exception
                                'Throw New FaultException("ERROR CODE : WDBRJ800F200 : " + ex.ToString)
                            Finally
                                Dim client As New System.Net.WebClient()
                                Dim buffer As [Byte]() = client.DownloadData(Path)
                                If buffer IsNot Nothing Then
                                    HttpContext.Current.Response.AddHeader("content-length", buffer.Length.ToString())
                                    HttpContext.Current.Response.BinaryWrite(buffer)
                                    HttpContext.Current.Response.Flush()
                                    HttpContext.Current.Response.End()
                                End If
                            End Try
                        Catch ex As Exception

                        End Try
                    End If
                End Using
            End If


        End Function

#End Region
    End Class
End Namespace