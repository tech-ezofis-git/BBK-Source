Imports System.Data.Sql
Imports System.Globalization
Imports System.IO
Imports System.Xml
Imports dtSearch.Engine
Imports Newtonsoft.Json
Imports System.Runtime.Serialization
Imports System.Web.Script.Serialization
Imports System.Net.Mail
Imports System.Reflection
Imports ECMAPI.ParaVariables
Imports System.DirectoryServices.AccountManagement
Imports System.Security.Cryptography
Imports iTextSharp.text.pdf.qrcode
Imports Newtonsoft.Json.Linq
Imports Org.BouncyCastle.Ocsp

Public Class SharedGetFunction

#Region "Common"
    Shared DateformatWithTime As String = "dd-MMM-yyyy hh:mm:ss tt"
    Shared Dateformat As String = "dd-MMM-yyyy"
    Public Shared Function GetDatasetByQuery(ByRef StrQry As String) As DataSet
        Try
            Return DBLayer.DBLInstance.GetDatasetByQuery(StrQry)
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WDBR740F300DB30 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function

    Public Shared Function GetDatasetByStoredProcedureName(ByVal param As String()) As DataSet
        Try
            Dim ds = DBLayer.DBLInstance.GetDatasetByStoredProcedureName("SP_GeteZWorkflowDetailsListbyCondition", param)
            Return ds
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR1010F300 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function DateDateTimeToString(ByVal dt As DateTime, ByVal WithTime As Boolean) As String
        Try
            Dim dateValue As String
            If WithTime Then
                dateValue = dt.ToString(DateformatWithTime)
            Else
                dateValue = dt.ToString(Dateformat)
            End If

            Return dateValue
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR740F230 " + ex.ToString()
            '  Throw New FaultException(exc)
        End Try
    End Function


    Public Shared Function InsertAndUpdateAndDeleteeZUserDefined(ByRef query As String) As Integer
        Try
            Return DBLayer.DBLInstance.InsertAndUpdate(query)
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WDBR740F160DB10 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function InsertAndUpdateAndDeleteeZUserDefinedWithScope(ByRef query As String) As Integer
        Try
            Return DBLayer.DBLInstance.InsertAndUpdateWithScope(query)
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WDBR740F170DB10 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function

    Public Shared Function InsertAndUpdate(ByRef strQry As String) As Integer
        Try
            Return DBLayer.DBLInstance.InsertAndUpdate(strQry)
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WDBR740F500DB10 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function


    Public Shared Function GetUniqId(ByRef GenName As String) As String
        Try

            Dim SqlQuery = "select prefix, lastgenid, isnull(leftpad,'0') as leftpad from eZIdGeneration where genname='" + GenName + "';"
            Dim reqno = ""
            Dim dsgen = GetDatasetByQuery(SqlQuery)
            If Not dsgen Is Nothing Then
                If dsgen.Tables.Count > 0 Then
                    If dsgen.Tables(0).Rows.Count > 0 Then
                        Dim lastgenid As Integer
                        Integer.TryParse(Convert.ToString(dsgen.Tables(0).Rows(0)("lastgenid")), lastgenid)
                        reqno = (lastgenid + 1).ToString
                        reqno = Convert.ToString(dsgen.Tables(0).Rows(0)("prefix")) + reqno.PadLeft(Convert.ToInt32(dsgen.Tables(0).Rows(0)("leftpad")), "0")
                    End If
                End If
            End If
            If reqno = "" Then
                reqno = "1"
                SqlQuery = "  insert into eZIdGeneration values('" + GenName + "','','1','','" + DateDateTimeToString(DateTime.Now.ToString(), True) + "','" + DateDateTimeToString(DateTime.Now.ToString(), True) + "','1','',0);"
            Else
                SqlQuery = "update ezidgeneration set lastgenid=lastgenid+1, updatedon='" + DateDateTimeToString(DateTime.Now.ToString(), True) + "', updatedby='1' where genname='" + GenName + "';"
            End If

            InsertAndUpdate(SqlQuery)

            Return reqno
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WDBR740F500DB10 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function

#End Region

#Region "DB Config"
    Public Shared DBConfig As String = System.Web.Hosting.HostingEnvironment.MapPath("~/DBCONN.xml")

    Public Shared Function ConnectionString() As String
        ' Dim asd As New eZProfile
        Try
            Dim ServerName As String = ""
            Dim DataBaseName As String = ""
            Dim UserName As String = ""
            Dim Password As String = ""
            Dim loXMLDoc As XmlDocument = New XmlDocument
            Dim loNode As XmlNode
            If File.Exists(DBConfig) Then
                loXMLDoc.Load(DBConfig)
                loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/ServerName")
                If loNode.InnerText <> "" Then
                    ServerName = DBLayer.Decrypt(loNode.InnerText, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)

                End If
                loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/UserName")
                If loNode.InnerText <> "" Then
                    UserName = DBLayer.Decrypt(loNode.InnerText, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                End If
                loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/Password")
                If loNode.InnerText <> "" Then
                    Password = DBLayer.Decrypt(loNode.InnerText, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                End If
                loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/DataBaseName")
                If loNode.InnerText <> "" Then
                    DataBaseName = DBLayer.Decrypt(loNode.InnerText, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                End If
                If ServerName <> "" And UserName <> "" And Password <> "" Then
                    If DataBaseName <> "" Then
                        ConnectionString = "Data Source = " + ServerName + ";initial catalog = " + DataBaseName + ";User ID=" + UserName + ";password=" + Password + ";"

                    Else
                        ConnectionString = "Data Source = " + ServerName + ";User ID=" + UserName + ";password=" + Password + ";"
                    End If
                Else
                    ConnectionString = Nothing
                End If
            Else
                ConnectionString = "DBCONN file doesnt exist"
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            ConnectionString = Nothing
        End Try
    End Function




    Public Shared Function DBDelete() As String
        Try
            Dim strDB As String() = GetDBCONFIG()
            If strDB(1).ToString() <> "" Then
                If UpdateDataBaseName("") = "DataBaseName Updated" Then
                    DBLayer.DBLInstance.ConnectionStr = ConnectionString()
                    If DBLayer.DBLInstance.deleteDB(strDB(1)) = 0 Then
                        DBDelete = strDB(1) + " Database Removed Sucessfully"
                    Else
                        UpdateDataBaseName(strDB(1))
                        DBDelete = strDB(1) + " Problem In Removing Database"
                    End If
                Else
                    DBDelete = strDB(1) + " Problem In Removing Database"
                End If
            Else
                DBDelete = "Database Not Found"
            End If
        Catch ex As Exception
            DBDelete = ex.Message
        End Try
    End Function
    Public Shared Function GetDBCONFIG() As String()
        Dim result(3) As String
        Try
            Dim loXMLDoc As XmlDocument = New XmlDocument
            Dim loNode As XmlNode
            loXMLDoc.Load(DBConfig)
            loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/ServerName")
            result(0) = DBLayer.Decrypt(loNode.InnerText, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/UserName")
            result(2) = DBLayer.Decrypt(loNode.InnerText, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/Password")
            result(3) = DBLayer.Decrypt(loNode.InnerText, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/DataBaseName")
            result(1) = DBLayer.Decrypt(loNode.InnerText, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            Return result
        Catch ex As Exception
            Return result
        End Try
    End Function
    Public Shared Function UpdateServerName(ByVal strServerName As String) As String
        Try
            Dim loXMLDoc As XmlDocument = New XmlDocument
            Dim loNode As XmlNode
            loXMLDoc.Load(DBConfig)
            loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/ServerName")
            loNode.InnerText = DBLayer.Encrypt(strServerName, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            loXMLDoc.Save(DBConfig)
            DBLayer.DBLInstance.ConnectionStr = ConnectionString()
            Return "ServerName Updated"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
    Public Shared Function UpdateDataBaseName(ByVal strDataBaseName As String) As String
        Try
            Dim loXMLDoc As XmlDocument = New XmlDocument
            Dim loNode As XmlNode
            loXMLDoc.Load(DBConfig)
            loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/DataBaseName")
            loNode.InnerText = DBLayer.Encrypt(strDataBaseName, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            loXMLDoc.Save(DBConfig)
            DBLayer.DBLInstance.ConnectionStr = ConnectionString()
            Return "DataBaseName Updated"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
    Public Shared Function UpdateUsrNPw(ByVal strUserName As String, ByVal strPassword As String) As String
        Try
            Dim loXMLDoc As XmlDocument = New XmlDocument
            Dim loNode As XmlNode
            loXMLDoc.Load(DBConfig)
            loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/UserName")
            loNode.InnerText = DBLayer.Encrypt(strUserName, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            loNode = loXMLDoc.SelectSingleNode("//DBCON/Field/Password")
            loNode.InnerText = DBLayer.Encrypt(strPassword, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
            loXMLDoc.Save(DBConfig)
            DBLayer.DBLInstance.ConnectionStr = ConnectionString()
            Return "UserName & Password Updated"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
    Public Shared Function CheckConnectionString(ByVal strServerName As String, ByVal strDataBaseName As String, ByVal strUserName As String, ByVal strPassword As String) As String
        If strServerName <> "" And strUserName <> "" And strPassword <> "" Then
            If strDataBaseName <> "" Then
                Return DBLayer.DBLInstance.chkCS("Data Source = " + strServerName + ";initial catalog = " + strDataBaseName + ";User ID=" + strUserName + ";password=" + strPassword + ";")
            Else
                Return DBLayer.DBLInstance.chkCS("Data Source = " + strServerName + ";User ID=" + strUserName + ";password=" + strPassword + ";")
            End If
        Else
            Return "Connection Failed"
        End If
        Return Nothing
    End Function
    Public Shared Function GetSqlDBlist(ByVal strServerName As String, ByVal strUserName As String, ByVal strPassword As String) As List(Of String)
        Try
            If DBLayer.DBLInstance.chkCS("Data Source = " + strServerName + ";User ID=" + strUserName + ";password=" + strPassword + ";") = "Connection Successed" Then
                DBLayer.DBLInstance.ConnectionStr = "Data Source = " + strServerName + ";User ID=" + strUserName + ";password=" + strPassword + ";"
                Dim lstItems As New List(Of String)
                lstItems = DBLayer.DBLInstance.ReadAllDB()
                DBLayer.DBLInstance.ConnectionStr = ConnectionString()
                Return lstItems
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return Nothing
        End Try
    End Function
    Public Shared Function GetOracleDBlist(ByVal strServerName As String, ByVal strUserName As String, ByVal strPassword As String) As List(Of String)
        Try
            If DBLayer.DBLInstance.chkCS4Oracle("Data Source = " + strServerName + ";User ID=" + strUserName + ";password=" + strPassword + ";") = "Connection Successed" Then
                DBLayer.DBLInstance.ConnectionStr = "Data Source = " + strServerName + ";User ID=" + strUserName + ";password=" + strPassword + ";"
                Dim lstItems As New List(Of String)
                lstItems = DBLayer.DBLInstance.ReadAllDB()
                DBLayer.DBLInstance.ConnectionStr = ConnectionString()
                Return lstItems
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return Nothing
        End Try
    End Function
    Public Shared Function GetServerName() As List(Of String)
        Dim lstItems As New List(Of String)
        Dim instance As SqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance
        Dim table As System.Data.DataTable = instance.GetDataSources()
        For Each row As DataRow In table.Rows
            lstItems.Add(row(0) + "\" + row(1))
        Next
        Return lstItems
    End Function
#End Region

#Region "Users & Groups"



#Region "ECMLogin details"
    Public Shared Function InsertAndUpdateeZECMLoginWithUserInfo(ByVal ObjeZECMLogin As OldeZECMLogin, ByVal ObjeZECMUserInfo As eZECMUserInfo) As String
        Dim exc As String = ""
        Try
            Dim objEmp As IOldeZECMLogin = Nothing
            If ObjeZECMLogin.ECMLoginId = 0 Then
                Try
                    objEmp = DBLayer.DBLInstance.CreateeZECMLogin(ObjeZECMLogin)
                    exc = "Success"
                Catch ex As Exception
                    exc = "ERROR CODE:WDBR300F100DB10 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            Else
                Try
                    objEmp = DBLayer.DBLInstance.GlobalInstance.eZECMLogin(ObjeZECMLogin.ECMLoginId)
                    objEmp = ObjeZECMLogin
                    objEmp.SaveChanges()
                    exc = "Success"
                Catch ex As Exception
                    exc = "ERROR CODE:WDBR300F100DB20 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            End If
            If objEmp IsNot Nothing Then
                ObjeZECMUserInfo.ECMLoginId = objEmp.ECMLoginId
                Dim objEmp1 As IeZECMUserInfo = Nothing
                If ObjeZECMUserInfo.UserId = 0 Then
                    Try
                        objEmp1 = DBLayer.DBLInstance.CreateeZECMUserInfo(ObjeZECMUserInfo)
                        exc = "Success"
                    Catch ex As Exception
                        exc = "ERROR CODE:WDBR300F100DB11 " + ex.ToString()
                        Throw New FaultException(exc)
                    End Try
                Else
                    Try
                        objEmp1 = DBLayer.DBLInstance.GlobalInstance.eZECMUserInfo(ObjeZECMUserInfo.UserId)
                        objEmp1 = ObjeZECMUserInfo
                        objEmp1.SaveChanges()
                        exc = "Success"
                    Catch ex As Exception
                        exc = "ERROR CODE:WDBR300F100DB21 " + ex.ToString()
                        Throw New FaultException(exc)
                    End Try
                End If
                If objEmp1 IsNot Nothing Then
                    ' exc = objEmp1.ECMLoginId
                    Return exc
                Else
                    exc = "Record is not Added due to some error!"
                End If
            End If
        Catch ex As Exception
            exc = "ERROR CODE:WSR300F100 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
        Return exc
    End Function
    Public Shared Function InsertAndUpdateeZECMLogin(ByVal Obj As OldeZECMLogin) As String
        Dim exc As String = ""
        Try
            Dim objEmp As IOldeZECMLogin = Nothing
            If Obj.ECMLoginId = 0 Then
                Try
                    objEmp = DBLayer.DBLInstance.CreateeZECMLogin(Obj)
                    exc = "New User Login Details Created Successfully!"
                Catch ex As Exception
                    exc = "ERROR CODE:WDBR300F200DB10 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            Else
                Try
                    objEmp = DBLayer.DBLInstance.GlobalInstance.eZECMLogin(Obj.ECMLoginId)
                    objEmp = Obj
                    objEmp.SaveChanges()
                    exc = "User Detail Updated Successfully!"
                Catch ex As Exception
                    exc = "ERROR CODE:WDBR300F200DB20 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            End If
            If objEmp IsNot Nothing Then
                exc = objEmp.ECMLoginId
                Return exc
            Else
                exc = "Record is not Added due to some error!"
            End If
        Catch ex As Exception
            exc = "ERROR CODE:WSR300F200 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
        Return exc
    End Function
    Public Shared Function UpdateeZECMLoginPassword(ECMLoginId As Integer, Password As String) As String
        Dim exc As String = ""
        Try
            Return DBLayer.DBLInstance.UpdateeZECMLoginPassword(ECMLoginId, Password)
        Catch ex As Exception
            exc = "ERROR CODE:WDBR300F300DB20 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
        Return "Password Changed Successfully!"
    End Function
    Public Shared Function UpdateeZLanguage(ECMLoginId As Integer, LanguageId As Integer) As String
        Dim exc As String = ""
        Try
            Return DBLayer.DBLInstance.UpdateeZLanguage(ECMLoginId, LanguageId)
        Catch ex As Exception
            exc = "ERROR CODE:WDBR300F400DB20 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
        Return "Language Changed Successfully!"
    End Function
    'Public Shared Function ValidateActiveDirectoryLogin(ByVal Username As String, ByVal Password As String) As Boolean
    '    Dim Success As Boolean = False
    '    Dim a = ConfigurationManager.AppSettings("LDAPPath")
    '    Dim Entry As New System.DirectoryServices.DirectoryEntry(ConfigurationManager.AppSettings("LDAPPath"), Username, Password)
    '    Dim Searcher As New System.DirectoryServices.DirectorySearcher(Entry)
    '    Searcher.SearchScope = DirectoryServices.SearchScope.OneLevel
    '    Try
    '        Dim Results As System.DirectoryServices.SearchResult = Searcher.FindOne
    '        Success = Not (Results Is Nothing)
    '    Catch
    '        Success = False
    '    End Try
    '    Return Success
    'End Function
    'Public Function UserLogin(ByVal UserName As String, ByVal PW As String, ByVal Logged1 As Integer, ByVal LoggedFrom As String, ByVal LoggedAt As String) As eZECMLogin Implements ICACservice.UserLogin
    '    Dim objUserLogin As IOldeZECMLogin = Nothing
    '    Dim Session As New eZUserSession
    '    Try
    '        Dim isaduser As Boolean = False
    '        Dim loginid As Integer = 0
    '        Try
    '            loginid = GetLoginIdByUsername(UserName, isaduser)
    '        Catch ex As Exception
    '            Dim exc As String = ""
    '            exc = "ERROR CODE:WDBR300F500DB30 " + ex.ToString()
    '            Throw New FaultException(exc)
    '        End Try
    '        If Not loginid = 0 Then
    '            If isaduser = False Then
    '                If Logged1 = 1 Then
    '                    objUserLogin = New eZECMLogin(UserName, PW)
    '                    If objUserLogin.IsECMLoginExist And objUserLogin.Pasword.Equals(PW) Then
    '                        Dim obj As IOldeZECMLogin = New eZECMLogin(objUserLogin.ECMLoginId)
    '                        Session.ECMLoginId = objUserLogin.ECMLoginId
    '                        Session.Logged = Logged1
    '                        Session.CreatedBy = objUserLogin.ECMLoginId
    '                        Session.CreatedOn = DateDateTimeToString(Date.Now, True)
    '                        Session.LoggedAt = LoggedAt
    '                        Session.LoggedFrom = LoggedFrom
    '                        Try
    '                            InsertAndUpdateeZUserSession(Session)
    '                        Catch ex As Exception
    '                            Dim exc As String = ""
    '                            exc = "ERROR CODE:WDBR300F500DB10 " + ex.ToString()
    '                            Throw New FaultException(exc)
    '                        End Try
    '                    Else
    '                        objUserLogin = Nothing
    '                    End If
    '                Else
    '                    objUserLogin = New eZECMLogin(loginid)
    '                    Session.ECMLoginId = loginid
    '                    Session.Logged = Logged1
    '                    Session.CreatedBy = loginid
    '                    Session.CreatedOn = DateDateTimeToString(Date.Now, True)
    '                    Session.LoggedAt = LoggedAt
    '                    Session.LoggedFrom = LoggedFrom
    '                    Try
    '                        InsertAndUpdateeZUserSession(Session)
    '                    Catch ex As Exception
    '                        Dim exc As String = ""
    '                        exc = "ERROR CODE:WDBR300F500DB11 " + ex.ToString()
    '                        Throw New FaultException(exc)
    '                    End Try
    '                End If
    '            Else
    '                Dim qry = "select ld.ldapdomain from ezldapconnection ld left join ezadusers ad " +
    '                    "on ld.ldapconnid=ad.ldapconnid where ad.samaccountname=N'" + UserName + "'"
    '                Dim ds = GetDatasetByQuery(qry)
    '                Dim domain = ""
    '                If ds.Tables.Count > 0 Then
    '                    If ds.Tables(0).Rows.Count > 0 Then
    '                        domain = ds.Tables(0).Rows(0)(0).ToString
    '                    End If
    '                End If
    '                If domain <> "" Then
    '                    If ValidateAdCredentials("", domain, UserName, PW) Then
    '                        objUserLogin = DBLayer.DBLInstance.GlobalInstance.eZECMLogin(loginid)
    '                        objUserLogin.Pasword = PW
    '                        ' Dim Session As New eZUserSession
    '                        Session.ECMLoginId = loginid
    '                        Session.Logged = 1
    '                        Session.CreatedBy = loginid
    '                        Session.CreatedOn = DateDateTimeToString(Date.Now, True)
    '                        Session.LoggedAt = LoggedAt
    '                        Session.LoggedFrom = LoggedFrom
    '                        Try
    '                            InsertAndUpdateeZUserSession(Session)
    '                        Catch ex As Exception
    '                            Dim exc As String = ""
    '                            exc = "ERROR CODE:WDBR300F500DB12 " + ex.ToString()
    '                            Throw New FaultException(exc)
    '                        End Try
    '                    Else
    '                        objUserLogin = Nothing
    '                    End If
    '                Else
    '                    Throw New FaultException("Domain Name Not Available")
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Dim exc As String = ""
    '        exc = "ERROR CODE:WSR300F500 " + ex.ToString()
    '        Throw New FaultException(exc)
    '    End Try
    '    Return objUserLogin
    'End Function
    'Public Shared Function UserLogin(ByVal UserName As String, ByVal PW As String) As IOldeZECMLogin
    '    Dim objUserLogin As IOldeZECMLogin = Nothing
    '    'Dim Session As New eZUserSession
    '    Try
    '        Dim loginid As Integer = 0
    '        Try
    '            loginid = GetLoginIdByUsername(UserName)
    '        Catch ex As Exception
    '            Dim exc As String = ""
    '            exc = "ERROR CODE:WDBR300F500DB30 " + ex.ToString()
    '            Throw New FaultException(exc)
    '        End Try
    '        If Not loginid = 0 Then
    '            objUserLogin = New OldeZECMLogin(UserName, PW)
    '            If objUserLogin.IsECMLoginExist And objUserLogin.Pasword.Equals(PW) Then
    '                ' Dim Obj = New eZECMLogin(objUserLogin.ECMLoginId)
    '                Dim Obj = selectedeZECMGroupusersList("ECMLoginId", objUserLogin.ECMLoginId)
    '                For i As Integer = 0 To Obj.Count - 1
    '                    If i = Obj.Count - 1 Then
    '                        objUserLogin.ECMGroupList += Obj(i).ECMGroupId.ToString
    '                    Else
    '                        objUserLogin.ECMGroupList += Obj(i).ECMGroupId.ToString + ","
    '                    End If

    '                Next
    '                Return objUserLogin
    '            Else
    '                objUserLogin = Nothing
    '            End If
    '        Else
    '            objUserLogin = Nothing
    '        End If

    '    Catch ex As Exception
    '        Dim exc As String = ""
    '        exc = "ERROR CODE:WSR300F500 " + ex.ToString()
    '        Throw New FaultException(exc)
    '    End Try
    '    Return objUserLogin
    'End Function

    Public Shared Function UserLogin(ByVal UserName As String, ByVal PW As String) As OldeZECMLogin
        Dim objUserLogin As IOldeZECMLogin = Nothing
        Dim Session As New eZUserSession
        Try
            Dim isaduser As Boolean = False
            Dim loginid As Integer = 0
            Try
                loginid = GetLoginIdByUsername(UserName, isaduser)
            Catch ex As Exception
                Dim exc As String = ""
                exc = "ERROR CODE:WDBR300F500DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            If Not loginid = 0 Then
                If isaduser = False Then
                    ' If Logged1 = 1 Then
                    objUserLogin = New OldeZECMLogin(UserName, PW)
                    If objUserLogin.IsECMLoginExist And objUserLogin.Pasword.Equals(PW) Then
                        Dim obj As IOldeZECMLogin = New OldeZECMLogin(objUserLogin.ECMLoginId)
                        Session.ECMLoginId = objUserLogin.ECMLoginId
                        Session.Logged = 1
                        Session.CreatedBy = objUserLogin.ECMLoginId
                        Session.CreatedOn = DateDateTimeToString(Date.Now, True)
                        Session.loggedat = "Asset Inventry"
                        Session.loggedfrom = "Lab Owner"
                        'Try
                        '    InsertAndUpdateeZUserSession(Session)
                        'Catch ex As Exception
                        '    Dim exc As String = ""
                        '    exc = "ERROR CODE:WDBR300F500DB10 " + ex.ToString()
                        '    Throw New FaultException(exc)
                        'End Try
                        Dim Objgr = selectedeZECMGroupusersList("ECMLoginId", objUserLogin.ECMLoginId)
                        For i As Integer = 0 To Objgr.Count - 1
                            If i = Objgr.Count - 1 Then
                                objUserLogin.ECMGroupList += Objgr(i).ECMGroupId.ToString
                            Else
                                objUserLogin.ECMGroupList += Objgr(i).ECMGroupId.ToString + ","
                            End If

                        Next
                    Else
                        objUserLogin = Nothing
                    End If
                    'Else
                    '    objUserLogin = New OldeZECMLogin(loginid)
                    '    Session.ECMLoginId = loginid
                    '    Session.Logged = Logged1
                    '    Session.CreatedBy = loginid
                    '    Session.CreatedOn = DateDateTimeToString(Date.Now, True)
                    '    Session.LoggedAt = ""
                    '    Session.LoggedFrom = ""
                    '    Try
                    '        InsertAndUpdateeZUserSession(Session)
                    '    Catch ex As Exception
                    '        Dim exc As String = ""
                    '        exc = "ERROR CODE:WDBR300F500DB11 " + ex.ToString()
                    '        Throw New FaultException(exc)
                    '    End Try
                    'End If
                Else
                    Dim qry = "select ld.ldapdomain from ezldapconnection ld left join ezadusers ad " +
                        "on ld.ldapconnid=ad.ldapconnid where ad.samaccountname=N'" + UserName + "'"
                    Dim ds = GetDatasetByQuery(qry)
                    Dim domain = ""
                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            domain = ds.Tables(0).Rows(0)(0).ToString
                        End If
                    End If
                    If domain <> "" Then
                        If ValidateAdCredentials("", domain, UserName, PW) Then
                            objUserLogin = DBLayer.DBLInstance.GlobalInstance.eZECMLogin(loginid)
                            objUserLogin.Pasword = PW
                            ' Dim Session As New eZUserSession
                            Session.ECMLoginId = loginid
                            Session.Logged = 1
                            Session.CreatedBy = loginid
                            Session.CreatedOn = DateDateTimeToString(Date.Now, True)
                            Session.loggedat = "Asset Inventry"
                            Session.loggedfrom = "Lab Owner"
                            Try
                                ' InsertAndUpdateeZUserSession(Session)
                            Catch ex As Exception
                                Dim exc As String = ""
                                exc = "ERROR CODE:WDBR300F500DB12 " + ex.ToString()
                                Throw New FaultException(exc)
                            End Try
                            Dim Objgr = selectedeZECMGroupusersList("ECMLoginId", objUserLogin.ECMLoginId)
                            For i As Integer = 0 To Objgr.Count - 1
                                If i = Objgr.Count - 1 Then
                                    objUserLogin.ECMGroupList += Objgr(i).ECMGroupId.ToString
                                Else
                                    objUserLogin.ECMGroupList += Objgr(i).ECMGroupId.ToString + ","
                                End If

                            Next
                        Else
                            objUserLogin = Nothing
                        End If
                    Else
                        Throw New FaultException("Domain Name Not Available")
                    End If
                End If
            End If
        Catch ex As Exception
            Dim exc As String = ""
            exc = "ERROR CODE:WSR300F500 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
        Return objUserLogin
    End Function

    Public Shared Function ValidateAdCredentials(ByVal Server As String, ByVal Domain As String, ByVal Username As String, ByVal Password As String) As Boolean
        Dim res As Boolean = False
        Try
            Dim serv As String = ""
            If Server = "" Then
                serv = Domain
            Else
                serv = Server + "." + Domain
            End If
            Using pc As New PrincipalContext(ContextType.Domain, Domain)
                res = pc.ValidateCredentials(Username, Password)
            End Using
        Catch ex As Exception
            '  Throw New FaultException("ERROR CODE : WDBRJ1000F1200 : " + ex.ToString)
        End Try
        Return res
    End Function

    Public Shared Function GetLoginIdByUsername(ByVal UserName As String, ByRef isaduser As Boolean) As Integer
        Try
            Dim Lst1 As New List(Of IOldeZECMLogin)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZECMLogin("LoginName", UserName)
            Catch ex As Exception
                Dim exc As String = ""
                exc = "ERROR CODE:WDBR300F800DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of OldeZECMLogin)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    isaduser = Lst1(i).IsADUser
                    Return Lst1(i).ECMLoginId
                Next
            End If
        Catch ex As Exception
            Dim exc As String = ""
            exc = "ERROR CODE:WSR300F800 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function

    Public Shared Function eZECMLoginList() As List(Of OldeZECMLogin)
        Try
            Dim Lst1 As New List(Of IOldeZECMLogin)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadAlleZECMLogin()
            Catch ex As Exception
                Dim exc As String = ""
                exc = "ERROR CODE:WDBR300F600DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of OldeZECMLogin)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New OldeZECMLogin
                    lst.ECMLoginId = Lst1(i).ECMLoginId
                    'lst.ECMGroupId = Lst1(i).ECMGroupId
                    lst.ECMUserTypeId = Lst1(i).ECMUserTypeId
                    lst.LanguageId = Lst1(i).LanguageId
                    lst.Signatureid = Lst1(i).Signatureid
                    lst.Chart2 = Lst1(i).Chart2
                    lst.Chart1 = Lst1(i).Chart1
                    lst.Chart3 = Lst1(i).Chart3
                    lst.IsADUser = Lst1(i).IsADUser
                    lst.IsFaxUser = Lst1(i).IsFaxUser
                    lst.LoginName = Lst1(i).LoginName
                    'lst.ECMGroup = Lst1(i).ECMGroup
                    lst.Pasword = Lst1(i).Pasword
                    lst.ECMProfileId = Lst1(i).ECMProfileId
                    lst.ECMProfile = Lst1(i).ECMProfile
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String = ""
            exc = "ERROR CODE:WSR300F600 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function FilteredeZECMLoginList(ByVal Criteria As String, ByVal Value As String) As List(Of OldeZECMLogin)
        Try
            Dim Lst1 As New List(Of IOldeZECMLogin)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadFilteredeZECMLogin(Criteria, Value)
            Catch ex As Exception
                Dim exc As String = ""
                exc = "ERROR CODE:WDBR300F700DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of OldeZECMLogin)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New OldeZECMLogin
                    lst.ECMLoginId = Lst1(i).ECMLoginId
                    'lst.ECMGroupId = Lst1(i).ECMGroupId
                    lst.ECMUserTypeId = Lst1(i).ECMUserTypeId
                    lst.IsFaxUser = Lst1(i).IsFaxUser
                    lst.Signatureid = Lst1(i).Signatureid
                    lst.Chart2 = Lst1(i).Chart2
                    lst.Chart1 = Lst1(i).Chart1
                    lst.Chart3 = Lst1(i).Chart3
                    lst.LanguageId = Lst1(i).LanguageId
                    lst.IsADUser = Lst1(i).IsADUser
                    lst.LoginName = Lst1(i).LoginName
                    'lst.ECMGroup = Lst1(i).ECMGroup
                    lst.Pasword = Lst1(i).Pasword
                    lst.ECMProfileId = Lst1(i).ECMProfileId
                    lst.ECMProfile = Lst1(i).ECMProfile
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String = ""
            exc = "ERROR CODE:WSR300F700 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function GetLoginIdByUsername(ByVal UserName As String) As Integer
        Try
            Dim Lst1 As New List(Of IOldeZECMLogin)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZECMLogin("LoginName", UserName)
            Catch ex As Exception
                Dim exc As String = ""
                exc = "ERROR CODE:WDBR300F800DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of OldeZECMLogin)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Return Lst1(i).ECMLoginId
                Next
            End If
        Catch ex As Exception
            Dim exc As String = ""
            exc = "ERROR CODE:WSR300F800 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function GetECMUserTypeByLoginId(ByVal ECMLoginId As Integer) As Integer
        Try
            Dim Lst1 As New List(Of IOldeZECMLogin)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZECMLogin("ECMLoginId", ECMLoginId.ToString)
            Catch ex As Exception
                Dim exc As String = ""
                exc = "ERROR CODE:WDBR300F900DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of OldeZECMLogin)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    ''dsc
                    'If Lst1(i).ECMUserTypeId = 1 Then
                    '    Lst1(i).ECMUserTypeId = 2
                    'End If
                    Return Lst1(i).ECMUserTypeId
                Next
            End If
        Catch ex As Exception
            Dim exc As String = ""
            exc = "ERROR CODE:WSR300F900 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function SelectedeZECMLoginList(ByVal Criteria As String, ByVal Value As String) As List(Of OldeZECMLogin)
        Try
            Dim Lst1 As New List(Of IOldeZECMLogin)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZECMLogin(Criteria, Value)
            Catch ex As Exception
                Dim exc As String = ""
                exc = "ERROR CODE:WDBR300F110DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of OldeZECMLogin)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New OldeZECMLogin
                    lst.ECMLoginId = Lst1(i).ECMLoginId
                    'lst.ECMGroupId = Lst1(i).ECMGroupId
                    lst.ECMUserTypeId = Lst1(i).ECMUserTypeId
                    lst.LanguageId = Lst1(i).LanguageId
                    lst.IsFaxUser = Lst1(i).IsFaxUser
                    lst.Signatureid = Lst1(i).Signatureid
                    lst.Chart2 = Lst1(i).Chart2
                    lst.Chart1 = Lst1(i).Chart1
                    lst.Chart3 = Lst1(i).Chart3
                    lst.IsADUser = Lst1(i).IsADUser
                    lst.LoginName = Lst1(i).LoginName
                    'lst.ECMGroup = Lst1(i).ECMGroup
                    lst.Pasword = Lst1(i).Pasword
                    lst.ECMProfileId = Lst1(i).ECMProfileId
                    lst.ECMProfile = Lst1(i).ECMProfile
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String = ""
            exc = "ERROR CODE:WSR300F110 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function DeleteeZECMLogin(ByVal CabinetID As Integer) As String
        Try
            Dim objemp As IOldeZECMLogin = Nothing
            objemp = DBLayer.DBLInstance.GlobalInstance.eZECMLogin(CabinetID)
            Dim emp As New OldeZECMLogin()
            If objemp IsNot Nothing Then
                DBLayer.DBLInstance.Delete(objemp)
            End If
            DeleteeZECMLogin = "User Removed Successfully!"
        Catch ex As Exception
            Dim exc As String = ""
            exc = "ERROR CODE:WDBR300F120DB40 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    'Private _directoryEntry As DirectoryEntry = Nothing
    'Private ReadOnly Property SearchRoot() As DirectoryEntry
    '    Get
    '        If _directoryEntry Is Nothing Then
    '            _directoryEntry = New DirectoryEntry(ConfigurationManager.AppSettings("LDAPPath"), ConfigurationManager.AppSettings("LDAPUser"), ConfigurationManager.AppSettings("LDAPPassword"), AuthenticationTypes.Secure)
    '        End If
    '        Return _directoryEntry
    '    End Get
    'End Property
    'Public Shared Function GetADUserList() As List(Of eZECMLogin)
    '    Dim ListItems As New List(Of eZECMLogin)()
    '    Try
    '        Try
    '            ListItems = SelectedeZECMLoginList("IsADUser", "False")
    '        Catch ex As Exception
    '            Dim exc As String = ""
    '            exc = "ERROR CODE:WDBR300F130DB30 " + ex.ToString()
    '            Throw New FaultException(exc)
    '        End Try
    '        _directoryEntry = Nothing
    '        Dim directorySearch As New DirectorySearcher(SearchRoot)
    '        directorySearch.Filter = "(&(objectClass=user))"
    '        Dim results As SearchResultCollection = directorySearch.FindAll()
    '        If results.Count <> 0 Then
    '            For Each objResult As SearchResult In results
    '                Dim obj As New eZECMLogin
    '                Dim user As New DirectoryEntry(objResult.Path, ConfigurationManager.AppSettings("LDAPUser"), ConfigurationManager.AppSettings("LDAPPassword"))
    '                If user.Properties.Contains("userPrincipalName") Then
    '                    obj.LoginName = user.Properties("userPrincipalName")(0).ToString()
    '                    ListItems.Add(obj)
    '                End If
    '            Next
    '            Return ListItems
    '        Else
    '            Throw New Exception("No groups found")
    '        End If
    '    Catch ex As Exception
    '        Dim exc As String = ""
    '        exc = "ERROR CODE:WSR300F130 " + ex.ToString()
    '        Throw New FaultException(exc)
    '    End Try
    '    Return ListItems
    'End Function
    'Public Shared Function GeteZECMUserInfoByLoginName(LoginName As String) As eZECMUserInfo
    '    Try
    '        Dim obj As New eZECMUserInfo
    '        _directoryEntry = Nothing
    '        Dim directorySearch As New DirectorySearcher(SearchRoot)
    '        directorySearch.Filter = "(&(objectClass=user)(userPrincipalName=" + LoginName + "))"
    '        Dim results As SearchResult = directorySearch.FindOne()
    '        If results IsNot Nothing Then
    '            Dim user As New DirectoryEntry(results.Path, ConfigurationManager.AppSettings("LDAPUser"), ConfigurationManager.AppSettings("LDAPPassword"))
    '            If user.Properties.Contains("givenName") Then
    '                obj.FirstName = user.Properties("givenName")(0).ToString()
    '            End If
    '            If user.Properties.Contains("mail") Then
    '                obj.EmailAddress = user.Properties("mail")(0).ToString()
    '            End If
    '            If user.Properties.Contains("mobile") Then
    '                obj.Mobile = user.Properties("mobile")(0).ToString()
    '            End If
    '            Return obj
    '        Else
    '            Return Nothing
    '        End If
    '        Return Nothing
    '    Catch ex As Exception
    '        Dim exc As String = ""
    '        exc = "ERROR CODE:WSR300F140 " + ex.ToString()
    '        Throw New FaultException(exc)
    '    End Try
    'End Function
    'Public Shared Sub SyncADUser(ByVal CreatedBy As Integer)
    '    Dim eZECMLoginList As New List(Of eZECMLogin)()
    '    Try
    '        _directoryEntry = Nothing
    '        Dim directorySearch As New DirectorySearcher(SearchRoot)
    '        directorySearch.Filter = "(&(objectClass=user))"
    '        Dim results As SearchResultCollection = directorySearch.FindAll()
    '        If results.Count <> 0 Then
    '            For Each objResult As SearchResult In results
    '                Dim obj As New eZECMLogin
    '                Dim user As New DirectoryEntry(objResult.Path, ConfigurationManager.AppSettings("LDAPUser"), ConfigurationManager.AppSettings("LDAPPassword"))
    '                If user.Properties.Contains("userPrincipalName") Then
    '                    obj.LoginName = user.Properties("userPrincipalName")(0).ToString()
    '                    'obj.Pasword = user.Properties("userPassword")(0).ToString
    '                    Try
    '                        eZECMLoginList.Add(obj)
    '                    Catch ex As Exception
    '                        Dim exc As String = ""
    '                        exc = "ERROR CODE:WDBR300F150DB30 " + ex.ToString()
    '                        Throw New FaultException(exc)
    '                    End Try
    '                End If
    '            Next
    '            If eZECMLoginList.Count <> 0 Then
    '                For i As Integer = 0 To eZECMLoginList.Count - 1
    '                    Dim eZECMUserInfoList As New eZECMUserInfo
    '                    Try
    '                        eZECMUserInfoList = GeteZECMUserInfoByLoginName(eZECMLoginList(i).LoginName)
    '                    Catch ex As Exception
    '                        Dim exc As String = ""
    '                        exc = "ERROR CODE:WDBR300F150DB31 " + ex.ToString()
    '                        Throw New FaultException(exc)
    '                    End Try
    '                    If Not eZECMUserInfoList Is Nothing Then
    '                        eZECMLoginList(i).IsADUser = True
    '                        eZECMLoginList(i).Pasword = "xxx"
    '                        eZECMLoginList(i).ECMUserTypeId = 3
    '                        eZECMLoginList(i).CreatedBy = CreatedBy
    '                        eZECMLoginList(i).UpdatedBy = CreatedBy
    '                        eZECMLoginList(i).CreatedOn = DateDateTimeToString(Date.Now, True)
    '                        eZECMLoginList(i).UpdatedOn = DateDateTimeToString(Date.Now, True)
    '                        Try
    '                            InsertAndUpdateeZECMLoginWithUserInfo(eZECMLoginList(i), eZECMUserInfoList)
    '                        Catch ex As Exception
    '                            Dim exc As String = ""
    '                            exc = "ERROR CODE:WDBR300F150DB10 " + ex.ToString()
    '                            Throw New FaultException(exc)
    '                        End Try
    '                    End If
    '                Next
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Dim exc As String = ""
    '        exc = "ERROR CODE:WSR300F150 " + ex.ToString()
    '        Throw New FaultException(exc)
    '    End Try
    'End Sub
#End Region

#Region "ECMUserInfo details"
    Public Shared Function InsertAndUpdateeZECMUserInfo(ByVal Obj As eZECMUserInfo) As String
        Dim exc As String = ""
        Dim objEmp As IeZECMUserInfo = Nothing
        If Obj.UserId = 0 Then
            Try
                objEmp = DBLayer.DBLInstance.CreateeZECMUserInfo(Obj)
                If objEmp IsNot Nothing Then
                    exc = "New User Added Successfully!"
                End If
            Catch ex As Exception
                exc = "ERROR CODE:WDBR200F100DB10 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
        Else
            Try
                objEmp = DBLayer.DBLInstance.GlobalInstance.eZECMUserInfo(Obj.UserId)
                objEmp = Obj
                objEmp.SaveChanges()
                If objEmp IsNot Nothing Then
                    exc = "New User updated Successfully!"
                Else
                    exc = "ERROR CODE:WDBR200F100DB20"
                End If
            Catch ex As Exception
                exc = "ERROR CODE:WDBR200F100DB20 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
        End If
        Return exc
    End Function
    Public Shared Function eZECMUserInfoList() As List(Of eZECMUserInfo)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZECMUserInfo)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadAlleZECMUserInfo()
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F200DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZECMUserInfo)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZECMUserInfo
                    lst.UserId = Lst1(i).UserId
                    lst.ECMLoginId = Lst1(i).ECMLoginId
                    lst.Mobile = Lst1(i).Mobile
                    lst.EmailAddress = Lst1(i).EmailAddress
                    lst.UserId = Lst1(i).UserId
                    lst.FirstName = Lst1(i).FirstName
                    lst.Department = Lst1(i).Department
                    lst.Manager = Lst1(i).Manager
                    lst.ManagerName = Lst1(i).ManagerName
                    lst.Designation = Lst1(i).Designation
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F200 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function FilteredeZECMUserInfoList(ByVal Criteria As String, ByVal Value As String) As List(Of eZECMUserInfo)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZECMUserInfo)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadFilteredeZECMUserInfo(Criteria, Value)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F300DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZECMUserInfo)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZECMUserInfo
                    lst.UserId = Lst1(i).UserId
                    lst.ECMLoginId = Lst1(i).ECMLoginId
                    lst.Mobile = Lst1(i).Mobile
                    lst.EmailAddress = Lst1(i).EmailAddress
                    lst.UserId = Lst1(i).UserId
                    lst.FirstName = Lst1(i).FirstName
                    lst.Department = Lst1(i).Department
                    lst.Manager = Lst1(i).Manager
                    lst.ManagerName = Lst1(i).ManagerName
                    lst.Designation = Lst1(i).Designation
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F300 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function SelectedeZECMUserInfoList(ByVal Criteria As String, ByVal Value As String) As List(Of eZECMUserInfo)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZECMUserInfo)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadSelectedeZECMUserInfo(Criteria, Value)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F400DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZECMUserInfo)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZECMUserInfo
                    lst.UserId = Lst1(i).UserId
                    lst.ECMLoginId = Lst1(i).ECMLoginId
                    lst.Mobile = Lst1(i).Mobile
                    lst.EmailAddress = Lst1(i).EmailAddress
                    lst.UserId = Lst1(i).UserId
                    lst.FirstName = Lst1(i).FirstName
                    lst.Department = Lst1(i).Department
                    lst.Manager = Lst1(i).Manager
                    lst.ManagerName = Lst1(i).ManagerName
                    lst.Designation = Lst1(i).Designation
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F400 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function DeleteeZECMUserInfo(ByVal CabinetID As Integer) As String
        Dim faultmsg As String
        Try
            Dim objemp As IeZECMUserInfo = Nothing
            objemp = DBLayer.DBLInstance.GlobalInstance.eZECMUserInfo(CabinetID)
            Dim emp As New eZECMUserInfo()
            If objemp IsNot Nothing Then
                DBLayer.DBLInstance.Delete(objemp)
            End If
            DeleteeZECMUserInfo = "User Removed Successfully!"
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F500DB40 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
    End Function
#End Region


#Region "eZECMGroupUsers"
    Public Shared Function InsertAndUpdateeZECMGroupusers(ByVal Obj As eZECMGroupusers) As String
        Dim exc As String = ""
        Try
            Dim objEmp As IeZECMGroupusers = Nothing
            If Obj.ECMGroupUserId = 0 Then
                Try
                    objEmp = DBLayer.DBLInstance.CreateeZECMGroupusers(Obj)
                Catch ex As Exception
                    Throw New FaultException("ERROR CODE:WDBR701F100DB10 " + ex.ToString())
                End Try
            Else
                Try
                    objEmp = DBLayer.DBLInstance.GlobalInstance.eZECMGroupusers(Obj.ECMGroupUserId)
                    objEmp = Obj
                    objEmp.SaveChanges()
                Catch ex As Exception
                    Throw New FaultException("ERROR CODE:WDBR701F100DB30 " + ex.ToString())
                End Try
            End If
            If objEmp IsNot Nothing Then
                exc = objEmp.ECMGroupUserId.ToString
            Else
                exc = "0"
            End If
        Catch ex As Exception
            Throw New FaultException("ERROR CODE:WSR701F100 " + ex.ToString())
        End Try
        Return exc
    End Function
    Public Shared Function eZECMGroupusersList() As List(Of eZECMGroupusers)
        Try
            Dim Lst1 As New List(Of IeZECMGroupusers)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadAlleZECMGroupusers()
            Catch ex As Exception
                Dim exc As String = "ERROR CODE:WDBR701F200DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of eZECMGroupusers)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZECMGroupusers
                    lst.ECMGroupUserId = Lst1(i).ECMGroupUserId
                    lst.ECMGroupId = Lst1(i).ECMGroupId
                    lst.ECMLoginId = Lst1(i).ECMLoginId
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).Createdby1
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).updatedby1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String = "ERROR CODE:WSR701F200 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function selectedeZECMGroupusersList(Criteria As String, value As String) As List(Of eZECMGroupusers)
        Try
            Dim Lst1 As New List(Of IeZECMGroupusers)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZECMGroupusers(Criteria, value.ToString())
            Catch ex As Exception
                Dim exc As String = "ERROR CODE:WDBR701F300DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of eZECMGroupusers)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZECMGroupusers
                    lst.ECMGroupUserId = Lst1(i).ECMGroupUserId
                    lst.ECMGroupId = Lst1(i).ECMGroupId
                    lst.ECMLoginId = Lst1(i).ECMLoginId
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).Createdby1
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).updatedby1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String = "ERROR CODE:WSR701F300 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function FilteredeZECMGroupusersList(Criteria As String, value As String) As List(Of eZECMGroupusers)
        Try
            Dim Lst1 As New List(Of IeZECMGroupusers)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadFilteredeZECMGroupusers(Criteria, value.ToString())
            Catch ex As Exception
                Dim exc As String = ""
                exc = "ERROR CODE:WDBR701F300DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of eZECMGroupusers)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZECMGroupusers
                    lst.ECMGroupUserId = Lst1(i).ECMGroupUserId
                    lst.ECMGroupId = Lst1(i).ECMGroupId
                    lst.ECMLoginId = Lst1(i).ECMLoginId
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).Createdby1
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).updatedby1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String = ""
            exc = "ERROR CODE:WSR701F300 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function DeleteeZECMGroupusers(ByVal ECMGroupuserID As Integer) As String
        Try
            Dim objemp As IeZECMGroup = Nothing
            Try
                objemp = DBLayer.DBLInstance.GlobalInstance.eZECMGroup(ECMGroupuserID)
            Catch ex As Exception
                Dim exc As String = ""
                exc = "ERROR CODE:WDBR400F300DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim emp As New eZECMGroup()
            If objemp IsNot Nothing Then
                Try
                    DBLayer.DBLInstance.Delete(objemp)
                    DeleteeZECMGroupusers = "Group Deleted Successfully!"
                Catch ex As Exception
                    Dim exc As String = ""
                    exc = "ERROR CODE:WDBR400F300DB40 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            End If
        Catch ex As Exception
            Dim exc As String = ""
            exc = "ERROR CODE:WSR400F300 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
#End Region
#End Region

#Region "Workflow Details"


    Public Shared Function InsertAndUpdateeZMail(ByVal Obj As eZMail) As String
        Dim exc As String = ""
        Try
            Dim objEmp As IeZMail = Nothing

            If Obj.MailId = 0 Then
                Try
                    objEmp = DBLayer.DBLInstance.CreateZMail(Obj)
                    exc = "Mail Send"
                Catch ex As Exception
                    'Dim exc As String
                    exc = "ERROR CODE:WDBR690F100DB10 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            Else
                Try
                    objEmp = DBLayer.DBLInstance.GlobalInstance.eZMail(Obj.MailId)
                    objEmp = Obj
                    objEmp.SaveChanges()
                Catch ex As Exception
                    ' Dim exc As String
                    exc = "ERROR CODE:WDBR690F100DB20 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            End If

            If objEmp IsNot Nothing Then
                exc = "Record Added!"
                Return exc
            Else
                exc = "Record is not Added due to some error!"
            End If
        Catch ex As Exception
            'Dim exc As String
            exc = "ERROR CODE:WSR690F100 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
        Return exc

    End Function

    Public Shared Function eZWorkflowDetailsList() As List(Of eZWorkflowDetails)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWorkflowDetails)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadAlleZWorkflowDetails()
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F200DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWorkflowDetails)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWorkflowDetails
                    lst.Workflowitemid = Lst1(i).Workflowitemid
                    lst.Status = Lst1(i).Status
                    lst.Workflowid = Lst1(i).Workflowid

                    lst.XMLDS = Lst1(i).XMLDS
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.WorkflowName = Lst1(i).WorkflowName

                    lst.MailSettingsId = Lst1(i).MailSettingsId
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F200 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function FilteredeZWorkflowDetailsList(ByVal Criteria As String, ByVal Value As String) As List(Of eZWorkflowDetails)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWorkflowDetails)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadFilteredeZWorkflowDetails(Criteria, Value)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F300DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWorkflowDetails)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWorkflowDetails
                    lst.Workflowitemid = Lst1(i).Workflowitemid
                    lst.Status = Lst1(i).Status
                    lst.Workflowid = Lst1(i).Workflowid
                    lst.XMLDS = Lst1(i).XMLDS
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.WorkflowName = Lst1(i).WorkflowName

                    lst.MailSettingsId = Lst1(i).MailSettingsId
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F300 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function SelectedeZWorkflowDetailsList(ByVal Criteria As String, ByVal Value As String) As List(Of eZWorkflowDetails)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWorkflowDetails)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadSelectedeZWorkflowDetails(Criteria, Value)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F400DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWorkflowDetails)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWorkflowDetails
                    lst.Workflowitemid = Lst1(i).Workflowitemid
                    lst.Status = Lst1(i).Status
                    lst.Workflowid = Lst1(i).Workflowid

                    lst.XMLDS = Lst1(i).XMLDS
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.WorkflowName = Lst1(i).WorkflowName

                    lst.MailSettingsId = Lst1(i).MailSettingsId
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F400 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function

    Public Shared Function GetWorkflowDetailsByLoginId(ByVal ECMLoginId As String, ECMGroupList As String) As List(Of eZWorkflowDetails)
        Try
            Dim Lst1 As New List(Of IeZWorkflowDetails)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadRunningWorkflowDetailsByLoginId(ECMLoginId)
            Catch ex As Exception
                ' Dim exc As String
                Throw New FaultException("ERROR CODE : WDBRJ500F3700DB10 : " + ex.ToString)
            End Try
            Dim ListItems As New List(Of eZWorkflowDetails)
            For i As Integer = 0 To Lst1.Count - 1
                Dim lst As New eZWorkflowDetails
                lst.Workflowitemid = Lst1(i).Workflowitemid
                lst.Status = Lst1(i).Status
                lst.Workflowid = Lst1(i).Workflowid
                lst.XMLDS = Lst1(i).XMLDS
                lst.Createdby = Lst1(i).Createdby
                lst.Createdon = Lst1(i).Createdon
                lst.Updatedby = Lst1(i).Updatedby
                lst.Updatedon = Lst1(i).Updatedon
                lst.CreatedBy1 = Lst1(i).CreatedBy1
                lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                lst.WorkflowName = Lst1(i).WorkflowName
                lst.MailSettingsId = Lst1(i).MailSettingsId
                lst.FlowTreeInfo = WorkflowProcessCount(Lst1(i).Workflowid.ToString(), ECMLoginId, ECMGroupList)
                ListItems.Add(lst)
            Next
            Return ListItems
        Catch ex As Exception
            Throw New FaultException("ERROR CODE : WDBRJ500F3700 : " + ex.ToString)
        End Try
    End Function


    Public Shared Function ListeZWorkflowDetailsbyCriteria(ByVal Criteria As String, ByVal value As String) As DataSet
        Try
            Dim ds As New DataSet
            Dim GetXml As StreamReader
            Dim str As String
            Dim param As String() = {Criteria, value}
            ds = DBLayer.DBLInstance.GetDatasetByStoredProcedureName("SP_GeteZWorkflowDetailsListbyCondition", param)
            If ds.Tables.Count <> 0 Then
                ds.Tables(0).Columns.Add("XMLstring", Type.GetType("System.String"))
                For L As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    GetXml = New System.IO.StreamReader(ds.Tables(0).Rows(L).Item("iFilePath").ToString())
                    str = GetXml.ReadToEnd
                    GetXml.Close()
                    ' Dim xElem = XElement.Load(ds.Tables(0).Rows(L).Item("iFilePath").ToString())
                    If String.IsNullOrEmpty(str) Then
                        ds.Tables(0).Rows(L).Item("XMLString") = ""
                    Else
                        ds.Tables(0).Rows(L).Item("XMLString") = str.ToString()
                    End If
                Next
            End If
            Return ds
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR1010F300 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function


    Public Shared Function WorkflowProcessCount(Workflowid As String, ECMLoginId As String, ECMGroupList As String) As List(Of FlowNodes)

        Dim result As New List(Of FlowNodes)
        Try

            Dim Node As New FlowNodes
            Node.NodeName = "Inbox"
            Dim QueryInbox = "select count(1) as count from ezwflowtransation w where ((Activityuserid='" + ECMLoginId.ToString + "' and Activitygroupid='0') "
            If ECMGroupList <> "0" And ECMGroupList <> "" Then
                QueryInbox += "or (Activitygroupid in (" + ECMGroupList + ") and Activityuserid='0')"
            End If
            QueryInbox += ") and ActivityId<>'a5d0b578-3ded-40bb-8770-2a5cef442b55' and processid in (select processid from ezwfprocess where flowstatus='Running' and workflowid=" + Workflowid.ToString() + ") and transactionstatus=0  "
            Dim DSInbox As DataSet = GetDatasetByQuery(QueryInbox)
            If Not DSInbox Is Nothing AndAlso DSInbox.Tables.Count > 0 AndAlso DSInbox.Tables(0).Rows.Count > 0 Then
                Node.ProcessCountInfo = DSInbox.Tables(0).Rows(0)(0).ToString
            Else
                Node.ProcessCountInfo = "0"
            End If
            result.Add(Node)

            Node = New FlowNodes
            Node.NodeName = "UnRead"
            Dim QueryUnRead = "select count(1) as count from ezwflowtransation w where ((Activityuserid='" + ECMLoginId.ToString + "' and Activitygroupid='0')"
            If ECMGroupList <> "0" And ECMGroupList <> "" Then
                QueryUnRead += "or (Activitygroupid in (" + ECMGroupList + ") and Activityuserid='0')"
            End If
            QueryUnRead += ") and ActivityId<>'a5d0b578-3ded-40bb-8770-2a5cef442b55' and processid in (select processid from ezwfprocess where flowstatus='Running' and workflowid=" + Workflowid.ToString() + ") and transactionstatus=0 and updatedby=0 "
            Dim DSUnRead As DataSet = GetDatasetByQuery(QueryUnRead)
            If Not DSUnRead Is Nothing AndAlso DSUnRead.Tables.Count > 0 AndAlso DSUnRead.Tables(0).Rows.Count > 0 Then
                Node.ProcessCountInfo = DSUnRead.Tables(0).Rows(0)(0).ToString
            Else
                Node.ProcessCountInfo = "0"
            End If
            result.Add(Node)

            Dim IsOwner As Boolean = False
            Dim QueryOwner = "select * from eZWorkflowUsers where (UserType='Owner' or UserType='Co-Ordinator') and ECMLoginId =" + ECMLoginId.ToString + " and WorkflowId =" + Workflowid.ToString()
            Dim DSOwners As DataSet = GetDatasetByQuery(QueryOwner)
            If Not DSOwners Is Nothing AndAlso DSOwners.Tables.Count > 0 AndAlso DSOwners.Tables(0).Rows.Count > 0 Then
                IsOwner = True
            End If

            Node = New FlowNodes
            Node.NodeName = "Process"
            Dim QueryProcess = "select Count(1) from (select distinct processid from ezwflowtransation where (ActivityUserId=" + ECMLoginId.ToString + " or(ActivityGroupId<>0 and updatedby=" + ECMLoginId.ToString + "))  and Processid in(select Processid from eZWFProcess where FlowStatus='Running' and WorkflowId=" + Workflowid.ToString() + ") and TransactionStatus<>0 and Processid not in(select Processid from eZWFlowTransation where TransactionStatus=0 and  ActivityUserId=" + ECMLoginId.ToString + ")) as x"

            If IsOwner Then
                QueryProcess = "select Count(1) from (select distinct processid from ezwflowtransation where Processid in(select Processid from eZWFProcess where FlowStatus='Running' and WorkflowId=" + Workflowid.ToString() + ") and TransactionStatus<>0 and Processid not in(select Processid from eZWFlowTransation where TransactionStatus=0 and  ActivityUserId=" + ECMLoginId.ToString + ")) as x"
            End If


            Dim DSProcess As DataSet = GetDatasetByQuery(QueryProcess)
            If Not DSProcess Is Nothing AndAlso DSProcess.Tables.Count > 0 AndAlso DSProcess.Tables(0).Rows.Count > 0 Then
                Node.ProcessCountInfo = DSProcess.Tables(0).Rows(0)(0).ToString
            Else
                Node.ProcessCountInfo = "0"
            End If
            result.Add(Node)

            Node = New FlowNodes
            Node.NodeName = "Completed"
            'Dim QueryCompleted = " select Count(1) from (select distinct processid from ezwflowtransation_Completed where (ActivityUserId=" + ECMLoginId + " or(ActivityGroupId<>0 and updatedby=" + ECMLoginId + "))  and Processid in(select Processid from eZWFProcess where FlowStatus='Completed' and WorkflowId=" + Workflowid + " and convert(datetime,updatedon,106) between CONVERT(varchar,dateadd(d,-(day(dateadd(m,-1,getdate()-2))),dateadd(m,-1,getdate()-1)),106) and GETDATE())) as x"
            Dim QueryCompleted = " select Count(1) from (select distinct processid from ezwflowtransation_Completed where (ActivityUserId=" + ECMLoginId + " or(ActivityGroupId<>0 and updatedby=" + ECMLoginId + "))  and Processid in(select Processid from eZWFProcess where FlowStatus='Completed' and WorkflowId=" + Workflowid + ")) as x"
            If IsOwner Then
                'QueryCompleted = "select Count(1) from (select distinct processid from ezwflowtransation_Completed where Processid in(select Processid from eZWFProcess where FlowStatus='Completed' and WorkflowId=" + Workflowid + " and convert(datetime,updatedon,106) between CONVERT(varchar,dateadd(d,-(day(dateadd(m,-1,getdate()-2))),dateadd(m,-1,getdate()-1)),106) and GETDATE())) as x"
                QueryCompleted = "select Count(1) from (select distinct processid from ezwflowtransation_Completed where Processid in(select Processid from eZWFProcess where FlowStatus='Completed' and WorkflowId=" + Workflowid + ")) as x"
            End If

            Dim DSCompleted As DataSet = GetDatasetByQuery(QueryCompleted)
            If Not DSCompleted Is Nothing AndAlso DSCompleted.Tables.Count > 0 AndAlso DSCompleted.Tables(0).Rows.Count > 0 Then
                Node.ProcessCountInfo = DSCompleted.Tables(0).Rows(0)(0).ToString
            Else
                Node.ProcessCountInfo = "0"
            End If
            result.Add(Node)

            Node = New FlowNodes
            Node.NodeName = "Suspended"
            Dim QuerySuspended = "select count(1) as [count] from eZWFlowTransation w left join eZWFProcess p on p.ProcessId=w.Processid where Activityuserid=" + ECMLoginId + " and Activitygroupid='0' and p.flowstatus='Running' and p.workflowid=" + Workflowid + " and w.Action='Suspended' and transactionstatus=0"
            Dim DSSuspended As DataSet = GetDatasetByQuery(QuerySuspended)
            If Not DSSuspended Is Nothing AndAlso DSSuspended.Tables.Count > 0 AndAlso DSSuspended.Tables(0).Rows.Count > 0 Then
                Node.ProcessCountInfo = DSSuspended.Tables(0).Rows(0)(0).ToString
            Else
                Node.ProcessCountInfo = "0"
            End If
            result.Add(Node)

            Node = New FlowNodes
            Node.NodeName = "Initiate"
            Dim QueryInitiate = "select count(processid) from (select processid,stuff((select distinct ','+cast(ActivityUserId as nvarchar(100)) from eZWFlowTransation t1 where t1.processid=t.processid for xml path('')),1,1,'')+','+stuff((select distinct ','+cast(Createdby as nvarchar(100)) from eZWFlowTransation t1 where t1.processid=t.processid for xml path('')),1,1,'')+',' as actionusers from ezwflowtransation t where transactionstatus<>0 and processid in (select processid from (select count(*) as count, processid from eZWFlowTransation where processid in (select processid from ezwfprocess where flowstatus='Running' and workflowid=" + Workflowid.ToString() + ") and TransactionStatus<>0 group by Processid) as x where count=1)) as x where actionusers like '%," + ECMLoginId.ToString + ",%'"
            Dim DSInitiate As DataSet = GetDatasetByQuery(QueryInitiate)
            If Not DSInitiate Is Nothing AndAlso DSInitiate.Tables.Count > 0 AndAlso DSInitiate.Tables(0).Rows.Count > 0 Then
                Node.ProcessCountInfo = DSInitiate.Tables(0).Rows(0)(0).ToString
            Else
                Node.ProcessCountInfo = "0"
            End If
            result.Add(Node)

            Node = New FlowNodes
            Node.NodeName = "Queue"
            Dim QueryQueue = "Select count(1) As count from ezwflowtransation w where ((Activityuserid='" + ECMLoginId.ToString + "' and Activitygroupid='0')"
            If ECMGroupList <> "0" And ECMGroupList <> "" Then
                QueryQueue += "or (Activitygroupid in (" + ECMGroupList + ") and Activityuserid='0')"

            End If
            QueryQueue += ")  and ActivityId='a5d0b578-3ded-40bb-8770-2a5cef442b55' and processid in (select processid from ezwfprocess where flowstatus='Running' and workflowid=" + Workflowid.ToString() + ") and transactionstatus=0"
            Dim DSQueue As DataSet = GetDatasetByQuery(QueryQueue)
            If Not DSQueue Is Nothing AndAlso DSQueue.Tables.Count > 0 AndAlso DSQueue.Tables(0).Rows.Count > 0 Then
                Node.ProcessCountInfo = DSQueue.Tables(0).Rows(0)(0).ToString
            Else
                Node.ProcessCountInfo = "0"
            End If
            result.Add(Node)

            Return result

        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR980F400 " + ex.ToString()
            Throw New FaultException(exc)
        End Try


    End Function


    Public Shared Function ReadXML(ByVal workflowid As Integer) As String
        Dim res = ""
        Try
            Dim ds As DataSet = ListeZWorkflowDetailsbyCriteria("Workflowid", workflowid.ToString)
            If Not ds Is Nothing Then
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count <> 0 Then
                        res = ds.Tables(0).Rows(0).Item("XMLString").ToString()
                    End If
                End If
            End If
            Return res
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR980F400 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function LoadFlowProcessesForGrid(para As FlowGridLoadPara) As String()
        Dim json As String = ""
        Dim condition As String = ""
        Dim totalcount As Integer = 0
        Dim sql = ""
        Dim removelist = ""
        Try
            Dim xml As String = ReadXML(para.WorkflowInfo.Workflowid.ToString())
            Dim xmlds As New DataSet
            xmlds.ReadXml(New StringReader(xml))
            Dim processinfocol = ""
            Dim processinfo() As String = xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString.Replace("[", "").Replace("]", "").Split({","}, StringSplitOptions.RemoveEmptyEntries)
            'Raja
            Dim table As String = "", formtable = ""
            If xmlds.Tables("Activity").Rows(0)("tablename").ToString <> "" And xmlds.Tables("Activity").Rows(0)("formid") = "0" Then
                table = xmlds.Tables("Activity").Rows(0)("tablename").ToString
            Else
                If xmlds.Tables("Activity").Rows(0)("formid") <> "0" Then
                    Dim sqltblform = "select tablename from ezwflowformdetails where formid='" + xmlds.Tables("Activity").Rows(0)("formid") + "'" +
                                " and workflowid='" + para.WorkflowInfo.Workflowid.ToString() + "'"
                    Dim dstblform = GetDatasetByQuery(sqltblform)
                    If Not dstblform Is Nothing Then
                        If dstblform.Tables(0).Rows.Count > 0 Then
                            formtable = "[" + dstblform.Tables(0).Rows(0)("tablename").ToString + "]"
                        End If
                    End If
                ElseIf xmlds.Tables("Activity").Rows(0)("HTMLFormTable") <> "" Then
                    formtable = "[" + xmlds.Tables("Activity").Rows(0)("HTMLFormTable") + "]"
                End If
            End If
            Dim processcols As String = "'"
            Dim tmpprocols As String = "'"
            processcols += """RequestNo"":""'+isnull(cast(p.RequestNo as nvarchar(max)),'')+'"","
            processinfocol += "RequestNo::"
            For j As Integer = 0 To processinfo.Length - 1
                'processcols += "b.[" + processinfo(j) + "],"
                processcols += """" + processinfo(j) + """:""'+isnull(cast(b.[" + processinfo(j) + "] as nvarchar(max)),'')+'"","
                processinfocol += processinfo(j) + "::"
                If processinfo(j) = "RequestNo" Then
                    tmpprocols += """" + processinfo(j) + """:""'+isnull(cast([RequestNo] as nvarchar(max)),'')+'"","
                Else
                    tmpprocols += """" + processinfo(j) + """:"""","
                End If
            Next
            If processcols.Length > 0 Then
                processcols = processcols.Substring(0, processcols.Length - 1) + "' as processcols,"
                tmpprocols = tmpprocols.Substring(0, tmpprocols.Length - 1) + "' as processcols, "
            End If
            If processinfocol.Length > 0 Then
                processinfocol = processinfocol.Substring(0, processinfocol.Length - 2)
            End If
            processcols += "a.processid"
            tmpprocols += "[RequestNo] as processid"
            Dim orderbysql As String = ""
            If para.OrderBy <> "" Then
                orderbysql = " order by " + para.OrderBy
            End If
            condition = ""
            Dim tablename = "", tmpid = ""
            Dim userbasedjoin = "", userbasedcond = ""
            userbasedjoin = ",stuff((select distinct ','+cast(ActivityUserId as nvarchar(100)) from eZWFlowTransation w1 where w1.processid=w.processid for xml path('')),1,1,'')+','+stuff((select distinct ','+cast(w1.Createdby as nvarchar(100)) from eZWFlowTransation w1 where w1.processid=w.processid for xml path('')),1,1,'')+',' as actionusers "
            userbasedcond = " where actionusers like '%," + para.LoggedInfo.ECMLoginId.ToString() + ",%'"
            Dim rowfiltercond = ""
            If para.Rowto <> "" Then
                rowfiltercond = "where rn between " + para.Rowfrom + " And " + para.Rowto + " "
            End If
            Select Case para.NodeName
                Case "Initiate"
                    If para.WorkflowInfo.Workflowid.ToString() <> "" Then
                        condition += " and w.processid in (select processid from ezwfprocess where flowstatus='Running' and workflowid=" + para.WorkflowInfo.Workflowid.ToString() + ")"
                    End If


                    condition = "where w.processid in (select processid from (select count(*) as count, processid from eZWFlowTransation where TransactionStatus<>0 group by Processid) as x where count=1) and transactionstatus=0 " + condition

                    Dim itmcond = ""
                    If table <> "" Then
                        itmcond = " left join ezprocessitems wp on w.processid=wp.processid left join " + table + " itm on itm.itemid=wp.itemid "
                    ElseIf formtable <> "" Then
                        itmcond = " left join ezprocessitems wp on w.processid=wp.processid left join " + formtable + " itm on itm.itemid=wp.formentryid "
                    End If

                    sql = "select * into #temp from (select distinct x2.*,u.firstname,g.ECMGroup from (select action, w.processid, 0 As transactionid, w.createdon, cast(w.ActivityUserId as nvarchar(100)) as ActivityUserId, w.usertype, cast(w.ActivityGroupId as nvarchar(100)) as ActivityGroupId, w.activityid, convert(datetime,w.createdon,101) As createdon1, " + para.WorkflowInfo.Workflowid.ToString() + " As workflowid, w.updatedby, w.updatedon" + userbasedjoin + " from ezwflowtransation w " + itmcond + condition + ") as x2 left join ezecmuserinfo u on x2.ActivityUserId=u.ECMLoginId left join ezecmgroup g on g.ECMGroupId=x2.ActivityGroupId" + userbasedcond + ") as x2;"

                    sql += "select x2.*,"
                    If (table <> "") Then
                        sql += "(select isnull(sum(nopages),0) from ezprocessitems ipt left join " + table + " iitm on ipt.itemid=iitm.itemid where ipt.processid=x2.processid) as nopages, "
                    End If
                    sql += "(select top 1 review from eZWFlowTransation w2 where transactionid<x2.transactionid And w2.processid=x2.processid order by transactionid desc) as review, (select count(processitemsid) from ezprocessitems where processid=x2.processid And itemid<>0) as docscount," +
                                "(select count(commentsid) as commentscount from eZComments where processid=x2.processid) as commentscount, " +
                                "(select action from ezwflowtransation where transactionid in (select max(transactionid) from eZWFlowTransation where processid=x2.processid and transactionstatus=1)) as prevaction, " +
                                "(select firstname from ezwflowtransation prevw left join ezecmuserinfo prevu on prevw.updatedby=prevu.ecmloginid where transactionid in (select max(transactionid) from eZWFlowTransation where processid=x2.processid and transactionstatus=1)) as prevactionby," +
                                "(select Createdon from ezwfprocess where processid=x2.processid) as raisedon," +
                                "(select firstname from ezwfprocess p left join ezecmuserinfo u on p.createdby=u.ecmloginid where processid=x2.processid) as raisedby,(select top 1 frommail from ezwflowtransation where processid=x2.processid) as frommail," +
                                "isnull((Select count(action) from eZWFlowTransation w1 where w1.action=x2.action And w1.processid=x2.processid " +
                                " And w1.ActivityUserId='" + para.LoggedInfo.ECMLoginId.ToString() + "'),0) As count," +
                                "(select RequestNo from ezwfprocess where processid=x2.processid) as requestno from " +
                                "(Select row_number() over(partition by action  " + orderbysql + ") As rn, * from (select distinct action, processid, transactionid, createdon, " +
                                "stuff((select ','+ActivityUserId from #temp t1 where t1.processid=t.processid for xml path('')),1,1,'') as ActivityUserId, " +
                                "stuff((select case when usertype is null or usertype='' then '<b>User</b>' else '<b>'+usertype+'</b>' end+' : '+firstname+'<br/>' from #temp t1 where t1.processid=t.processid for xml path('')),1,0,'') as UserType, " +
                                "stuff((select ','+firstname from #temp t1 where t1.processid=t.processid for xml path('')),1,1,'') as firstname, " +
                                "stuff((select ','+ActivityGroupId from #temp t1 where t1.processid=t.processid and activitygroupid<>0 for xml path('')),1,1,'') as ActivityGroupId, " +
                                "stuff((select ','+ECMGroup from #temp t1 where t1.processid=t.processid and activitygroupid<>0 for xml path('')),1,1,'') as ECMGroup, " +
                                "activityid, createdon1, workflowid, 0 as updatedby,0 as updatedon from #temp t group by action, processid, transactionid, createdon, activityid, createdon1, workflowid) As x1) As x2 " + rowfiltercond + orderbysql + ";drop table #temp;"


                    If table <> "" Then
                        sql += "select distinct " + processcols + " from " + table + " b left join ezprocessitems a on a.itemid=b.itemid " +
                                    " and a.templateid=b.templateid left join ezwfprocess p on p.processid=a.processid where a.itemid in (select max(itemid) from ezprocessitems where processid in ("
                        sql += "select processid from (select row_number() over(order by processid desc) as rn,* from ("
                        sql += "select w.processid from ezwflowtransation w " + condition + " ) as x) as x1 " + rowfiltercond + ") group by processid);"
                    End If
                    If formtable <> "" Then
                        sql += "Select " + processcols + " from ezprocessitems a left join " + formtable + " b On a.formentryid=b.itemid left join ezwfprocess p on p.processid=a.processid" +
                                    " left join (select Max(ProcessItemsid) as ProcessItemsid,processid from ezProcessItems where processid in ("
                        sql += "Select w.processid from ezwflowtransation w " + condition + ") and formentryid<>0 group by processid) as pitm on p.ProcessId=pitm.Processid where a.ProcessItemsid=pitm.ProcessItemsid;"
                    End If
                Case "Process"
                    If para.WorkflowInfo.Workflowid.ToString() <> "" Then
                        condition += "w.processid In (Select processid from ezwfprocess where flowstatus='Running' and workflowid=" + para.WorkflowInfo.Workflowid.ToString() + ") and "
                    End If

                    condition = "where " + condition + " transactionstatus=0 and w.processid not in (select processid from ezwflowtransation where processid in (select processid from ezwfprocess where flowstatus='Running' and workflowid=" + para.WorkflowInfo.Workflowid.ToString() + ") and ActivityUserId='" + para.LoggedInfo.ECMLoginId.ToString() + "' and TransactionStatus=0)"
                    Dim itmcond = ""
                    If table <> "" Then
                        itmcond = " left join (select max(itemid) as itemid,processid from ezprocessitems where itemid<>0 group by processid) as wp on w.processid=wp.processid left join " + table + " itm on itm.itemid=wp.itemid "
                    ElseIf formtable <> "" Then
                        itmcond = " left join (select max(formentryid) as formentryid,processid from ezprocessitems where formentryid<>0 group by processid) as wp on w.processid=wp.processid left join " + formtable + " itm on itm.itemid=wp.formentryid "
                    End If

                    sql = "select * into #temp from (select distinct x2.*,u.firstname,g.ECMGroup from (select action, w.processid, 0 As transactionid, w.createdon, cast(w.ActivityUserId as nvarchar(100)) as ActivityUserId, w.usertype, cast(w.ActivityGroupId as nvarchar(100)) as ActivityGroupId, w.activityid, convert(datetime,w.createdon,101) As createdon1, " + para.WorkflowInfo.Workflowid.ToString() + " As workflowid, w.updatedby, w.updatedon" + userbasedjoin + " from ezwflowtransation w left join ezwfprocess wfp on w.processid=wfp.processid " + itmcond + condition + ") as x2 left join ezecmuserinfo u on x2.ActivityUserId=u.ECMLoginId left join ezecmgroup g on g.ECMGroupId=x2.ActivityGroupId" + userbasedcond + ") as x2;"

                    sql += "select x2.*, "
                    If table <> "" Then
                        sql += "(select isnull(sum(nopages),0) from ezprocessitems ipt left join " + table + " iitm on ipt.itemid=iitm.itemid where ipt.processid=x2.processid) as nopages, "
                    End If
                    sql += "isnull((select count(action) from eZWFlowTransation w1 where w1.action=x2.action And w1.processid=x2.processid " +
                                "  And w1.ActivityUserId='" + para.LoggedInfo.ECMLoginId.ToString() + "'),0) as count," +
                                "(select top 1 review from eZWFlowTransation w2 where transactionid<x2.transactionid And w2.processid=x2.processid order by transactionid desc) as review, (select count(processitemsid) from ezprocessitems where processid=x2.processid And itemid<>0) as docscount," +
                                "(select count(commentsid) as commentscount from eZComments where processid=x2.processid) as commentscount, " +
                                "(select action from ezwflowtransation  where transactionid in (select max(transactionid) from eZWFlowTransation where processid=x2.processid and transactionstatus=1)) as prevaction, " +
                                "(select firstname from ezwflowtransation prevw left join ezecmuserinfo prevu on prevw.updatedby=prevu.ecmloginid where transactionid in (select max(transactionid) from eZWFlowTransation where processid=x2.processid and transactionstatus=1)) as prevactionby," +
                                "(select Createdon from ezwfprocess where processid=x2.processid) as raisedon," +
                                "(select firstname from ezwfprocess p left join ezecmuserinfo u on p.createdby=u.ecmloginid where processid=x2.processid) as raisedby, (select top 1 frommail from ezwflowtransation where processid=x2.processid) as frommail, " +
                                "(select RequestNo from ezwfprocess where processid=x2.processid) as requestno from " +
                                "(Select row_number() over(partition by action  " + orderbysql + ") As rn,* from (select distinct action, processid, transactionid, createdon, " +
                                "stuff((select ','+ActivityUserId from #temp t1 where t1.processid=t.processid for xml path('')),1,1,'') as ActivityUserId, " +
                                "stuff((select case when usertype is null or usertype='' then '<b>'+action+'</b>' else '<b>'+usertype+'</b>' end+' : '+firstname+'<br/>' from #temp t1 where t1.processid=t.processid for xml path('')),1,0,'') as UserType, " +
                                "stuff((select ','+firstname from #temp t1 where t1.processid=t.processid for xml path('')),1,1,'') as firstname, " +
                                "stuff((select ','+ActivityGroupId from #temp t1 where t1.processid=t.processid and activitygroupid<>0 for xml path('')),1,1,'') as ActivityGroupId, " +
                                "stuff((select ','+ECMGroup from #temp t1 where t1.processid=t.processid and activitygroupid<>0 for xml path('')),1,1,'') as ECMGroup, " +
                                "activityid, createdon1, workflowid, 0 as updatedby,0 as updatedon from #temp t group by action, processid, transactionid, createdon, activityid, createdon1, workflowid) As x1) As x2 " + rowfiltercond + orderbysql + ";drop table #temp;"

                    If table <> "" Then
                        sql += "select distinct " + processcols + " from " + table + " b left join ezprocessitems a on a.itemid=b.itemid " +
                                    " and a.templateid=b.templateid left join ezwfprocess p on p.processid=a.processid where a.itemid in (select max(itemid) from ezprocessitems where processid in ("
                        sql += "select processid from (select row_number() over(order by processid desc) as rn,* from ("
                        sql += "select w.processid from ezwflowtransation w " + condition + " ) as x) as x1 " + rowfiltercond + ") group by processid);"
                    End If
                    If formtable <> "" Then
                        sql += "Select " + processcols + " from ezprocessitems a left join " + formtable + " b On a.formentryid=b.itemid left join ezwfprocess p on p.processid=a.processid" +
                                    " left join (select Max(ProcessItemsid) as ProcessItemsid,processid from ezProcessItems where processid in ("
                        sql += "Select w.processid from ezwflowtransation w " + condition + ") and formentryid<>0 group by processid) as pitm on p.ProcessId=pitm.Processid where a.ProcessItemsid=pitm.ProcessItemsid;"
                    End If
                Case "Completed"
                    If para.WorkflowInfo.Workflowid.ToString() <> "" Then
                        condition += "w.processid In (Select processid from ezwfprocess where flowstatus='Completed' and workflowid=" + para.WorkflowInfo.Workflowid.ToString() + ")"
                    End If

                    Dim tempcond = " where " + condition
                    condition = "where transactionid in (select max(transactionid) from ezwflowtransation w where " + condition + " group by processid) and " +
                                "transactionstatus<>0 "

                    Dim itmcond = ""
                    If table <> "" Then
                        itmcond = " left join (select max(itemid) as itemid,processid from ezprocessitems where itemid<>0 group by processid) as wp on w.processid=wp.processid left join " + table + " itm on itm.itemid=wp.itemid "
                    ElseIf formtable <> "" Then
                        itmcond = " left join (select max(formentryid) as formentryid,processid from ezprocessitems where formentryid<>0 group by processid) as wp on w.processid=wp.processid left join " + formtable + " itm on itm.itemid=wp.formentryid "
                    End If

                    sql = "select x2.*,u.firstname as UserType, u.firstname, g.ECMGroup, "
                    If table <> "" Then
                        sql += "(select isnull(sum(nopages),0) from ezprocessitems ipt left join " + table + " iitm on ipt.itemid=iitm.itemid where ipt.processid=x2.processid) as nopages, "
                    End If
                    sql += "isnull((select count(action) from eZWFlowTransation w1 where w1.action=x2.action And w1.processid=x2.processid " +
                                 "  And w1.ActivityUserId='" + para.LoggedInfo.ECMLoginId.ToString() + "'),0) as count," +
                                 "(select top 1 review from eZWFlowTransation w2 where transactionid<x2.transactionid and w2.processid=x2.processid order by transactionid desc) as review, (select count(processitemsid) from ezprocessitems where processid=x2.processid and itemid<>0) as docscount," +
                                 "(select count(commentsid) as commentscount from eZComments where processid=x2.processid) as commentscount, " +
                                 "(select action from ezwflowtransation  where transactionid in (select max(transactionid) from eZWFlowTransation where processid=x2.processid and transactionstatus=1)) as prevaction, " +
                                 "(select firstname from ezwflowtransation prevw left join ezecmuserinfo prevu on prevw.updatedby=prevu.ecmloginid where transactionid in (select max(transactionid) from eZWFlowTransation where processid=x2.processid and transactionstatus=1)) as prevactionby," +
                                 "(select Createdon from ezwfprocess where processid=x2.processid) as raisedon,(select firstname from ezwfprocess p left join ezecmuserinfo u on p.createdby=u.ecmloginid where processid=x2.processid) as raisedby,(select top 1 frommail from ezwflowtransation where processid=x2.processid) as frommail,(select RequestNo from ezwfprocess where processid=x2.processid) as requestno from " +
                                 "(select row_number() over(partition by action  " + orderbysql + ") as rn,* from (select distinct action, w.processid, 0 as transactionid, " +
                                 "w.createdon, w.ActivityUserId, w.ActivityGroupId, w.activityid, convert(datetime,w.createdon,101) as createdon1, w.updatedon, w.updatedby, " +
                                 para.WorkflowInfo.Workflowid.ToString() + " as workflowid" + userbasedjoin + " from ezwflowtransation w left join ezwfprocess wfp on w.processid=wfp.processid " + itmcond + condition + ") as x1" + userbasedcond + ") as x2 left join ezecmuserinfo u on x2.activityuserid=u.ecmloginid left join ezecmgroup g on g.ECMGroupId=x2.ActivityGroupId " + rowfiltercond + orderbysql + ";"

                    If table <> "" Then
                        sql += "select distinct " + processcols + " from " + table + " b left join ezprocessitems a on a.itemid=b.itemid " +
                                    " and a.templateid=b.templateid left join ezwfprocess p on p.processid=a.processid where a.itemid in (select max(itemid) from ezprocessitems where processid in ("
                        sql += "select processid from (select row_number() over(order by processid desc) as rn,* from ("
                        sql += "select w.processid from ezwflowtransation w " + condition + " ) as x) as x1 " + rowfiltercond + ") group by processid);"
                    End If
                    If formtable <> "" Then
                        sql += "Select " + processcols + " from ezprocessitems a left join " + formtable + " b On a.formentryid=b.itemid left join ezwfprocess p on p.processid=a.processid" +
                                    " left join (select Max(ProcessItemsid) as ProcessItemsid,processid from ezProcessItems where processid in ("
                        sql += "Select w.processid from ezwflowtransation w " + condition + ") and formentryid<>0 group by processid) as pitm on p.ProcessId=pitm.Processid where a.ProcessItemsid=pitm.ProcessItemsid;"
                    End If
                Case "Inbox"
                    condition = "where "
                    If para.WorkflowInfo.Workflowid.ToString() <> "" Then
                        condition += " w.processid in (select processid from ezwfprocess where flowstatus='Running' and workflowid=" + para.WorkflowInfo.Workflowid.ToString() + ") And "
                    End If

                    condition += " ((Activityuserid='" + para.LoggedInfo.ECMLoginId.ToString() + "' and Activitygroupid='0')"
                    If (para.LoggedInfo.ECMGroupList <> "0") Then
                        condition += " or (Activitygroupid in (" + para.LoggedInfo.ECMGroupList + ") and Activityuserid='0') "
                    End If
                    condition += ") and transactionstatus=0 "

                    Dim itmcond = ""
                    If table <> "" Then
                        itmcond = " left join (select max(itemid) as itemid,processid from ezprocessitems where itemid<>0 group by processid) as wp on w.processid=wp.processid left join " + table + " itm on itm.itemid=wp.itemid "
                    ElseIf formtable <> "" Then
                        itmcond = " left join (select max(formentryid) as formentryid,processid from ezprocessitems where formentryid<>0 group by processid) as wp on w.processid=wp.processid left join " + formtable + " itm on itm.itemid=wp.formentryid "
                    End If

                    sql = "select x2.*, u.firstname, g.ECMGroup, "
                    If table <> "" Then
                        sql += "(select isnull(sum(nopages),0) from ezprocessitems ipt left join " + table + " iitm on ipt.itemid=iitm.itemid where ipt.processid=x2.processid) as nopages,"
                    End If
                    sql += "isnull((select count(action) from eZWFlowTransation w1 where w1.action=x2.action and w1.processid=x2.processid " +
                                "  and w1.ActivityUserId='" + para.LoggedInfo.ECMLoginId.ToString() + "'),0) as count," +
                                "(select top 1 review from eZWFlowTransation w2 where transactionid<x2.transactionid and w2.processid=x2.processid order by transactionid desc) as review, (select count(processitemsid) from ezprocessitems where processid=x2.processid and itemid<>0) as docscount," +
                                "(select count(commentsid) as commentscount from eZComments where processid=x2.processid) as commentscount, " +
                                "(select action from ezwflowtransation  where transactionid in (select max(transactionid) from eZWFlowTransation where processid=x2.processid and transactionstatus=1)) as prevaction, " +
                                "(select firstname from ezwflowtransation prevw left join ezecmuserinfo prevu on prevw.updatedby=prevu.ecmloginid where transactionid in (select max(transactionid) from eZWFlowTransation where processid=x2.processid and transactionstatus=1)) as prevactionby," +
                                "(select Createdon from ezwfprocess where processid=x2.processid) as raisedon,(select firstname from ezwfprocess p left join ezecmuserinfo u on p.createdby=u.ecmloginid where processid=x2.processid) as raisedby,(select top 1 frommail from ezwflowtransation where processid=x2.processid) as frommail,(select RequestNo from ezwfprocess where processid=x2.processid) as requestno, stuff((select case when usertype='' then '<b>'+action+'</b>' else '<b>'+cast(usertype as nvarchar(100))+'</b>' end +' : '+cast(ui.firstname as nvarchar(100))+',' from eZWFlowTransation wft left join ezecmuserinfo ui on activityuserid=ui.ecmloginid where Processid=x2.processid and transactionstatus=0 for xml path('')),1,0,'') as usertype from  (select row_number() over(partition by action " + orderbysql + ") as rn,* from (select distinct action, w.processid, w.transactionid, w.createdon, w.ActivityUserId, w.ActivityGroupId, w.activityid, convert(datetime,w.createdon,101) as createdon1, w.updatedon, w.updatedby, " +
                                para.WorkflowInfo.Workflowid.ToString() + " as workflowid from ezwflowtransation w left join ezwfprocess wfp on w.processid=wfp.processid " + itmcond + condition + ") as x1) as x2 left join ezecmuserinfo u on " +
                                "x2.activityuserid=u.ecmloginid left join ezecmgroup g on g.ECMGroupId=x2.ActivityGroupId " + rowfiltercond + orderbysql + ";"

                    If table <> "" Then
                        sql += "select distinct " + processcols + " from " + table + " b left join ezprocessitems a on a.itemid=b.itemid " +
                                    " and a.templateid=b.templateid left join ezwfprocess p on p.processid=a.processid where a.itemid in (select max(itemid) from ezprocessitems where processid in ("
                        sql += "select processid from (select row_number() over(order by processid desc) as rn,* from ("
                        sql += "select w.processid from ezwflowtransation w " + condition + " ) as x) as x1 " + rowfiltercond + ") group by processid);"
                    End If
                    If formtable <> "" Then
                        sql += "Select " + processcols + " from ezprocessitems a left join " + formtable + " b On a.formentryid=b.itemid left join ezwfprocess p on p.processid=a.processid" +
                                    " left join (select Max(ProcessItemsid) as ProcessItemsid,processid from ezProcessItems where processid in ("
                        sql += "Select w.processid from ezwflowtransation w " + condition + ") and formentryid<>0 group by processid) as pitm on p.ProcessId=pitm.Processid where a.ProcessItemsid=pitm.ProcessItemsid;"
                    End If
                Case "Suspended"
                    If para.WorkflowInfo.Workflowid.ToString() <> "" Then
                        condition += "w.processid in (select processid from ezwfprocess where flowstatus='Completed' and workflowid=" + para.WorkflowInfo.Workflowid.ToString() + " and " +
                                    " w.processid in (select processid from ezwflowtransation where Action='End' and TransactionStatus=2))"
                    End If

                    condition = "where transactionid in (select max(transactionid) from ezwflowtransation w where " + condition + " group by processid) and " +
                                "transactionstatus<>0"

                    sql = "select x2.*, u.firstname, g.ECMGroup, "
                    If table <> "" Then
                        sql += "(select isnull(sum(nopages),0) from ezprocessitems ipt left join " + table + " iitm on ipt.itemid=iitm.itemid where ipt.processid=x2.processid) as nopages, "
                    End If
                    sql += "isnull((select count(action) from eZWFlowTransation w1 where w1.action=x2.action And w1.processid=x2.processid " +
                                 " And w1.ActivityUserId='" + para.LoggedInfo.ECMLoginId.ToString() + "'),0) as count, " +
                                "(select top 1 review from eZWFlowTransation w2 where transactionid<x2.transactionid and w2.processid=x2.processid order by transactionid desc) as review, (select count(processitemsid) from ezprocessitems where processid=x2.processid and itemid<>0) as docscount, " +
                                "(select count(commentsid) as commentscount from eZComments where processid=x2.processid) as commentscount, " +
                                "(select action from ezwflowtransation  where transactionid in (select max(transactionid) from eZWFlowTransation where processid=x2.processid and transactionstatus=1)) as prevaction, " +
                                "(select firstname from ezwflowtransation prevw left join ezecmuserinfo prevu on prevw.updatedby=prevu.ecmloginid where transactionid in (select max(transactionid) from eZWFlowTransation where processid=x2.processid and transactionstatus=1)) as prevactionby," +
                                "(select Createdon from ezwfprocess where processid=x2.processid) as raisedon," +
                                "(select firstname from ezwfprocess p left join ezecmuserinfo u on p.createdby=u.ecmloginid where processid=x2.processid) as raisedby," +
    "(select RequestNo from ezwfprocess where processid=x2.processid) as requestno from " +
                                "(select row_number() over(partition by action " + orderbysql + ") as rn,* from (select distinct action, w.processid, w.transactionid, w.createdon, " +
                                "w.ActivityUserId, w.ActivityGroupId, w.activityid, convert(datetime,w.createdon,101) as createdon1, w.updatedon, w.updatedby, " +
                                para.WorkflowInfo.Workflowid.ToString() + " as workflowid from ezwflowtransation w " + condition + ") as x1) as x2 left join ezecmuserinfo u on " +
                                "x2.activityuserid=u.ecmloginid left join ezecmgroup g on g.ECMGroupId=x2.ActivityGroupId " + rowfiltercond + orderbysql + ";"

                    If table <> "" Then
                        sql += "select distinct " + processcols + " from " + table + " b left join ezprocessitems a on a.itemid=b.itemid " +
                                    " and a.templateid=b.templateid left join ezwfprocess p on p.processid=a.processid where a.itemid in (select max(itemid) from ezprocessitems where processid in ("
                        sql += "select processid from (select row_number() over(order by processid desc) as rn,* from ("
                        sql += "select w.processid from ezwflowtransation w " + condition + " ) as x) as x1 " + rowfiltercond + ") group by processid);"
                    End If
                    If formtable <> "" Then
                        sql += "Select " + processcols + " from ezprocessitems a left join " + formtable + " b On a.formentryid=b.itemid left join ezwfprocess p on p.processid=a.processid" +
                                    " left join (select Max(ProcessItemsid) as ProcessItemsid,processid from ezProcessItems where processid in ("
                        sql += "Select w.processid from ezwflowtransation w " + condition + ") and formentryid<>0 group by processid) as pitm on p.ProcessId=pitm.Processid where a.ProcessItemsid=pitm.ProcessItemsid;"
                    End If
            End Select

            If para.Rowfrom = "" And table <> "" Then
                sql += "select count(*) from (select distinct w.processid" + userbasedjoin + " from ezwflowtransation w " +
                                        "left join ezprocessitems wp on w.processid=wp.processid left join " + table + " itm on itm.itemid=wp.itemid left join ezwfprocess wfp on w.processid=wfp.processid " +
                                        condition + ") as x " + userbasedcond

            ElseIf para.Rowfrom = "" And formtable <> "" Then
                sql += "select count(*) from (select distinct w.processid" + userbasedjoin + " from ezwflowtransation w " +
                                        "left join ezprocessitems wp on w.processid=wp.processid left join " + formtable + " itm on itm.itemid=wp.formentryid left join ezwfprocess wfp on w.processid=wfp.processid " +
                                        condition + ") as x " + userbasedcond
            End If

            'errormsg += sql
            Dim ds As DataSet = GetDatasetByQuery(sql)
            If Not ds Is Nothing Then
                If ds.Tables.Count > 0 Then
                    Dim pidlist = ","
                    Dim totcount = 0
                    json += "{""processcol"":""" + processinfocol + "::Stage" + """,""details"":["
                    For Each row As DataRow In ds.Tables(0).DefaultView.ToTable(True, "action").Rows
                        Dim datarows() As DataRow = ds.Tables(0).Select("action='" + row(0) + "'")
                        Dim rowcount As String = "0"
                        Dim tblindex = 1
                        If table <> "" Or formtable <> "" Then
                            If ds.Tables.Count > 2 Then
                                tblindex = 2
                            End If
                        End If

                        If para.Rowfrom = "" And ds.Tables(tblindex).Rows.Count > 0 Then
                            rowcount = ds.Tables(tblindex).Rows(0)(0).ToString
                            totcount = rowcount
                        End If

                        Dim pid = 0
                        Dim processfound = False, processcount = 0
                        For Each row1 As DataRow In datarows
                            Dim dtgrp = datarows.CopyToDataTable()
                            Dim actionuser = ""
                            For Each grprow As DataRow In dtgrp.Select("processid='" + row1("processid").ToString + "'")
                                If (Not IsDBNull(grprow("usertype"))) Then
                                    actionuser = grprow("usertype") + "/"
                                End If
                            Next
                            If actionuser.Length > 0 Then
                                actionuser = actionuser.Substring(0, actionuser.Length - 1)
                            End If
                            If pid <> row1("processid").ToString And Not pidlist.Contains("," + row1("processid").ToString + ",") Then
                                processcount += 1
                                processfound = True
                                pid = row1("processid")
                                pidlist += row1("processid").ToString + ","
                                json += "{""status"":""" + row(0).ToString + """,""processid"":""" + row1("processid").ToString + """,""transactionid"":""" + row1("transactionid").ToString + """,""activityid"":""" + row1("ActivityId").ToString + ""","
                                json += """createdon"":""" + row1("createdon") + """,""createdon1"":""" + row1("createdon1") + """,""firstname"":""" + actionuser + ""","
                                json += """ecmgroup"":""" + row1("ECMGroup").ToString + """,""workflowid"":""" + para.WorkflowInfo.Workflowid.ToString() + """,""count"":" + row1("count").ToString
                                json += ",""processname"":""" + row1("requestno").ToString + """"
                                Dim rechecksql = "select * from ezwflowtransation where processid='" + row1("processid").ToString + "' and transactionstatus=0 and review<>'';"
                                Dim transreview = row1("review").ToString
                                Dim dsrecheck = GetDatasetByQuery(rechecksql)
                                If Not dsrecheck Is Nothing AndAlso dsrecheck.Tables.Count > 0 AndAlso dsrecheck.Tables(0).Rows.Count > 0 Then
                                    transreview = dsrecheck.Tables(0).Rows(0)("Review").ToString
                                End If

                                json += ",""review"":""" + transreview + """"

                                'raja
                                If para.NodeName = "Inbox" Then
                                    If (row1("Updatedby").ToString = "0") Then
                                        json += ",""makeasread"":""false"""
                                    Else
                                        json += ",""makeasread"":""true"""
                                    End If
                                    Dim makeaccess = ""
                                    If row1("count") > 1 Then
                                        Dim accesssql = "select w.*,u.Firstname from ezwflowtransation w left join ezecmuserinfo u on w.activityuserid=u.ecmloginid " +
                                                    " where w.processid='" + row1("processid").ToString + "' and w.transactionstatus=0 and w.activityid='" + row1("ActivityId").ToString + "'"
                                        'errormsg = accesssql
                                        Dim accessds = GetDatasetByQuery(accesssql)
                                        If Not accessds Is Nothing Then
                                            If accessds.Tables(0).Rows.Count > 1 Then
                                                For Each accessrow In accessds.Tables(0).Select("(Updatedon<>'0' and Updatedon<>'') and updatedby<>'" + para.LoggedInfo.ECMLoginId.ToString() + "'")
                                                    makeaccess = accessrow("Firstname").ToString
                                                Next
                                            End If
                                        End If
                                        json += ",""makeaccess"":""" + makeaccess + """"
                                    Else
                                        json += ",""makeaccess"":"""""
                                    End If
                                End If

                                If (para.LoggedInfo.ECMLoginId.ToString() = row1("ActivityUserId").ToString) Then
                                    json += ",""myprocess"":""true"""
                                Else
                                    json += ",""myprocess"":""false"""
                                End If


                                Dim tblinfo = ""
                                If table <> "" Then
                                    tblinfo = table
                                ElseIf formtable <> "" Then
                                    tblinfo = formtable
                                End If

                                json += ",""attachments"":""" + row1("docscount").ToString + """"
                                If ds.Tables(0).Columns.IndexOf("nopages") > -1 Then
                                    json += ",""nopages"":""" + row1("nopages").ToString + """"
                                Else
                                    json += ",""nopages"":0"
                                End If
                                json += ",""comments"":""" + row1("commentscount").ToString + """"
                                json += ",""prevaction"":""" + row1("prevaction").ToString + """"
                                If (IsDBNull(row1("prevactionby"))) Then
                                    json += ",""updatedby"":""" + row1("FromMail").ToString + """"
                                Else
                                    json += ",""updatedby"":""" + row1("prevactionby").ToString + """"
                                End If
                                json += ",""receiveddate"":""" + row1("createdon").ToString + """"
                                json += ",""raisedon"":""" + row1("raisedon").ToString + """"
                                If (row1("raisedby").ToString <> "") Then
                                    json += ",""raisedby"":""" + row1("raisedby").ToString + """"
                                Else
                                    json += ",""raisedby"":""" + row1("FromMail").ToString + """"
                                End If
                                If row1("createdon") <> "" And row1("createdon") <> "0" Then
                                    Dim dt = DateTime.Parse(row1("createdon"))
                                    Dim curdt = DateTime.Now
                                    Dim duration = curdt.Subtract(dt)
                                    Dim timespan = duration.Days
                                    Dim str = " days ago"
                                    If timespan = 0 Then
                                        timespan = duration.Hours
                                        str = " hours ago"
                                    End If
                                    If timespan = 0 Then
                                        timespan = duration.Minutes
                                        str = " minutes ago"
                                    End If
                                    json += ",""updatedon"":""" + timespan.ToString + str + """"
                                End If

                                If row1("raisedon") <> "" And row1("raisedon") <> "0" Then
                                    Dim dt = DateTime.Parse(row1("raisedon"))
                                    Dim curdt = DateTime.Now
                                    Dim duration = curdt.Subtract(dt)
                                    Dim timespan = duration.Days
                                    Dim str = " days"
                                    If timespan = 0 Then
                                        timespan = duration.Hours
                                        str = " hours"
                                    End If
                                    If timespan = 0 Then
                                        timespan = duration.Minutes
                                        str = " minutes"
                                    End If
                                    json += ",""daysopened"":""" + timespan.ToString + str + """"
                                End If

                                If ds.Tables.Count > 1 And processinfo.Length > 0 And (formtable <> "" Or table <> "") Then
                                    Dim tmprow() As DataRow = ds.Tables(1).Select("processid=" + row1("processid").ToString)
                                    If tmprow.Length > 0 Then
                                        json += ",""processinfo"":{"
                                        json += tmprow(tmprow.Length - 1)("processcols").ToString + ",""Stage"":""" + row(0).ToString + """}"
                                    End If
                                Else
                                    json += ",""processinfo"":{""processid"":" + row1("processid").ToString + "}"
                                End If
                                json += "},"
                            End If
                        Next
                        If (rowcount <> processcount) Then
                            removelist += row(0).ToString + "::" + processcount.ToString + ","
                        Else

                        End If
                    Next
                    If json.EndsWith(",") Then
                        json = json.Substring(0, json.Length - 1) + "],""rowcount"":""" + totcount.ToString + """}"
                    Else
                        json = json + "],""rowcount"":""" + totcount.ToString + """}"
                    End If
                    If removelist.Length > 0 Then
                        removelist = removelist.Substring(0, removelist.Length - 1)
                    End If
                End If
            End If
            If json.Length > 0 Then
                json += ","
            End If
            If json.Length > 0 Then
                json = json.Substring(0, json.Length - 1)
            End If
            json = "[" + json + "]"
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR980F400 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
        json = json.Replace(vbCr, " ").Replace(vbLf, " ").Replace(vbTab, " ")
        Return {json}
    End Function

    Public Shared Function InsertAndUpdateeZWorkflowUsers(ByVal Obj As eZWorkflowUsers) As Integer
        Try
            Dim objEmp As IeZWorkflowUsers = Nothing
            If Obj.WorkflowUsersId = 0 Then
                Try
                    objEmp = DBLayer.DBLInstance.CreateeZWorkflowUsers(Obj)
                Catch ex As Exception
                    Dim exc As String
                    exc = "ERROR CODE : WDBRJ500F800DB10 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            Else
                Try
                    objEmp = DBLayer.DBLInstance.GlobalInstance.eZWorkflowUsers(Obj.WorkflowUsersId)
                    objEmp = Obj
                    objEmp.SaveChanges()
                Catch ex As Exception
                    Dim exc As String
                    exc = "ERROR CODE : WDBRJ500F800DB20 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            End If
            If objEmp IsNot Nothing Then
                Return objEmp.WorkflowUsersId
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New FaultException("ERROR CODE : WDBRJ500F800 : " + ex.ToString)
        End Try
    End Function
#End Region
#Region "Transaction"
    Public Shared Function Rjunk(ByVal str As String) As String
        Return str.Replace("/", "").Trim(" ").Replace(":", "")
    End Function



    Public Shared Function InsertandUpdateeZWFlowTransation(ByVal OBJ As eZWFlowTransation) As String

        Dim excx As String = ""
        Try
            Dim objEmp As IeZWFlowTransation = Nothing
            If OBJ.Transactionid = 0 Then
                Try
                    objEmp = DBLayer.DBLInstance.CreateeZWFlowTransation(OBJ)
                Catch ex As Exception
                    Dim exc As String
                    exc = "ERROR CODE : WSR980F100DB10 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            Else
                Try
                    objEmp = DBLayer.DBLInstance.GlobalInstance.eZWFlowTransation(OBJ.Transactionid)
                    objEmp = OBJ
                    objEmp.SaveChanges()
                Catch ex As Exception
                    Dim exc As String
                    exc = "ERROR CODE : WSR980F100DB20 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            End If
            If objEmp IsNot Nothing Then
                Return objEmp.Transactionid
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New FaultException("ERROR CODE : WSR980F100 : " + ex.ToString)
        End Try
        Return excx
    End Function
    Public Shared Function eZWFlowTransationList() As List(Of eZWFlowTransation)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWFlowTransation)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadAlleZWFlowTransation()
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F200DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWFlowTransation)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWFlowTransation
                    lst.Action = Lst1(i).Action
                    lst.ActionBy = Lst1(i).ActionBy
                    lst.ActionGroupBy = Lst1(i).ActionGroupBy
                    lst.ActivityGroupId = Lst1(i).ActivityGroupId
                    lst.ActivityId = Lst1(i).ActivityId
                    lst.ActivityUserId = Lst1(i).ActivityUserId
                    lst.Attachment = Lst1(i).Attachment
                    lst.DocCount = Lst1(i).DocCount
                    lst.FileType = Lst1(i).FileType
                    lst.Formid = Lst1(i).Formid
                    lst.LastActionStage = Lst1(i).LastActionStage
                    lst.LastActionReview = Lst1(i).LastActionReview
                    ' lst.FormName = Lst1(i).FormName
                    lst.FormTableName = Lst1(i).FormTableName
                    lst.FromMail = Lst1(i).FromMail
                    lst.FTemplateid = Lst1(i).FTemplateid
                    lst.ItemTableName = Lst1(i).ItemTableName
                    lst.LastActedBy = Lst1(i).LastActedBy
                    lst.LastActedOn = Lst1(i).LastActedOn
                    lst.notification = Lst1(i).Notification
                    lst.ProcessId = Lst1(i).ProcessId
                    lst.RaisedBy = Lst1(i).RaisedBy
                    lst.RaisedOn = Lst1(i).RaisedOn
                    lst.RequestNo = Lst1(i).RequestNo
                    lst.RequestType = Lst1(i).RequestType
                    lst.Review = Lst1(i).Review
                    lst.RuleId = Lst1(i).RuleId
                    lst.SkipTo = Lst1(i).SkipTo
                    lst.templateid = Lst1(i).Templateid
                    lst.TranPath = Lst1(i).TranPath
                    lst.Transactionid = Lst1(i).Transactionid
                    lst.TransactionStatus = Lst1(i).TransactionStatus
                    lst.Escalated = Lst1(i).Escalated
                    lst.DaysOpen = Lst1(i).DaysOpen
                    lst.Month = Lst1(i).Month
                    lst.SplUsers = Lst1(i).SplUsers
                    lst.UserType = Lst1(i).UserType
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.DynamicProperty = Lst1(i).DynamicProperty
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F200 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function FilteredeZWFlowTransationList(ByVal Criteria As String, ByVal Value As String) As List(Of eZWFlowTransation)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWFlowTransation)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadFilteredeZWFlowTransation(Criteria, Value)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F300DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWFlowTransation)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWFlowTransation
                    lst.Action = Lst1(i).Action
                    lst.ActionBy = Lst1(i).ActionBy
                    lst.ActionGroupBy = Lst1(i).ActionGroupBy
                    lst.ActivityGroupId = Lst1(i).ActivityGroupId
                    lst.ActivityId = Lst1(i).ActivityId
                    lst.ActivityUserId = Lst1(i).ActivityUserId
                    lst.Attachment = Lst1(i).Attachment
                    lst.DocCount = Lst1(i).DocCount
                    lst.FileType = Lst1(i).FileType
                    lst.Formid = Lst1(i).Formid
                    lst.LastActionStage = Lst1(i).LastActionStage
                    lst.LastActionReview = Lst1(i).LastActionReview
                    '  lst.FormName = Lst1(i).FormName
                    lst.FormTableName = Lst1(i).FormTableName
                    lst.FromMail = Lst1(i).FromMail
                    lst.FTemplateid = Lst1(i).FTemplateid
                    lst.ItemTableName = Lst1(i).ItemTableName
                    lst.LastActedBy = Lst1(i).LastActedBy
                    lst.LastActedOn = Lst1(i).LastActedOn
                    lst.notification = Lst1(i).Notification
                    lst.ProcessId = Lst1(i).ProcessId
                    lst.RaisedBy = Lst1(i).RaisedBy
                    lst.RaisedOn = Lst1(i).RaisedOn
                    lst.RequestNo = Lst1(i).RequestNo
                    lst.RequestType = Lst1(i).RequestType
                    lst.Review = Lst1(i).Review
                    lst.RuleId = Lst1(i).RuleId
                    lst.SkipTo = Lst1(i).SkipTo
                    lst.templateid = Lst1(i).Templateid
                    lst.TranPath = Lst1(i).TranPath
                    lst.Transactionid = Lst1(i).Transactionid
                    lst.TransactionStatus = Lst1(i).TransactionStatus
                    lst.Escalated = Lst1(i).Escalated
                    lst.DaysOpen = Lst1(i).DaysOpen
                    lst.Month = Lst1(i).Month
                    lst.SplUsers = Lst1(i).SplUsers
                    lst.UserType = Lst1(i).UserType
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.DynamicProperty = Lst1(i).DynamicProperty
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F300 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function SelectedeZWFlowTransationList(ByVal Criteria As String, ByVal Value As String) As List(Of eZWFlowTransation)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWFlowTransation)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadSelectedeZWFlowTransation(Criteria, Value)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F400DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWFlowTransation)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWFlowTransation
                    lst.Action = Lst1(i).Action
                    lst.ActionBy = Lst1(i).ActionBy
                    lst.ActionGroupBy = Lst1(i).ActionGroupBy
                    lst.ActivityGroupId = Lst1(i).ActivityGroupId
                    lst.ActivityId = Lst1(i).ActivityId
                    lst.ActivityUserId = Lst1(i).ActivityUserId
                    lst.Attachment = Lst1(i).Attachment
                    lst.DocCount = Lst1(i).DocCount
                    lst.FileType = Lst1(i).FileType
                    lst.Formid = Lst1(i).Formid
                    '  lst.FormName = Lst1(i).FormName
                    lst.FormTableName = Lst1(i).FormTableName
                    lst.FromMail = Lst1(i).FromMail
                    lst.FTemplateid = Lst1(i).FTemplateid
                    lst.LastActionStage = Lst1(i).LastActionStage
                    lst.LastActionReview = Lst1(i).LastActionReview
                    lst.ItemTableName = Lst1(i).ItemTableName
                    lst.LastActedBy = Lst1(i).LastActedBy
                    lst.LastActedOn = Lst1(i).LastActedOn
                    lst.notification = Lst1(i).Notification
                    lst.ProcessId = Lst1(i).ProcessId
                    lst.RaisedBy = Lst1(i).RaisedBy
                    lst.RaisedOn = Lst1(i).RaisedOn
                    lst.RequestNo = Lst1(i).RequestNo
                    lst.RequestType = Lst1(i).RequestType
                    lst.Review = Lst1(i).Review
                    lst.RuleId = Lst1(i).RuleId
                    lst.SkipTo = Lst1(i).SkipTo
                    lst.templateid = Lst1(i).Templateid
                    lst.TranPath = Lst1(i).TranPath
                    lst.Transactionid = Lst1(i).Transactionid
                    lst.TransactionStatus = Lst1(i).TransactionStatus
                    lst.Escalated = Lst1(i).Escalated
                    lst.DaysOpen = Lst1(i).DaysOpen
                    lst.Month = Lst1(i).Month
                    lst.SplUsers = Lst1(i).SplUsers
                    lst.UserType = Lst1(i).UserType
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.DynamicProperty = Lst1(i).DynamicProperty
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F400 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function InboxListbyUserid(Para As ProcessInfo) As List(Of eZWFlowTransation)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWFlowTransation)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadInboxListbyUserid(Para.WorkflowId, Para.ECMLoginId, Para.ECMGroupList, Para.RowFrom, Para.RowCount)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F400DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWFlowTransation)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWFlowTransation
                    lst.Action = Lst1(i).Action
                    lst.ActionBy = Lst1(i).ActionBy
                    lst.ActionGroupBy = Lst1(i).ActionGroupBy
                    lst.ActivityGroupId = Lst1(i).ActivityGroupId
                    lst.ActivityId = Lst1(i).ActivityId
                    lst.ActivityUserId = Lst1(i).ActivityUserId
                    lst.Attachment = Lst1(i).Attachment
                    lst.DocCount = Lst1(i).DocCount
                    lst.FileType = Lst1(i).FileType
                    lst.Formid = Lst1(i).Formid
                    lst.LastActionStage = Lst1(i).LastActionStage
                    lst.LastActionReview = Lst1(i).LastActionReview
                    '  lst.FormName = Lst1(i).FormName
                    lst.FormTableName = Lst1(i).FormTableName
                    lst.FromMail = Lst1(i).FromMail
                    lst.FTemplateid = Lst1(i).FTemplateid
                    lst.ItemTableName = Lst1(i).ItemTableName
                    lst.LastActedBy = Lst1(i).LastActedBy
                    lst.LastActedOn = Lst1(i).LastActedOn
                    lst.notification = Lst1(i).Notification
                    lst.ProcessId = Lst1(i).ProcessId
                    lst.RaisedBy = Lst1(i).RaisedBy
                    lst.RaisedOn = Lst1(i).RaisedOn
                    lst.RequestNo = Lst1(i).RequestNo
                    lst.RequestType = Lst1(i).RequestType
                    lst.Review = Lst1(i).Review
                    lst.RuleId = Lst1(i).RuleId
                    lst.SkipTo = Lst1(i).SkipTo
                    lst.templateid = Lst1(i).Templateid
                    lst.TranPath = Lst1(i).TranPath
                    lst.Transactionid = Lst1(i).Transactionid
                    lst.TransactionStatus = Lst1(i).TransactionStatus
                    lst.DynamicProperty = Lst1(i).DynamicProperty
                    lst.Escalated = Lst1(i).Escalated
                    lst.DaysOpen = Lst1(i).DaysOpen
                    lst.Month = Lst1(i).Month
                    lst.SplUsers = Lst1(i).SplUsers
                    lst.UserType = Lst1(i).UserType
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F400 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function

    Public Shared Function QueueListbyUserid(Para As ProcessInfo) As List(Of eZWFlowTransation)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWFlowTransation)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadQueueListbyUserid(Para.WorkflowId, Para.ECMLoginId, Para.ECMGroupList, Para.RowFrom, Para.RowCount)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F400DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWFlowTransation)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWFlowTransation
                    lst.Action = Lst1(i).Action
                    lst.ActionBy = Lst1(i).ActionBy
                    lst.ActionGroupBy = Lst1(i).ActionGroupBy
                    lst.ActivityGroupId = Lst1(i).ActivityGroupId
                    lst.ActivityId = Lst1(i).ActivityId
                    lst.ActivityUserId = Lst1(i).ActivityUserId
                    lst.Attachment = Lst1(i).Attachment
                    lst.DocCount = Lst1(i).DocCount
                    lst.FileType = Lst1(i).FileType
                    lst.Formid = Lst1(i).Formid
                    lst.LastActionStage = Lst1(i).LastActionStage
                    lst.LastActionReview = Lst1(i).LastActionReview
                    '  lst.FormName = Lst1(i).FormName
                    lst.FormTableName = Lst1(i).FormTableName
                    lst.FromMail = Lst1(i).FromMail
                    lst.FTemplateid = Lst1(i).FTemplateid
                    lst.ItemTableName = Lst1(i).ItemTableName
                    lst.LastActedBy = Lst1(i).LastActedBy
                    lst.LastActedOn = Lst1(i).LastActedOn
                    lst.notification = Lst1(i).Notification
                    lst.ProcessId = Lst1(i).ProcessId
                    lst.RaisedBy = Lst1(i).RaisedBy
                    lst.RaisedOn = Lst1(i).RaisedOn
                    lst.RequestNo = Lst1(i).RequestNo
                    lst.RequestType = Lst1(i).RequestType
                    lst.Review = Lst1(i).Review
                    lst.RuleId = Lst1(i).RuleId
                    lst.SkipTo = Lst1(i).SkipTo
                    lst.templateid = Lst1(i).Templateid
                    lst.TranPath = Lst1(i).TranPath
                    lst.Transactionid = Lst1(i).Transactionid
                    lst.TransactionStatus = Lst1(i).TransactionStatus
                    lst.DynamicProperty = Lst1(i).DynamicProperty
                    lst.Escalated = Lst1(i).Escalated
                    lst.DaysOpen = Lst1(i).DaysOpen
                    lst.Month = Lst1(i).Month
                    lst.SplUsers = Lst1(i).SplUsers
                    lst.UserType = Lst1(i).UserType
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F400 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
#End Region
#Region "eZWFProcess"
    Public Shared Function InsertAndUpdateeZWFProcess(ByVal OBJ As eZWFProcess) As Integer

        Try
            Dim objEmp As IeZWFProcess = Nothing
            If OBJ.ProcessId = 0 Then
                Try
                    objEmp = DBLayer.DBLInstance.CreateeZWFProcess(OBJ)
                Catch ex As Exception
                    Dim exc As String
                    exc = "ERROR CODE : WDBRJ500F3000DB10 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            Else
                Try
                    objEmp = DBLayer.DBLInstance.GlobalInstance.eZWFProcess(OBJ.ProcessId)
                    objEmp = OBJ
                    objEmp.SaveChanges()
                Catch ex As Exception
                    Dim exc As String
                    exc = "ERROR CODE : WDBRJ500F3000DB20 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            End If
            If objEmp IsNot Nothing Then
                Return objEmp.ProcessId
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New FaultException("ERROR CODE : WDBRJ500F3000 : " + ex.ToString)
        End Try
    End Function
    Public Shared Function ListeZWFProcess() As DataSet
        Try
            Dim GetXml As StreamReader
            Dim str As String
            Dim ds As New DataSet
            Dim column() = {"ifilepath"}
            Dim dtTemp As New DataTable
            Dim dt As New DataTable
            ds = DBLayer.DBLInstance.GetdatasetbySPwithoutParam("SP_ListeZWFProcess")
            If ds.Tables.Count <> 0 Then
                Dim dtpath = ds.Tables(0).DefaultView.ToTable(True, column)
                For L As Integer = 0 To dtpath.Rows.Count - 1
                    GetXml = New System.IO.StreamReader(dtpath.Rows(L).Item("iFilePath").ToString())
                    str = GetXml.ReadToEnd
                    GetXml.Close()
                    Dim ifilepa = dtpath.Rows(L).Field(Of String)("ifilepath")
                    dtTemp = ds.Tables(0).Select("ifilepath='" + ifilepa.ToString() + "'").CopyToDataTable
                    ' Dim xElem = XElement.Load(ds.Tables(0).Rows(L).Item("iFilePath").ToString())
                    If String.IsNullOrEmpty(str) Then
                        Array.ForEach(dtTemp.AsEnumerable().ToArray(), Sub(row) row("XMLString") = "")
                    Else
                        Array.ForEach(dtTemp.AsEnumerable().ToArray(), Sub(row) row("XMLString") = str.ToString())
                    End If
                    dt.Merge(dtTemp)
                    dtTemp.Clear()
                Next
                ds.Tables(0).Clear()
                ds.Tables(0).Merge(dt)
            End If
            Return ds
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR990F200 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function
    Public Shared Function eZWFProcessList() As List(Of eZWFProcess)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWFProcess)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadAlleZWFProcess()
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F200DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWFProcess)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWFProcess
                    lst.FlowStatus = Lst1(i).FlowStatus
                    lst.ProcessId = Lst1(i).ProcessId
                    lst.RequestNo = Lst1(i).RequestNo
                    lst.Templateid = Lst1(i).Templateid
                    lst.WorkflowId = Lst1(i).WorkflowId
                    lst.Workflowtypeid = Lst1(i).Workflowtypeid
                    lst.Itemid = Lst1(i).Itemid
                    lst.DocCount = Lst1(i).DocCount
                    lst.Action = Lst1(i).Action
                    lst.SplUsers = Lst1(i).SplUsers
                    lst.Month = Lst1(i).Month
                    lst.DaysOpen = Lst1(i).DaysOpen
                    lst.Escalated = Lst1(i).Escalated
                    lst.LastActionStage = Lst1(i).LastActionStage
                    lst.LastActionReview = Lst1(i).LastActionReview
                    lst.ActionBy = Lst1(i).ActionBy
                    lst.Formid = Lst1(i).Formid
                    lst.DynamicProperty = Lst1(i).DynamicProperty
                    lst.FormTableName = Lst1(i).FormTableName
                    lst.FTemplateid = Lst1(i).FTemplateid
                    lst.ifilepath = Lst1(i).ifilepath
                    lst.ItemTableName = Lst1(i).ItemTableName
                    lst.LastActedBy = Lst1(i).LastActedBy
                    lst.LastActedOn = Lst1(i).LastActedOn
                    lst.RaisedBy = Lst1(i).RaisedBy
                    lst.RaisedOn = Lst1(i).RaisedOn
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F200 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function FilteredeZWFProcessList(ByVal Criteria As String, ByVal Value As String) As List(Of eZWFProcess)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWFProcess)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadFilteredeZWFProcess(Criteria, Value)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F300DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWFProcess)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWFProcess
                    lst.FlowStatus = Lst1(i).FlowStatus
                    lst.ProcessId = Lst1(i).ProcessId
                    lst.RequestNo = Lst1(i).RequestNo
                    lst.Templateid = Lst1(i).Templateid
                    lst.WorkflowId = Lst1(i).WorkflowId
                    lst.Workflowtypeid = Lst1(i).Workflowtypeid
                    lst.Itemid = Lst1(i).Itemid
                    lst.Action = Lst1(i).Action
                    lst.LastActionStage = Lst1(i).LastActionStage
                    lst.LastActionReview = Lst1(i).LastActionReview
                    lst.DocCount = Lst1(i).DocCount
                    lst.SplUsers = Lst1(i).SplUsers
                    lst.Month = Lst1(i).Month
                    lst.DaysOpen = Lst1(i).DaysOpen
                    lst.Escalated = Lst1(i).Escalated
                    lst.ActionBy = Lst1(i).ActionBy
                    lst.Formid = Lst1(i).Formid
                    lst.DynamicProperty = Lst1(i).DynamicProperty
                    lst.FormTableName = Lst1(i).FormTableName
                    lst.FTemplateid = Lst1(i).FTemplateid
                    lst.ifilepath = Lst1(i).ifilepath
                    lst.ItemTableName = Lst1(i).ItemTableName
                    lst.LastActedBy = Lst1(i).LastActedBy
                    lst.LastActedOn = Lst1(i).LastActedOn
                    lst.RaisedBy = Lst1(i).RaisedBy
                    lst.RaisedOn = Lst1(i).RaisedOn
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F300 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function SelectedeZWFProcessList(ByVal Criteria As String, ByVal Value As String) As List(Of eZWFProcess)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWFProcess)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadSelectedeZWFProcess(Criteria, Value)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F400DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWFProcess)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWFProcess
                    lst.FlowStatus = Lst1(i).FlowStatus
                    lst.ProcessId = Lst1(i).ProcessId
                    lst.RequestNo = Lst1(i).RequestNo
                    lst.Templateid = Lst1(i).Templateid
                    lst.WorkflowId = Lst1(i).WorkflowId
                    lst.LastActionStage = Lst1(i).LastActionStage
                    lst.LastActionReview = Lst1(i).LastActionReview
                    lst.Action = Lst1(i).Action
                    lst.Workflowtypeid = Lst1(i).Workflowtypeid
                    lst.Itemid = Lst1(i).Itemid
                    lst.DocCount = Lst1(i).DocCount
                    lst.SplUsers = Lst1(i).SplUsers
                    lst.Month = Lst1(i).Month
                    lst.DaysOpen = Lst1(i).DaysOpen
                    lst.Escalated = Lst1(i).Escalated
                    lst.ActionBy = Lst1(i).ActionBy
                    lst.Formid = Lst1(i).Formid
                    lst.DynamicProperty = Lst1(i).DynamicProperty
                    lst.FormTableName = Lst1(i).FormTableName
                    lst.FTemplateid = Lst1(i).FTemplateid
                    lst.ifilepath = Lst1(i).ifilepath
                    lst.ItemTableName = Lst1(i).ItemTableName
                    lst.LastActedBy = Lst1(i).LastActedBy
                    lst.LastActedOn = Lst1(i).LastActedOn
                    lst.RaisedBy = Lst1(i).RaisedBy
                    lst.RaisedOn = Lst1(i).RaisedOn
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F400 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function ProcessListbyUserid(Para As ProcessInfo) As List(Of eZWFProcess)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWFProcess)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadProcessListbyUserid(Para.WorkflowId, Para.ECMLoginId, Para.ECMGroupList, Para.RowFrom, Para.RowCount)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F400DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWFProcess)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWFProcess
                    lst.FlowStatus = Lst1(i).FlowStatus
                    lst.ProcessId = Lst1(i).ProcessId
                    lst.RequestNo = Lst1(i).RequestNo
                    lst.Templateid = Lst1(i).Templateid
                    lst.WorkflowId = Lst1(i).WorkflowId
                    lst.Workflowtypeid = Lst1(i).Workflowtypeid
                    lst.Itemid = Lst1(i).Itemid
                    lst.DocCount = Lst1(i).DocCount
                    lst.SplUsers = Lst1(i).SplUsers
                    lst.Month = Lst1(i).Month
                    lst.DaysOpen = Lst1(i).DaysOpen
                    lst.LastActionStage = Lst1(i).LastActionStage
                    lst.LastActionReview = Lst1(i).LastActionReview
                    lst.Escalated = Lst1(i).Escalated
                    lst.ActionBy = Lst1(i).ActionBy
                    lst.Action = Lst1(i).Action
                    lst.Formid = Lst1(i).Formid
                    lst.DynamicProperty = Lst1(i).DynamicProperty
                    lst.FormTableName = Lst1(i).FormTableName
                    lst.FTemplateid = Lst1(i).FTemplateid
                    lst.ifilepath = Lst1(i).ifilepath
                    lst.ItemTableName = Lst1(i).ItemTableName
                    lst.LastActedBy = Lst1(i).LastActedBy
                    lst.LastActedOn = Lst1(i).LastActedOn
                    lst.RaisedBy = Lst1(i).RaisedBy
                    lst.RaisedOn = Lst1(i).RaisedOn
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F400 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function



    Public Shared Function CompletedListbyUserid(Para As ProcessInfo) As List(Of eZWFProcess)
        Dim faultmsg As String
        Dim Lst1 As New List(Of IeZWFProcess)()
        Try
            Lst1 = DBLayer.DBLInstance.ReadCompletedListbyUserid(Para.WorkflowId, Para.ECMLoginId, Para.ECMGroupList, Para.RowFrom, Para.RowCount)
        Catch ex As Exception
            faultmsg = "ERROR CODE:WDBR200F400DB30 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Dim ListItems As New List(Of eZWFProcess)()
        Try
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZWFProcess
                    lst.FlowStatus = Lst1(i).FlowStatus
                    lst.ProcessId = Lst1(i).ProcessId
                    lst.RequestNo = Lst1(i).RequestNo
                    lst.Templateid = Lst1(i).Templateid
                    lst.WorkflowId = Lst1(i).WorkflowId
                    lst.Workflowtypeid = Lst1(i).Workflowtypeid
                    lst.Itemid = Lst1(i).Itemid
                    lst.DocCount = Lst1(i).DocCount
                    lst.Action = Lst1(i).Action
                    lst.LastActionStage = Lst1(i).LastActionStage
                    lst.LastActionReview = Lst1(i).LastActionReview
                    lst.SplUsers = Lst1(i).SplUsers
                    lst.Month = Lst1(i).Month
                    lst.DaysOpen = Lst1(i).DaysOpen
                    lst.Escalated = Lst1(i).Escalated
                    lst.ActionBy = Lst1(i).ActionBy
                    lst.Formid = Lst1(i).Formid
                    lst.DynamicProperty = Lst1(i).DynamicProperty
                    lst.FormTableName = Lst1(i).FormTableName
                    lst.FTemplateid = Lst1(i).FTemplateid
                    lst.ifilepath = Lst1(i).ifilepath
                    lst.ItemTableName = Lst1(i).ItemTableName
                    lst.LastActedBy = Lst1(i).LastActedBy
                    lst.LastActedOn = Lst1(i).LastActedOn
                    lst.RaisedBy = Lst1(i).RaisedBy
                    lst.RaisedOn = Lst1(i).RaisedOn
                    lst.Createdby = Lst1(i).Createdby
                    lst.Createdon = Lst1(i).Createdon
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Updatedby = Lst1(i).Updatedby
                    lst.Updatedon = Lst1(i).Updatedon
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
        Catch ex As Exception
            faultmsg = "ERROR CODE:WSR200F400 " + ex.ToString()
            Throw New FaultException(faultmsg)
        End Try
        Return ListItems
    End Function
    Public Shared Function GetFormvalues(Processid As String, Transid As String, Workflowid As String) As List(Of ezFrmDetails)
        Dim sql = "", Formid = "", Formentryid = ""
        Dim objfrmdet As New List(Of ezFrmDetails)
        Dim iobjfrmdet As New ezFrmDetails
        sql = "Select * from ezprocessitems where Processid='" + Processid.ToString + "' and Formid<>0 and FormEntryId<>0"
        Dim pitemds = GetDatasetByQuery(sql)
        If Not pitemds Is Nothing AndAlso pitemds.Tables.Count > 0 AndAlso pitemds.Tables(0).Rows.Count > 0 Then
            Formid = pitemds.Tables(0).Rows(0).Item("FormId")
            Formentryid = pitemds.Tables(0).Rows(0).Item("FormEntryId")
        End If
        Dim Tablename = ""
        sql = "Select * from eZWFlowFormDetails where Formid='" + Formid.ToString + "'"
        Dim Formds = GetDatasetByQuery(sql)
        If Not Formds Is Nothing AndAlso Formds.Tables.Count > 0 AndAlso Formds.Tables(0).Rows.Count > 0 Then
            Tablename = Formds.Tables(0).Rows(0).Item("tablename")
        End If
        sql = "Select * from [" + Tablename + "] where itemid='" + Formentryid.ToString + "'"
        Dim Formvalueds = GetDatasetByQuery(sql)
        Dim dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)
        If Not Formvalueds Is Nothing AndAlso Formvalueds.Tables.Count > 0 AndAlso Formvalueds.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In Formvalueds.Tables(0).Rows
                For Each col As DataColumn In Formvalueds.Tables(0).Columns
                    ' dictionary.Add(col.ColumnName.Replace("[", "").Replace("]", ""), row(col.ColumnName).ToString)
                    Try
                        dictionary.Add(col.ColumnName.Replace("[", "").Replace("]", ""), row(col.ColumnName).ToString)
                    Catch ex As Exception
                    End Try
                Next
            Next
        End If
        iobjfrmdet.DynamicProperty = dictionary

        Dim Sql1 = "Select * from eZWFlowFormDetails where parentformid='" + Formid.ToString + "'"
        Dim subtableds = GetDatasetByQuery(Sql1)
        Dim SubTablename = ""
        If Not subtableds Is Nothing AndAlso subtableds.Tables.Count > 0 AndAlso subtableds.Tables(0).Rows.Count > 0 Then
            SubTablename = subtableds.Tables(0).Rows(0).Item("tablename")
        End If
        sql = "Select * from [" + SubTablename + "] where fid='" + Formentryid.ToString + "'"
        Dim SubTablenameds = GetDatasetByQuery(sql)
        Dim subdictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)
        If Not SubTablenameds Is Nothing AndAlso SubTablenameds.Tables.Count > 0 AndAlso SubTablenameds.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In SubTablenameds.Tables(0).Rows
                For Each col As DataColumn In SubTablenameds.Tables(0).Columns
                    ' dictionary.Add(col.ColumnName.Replace("[", "").Replace("]", ""), row(col.ColumnName).ToString)
                    Try
                        subdictionary.Add(col.ColumnName.Replace("[", "").Replace("]", ""), row(col.ColumnName).ToString)
                    Catch ex As Exception
                    End Try
                Next
            Next
        End If
        iobjfrmdet.DynamicProp = subdictionary
        sql = "Select WorkflowName from eZWorkflowDetails where WorkflowId='" + Workflowid.ToString + "' and isdeleted=0"
        Dim workflowsqLds = GetDatasetByQuery(sql)
        Dim Workflowname = ""
        If Not workflowsqLds Is Nothing AndAlso workflowsqLds.Tables.Count > 0 AndAlso workflowsqLds.Tables(0).Rows.Count > 0 Then
            Workflowname = workflowsqLds.Tables(0).Rows(0).Item("WorkflowName")
        End If
        sql = "Select ifilepath,ifilename,ERSId from [eZCA_1_4_items] where DesignName='" + Workflowname.ToString + "' and DesignFor='Workflow'"
        Dim ds = GetDatasetByQuery(sql)
        Dim ifilepath = "", ifilename = "", ERSId = ""
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            ifilepath = ds.Tables(0).Rows(0).Item("ifilepath")
            ifilename = ds.Tables(0).Rows(0).Item("ifilename")
            ERSId = ds.Tables(0).Rows(0).Item("ERSId")
        End If
        sql = "Select ERSDirPath from ezersinfo Where ERSId='" + ERSId.ToString + "'"
        Dim ersds = GetDatasetByQuery(sql)
        Dim ersdirpath = ""
        If Not ersds Is Nothing AndAlso ersds.Tables.Count > 0 AndAlso ersds.Tables(0).Rows.Count > 0 Then
            ersdirpath = ersds.Tables(0).Rows(0).Item("ERSDirPath")
        End If
        sql = "Select ActivityID from ezwflowtransation where Transactionid='" + Transid.ToString + "'"
        Dim transds = GetDatasetByQuery(sql)
        Dim Activityid = ""
        If Not transds Is Nothing AndAlso transds.Tables.Count > 0 AndAlso transds.Tables(0).Rows.Count > 0 Then
            Activityid = transds.Tables(0).Rows(0).Item("ActivityID")
        End If
        Dim Proceedwith = ""
        Dim xmlds As New DataSet
        Dim filepath = ersdirpath + ifilepath + ifilename
        xmlds.ReadXml(filepath)
        For Each xmlactrow1 As DataRow In xmlds.Tables("Activity").Select("ActivityID='" + Activityid + "'")
            If xmlactrow1.Table.Columns.Contains("ProceedWith") Then
                Proceedwith = xmlactrow1("ProceedWith").ToString
            End If
        Next
        iobjfrmdet.Proceedwith = Proceedwith.ToString
        objfrmdet.Add(iobjfrmdet)
        Return objfrmdet
    End Function
#End Region

#Region "Cabinet And Template"

    Public Shared Function GetItemUserList(ByVal TemplateId As Integer, ByVal reportfor As String) As IEnumerable(Of Object)

        Try
            Dim Tablename = GetTableName(TemplateId)
            Dim Userlist As New List(Of String)
            If Tablename <> "" And reportfor = "ECM-Capture" Then
                Dim ItemList = GetDatasetByQuery("Select distinct dbo.udf_LoginName (CreatedBy) as LoginName from " + Tablename + " where ezfrom like '%Scanned(%' Or ezfrom like '%Digital(%'")
                If Not IsNothing(ItemList) AndAlso ItemList.Tables.Count > 0 AndAlso ItemList.Tables(0).Rows.Count > 0 Then
                    For Each row In ItemList.Tables(0).Rows
                        Userlist.Add(row("LoginName").ToString())
                    Next
                End If

                Dim results = Userlist.ToList()
                Return results
            ElseIf Tablename <> "" And reportfor = "All" Then
                Dim ItemList = GetDatasetByQuery("Select distinct dbo.udf_LoginName (CreatedBy) as LoginName from " + Tablename)
                If Not IsNothing(ItemList) AndAlso ItemList.Tables.Count > 0 AndAlso ItemList.Tables(0).Rows.Count > 0 Then
                    For Each row In ItemList.Tables(0).Rows
                        Userlist.Add(row("LoginName").ToString())
                    Next
                End If

                Dim results = Userlist.ToList()
                Return results

            End If
        Catch ex As Exception

        End Try
    End Function


    Public Shared Function GetItemApplicationList(ByVal TemplateId As Integer, ByVal reportfor As String) As IEnumerable(Of Object)

        Try
            Dim Tablename = GetTableName(TemplateId)

            Dim Applicationlist As New List(Of String)
            If Tablename <> "" And reportfor = "ECM-Capture" Then
                Dim ItemList = GetDatasetByQuery("Select distinct Replace(Replace(Replace(ezfrom,'ECM-Capture',''),'Scanned',''),'Digital','') as ezfrom from ezca_3_9_items where eZFrom<>'' and eZFrom not like 'ECM-Server%'")
                If Not IsNothing(ItemList) AndAlso ItemList.Tables.Count > 0 AndAlso ItemList.Tables(0).Rows.Count > 0 Then
                    For Each row In ItemList.Tables(0).Rows
                        Applicationlist.Add(row("ezfrom").ToString())
                    Next
                End If
                Dim results = Applicationlist.ToList()
                Return results
            ElseIf Tablename <> "" And reportfor = "All" Then
                Dim ItemList = GetDatasetByQuery("Select distinct ezfrom from " + Tablename)
                If Not IsNothing(ItemList) AndAlso ItemList.Tables.Count > 0 AndAlso ItemList.Tables(0).Rows.Count > 0 Then
                    For Each row In ItemList.Tables(0).Rows
                        Applicationlist.Add(row("ezfrom").ToString())
                    Next
                End If
                Dim results = Applicationlist.ToList()
                Return results
            End If
        Catch ex As Exception

        End Try
    End Function





    Public Shared Function SelectedeZCabinetList(ByVal Criteria As String, ByVal Value As String) As List(Of eZCabinet)
        Try
            Dim Lst1 As New List(Of IeZCabinet)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZCabinet(Criteria, Value)
            Catch ex As Exception
                Dim exc As String
                exc = "ERROR CODE:WDBR210F600DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of eZCabinet)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZCabinet
                    lst.CabinetName = Lst1(i).CabinetName
                    lst.CabinetID = Lst1(i).CabinetID
                    lst.CabOwnerID = Lst1(i).CabOwnerID
                    'lst.CabIcon = Lst1(i).CabIcon
                    lst.CabExpiryDate = Lst1(i).CabExpiryDate
                    lst.UserId = Lst1(i).UserId
                    lst.CabOwnerName = Lst1(i).CabOwnerName
                    lst.CabSize = Lst1(i).CabSize
                    lst.CabCurrentSize = Lst1(i).CabCurrentSize
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Description = Lst1(i).Description
                    lst.ERSDirPath = Lst1(i).ERSDirPath
                    lst.ERSIndexinpath = Lst1(i).ERSIndexinpath
                    lst.ERSId = Lst1(i).ERSId
                    lst.ERSName = Lst1(i).ERSName
                    lst.ERSServerName = Lst1(i).ERSServerName
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR210F600 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function

    Public Shared Function eZCabinetListByLoginId(ByVal LoginId As Integer) As List(Of eZCabinet)
        Try
            Dim Result As New List(Of eZCabinet)()
            If GetECMUserTypeByLoginId(LoginId) = 1 Then
                Try
                    Result = eZCabinetList()
                    'If Result.Count <> 0 Then
                    '    Result.RemoveAll(Function(i) i.CabinetID = "1")
                    'End If
                Catch ex As Exception
                    Dim exc As String
                    exc = "eZCabinetListByLoginId Cabinet Retrival : " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
            Else
                'If GetECMUserTypeByLoginId(LoginId) = 2 Then
                Result = SelectedeZCabinetListByUserId(LoginId)
                If Result.Count <> 0 Then
                    Result.RemoveAll(Function(i) i.CabinetID = "1")
                End If
                'Else
                '    Result = SelectedeZCabinetList("cabinetid", "1")
                'End If
                Dim Lst1 As New List(Of IeZECMCabinetLevel)()
                Try
                    Lst1 = DBLayer.DBLInstance.ReadSelectedeZECMCabinetLevel("ECMLoginId", LoginId.ToString())
                Catch ex As Exception
                    Dim exc As String
                    exc = "ERROR CODE:WDBR210F400DB30 " + ex.ToString()
                    Throw New FaultException(exc)
                End Try
                If Lst1.Count <> 0 Then
                    For K As Integer = 0 To Lst1.Count - 1
                        Dim fl As Integer = 0
                        For l As Integer = 0 To Result.Count - 1
                            If Result(l).CabinetID = Lst1(K).CabinetId Then
                                fl = 1
                            End If
                        Next
                        If fl = 0 Then
                            Dim CabinetLst1 As New List(Of IeZCabinet)()
                            Try
                                CabinetLst1 = DBLayer.DBLInstance.ReadSelectedeZCabinet("CabinetID", Lst1(K).CabinetId.ToString())
                            Catch ex As Exception
                                Dim exc As String
                                exc = "ERROR CODE:WDBR210F400DB31 " + ex.ToString()
                                Throw New FaultException(exc)
                            End Try
                            If CabinetLst1.Count <> 0 Then
                                For i As Integer = 0 To CabinetLst1.Count - 1
                                    Dim lst As New eZCabinet
                                    lst.CabinetName = CabinetLst1(i).CabinetName
                                    lst.CabinetID = CabinetLst1(i).CabinetID
                                    lst.CabOwnerID = CabinetLst1(i).CabOwnerID
                                    'lst.CabIcon = CabinetLst1(i).CabIcon
                                    lst.CabExpiryDate = CabinetLst1(i).CabExpiryDate
                                    lst.UserId = CabinetLst1(i).UserId
                                    lst.CabOwnerName = CabinetLst1(i).CabOwnerName
                                    lst.CabSize = CabinetLst1(i).CabSize
                                    lst.CabCurrentSize = CabinetLst1(i).CabCurrentSize
                                    'lst.CabCurrentSize = GetSizeById(CabinetLst1(i).CabinetID, 0) ' GetFolderSize(CabinetLst1(i).ERSDirPath + "\" + CabinetLst1(i).CabinetName)
                                    lst.CreatedBy = CabinetLst1(i).CreatedBy
                                    lst.CreatedOn = CabinetLst1(i).CreatedOn
                                    lst.CreatedBy1 = CabinetLst1(i).CreatedBy1
                                    lst.Description = CabinetLst1(i).Description
                                    lst.ERSDirPath = CabinetLst1(i).ERSDirPath
                                    lst.ERSIndexinpath = CabinetLst1(i).ERSIndexinpath
                                    lst.ERSId = CabinetLst1(i).ERSId
                                    lst.ERSName = CabinetLst1(i).ERSName
                                    lst.ERSServerName = CabinetLst1(i).ERSServerName
                                    lst.UpdatedBy = CabinetLst1(i).UpdatedBy
                                    lst.UpdatedOn = CabinetLst1(i).UpdatedOn
                                    lst.UpdatedBy1 = CabinetLst1(i).UpdatedBy1
                                    lst.SNo = i + 1
                                    Result.Add(lst)
                                Next
                            End If
                        End If
                    Next
                End If
            End If
            Return Result
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR210F400 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function

    Public Shared Function eZCabinetList() As List(Of eZCabinet)
        Try
            Dim Lst1 As New List(Of IeZCabinet)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadAlleZCabinet()
            Catch ex As Exception
                Dim exc As String
                exc = "ERROR CODE:WDBR210F200DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of eZCabinet)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZCabinet
                    lst.CabinetName = Lst1(i).CabinetName
                    lst.CabinetID = Lst1(i).CabinetID
                    lst.CabOwnerID = Lst1(i).CabOwnerID
                    'lst.CabIcon = Lst1(i).CabIcon
                    lst.CabExpiryDate = Lst1(i).CabExpiryDate
                    lst.UserId = Lst1(i).UserId
                    lst.CabOwnerName = Lst1(i).CabOwnerName
                    lst.CabSize = Lst1(i).CabSize
                    lst.CabCurrentSize = Lst1(i).CabCurrentSize
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Description = Lst1(i).Description
                    lst.ERSDirPath = Lst1(i).ERSDirPath
                    lst.ERSIndexinpath = Lst1(i).ERSIndexinpath
                    lst.ERSId = Lst1(i).ERSId
                    lst.ERSName = Lst1(i).ERSName
                    lst.ERSServerName = Lst1(i).ERSServerName
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR210F200 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function

    Public Shared Function SelectedeZCabinetListByUserId(ByVal UserId As String) As List(Of eZCabinet)
        Try
            Dim Lst1 As New List(Of IeZCabinet)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZCabinetByuserid(UserId)
            Catch ex As Exception
                Dim exc As String
                exc = "ERROR CODE:WDBR210F700DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of eZCabinet)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZCabinet
                    lst.CabinetName = Lst1(i).CabinetName
                    lst.CabinetID = Lst1(i).CabinetID
                    lst.CabOwnerID = Lst1(i).CabOwnerID
                    'lst.CabIcon = Lst1(i).CabIcon
                    lst.CabExpiryDate = Lst1(i).CabExpiryDate
                    lst.UserId = Lst1(i).UserId
                    lst.CabOwnerName = Lst1(i).CabOwnerName
                    lst.CabSize = Lst1(i).CabSize
                    lst.CabCurrentSize = Lst1(i).CabCurrentSize
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Description = Lst1(i).Description
                    lst.ERSDirPath = Lst1(i).ERSDirPath
                    lst.ERSIndexinpath = Lst1(i).ERSIndexinpath
                    lst.ERSId = Lst1(i).ERSId
                    lst.ERSName = Lst1(i).ERSName
                    lst.ERSServerName = Lst1(i).ERSServerName
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR210F700 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function


    Public Shared Function eZTemplateListWithCabinetId(CabinetID As Integer) As List(Of eZTemplate)
        Try
            Dim Lst1 As New List(Of IeZTemplate)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZTemplate("cabinetid", CabinetID.ToString)
            Catch ex As Exception
                Dim exc As String
                exc = "ERROR CODE:WDBR220F200DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of eZTemplate)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZTemplate
                    lst.TemplateName = Lst1(i).TemplateName
                    lst.TableName = Lst1(i).TableName
                    lst.TemplateId = Lst1(i).TemplateId
                    lst.DuplicateType = Lst1(i).DuplicateType
                    lst.DuplicateTypeId = Lst1(i).DuplicateTypeId
                    lst.CabinetID = Lst1(i).CabinetID
                    lst.CabinetName = Lst1(i).CabinetName
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Description = Lst1(i).Description
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.TempCurrentSize = Lst1(i).TempCurrentSize
                    lst.Encrypt = Lst1(i).Encrypt
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR220F200 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function


    Public Shared Function SelectedeZTemplateFieldList(ByVal Criteria As String, ByVal Value As String) As List(Of eZTemplateField)
        Try
            Dim Lst1 As New List(Of IeZTemplateField)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZTemplateField(Criteria, Value)
            Catch ex As Exception
                Dim exc As String
                exc = "ERROR CODE:WDBR250F400DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            'Lst1 = DBLayer.DBLInstance.ReadSelectedeZTemplateFieldForGCC(Criteria, Value)
            Dim ListItems As New List(Of eZTemplateField)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZTemplateField
                    lst.TemplateName = Lst1(i).TemplateName
                    lst.TemplateId = Lst1(i).TemplateID
                    lst.DataType = Lst1(i).DataType
                    lst.DataTypeId = Lst1(i).DataTypeId
                    lst.FieldId = Lst1(i).FieldId
                    'lst.BarcodeTypeId = Lst1(i).BarcodeTypeId
                    'lst.BarcodeType = Lst1(i).BarcodeType
                    lst.TableName = Lst1(i).TableName
                    lst.FieldName = Lst1(i).FieldName
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.FieldLevel = Lst1(i).FieldLevel
                    lst.Mandatory = Lst1(i).Mandatory
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.IsEditable = Lst1(i).IsEditable
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR250F400 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function


    Public Shared Function SelectedeZTemplateList(ByVal Criteria As String, ByVal Value As String) As List(Of eZTemplate)
        Try
            Dim Lst1 As New List(Of IeZTemplate)()
            Try
                Lst1 = DBLayer.DBLInstance.ReadSelectedeZTemplate(Criteria, Value)
            Catch ex As Exception
                Dim exc As String
                exc = "ERROR CODE:WDBR220F500DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            Dim ListItems As New List(Of eZTemplate)()
            If Lst1.Count <> 0 Then
                For i As Integer = 0 To Lst1.Count - 1
                    Dim lst As New eZTemplate
                    lst.TableName = Lst1(i).TableName
                    lst.TemplateName = Lst1(i).TemplateName
                    lst.TemplateId = Lst1(i).TemplateId
                    lst.DuplicateType = Lst1(i).DuplicateType
                    lst.DuplicateTypeId = Lst1(i).DuplicateTypeId
                    lst.CabinetID = Lst1(i).CabinetID
                    lst.CabinetName = Lst1(i).CabinetName
                    lst.CreatedBy = Lst1(i).CreatedBy
                    lst.CreatedOn = Lst1(i).CreatedOn
                    lst.CreatedBy1 = Lst1(i).CreatedBy1
                    lst.Description = Lst1(i).Description
                    lst.UpdatedBy = Lst1(i).UpdatedBy
                    lst.UpdatedOn = Lst1(i).UpdatedOn
                    lst.UpdatedBy1 = Lst1(i).UpdatedBy1
                    lst.Encrypt = Lst1(i).Encrypt
                    lst.SNo = i + 1
                    ListItems.Add(lst)
                Next
            End If
            Return ListItems
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR220F500 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function

    Public Shared Function GetTableName(ByVal TemplateId As Integer) As String
        Try

            'Dim query As String = "select dbo.udf_TableName(" + TemplateId.ToString + ") as TableName"
            'Dim ds As DataSet = DBLayer.DBLInstance.GetDatasetByQuery(query)
            'If ds.Tables.Count <> 0 Then
            '    If ds.Tables(0).Rows.Count <> 0 Then
            '        Return ds.Tables(0).Rows(0).Item(0).ToString()
            '    Else
            '        Return ""
            '    End If
            'Else
            '    Return ""
            'End If
            Return DBLayer.DBLInstance.GetTableNameByTemplateId(TemplateId)
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WDBR740F140DB30 " + ex.ToString()
            Throw New FaultException(exc)
        End Try

    End Function


    Public Shared Function RemoveSpecialChar(ByVal value As String, ByVal ReplaceWith As String) As String
        Dim res = ""
        Try
            res = value.Trim.Replace("/", ReplaceWith).Replace(":", ReplaceWith).Replace("\", ReplaceWith).Replace("*", ReplaceWith).
                Replace("<", ReplaceWith).Replace(">", ReplaceWith).Replace("?", ReplaceWith).Replace("|", ReplaceWith).Replace("""", ReplaceWith)
        Catch ex As Exception
            Throw New FaultException("ERROR CODE : WsBRJ100F10 : " + ex.ToString)
        End Try
        Return res
    End Function

    Public Shared Function GetERSPath(ByVal CabinetID As String, ByRef ERSDirPath As String, ByRef SettingPath As String) As ForERSPath
        Try
            Dim ds As DataSet = Nothing
            Dim res As New ForERSPath
            Try
                ds = GetDatasetByQuery("select dbo.udf_ERSDirPath(ERSId) as ERSDirPath,dbo.udf_SettingPath(ERSId) as SettingPath from eZCabinet where CabinetID = " + CabinetID + "")
            Catch ex As Exception
                Dim exc As String
                exc = "ERROR CODE:WDBR210F120DB30 " + ex.ToString()
                Throw New FaultException(exc)
            End Try
            If ds.Tables(0).Rows.Count = 0 Then

                Return Nothing
            Else
                Try
                    res.SettingPath = ds.Tables(0).Rows(0).Item("SettingPath").ToString()
                    res.ERSDirPath = ds.Tables(0).Rows(0).Item("ERSDirPath").ToString()
                    Return res
                Catch ex As Exception

                    Return Nothing
                End Try
            End If
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR210F120 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function


    Public Shared Function XMLCreation(cabid As Integer, cabname As String, tmpid As Integer, tmpname As String, fields As String(), fieldvalues As String(),
                                filename As String, size As Integer, xmlfilename As String, ByVal loginid As String, ByVal ipaddress As String,
                                ezfrom As String, nopages As String) As String
        Dim xml As String = ""
        Try
            Dim cabdet = SelectedeZTemplateList("templateid", tmpid.ToString)
            If cabdet.Count > 0 Then
                cabname = cabdet(0).CabinetName
                cabid = cabdet(0).CabinetID
                tmpname = cabdet(0).TemplateName
            Else
                Throw New FaultException("ERROR CODE : WSJS014A100DB10 : Invalid Template")
            End If
            Dim ifilepath = "", fileext = filename.Substring(filename.LastIndexOf(".") + 1), username = ""
            fileext = fileext.Replace("&", "&amp;").Replace("'", "&apos;")
            xml = "<data>"
            xml += "<version>1</version>"
            xml += "<MYIP>" + ipaddress + "</MYIP>"
            xml += "<tablename>" + GetTableName(tmpid) + "</tablename>"
            xml += "<name>cabinetid</name><value>\\\" + cabid.ToString + "</value>"
            xml += "<name>templateid</name><value>" + tmpid.ToString + "</value>"
            xml += "<name>ifiletype</name><value>" + fileext + "</value>"
            xml += "<name>CreatedOn</name><value>" + DateDateTimeToString(DateTime.Now, True) + "</value>"
            xml += "<name>createdby</name><value>" + loginid + "</value>"
            xml += "<name>updatedby</name><value>" + loginid + "</value>"
            xml += "<name>dstatus</name><value>Active</value>"
            xml += "<name>dsize</name><value>" + size.ToString + "</value>"
            xml += "<name>nopages</name><value>" + nopages + "</value>"
            xml += "<name>ezfrom</name><value>" + ezfrom + "</value>"
            If cabid = 1 Then
                If tmpid <> 3 Then
                    Dim dir As String = "\\" + cabname.Replace("&", "&amp;").Replace("'", "&apos;") & "\" & tmpname.Replace("&", "&amp;").Replace("'", "&apos;") &
                        "\" & RemoveSpecialChar(fieldvalues(0).ToString().Replace("&", "&amp;").Replace("'", "&apos;"), "-") + "\" + ezfrom +
                        "\" & DateDateTimeToString(DateTime.Now, False).Replace(":", "") + "\" & fileext.ToUpper() + "\"
                    xml += "<name>User Name</name><value>" + fieldvalues(0).ToString() + "</value>" +
                        "<name>Date</name><value>" + DateDateTimeToString(DateTime.Now, False) + "</value>" +
                        "<name>File Source</name><value>" + ezfrom + "</value>" +
                        "<name>File Type</name><value>" + fileext.ToUpper() + "</value>" +
                        "<name>ifilename</name><value>" + filename + "</value>" +
                        "<filename>" + filename + "</filename>" +
                        "<name>ifilepath</name><value>" + dir + "</value>"
                Else
                    If fields.Count > 1 Then
                        Dim dir = "\\" + cabname.Replace("&", "&amp;").Replace("'", "&apos;") + "\" & tmpname.Replace("&", "&amp;").Replace("'", "&apos;") + "\"
                        For i As Integer = 0 To fields.Length - 1
                            xml += "<name>" + fields(i) + "</name><value>" + fieldvalues(i).Replace("&", "&amp;").Replace("'", "&apos;") + "</value>"
                        Next
                        Dim Sql = "Select FieldLevel,FieldName From eZTemplateField Where Mandatory=1 and FieldLevel > 0 and Isdeleted=0 and TemplateId=" +
                            tmpid.ToString + " order by FieldLevel"
                        Dim ds = GetDatasetByQuery(Sql)
                        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            Dim row = ds.Tables(0).Rows(i)
                            For j As Integer = 0 To fields.Length - 1
                                If (row("FieldName") = fields(j)) Then
                                    dir += RemoveSpecialChar(fieldvalues(j), "-") + "\"
                                    Exit For
                                End If
                            Next
                        Next
                        xml += "<name>ifilename</name><value>" + filename + "</value>" +
                            "<filename>" + filename + "</filename>" +
                            "<name>ifilepath</name><value>" + dir + "</value>"
                    Else
                        Dim dir As String = "\\" + cabname.Replace("&", "&amp;").Replace("'", "&apos;") & "\" & tmpname.Replace("&", "&amp;").Replace("'", "&apos;") &
                        "\" & RemoveSpecialChar(fieldvalues(0).ToString().Replace("&", "&amp;").Replace("'", "&apos;"), "-") + "\" + ezfrom +
                        "\" & DateDateTimeToString(DateTime.Now, False).Replace(":", "") + "\" & fileext.ToUpper() + "\"
                        xml += "<name>User Name</name><value>" + fieldvalues(0).ToString() + "</value>" +
                        "<name>Date</name><value>" + DateDateTimeToString(DateTime.Now, False) + "</value>" +
                        "<name>File Source</name><value>" + ezfrom + "</value>" +
                        "<name>File Type</name><value>" + fileext.ToUpper() + "</value>" +
                        "<name>ifilename</name><value>" + filename + "</value>" +
                        "<filename>" + filename + "</filename>" +
                        "<name>ifilepath</name><value>" + dir + "</value>"
                    End If
                End If
            Else
                For i As Integer = 0 To fields.Length - 1
                    If fieldvalues(i) <> Nothing Then
                        xml += "<name>" + fields(i) + "</name><value>" + fieldvalues(i).Replace("&", "&amp;").Replace("'", "&apos;") + "</value>"
                    Else
                        xml += "<name>" + fields(i) + "</name><value></value>"
                    End If

                Next
                Dim Sql = "Select FieldLevel,FieldName,DataTypeId From eZTemplateField Where Mandatory=1 and FieldLevel > 0 and Isdeleted=0 and TemplateId=" +
                    tmpid.ToString + " order by FieldLevel"
                Dim ds = GetDatasetByQuery(Sql)
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    Dim row = ds.Tables(0).Rows(i)
                    If i < ds.Tables(0).Rows.Count - 1 Then
                        For j As Integer = 0 To fields.Length - 1
                            If (row("FieldName") = fields(j)) Then
                                'ifilepath += RemoveSpecialChar(fieldvalues(j), "-") + "\"
                                'srini
                                If (row("DataTypeId").ToString = "5") Then
                                    If fieldvalues(j).Contains(" ") Then
                                        ifilepath += RemoveSpecialChar(fieldvalues(j).Substring(0, fieldvalues(j).IndexOf(" ")), "-") + "\"
                                    Else
                                        ifilepath += RemoveSpecialChar(fieldvalues(j), "-") + "\"
                                    End If
                                Else
                                    ifilepath += RemoveSpecialChar(fieldvalues(j), "-") + "\"
                                End If
                                Exit For
                            End If
                        Next
                    Else
                        For j As Integer = 0 To fields.Length - 1
                            If (row("FieldName").ToString().ToLower() = fields(j).ToLower()) Then
                                Dim file = RemoveSpecialChar(fieldvalues(j).Replace("&", "&amp;").Replace("'", "&apos;"), "-") + filename.Substring(filename.LastIndexOf("."))
                                xml += "<name>ifilename</name><value>" + file + "</value>"
                                xml += "<filename>" + file + "</filename>"
                            End If
                        Next
                    End If
                Next
                xml += "<name>ifilepath</name><value>\\" + cabname.Replace("&", "&amp;").Replace("'", "&apos;") + "\" + tmpname.Replace("&", "&amp;").Replace("'", "&apos;") +
                    "\" + ifilepath.Replace("&", "&amp;").Replace("'", "&apos;") + "</value>"
            End If
            xml += "</data>"
        Catch ex As Exception
            Throw New FaultException("ERROR CODE : WSJS014A100" + ex.ToString)
        End Try
        Return xml
    End Function

#End Region


#Region "Workflows"



#End Region

#Region "Encrypt and Decrypt"
    Public Shared Function CreateKey(ByVal strPassword As String) As Byte()
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
            ' writetxtfle("CreateKey : " + ex.ToString())
            ' Throw New FaultException(exc)
        End Try
    End Function

    Public Shared Function CreateIV(ByVal strPassword As String) As Byte()
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
    Public Enum CryptoAction

        ActionEncrypt = 1
        ActionDecrypt = 2
    End Enum

    Public Shared Function EncryptOrDecryptFile(ByVal strInputFile As String,
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
    Public Shared Function EncryptOrDecryptFileView(ByVal strInputFile As String, ByVal strOutputFile As String, ByVal bytKey() As Byte, ByVal bytIV() As Byte,
                                 ByVal Direction As CryptoAction) As MemoryStream

        Dim fsInput As System.IO.FileStream
        Try
            fsInput = New System.IO.FileStream(strInputFile, FileMode.Open, FileAccess.Read)
            Dim csCryptoStream As CryptoStream
            Dim fsoutput As New MemoryStream
            'Declare variables for encrypt/decrypt process.
            Dim bytBuffer(4096) As Byte 'holds a block of bytes for processing
            Dim lngBytesProcessed As Long = 0 'running count of bytes processed
            Dim lngFileLength As Long = fsInput.Length 'the input file's length
            Dim intBytesInCurrentBlock As Integer 'current bytes being processed
            'Declare your CryptoServiceProvider.
            Dim cspRijndael As New System.Security.Cryptography.RijndaelManaged
            'Setup Progress Bar
            'Determine if ecryption or decryption and setup CryptoStream.
            Select Case Direction
                Case CryptoAction.ActionEncrypt
                    csCryptoStream = New CryptoStream(fsoutput, cspRijndael.CreateEncryptor(bytKey, bytIV), CryptoStreamMode.Write)
                Case CryptoAction.ActionDecrypt
                    csCryptoStream = New CryptoStream(fsoutput, cspRijndael.CreateDecryptor(bytKey, bytIV), CryptoStreamMode.Write)
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
            csCryptoStream.Flush()
            fsInput.Close()
            Return fsoutput
        Catch When Err.Number = 53 'if file not found

        Catch
            fsInput.Close()
        End Try
    End Function
#End Region

    'Public Shared Function UploadFn(para As InsUpload, EcmLoginId As Integer, CallHistoryId As Integer) As resmessage ' 
    '    Dim resmsg As New resmessage()
    '    Try
    '        Dim strqry = "", filename = "", xmlfilename = "", filepath = "", xmlfilepath = "", strRimNumber = ""
    '        Dim templateId = 0, cabinetId = 0
    '        Dim hasfield = False
    '        Dim fieldlist As New List(Of FieldWithValues)
    '        Dim streZXmlCreateCondition As String = "", xmlcreatecondition = ""
    '        Dim dseZXmlCreateCondition As DataSet
    '        Dim strqryHistory = "", UCallHistoryId = ""
    '        Dim strCreatedOn = DateDateTimeToString(Date.Now, True)
    '        If para.CabinetName <> "" Then
    '            If para.CabinetName.ToLower = "corporate" Then
    '                strqry = "Select  isnull([dbo].udf_Templateidbytempname('" + para.CabinetName + "'),'0') as TemplateId "
    '                Dim dsTemplate = GetDatasetByQuery(strqry)
    '                If Not IsNothing(dsTemplate) AndAlso dsTemplate.Tables.Count > 0 AndAlso dsTemplate.Tables(0).Rows.Count > 0 Then
    '                    templateId = dsTemplate.Tables(0).Rows(0)("TemplateId").ToString()
    '                    If templateId > 0 Then

    '                        Dim templateList = SelectedeZTemplateList("TemplateId", templateId)
    '                        If Not IsNothing(templateList) AndAlso templateList.Count > 0 Then

    '                            strqryHistory = "update eZAPICallHistory set CabinetId=" + templateList(0).CabinetID.ToString + " ,TemplateId=" + templateList(0).TemplateId.ToString + " ,UpdatedOnAPI='" + strCreatedOn + "',Createdby='" + EcmLoginId.ToString + "',UpdatedBy='" + EcmLoginId.ToString + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)

    '                            If Not IsNothing(para.Fields) AndAlso para.Fields.Count > 0 Then
    '                                If para.CabinetName.ToLower = "corporate" Then
    '                                    streZXmlCreateCondition = "select CabinetId,[dbo].[udf_Cabinet](CabinetId) CabinetName,TemplateId,[dbo].[udf_Template](TemplateId) TemplateName,ConditionFields from eZXmlCreateCondition where Isdeleted=0"
    '                                    dseZXmlCreateCondition = GetDatasetByQuery(streZXmlCreateCondition)
    '                                End If

    '                                Dim fieldsList = SelectedeZTemplateFieldList("TemplateId", templateId.ToString)
    '                                Dim fields() As String = New String(fieldsList.Count - 1) {}
    '                                Dim fieldvalues() As String = New String(fieldsList.Count - 1) {}
    '                                For i As Integer = 0 To fieldsList.Count - 1
    '                                    hasfield = False
    '                                    For Each inputFieldList In para.Fields
    '                                        If fieldsList(i).FieldName.ToLower = inputFieldList.FieldName.ToLower Then
    '                                            If fieldsList(i).FieldName.ToLower = "rim number" Then
    '                                                strRimNumber = inputFieldList.FieldValue
    '                                            End If
    '                                            fields(i) = inputFieldList.FieldName
    '                                            If fieldsList(i).Mandatory Then
    '                                                If inputFieldList.FieldValue = "" Or inputFieldList.FieldValue = Nothing Then
    '                                                    If fieldsList(i).FieldName.ToLower = "file location" Then
    '                                                        fieldvalues(i) = "Digital"
    '                                                    Else
    '                                                        'Throw New FaultException("Mandatory should not empty")
    '                                                        resmsg.errorCode = 6
    '                                                        resmsg.value = "Error code: 3_3 - Mandatory Fieldname(" + fieldsList(i).FieldName + ") should not be Empty"
    '                                                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                                        Return resmsg
    '                                                    End If
    '                                                Else
    '                                                    fieldvalues(i) = inputFieldList.FieldValue

    '                                                End If
    '                                            Else
    '                                                If inputFieldList.FieldValue = "" Or inputFieldList.FieldValue = Nothing Then
    '                                                    If fieldsList(i).FieldName.ToLower = "file location" Then
    '                                                        fieldvalues(i) = "Digital"
    '                                                    Else
    '                                                        fieldvalues(i) = ""
    '                                                    End If
    '                                                Else
    '                                                    fieldvalues(i) = inputFieldList.FieldValue
    '                                                End If
    '                                            End If

    '                                            If Not IsNothing(dseZXmlCreateCondition) AndAlso dseZXmlCreateCondition.Tables.Count > 0 AndAlso dseZXmlCreateCondition.Tables(0).Rows.Count > 0 Then
    '                                                Dim xmlcreateconditionArr = dseZXmlCreateCondition.Tables(0).Rows(0)("ConditionFields").ToString().Split({","}, StringSplitOptions.RemoveEmptyEntries)
    '                                                For n As Integer = 0 To xmlcreateconditionArr.Length - 1
    '                                                    If fieldsList(i).FieldName.ToLower = xmlcreateconditionArr(n).ToLower Then
    '                                                        xmlcreatecondition = xmlcreatecondition + " and [" + fieldsList(i).FieldName + "]='" + inputFieldList.FieldValue + "'"
    '                                                        Exit For
    '                                                    End If
    '                                                Next
    '                                            End If

    '                                            hasfield = True
    '                                            Exit For
    '                                        End If
    '                                    Next
    '                                    If hasfield = False Then
    '                                        resmsg.errorCode = 5
    '                                        resmsg.value = "Error code: 3_2 - Input Fieldname(" + fieldsList(i).FieldName + ") not found"
    '                                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                        Return resmsg
    '                                    End If
    '                                Next
    '                                Dim ersvalue = GetERSPath(templateList(0).CabinetID, "", "")
    '                                Dim Monitorpath = Path.Combine(ersvalue.SettingPath, "Monitor")
    '                                If Not System.IO.Directory.Exists(Monitorpath) Then
    '                                    System.IO.Directory.CreateDirectory(Monitorpath)
    '                                End If
    '                                Dim timestamp = Date.Now.ToString("yyyyMMddhhmmssffftt")
    '                                If para.filetype <> "" Then
    '                                    filename = timestamp + "." + para.filetype
    '                                    xmlfilename = timestamp + ".xml"
    '                                    filepath = Path.Combine(Monitorpath, filename)
    '                                    xmlfilepath = Path.Combine(Monitorpath, xmlfilename)
    '                                    If para.file.Length > 0 Then
    '                                        System.IO.File.WriteAllBytes(filepath, para.file)
    '                                        If File.Exists(filepath) Then
    '                                            Dim xmlstring = XMLCreation(templateList(0).CabinetID, templateList(0).CabinetName, templateList(0).TemplateId, templateList(0).TemplateName, fields, fieldvalues, filename, para.file.Length, xmlfilename, EcmLoginId, "", "EZOFIS(API)", "0")
    '                                            xmlstring = xmlstring.Replace("</data>", "<apicallid>" + CallHistoryId.ToString + "</apicallid><noversion>noversion</noversion></data>")
    '                                            IO.File.WriteAllBytes(xmlfilepath, System.Text.Encoding.Unicode.GetBytes(xmlstring))

    '                                            If xmlcreatecondition <> "" Then
    '                                                Dim tablename = "ezca_" + dseZXmlCreateCondition.Tables(0).Rows(0)("CabinetId").ToString() + "_" + dseZXmlCreateCondition.Tables(0).Rows(0)("TemplateId").ToString() + "_items"
    '                                                strqry = "select * from " + tablename + " where isdeleted=0 " + xmlcreatecondition + ""
    '                                                Dim dsRetail = GetDatasetByQuery(strqry)
    '                                                If Not IsNothing(dsRetail) AndAlso dsRetail.Tables.Count > 0 AndAlso dsRetail.Tables(0).Rows.Count > 0 Then
    '                                                    fieldsList = SelectedeZTemplateFieldList("TemplateId", dseZXmlCreateCondition.Tables(0).Rows(0)("TemplateId").ToString())
    '                                                    fields = New String(fieldsList.Count - 1) {}
    '                                                    fieldvalues = New String(fieldsList.Count - 1) {}
    '                                                    For i As Integer = 0 To fieldsList.Count - 1
    '                                                        For Each inputFieldList In para.Fields
    '                                                            If fieldsList(i).FieldName.ToLower = inputFieldList.FieldName.ToLower Then
    '                                                                fields(i) = inputFieldList.FieldName
    '                                                                If fieldsList(i).Mandatory Then
    '                                                                    If inputFieldList.FieldValue = "" Or inputFieldList.FieldValue = Nothing Then
    '                                                                        If fieldsList(i).FieldName.ToLower = "file location" Then
    '                                                                            fieldvalues(i) = "Digital"
    '                                                                        Else
    '                                                                            'Throw New FaultException("Mandatory should not empty")
    '                                                                            resmsg.errorCode = 6
    '                                                                            resmsg.value = "Error code: 3_3 - Mandatory Fieldname(" + fieldsList(i).FieldName + ") should not be Empty"
    '                                                                            strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                                                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                                                            Return resmsg
    '                                                                        End If
    '                                                                    Else
    '                                                                        fieldvalues(i) = inputFieldList.FieldValue
    '                                                                    End If
    '                                                                Else
    '                                                                    If inputFieldList.FieldValue = "" Or inputFieldList.FieldValue = Nothing Then
    '                                                                        fieldvalues(i) = ""
    '                                                                    Else
    '                                                                        fieldvalues(i) = inputFieldList.FieldValue
    '                                                                    End If
    '                                                                End If
    '                                                            End If
    '                                                        Next
    '                                                    Next

    '                                                    timestamp = Date.Now.ToString("yyyyMMddhhmmssffftt")
    '                                                    filename = timestamp + "." + para.filetype
    '                                                    Dim xmlfilename1 = timestamp + ".xml"
    '                                                    filepath = Path.Combine(Monitorpath, filename)
    '                                                    xmlfilepath = Path.Combine(Monitorpath, xmlfilename1)
    '                                                    System.IO.File.WriteAllBytes(filepath, para.file)
    '                                                    xmlstring = XMLCreation(dseZXmlCreateCondition.Tables(0).Rows(0)("CabinetId").ToString(), dseZXmlCreateCondition.Tables(0).Rows(0)("CabinetName").ToString(), dseZXmlCreateCondition.Tables(0).Rows(0)("TemplateId").ToString(), dseZXmlCreateCondition.Tables(0).Rows(0)("TemplateName").ToString(), fields, fieldvalues, filename, para.file.Length, xmlfilename1, EcmLoginId, "", "EZOFIS(API)", "0")
    '                                                    xmlstring = xmlstring.Replace("</data>", "<apicallid>" + CallHistoryId.ToString + "</apicallid><noversion>noversion</noversion></data>")
    '                                                    IO.File.WriteAllBytes(xmlfilepath, System.Text.Encoding.Unicode.GetBytes(xmlstring))
    '                                                    resmsg.errorCode = 1
    '                                                    resmsg.value = "Success Code: 6_2 - The File will be archived Corporate and Retail."
    '                                                    strqryHistory = "insert into eZAPICallHistory (Template,CabinetId ,TemplateId ,Status,Remarks,[RIM Number],ItemId,ParentCallId ,APIFunction,XmlFileName,CreatedOn,UpdatedOnAPI,CreatedBy,UpdatedBy,Isdeleted ) values ('" + para.CabinetName + "','" + dseZXmlCreateCondition.Tables(0).Rows(0)("CabinetId").ToString() + "','" + dseZXmlCreateCondition.Tables(0).Rows(0)("TemplateId").ToString() + "','Processing','" + resmsg.value + "','" + strRimNumber + "',0," + CallHistoryId.ToString + ",'Upload','" + xmlfilename1 + "','" + strCreatedOn + "','','" + EcmLoginId.ToString + "','" + EcmLoginId.ToString + "',0)"
    '                                                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                                Else
    '                                                    resmsg.errorCode = 1
    '                                                    resmsg.value = "Success Code: 6_1 - The File will be archived in " + para.CabinetName + "."
    '                                                End If
    '                                            Else
    '                                                resmsg.errorCode = 1
    '                                                resmsg.value = "Success Code: 6_1 - The File will be archived in " + para.CabinetName + "."
    '                                            End If

    '                                            'strqryHistory = "select top 1 RefNumber from eZAPICallHistory  where CallHistoryId!=" + CallHistoryId.ToString + " order by CallHistoryId desc"
    '                                            'Dim dsHistory = GetDatasetByQuery(strqryHistory)
    '                                            'Dim strRefnumber = ""
    '                                            'If Not IsNothing(dsHistory) AndAlso dsHistory.Tables.Count > 0 AndAlso dsHistory.Tables(0).Rows.Count > 0 AndAlso dsHistory.Tables(0).Rows(0)("RefNumber").ToString() <> "" Then
    '                                            '    Dim RefNum = Convert.ToInt32(dsHistory.Tables(0).Rows(0)("RefNumber").ToString().Replace("Ref_", "")) + 1
    '                                            '    strRefnumber = "Ref_" + RefNum.ToString() + ""
    '                                            '    strqryHistory = "update eZAPICallHistory set Status='Processing',Remarks='" + resmsg.value + "',RefNumber='" + strRefnumber + "',XmlFileName='" + xmlfilename + "', UpdatedOn='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                            '    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                            'Else
    '                                            '    strRefnumber = "Ref_1"
    '                                            '    strqryHistory = "update eZAPICallHistory set Status='Processing',Remarks='" + resmsg.value + "',RefNumber='" + strRefnumber + "',XmlFileName='" + xmlfilename + "', UpdatedOn='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                            '    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                            'End If

    '                                            Dim strRefnumber = APICallId_Prefix + CallHistoryId.ToString() + ""
    '                                            strqryHistory = "update eZAPICallHistory set Status='Processing',Remarks='" + resmsg.value + "',XmlFileName='" + xmlfilename + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)

    '                                            resmsg.value = resmsg.value + " Your Reffernce Number is " + strRefnumber + ""
    '                                        Else
    '                                            resmsg.errorCode = 9
    '                                            resmsg.value = "Error code: 5_2 - Invalid Base64 value"
    '                                            strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                            Return resmsg
    '                                        End If
    '                                    Else
    '                                        resmsg.errorCode = 8
    '                                        resmsg.value = "Error code: 5_1 - Base64 value should not be Empty"
    '                                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                        Return resmsg
    '                                    End If
    '                                Else
    '                                    resmsg.errorCode = 7
    '                                    resmsg.value = "Error code: 4_1 - Filetype should not be Empty"
    '                                    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                    Return resmsg
    '                                End If

    '                            Else
    '                                resmsg.errorCode = 4
    '                                resmsg.value = "Error code: 3_1 - Fields should not be Empty"
    '                                strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                            End If
    '                        End If
    '                    Else
    '                        resmsg.errorCode = 3
    '                        resmsg.value = "Error code: 2_3 - Invalid Cabinet Name"
    '                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                    End If
    '                Else
    '                    resmsg.errorCode = 3
    '                    resmsg.value = "Error code: 2_2 - Invalid Cabinet Name"
    '                    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                End If
    '            Else
    '                resmsg.errorCode = 3
    '                resmsg.value = "Error code: 2_4 - Cabinet Name must be in 'Corporate'"
    '                strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '            End If
    '        Else
    '            resmsg.errorCode = 2
    '            resmsg.value = "Error code: 2_1 - Cabinet Name should not be Empty"
    '            strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + strCreatedOn + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '        End If

    '    Catch ex As Exception
    '        Dim strqryHistory = "update eZAPICallHistory set Remarks='Exception: " + ex.ToString + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '        Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '        Throw New FaultException("Exception UploadFn(): " + ex.ToString())
    '    End Try
    '    Return resmsg
    'End Function

    Public Shared Function UploadFn(para As InsUpload, EcmLoginId As Integer, CallHistoryId As Integer) As resmessage
        Dim resmsg As New resmessage()
        Try
            Dim strqry = "", filename = "", xmlfilename = "", filepath = "", xmlfilepath = "", strRimNumber = ""
            Dim templateId = 0, cabinetId = 0, strRefnumber = ""
            Dim hasfield = False
            Dim fieldlist As New List(Of FieldWithValues)
            ' Dim streZXmlCreateCondition As String = "", xmlcreatecondition = ""
            ' Dim dseZXmlCreateCondition As DataSet
            Dim strqryHistory = "", UCallHistoryId = ""
            Dim logf As String = ""
            Dim strCreatedOn = DateDateTimeToString(Date.Now, True)
            If para.CabinetName <> "" Then
                strqry = "Select  isnull([dbo].udf_Templateidbytempname('" + para.CabinetName + "'),'0') as TemplateId "
                Dim dsTemplate = GetDatasetByQuery(strqry)
                If Not IsNothing(dsTemplate) AndAlso dsTemplate.Tables.Count > 0 AndAlso dsTemplate.Tables(0).Rows.Count > 0 Then
                    Dim buffer As Byte()
                    buffer = Convert.FromBase64String(para.file)
                    templateId = dsTemplate.Tables(0).Rows(0)("TemplateId").ToString()
                    If templateId > 0 Then
                        Dim templateList = SelectedeZTemplateList("TemplateId", templateId)
                        If Not IsNothing(templateList) AndAlso templateList.Count > 0 Then

                            strqryHistory = "update eZAPICallHistory set CabinetId=" + templateList(0).CabinetID.ToString + " ,TemplateId=" + templateList(0).TemplateId.ToString + " ,Createdby='" + EcmLoginId.ToString + "',UpdatedBy='" + EcmLoginId.ToString + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                            logf = " updated ezapicallhistor for createdby"
                            If Not IsNothing(para.Fields) AndAlso para.Fields.Count > 0 Then
                                'If para.CabinetName.ToLower = "corporate" Then
                                '    streZXmlCreateCondition = "select CabinetId,[dbo].[udf_Cabinet](CabinetId) CabinetName,TemplateId,[dbo].[udf_Template](TemplateId) TemplateName,ConditionFields from eZXmlCreateCondition where Isdeleted=0"
                                '    dseZXmlCreateCondition = GetDatasetByQuery(streZXmlCreateCondition)
                                'End If

                                Dim fieldsList = SelectedeZTemplateFieldList("TemplateId", templateId.ToString)
                                Dim fields() As String = New String(fieldsList.Count - 1) {}
                                Dim fieldvalues() As String = New String(fieldsList.Count - 1) {}
                                For i As Integer = 0 To fieldsList.Count - 1
                                    hasfield = False
                                    'If getDefaultCabinetName(para.CabinetName).ToLower() = "bbk cad" Then
                                    '    If fieldsList(i).FieldName.ToLower() = "remarks" Then
                                    '        fields(i) = fieldsList(i).FieldName
                                    '        fieldvalues(i) = "No Remarks"
                                    '        Continue For
                                    '    End If
                                    'End If
                                    For Each inputFieldList In para.Fields
                                        If fieldsList(i).FieldName.ToLower = inputFieldList.FieldName.ToLower Then
                                            If fieldsList(i).FieldName.ToLower = "rim number" Then
                                                strRimNumber = inputFieldList.FieldValue
                                            End If
                                            fields(i) = inputFieldList.FieldName
                                            If getDefaultCabinetName(para.CabinetName).ToLower() <> "bbk cad" Then
                                                If fieldsList(i).Mandatory Then
                                                    If inputFieldList.FieldValue = "" Or inputFieldList.FieldValue = Nothing Then
                                                        If fieldsList(i).FieldName.ToLower = "file location" Then
                                                            fieldvalues(i) = "Digital"
                                                        Else
                                                            'Throw New FaultException("Mandatory should not empty")
                                                            resmsg.errorCode = 6
                                                            resmsg.value = "Error code: 3_3 - Mandatory Fieldname(" + fieldsList(i).FieldName + ") should not be Empty"
                                                            strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                                            Return resmsg
                                                        End If
                                                    Else
                                                        fieldvalues(i) = inputFieldList.FieldValue
                                                    End If
                                                Else
                                                    If inputFieldList.FieldValue = "" Or inputFieldList.FieldValue = Nothing Then
                                                        If fieldsList(i).FieldName.ToLower = "file location" Then
                                                            fieldvalues(i) = "Digital"
                                                        Else
                                                            fieldvalues(i) = ""
                                                        End If
                                                    Else
                                                        fieldvalues(i) = inputFieldList.FieldValue
                                                    End If
                                                End If
                                            Else
                                                fieldvalues(i) = inputFieldList.FieldValue
                                            End If


                                            'If Not IsNothing(dseZXmlCreateCondition) AndAlso dseZXmlCreateCondition.Tables.Count > 0 AndAlso dseZXmlCreateCondition.Tables(0).Rows.Count > 0 Then
                                            '    Dim xmlcreateconditionArr = dseZXmlCreateCondition.Tables(0).Rows(0)("ConditionFields").ToString().Split({","}, StringSplitOptions.RemoveEmptyEntries)
                                            '    For n As Integer = 0 To xmlcreateconditionArr.Length - 1
                                            '        If fieldsList(i).FieldName.ToLower = xmlcreateconditionArr(n).ToLower Then
                                            '            xmlcreatecondition = xmlcreatecondition + " and [" + fieldsList(i).FieldName + "]='" + inputFieldList.FieldValue + "'"
                                            '            Exit For
                                            '        End If
                                            '    Next
                                            'End If

                                            hasfield = True
                                            Exit For
                                        End If
                                    Next
                                    If hasfield = False Then
                                        resmsg.errorCode = 5
                                        resmsg.value = "Error code: 3_2 - Input Fieldname(" + fieldsList(i).FieldName + ") not found"
                                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                        Return resmsg
                                    End If
                                Next
                                Dim ersvalue = GetERSPath(templateList(0).CabinetID, "", "")
                                Dim Monitorpath = Path.Combine(ersvalue.SettingPath, "Monitor")
                                If Not System.IO.Directory.Exists(Monitorpath) Then
                                    System.IO.Directory.CreateDirectory(Monitorpath)
                                End If
                                logf += " taken monitor path"
                                Dim timestamp = Date.Now.ToString("yyyyMMddhhmmssffftt")
                                If para.filetype <> "" Then
                                    filename = timestamp + "." + para.filetype
                                    xmlfilename = timestamp + ".xml"
                                    filepath = Path.Combine(Monitorpath, filename)
                                    xmlfilepath = Path.Combine(Monitorpath, xmlfilename)
                                    logf += "xmlfilepath: " + xmlfilepath
                                    If buffer.Length > 0 Then
                                        System.IO.File.WriteAllBytes(filepath, buffer)
                                        If File.Exists(filepath) Then
                                            Dim xmlstring = XMLCreation(templateList(0).CabinetID, templateList(0).CabinetName, templateList(0).TemplateId, templateList(0).TemplateName, fields, fieldvalues, filename, buffer.Length, xmlfilename, EcmLoginId, "", "EZOFIS(API)", "0")
                                            xmlstring = xmlstring.Replace("</data>", "<apicallid>" + CallHistoryId.ToString + "</apicallid><noversion>noversion</noversion></data>")
                                            IO.File.WriteAllBytes(xmlfilepath, System.Text.Encoding.Unicode.GetBytes(xmlstring))
                                            If xmlstring <> "" Then
                                                logf += " xmlstring created"
                                            End If

                                            If para.CabinetName.ToLower = "corporate" Then
                                                If para.Individual_RIM_Number <> "" AndAlso para.Individual_TIN_Number <> "" Then
                                                    Dim strQryRetail = "select CabinetId,TemplateId,[dbo].[udf_Cabinet](CabinetId) CabinetName,[dbo].[udf_Template](TemplateId) TemplateName from eztemplate where cabinetId=" + templateList(0).CabinetID.ToString + " and templateName='RETAIL' and isdeleted=0"
                                                    Dim dsRetailTemp = GetDatasetByQuery(strQryRetail)
                                                    If Not IsNothing(dsRetailTemp) AndAlso dsRetailTemp.Tables.Count > 0 AndAlso dsRetailTemp.Tables(0).Rows.Count > 0 Then
                                                        Dim tablename = "ezca_" + dsRetailTemp.Tables(0).Rows(0)("CabinetId").ToString() + "_" + dsRetailTemp.Tables(0).Rows(0)("TemplateId").ToString() + "_items"
                                                        'strqry = "select * from " + tablename + " where isdeleted=0 "
                                                        'Dim dsRetails = GetDatasetByQuery(strqry)
                                                        'If Not IsNothing(dsRetail) AndAlso dsRetail.Tables.Count > 0 AndAlso dsRetail.Tables(0).Rows.Count > 0 Then
                                                        fieldsList = SelectedeZTemplateFieldList("TemplateId", dsRetailTemp.Tables(0).Rows(0)("TemplateId").ToString())
                                                        fields = New String(fieldsList.Count - 1) {}
                                                        fieldvalues = New String(fieldsList.Count - 1) {}
                                                        For i As Integer = 0 To fieldsList.Count - 1
                                                            For Each inputFieldList In para.Fields
                                                                If fieldsList(i).FieldName.ToLower = inputFieldList.FieldName.ToLower Then
                                                                    fields(i) = inputFieldList.FieldName
                                                                    If inputFieldList.FieldName.ToLower = "rim number" Then
                                                                        fieldvalues(i) = para.Individual_RIM_Number
                                                                    ElseIf inputFieldList.FieldName.ToLower = "tin number" Then
                                                                        fieldvalues(i) = para.Individual_TIN_Number
                                                                    Else
                                                                        If fieldsList(i).Mandatory Then
                                                                            If inputFieldList.FieldValue = "" Or inputFieldList.FieldValue = Nothing Then
                                                                                If fieldsList(i).FieldName.ToLower = "file location" Then
                                                                                    fieldvalues(i) = "Digital"
                                                                                Else
                                                                                    'Throw New FaultException("Mandatory should not empty")
                                                                                    resmsg.errorCode = 6
                                                                                    resmsg.value = "Error code: 3_3 - Mandatory Fieldname(" + fieldsList(i).FieldName + ") should not be Empty"
                                                                                    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                                                                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                                                                    Return resmsg
                                                                                End If
                                                                            Else
                                                                                fieldvalues(i) = inputFieldList.FieldValue
                                                                            End If
                                                                        Else
                                                                            If inputFieldList.FieldValue = "" Or inputFieldList.FieldValue = Nothing Then
                                                                                fieldvalues(i) = ""
                                                                            Else
                                                                                fieldvalues(i) = inputFieldList.FieldValue
                                                                            End If
                                                                        End If
                                                                    End If
                                                                End If
                                                            Next
                                                        Next

                                                        timestamp = Date.Now.ToString("yyyyMMddhhmmssffftt")
                                                        filename = timestamp + "." + para.filetype
                                                        Dim xmlfilename1 = timestamp + ".xml"
                                                        filepath = Path.Combine(Monitorpath, filename)
                                                        xmlfilepath = Path.Combine(Monitorpath, xmlfilename1)
                                                        System.IO.File.WriteAllBytes(filepath, buffer)
                                                        xmlstring = XMLCreation(dsRetailTemp.Tables(0).Rows(0)("CabinetId").ToString(), dsRetailTemp.Tables(0).Rows(0)("CabinetName").ToString(), dsRetailTemp.Tables(0).Rows(0)("TemplateId").ToString(), dsRetailTemp.Tables(0).Rows(0)("TemplateName").ToString(), fields, fieldvalues, filename, buffer.Length, xmlfilename1, EcmLoginId, "", "EZOFIS(API)", "0")
                                                        xmlstring = xmlstring.Replace("</data>", "<apicallid>" + CallHistoryId.ToString + "</apicallid><noversion>noversion</noversion></data>")
                                                        IO.File.WriteAllBytes(xmlfilepath, System.Text.Encoding.Unicode.GetBytes(xmlstring))
                                                        resmsg.errorCode = 1
                                                        resmsg.value = "Success Code: 6_2 - The File will be archived Corporate and Retail."
                                                        strqryHistory = "insert into eZAPICallHistory (Template,CabinetId ,TemplateId ,Status,Remarks,[RIM Number],ItemId,ParentCallId ,APIFunction,XmlFileName,CreatedOn,UpdatedOnAPI,CreatedBy,UpdatedBy,Isdeleted ) values ('" + para.CabinetName + "','" + dsRetailTemp.Tables(0).Rows(0)("CabinetId").ToString() + "','" + dsRetailTemp.Tables(0).Rows(0)("TemplateId").ToString() + "','Processing','" + resmsg.value + "','" + strRimNumber + "',0," + CallHistoryId.ToString + ",'Upload','" + xmlfilename1 + "','" + DateDateTimeToString(Date.Now, True) + "','','" + EcmLoginId.ToString + "','" + EcmLoginId.ToString + "',0)"
                                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                                        'Else
                                                        '    resmsg.errorCode = 1
                                                        '    resmsg.value = "Success Code: 6_1 - The File will be archived in " + para.CabinetName + "."
                                                        'End If
                                                    Else
                                                        resmsg.errorCode = 1
                                                        resmsg.value = "Success Code: 6_1 - The File will be archived in " + para.CabinetName + "."
                                                    End If
                                                Else
                                                    resmsg.errorCode = 1
                                                    resmsg.value = "Success Code: 6_1 - The File will be archived in " + para.CabinetName + "."
                                                End If
                                            End If
                                            strRefnumber = APICallId_Prefix + CallHistoryId.ToString() + ""
                                            logf += "  strRefnumber: taken"
                                            strqryHistory = "update eZAPICallHistory set Status='Processing',Remarks='" + resmsg.value + "',XmlFileName='" + xmlfilename + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                            If UCallHistoryId = 0 Then
                                                strqryHistory = "select * from ezAPICallHistory where Remarks='" + resmsg.value + "' and CallHistoryId='" + CallHistoryId.ToString + "'"
                                                Dim dsqryhis As DataSet = GetDatasetByQuery(strqryHistory)
                                                If Not IsNothing(dsqryhis) AndAlso dsqryhis.Tables.Count > 0 AndAlso dsqryhis.Tables(0).Rows.Count > 0 Then
                                                    resmsg.errorCode = 1
                                                    UCallHistoryId = CallHistoryId.ToString
                                                End If
                                            End If
                                            resmsg.value = "Success Code: 6_1 -  Your Reffernce Number is " + strRefnumber + ""
                                            'If getDefaultCabinetName(para.CabinetName).ToLower() = "bbk cad" Then
                                            '    Dim res = New With {.ItemId = UCallHistoryId, .URL = APIurl + "/V1/CMAP/viewfile/" & para.CabinetName & "/" & UCallHistoryId}
                                            '    Dim jsonStr = JsonConvert.SerializeObject(res)
                                            '    resmsg.value = jsonStr
                                            'Else
                                            'End If
                                        Else
                                            resmsg.errorCode = 9
                                            resmsg.value = "Error code: 5_2 - Invalid Base64 value"
                                            strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                            Return resmsg
                                        End If
                                    Else
                                        resmsg.errorCode = 8
                                        resmsg.value = "Error code: 5_1 - Base64 value should not be Empty"
                                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                        Return resmsg
                                    End If
                                Else
                                    resmsg.errorCode = 7
                                    resmsg.value = "Error code: 4_1 - Filetype should not be Empty"
                                    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                    Return resmsg
                                End If
                            Else
                                resmsg.errorCode = 4
                                resmsg.value = "Error code: 3_1 - Fields should not be Empty"
                                strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                            End If
                        Else
                            resmsg.errorCode = 3
                            resmsg.value = "Error code: 2_3 - Empty TemplateList "
                            strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                        End If
                    Else
                        resmsg.errorCode = 3
                        resmsg.value = "Error code: 2_3 - Invalid Template Id " + templateId.ToString()
                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                    End If
                Else
                    resmsg.errorCode = 3
                    resmsg.value = "Error code: 2_2 - Invalid Cabinet Name"
                    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                End If
                'Else
                '    resmsg.errorCode = 3
                '    resmsg.value = "Error code: 2_4 - Cabinet Name must be in ''Corporate''"
                '    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                '    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                'End If
            Else
                resmsg.errorCode = 2
                resmsg.value = "Error code: 2_1 - Cabinet Name should not be Empty"
                strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
            End If
            ' resmsg.value = logf + resmsg.value

        Catch ex As Exception
            Dim strqryHistory = "update eZAPICallHistory set Remarks='Exception: " + ex.ToString + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
            Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
            resmsg.errorCode = 2
            resmsg.value = "Excepiton " & ex.ToString()
            ' Throw New FaultException("Exception UploadFn(): " + ex.ToString())
        End Try
        Return resmsg
    End Function

    Public Shared Function CMAPUploadFn1(para As InsUpload, EcmLoginId As Integer, CallHistoryId As Integer) As resmessage
        Dim resmsg As New resmessage()
        Dim logf As String = "Entered cmapUploadfn"
        Try
            Dim strqry = "", filename = "", xmlfilename = "", filepath = "", xmlfilepath = "", strRimNumber = ""
            Dim templateId = 0, cabinetId = 0, strRefnumber = ""

            Dim hasfield = False
            Dim fieldlist As New List(Of FieldWithValues)
            Dim strqryHistory = "", UCallHistoryId = "", ItemId = ""
            Dim strCreatedOn = DateDateTimeToString(Date.Now, True)
            If para.CabinetName <> "" Then
                strqry = "Select  isnull([dbo].udf_Templateidbytempname('" + para.CabinetName + "'),'0') as TemplateId "
                Dim dsTemplate = GetDatasetByQuery(strqry)
                If Not IsNothing(dsTemplate) AndAlso dsTemplate.Tables.Count > 0 AndAlso dsTemplate.Tables(0).Rows.Count > 0 Then
                    Dim buffer As Byte()
                    buffer = Convert.FromBase64String(para.file)

                    If buffer Is Nothing Then
                        resmsg.errorCode = 3
                        resmsg.value = "Invalid File"
                        Return resmsg
                    End If

                    templateId = dsTemplate.Tables(0).Rows(0)("TemplateId").ToString()
                    If templateId > 0 Then
                        Dim templateList = SelectedeZTemplateList("TemplateId", templateId)
                        If Not IsNothing(templateList) AndAlso templateList.Count > 0 Then

                            strqryHistory = "update eZAPICallHistory set CabinetId=" + templateList(0).CabinetID.ToString + " ,TemplateId=" + templateList(0).TemplateId.ToString + " ,Createdby='" + EcmLoginId.ToString + "',UpdatedBy='" + EcmLoginId.ToString + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                            logf &= " ezapicallhistory updated cabinet "
                            If Not IsNothing(para.Fields) AndAlso para.Fields.Count > 0 Then
                                'If para.CabinetName.ToLower = "corporate" Then
                                '    streZXmlCreateCondition = "select CabinetId,[dbo].[udf_Cabinet](CabinetId) CabinetName,TemplateId,[dbo].[udf_Template](TemplateId) TemplateName,ConditionFields from eZXmlCreateCondition where Isdeleted=0"
                                '    dseZXmlCreateCondition = GetDatasetByQuery(streZXmlCreateCondition)
                                'End If
                                Dim strqrycols As String = "", strqryvals As String = ""

                                Dim fieldsList = SelectedeZTemplateFieldList("TemplateId", templateId.ToString)
                                Dim fields() As String = New String(fieldsList.Count - 1) {}
                                Dim fieldvalues() As String = New String(fieldsList.Count - 1) {}
                                For i As Integer = 0 To fieldsList.Count - 1
                                    hasfield = False
                                    If fieldsList(i).FieldName.ToLower() = "remarks" Then
                                        strqrycols += "Remarks,"
                                        strqryvals += "'No Remarks'" + ","
                                        Continue For
                                    End If
                                    For Each inputFieldList In para.Fields
                                        If fieldsList(i).FieldName.ToLower = inputFieldList.FieldName.ToLower Then
                                            strqrycols += "[" + inputFieldList.FieldName + "]" + ","
                                            strqryvals += "'" & inputFieldList.FieldValue.Replace("'", "''") & "'" + ","
                                            If fieldsList(i).FieldName.ToLower = "rim number" Then
                                                strRimNumber = inputFieldList.FieldValue
                                            End If
                                            fields(i) = inputFieldList.FieldName
                                            fieldvalues(i) = inputFieldList.FieldValue



                                            'If Not IsNothing(dseZXmlCreateCondition) AndAlso dseZXmlCreateCondition.Tables.Count > 0 AndAlso dseZXmlCreateCondition.Tables(0).Rows.Count > 0 Then
                                            '    Dim xmlcreateconditionArr = dseZXmlCreateCondition.Tables(0).Rows(0)("ConditionFields").ToString().Split({","}, StringSplitOptions.RemoveEmptyEntries)
                                            '    For n As Integer = 0 To xmlcreateconditionArr.Length - 1
                                            '        If fieldsList(i).FieldName.ToLower = xmlcreateconditionArr(n).ToLower Then
                                            '            xmlcreatecondition = xmlcreatecondition + " and [" + fieldsList(i).FieldName + "]='" + inputFieldList.FieldValue + "'"
                                            '            Exit For
                                            '        End If
                                            '    Next
                                            'End If

                                            hasfield = True
                                            Exit For
                                        End If
                                    Next
                                    If hasfield = False Then
                                        resmsg.errorCode = 5
                                        resmsg.value = "Error code: 3_2 - Input Fieldname(" + fieldsList(i).FieldName + ") not found"
                                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                        Return resmsg
                                    End If
                                Next
                                Dim ersvalue = GetERSPath(templateList(0).CabinetID, "", "")
                                Dim Monitorpath = Path.Combine(ersvalue.SettingPath, "Monitor")
                                If Not System.IO.Directory.Exists(Monitorpath) Then
                                    System.IO.Directory.CreateDirectory(Monitorpath)
                                End If
                                strqry = "select ERSID from ezcabinet where cabinetId='" + templateList(0).CabinetID.ToString() + "'"
                                Dim ds_ErsID As DataSet = GetDatasetByQuery(strqry)

                                Dim sourcePath As String = ""
                                If (ds_ErsID IsNot Nothing And ds_ErsID.Tables.Count > 0 And ds_ErsID.Tables(0).Rows.Count > 0) Then
                                    Dim ersId As String = ds_ErsID.Tables(0).Rows(0)(0).ToString()
                                    Dim timestamp = Date.Now.ToString("yyyyMMddhhmmssffftt")

                                    If para.filetype <> "" Then
                                        filename = timestamp + "." + para.filetype
                                        sourcePath = Path.Combine(Monitorpath, filename)

                                        If buffer.Length > 0 Then
                                            System.IO.File.WriteAllBytes(sourcePath, buffer)
                                        End If

                                        Dim strlastfieldname = ""
                                        Dim qry = " select top 1 [FieldName] from eZTemplateField where TemplateId = " + templateId.ToString() + " and FieldLevel != 0 order by FieldLevel desc"

                                        Dim ds1 = GetDatasetByQuery(qry)
                                        If ds1 IsNot Nothing And ds1.Tables.Count > 0 And ds1.Tables(0).Rows.Count > 0 Then
                                            strlastfieldname = ds1.Tables(0).Rows(0)(0).ToString()
                                        End If

                                        Dim Sql = "Select FieldLevel,FieldName,DataTypeId From eZTemplateField Where Mandatory=1 and FieldLevel > 0 and Isdeleted=0 and TemplateId=" +
                    templateId.ToString + " order by FieldLevel"
                                        Dim ds = GetDatasetByQuery(Sql)
                                        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                                            Dim row = ds.Tables(0).Rows(i)
                                            If i < ds.Tables(0).Rows.Count Then
                                                For j As Integer = 0 To fields.Length - 1
                                                    If (row("FieldName").ToString().ToLower() = fields(j).ToLower()) Then
                                                        'ifilepath += RemoveSpecialChar(fieldvalues(j), "-") + "\"
                                                        'srini
                                                        If row("fieldName").ToString().ToLower() = strlastfieldname.ToLower() Then
                                                            filename = fieldvalues(j) + "." + para.filetype
                                                            Exit For
                                                        Else
                                                            If (row("DataTypeId").ToString = "5") Then
                                                                If fieldvalues(j).Contains(" ") Then
                                                                    filepath += RemoveSpecialChar(fieldvalues(j).Substring(0, fieldvalues(j).IndexOf(" ")), "-") + "\"
                                                                    Exit For
                                                                Else
                                                                    filepath += RemoveSpecialChar(fieldvalues(j), "-") + "\"
                                                                    Exit For
                                                                End If
                                                            Else
                                                                filepath += RemoveSpecialChar(fieldvalues(j), "-") + "\"
                                                                Exit For
                                                            End If

                                                        End If

                                                    End If

                                                Next

                                            End If
                                        Next

                                        Dim version As String = "1.0"

                                        If System.IO.File.Exists(ersvalue.ERSDirPath + filepath + filename.Replace("." + para.filetype, ".ezo")) Then
                                            Dim values() = Function_VersionCreation(templateList(0).CabinetID.ToString(), templateId.ToString(), filepath, filename)

                                            Dim oldversion As String = ""
                                            Dim noversion As String = ""

                                            version = values(0)
                                            oldversion = values(1)

                                            filename = filename.Replace(Path.GetExtension(filename), "") & "_" & version & Path.GetExtension(filename)
                                            strqryvals = strqryvals.Replace(strqryvals.LastIndexOf(","), "")
                                        End If

                                        Dim archievePath As String = ersvalue.ERSDirPath + filepath + filename

                                        If Not Directory.Exists(System.IO.Path.GetDirectoryName(archievePath)) Then
                                            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(archievePath))
                                        End If
                                        File.WriteAllBytes(archievePath, buffer)

                                        logf &= " file copied to localfile " & archievePath
                                        If File.Exists(archievePath) Then
                                            Dim strInputFile = archievePath
                                            Dim strOutputFile = archievePath.Replace(para.filetype, "ezo")
                                            Dim bytKey As Byte()
                                            Dim bytIV As Byte()
                                            bytKey = CreateKey("3z0f1s$ecm")
                                            bytIV = CreateIV("3z0f1s$ecm")
                                            Dim resulte As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionEncrypt)
                                        End If
                                        logf &= " Entered decrypting the file  " & archievePath



                                        strqry = " insert into ezca_" + templateList(0).CabinetID.ToString() + "_" + templateId.ToString() + "_items"
                                        strqry += " (ERSId,TemplateId," + strqrycols + "ifilepath,ifilename,ifiletype,ezFrom,Version,dstatus,dsize,createdon,createdBy) values(" & ersId & "," & templateId.ToString() & "," & strqryvals & "" & "'" + filepath.Replace("'", "''") + "'" & " , '" & filename & "','" & para.filetype & "','EZOFIS(API)','" & version & "','Active','0','" & DateDateTimeToString(Date.Now, True) & "','" + EcmLoginId.ToString + "')"
                                        logf &= "qry " & strqry

                                        ItemId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqry)


                                        Dim res = New With {.ItemId = ItemId, .URL = APIurl + "/V1/CMAP/viewfile/" & para.CabinetName & "/" & ItemId}
                                        Dim jsonStr = JsonConvert.SerializeObject(res)


                                        resmsg.errorCode = 1
                                        resmsg.value = jsonStr

                                        strqryHistory = "insert into eZAPICallHistory (Template,CabinetId ,TemplateId ,Status,Remarks,[RIM Number],ItemId,ParentCallId ,APIFunction,XmlFileName,CreatedOn,UpdatedOnAPI,CreatedBy,UpdatedBy,Isdeleted ) values ('" + para.CabinetName + "','" + templateList(0).CabinetID.ToString() + "','" + templateId.ToString() + "','Processing','" + resmsg.value + "','" + strRimNumber + "'," + ItemId + "," + CallHistoryId.ToString + ",'Archived','" + archievePath + "','" + DateDateTimeToString(Date.Now, True) + "','','" + EcmLoginId.ToString + "','" + EcmLoginId.ToString + "',0)"
                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)

                                    Else
                                        resmsg.errorCode = 4
                                        resmsg.value = "Error code: 3_1 - filetype should not be Empty"
                                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                    End If
                                End If
                            Else
                                resmsg.errorCode = 3
                                resmsg.value = "Error code: 2_3 - Empty TemplateList "
                                strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                            End If
                        Else
                            resmsg.errorCode = 3
                            resmsg.value = "Error code: 2_3 - Invalid Template Id " + templateId.ToString()
                            strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                        End If
                    Else
                        resmsg.errorCode = 3
                        resmsg.value = "Error code: 2_2 - Invalid Cabinet Name"
                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                    End If
                    'Else
                    '    resmsg.errorCode = 3
                    '    resmsg.value = "Error code: 2_4 - Cabinet Name must be in ''Corporate''"
                    '    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                    '    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                    'End If
                Else
                    resmsg.errorCode = 2
                    resmsg.value = "Error code: 2_1 - Cabinet Name should not be Empty"
                    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                End If
            End If

        Catch ex As Exception
            Dim strqryHistory = "update eZAPICallHistory set Remarks='Exception: " + ex.ToString + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
            Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
            resmsg.errorCode = 2
            resmsg.value = logf & "Excepiton " & ex.ToString()
            ' Throw New FaultException("Exception UploadFn(): " + ex.ToString())
        End Try
        Return resmsg
    End Function

    Public Shared Function CMAPUploadFn(para As InsUpload, EcmLoginId As Integer, CallHistoryId As Integer) As resmessage
        Dim resmsg As New resmessage()
        Dim logf As String = "Entered cmapUploadfn"
        Try
            Dim strqry = "", filename = "", xmlfilename = "", filepath = "", xmlfilepath = "", strRimNumber = ""
            Dim templateId = 0, cabinetId = 0, strRefnumber = ""
            Dim hasfield = False
            Dim fieldlist As New List(Of FieldWithValues)
            Dim strqryHistory = "", UCallHistoryId = "", ItemId = "", encodeType = ""

            Dim strCreatedOn = DateDateTimeToString(Date.Now, True)
            If para.CabinetName <> "" Then
                strqry = "Select  isnull([dbo].udf_Templateidbytempname('" + para.CabinetName + "'),'0') as TemplateId "
                Dim dsTemplate = GetDatasetByQuery(strqry)
                If Not IsNothing(dsTemplate) AndAlso dsTemplate.Tables.Count > 0 AndAlso dsTemplate.Tables(0).Rows.Count > 0 Then
                    Dim buffer As Byte()
                    buffer = Convert.FromBase64String(para.file)
                    Dim plaintxt As String = Encoding.UTF8.GetString(buffer).Trim(""""c)
                    Try
                        buffer = Convert.FromBase64String(plaintxt)
                        encodeType = 2
                    Catch ex As Exception
                        buffer = Convert.FromBase64String(para.file)
                        encodeType = 1
                    End Try
                    templateId = dsTemplate.Tables(0).Rows(0)("TemplateId").ToString()
                    If templateId > 0 Then
                        Dim templateList = SelectedeZTemplateList("TemplateId", templateId)
                        If Not IsNothing(templateList) AndAlso templateList.Count > 0 Then
                            strqryHistory = "update eZCMAPAPICallHistory set CabinetId=" + templateList(0).CabinetID.ToString + " ,TemplateId=" + templateList(0).TemplateId.ToString + " ,Createdby='" + EcmLoginId.ToString + "',UpdatedBy='" + EcmLoginId.ToString + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                            logf &= " ezCMAPapicallhistory updated cabinet "
                            If Not IsNothing(para.Fields) AndAlso para.Fields.Count > 0 Then

                                Dim strqrycols As String = "", strqryvals As String = ""
                                Dim docFileNameField As New eZTemplateField
                                Dim fieldsList = SelectedeZTemplateFieldList("TemplateId", templateId.ToString)
                                Dim fields() As String = New String(fieldsList.Count - 1) {}
                                Dim fieldvalues() As String = New String(fieldsList.Count - 1) {}
                                For i As Integer = 0 To fieldsList.Count - 1
                                    hasfield = False
                                    If fieldsList(i).FieldName.ToLower() = "remarks" Then
                                        strqrycols += "Remarks,"
                                        strqryvals += "'No Remarks'" + ","
                                        Continue For
                                    End If
                                    For Each inputFieldList In para.Fields
                                        If fieldsList(i).FieldName.ToLower = inputFieldList.FieldName.ToLower Then
                                            strqrycols += "[" + inputFieldList.FieldName + "]" + ","

                                            'If fieldsList(i).DataType = "5" Then
                                            '    Dim format As String = "dd-MMM-yyyy" ' Format of your date string
                                            '    Dim documentExpiryDate As DateTime = DateTime.ParseExact(inputFieldList.FieldValue, format, System.Globalization.CultureInfo.InvariantCulture)
                                            '    strqryvals += "'" & documentExpiryDate & "'" + ","
                                            'Else
                                            '    strqryvals += "'" & inputFieldList.FieldValue.Replace("'", "''") & "'" + ","
                                            'End If

                                            strqryvals += "'" & inputFieldList.FieldValue.Replace("'", "''") & "'" + ","

                                            If fieldsList(i).FieldName.ToLower = "rim number" Then
                                                strRimNumber = inputFieldList.FieldValue
                                            End If
                                            If docFileNameField.FieldName = "" Then
                                                If fieldsList(i).FieldName.Replace(" ", "").ToLower() = "documentfilename" Then
                                                    docFileNameField = fieldsList(i)
                                                End If
                                            End If

                                            fields(i) = inputFieldList.FieldName
                                            fieldvalues(i) = inputFieldList.FieldValue
                                            hasfield = True
                                            Exit For
                                        End If
                                        If fieldsList(i).Mandatory = False Then
                                            hasfield = True
                                        End If
                                    Next
                                    If hasfield = False Then
                                        resmsg.errorCode = 5
                                        resmsg.value = "Error code: 3_2 - Input Fieldname(" + fieldsList(i).FieldName + ") not found"
                                        strqryHistory = "update eZCMAPAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                                        Return resmsg
                                    End If
                                Next
                                Dim ersvalue = GetERSPath(templateList(0).CabinetID, "", "")
                                'Dim Monitorpath = Path.Combine(ersvalue.SettingPath, "Monitor")
                                'If Not System.IO.Directory.Exists(Monitorpath) Then
                                '    System.IO.Directory.CreateDirectory(Monitorpath)
                                'End If
                                strqry = "select ERSID from ezcabinet where cabinetId='" + templateList(0).CabinetID.ToString() + "'"
                                Dim ds_ErsID As DataSet = GetDatasetByQuery(strqry)

                                If (ds_ErsID IsNot Nothing And ds_ErsID.Tables.Count > 0 And ds_ErsID.Tables(0).Rows.Count > 0) Then
                                    Dim ersId As String = ds_ErsID.Tables(0).Rows(0)(0).ToString()
                                    Dim timestamp = Date.Now.ToString("yyyyMMddhhmmssffftt")

                                    If para.filetype <> "" Then

                                        Dim strlastfieldname = "Document Filename"
                                        'Dim qry = " select top 1 [FieldName] from eZTemplateField where TemplateId = " + templateId.ToString() + " and FieldLevel != 0 order by FieldLevel desc"

                                        'Dim ds1 = GetDatasetByQuery(qry)
                                        'If ds1 IsNot Nothing And ds1.Tables.Count > 0 And ds1.Tables(0).Rows.Count > 0 Then
                                        '    strlastfieldname = ds1.Tables(0).Rows(0)(0).ToString()
                                        'End If

                                        Dim fileExtensions As String() = {
                                                                ".txt", ".doc", ".docx", ".pdf", ".odt", ".rtf", ".tex", ".wpd", ".md",
                                                                ".xls", ".xlsx", ".ods", ".csv", ".tsv",
                                                                ".ppt", ".pptx", ".odp",
                                                                ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif", ".svg", ".psd", ".ai",
                                                                ".mp3", ".wav", ".flac", ".aac", ".ogg", ".m4a", ".wma",
                                                                ".mp4", ".avi", ".mkv", ".mov", ".flv", ".wmv", ".webm", ".mpeg",
                                                                ".zip", ".rar", ".7z", ".tar", ".gz", ".bz2", ".xz", ".iso",
                                                                ".exe", ".bat", ".msi", ".sh", ".bin", ".cmd",
                                                                ".html", ".css", ".js", ".php", ".py", ".rb", ".java", ".c", ".cpp", ".cs", ".vb", ".xml", ".json", ".yml", ".sql",
                                                                ".db", ".sqlite", ".accdb", ".mdb", ".sql", ".dbf", ".myd", ".frm",
                                                                ".dll", ".sys", ".ini", ".log", ".cfg", ".dat"
                                                                         }

                                        Dim index As Integer = fieldsList.IndexOf(docFileNameField)
                                        Dim extension As String = System.IO.Path.GetExtension(fieldvalues(index))

                                        If fileExtensions.Contains(extension.ToLower()) Then
                                            filename = fieldvalues(index).Substring(0, fieldvalues(index).LastIndexOf("."c)) + "." + para.filetype.ToLower()
                                        Else
                                            If extension <> "" Then
                                                filename = fieldvalues(index)
                                            Else
                                                filename = fieldvalues(index) + "." + para.filetype.ToLower()
                                            End If

                                        End If


                                        filepath = fieldvalues(0) + "\" + fieldvalues(1) + "\" + fieldvalues(2) + "\"

                                        'filename = fieldvalues(3) + "." + para.filetype

                                        '                    Dim Sql = "Select FieldLevel,FieldName,DataTypeId From eZTemplateField Where Mandatory=1 and FieldLevel > 0 and Isdeleted=0 and TemplateId=" +
                                        'templateId.ToString + " order by FieldLevel"
                                        '                    Dim ds = GetDatasetByQuery(Sql)
                                        '                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                                        '                        Dim row = ds.Tables(0).Rows(i)
                                        '                        If i < ds.Tables(0).Rows.Count Then
                                        '                            For j As Integer = 0 To fields.Length - 1
                                        '                                If (row("FieldName").ToString().ToLower() = fields(j).ToLower()) Then

                                        '                                    If row("fieldName").ToString().ToLower() = strlastfieldname.ToLower() Then
                                        '                                        filename = fieldvalues(j) + "." + para.filetype
                                        '                                        Exit For
                                        '                                    Else
                                        '                                        If (row("DataTypeId").ToString = "5") Then
                                        '                                            If fieldvalues(j).Contains(" ") Then
                                        '                                                filepath += RemoveSpecialChar(fieldvalues(j).Substring(0, fieldvalues(j).IndexOf(" ")), "-") + "\"
                                        '                                                Exit For
                                        '                                            Else
                                        '                                                filepath += RemoveSpecialChar(fieldvalues(j), "-") + "\"
                                        '                                                Exit For
                                        '                                            End If
                                        '                                        Else
                                        '                                            filepath += RemoveSpecialChar(fieldvalues(j), "-") + "\"
                                        '                                            Exit For
                                        '                                        End If

                                        '                                    End If

                                        '                                End If

                                        '                            Next

                                        '                        End If
                                        '                    Next

                                        Dim version As String = "1.0"

                                        If System.IO.File.Exists(System.IO.Path.Combine(ersvalue.ERSDirPath, templateList(0).CabinetName, templateList(0).TemplateName, filepath, filename.Replace("." + para.filetype, ".ezo"))) Then
                                            Dim values() = Function_VersionCreation(templateList(0).CabinetID.ToString(), templateId.ToString(), templateList(0).CabinetName + "\" + templateList(0).TemplateName + "\" + filepath, filename)

                                            Dim oldversion As String = ""
                                            Dim noversion As String = ""

                                            version = values(0)
                                            oldversion = values(1)

                                            filename = filename.Replace(Path.GetExtension(filename), "") & "_" & version & Path.GetExtension(filename)
                                            strqryvals = strqryvals.Replace(strqryvals.LastIndexOf(","), "")
                                        End If
                                        logf &= " Entered for the archievepath"
                                        Dim archievePath As String = System.IO.Path.Combine(ersvalue.ERSDirPath, templateList(0).CabinetName, templateList(0).TemplateName, filepath, filename)
                                        logf &= " Got Archievepath " & archievePath
                                        If Not Directory.Exists(System.IO.Path.GetDirectoryName(archievePath)) Then
                                            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(archievePath))
                                        End If
                                        logf &= "Create directory for the archievepath"
                                        File.WriteAllBytes(archievePath, buffer)
                                        logf &= " file copied to localfile " & archievePath
                                        If File.Exists(archievePath) Then
                                            Dim strInputFile = archievePath
                                            Dim strOutputFile = archievePath.Replace("." + para.filetype, ".ezo")
                                            Dim bytKey As Byte()
                                            Dim bytIV As Byte()
                                            bytKey = CreateKey("3z0f1s$ecm")
                                            bytIV = CreateIV("3z0f1s$ecm")
                                            Dim resulte As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionEncrypt)
                                        End If
                                        logf &= " Finished decrypting the file  " & archievePath


                                        encodeType = "1"
                                        strqry = " insert into ezca_" + templateList(0).CabinetID.ToString() + "_" + templateId.ToString() + "_items"
                                        strqry += " (ERSId,TemplateId," + strqrycols + "ifilepath,ifilename,ifiletype,ezFrom,Version,dstatus,dsize,createdon,createdBy,encodeType) values(" & ersId & "," & templateId.ToString() & "," & strqryvals & "" & "'" + templateList(0).CabinetName + "\" + templateList(0).TemplateName + "\" + filepath.Replace("'", "''") + "'" & " , '" & filename.Replace("'", "''") & "','" & para.filetype.ToLower() & "','ACP CA Documentation','" & version & "','Active','0','" & DateDateTimeToString(Date.Now, True) & "','" + EcmLoginId.ToString + "'," + encodeType + ")"
                                        logf &= "qry " & strqry


                                        ItemId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqry)
                                        Dim res = New With {.ItemId = ItemId, .URL = APIurl + "/V1/CMAP/viewfile/" & para.CabinetName & "/" & ItemId}

                                        Dim jsonStr = JsonConvert.SerializeObject(res)


                                        resmsg.errorCode = 1
                                        resmsg.value = jsonStr

                                        strqryHistory = "update eZCMAPAPICallHistory set ItemId = '" + ItemId + "',Status = 'Archived', Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "',encodeType=" + encodeType + " where CallHistoryId='" + CallHistoryId.ToString + "'"

                                        'strqryHistory = "insert into eZCMAPAPICallHistory (Template,CabinetId ,TemplateId ,Status,Remarks,[RIM Number],ItemId,ParentCallId ,APIFunction,XmlFileName,CreatedOn,UpdatedOnAPI,CreatedBy,UpdatedBy,Isdeleted ) values ('" + para.CabinetName + "','" + templateList(0).CabinetID.ToString() + "','" + templateId.ToString() + "','Processing','" + resmsg.value + "','" + strRimNumber + "'," + ItemId + "," + CallHistoryId.ToString + ",'Archived','" + archievePath + "','" + DateDateTimeToString(Date.Now, True) + "','','" + EcmLoginId.ToString + "','" + EcmLoginId.ToString + "',0)"
                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefined(strqryHistory)

                                    Else
                                        resmsg.errorCode = 4
                                        resmsg.value = "Error code: 3_1 - filetype should not be Empty"
                                        strqryHistory = "update eZCMAPAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "',encodeType=" + encodeType + " , UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefined(strqryHistory)
                                    End If
                                End If
                            Else
                                resmsg.errorCode = 3
                                resmsg.value = "Error code: 2_3 - Empty TemplateList "
                                strqryHistory = "update eZCMAPAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "',encodeType=" + encodeType + " , UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                                UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                            End If
                        Else
                            resmsg.errorCode = 3
                            resmsg.value = "Error code: 2_3 - Invalid Template Id " + templateId.ToString()
                            strqryHistory = "update eZCMAPAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "',encodeType=" + encodeType + " , UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                        End If
                    Else
                        resmsg.errorCode = 3
                        resmsg.value = "Error code: 2_2 - Invalid Cabinet Name"
                        strqryHistory = "update eZCMAPAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "',encodeType=" + encodeType + " , UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                    End If
                    'Else
                    '    resmsg.errorCode = 3
                    '    resmsg.value = "Error code: 2_4 - Cabinet Name must be in ''Corporate''"
                    '    strqryHistory = "update eZCMAPAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                    '    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                    'End If
                Else
                    resmsg.errorCode = 2
                    resmsg.value = "Error code: 2_1 - Cabinet Name should not be Empty"
                    strqryHistory = "update eZCMAPAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "',encodeType=" + encodeType + " , UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
                End If
            End If

        Catch ex As Exception
            Dim strqryHistory = "update eZCMAPAPICallHistory set Remarks='Exception: " + ex.ToString + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
            Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
            resmsg.errorCode = 2
            resmsg.value = logf & "Excepiton " & ex.ToString()
            ' Throw New FaultException("Exception UploadFn(): " + ex.ToString())
        End Try
        Return resmsg
    End Function


    Public Shared Function HSBCUploadFn(para As InsUpload, EcmLoginId As Integer) As resmessage
        Dim resmsg As New resmessage()
        Dim logf As String = "Entered HSBCUploadfn"
        Try
            Dim strqry = "", filename = "", xmlfilename = "", filepath = "", xmlfilepath = "", strRimNumber = "", strRimType = "", strDocumentType = "", strTinNumber = ""
            Dim templateId = 0, cabinetId = 0, strRefnumber = ""
            Dim hasfield = False
            Dim fieldlist As New List(Of FieldWithValues)
            Dim strqryHistory = "", UCallHistoryId = "", ItemId = "", encodeType = "", logError = ""

            Dim strCreatedOn = DateDateTimeToString(Date.Now, True)
            If para.CabinetName <> "" Then
                strqry = "Select  isnull([dbo].udf_Templateidbytempname('" + para.CabinetName + "'),'0') as TemplateId "
                Dim dsTemplate = GetDatasetByQuery(strqry)
                If Not IsNothing(dsTemplate) AndAlso dsTemplate.Tables.Count > 0 AndAlso dsTemplate.Tables(0).Rows.Count > 0 Then
                    Dim buffer As Byte()
                    buffer = Convert.FromBase64String(para.file)
                    templateId = dsTemplate.Tables(0).Rows(0)("TemplateId").ToString()
                    If templateId > 0 Then
                        Dim templateList = SelectedeZTemplateList("TemplateId", templateId)
                        If Not IsNothing(templateList) AndAlso templateList.Count > 0 Then
                            If Not IsNothing(para.Fields) AndAlso para.Fields.Count > 0 Then

                                Dim strqrycols As String = "", strqryvals As String = ""
                                Dim docFileNameField As New eZTemplateField
                                Dim fieldsList = SelectedeZTemplateFieldList("TemplateId", templateId.ToString)
                                Dim fields() As String = New String(fieldsList.Count - 1) {}
                                Dim fieldvalues() As String = New String(fieldsList.Count - 1) {}
                                For i As Integer = 0 To fieldsList.Count - 1
                                    hasfield = False

                                    For Each inputFieldList In para.Fields
                                        If fieldsList(i).FieldName.ToLower = inputFieldList.FieldName.ToLower Then
                                            strqrycols += "[" + inputFieldList.FieldName + "]" + ","
                                            strqryvals += "'" & inputFieldList.FieldValue.Replace("'", "''") & "'" + ","

                                            If fieldsList(i).FieldName.ToLower = "rim number" Then
                                                strRimNumber = inputFieldList.FieldValue
                                            End If
                                            If fieldsList(i).FieldName.ToLower = "document type" Then
                                                strDocumentType = inputFieldList.FieldValue
                                            End If
                                            If fieldsList(i).FieldName.ToLower = "rim type" Then
                                                strRimType = inputFieldList.FieldValue
                                            End If
                                            If fieldsList(i).FieldName.ToLower = "tin number" Then
                                                strTinNumber = inputFieldList.FieldValue
                                            End If
                                            fields(i) = inputFieldList.FieldName
                                            fieldvalues(i) = inputFieldList.FieldValue
                                            hasfield = True
                                            Exit For
                                        End If
                                        If fieldsList(i).Mandatory = False Then
                                            hasfield = True
                                        End If
                                    Next
                                    If hasfield = False Then
                                        resmsg.errorCode = 5
                                        resmsg.value = "Error code: 3_2 - Input Fieldname(" + fieldsList(i).FieldName + ") not found"
                                        Return resmsg
                                    End If
                                Next
                                Dim ersvalue = GetERSPath(templateList(0).CabinetID, "", "")
                                strqry = "select ERSID from ezcabinet where cabinetId='" + templateList(0).CabinetID.ToString() + "'"
                                Dim ds_ErsID As DataSet = GetDatasetByQuery(strqry)

                                If (ds_ErsID IsNot Nothing And ds_ErsID.Tables.Count > 0 And ds_ErsID.Tables(0).Rows.Count > 0) Then
                                    Dim ersId As String = ds_ErsID.Tables(0).Rows(0)(0).ToString()
                                    Dim timestamp = Date.Now.ToString("yyyyMMddhhmmssffftt")
                                    Dim orgFileName As String = ""
                                    Dim archievePath As String = ""
                                    If para.filetype <> "" Then
                                        filename = strDocumentType + "." + para.filetype
                                        filepath = fieldvalues(0) + "\" + fieldvalues(1) + "\" + fieldvalues(2) + "\"
                                        Dim version As String = "1.0"
                                        orgFileName = filename

                                        If System.IO.File.Exists(System.IO.Path.Combine(ersvalue.ERSDirPath, templateList(0).CabinetName, templateList(0).TemplateName, filepath, filename.Replace("." + para.filetype, ".ezo"))) Then
                                            Dim values() = Function_VersionCreation(templateList(0).CabinetID.ToString(), templateId.ToString(), templateList(0).CabinetName + "\" + templateList(0).TemplateName + "\" + filepath, filename)

                                            Dim oldversion As String = ""
                                            Dim noversion As String = ""

                                            version = values(0)
                                            oldversion = values(1)

                                            filename = filename.Replace(Path.GetExtension(filename), "") & "_" & version & Path.GetExtension(filename)
                                            strqryvals = strqryvals.Replace(strqryvals.LastIndexOf(","), "")
                                        End If
                                        logf &= " Entered for the archievepath"
                                        archievePath = System.IO.Path.Combine(ersvalue.ERSDirPath, templateList(0).CabinetName, templateList(0).TemplateName, filepath, filename)
                                        logf &= " Got Archievepath " & archievePath
                                        If Not Directory.Exists(System.IO.Path.GetDirectoryName(archievePath)) Then
                                            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(archievePath))
                                        End If
                                        Dim maxretryattempts As Integer = 15
                                        Dim retryms As Integer = 5
                                        Dim cnt As Integer = 0
                                        Dim success As Boolean = False
                                        If File.Exists(archievePath) Then
                                            Do
                                                Try
                                                    cnt = cnt + 1
                                                    If cnt >= maxretryattempts Then
                                                        ' logError = " File is being used by another process even after multiple attempts for " & archievePath
                                                        Exit Do
                                                    End If
                                                    Dim values() = Function_VersionCreation(templateList(0).CabinetID.ToString(), templateId.ToString(), templateList(0).CabinetName + "\" + templateList(0).TemplateName + "\" + filepath, filename)
                                                    Dim oldversion As String = ""
                                                    Dim noversion As String = ""
                                                    version = values(0)
                                                    oldversion = values(1)
                                                    filename = orgFileName.Replace(Path.GetExtension(orgFileName), "") & "_" & version & Path.GetExtension(orgFileName)
                                                    '  strqryvals = strqryvals.Replace(strqryvals.LastIndexOf(","), "")
                                                    archievePath = System.IO.Path.Combine(ersvalue.ERSDirPath, templateList(0).CabinetName, templateList(0).TemplateName, filepath, filename)
                                                Catch ex As Exception
                                                    'logError = logError & "ERROR in version creation while retrying for " & archievePath & ": " & ex.Message.ToString()
                                                End Try
                                            Loop While File.Exists(archievePath)
                                        End If
                                        logf &= "Create directory for the archievepath"
                                        Try
                                            Using fs As New FileStream(archievePath, FileMode.Create, FileAccess.Write, FileShare.None)   ' ⬅ exclusive lock
                                                fs.Write(buffer, 0, buffer.Length)
                                                fs.Flush(True)
                                            End Using
                                        Catch ex As Exception
                                            logError = " Failed to write file in " & archievePath & ": " & ex.Message.ToString()
                                        End Try

                                        ' File.WriteAllBytes(archievePath, buffer)
                                        logf &= " file copied to localfile " & archievePath
                                        Try
                                            If File.Exists(archievePath) Then
                                                Dim strInputFile = archievePath
                                                Dim strOutputFile = archievePath.Replace("." + para.filetype, ".ezo")
                                                Dim bytKey As Byte()
                                                Dim bytIV As Byte()
                                                bytKey = CreateKey("3z0f1s$ecm")
                                                bytIV = CreateIV("3z0f1s$ecm")
                                                Dim resulte As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionEncrypt)
                                            End If
                                        Catch ex As Exception
                                            logError = logError & " Failed in encryption for " & archievePath & ": " & ex.Message.ToString()
                                        End Try

                                        logf &= " Entered coping  the file to " & archievePath
                                        Try
                                            Using fs As New FileStream(archievePath, FileMode.CreateNew, FileAccess.Write, FileShare.None)  ' ⬅ exclusive lock
                                                fs.Write(buffer, 0, buffer.Length)
                                                fs.Flush(True)
                                            End Using
                                            ' Exit For
                                        Catch ex As Exception
                                            logError = logError & " Failed to write org file in " & archievePath & ": " & ex.Message.ToString()
                                        End Try


                                        ' File.WriteAllBytes(archievePath, buffer)

                                        logf &= " Entered decrypting the file  " & archievePath
                                        If logError <> "" Then
                                            resmsg.errorCode = 3
                                            resmsg.value = "Error code: 3_6 - " & logError & " "
                                            Return resmsg
                                        End If

                                        strqry = " insert into ezca_" + templateList(0).CabinetID.ToString() + "_" + templateId.ToString() + "_items"
                                        strqry += " (ERSId,TemplateId," + strqrycols + "ifilepath,ifilename,ifiletype,ezFrom,Version,dstatus,dsize,createdon,createdBy,dtitle,dauthor,dsubject,dkeywords,checkoutby,UpdatedBy,Isdeleted) values(" & ersId & "," & templateId.ToString() & "," & strqryvals & "" & "'" + templateList(0).CabinetName + "\" + templateList(0).TemplateName + "\" + filepath.Replace("'", "''") + "'" & " , '" & filename.Replace("'", "''") & "','" & para.filetype.ToLower() & "','HSBC Documentation','" & version & "','Active','0','" & DateDateTimeToString(Date.Now, True) & "','" + EcmLoginId.ToString + "','" + strRimNumber + "','" + strRimType + "','" + strTinNumber + "','" + strDocumentType + "',0,0,'0')"


                                        ItemId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqry)
                                        Dim res = New With {.ItemId = ItemId}

                                        Dim jsonStr = JsonConvert.SerializeObject(res)


                                        resmsg.errorCode = 1
                                        resmsg.value = jsonStr
                                    Else
                                        resmsg.errorCode = 4
                                        resmsg.value = "Error code: 3_1 - filetype should not be Empty"
                                    End If
                                End If
                            Else
                                resmsg.errorCode = 3
                                resmsg.value = "Error code: 2_3 - Empty TemplateList "
                            End If
                        Else
                            resmsg.errorCode = 3
                            resmsg.value = "Error code: 2_3 - Invalid Template Id " + templateId.ToString()
                        End If
                    Else
                        resmsg.errorCode = 3
                        resmsg.value = "Error code: 2_2 - Invalid Cabinet Name"
                    End If
                Else
                    resmsg.errorCode = 7
                    resmsg.value = "Error code: 2_1 - Cabinet Name should not be Empty"
                End If
            End If

        Catch ex As Exception
            resmsg.errorCode = 2
            resmsg.value = logf & "Exception " & ex.ToString()
        End Try
        Return resmsg
    End Function

    Public Shared Function SearchandGetUrlfn(para As InsSearchandGetURL, strItemId As String) As resmessage
        Dim resmsg As New resmessage()
        Dim logf As String = "Emtered search and GetUrlsubfunction"
        Try
            Dim StrQry As String = "select templateId from ezTemplate where templateName ='" & para.CabinetName & "' and isdeleted=0"
            Dim ds_Cabinet = GetDatasetByQuery(StrQry)
            If ds_Cabinet IsNot Nothing AndAlso ds_Cabinet.Tables.Count > 0 AndAlso ds_Cabinet.Tables(0).Rows.Count > 0 Then
                Dim templateId As String = ds_Cabinet.Tables(0).Rows(0)("templateId").ToString()
                logf &= " Got TemplateId"

                Dim templateList = SelectedeZTemplateList("TemplateId", templateId)

                StrQry = "select i.ERSid,e.ERSDirPath DirPath,ifilepath,ifilename,ifiletype,[Document Type] from ezca_3_" & templateId & "_items i left join eZERSInfo e on e.ERSId=i.ERSId where i.itemId=" & strItemId & ""
                Dim ds = GetDatasetByQuery(StrQry)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    ' Dim filePath As String = Path.Combine(ds.Tables(0).Rows(0)("DirPath").ToString, templateList(0).CabinetName, templateList(0).TemplateName, ds.Tables(0).Rows(0)("ifilepath").ToString, Path.GetFileNameWithoutExtension(ds.Tables(0).Rows(0)("ifilename").ToString) + ".ezo")
                    Dim filePath As String = Path.Combine(ds.Tables(0).Rows(0)("DirPath").ToString, ds.Tables(0).Rows(0)("ifilepath").ToString, Path.GetFileNameWithoutExtension(ds.Tables(0).Rows(0)("ifilename").ToString) + ".ezo")
                    ' filePath = "C:\Users\Shiva\Downloads\[PRIVE] CA Template_2.ezo"
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
                                ' Dim bytes As Byte() = File.ReadAllBytes(strOutputFile)
                                'Dim base64string As String = Convert.ToBase64String(bytes)
                                Dim bytes As Byte() = File.ReadAllBytes(strOutputFile)
                                Dim base64string As String = Convert.ToBase64String(bytes)
                                Dim utf8Bytes As Byte() = Encoding.UTF8.GetBytes(base64string)
                                Dim finalBase64String As String = Convert.ToBase64String(utf8Bytes)
                                logf &= " got base64string"
                                Dim jsonObj = New With {
                                .File = finalBase64String
                                }
                                Dim jsonStr = JsonConvert.SerializeObject(jsonObj)
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

                                resmsg.errorCode = 1
                                resmsg.value = jsonStr

                                Return resmsg
                            Else
                                resmsg.errorCode = "3_1"
                                resmsg.value = "Error in encryption"
                                Return resmsg
                            End If
                        Else
                            resmsg.errorCode = "3_1"
                            resmsg.value = "File Not exists in path " & localfile
                            Return resmsg
                        End If
                    Else
                        resmsg.errorCode = "3_1"
                        resmsg.value = "File Not exists in path " & filePath
                        Return resmsg
                    End If
                Else
                    resmsg.errorCode = "3_1"
                    resmsg.value = "No record exists for Itemid " & strItemId
                    Return resmsg
                End If
            Else
                resmsg.errorCode = "3_1"
                resmsg.value = "Template  does not exists for cabinet " & para.CabinetName
                Return resmsg
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Public Shared Function CMAPUploadFnPostMan(para As InsUpload, EcmLoginId As Integer, CallHistoryId As Integer) As resmessage
    '    Dim resmsg As New resmessage()
    '    Dim logf As String = "Entered cmapUploadfn"
    '    Try
    '        Dim strqry = "", filename = "", xmlfilename = "", filepath = "", xmlfilepath = "", strRimNumber = ""
    '        Dim templateId = 0, cabinetId = 0, strRefnumber = ""

    '        Dim hasfield = False
    '        Dim fieldlist As New List(Of FieldWithValues)
    '        Dim strqryHistory = "", UCallHistoryId = "", ItemId = ""
    '        Dim strCreatedOn = DateDateTimeToString(Date.Now, True)
    '        If para.CabinetName <> "" Then
    '            strqry = "Select  isnull([dbo].udf_Templateidbytempname('" + para.CabinetName + "'),'0') as TemplateId "
    '            Dim dsTemplate = GetDatasetByQuery(strqry)
    '            If Not IsNothing(dsTemplate) AndAlso dsTemplate.Tables.Count > 0 AndAlso dsTemplate.Tables(0).Rows.Count > 0 Then
    '                Dim buffer As Byte()
    '                'buffer = Convert.FromBase64String(para.file)
    '                buffer = para.fileBytes

    '                If buffer Is Nothing Or buffer.Length = 0 Then
    '                    resmsg.errorCode = 3
    '                    resmsg.value = "Invalid File"
    '                    Return resmsg
    '                End If

    '                templateId = dsTemplate.Tables(0).Rows(0)("TemplateId").ToString()
    '                If templateId > 0 Then
    '                    Dim templateList = SelectedeZTemplateList("TemplateId", templateId)
    '                    If Not IsNothing(templateList) AndAlso templateList.Count > 0 Then

    '                        strqryHistory = "update eZAPICallHistory set CabinetId=" + templateList(0).CabinetID.ToString + " ,TemplateId=" + templateList(0).TemplateId.ToString + " ,Createdby='" + EcmLoginId.ToString + "',UpdatedBy='" + EcmLoginId.ToString + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                        logf &= " ezapicallhistory updated cabinet "
    '                        If Not IsNothing(para.Fields) AndAlso para.Fields.Count > 0 Then

    '                            Dim strqrycols As String = "", strqryvals As String = ""

    '                            Dim fieldsList = SelectedeZTemplateFieldList("TemplateId", templateId.ToString)
    '                            Dim fields() As String = New String(fieldsList.Count - 1) {}
    '                            Dim fieldvalues() As String = New String(fieldsList.Count - 1) {}
    '                            For i As Integer = 0 To fieldsList.Count - 1
    '                                hasfield = False
    '                                If fieldsList(i).FieldName.ToLower() = "remarks" Then
    '                                    strqrycols += "Remarks,"
    '                                    strqryvals += "'No Remarks'" + ","
    '                                    Continue For
    '                                End If
    '                                For Each inputFieldList In para.Fields
    '                                    If fieldsList(i).FieldName.ToLower = inputFieldList.FieldName.ToLower Then
    '                                        strqrycols += "[" + inputFieldList.FieldName + "]" + ","

    '                                        'If fieldsList(i).DataType = "5" Then
    '                                        '    Dim format As String = "dd-MMM-yyyy" ' Format of your date string
    '                                        '    Dim documentExpiryDate As DateTime = DateTime.ParseExact(inputFieldList.FieldValue, format, System.Globalization.CultureInfo.InvariantCulture)
    '                                        '    strqryvals += "'" & documentExpiryDate & "'" + ","
    '                                        'Else
    '                                        '    strqryvals += "'" & inputFieldList.FieldValue.Replace("'", "''") & "'" + ","
    '                                        'End If

    '                                        strqryvals += "'" & inputFieldList.FieldValue.Replace("'", "''") & "'" + ","

    '                                        If fieldsList(i).FieldName.ToLower = "rim number" Then
    '                                            strRimNumber = inputFieldList.FieldValue
    '                                        End If
    '                                        fields(i) = inputFieldList.FieldName
    '                                        fieldvalues(i) = inputFieldList.FieldValue

    '                                        hasfield = True
    '                                        Exit For
    '                                    End If
    '                                    If fieldsList(i).Mandatory = False Then
    '                                        hasfield = True
    '                                    End If
    '                                Next
    '                                If hasfield = False Then
    '                                    resmsg.errorCode = 5
    '                                    resmsg.value = "Error code: 3_2 - Input Fieldname(" + fieldsList(i).FieldName + ") not found"
    '                                    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                    Return resmsg
    '                                End If
    '                            Next
    '                            Dim ersvalue = GetERSPath(templateList(0).CabinetID, "", "")
    '                            'Dim Monitorpath = Path.Combine(ersvalue.SettingPath, "Monitor")
    '                            'If Not System.IO.Directory.Exists(Monitorpath) Then
    '                            '    System.IO.Directory.CreateDirectory(Monitorpath)
    '                            'End If
    '                            strqry = "select ERSID from ezcabinet where cabinetId='" + templateList(0).CabinetID.ToString() + "'"
    '                            Dim ds_ErsID As DataSet = GetDatasetByQuery(strqry)

    '                            If (ds_ErsID IsNot Nothing And ds_ErsID.Tables.Count > 0 And ds_ErsID.Tables(0).Rows.Count > 0) Then
    '                                Dim ersId As String = ds_ErsID.Tables(0).Rows(0)(0).ToString()
    '                                Dim timestamp = Date.Now.ToString("yyyyMMddhhmmssffftt")

    '                                If para.filetype <> "" Then

    '                                    Dim strlastfieldname = "Document Filename"
    '                                    'Dim qry = " select top 1 [FieldName] from eZTemplateField where TemplateId = " + templateId.ToString() + " and FieldLevel != 0 order by FieldLevel desc"

    '                                    'Dim ds1 = GetDatasetByQuery(qry)
    '                                    'If ds1 IsNot Nothing And ds1.Tables.Count > 0 And ds1.Tables(0).Rows.Count > 0 Then
    '                                    '    strlastfieldname = ds1.Tables(0).Rows(0)(0).ToString()
    '                                    'End If

    '                                    Dim fileExtensions As String() = {
    '                                                            ".txt", ".doc", ".docx", ".pdf", ".odt", ".rtf", ".tex", ".wpd", ".md",
    '                                                            ".xls", ".xlsx", ".ods", ".csv", ".tsv",
    '                                                            ".ppt", ".pptx", ".odp",
    '                                                            ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".svg", ".psd", ".ai",
    '                                                            ".mp3", ".wav", ".flac", ".aac", ".ogg", ".m4a", ".wma",
    '                                                            ".mp4", ".avi", ".mkv", ".mov", ".flv", ".wmv", ".webm", ".mpeg",
    '                                                            ".zip", ".rar", ".7z", ".tar", ".gz", ".bz2", ".xz", ".iso",
    '                                                            ".exe", ".bat", ".msi", ".sh", ".bin", ".cmd",
    '                                                            ".html", ".css", ".js", ".php", ".py", ".rb", ".java", ".c", ".cpp", ".cs", ".vb", ".xml", ".json", ".yml", ".sql",
    '                                                            ".db", ".sqlite", ".accdb", ".mdb", ".sql", ".dbf", ".myd", ".frm",
    '                                                            ".dll", ".sys", ".ini", ".log", ".cfg", ".dat"
    '                                                                     }


    '                                    Dim extension As String = System.IO.Path.GetExtension(fieldvalues(3))

    '                                    If fileExtensions.Contains(extension) Then
    '                                        filename = fieldvalues(3).Substring(0, fieldvalues(3).LastIndexOf("."c)) + "." + para.filetype
    '                                    Else
    '                                        filename = fieldvalues(3) + "." + para.filetype
    '                                    End If


    '                                    filepath = fieldvalues(0) + "/" + fieldvalues(1) + "/" + fieldvalues(2) + "/"

    '                                    'filename = fieldvalues(3) + "." + para.filetype

    '                                    '                    Dim Sql = "Select FieldLevel,FieldName,DataTypeId From eZTemplateField Where Mandatory=1 and FieldLevel > 0 and Isdeleted=0 and TemplateId=" +
    '                                    'templateId.ToString + " order by FieldLevel"
    '                                    '                    Dim ds = GetDatasetByQuery(Sql)
    '                                    '                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
    '                                    '                        Dim row = ds.Tables(0).Rows(i)
    '                                    '                        If i < ds.Tables(0).Rows.Count Then
    '                                    '                            For j As Integer = 0 To fields.Length - 1
    '                                    '                                If (row("FieldName").ToString().ToLower() = fields(j).ToLower()) Then

    '                                    '                                    If row("fieldName").ToString().ToLower() = strlastfieldname.ToLower() Then
    '                                    '                                        filename = fieldvalues(j) + "." + para.filetype
    '                                    '                                        Exit For
    '                                    '                                    Else
    '                                    '                                        If (row("DataTypeId").ToString = "5") Then
    '                                    '                                            If fieldvalues(j).Contains(" ") Then
    '                                    '                                                filepath += RemoveSpecialChar(fieldvalues(j).Substring(0, fieldvalues(j).IndexOf(" ")), "-") + "\"
    '                                    '                                                Exit For
    '                                    '                                            Else
    '                                    '                                                filepath += RemoveSpecialChar(fieldvalues(j), "-") + "\"
    '                                    '                                                Exit For
    '                                    '                                            End If
    '                                    '                                        Else
    '                                    '                                            filepath += RemoveSpecialChar(fieldvalues(j), "-") + "\"
    '                                    '                                            Exit For
    '                                    '                                        End If

    '                                    '                                    End If

    '                                    '                                End If

    '                                    '                            Next

    '                                    '                        End If
    '                                    '                    Next

    '                                    Dim version As String = "1.0"

    '                                    If System.IO.File.Exists(System.IO.Path.Combine(ersvalue.ERSDirPath, templateList(0).CabinetName, templateList(0).TemplateName, filepath, filename.Replace("." + para.filetype, ".ezo"))) Then
    '                                        Dim values() = Function_VersionCreation(templateList(0).CabinetID.ToString(), templateId.ToString(), filepath, filename)

    '                                        Dim oldversion As String = ""
    '                                        Dim noversion As String = ""

    '                                        version = values(0)
    '                                        oldversion = values(1)

    '                                        filename = filename.Replace(Path.GetExtension(filename), "") & "_" & version & Path.GetExtension(filename)
    '                                        strqryvals = strqryvals.Replace(strqryvals.LastIndexOf(","), "")
    '                                    End If

    '                                    Dim archievePath As String = System.IO.Path.Combine(ersvalue.ERSDirPath, templateList(0).CabinetName, templateList(0).TemplateName, filepath, filename)

    '                                    If Not Directory.Exists(System.IO.Path.GetDirectoryName(archievePath)) Then
    '                                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(archievePath))
    '                                    End If
    '                                    File.WriteAllBytes(archievePath, buffer)

    '                                    logf &= " file copied to localfile " & archievePath
    '                                    If File.Exists(archievePath) Then
    '                                        Dim strInputFile = archievePath
    '                                        Dim strOutputFile = archievePath.Replace("." + para.filetype, ".ezo")
    '                                        Dim bytKey As Byte()
    '                                        Dim bytIV As Byte()
    '                                        bytKey = CreateKey("3z0f1s$ecm")
    '                                        bytIV = CreateIV("3z0f1s$ecm")
    '                                        Dim resulte As String = EncryptOrDecryptFile(strInputFile, strOutputFile, bytKey, bytIV, CryptoAction.ActionEncrypt)
    '                                    End If
    '                                    logf &= " Entered decrypting the file  " & archievePath



    '                                    strqry = " insert into ezca_" + templateList(0).CabinetID.ToString() + "_" + templateId.ToString() + "_items"
    '                                    strqry += " (ERSId,TemplateId," + strqrycols + "ifilepath,ifilename,ifiletype,ezFrom,Version,dstatus,dsize,createdon,createdBy) values(" & ersId & "," & templateId.ToString() & "," & strqryvals & "" & "'" + filepath.Replace("'", "''") + "'" & " , '" & filename & "','" & para.filetype & "','EZOFIS(API)','" & version & "','Active','0','" & DateDateTimeToString(Date.Now, True) & "','" + EcmLoginId.ToString + "')"
    '                                    logf &= "qry " & strqry

    '                                    ItemId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqry)


    '                                    Dim res = New With {.ItemId = ItemId, .URL = APIurl + "/V1/CMAP/viewfile/" & para.CabinetName & "/" & ItemId}
    '                                    Dim jsonStr = JsonConvert.SerializeObject(res)


    '                                    resmsg.errorCode = 1
    '                                    resmsg.value = jsonStr

    '                                    strqryHistory = "update eZAPICallHistory set ItemId = '" + ItemId + "',Status = 'Archived', Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"

    '                                    'strqryHistory = "insert into eZAPICallHistory (Template,CabinetId ,TemplateId ,Status,Remarks,[RIM Number],ItemId,ParentCallId ,APIFunction,XmlFileName,CreatedOn,UpdatedOnAPI,CreatedBy,UpdatedBy,Isdeleted ) values ('" + para.CabinetName + "','" + templateList(0).CabinetID.ToString() + "','" + templateId.ToString() + "','Processing','" + resmsg.value + "','" + strRimNumber + "'," + ItemId + "," + CallHistoryId.ToString + ",'Archived','" + archievePath + "','" + DateDateTimeToString(Date.Now, True) + "','','" + EcmLoginId.ToString + "','" + EcmLoginId.ToString + "',0)"
    '                                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)

    '                                Else
    '                                    resmsg.errorCode = 4
    '                                    resmsg.value = "Error code: 3_1 - filetype should not be Empty"
    '                                    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                                End If
    '                            End If
    '                        Else
    '                            resmsg.errorCode = 3
    '                            resmsg.value = "Error code: 2_3 - Empty TemplateList "
    '                            strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                            UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                        End If
    '                    Else
    '                        resmsg.errorCode = 3
    '                        resmsg.value = "Error code: 2_3 - Invalid Template Id " + templateId.ToString()
    '                        strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                        UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                    End If
    '                Else
    '                    resmsg.errorCode = 3
    '                    resmsg.value = "Error code: 2_2 - Invalid Cabinet Name"
    '                    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                End If
    '                'Else
    '                '    resmsg.errorCode = 3
    '                '    resmsg.value = "Error code: 2_4 - Cabinet Name must be in ''Corporate''"
    '                '    strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                '    UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '                'End If
    '            Else
    '                resmsg.errorCode = 2
    '                resmsg.value = "Error code: 2_1 - Cabinet Name should not be Empty"
    '                strqryHistory = "update eZAPICallHistory set Remarks='" + resmsg.value + "',[RIM Number]='" + strRimNumber + "', UpdatedOnAPI='" + DateDateTimeToString(Date.Now, True) + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '                UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Dim strqryHistory = "update eZAPICallHistory set Remarks='Exception: " + ex.ToString + "' where CallHistoryId='" + CallHistoryId.ToString + "'"
    '        Dim UCallHistoryId = InsertAndUpdateAndDeleteeZUserDefinedWithScope(strqryHistory)
    '        resmsg.errorCode = 2
    '        resmsg.value = logf & "Excepiton " & ex.ToString()
    '        ' Throw New FaultException("Exception UploadFn(): " + ex.ToString())
    '    End Try
    '    Return resmsg
    'End Function


    Private Shared Function Function_VersionCreation(ByVal CabId As String, ByVal TempId As String, ByVal IfilePath As String, filename As String) As String()
        Dim NewVersion As String = String.Empty
        Dim DublicateType As String = ""
        Dim ExistVersion As String = String.Empty
        Dim TblName As String = "eZCA_" & CabId & "_" & TempId & "_Items"
        Dim StrSql As String = "Select DuplicateType From ezDuplicateType Where DuplicateTypeId = (Select DuplicateTypeID From ezTemplate Where TemplateId=" & TempId & ")"
        Dim Dt As DataSet = GetDatasetByQuery(StrSql)
        If Dt.Tables(0).Rows.Count > 0 Then
            DublicateType = Dt.Tables(0).Rows(0).Item("DuplicateType")
            StrSql = "Select Version From " & TblName & " Where Isdeleted=0 and (ifileName Like N'" + filename.Replace("'", "''") + "' or " +
                "ifileName Like '" + filename.Insert(filename.LastIndexOf("."), "[_]%").Replace("'", "''") + "') " +
                "And IfilePath=N'" & IfilePath.Replace("'", "''") & "' order by itemid desc"

            Dt = GetDatasetByQuery(StrSql)

            If Dt.Tables(0).Rows.Count > 0 Then
                'pubFileExist = True
                ExistVersion = Dt.Tables(0).Rows(0).Item("Version")
            Else
                'writetxtfle(StrSql)
                'ExistVersion = "RenameOldFile"
                'NewVersion = "RenameOldFile"
                ExistVersion = "1.0"
            End If
            If DublicateType = "_A" Then
                If ExistVersion = "1.0" Then
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
                ElseIf ExistVersion = "1.0" Then
                    NewVersion = "2"
                ElseIf ExistVersion <> "" And ExistVersion <> "RenameOldFile" Then
                    NewVersion = ExistVersion + 1
                End If
            ElseIf DublicateType = "DateTime" Then
                NewVersion = Replace(Rjunk(DateTime.Now), " ", "")
                ExistVersion = "datetime"
            Else
                NewVersion = DublicateType
            End If
        End If
        Return {NewVersion, ExistVersion}
    End Function

    Public Shared Function GetAPICallHistoryOptionsValueFn(Para As GetOptionsValue) As DataSet
        Try
            Dim strQry As String
            Dim rwcount As Integer = 0
            Dim ds As New DataSet
            Dim dt As New DataTable("Options")
            Dim paracre As New Criteria()

            ''paracre.Criteria = "TemplateId"
            ''paracre.Value = Para.TemplateId.ToString
            ''Dim Objvalue = SelectedeZTemplateList(paracre.Criteria, paracre.Value)
            ''If Not Objvalue Is Nothing Then
            ''    Dim dr As DataRow = dt.NewRow()
            ''    For Each clm In Para.Column
            ''        dt.Columns.Add(New DataColumn(clm, GetType(String)))
            ''        strQry = "select(select distinct  " + clm + "+',' from " + Objvalue(0).TableName + " where [" + clm + "]!='' for xml path ('')) as  " + clm + " "
            ''        Dim dsq = DBLayer.DBLInstance.GetDatasetByQuery(strQry)
            ''        If Not IsNothing(dsq) AndAlso dsq.Tables.Count > 0 AndAlso dsq.Tables(0).Rows.Count > 0 Then
            ''            dr(clm) = dsq.Tables(0).Rows(0)(clm).ToString.TrimEnd(",")
            ''        End If

            ''    Next
            ''    dt.Rows.Add(dr)
            ''    ds.Tables.Add(dt)
            ''    Return ds
            ''Else
            ''    Return Nothing
            ''End If

            Dim dr As DataRow = dt.NewRow()
            For Each clm In Para.Column
                dt.Columns.Add(New DataColumn(clm, GetType(String)))
                strQry = "select(select distinct  [" + clm + "]+',' from eZAPICallHistory where [" + clm + "]!='' for xml path ('')) as  '" + clm + "' "
                Dim dsq = DBLayer.DBLInstance.GetDatasetByQuery(strQry)
                If Not IsNothing(dsq) AndAlso dsq.Tables.Count > 0 AndAlso dsq.Tables(0).Rows.Count > 0 Then
                    dr(clm) = dsq.Tables(0).Rows(0)(clm).ToString.TrimEnd(",")
                End If

            Next
            dt.Rows.Add(dr)
            ds.Tables.Add(dt)
            Return ds

        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WDBR740F300DB30 " + ex.ToString()
            Throw New FaultException(exc)
        End Try
    End Function

    Public Shared Function getDefaultCabinetName(cabinetName As String) As String
        Dim defaultCabinetName As String = ""
        Try
            Dim strQry As String = "select defaultName from ezCabinetInfo where cabinetName='" & cabinetName & "'"
            Dim ds As DataSet = GetDatasetByQuery(strQry)
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                defaultCabinetName = ds.Tables(0).Rows(0)(0).ToString()
            End If
        Catch ex As Exception

        End Try
        Return defaultCabinetName
    End Function

    Public Async Function AuthenticatUserAzureADService(username As String, password As String) As Threading.Tasks.Task
        Try
            Dim _clientId As String = ConfigurationManager.AppSettings("ClientId")


        Catch ex As Exception

        End Try
    End Function


End Class
