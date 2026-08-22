Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "Documents Comments"


    Public Function CreateeZComments(objtemp As eZComments) As IeZComments
        Dim newObject As IeZComments = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            'strQry = "Select CommentsId From eZComments Where CommentsBy = @CommentsBy And itemid=@itemid and Isdeleted=0"
            'objParam = New SqlParameter(1) {}
            'param = New SqlParameter("@itemid", objtemp.itemid)
            'objParam(0) = param
            'param = New SqlParameter("@CommentsBy", objtemp.CommentsBy)
            'objParam(1) = param
            'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            'If obj IsNot Nothing Then
            '    Update(objtemp)
            '    'Return Nothing
            '    'Throw New Exception("eZComments  already exist!")
            'End If
            strQry = "INSERT INTO eZComments(TemplateId,itemid,Comments,CommentsBy,Processid,ExternalCommentsBy,CreatedOn,CreatedBy) VALUES(@TemplateId,@itemid,@Comments,@CommentsBy,@Processid,@ExternalCommentsBy,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(7) {}
           
            param = New SqlParameter("@itemid", objtemp.itemid)
            objParam(0) = param
            param = New SqlParameter("@Comments", objtemp.Comments)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@CommentsBy", objtemp.CommentsBy)
            objParam(3) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(4) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(5) = param
            param = New SqlParameter("@Processid", objtemp.Processid)
            objParam(6) = param
            param = New SqlParameter("@ExternalCommentsBy", objtemp.ExternalCommentsBy)
            objParam(7) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZComments(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZComments)
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
            If objRead.CreatedOn Is Nothing Then
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZComments Where Isdeleted=0 and CommentsId=@CommentsId"
                param = New SqlParameter("@CommentsId", objRead.CommentsId)
                objParam(0) = param
            Else
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZComments Where Isdeleted=0 and CommentsId=@CommentsId"
                param = New SqlParameter("@CommentsId", objRead.CommentsId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.CommentsId = GetInteger(sqlRdr("CommentsId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.itemid = GetInteger(sqlRdr("itemid"))
                objRead.Comments = sqlRdr("Comments").ToString
                objRead.CommentsBy = GetInteger(sqlRdr("CommentsBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                objRead.ExternalCommentsBy = sqlRdr("ExternalCommentsBy").ToString()
                objRead.Processid = GetInteger(sqlRdr("processid"))
            Else
                'throw new Exception("Attempt to read Invalid eZComments.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZComments() As System.Collections.Generic.List(Of IeZComments)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZComments)()
        Dim objItem As IeZComments
        Try
            Dim strQry As String = ""
            strQry = "Select CommentsId From eZComments where Isdeleted=0 order by CreatedOn"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZComments(GetSmallInterger(sqlRdr("CommentsId")))
                objItem.CommentsId = GetSmallInterger(sqlRdr("CommentsId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZComments(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZComments)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZComments)()
        Dim objItem As IeZComments
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select CommentsId From eZComments where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select CommentsId From eZComments where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZComments(GetSmallInterger(sqlRdr("CommentsId")))
                objItem.CommentsId = GetSmallInterger(sqlRdr("CommentsId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZComments(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZComments)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZComments)()
        Dim objItem As IeZComments
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select CommentsId From eZComments where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CreatedOn"
            Else
                strQry = "Select CommentsId From eZComments where Isdeleted=0 order by CreatedOn"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZComments(GetSmallInterger(sqlRdr("CommentsId")))
                objItem.CommentsId = GetSmallInterger(sqlRdr("CommentsId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
   
    Public Function ReadSelectedeZComments(Criteria As String, Value As String, TemplateId As String) As System.Collections.Generic.List(Of IeZComments)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZComments)()
        Dim objItem As IeZComments
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select CommentsId From eZComments where Isdeleted=0 and TemplateId=" + TemplateId.ToString() + " and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by CONVERT(DateTime, CreatedOn,101) DESC"
            Else
                strQry = "Select CommentsId From eZComments where Isdeleted=0 order by CONVERT(DateTime, CreatedOn,101) DESC"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZComments.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZComments(GetSmallInterger(sqlRdr("CommentsId")))
                objItem.CommentsId = GetSmallInterger(sqlRdr("CommentsId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZComments)
        'If Not objToUpdate.IsModified Then
        '    Return
        'End If
        'If Not objToUpdate.IsReadFromDB Then
        '    Return
        'End If
        'Dim strQry As String = ""
        'Dim objParam As SqlParameter()
        'Dim param As SqlParameter

        'strQry = "Update eZComments Set Comments=@Comments,CommentsBy=@CommentsBy,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where TemplateId=@TemplateId"
        '    objParam = New SqlParameter(6) {}
        'param = New SqlParameter("@Comments", objToUpdate.Comments)
        '    objParam(0) = param
        'param = New SqlParameter("@CommentsId", objToUpdate.CommentsId)
        '    objParam(1) = param
        'param = New SqlParameter("@CommentsBy", objToUpdate.CommentsBy)
        '    objParam(2) = param
        '    param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        '    objParam(3) = param
        '    param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        '    objParam(4) = param
        '    param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        '    objParam(5) = param
        'param = New SqlParameter("@DuplicateTypeId", objToUpdate)
        '    objParam(6) = param
        '    If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
        '    Throw New Exception("Record Not updated due to some error")

        '    End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZComments)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZComments set Isdeleted=1 where CommentsId=@CommentsId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@CommentsId", objToDelete.CommentsId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

