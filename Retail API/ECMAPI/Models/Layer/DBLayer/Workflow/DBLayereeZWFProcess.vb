Imports System.Data.SqlClient
Imports System.IO
Imports ECMAPI.DBLibrary
Imports ECMAPI.ParaVariables

Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZWFProcess)
        If objRead.IsReadFromDB Then
            Return
        End If
        If objRead.IsModified Then
            Throw New InvalidOperationException()
        End If
        Dim sqlRdr As SqlDataReader = Nothing
        objRead.IsReadFromDB = True
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            objParam = New SqlParameter(0) {}
            'strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZWFProcess ez " +
            '    "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
            '    "Where ez.ProcessId=@ProcessId and ez.Isdeleted=0"
            strQry = "select y.count as DocCount,x.*,case when slaResolution.slacnt is null then 0 else 1 end as ResolutionEscalated,case when slaResp.slacnt is null then 0 else 1 end as ResponseEscalated,case when othersla.slacnt is null then 0 else 1 end as OtherEscalated from (select p.Createdon as RaisedOn, 0 as DaysOpen,cast(datename(m,cast(p.Createdon as datetime)) as varchar(3))+''''+cast(datepart(yy,cast(p.Createdon as datetime)) as varchar) as months,e.firstname as RaisedBy,e.firstname as CreatedBy1,frm.Formid ,frm.tablename as FormTableName,det.Formid as FTemplateid,[dbo].[udf_TableName](det.Templateid) as ItemTableName,euu.firstname as Updatedby1,ers.ERSDirPath+itm.ifilepath+itm.ifilename as ifilepath,P.* from eZWFProcess p left join ezecmuserinfo e on p.Createdby=e.ecmloginid  left join ezecmuserinfo euu on p.Updatedby=euu.ecmloginid ,ezworkflowdetails det,ezwflowformdetails frm ,eZCA_1_4_items itm,eZERSInfo ers where ers.ERSId =itm.ERSId and itm.itemid =det.WorkflowItemId and  p.WorkflowId=det.WorkflowId and det.workflowid=frm.WorkflowId and p.ProcessId=@ProcessId) as x left join (select processid,count(1) as count from ezProcessItems where  ItemId <> 0 and Processid=@ProcessId group by Processid) as y on x.Processid=y.Processid left join (select processid,count(1) as slacnt from eZProcessSLA_Details where Processid=@Processid and [SLA Level]='Level 1 - Resolution' group by Processid) as slaResolution on x.Processid=slaResolution.Processid left join (select processid,count(1) as slacnt from eZProcessSLA_Details where Processid=@Processid and [SLA Level]='Level 1 - Response1' group by Processid) as slaResp on x.Processid=slaResp.Processid left join (select processid,count(1) as slacnt from eZProcessSLA_Details where Processid=@Processid group by Processid) as othersla on x.Processid=othersla.Processid"
            param = New SqlParameter("@ProcessId", objRead.ProcessId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFProcess")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ProcessId = GetInteger(sqlRdr("ProcessId"))
                objRead.WorkflowId = GetInteger(sqlRdr("WorkflowId"))
                objRead.Workflowtypeid = GetInteger(sqlRdr("Workflowtypeid"))
                objRead.Itemid = GetInteger(sqlRdr("Itemid"))
                objRead.Templateid = GetInteger(sqlRdr("Templateid"))
                objRead.FlowStatus = sqlRdr("FlowStatus").ToString()
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.RequestNo = sqlRdr("RequestNo").ToString()
                objRead.RaisedOn = sqlRdr("RaisedOn").ToString()
                objRead.RaisedBy = sqlRdr("RaisedBy").ToString()
                objRead.Formid = sqlRdr("Formid").ToString()
                objRead.FormTableName = sqlRdr("FormTableName").ToString()
                objRead.FTemplateid = sqlRdr("FTemplateid").ToString()
                objRead.ItemTableName = sqlRdr("ItemTableName").ToString()
                objRead.ifilepath = sqlRdr("ifilepath").ToString()
                objRead.DocCount = sqlRdr("DocCount").ToString()
                If GetInteger(sqlRdr("ResolutionEscalated")) = 1 Or GetInteger(sqlRdr("ResponseEscalated")) = 1 Then
                    objRead.Escalated = 1
                ElseIf GetInteger(sqlRdr("OtherEscalated")) = 1 Then
                    objRead.Escalated = 1
                End If
                objRead.DaysOpen = sqlRdr("DaysOpen").ToString()
                objRead.Month = sqlRdr("months").ToString()
                Dim tablename As String = "eZWFlowTransation"
                If objRead.FlowStatus = "Completed" Then
                    tablename = "ezwflowtransation_Completed"
                    objRead.DaysOpen = ""
                End If
                Dim ActionBy As New List(Of Userslist)
                Dim Action As String = ""
                Try

                    Dim sqlRdr1 As SqlDataReader = Nothing
                    Dim objParam1 As SqlParameter()
                    Dim strQry1 As String = "select e.firstname as LastActedBy,t.CreatedOn LastActedOn,eu.firstname as ActionUserBy,g.ECMGroup as ActionGroupBy,t.Action,t.UserType as UserRole from eZWFlowTransation t left join ezecmuserinfo e on t.CreatedBy=e.ecmloginid left join ezecmuserinfo eu on t.ActivityUserId=eu.ecmloginid left join  eZECMGroup g on t.ActivityGroupId=g.ECMGroupId where Processid=@Processid and TransactionStatus=0"
                    If objRead.FlowStatus = "Completed" Then
                        strQry1 = "select e.firstname as LastActedBy,t.CreatedOn LastActedOn,t.UserType as UserRole from ezwflowtransation_Completed t left join ezecmuserinfo e on t.CreatedBy=e.ecmloginid where Processid=@Processid and Action='End'"
                    End If
                    Dim obj1 As Object = ""
                    objParam1 = New SqlParameter(0) {}
                    Dim param1 As SqlParameter
                    param1 = New SqlParameter("@Processid", objRead.ProcessId.ToString())
                    objParam1(0) = param1
                    obj1 = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry1.ToString(), objParam1)
                    If obj1 Is Nothing Then
                        Throw New Exception("Attempt to read Invalid eZWFlowTransation")
                    End If
                    sqlRdr1 = DirectCast(obj1, SqlDataReader)
                    While sqlRdr1.Read()
                        objRead.LastActedOn = sqlRdr1("LastActedOn").ToString()
                        objRead.LastActedBy = sqlRdr1("LastActedBy").ToString()
                        If objRead.FlowStatus = "Running" Then
                            Dim susers As New Userslist
                            susers.UserType = sqlRdr1("Action").ToString()
                            susers.UserRole = sqlRdr1("UserRole").ToString()
                            If Action = "" Then
                                Action = sqlRdr1("Action").ToString()
                            Else
                                If Not Action.Contains(sqlRdr1("Action").ToString()) Then
                                    Action = Action + "," + sqlRdr1("Action").ToString()
                                End If

                            End If

                            If sqlRdr1("ActionUserBy").ToString() = "" Then
                                susers.Username = sqlRdr1("ActionGroupBy").ToString()
                            Else
                                susers.Username = sqlRdr1("ActionUserBy").ToString()
                            End If
                            ActionBy.Add(susers)
                        End If
                    End While
                Catch ex As Exception

                End Try
                objRead.ActionBy = ActionBy
                objRead.Action = Action
                Try
                    Dim sqlRdr1 As SqlDataReader = Nothing
                    Dim objParam1 As SqlParameter()
                    Dim strQry1 As String = "select top 1 Action,Review from " + tablename + " where processid=@Processid and TransactionStatus=1 and Action<>'End' order by Transactionid desc"
                    Dim obj1 As Object = ""
                    objParam1 = New SqlParameter(0) {}
                    Dim param1 As SqlParameter
                    param1 = New SqlParameter("@Processid", objRead.ProcessId.ToString())
                    objParam1(0) = param1
                    obj1 = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry1.ToString(), objParam1)
                    If obj1 Is Nothing Then
                        Throw New Exception("Attempt to read Invalid eZWFlowTransation")
                    End If
                    sqlRdr1 = DirectCast(obj1, SqlDataReader)
                    If sqlRdr1.Read() Then
                        objRead.LastActionStage = sqlRdr1("Action").ToString()
                        objRead.LastActionReview = sqlRdr1("Review").ToString()
                    End If
                Catch ex As Exception

                End Try


                objRead.SplUsers = New List(Of Userslist)
                    Dim xmlds As New DataSet
                    xmlds.ReadXml(New StringReader(New System.IO.StreamReader(sqlRdr("ifilepath").ToString()).ReadToEnd()))
                    Dim dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)
                    Try
                        Dim sqlRdr1 As SqlDataReader = Nothing
                        Dim objParam1 As SqlParameter()
                        Dim strQry1 As String = "select Top 1 " + xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString() + " from [" + objRead.FormTableName + "] where itemid in (select Top 1 FormEntryId  from ezProcessItems where Processid =@Processid and FormEntryId <> '0' order by ProcessItemsid desc)"
                        If objRead.FormTableName = "" Then
                            strQry1 = "select Top 1 " + xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString() + " from [" + objRead.ItemTableName + "] where itemid in (select Top 1 Itemid  from ezProcessItems where Processid =@Processid and Templateid <> '0' and Itemid <> 0 order by ProcessItemsid desc)"
                        End If
                        Dim obj1 As Object = ""
                        objParam1 = New SqlParameter(0) {}
                        Dim param1 As SqlParameter
                        param1 = New SqlParameter("@Processid", objRead.ProcessId.ToString())
                        objParam1(0) = param1
                        'param1 = New SqlParameter("@Fields", xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString())
                        'objParam1(1) = param1
                        obj1 = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry1.ToString(), objParam1)
                        If obj1 Is Nothing Then
                            Throw New Exception("Attempt to read Invalid Formtable")
                        End If
                        sqlRdr1 = DirectCast(obj1, SqlDataReader)
                        If sqlRdr1.Read() Then

                            Dim processinfo() As String = xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                            For j As Integer = 0 To processinfo.Length - 1
                                dictionary.Add(processinfo(j).Replace("[", "").Replace("]", ""), sqlRdr1(processinfo(j).Replace("[", "").Replace("]", "")).ToString())
                            Next
                        End If
                    Catch ex As Exception
                        ' Throw New FaultException(ex.Message)
                    End Try
                    objRead.DynamicProperty = dictionary

            Else
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function CreateeZWFProcess(objEmp As eZWFProcess) As eZWFProcess
        Dim newObject As eZWFProcess = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZWFProcess(WorkflowId,FlowStatus,Workflowtypeid,Itemid,Templateid,CreatedBy,CreatedOn,RequestNo) VALUES " +
                "(@WorkflowId,@FlowStatus,@Workflowtypeid,@Itemid,@Templateid,@CreatedBy,@CreatedOn,@RequestNo);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@WorkflowId", objEmp.WorkflowId)
            objParam(0) = param
            param = New SqlParameter("@FlowStatus", objEmp.FlowStatus)
            objParam(1) = param
            param = New SqlParameter("@Workflowtypeid", objEmp.Workflowtypeid)
            objParam(2) = param
            param = New SqlParameter("@Itemid", objEmp.Itemid)
            objParam(3) = param
            param = New SqlParameter("@Templateid", objEmp.Templateid)
            objParam(4) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(6) = param
            param = New SqlParameter("@RequestNo", objEmp.RequestNo)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZWFProcess(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZWFProcess)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZWFProcess Set WorkflowId=@WorkflowId,FlowStatus=@FlowStatus,Workflowtypeid=@Workflowtypeid,Itemid=@Itemid," +
            "Templateid=@Templateid,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,RequestNo=@RequestNo where ProcessId=@ProcessId"
        objParam = New SqlParameter(8) {}
        param = New SqlParameter("@WorkflowId", objToUpdate.WorkflowId)
        objParam(0) = param
        param = New SqlParameter("@FlowStatus", objToUpdate.FlowStatus)
        objParam(1) = param
        param = New SqlParameter("@Workflowtypeid", objToUpdate.Workflowtypeid)
        objParam(2) = param
        param = New SqlParameter("@Itemid", objToUpdate.Itemid)
        objParam(3) = param
        param = New SqlParameter("@Templateid", objToUpdate.Templateid)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(5) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(6) = param
        param = New SqlParameter("@RequestNo", objToUpdate.RequestNo)
        objParam(7) = param
        param = New SqlParameter("@ProcessId", objToUpdate.ProcessId)
        objParam(8) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZWFProcess)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZWFProcess set Isdeleted=1 where ProcessId=@ProcessId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ProcessId", objToDelete.ProcessId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZWFProcess() As System.Collections.Generic.List(Of IeZWFProcess)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFProcess)()
        Dim objItem As IeZWFProcess
        Try
            Dim strQry As String = ""
            strQry = "Select ProcessId From eZWFProcess where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFProcess")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFProcess(GetInteger(sqlRdr("ProcessId")))
                objItem.ProcessId = GetInteger(sqlRdr("ProcessId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZWFProcess(Criteria As String, Value As String) As List(Of IeZWFProcess)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFProcess)()
        Dim objItem As IeZWFProcess
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ProcessId From eZWFProcess where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ProcessId"
            Else
                strQry = "Select ProcessId From eZWFProcess where Isdeleted=0 order by ProcessId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFProcess")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFProcess(GetInteger(sqlRdr("ProcessId")))
                objItem.ProcessId = GetInteger(sqlRdr("ProcessId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZWFProcess(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZWFProcess)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFProcess)()
        Dim objItem As IeZWFProcess
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ProcessId From eZWFProcess where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ProcessId"
            Else
                strQry = "Select ProcessId From eZWFProcess where Isdeleted=0 order by ProcessId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFProcess")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFProcess(GetInteger(sqlRdr("ProcessId")))
                objItem.ProcessId = GetInteger(sqlRdr("ProcessId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function






    Public Function ReadProcessListbyUserid(WorkflowId As String, ECMLoginId As String, ECMGroupList As String, RowFrom As Integer, RowCount As Integer) As System.Collections.Generic.List(Of IeZWFProcess)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFProcess)()
        Dim objItem As IeZWFProcess
        Try
            Dim strQry As String = ""

            strQry = "select distinct processid from ezwflowtransation where (ActivityUserId=" + ECMLoginId + " or(ActivityGroupId<>0 and updatedby=" + ECMLoginId + "))  and Processid in(select Processid from eZWFProcess where FlowStatus='running' and WorkflowId=" + WorkflowId + ") and TransactionStatus<>0 and Processid not in(select Processid from eZWFlowTransation where TransactionStatus=0 and ActivityUserId=" + ECMLoginId + ") "
            strQry = strQry
            Dim Isowner As Boolean = CheckUserAsOwner(WorkflowId, ECMLoginId)
            If Isowner Then
                strQry = "select distinct processid from ezwflowtransation where Processid in(select Processid from eZWFProcess where FlowStatus='running' and WorkflowId=" + WorkflowId + ") and TransactionStatus<>0  "
            End If


            If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                If RowCount <> 0 Then
                    strQry = strQry & " order by ProcessId desc OFFSET " + RowFrom.ToString() + " ROWS FETCH NEXT " + RowCount.ToString() + " ROWS ONLY"
                End If
            Else
                If RowCount <> 0 Then
                    strQry = "SELECT distinct processid FROM (" + strQry.Replace("distinct processid", "distinct processid, ROW_NUMBER() OVER (ORDER BY processid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                End If

            End If


            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFProcess")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFProcess(GetInteger(sqlRdr("ProcessId")))
                objItem.ProcessId = GetInteger(sqlRdr("ProcessId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadCompletedListbyUserid(WorkflowId As String, ECMLoginId As String, ECMGroupList As String, RowFrom As Integer, RowCount As Integer) As System.Collections.Generic.List(Of IeZWFProcess)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFProcess)()
        Dim objItem As IeZWFProcess
        Try
            Dim strQry As String = ""

            'strQry = "select distinct processid from ezwflowtransation_Completed where (ActivityUserId=" + ECMLoginId + " or(ActivityGroupId<>0 and updatedby=" + ECMLoginId + "))  and Processid in(select Processid from eZWFProcess where FlowStatus='Completed' and WorkflowId=" + WorkflowId + " and convert(datetime,updatedon,106) between CONVERT(varchar,dateadd(d,-(day(dateadd(m,-1,getdate()-2))),dateadd(m,-1,getdate()-1)),106) and GETDATE()) "
            strQry = "select distinct processid from ezwflowtransation_Completed where (ActivityUserId=" + ECMLoginId + " or(ActivityGroupId<>0 and updatedby=" + ECMLoginId + "))  and Processid in(select Processid from eZWFProcess where FlowStatus='Completed' and WorkflowId=" + WorkflowId + ") "

            Dim Isowner As Boolean = CheckUserAsOwner(WorkflowId, ECMLoginId)
            If Isowner Then
                'strQry = "select distinct processid from ezwflowtransation_Completed where Processid in(select Processid from eZWFProcess where FlowStatus='Completed' and WorkflowId=" + WorkflowId + " and convert(datetime,updatedon,106) between CONVERT(varchar,dateadd(d,-(day(dateadd(m,-1,getdate()-2))),dateadd(m,-1,getdate()-1)),106) and GETDATE()) "
                strQry = "select distinct processid from ezwflowtransation_Completed where Processid in(select Processid from eZWFProcess where FlowStatus='Completed' and WorkflowId=" + WorkflowId + ") "
            End If


            If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                If RowCount <> 0 Then
                    strQry = strQry & " order by ProcessId desc OFFSET " + RowFrom.ToString() + " ROWS FETCH NEXT " + RowCount.ToString() + " ROWS ONLY"
                End If
            Else
                If RowCount <> 0 Then
                    strQry = "SELECT distinct processid FROM (" + strQry.Replace("distinct processid", "distinct processid, ROW_NUMBER() OVER (ORDER BY processid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                End If

            End If

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFProcess" + strQry.ToString())
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFProcess(GetInteger(sqlRdr("ProcessId")))
                objItem.ProcessId = GetInteger(sqlRdr("ProcessId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function


    'Public Sub Read(objread As IeZWFProcess)
    '    If objread.IsReadFromDB Then
    '        Return
    '    End If
    '    If objread.IsModified Then
    '        Throw New InvalidOperationException()
    '    End If
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    objread.IsReadFromDB = True
    '    Try
    '        Dim strQry As String = ""
    '        Dim objParam As SqlParameter()
    '        Dim param As SqlParameter
    '        objParam = New SqlParameter(0) {}
    '        If objread.WorkflowId = 0 Then
    '            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZWFProcess Where ProcessId=@ProcessId and Isdeleted=0"
    '            param = New SqlParameter("@ProcessId", objread.ProcessId)
    '            objParam(0) = param
    '        Else
    '            objParam = New SqlParameter(1) {}
    '            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZWFProcess Where WorkflowId=@WorkflowId and Isdeleted=0"
    '            param = New SqlParameter("@WorkflowId", objread.WorkflowId)
    '            objParam(0) = param
    '        End If
    '        Dim obj As Object = ""
    '        obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid Transaction.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        If sqlRdr.Read() Then
    '            objread.ProcessId = GetInteger(sqlRdr("ProcessId"))
    '            objread.WorkflowId = GetInteger(sqlRdr("WorkflowId"))
    '            objread.Workflowtypeid = GetInteger(sqlRdr("Workflowtypeid"))
    '            objread.Itemid = GetInteger(sqlRdr("Itemid"))
    '            objread.Templateid = GetInteger(sqlRdr("Templateid"))
    '            objread.FlowStatus = sqlRdr("FlowStatus").ToString()
    '            objread.Createdon = sqlRdr("CreatedOn").ToString
    '            'objread.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
    '            objread.Createdby = sqlRdr("CreatedBy").ToString()
    '            objread.Updatedon = sqlRdr("UpdatedOn").ToString()
    '            'objread.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
    '            objread.Updatedby = sqlRdr("UpdatedBy").ToString()
    '        Else
    '            Return
    '        End If
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '        objread.IsModified = False
    '    End Try
    'End Sub
    Public Function InsertandUpdateeZWFProcess(ByVal OBJ As eZWFProcess) As String
        Try
            Dim exc As String
            Dim param As String()
            If String.IsNullOrEmpty(OBJ.Updatedon) Then
                OBJ.Updatedon = "0"
            End If
            If String.IsNullOrEmpty(OBJ.FlowStatus) Then
                OBJ.FlowStatus = "0"
            End If
            If String.IsNullOrEmpty(OBJ.Createdon) Then
                OBJ.Createdon = "0"
            End If
            param = {OBJ.ProcessId.ToString(), OBJ.WorkflowId.ToString(), OBJ.FlowStatus.ToString(), OBJ.Workflowtypeid.ToString(),
                OBJ.Itemid.ToString(), OBJ.Templateid.ToString(), OBJ.Createdon.ToString(), OBJ.Updatedon.ToString(),
                OBJ.Createdby.ToString(), OBJ.Updatedby.ToString()}
            If OBJ.ProcessId <> 0 Then
                exc = DBLayer.DBLInstance.InsertandUpdateStoredProcedure("SP_InsertAndUpdateeZWFProcess", param)
            Else
                Dim ds As New DataSet
                ds = DBLayer.DBLInstance.GetDatasetByStoredProcedureName("SP_InsertAndUpdateeZWFProcess", param)
                If ds.Tables.Count <> 0 Then
                    exc = ds.Tables(0).Rows(0).Item(0).ToString()
                End If
            End If
            Return exc
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
End Class
