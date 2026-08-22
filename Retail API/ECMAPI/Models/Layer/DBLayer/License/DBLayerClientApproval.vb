Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer
    'Public Sub Read(objRead As IeZClientAppproval)
    '    If objRead.IsReadFromDB Then
    '        Return
    '    End If
    '    If objRead.IsModified Then
    '        Throw New InvalidOperationException()
    '    End If
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    objRead.IsReadFromDB = True
    '    Try
    '        Dim strQry As String = ""
    '        Dim objParam As SqlParameter()
    '        Dim param As SqlParameter
    '        objParam = New SqlParameter(0) {}
    '        'If objRead.ClientApprovalId <> 0 Then
    '        '    strQry = "exec dbo.openprime;Select * from ezclientapprovalview where ClientApprovalId=@ClientApprovalId"
    '        '    param = New SqlParameter("@ClientApprovalId", objRead.ClientApprovalId)
    '        '    objParam(0) = param
    '        'Else
    '        strQry = "exec dbo.openprime;Select *,isnull(dbo.udf_UserName(UpdatedBy),'') as UpdatedBy1,isnull(dbo.udf_UserName(CreatedBy),'') as CreatedBy1" +
    '            " from ezclientapprovalview where ClientApprovalId=@ClientApprovalId"
    '        param = New SqlParameter("@ClientApprovalId", objRead.ClientApprovalId)
    '        objParam(0) = param
    '        'End If
    '        Dim obj As Object = ""
    '        obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid eZClientApproval.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        If sqlRdr.Read() Then
    '            objRead.ClientApprovalId = GetInteger(sqlRdr("ClientApprovalId"))
    '            objRead.ConfigPrimeId = GetInteger(sqlRdr("ConfigPrimeId"))
    '            objRead.Approval = sqlRdr("Approval").ToString()
    '            objRead.Appprime = DBLayer.DBLInstance.AES_Decrypt(sqlRdr("Appprime").ToString(), "ezofis")
    '            objRead.ApprovalCode = sqlRdr("ApprovalCode").ToString()
    '            objRead.ISA = sqlRdr("ISA").ToString()
    '            objRead.PrimeOn = sqlRdr("PrimeOn").ToString()
    '            objRead.PrimeCount = sqlRdr("PrimeCount").ToString()
    '            objRead.CreatedOn = sqlRdr("CreatedOn").ToString
    '            objRead.PrimeDepart = sqlRdr("PrimeDepart").ToString()
    '            objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
    '            objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
    '            objRead.CreatedBy = sqlRdr("CreatedBy").ToString
    '            objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
    '            objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString
    '        Else
    '            Throw New Exception("Attempt to read Invalid eZClientApproval from SqlRead.")
    '            Return
    '        End If
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '        objRead.IsModified = False
    '    End Try
    'End Sub
    Public Sub Read(objRead As IeZClientAppproval)
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
            If objRead.ClientApprovalId <> 0 Then
                strQry = "exec dbo.openprime;Select *,isnull(dbo.udf_UserName(UpdatedBy),'') as UpdatedBy1,isnull(dbo.udf_UserName(CreatedBy),'') as CreatedBy1" +
                    " from ezclientapprovalview where ClientApprovalId=@ClientApprovalId"
                param = New SqlParameter("@ClientApprovalId", objRead.ClientApprovalId)
                objParam(0) = param
                Dim obj As Object = ""
                obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
                If obj Is Nothing Then
                    Throw New Exception("Attempt to read Invalid Client Approval.")
                End If
                sqlRdr = DirectCast(obj, SqlDataReader)
                If sqlRdr.Read() Then
                    objRead.ClientApprovalId = GetInteger(sqlRdr("ClientApprovalId"))
                    objRead.ConfigPrimeId = GetInteger(sqlRdr("ConfigPrimeId"))
                    objRead.Approval = sqlRdr("Approval").ToString()
                    objRead.Appprime = DBLayer.DBLInstance.AES_Decrypt(sqlRdr("Appprime").ToString(), "ezofis")
                    objRead.ApprovalCode = sqlRdr("ApprovalCode").ToString()
                    objRead.ISA = Convert.ToInt32(Convert.ToBoolean(sqlRdr("ISA")))
                    objRead.PrimeOn = sqlRdr("PrimeOn").ToString()
                    objRead.PrimeCount = sqlRdr("PrimeCount").ToString()
                    objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                    objRead.PrimeDepart = sqlRdr("PrimeDepart").ToString()
                    objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                    objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                    objRead.CreatedBy = sqlRdr("CreatedBy").ToString
                    objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                    objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString
                    objRead.Active = Convert.ToInt32(Convert.ToBoolean(sqlRdr("Active")))
                    objRead.UserId = GetInteger(sqlRdr("UserId"))
                Else
                    Return
                End If
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZClientAppproval() As System.Collections.Generic.List(Of IeZClientAppproval)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZClientAppproval)()
        Dim objItem As IeZClientAppproval
        Try
            Dim strQry As String = ""
            strQry = "exec dbo.openprime;Select ClientApprovalId From ezclientapprovalview where Isdeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezclientapproval.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezclientapproval(GetSmallInterger(sqlRdr("ClientApprovalId")))
                objItem.ClientApprovalId = GetSmallInterger(sqlRdr("ClientApprovalId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZClientApproval(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZClientAppproval)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZClientAppproval)()
        Dim objItem As IeZClientAppproval
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "exec dbo.openprime;Select ClientApprovalId From ezclientapprovalview where Isdeleted=0  and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
            Else
                strQry = "exec dbo.openprime;Select ClientApprovalId From ezclientapprovalview where Isdeleted=0"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZClientApproval.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezclientapproval(GetSmallInterger(sqlRdr("ClientApprovalId")))
                objItem.ClientApprovalId = GetSmallInterger(sqlRdr("ClientApprovalId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZClientApproval(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZClientAppproval)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZClientAppproval)()
        Dim objItem As IeZClientAppproval
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "exec dbo.openprime;Select ClientApprovalId From ezclientapprovalview where Isdeleted=0  and "
                strQry = strQry & "Convert(Nvarchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
            Else
                strQry = "exec dbo.openprime;Select ClientApprovalId From ezclientapprovalview where Isdeleted=0"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZClientApproval.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezclientapproval(GetSmallInterger(sqlRdr("ClientApprovalId")))
                objItem.ClientApprovalId = GetSmallInterger(sqlRdr("ClientApprovalId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZClientAppproval)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "exec dbo.openprime;Select ClientApprovalId From ezclientapprovalview Where ClientApprovalId = @ClientApprovalId and Isdeleted=0"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ClientApprovalId", objToUpdate.ClientApprovalId)
        objParam(0) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZClientApproval Code already exist!")
        Else
            strQry = "exec dbo.Openprime;UPDATE ezClientapproval SET configprimeid=@configprimeid, ISA=@ISA,Active=@Active,primeOn=dbo.EPrime(@primeOn)," +
                "createdon=dbo.EPrime(@createdon),primedepart=DBO.EPrime(@primedepart),primecount=dbo.EPrime(@primecount),UserId=@UserId where Clientapprovalid" +
                " in(@Clientapprovalid)"
            objParam = New SqlParameter(9) {}
            param = New SqlParameter("@configprimeid", objToUpdate.ConfigPrimeId)
            objParam(0) = param
            param = New SqlParameter("@ISA", objToUpdate.ISA)
            objParam(1) = param
            param = New SqlParameter("@primeOn", objToUpdate.PrimeOn)
            objParam(2) = param
            param = New SqlParameter("@createdon", objToUpdate.CreatedOn)
            objParam(3) = param
            'param = New SqlParameter("@CabIcon", objToUpdate.CabIcon)
            'objParam(4) = param
            param = New SqlParameter("@primedepart", objToUpdate.PrimeDepart)
            objParam(4) = param
            param = New SqlParameter("@primecount", objToUpdate.PrimeCount)
            objParam(5) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(6) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(7) = param
            param = New SqlParameter("@Active", objToUpdate.Active)
            objParam(8) = param
            param = New SqlParameter("@UserId", objToUpdate.UserId)
            objParam(9) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZClientAppproval)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezclientapproval set Isdeleted=1 where clientapprovalid=@clientapprovalid"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@clientapprovalid", objToDelete.ClientApprovalId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class
