Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateInboxTransaction(objEmp As eZInboxTransaction) As IeZInboxTransaction
        Dim newObject As IeZInboxTransaction = Nothing

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZInboxTransaction(Itemid,TemplateId,ProcessId,URL,FromUserId,ToUserId,Status,CreatedOn,CreatedBy) VALUES(@Itemid,@TemplateId,@ProcessId,@URL,@FromUserId,@ToUserId,@Status,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@Itemid", objEmp.Itemid)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@ProcessId", objEmp.ProcessId)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            param = New SqlParameter("@URL", objEmp.URL)
            objParam(5) = param
            param = New SqlParameter("@FromUserId", objEmp.FromUserId)
            objParam(6) = param
            param = New SqlParameter("@ToUserId", objEmp.ToUserId)
            objParam(7) = param
            param = New SqlParameter("@Status", objEmp.Status)
            objParam(8) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZInboxTransaction(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Read(objRead As IeZInboxTransaction)
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
            If objRead.Itemid = 0 Then
                strQry = "Select *,dbo.udf_UserName(FromUserId) as FromUser,dbo.udf_UserName(ToUserId) as ToUser,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZInboxTransaction Where InboxId=@InboxId and  Isdeleted=0"
                param = New SqlParameter("@InboxId", objRead.InboxId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(FromUserId) as FromUser,dbo.udf_UserName(ToUserId) as ToUser,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZInboxTransaction Where Itemid=@Itemid and Isdeleted=0"
                param = New SqlParameter("@Itemid", objRead.Itemid)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid InboxId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.InboxId = GetInteger(sqlRdr("InboxId"))
                objRead.ItemId = GetInteger(sqlRdr("ItemId"))
                objRead.ProcessId = GetInteger(sqlRdr("ProcessId"))
                objRead.FromUserId = GetInteger(sqlRdr("FromUserId"))
                objRead.ToUserId = GetInteger(sqlRdr("ToUserId"))
                objRead.FromUser = sqlRdr("FromUser").ToString()
                objRead.ToUser = sqlRdr("ToUser").ToString()
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.Status = sqlRdr("Status").ToString()
                objRead.URL = sqlRdr("URL").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString()
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
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
    Public Function ReadAllInboxTransaction() As System.Collections.Generic.List(Of IeZInboxTransaction)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZInboxTransaction)()
        Dim objItem As IeZInboxTransaction

        Try
            Dim strQry As String = ""
            strQry = "Select InboxId From eZInboxTransaction where Isdeleted=0 order by InboxId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid InboxId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZInboxTransaction(GetInteger(sqlRdr("InboxId")))
                objItem.InboxId = GetInteger(sqlRdr("InboxId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedInboxTransaction1(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZInboxTransaction)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZInboxTransaction)()
        Dim objItem As IeZInboxTransaction
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select InboxId From eZInboxTransaction where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select InboxId From eZInboxTransaction where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZInboxTransaction.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZInboxTransaction(GetSmallInterger(sqlRdr("InboxId")))
                objItem.InboxId = GetSmallInterger(sqlRdr("InboxId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedInboxTransaction1WithCondition(condition As String) As System.Collections.Generic.List(Of IeZInboxTransaction)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZInboxTransaction)()
        Dim objItem As IeZInboxTransaction
        Try
            Dim strQry As String = ""
            strQry = "Select InboxId From eZInboxTransaction where Isdeleted=0 and " + condition.ToString

            strQry = strQry & " order by CreatedOn"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZInboxTransaction.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZInboxTransaction(GetSmallInterger(sqlRdr("InboxId")))
                objItem.InboxId = GetSmallInterger(sqlRdr("InboxId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedInboxTransaction(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZInboxTransaction)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZInboxTransaction)()
        Dim objItem As IeZInboxTransaction
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select InboxId From eZInboxTransaction where Isdeleted=0 and FromUserId=" + Unquote(Value) + " or ToUserId=" + Unquote(Value) + " or "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select InboxId From eZInboxTransaction where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZInboxTransaction(GetSmallInterger(sqlRdr("InboxId")))
                objItem.InboxId = GetSmallInterger(sqlRdr("InboxId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function


    Public Sub Update(objToUpdate As IeZInboxTransaction)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZInboxTransaction Set ItemId=@ItemId,TemplateId=@TemplateId,ProcessId=@ProcessId,URL=@URL,FromUserId=@FromUserId,ToUserId=@ToUserId,Status=@Status where InboxId=@InboxId"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@Itemid", objToUpdate.Itemid)
        objParam(0) = param
        param = New SqlParameter("@InboxId", objToUpdate.InboxId)
        objParam(1) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(2) = param
        param = New SqlParameter("@ProcessId", objToUpdate.ProcessId)
        objParam(3) = param
        param = New SqlParameter("@FromUserId", objToUpdate.FromUserId)
        objParam(4) = param
        param = New SqlParameter("@ToUserId", objToUpdate.ToUserId)
        objParam(5) = param
        param = New SqlParameter("@Status", objToUpdate.Status)
        objParam(6) = param
        param = New SqlParameter("@URL", objToUpdate.URL)
        objParam(7) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZInboxTransaction)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZInboxTransaction set Isdeleted=1 where InboxId=@InboxId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@InboxId", objToDelete.InboxId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class