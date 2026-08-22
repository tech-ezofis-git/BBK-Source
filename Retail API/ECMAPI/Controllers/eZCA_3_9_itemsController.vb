Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports System.Web.Http.Description
Imports ECMAPI
Imports ECMAPI.ParaVariables

Namespace Controllers
    Public Class eZCA_3_9_itemsController
        Inherits System.Web.Http.ApiController

        Private db As New InvitaECMDBEntities

        ' GET: api/eZCA_3_9_items
        Function GeteZCA_3_9_items() As IQueryable(Of eZCA_3_9_items)
            Return db.eZCA_3_9_items
        End Function

        ' GET: api/eZCA_3_9_items/5
        <ResponseType(GetType(eZCA_3_9_items))>
        Async Function GeteZCA_3_9_items(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim eZCA_3_9_items As eZCA_3_9_items = Await db.eZCA_3_9_items.FindAsync(id)
            If IsNothing(eZCA_3_9_items) Then
                Return NotFound()
            End If

            Return Ok(eZCA_3_9_items)
        End Function

        ' PUT: api/eZCA_3_9_items/5
        <ResponseType(GetType(Void))>
        Async Function PuteZCA_3_9_items(ByVal id As Integer, ByVal eZCA_3_9_items As eZCA_3_9_items) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = eZCA_3_9_items.itemid Then
                Return BadRequest()
            End If

            db.Entry(eZCA_3_9_items).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (eZCA_3_9_itemsExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/eZCA_3_9_items
        <ResponseType(GetType(eZCA_3_9_items))>
        Async Function PosteZCA_3_9_items(ByVal eZCA_3_9_items As eZCA_3_9_items) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.eZCA_3_9_items.Add(eZCA_3_9_items)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = eZCA_3_9_items.itemid}, eZCA_3_9_items)
        End Function

        ' DELETE: api/eZCA_3_9_items/5
        <ResponseType(GetType(eZCA_3_9_items))>
        Async Function DeleteeZCA_3_9_items(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim eZCA_3_9_items As eZCA_3_9_items = Await db.eZCA_3_9_items.FindAsync(id)
            If IsNothing(eZCA_3_9_items) Then
                Return NotFound()
            End If

            db.eZCA_3_9_items.Remove(eZCA_3_9_items)
            Await db.SaveChangesAsync()

            Return Ok(eZCA_3_9_items)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function eZCA_3_9_itemsExists(ByVal id As Integer) As Boolean
            Return db.eZCA_3_9_items.Count(Function(e) e.itemid = id) > 0
        End Function

        <HttpPost>
        Public Function GetItemReport(Para As SearchRegistries) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                'Dim Query = ""
                'If Para.Fromdate = "" And Para.Todate = "" Then
                '    Query = "select * from ezca_" + Para.Tenantid.ToString() + "_items"
                'ElseIf Para.Fromdate <> "" And Para.Todate = "" Then
                '    Query = "select * from ezca_" + Para.Tenantid.ToString() + "_items where convert(datetime,[Date],101) between convert(datetime,'" + Para.Fromdate + "',101) and convert(datetime,'" + Para.Fromdate + "',101)"
                'Else
                '    Query = "select * from ezca_" + Para.Tenantid.ToString() + "_items where convert(datetime,[Date],101) between convert(datetime,'" + Para.Fromdate + "',101) and convert(datetime,'" + Para.Todate + "',101)"
                'End If
                'Dim DSItem = SharedGetFunction.GetDatasetByQuery(query)
                Dim lst3_9Items = db.eZCA_3_9_items.Where(Function(x) x.Isdeleted = 0)
                Dim employees = From e In db.eZCA_3_9_items Where e.Account_Number = "Tendulkar" Select e.Account_Number, e.Account_Status

                Dim results = (From itm In lst3_9Items Select New With {itm.RIM_Number, itm.CreatedOn.Max(), itm.nopages, itm.Account_Number}).ToList()
                response = Request.CreateResponse(HttpStatusCode.OK, results)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function



    End Class
End Namespace