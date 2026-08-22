Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZScanSettings)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZScanSettings ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.SettingId=@SettingId and ez.Isdeleted=0"
            param = New SqlParameter("@SettingId", objRead.SettingId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScanSettings")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Dpi = GetInteger(sqlRdr("Dpi"))
                objRead.LoginId = GetInteger(sqlRdr("LoginId"))
                objRead.SettingId = GetInteger(sqlRdr("SettingId"))
                objRead.Colour = GetBoolean(sqlRdr("TemplateId"))
                objRead.Dublex = GetBoolean(sqlRdr("nopages"))
                objRead.DupType = GetInteger(sqlRdr("DupType"))
                objRead.FileNameType = GetInteger(sqlRdr("FileNameType"))
                objRead.FileName = sqlRdr("FileName").ToString
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
    Public Function CreateeZScanSettings(objEmp As eZScanSettings) As eZScanSettings
        Dim newObject As eZScanSettings = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZScanSettings(Dublex,Colour,Dpi,LoginId,FileName,FileNameType,DupType,CreatedBy,CreatedOn) VALUES " +
                "(@Dublex,@Colour,@Dpi,@LoginId,@FileName,@FileNameType,@DupType,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@Dublex", objEmp.Dublex)
            objParam(0) = param
            param = New SqlParameter("@Colour", objEmp.Colour)
            objParam(1) = param
            param = New SqlParameter("@Dpi", objEmp.Dpi)
            objParam(2) = param
            param = New SqlParameter("@LoginId", objEmp.LoginId)
            objParam(3) = param
            param = New SqlParameter("@FileName", objEmp.FileName)
            objParam(4) = param
            param = New SqlParameter("@FileNameType", objEmp.FileNameType)
            objParam(5) = param
            param = New SqlParameter("@DupType", objEmp.DupType)
            objParam(6) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(7) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(8) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZScanSettings(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZScanSettings)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZScanSettings Set Dublex=@Dublex,Colour=@Colour,Dpi=@Dpi,LoginId=@LoginId," +
            "FileName=@FileName,FileNameType=@FileNameType,DupType=@DupType,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where SettingId=@SettingId"
        objParam = New SqlParameter(9) {}
        param = New SqlParameter("@Dublex", objToUpdate.Dublex)
        objParam(0) = param
        param = New SqlParameter("@Colour", objToUpdate.Colour)
        objParam(1) = param
        param = New SqlParameter("@Dpi", objToUpdate.Dpi)
        objParam(2) = param
        param = New SqlParameter("@LoginId", objToUpdate.LoginId)
        objParam(3) = param
        param = New SqlParameter("@FileName", objToUpdate.FileName)
        objParam(4) = param
        param = New SqlParameter("@FileNameType", objToUpdate.FileNameType)
        objParam(5) = param
        param = New SqlParameter("@DupType", objToUpdate.DupType)
        objParam(6) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(7) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(8) = param
        param = New SqlParameter("@SettingId", objToUpdate.SettingId)
        objParam(9) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZScanSettings)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZScanSettings set Isdeleted=1 where SettingId=@SettingId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@SettingId", objToDelete.SettingId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZScanSettings() As System.Collections.Generic.List(Of IeZScanSettings)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScanSettings)()
        Dim objItem As IeZScanSettings
        Try
            Dim strQry As String = ""
            strQry = "Select SettingId From eZScanSettings where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScanSettings")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScanSettings(GetInteger(sqlRdr("SettingId")))
                objItem.SettingId = GetInteger(sqlRdr("SettingId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZScanSettings(Criteria As String, Value As String) As List(Of IeZScanSettings)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScanSettings)()
        Dim objItem As IeZScanSettings
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select SettingId From eZScanSettings where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by SettingId"
            Else
                strQry = "Select SettingId From eZScanSettings where Isdeleted=0 order by SettingId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScanSettings")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScanSettings(GetInteger(sqlRdr("SettingId")))
                objItem.SettingId = GetInteger(sqlRdr("SettingId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZScanSettings(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZScanSettings)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZScanSettings)()
        Dim objItem As IeZScanSettings
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select SettingId From eZScanSettings where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by SettingId"
            Else
                strQry = "Select SettingId From eZScanSettings where Isdeleted=0 order by SettingId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZScanSettings")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZScanSettings(GetInteger(sqlRdr("SettingId")))
                objItem.SettingId = GetInteger(sqlRdr("SettingId"))
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
