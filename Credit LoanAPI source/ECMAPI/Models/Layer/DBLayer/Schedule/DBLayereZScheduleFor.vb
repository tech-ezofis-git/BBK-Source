Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZScheduleFor)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZScheduleFor ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.ForScheduleId=@ForScheduleId and ez.Isdeleted=0"
            param = New SqlParameter("@ForScheduleId", objRead.ForScheduleId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScheduleFor")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ForScheduleId = GetInteger(sqlRdr("ForScheduleId"))
                objRead.ForSchedule = sqlRdr("ForSchedule").ToString
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
    Public Function CreateeZScheduleFor(objEmp As eZScheduleFor) As eZScheduleFor
        Dim newObject As eZScheduleFor = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZScheduleFor(ForSchedule,CreatedBy,CreatedOn) VALUES " +
                "(@ForSchedule,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@ForSchedule", objEmp.ForSchedule)
            objParam(0) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZScheduleFor(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZScheduleFor)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZScheduleFor Set ForSchedule=@ForSchedule,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where ForScheduleId=@ForScheduleId"
        objParam = New SqlParameter(3) {}
        param = New SqlParameter("@ForSchedule", objToUpdate.ForSchedule)
        objParam(0) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(1) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(2) = param
        param = New SqlParameter("@ForScheduleId", objToUpdate.ForScheduleId)
        objParam(3) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZScheduleFor)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZScheduleFor set Isdeleted=1 where ForScheduleId=@ForScheduleId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ForScheduleId", objToDelete.ForScheduleId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZScheduleFor() As System.Collections.Generic.List(Of IeZScheduleFor)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScheduleFor)()
        Dim objItem As IeZScheduleFor
        Try
            Dim strQry As String = ""
            strQry = "Select ForScheduleId From eZScheduleFor where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScheduleFor")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScheduleFor(GetInteger(sqlRdr("ForScheduleId")))
                objItem.ForScheduleId = GetInteger(sqlRdr("ForScheduleId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZScheduleFor(Criteria As String, Value As String) As List(Of IeZScheduleFor)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScheduleFor)()
        Dim objItem As IeZScheduleFor
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ForScheduleId From eZScheduleFor where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ForScheduleId"
            Else
                strQry = "Select ForScheduleId From eZScheduleFor where Isdeleted=0 order by ForScheduleId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScheduleFor")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScheduleFor(GetInteger(sqlRdr("ForScheduleId")))
                objItem.ForScheduleId = GetInteger(sqlRdr("ForScheduleId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZScheduleFor(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZScheduleFor)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScheduleFor)()
        Dim objItem As IeZScheduleFor
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ForScheduleId From eZScheduleFor where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ForScheduleId"
            Else
                strQry = "Select ForScheduleId From eZScheduleFor where Isdeleted=0 order by ForScheduleId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScheduleFor")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScheduleFor(GetInteger(sqlRdr("ForScheduleId")))
                objItem.ForScheduleId = GetInteger(sqlRdr("ForScheduleId"))
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
