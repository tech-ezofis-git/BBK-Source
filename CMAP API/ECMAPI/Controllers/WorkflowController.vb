Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports ECMAPI.ParaVariables
Imports ECMAPI.SharedGetFunction
Namespace Controllers
    Public Class WorkflowController
        Inherits ApiController


        <HttpPost>
        Public Function GetWorkflowListByUserId(para As ECMLoginid) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = GetWorkflowDetailsByLoginId(para.ECMLoginid, para.ECMGroupList)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GeteZWorkflowDetailsList() As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = eZWorkflowDetailsList()
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function



        <HttpPost>
        Public Function GetSelectedeZWorkflowDetailsList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = SelectedeZWorkflowDetailsList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetFilteredeZWorkflowDetailsList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = FilteredeZWorkflowDetailsList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetListeZWorkflowDetailsbyCriteria(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = ListeZWorkflowDetailsbyCriteria(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPatch>
        Public Function InsertAndUpdateeZWorkflowUsers(Para As eZWorkflowUsers) As Integer
            Dim res = ""
            Try
                res = SharedGetFunction.InsertAndUpdateeZWorkflowUsers(Para)
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