Imports ECMAPI.DBLibrary
Imports System.Data.SqlClient
Partial Public Class DBLayer
#Region "Core"
    Public Sub Read(objRead As IeZFieldAlertDoc)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZFieldAlertDoc ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                "Where ez.FieldAlertDocId=@FieldAlertDocId and ez.Isdeleted=0"
            param = New SqlParameter("@FieldAlertDocId", objRead.FieldAlertDocId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlertDoc")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.FieldAlertDetailId = GetInteger(sqlRdr("FieldAlertDetailId"))
                objRead.FieldAlertDocId = GetInteger(sqlRdr("FieldAlertDocId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.Filename = sqlRdr("Filename").ToString
                objRead.ToMail = sqlRdr("ToMail").ToString
                objRead.itemid = GetInteger(sqlRdr("itemid"))
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
    Public Function CreateeZFieldAlertDoc(objEmp As eZFieldAlertDoc) As eZFieldAlertDoc
        Dim newObject As eZFieldAlertDoc = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "INSERT INTO eZFieldAlertDoc(FieldAlertDetailId,TemplateId,Filename,ToMail,itemid,CreatedBy,CreatedOn) VALUES " +
                "(@FieldAlertDetailId,@TemplateId,@Filename,@ToMail,@itemid,@CreatedBy,@CreatedOn);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@FieldAlertDetailId", objEmp.FieldAlertDetailId)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@Filename", objEmp.Filename)
            objParam(2) = param
            param = New SqlParameter("@ToMail", objEmp.ToMail)
            objParam(3) = param
            param = New SqlParameter("@itemid", objEmp.itemid)
            objParam(4) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(5) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(6) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZFieldAlertDoc(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZFieldAlertDoc)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFieldAlertDoc Set FieldAlertDetailId=@FieldAlertDetailId,TemplateId=@TemplateId,Filename=@Filename,ToMail=@ToMail," +
            "itemid=@itemid,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where FieldAlertDocId=@FieldAlertDocId"
        objParam = New SqlParameter(7) {}
        param = New SqlParameter("@FieldAlertDetailId", objToUpdate.FieldAlertDetailId)
        objParam(0) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(1) = param
        param = New SqlParameter("@Filename", objToUpdate.Filename)
        objParam(2) = param
        param = New SqlParameter("@ToMail", objToUpdate.ToMail)
        objParam(3) = param
        param = New SqlParameter("@itemid", objToUpdate.itemid)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(6) = param
        param = New SqlParameter("@FieldAlertDocId", objToUpdate.FieldAlertDocId)
        objParam(7) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZFieldAlertDoc)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZFieldAlertDoc set Isdeleted=1 where FieldAlertDocId=@FieldAlertDocId "
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@FieldAlertDocId", objToDelete.FieldAlertDocId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region
    Public Function ReadAlleZFieldAlertDoc() As System.Collections.Generic.List(Of IeZFieldAlertDoc)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlertDoc)()
        Dim objItem As IeZFieldAlertDoc
        Try
            Dim strQry As String = ""
            strQry = "Select FieldAlertDocId From eZFieldAlertDoc where IsDeleted=0"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlertDoc")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlertDoc(GetInteger(sqlRdr("FieldAlertDocId")))
                objItem.FieldAlertDocId = GetInteger(sqlRdr("FieldAlertDocId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZFieldAlertDoc(Criteria As String, Value As String) As List(Of IeZFieldAlertDoc)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlertDoc)()
        Dim objItem As IeZFieldAlertDoc
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldAlertDocId From eZFieldAlertDoc where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by FieldAlertDocId"
            Else
                strQry = "Select FieldAlertDocId From eZFieldAlertDoc where Isdeleted=0 order by FieldAlertDocId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlertDoc")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlertDoc(GetInteger(sqlRdr("FieldAlertDocId")))
                objItem.FieldAlertDocId = GetInteger(sqlRdr("FieldAlertDocId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZFieldAlertDoc(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFieldAlertDoc)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFieldAlertDoc)()
        Dim objItem As IeZFieldAlertDoc
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select FieldAlertDocId From eZFieldAlertDoc where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " = N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by FieldAlertDocId"
            Else
                strQry = "Select FieldAlertDocId From eZFieldAlertDoc where Isdeleted=0 order by FieldAlertDocId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFieldAlertDoc")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFieldAlertDoc(GetInteger(sqlRdr("FieldAlertDocId")))
                objItem.FieldAlertDocId = GetInteger(sqlRdr("FieldAlertDocId"))
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
