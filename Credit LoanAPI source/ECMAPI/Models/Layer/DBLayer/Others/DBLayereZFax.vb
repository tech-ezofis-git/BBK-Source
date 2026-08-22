Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateFax(objEmp As eZFax) As IeZFax
        Dim newObject As IeZFax = Nothing
        If String.IsNullOrEmpty(objEmp.FaxName) Then
            Return Nothing
        End If
        objEmp.FaxName = objEmp.FaxName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select FaxId From eZFax Where FaxNumber = @FaxNumber And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@FaxNumber", objEmp.FaxNumber)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("FaxNumber Code already exist!")
            End If
            strQry = "INSERT INTO eZFax(FaxName,FaxNumber,FaxType,FaxReceiverRuleId) VALUES(@FaxName,@FaxNumber,@FaxType,@FaxReceiverRuleId);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@FaxName", objEmp.FaxName)
            objParam(0) = param
            param = New SqlParameter("@FaxNumber", objEmp.FaxNumber)
            objParam(1) = param
            param = New SqlParameter("@FaxType", objEmp.FaxType)
            objParam(2) = param
            param = New SqlParameter("@FaxReceiverRuleId", objEmp.FaxReceiverRuleId)
            objParam(3) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZFax(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFax)
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
            If objRead.FaxNumber Is Nothing Then
                strQry = "Select *,(Case When Faxtype=1 Then 'From' When FaxType=2 Then 'To' End) As FaxTypeValue From eZFax Where FaxId=@Fax_ID and Isdeleted=0"
                param = New SqlParameter("@Fax_ID", objRead.FaxId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,(Case When Faxtype=1 Then 'From' When FaxType=2 Then 'To' End) As FaxTypeValue From eZFax Where FaxNumber=@FaxNumber and Isdeleted=0"
                param = New SqlParameter("@FaxNumber", objRead.FaxNumber)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FaxName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.FaxId = GetInteger(sqlRdr("FaxId"))
                objRead.FaxName = sqlRdr("FaxName").ToString()
                objRead.FaxNumber = sqlRdr("FaxNumber").ToString()
                objRead.FaxType = GetInteger(sqlRdr("FaxType"))
                objRead.FaxTypeValue = sqlRdr("FaxTypeValue").ToString()
                objRead.FaxReceiverRuleId = GetInteger(sqlRdr("FaxReceiverRuleId"))
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
    Public Function ReadAllFax() As System.Collections.Generic.List(Of IeZFax)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFax)()
        Dim objItem As IeZFax

        Try
            Dim strQry As String = ""
            strQry = "Select FaxId From eZFax where Isdeleted=0 order by FaxNumber"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid FaxNumber.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFax(GetInteger(sqlRdr("FaxId")))
                objItem.FaxId = GetInteger(sqlRdr("FaxId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function

    Public Function ReadFilteredeZFax(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFax)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFax)()
        Dim objItem As IeZFax
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FaxId From eZFax where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by FaxNumber"
            Else
                strQry = "Select FaxId From eZFax where Isdeleted=0 order by FaxNumber"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFax.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFax(GetSmallInterger(sqlRdr("FaxId")))
                objItem.FaxId = GetSmallInterger(sqlRdr("FaxId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZFax(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFax)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFax)()
        Dim objItem As IeZFax
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FaxId From eZFax where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FaxNumber"
            Else
                strQry = "Select FaxId From eZFax where Isdeleted=0 order by FaxNumber"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFax.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            While sqlRdr.Read()
                objItem = GlobalInstance.eZFax(GetSmallInterger(sqlRdr("FaxId")))
                objItem.FaxId = GetSmallInterger(sqlRdr("FaxId"))
                lstItems.Add(objItem)
            End While
            If lstItems.Count = 0 Then


                strQry = "Select FaxId From eZFax where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote("Others")
                strQry = strQry & "' "
                strQry = strQry & " order by FaxNumber"
                obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
                If obj Is Nothing Then
                    Throw New Exception("Attempt to read Invalid eZFax.")
                End If
                sqlRdr = DirectCast(obj, SqlDataReader)
                While sqlRdr.Read()
                    objItem = GlobalInstance.eZFax(GetSmallInterger(sqlRdr("FaxId")))
                    objItem.FaxId = GetSmallInterger(sqlRdr("FaxId"))
                    lstItems.Add(objItem)
                End While
            End If

            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function




    Public Sub Update(objToUpdate As IeZFax)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select FaxId From eZFax Where FaxNumber = @FaxNumber and FaxId <> @FaxId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@FaxNumber", objToUpdate.FaxNumber)
        objParam(0) = param
        param = New SqlParameter("@FaxId", objToUpdate.FaxId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("FaxName Code already exist!")
        Else
            strQry = "Update eZFax Set FaxType=@FaxType,FaxNumber=@FaxNumber,FaxName=@FaxName where FaxId=@Fax_ID"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@FaxName", objToUpdate.FaxName)
            objParam(0) = param
            param = New SqlParameter("@Fax_ID", objToUpdate.FaxId)
            objParam(1) = param
            param = New SqlParameter("@FaxNumber", objToUpdate.FaxNumber)
            objParam(2) = param
            param = New SqlParameter("@FaxType", objToUpdate.FaxType)
            objParam(3) = param
            param = New SqlParameter("@FaxReceiverRuleId", objToUpdate.FaxReceiverRuleId)
            objParam(4) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub

    Public Sub Delete(objToDelete As IeZFax)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFax set Isdeleted=1 where FaxId=@Fax_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Fax_ID", objToDelete.FaxId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class