Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZOutlookDetail)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZOutlookDetail ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Outlookdetailid=@Outlookdetailid and ez.Isdeleted=0"
            param = New SqlParameter("@Outlookdetailid", objRead.Outlookdetailid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZOutlookDetail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.itemid = GetInteger(sqlRdr("itemid"))
                objRead.Outlookdetailid = GetInteger(sqlRdr("Outlookdetailid"))
                objRead.templateid = GetInteger(sqlRdr("TemplateId"))
                objRead.ConversationIndex = sqlRdr("ConversationIndex").ToString
                objRead.EntryId = sqlRdr("EntryId").ToString
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
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
    Public Function CreateeZOutlookDetail(objEmp As eZOutlookDetail) As eZOutlookDetail
        Dim newObject As eZOutlookDetail = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZOutlookDetail(ConversationIndex,TemplateId,EntryId,itemid,CreatedBy,CreatedOn) VALUES " +
                "(@ConversationIndex,@TemplateId,@EntryId,@itemid,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@ConversationIndex", objEmp.ConversationIndex)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objEmp.templateid)
            objParam(1) = param
            param = New SqlParameter("@EntryId", objEmp.EntryId)
            objParam(2) = param
            param = New SqlParameter("@itemid", objEmp.itemid)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(5) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZOutlookDetail(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZOutlookDetail)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZOutlookDetail Set ConversationIndex=@ConversationIndex,TemplateId=@TemplateId,EntryId=@EntryId,itemid=@itemid," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where Outlookdetailid=@Outlookdetailid"
        objParam = New SqlParameter(6) {}
        param = New SqlParameter("@ConversationIndex", objToUpdate.ConversationIndex)
        objParam(0) = param
        param = New SqlParameter("@TemplateId", objToUpdate.templateid)
        objParam(1) = param
        param = New SqlParameter("@EntryId", objToUpdate.EntryId)
        objParam(2) = param
        param = New SqlParameter("@itemid", objToUpdate.itemid)
        objParam(3) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(4) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(5) = param
        param = New SqlParameter("@Outlookdetailid", objToUpdate.Outlookdetailid)
        objParam(6) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZOutlookDetail)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZOutlookDetail set Isdeleted=1 where Outlookdetailid=@Outlookdetailid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Outlookdetailid", objToDelete.Outlookdetailid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZOutlookDetail() As System.Collections.Generic.List(Of IeZOutlookDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZOutlookDetail)()
        Dim objItem As IeZOutlookDetail
        Try
            Dim strQry As String = ""
            strQry = "Select Outlookdetailid From eZOutlookDetail where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZOutlookDetail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZOutlookDetail(GetInteger(sqlRdr("Outlookdetailid")))
                objItem.Outlookdetailid = GetInteger(sqlRdr("Outlookdetailid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZOutlookDetail(Criteria As String, Value As String) As List(Of IeZOutlookDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZOutlookDetail)()
        Dim objItem As IeZOutlookDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Outlookdetailid From eZOutlookDetail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Outlookdetailid"
            Else
                strQry = "Select Outlookdetailid From eZOutlookDetail where Isdeleted=0 order by Outlookdetailid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZOutlookDetail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZOutlookDetail(GetInteger(sqlRdr("Outlookdetailid")))
                objItem.Outlookdetailid = GetInteger(sqlRdr("Outlookdetailid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZOutlookDetail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZOutlookDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZOutlookDetail)()
        Dim objItem As IeZOutlookDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Outlookdetailid From eZOutlookDetail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Outlookdetailid"
            Else
                strQry = "Select Outlookdetailid From eZOutlookDetail where Isdeleted=0 order by Outlookdetailid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZOutlookDetail")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZOutlookDetail(GetInteger(sqlRdr("Outlookdetailid")))
                objItem.Outlookdetailid = GetInteger(sqlRdr("Outlookdetailid"))
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
