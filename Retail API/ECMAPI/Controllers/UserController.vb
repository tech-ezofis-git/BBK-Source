Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports ECMAPI.ParaVariables
Imports Newtonsoft.Json
Imports ECMAPI.SharedGetFunction
Imports System.IO

Namespace Controllers
    Public Class UserController
        Inherits ApiController
        <HttpPost>
        Public Function UserLogin(Para As UserLogin) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                If Para.UserName <> "" AndAlso Para.Password <> "" Then
                    ' Dim pasword = DBLayer.Encrypt(Para.Password, "vairavaraj", "vairavaraj", "SHA1", 1, "@v#a5i%r&a7v&a#j", 192)
                    Dim ECMLogininfo As New OldeZECMLogin
                    ECMLogininfo = SharedGetFunction.UserLogin(Para.UserName, Para.Password)
                    If Not IsNothing(ECMLogininfo) Then

                        response = Request.CreateResponse(HttpStatusCode.OK, ECMLogininfo)
                    Else
                        response = Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, "Invalid Username And Password")
                    End If
                Else
                    response = Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, "Invalid Username And Password")
                End If
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function


        <HttpPost>
        Public Function GetECMUserInfoByUserId(ECMLoginid As Integer) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = SelectedeZECMUserInfoList("ECMLoginid", ECMLoginid.ToString())
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPatch>
        Public Function UpdateUserInfos(Para As Userinfo) As HttpResponseMessage
            Dim response As HttpResponseMessage

            Try
                Dim Res = InsertAndUpdateeZECMLoginWithUserInfo(Para.ECMLoginInfo, Para.ECMUserInfo)
                If Res = "Success" Then
                    response = Request.CreateResponse(HttpStatusCode.Created, Res)
                Else
                    response = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Recored Not Inserted")
                End If
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPatch>
        Public Function UpdateUserPassword(Para As UpdatePassword) As HttpResponseMessage
            Dim response As HttpResponseMessage

            Try
                Dim Res = UpdateeZECMLoginPassword(Para.ECMLoginId, Para.Password)
                If Res = "Success" Then
                    response = Request.CreateResponse(HttpStatusCode.Created, "Password Updated Successfully")
                Else
                    response = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Password Not Updated")
                End If
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetSelectedeZECMUserInfoList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = SelectedeZECMUserInfoList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetSelectedeZECMLoginList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = SelectedeZECMLoginList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetselectedeZECMGroupusersList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = selectedeZECMGroupusersList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

    End Class
End Namespace