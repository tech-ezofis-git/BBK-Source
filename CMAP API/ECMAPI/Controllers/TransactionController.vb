Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports ECMAPI.ParaVariables
Imports ECMAPI.SharedGetFunction

Namespace Controllers
    Public Class TransactionController
        Inherits ApiController


        <HttpPost>
        Public Function GetWorkflowInboxList(Para As ProcessInfo) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = InboxListbyUserid(Para)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetWorkflowQueueList(Para As ProcessInfo) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = QueueListbyUserid(Para)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function



        <HttpPost>
        Public Function GeteZWFlowTransationList() As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = eZWFlowTransationList()
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function



        <HttpPost>
        Public Function GetSelectedeZWFlowTransationList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = SelectedeZWFlowTransationList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetFilteredeZWFlowTransationList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = FilteredeZWFlowTransationList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPatch>
        Public Function InsertAndUpdateeZWFlowTransaction(Para As eZWFlowTransation) As Integer
            Dim res = ""
            Try
                res = InsertandUpdateeZWFlowTransation(Para)
            Catch ex As Exception
                res = Nothing
                Dim exc As String
                exc = "ERROR CODE : WDSB002F200 " + ex.Message.ToString()
                Throw New FaultException(exc)
            End Try
            Return res
        End Function




    End Class
End Namespace