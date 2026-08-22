Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "eZFaxReceiver Details"


    Public Function CreateeZFaxReceiver(objtemp As eZFaxReceiver) As IeZFaxReceiver
        Dim newObject As IeZFaxReceiver = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select FaxReceiverId From eZFaxReceiver Where  FaxReceiverRuleId=@FaxReceiverRuleId And ECMLoginId=@ECMLoginId and Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ECMLoginId", objtemp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@FaxReceiverRuleId", objtemp.FaxReceiverRuleId)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZFaxReceiver Code already exist!")
            End If
            strQry = "INSERT INTO eZFaxReceiver(ECMLoginId,IsPrimaryUser,FaxReceiverRuleId,CreatedOn,CreatedBy) VALUES(@ECMLoginId,@IsPrimaryUser,@FaxReceiverRuleId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@ECMLoginId", objtemp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@IsPrimaryUser", objtemp.IsPrimaryUser)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@FaxReceiverRuleId", objtemp.FaxReceiverRuleId)
            objParam(4) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZFaxReceiver(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFaxReceiver)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZFaxReceiver Where Isdeleted=0 and FaxReceiverId=@FaxReceiverId"
            param = New SqlParameter("@FaxReceiverId", objRead.FaxReceiverId)
            objParam(0) = param
            'Else
            '    strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZFaxReceiver Where Isdeleted=0 and FaxReceiverId=@FaxReceiverId"
            '    param = New SqlParameter("@FaxReceiverId", objRead.FaxReceiverId)
            '    objParam(0) = param
            'End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFaxReceiver.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.FaxReceiverId = GetInteger(sqlRdr("FaxReceiverId"))
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                objRead.FaxReceiverRuleId = GetInteger(sqlRdr("FaxReceiverRuleId"))
                objRead.IsPrimaryUser = sqlRdr("IsPrimaryUser")
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZFaxReceiver.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZFaxReceiver() As System.Collections.Generic.List(Of IeZFaxReceiver)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxReceiver)()
        Dim objItem As IeZFaxReceiver
        Try
            Dim strQry As String = ""
            strQry = "Select FaxReceiverId From eZFaxReceiver where Isdeleted=0 order by IsPrimaryUser"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFaxReceiver.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxReceiver(GetSmallInterger(sqlRdr("FaxReceiverId")))
                objItem.FaxReceiverId = GetSmallInterger(sqlRdr("FaxReceiverId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZFaxReceiver(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFaxReceiver)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxReceiver)()
        Dim objItem As IeZFaxReceiver
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FaxReceiverId From eZFaxReceiver where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by IsPrimaryUser"
            Else
                strQry = "Select FaxReceiverId From eZFaxReceiver where Isdeleted=0 order by IsPrimaryUser"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFaxReceiver.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxReceiver(GetSmallInterger(sqlRdr("FaxReceiverId")))
                objItem.FaxReceiverId = GetSmallInterger(sqlRdr("FaxReceiverId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZFaxReceiver(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFaxReceiver)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxReceiver)()
        Dim objItem As IeZFaxReceiver
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FaxReceiverId From eZFaxReceiver where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by IsPrimaryUser"
            Else
                strQry = "Select FaxReceiverId From eZFaxReceiver where Isdeleted=0 order by IsPrimaryUser"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFaxReceiver.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxReceiver(GetSmallInterger(sqlRdr("FaxReceiverId")))
                objItem.FaxReceiverId = GetSmallInterger(sqlRdr("FaxReceiverId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function


    Public Function ReadSelectedeZFaxReceiverByField(ByVal ReceiverRuleId As String, ByVal IsPrimaryUser As Boolean) As System.Collections.Generic.List(Of IeZFaxReceiver)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxReceiver)()
        Dim objItem As IeZFaxReceiver
        Try
            Dim strQry As String = ""
            strQry = "Select * From eZFaxReceiver where Isdeleted=0 and FaxReceiverRuleId=N'" & ReceiverRuleId & "' and IsPrimaryUser=N'" & IsPrimaryUser & "'"

            strQry = strQry & " order by FaxReceiverId"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFaxReceiver.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxReceiver(GetSmallInterger(sqlRdr("FaxReceiverId")))
                objItem.FaxReceiverId = GetSmallInterger(sqlRdr("FaxReceiverId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZFaxReceiverByECMLoginId(ByVal ReceiverRuleId As Integer, ByVal ECMLoginId As Integer) As System.Collections.Generic.List(Of IeZFaxReceiver)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxReceiver)()
        Dim objItem As IeZFaxReceiver
        Try
            Dim strQry As String = ""
            strQry = "Select * From eZFaxReceiver where Isdeleted=0 and FaxReceiverRuleId=N'" & ReceiverRuleId.ToString & "' and ECMLoginId=N'" & ECMLoginId.ToString & "'"

            strQry = strQry & " order by FaxReceiverId"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFaxReceiver.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxReceiver(GetSmallInterger(sqlRdr("FaxReceiverId")))
                objItem.FaxReceiverId = GetSmallInterger(sqlRdr("FaxReceiverId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(ByVal objToUpdate As IeZFaxReceiver)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        If objToUpdate Is Nothing Then
            Return
        End If
        'strQry = "Select FaxReceiverId From eZFaxReceiver Where ECMLoginId = @ECMLoginId and FaxReceiverRuleId = @FaxReceiverRuleId and Isdeleted=0"
        'objParam = New SqlParameter(1) {}
        'param = New SqlParameter("@FaxReceiverRuleId", objToUpdate.FaxReceiverRuleId)
        'objParam(0) = param
        'param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        'objParam(1) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("eZFaxReceiver Code already exist!")
        'Else
        strQry = "Update eZFaxReceiver Set FaxReceiverRuleId=@FaxReceiverRuleId,ECMLoginId=@ECMLoginId,IsPrimaryUser=@IsPrimaryUser,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where FaxReceiverId=@FaxReceiverId"
        objParam = New SqlParameter(5) {}
        param = New SqlParameter("@FaxReceiverRuleId", objToUpdate.FaxReceiverRuleId)
        objParam(0) = param
        param = New SqlParameter("@IsPrimaryUser", objToUpdate.IsPrimaryUser)
        objParam(1) = param
        param = New SqlParameter("@FaxReceiverId", objToUpdate.FaxReceiverId)
        objParam(2) = param
        param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param

        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")

        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(ByVal objToDelete As IeZFaxReceiver)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFaxReceiver set Isdeleted=1 where FaxReceiverId=@FaxReceiverId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@FaxReceiverId", objToDelete.FaxReceiverId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub

    Public Function ReadSelectedeZFaxReceiverRuleByCreatedBy(CreatedBy As Integer) As System.Collections.Generic.List(Of IeZFaxReceiver)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFaxReceiver)()
        Dim objItem As IeZFaxReceiver
        Try
            Dim strQry As String = ""

            strQry = " Select FaxReceiverRule As RuleName,FaxReceiverId,rr.FaxReceiverRuleId,"
            strQry = strQry & " (Case When DisplayFrom=1 Then N'Blank'"
            strQry = strQry & " When DisplayFrom=2 Then N'FileName'"
            strQry = strQry & " When DisplayFrom=3 Then N'Blank :' + DisplayText End) As SenderType,"
            strQry = strQry & " (Case When IsPrimaryUser=N'True' Then l.LoginName End) As PrimaryUser,Hours,"
            strQry = strQry & " (Select SUBSTRING(( SELECT ', '  + R.LoginName "
            strQry = strQry & " From eZFaxReceiver as L "
            strQry = strQry & " Left Join eZECMLogin As R On R.ECMLoginId=L.ECMLoginId Where L.FaxReceiverRuleId=rr.FaxReceiverRuleId And L.IsPrimaryUser=N'False' And L.IsDeleted=N'False' And L.CreatedBy=" & CreatedBy & ""
            strQry = strQry & " FOR XML PATH('')"
            strQry = strQry & " ), 3, 200000))"
            strQry = strQry & " As SecondaryUser"
            strQry = strQry & " From eZFaxReceiverRule As RR"
            strQry = strQry & " Left Join eZFaxReceiver As R On R.FaxReceiverRuleId=rr.FaxReceiverRuleId And R.IsDeleted=N'False'"
            strQry = strQry & " Left Join eZECMLogin As L On r.ECMLoginId=l.ECMLoginId "
            strQry = strQry & " Where rr.CreatedBy=" & CreatedBy & " And R.IsPrimaryUser=N'True' And RR.IsDeleted=N'False'"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid RuleName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFaxReceiver(GetSmallInterger(sqlRdr("FaxreceiverId")))
                objItem.RuleName = sqlRdr("RuleName").ToString
                objItem.FaxReceiverRuleId = sqlRdr("FaxReceiverRuleId").ToString
                objItem.PrimaryUser = sqlRdr("PrimaryUser").ToString
                objItem.SecondaryUser = sqlRdr("SecondaryUser").ToString
                objItem.Hours = sqlRdr("Hours").ToString
                objItem.SenderType = sqlRdr("SenderType").ToString
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function


#End Region

End Class

