Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports System.Web.Http.Description
Imports ECMAPI
Imports ECMAPI.ParaVariables

Namespace Controllers
    Public Class eZUserSessionsController
        Inherits System.Web.Http.ApiController

        Private db As New InvitaECMDBEntities

        ' GET: api/eZUserSessions
        Function GeteZUserSessions() As IQueryable(Of eZUserSession)
            Return db.eZUserSessions
        End Function

        ' GET: api/eZUserSessions/5
        <ResponseType(GetType(eZUserSession))>
        Async Function GeteZUserSession(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim eZUserSession As eZUserSession = Await db.eZUserSessions.FindAsync(id)
            If IsNothing(eZUserSession) Then
                Return NotFound()
            End If

            Return Ok(eZUserSession)
        End Function

        ' PUT: api/eZUserSessions/5
        <ResponseType(GetType(Void))>
        Async Function PuteZUserSession(ByVal id As Integer, ByVal eZUserSession As eZUserSession) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = eZUserSession.SessionId Then
                Return BadRequest()
            End If

            db.Entry(eZUserSession).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (eZUserSessionExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/eZUserSessions
        <ResponseType(GetType(eZUserSession))>
        Async Function PosteZUserSession(ByVal eZUserSession As eZUserSession) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.eZUserSessions.Add(eZUserSession)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = eZUserSession.SessionId}, eZUserSession)
        End Function

        ' DELETE: api/eZUserSessions/5
        <ResponseType(GetType(eZUserSession))>
        Async Function DeleteeZUserSession(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim eZUserSession As eZUserSession = Await db.eZUserSessions.FindAsync(id)
            If IsNothing(eZUserSession) Then
                Return NotFound()
            End If

            db.eZUserSessions.Remove(eZUserSession)
            Await db.SaveChangesAsync()

            Return Ok(eZUserSession)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function eZUserSessionExists(ByVal id As Integer) As Boolean
            Return db.eZUserSessions.Count(Function(e) e.SessionId = id) > 0
        End Function

#Region "Custom"

        <HttpPost>
        Function GeteZUserSessionsByCriteria(ByVal Para As SearchRegistries) As IEnumerable(Of Object)
            Try
                Dim CondtionReg As String = ""
                For Each cond In Para.Criteria
                    If cond.Criteria = "ECMGroup" Then
                        Dim Usrlist = SharedGetFunction.GetDatasetByQuery("select stuff((select distinct ','+cast(ecmloginid as nvarchar(100)) from ezecmgroupusers where ECMGroupid = (select ecmgroupid from ezecmgroup where ECMGroup='" + cond.Value1 + "') for xml path('')),1,1,'') ")
                        If Not IsNothing(Usrlist) AndAlso Usrlist.Tables.Count > 0 AndAlso Usrlist.Tables(0).Rows.Count > 0 Then
                            cond.Criteria = "ECMLoginId"
                            cond.Value1 = Usrlist.Tables(0).Rows(0)(0).ToString()
                        Else
                            cond.Value1 = ""
                        End If

                    ElseIf cond.Criteria = "CabinetId" Then
                        Dim Usrlist = SharedGetFunction.GetDatasetByQuery("select stuff((select distinct ','+cast(templateid as nvarchar(100)) from eztemplate where Cabinetid = '" + cond.Value1 + "' for xml path('')),1,1,'')")
                        If Not IsNothing(Usrlist) AndAlso Usrlist.Tables.Count > 0 AndAlso Usrlist.Tables(0).Rows.Count > 0 Then
                            cond.Criteria = "TemplateId"
                            cond.Value1 = Usrlist.Tables(0).Rows(0)(0).ToString()
                        Else
                            cond.Value1 = ""
                        End If
                    End If

                    If cond.DataTypeId = "2" Then
                        If cond.Value1.Contains(",") Then
                            Dim Inval = ""
                            Dim values = cond.Value1.ToString.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                            For j As Integer = 0 To values.Count - 1
                                If j = 0 Then
                                    Inval = "'" + values(j) + "'"
                                Else
                                    Inval = Inval + ",'" + values(j) + "'"
                                End If
                            Next

                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in (" + Inval + ") "
                        Else
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] = '" + cond.Value1 + "'"
                        End If
                        ' CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] = '" + cond.Value1 + "'"

                    ElseIf cond.DataTypeId = "4" Then

                        CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"

                    ElseIf cond.DataTypeId = "5" Then
                        If cond.Value1 <> "" And cond.Value2 <> "" Then
                            If cond.Value1 = cond.Value2 Then
                                CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            Else

                                CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + "',101) and convert(datetime,'" + cond.Value2 + "',101)  "
                            End If

                        ElseIf cond.Value1 <> "" Then
                            CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and convert(datetime,[" + cond.Criteria + "],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                        ElseIf cond.Value2 <> "" Then
                            CondtionReg = CondtionReg + "  and convert(datetime,[" + cond.Criteria + "],101) <= convert(datetime,'" + cond.Value2 + "',101) "
                        End If
                    End If

                Next


                '  Dim result = db.FindFileName(1, 7)(0).ToString()


                Dim UserSessions = db.Database.SqlQuery(Of eZUserSessionRpt)("select dbo.udf_CabinetByTemplateId(TemplateId) as Cabinet,dbo.udf_Template(TemplateId) as Template,itemid,TemplateId, case when UplaodDocument<>0 then 'Document Uploaded' when ViewDocument<>0 then 'Document Viewed' when CommentsId <>0 then 'Document Commented' when CheckOut<>0 then 'Document Checked Out' when linkid<>0 then 'Document Linked' when AlertDocument<>0 then 'Document Alerted' when IndexingChange<>0 then 'Document Indexing Value Changed' when Deleted<>0 then 'Document Deleted' when bookmarks<>0 then 'Document Bookmarked' when email<>0 then 'Document Sent by Email' when checkin<>0 then 'Document Checked In' when PrintDoc<>0 then 'Document Printed' else '' end as Action,dbo.udf_UserName(ECMLoginId) as ActionBy,CreatedOn as ActedOn from eZUserSession where (isdeleted=0 or isdeleted=1)  " + CondtionReg + " order by Sessionid").ToList()

                Dim TotalRows = UserSessions.Count

                If Para.RowCount = 0 Then
                    Dim UserSessionss = UserSessions.Skip(Para.RowFrom)
                    Dim results = (From Reg In UserSessionss Select New With {Reg.Cabinet, Reg.Action, Reg.ActionBy, Reg.Template, Reg.ActedOn, .Filename = db.FindFileName(Reg.Itemid.ToString(), Reg.TemplateId.ToString())(0).ToString(), .TotalRow = TotalRows}).ToList()
                    Return results
                Else
                    Dim UserSessionss = UserSessions.Skip(Para.RowFrom).Take(Para.RowCount)
                    Dim results = (From Reg In UserSessionss Select New With {Reg.Cabinet, Reg.Action, Reg.ActionBy, Reg.Template, Reg.ActedOn, .Filename = db.FindFileName(Reg.Itemid.ToString(), Reg.TemplateId.ToString())(0).ToString(), .TotalRow = TotalRows}).ToList()
                    Return results
                End If

                '  Dim results = (From Reg In UserSessions Select New With {Reg.AlertDocument, Reg.bookmarks, Reg.checkin, Reg.CheckOut, Reg.TemplateId, Reg.loggedfrom, Reg.Logged, Reg.loggedat, Reg.CommentsId, Reg.Deleted, Reg.email, Reg.LinkId, Reg.itemid, Reg.PrintDoc, Reg.UplaodDocument, Reg.ViewDocument}).ToList()

                '  Return UserSessions
                'If Para.RowCount = 0 Then
                '    Return results.Skip(Para.RowFrom)
                'Else
                '    Return results.Skip(Para.RowFrom).Take(Para.RowCount)
                'End If
            Catch ex As Exception

            End Try
        End Function

#End Region

    End Class

    Partial Public Class eZUserSessionRpt


        Public Property Cabinet As String
        Public Property TemplateId As Integer
        Public Property Itemid As Integer
        Public Property Template As String
        ' Public Property FileName As String
        Public Property Action As String

        Public Property ActionBy As String
        Public Property ActedOn As String




    End Class
End Namespace