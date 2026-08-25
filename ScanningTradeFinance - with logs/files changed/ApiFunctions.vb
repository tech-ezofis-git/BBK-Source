Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data
Imports System.Net
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
            '  MsgBox("Exception in apifunctions tf_acct_basic_info  : " + ex.Message.ToString)
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
                '                                                        ""acct_no"": ""100000027062"",
                '                                                        ""acct_type"": ""CUR"",
                '                                                        ""rim_no"": ""10015730"",
                '                                                        ""status"": ""Active"",
                '                                                        ""rim_type"": ""NonPersonal"",
                '                                                        ""first_name"": ""10015730"",
                '                                                        ""middle_initial"": """",
                '                                                        ""last_name"": ""10015730""
                '                                                    }"
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
            MsgBox("Exception in GetInfoFromAccountNo : " + ex.Message)
        End Try
        Return ds

    End Function

    Public Function GetDatasetByQuery(query As String) As DataSet
        Dim ds As New DataSet
        Try
            Dim Input = New publicvariables.ByQuery()
            Input.StrQry = query
            Dim client = CreateWebClient()
            Dim inputJson = Newtonsoft.Json.JsonConvert.SerializeObject(Input)
            Dim json = client.UploadString(apiUrlInvita + "/v1/Common/GetDatasetByQuery", inputJson)
            ds = Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataSet)(json)
        Catch ex As Exception
            MsgBox("Exception in GetDatasetByQuery" + ex.Message)
            'writetxtfle("Exception in LoadDatasetbyquery : query: " & query & " *** " & ex.ToString)
        End Try
        Return ds

    End Function

    Public Function InsertAndUpdateAndDeleteeZUserDefinedWithScope(query As String) As Integer
        Dim ds As New DataSet
        Dim i As Integer
        Try
            Dim Input = New publicvariables.ByQuery()
            Input.StrQry = query
            '  Input.query = qry.Replace("\", "\\")
            Dim client = New WebClient()
            client.Headers("Content-Type") = "application/json"
            client.Encoding = System.Text.Encoding.UTF8
            Dim inputJson = JsonConvert.SerializeObject(Input)
            Dim json = client.UploadString(apiUrlInvita + "/v1/Common/InsertAndUpdateAndDeleteeZUserDefinedWithScope", inputJson)
            i = Newtonsoft.Json.JsonConvert.DeserializeObject(json)
            If i = 0 Then
                ' MsgBox("Error in InsertAndUpdateAndDeleteeZUserDefinedWithScope")
            End If
        Catch ex As Exception
            MsgBox("Exception in InsertAndUpdateAndDeleteeZUserDefinedWithScope : " + query + " : Exception" + ex.ToString)
        End Try
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
            ' writetxtfle("Load InsertAndUpdateAndDeleteeZUserDefinedWithScope  : Exception" + ex.ToString)
        End Try
        Return i
    End Function


End Class
