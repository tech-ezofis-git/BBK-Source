Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "eZFaxReceiverRule Details"


    Public Function CreateeZFaxReceiverRule(objtemp As eZFaxReceiverRule) As IeZFaxReceiverRule
        Dim newObject As IeZFaxReceiverRule = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select FaxReceiverRuleId From eZFaxReceiverRule Where FaxReceiverRule = @FaxReceiverRule  and Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@FaxReceiverRule", objtemp.FaxReceiverRule)
            objParam(0) = param
           
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZFaxReceiverRule Code already exist!")
            End If
            strQry = "INSERT INTO eZFaxReceiverRule(DisplayFrom,FaxReceiverRule,DisplayText,Hours,ValidityOfFax,CreatedOn,CreatedBy) VALUES(@DisplayFrom,@FaxReceiverRule,@DisplayText,@Hours,@ValidityOfFax,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@DisplayFrom", objtemp.DisplayFrom)
            objParam(0) = param
            param = New SqlParameter("@FaxReceiverRule", objtemp.FaxReceiverRule)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@DisplayText", objtemp.DisplayText)
            objParam(4) = param
            param = New SqlParameter("@Hours", objtemp.Hours)
            objParam(5) = param
            param = New SqlParameter("@ValidityOfFax", objtemp.ValidityOfFax)
            objParam(6) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZFaxReceiverRule(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFaxReceiverRule)
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
            'If objRead.CreatedOn Is Nothing Then
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZFaxReceiverRule Where Isdeleted=0 and FaxReceiverRuleId=@FaxReceiverRuleId"
            param = New SqlParameter("@FaxReceiverRuleId", objRead.FaxReceiverRuleId)
            objParam(0) = param
            'Else
            '    strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZFaxReceiverRule Where Isdeleted=0 and FaxReceiverRuleId=@FaxReceiverRuleId"
            '    param = New SqlParameter("@FaxReceiverRuleId", objRead.FaxReceiverRuleId)
            '    objParam(0) = param
            'End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFaxReceiverRule.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.FaxReceiverRuleId = GetInteger(sqlRdr("FaxReceiverRuleId"))
                objRead.DisplayFrom = GetInteger(sqlRdr("DisplayFrom"))
                objRead.Hours = GetInteger(sqlRdr("Hours"))
                objRead.ValidityOfFax = GetInteger(sqlRdr("ValidityOfFax"))
                objRead.FaxReceiverRule = sqlRdr("FaxReceiverRule")
                objRead.DisplayText = sqlRdr("DisplayText")
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZFaxReceiverRule.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZFaxReceiverRule() As System.Collections.Generic.List(Of IeZFaxReceiverRule)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxReceiverRule)()
        Dim objItem As IeZFaxReceiverRule
        Try
            Dim strQry As String = ""
            strQry = "Select FaxReceiverRuleId From eZFaxReceiverRule where Isdeleted=0 order by FaxReceiverRule"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFaxReceiverRule.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxReceiverRule(GetSmallInterger(sqlRdr("FaxReceiverRuleId")))
                objItem.FaxReceiverRuleId = GetSmallInterger(sqlRdr("FaxReceiverRuleId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZFaxReceiverRule(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFaxReceiverRule)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxReceiverRule)()
        Dim objItem As IeZFaxReceiverRule
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FaxReceiverRuleId From eZFaxReceiverRule where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by FaxReceiverRule"
            Else
                strQry = "Select FaxReceiverRuleId From eZFaxReceiverRule where Isdeleted=0 order by FaxReceiverRule"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFaxReceiverRule.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxReceiverRule(GetSmallInterger(sqlRdr("FaxReceiverRuleId")))
                objItem.FaxReceiverRuleId = GetSmallInterger(sqlRdr("FaxReceiverRuleId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZFaxReceiverRule(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFaxReceiverRule)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxReceiverRule)()
        Dim objItem As IeZFaxReceiverRule
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FaxReceiverRuleId From eZFaxReceiverRule where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FaxReceiverRule"
            Else
                strQry = "Select FaxReceiverRuleId From eZFaxReceiverRule where Isdeleted=0 order by FaxReceiverRule"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFaxReceiverRule.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxReceiverRule(GetSmallInterger(sqlRdr("FaxReceiverRuleId")))
                objItem.FaxReceiverRuleId = GetSmallInterger(sqlRdr("FaxReceiverRuleId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZFaxReceiverRule)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select FaxReceiverRuleId From eZFaxReceiverRule Where FaxReceiverRule = @FaxReceiverRule and FaxReceiverRuleId <> @FaxReceiverRuleId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@FaxReceiverRule", objToUpdate.FaxReceiverRule)
        objParam(0) = param
        param = New SqlParameter("@FaxReceiverRuleId", objToUpdate.FaxReceiverRuleId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZFaxReceiverRule Code already exist!")
        Else
            strQry = "Update eZFaxReceiverRule Set ValidityOfFax=@ValidityOfFax,FaxReceiverRule=@FaxReceiverRule,DisplayText=@DisplayText,Hours=@Hours,DisplayFrom=@DisplayFrom,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where FaxReceiverRuleId=@FaxReceiverRuleId"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@FaxReceiverRule", objToUpdate.FaxReceiverRule)
            objParam(0) = param
            param = New SqlParameter("@DisplayFrom", objToUpdate.DisplayFrom)
            objParam(1) = param
            param = New SqlParameter("@Hours", objToUpdate.Hours)
            objParam(2) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(3) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(4) = param
            param = New SqlParameter("@DisplayText", objToUpdate.DisplayText)
            objParam(5) = param
            param = New SqlParameter("@FaxReceiverRuleId", objToUpdate.FaxReceiverRuleId)
            objParam(6) = param
            param = New SqlParameter("@ValidityOfFax", objToUpdate.ValidityOfFax)
            objParam(7) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFaxReceiverRule)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFaxReceiverRule set Isdeleted=1 where FaxReceiverRuleId=@FaxReceiverRuleId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@FaxReceiverRuleId", objToDelete.FaxReceiverRuleId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

