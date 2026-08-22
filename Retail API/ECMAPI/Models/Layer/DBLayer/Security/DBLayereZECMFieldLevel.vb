Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "User ECMFieldLevels"
#Region "Core"
    Public Function CreateECMFieldLevel(objEmp As eZECMFieldLevel) As IeZECMFieldLevel
        Dim newObject As IeZECMFieldLevel = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            'strQry = "Select ECMFieldLevelId From eZECMFieldLevel Where ECMLoginId = @ECMLoginId and FieldId = @FieldId And Isdeleted=0"
            'objParam = New SqlParameter(1) {}
            'param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            'objParam(0) = param
            'param = New SqlParameter("@FieldId", objEmp.FieldId)
            'objParam(1) = param
            'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            'If obj IsNot Nothing Then
            '    Throw New Exception("ECMLoginId Code already exist!")
            'End If
            strQry = "INSERT INTO eZECMFieldLevel(ECMLoginId,FieldId,FieldValue,conditionid,templateid,visibility,createdon,createdby,ECMGroupId) " +
                "VALUES(@ECMLoginId,@FieldId,@FieldValue,@conditionid,@templateid,@visibility,@createdon,@createdby,@ECMGroupId);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(8) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@FieldId", objEmp.FieldId)
            objParam(1) = param
            param = New SqlParameter("@FieldValue", objEmp.FieldValue)
            objParam(2) = param
            param = New SqlParameter("@conditionid", objEmp.ConditionId)
            objParam(3) = param
            param = New SqlParameter("@templateid", objEmp.TemplateId)
            objParam(4) = param
            param = New SqlParameter("@visibility", objEmp.Visibility)
            objParam(5) = param
            param = New SqlParameter("@createdon", objEmp.CreatedOn)
            objParam(6) = param
            param = New SqlParameter("@createdby", objEmp.CreatedBy)
            objParam(7) = param
            param = New SqlParameter("@ECMGroupId", objEmp.ECMGroupId)
            objParam(8) = param
            Dim obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZECMFieldLevel(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZECMFieldLevel)
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
            If objRead.ECMLoginId = 0 Then
                strQry = "Select *,dbo.udf_LoginName(ECMLoginId) as LoginName,dbo.udf_UserName(UpdatedBy) as UpdatedBy1," +
                    "dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZECMFieldLevel Where ECMFieldLevelId=@ECMFieldLevel_ID and Isdeleted=0"
                param = New SqlParameter("@ECMFieldLevel_ID", objRead.ECMFieldLevelId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_LoginName(ECMLoginId) as LoginName,dbo.udf_UserName(UpdatedBy) as UpdatedBy1," +
                    "dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZECMFieldLevel Where ECMLoginId=@ECMLoginId and Isdeleted=0"
                param = New SqlParameter("@ECMLoginId", objRead.ECMLoginId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMLoginId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ECMFieldLevelId = GetInteger(sqlRdr("ECMFieldLevelId"))
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                objRead.FieldId = GetInteger(sqlRdr("FieldId"))
                objRead.FieldValue = sqlRdr("FieldValue").ToString()
                objRead.LoginName = sqlRdr("LoginName").ToString()
                objRead.ConditionId = GetInteger(sqlRdr("ConditionId"))
                objRead.Visibility = GetInteger(sqlRdr("Visibility"))
                objRead.TemplateId = GetInteger(sqlRdr("Templateid"))
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.ECMGroupId = GetInteger(sqlRdr("ECMGroupId"))
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
    Public Sub Update(objToUpdate As IeZECMFieldLevel)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        'objParam = New SqlParameter(1) {}
        'param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        'objParam(0) = param
        'param = New SqlParameter("@ECMFieldLevelId", objToUpdate.ECMFieldLevelId)
        'objParam(1) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("ECMLoginId Code already exist!")
        'Else
        strQry = "Update eZECMFieldLevel Set ECMLoginId=@ECMLoginId,FieldValue=@FieldValue,FieldId=@FieldId,conditionid=@conditionid," +
            "Visibility=@Visibility,templateid=@templateid,updatedby=@updatedby,updatedon=@updatedon,ECMGroupId=@ECMGroupId " +
            " where ECMFieldLevelId=@ECMFieldLevel_ID"
        objParam = New SqlParameter(9) {}
        param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        objParam(0) = param
        param = New SqlParameter("@ECMFieldLevel_ID", objToUpdate.ECMFieldLevelId)
        objParam(1) = param
        param = New SqlParameter("@FieldId", objToUpdate.FieldId)
        objParam(2) = param
        param = New SqlParameter("@FieldValue", objToUpdate.FieldValue)
        objParam(3) = param
        param = New SqlParameter("@conditionid", objToUpdate.ConditionId)
        objParam(4) = param
        param = New SqlParameter("@Visibility", objToUpdate.Visibility)
        objParam(5) = param
        param = New SqlParameter("@templateid", objToUpdate.TemplateId)
        objParam(6) = param
        param = New SqlParameter("@updatedby", objToUpdate.UpdatedBy)
        objParam(7) = param
        param = New SqlParameter("@updatedon", objToUpdate.UpdatedOn)
        objParam(8) = param
        param = New SqlParameter("@ECMGroupId", objToUpdate.ECMGroupId)
        objParam(9) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMFieldLevel)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMFieldLevel set Isdeleted=1 where ECMFieldLevelId=@ECMFieldLevel_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ECMFieldLevel_ID", objToDelete.ECMFieldLevelId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAllECMFieldLevel() As System.Collections.Generic.List(Of IeZECMFieldLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMFieldLevel)()
        Dim objItem As IeZECMFieldLevel
        Try
            Dim strQry As String = ""
            strQry = "Select ECMFieldLevelId From eZECMFieldLevel where Isdeleted=0 order by ECMLoginId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECM Field Level.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMFieldLevel(GetInteger(sqlRdr("ECMFieldLevelId")))
                objItem.ECMFieldLevelId = GetInteger(sqlRdr("ECMFieldLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadFilteredeZECMFieldLevel(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMFieldLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMFieldLevel)()
        Dim objItem As IeZECMFieldLevel
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMFieldLevelId From eZECMFieldLevel where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMLoginId,ECMFieldLevelId"
            Else
                strQry = "Select ECMFieldLevelId From eZECMFieldLevel where Isdeleted=0 order by ECMLoginId,ECMFieldLevelId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECM Field Level.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMFieldLevel(GetInteger(sqlRdr("ECMFieldLevelId")))
                objItem.ECMFieldLevelId = GetInteger(sqlRdr("ECMFieldLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMFieldLevel(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMFieldLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMFieldLevel)()
        Dim objItem As IeZECMFieldLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMFieldLevelId From eZECMFieldLevel where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId,ECMFieldLevelId"
            Else
                strQry = "Select ECMFieldLevelId From eZECMFieldLevel where Isdeleted=0 order by ECMLoginId,ECMFieldLevelId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECM Field Level.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMFieldLevel(GetInteger(sqlRdr("ECMFieldLevelId")))
                objItem.ECMFieldLevelId = GetInteger(sqlRdr("ECMFieldLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    'Public Function ReadSelectedeZECMFieldLevelWithProfileId(Criteria As String, Value As String, ProfileId As String) As System.Collections.Generic.List(Of IeZECMFieldLevel)
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    Dim lstItems As New System.Collections.Generic.List(Of IeZECMFieldLevel)()
    '    Dim objItem As IeZECMFieldLevel

    '    Try
    '        Dim strQry As String = ""
    '        If Criteria <> "All" Then
    '            strQry = "Select ECMFieldLevelId From eZECMFieldLevel where Isdeleted=0 and ECMLoginId=" + ProfileId + " and " + Criteria
    '            'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
    '            strQry = strQry & " =N'"
    '            strQry = strQry & Unquote(Value)
    '            strQry = strQry & "' "
    '            strQry = strQry & " order by ECMLoginId"
    '        Else
    '            strQry = "Select ECMFieldLevelId From eZECMFieldLevel where Isdeleted=0 order by ECMLoginId"
    '        End If
    '        Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid Profile.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        While sqlRdr.Read()
    '            objItem = GlobalInstance.eZECMFieldLevel(GetInteger(sqlRdr("ECMFieldLevelId")))
    '            objItem.ECMFieldLevelId = GetInteger(sqlRdr("ECMFieldLevelId"))
    '            lstItems.Add(objItem)
    '        End While
    '        Return lstItems
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '    End Try
    'End Function
#End Region
End Class
