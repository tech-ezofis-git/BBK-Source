Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "User MailArchives"

    Public Function CreateMailArchive(objEmp As eZMailArchive) As IeZMailArchive
        Dim newObject As IeZMailArchive = Nothing

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select MailArchiveId From eZMailArchive Where CreatedBy=@CreatedBy and ScheduleId = @ScheduleId and MailArchiveTypeId = @MailArchiveTypeId And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(0) = param
            param = New SqlParameter("@ScheduleId", objEmp.ScheduleId)
            objParam(1) = param
            param = New SqlParameter("@MailArchiveTypeId", objEmp.MailArchiveTypeId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("MailArchiveValueId Code already exist!")
            End If
            strQry = "INSERT INTO eZMailArchive(ScheduleId,MailArchiveTypeId,CreatedOn,CreatedBy) VALUES(@ScheduleId,@MailArchiveTypeId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@ScheduleId", objEmp.ScheduleId)
            objParam(0) = param
            param = New SqlParameter("@MailArchiveTypeId", objEmp.MailArchiveTypeId)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZMailArchive(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function

    Public Sub InsertAndUpdateMailArchiveValue(objEmp As eZMailArchive)
        Dim newObject As IeZMailArchive = Nothing
        Try
            If objEmp.MailArchiveValueId = 0 Then
                Dim strQry As String = ""
                Dim objParam As SqlParameter()
                Dim param As SqlParameter
                strQry = "INSERT INTO eZMailArchiveValue(MailArchiveValue,MailArchiveId,CreatedOn,CreatedBy) VALUES(@MailArchiveValue,@MailArchiveId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
                objParam = New SqlParameter(3) {}
                param = New SqlParameter("@MailArchiveValue", objEmp.MailArchiveValue)
                objParam(0) = param
                param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
                objParam(1) = param
                param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
                objParam(2) = param
                param = New SqlParameter("@MailArchiveId", objEmp.MailArchiveId)
                objParam(3) = param
                Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
                If obj Is Nothing Then
                    Throw New Exception("Record Not updated due to some error")
                End If
            Else
                Dim strQry As String = ""

                strQry = "Update eZMailArchiveValue Set MailArchiveId=" + objEmp.MailArchiveId.ToString() + ",MailArchiveValue=N'" + objEmp.MailArchiveValue.ToString() + "',UpdatedOn=N'" + objEmp.UpdatedOn.ToString() + "',UpdatedBy=" + objEmp.UpdatedBy.ToString() + " where MailArchiveValueId=" + objEmp.MailArchiveValueId.ToString() + ""

                DBLayer.DBLInstance.InsertAndUpdate(strQry)
                'If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                '    Throw New Exception("Record Not updated due to some error")
                'End If
            End If

        Catch e As Exception
            Throw New Exception(e.Message)
        End Try
    End Sub
    Public Sub Read(objRead As IeZMailArchive)
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
            If objRead.MailArchiveTypeId = 0 Then
                strQry = "Select Arch.*,Val.MailArchiveValueId as MailArchiveValueId,Val.MailArchiveValue as MailArchiveValue,dbo.udf_UserName(Arch.UpdatedBy) as UpdatedBy1,dbo.udf_UserName(Arch.CreatedBy) as CreatedBy1,dbo.udf_MailArchiveType(Arch.MailArchiveTypeId) as MailArchiveType,dbo.udf_MailArchiveValue(Val.MailArchiveValueId) as MailArchiveValue From eZMailArchive Arch left outer join eZMailArchiveValue Val on Arch.MailArchiveId =Val.MailArchiveId Where Arch.MailArchiveId=@MailArchiveId and Arch.Isdeleted=0 and Val.Isdeleted=0"
                param = New SqlParameter("@MailArchiveId", objRead.MailArchiveId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(0) {}
                strQry = "Select Arch.*,dbo.udf_UserName(Arch.UpdatedBy) as UpdatedBy1,dbo.udf_UserName(Arch.CreatedBy) as CreatedBy1,dbo.udf_MailArchiveType(Arch.MailArchiveTypeId) as MailArchiveType,dbo.udf_MailArchiveValue(Val.MailArchiveValueId) as MailArchiveValue From eZMailArchive Arch left outer join eZMailArchiveValue Val on Arch.MailArchiveId =Val.MailArchiveId Where Arch.MailArchiveTypeId=@MailArchiveTypeId and Arch.Isdeleted=0 and Val.Isdeleted=0"
                param = New SqlParameter("@MailArchiveTypeId", objRead.MailArchiveTypeId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid MailArchiveValueId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.MailArchiveId = GetInteger(sqlRdr("MailArchiveId"))
                objRead.MailArchiveValueId = GetInteger(sqlRdr("MailArchiveValueId"))
                objRead.ScheduleId = GetInteger(sqlRdr("ScheduleId"))
                objRead.MailArchiveTypeId = GetInteger(sqlRdr("MailArchiveTypeId"))
                objRead.MailArchiveType = sqlRdr("MailArchiveType").ToString()
                objRead.MailArchiveValue = sqlRdr("MailArchiveValue").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
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
    Public Function ReadAllMailArchive() As System.Collections.Generic.List(Of IeZMailArchive)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailArchive)()
        Dim objItem As IeZMailArchive

        Try
            Dim strQry As String = ""
            strQry = "Select MailArchiveId From eZMailArchive where Isdeleted=0 order by ScheduleId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ScheduleId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailArchive(GetInteger(sqlRdr("MailArchiveId")))
                objItem.MailArchiveId = GetInteger(sqlRdr("MailArchiveId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZMailArchive)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select MailArchiveId From eZMailArchive Where ScheduleId = @ScheduleId and MailArchiveId <> @MailArchiveId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ScheduleId", objToUpdate.ScheduleId)
        objParam(0) = param
        param = New SqlParameter("@MailArchiveId", objToUpdate.MailArchiveId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ScheduleId Code already exist!")
        Else
            strQry = "Update eZMailArchive Set ScheduleId=@ScheduleId,MailArchiveTypeId=@MailArchiveTypeId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where MailArchiveId=@MailArchive_ID"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@ScheduleId", objToUpdate.ScheduleId)
            objParam(0) = param
            param = New SqlParameter("@MailArchive_ID", objToUpdate.MailArchiveId)
            objParam(1) = param
            param = New SqlParameter("@MailArchiveTypeId", objToUpdate.MailArchiveTypeId)
            objParam(2) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(3) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(4) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZMailArchive)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailArchive set Isdeleted=1 where MailArchiveId=@MailArchive_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@MailArchive_ID", objToDelete.MailArchiveId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub

    Public Sub DeleteMailArchiveType(MailArchiveTypeId As Integer)

        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZMailArchive set Isdeleted=1 where MailArchiveTypeId=@MailArchiveTypeId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@MailArchiveTypeId", MailArchiveTypeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Function ReadFilteredeZMailArchive(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailArchive)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailArchive)()
        Dim objItem As IeZMailArchive

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailArchiveId From eZMailArchive where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ScheduleId"
            Else
                strQry = "Select MailArchiveId From eZMailArchive where Isdeleted=0 order by ScheduleId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailArchive(GetInteger(sqlRdr("MailArchiveId")))
                objItem.MailArchiveId = GetInteger(sqlRdr("MailArchiveId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMailArchive(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZMailArchive)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailArchive)()
        Dim objItem As IeZMailArchive
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailArchiveId From eZMailArchive where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ScheduleId"
            Else
                strQry = "Select MailArchiveId From eZMailArchive where Isdeleted=0 order by ScheduleId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailArchive(GetInteger(sqlRdr("MailArchiveId")))
                objItem.MailArchiveId = GetInteger(sqlRdr("MailArchiveId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZMailArchiveWithMailArchiveTypeId(Criteria As String, Value As String, MailArchiveTypeId As String) As System.Collections.Generic.List(Of IeZMailArchive)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZMailArchive)()
        Dim objItem As IeZMailArchive

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select MailArchiveId From eZMailArchive where Isdeleted=0 and MailArchiveValueId=" + MailArchiveTypeId + " and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ScheduleId"
            Else
                strQry = "Select MailArchiveId From eZMailArchive where Isdeleted=0 order by ScheduleId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZMailArchive(GetInteger(sqlRdr("MailArchiveId")))
                objItem.MailArchiveId = GetInteger(sqlRdr("MailArchiveId"))
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
