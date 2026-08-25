Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data
Imports System.Diagnostics
Imports System.Net
Imports System.Threading
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports ScanningTradeFinance.publicvariables

Public Class ApiFunctions
    Private Const ApiTimeoutMs As Integer = 30000

    Dim Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
    Public apiUrlExternal = Appcon("apiUrlExternal").ToString()
    Dim apiUrlInvita = Appcon("InvitaAPI").ToString()
    Public TemplateId = Appcon("TemplateId").ToString()
    Public scanfrom = Appcon("scanfrom").ToString()
    Public scanfilenamepath = Appcon("scanfilenamepath").ToString()
    Public enableProcessLog As String = If(Appcon("enableProcessLog") Is Nothing, "false", Appcon("enableProcessLog").ToString())

    Private Function IsApiSupportMessageEnabled() As Boolean
        Try
            If Appcon Is Nothing OrElse Appcon("enableApiSupportMessage") Is Nothing Then
                Return False
            End If
            Return String.Equals(Appcon("enableApiSupportMessage").ToString().Trim(), "true", StringComparison.OrdinalIgnoreCase)
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Reads apiRetryAttempts from api config. Defaults to 3 when missing/invalid. Minimum 1.
    ''' </summary>
    Public Shared Function GetApiRetryAttempts() As Integer
        Try
            Dim apiConfig As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
            Dim raw = If(apiConfig Is Nothing OrElse apiConfig("apiRetryAttempts") Is Nothing, "", apiConfig("apiRetryAttempts").ToString())
            Dim parsed As Integer
            If Integer.TryParse(raw, parsed) AndAlso parsed >= 1 Then
                Return parsed
            End If
        Catch
        End Try
        Return 3
    End Function

    ''' <summary>
    ''' Reads apiRetryDelayMs from api config. Defaults to 2000 when missing/invalid. Minimum 0.
    ''' </summary>
    Public Shared Function GetApiRetryDelayMs() As Integer
        Try
            Dim apiConfig As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
            Dim raw = If(apiConfig Is Nothing OrElse apiConfig("apiRetryDelayMs") Is Nothing, "", apiConfig("apiRetryDelayMs").ToString())
            Dim parsed As Integer
            If Integer.TryParse(raw, parsed) AndAlso parsed >= 0 Then
                Return parsed
            End If
        Catch
        End Try
        Return 2000
    End Function

    Private Function CreateWebClient() As WebClient
        Dim client As New TimeoutWebClient(ApiTimeoutMs)
        client.Headers("Content-Type") = "application/json"
        client.Encoding = System.Text.Encoding.UTF8
        Return client
    End Function

    Private Class TimeoutWebClient
        Inherits WebClient

        Private ReadOnly _timeoutMs As Integer

        Public Sub New(timeoutMs As Integer)
            _timeoutMs = timeoutMs
        End Sub

        Protected Overrides Function GetWebRequest(address As Uri) As WebRequest
            Dim request = MyBase.GetWebRequest(address)
            If request IsNot Nothing Then
                request.Timeout = _timeoutMs
            End If
            Return request
        End Function
    End Class


    Public Function tf_acct_basic_info(para As AccBasicInfo) As DataSet
        Dim resaccbasicinfo As New DataSet
        Dim ds As New DataSet
        Try
            Dim Input = para
            Dim client = New WebClient()
            client.Headers("Content-Type") = "application/json"
            client.Encoding = System.Text.Encoding.UTF8
            Dim inputJson = JsonConvert.SerializeObject(Input)

            Dim json = client.UploadString(apiUrlExternal + "/v1/TradeFinance/tf_acct_basic_info", inputJson)
            'Dim json = client.UploadString("http://localhost/TradeFinanceAPI/v1/TradeFinance/tf_acct_basic_info", inputJson)
            If (json <> "{}") Then
                json = "{'Table': [" + json + "]}"
                ' Dim jsonobj = Newtonsoft.Json.JsonConvert.DeserializeObject(Of AccountBasicInfo)(json)
                resaccbasicinfo = Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataSet)(json)
                If resaccbasicinfo IsNot Nothing AndAlso resaccbasicinfo.Tables.Count > 0 AndAlso resaccbasicinfo.Tables(0).Rows.Count > 0 Then
                    resaccbasicinfo.Tables(0).Columns("acct_no").ColumnName = "AccountNo"
                    resaccbasicinfo.Tables(0).Columns("acct_type").ColumnName = "AccountType"
                    resaccbasicinfo.Tables(0).Columns("rim_no").ColumnName = "RIMNumber"
                    resaccbasicinfo.Tables(0).Columns("status").ColumnName = "Status"
                End If
            Else
                resaccbasicinfo = Nothing
            End If


        Catch ex As Exception
            AppLogger.LogException("ApiFunctions.tf_acct_basic_info", ex)
        End Try
        Return resaccbasicinfo
    End Function


    Public Function GetInfoFromAccountNo(Input As AccountInfo) As DataSet
        Dim ds As New DataSet
        Try
            '  Dim Input = New AccountInfo
            'Input.acct_no = query.acct_no
            'Input.url = query.url
            Dim client = CreateWebClient()
            Dim inputJson = Newtonsoft.Json.JsonConvert.SerializeObject(Input)
            Dim json = client.UploadString(apiUrlInvita + "/v1/Common/GetInfoFromAccountNo", inputJson)

            If (json <> "{}") Then
                '                json =
                '"{                     
                '                                                                                                                                                                                        ""acct_no"": ""100000027062"",
                '                                                                                                                                                                                        ""acct_type"": ""CUR"",
                '                                                                                                                                                                                        ""rim_no"": ""10015730"",
                '                                                                                                                                                                                        ""status"": ""Active"",
                '                                                                                                                                                                                        ""rim_type"": ""NonPersonal"",
                '                                                                                                                                                                                        ""first_name"": ""10015730"",
                '                                                                                                                                                                                        ""middle_initial"": """",
                '                                                                                                                                                                                        ""last_name"": ""10015730""
                '                                                                                                                                                                                    }"
                Dim obj As JObject = JObject.Parse(json)
                ' Dim rimName As String = obj("first_name").ToString() &
                'obj("middle_initial").ToString() &
                'obj("last_name").ToString()
                Dim rimName As String = ""
                If obj("rim_type").ToString().ToLower() = "nonpersonal" Then
                    rimName = obj("last_name").ToString() & " " & obj("first_name").ToString()
                Else
                    rimName = obj("first_name").ToString() & " " &
                       obj("middle_initial").ToString() & " " &
                       obj("last_name").ToString()
                End If
                obj("rim_name") = rimName
                Dim result As String = obj.ToString(Formatting.Indented)
                json = result
                json = "{'Table': [" + json + "]}"
                ' MsgBox("Output json is" + json)
                ds = Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataSet)(json)
                ' MsgBox("having result")
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    ds.Tables(0).Columns("acct_no").ColumnName = "AccountNo"
                    ds.Tables(0).Columns("acct_type").ColumnName = "AccountType"
                    ds.Tables(0).Columns("rim_no").ColumnName = "RIMNumber"
                    ds.Tables(0).Columns("status").ColumnName = "Status"
                    ds.Tables(0).Columns("rim_type").ColumnName = "RIMTYPE"
                    ds.Tables(0).Columns("rim_name").ColumnName = "RIMName"
                    ds.Tables(0).Columns("first_name").ColumnName = "FirstName"
                    ds.Tables(0).Columns("middle_initial").ColumnName = "MAIDENNAME"
                    ds.Tables(0).Columns("last_name").ColumnName = "Lastname"
                End If
            Else
                ds = Nothing
            End If

        Catch ex As Exception
            'writetxtfle("Exception in LoadDatasetbyquery : query: " & query & " *** " & ex.ToString)
            AppLogger.LogException("ApiFunctions.GetInfoFromAccountNo", ex)
            MsgBox("Exception in GetInfoFromAccountNo : " + ex.Message)
        End Try
        Return ds

    End Function

    Private Function TruncateForLog(value As String, Optional maxLen As Integer = 200) As String
        Dim text = If(value, "")
        text = text.Replace(vbCrLf, " ").Replace(vbLf, " ").Replace(vbCr, " ").Trim()
        If text.Length > maxLen Then
            Return text.Substring(0, maxLen) & "..."
        End If
        Return text
    End Function

    Private Function GetCallerFunctionName() As String
        Try
            Dim st As New StackTrace()
            Dim appAssembly = GetType(ApiFunctions).Assembly

            For Each frame As StackFrame In st.GetFrames()
                Dim m = frame.GetMethod()
                If m Is Nothing OrElse m.DeclaringType Is Nothing Then
                    Continue For
                End If

                Dim declType = m.DeclaringType
                Dim typeName = declType.Name
                Dim methodName = m.Name
                Dim fullTypeName = If(declType.FullName, "")
                Dim ns = If(declType.Namespace, "")

                ' Skip ApiFunctions itself
                If declType Is GetType(ApiFunctions) OrElse fullTypeName.StartsWith("ScanningTradeFinance.ApiFunctions") Then
                    Continue For
                End If

                ' Skip compiler-generated lambdas / closures
                If typeName.Contains("DisplayClass") OrElse typeName.Contains("<>c") OrElse
                   methodName.Contains("_Lambda$") OrElse methodName.StartsWith("<") OrElse
                   (methodName.Contains("__") AndAlso methodName.Contains("$")) Then
                    Continue For
                End If

                ' Skip WPF / framework plumbing (Dispatcher.Invoke, etc.)
                If ns.StartsWith("System.") OrElse ns = "System" OrElse
                   ns.StartsWith("Microsoft.") OrElse ns.StartsWith("MS.") OrElse
                   typeName = "Dispatcher" OrElse
                   methodName = "Invoke" OrElse methodName = "BeginInvoke" OrElse
                   methodName = "InvokeAsync" OrElse methodName = "Send" Then
                    Continue For
                End If

                ' Prefer frames from this application assembly
                If declType.Assembly IsNot appAssembly Then
                    Continue For
                End If

                ' Walk out of nested compiler types to the owning class (e.g. MainWindow)
                Dim ownerType = declType
                While ownerType IsNot Nothing AndAlso
                      (ownerType.Name.Contains("DisplayClass") OrElse ownerType.Name.Contains("<>c") OrElse ownerType.IsNestedPrivate)
                    If ownerType.DeclaringType Is Nothing Then
                        Exit While
                    End If
                    ownerType = ownerType.DeclaringType
                End While

                Dim ownerName = If(ownerType Is Nothing, typeName, ownerType.Name)
                Return ownerName & "." & methodName
            Next
        Catch
        End Try
        Return "Unknown"
    End Function

    Private Function GetSwaggerUrl() As String
        Try
            Dim baseUrl = If(apiUrlInvita, "").ToString().Trim().TrimEnd("/"c)
            If String.IsNullOrWhiteSpace(baseUrl) Then
                Return ""
            End If
            Return baseUrl & "/swagger"
        Catch
            Return ""
        End Try
    End Function

    Private Sub ShowApiSupportMessage(functionName As String, endpoint As String, teamHint As String, detail As String)
        Try
            If Not IsApiSupportMessageEnabled() Then
                Return
            End If
            Dim caller = GetCallerFunctionName()
            Dim detailText = TruncateForLog(detail, 400)
            Dim swaggerUrl = GetSwaggerUrl()
            Dim showAction =
                Sub()
                    Try
                        Dim win As New ApiSupportMessageWin(teamHint, functionName, caller, swaggerUrl, detailText)
                        If Application.Current IsNot Nothing AndAlso Application.Current.MainWindow IsNot Nothing AndAlso
                           Application.Current.MainWindow.IsLoaded Then
                            win.Owner = Application.Current.MainWindow
                        End If
                        win.ShowDialog()
                    Catch
                        ' Fallback if custom dialog cannot be shown
                        MsgBox(
                            teamHint & Environment.NewLine & Environment.NewLine &
                            "Function: " & functionName & Environment.NewLine &
                            "Called from: " & caller & Environment.NewLine &
                            "Swagger URL: " & swaggerUrl & Environment.NewLine &
                            "Detail: " & detailText,
                            MsgBoxStyle.Exclamation,
                            "API Support")
                    End Try
                End Sub

            If Application.Current IsNot Nothing AndAlso Application.Current.Dispatcher IsNot Nothing Then
                If Application.Current.Dispatcher.CheckAccess() Then
                    showAction()
                Else
                    Application.Current.Dispatcher.Invoke(showAction)
                End If
            Else
                showAction()
            End If
        Catch
        End Try
    End Sub

    Public Function GetDatasetByQuery(query As String, Optional showSupportMessage As Boolean = True) As DataSet
        Return ExecuteGetDatasetByQuery(apiUrlInvita & "/v1/Common/GetDatasetByQuery", query, showSupportMessage)
    End Function

    Public Function GetDatasetByQueryTesting(query As String, Optional showSupportMessage As Boolean = True) As DataSet
        Return ExecuteGetDatasetByQuery(apiUrlInvita & "/v1/Commons/GetDatasetByQuery", query, showSupportMessage)
    End Function

    Private Function ExecuteGetDatasetByQuery(endpoint As String, query As String, showSupportMessage As Boolean) As DataSet
        Const functionName As String = "GetDatasetByQuery"
        Dim maxAttempts As Integer = GetApiRetryAttempts()
        Dim retryDelayMs As Integer = GetApiRetryDelayMs()
        Dim ds As DataSet = Nothing
        Dim json As String = Nothing
        Dim lastException As Exception = Nothing
        Dim lastHttpStatus As String = ""
        Dim lastTableCount As Integer = 0
        Dim lastRowCount As Integer = 0

        For attempt As Integer = 1 To maxAttempts
            ds = Nothing
            json = Nothing
            lastException = Nothing
            lastHttpStatus = ""
            lastTableCount = 0
            lastRowCount = 0
            Try
                Dim Input = New publicvariables.ByQuery()
                Input.StrQry = query
                Dim client = CreateWebClient()
                Dim inputJson = Newtonsoft.Json.JsonConvert.SerializeObject(Input)
                json = client.UploadString(endpoint, inputJson)
                ds = Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataSet)(json)

                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lastTableCount = ds.Tables.Count
                    lastRowCount = ds.Tables(0).Rows.Count
                    If lastRowCount = 0 Then
                        AppLogger.LogStep(
                            "GetDatasetByQuery returned empty result (no exception) | endpoint=" & endpoint &
                            " | Tables=" & lastTableCount.ToString() &
                            " | Rows=0" &
                            " | ResponseLen=" & If(json, "").Length.ToString() &
                            " | ResponsePreview='" & TruncateForLog(json, 300) & "'" &
                            " | Query='" & TruncateForLog(query) & "'")
                        If showSupportMessage Then
                            ShowApiSupportMessage(
                                functionName,
                                endpoint,
                                "API reached. Check by - Ezofis Team",
                                "Tables=" & lastTableCount.ToString() & " | Rows=0 | Query=" & TruncateForLog(query, 200))
                        End If
                        Return ds
                    End If
                    Return ds
                End If

                AppLogger.LogStep(
                    "GetDatasetByQuery returned empty result (no exception) | endpoint=" & endpoint &
                    " | attempt=" & attempt.ToString() & "/" & maxAttempts.ToString() &
                    " | Tables=" & lastTableCount.ToString() &
                    " | Rows=" & lastRowCount.ToString() &
                    " | ResponseLen=" & If(json, "").Length.ToString() &
                    " | ResponsePreview='" & TruncateForLog(json, 300) & "'" &
                    " | Query='" & TruncateForLog(query) & "'")
            Catch ex As Exception
                lastException = ex
                Dim webEx = TryCast(ex, WebException)
                If webEx IsNot Nothing Then
                    Dim httpResp = TryCast(webEx.Response, HttpWebResponse)
                    If httpResp IsNot Nothing Then
                        lastHttpStatus = " | HttpStatus=" & CInt(httpResp.StatusCode).ToString() & " (" & httpResp.StatusCode.ToString() & ")"
                    Else
                        lastHttpStatus = " | WebStatus=" & webEx.Status.ToString()
                    End If
                End If
                AppLogger.LogStep(
                    "GetDatasetByQuery API failed | endpoint=" & endpoint &
                    " | attempt=" & attempt.ToString() & "/" & maxAttempts.ToString() &
                    lastHttpStatus &
                    " | ExceptionType=" & ex.GetType().FullName &
                    " | Message='" & ex.Message & "'" &
                    " | ResponsePreview='" & TruncateForLog(json, 300) & "'" &
                    " | Query='" & TruncateForLog(query) & "'",
                    ex)
            End Try

            If attempt < maxAttempts Then
                Thread.Sleep(retryDelayMs)
            End If
        Next

        If showSupportMessage Then
            If lastException IsNot Nothing Then
                ShowApiSupportMessage(
                    functionName,
                    endpoint,
                    "Unable to complete request. API not reached - BBK Team",
                    lastException.Message & lastHttpStatus)
            Else
                ShowApiSupportMessage(
                    functionName,
                    endpoint,
                    "Unable to complete request. API response empty - BBK Team",
                    "Tables=" & lastTableCount.ToString() & " | Rows=" & lastRowCount.ToString() &
                    " | ResponsePreview=" & TruncateForLog(json, 200))
            End If
        End If
        Return Nothing
    End Function

    Public Function InsertAndUpdateAndDeleteeZUserDefinedWithScope(query As String, Optional showSupportMessage As Boolean = True) As Integer
        Dim i As Integer = 0
        Dim endpoint = apiUrlInvita & "/v1/Common/InsertAndUpdateAndDeleteeZUserDefinedWithScope"
        Const functionName As String = "InsertAndUpdateAndDeleteeZUserDefinedWithScope"
        Dim maxAttempts As Integer = GetApiRetryAttempts()
        Dim retryDelayMs As Integer = GetApiRetryDelayMs()
        Dim json As String = Nothing
        Dim lastException As Exception = Nothing

        For attempt As Integer = 1 To maxAttempts
            i = 0
            json = Nothing
            lastException = Nothing
            Try
                Dim Input = New publicvariables.ByQuery()
                Input.StrQry = query
                Dim client = New WebClient()
                client.Headers("Content-Type") = "application/json"
                client.Encoding = System.Text.Encoding.UTF8
                Dim inputJson = JsonConvert.SerializeObject(Input)
                json = client.UploadString(endpoint, inputJson)
                i = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Integer)(json)
                If i <> 0 Then
                    Return i
                End If
                AppLogger.LogStep(
                    "InsertAndUpdateAndDeleteeZUserDefinedWithScope returned 0 | endpoint=" & endpoint &
                    " | attempt=" & attempt.ToString() & "/" & maxAttempts.ToString() &
                    " | ResponsePreview='" & TruncateForLog(json, 300) & "'" &
                    " | Query='" & TruncateForLog(query) & "'")
            Catch ex As Exception
                lastException = ex
                AppLogger.LogStep(
                    "InsertAndUpdateAndDeleteeZUserDefinedWithScope API failed | endpoint=" & endpoint &
                    " | attempt=" & attempt.ToString() & "/" & maxAttempts.ToString() &
                    " | Message='" & ex.Message & "'" &
                    " | Query='" & TruncateForLog(query) & "'",
                    ex)
            End Try

            If attempt < maxAttempts Then
                Thread.Sleep(retryDelayMs)
            End If
        Next

        If showSupportMessage Then
            If lastException IsNot Nothing Then
                ShowApiSupportMessage(
                    functionName,
                    endpoint,
                    "Insert/Update/Delete failed. API not reached - BBK Team",
                    lastException.Message & " | Query=" & TruncateForLog(query, 200))
            Else
                ShowApiSupportMessage(
                    functionName,
                    endpoint,
                    "Insert/Update/Delete failed. API reached - check by Ezofis Team",
                    "Result=0 | Query=" & TruncateForLog(query, 200))
            End If
        End If
        Return i
    End Function





    Public Function InserteZSBUserSession(usersessionobj As UserSession) As Integer
        Dim ds As New DataSet
        Dim i As Integer
        Try
            Dim Input = New UserSession()
            Input.ActionId = usersessionobj.ActionId
            Input.Itemid = usersessionobj.Itemid
            Input.CabinetId = usersessionobj.CabinetId
            Input.CreatedOn = usersessionobj.CreatedOn
            Input.CreatedBy = usersessionobj.CreatedBy
            '  Input.query = qry.Replace("\", "\\")
            Dim client = New WebClient()
            client.Headers("Content-Type") = "application/json"
            client.Encoding = System.Text.Encoding.UTF8
            Dim inputJson = JsonConvert.SerializeObject(Input)
            Dim json = client.UploadString(apiUrlInvita + "/v1/Session/InserteZSBUserSession", "PATCH", inputJson)
            ' Dim json = client.UploadString(apiUrl + "/v1/Session/InserteZSBUserSession", inputJson)
            i = Newtonsoft.Json.JsonConvert.DeserializeObject(json)
            If i = 0 Then
                '     writetxtfle("Error in InsertAndUpdateAndDeleteeZUserDefinedWithScope")
            End If
        Catch ex As Exception
            AppLogger.LogException("ApiFunctions.InserteZSBUserSession", ex)
        End Try
        Return i
    End Function


End Class
