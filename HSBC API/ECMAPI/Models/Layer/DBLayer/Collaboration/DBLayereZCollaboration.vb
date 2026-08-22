Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZCollaboration)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZCollaboration ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.CollId=@CollId and ez.Isdeleted=0"
            param = New SqlParameter("@CollId", objRead.CollId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCollaboration")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.CollId = GetInteger(sqlRdr("CollId"))
                objRead.CollName = sqlRdr("CollName").ToString
                objRead.itemid = GetInteger(sqlRdr("itemid"))
                objRead.Templateid = GetInteger(sqlRdr("Templateid"))
                objRead.XMLHistorypath = sqlRdr("XMLHistorypath").ToString
                objRead.XMLPath = sqlRdr("XMLPath").ToString
                objRead.Status = sqlRdr("Status").ToString
                objRead.StartDateTime = sqlRdr("StartDateTime").ToString
                objRead.EndDateTime = sqlRdr("EndDateTime").ToString
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
    Public Function CreateeZCollaboration(objEmp As eZCollaboration) As eZCollaboration
        Dim newObject As eZCollaboration = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZCollaboration(CollName,itemid,Templateid,XMLHistorypath,XMLPath,Status,StartDateTime,EndDateTime," +
                "CreatedBy,CreatedOn) VALUES (@CollName,@itemid,@Templateid,@XMLHistorypath,@XMLPath,@Status,@StartDateTime,@EndDateTime," +
                "@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(9) {}
            param = New SqlParameter("@CollName", objEmp.CollName)
            objParam(0) = param
            param = New SqlParameter("@itemid", objEmp.itemid)
            objParam(1) = param
            param = New SqlParameter("@Templateid", objEmp.Templateid)
            objParam(2) = param
            param = New SqlParameter("@XMLHistorypath", objEmp.XMLHistorypath)
            objParam(3) = param
            param = New SqlParameter("@XMLPath", objEmp.XMLPath)
            objParam(4) = param
            param = New SqlParameter("@Status", objEmp.Status)
            objParam(5) = param
            param = New SqlParameter("@StartDateTime", objEmp.StartDateTime)
            objParam(6) = param
            param = New SqlParameter("@EndDateTime", objEmp.EndDateTime)
            objParam(7) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(8) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(9) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZCollaboration(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZCollaboration)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZCollaboration Set CollName=@CollName,itemid=@itemid,Templateid=@Templateid,XMLHistorypath=@XMLHistorypath,XMLPath=@XMLPath," +
            "Status=@Status,StartDateTime=@StartDateTime,EndDateTime=@EndDateTime,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where CollId=@CollId"
        objParam = New SqlParameter(10) {}
        param = New SqlParameter("@CollName", objToUpdate.CollName)
        objParam(0) = param
        param = New SqlParameter("@itemid", objToUpdate.itemid)
        objParam(1) = param
        param = New SqlParameter("@Templateid", objToUpdate.Templateid)
        objParam(2) = param
        param = New SqlParameter("@XMLHistorypath", objToUpdate.XMLHistorypath)
        objParam(3) = param
        param = New SqlParameter("@XMLPath", objToUpdate.XMLPath)
        objParam(4) = param
        param = New SqlParameter("@Status", objToUpdate.Status)
        objParam(5) = param
        param = New SqlParameter("@StartDateTime", objToUpdate.StartDateTime)
        objParam(6) = param
        param = New SqlParameter("@EndDateTime", objToUpdate.EndDateTime)
        objParam(7) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(8) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(9) = param
        param = New SqlParameter("@CollId", objToUpdate.CollId)
        objParam(10) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZCollaboration)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZCollaboration set Isdeleted=1 where CollId=@CollId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@CollId", objToDelete.CollId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZCollaboration() As System.Collections.Generic.List(Of IeZCollaboration)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCollaboration)()
        Dim objItem As IeZCollaboration
        Try
            Dim strQry As String = ""
            strQry = "Select CollId From eZCollaboration where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCollaboration")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCollaboration(GetInteger(sqlRdr("CollId")))
                objItem.CollId = GetInteger(sqlRdr("CollId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZCollaboration(Criteria As String, Value As String) As List(Of IeZCollaboration)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCollaboration)()
        Dim objItem As IeZCollaboration
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select CollId From eZCollaboration where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by CollId"
            Else
                strQry = "Select CollId From eZCollaboration where Isdeleted=0 order by CollId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCollaboration")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCollaboration(GetInteger(sqlRdr("CollId")))
                objItem.CollId = GetInteger(sqlRdr("CollId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZCollaboration(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZCollaboration)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCollaboration)()
        Dim objItem As IeZCollaboration
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select CollId From eZCollaboration where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CollId"
            Else
                strQry = "Select CollId From eZCollaboration where Isdeleted=0 order by CollId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCollaboration")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCollaboration(GetInteger(sqlRdr("CollId")))
                objItem.CollId = GetInteger(sqlRdr("CollId"))
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
