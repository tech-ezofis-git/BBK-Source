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
Imports ECMAPI.SharedGetFunction
Namespace Controllers
    Public Class eZBatchFilesController
        Inherits System.Web.Http.ApiController

        Private db As New InvitaECMDBEntities

        ' GET: api/eZBatchFiles
        Function GeteZBatchFiles() As IQueryable(Of eZBatchFile)
            Return db.eZBatchFiles
        End Function

        ' GET: api/eZBatchFiles/5
        <ResponseType(GetType(eZBatchFile))>
        Async Function GeteZBatchFile(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim eZBatchFile As eZBatchFile = Await db.eZBatchFiles.FindAsync(id)
            If IsNothing(eZBatchFile) Then
                Return NotFound()
            End If

            Return Ok(eZBatchFile)
        End Function

        ' PUT: api/eZBatchFiles/5
        <ResponseType(GetType(Void))>
        Async Function PuteZBatchFile(ByVal id As Integer, ByVal eZBatchFile As eZBatchFile) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = eZBatchFile.BatchFileId Then
                Return BadRequest()
            End If

            db.Entry(eZBatchFile).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (eZBatchFileExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/eZBatchFiles
        <ResponseType(GetType(eZBatchFile))>
        Async Function PosteZBatchFile(ByVal eZBatchFile As eZBatchFile) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.eZBatchFiles.Add(eZBatchFile)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = eZBatchFile.BatchFileId}, eZBatchFile)
        End Function

        ' DELETE: api/eZBatchFiles/5
        <ResponseType(GetType(eZBatchFile))>
        Async Function DeleteeZBatchFile(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim eZBatchFile As eZBatchFile = Await db.eZBatchFiles.FindAsync(id)
            If IsNothing(eZBatchFile) Then
                Return NotFound()
            End If

            db.eZBatchFiles.Remove(eZBatchFile)
            Await db.SaveChangesAsync()

            Return Ok(eZBatchFile)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function eZBatchFileExists(ByVal id As Integer) As Boolean
            Return db.eZBatchFiles.Count(Function(e) e.BatchFileId = id) > 0
        End Function


        <HttpPost>
        Function GetBatchFilesList(ByVal Para As SearchRegistries) As DataSet

            Dim CondtionReg As String = ""
            Dim Tablename = ""
            Dim templateid = ""
            Dim ECMLoginId = ""

            Try


                For Each cond In Para.Criteria

                    If cond.Criteria.ToLower() = "createdby" Then
                        If cond.Value1 <> "" AndAlso Not cond.Value1.Contains(",") Then
                            cond.Value1 = GetLoginIdByUsername(cond.Value1)
                        End If
                    End If
                    If cond.DataTypeId = "2" Then
                        If cond.Value1.Contains(",") Then
                            Dim Inval = ""
                            Dim values = cond.Value1.ToString.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                            For j As Integer = 0 To values.Count - 1
                                If cond.Criteria.ToLower() = "createdby" Then
                                    If values(j) <> "" Then
                                        values(j) = GetLoginIdByUsername(values(j))
                                    End If
                                End If

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


                    ElseIf cond.DataTypeId = "4" Then
                        CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                    ElseIf cond.DataTypeId = "5" Then
                        If cond.Value1 <> "" And cond.Value2 <> "" Then
                            If cond.Value1 = cond.Value2 Then
                                CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            Else

                                CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            End If
                        ElseIf cond.Value1 <> "" Then
                            CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and convert(datetime,[" + cond.Criteria + "],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                        ElseIf cond.Value2 <> "" Then
                            CondtionReg = CondtionReg + "  and convert(datetime,[" + cond.Criteria + "],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                        End If
                    End If
                Next

                Dim FinalCountQuery = "select count(1) from ezBatchProcessing as BP,ezbatchfiles as Bf where bp.Batchid=bf.batchid " + CondtionReg
                Dim TotalRow = "0"
                Dim ItemCountList = GetDatasetByQuery(FinalCountQuery)
                If Not IsNothing(ItemCountList) AndAlso ItemCountList.Tables.Count > 0 AndAlso ItemCountList.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemCountList.Tables(0).Rows(0)(0).ToString()
                End If

                Dim FinalQuery = ""
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then

                        FinalQuery = "select 'B'+ Bf.BatchId as BatchId, Bf.Rimnumber,Bp.ScanedFile as [Scaned File Name],Bf.Filename as [Splited File Name],BP.ScanedAt as [Scaned At],BP.ScanedOn as [Scaned On],BP.ImportedAt as [Imported At],BP.ImportedOn as [Imported On],BF.ExportedAt as [Exported At],BF.ExportedOn as [Exported On],dbo.udf_LoginName (BP.Importedby) as [Imported By],'" + TotalRow + "' as TotalRow from ezBatchProcessing as BP,ezbatchfiles as Bf where bp.Batchid=bf.batchid " + CondtionReg & " order by Bp.Batchid OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "select 'B'+ Bf.BatchId as BatchId,Bf.Rimnumber,Bp.ScanedFile as [Scaned File Name],Bf.Filename as [Splited File Name],BP.ScanedAt as [Scaned At],BP.ScanedOn as [Scaned On],BP.ImportedAt as [Imported At],BP.ImportedOn as [Imported On],BF.ExportedAt as [Exported At],BF.ExportedOn as [Exported On],dbo.udf_LoginName (BP.Importedby) as [Imported By],'" + TotalRow + "' as TotalRow from ezBatchProcessing as BP,ezbatchfiles as Bf where bp.Batchid=bf.batchid  " + CondtionReg & " order by Bp.Batchid "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        FinalQuery = "SELECT * FROM (select 'B'+ Bf.BatchId as BatchId,Bf.Rimnumber,Bp.ScanedFile as [Scaned File Name],Bf.Filename as [Splited File Name],BP.ScanedAt as [Scaned At],BP.ScanedOn as [Scaned On],BP.ImportedAt as [Imported At],BP.ImportedOn as [Imported On],BF.ExportedAt as [Exported At],BF.ExportedOn as [Exported On],dbo.udf_LoginName (BP.Importedby) as [Imported By],'" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY Bp.Batchid) AS Seq from ezBatchProcessing as BP,ezbatchfiles as Bf where bp.Batchid=bf.batchid " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                    Else
                        FinalQuery = "select 'B'+ Bf.BatchId as BatchId, Bf.Rimnumber,Bp.ScanedFile as [Scaned File Name],Bf.Filename as [Splited File Name],BP.ScanedAt as [Scaned At],BP.ScanedOn as [Scaned On],BP.ImportedAt as [Imported At],BP.ImportedOn as [Imported On],BF.ExportedAt as [Exported At],BF.ExportedOn as [Exported On],dbo.udf_LoginName (BP.Importedby) as [Imported By],'" + TotalRow + "' as TotalRow from ezBatchProcessing as BP,ezbatchfiles as Bf where bp.Batchid=bf.batchid  " + CondtionReg & " order by Bp.Batchid"

                    End If
                End If
                'Dim fields As DataSet
                'If templateid <> "" Then
                '    Dim FieldQuery = "Select * from eZTemplateField where templateid=  " + templateid
                '    fields = GetDatasetByQuery(FieldQuery)
                'End If

                Dim ItemList = GetDatasetByQuery(FinalQuery)

                ' Dim res = {ItemList, fields}

                Return ItemList
            Catch ex As Exception

            End Try

        End Function


    End Class
End Namespace