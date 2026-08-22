Imports ECMAPI.DBLibrary
Imports ECMAPI.ParaVariables
Imports System.Data.SqlClient
Imports System.IO

Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZWFlowTransation)
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
            'strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZWFlowTransation ez " +
            '    "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
            '    "Where ez.Transactionid=@Transactionid and ez.Isdeleted=0"
            strQry = "select y.count as DocCount,x.*,case when slaResolution.slacnt is null then 0 else 1 end as ResolutionEscalated,case when slaResp.slacnt is null then 0 else 1 end as ResponseEscalated from (select p.RequestNo,p.Createdon as RaisedOn, 0 as DaysOpen,cast(datename(m,cast(p.Createdon as datetime)) as varchar(3))+''''+cast(datepart(yy,cast(p.Createdon as datetime)) as varchar) as months,ei.firstname as RaisedBy,p.workflowid,frm.Formid ,frm.tablename as FormTableName,det.Templateid as FTemplateid,[dbo].[udf_TableName](det.Templateid) as ItemTableName,eu.firstname as LastActedBy,t.createdon as LastActedOn,euu.firstname as Updatedby1,ers.ERSDirPath+itm.ifilepath+itm.ifilename as ifilepath,t.* from ezwflowtransation t left join ezecmuserinfo eu on t.createdby=eu.ecmloginid left join ezecmuserinfo euu on t.Updatedby=euu.ecmloginid,ezwfprocess p left join eZECMUserInfo ei on p.Createdby=ei.ecmloginid,ezworkflowdetails det,ezwflowformdetails frm ,eZCA_1_4_items itm,eZERSInfo ers where ers.ERSId =itm.ERSId and itm.itemid =det.WorkflowItemId and t.transactionid =@Transactionid and t.processid=p.processid and p.WorkflowId=det.WorkflowId and det.workflowid=frm.WorkflowId) as x left join (select processid,count(1) as count from ezProcessItems where Processid=(select processid from eZWFlowTransation where Transactionid=@Transactionid) and itemid<>0 group by Processid) as y on x.Processid=y.Processid left join (select processid,count(1) as slacnt from eZProcessSLA_Details where Processid=(select processid from eZWFlowTransation where Transactionid=@Transactionid) and [SLA Level]='Level 1 - Resolution' group by Processid) as slaResolution on x.Processid=slaResolution.Processid left join (select processid,count(1) as slacnt from eZProcessSLA_Details where Processid=(select processid from eZWFlowTransation where Transactionid=@Transactionid) and [SLA Level]='Level 1 - Response1' group by Processid) as slaResp on x.Processid=slaResp.Processid"
            param = New SqlParameter("@Transactionid", objRead.Transactionid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowTransation")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Transactionid = GetInteger(sqlRdr("Transactionid"))
                objRead.Processid = GetInteger(sqlRdr("Processid"))
                objRead.ActivityGroupId = GetInteger(sqlRdr("ActivityGroupId"))
                objRead.ActivityUserId = GetInteger(sqlRdr("ActivityUserId"))
                objRead.TransactionStatus = GetInteger(sqlRdr("TransactionStatus"))
                objRead.Templateid = GetInteger(sqlRdr("Templateid"))
                objRead.itemid = GetInteger(sqlRdr("itemid"))
                objRead.Notification = GetBoolean(sqlRdr("Notification"))
                objRead.ActivityId = sqlRdr("ActivityId").ToString
                objRead.RuleId = sqlRdr("RuleId").ToString
                objRead.Action = sqlRdr("Action").ToString
                objRead.Review = sqlRdr("Review").ToString
                objRead.TranPath = sqlRdr("TranPath").ToString
                objRead.FileType = sqlRdr("FileType").ToString
                objRead.SkipTo = sqlRdr("SkipTo").ToString
                objRead.FromMail = sqlRdr("FromMail").ToString
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("LastActedBy").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.RequestType = GetBoolean(sqlRdr("RequestType"))
                objRead.UserType = sqlRdr("UserType").ToString()
                objRead.Attachment = GetInteger(sqlRdr("Attachment"))
                objRead.Formid = GetInteger(sqlRdr("Formid"))
                objRead.FTemplateid = GetInteger(sqlRdr("FTemplateid"))
                objRead.RequestNo = sqlRdr("RequestNo").ToString()
                objRead.RaisedOn = sqlRdr("RaisedOn").ToString()
                objRead.RaisedBy = sqlRdr("RaisedBy").ToString()
                ' objRead.ActionGroupBy = sqlRdr("ActionGroupBy").ToString()
                objRead.FormTableName = "[" + sqlRdr("FormTableName").ToString() + "]"
                objRead.ItemTableName = sqlRdr("ItemTableName").ToString()
                objRead.LastActedBy = sqlRdr("LastActedBy").ToString()
                objRead.LastActedOn = sqlRdr("LastActedOn").ToString()
                objRead.DocCount = sqlRdr("DocCount").ToString()
                If GetInteger(sqlRdr("ResolutionEscalated")) = 1 Or GetInteger(sqlRdr("ResponseEscalated")) = 1 Then
                    objRead.Escalated = 1
                End If
                objRead.DaysOpen = sqlRdr("DaysOpen").ToString()
                objRead.Month = sqlRdr("months").ToString()
                Dim ActionBy As New List(Of Userslist)
                Try

                    Dim sqlRdr1 As SqlDataReader = Nothing
                    Dim objParam1 As SqlParameter()
                    Dim strQry1 As String = "select Action,ui.FirstName,g.ECMGroup as ActionGroupBy,UserType as UserRole from eZWFlowTransation tr left join eZECMUserInfo ui on tr.ActivityUserId=ui.ECMLoginId left join  eZECMGroup g on tr.ActivityGroupId=g.ECMGroupId where processid=@Processid and TransactionStatus=0"
                    Dim obj1 As Object = ""
                    objParam1 = New SqlParameter(0) {}
                    Dim param1 As SqlParameter
                    param1 = New SqlParameter("@Processid", objRead.Processid.ToString())
                    objParam1(0) = param1
                    obj1 = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry1.ToString(), objParam1)
                    If obj1 Is Nothing Then
                        Throw New Exception("Attempt to read Invalid eZWFlowTransation")
                    End If
                    sqlRdr1 = DirectCast(obj1, SqlDataReader)
                    While sqlRdr1.Read()
                        Dim susers As New Userslist
                        susers.UserType = sqlRdr1("Action").ToString()
                        susers.Username = sqlRdr1("FirstName").ToString()
                        susers.UserRole = sqlRdr1("UserRole").ToString()
                        If sqlRdr1("FirstName").ToString() = "" Then
                            susers.Username = sqlRdr1("ActionGroupBy").ToString()
                        Else
                            susers.Username = sqlRdr1("FirstName").ToString()
                        End If
                        ActionBy.Add(susers)
                    End While
                Catch ex As Exception

                End Try
                objRead.ActionBy = ActionBy

                Try
                    Dim sqlRdr1 As SqlDataReader = Nothing
                    Dim objParam1 As SqlParameter()
                    Dim strQry1 As String = "select top 1 Action,Review from eZWFlowTransation where processid=@Processid and TransactionStatus=1 order by Transactionid desc"
                    Dim obj1 As Object = ""
                    objParam1 = New SqlParameter(0) {}
                    Dim param1 As SqlParameter
                    param1 = New SqlParameter("@Processid", objRead.Processid.ToString())
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
                        Dim strQry1 As String = "select Top 1 " + xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString() + " from " + objRead.FormTableName + " where itemid in (select Top 1 FormEntryId  from ezProcessItems where Processid =@Processid and FormEntryId <> '0' order by ProcessItemsid desc)"
                        If objRead.FormTableName = "" Then
                            strQry1 = "select Top 1 " + xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString() + " from " + objRead.ItemTableName + " where itemid in (select Top 1 Itemid  from ezProcessItems where Processid =@Processid and Templateid <> '0' and Itemid <> 0 order by ProcessItemsid desc)"
                        End If
                        Dim obj1 As Object = ""
                        objParam1 = New SqlParameter(0) {}
                        Dim param1 As SqlParameter
                        param1 = New SqlParameter("@Processid", objRead.Processid.ToString())
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


    Private Function CheckResponse(Processid As String) As Integer
        Try
            Dim sqlRdr1 As SqlDataReader = Nothing
            Dim objParam1 As SqlParameter()
            Dim strQry1 As String = "select count(1) as ResCount  from eZWFlowTransation where processid=@Processid and action in ('Submit Service Request','Query Ticket Submission') "
            Dim obj1 As Object = ""
            objParam1 = New SqlParameter(0) {}
            Dim param1 As SqlParameter
            param1 = New SqlParameter("@Processid", Processid.ToString())
            objParam1(0) = param1
            'param1 = New SqlParameter("@Fields", xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString())
            'objParam1(1) = param1
            obj1 = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry1.ToString(), objParam1)
            If obj1 Is Nothing Then
                Throw New Exception("Attempt to read Invalid Formtable")
            End If
            sqlRdr1 = DirectCast(obj1, SqlDataReader)
            If sqlRdr1.Read() Then
                Return sqlRdr1("ResCount").ToString()
            End If
            Return 0
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function CheckResponseOn(Processid As String) As String
        Try
            Dim sqlRdr1 As SqlDataReader = Nothing
            Dim objParam1 As SqlParameter()
            Dim strQry1 As String = "select Createdon from ezwflowtransation where Transactionid in( Select min (Transactionid)  from ezwflowtransation where processid=@Processid and action  in ('Submit Service Request','Query Ticket Submission') and Review <> 'Send')"
            Dim obj1 As Object = ""
            objParam1 = New SqlParameter(0) {}
            Dim param1 As SqlParameter
            param1 = New SqlParameter("@Processid", Processid.ToString())
            objParam1(0) = param1
            'param1 = New SqlParameter("@Fields", xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString())
            'objParam1(1) = param1
            obj1 = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry1.ToString(), objParam1)
            If obj1 Is Nothing Then
                Throw New Exception("Attempt to read Invalid Formtable")
            End If
            sqlRdr1 = DirectCast(obj1, SqlDataReader)
            If sqlRdr1.Read() Then
                Return sqlRdr1("Createdon").ToString()
            End If
            Return ""
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Function CheckCompletedResponseOn(Processid As String) As String
        Try
            Dim QueryType As Integer = 0
            Try
                Dim sqlRdr1 As SqlDataReader = Nothing
                Dim objParam1 As SqlParameter()
                Dim strQry1 As String = "Select count(1) as ResCount  from ezwflowtransation_Completed where processid=@Processid and action  in ('Submit Service Request','Query Ticket Submission')"
                Dim obj1 As Object = ""
                objParam1 = New SqlParameter(0) {}
                Dim param1 As SqlParameter
                param1 = New SqlParameter("@Processid", Processid.ToString())
                objParam1(0) = param1
                'param1 = New SqlParameter("@Fields", xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString())
                'objParam1(1) = param1
                obj1 = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry1.ToString(), objParam1)
                If obj1 Is Nothing Then
                    Throw New Exception("Attempt to read Invalid Formtable")
                End If
                sqlRdr1 = DirectCast(obj1, SqlDataReader)
                If sqlRdr1.Read() Then
                    QueryType = GetInteger(sqlRdr1("ResCount"))
                End If
            Catch ex As Exception

            End Try
            Try
                Dim sqlRdr1 As SqlDataReader = Nothing
                Dim objParam1 As SqlParameter()
                Dim strQry1 As String = "select Createdon from ezwflowtransation_Completed where Transactionid in(Select min (Transactionid) from ezwflowtransation_Completed where processid=@Processid and action ='End')"
                If QueryType > 1 Then
                    strQry1 = "select Createdon from ezwflowtransation_Completed where Transactionid in( Select min (Transactionid)  from ezwflowtransation_Completed where processid=@Processid and action  in ('Submit Service Request','Query Ticket Submission') and Review <> 'Send')"
                End If
                Dim obj1 As Object = ""
                objParam1 = New SqlParameter(0) {}
                Dim param1 As SqlParameter
                param1 = New SqlParameter("@Processid", Processid.ToString())
                objParam1(0) = param1
                'param1 = New SqlParameter("@Fields", xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString())
                'objParam1(1) = param1
                obj1 = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry1.ToString(), objParam1)
                If obj1 Is Nothing Then
                    Throw New Exception("Attempt to read Invalid Formtable")
                End If
                sqlRdr1 = DirectCast(obj1, SqlDataReader)
                If sqlRdr1.Read() Then
                    Return sqlRdr1("Createdon").ToString()
                End If
                Return ""
            Catch ex As Exception
                Return ""
            End Try
        Catch ex As Exception

        End Try

    End Function


    Public Function CreateeZWFlowTransation(objEmp As eZWFlowTransation) As eZWFlowTransation
        Dim newObject As eZWFlowTransation = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZWFlowTransation(Processid,ActivityId,RuleId,ActivityUserId,ActivityGroupId,Action,Review,TranPath,TransactionStatus," +
                "templateid,notification,itemid,FileType,SkipTo,FromMail,CreatedBy,CreatedOn,RequestType,UserType,Updatedby,Updatedon,Attachment) VALUES (@Processid," +
                "@ActivityId,@RuleId,@ActivityUserId,@ActivityGroupId,@Action,@Review,@TranPath,@TransactionStatus,@templateid,@notification,@itemid,@FileType," +
                "@SkipTo,@FromMail,@CreatedBy,@CreatedOn,@RequestType,@UserType,@Updatedby,@Updatedon,@Attachment);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(21) {}
            param = New SqlParameter("@Processid", objEmp.Processid)
            objParam(0) = param
            param = New SqlParameter("@ActivityId", objEmp.ActivityId)
            objParam(1) = param
            param = New SqlParameter("@RuleId", objEmp.RuleId)
            objParam(2) = param
            param = New SqlParameter("@ActivityUserId", objEmp.ActivityUserId)
            objParam(3) = param
            param = New SqlParameter("@ActivityGroupId", objEmp.ActivityGroupId)
            objParam(4) = param
            param = New SqlParameter("@Action", objEmp.Action)
            objParam(5) = param
            param = New SqlParameter("@Review", objEmp.Review)
            objParam(6) = param
            param = New SqlParameter("@TranPath", objEmp.TranPath)
            objParam(7) = param
            param = New SqlParameter("@TransactionStatus", objEmp.TransactionStatus)
            objParam(8) = param
            param = New SqlParameter("@Templateid", objEmp.templateid)
            objParam(9) = param
            param = New SqlParameter("@Notification", objEmp.notification)
            objParam(10) = param
            param = New SqlParameter("@itemid", objEmp.itemid)
            objParam(11) = param
            param = New SqlParameter("@FileType", objEmp.FileType)
            objParam(12) = param
            param = New SqlParameter("@SkipTo", objEmp.SkipTo)
            objParam(13) = param
            param = New SqlParameter("@FromMail", objEmp.FromMail)
            objParam(14) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(15) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(16) = param
            param = New SqlParameter("@RequestType", objEmp.RequestType)
            objParam(17) = param
            param = New SqlParameter("@UserType", objEmp.UserType)
            objParam(18) = param
            param = New SqlParameter("@Updatedby", objEmp.Updatedby)
            objParam(19) = param
            param = New SqlParameter("@Updatedon", objEmp.Updatedon)
            objParam(20) = param
            param = New SqlParameter("@Attachment", objEmp.Attachment)
            objParam(21) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZWFlowTransation(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZWFlowTransation)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZWFlowTransation Set " +
            "Processid=@Processid,ActivityId=@ActivityId,RuleId=@RuleId,ActivityUserId=@ActivityUserId,ActivityGroupId=@ActivityGroupId," +
            "Action=@Action,Review=@Review,TranPath=@TranPath,TransactionStatus=@TransactionStatus,Templateid=@Templateid,Notification=@Notification,itemid=@itemid" +
            ",FileType=@FileType,SkipTo=@SkipTo,FromMail=@FromMail,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,RequestType=@RequestType,UserType=@UserType," +
            "Attachment=@Attachment where Transactionid=@Transactionid"
        objParam = New SqlParameter(20) {}
        param = New SqlParameter("@Processid", objToUpdate.Processid)
        objParam(0) = param
        param = New SqlParameter("@ActivityId", objToUpdate.ActivityId)
        objParam(1) = param
        param = New SqlParameter("@RuleId", objToUpdate.RuleId)
        objParam(2) = param
        param = New SqlParameter("@ActivityUserId", objToUpdate.ActivityUserId)
        objParam(3) = param
        param = New SqlParameter("@ActivityGroupId", objToUpdate.ActivityGroupId)
        objParam(4) = param
        param = New SqlParameter("@Action", objToUpdate.Action)
        objParam(5) = param
        param = New SqlParameter("@Review", objToUpdate.Review)
        objParam(6) = param
        param = New SqlParameter("@TranPath", objToUpdate.TranPath)
        objParam(7) = param
        param = New SqlParameter("@TransactionStatus", objToUpdate.TransactionStatus)
        objParam(8) = param
        param = New SqlParameter("@Templateid", objToUpdate.Templateid)
        objParam(9) = param
        param = New SqlParameter("@Notification", objToUpdate.Notification)
        objParam(10) = param
        param = New SqlParameter("@itemid", objToUpdate.itemid)
        objParam(11) = param
        param = New SqlParameter("@FileType", objToUpdate.FileType)
        objParam(12) = param
        param = New SqlParameter("@SkipTo", objToUpdate.SkipTo)
        objParam(13) = param
        param = New SqlParameter("@FromMail", objToUpdate.FromMail)
        objParam(14) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(15) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(16) = param
        param = New SqlParameter("@RequestType", objToUpdate.RequestType)
        objParam(17) = param
        param = New SqlParameter("@Transactionid", objToUpdate.Transactionid)
        objParam(18) = param
        param = New SqlParameter("@UserType", objToUpdate.UserType)
        objParam(19) = param
        param = New SqlParameter("@Attachment", objToUpdate.Attachment)
        objParam(20) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZWFlowTransation)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZWFlowTransation set Isdeleted=1 where Transactionid=@Transactionid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Transactionid", objToDelete.Transactionid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZWFlowTransation() As System.Collections.Generic.List(Of IeZWFlowTransation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFlowTransation)()
        Dim objItem As IeZWFlowTransation
        Try
            Dim strQry As String = ""
            strQry = "Select Transactionid From eZWFlowTransation where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowTransation")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFlowTransation(GetInteger(sqlRdr("Transactionid")))
                objItem.Transactionid = GetInteger(sqlRdr("Transactionid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZWFlowTransation(Criteria As String, Value As String) As List(Of IeZWFlowTransation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFlowTransation)()
        Dim objItem As IeZWFlowTransation
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Transactionid From eZWFlowTransation where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Transactionid"
            Else
                strQry = "Select Transactionid From eZWFlowTransation where Isdeleted=0 order by Transactionid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowTransation")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFlowTransation(GetInteger(sqlRdr("Transactionid")))
                objItem.Transactionid = GetInteger(sqlRdr("Transactionid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZWFlowTransation(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZWFlowTransation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFlowTransation)()
        Dim objItem As IeZWFlowTransation
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Transactionid From eZWFlowTransation where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Transactionid"
            Else
                strQry = "Select Transactionid From eZWFlowTransation where Isdeleted=0 order by Transactionid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowTransation")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFlowTransation(GetInteger(sqlRdr("Transactionid")))
                objItem.Transactionid = GetInteger(sqlRdr("Transactionid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function


    Public Function ReadInboxListbyUserid(WorkflowId As String, ECMLoginId As String, ECMGroupList As String, RowFrom As Integer, RowCount As Integer) As System.Collections.Generic.List(Of IeZWFlowTransation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFlowTransation)()
        Dim objItem As IeZWFlowTransation
        Try
            Dim strQry As String = ""


            strQry = "select Transactionid from ezwflowtransation Trans,eZWFProcess WP where Trans.Processid =wp.ProcessId and wp.WorkflowId='" + WorkflowId + "' and wp.FlowStatus='Running' and TransactionStatus=0 and ActivityId<>'a5d0b578-3ded-40bb-8770-2a5cef442b55' and ((trans.ActivityUserId ='" + ECMLoginId + "' and ActivityGroupId='0')"

            If ECMGroupList <> "" Then
                strQry += "or (trans.ActivityGroupId in (" + ECMGroupList + ") and ActivityUserId='0')"
            End If
            strQry = strQry & " ) "
            If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                If RowCount <> 0 Then
                    strQry = strQry & " order by Transactionid desc OFFSET " + RowFrom.ToString() + " ROWS FETCH NEXT " + RowCount.ToString() + " ROWS ONLY"
                End If
            Else
                If RowCount <> 0 Then
                    strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                End If
            End If

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowTransation" + strQry.ToString())
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFlowTransation(GetInteger(sqlRdr("Transactionid")))
                objItem.Transactionid = GetInteger(sqlRdr("Transactionid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadQueueListbyUserid(WorkflowId As String, ECMLoginId As String, ECMGroupList As String, RowFrom As Integer, RowCount As Integer) As System.Collections.Generic.List(Of IeZWFlowTransation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFlowTransation)()
        Dim objItem As IeZWFlowTransation
        Try
            Dim strQry As String = ""


            strQry = "select Transactionid from ezwflowtransation Trans,eZWFProcess WP where Trans.Processid =wp.ProcessId and wp.WorkflowId='" + WorkflowId + "' and wp.FlowStatus='Running' and TransactionStatus=0 and ActivityId='a5d0b578-3ded-40bb-8770-2a5cef442b55' and ((trans.ActivityUserId ='" + ECMLoginId + "' and ActivityGroupId='0')"

            If ECMGroupList <> "" Then
                strQry += "or (trans.ActivityGroupId in (" + ECMGroupList + ") and ActivityUserId='0')"
            End If
            strQry = strQry & " ) "
            If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                If RowCount <> 0 Then
                    strQry = strQry & " order by Transactionid desc OFFSET " + RowFrom.ToString() + " ROWS FETCH NEXT " + RowCount.ToString() + " ROWS ONLY"
                End If
            Else
                If RowCount <> 0 Then
                    strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                End If
            End If

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowTransation" + strQry.ToString())
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFlowTransation(GetInteger(sqlRdr("Transactionid")))
                objItem.Transactionid = GetInteger(sqlRdr("Transactionid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function


    Public Function ReadInboxListbyUseridWithSearchby(WorkflowId As String, ECMLoginId As String, ECMGroupList As String, fieldname As String, fieldvalue As String, RowFrom As Integer, RowCount As Integer) As System.Collections.Generic.List(Of IeZWFlowTransation)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZWFlowTransation)()
        Dim objItem As IeZWFlowTransation
        Try
            Dim strQry As String = ""
            Dim Formtablename As String = ""

            Formtablename = GetFormtablenameByworkflowid(WorkflowId)


            If fieldname = "TicketNo" Then
                fieldname = "RequestNo"
            End If
            If fieldname = "RaisedOn" Or fieldname = "RaisedBy" Or fieldname = "RequestNo" Then
                If fieldname = "RaisedOn" Then
                    fieldname = "Createdon"
                End If
                If fieldname = "RaisedBy" Then
                    fieldname = "Createdby"
                End If
                strQry = "select distinct Transactionid from ezwflowtransation Trans,eZWFProcess WP where Trans.Processid =wp.ProcessId and wp.WorkflowId='" + WorkflowId + "' and wp.FlowStatus='Running' and TransactionStatus=0 and ((trans.ActivityUserId ='" + ECMLoginId + "' and ActivityGroupId='0')"

                If ECMGroupList <> "" Then
                    strQry += "or (trans.ActivityGroupId in (" + ECMGroupList + ") and ActivityUserId='0')"
                End If
                strQry = strQry & " ) and wp." + fieldname + " like '%" + fieldvalue + "%' order by Transactionid desc"

                Dim Isowner As Boolean = CheckUserAsOwner(WorkflowId, ECMLoginId)
                If Isowner Then
                    strQry = "select distinct Transactionid from ezwflowtransation Trans,eZWFProcess WP where Trans.Processid =wp.ProcessId and wp.WorkflowId='" + WorkflowId + "' and wp.FlowStatus='Running' and TransactionStatus=0 and wp." + fieldname + " like '%" + fieldvalue + "%' order by Transactionid desc"

                End If
            Else
                strQry = "select distinct Transactionid from ezwflowtransation Trans,eZWFProcess WP left join " + Formtablename + " formtbl on  formtbl.TicketNo =wp.RequestNo where Trans.Processid =wp.ProcessId and wp.WorkflowId='" + WorkflowId + "' and wp.FlowStatus='Running' and TransactionStatus=0 and ((trans.ActivityUserId ='" + ECMLoginId + "' and ActivityGroupId='0')"

                If ECMGroupList <> "" Then
                    strQry += "or (trans.ActivityGroupId in (" + ECMGroupList + ") and ActivityUserId='0')"
                End If
                strQry = strQry & " ) and formtbl." + fieldname + " like '%" + fieldvalue + "%' order by Transactionid desc"

                Dim Isowner As Boolean = CheckUserAsOwner(WorkflowId, ECMLoginId)
                If Isowner Then
                    strQry = "select distinct Transactionid from ezwflowtransation Trans,eZWFProcess WP left join " + Formtablename + " formtbl on  formtbl.TicketNo =wp.RequestNo where Trans.Processid =wp.ProcessId and wp.WorkflowId='" + WorkflowId + "' and wp.FlowStatus='Running' and TransactionStatus=0 and formtbl." + fieldname + " like '%" + fieldvalue + "%' order by Transactionid desc"
                End If
            End If

            If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                If RowCount <> 0 Then
                    strQry = strQry & "  OFFSET " + RowFrom.ToString() + " ROWS FETCH NEXT " + RowCount.ToString() + " ROWS ONLY"
                End If
            Else
                If RowCount <> 0 Then
                    strQry = "SELECT distinct Transactionid FROM (" + strQry.Replace("select distinct Transactionid", "select distinct Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                End If

            End If

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZWFlowTransation" + " : " + strQry)
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZWFlowTransation(GetInteger(sqlRdr("Transactionid")))
                objItem.Transactionid = GetInteger(sqlRdr("Transactionid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Private Function CheckUserAsOwner(WorkflowId As String, EcmLoginid As String) As Boolean
        Dim result As Boolean = False
        Try
            Dim sqlRdr1 As SqlDataReader = Nothing
            Dim objParam1 As SqlParameter()
            Dim strQry1 As String = "select * from eZWorkflowUsers where (Usertype = ('Owner') or Usertype = ('Co-Ordinator'))  and  WorkflowId =@WorkflowId and ECMLoginId = @ECMLoginId"
            Dim obj1 As Object = ""
            objParam1 = New SqlParameter(1) {}
            Dim param1 As SqlParameter
            param1 = New SqlParameter("@WorkflowId", WorkflowId.ToString())
            objParam1(0) = param1
            Dim param2 As SqlParameter
            param2 = New SqlParameter("@ECMLoginId", EcmLoginid.ToString())
            objParam1(1) = param2
            'param1 = New SqlParameter("@Fields", xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString())
            'objParam1(1) = param1
            obj1 = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry1.ToString(), objParam1)
            If obj1 Is Nothing Then
                Throw New Exception("Attempt to read Invalid Formtable")
            End If
            sqlRdr1 = DirectCast(obj1, SqlDataReader)
            If sqlRdr1.Read() Then
                result = True
            End If

        Catch ex As Exception
            result = False
        End Try
        Return result
    End Function

    Private Function GetFormtablenameByworkflowid(WorkflowId As String) As String
        Dim result As String = ""
        Try
            Dim sqlRdr1 As SqlDataReader = Nothing
            Dim objParam1 As SqlParameter()
            Dim strQry1 As String = "select tablename from ezwflowformdetails where WorkflowId =@WorkflowId"
            Dim obj1 As Object = ""
            objParam1 = New SqlParameter(0) {}
            Dim param1 As SqlParameter
            param1 = New SqlParameter("@WorkflowId", WorkflowId.ToString())
            objParam1(0) = param1
            'param1 = New SqlParameter("@Fields", xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString())
            'objParam1(1) = param1
            obj1 = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry1.ToString(), objParam1)
            If obj1 Is Nothing Then
                Throw New Exception("Attempt to read Invalid Formtable")
            End If
            sqlRdr1 = DirectCast(obj1, SqlDataReader)
            If sqlRdr1.Read() Then
                result = sqlRdr1("tablename").ToString()
            End If
        Catch ex As Exception
            result = ""
        End Try
        Return result
    End Function

    Public Function InsertandUpdateWorkflowtransaction(OBJEMP As eZWFlowTransation) As String
        Try
            Dim exc As String = ""
            Dim param As String()
            If String.IsNullOrEmpty(OBJEMP.Updatedon) Then
                OBJEMP.Updatedon = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.ActivityGroupId) Then
                OBJEMP.ActivityGroupId = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.ActivityUserId) Then
                OBJEMP.ActivityUserId = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.Createdon) Then
                OBJEMP.Createdon = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.ActivityId) Then
                OBJEMP.ActivityId = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.RuleId) Then
                OBJEMP.RuleId = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.Action) Then
                OBJEMP.Action = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.Review) Then
                OBJEMP.Review = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.TranPath) Then
                OBJEMP.TranPath = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.templateid) Then
                OBJEMP.templateid = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.itemid) Then
                OBJEMP.itemid = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.FileType) Then
                OBJEMP.FileType = "0"
            End If
            If String.IsNullOrEmpty(OBJEMP.SkipTo) Then
                OBJEMP.SkipTo = ""
            End If
            If String.IsNullOrEmpty(OBJEMP.FromMail) Then
                OBJEMP.FromMail = ""
            End If
            If String.IsNullOrEmpty(OBJEMP.RequestType) Then
                OBJEMP.RequestType = 0
            End If
            If String.IsNullOrEmpty(OBJEMP.UserType) Then
                OBJEMP.UserType = ""
            End If
            param = {OBJEMP.Transactionid.ToString(), OBJEMP.Processid.ToString(), OBJEMP.Action.ToString(), OBJEMP.ActivityId.ToString(),
                OBJEMP.ActivityUserId.ToString(), OBJEMP.ActivityGroupId.ToString(), OBJEMP.Review.ToString(), OBJEMP.TranPath.ToString(),
                OBJEMP.TransactionStatus.ToString(), OBJEMP.templateid.ToString(), OBJEMP.itemid.ToString(), OBJEMP.notification.ToString(),
                OBJEMP.FileType.ToString(), OBJEMP.Createdon.ToString(), OBJEMP.Updatedon.ToString(), OBJEMP.Createdby.ToString(),
                OBJEMP.Updatedby.ToString(), OBJEMP.RuleId.ToString(), OBJEMP.SkipTo.ToString(), OBJEMP.FromMail.ToString(), OBJEMP.RequestType.ToString(),
                OBJEMP.UserType}
            If OBJEMP.Transactionid <> 0 Then
                exc = DBLayer.DBLInstance.InsertandUpdateStoredProcedure("SP_InsertandUpdateeZWFlowTransation", param)
            Else
                Dim ds As New DataSet
                ds = DBLayer.DBLInstance.GetDatasetByStoredProcedureName("SP_InsertandUpdateeZWFlowTransation", param)
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


