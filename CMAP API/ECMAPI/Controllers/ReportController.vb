Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Web.Http
Imports ECMAPI.ParaVariables
Imports ECMAPI.SharedGetFunction

Namespace Controllers
    Public Class ReportController
        Inherits ApiController
        <HttpPost>
        Public Function SessionReport(para As SearchRegistries) As HttpResponseMessage
            Dim StrQry = "", rowqry = ""
            Dim ds As New DataSet
            Dim result As New ResSessionReport()
            Dim response As HttpResponseMessage
            Try
                Dim CondtionReg As String = ""
                Dim Tablename = ""
                For Each cond In para.Criteria
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

                            CondtionReg = CondtionReg + " and  us.[" + cond.Criteria + "] in (" + Inval + ") "
                        Else
                            If cond.Criteria.ToLower = "commentsid" Or cond.Criteria.ToLower = "linkid" Then
                                CondtionReg = CondtionReg + " and  us.[" + cond.Criteria + "] != '0'"
                            Else
                                CondtionReg = CondtionReg + " and  us.[" + cond.Criteria + "] = '" + cond.Value1 + "'"
                            End If

                        End If

                    ElseIf cond.DataTypeId = "4" Then
                        CondtionReg = CondtionReg + " and  us.[" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                    ElseIf cond.DataTypeId = "5" Then
                        If cond.Value1 <> "" And cond.Value2 <> "" Then
                            If cond.Value1 = cond.Value2 Then
                                CondtionReg = CondtionReg + " and us.[" + cond.Criteria + "] <> '' and  convert(datetime,us.[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            Else
                                CondtionReg = CondtionReg + " and us.[" + cond.Criteria + "] <> '' and  convert(datetime,us.[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            End If

                        ElseIf cond.Value1 <> "" Then
                            CondtionReg = CondtionReg + " and us.[" + cond.Criteria + "] <> '' and convert(datetime,us.[" + cond.Criteria + "],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                        ElseIf cond.Value2 <> "" Then
                            CondtionReg = CondtionReg + "  and convert(datetime,us.[" + cond.Criteria + "],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                        End If
                    End If
                Next
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from ezusersession us where isdeleted=0 " + CondtionReg)
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    result.totalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If para.RowCount <> 0 Then
                    rowqry = "  OFFSET " + para.RowFrom.ToString() + " ROWS FETCH NEXT " + para.RowCount.ToString() + " ROWS ONLY"
                End If
                StrQry = "select sessionid,case when Module is null then loggedfrom+' ['+loggedat+']' when [Module]='Group' then [Module]+' ['+(select ECMGroup  from [eZECMGroup] where ECMGroupId=us.Actionid)+']' when [Module]='Profile' and Action='User Profile Modified' then [Module]+' ['+(select case when l.ECMProfileId=0 then 'Removed' else p.ECMProfile end  from [eZECMLogin] l left join [eZECMProfile] p on p.ECMProfileId=l.ECMProfileId where ECMLoginId=us.Actionid)+']'  when [Module]='Profile' and Action!='User Profile Modified' then [Module]+' ['+(select ECMProfile  from [eZECMProfile] where ECMProfileId=us.Actionid)+']' else Module end as Module,case when Logged<>0 then 'Logged' when UplaodDocument<>0 then 'Document Uploaded' when ViewDocument<>0 then 'Document Viewed' when CommentsId <>0 then 'Document Commented' when us.CheckOut<>0 then 'Document Checked Out' when linkid<>0 then 'Document Linked' when AlertDocument<>0 then 'Document Alerted' when IndexingChange<>0 then 'Document Indexing Value Changed' when Deleted<>0 then 'Document Deleted' when bookmarks<>0 then 'Document Bookmarked' when email<>0 then 'Document Sent by Email' when checkin<>0 then 'Document Checked In' when PrintDoc<>0 then 'Document Printed' else case when Module!='' and [Module]='User' then [Action]+' ['+(select LoginName  from [eZECMLogin] where ECMLoginId=us.Actionid)+']' when Module!='' and [Module]='Profile' and Action='User Profile Modified' then [Action]+' ['+(select LoginName  from [eZECMLogin] where ECMLoginId=us.Actionid)+']' else case when Module!='' then [Action] else ''  end end end as [Action] ,l.LoginName as [Action By],us.CreatedOn as [Acted On] from ezusersession us left join eZECMLogin l on us.ECMLoginId=l.ECMLoginId where us.isdeleted=0 " + CondtionReg + "  order by sessionid desc " + rowqry + ""

                ds = SharedGetFunction.GetDatasetByQuery(StrQry)
                result.data = ds
                result.rowCount = para.RowCount
                response = Request.CreateResponse(HttpStatusCode.OK, result)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function SessionDetailedReport(para As SessionDetailed) As HttpResponseMessage
            Dim StrQry = "", rowqry = "", strqry1 = ""
            Dim ds As New DataSet
            Dim ds1 As New DataSet
            Dim response As HttpResponseMessage
            Try

                StrQry = "select sessionid,[dbo].[udf_Cabinetid](9) cabinetId,TemplateId,itemid,dbo.udf_CabinetByTemplateId(TemplateId) cabinet,[dbo].[udf_Template](TemplateId) Template,stuff((select ',['+FieldName+']'  from eZTemplateField where Isdeleted=0 and TemplateId=us.TemplateId for xml path('')),1,1,'') as fieldlst,CommentsId,case when [Module] is not null then [Module] else '' end as Module,case when Logged<>0 then 'Logged' when UplaodDocument<>0 then 'Document Uploaded' when ViewDocument<>0 then 'Document Viewed' when CommentsId <>0 then 'Document Commented' when us.CheckOut<>0 then 'Document Checked Out' when linkid<>0 then 'Document Linked' when AlertDocument<>0 then 'Document Alerted' when IndexingChange<>0 then 'Document Indexing Value Changed' when Deleted<>0 then 'Document Deleted' when bookmarks<>0 then 'Document Bookmarked' when email<>0 then 'Document Sent by Email' when checkin<>0 then 'Document Checked In' when PrintDoc<>0 then 'Document Printed' else case when Module!='' and [Module]='User' then [Action]+' ['+(select LoginName  from [eZECMLogin] where ECMLoginId=us.Actionid)+']' when Module!='' and [Module]='Profile' and Action='User Profile Modified' then [Action]+' ['+(select LoginName  from [eZECMLogin] where ECMLoginId=us.Actionid)+']' else case when Module!='' then [Action] else ''  end end end as [Action] ,l.LoginName as [Action By],us.CreatedOn as [Acted On],case when Logged<>0 then Logged when UplaodDocument<>0 then UplaodDocument when ViewDocument<>0 then ViewDocument when CommentsId <>0 then CommentsId when us.CheckOut<>0 then us.CheckOut when linkid<>0 then linkid when AlertDocument<>0 then AlertDocument when IndexingChange<>0 then IndexingChange when Deleted<>0 then Deleted when bookmarks<>0 then bookmarks when email<>0 then email when checkin<>0 then checkin when PrintDoc<>0 then PrintDoc else case when Module!='' then [ActionId] else ''  end end as ActionId,ActionFor from ezusersession us left join eZECMLogin l on us.ECMLoginId=l.ECMLoginId where us.isdeleted=0 and sessionid=" + para.sessionId.ToString + ""
                ds = SharedGetFunction.GetDatasetByQuery(StrQry)
                If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    Dim strselect = ""
                    If ds.Tables(0).Rows(0)("ActionFor").ToString <> "" Then
                        strselect = ds.Tables(0).Rows(0)("ActionFor").ToString
                    ElseIf ds.Tables(0).Rows(0)("Action").ToString <> "" Then
                        strselect = ds.Tables(0).Rows(0)("Action").ToString
                    End If
                    Select Case strselect
                        Case "Logged", "User"
                            strqry1 = "select firstName 'First Name',mobile 'Mobile',emailAddress 'Email Address',designation 'Designation',department 'Department',[dbo].[udf_LoginName](manager) 'Manager' from eZECMUserInfo where UserId=" + ds.Tables(0).Rows(0)("ActionId").ToString + ""
                        Case "Document Uploaded", "Document Viewed"
                            strqry1 = "select  '" + ds.Tables(0).Rows(0)("cabinet").ToString + "' Cabinet,'" + ds.Tables(0).Rows(0)("Template").ToString + "' Template," + ds.Tables(0).Rows(0)("fieldlst").ToString + ",ifilename 'File Name',version 'Version',dsize 'File Size',createdOn 'Archived On',[dbo].[udf_LoginName](CreatedBy) 'Archived By'  from eZCA_" + ds.Tables(0).Rows(0)("cabinetId").ToString + "_" + ds.Tables(0).Rows(0)("TemplateId").ToString + "_items where itemid=" + ds.Tables(0).Rows(0)("itemid").ToString + ""
                        Case "Document Commented"
                            strqry1 = "select  '" + ds.Tables(0).Rows(0)("cabinet").ToString + "' Cabinet,'" + ds.Tables(0).Rows(0)("Template").ToString + "' Template," + ds.Tables(0).Rows(0)("fieldlst").ToString + ",ifilename 'File Name',version 'Version',dsize 'File Size',createdOn 'Archived On',[dbo].[udf_LoginName](CreatedBy) 'Archived By',(select Comments  from eZComments where CommentsId='" + ds.Tables(0).Rows(0)("CommentsId").ToString + "') Comments  from eZCA_" + ds.Tables(0).Rows(0)("cabinetId").ToString + "_" + ds.Tables(0).Rows(0)("TemplateId").ToString + "_items where itemid=" + ds.Tables(0).Rows(0)("itemid").ToString + ""
                        'Case "Document Checked Out"
                        '    strqry1 = ""
                        'Case "Document Linked"
                        '    strqry1 = ""
                        'Case "Document Alerted"
                        '    strqry1 = ""
                        'Case "Document Indexing Value Changed"
                        '    strqry1 = ""
                        'Case "Document Deleted"
                        '    strqry1 = ""
                        'Case "Document Bookmarked"
                        '    strqry1 = ""
                        'Case "Document Sent by Email"
                        '    strqry1 = ""
                        'Case "Document Checked In"
                        '    strqry1 = ""
                        'Case "Document Printed"
                        '    strqry1 = ""
                        Case "Profile"
                            strqry1 = "select ECMProfile 'Profile Name',description 'Description', stuff((select ','+ECMControl  from ezecmcontrol where Isdeleted=0  and ecmcontrolid in (select ECMControlId from eZECMControlLevel where ECMProfileId='" + ds.Tables(0).Rows(0)("ActionId").ToString + "' and Isdeleted=0)for xml path('')),1,1,'') as 'Control List'   from eZECMProfile where ECMProfileId='" + ds.Tables(0).Rows(0)("ActionId").ToString + "'"
                        Case "Profile User"
                            Dim profile = ds.Tables(0).Rows(0)("Module").ToString.Replace("Profile [", "").Replace("]", "")
                            strqry1 = "select ECMProfile 'Profile Name',description 'Description', stuff((select ','+ECMControl  from ezecmcontrol where Isdeleted=0  and ecmcontrolid in (select ECMControlId from eZECMControlLevel where ECMProfileId=p.ECMProfileId and Isdeleted=0)for xml path('')),1,1,'') as 'Control List'   from eZECMProfile p where ECMProfile='" + profile + "'"
                        Case "Group"
                            strqry1 = "select ECMGroup 'Group Name',description 'Description',createdOn 'Created On',[dbo].[udf_LoginName](CreatedBy) 'Created By', stuff((select  ','+[dbo].[udf_LoginName](ECMLoginId ) from eZECMGroupUsers where Isdeleted=0 and ECMGroupId='" + ds.Tables(0).Rows(0)("ActionId").ToString + "' for xml path('')),1,1,'') as 'Group List' from eZECMGroup where ECMGroupId='" + ds.Tables(0).Rows(0)("ActionId").ToString + "'"
                        Case "Master Table"
                            Dim qry = "select stuff(( select ',['+COLUMN_NAME+']' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = N'ezfb_" + ds.Tables(0).Rows(0)("Module").ToString + "' and COLUMN_NAME NOT IN ('itemid','createdon','CreatedBY','UpdatedOn','UpdatedBy','Isdeleted')for xml path('')),1,1,'') as columnlist"
                            Dim dsq = SharedGetFunction.GetDatasetByQuery(qry)
                            If Not dsq Is Nothing AndAlso dsq.Tables.Count > 0 AndAlso dsq.Tables(0).Rows.Count > 0 Then
                                strqry1 = "select " + dsq.Tables(0).Rows(0)("columnlist").ToString + " from [ezfb_" + ds.Tables(0).Rows(0)("Module").ToString + "] where itemid='" + ds.Tables(0).Rows(0)("ActionId").ToString + "'"
                            End If

                        Case "Transaction"
                            Dim wfarr = ds.Tables(0).Rows(0)("Module").ToString.Split({"]"}, StringSplitOptions.RemoveEmptyEntries)
                            If wfarr.Count > 0 Then
                                Dim wfname = wfarr(0).ToString().Replace("[", "")
                                Dim wfstage = wfarr(1).ToString().Trim
                                strqry1 = "select '" + wfname + "' 'Workflow Name',(select RequestNo  from eZWFProcess where ProcessId=t.Processid) 'Request No','" + wfstage + "' 'Stage Name',t.review Action,Createdon 'Raised On',[dbo].[udf_LoginName](t.Createdby  ) 'Raised By' from eZWFlowTransation t where Transactionid='" + ds.Tables(0).Rows(0)("ActionId").ToString + "'"
                            End If
                        Case Else
                            strqry1 = ""
                    End Select
                    If strqry1 <> "" Then
                        ds1 = SharedGetFunction.GetDatasetByQuery(strqry1)
                    End If
                End If
                response = Request.CreateResponse(HttpStatusCode.OK, ds1)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function
#Region "BBK TradeFinance Report"
        <HttpPost>
        Public Function TATReport(para As SearchRegistries) As HttpResponseMessage
            Dim strQry As String = "", rowQry As String = ""
            Dim response As HttpResponseMessage
            Dim result As New TATReport
            Dim lstItems As New List(Of TATReportA)
            Dim transDtCond As String = ""
            Dim mainqrycondMakerchecker As String = ""
            Try
                Dim CondtionReg As String = ""
                Dim Tablename = ""
                For Each cond In para.Criteria
                    'If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                    '    cond.Criteria = "wf.[createdBy]"
                    'ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                    '    cond.Criteria = "wf.createdon"
                    'ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                    '    cond.Criteria = "BTH.createdon"
                    'End If
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
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                                CondtionReg = CondtionReg + " and wf.[createdby] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                                CondtionReg = CondtionReg + " and [RIM Number] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                                CondtionReg = CondtionReg + " and [Account Number] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                                transDtCond = " and action like '%maker%' and ( ActivityUserId in (" + Inval + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + Inval + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + ")))) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                                transDtCond = " and action like '%checker%' and ( ActivityUserId in (" + Inval + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + Inval + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + ")))) "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in (" + Inval + ") "
                            End If
                        Else
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                                CondtionReg = CondtionReg + " and wf.[createdby] in ('" + cond.Value1 + "') "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                                CondtionReg = CondtionReg + " and [RIM Number] in (" + cond.Value1 + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                                CondtionReg = CondtionReg + " and [Account Number] in (" + cond.Value1 + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                                transDtCond = " and action like '%maker%' and ( ActivityUserId in (" + cond.Value1 + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + cond.Value1 + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                                transDtCond = " and action like '%checker%' and ( ActivityUserId in (" + cond.Value1 + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + cond.Value1 + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in ('" + cond.Value1 + "') "
                            End If

                        End If
                    ElseIf cond.DataTypeId = "4" Then
                        If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                            CondtionReg = CondtionReg + " and wf.[createdby] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                            CondtionReg = CondtionReg + " and [RIM Number] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                            CondtionReg = CondtionReg + " and [Account Number] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                            transDtCond = " and action like '%maker%' and ( ActivityUserId like %" + cond.Value1 + "% or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId like %" + cond.Value1 + "%)) "
                            mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                            transDtCond = " and action like '%checker%' and ( ActivityUserId like %" + cond.Value1 + "% or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId like %" + cond.Value1 + "%)) "
                            mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                        Else
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                        End If
                    ElseIf cond.DataTypeId = "5" Then
                        If cond.Value1 <> "" And cond.Value2 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + " and wf.[createdon] <> '' and  convert(datetime,wf.[createdon],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + " and BTH.[createdon] <> '' and  convert(datetime,BTH.[createdon],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                'Else

                                '    CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            End If

                        ElseIf cond.Value1 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + " and wf.[createdon] <> '' and convert(datetime,wf.[createdon],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + " and BTH.[createdon] <> '' and convert(datetime,BTH.[createdon],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                            End If

                        ElseIf cond.Value2 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + "  and convert(datetime,wf.[createdon],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + "  and convert(datetime,BTH.[createdon],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                            End If
                        End If
                    End If
                Next
                If para.RowCount <> 0 Then
                    rowQry = "  OFFSET " + para.RowFrom.ToString() + " ROWS FETCH NEXT " + para.RowCount.ToString() + " ROWS ONLY"
                End If
                strQry = "SELECT wf.processid,wf.workflowId, wf.requestNo,flowstatus,format(convert(datetime,BTH.CreatedOn,113),'dd-MMM-yyyy hh:mm:ss tt') AS 'scannedat',Bth.FirstName as 'scannedby',case when (ezt.[Eximbills Reference] is null or ezt.[Eximbills Reference]='')  then 'NONE' else ezt.[Eximbills Reference] end  as [Transaction reference],[RIM Number] AS RIM,[Account Number] as AccountNO ,Product,   Phase,[RIM Number],isnull(Type,'None') Type ,'' as Stage,wf.createdon AS 'claimed On',Bth.FirstName AS 'claimed By','' AS 'Submitted to Approval','' AS 'Received by' FROM  ezwfprocess wf LEFT JOIN (select distinct [Work Item Reference],isdeleted ,[RIM Number],[Account Number],Product,Phase,Type,[Eximbills Module]  from  ezca_3_15_items) itm  ON itm.[Work Item Reference] = wf.requestNo  left join (select distinct [work item reference] ,Bt.createdOn,dbo.udf_Firstname(BT.createdBy) as FirstName  from  BBK_TicketQueue  BT  )BTH on BTH.[Work Item Reference]=itm.[Work Item Reference]  left join (select [Eximbills Reference],[Work Item Reference] from [ezfb_Trade Finance Form]) ezt on ezt.[Work Item Reference]=wf.RequestNo  WHERE itm.isdeleted = 0 and wf.WorkflowId=5 and wf.isdeleted=0 and wf.RequestNo  is not null " & CondtionReg & " " & mainqrycondMakerchecker & " order by wf.createdon desc " & rowQry
                Dim ds As DataSet = GetDatasetByQuery(strQry)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    Dim datas = ds.Tables(0).AsEnumerable().Select(Function(objRead)
                                                                       Dim objItems = New TATReportA With {
                    .processId = objRead.Field(Of Int32)("processid"),
                    .requestNo = objRead.Field(Of String)("requestNo"),
                    .workflowId = objRead.Field(Of Int32)("workflowId"),
                    .transactionReference = objRead.Field(Of String)("Transaction reference"),
                    .rim = objRead.Field(Of String)("RIM"),
                    .scannedBy = objRead.Field(Of String)("scannedby"),
                    .scanDateandTime = objRead.Field(Of String)("scannedat"),
                    .accountNo = objRead.Field(Of String)("AccountNo"),
                    .product = objRead.Field(Of String)("Product"),
                    .phase = objRead.Field(Of String)("Phase"),
                    .type = objRead.Field(Of String)("Type"),
                    .rimNumber = objRead.Field(Of String)("RIM Number"),
                    .claimedOn = objRead.Field(Of String)("claimed On"),
                    .claimedBy = objRead.Field(Of String)("claimed By")
                    }
                                                                       Dim processid = objRead.Field(Of Int32)("processid")
                                                                       'for taking stage 
                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where processid=" + processid.ToString() + "  and isdeleted=0  order by transactionid desc"
                                                                           Dim dstrans As DataSet = GetDatasetByQuery(strQry)
                                                                           If dstrans IsNot Nothing AndAlso dstrans.Tables.Count > 0 AndAlso dstrans.Tables(0).Rows.Count > 0 Then
                                                                               objItems.stage = dstrans.Tables(0).Rows(0)("Action").ToString()
                                                                               objItems.activityId = dstrans.Tables(0).Rows(0)("ActivityId").ToString()
                                                                               objItems.currentlyReceivingTime = dstrans.Tables(0).Rows(0)("Createdon").ToString()
                                                                           Else
                                                                               strQry = "select Action,ActivityId,Updatedon from ezwflowtransation_completed where processid=" + processid.ToString() + "  and isdeleted=0  order by transactionid desc"
                                                                               Dim dstrans_comp As DataSet = GetDatasetByQuery(strQry)
                                                                               If dstrans_comp IsNot Nothing AndAlso dstrans_comp.Tables.Count > 0 AndAlso dstrans_comp.Tables(0).Rows.Count > 0 Then
                                                                                   objItems.stage = dstrans_comp.Tables(0).Rows(0)("Action").ToString()
                                                                                   objItems.activityId = dstrans_comp.Tables(0).Rows(0)("ActivityId").ToString()
                                                                                   objItems.currentlyReceivingTime = dstrans.Tables(0).Rows(0)("Updatedon").ToString()
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try


                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where processid=" + processid.ToString() + " and Action in ('Pending in Queue','waiting for documents') and transactionstatus=0"
                                                                           Dim dstrans As DataSet = GetDatasetByQuery(strQry)
                                                                           If dstrans IsNot Nothing AndAlso dstrans.Tables.Count > 0 AndAlso dstrans.Tables(0).Rows.Count > 0 Then
                                                                               objItems.receivedBy = "None"
                                                                           Else
                                                                               strQry = "select distinct loginName from ezecmlogin where isdeleted=0 and  ecmloginid in (select activityuserid from ezwflowtransation where transactionstatus=0 and processid=" + processid.ToString() + " and isdeleted=0 UNION select ezg.ECMloginid from  ezecmgroupusers ezg JOIN ezwflowtransation ezt on ezg.ecmgroupid = ezt.ActivityGroupId where transactionstatus=0 and processid=" + processid.ToString() + " and ezt.isdeleted=0 and ezg.isdeleted=0)"

                                                                               Dim dsuser As DataSet = GetDatasetByQuery(strQry)
                                                                               If dsuser IsNot Nothing AndAlso dsuser.Tables.Count > 0 AndAlso dsuser.Tables(0).Rows.Count > 0 Then
                                                                                   Dim res As List(Of String) = dsuser.Tables(0).AsEnumerable().Select(Function(row) row.Field(Of String)("loginName")).ToList()
                                                                                   If res.Count > 0 Then
                                                                                       objItems.receivedBy = res(0)
                                                                                   End If
                                                                               Else
                                                                                   strQry = "select distinct loginName from ezecmlogin where isdeleted=0 and  ecmloginid in (select activityuserid from eZWFlowTransation_Completed where transactionstatus=1 and processid=" + processid.ToString() + " and isdeleted=0 and review='Approve to Close' UNION select ezg.ECMloginid from  ezecmgroupusers ezg JOIN eZWFlowTransation_Completed ezt on ezg.ecmgroupid = ezt.ActivityGroupId where transactionstatus=1 and processid=" + processid.ToString() + " and ezt.isdeleted=0 and ezg.isdeleted=0 and ezt.review='Approve to Close')"
                                                                                   Dim ds_transcompl As DataSet = GetDatasetByQuery(strQry)
                                                                                   If ds_transcompl IsNot Nothing AndAlso ds_transcompl.Tables.Count > 0 AndAlso ds_transcompl.Tables(0).Rows.Count > 0 Then
                                                                                       Dim res As List(Of String) = dsuser.Tables(0).AsEnumerable().Select(Function(row) row.Field(Of String)("loginName")).ToList()
                                                                                       If res.Count > 0 Then
                                                                                           objItems.receivedBy = res(0)
                                                                                       Else
                                                                                           objItems.receivedBy = "None"
                                                                                       End If
                                                                                   End If
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try

                                                                           'strQry = "select * from ezwflowtransation where ( [Action] not in ( 'Initiate') and  [Action] not in ('Pending in Queue') and [Action] <> 'waiting for Documents') and processid=" + processid.ToString() + " " & transDtCond & " order by transactionid desc "

                                                                           strQry = "select * from ezwflowtransation where  [Action] ='Pending with TF Maker - L1' and processid=" + processid.ToString() + "  order by transactionid desc "

                                                                           Dim dsapprover As DataSet = GetDatasetByQuery(strQry)
                                                                           If dsapprover IsNot Nothing AndAlso dsapprover.Tables.Count > 0 AndAlso dsapprover.Tables(0).Rows.Count > 0 Then

                                                                               objItems.submittedToApproval = dsapprover.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                           Else
                                                                               'strQry = "select * from ezwflowtransation_Completed where ( [Action] not in ( 'Initiate') and  [Action] not in ('Pending in Queue') and [Action] <> 'waiting for Documents') and processid=" + processid.ToString() + " " & transDtCond & " order by transactionid desc "

                                                                               strQry = "select * from ezwflowtransation_Completed where  [Action] ='Pending with TF Maker - L1' and processid=" + processid.ToString() + "  order by transactionid desc "

                                                                               Dim ds_approvecomp As DataSet = GetDatasetByQuery(strQry)
                                                                               If ds_approvecomp IsNot Nothing AndAlso ds_approvecomp.Tables.Count > 0 AndAlso ds_approvecomp.Tables(0).Rows.Count > 0 Then
                                                                                   '  objItems.receivedBy = ds_approvecomp.Tables(0).Rows(0)("Action").ToString()
                                                                                   objItems.submittedToApproval = ds_approvecomp.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try
                                                                           strQry = "select CreatedOn,UpdatedOn from ezwflowtransation_completed where processid=" + processid.ToString() + " and review = 'Approve to Close' and isdeleted=0  order by transactionid desc"
                                                                           Dim dscompleted As DataSet = GetDatasetByQuery(strQry)
                                                                           If dscompleted IsNot Nothing AndAlso dscompleted.Tables.Count > 0 AndAlso dscompleted.Tables(0).Rows.Count > 0 Then
                                                                               objItems.completedBy = dscompleted.Tables(0).Rows(0)("UpdatedOn").ToString()

                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try
                                                                       Dim scancompletedby As String = ""
                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where processid=" + processid.ToString() + " and action like '%Pending in Queue%' and transactionstatus=1 "
                                                                           Dim dsscancompleted As DataSet = GetDatasetByQuery(strQry)
                                                                           If dsscancompleted IsNot Nothing AndAlso dsscancompleted.Tables.Count > 0 AndAlso dsscancompleted.Tables(0).Rows.Count > 0 Then
                                                                               scancompletedby = dsscancompleted.Tables(0).Rows(0)("UpdatedOn").ToString()
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try
                                                                       'Time calculation for scantocomplete
                                                                       Try
                                                                           If objItems.scanDateandTime <> "" And scancompletedby <> "" Then
                                                                               Dim scannedDate As DateTime = DateTime.ParseExact(objItems.scanDateandTime, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                               Dim completedDate As DateTime = DateTime.ParseExact(scancompletedby, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                               Dim difference As TimeSpan = completedDate - scannedDate
                                                                               If difference.Seconds > 0 Then
                                                                                   objItems.totalTimeScanToComplete = difference.Seconds.ToString() & " secs"
                                                                               End If
                                                                               If difference.Minutes > 0 Then
                                                                                   objItems.totalTimeScanToComplete = difference.Minutes.ToString() & " Mins"
                                                                               End If
                                                                               If difference.Hours > 0 Then
                                                                                   objItems.totalTimeScanToComplete = difference.Hours.ToString() & " hours"
                                                                               End If
                                                                               If difference.Days > 0 Then
                                                                                   objItems.totalTimeScanToComplete = difference.Days.ToString() & " days"
                                                                               End If
                                                                           Else
                                                                               objItems.totalTimeScanToComplete = "None"
                                                                           End If
                                                                       Catch ex As Exception
                                                                           objItems.totalTimeScanToComplete = ex.Message.ToString()
                                                                       End Try
                                                                       'Time calcuation of claimedon to submittoAproval
                                                                       Try
                                                                           If objItems.claimedOn <> "" And objItems.submittedToApproval <> "" Then
                                                                               Dim scannedDate As DateTime = DateTime.ParseExact(objItems.claimedOn, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                               Dim completedDate As DateTime = DateTime.ParseExact(objItems.submittedToApproval, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                               Dim difference As TimeSpan = completedDate - scannedDate
                                                                               If difference.Seconds > 0 Then
                                                                                   objItems.totalTimeClaimToApproval = difference.Seconds.ToString() & " secs"
                                                                               End If
                                                                               If difference.Minutes > 0 Then
                                                                                   objItems.totalTimeClaimToApproval = difference.Minutes.ToString() & " Mins"
                                                                               End If
                                                                               If difference.Hours > 0 Then
                                                                                   objItems.totalTimeClaimToApproval = difference.Hours.ToString() & " hours"
                                                                               End If
                                                                               If difference.Days > 0 Then
                                                                                   objItems.totalTimeClaimToApproval = difference.Days.ToString() & " days"
                                                                               End If
                                                                           Else
                                                                               objItems.submittedToApproval = "None"
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try
                                                                       'Time calculation of receiving ticket to Approval
                                                                       Try
                                                                           Dim createdDate As String = objItems.submittedToApproval
                                                                           Dim updatedDate As String = ""
                                                                           strQry = "select top 1  CreatedOn,UpdatedOn from ezwflowtransation where processid=" & processid.ToString() & " and isdeleted=0 and ( [Action] not in ( 'Initiate') and  [Action] <> 'Pending in Queue' and [Action] <> 'waiting for Documents') order by transactionid desc  "
                                                                           Dim ds_received As DataSet = GetDatasetByQuery(strQry)
                                                                           If ds_received IsNot Nothing AndAlso ds_received.Tables.Count > 0 AndAlso ds_received.Tables(0).Rows.Count > 0 Then
                                                                               ' createdDate = ds_received.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                               updatedDate = ds_received.Tables(0).Rows(0)("UpdatedOn").ToString()
                                                                               If updatedDate = "" Then
                                                                                   updatedDate = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt")
                                                                               End If
                                                                           Else
                                                                               strQry = "select  CreatedOn,UpdatedOn from ezwflowtransation_completed where processid=" & processid.ToString() & " and isdeleted=0 and [Review] ='Approve to Close'"
                                                                               Dim ds_received_comp As DataSet = GetDatasetByQuery(strQry)
                                                                               If ds_received_comp IsNot Nothing AndAlso ds_received_comp.Tables.Count > 0 AndAlso ds_received_comp.Tables(0).Rows.Count > 0 Then
                                                                                   'createdDate = ds_received_comp.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                                   updatedDate = ds_received_comp.Tables(0).Rows(0)("UpdatedOn").ToString()
                                                                               End If
                                                                           End If
                                                                           If createdDate <> "" And updatedDate <> "" Then
                                                                               Dim scannedDate As DateTime = DateTime.ParseExact(createdDate, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                               Dim completedDate As DateTime = DateTime.ParseExact(updatedDate, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                               Dim difference As TimeSpan = completedDate - scannedDate
                                                                               If difference.Seconds > 0 Then
                                                                                   objItems.totalTimeReceivingTicketToApproval = difference.Seconds.ToString() & " secs"
                                                                               End If
                                                                               If difference.Minutes > 0 Then
                                                                                   objItems.totalTimeReceivingTicketToApproval = difference.Minutes.ToString() & " Mins"
                                                                               End If
                                                                               If difference.Hours > 0 Then
                                                                                   objItems.totalTimeReceivingTicketToApproval = difference.Hours.ToString() & " hours"
                                                                               End If
                                                                               If difference.Days > 0 Then
                                                                                   objItems.totalTimeReceivingTicketToApproval = difference.Days.ToString() & " days"
                                                                               End If
                                                                           Else
                                                                               objItems.totalTimeReceivingTicketToApproval = "None"
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try


                                                                       Return objItems
                                                                   End Function).ToList()
                    lstItems = datas
                    Try
                        result.totalRow = lstItems.Count
                        If CondtionReg = "" AndAlso transDtCond = "" Then
                            strQry = "select count(*) as totcnt from ezwfprocess where workflowid=5 and isdeleted=0"
                            Dim ds_tot As DataSet = GetDatasetByQuery(strQry)
                            If ds_tot IsNot Nothing AndAlso ds_tot.Tables.Count > 0 AndAlso ds_tot.Tables(0).Rows.Count > 0 Then
                                result.totalRow = CInt(ds_tot.Tables(0).Rows(0)("totcnt").ToString())
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                    result.data = lstItems
                    response = Request.CreateResponse(HttpStatusCode.OK, result)
                Else
                    response = Request.CreateErrorResponse(HttpStatusCode.OK, "No record found")
                End If


            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function AvailamentTicketsReport(para As SearchRegistries) As HttpResponseMessage
            Dim strQry As String = "", rowQry As String = ""
            Dim response As HttpResponseMessage
            Dim result As New AvailmentTicketReport
            Dim lstItems As New List(Of AvailmentTicketReportA)
            Dim transDtCond As String = ""
            Dim mainqrycondMakerchecker As String = ""
            Try
                Dim CondtionReg As String = "", ATticketNoCond = "", ATticketRaisedOncond = ""
                Dim Tablename = ""
                Dim isEntered As Boolean = False
                For Each cond In para.Criteria
                    'If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                    '    cond.Criteria = "wf.[createdBy]"
                    'ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                    '    cond.Criteria = "wf.[createdon]"
                    'ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                    '    cond.Criteria = "BTH.[createdon]"
                    'End If
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
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                                CondtionReg = CondtionReg + " and wf.[createdby] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                                CondtionReg = CondtionReg + " and [RIM Number] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                                CondtionReg = CondtionReg + " and [Account Number] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                                transDtCond = " and action like '%maker%' and ( ActivityUserId in (" + Inval + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + Inval + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + ")))) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                                transDtCond = " and action like '%checker%' and ( ActivityUserId in (" + Inval + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + Inval + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + ")))) "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in (" + Inval + ") "
                            End If
                        Else
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                                CondtionReg = CondtionReg + " and wf.[createdby] in ('" + cond.Value1 + "') "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                                CondtionReg = CondtionReg + " and [RIM Number] in (" + cond.Value1 + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                                CondtionReg = CondtionReg + " and [Account Number] in (" + cond.Value1 + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                                transDtCond = " and action like '%maker%' and ( ActivityUserId in (" + cond.Value1 + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + cond.Value1 + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                                transDtCond = " and action like '%checker%' and ( ActivityUserId in (" + cond.Value1 + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + cond.Value1 + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in ('" + cond.Value1 + "') "
                            End If

                        End If
                    ElseIf cond.DataTypeId = "4" Then
                        If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                            CondtionReg = CondtionReg + " and wf.[createdby] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                            CondtionReg = CondtionReg + " and [RIM Number] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                            CondtionReg = CondtionReg + " and [Account Number] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "atrequestnumber" Then
                            ATticketNoCond = " and [Ticket Number] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                            transDtCond = " and action like '%maker%' and ( ActivityUserId like %" + cond.Value1 + "% or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId like %" + cond.Value1 + "%)) "
                            mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                            transDtCond = " and action like '%checker%' and ( ActivityUserId like %" + cond.Value1 + "% or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId like %" + cond.Value1 + "%)) "
                            mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                        Else
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                        End If
                    ElseIf cond.DataTypeId = "5" Then
                        If cond.Value1 <> "" And cond.Value2 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + " and wf.[createdon] <> '' and  convert(datetime,wf.[createdon],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + " and BTH.[createdon] <> '' and  convert(datetime,BTH.[createdon],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "atraisedon" Then
                                ATticketRaisedOncond = " and [createdon] <> '' and  convert(datetime,[createdon],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                'Else

                                '    CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            End If

                        ElseIf cond.Value1 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + " and wf.[createdon] <> '' and convert(datetime,wf.[createdon],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + " and BTH.[createdon] <> '' and convert(datetime,BTH.[createdon],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "atraisedon" Then
                                ATticketRaisedOncond = " and [createdon] <> '' and  convert(datetime,[createdon],101) >= convert(datetime,'" + cond.Value1 + " 00:00:00',101) "
                            End If

                        ElseIf cond.Value2 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + "  and convert(datetime,wf.[createdon],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + "  and convert(datetime,BTH.[createdon],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "atraisedon" Then
                                ATticketRaisedOncond = " and [createdon] <> '' and  convert(datetime,[createdon],101) <= convert(datetime,'" + cond.Value2 + " 00:00:00',101) "
                            End If
                        End If
                    End If
                Next
                If para.RowCount <> 0 Then
                    rowQry = "  OFFSET " + para.RowFrom.ToString() + " ROWS FETCH NEXT " + para.RowCount.ToString() + " ROWS ONLY"
                End If
                strQry = "SELECT  wf.processid,wf.workflowId, wf.requestNo,flowstatus,format(convert(datetime,BTH.CreatedOn,113),'dd-MMM-yyyy hh:mm:ss tt') AS 'scannedat',Bth.FirstName as 'scannedby',case when (ezt.[Eximbills Reference] is null or ezt.[Eximbills Reference]='')  then 'NONE' else ezt.[Eximbills Reference] end  as [Transaction reference],[RIM Number] AS RIM,[Account Number] as AccountNO ,Product,   Phase,[RIM Number],isnull(Type,'None') Type ,'' as Stage,wf.createdon AS 'claimed On',Bth.FirstName AS 'claimed By','' AS 'Submitted to Approval','' AS 'Received by' FROM  ezwfprocess wf LEFT JOIN (select distinct [Work Item Reference],isdeleted ,[RIM Number],[Account Number],Product,Phase,Type,[Eximbills Module]  from  ezca_3_15_items) itm  ON itm.[Work Item Reference] = wf.requestNo left join (select distinct [work item reference] ,Bt.createdOn,dbo.udf_firstName(BT.createdby)as FirstName  from  BBK_TicketQueue  BT)BTH on BTH.[Work Item Reference]=itm.[Work Item Reference]  left join (select [Eximbills Reference],[Work Item Reference] from [ezfb_Trade Finance Form]) ezt on ezt.[Work Item Reference]=wf.RequestNo  WHERE itm.isdeleted = 0 and wf.WorkflowId=5 and wf.isdeleted=0 and wf.processid in (select processid from BBK_AvailmentTicket where isdeleted=0 " + ATticketNoCond + " " + ATticketRaisedOncond + ") and wf.RequestNo  is not null " & CondtionReg & " " & mainqrycondMakerchecker & " order by wf.createdon desc " & rowQry
                Dim ds As DataSet = GetDatasetByQuery(strQry)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    Dim datas = ds.Tables(0).AsEnumerable().Select(Function(objRead)
                                                                       Dim objItems = New AvailmentTicketReportA With {
                    .processId = objRead.Field(Of Int32)("processid"),
                    .workflowId = objRead.Field(Of Int32)("workflowId"),
                    .requestNo = objRead.Field(Of String)("requestNo"),
                    .rim = objRead.Field(Of String)("RIM"),
                    .transactionReference = objRead.Field(Of String)("Transaction reference"),
                    .scannedBy = objRead.Field(Of String)("scannedby"),
                    .scanDateandTime = objRead.Field(Of String)("scannedat"),
                    .accountNo = objRead.Field(Of String)("AccountNo"),
                    .product = objRead.Field(Of String)("Product"),
                    .phase = objRead.Field(Of String)("Phase"),
                    .type = objRead.Field(Of String)("Type"),
                    .claimedOn = objRead.Field(Of String)("claimed On"),
                    .claimedBy = objRead.Field(Of String)("claimed By")
                    }
                                                                       Dim processid = objRead.Field(Of Int32)("processid")
                                                                       Try

                                                                           'strQry = "select * from ezwflowtransation where ( [Action] not in ( 'Initiate') and  [Action] <> 'Pending in Queue' and [Action] <> 'waiting for Documents') and processid=" + processid.ToString() + " " + transDtCond + " order by transactionid desc "

                                                                           strQry = "select ActivityId, CreatedOn from ezwflowtransation where processid=" + processid.ToString() + "  and isdeleted=0  order by transactionid desc"

                                                                           Dim dsapprover As DataSet = GetDatasetByQuery(strQry)
                                                                           If dsapprover IsNot Nothing AndAlso dsapprover.Tables.Count > 0 AndAlso dsapprover.Tables(0).Rows.Count > 0 Then
                                                                               'objItems.receivedBy = dsapprover.Tables(0).Rows(0)("Action").ToString()
                                                                               ' objItems.submittedToApproval = dsapprover.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                               objItems.activityId = dsapprover.Tables(0).Rows(0)("ActivityId").ToString()
                                                                               objItems.currentlyReceivingTime = dsapprover.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                           Else
                                                                               'strQry = "select * from ezwflowtransation_Completed  where ( [Action] not in ( 'Initiate') and  [Action] <> 'Pending in Queue' and [Action] <> 'waiting for Documents') and processid=" + processid.ToString() + " " + transDtCond + " order by transactionid desc "
                                                                               strQry = "select * from ezwflowtransation_Completed where processid=" + processid.ToString() + "  and isdeleted=0  order by transactionid desc"
                                                                               Dim dsapprover_comp As DataSet = GetDatasetByQuery(strQry)
                                                                               If dsapprover_comp IsNot Nothing AndAlso dsapprover_comp.Tables.Count > 0 AndAlso dsapprover_comp.Tables(0).Rows.Count > 0 Then
                                                                                   ' objItems.receivedBy = dsapprover_comp.Tables(0).Rows(0)("Action").ToString()
                                                                                   ' objItems.submittedToApproval = dsapprover_comp.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                                   objItems.activityId = dsapprover_comp.Tables(0).Rows(0)("ActivityId").ToString()
                                                                                   objItems.currentlyReceivingTime = dsapprover.Tables(0).Rows(0)("Updatedon").ToString()
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where  [Action] ='Pending with TF Maker - L1' and processid=" + processid.ToString() + "  order by transactionid desc "
                                                                           Dim dsapprover As DataSet = GetDatasetByQuery(strQry)
                                                                           If dsapprover IsNot Nothing AndAlso dsapprover.Tables.Count > 0 AndAlso dsapprover.Tables(0).Rows.Count > 0 Then
                                                                               objItems.submittedToApproval = dsapprover.Tables(0).Rows(0)("Createdon").ToString()
                                                                           Else
                                                                               strQry = "select * from ezwflowtransation_completed where  [Action] ='Pending with TF Maker - L1' and processid=" + processid.ToString() + "  order by transactionid desc "
                                                                               Dim dsapprover_comp As DataSet = GetDatasetByQuery(strQry)
                                                                               If dsapprover_comp IsNot Nothing AndAlso dsapprover_comp.Tables.Count > 0 AndAlso dsapprover_comp.Tables(0).Rows.Count > 0 Then
                                                                                   objItems.submittedToApproval = dsapprover_comp.Tables(0).Rows(0)("Createdon").ToString()
                                                                               Else
                                                                                   objItems.submittedToApproval = "None"
                                                                               End If
                                                                           End If

                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where processid=" + processid.ToString() + " and Action in ('Pending in Queue','waiting for documents') and transactionstatus=0"
                                                                           Dim dstrans As DataSet = GetDatasetByQuery(strQry)
                                                                           If dstrans IsNot Nothing AndAlso dstrans.Tables.Count > 0 AndAlso dstrans.Tables(0).Rows.Count > 0 Then
                                                                               objItems.receivedBy = "None"
                                                                           Else
                                                                               strQry = "select distinct loginName from ezecmlogin where isdeleted=0 and  ecmloginid in (select activityuserid from ezwflowtransation where transactionstatus=0 and processid=" + processid.ToString() + " and isdeleted=0 UNION select ezg.ECMloginid from  ezecmgroupusers ezg JOIN ezwflowtransation ezt on ezg.ecmgroupid = ezt.ActivityGroupId where transactionstatus=0 and processid=" + processid.ToString() + " and ezt.isdeleted=0 and ezg.isdeleted=0)"

                                                                               Dim dsuser As DataSet = GetDatasetByQuery(strQry)
                                                                               If dsuser IsNot Nothing AndAlso dsuser.Tables.Count > 0 AndAlso dsuser.Tables(0).Rows.Count > 0 Then
                                                                                   Dim res As List(Of String) = dsuser.Tables(0).AsEnumerable().Select(Function(row) row.Field(Of String)("loginName")).ToList()
                                                                                   If res.Count > 0 Then
                                                                                       objItems.receivedBy = res(0)
                                                                                   End If
                                                                               Else
                                                                                   strQry = "select distinct loginName from ezecmlogin where isdeleted=0 and  ecmloginid in (select activityuserid from eZWFlowTransation_Completed where transactionstatus=1 and processid=" + processid.ToString() + " and isdeleted=0 and review='Approve to Close' UNION select ezg.ECMloginid from  ezecmgroupusers ezg JOIN eZWFlowTransation_Completed ezt on ezg.ecmgroupid = ezt.ActivityGroupId where transactionstatus=1 and processid=" + processid.ToString() + " and ezt.isdeleted=0 and ezg.isdeleted=0 and ezt.review='Approve to Close')"
                                                                                   Dim ds_transcompl As DataSet = GetDatasetByQuery(strQry)
                                                                                   If ds_transcompl IsNot Nothing AndAlso ds_transcompl.Tables.Count > 0 AndAlso ds_transcompl.Tables(0).Rows.Count > 0 Then
                                                                                       Dim res As List(Of String) = dsuser.Tables(0).AsEnumerable().Select(Function(row) row.Field(Of String)("loginName")).ToList()
                                                                                       If res.Count > 0 Then
                                                                                           objItems.receivedBy = res(0)
                                                                                       Else
                                                                                           objItems.receivedBy = "None"
                                                                                       End If
                                                                                   End If
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try
                                                                       'Submitted for RM approval
                                                                       Try

                                                                           strQry = "select createdOn,UpdatedOn from ezwflowtransation where ( [Action] ='Pending with Relationship Manager') and processid=" + processid.ToString() + " and isdeleted=0  order by transactionid desc "

                                                                           Dim dsapprover As DataSet = GetDatasetByQuery(strQry)
                                                                           If dsapprover IsNot Nothing AndAlso dsapprover.Tables.Count > 0 AndAlso dsapprover.Tables(0).Rows.Count > 0 Then
                                                                               objItems.submittedForRMApproval = dsapprover.Tables(0).Rows(0)("createdOn").ToString()
                                                                               objItems.approvedAtReceived = dsapprover.Tables(0).Rows(0)("UpdatedOn").ToString()
                                                                           Else
                                                                               strQry = "select createdOn,UpdatedOn from ezwflowtransation_completed where ( [Action] ='Pending with Relationship Manager') and processid=" + processid.ToString() + " and isdeleted=0  order by transactionid desc "
                                                                               Dim dsapprover_comp As DataSet = GetDatasetByQuery(strQry)
                                                                               If dsapprover_comp IsNot Nothing AndAlso dsapprover_comp.Tables.Count > 0 AndAlso dsapprover_comp.Tables(0).Rows.Count > 0 Then
                                                                                   objItems.submittedForRMApproval = dsapprover_comp.Tables(0).Rows(0)("createdOn").ToString()
                                                                                   objItems.approvedAtReceived = dsapprover_comp.Tables(0).Rows(0)("UpdatedOn").ToString()
                                                                               Else
                                                                                   objItems.submittedForRMApproval = "None"
                                                                                   objItems.approvedAtReceived = "None"
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       'Time calculation for submitto approve 
                                                                       Try
                                                                           Dim isrejected As Boolean = False
                                                                           Dim submittedOn As String = ""
                                                                           Dim approvedOn As String = ""
                                                                           strQry = "select * from ezwflowtransation where review like '%reject%' and processid=" + processid.ToString() + "  "
                                                                           Dim dsapprover As DataSet = GetDatasetByQuery(strQry)
                                                                           If dsapprover IsNot Nothing AndAlso dsapprover.Tables.Count > 0 AndAlso dsapprover.Tables(0).Rows.Count > 0 Then
                                                                               isrejected = True
                                                                           Else
                                                                               strQry = "select * from ezwflowtransation_completed where review like '%reject%' and processid=" + processid.ToString() + "  order by transactionid desc "
                                                                               Dim dsapprover_comp As DataSet = GetDatasetByQuery(strQry)
                                                                               If dsapprover_comp IsNot Nothing AndAlso dsapprover_comp.Tables.Count > 0 AndAlso dsapprover_comp.Tables(0).Rows.Count > 0 Then
                                                                                   isrejected = True
                                                                               End If
                                                                           End If
                                                                           If isrejected = False Then
                                                                               submittedOn = objItems.submittedForRMApproval
                                                                               approvedOn = objItems.approvedAtReceived
                                                                           End If
                                                                           If submittedOn <> "" And approvedOn <> "" Then
                                                                               Dim submitDate As DateTime = DateTime.ParseExact(submittedOn, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                               Dim approveDate As DateTime = DateTime.ParseExact(approvedOn, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                               Dim difference As TimeSpan = approveDate - submitDate
                                                                               If difference.Seconds > 0 Then
                                                                                   objItems.totalTimeFromSubmitingATUntilApproved = difference.Seconds.ToString() & " secs"
                                                                               End If
                                                                               If difference.Minutes > 0 Then
                                                                                   objItems.totalTimeFromSubmitingATUntilApproved = difference.Minutes.ToString() & " Mins"
                                                                               End If
                                                                               If difference.Hours > 0 Then
                                                                                   objItems.totalTimeFromSubmitingATUntilApproved = difference.Hours.ToString() & " hours"
                                                                               End If
                                                                               If difference.Days > 0 Then
                                                                                   objItems.totalTimeFromSubmitingATUntilApproved = difference.Days.ToString() & " days"
                                                                               End If
                                                                           Else
                                                                               objItems.totalTimeFromSubmitingATUntilApproved = "None"
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try


                                                                       Try
                                                                           strQry = "select * from eZECMUserInfo EF inner join eZWFlowTransation Ez on EF.ECMLoginId=ez.Updatedby  where [action]='Pending with Relationship Manager' and processid=" & processid.ToString() & ""
                                                                           Dim ds_comment As DataSet = GetDatasetByQuery(strQry)
                                                                           If ds_comment IsNot Nothing AndAlso ds_comment.Tables.Count > 0 AndAlso ds_comment.Tables(0).Rows.Count > 0 Then
                                                                               objItems.comment = ds_comment.Tables(0).Rows(0)("FirstName").ToString()
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try
                                                                       Return objItems
                                                                   End Function).ToList()
                    lstItems = datas
                    Try
                        result.totalRow = lstItems.Count
                        If CondtionReg = "" AndAlso transDtCond = "" Then
                            strQry = "select count(*) as totcnt from ezwfprocess where workflowid=5 and isdeleted=0"
                            Dim ds_tot As DataSet = GetDatasetByQuery(strQry)
                            If ds_tot IsNot Nothing AndAlso ds_tot.Tables.Count > 0 AndAlso ds_tot.Tables(0).Rows.Count > 0 Then
                                result.totalRow = CInt(ds_tot.Tables(0).Rows(0)("totcnt").ToString())
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                    result.data = lstItems
                    response = Request.CreateResponse(HttpStatusCode.OK, result)
                Else
                    response = Request.CreateErrorResponse(HttpStatusCode.OK, "No records found")
                End If
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message.ToString())
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function RejectionsReport(para As SearchRegistries) As HttpResponseMessage
            Dim strQry As String = "", rowQry As String = ""
            Dim response As HttpResponseMessage
            Dim result As New RejectionReport
            Dim lstItems As New List(Of RejectionReportA)
            Dim transDtCond As String = ""
            Dim mainqrycondMakerchecker As String = ""
            Try
                Dim CondtionReg As String = ""
                Dim Tablename = ""
                For Each cond In para.Criteria
                    'If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                    '    cond.Criteria = "wf.createdBy"
                    'ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                    '    cond.Criteria = "wf.createdon"
                    'ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                    '    cond.Criteria = "BTH.createdon"
                    'End If
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
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                                CondtionReg = CondtionReg + " and wf.[createdby] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                                CondtionReg = CondtionReg + " and [RIM Number] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                                CondtionReg = CondtionReg + " and [Account Number] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                                transDtCond = " and action like '%maker%' and ( ActivityUserId in (" + Inval + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + Inval + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + ")))) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                                transDtCond = " and action like '%checker%' and ( ActivityUserId in (" + Inval + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + Inval + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + ")))) "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in (" + Inval + ") "
                            End If
                        Else
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                                CondtionReg = CondtionReg + " and wf.[createdby] in ('" + cond.Value1 + "') "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                                CondtionReg = CondtionReg + " and [RIM Number] in (" + cond.Value1 + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                                CondtionReg = CondtionReg + " and [Account Number] in (" + cond.Value1 + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                                transDtCond = " and action like '%maker%' and ( ActivityUserId in (" + cond.Value1 + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + cond.Value1 + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                                transDtCond = " and action like '%checker%' and ( ActivityUserId in (" + cond.Value1 + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + cond.Value1 + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in ('" + cond.Value1 + "') "
                            End If

                        End If
                    ElseIf cond.DataTypeId = "4" Then
                        If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                            CondtionReg = CondtionReg + " and wf.[createdby] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                            CondtionReg = CondtionReg + " and [RIM Number] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                            CondtionReg = CondtionReg + " and [Account Number] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                            transDtCond = " and action like '%maker%' and ( ActivityUserId like %" + cond.Value1 + "% or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId like %" + cond.Value1 + "%)) "
                            mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                            transDtCond = " and action like '%checker%' and ( ActivityUserId like %" + cond.Value1 + "% or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId like %" + cond.Value1 + "%)) "
                            mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                        Else
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                        End If
                    ElseIf cond.DataTypeId = "5" Then
                        If cond.Value1 <> "" And cond.Value2 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + " and wf.[createdon] <> '' and  convert(datetime,wf.[createdon],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + " and BTH.[createdon] <> '' and  convert(datetime,BTH.[createdon],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                'Else

                                '    CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            End If

                        ElseIf cond.Value1 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + " and wf.[createdon] <> '' and convert(datetime,wf.[createdon],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + " and BTH.[createdon] <> '' and convert(datetime,BTH.[createdon],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                            End If

                        ElseIf cond.Value2 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + "  and convert(datetime,wf.[createdon],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + "  and convert(datetime,BTH.[createdon],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                            End If
                        End If
                    End If
                Next
                If para.RowCount <> 0 Then
                    rowQry = "  OFFSET " + para.RowFrom.ToString() + " ROWS FETCH NEXT " + para.RowCount.ToString() + " ROWS ONLY"
                End If
                Dim rejectcond As String = " and wf.processid in (select Processid from eZWFlowTransation where isdeleted=0 and review like '%reject%' UNION select processid from ezwflowTransation_completed where isdeleted=0 and review like '%reject%')"
                strQry = "SELECT  wf.processid,wf.workflowId, wf.requestNo,flowstatus,format(convert(datetime,BTH.CreatedOn,113),'dd-MMM-yyyy hh:mm:ss tt') AS 'scannedat',Bth.FirstName as 'scannedby',case when (ezt.[Eximbills Reference] is null or ezt.[Eximbills Reference]='')  then 'NONE' else ezt.[Eximbills Reference] end  as [Transaction reference],[RIM Number] AS RIM,[Account Number] as AccountNO ,Product,   Phase,[RIM Number],'' as Stage,wf.createdon AS 'claimed On',Bth.FirstName AS 'claimed By','' AS 'Submitted to Approval','' AS 'Received by',isnull(Type,'None') Type  FROM  ezwfprocess wf LEFT JOIN (select distinct [Work Item Reference],isdeleted ,[RIM Number],[Account Number],Product,Phase,Type,[Eximbills Module]  from  ezca_3_15_items) itm  ON itm.[Work Item Reference] = wf.requestNo  left join (select distinct [work item reference] ,Bt.createdOn,dbo.udf_Firstname(BT.createdBy) as FirstName  from  BBK_TicketQueue  BT  )BTH on BTH.[Work Item Reference]=itm.[Work Item Reference] left join (select [Eximbills Reference],[Work Item Reference] from [ezfb_Trade Finance Form]) ezt on ezt.[Work Item Reference]=wf.RequestNo   WHERE itm.isdeleted = 0 and wf.WorkflowId=5 and wf.isdeleted=0 and wf.RequestNo  is not null " & CondtionReg & " " & mainqrycondMakerchecker & " " & rejectcond & " order by wf.createdon desc " & rowQry
                Dim ds As DataSet = GetDatasetByQuery(strQry)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    Dim datas = ds.Tables(0).AsEnumerable().Select(Function(objRead)
                                                                       Dim objItems = New RejectionReportA With {
                   .processId = objRead.Field(Of Int32)("processid"),
                    .workflowId = objRead.Field(Of Int32)("workflowId"),
                    .requestNo = objRead.Field(Of String)("requestNo"),
                     .transactionReference = objRead.Field(Of String)("Transaction reference"),
                    .rim = objRead.Field(Of String)("RIM"),
                    .type = objRead.Field(Of String)("Type"),
                    .scannedBy = objRead.Field(Of String)("scannedby"),
                    .scanDateandTime = objRead.Field(Of String)("scannedat"),
                    .accountNo = objRead.Field(Of String)("AccountNo"),
                    .product = objRead.Field(Of String)("Product"),
                    .phase = objRead.Field(Of String)("Phase"),
                    .claimedOn = objRead.Field(Of String)("claimed On"),
                    .claimedBy = objRead.Field(Of String)("claimed By")
                    }
                                                                       Dim processid = objRead.Field(Of Int32)("processid")
                                                                       Try

                                                                           'strQry = "select * from ezwflowtransation where ( [Action] not in ( 'Initiate') and  [Action] <> 'Pending in Queue' and [Action] <> 'waiting for Documents') and processid=" + processid.ToString() + " " + transDtCond + "  order by transactionid desc "
                                                                           strQry = "select * from ezwflowtransation where processid=" + processid.ToString() + "  and isdeleted=0  order by transactionid desc"
                                                                           Dim dsapprover As DataSet = GetDatasetByQuery(strQry)
                                                                           If dsapprover IsNot Nothing AndAlso dsapprover.Tables.Count > 0 AndAlso dsapprover.Tables(0).Rows.Count > 0 Then
                                                                               'objItems.stage = dsapprover.Tables(0).Rows(0)("Action").ToString()
                                                                               'objItems.submittedToApproval = dsapprover.Tables(0).Rows(0)("createdOn").ToString()
                                                                               objItems.activityId = dsapprover.Tables(0).Rows(0)("ActivityId").ToString()
                                                                               objItems.currentlyReceivingTime = dsapprover.Tables(0).Rows(0)("Createdon").ToString()
                                                                           Else
                                                                               'strQry = "select * from ezwflowtransation_completed where ( [Action] not in ( 'Initiate') and  [Action] <> 'Pending in Queue' and [Action] <> 'waiting for Documents') and processid=" + processid.ToString() + " " + transDtCond + "  order by transactionid desc "
                                                                               strQry = "select * from ezwflowtransation_completed where processid=" + processid.ToString() + "  and isdeleted=0  order by transactionid desc"
                                                                               Dim dsapprover_comp As DataSet = GetDatasetByQuery(strQry)
                                                                               If dsapprover_comp IsNot Nothing AndAlso dsapprover_comp.Tables.Count > 0 AndAlso dsapprover_comp.Tables(0).Rows.Count > 0 Then
                                                                                   'objItems.receivedBy = dsapprover_comp.Tables(0).Rows(0)("Action").ToString()
                                                                                   'objItems.submittedToApproval = dsapprover_comp.Tables(0).Rows(0)("createdOn").ToString()
                                                                                   objItems.activityId = dsapprover_comp.Tables(0).Rows(0)("ActivityId").ToString()
                                                                                   objItems.currentlyReceivingTime = dsapprover.Tables(0).Rows(0)("Updatedon").ToString()
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where processid=" + processid.ToString() + " and Action in ('Pending in Queue','waiting for documents') and transactionstatus=0"
                                                                           Dim dstrans As DataSet = GetDatasetByQuery(strQry)
                                                                           If dstrans IsNot Nothing AndAlso dstrans.Tables.Count > 0 AndAlso dstrans.Tables(0).Rows.Count > 0 Then
                                                                               objItems.receivedBy = "None"
                                                                           Else
                                                                               strQry = "select distinct loginName from ezecmlogin where isdeleted=0 and  ecmloginid in (select activityuserid from ezwflowtransation where transactionstatus=0 and processid=" + processid.ToString() + " and isdeleted=0 UNION select ezg.ECMloginid from  ezecmgroupusers ezg JOIN ezwflowtransation ezt on ezg.ecmgroupid = ezt.ActivityGroupId where transactionstatus=0 and processid=" + processid.ToString() + " and ezt.isdeleted=0 and ezg.isdeleted=0)"

                                                                               Dim dsuser As DataSet = GetDatasetByQuery(strQry)
                                                                               If dsuser IsNot Nothing AndAlso dsuser.Tables.Count > 0 AndAlso dsuser.Tables(0).Rows.Count > 0 Then
                                                                                   Dim res As List(Of String) = dsuser.Tables(0).AsEnumerable().Select(Function(row) row.Field(Of String)("loginName")).ToList()
                                                                                   If res.Count > 0 Then
                                                                                       objItems.receivedBy = res(0)
                                                                                   End If
                                                                               Else
                                                                                   strQry = "select distinct loginName from ezecmlogin where isdeleted=0 and  ecmloginid in (select activityuserid from eZWFlowTransation_Completed where transactionstatus=1 and processid=" + processid.ToString() + " and isdeleted=0 and review='Approve to Close' UNION select ezg.ECMloginid from  ezecmgroupusers ezg JOIN eZWFlowTransation_Completed ezt on ezg.ecmgroupid = ezt.ActivityGroupId where transactionstatus=1 and processid=" + processid.ToString() + " and ezt.isdeleted=0 and ezg.isdeleted=0 and ezt.review='Approve to Close')"
                                                                                   Dim ds_transcompl As DataSet = GetDatasetByQuery(strQry)
                                                                                   If ds_transcompl IsNot Nothing AndAlso ds_transcompl.Tables.Count > 0 AndAlso ds_transcompl.Tables(0).Rows.Count > 0 Then
                                                                                       Dim res As List(Of String) = dsuser.Tables(0).AsEnumerable().Select(Function(row) row.Field(Of String)("loginName")).ToList()
                                                                                       If res.Count > 0 Then
                                                                                           objItems.receivedBy = res(0)
                                                                                       Else
                                                                                           objItems.receivedBy = "None"
                                                                                       End If
                                                                                   End If
                                                                               End If
                                                                           End If


                                                                       Catch ex As Exception

                                                                       End Try
                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where [Action] like'Pending with TF Maker%' and processid=" + processid.ToString() + "  and isdeleted=0 order by transactionid desc "
                                                                           Dim ds_approveafterrej = GetDatasetByQuery(strQry)
                                                                           If ds_approveafterrej IsNot Nothing AndAlso ds_approveafterrej.Tables.Count > 0 AndAlso ds_approveafterrej.Tables(0).Rows.Count > 0 Then
                                                                               objItems.submittedForApproval = ds_approveafterrej.Tables(0).Rows(0)("createdOn").ToString()
                                                                           Else
                                                                               strQry = "select * from ezwflowtransation_completed where [Action] like'Pending with TF Maker%' and processid=" + processid.ToString() + " and isdeleted=0 order by transactionid desc "
                                                                               Dim ds_approveafterrejComp = GetDatasetByQuery(strQry)
                                                                               If ds_approveafterrejComp IsNot Nothing AndAlso ds_approveafterrejComp.Tables.Count > 0 AndAlso ds_approveafterrejComp.Tables(0).Rows.Count > 0 Then
                                                                                   objItems.submittedForApproval = ds_approveafterrejComp.Tables(0).Rows(0)("createdOn").ToString()
                                                                               Else
                                                                                   objItems.submittedForApproval = "None"
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try

                                                                           strQry = "select * from ezwflowtransation where  [Action] ='Pending with TF Maker - L1' and processid=" + processid.ToString() + "  order by transactionid desc "

                                                                           Dim dsapprover As DataSet = GetDatasetByQuery(strQry)
                                                                           If dsapprover IsNot Nothing AndAlso dsapprover.Tables.Count > 0 AndAlso dsapprover.Tables(0).Rows.Count > 0 Then

                                                                               objItems.submittedToApproval = dsapprover.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                           Else

                                                                               strQry = "select * from ezwflowtransation_Completed where  [Action] ='Pending with TF Maker - L1' and processid=" + processid.ToString() + "  order by transactionid desc "

                                                                               Dim ds_approvecomp As DataSet = GetDatasetByQuery(strQry)
                                                                               If ds_approvecomp IsNot Nothing AndAlso ds_approvecomp.Tables.Count > 0 AndAlso ds_approvecomp.Tables(0).Rows.Count > 0 Then
                                                                                   '  objItems.receivedBy = ds_approvecomp.Tables(0).Rows(0)("Action").ToString()
                                                                                   objItems.submittedToApproval = ds_approvecomp.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try
                                                                           strQry = "select * from ezwflowtransation_completed where processid=" + processid.ToString() + " and [review] ='Approve to Close' and isdeleted=0     order by transactionid desc"
                                                                           Dim dscompleted As DataSet = GetDatasetByQuery(strQry)
                                                                           If dscompleted IsNot Nothing AndAlso dscompleted.Tables.Count > 0 AndAlso dscompleted.Tables(0).Rows.Count > 0 Then
                                                                               objItems.completedBy = dscompleted.Tables(0).Rows(0)("createdOn").ToString()
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where processid=" + processid.ToString() + " and (action like '%Reject%' or review like '%Reject%') and isdeleted=0   order by transactionid desc"
                                                                           Dim dscompleted As DataSet = GetDatasetByQuery(strQry)
                                                                           If dscompleted IsNot Nothing AndAlso dscompleted.Tables.Count > 0 AndAlso dscompleted.Tables(0).Rows.Count > 0 Then
                                                                               objItems.rejectForCorrection = dscompleted.Tables(0).Rows(0)("createdOn").ToString()
                                                                               objItems.totalNumberOfRejections = dscompleted.Tables(0).Rows.Count.ToString()
                                                                           Else
                                                                               strQry = "select * from ezwflowtransation_completed where processid=" + processid.ToString() + " and (action like '%Reject%' or review like '%Reject%') and isdeleted=0   order by transactionid desc"
                                                                               Dim ds_cmplete As DataSet = GetDatasetByQuery(strQry)
                                                                               If ds_cmplete IsNot Nothing AndAlso ds_cmplete.Tables.Count > 0 AndAlso ds_cmplete.Tables(0).Rows.Count > 0 Then
                                                                                   objItems.rejectForCorrection = ds_cmplete.Tables(0).Rows(0)("createdOn").ToString()
                                                                                   objItems.totalNumberOfRejections = ds_cmplete.Tables(0).Rows.Count.ToString()
                                                                               Else
                                                                                   objItems.totalNumberOfRejections = "0"
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try
                                                                           strQry = "select top 1 Comments from ezcomments where processid=" & processid.ToString() & " order by commentsId desc "
                                                                           Dim ds_comment As DataSet = GetDatasetByQuery(strQry)
                                                                           If ds_comment IsNot Nothing AndAlso ds_comment.Tables.Count > 0 AndAlso ds_comment.Tables(0).Rows.Count > 0 Then
                                                                               objItems.comment = ds_comment.Tables(0).Rows(0)("Comments").ToString()
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try
                                                                       Return objItems
                                                                   End Function).ToList()
                    lstItems = datas
                    Try
                        result.totalRow = lstItems.Count
                        'If CondtionReg = "" AndAlso transDtCond = "" Then
                        '    strQry = "select count(*) as totcnt from ezwfprocess where workflowid=5 and isdeleted=0"
                        '    Dim ds_tot As DataSet = GetDatasetByQuery(strQry)
                        '    If ds_tot IsNot Nothing AndAlso ds_tot.Tables.Count > 0 AndAlso ds_tot.Tables(0).Rows.Count > 0 Then
                        '        result.totalRow = CInt(ds_tot.Tables(0).Rows(0)("totcnt").ToString())
                        '    End If
                        'End If
                    Catch ex As Exception

                    End Try
                    result.data = lstItems
                    response = Request.CreateResponse(HttpStatusCode.OK, result)
                Else
                    response = Request.CreateErrorResponse(HttpStatusCode.OK, "No records found")
                End If
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function ProcessingTimeReport(para As SearchRegistries) As HttpResponseMessage
            Dim strQry As String = "", rowQry As String = ""
            Dim response As HttpResponseMessage
            Dim result As New ProcessingTimeReport
            Dim lstItems As New List(Of ProcessingTimeReportA)
            Dim transDtCond As String = ""
            Dim mainqrycondMakerchecker As String = ""
            Try
                Dim CondtionReg As String = ""
                Dim Tablename = ""
                For Each cond In para.Criteria
                    'If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                    '    cond.Criteria = "wf.createdBy"
                    'ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                    '    cond.Criteria = "wf.createdon"
                    'ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                    '    cond.Criteria = "BTH.createdon"
                    'End If
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
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                                CondtionReg = CondtionReg + " and wf.[createdby] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                                CondtionReg = CondtionReg + " and [RIM Number] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                                CondtionReg = CondtionReg + " and [Account Number] in (" + Inval + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                                transDtCond = " and action like '%maker%' and ( ActivityUserId in (" + Inval + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + Inval + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + ")))) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                                transDtCond = " and action like '%checker%' and ( ActivityUserId in (" + Inval + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + Inval + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + Inval + ")) or activityuserid  in (" + Inval + ")))) "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in (" + Inval + ") "
                            End If
                        Else
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                                CondtionReg = CondtionReg + " and wf.[createdby] in ('" + cond.Value1 + "') "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                                CondtionReg = CondtionReg + " and [RIM Number] in (" + cond.Value1 + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                                CondtionReg = CondtionReg + " and [Account Number] in (" + cond.Value1 + ") "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                                transDtCond = " and action like '%maker%' and ( ActivityUserId in (" + cond.Value1 + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + cond.Value1 + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                                transDtCond = " and action like '%checker%' and ( ActivityUserId in (" + cond.Value1 + ") or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId in (" + cond.Value1 + "))) "
                                mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in ('" + cond.Value1 + "') "
                            End If

                        End If
                    ElseIf cond.DataTypeId = "4" Then
                        If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedby" Then
                            CondtionReg = CondtionReg + " and wf.[createdby] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "rimnumber" Then
                            CondtionReg = CondtionReg + " and [RIM Number] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "accountno" Then
                            CondtionReg = CondtionReg + " and [Account Number] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "maker" Then
                            transDtCond = " and action like '%maker%' and ( ActivityUserId like %" + cond.Value1 + "% or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId like %" + cond.Value1 + "%)) "
                            mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%maker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                        ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "checker" Then
                            transDtCond = " and action like '%checker%' and ( ActivityUserId like %" + cond.Value1 + "% or ActivityGroupId in ( select ECMGroupId from eZECMGroupUsers  where  ECMLoginId like %" + cond.Value1 + "%)) "
                            mainqrycondMakerchecker = " and ( wf.processid in (select processid from ezwflowtransation where [action] like '%checker%' and  ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + "))) or wf.processid in (select processid from eZWFlowTransation_Completed where [action] like '%checker%' and ( activitygroupid in (select ECMGroupId  from eZECMGroupUsers where ECMloginId in (" + cond.Value1 + ")) or activityuserid  in (" + cond.Value1 + ")))) "
                        Else
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                        End If
                    ElseIf cond.DataTypeId = "5" Then
                        If cond.Value1 <> "" And cond.Value2 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + " and wf.[createdon] <> '' and  convert(datetime,wf.[createdon],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + " and BTH.[createdon] <> '' and  convert(datetime,BTH.[createdon],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                'Else

                                '    CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            End If

                        ElseIf cond.Value1 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + " and wf.[createdon] <> '' and convert(datetime,wf.[createdon],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + " and BTH.[createdon] <> '' and convert(datetime,BTH.[createdon],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                            End If

                        ElseIf cond.Value2 <> "" Then
                            If cond.Criteria.Replace(" ", "").ToLower().Trim() = "raisedon" Then
                                CondtionReg = CondtionReg + "  and convert(datetime,wf.[createdon],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                            ElseIf cond.Criteria.Replace(" ", "").ToLower().Trim() = "scandateandtime" Then
                                CondtionReg = CondtionReg + "  and convert(datetime,BTH.[createdon],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                            End If
                        End If
                    End If
                Next
                If para.RowCount <> 0 Then
                    rowQry = "  OFFSET " + para.RowFrom.ToString() + " ROWS FETCH NEXT " + para.RowCount.ToString() + " ROWS ONLY"
                End If
                strQry = "SELECT  wf.processid,wf.workflowId, wf.requestNo,flowstatus,format(convert(datetime,BTH.CreatedOn,113),'dd-MMM-yyyy hh:mm:ss tt') AS 'scannedat',Bth.FirstName as 'scannedby',case when (ezt.[Eximbills Reference] is null or ezt.[Eximbills Reference]='')  then 'NONE' else ezt.[Eximbills Reference] end  as [Transaction reference],[RIM Number] AS RIM,[Account Number] as AccountNO ,Product,   Phase,[RIM Number],'' as Stage,wf.createdon AS 'claimed On',Bth.FirstName AS 'claimed By','' AS 'Submitted to Approval','' AS 'Received by',isnull(Type,'None') Type FROM  ezwfprocess wf LEFT JOIN (select distinct [Work Item Reference],isdeleted ,[RIM Number],[Account Number],Product,Phase,Type  from  ezca_3_15_items) itm  ON itm.[Work Item Reference] = wf.requestNo  left join (select distinct [work item reference] ,Bt.createdOn,dbo.udf_Firstname(BT.createdBy) as FirstName  from  BBK_TicketQueue  BT  )BTH on BTH.[Work Item Reference]=itm.[Work Item Reference] left join (select [Eximbills Reference],[Work Item Reference] from [ezfb_Trade Finance Form]) ezt on ezt.[Work Item Reference]=wf.RequestNo  WHERE itm.isdeleted = 0 and wf.WorkflowId=5 and wf.isdeleted=0 and wf.RequestNo  is not null " & CondtionReg & " " & mainqrycondMakerchecker & "  order by wf.createdon desc " & rowQry
                Dim ds As DataSet = GetDatasetByQuery(strQry)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    Dim datas = ds.Tables(0).AsEnumerable().Select(Function(objRead)
                                                                       Dim objItems = New ProcessingTimeReportA With {
                    .processId = objRead.Field(Of Int32)("processid"),
                    .workflowId = objRead.Field(Of Int32)("workflowId"),
                    .requestNo = objRead.Field(Of String)("requestNo"),
                    .transactionReference = objRead.Field(Of String)("Transaction reference"),
                    .product = objRead.Field(Of String)("Product"),
                    .phase = objRead.Field(Of String)("Phase"),
                    .type = objRead.Field(Of String)("Type"),
                    .claimedOn = objRead.Field(Of String)("claimed On"),
                    .claimedBy = objRead.Field(Of String)("claimed By")
                    }
                                                                       Dim processid = objRead.Field(Of Int32)("processid")
                                                                       Try
                                                                           strQry = "select ActivityId,CreatedOn from ezwflowtransation where processid=" + processid.ToString() + "  and isdeleted=0  order by transactionid desc"
                                                                           Dim dstrans As DataSet = GetDatasetByQuery(strQry)
                                                                           If dstrans IsNot Nothing AndAlso dstrans.Tables.Count > 0 AndAlso dstrans.Tables(0).Rows.Count > 0 Then
                                                                               'objItems.stage = dstrans.Tables(0).Rows(0)("Action").ToString()
                                                                               objItems.activityId = dstrans.Tables(0).Rows(0)("ActivityId").ToString()
                                                                               objItems.currentlyReceivingTime = dstrans.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                           Else
                                                                               strQry = "select * from ezwflowtransation_completed where processid=" + processid.ToString() + "  and isdeleted=0  order by transactionid desc"
                                                                               Dim dstrans_comp As DataSet = GetDatasetByQuery(strQry)
                                                                               If dstrans_comp IsNot Nothing AndAlso dstrans_comp.Tables.Count > 0 AndAlso dstrans_comp.Tables(0).Rows.Count > 0 Then
                                                                                   'objItems.stage = dstrans_comp.Tables(0).Rows(0)("Action").ToString()
                                                                                   objItems.activityId = dstrans_comp.Tables(0).Rows(0)("ActivityId").ToString()
                                                                                   objItems.currentlyReceivingTime = dstrans.Tables(0).Rows(0)("Updatedon").ToString()
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try
                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where processid=" + processid.ToString() + " and (action like '%Reject%' or review like '%Reject%') and isdeleted=0 " + transDtCond + "  order by transactionid desc"
                                                                           Dim dscompleted As DataSet = GetDatasetByQuery(strQry)
                                                                           If dscompleted IsNot Nothing AndAlso dscompleted.Tables.Count > 0 AndAlso dscompleted.Tables(0).Rows.Count > 0 Then
                                                                               objItems.totalNumberOfRejections = dscompleted.Tables(0).Rows.Count.ToString()
                                                                           Else
                                                                               strQry = "select * from ezwflowtransation_completed where processid=" + processid.ToString() + " and (action like '%Reject%' or review like '%Reject%') and isdeleted=0 " + transDtCond + "  order by transactionid desc"
                                                                               Dim ds_comp As DataSet = GetDatasetByQuery(strQry)
                                                                               If ds_comp IsNot Nothing AndAlso ds_comp.Tables.Count > 0 AndAlso ds_comp.Tables(0).Rows.Count > 0 Then
                                                                                   objItems.totalNumberOfRejections = ds_comp.Tables(0).Rows.Count.ToString()
                                                                               Else
                                                                                   objItems.totalNumberOfRejections = "0"
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try
                                                                       'Total processing time 
                                                                       Try
                                                                           Dim processStatus As String = objRead.Field(Of String)("flowstatus")
                                                                           strQry = "select * from ezwflowtransation where processid=" + processid.ToString() + " and  isdeleted=0   order by transactionid desc"
                                                                           Dim dsProcessTime As DataSet = GetDatasetByQuery(strQry)
                                                                           If dsProcessTime IsNot Nothing AndAlso dsProcessTime.Tables.Count > 0 AndAlso dsProcessTime.Tables(0).Rows.Count > 0 Then
                                                                               Dim processStartDate As String = dsProcessTime.Tables(0).Rows(dsProcessTime.Tables(0).Rows.Count - 1)("CreatedOn").ToString()
                                                                               Dim processEndDate As String = dsProcessTime.Tables(0).Rows(0)("Createdon").ToString()
                                                                               If processStartDate <> "" And processEndDate <> "" Then
                                                                                   Dim startDate As DateTime = DateTime.ParseExact(processStartDate, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                                   Dim endDate As DateTime = DateTime.ParseExact(processEndDate, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                                   Dim difference As TimeSpan = endDate - startDate
                                                                                   If difference.Seconds > 0 Then
                                                                                       objItems.totalProcessingTime = difference.Seconds.ToString() & " secs"
                                                                                   End If
                                                                                   If difference.Minutes > 0 Then
                                                                                       objItems.totalProcessingTime = difference.Minutes.ToString() & " Mins"
                                                                                   End If
                                                                                   If difference.Hours > 0 Then
                                                                                       objItems.totalProcessingTime = difference.Hours.ToString() & " hours"
                                                                                   End If
                                                                                   If difference.Days > 0 Then
                                                                                       objItems.totalProcessingTime = difference.Days.ToString() & " days"
                                                                                   End If
                                                                               End If
                                                                           Else
                                                                               strQry = "select * from eZWFlowTransation_Completed where processid=" + processid.ToString() + " and isdeleted=0 order by transactionId desc"
                                                                               Dim dsProcessComplete = GetDatasetByQuery(strQry)
                                                                               If dsProcessComplete IsNot Nothing AndAlso dsProcessComplete.Tables.Count > 0 AndAlso dsProcessComplete.Tables(0).Rows.Count > 0 Then
                                                                                   Dim processcompStartDate As String = dsProcessComplete.Tables(0).Rows(dsProcessComplete.Tables(0).Rows.Count - 1)("CreatedOn").ToString()
                                                                                   Dim processcompEndDate As String = dsProcessComplete.Tables(0).Rows(0)("Createdon").ToString()
                                                                                   Dim startDate As DateTime = DateTime.ParseExact(processcompStartDate, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                                   Dim endDate As DateTime = DateTime.ParseExact(processcompEndDate, "dd-MMM-yyyy hh:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                                                                                   Dim difference As TimeSpan = endDate - startDate
                                                                                   If difference.Seconds > 0 Then
                                                                                       objItems.totalProcessingTime = difference.Seconds.ToString() & " secs"
                                                                                   End If
                                                                                   If difference.Minutes > 0 Then
                                                                                       objItems.totalProcessingTime = difference.Minutes.ToString() & " Mins"
                                                                                   End If
                                                                                   If difference.Hours > 0 Then
                                                                                       objItems.totalProcessingTime = difference.Hours.ToString() & " hours"
                                                                                   End If
                                                                                   If difference.Days > 0 Then
                                                                                       objItems.totalProcessingTime = difference.Days.ToString() & " days"
                                                                                   End If

                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where  [Action] ='Pending with TF Maker - L1' and processid=" + processid.ToString() + "  order by transactionid desc "

                                                                           Dim dsapprover As DataSet = GetDatasetByQuery(strQry)
                                                                           If dsapprover IsNot Nothing AndAlso dsapprover.Tables.Count > 0 AndAlso dsapprover.Tables(0).Rows.Count > 0 Then

                                                                               objItems.submittedToApproval = dsapprover.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                           Else

                                                                               strQry = "select * from ezwflowtransation_Completed where  [Action] ='Pending with TF Maker - L1' and processid=" + processid.ToString() + "  order by transactionid desc "

                                                                               Dim ds_approvecomp As DataSet = GetDatasetByQuery(strQry)
                                                                               If ds_approvecomp IsNot Nothing AndAlso ds_approvecomp.Tables.Count > 0 AndAlso ds_approvecomp.Tables(0).Rows.Count > 0 Then
                                                                                   '  objItems.receivedBy = ds_approvecomp.Tables(0).Rows(0)("Action").ToString()
                                                                                   objItems.submittedToApproval = ds_approvecomp.Tables(0).Rows(0)("CreatedOn").ToString()
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try
                                                                           strQry = "select * from ezwflowtransation where processid=" + processid.ToString() + " and Action in ('Pending in Queue','waiting for documents')"
                                                                           Dim dstrans As DataSet = GetDatasetByQuery(strQry)
                                                                           If dstrans IsNot Nothing AndAlso dstrans.Tables.Count > 0 AndAlso dstrans.Tables(0).Rows.Count > 0 Then
                                                                               objItems.receivedBy = "None"
                                                                           Else
                                                                               strQry = "select distinct loginName from ezecmlogin where isdeleted=0 and  ecmloginid in (select activityuserid from ezwflowtransation where transactionstatus=0 and processid=" + processid.ToString() + " and isdeleted=0 UNION select ezg.ECMloginid from  ezecmgroupusers ezg JOIN ezwflowtransation ezt on ezg.ecmgroupid = ezt.ActivityGroupId where transactionstatus=0 and processid=" + processid.ToString() + " and ezt.isdeleted=0 and ezg.isdeleted=0)"

                                                                               Dim dsuser As DataSet = GetDatasetByQuery(strQry)
                                                                               If dsuser IsNot Nothing AndAlso dsuser.Tables.Count > 0 AndAlso dsuser.Tables(0).Rows.Count > 0 Then
                                                                                   Dim res As List(Of String) = dsuser.Tables(0).AsEnumerable().Select(Function(row) row.Field(Of String)("loginName")).ToList()
                                                                                   If res.Count > 0 Then
                                                                                       If res.Count = 1 Then
                                                                                           objItems.receivedBy = res(0)
                                                                                       Else
                                                                                           objItems.receivedBy = "None"
                                                                                       End If
                                                                                   End If
                                                                               Else
                                                                                   strQry = "select distinct loginName from ezecmlogin where isdeleted=0 and  ecmloginid in (select activityuserid from eZWFlowTransation_Completed where transactionstatus=1 and processid=" + processid.ToString() + " and isdeleted=0 and review='Approve to Close' UNION select ezg.ECMloginid from  ezecmgroupusers ezg JOIN eZWFlowTransation_Completed ezt on ezg.ecmgroupid = ezt.ActivityGroupId where transactionstatus=1 and processid=" + processid.ToString() + " and ezt.isdeleted=0 and ezg.isdeleted=0 and ezt.review='Approve to Close')"
                                                                                   Dim ds_transcompl As DataSet = GetDatasetByQuery(strQry)
                                                                                   If ds_transcompl IsNot Nothing AndAlso ds_transcompl.Tables.Count > 0 AndAlso ds_transcompl.Tables(0).Rows.Count > 0 Then
                                                                                       Dim res As List(Of String) = dsuser.Tables(0).AsEnumerable().Select(Function(row) row.Field(Of String)("loginName")).ToList()
                                                                                       If res.Count > 0 Then
                                                                                           objItems.receivedBy = res(0)
                                                                                       Else
                                                                                           objItems.receivedBy = "None"
                                                                                       End If
                                                                                   End If
                                                                               End If
                                                                           End If
                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Try
                                                                           Dim transactioncntratio As String = "0"
                                                                           Dim transrejectionratio As String = "0"
                                                                           strQry = "select count(*) from ezwflowtransation where processid=" & processid.ToString() & " group by activityId "
                                                                           Dim ds_transcnt As DataSet = GetDatasetByQuery(strQry)
                                                                           If ds_transcnt IsNot Nothing AndAlso ds_transcnt.Tables.Count > 0 AndAlso ds_transcnt.Tables(0).Rows.Count > 0 Then
                                                                               transactioncntratio = ds_transcnt.Tables(0).Rows.Count.ToString()
                                                                               objItems.totalTransactionProcessed = transactioncntratio
                                                                           Else
                                                                               strQry = "select Count(*) from ezwflowtransation_completed where processid=" & processid.ToString() & " group by activityId"
                                                                               Dim ds_transcntcompleted As DataSet = GetDatasetByQuery(strQry)
                                                                               If ds_transcntcompleted IsNot Nothing AndAlso ds_transcntcompleted.Tables.Count > 0 AndAlso ds_transcntcompleted.Tables(0).Rows.Count > 0 Then
                                                                                   transactioncntratio = ds_transcntcompleted.Tables(0).Rows.Count.ToString()
                                                                                   objItems.totalTransactionProcessed = transactioncntratio
                                                                               End If
                                                                           End If
                                                                           strQry = "select * from ezwflowtransation where processid=" & processid.ToString() & " and [review]='reject'"
                                                                           Dim ds_rejcnt As DataSet = GetDatasetByQuery(strQry)
                                                                           If ds_rejcnt IsNot Nothing AndAlso ds_rejcnt.Tables.Count > 0 AndAlso ds_rejcnt.Tables(0).Rows.Count > 0 Then
                                                                               transrejectionratio = ds_rejcnt.Tables(0).Rows.Count.ToString()
                                                                           Else
                                                                               strQry = "select * from ezwflowtransation_completed where processid=" & processid.ToString() & " and [review]='reject'"
                                                                               Dim ds_rejcntcompleted As DataSet = GetDatasetByQuery(strQry)
                                                                               If ds_rejcntcompleted IsNot Nothing AndAlso ds_rejcntcompleted.Tables.Count > 0 AndAlso ds_rejcntcompleted.Tables(0).Rows.Count > 0 Then
                                                                                   transrejectionratio = ds_rejcntcompleted.Tables(0).Rows.Count.ToString()
                                                                               End If
                                                                           End If
                                                                           objItems.transactionRatio = transrejectionratio & ":" & transactioncntratio

                                                                       Catch ex As Exception

                                                                       End Try

                                                                       Return objItems
                                                                   End Function).ToList()
                    lstItems = datas
                    Try
                        result.totalRow = lstItems.Count
                        If CondtionReg = "" AndAlso transDtCond = "" Then
                            strQry = "select count(*) as totcnt from ezwfprocess where workflowid=5 and isdeleted=0"
                            Dim ds_tot As DataSet = GetDatasetByQuery(strQry)
                            If ds_tot IsNot Nothing AndAlso ds_tot.Tables.Count > 0 AndAlso ds_tot.Tables(0).Rows.Count > 0 Then
                                result.totalRow = CInt(ds_tot.Tables(0).Rows(0)("totcnt").ToString())
                            End If
                        End If

                    Catch ex As Exception

                    End Try
                    result.data = lstItems
                    response = Request.CreateResponse(HttpStatusCode.OK, result)
                Else
                    response = Request.CreateErrorResponse(HttpStatusCode.OK, "No records found")
                End If
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function
#End Region
    End Class

End Namespace