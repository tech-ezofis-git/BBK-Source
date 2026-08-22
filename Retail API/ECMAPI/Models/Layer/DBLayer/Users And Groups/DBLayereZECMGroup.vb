Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer

#Region "User ECMGroups"
    Public Function CreateECMGroup(objEmp As eZECMGroup) As IeZECMGroup
        Dim newObject As IeZECMGroup = Nothing
        If String.IsNullOrEmpty(objEmp.ECMGroup) Then
            Return Nothing
        End If
        objEmp.ECMGroup = objEmp.ECMGroup.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ECMGroupId From eZECMGroup Where ECMGroup = @ECMGroup And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ECMGroup", objEmp.ECMGroup)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ECMGroup Code already exist!")
            End If
            strQry = "INSERT INTO eZECMGroup(ECMGroup,Description,Createdon,Createdby) " +
                "VALUES(@ECMGroup,@Description,@Createdon,@Createdby);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@ECMGroup", objEmp.ECMGroup)
            objParam(0) = param
            param = New SqlParameter("@Description", objEmp.Description)
            objParam(1) = param
            param = New SqlParameter("@Createdon", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@Createdby", objEmp.CreatedBy)
            objParam(3) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZECMGroup(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.ToString)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZECMGroup)
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
            If objRead.ECMGroup Is Nothing Then
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 " +
                    "From eZECMGroup Where ECMGroupId=@ECMGroup_ID and Isdeleted=0"
                param = New SqlParameter("@ECMGroup_ID", objRead.ECMGroupId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 " +
                    "From eZECMGroup Where ECMGroup=@ECMGroup and Isdeleted=0"
                param = New SqlParameter("@ECMGroup", objRead.ECMGroup)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMGroup.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ECMGroupId = GetInteger(sqlRdr("ECMGroupId"))
                objRead.ECMGroup = sqlRdr("ECMGroup").ToString()
                objRead.Description = sqlRdr("Description").ToString()
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
    Public Function ReadAllECMGroup() As System.Collections.Generic.List(Of IeZECMGroup)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMGroup)()
        Dim objItem As IeZECMGroup
        Try
            Dim strQry As String = ""
            strQry = "Select ECMGroupId From eZECMGroup where Isdeleted=0 order by ECMGroup"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMGroup.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMGroup(GetInteger(sqlRdr("ECMGroupId")))
                objItem.ECMGroupId = GetInteger(sqlRdr("ECMGroupId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZECMGroup)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ECMGroupId From eZECMGroup Where ECMGroup = @ECMGroup and ECMGroupId <> @ECMGroupId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ECMGroup", objToUpdate.ECMGroup)
        objParam(0) = param
        param = New SqlParameter("@ECMGroupId", objToUpdate.ECMGroupId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ECMGroup Code already exist!")
        Else
            strQry = "Update eZECMGroup Set ECMGroup=@ECMGroup,Description=@Description,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where ECMGroupId=@ECMGroup_ID"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@ECMGroup", objToUpdate.ECMGroup)
            objParam(0) = param
            param = New SqlParameter("@ECMGroup_ID", objToUpdate.ECMGroupId)
            objParam(1) = param
            param = New SqlParameter("@Description", objToUpdate.Description)
            objParam(2) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(3) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(4) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry, objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMGroup)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ECMGroup set Isdeleted=1 where ECMGroupId=@ECMGroup_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ECMGroup_ID", objToDelete.ECMGroupId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry, objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Function ReadFilteredeZECMGroup(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMGroup)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMGroup)()
        Dim objItem As IeZECMGroup
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMGroupId From eZECMGroup where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMGroup"
            Else
                strQry = "Select ECMGroupId From eZECMGroup where Isdeleted=0 order by ECMGroup"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMGroup(GetInteger(sqlRdr("ECMGroupId")))
                objItem.ECMGroupId = GetInteger(sqlRdr("ECMGroupId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMGroup(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMGroup)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMGroup)()
        Dim objItem As IeZECMGroup
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMGroupId From eZECMGroup where Isdeleted=0 and " + Criteria
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMGroup"
            Else
                strQry = "Select ECMGroupId From eZECMGroup where Isdeleted=0 order by ECMGroup"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMGroup(GetInteger(sqlRdr("ECMGroupId")))
                objItem.ECMGroupId = GetInteger(sqlRdr("ECMGroupId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMGroupByProfileId(Value As String) As System.Collections.Generic.List(Of IeZECMGroup)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMGroup)()
        Dim objItem As IeZECMGroup
        Try
            Dim strQry As String = ""
            strQry = "Select ECMGroupId From eZECMGroup where Isdeleted=0 and ECMGroupId in(Select ECMGroupId From eZECMLogin  " +
                "where Isdeleted=0 and ECMProfileId=" + Value + " group by ECMGroupId )"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMGroup(GetInteger(sqlRdr("ECMGroupId")))
                objItem.ECMGroupId = GetInteger(sqlRdr("ECMGroupId"))
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
