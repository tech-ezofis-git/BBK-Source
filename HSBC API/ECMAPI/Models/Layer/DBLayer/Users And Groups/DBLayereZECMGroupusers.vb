Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary

Partial Public Class DBLayer
    Public Function CreateeZECMGroupusers(objtemp As eZECMGroupusers) As IeZECMGroupusers
        Dim newObject As IeZECMGroupusers = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ECMGroupUserId From ezecmgroupusers Where ECMGroupId = @ECMGroupId and ECMLoginId=@ECMLoginId And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ECMGroupId", objtemp.ECMGroupId)
            objParam(0) = param
            param = New SqlParameter("@ECMLoginId", objtemp.ECMLoginId)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZECMgroupusers Code already exist!")
            End If
            strQry = "INSERT INTO eZECMgroupusers(ECMGroupId,ECMLoginId,CreatedOn,CreatedBy) " +
                "VALUES(@ECMGroupId,@ECMLoginId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@ECMGroupId", objtemp.ECMGroupId)
            objParam(0) = param
            param = New SqlParameter("@ECMLoginId", objtemp.ECMLoginId)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZECMGroupusers(Convert.ToInt32(obj))
            read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.ToString)
            Return Nothing
        End Try
    End Function
    Public Sub read(objread As IeZECMGroupusers)
        If objread.IsReadFromDB Then
            Return
        End If
        If objread.IsModified Then
            Throw New InvalidOperationException()
        End If
        Dim sqlRdr As SqlDataReader = Nothing
        objread.IsReadFromDB = True
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            objParam = New SqlParameter(0) {}
            strQry = "Select *, dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  " +
                "From eZECMGroupusers Where Isdeleted=0 and ECMGroupUserId=@ECMGroupUserId"
            param = New SqlParameter("@ECMGroupUserId", objread.ECMGroupUserId)
            objParam(0) = param

            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ezECMGroupUsers.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objread.ECMGroupUserId = GetInteger(sqlRdr("ECMGroupUserId"))
                objread.ECMGroupId = GetSmallInterger(sqlRdr("ECMGroupId"))
                objread.ECMLoginId = GetSmallInterger(sqlRdr("ECMLoginId").ToString())
                objread.CreatedOn = sqlRdr("CreatedOn").ToString
                objread.Createdby1 = sqlRdr("CreatedBy1").ToString()
                objread.CreatedBy = sqlRdr("CreatedBy").ToString()
                objread.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objread.updatedby1 = sqlRdr("UpdatedBy1").ToString()
                objread.UpdatedBy = GetSmallInterger(sqlRdr("UpdatedBy"))
            Else
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objread.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZECMGroupusers() As System.Collections.Generic.List(Of IeZECMGroupusers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMGroupusers)()
        Dim objItem As IeZECMGroupusers
        Try
            Dim strQry As String = ""
            strQry = "Select ECMGroupUserId From eZECMGroupusers where Isdeleted=0 order by ECMGroupUserId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZECMGroupusers.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMGroupusers(GetSmallInterger(sqlRdr("ECMGroupUserId")))
                objItem.ECMGroupUserId = GetSmallInterger(sqlRdr("ECMGroupUserId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMGroupusers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMGroupusers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMGroupusers)()
        Dim objItem As IeZECMGroupusers
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMGroupUserId From eZECMGroupusers where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(100)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                'strQry = strQry & " order by FieldLevel"
                strQry = strQry & "ORDER BY ECMGroupUserId"
            Else
                strQry = "Select ECMGroupUserId From eZECMGroupusers where Isdeleted=0 ORDER BY ECMGroupUserId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZECMGroupusers.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMGroupusers(GetSmallInterger(sqlRdr("ECMGroupUserId")))
                objItem.ECMGroupUserId = GetSmallInterger(sqlRdr("ECMGroupUserId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZECMGroupusers(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMGroupusers)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMGroupusers)()
        Dim objItem As IeZECMGroupusers
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMGroupUserId From eZECMGroupusers where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & "ORDER BY ECMGroupUserId"
            Else
                strQry = "Select ECMGroupUserId From eZECMGroupusers where Isdeleted=0 ORDER BY ECMGroupUserId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZECMGroupusers.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMGroupusers(GetSmallInterger(sqlRdr("ECMGroupUserId")))
                objItem.ECMGroupUserId = GetSmallInterger(sqlRdr("ECMGroupUserId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZECMGroupusers)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ECMGroupUserId From eZECMGroupusers Where ECMGroupId = @ECMGroupId and ECMLoginId=@ECMLoginId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ECMGroupId", objToUpdate.ECMGroupId)
        objParam(0) = param
        param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZECMGroupusers Code already exist!")
        Else
            strQry = "Update eZECMGroupusers Set ECMGroupId=@ECMGroupId,ECMLoginId=@ECMLoginId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy " +
                "where ECMGroupUserId=@ECMGroupUserId"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@ECMGroupId", objToUpdate.ECMGroupId)
            objParam(0) = param
            param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
            objParam(1) = param
            param = New SqlParameter("@ECMGroupUserId", objToUpdate.ECMGroupUserId)
            objParam(2) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(4) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(3) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry, objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error1")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMGroupusers)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ezECMGroupusers set Isdeleted=1 where ECMGroupUserId=@ECMGroupUserId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ECMGroupUserId", objToDelete.ECMGroupUserId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry, objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class
