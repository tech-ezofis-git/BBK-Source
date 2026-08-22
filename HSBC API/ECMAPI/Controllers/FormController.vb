Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports ECMAPI.ParaVariables
Imports ECMAPI.SharedGetFunction

Namespace Controllers
    Public Class FormController
        Inherits ApiController
        <HttpPost>
        Public Function GetFormDetails(Para As Forminfo) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = GetFormvalues(Para.Processid, Para.Transid, Para.Workflowid)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function
    End Class
End Namespace