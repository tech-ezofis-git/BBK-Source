Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZProcessItems)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From ezprocessitems " +
                "Where processitemsid=@processitemsid and Isdeleted=0"
            param = New SqlParameter("@processitemsid", objRead.ProcessItemsId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Process Items")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ProcessItemsId = GetInteger(sqlRdr("processitemsid"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.ItemId = GetInteger(sqlRdr("ItemId"))
                objRead.ProcessId = GetInteger(sqlRdr("ProcessId"))
                objRead.FormEntryId = GetInteger(sqlRdr("FormEntryId"))
                objRead.FormId = GetInteger(sqlRdr("FormId"))
                objRead.Workflowid = GetInteger(sqlRdr("workflowid"))
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
                objRead.Createdby1 = sqlRdr("CreatedBy1").ToString()
                objRead.Updatedby1 = sqlRdr("UpdatedBy1").ToString()
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

    Public Function CreateeZProcessItems(objEmp As eZProcessItems) As eZProcessItems
        Dim newObject As eZProcessItems = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezprocessitems(templateid,itemid,ProcessId,Workflowid,FormId,FormEntryId,CreatedBy,CreatedOn) VALUES" +
                "(@templateid,@itemid,@ProcessId,@Workflowid,@FormId,@FormEntryId,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(0) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(1) = param
            param = New SqlParameter("@templateid", objEmp.TemplateId)
            objParam(2) = param
            param = New SqlParameter("@itemid", objEmp.ItemId)
            objParam(3) = param
            param = New SqlParameter("@ProcessId", objEmp.ProcessId)
            objParam(4) = param
            param = New SqlParameter("@Workflowid", objEmp.Workflowid)
            objParam(5) = param
            param = New SqlParameter("@FormId", objEmp.FormId)
            objParam(6) = param
            param = New SqlParameter("@FormEntryId", objEmp.FormEntryId)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            'obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZProcessItems(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZProcessItems)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezprocessitems Set templateid=@templateid,itemid=@itemid,ProcessId=@ProcessId,Workflowid=@Workflowid,FormId=@FormId," +
            "FormEntryId=@FormEntryId,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where hidefileid=@hidefileid"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@Updatedby", objToUpdate.Updatedby)
        objParam(0) = param
        param = New SqlParameter("@Updatedon", objToUpdate.Updatedon)
        objParam(1) = param
        param = New SqlParameter("@templateid", objToUpdate.TemplateId)
        objParam(2) = param
        param = New SqlParameter("@itemid", objToUpdate.ItemId)
        objParam(3) = param
        param = New SqlParameter("@ProcessId", objToUpdate.ProcessId)
        objParam(4) = param
        param = New SqlParameter("@Workflowid", objToUpdate.Workflowid)
        objParam(5) = param
        param = New SqlParameter("@FormId", objToUpdate.FormId)
        objParam(6) = param
        param = New SqlParameter("@FormEntryId", objToUpdate.FormEntryId)
        objParam(7) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZProcessItems)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezprocessitems set Isdeleted=1 where processitemsid=@processitemsid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@processitemsid", objToDelete.ProcessItemsId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region

    Public Function ReadAlleZProcessItems() As System.Collections.Generic.List(Of IeZProcessItems)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZProcessItems)()
        Dim objItem As IeZProcessItems
        Try
            Dim strQry As String = ""
            strQry = "Select processitemsid From ezprocessitems where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Process Items")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZProcessItems(GetInteger(sqlRdr("processitemsid")))
                objItem.ProcessItemsId = GetInteger(sqlRdr("processitemsid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadFilteredeZProcessItems(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZProcessItems)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZProcessItems)()
        Dim objItem As IeZProcessItems
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select processitemsid From ezprocessitems where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by processitemsid"
            Else
                strQry = "Select processitemsid From ezprocessitems where Isdeleted=0 order by processitemsid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Process Items.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZProcessItems(GetInteger(sqlRdr("processitemsid")))
                objItem.ProcessItemsId = GetInteger(sqlRdr("processitemsid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZProcessItems(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZProcessItems)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZProcessItems)()
        Dim objItem As IeZProcessItems
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select processitemsid From ezprocessitems where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by processitemsid"
            Else
                strQry = "Select processitemsid From ezprocessitems where Isdeleted=0 order by processitemsid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Process Items.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZProcessItems(GetInteger(sqlRdr("processitemsid")))
                objItem.ProcessItemsId = GetInteger(sqlRdr("processitemsid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

End Class
