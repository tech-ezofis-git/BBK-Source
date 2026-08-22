Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
    'Public Function CreateeZLinkedItems(objtemp As eZLinkedItems) As IeZLinkedItems
    '    Dim newObject As IeZLinkedItems = Nothing
    '    Try
    '        Dim strQry As String = ""
    '        Dim objParam As SqlParameter()
    '        Dim param As SqlParameter
    '        strQry = "Select Linkedid From eZLinkedItems Where SourceFieldid = @SourceFieldid And Linkedfieldid=@Linkedfieldid " +
    '            "And templateid=@templateid and Isdeleted=0"
    '        objParam = New SqlParameter(2) {}
    '        param = New SqlParameter("@SourceFieldid", objtemp.SourceFieldid)
    '        objParam(0) = param
    '        param = New SqlParameter("@Linkedfieldid", objtemp.Linkedfieldid)
    '        objParam(1) = param
    '        param = New SqlParameter("@TemplateId", objtemp.templateid)
    '        objParam(2) = param
    '        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
    '        If obj IsNot Nothing Then
    '            Throw New Exception("eZLinkeditems Code already exist!")
    '        End If
    '        strQry = "INSERT INTO eZLinkedItems(SourceFieldid,Linkedfieldid,TemplateId,CreatedOn,CreatedBy) " +
    '            "VALUES(@SourceFieldid,@Linkedfieldid,@TemplateId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
    '        objParam = New SqlParameter(4) {}
    '        param = New SqlParameter("@SourceFieldid", objtemp.SourceFieldid)
    '        objParam(0) = param
    '        param = New SqlParameter("@Linkedfieldid", objtemp.Linkedfieldid)
    '        objParam(1) = param
    '        param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
    '        objParam(2) = param
    '        param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
    '        objParam(3) = param
    '        param = New SqlParameter("@TemplateId", objtemp.templateid)
    '        objParam(4) = param
    '        obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
    '        If obj Is Nothing Then
    '            Return Nothing
    '        End If
    '        newObject = GlobalInstance.ezlinkeditems(Convert.ToInt32(obj))
    '        Read(newObject)
    '        Return newObject
    '    Catch e As Exception
    '        Throw New Exception(e.Message)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Sub Read(objRead As IeZLinkedItems)
    '    If objRead.IsReadFromDB Then
    '        Return
    '    End If
    '    If objRead.IsModified Then
    '        Throw New InvalidOperationException()
    '    End If
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    objRead.IsReadFromDB = True
    '    Try
    '        Dim strQry As String = ""
    '        Dim objParam As SqlParameter()
    '        Dim param As SqlParameter
    '        objParam = New SqlParameter(0) {}
    '        'If objRead.CreatedOn Is Nothing Then
    '        strQry = "Select * From eZLinkedItems Where Isdeleted=0 and Linkedid=@Linkedid"
    '        param = New SqlParameter("@Linkedid", objRead.Linkedid)
    '        objParam(0) = param
    '        Dim obj As Object = ""
    '        obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid eZLinkeditems.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        If sqlRdr.Read() Then
    '            objRead.Linkedid = GetInteger(sqlRdr("Linkedid"))
    '            objRead.templateid = GetInteger(sqlRdr("templateid"))
    '            objRead.SourceFieldid = GetInteger(sqlRdr("SourceFieldid"))
    '            objRead.Linkedfieldid = GetInteger(sqlRdr("Linkedfieldid"))
    '            objRead.CreatedOn = sqlRdr("CreatedOn").ToString
    '            objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
    '            objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
    '            objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
    '        Else
    '            Return
    '        End If
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '        objRead.IsModified = False
    '    End Try
    'End Sub
    'Public Function ReadAlleZLinkedItems() As System.Collections.Generic.List(Of IeZLinkedItems)
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    Dim lstItems As New System.Collections.Generic.List(Of IeZLinkedItems)()
    '    Dim objItem As IeZLinkedItems
    '    Try
    '        Dim strQry As String = ""
    '        strQry = "Select Linkedid From eZLinkedItems where Isdeleted=0 order by Linkedid"
    '        Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid eZLinkedItems.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        While sqlRdr.Read()
    '            objItem = GlobalInstance.ezlinkeditems(GetSmallInterger(sqlRdr("Linkedid")))
    '            objItem.Linkedid = GetSmallInterger(sqlRdr("Linkedid"))
    '            lstItems.Add(objItem)
    '        End While
    '        Return lstItems
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '    End Try
    'End Function
    'Public Function ReadFilteredeZLinkedItems(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLinkedItems)
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    Dim lstItems As New System.Collections.Generic.List(Of IeZLinkedItems)()
    '    Dim objItem As IeZLinkedItems
    '    Try
    '        Dim strQry As String = ""
    '        If Criteria <> "All" Then
    '            strQry = "Select Linkedid From eZLinkedItems where Isdeleted=0 and "
    '            strQry = strQry & Criteria
    '            strQry = strQry & " like N'%"
    '            strQry = strQry & Unquote(Value)
    '            strQry = strQry & "%' "
    '            strQry = strQry & " order by Linkedid"
    '        Else
    '            strQry = "Select Linkedid From eZLinkedItems where Isdeleted=0 order by Linkedid"
    '        End If
    '        Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid eZLinkedItems.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        While sqlRdr.Read()
    '            objItem = GlobalInstance.ezlinkeditems(GetSmallInterger(sqlRdr("Linkedid")))
    '            objItem.Linkedid = GetSmallInterger(sqlRdr("Linkedid"))
    '            lstItems.Add(objItem)
    '        End While
    '        Return lstItems
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '    End Try
    'End Function
    'Public Function ReadSelectedeZLinkedItems(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLinkedItems)
    '    Dim sqlRdr As SqlDataReader = Nothing
    '    Dim lstItems As New System.Collections.Generic.List(Of IeZLinkedItems)()
    '    Dim objItem As IeZLinkedItems
    '    Try
    '        Dim strQry As String = ""
    '        If Criteria <> "All" Then
    '            strQry = "Select Linkedid From eZLinkedItems where Isdeleted=0  and "
    '            strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
    '            strQry = strQry & " =N'"
    '            strQry = strQry & Unquote(Value)
    '            strQry = strQry & "' "
    '            strQry = strQry & " order by Linkedid"
    '        Else
    '            strQry = "Select Linkedid From eZLinkedItems where Isdeleted=0 order by Linkedid"
    '        End If
    '        Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
    '        If obj Is Nothing Then
    '            Throw New Exception("Attempt to read Invalid eZLinkedItems.")
    '        End If
    '        sqlRdr = DirectCast(obj, SqlDataReader)
    '        While sqlRdr.Read()
    '            objItem = GlobalInstance.ezlinkeditems(GetSmallInterger(sqlRdr("Linkedid")))
    '            objItem.Linkedid = GetSmallInterger(sqlRdr("Linkedid"))
    '            lstItems.Add(objItem)
    '        End While
    '        Return lstItems
    '    Finally
    '        If sqlRdr IsNot Nothing Then
    '            sqlRdr.Close()
    '        End If
    '    End Try
    'End Function
#Region "Core"
    Public Sub Read(objRead As IeZLinkedItems)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZLinkedItems ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.Linkedid=@Linkedid and ez.Isdeleted=0"
            param = New SqlParameter("@Linkedid", objRead.Linkedid)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLinkedItems")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.Linkedid = GetInteger(sqlRdr("Linkedid"))
                objRead.SourceFieldid = GetInteger(sqlRdr("SourceFieldid"))
                objRead.Linkedfieldid = GetInteger(sqlRdr("Linkedfieldid"))
                objRead.templateid = GetInteger(sqlRdr("TemplateId"))
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
    Public Function CreateeZLinkedItems(objEmp As eZLinkedItems) As eZLinkedItems
        Dim newObject As eZLinkedItems = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select Linkedid From eZLinkedItems Where SourceFieldid = @SourceFieldid And Linkedfieldid=@Linkedfieldid " +
                "And templateid=@templateid and Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@SourceFieldid", objEmp.SourceFieldid)
            objParam(0) = param
            param = New SqlParameter("@Linkedfieldid", objEmp.Linkedfieldid)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objEmp.templateid)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZLinkeditems Code already exist!")
            End If
            strQry = "INSERT INTO eZLinkedItems(TemplateId,SourceFieldid,Linkedfieldid,CreatedBy,CreatedOn) VALUES " +
                "(@TemplateId,@SourceFieldid,@Linkedfieldid,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@TemplateId", objEmp.templateid)
            objParam(0) = param
            param = New SqlParameter("@SourceFieldid", objEmp.SourceFieldid)
            objParam(1) = param
            param = New SqlParameter("@Linkedfieldid", objEmp.Linkedfieldid)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(4) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.ezlinkeditems(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZLinkedItems)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLinkedItems Set TemplateId=@TemplateId,SourceFieldid=@SourceFieldid,Linkedfieldid=@Linkedfieldid," +
            "UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where Linkedid=@Linkedid"
        objParam = New SqlParameter(5) {}
        param = New SqlParameter("@TemplateId", objToUpdate.templateid)
        objParam(0) = param
        param = New SqlParameter("@SourceFieldid", objToUpdate.SourceFieldid)
        objParam(1) = param
        param = New SqlParameter("@Linkedfieldid", objToUpdate.Linkedfieldid)
        objParam(2) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@Linkedid", objToUpdate.Linkedid)
        objParam(5) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZLinkedItems)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZLinkedItems set Isdeleted=1 where Linkedid=@Linkedid "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Linkedid", objToDelete.Linkedid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZLinkedItems() As System.Collections.Generic.List(Of IeZLinkedItems)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLinkedItems)()
        Dim objItem As IeZLinkedItems
        Try
            Dim strQry As String = ""
            strQry = "Select Linkedid From eZLinkedItems where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLinkedItems")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezlinkeditems(GetInteger(sqlRdr("Linkedid")))
                objItem.Linkedid = GetInteger(sqlRdr("Linkedid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZLinkedItems(Criteria As String, Value As String) As List(Of IeZLinkedItems)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLinkedItems)()
        Dim objItem As IeZLinkedItems
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Linkedid From eZLinkedItems where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by Linkedid"
            Else
                strQry = "Select Linkedid From eZLinkedItems where Isdeleted=0 order by Linkedid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLinkedItems")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezlinkeditems(GetInteger(sqlRdr("Linkedid")))
                objItem.Linkedid = GetInteger(sqlRdr("Linkedid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZLinkedItems(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZLinkedItems)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZLinkedItems)()
        Dim objItem As IeZLinkedItems
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Linkedid From eZLinkedItems where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by Linkedid"
            Else
                strQry = "Select Linkedid From eZLinkedItems where Isdeleted=0 order by Linkedid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZLinkedItems")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.ezlinkeditems(GetInteger(sqlRdr("Linkedid")))
                objItem.Linkedid = GetInteger(sqlRdr("Linkedid"))
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
