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
Imports Microsoft.AspNetCore.Routing


Namespace Controllers
    Public Class CreditController
        Inherits ApiController

        <HttpPost>
        Public Function FileUpload(para As InsUpload) As HttpResponseMessage
            Dim resmsg As New resmessage()
            Dim res As HttpResponseMessage
            Dim inputJson = ""
            Dim strLoanType = ""
            Dim errorMsg = ""
            Dim errors As New List(Of String)
            Dim logf = "upload function called.."
            Try
                logf &= "get cabinet name : " & para.CabinetName
                If getDefaultCabinetName(para.CabinetName).ToLower() = "creditloan" Then
                    If SaveAPICallInput.ToLower = "yes" Then
                        Dim parafile = para.file
                        Dim inscopy As New InsUpload
                        inscopy = para
                        inscopy.file = Nothing
                        inputJson = JsonConvert.SerializeObject(inscopy)
                        If String.IsNullOrEmpty(parafile) Then
                            errorMsg = "File cannot be empty"
                            errors.Add("""File""cannot be Empty")
                        End If
                        para.file = parafile
                    Else
                        inputJson = ""
                    End If
                    inputJson = inputJson.Replace("'", "")
                    logf &= "step 1.1---Input is serialized to json format "
                    Dim strqry = "insert into eZCREDITAPICallHistory (Template,CabinetId ,TemplateId ,Status,Remarks,[RIM Number],ItemId,ParentCallId ,APIFunction,XmlFileName,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Isdeleted,inputJson ) values ('" + para.CabinetName + "',0,0,'Process','','',0,0,'Upload','','" + DateDateTimeToString(Date.Now, True) + "','',0,0,0,'" + inputJson + "')"
                    'logf &= "step1.2---  for 1st insert :" & strqry
                    Dim CallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqry)
                    logf &= "step1.3--callhistoryid : " & CallHistoryId.ToString()

                    'fields validation Starts
                    For Each inputFieldList In para.Fields
                        If inputFieldList.FieldName.ToLower().Trim() = "rim number" Then
                            If inputFieldList.FieldValue = "" Then
                                errors.Add("""RIM Number""cannot be Empty")
                                If errorMsg <> "" Then
                                    errorMsg += " ,RIM Number cannot be empty"
                                Else
                                    errorMsg = "RIM Number cannot be empty"
                                End If
                            ElseIf Not IsNumeric(inputFieldList.FieldValue) Then
                                errors.Add("""RIM Number""should be numeric")
                                If errorMsg <> "" Then
                                    errorMsg += " ,RIM Number should be numeric"
                                Else
                                    errorMsg = "RIM Number should be numeric"
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "loan type" Then
                            Dim allowedLoanTypes As String() = {
                                                               "Corporate Loans",
                                                               "Retail"
                                                               }
                            If String.IsNullOrWhiteSpace(inputFieldList.FieldValue) Then
                                errors.Add("""Loan Type""cannot be Empty")
                                If errorMsg <> "" Then
                                    errorMsg += " ,Loan Type cannot be empty"
                                Else
                                    errorMsg = "Loan Type cannot be empty"
                                End If
                            ElseIf Not allowedLoanTypes.Any(Function(x) _
    x.Equals(inputFieldList.FieldValue.Trim(), StringComparison.OrdinalIgnoreCase)) Then
                                Dim validValues As String = String.Join(", ", allowedLoanTypes)
                                errors.Add("""Loan Type""should be one of the values:" & validValues)
                                If errorMsg <> "" Then
                                    errorMsg += ",  Loan Type should be one of the following values: " & validValues
                                Else
                                    errorMsg = " Loan Type should be one of the following values: " & validValues
                                End If
                            Else
                                strLoanType = inputFieldList.FieldValue
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "sub loan type" Then
                            If strLoanType <> "" Then
                                If Not String.IsNullOrWhiteSpace(inputFieldList.FieldValue) Then
                                    Dim categoryQry As String = $"SELECT DISTINCT SubLoanType FROM [eZSubLoanType] WHERE [LoanType] = '{strLoanType }'"
                                    Dim ds_subloanType As DataSet = GetDatasetByQuery(categoryQry)
                                    Dim isValid As Boolean = False
                                    Dim allowedValues As String = ""
                                    If ds_subloanType IsNot Nothing AndAlso ds_subloanType.Tables.Count > 0 AndAlso ds_subloanType.Tables(0).Rows.Count > 0 Then
                                        isValid = ds_subloanType.Tables(0).AsEnumerable().
                                                    Any(Function(r) String.Equals(
                                                    r("SubLoanType").ToString().Trim(),
                                                    inputFieldList.FieldValue.Trim(),
                                                    StringComparison.OrdinalIgnoreCase))
                                        allowedValues = String.Join(", ", ds_subloanType.Tables(0).AsEnumerable().
                   Select(Function(r) r("SubLoanType").ToString().Trim()))
                                        If Not isValid Then
                                            errors.Add("""Sub Loan Type""should be one of the values:" & allowedValues)
                                            If errorMsg <> "" Then
                                                errorMsg += ",  Sub Loan Type should be one of the following values: " & allowedValues
                                            Else
                                                errorMsg = " Sub Loan Type should be one of the following values: " & allowedValues
                                            End If
                                        End If
                                    End If
                                Else
                                    Dim categoryQry As String = $"SELECT DISTINCT SubLoanType as [Sub Loan Type] FROM [eZSubLoanType] WHERE [LoanType] = '{strLoanType }'"
                                    Dim ds_subloanType As DataSet = GetDatasetByQuery(categoryQry)
                                    If ds_subloanType IsNot Nothing AndAlso ds_subloanType.Tables.Count > 0 AndAlso ds_subloanType.Tables(0).Rows.Count > 0 Then
                                        Dim allowedValues As String = String.Join(", ", ds_subloanType.Tables(0).AsEnumerable().
                  Select(Function(r) r("SubLoanType").ToString().Trim()))
                                        errors.Add("""Sub Loan Type""should be one of the values:" & allowedValues)
                                        If errorMsg <> "" Then
                                            errorMsg += ",  Sub Loan Type should be one of the following values: " & allowedValues
                                        Else
                                            errorMsg = " Sub Loan Type should be one of the following values: " & allowedValues
                                        End If
                                    End If
                                End If
                            Else
                                errors.Add("A valid Loan Type must be provided when Sub Loan Type is provided")
                                If errorMsg <> "" Then
                                    errorMsg += " ,Loan Type is required when Sub Loan Type is provided."
                                Else
                                    errorMsg = "Loan Type is required when Sub Loan Type is provided."
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "rim name" Then
                            If inputFieldList.FieldValue = "" Then
                                errors.Add("""RIM Name"" cannot be empty")
                                If errorMsg <> "" Then
                                    errorMsg += " ,RIM Name cannot be empty"
                                Else
                                    errorMsg = "RIM Name cannot be empty"
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "tin number" Then
                            If inputFieldList.FieldValue = "" Then
                                errors.Add("""TIN Number"" cannot be empty")
                                If errorMsg <> "" Then
                                    errorMsg += " ,TIN Number cannot be empty"
                                Else
                                    errorMsg = "TIN Number cannot be empty"
                                End If
                            ElseIf Not IsNumeric(inputFieldList.FieldValue) Then
                                errors.Add("""TIN Number"" should be numeric")
                                If errorMsg <> "" Then
                                    errorMsg += " ,TIN Number should be numeric"
                                Else
                                    errorMsg = "TIN Number should be numeric"
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "rim type" Then
                            Dim allowedrimTypes As String() = {
                                                               "Personal",
                                                               "Non-Personal"
                                                               }
                            If String.IsNullOrWhiteSpace(inputFieldList.FieldValue) Then
                                errors.Add("""Rim Type"" should be numeric")
                                If errorMsg <> "" Then
                                    errorMsg += " ,Rim Type cannot be empty"
                                Else
                                    errorMsg = "Rim Type cannot be empty"
                                End If
                            ElseIf Not allowedrimTypes.Any(Function(x) _
    x.Equals(inputFieldList.FieldValue.Trim(), StringComparison.OrdinalIgnoreCase)) Then
                                Dim validValues As String = String.Join(", ", allowedrimTypes)
                                errors.Add("""Rim Type"" should be one of the following values: " & validValues)
                                If errorMsg <> "" Then
                                    errorMsg += ",  Rim Type should be one of the following values: " & validValues
                                Else
                                    errorMsg = " Rim Type should be one of the following values: " & validValues
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "document type" Then
                            Dim allowedDocTypes As String() = {
                                                               "Compliance",
                                                               "Consumer Loan Account",
                                                               "Corporate Loan - ATS/Rollovers/LIS Documents",
                                                               "Corporate Loan - Other Documents",
                                                               "Customer Identification",
                                                               "Deposit",
                                                               "Loans",
                                                               "Old Closed Loans",
                                                               "Other Documents",
                                                               "Primary Loan Documents",
                                                               "RIM Details",
                                                               "Supporting Documents"
                                                               }
                            If String.IsNullOrWhiteSpace(inputFieldList.FieldValue) Then
                                If errorMsg <> "" Then
                                    errors.Add("""Document Type"" cannot be empty")
                                    errorMsg += " ,Document Type cannot be empty"
                                Else
                                    errorMsg = "Document Type cannot be empty"
                                End If
                            ElseIf Not allowedDocTypes.Any(Function(x) _
    x.Equals(inputFieldList.FieldValue.Trim(), StringComparison.OrdinalIgnoreCase)) Then
                                Dim validValues As String = String.Join(", ", allowedDocTypes)
                                errors.Add("""Document Type"" should be one of the following values: " & validValues)
                                If errorMsg <> "" Then
                                    errorMsg += ",  Document Type should be one of the following values: " & validValues
                                Else
                                    errorMsg = " Document Type should be one of the following values: " & validValues
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "account status" Then
                            Dim allowedDocTypes As String() = {
                                                               "Active",
                                                               "Closed"
                                                               }
                            If Not String.IsNullOrWhiteSpace(inputFieldList.FieldValue) And Not allowedDocTypes.Any(Function(x) _
    x.Equals(inputFieldList.FieldValue.Trim(), StringComparison.OrdinalIgnoreCase)) Then
                                Dim validValues As String = String.Join(", ", allowedDocTypes)
                                errors.Add("""Account Status"" should be one of the following values: " & validValues)
                                If errorMsg <> "" Then
                                    errorMsg += ",  Account Status should be one of the following values: " & validValues
                                Else
                                    errorMsg = " Account Status should be one of the following values: " & validValues
                                End If
                                '                        ElseIf Not allowedDocTypes.Any(Function(x) _
                                'x.Equals(inputFieldList.FieldValue.Trim(), StringComparison.OrdinalIgnoreCase)) Then
                                '                            Dim validValues As String = String.Join(", ", allowedDocTypes)
                                '                            errors.Add("""Account Status"" should be one of the following values: " & validValues)
                                '                            If errorMsg <> "" Then
                                '                                errorMsg += ",  Account Status should be one of the following values: " & validValues
                                '                            Else
                                '                                errorMsg = " Account Status should be one of the following values: " & validValues
                                '                            End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "rim branch" Then
                            If inputFieldList.FieldValue = "" Then
                                errors.Add("""RIM Branch"" cannot be empty")
                                If errorMsg <> "" Then
                                    errorMsg += " ,RIM Branch cannot be empty"
                                Else
                                    errorMsg = "RIM Branch cannot be empty"
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "mandate type" Then
                            Dim allowedMandateType As String() = {
                                                               "Normal",
                                                               "Staff",
                                                               "VIP"
                                                               }
                            If String.IsNullOrWhiteSpace(inputFieldList.FieldValue) Then
                                errors.Add("""Mandate Type"" cannot be empty")
                                If errorMsg <> "" Then
                                    errorMsg += " ,Mandate Type cannot be empty"
                                Else
                                    errorMsg = "Mandate Type cannot be empty"
                                End If
                            ElseIf Not allowedMandateType.Any(Function(x) _
    x.Equals(inputFieldList.FieldValue.Trim(), StringComparison.OrdinalIgnoreCase)) Then
                                Dim validValues As String = String.Join(", ", allowedMandateType)
                                errors.Add("""Mandate Type"" hould be one of the following values: " & validValues)
                                If errorMsg <> "" Then
                                    errorMsg += ",  Mandate Type should be one of the following values: " & validValues
                                Else
                                    errorMsg = " Mandate Type should be one of the following values: " & validValues
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "file status" Then
                            Dim allowedFileStatus As String() = {
                                                               "Original File not Received",
                                                               "Original File Received"
                                                               }
                            If String.IsNullOrWhiteSpace(inputFieldList.FieldValue) Then
                                errors.Add("""File Status"" cannot be empty")
                                If errorMsg <> "" Then
                                    errorMsg += " ,File Status Be cannot be empty"
                                Else
                                    errorMsg = "File Status Be cannot be empty"
                                End If
                            ElseIf Not allowedFileStatus.Any(Function(x) _
x.Equals(inputFieldList.FieldValue.Trim(), StringComparison.OrdinalIgnoreCase)) Then
                                Dim validValues As String = String.Join(", ", allowedFileStatus)
                                errors.Add("""File Status""  should be one of the following values: " & validValues)
                                If errorMsg <> "" Then
                                    errorMsg += ", File Status should be one of the following values: " & validValues
                                Else
                                    errorMsg = " File status should be one of the following values: " & validValues
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "quality check" Then
                            Dim allowedQualityCheck As String() = {
                                                               "Yes",
                                                               "No"
                                                               }
                            If Not String.IsNullOrWhiteSpace(inputFieldList.FieldValue) And Not allowedQualityCheck.Any(Function(x) _
x.Equals(inputFieldList.FieldValue.Trim(), StringComparison.OrdinalIgnoreCase)) Then
                                Dim validValues As String = String.Join(", ", allowedQualityCheck)
                                errors.Add("""Quality Check""  should be one of the following values: " & validValues)
                                If errorMsg <> "" Then
                                    errorMsg += " ,Quality Check should be one of the following values: " & validValues
                                Else
                                    errorMsg = "Quality Check should be one of the following values: " & validValues
                                End If
                            End If
                        ElseIf inputFieldList.FieldName.ToLower().Trim() = "last kyc date" Then
                            If Not String.IsNullOrWhiteSpace(inputFieldList.FieldValue) Then
                                Dim isValid As Boolean = IsValidDate(inputFieldList.FieldValue)
                                If Not isValid Then
                                    errors.Add("""Last Kyc Date""  should be in the format dd-mm-yyyy")
                                    If errorMsg <> "" Then
                                        errorMsg += " ,Last Kyc Date should be in the format dd-mm-yyyy"
                                    Else
                                        errorMsg = "Last Kyc Date should be in the format dd-mm-yyyy"
                                    End If
                                End If
                            End If
                        End If
                    Next

                    If errorMsg <> "" Then
                        'res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, errorMsg)
                        Dim responseObj = New With {
                                 .ErrorMessage = errors
                            }
                        res = Request.CreateResponse(HttpStatusCode.NotAcceptable, responseObj)
                        Dim strqryHistory = "update eZCREDITAPICallHistory set Remarks='Error code: 1_1 - " + errorMsg + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
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
                                            resmsg = CREDITUploadFn(para, TokenResult.value, CallHistoryId)
                                        Else
                                            strqry = " Update eZCREDITAPICallHistory set Remarks='Error code:1_1 - UNC Credentials validation returns false', UPdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' Where CallHistoryId='" + CallHistoryId.ToString() + "'"
                                            Dim callHistoryIdUNC = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqry)
                                            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, logf + "   UNC credentials validation returns false")
                                        End If
                                    End Using
                                Else
                                    resmsg = CREDITUploadFn(para, TokenResult.value, CallHistoryId)
                                    ' resmsg.value = "Error code: 1_1 - UNC Path empty"
                                End If
                                'resmsg = CMAPUploadFn(para, TokenResult.value, CallHistoryId)
                                ' logf += " step4.." + resmsg.value
                                If resmsg.errorCode = 1 Then
                                    Dim resObj = JsonConvert.DeserializeObject(resmsg.value)
                                    res = Request.CreateResponse(HttpStatusCode.OK, resObj)
                                Else
                                    errors.Add(resmsg.value)
                                    Dim responseObj = New With {
                                 .ErrorMessage = errors
                            }
                                    res = Request.CreateResponse(HttpStatusCode.BadRequest, responseObj)
                                End If
                            Else
                                errors.Add(TokenResult.value)
                                Dim responseObj = New With {
                                 .ErrorMessage = errors
                            }
                                res = Request.CreateResponse(HttpStatusCode.BadRequest, responseObj)
                                'res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, TokenResult.value)
                                Dim strqryHistory = "update eZCREDITAPICallHistory set Remarks='" + TokenResult.value + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                            End If
                        Else
                            errors.Add("Token should not be empty")
                            Dim responseObj = New With {
                                 .ErrorMessage = errors
                            }
                            res = Request.CreateResponse(HttpStatusCode.BadRequest, responseObj)
                            ' res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 - Token should not be Empty")
                            Dim strqryHistory = "update eZCREDITAPICallHistory set Remarks='Error code: 1_1 - Token should not be Empty', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                            Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                        End If
                    End If
                Else
                    errors.Add("Invalid Cabinet")
                    Dim responseObj = New With {
                                 .ErrorMessage = errors
                            }
                    res = Request.CreateResponse(HttpStatusCode.BadRequest, responseObj)
                    'res = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Error code: 1_1 - InValid Cabinet")
                End If


            Catch ex As Exception
                errors.Add("Catch: " + "   Log: " + logf + " " + ex.Message.ToString())
                Dim responseObj = New With {
                                 .ErrorMessage = errors
                            }
                res = Request.CreateResponse(HttpStatusCode.BadRequest, responseObj)
                ' res = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Catch: " + "   Log: " + logf + " " + ex.Message.ToString())
            End Try
            Return res
        End Function

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
            Dim errorMsg = "", itemQry = ""
            Dim errors As New List(Of String)
            Dim strRimNumber = "", strCabinetName = "", strItemId = "", templateId = ""
            Dim strqryconds As String = ""
            Try
                If Not IsNothing(para.Fields) AndAlso para.Fields.Count > 0 Then
                    Dim StrQry As String = "select templateId from ezTemplate where templateName ='" & para.CabinetName & "' and isdeleted=0"
                    Dim ds_Cabinet = GetDatasetByQuery(StrQry)
                    If ds_Cabinet IsNot Nothing AndAlso ds_Cabinet.Tables.Count > 0 AndAlso ds_Cabinet.Tables(0).Rows.Count > 0 Then
                        templateId = ds_Cabinet.Tables(0).Rows(0)("templateId").ToString()
                        Dim fieldsList = SelectedeZTemplateFieldList("TemplateId", ds_Cabinet.Tables(0).Rows(0)("templateId").ToString())
                        For i As Integer = 0 To fieldsList.Count - 1
                            For Each inputFieldList In para.Fields
                                If fieldsList(i).FieldName.ToLower = inputFieldList.FieldName.ToLower Then
                                    If inputFieldList.FieldValue <> "" Then
                                        strqryconds += " and  i.[" + inputFieldList.FieldName + "]" + "= '" & inputFieldList.FieldValue.Replace("'", "''") & "'"
                                    End If
                                ElseIf inputFieldList.FieldName.ToLower().Replace(" ", "").Trim() = "itemid" Then
                                    If inputFieldList.FieldValue <> "" Then
                                        strItemId = inputFieldList.FieldValue
                                        itemQry = " and i.itemId='" & strItemId & "'"
                                        logf &= " got itemId :" & strItemId
                                    End If
                                ElseIf inputFieldList.FieldName = "" Or inputFieldList.FieldName.ToLower() = "string" Then
                                    errorMsg = "field values cannot be empty"
                                    If Not errors.Contains("Field Values cannot be empty") Then
                                        errors.Add("Field Values cannot be empty")
                                    End If
                                End If
                            Next
                        Next
                    End If

                End If
                If para.Token = "" Or para.Token.ToLower() = "string" Then
                    errors.Add("Token cannot be empty")
                    If errorMsg <> "" Then
                        errorMsg += " ,Token cannot be empty"
                    Else
                        errorMsg = "Token cannot be empty"
                    End If
                End If
                If para.CabinetName = "" Or para.CabinetName.ToLower() = "string" Then
                    errors.Add("cabinetName cannot be empty")
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
                                    Dim StrQry As String = "select i.itemId, i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype,[Document Type],encodeType from ezca_3_" & templateId & "_items i left join eZERSInfo e on e.ERSId=i.ERSId where i.isdeleted=0 " & itemQry & strqryconds
                                    Dim ds = GetDatasetByQuery(StrQry)
                                    If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                                        Dim result = New List(Of Object)
                                        For Each row As DataRow In ds.Tables(0).Rows
                                            result.Add(New With {
                .URL = APIurl & "/V1/Credit/viewfile/" & para.CabinetName & "/" & row("itemId").ToString()
            })

                                        Next
                                        ' Return Ok(result)
                                        Dim jsonResult As String = JsonConvert.SerializeObject(result)
                                        Return Ok(jsonResult)
                                    End If
                                Else
                                    errors.Add("UNC Credentials not get validated")
                                    Dim responseObj = New With {
          .ErrorMessage = errors
     }
                                    Return Content(HttpStatusCode.BadRequest, responseObj)
                                    'Return BadRequest("UNC Credentials not get validated")
                                End If
                            End Using
                        Else
                            Dim StrQry As String = "select i.itemId, i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype,[Document Type],encodeType from ezca_3_" & templateId & "_items i left join eZERSInfo e on e.ERSId=i.ERSId where i.isdeleted=0 " & itemQry & strqryconds
                            Dim ds = GetDatasetByQuery(StrQry)
                            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                                Dim result = New List(Of Object)
                                For Each row As DataRow In ds.Tables(0).Rows
                                    result.Add(New With {
        .URL = APIurl & "/V1/Credit/viewfile/" & para.CabinetName & "/" & row("itemId").ToString()
    })

                                Next
                                'Return Ok(result)
                                Dim jsonResult As String = JsonConvert.SerializeObject(result)
                                Return Ok(jsonResult)
                            End If
                        End If
                    Else
                        errors.Add(TokenResult.value)
                        Dim responseObj = New With {
          .ErrorMessage = errors
     }
                        Return Content(HttpStatusCode.BadRequest, responseObj)
                        ' Return BadRequest(TokenResult.value)
                    End If
                Else
                    Dim responseObj = New With {
          .ErrorMessage = errors
     }
                    Return Content(HttpStatusCode.BadRequest, responseObj)
                    ' Return BadRequest(errorMsg)
                End If

            Catch ex As Exception
                errors.Add(logf & ex.Message.ToString())
                Dim responseObj = New With {
          .ErrorMessage = errors
     }
                Return Content(HttpStatusCode.BadRequest, responseObj)
                ' Return BadRequest(logf & ex.Message.ToString())
            End Try

            errors.Add("No records found")
            Dim responseObj1 = New With {
          .ErrorMessage = errors
     }
            Return Content(HttpStatusCode.NotAcceptable, responseObj1)
            'Return StatusCode(204)

        End Function

        <HttpGet>
        <Route("v1/Credit/viewfile/{cabinetName}/{itemid}")>
        Public Function viewFile(cabinetName As String, itemId As Integer) As HttpResponseMessage
            Dim resmsg As HttpResponseMessage
            Dim logf As String = "View file function called...."
            Dim errors As New List(Of String)
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
                                                        errors.Add("Error in encryption")
                                                        'resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error in encryption")
                                                    End If
                                                End Try
                                            Else
                                                errors.Add("Error in encryption")
                                                'resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error in encryption")
                                            End If
                                        Else
                                            errors.Add("File Not Exists" & localfile)
                                            ' resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File Not exists" & localfile)
                                        End If
                                    Else
                                        errors.Add("File Not Exists" & filePath)
                                        'resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File Not exists" & filePath)
                                    End If
                                Else
                                    errors.Add("UNC Credentials not get validated")
                                    ' resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "UNC Credentials not get validated")
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
                                                errors.Add("Error in encryption")
                                                ' resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error in encryption")
                                            End If
                                        End Try
                                    Else
                                        errors.Add("Error in encryption")
                                        ' resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error in encryption")
                                    End If
                                Else
                                    errors.Add("File Not exists" & localfile)
                                    'resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File Not exists" & localfile)
                                End If
                            Else
                                errors.Add("File Not exists" & filePath)
                                'resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File Not exists" & filePath)
                            End If
                            'resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, " UNC Credentials empty ")
                        End If
                    Else
                        errors.Add("No record exists")
                        ' resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, " No record exists ")
                    End If
                Else
                    errors.Add(" cabinet does not exists ")
                    ' resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "cabinet does not exists ")
                End If


            Catch ex As Exception
                errors.Add(logf & ex.Message.ToString())
                ' resmsg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, logf & ex.Message.ToString())
            End Try
            Dim responseObj = New With {
     .ErrorMessage = errors
}
            resmsg = Request.CreateResponse(HttpStatusCode.BadRequest, responseObj)
            Return resmsg
        End Function
    End Class
End Namespace

