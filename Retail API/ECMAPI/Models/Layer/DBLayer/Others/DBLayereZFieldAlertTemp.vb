Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZFieldAlertTemp)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZFieldAlertTemp ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Id=@Id and ez.Isdeleted=0"
            param = New SqlParameter("@Id", objRead.Id)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlertTemp")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Id = GetInteger(sqlRdr("Id"))
                objRead.ToAdd = sqlRdr("ToAdd").ToString
                objRead.BodyMessage = sqlRdr("BodyMessage").ToString
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
    Public Function CreateeZFieldAlertTemp(objEmp As eZFieldAlertTemp) As eZFieldAlertTemp
        Dim newObject As eZFieldAlertTemp = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZFieldAlertTemp(ToAdd,BodyMessage,CreatedBy,CreatedOn) VALUES " +
                "(@ToAdd,@BodyMessage,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@ToAdd", objEmp.ToAdd)
            objParam(0) = param
            param = New SqlParameter("@BodyMessage", objEmp.BodyMessage)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZFieldAlertTemp(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZFieldAlertTemp)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFieldAlertTemp Set ToAdd=@ToAdd,BodyMessage=@BodyMessage,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where Id=@Id"
        objParam = New SqlParameter(4) {}
        param = New SqlParameter("@ToAdd", objToUpdate.ToAdd)
        objParam(0) = param
        param = New SqlParameter("@BodyMessage", objToUpdate.BodyMessage)
        objParam(1) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(2) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(3) = param
        param = New SqlParameter("@Id", objToUpdate.Id)
        objParam(4) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFieldAlertTemp)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFieldAlertTemp set Isdeleted=1 where Id=@Id "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Id", objToDelete.Id)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZFieldAlertTemp() As System.Collections.Generic.List(Of IeZFieldAlertTemp)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlertTemp)()
        Dim objItem As IeZFieldAlertTemp
        Try
            Dim strQry As String = ""
            strQry = "Select Id From eZFieldAlertTemp where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlertTemp")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlertTemp(GetInteger(sqlRdr("Id")))
                objItem.Id = GetInteger(sqlRdr("Id"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZFieldAlertTemp(Criteria As String, Value As String) As List(Of IeZFieldAlertTemp)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlertTemp)()
        Dim objItem As IeZFieldAlertTemp
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Id From eZFieldAlertTemp where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Id"
            Else
                strQry = "Select Id From eZFieldAlertTemp where Isdeleted=0 order by Id"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlertTemp")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlertTemp(GetInteger(sqlRdr("Id")))
                objItem.Id = GetInteger(sqlRdr("Id"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZFieldAlertTemp(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFieldAlertTemp)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlertTemp)()
        Dim objItem As IeZFieldAlertTemp
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Id From eZFieldAlertTemp where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Id"
            Else
                strQry = "Select Id From eZFieldAlertTemp where Isdeleted=0 order by Id"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlertTemp")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlertTemp(GetInteger(sqlRdr("Id")))
                objItem.Id = GetInteger(sqlRdr("Id"))
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
