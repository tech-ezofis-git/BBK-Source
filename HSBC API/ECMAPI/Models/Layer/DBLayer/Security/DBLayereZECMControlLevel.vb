Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "User ECMControlLevels"
    Public Function CreateECMControlLevel(objEmp As eZECMControlLevel) As IeZECMControlLevel
        Dim newObject As IeZECMControlLevel = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ECMControlLevelId From eZECMControlLevel Where ECMProfileid = @ECMProfileId and ECMControlId = @ECMControlId And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ECMProfileId", objEmp.ECMProfileId)
            objParam(0) = param
            param = New SqlParameter("@ECMControlId", objEmp.ECMControlId)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ECMProfileId Code already exist!")
            End If
            strQry = "INSERT INTO eZECMControlLevel(ECMControlId,ECMProfileId,templateid,CreatedOn,CreatedBy) VALUES" +
                "(@ECMControlId,@ECMProfileId,@templateid,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@ECMControlId", objEmp.ECMControlId)
            objParam(0) = param
            param = New SqlParameter("@ECMProfileId", objEmp.ECMProfileId)
            objParam(1) = param
            param = New SqlParameter("@templateid", objEmp.templateid)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(4) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZECMControlLevel(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZECMControlLevel)
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
            strQry = "Select a.*,t.templatename as templatename,b.ecmcontrol,b.ecmcontroltype,ezl.loginname as createdby1,ezlg.loginname as updatedby1 From eZECMControlLevel a " +
                "left join eZECMControl b on a.ECMControlId = b.ECMControlId left join eztemplate t on a.templateid=t.templateid left join ezecmlogin ezl on " +
                "a.createdby=ezl.ECMLoginId left join ezecmlogin ezlg on a.updatedby=ezlg.ECMLoginId Where a.ECMControlLevelId=@ECMControlLevel_ID and a.Isdeleted=0"
            param = New SqlParameter("@ECMControlLevel_ID", objRead.ECMControlLevelId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMControlLevelId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ECMControlLevelId = GetInteger(sqlRdr("ECMControlLevelId"))
                objRead.ECMControlId = GetInteger(sqlRdr("ECMControlId"))
                objRead.ECMControl = sqlRdr("ECMControl").ToString()
                objRead.ECMControlType = GetInteger(sqlRdr("ECMControlType"))
                objRead.templatename = sqlRdr("templatename").ToString()
                objRead.templateid = GetInteger(sqlRdr("templateid"))
                objRead.ECMProfileId = GetInteger(sqlRdr("ECMProfileId"))
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
    Public Function ReadAllECMControlLevel() As System.Collections.Generic.List(Of IeZECMControlLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMControlLevel)()
        Dim objItem As IeZECMControlLevel

        Try
            Dim strQry As String = ""
            strQry = "Select ECMControlLevelId From eZECMControlLevel where Isdeleted=0 order by ECMControlLevelId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMControlLevelId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMControlLevel(GetInteger(sqlRdr("ECMControlLevelId")))
                objItem.ECMControlLevelId = GetInteger(sqlRdr("ECMControlLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZECMControlLevel)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        'shankar
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ECMControlLevelId From eZECMControlLevel Where ECMProfileid = @ECMProfileId and ECMControlId = @ECMControlId " +
            "and ECMControlLevelId <> @ECMControlLevelId and Isdeleted=0"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@ECMProfileid", objToUpdate.ECMProfileId)
        objParam(0) = param
        param = New SqlParameter("@ECMControlId", objToUpdate.ECMControlId)
        objParam(1) = param
        param = New SqlParameter("@ECMControlLevelId", objToUpdate.ECMControlLevelId)
        objParam(2) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ECMControlLevel Code already exist!")
        Else
            strQry = "Update eZECMControlLevel Set ECMControlId=@ECMControlId,ECMProfileId=@ECMProfileId,templateid=@templateid,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy" +
                "  where ECMControlLevelId=@ECMControlLevel_ID"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@ECMControlId", objToUpdate.ECMControlId)
            objParam(0) = param
            param = New SqlParameter("@ECMProfileId", objToUpdate.ECMProfileId)
            objParam(1) = param
            param = New SqlParameter("@templateid", objToUpdate.templateid)
            objParam(2) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(3) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(4) = param
            param = New SqlParameter("@ECMControlLevel_ID", objToUpdate.ECMControlLevelId)
            objParam(5) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMControlLevel)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMControlLevel set Isdeleted=1,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where ECMControlLevelId=@ECMControlLevel_ID"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@UpdatedOn", objToDelete.UpdatedOn)
        objParam(0) = param
        param = New SqlParameter("@UpdatedBy", objToDelete.UpdatedBy)
        objParam(1) = param
        param = New SqlParameter("@ECMControlLevel_ID", objToDelete.ECMControlLevelId)
        objParam(2) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Function ReadFilteredeZECMControlLevel(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMControlLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMControlLevel)()
        Dim objItem As IeZECMControlLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMControlLevelId From eZECMControlLevel where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMControlLevelId"
            Else
                strQry = "Select ECMControlLevelId From eZECMControlLevel where Isdeleted=0 order by ECMControlLevelId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMControlLevel(GetInteger(sqlRdr("ECMControlLevelId")))
                objItem.ECMControlLevelId = GetInteger(sqlRdr("ECMControlLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMControlLevel(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMControlLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMControlLevel)()
        Dim objItem As IeZECMControlLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMControlLevelId From eZECMControlLevel where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMControlLevelId"
            Else
                strQry = "Select ECMControlLevelId From eZECMControlLevel where Isdeleted=0 order by ECMControlLevelId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMControlLevel(GetInteger(sqlRdr("ECMControlLevelId")))
                objItem.ECMControlLevelId = GetInteger(sqlRdr("ECMControlLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMControlLevelWithProfileId(Criteria As String, Value As String, ProfileId As String) As System.Collections.Generic.List(Of IeZECMControlLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMControlLevel)()
        Dim objItem As IeZECMControlLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMControlLevelId From eZECMControlLevel where Isdeleted=0 and ECMprofileid=" + ProfileId + " and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMControlLevelId"
            Else
                strQry = "Select ECMControlLevelId From eZECMControlLevel where Isdeleted=0 order by ECMControlLevelId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMControlLevel(GetInteger(sqlRdr("ECMControlLevelId")))
                objItem.ECMControlLevelId = GetInteger(sqlRdr("ECMControlLevelId"))
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
