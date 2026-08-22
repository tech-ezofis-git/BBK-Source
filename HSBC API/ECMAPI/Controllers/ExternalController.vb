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

Namespace Controllers
    Public Class ExternalController
        Inherits ApiController

        <HttpPost>
        Public Function GetToken(para As InsGetToken) As HttpResponseMessage
            Dim res As HttpResponseMessage
            Try
                Dim ECMLogininfo As New OldeZECMLogin
                ECMLogininfo = SharedGetFunction.UserLogin(para.LoginName, para.Password)
                If Not IsNothing(ECMLogininfo) Then
                    Dim ConvString = "Ez#e@z" & ECMLogininfo.ECMLoginId.ToString & "#e@z" & Date.Now.ToString("dd-MMM-yyyy hh:mm:ss tt")
                    Dim EncToken = System.Web.HttpServerUtility.UrlTokenEncode(System.Text.Encoding.UTF8.GetBytes(ConvString))
                    If NewAPI = "true" Then
                        Dim TokenRes = New With {.Token = EncToken}
                        res = Request.CreateResponse(HttpStatusCode.OK, TokenRes)
                    Else
                        res = Request.CreateResponse(HttpStatusCode.OK, EncToken)
                    End If
                Else
                    res = Request.CreateErrorResponse(HttpStatusCode.Conflict, "Incorrect Username or Password")
                End If
                Return res
            Catch ex As Exception
                Throw New FaultException("GetToken : " + ex.ToString())
            End Try
        End Function

        Function DecryptToken(value) As resmessage
            Dim resmsg As New resmessage()
            Try
                If value <> "" Then
                    Dim DecToken = System.Text.Encoding.UTF8.GetString(System.Web.HttpServerUtility.UrlTokenDecode(value))
                    If DecToken <> "" Then
                        Dim DecTokenArr = DecToken.Split({"#e@z"}, StringSplitOptions.RemoveEmptyEntries)
                        Dim s = Convert.ToDateTime(DecTokenArr(2))
                        If Not IsNothing(DecTokenArr) AndAlso DecTokenArr.Length > 1 AndAlso Convert.ToDateTime(DecTokenArr(2)).ToString("dd-MMM-yyyy") = Now.ToString("dd-MMM-yyyy") Then
                            resmsg.errorCode = 1
                            resmsg.value = DecTokenArr(1) 'ECMLoginId
                        Else
                            resmsg.errorCode = 3
                            resmsg.value = "Error code: 1_3 - Token Expired"
                        End If
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

        Function IsValidDate(dateStr As String) As Boolean
            ' Define the regex pattern for dd-mmm-yyyy format
            Dim pattern As String = "^(0[1-9]|[12][0-9]|3[01])-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-(\d{4})$"
            Dim regex As New Regex(pattern, RegexOptions.IgnoreCase)
            Dim match As Match = regex.Match(dateStr)
            Return match.Success
        End Function

        <HttpPost>
        Public Function Upload(para As InsUpload) As HttpResponseMessage
            Dim resmsg As New resmessage()
            Dim res As HttpResponseMessage
            Dim inputJson = ""
            Dim logf = "upload function called.."
            Try

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
                logf += " step1.."
                Dim strqry = "insert into eZAPICallHistory (Template,CabinetId ,TemplateId ,Status,Remarks,[RIM Number],ItemId,ParentCallId ,APIFunction,XmlFileName,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Isdeleted,inputJson ) values ('" + para.CabinetName + "',0,0,'Process','','',0,0,'Upload','','" + DateDateTimeToString(Date.Now, True) + "','',0,0,0,'" + inputJson + "')"
                Dim CallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqry)

                Dim errorMsg = ""
                'fields validation Starts
                For Each inputFieldList In para.Fields
                    'cabinet Name
                    If inputFieldList.FieldName.ToLower() = "cabinetname" Then
                        If inputFieldList.FieldName <> "" Then
                        Else
                            If errorMsg <> "" Then
                                errorMsg += " ,CabinetName cannot be empty"
                            Else
                                errorMsg = "CabinetName cannot be empty"
                            End If
                        End If
                    End If

                    'RIM Number
                    'If inputFieldList.FieldName.ToLower() = "rim number" Then
                    '    If inputFieldList.FieldName <> "" Then
                    '        If IsNumeric(inputFieldList.FieldValue) Then
                    '            If inputFieldList.FieldValue.Length = 8 Then
                    '            Else
                    '                If errorMsg <> "" Then
                    '                    errorMsg += " ,RIM Number must be 8 digit in Numeric"
                    '                Else
                    '                    errorMsg = "RIM Number must be 8 digit in Numeric"
                    '                End If

                    '            End If
                    '        Else
                    '            If errorMsg <> "" Then
                    '                errorMsg += " ,RIM Number must be 8 digit in Numeric"
                    '            Else
                    '                errorMsg = "RIM Number must be 8 digit in Numeric"
                    '            End If

                    '        End If
                    '    Else
                    '        If errorMsg <> "" Then
                    '            errorMsg += " ,RIM Number cannot be empty"
                    '        Else
                    '            errorMsg = "RIM Number cannot be empty"
                    '        End If
                    '    End If
                    'End If
                    'TIN Number
                    'If inputFieldList.FieldName.ToLower() = "tin number" Then
                    '    If inputFieldList.FieldName <> "" Then
                    '        If IsNumeric(inputFieldList.FieldValue) Then
                    '            If inputFieldList.FieldValue.Length = 9 Then

                    '            Else
                    '                If errorMsg <> "" Then
                    '                    errorMsg += " ,TIN Number must be 9 digit in Numeric"
                    '                Else
                    '                    errorMsg = "TIN Number must be 9 digit in Numeric"
                    '                End If

                    '            End If
                    '        Else
                    '            If errorMsg <> "" Then
                    '                errorMsg += " ,TIN Number must be 9 digits in Numeric"
                    '            Else
                    '                errorMsg = "TIN Number must be 9 digits in Numeric"
                    '            End If
                    '        End If
                    '    Else
                    '        If errorMsg <> "" Then
                    '            errorMsg += " ,TIN Number cannot be empty"
                    '        Else
                    '            errorMsg = "TIN Number cannot be empty"
                    '        End If
                    '    End If
                    'End If
                    'Account Number 
                    If inputFieldList.FieldName.ToLower() = "account number" Then
                        If inputFieldList.FieldValue <> "" Then
                            If IsNumeric(inputFieldList.FieldValue) Then
                                If inputFieldList.FieldValue.Length = 12 Then
                                Else
                                    If errorMsg <> "" Then
                                        errorMsg += " ,Account Number must be 12 digit in Numeric"
                                    Else
                                        errorMsg = "Account Number must be 12 digit in Numeric"
                                    End If
                                End If
                            Else
                                If errorMsg <> "" Then
                                    errorMsg += " ,Account Number must be 12 digits in Numeric"
                                Else
                                    errorMsg = "Account Number must be 12 digits in Numeric"
                                End If
                            End If
                        End If
                    End If

                    'RIM Name
                    If inputFieldList.FieldName.ToLower() = "rim name" Then
                        If inputFieldList.FieldValue = "" Then
                            If errorMsg <> "" Then
                                errorMsg += " ,Rim Name cannot be empty"
                            Else
                                errorMsg = "Rim Name cannot be empty"
                            End If
                        End If
                    End If

                    'RIM Branch
                    If inputFieldList.FieldName.ToLower() = "rim branch" Then
                        If inputFieldList.FieldValue = "" Then
                            If errorMsg <> "" Then
                                errorMsg += " ,Rim Branch cannot be empty"
                            Else
                                errorMsg = "Rim Branch cannot be empty"
                            End If
                        End If
                    End If

                    'file location
                    'If inputFieldList.FieldName.ToLower() = "file location" Then
                    '    If inputFieldList.FieldValue = "" Then
                    '        If errorMsg <> "" Then
                    '            errorMsg += " ,File Location cannot be empty"
                    '        Else
                    '            errorMsg = "File Location cannot be empty"
                    '        End If
                    '    End If
                    'End If

                    'Document Type
                    If inputFieldList.FieldName.ToLower() = "document type" Then
                        If inputFieldList.FieldValue = "" Then
                            If errorMsg <> "" Then
                                errorMsg += " ,Document Type cannot be empty"
                            Else
                                errorMsg = "Document Type cannot be empty"
                            End If
                        End If
                    End If
                    'RIM Type
                    If inputFieldList.FieldName.ToLower() = "rim type" Then
                        If inputFieldList.FieldValue = "" Then
                            If errorMsg <> "" Then
                                errorMsg += " ,RIM Type cannot be empty"
                            Else
                                errorMsg = "RIM Type cannot be empty"
                            End If
                        End If
                    End If
                    'Mandate Type
                    If inputFieldList.FieldName.ToLower() = "mandate type" Then
                        If inputFieldList.FieldValue = "" Then
                            If errorMsg <> "" Then
                                errorMsg += " ,Mandate Type cannot be empty"
                            Else
                                errorMsg = "Mandate Type cannot be empty"
                            End If
                        Else
                            If inputFieldList.FieldValue.ToLower() = "normal" Or inputFieldList.FieldValue.ToLower() = "staff" Or inputFieldList.FieldValue.ToLower() = "vip" Then
                            Else
                                If errorMsg <> "" Then
                                    errorMsg += " ,Mandate Type should be either of Normal / Staff / VIP"
                                Else
                                    errorMsg = "Mandate Type should be either of Normal / Staff / VIP"
                                End If
                            End If
                        End If
                    End If

                    'File status
                    If inputFieldList.FieldName.ToLower() = "file status" Then
                        If inputFieldList.FieldValue = "" Then
                            If errorMsg <> "" Then
                                errorMsg += " ,File Status cannot be empty"
                            Else
                                errorMsg = "File Status cannot be empty"
                            End If
                        End If
                    End If

                    'qualitycheck
                    If inputFieldList.FieldName.ToLower() = "quality check" Then
                        If inputFieldList.FieldValue <> "" Then
                            If inputFieldList.FieldValue.ToLower() = "yes" Or inputFieldList.FieldValue.ToLower() = "no" Then
                            Else
                                If errorMsg <> "" Then
                                    errorMsg += " ,Quality Check  must be either of Yes/No"
                                Else
                                    errorMsg = "Quality Check  must be either of Yes/No"
                                End If
                            End If
                        End If
                    End If

                    'Account status
                    If inputFieldList.FieldName.ToLower() = "account status" Then
                        If inputFieldList.FieldValue <> "" Then
                            If inputFieldList.FieldValue.ToLower() = "active" Or inputFieldList.FieldValue.ToLower() = "closed" Then
                            Else
                                If errorMsg <> "" Then
                                    errorMsg += " ,Account Status  must be either of Active/Closed"
                                Else
                                    errorMsg = "Account Status  must be either of Active/Closed"
                                End If
                            End If
                        End If
                    End If

                    'KYC Date
                    If inputFieldList.FieldName.ToLower() = "last kyc date" Then
                        If inputFieldList.FieldValue <> "" Then
                            Try
                                Dim iscorrectdate As Boolean = IsValidDate(inputFieldList.FieldValue)
                                If iscorrectdate = False Then
                                    If errorMsg <> "" Then
                                        errorMsg += " ,LAST KYC Date  must be in format dd-mmm-yyyy (05-Aug-2024)"
                                    Else
                                        errorMsg = "LAST KYC Date  must be in format dd-mmm-yyyy (05-Aug-2024)"
                                    End If
                                End If
                            Catch ex As Exception
                                If errorMsg <> "" Then
                                    errorMsg += " ,LAST KYC Date  must be in format dd-mmm-yyyy  (05-Aug-2024)"
                                Else
                                    errorMsg = "LAST KYC Date  must be in format dd-mmm-yyyy (05-Aug-2024)"
                                End If
                            End Try
                        End If
                    End If



                Next



                If para.CabinetName.ToLower() = "corporate" Then
                    'Individual RIM Number
                    'If para.Individual_RIM_Number <> "" Then
                    '    If IsNumeric(para.Individual_RIM_Number) Then
                    '        If para.Individual_RIM_Number.Length <> 9 Then
                    '            If errorMsg <> "" Then
                    '                errorMsg += " ,Individual Rim Number must be 9 digits in Numeric"
                    '            Else
                    '                errorMsg = "Individual Rim Number must be 9 digits in Numeric"
                    '            End If
                    '        End If
                    '    Else
                    '        If errorMsg <> "" Then
                    '            errorMsg += " ,Individual Rim Number must be 9 digits in Numeric"
                    '        Else
                    '            errorMsg = "Individual Rim Number must be 9 digits in Numeric"
                    '        End If
                    '    End If
                    'End If
                    'Individual TIN Number
                    'If para.Individual_TIN_Number <> "" Then
                    '    If IsNumeric(para.Individual_TIN_Number) Then
                    '        If para.Individual_TIN_Number.Length <> 9 Then
                    '            If errorMsg <> "" Then
                    '                errorMsg += " ,Individual TIN Number must be 9 digits in Nmeric"
                    '            Else
                    '                errorMsg = "Individual TIN Number must be 9 digits in Numeric"
                    '            End If
                    '        End If
                    '    Else
                    '        If errorMsg <> "" Then
                    '            errorMsg += " ,Individual TIN Number must be 9 digits in Numeric"
                    '        Else
                    '            errorMsg = "Individual TIN Number must be 9 digits in Numeric"
                    '        End If
                    '    End If
                    'End If
                End If


                If errorMsg <> "" Then
                    'res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, errorMsg)
                    res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 -" & errorMsg)
                    Dim strqryHistory = "update eZAPICallHistory set Remarks='Error code: 1_1 - " + errorMsg + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                    Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                End If
                'fields validation ends 
                If errorMsg = "" Then
                    If para.Token <> "" Then
                        logf += " step2.." + CallHistoryId.ToString()
                        Dim TokenResult = DecryptToken(para.Token)
                        If TokenResult.errorCode = 1 Then
                            If IUsername <> "" AndAlso IUNCpath <> "" Then
                                Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                                    Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
                                    If uncval Then
                                        logf += " step3..entered thefunction uploadfn "
                                        resmsg = UploadFn(para, TokenResult.value, CallHistoryId)
                                    End If
                                End Using
                            End If


                            logf += " step4.." + resmsg.value
                                        If resmsg.errorCode = 1 Then
                                            res = Request.CreateResponse(HttpStatusCode.OK, resmsg.value)
                                        Else
                                            res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, resmsg.value)
                                        End If
                                    Else
                                        res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, TokenResult.value)
                            Dim strqryHistory = "update eZAPICallHistory set Remarks='" + TokenResult.value + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                            Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                        End If
                    Else
                        res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 - Token should not be Empty")
                        Dim strqryHistory = "update eZAPICallHistory set Remarks='Error code: 1_1 - Token should not be Empty', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                        Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                    End If
                End If


            Catch ex As Exception
                res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Catch: " + logf + "    " + ex.Message.ToString())
            End Try
            Return res
        End Function

        <HttpPost>
        Public Function HSBCFileUpload(para As InsUpload) As HttpResponseMessage
            Dim resmsg As New resmessage()
            Dim res As HttpResponseMessage
            Dim inputJson = ""
            Dim logf = "HSBCupload function called.."
            Try
                logf &= "get cabinet name : " & para.CabinetName
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
                    ElseIf inputFieldList.FieldName.ToLower().Trim() = "document type" Then
                        If inputFieldList.FieldValue = "" Then
                            If errorMsg <> "" Then
                                errorMsg += " ,Document Type cannot be empty"
                            Else
                                errorMsg = "Document Type cannot be empty"
                            End If
                        End If
                    End If
                Next

                If errorMsg <> "" Then
                    'res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, errorMsg)
                    res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 -" & errorMsg)

                End If

                If errorMsg = "" Then
                    If para.Token <> "" Then
                        Dim TokenResult = DecryptToken(para.Token)
                        If TokenResult.errorCode = 1 Then
                            If IUsername <> "" AndAlso IUNCpath <> "" Then
                                logf += " step2. entered Iusername and Iuncpath not empty"
                                Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                                    Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
                                    logf += " step3 validated uncVal UNCPath: " + IUNCpath + "-UserName: " + IUsername + "-Domain:" + IDomain + "-Password:" + IPassword
                                    If uncval Then
                                        resmsg = HSBCUploadFn(para, TokenResult.value)
                                    Else
                                        Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, logf + "   UNC credentials validation returns false")
                                    End If
                                End Using
                            Else
                                resmsg = HSBCUploadFn(para, 1)
                            End If
                        Else
                            res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, TokenResult.value)
                        End If
                    End If


                    If resmsg.errorCode = 1 Then
                        Dim resObj = JsonConvert.DeserializeObject(resmsg.value)
                        res = Request.CreateResponse(HttpStatusCode.OK, resObj)
                    Else
                        res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, resmsg.value)
                    End If
                End If
            Catch ex As Exception
                res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Catch: " + "   Log: " + logf + " " + ex.Message.ToString())
            End Try
            Return res
        End Function

        <HttpGet>
        Public Function UploadStatusByRefNumber(RefNumber As String) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim strqry = "select * from eZAPICallHistory where CallHistoryId='" + RefNumber.ToString.Replace(APICallId_Prefix, "") + "'"
                Dim ds = GetDatasetByQuery(strqry)
                response = Request.CreateResponse(HttpStatusCode.OK, ds)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function FileReport(para As SearchRegistries) As HttpResponseMessage
            Dim StrQry = "", rowqry = ""
            Dim ds As New DataSet
            Dim dt As DataTable
            Dim result As New ResFileReport()
            Dim objList As New List(Of ResFileReportA)
            Dim response As HttpResponseMessage
            Dim errorlog = ""
            Try
                Dim CondtionReg As String = ""
                Dim Tablename = ""
                Dim TotNoOfPages = 0
                For Each cond In para.Criteria
                    If cond.DataTypeId = "2" Then
                        CondtionReg = CondtionReg + " and  us.[" + cond.Criteria + "] = '" + cond.Value1 + "'"
                    ElseIf cond.DataTypeId = "4" Then
                        CondtionReg = CondtionReg + " and  us.[" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                    ElseIf cond.DataTypeId = "5" Then
                        If cond.Value1 <> "" And cond.Value2 <> "" Then
                            If cond.Value1 = cond.Value2 Then
                                CondtionReg = CondtionReg + " and us.[" + cond.Criteria + "] <> '' and  convert(datetime,us.[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            Else
                                CondtionReg = CondtionReg + " and us.[" + cond.Criteria + "] <> '' and  convert(datetime,us.[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            End If

                        ElseIf cond.Value1 <> "" Then
                            CondtionReg = CondtionReg + " and us.[" + cond.Criteria + "] <> '' and convert(datetime,us.[" + cond.Criteria + "],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                        ElseIf cond.Value2 <> "" Then
                            CondtionReg = CondtionReg + "  and convert(datetime,us.[" + cond.Criteria + "],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                        End If
                    Else
                        CondtionReg = CondtionReg + " and  us.[" + cond.Criteria + "] = '" + cond.Value1 + "'"
                    End If
                Next
                If para.RowCount <> 0 Then
                    rowqry = "  OFFSET " + para.RowFrom.ToString() + " ROWS FETCH NEXT " + para.RowCount.ToString() + " ROWS ONLY"
                End If
                StrQry = "select us.*,c.CabinetName 'Cabinet Name',t.TemplateName 'Template Name',dbo.udf_LoginName(us.CreatedBy) as LoginName,'ezca_'+cast(us.CabinetId as nvarchar)+'_'+cast(us.TemplateId as nvarchar)+'_items' 'Table Name' from eZAPICallHistory us left join ezcabinet c on c.CabinetID=us.CabinetId left join eztemplate t on t.TemplateId=us.TemplateId   where us.isdeleted=0 " + CondtionReg + "  order by CallHistoryId desc " ' + rowqry + ""
                ds = SharedGetFunction.GetDatasetByQuery(StrQry)
                If Not IsNothing(ds) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                    If para.RowCount > 0 Then
                        dt = ds.Tables(0).Rows.Cast(Of DataRow)().Skip(para.RowFrom).Take(para.RowCount).CopyToDataTable
                    Else
                        dt = ds.Tables(0).Rows.Cast(Of DataRow)().Skip(para.RowFrom).CopyToDataTable
                    End If
                    dt.Columns.Add("Tin Number", GetType(String))
                    dt.Columns.Add("File Name", GetType(String))
                    dt.Columns.Add("Corporate", GetType(Boolean))
                    dt.Columns.Add("Retail", GetType(Boolean))
                    dt.Columns.Add("NoOfPages", GetType(Integer))
                    dt.Columns.Add("Call Duration", GetType(Double))

                    'ds.Tables(0).Columns.Add("File Name", GetType(String))
                    'ds.Tables(0).Columns.Add("Retail File Name", GetType(Boolean))
                    'ds.Tables(0).Columns.Add("NoOfPages", GetType(Integer))
                    'ds.Tables(0).Columns.Add("Call Duration", GetType(Double))
                    For Each row As DataRow In dt.Rows  'ds.Tables(0).Rows
                        row("Retail") = False
                        row("Corporate") = False

                        errorlog = errorlog + " Rowcount: " + dt.Rows.Count.ToString
                        If row("CabinetId") <> 0 AndAlso row("TemplateId") <> 0 AndAlso row("ItemId") <> 0 Then
                            errorlog = errorlog + " callhistoryId: " + row("CallHistoryId").ToString
                            Dim paracre As New Criteria()
                            paracre.Criteria = "TemplateId"
                            paracre.Value = row("TemplateId").ToString
                            Dim Objvalue = SelectedeZTemplateList(paracre.Criteria, paracre.Value)
                            If Not Objvalue Is Nothing Then
                                StrQry = "select ifilename,isnull(nopages,'0') nopages,[Tin Number] TinNumber from " + Objvalue(0).TableName.ToString + " where ItemId= " + row("ItemId").ToString + ""
                                Dim dsitem = GetDatasetByQuery(StrQry)
                                If Not IsNothing(dsitem) AndAlso dsitem.Tables.Count > 0 AndAlso dsitem.Tables(0).Rows.Count > 0 Then
                                    row("File Name") = dsitem.Tables(0).Rows(0)("ifilename").ToString
                                    row("NoOfPages") = Convert.ToInt32(dsitem.Tables(0).Rows(0)("nopages").ToString)
                                    row("Tin Number") = dsitem.Tables(0).Rows(0)("TinNumber").ToString
                                    row("Corporate") = True
                                    ' TotNoOfPages = TotNoOfPages + Convert.ToInt32(dsitem.Tables(0).Rows(0)("nopages").ToString)
                                Else
                                    row("File Name") = ""
                                    row("NoOfPages") = 0
                                End If
                                If row("ParentCallId") <> 0 Then
                                    StrQry = "select ifilename from " + Objvalue(0).TableName.ToString + " where ItemId= " + row("ItemId").ToString + ""
                                    Dim dsitemRetail = GetDatasetByQuery(StrQry)
                                    If Not IsNothing(dsitem) AndAlso dsitem.Tables.Count > 0 AndAlso dsitem.Tables(0).Rows.Count > 0 Then
                                        row("Retail") = True
                                    Else
                                        row("Retail") = False
                                    End If
                                Else
                                    row("Retail") = False
                                End If
                                If row("UpdatedOnAPI").ToString <> "" Then
                                    Dim _hour = DateDiff(DateInterval.Hour, Convert.ToDateTime(row("CreatedOn").ToString), Convert.ToDateTime(row("UpdatedOnAPI").ToString))
                                    Dim _minute = DateDiff(DateInterval.Minute, Convert.ToDateTime(row("CreatedOn").ToString), Convert.ToDateTime(row("UpdatedOnAPI").ToString))
                                    Dim _second = DateDiff(DateInterval.Second, Convert.ToDateTime(row("CreatedOn").ToString), Convert.ToDateTime(row("UpdatedOnAPI").ToString))
                                    Dim ts As TimeSpan = TimeSpan.FromSeconds(_second)
                                    row("Call Duration") = ts.TotalMilliseconds ' + ":" + ts.Minutes.ToString + ":" + ts.Seconds.ToString
                                Else
                                    row("Call Duration") = 0
                                End If
                            End If
                        Else
                            row("File Name") = ""
                            row("Retail") = False
                            row("NoOfPages") = 0
                            row("Call Duration") = 0
                            row("Tin Number") = ""
                            row("Corporate") = False
                        End If

                        Dim obj As New ResFileReportA()
                        Try

                            obj.CallHistoryId = Convert.ToInt32(row("CallHistoryId").ToString)
                            obj.CabinetId = Convert.ToInt32(row("CabinetId").ToString)
                            obj.TemplateId = Convert.ToInt32(row("TemplateId").ToString)
                            obj.ItemId = Convert.ToInt32(row("ItemId").ToString)
                            obj.Status = row("Status").ToString
                            obj.RimNumber = row("RIM Number").ToString
                            obj.TinNumber = row("Tin Number").ToString
                            obj.InitiatedAT = row("CreatedOn").ToString
                            obj.CompletedAt = row("CreatedOn").ToString
                            obj.FileName = row("File Name").ToString
                            obj.NoofPages = Convert.ToInt32(row("NoOfPages").ToString)
                            obj.CallDuration = Convert.ToDouble(row("Call Duration").ToString)
                            obj.Corporate = Convert.ToBoolean(row("Corporate").ToString)
                            obj.Retail = Convert.ToBoolean(row("Retail").ToString)
                            objList.Add(obj)
                        Catch ex As Exception
                            response = Request.CreateResponse(HttpStatusCode.Forbidden, " CallHistoryId1: " + obj.CallHistoryId.ToString + ex.Message + "      " + errorlog)
                            Return response
                        End Try
                    Next
                    StrQry = "Select isnull(sum(case when itemid!=0 then 1 else 0 end),0) TotalFilesCount,count(distinct (cast([RIM NUmber] as int))) TotalRIMNumberCount,(select isnull(sum(case when Status='Archived' then 1 else 0 end),0) from eZAPICallHistory) 'NoofSucessfulAPI' ,(select isnull(sum(case when Status='Unprocessed' then 1 else 0 end),0) 'NoofUnSucessfulAPI' from eZAPICallHistory) 'NoofUnSucessfulAPI'  from eZAPICallHistory us  where us.isdeleted=0 and [RIM NUmber]!='' and Status!='Unprocessed' " + CondtionReg
                    Dim ItemListCount = GetDatasetByQuery(StrQry)
                    If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                        result.FileCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                        result.RIMCount = ItemListCount.Tables(0).Rows(0)(1).ToString()
                        result.NoofSucessfulAPI = ItemListCount.Tables(0).Rows(0)(2).ToString()
                        result.NoofUnSucessfulAPI = ItemListCount.Tables(0).Rows(0)(3).ToString()
                    End If

                    StrQry = "select isnull(sum(case when nopages!=0 then nopages else 0 end),0) TotNoOfPages from " + ds.Tables(0).Rows(0)("Table Name").ToString + " where itemid in (select us.ItemId from eZAPICallHistory us left join ezcabinet c on c.CabinetID=us.CabinetId left join eztemplate t on t.TemplateId=us.TemplateId   where us.isdeleted=0 and us.itemid!=0 " + CondtionReg + ")"
                    Dim ItemCountds = GetDatasetByQuery(StrQry)
                    If Not IsNothing(ItemCountds) AndAlso ItemCountds.Tables.Count > 0 AndAlso ItemCountds.Tables(0).Rows.Count > 0 Then
                        result.PageCount = ItemCountds.Tables(0).Rows(0)(0).ToString()
                    End If

                    result.TotalRows = ds.Tables(0).Rows.Count.ToString()
                End If

                ' result.Data1 = dt
                result.Data = objList
                ' result.PageCount = TotNoOfPages
                response = Request.CreateResponse(HttpStatusCode.OK, result)
            Catch ex As Exception
                response = Request.CreateResponse(HttpStatusCode.Forbidden, ex.Message + "    " + errorlog)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetAPICallHistoryOptionsValue(Para As GetOptionsValue) As DataSet
            Dim res As New DataSet
            Try
                res = GetAPICallHistoryOptionsValueFn(Para)
            Catch ex As Exception
                Throw New FaultException("GetOptionsValue : " + ex.ToString())
            End Try
            Return res
        End Function

        <HttpPost>
        Public Function GetFilesForAdvancedSearch(para As Condition) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Dim result As New Data
            Dim paraqry As New ByQuery
            Dim pathid = "", cabinetName = "", templateId = ""
            Dim ext = "", pathlist = "", imglist = "", pdfpath = ""
            Dim logf As String = ""
            Try
                pathid = para.RIMNumber '+ "_" + DateTime.Now.ToString("yyyyMMddhhmmssffftt")
                cabinetName = para.cabinetName
                paraqry.StrQry = "select templateId from ezTemplate where templateName ='" & cabinetName & "' and isdeleted=0"
                Dim ds_Cabinet = GetDatasetByQuery(paraqry.StrQry)
                If Not ds_Cabinet Is Nothing AndAlso ds_Cabinet.Tables.Count > 0 AndAlso ds_Cabinet.Tables(0).Rows.Count > 0 Then
                    templateId = ds_Cabinet.Tables(0).Rows(0)("templateId").ToString()
                    If cabinetName.ToLower() = "cad_sep24" Then
                        paraqry.StrQry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype,[Document Type] from ezca_3_" & templateId & "_items i left join eZERSInfo e on e.ERSId=i.ERSId  where [rim number]='" + para.RIMNumber + "' and i.isdeleted=0"
                    Else
                        paraqry.StrQry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype,[Tin Number],[Rim Type],[Document Type],[Rim Branch],[Rim Name],[File Location] from ezca_3_" & templateId & "_items i left join eZERSInfo e on e.ERSId=i.ERSId  where [rim number]='" + para.RIMNumber + "' and i.isdeleted=0"
                    End If

                    'strqry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype from ezca_101_items i left join eZERSInfo e on e.ERSId=i.ERSId"
                    Dim ds = GetDatasetByQuery(paraqry.StrQry)
                    logf &= Environment.NewLine & " step 2... taking rim Number details " & paraqry.StrQry
                    If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then

                        Dim TempFilepath = System.Web.Hosting.HostingEnvironment.MapPath("~\Mergefiles\" + pathid.ToString + "")
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
                            logf &= Environment.NewLine & " step 4...fileName " & Filename
                            ' Dim writePermission As FileIOPermission = New FileIOPermission(FileIOPermissionAccess.AllAccess, Filename)

                            If IUsername <> "" AndAlso IUNCpath <> "" Then
                                Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                                    Dim uncval = unc.NetUseWithCredentials(IUNCpath, IUsername, IDomain, IPassword)
                                    If uncval Then
                                        If File.Exists(Filename) Then
                                            logf &= Environment.NewLine & "fileName " & Filename
                                            localfile = System.IO.Path.Combine(TempFilepath, Path.GetFileNameWithoutExtension(Row("ifilename").ToString) + ".ezo")
                                            If File.Exists(localfile) Then
                                                localfile = System.IO.Path.Combine(TempFilepath, Path.GetFileNameWithoutExtension(Row("ifilename").ToString) + DateTime.Now.ToString("yyyyMMddhhmmssffftt") + ".ezo")
                                            End If
                                            clientD.DownloadFile(Filename, localfile)
                                            clientD.Dispose()
                                            logf &= Environment.NewLine & " step 5...localfile " & localfile
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
                                            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Files not exist in " + Filename + " additional Logs: " + logf + "")
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
                                logf &= Environment.NewLine & " step 6...download url  " & result.url
                                'result.url = "http://52.172.32.88/InvitaAPI/v1/Common/DownloadFiles?userid=" + UserId + "&filename=" + pdfname + ""
                                response = Request.CreateResponse(HttpStatusCode.OK, result)
                            Else
                                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, res)
                            End If
                        Else
                            response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Files not found .. path list: " + pathlist + "  .. Imagelist: " + imglist)
                        End If
                    Else
                        response = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid RIM NUmber" & logf)
                    End If

                End If

            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "E5_" + ex.Message & logf)
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
    End Class
End Namespace