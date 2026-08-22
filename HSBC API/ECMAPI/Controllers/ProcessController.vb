Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports ECMAPI.ParaVariables
Imports ECMAPI.SharedGetFunction
Namespace Controllers
    Public Class ProcessController
        Inherits ApiController

        <HttpPost>
        Public Function GetWorkflowProcessCount(para As FlowInfo) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = WorkflowProcessCount(para.WorkflowId, para.ECMLoginId, para.ECMGroupList)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetDatasetbyQuery(para As query) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = SharedGetFunction.GetDatasetByQuery(para.query)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function


        <HttpPost>
        Public Function GetWorkflowProcessList(Para As ProcessInfo) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = ProcessListbyUserid(Para)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetWorkflowCompletedList(Para As ProcessInfo) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = CompletedListbyUserid(Para)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function



        <HttpPost>
        Public Function GeteZWFProcessList() As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = eZWFProcessList()
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function



        <HttpPost>
        Public Function GetSelectedeZWFProcessList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = SelectedeZWFProcessList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetFilteredeZWFProcessList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = FilteredeZWFProcessList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function


        <HttpPatch>
        Public Function InsertAndUpdateeZWFProcess(Para As eZWFProcess) As Integer
            Dim res = ""
            Try
                res = SharedGetFunction.InsertAndUpdateeZWFProcess(Para)
            Catch ex As Exception
                res = Nothing
                Dim exc As String
                exc = "ERROR CODE : WDSB002F200 " + ex.Message.ToString()
                Throw New FaultException(exc)
            End Try
            Return res
        End Function

        <HttpPatch>
        Public Function InsertAndUpdateeZMail(Para As eZMail) As Integer
            Dim res = ""
            Try
                res = SharedGetFunction.InsertAndUpdateeZMail(Para)
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