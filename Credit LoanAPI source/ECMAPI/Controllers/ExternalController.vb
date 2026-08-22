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





    End Class
End Namespace