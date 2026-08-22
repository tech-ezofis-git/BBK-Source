Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IezRetentionRule)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From ezRetentionRule ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.RetentionId=@RetentionId and ez.Isdeleted=0"
            param = New SqlParameter("@RetentionId", objRead.RetentionId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezRetentionRule")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.RetentionId = GetInteger(sqlRdr("RetentionId"))
                objRead.RuleName = sqlRdr("RuleName").ToString
                objRead.RetentionType = GetInteger(sqlRdr("RetentionType"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.RetentionRule = sqlRdr("RetentionRule").ToString
                objRead.RetentionField = GetInteger(sqlRdr("RetentionField"))
                objRead.Period = GetInteger(sqlRdr("Period"))
                objRead.PeriodType = sqlRdr("PeriodType").ToString
                objRead.NotifyMail = sqlRdr("NotifyMail").ToString
                objRead.RemainderDays = GetInteger(sqlRdr("RemainderDays"))
                objRead.Createdby = GetInteger(sqlRdr("CreatedBy"))
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.Updatedby = GetInteger(sqlRdr("UpdatedBy"))
                objRead.Updatedon = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.RetentionRuleJSON = sqlRdr("RetentionRuleJSON").ToString()
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
    Public Function CreateezRetentionRule(objEmp As ezRetentionRule) As ezRetentionRule
        Dim newObject As ezRetentionRule = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO ezRetentionRule " +
                "(RuleName,RetentionType,TemplateId,RetentionRule,RetentionField,Period,PeriodType,NotifyMail,RemainderDays,CreatedBy,CreatedOn,RetentionRuleJSON) " +
                "VALUES (@RuleName,@RetentionType,@TemplateId,@RetentionRule,@RetentionField,@Period,@PeriodType,@NotifyMail,@RemainderDays,@CreatedBy,@CreatedOn," +
                "@RetentionRuleJSON);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(11) {}
            param = New SqlParameter("@RuleName", objEmp.RuleName)
            objParam(0) = param
            param = New SqlParameter("@RetentionType", objEmp.RetentionType)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(2) = param
            param = New SqlParameter("@RetentionRule", objEmp.RetentionRule)
            objParam(3) = param
            param = New SqlParameter("@RetentionField", objEmp.RetentionField)
            objParam(4) = param
            param = New SqlParameter("@Period", objEmp.Period)
            objParam(5) = param
            param = New SqlParameter("@PeriodType", objEmp.PeriodType)
            objParam(6) = param
            param = New SqlParameter("@NotifyMail", objEmp.NotifyMail)
            objParam(7) = param
            param = New SqlParameter("@RemainderDays", objEmp.RemainderDays)
            objParam(8) = param
            param = New SqlParameter("@CreatedBy", objEmp.Createdby)
            objParam(9) = param
            param = New SqlParameter("@CreatedOn", objEmp.Createdon)
            objParam(10) = param
            param = New SqlParameter("@RetentionRuleJSON", objEmp.RetentionRuleJSON)
            objParam(11) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezRetentionRule(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IezRetentionRule)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezRetentionRule Set " +
           "RuleName=@RuleName,RetentionType=@RetentionType,TemplateId=@TemplateId,RetentionRule=@RetentionRule,RetentionField=@RetentionField," +
           "Period=@Period,PeriodType=@PeriodType,NotifyMail=@NotifyMail,RemainderDays=@RemainderDays,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn," +
            "RetentionRuleJSON=@RetentionRuleJSON where RetentionId=@RetentionId"
        objParam = New SqlParameter(12) {}
        param = New SqlParameter("@RuleName", objToUpdate.RuleName)
        objParam(0) = param
        param = New SqlParameter("@RetentionType", objToUpdate.RetentionType)
        objParam(1) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(2) = param
        param = New SqlParameter("@RetentionRule", objToUpdate.RetentionRule)
        objParam(3) = param
        param = New SqlParameter("@RetentionField", objToUpdate.RetentionField)
        objParam(4) = param
        param = New SqlParameter("@Period", objToUpdate.Period)
        objParam(5) = param
        param = New SqlParameter("@PeriodType", objToUpdate.PeriodType)
        objParam(6) = param
        param = New SqlParameter("@NotifyMail", objToUpdate.NotifyMail)
        objParam(7) = param
        param = New SqlParameter("@RemainderDays", objToUpdate.RemainderDays)
        objParam(8) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.Updatedby)
        objParam(9) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
        objParam(10) = param
        param = New SqlParameter("@RetentionId", objToUpdate.RetentionId)
        objParam(11) = param
        param = New SqlParameter("@RetentionRuleJSON", objToUpdate.RetentionRuleJSON)
        objParam(12) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IezRetentionRule)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezRetentionRule set Isdeleted=1,updatedby=@updatedby,updatedon=@updatedon where RetentionId=@RetentionId"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@RetentionId", objToDelete.RetentionId)
        objParam(0) = param
        param = New SqlParameter("@updatedby", objToDelete.Updatedby)
        objParam(1) = param
        param = New SqlParameter("@updatedon", objToDelete.Updatedon)
        objParam(2) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllezRetentionRule() As System.Collections.Generic.List(Of IezRetentionRule)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezRetentionRule)()
        Dim objItem As IezRetentionRule
        Try
            Dim strQry As String = ""
            strQry = "Select RetentionId From ezRetentionRule where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezRetentionRule")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezRetentionRule(GetInteger(sqlRdr("RetentionId")))
                objItem.RetentionId = GetInteger(sqlRdr("RetentionId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredezRetentionRule(Criteria As String, Value As String) As List(Of IezRetentionRule)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezRetentionRule)()
        Dim objItem As IezRetentionRule
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select RetentionId From ezRetentionRule where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by RetentionId"
            Else
                strQry = "Select RetentionId From ezRetentionRule where Isdeleted=0 order by RetentionId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezRetentionRule")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezRetentionRule(GetInteger(sqlRdr("RetentionId")))
                objItem.RetentionId = GetInteger(sqlRdr("RetentionId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedezRetentionRule(Criteria As String, Value As String) As System.Collections.Generic.List(Of IezRetentionRule)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IezRetentionRule)()
        Dim objItem As IezRetentionRule
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select RetentionId From ezRetentionRule where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by RetentionId"
            Else
                strQry = "Select RetentionId From ezRetentionRule where Isdeleted=0 order by RetentionId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezRetentionRule")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezRetentionRule(GetInteger(sqlRdr("RetentionId")))
                objItem.RetentionId = GetInteger(sqlRdr("RetentionId"))
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
