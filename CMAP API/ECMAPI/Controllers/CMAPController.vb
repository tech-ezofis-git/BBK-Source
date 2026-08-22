Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.IO.Compression
Imports ECMAPI.ParaVariables
Imports ECMAPI.SharedGetFunction

Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports Newtonsoft.Json
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Microsoft.VisualBasic.Logging
Imports iTextSharp.text.pdf.AcroFields
Imports Newtonsoft.Json.Linq
Imports Org.BouncyCastle.X509
Imports System.Threading.Tasks
Imports iTextSharp.text.pdf.qrcode

Namespace Controllers
    Public Class CMAPController
        Inherits ApiController

        <HttpPost>
        Public Function FileUpload(para As InsUpload) As HttpResponseMessage
            Dim resmsg As New resmessage()
            Dim res As HttpResponseMessage
            Dim inputJson = ""
            Dim logf = "upload function called.."
            Try
                logf &= "get cabinet name : " & para.CabinetName
                If getDefaultCabinetName(para.CabinetName).ToLower() = "bbk cad" Then
                    If SaveAPICallInput.ToLower = "yes" Then
                        Dim parafile = para.file
                        Dim inscopy As New InsUpload
                        inscopy = para
                        inscopy.file = Nothing
                        inputJson = JsonConvert.SerializeObject(inscopy)
                        para.file = parafile
                    Else
                        inputJson = ""
                    End If
                    inputJson = inputJson.Replace("'", "")
                    logf &= "step 1.1---Input is serialized to json format "
                    Dim strqry = "insert into eZCMAPAPICallHistory (Template,CabinetId ,TemplateId ,Status,Remarks,[RIM Number],ItemId,ParentCallId ,APIFunction,XmlFileName,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Isdeleted,inputJson ) values ('" + para.CabinetName + "',0,0,'Process','','',0,0,'Upload','','" + DateDateTimeToString(Date.Now, True) + "','',0,0,0,'" + inputJson + "')"
                    'logf &= "step1.2---  for 1st insert :" & strqry
                    Dim CallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqry)
                    logf &= "step1.3--callhistoryid : " & CallHistoryId.ToString()
                    Dim errorMsg = ""
                    'fields validation Starts
                    For Each inputFieldList In para.Fields
                        If inputFieldList.FieldName.ToLower().Trim() = "rim number" Then
                            If inputFieldList.FieldValue = "" Then
                                If errorMsg <> "" Then
                                    errorMsg += " ,RIM Number cannot be empty"
                                Else
                                    errorMsg = "RIM Number cannot be empty"
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "rim name" Then
                            If inputFieldList.FieldValue = "" Then
                                If errorMsg <> "" Then
                                    errorMsg += " ,RIM Name cannot be empty"
                                Else
                                    errorMsg = "RIM Name cannot be empty"
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "reference number" Then
                            If inputFieldList.FieldValue = "" Then
                                If errorMsg <> "" Then
                                    errorMsg += " ,Reference Number cannot be empty"
                                Else
                                    errorMsg = "Reference Number cannot be empty"
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "document type" Then
                            If inputFieldList.FieldValue = "" Then
                                If errorMsg <> "" Then
                                    errorMsg += " ,Document Type cannot be empty"
                                Else
                                    errorMsg = "Document Type cannot be empty"
                                End If
                            End If

                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "document category" Then
                            If inputFieldList.FieldValue = "" Then
                                If errorMsg <> "" Then
                                    errorMsg += " ,Document Category cannot be empty"
                                Else
                                    errorMsg = "Document Category cannot be empty"
                                End If
                            End If

                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "document filename" Then
                            If inputFieldList.FieldValue = "" Then
                                If errorMsg <> "" Then
                                    errorMsg += " ,Document Filename cannot be empty"
                                Else
                                    errorMsg = "Document Filename cannot be empty"
                                End If
                            End If


                            'ElseIf inputFieldList.FieldName.ToLower().Trim() = "document source" Then
                            '    If inputFieldList.FieldValue = "" Then
                            '        If errorMsg <> "" Then
                            '            errorMsg += " ,Document Source cannot be empty"
                            '        Else
                            '            errorMsg = "Document Source cannot be empty"
                            '        End If
                            '    End If

                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "document upload date" Then
                            If inputFieldList.FieldValue = "" Then
                                If errorMsg <> "" Then
                                    errorMsg += " ,Document Upload Date cannot be empty"
                                Else
                                    errorMsg = "Document Upload Date cannot be empty"
                                End If
                            End If

                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "document upload by" Then
                            If inputFieldList.FieldValue = "" Then
                                If errorMsg <> "" Then
                                    errorMsg += " ,Document Upload By cannot be empty"
                                Else
                                    errorMsg = "Document Upload By cannot be empty"
                                End If
                            End If
                        End If
                    Next

                    If errorMsg <> "" Then
                        'res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, errorMsg)
                        res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 -" & errorMsg)
                        Dim strqryHistory = "update eZCMAPAPICallHistory set Remarks='Error code: 1_1 - " + errorMsg + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                        Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                    End If

                    If errorMsg = "" Then
                        If para.Token <> "" Then
                            ' logf += " step2.." + CallHistoryId.ToString()
                            Dim TokenResult = DecryptToken(para.Token)
                            If TokenResult.errorCode = 1 Then
                                If IUsername <> "" AndAlso IUNCpath <> "" Then
                                    logf += " step2. entered Iusername and Iuncpath not empty"
                                    Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                                        Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
                                        logf += " step3 validated uncVal UNCPath: " + IUNCpath + "-UserName: " + IUsername + "-Domain:" + IDomain + "-Password:" + IPassword
                                        If uncval Then
                                            resmsg = CMAPUploadFn(para, TokenResult.value, CallHistoryId)
                                        Else
                                            strqry = " Update ezCMAPapicallhistory set Remarks='Error code:1_1 - UNC Credentials validation returns false', UPdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' Where CallHistoryId='" + CallHistoryId.ToString() + "'"
                                            Dim callHistoryIdUNC = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqry)
                                            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, logf + "   UNC credentials validation returns false")
                                        End If
                                    End Using
                                Else
                                    resmsg = CMAPUploadFn(para, TokenResult.value, CallHistoryId)
                                    ' resmsg.value = "Error code: 1_1 - UNC Path empty"
                                End If
                                'resmsg = CMAPUploadFn(para, TokenResult.value, CallHistoryId)
                                ' logf += " step4.." + resmsg.value
                                If resmsg.errorCode = 1 Then
                                    Dim resObj = JsonConvert.DeserializeObject(resmsg.value)
                                    res = Request.CreateResponse(HttpStatusCode.OK, resObj)
                                Else
                                    res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, resmsg.value)
                                End If
                            Else
                                res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, TokenResult.value)
                                Dim strqryHistory = "update eZCMAPAPICallHistory set Remarks='" + TokenResult.value + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                            End If
                        Else
                            res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 - Token should not be Empty")
                            Dim strqryHistory = "update eZCMAPAPICallHistory set Remarks='Error code: 1_1 - Token should not be Empty', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                            Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                        End If
                    End If
                Else
                    res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 - InValid Cabinet")
                End If


            Catch ex As Exception
                res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Catch: " + "   Log: " + logf + " " + ex.Message.ToString())
            End Try
            Return res
        End Function



        '<HttpPost>
        'Public Async Function FileUploadAsBinary() As Task(Of HttpResponseMessage)
        '    Dim resmsg As New resmessage()
        '    Dim res As HttpResponseMessage
        '    Dim inputJson = ""
        '    Dim logf = "upload function called.."
        '    Dim para As New InsUpload

        '    Try
        '        ' Check content type
        '        If Not Request.Content.Headers.ContentType.MediaType.StartsWith("multipart/") Then
        '            Return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Unsupported media type")
        '        End If
        '        logf &= "Entered after multipart"
        '        Dim provider = Await Request.Content.ReadAsMultipartAsync()
        '        logf &= "Entered Provider"
        '        Dim CabinetName As String = provider.Contents.FirstOrDefault(Function(c) c.Headers.ContentDisposition.Name.Trim(""""c) = "CabinetName").ReadAsStringAsync().Result
        '        Dim Token As String = provider.Contents.FirstOrDefault(Function(c) c.Headers.ContentDisposition.Name.Trim(""""c) = "Token").ReadAsStringAsync().Result
        '        Dim fieldsJson As String = provider.Contents.FirstOrDefault(Function(c) c.Headers.ContentDisposition.Name.Trim(""""c) = "Fields").ReadAsStringAsync().Result

        '        Dim Fields As List(Of FieldWithValues) = JsonConvert.DeserializeObject(Of List(Of FieldWithValues))(fieldsJson)
        '        Dim filetype As String = provider.Contents.FirstOrDefault(Function(c) c.Headers.ContentDisposition.Name.Trim(""""c) = "filetype").ReadAsStringAsync().Result

        '        logf &= "got filetype"
        '        para.Token = Token
        '        para.CabinetName = CabinetName
        '        para.filetype = filetype
        '        para.Fields = Fields


        '        Dim fileBytes As Byte() = Nothing

        '        For Each content As HttpContent In provider.Contents
        '            Dim contentDisposition = content.Headers.ContentDisposition
        '            If contentDisposition IsNot Nothing AndAlso Not String.IsNullOrEmpty(contentDisposition.FileName) Then
        '                fileBytes = Await content.ReadAsByteArrayAsync()
        '                Exit For
        '            End If
        '        Next



        '        If getDefaultCabinetName(para.CabinetName).ToLower() = "bbk cad" Then
        '            If SaveAPICallInput.ToLower = "yes" Then
        '                Dim parafile = para.file
        '                Dim inscopy As New InsUpload
        '                inscopy = para
        '                inscopy.fileBytes = Nothing
        '                inscopy.file = Nothing
        '                inputJson = JsonConvert.SerializeObject(inscopy)
        '                para.file = parafile
        '            Else
        '                inputJson = ""
        '            End If
        '            inputJson = inputJson.Replace("'", "")
        '            para.fileBytes = fileBytes
        '            logf &= "step 1.1---Input is serialized to json format and got filebytes "
        '            Dim strqry = "insert into eZCMAPAPICallHistory (Template,CabinetId ,TemplateId ,Status,Remarks,[RIM Number],ItemId,ParentCallId ,APIFunction,XmlFileName,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Isdeleted,inputJson ) values ('" + para.CabinetName + "',0,0,'Process','','',0,0,'Upload','','" + DateDateTimeToString(Date.Now, True) + "','',0,0,0,'" + inputJson + "')"
        '            logf &= "step1.2---  for 1st insert :" & strqry
        '            Dim CallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqry)
        '            logf &= "step1.3--callhistoryid : " & CallHistoryId.ToString()
        '            Dim errorMsg = ""
        '            'fields validation Starts
        '            For Each inputFieldList In para.Fields
        '                'cabinet Name
        '                If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "cabinetname" Then
        '                    If inputFieldList.FieldName <> "" Then
        '                    Else
        '                        If errorMsg <> "" Then
        '                            errorMsg += " ,CabinetName cannot be empty"
        '                        Else
        '                            errorMsg = "CabinetName cannot be empty"
        '                        End If
        '                    End If
        '                End If
        '                'reference Number
        '                If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "referencenumber" Then
        '                    If inputFieldList.FieldName <> "" Then
        '                    Else
        '                        If errorMsg <> "" Then
        '                            errorMsg += " ,reference Number cannot be empty"
        '                        Else
        '                            errorMsg = "reference Number cannot be empty"
        '                        End If
        '                    End If
        '                End If
        '                'Document Type
        '                If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "documenttype" Then
        '                    If inputFieldList.FieldName <> "" Then
        '                    Else
        '                        If errorMsg <> "" Then
        '                            errorMsg += " ,document Type cannot be empty"
        '                        Else
        '                            errorMsg = "document Type cannot be empty"
        '                        End If
        '                    End If
        '                End If
        '                'Document Category
        '                If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "documentcategory" Then
        '                    If inputFieldList.FieldName <> "" Then
        '                    Else
        '                        If errorMsg <> "" Then
        '                            errorMsg += " ,document Category cannot be empty"
        '                        Else
        '                            errorMsg = "document Category cannot be empty"
        '                        End If
        '                    End If
        '                End If
        '                'Document File Name
        '                If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "documentfilename" Then
        '                    If inputFieldList.FieldName <> "" Then
        '                    Else
        '                        If errorMsg <> "" Then
        '                            errorMsg += " ,document File Name cannot be empty"
        '                        Else
        '                            errorMsg = "document File Name cannot be empty"
        '                        End If
        '                    End If
        '                End If

        '                'Document Upload Date
        '                If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "documentuploaddate" Then
        '                    If inputFieldList.FieldName <> "" Then
        '                    Else
        '                        If errorMsg <> "" Then
        '                            errorMsg += " ,document Upload Date cannot be empty"
        '                        Else
        '                            errorMsg = "document Upload Date cannot be empty"
        '                        End If
        '                    End If
        '                End If

        '                'Document Upload By
        '                If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "documentuploadby" Then
        '                    If inputFieldList.FieldName <> "" Then
        '                    Else
        '                        If errorMsg <> "" Then
        '                            errorMsg += " ,document Upload By cannot be empty"
        '                        Else
        '                            errorMsg = "document Upload By cannot be empty"
        '                        End If
        '                    End If
        '                End If
        '            Next

        '            If errorMsg <> "" Then
        '                'res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, errorMsg)
        '                res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 -" & errorMsg)
        '                Dim strqryHistory = "update eZCMAPAPICallHistory set Remarks='Error code: 1_1 - " + errorMsg + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
        '                Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
        '            End If

        '            If errorMsg = "" Then
        '                If para.Token <> "" Then
        '                    logf += " step2.." + CallHistoryId.ToString()
        '                    Dim TokenResult = DecryptToken(para.Token)
        '                    If TokenResult.errorCode = 1 Then
        '                        If IUsername <> "" AndAlso IUNCpath <> "" Then
        '                            Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
        '                                Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
        '                                If uncval Then
        '                                    resmsg = CMAPUploadFnPostMan(para, TokenResult.value, CallHistoryId)
        '                                Else
        '                                    Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "UNC credentials validation returns false")
        '                                End If
        '                            End Using
        '                        End If
        '                        logf += " step4.." + resmsg.value
        '                        If resmsg.errorCode = 1 Then
        '                            Dim resObj = JsonConvert.DeserializeObject(resmsg.value)
        '                            res = Request.CreateResponse(HttpStatusCode.OK, resObj)
        '                        Else
        '                            res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, resmsg.value)
        '                        End If
        '                    Else
        '                        res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, TokenResult.value)
        '                        Dim strqryHistory = "update eZCMAPAPICallHistory set Remarks='" + TokenResult.value + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
        '                        Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
        '                    End If
        '                Else
        '                    res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 - Token should not be Empty")
        '                    Dim strqryHistory = "update eZCMAPAPICallHistory set Remarks='Error code: 1_1 - Token should not be Empty', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
        '                    Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
        '                End If
        '            End If
        '        Else
        '            res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 - InValid Cabinet")
        '        End If

        '    Catch ex As Exception
        '        res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Catch: " + logf + "    " + ex.Message.ToString())
        '    End Try
        '    Return res
        'End Function

        Private Function DecryptToken(value) As resmessage
            Dim resmsg As New resmessage()
            Try
                If value <> "" Then
                    Dim DecToken = System.Text.Encoding.UTF8.GetString(System.Web.HttpServerUtility.UrlTokenDecode(value))
                    If DecToken <> "" Then
                        Dim DecTokenArr = DecToken.Split({"#e@z"}, StringSplitOptions.RemoveEmptyEntries)
                        If Not IsNothing(DecTokenArr) AndAlso DecTokenArr.Length > 1 Then
                            Dim s = Convert.ToDateTime(DecTokenArr(2))
                            Dim expirationDateTime As DateTime = s.AddHours(24)
                            'Dim expirationDateTime As DateTime = s.AddSeconds(5)
                            If DateTime.Now > expirationDateTime Then
                                resmsg.errorCode = 3
                                resmsg.value = "Error code: 1_3 - Token Expired"
                            Else
                                resmsg.errorCode = 1
                                resmsg.value = DecTokenArr(1) 'ECMLoginId
                            End If
                        Else
                            resmsg.errorCode = 2
                            resmsg.value = "Error code: 1_2 - Invalid Token"
                        End If



                        'If Not IsNothing(DecTokenArr) AndAlso DecTokenArr.Length > 1 AndAlso Convert.ToDateTime(DecTokenArr(2)).ToString("dd-MMM-yyyy") = Now.ToString("dd-MMM-yyyy") Then
                        '    resmsg.errorCode = 1
                        '    resmsg.value = DecTokenArr(1) 'ECMLoginId
                        'Else
                        '    resmsg.errorCode = 3
                        '    resmsg.value = "Error code: 1_3 - Token Expired"
                        'End If
                    Else
                        resmsg.errorCode = 2
                        resmsg.value = "Error code: 1_2 - Invalid Token"
                    End If

                End If
            Catch ex As Exception
                resmsg.errorCode = 0
                resmsg.value = "Error code: 1_0 - " + ex.ToString()
            End Try
            Return resmsg
        End Function

        <HttpPost>
        Public Function searchAndGetURL(para As InsSearchandGetURL) As IHttpActionResult
            Dim logf = "searchandGetURL function called.."
            Dim resmsg As HttpResponseMessage
            Dim errorMsg = ""
            Dim strRimNumber = "", strCabinetName = "", strItemId = ""
            Try
                For Each inputFieldList In para.Fields
                    If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "itemid" Then
                        If inputFieldList.FieldValue <> "" Then
                            strItemId = inputFieldList.FieldValue
                            logf &= " got itemId :" & strItemId
                        Else
                            If errorMsg <> "" Then
                                errorMsg += " ,ItemId cannot be empty"
                            Else
                                errorMsg = "ItemId cannot be empty"
                            End If
                        End If
                    End If
                    'If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "rimnumber" Then
                    '    If inputFieldList.FieldValue <> "" Then
                    '        strRimNumber = inputFieldList.FieldValue
                    '        logf &= " got rimnumber :" & strRimNumber
                    '    Else
                    '        If errorMsg <> "" Then
                    '            errorMsg += " ,RIM Number cannot be empty"
                    '        Else
                    '            errorMsg = "RIM Number cannot be empty"
                    '        End If
                    '    End If
                    'End If
                    'reference Number
                    'If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "referencenumber" Then
                    '    If inputFieldList.FieldValue <> "" Then
                    '    Else
                    '        If errorMsg <> "" Then
                    '            errorMsg += " ,reference Number cannot be empty"
                    '        Else
                    '            errorMsg = "reference Number cannot be empty"
                    '        End If
                    '    End If
                    'End If
                Next
                If para.Token = "" Then
                    If errorMsg <> "" Then
                        errorMsg += " ,Token cannot be empty"
                    Else
                        errorMsg = "Token cannot be empty"
                    End If
                End If
                If para.CabinetName = "" Then
                    If errorMsg <> "" Then
                        errorMsg += " ,cabinetName cannot be empty"
                    Else
                        errorMsg = "cabinetName cannot be empty"
                    End If
                Else
                    strCabinetName = para.CabinetName
                End If
                If errorMsg = "" Then
                    Dim TokenResult = DecryptToken(para.Token)
                    If TokenResult.errorCode = 1 Then
                        If IUsername <> "" AndAlso IUNCpath <> "" Then
                            Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                                Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
                                If uncval Then
                                    Dim StrQry As String = "select templateId from ezTemplate where templateName ='" & para.CabinetName & "' and isdeleted=0"
                                    Dim ds_Cabinet = GetDatasetByQuery(StrQry)
                                    If ds_Cabinet IsNot Nothing AndAlso ds_Cabinet.Tables.Count > 0 AndAlso ds_Cabinet.Tables(0).Rows.Count > 0 Then
                                        Dim templateId As String = ds_Cabinet.Tables(0).Rows(0)("templateId").ToString()
                                        logf &= " Got TemplateId"

                                        Dim templateList = SelectedeZTemplateList("TemplateId", templateId)

                                        StrQry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype,[Document Type],encodeType from ezca_3_" & templateId & "_items i left join eZERSInfo e on e.ERSId=i.ERSId where i.itemId=" & strItemId & ""
                                        Dim ds = GetDatasetByQuery(StrQry)
                                        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                                            Dim filePath As String = Path.Combine(ds.Tables(0).Rows(0)("DirPath").ToString, ds.Tables(0).Rows(0)("ifilepath").ToString, Path.GetFileNameWithoutExtension(ds.Tables(0).Rows(0)("ifilename").ToString) + ".ezo")
                                            logf &= " got filepath"
                                            If File.Exists(filePath) Then
                                                Dim TempFilepath As String = "C:\\TempStorage\\" & strItemId
                                                logf &= "got tempfilepath"
                                                If (Not System.IO.Directory.Exists(TempFilepath)) Then
                                                    System.IO.Directory.CreateDirectory(TempFilepath)
                                                End If
                                                Dim localfile As String = System.IO.Path.Combine(TempFilepath, Path.GetFileNameWithoutExtension(ds.Tables(0).Rows(0)("ifilename").ToString) + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + ".ezo")
                                                logf &= "created local file"
                                                File.Copy(filePath, localfile, True)
                                                If File.Exists(localfile) Then
                                                    Dim strInputFile = localfile
                                                    Dim strOutputFile = localfile.Replace(".ezo", "." + ds.Tables(0).Rows(0)("ifiletype").ToString())
                                                    logf &= "got stroutputfile"
                                                    Dim bytKey As Byte()
                                                    Dim bytIV As Byte()
                                                    bytKey = CreateKey("3z0f1s$ecm")
                                                    bytIV = CreateIV("3z0f1s$ecm")
                                                    Dim resulte As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionDecrypt)
                                                    If resulte = "Decryption Complete" Then
                                                        Dim bytes As Byte() = File.ReadAllBytes(strOutputFile)
                                                        Dim encodeType As String = "", finalBase64String = ""
                                                        'StrQry = "select encodeType from ezCMAPapicallhistory where itemid=" + strItemId + " and cabinetId=3 and templateId=" + templateId + " and isdeleted=0 "
                                                        'Dim ds_encodeType = GetDatasetByQuery(StrQry)
                                                        'If ds_encodeType IsNot Nothing AndAlso ds_encodeType.Tables.Count > 0 AndAlso ds_encodeType.Tables(0).Rows.Count > 0 Then
                                                        '    encodeType = ds_encodeType.Tables(0).Rows(0)(0).ToString()
                                                        'End If
                                                        encodeType = ds.Tables(0).Rows(0)("encodeType").ToString()
                                                        Dim base64string As String = Convert.ToBase64String(bytes)
                                                        If encodeType = 1 Then
                                                            finalBase64String = base64string
                                                        ElseIf encodeType = 2 Then
                                                            Dim utf8Bytes As Byte() = Encoding.UTF8.GetBytes(base64string)
                                                            finalBase64String = Convert.ToBase64String(utf8Bytes)
                                                        End If
                                                        logf &= " got base64string"
                                                        Dim jsonObj = New With {
                                                        .File = finalBase64String
                                                        }
                                                        File.Delete(strOutputFile)
                                                        logf &= "file got deleted"
                                                        Try
                                                            Directory.Delete(Path.GetDirectoryName(strOutputFile))
                                                            If Directory.Exists(Path.GetDirectoryName(Path.GetDirectoryName(strOutputFile))) Then
                                                                Directory.Delete(Path.GetDirectoryName(Path.GetDirectoryName(strOutputFile)))
                                                            End If
                                                        Catch ex As Exception
                                                        End Try
                                                        logf &= "directory got deleted"
                                                        Return Ok(jsonObj)
                                                    Else
                                                        Return BadRequest("Error in encyrption")
                                                    End If
                                                Else
                                                    Return BadRequest("File Not exists in path " & localfile)
                                                End If
                                            Else
                                                Return BadRequest("File Not exists in path " & filePath)
                                            End If
                                        Else
                                            Return BadRequest("No record exists for Itemid " & strItemId)
                                        End If
                                    Else
                                        Return BadRequest("Template  does not exists for cabinet " & para.CabinetName)
                                    End If
                                Else
                                    Return BadRequest("UNC Credentials not get validated")
                                End If
                            End Using
                        Else
                            Dim StrQry As String = "select templateId from ezTemplate where templateName ='" & para.CabinetName & "' and isdeleted=0"
                            Dim ds_Cabinet = GetDatasetByQuery(StrQry)
                            If ds_Cabinet IsNot Nothing AndAlso ds_Cabinet.Tables.Count > 0 AndAlso ds_Cabinet.Tables(0).Rows.Count > 0 Then
                                Dim templateId As String = ds_Cabinet.Tables(0).Rows(0)("templateId").ToString()
                                logf &= " Got TemplateId"

                                Dim templateList = SelectedeZTemplateList("TemplateId", templateId)

                                StrQry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype,[Document Type],encodeType from ezca_3_" & templateId & "_items i left join eZERSInfo e on e.ERSId=i.ERSId where i.itemId=" & strItemId & ""
                                Dim ds = GetDatasetByQuery(StrQry)
                                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                                    Dim filePath As String = Path.Combine(ds.Tables(0).Rows(0)("DirPath").ToString, ds.Tables(0).Rows(0)("ifilepath").ToString, Path.GetFileNameWithoutExtension(ds.Tables(0).Rows(0)("ifilename").ToString) + ".ezo")
                                    logf &= " got filepath"
                                    If File.Exists(filePath) Then
                                        Dim TempFilepath As String = "C:\\TempStorage\\" & strItemId
                                        logf &= "got tempfilepath"
                                        If (Not System.IO.Directory.Exists(TempFilepath)) Then
                                            System.IO.Directory.CreateDirectory(TempFilepath)
                                        End If
                                        Dim localfile As String = System.IO.Path.Combine(TempFilepath, Path.GetFileNameWithoutExtension(ds.Tables(0).Rows(0)("ifilename").ToString) + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + ".ezo")
                                        logf &= "created local file"
                                        File.Copy(filePath, localfile, True)
                                        If File.Exists(localfile) Then
                                            Dim strInputFile = localfile
                                            Dim strOutputFile = localfile.Replace(".ezo", "." + ds.Tables(0).Rows(0)("ifiletype").ToString())
                                            logf &= "got stroutputfile"
                                            Dim bytKey As Byte()
                                            Dim bytIV As Byte()
                                            bytKey = CreateKey("3z0f1s$ecm")
                                            bytIV = CreateIV("3z0f1s$ecm")
                                            Dim resulte As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionDecrypt)
                                            If resulte = "Decryption Complete" Then
                                                Dim bytes As Byte() = File.ReadAllBytes(strOutputFile)
                                                Dim encodeType As String = "", finalBase64String = ""
                                                'StrQry = "select encodeType from ezCMAPapicallhistory where itemid=" + strItemId + " and cabinetId=3 and templateId=" + templateId + " and isdeleted=0 "
                                                'Dim ds_encodeType = GetDatasetByQuery(StrQry)
                                                'If ds_encodeType IsNot Nothing AndAlso ds_encodeType.Tables.Count > 0 AndAlso ds_encodeType.Tables(0).Rows.Count > 0 Then
                                                '    encodeType = ds_encodeType.Tables(0).Rows(0)(0).ToString()
                                                'End If
                                                encodeType = ds.Tables(0).Rows(0)("encodeType").ToString()
                                                Dim base64string As String = Convert.ToBase64String(bytes)
                                                If encodeType = 1 Then
                                                    finalBase64String = base64string
                                                ElseIf encodeType = 2 Then
                                                    Dim utf8Bytes As Byte() = Encoding.UTF8.GetBytes(base64string)
                                                    finalBase64String = Convert.ToBase64String(utf8Bytes)
                                                End If
                                                logf &= " got base64string"
                                                Dim jsonObj = New With {
                                                .File = finalBase64String
                                                }
                                                File.Delete(strOutputFile)
                                                logf &= "file got deleted"
                                                Try
                                                    Directory.Delete(Path.GetDirectoryName(strOutputFile))
                                                    If Directory.Exists(Path.GetDirectoryName(Path.GetDirectoryName(strOutputFile))) Then
                                                        Directory.Delete(Path.GetDirectoryName(Path.GetDirectoryName(strOutputFile)))
                                                    End If
                                                Catch ex As Exception
                                                End Try
                                                logf &= "directory got deleted"
                                                Return Ok(jsonObj)
                                            Else
                                                Return BadRequest("Error in encyrption")
                                            End If
                                        Else
                                            Return BadRequest("File Not exists in path " & localfile)
                                        End If
                                    Else
                                        Return BadRequest("File Not exists in path " & filePath)
                                    End If
                                Else
                                    Return BadRequest("No record exists for Itemid " & strItemId)
                                End If
                            Else
                                Return BadRequest("Template  does not exists for cabinet " & para.CabinetName)
                            End If
                            ' Return BadRequest("UNC Credentials empty")
                        End If

                    Else
                        Return BadRequest(TokenResult.value)
                    End If
                Else
                    Return BadRequest(errorMsg)
                End If

            Catch ex As Exception
                Return BadRequest(logf & ex.Message.ToString())
            End Try
            Return StatusCode(204)

        End Function


        '===========================================SearchAndGetURL======================================================
        '<HttpPost>
        'Public Function searchAndGetURL(para As InsSearchandGetURL) As IHttpActionResult
        '    Dim logf = "searchandGetURL function called.."
        '    Dim errorMsg = ""
        '    Dim strRimNumber = "", strCabinetName = "", strItemId = ""
        '    Try
        '        For Each inputFieldList In para.Fields
        '            If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "itemid" Then
        '                If inputFieldList.FieldValue <> "" Then
        '                    strRimNumber = inputFieldList.FieldValue
        '                    logf &= " got rimnumber :" & strRimNumber
        '                Else
        '                    If errorMsg <> "" Then
        '                        errorMsg += " ,RIM Number cannot be empty"
        '                    Else
        '                        errorMsg = "RIM Number cannot be empty"
        '                    End If
        '                End If
        '            End If
        '            If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "rimnumber" Then
        '                If inputFieldList.FieldValue <> "" Then
        '                    strRimNumber = inputFieldList.FieldValue
        '                    logf &= " got rimnumber :" & strRimNumber
        '                Else
        '                    If errorMsg <> "" Then
        '                        errorMsg += " ,RIM Number cannot be empty"
        '                    Else
        '                        errorMsg = "RIM Number cannot be empty"
        '                    End If
        '                End If
        '            End If
        '            'reference Number
        '            'If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "referencenumber" Then
        '            '    If inputFieldList.FieldValue <> "" Then
        '            '    Else
        '            '        If errorMsg <> "" Then
        '            '            errorMsg += " ,reference Number cannot be empty"
        '            '        Else
        '            '            errorMsg = "reference Number cannot be empty"
        '            '        End If
        '            '    End If
        '            'End If
        '        Next
        '        If para.Token = "" Then
        '            If errorMsg <> "" Then
        '                errorMsg += " ,Token cannot be empty"
        '            Else
        '                errorMsg = "Token cannot be empty"
        '            End If
        '        End If
        '        If para.CabinetName = "" Then
        '            If errorMsg <> "" Then
        '                errorMsg += " ,cabinetName cannot be empty"
        '            Else
        '                errorMsg = "cabinetName cannot be empty"
        '            End If
        '        Else
        '            strCabinetName = para.CabinetName
        '        End If
        '        If errorMsg = "" Then
        '            Dim TokenResult = DecryptToken(para.Token)
        '            If TokenResult.errorCode = 1 Then
        '                If IUsername <> "" AndAlso IUNCpath <> "" Then
        '                    Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
        '                        Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
        '                        If uncval Then
        '                            Dim StrQry As String = "select templateId from ezTemplate where templateName ='" & strCabinetName & "' and isdeleted=0"
        '                            Dim ds_Cabinet = GetDatasetByQuery(StrQry)
        '                            If ds_Cabinet IsNot Nothing AndAlso ds_Cabinet.Tables.Count > 0 AndAlso ds_Cabinet.Tables(0).Rows.Count > 0 Then
        '                                Dim templateId As String = ds_Cabinet.Tables(0).Rows(0)("templateId").ToString()
        '                                StrQry = "select itemid from ezca_3_" & templateId & "_items where [rim number]='" + strRimNumber + "' and isdeleted=0"
        '                                Dim ds = GetDatasetByQuery(StrQry)
        '                                Dim itemIds = New List(Of Object)
        '                                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
        '                                    For Each dr As DataRow In ds.Tables(0).Rows
        '                                        Dim itemId As String = dr("itemId").ToString()
        '                                        itemIds.Add(New With {
        '                                            .itemId = itemId, .URL = APIurl & "/CMAP/viewFile/CAD/" & itemId})
        '                                    Next
        '                                    Dim jsonResult = JsonConvert.SerializeObject(itemIds)
        '                                    logf &= " got jsonresult " & jsonResult
        '                                    Return Ok(jsonResult)
        '                                End If
        '                            End If
        '                        End If
        '                    End Using
        '                Else
        '                    Return BadRequest("UNC Credentials wrong")
        '                End If
        '            Else
        '                Return BadRequest(TokenResult.value)
        '            End If
        '        Else
        '            Return BadRequest("Error code: 1_1 -" & errorMsg)
        '        End If

        '    Catch ex As Exception
        '        Return BadRequest(ex.Message.ToString())
        '    End Try
        '    Return StatusCode(HttpStatusCode.NoContent)

        'End Function

        '===========================viewfile========================================================
        <HttpGet>
        <Route("v1/CMAP/viewfile/{cabinetName}/{itemid}")>
        Public Function viewFile(cabinetName As String, itemId As Integer) As HttpResponseMessage
            Dim resmsg As HttpResponseMessage
            Dim logf As String = "View file function called...."
            Try
                ' If Request.Headers.Contains("Token") Then
                'Dim token As String = Request.Headers.GetValues("Token").FirstOrDefault()
                'Dim TokenResult = DecryptToken(token)
                ' If TokenResult.errorCode = 1 Then

                Dim contentTypes As New Dictionary(Of String, String) From {
    {".txt", "text/plain"},
    {".doc", "application/msword"},
    {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
    {".pdf", "application/pdf"},
    {".odt", "application/vnd.oasis.opendocument.text"},
    {".rtf", "application/rtf"},
    {".wpd", "application/vnd.wordperfect"},
    {".xls", "application/vnd.ms-excel"},
    {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
    {".csv", "text/csv"},
    {".ppt", "application/vnd.ms-powerpoint"},
    {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
    {".jpg", "image/jpeg"},
    {".jpeg", "image/jpeg"},
    {".png", "image/png"},
    {".gif", "image/gif"},
    {".bmp", "image/bmp"},
    {".tiff", "image/tiff"},
    {".tif", "image/tiff"},
    {".svg", "image/svg+xml"},
    {".mp3", "audio/mpeg"},
    {".wav", "audio/wav"},
    {".flac", "audio/flac"},
    {".aac", "audio/aac"},
    {".ogg", "audio/ogg"},
    {".m4a", "audio/mp4"},
    {".wma", "audio/x-ms-wma"},
    {".mp4", "video/mp4"},
    {".avi", "video/x-msvideo"},
    {".mkv", "video/x-matroska"},
    {".mov", "video/quicktime"},
    {".flv", "video/x-flv"},
    {".wmv", "video/x-ms-wmv"},
    {".webm", "video/webm"},
    {".mpeg", "video/mpeg"},
    {".zip", "application/zip"},
    {".rar", "application/x-rar-compressed"},
    {".7z", "application/x-7z-compressed"},
    {".tar", "application/x-tar"},
    {".gz", "application/gzip"},
    {".bz2", "application/x-bzip2"},
    {".iso", "application/x-iso9660-image"},
    {".exe", "application/octet-stream"},
    {".bat", "application/x-msdownload"},
    {".msi", "application/x-msdownload"},
    {".sh", "application/x-sh"},
    {".bin", "application/octet-stream"},
    {".cmd", "application/octet-stream"},
    {".html", "text/html"},
    {".htm", "text/html"},
    {".css", "text/css"},
    {".js", "application/javascript"},
    {".php", "application/x-httpd-php"},
    {".py", "text/x-python"},
    {".rb", "application/x-ruby"},
    {".java", "text/x-java-source"},
    {".c", "text/x-c"},
    {".cpp", "text/x-c"},
    {".cs", "text/plain"},
    {".vb", "text/plain"},
    {".xml", "application/xml"},
    {".json", "application/json"},
    {".yml", "application/x-yaml"},
    {".sql", "application/sql"},
    {".db", "application/x-sqlite3"},
    {".sqlite", "application/x-sqlite3"},
    {".accdb", "application/msaccess"},
    {".mdb", "application/msaccess"},
    {".dbf", "application/dbase"},
    {".dll", "application/octet-stream"},
    {".sys", "application/octet-stream"},
    {".ini", "text/plain"},
    {".log", "text/plain"},
    {".cfg", "text/plain"},
    {".dat", "application/octet-stream"}
}

                Dim StrQry As String = "select templateId from ezTemplate where templateName ='" & cabinetName & "' and isdeleted=0"
                Dim ds_Cabinet = GetDatasetByQuery(StrQry)
                If ds_Cabinet IsNot Nothing AndAlso ds_Cabinet.Tables.Count > 0 AndAlso ds_Cabinet.Tables(0).Rows.Count > 0 Then
                    Dim templateId As String = ds_Cabinet.Tables(0).Rows(0)("templateId").ToString()

                    Dim templateList = SelectedeZTemplateList("TemplateId", templateId)

                    StrQry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype,[Document Type] from ezca_3_" & templateId & "_items i left join eZERSInfo e on e.ERSId=i.ERSId where i.itemId=" & itemId & " and i.isdeleted=0"
                    Dim ds = GetDatasetByQuery(StrQry)
                    If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                        'Dim filePath As String = Path.Combine(ds.Tables(0).Rows(0)("DirPath").ToString, templateList(0).CabinetName, templateList(0).TemplateName, ds.Tables(0).Rows(0)("ifilepath").ToString, Path.GetFileNameWithoutExtension(ds.Tables(0).Rows(0)("ifilename").ToString) + ".ezo")
                        Dim filePath As String = Path.Combine(ds.Tables(0).Rows(0)("DirPath").ToString, ds.Tables(0).Rows(0)("ifilepath").ToString, Path.GetFileNameWithoutExtension(ds.Tables(0).Rows(0)("ifilename").ToString) + ".ezo")
                        If IUsername <> "" AndAlso IUNCpath <> "" Then
                            Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                                Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
                                If uncval Then
                                    If File.Exists(filePath) Then
                                        Dim TempFilepath As String = "C:\\TempStorage\\" & itemId
                                        If (Not System.IO.Directory.Exists(TempFilepath)) Then
                                            System.IO.Directory.CreateDirectory(TempFilepath)
                                        End If
                                        logf &= "got tempfilepath "
                                        Dim localfile As String = System.IO.Path.Combine(TempFilepath, Path.GetFileNameWithoutExtension(ds.Tables(0).Rows(0)("ifilename").ToString) + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + ".ezo")
                                        File.Copy(filePath, localfile, True)
                                        logf &= " file copied to localfile " & localfile
                                        If File.Exists(localfile) Then
                                            Dim strInputFile = localfile
                                            Dim strOutputFile = localfile.Replace(".ezo", "." + ds.Tables(0).Rows(0)("ifiletype").ToString)
                                            Dim bytKey As Byte()
                                            Dim bytIV As Byte()
                                            bytKey = CreateKey("3z0f1s$ecm")
                                            bytIV = CreateIV("3z0f1s$ecm")
                                            Dim resulte As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionDecrypt)
                                            logf &= " Entered decrypting the file  " & localfile
                                            If resulte = "Decryption Complete" Then
                                                Try
                                                    If (System.IO.File.Exists(strOutputFile)) Then
                                                        'If strOutputFile.EndsWith("pdf") Then
                                                        '    HttpContext.Current.Response.ContentType = "application/pdf"
                                                        'ElseIf strOutputFile.EndsWith("jpg") Or strOutputFile.EndsWith("jpeg") Then
                                                        '    HttpContext.Current.Response.ContentType = "image/jpeg"
                                                        'ElseIf strOutputFile.EndsWith("png") Then
                                                        '    HttpContext.Current.Response.ContentType = "image/png"
                                                        'ElseIf strOutputFile.EndsWith("html") Then
                                                        '    HttpContext.Current.Response.ContentType = "Text/HTML"
                                                        'ElseIf strOutputFile.EndsWith("txt") Then
                                                        '    HttpContext.Current.Response.ContentType = "text/plain"
                                                        'ElseIf strOutputFile.EndsWith("xml") Then
                                                        '    HttpContext.Current.Response.ContentType = "text/xml"
                                                        'ElseIf strOutputFile.EndsWith("doc") Then
                                                        '    HttpContext.Current.Response.ContentType = "application/vnd.ms-word"
                                                        'ElseIf strOutputFile.EndsWith("docx") Then
                                                        '    HttpContext.Current.Response.ContentType = "application/vnd.ms-word"
                                                        'ElseIf strOutputFile.EndsWith("xls") Then
                                                        '    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
                                                        'ElseIf strOutputFile.EndsWith("xlsx") Then
                                                        '    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
                                                        'ElseIf strOutputFile.EndsWith("msg") Then
                                                        '    HttpContext.Current.Response.ContentType = "application/vnd.ms-outlook"
                                                        'ElseIf strOutputFile.EndsWith("ppt") Then
                                                        '    HttpContext.Current.Response.ContentType = "application/vnd.ms-powerpoint"
                                                        'ElseIf strOutputFile.EndsWith("pptx") Then
                                                        '    HttpContext.Current.Response.ContentType = "application/vnd.ms-powerpoint"
                                                        'ElseIf strOutputFile.EndsWith("gif") Then
                                                        '    HttpContext.Current.Response.ContentType = "image/gif"
                                                        'ElseIf strOutputFile.EndsWith("tif") Or strOutputFile.EndsWith("tiff") Then
                                                        '    HttpContext.Current.Response.ContentType = "image/tif"
                                                        'ElseIf strOutputFile.EndsWith("mp4") Or strOutputFile.EndsWith("MPEG-4") Then
                                                        '    HttpContext.Current.Response.ContentType = "video/mp4"
                                                        'End If

                                                        Dim extension As String = System.IO.Path.GetExtension(strOutputFile)
                                                        If contentTypes.ContainsKey(extension) Then
                                                            HttpContext.Current.Response.ContentType = contentTypes(extension)
                                                        End If

                                                        Dim attach = "attachment;filename=" + System.IO.Path.GetFileName(strOutputFile)
                                                        ' HttpContext.Current.Response.Headers.Add("Content-Disposition", "inline;filename=" + System.IO.Path.GetFileNameWithoutExtension(strOutputFile))
                                                        HttpContext.Current.Response.Headers.Add("Content-Disposition", "inline;filename=" + System.IO.Path.GetFileName(strOutputFile))
                                                    End If
                                                Catch ex As Exception
                                                Finally
                                                    logf &= "Entered converting file to bytes:  " & strOutputFile
                                                    Dim Buffer As Byte() = File.ReadAllBytes(strOutputFile)
                                                    If Buffer IsNot Nothing Then
                                                        File.Delete(strOutputFile)
                                                        Try
                                                            Directory.Delete(Path.GetDirectoryName(strOutputFile))
                                                            If Directory.Exists(Path.GetDirectoryName(Path.GetDirectoryName(strOutputFile))) Then
                                                                Directory.Delete(Path.GetDirectoryName(Path.GetDirectoryName(strOutputFile)))
                                                            End If
                                                        Catch ex As Exception

                                                        End Try
                                                        logf &= " file got converted to bytes"
                                                        HttpContext.Current.Response.AddHeader("content-length", Buffer.Length.ToString())
                                                        HttpContext.Current.Response.BinaryWrite(Buffer)
                                                        HttpContext.Current.Response.Flush()
                                                        HttpContext.Current.Response.End()
                                                    Else
                                                        resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error in encryption")
                                                    End If
                                                End Try
                                            Else
                                                resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error in encryption")
                                            End If
                                        Else
                                            resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File Not exists" & localfile)
                                        End If
                                    Else
                                        resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File Not exists" & filePath)
                                    End If
                                Else
                                    resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "UNC Credentials not get validated")
                                End If
                            End Using
                        Else
                            If File.Exists(filePath) Then
                                Dim TempFilepath As String = "C:\\TempStorage\\" & itemId
                                If (Not System.IO.Directory.Exists(TempFilepath)) Then
                                    System.IO.Directory.CreateDirectory(TempFilepath)
                                End If
                                logf &= "got tempfilepath "
                                Dim localfile As String = System.IO.Path.Combine(TempFilepath, Path.GetFileNameWithoutExtension(ds.Tables(0).Rows(0)("ifilename").ToString) + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + ".ezo")
                                File.Copy(filePath, localfile, True)
                                logf &= " file copied to localfile " & localfile
                                If File.Exists(localfile) Then
                                    Dim strInputFile = localfile
                                    Dim strOutputFile = localfile.Replace(".ezo", "." + ds.Tables(0).Rows(0)("ifiletype").ToString)
                                    Dim bytKey As Byte()
                                    Dim bytIV As Byte()
                                    bytKey = CreateKey("3z0f1s$ecm")
                                    bytIV = CreateIV("3z0f1s$ecm")
                                    Dim resulte As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionDecrypt)
                                    logf &= " Entered decrypting the file  " & localfile
                                    If resulte = "Decryption Complete" Then
                                        Try
                                            If (System.IO.File.Exists(strOutputFile)) Then
                                                'If strOutputFile.EndsWith("pdf") Then
                                                '    HttpContext.Current.Response.ContentType = "application/pdf"
                                                'ElseIf strOutputFile.EndsWith("jpg") Or strOutputFile.EndsWith("jpeg") Then
                                                '    HttpContext.Current.Response.ContentType = "image/jpeg"
                                                'ElseIf strOutputFile.EndsWith("png") Then
                                                '    HttpContext.Current.Response.ContentType = "image/png"
                                                'ElseIf strOutputFile.EndsWith("html") Then
                                                '    HttpContext.Current.Response.ContentType = "Text/HTML"
                                                'ElseIf strOutputFile.EndsWith("txt") Then
                                                '    HttpContext.Current.Response.ContentType = "text/plain"
                                                'ElseIf strOutputFile.EndsWith("xml") Then
                                                '    HttpContext.Current.Response.ContentType = "text/xml"
                                                'ElseIf strOutputFile.EndsWith("doc") Then
                                                '    HttpContext.Current.Response.ContentType = "application/vnd.ms-word"
                                                'ElseIf strOutputFile.EndsWith("docx") Then
                                                '    HttpContext.Current.Response.ContentType = "application/vnd.ms-word"
                                                'ElseIf strOutputFile.EndsWith("xls") Then
                                                '    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
                                                'ElseIf strOutputFile.EndsWith("xlsx") Then
                                                '    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
                                                'ElseIf strOutputFile.EndsWith("msg") Then
                                                '    HttpContext.Current.Response.ContentType = "application/vnd.ms-outlook"
                                                'ElseIf strOutputFile.EndsWith("ppt") Then
                                                '    HttpContext.Current.Response.ContentType = "application/vnd.ms-powerpoint"
                                                'ElseIf strOutputFile.EndsWith("pptx") Then
                                                '    HttpContext.Current.Response.ContentType = "application/vnd.ms-powerpoint"
                                                'ElseIf strOutputFile.EndsWith("gif") Then
                                                '    HttpContext.Current.Response.ContentType = "image/gif"
                                                'ElseIf strOutputFile.EndsWith("tif") Or strOutputFile.EndsWith("tiff") Then
                                                '    HttpContext.Current.Response.ContentType = "image/tif"
                                                'ElseIf strOutputFile.EndsWith("mp4") Or strOutputFile.EndsWith("MPEG-4") Then
                                                '    HttpContext.Current.Response.ContentType = "video/mp4"
                                                'End If

                                                Dim extension As String = System.IO.Path.GetExtension(strOutputFile)
                                                If contentTypes.ContainsKey(extension) Then
                                                    HttpContext.Current.Response.ContentType = contentTypes(extension)
                                                End If

                                                Dim attach = "attachment;filename=" + System.IO.Path.GetFileName(strOutputFile)
                                                'HttpContext.Current.Response.Headers.Add("Content-Disposition", "inline;filename=" + System.IO.Path.GetFileNameWithoutExtension(strOutputFile))
                                                HttpContext.Current.Response.Headers.Add("Content-Disposition", "inline;filename=" + System.IO.Path.GetFileName(strOutputFile))
                                            End If
                                        Catch ex As Exception
                                        Finally
                                            logf &= "Entered converting file to bytes:  " & strOutputFile
                                            Dim Buffer As Byte() = File.ReadAllBytes(strOutputFile)
                                            If Buffer IsNot Nothing Then
                                                File.Delete(strOutputFile)
                                                Try
                                                    Directory.Delete(Path.GetDirectoryName(strOutputFile))
                                                    If Directory.Exists(Path.GetDirectoryName(Path.GetDirectoryName(strOutputFile))) Then
                                                        Directory.Delete(Path.GetDirectoryName(Path.GetDirectoryName(strOutputFile)))
                                                    End If
                                                Catch ex As Exception

                                                End Try
                                                logf &= " file got converted to bytes"
                                                HttpContext.Current.Response.AddHeader("content-length", Buffer.Length.ToString())
                                                HttpContext.Current.Response.BinaryWrite(Buffer)
                                                HttpContext.Current.Response.Flush()
                                                HttpContext.Current.Response.End()
                                            Else
                                                resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error in encryption")
                                            End If
                                        End Try
                                    Else
                                        resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error in encryption")
                                    End If
                                Else
                                    resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File Not exists" & localfile)
                                End If
                            Else
                                resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File Not exists" & filePath)
                            End If
                            'resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, " UNC Credentials empty ")
                        End If
                    Else
                        resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, " No record exists ")
                    End If
                Else
                    resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "cabinet does not exists ")
                End If
                'Else
                '    resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, TokenResult.value)
                'End If
                'Else
                '    resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Token is missing ")
                'End If

            Catch ex As Exception
                resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, logf & ex.Message.ToString())
            End Try
            Return resmsg
        End Function

        <HttpPost>
        Public Function MasterEntryForCAD(para As InsMasterEntry) As HttpResponseMessage
            Dim resMsg As HttpResponseMessage
            Dim errorMsg As String = "", _lstcnt = 0
            Try
                If para.Token = "" Then
                    If errorMsg <> "" Then
                        errorMsg += " ,Token cannot be empty"
                    Else
                        errorMsg = "Token cannot be empty"
                    End If
                End If
                If para.Fields.Count = 2 Then
                    For Each inputFieldList In para.Fields
                        If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "documenttype" Then
                            _lstcnt = _lstcnt + 1
                            If inputFieldList.FieldValue <> "" Then

                            Else
                                If errorMsg <> "" Then
                                    errorMsg += " ,Document Type cannot be empty"
                                Else
                                    errorMsg = "Document Type cannot be empty"
                                End If
                            End If

                        End If
                        If inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "documentcategory" Then
                            _lstcnt = _lstcnt + 1
                            If inputFieldList.FieldValue <> "" Then

                            Else
                                If errorMsg <> "" Then
                                    errorMsg += " ,Document Category cannot be empty"
                                Else
                                    errorMsg = "Document Category cannot be empty"
                                End If
                            End If
                        End If
                    Next
                Else
                    If errorMsg <> "" Then
                        errorMsg += " ,Fields are missing  "
                    Else
                        errorMsg = "Fields are missing "
                    End If
                End If
                If _lstcnt <> 2 Then
                    If errorMsg <> "" Then
                        errorMsg += " ,Fields are missing  "
                    Else
                        errorMsg = "Fields are missing "
                    End If
                End If

                If errorMsg = "" Then
                    Dim TokenResult = DecryptToken(para.Token)
                    If TokenResult.errorCode = 1 Then
                        Dim idcnt As Integer = 0
                        Dim strQry As String = ""
                        Dim lst As List(Of FieldWithValues) = para.Fields
                        strQry = "Insert into eZDocument_category([Document_type], [Document_category], [Ez_status], [createdon]) values ("
                        For Each fieldwithvaluelst In para.Fields
                            If fieldwithvaluelst.FieldName = "Document Type" Then
                                strQry &= " '" & fieldwithvaluelst.FieldValue & "',"
                            ElseIf fieldwithvaluelst.FieldName = "Document Category" Then
                                strQry &= " '" & fieldwithvaluelst.FieldValue & "',"
                            End If
                        Next
                        strQry &= " 'Active',GETDATE())"
                        Dim rowcnt As Integer = InsertAndUpdateAndDeleteeZUserDefined(strQry)
                        If (rowcnt > 0) Then
                            Return Request.CreateResponse(HttpStatusCode.OK, "Success")
                        End If
                    Else
                        Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, TokenResult.value)
                    End If
                Else
                    Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMsg)
                End If

            Catch ex As Exception
                Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message.ToString())
            End Try
            Return resMsg
        End Function






    End Class

End Namespace