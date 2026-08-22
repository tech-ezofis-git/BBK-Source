Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZFolderMonitor)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZFolderMonitor ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Monitorid=@Monitorid and ez.Isdeleted=0"
            param = New SqlParameter("@Monitorid", objRead.Monitorid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Hide File")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Monitorid = GetInteger(sqlRdr("Monitorid"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.MonitorTypeId = GetInteger(sqlRdr("MonitorTypeId"))
                objRead.FileType = sqlRdr("FileType").ToString
                objRead.Monitortype = sqlRdr("Monitortype").ToString
                objRead.MonitorPath = sqlRdr("MonitorPath").ToString
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.IsActive = GetBoolean(sqlRdr("IsActive"))
                objRead.Schedule = GetBoolean(sqlRdr("Schedule"))
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
    Public Function CreateeZFolderMonitor(objEmp As eZFolderMonitor) As eZFolderMonitor
        Dim newObject As eZFolderMonitor = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZFolderMonitor(TemplateId,MonitorPath,Monitortype,MonitorTypeId,fileType,CreatedBy,CreatedOn,IsActive,Schedule) VALUES" +
                "(@TemplateId,@MonitorPath,@Monitortype,@MonitorTypeId,@fileType,@CreatedBy,@CreatedOn,@IsActive,@Schedule);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(0) = param
            param = New SqlParameter("@MonitorPath", objEmp.MonitorPath)
            objParam(1) = param
            param = New SqlParameter("@Monitortype", objEmp.Monitortype)
            objParam(2) = param
            param = New SqlParameter("@MonitorTypeId", objEmp.MonitorTypeId)
            objParam(3) = param
            param = New SqlParameter("@fileType", objEmp.FileType)
            objParam(4) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(6) = param
            param = New SqlParameter("@IsActive", objEmp.IsActive)
            objParam(7) = param
            param = New SqlParameter("@Schedule", objEmp.Schedule)
            objParam(8) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZFolderMonitor(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZFolderMonitor)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFolderMonitor Set TemplateId=@TemplateId,MonitorPath=@MonitorPath,Monitortype=@Monitortype,MonitorTypeId=@MonitorTypeId," +
            "fileType=@fileType,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,IsActive=@IsActive,Schedule=@Schedule where Monitorid=@Monitorid"
        objParam = New SqlParameter(9) {}
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(0) = param
        param = New SqlParameter("@MonitorPath", objToUpdate.MonitorPath)
        objParam(1) = param
        param = New SqlParameter("@Monitortype", objToUpdate.Monitortype)
        objParam(2) = param
        param = New SqlParameter("@MonitorTypeId", objToUpdate.MonitorTypeId)
        objParam(3) = param
        param = New SqlParameter("@fileType", objToUpdate.FileType)
        objParam(4) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(5) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(6) = param
        param = New SqlParameter("@Monitorid", objToUpdate.Monitorid)
        objParam(7) = param
        param = New SqlParameter("@IsActive", objToUpdate.IsActive)
        objParam(8) = param
        param = New SqlParameter("@Schedule", objToUpdate.Schedule)
        objParam(9) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFolderMonitor)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFolderMonitor set Isdeleted=1 where Monitorid=@Monitorid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Monitorid", objToDelete.Monitorid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZFolderMonitor() As System.Collections.Generic.List(Of IeZFolderMonitor)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolderMonitor)()
        Dim objItem As IeZFolderMonitor
        Try
            Dim strQry As String = ""
            strQry = "Select Monitorid From eZFolderMonitor where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Monitor Files")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolderMonitor(GetInteger(sqlRdr("Monitorid")))
                objItem.Monitorid = GetInteger(sqlRdr("Monitorid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZFolderMonitor(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFolderMonitor)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolderMonitor)()
        Dim objItem As IeZFolderMonitor
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Monitorid From eZFolderMonitor where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Monitorid"
            Else
                strQry = "Select Monitorid From eZFolderMonitor where Isdeleted=0 order by Monitorid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Monitor File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolderMonitor(GetInteger(sqlRdr("Monitorid")))
                objItem.Monitorid = GetInteger(sqlRdr("Monitorid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZFolderMonitor(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFolderMonitor)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFolderMonitor)()
        Dim objItem As IeZFolderMonitor
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Monitorid From eZFolderMonitor where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Monitorid"
            Else
                strQry = "Select Monitorid From eZFolderMonitor where Isdeleted=0 order by Monitorid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Monitor File.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFolderMonitor(GetInteger(sqlRdr("Monitorid")))
                objItem.Monitorid = GetInteger(sqlRdr("Monitorid"))
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
