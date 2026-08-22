Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateFaxTransaction(objEmp As eZFaxTransaction) As IeZFaxTransaction
        Dim newObject As IeZFaxTransaction = Nothing
     
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZFaxTransaction(Itemid,FromAdd,ToAdd,IsRead,Subject,FromName,DisplayFrom,IsArchived,FaxReceiverRuleId,CreatedOn,CreatedBy,DocType) VALUES(@Itemid,@FromAdd,@ToAdd,@IsRead,@Subject,@FromName,@DisplayFrom,@IsArchived,@FaxReceiverRuleId,@CreatedOn,@CreatedBy,@DocType);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(11) {}
            param = New SqlParameter("@Itemid", objEmp.Itemid)
            objParam(0) = param
            param = New SqlParameter("@FromAdd", objEmp.FromAdd)
            objParam(1) = param
            param = New SqlParameter("@ToAdd", objEmp.ToAdd)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            param = New SqlParameter("@IsRead", objEmp.IsRead)
            objParam(5) = param
            param = New SqlParameter("@Subject", objEmp.Subject)
            objParam(6) = param
            param = New SqlParameter("@DisplayFrom", objEmp.DisplayFrom)
            objParam(7) = param
            param = New SqlParameter("@FromName", objEmp.FromName)
            objParam(8) = param
            param = New SqlParameter("@IsArchived", objEmp.IsArchived)
            objParam(9) = param
            param = New SqlParameter("@FaxReceiverRuleId", objEmp.FaxReceiverRuleId)
            objParam(10) = param
            param = New SqlParameter("@DocType", objEmp.DocType)
            objParam(11) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZFaxTransaction(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub Read(objRead As IeZFaxTransaction)
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
                strQry = "Select *,dbo.udf_ifilepath(Itemid) as FilePath,dbo.udf_FaxNumber(Itemid) as FAXNUMBER,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFaxTransaction Where IsExpired=0 and FaxTransactionId=@FaxTransaction_ID and  Isdeleted=0"
                param = New SqlParameter("@FaxTransaction_ID", objRead.FaxTransactionId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_ifilepath(Itemid) as FilePath,dbo.udf_FaxNumber(Itemid) as FAXNUMBER,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFaxTransaction Where IsExpired=0 and Itemid=@Itemid and Isdeleted=0"
                param = New SqlParameter("@Itemid", objRead.Itemid)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Itemid.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.FaxTransactionId = GetInteger(sqlRdr("FaxTransactionId"))
                objRead.FaxReceiverRuleId = GetInteger(sqlRdr("FaxReceiverRuleId"))
                objRead.Itemid = GetInteger(sqlRdr("Itemid"))
                objRead.FromAdd = GetInteger(sqlRdr("FromAdd"))
                objRead.IsExpired = GetInteger(sqlRdr("IsExpired"))
                objRead.ArchivedItemid = GetInteger(sqlRdr("ArchivedItemid"))
                objRead.ArchivedTemplateId = GetInteger(sqlRdr("ArchivedTemplateId"))
                objRead.ToAdd = GetInteger(sqlRdr("ToAdd"))
                objRead.IsRead = sqlRdr("IsRead").ToString()
                objRead.IsArchived = sqlRdr("IsArchived").ToString()
                objRead.FAXNUMBER = sqlRdr("FAXNUMBER").ToString()
                objRead.Subject = sqlRdr("Subject").ToString()
                objRead.DocType = sqlRdr("DocType").ToString()
                objRead.DisplayFrom = sqlRdr("DisplayFrom").ToString()
                objRead.FromName = sqlRdr("FromName").ToString()
                If objRead.IsArchived = True Then
                    objRead.FilePath = GetValueFromeZUserDefinedByField(2, objRead.ArchivedTemplateId, objRead.ArchivedItemid, "ifilepath")
                    objRead.FilePath = objRead.FilePath + "\" + GetValueFromeZUserDefinedByField(2, objRead.ArchivedTemplateId, objRead.ArchivedItemid, "ifilename")
                Else
                    objRead.FilePath = sqlRdr("FilePath").ToString()
                End If
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString()
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                objRead.ArchivedBy = sqlRdr("ArchivedBy").ToString()
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
    Public Function ReadAllFaxTransaction() As System.Collections.Generic.List(Of IeZFaxTransaction)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxTransaction)()
        Dim objItem As IeZFaxTransaction

        Try
            Dim strQry As String = ""
            strQry = "Select FaxTransactionId From eZFaxTransaction where IsExpired=0 and Isdeleted=0 order by Itemid"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Itemid.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxTransaction(GetInteger(sqlRdr("FaxTransactionId")))
                objItem.FaxTransactionId = GetInteger(sqlRdr("FaxTransactionId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedFaxTransaction1(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFaxTransaction)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxTransaction)()
        Dim objItem As IeZFaxTransaction
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FaxTransactionId From eZFaxTransaction where IsExpired=0 and Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select FaxTransactionId From eZFaxTransaction where IsExpired=0 and Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxTransaction(GetSmallInterger(sqlRdr("FaxTransactionId")))
                objItem.FaxTransactionId = GetSmallInterger(sqlRdr("FaxTransactionId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedFaxTransaction1WithECMLoginId(condition As String) As System.Collections.Generic.List(Of IeZFaxTransaction)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxTransaction)()
        Dim objItem As IeZFaxTransaction
        Try
            Dim strQry As String = ""
            strQry = "Select FaxTransactionId From eZFaxTransaction where IsExpired=0 and " + condition.ToString + " and Isdeleted=0 "

            strQry = strQry & " order by CreatedOn"
           
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxTransaction(GetSmallInterger(sqlRdr("FaxTransactionId")))
                objItem.FaxTransactionId = GetSmallInterger(sqlRdr("FaxTransactionId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedFaxTransaction(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFaxTransaction)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxTransaction)()
        Dim objItem As IeZFaxTransaction
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FaxTransactionId From eZFaxTransaction where IsExpired=0 and  Isdeleted=0 and FromAdd=" + Unquote(Value) + " or ToAdd=" + Unquote(Value) + " or "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select FaxTransactionId From eZFaxTransaction where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxTransaction(GetSmallInterger(sqlRdr("FaxTransactionId")))
                objItem.FaxTransactionId = GetSmallInterger(sqlRdr("FaxTransactionId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function


    Public Sub Update(objToUpdate As IeZFaxTransaction)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFaxTransaction Set ArchivedBy=@ArchivedBy,ArchivedItemid=@ArchivedItemid,ArchivedTemplateId=@ArchivedTemplateId,IsExpired=@IsExpired,IsArchived=@IsArchived,DocType=@DocType,Subject=@Subject,DisplayFrom=@DisplayFrom,FromName=@FromName,IsRead=@IsRead,FromAdd=@FromAdd,ToAdd=@ToAdd where FaxTransactionId=@FaxTransaction_ID"
        objParam = New SqlParameter(9) {}
        param = New SqlParameter("@Itemid", objToUpdate.Itemid)
        objParam(0) = param
        param = New SqlParameter("@FaxTransaction_ID", objToUpdate.FaxTransactionId)
        objParam(1) = param
        param = New SqlParameter("@FromAdd", objToUpdate.FromAdd)
        objParam(2) = param
        param = New SqlParameter("@ToAdd", objToUpdate.ToAdd)
        objParam(3) = param
        param = New SqlParameter("@IsRead", objToUpdate.IsRead)
        objParam(4) = param
        param = New SqlParameter("@Subject", objToUpdate.Subject)
        objParam(5) = param
        param = New SqlParameter("@DisplayFrom", objToUpdate.DisplayFrom)
        objParam(6) = param
        param = New SqlParameter("@FromName", objToUpdate.FromName)
        objParam(7) = param
        param = New SqlParameter("@IsArchived", objToUpdate.IsArchived)
        objParam(8) = param
        param = New SqlParameter("@DocType", objToUpdate.DocType)
        objParam(9) = param
        param = New SqlParameter("@IsExpired", objToUpdate.IsExpired)
        objParam(10) = param
        param = New SqlParameter("@ArchivedTemplateId", objToUpdate.ArchivedTemplateId)
        objParam(11) = param
        param = New SqlParameter("@ArchivedItemid", objToUpdate.ArchivedItemid)
        objParam(12) = param
        param = New SqlParameter("@ArchivedBy", objToUpdate.ArchivedBy)
        objParam(13) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFaxTransaction)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFaxTransaction set Isdeleted=1 where FaxTransactionId=@FaxTransaction_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@FaxTransaction_ID", objToDelete.FaxTransactionId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Sub FaxReaded(objToDelete As IeZFaxTransaction)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFaxTransaction set IsRead=1 where FaxTransactionId=@FaxTransaction_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@FaxTransaction_ID", objToDelete.FaxTransactionId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Sub FaxArchived(objToDelete As IeZFaxTransaction)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFaxTransaction set ArchivedBy=@ArchivedBy,ArchivedTemplateId=@ArchivedTemplateId,ArchivedItemid=@ArchivedItemid,IsArchived=1 where Itemid=@Itemid"
        objParam = New SqlParameter(3) {}
        param = New SqlParameter("@Itemid", objToDelete.Itemid)
        objParam(0) = param
        param = New SqlParameter("@ArchivedItemid", objToDelete.ArchivedItemid)
        objParam(1) = param
        param = New SqlParameter("@ArchivedTemplateId", objToDelete.ArchivedTemplateId)
        objParam(2) = param
        param = New SqlParameter("@ArchivedBy", objToDelete.ArchivedBy)
        objParam(3) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Sub FaxExpired(objToDelete As IeZFaxTransaction)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFaxTransaction set IsExpired=1 where Itemid=@Itemid"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Itemid", objToDelete.Itemid)
        objParam(0) = param

        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class