Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "eZDocumentLink Details"
    Public Function CreateeZDocumentLink(objtemp As eZDocumentLink) As IeZDocumentLink
        Dim newObject As IeZDocumentLink = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select LinkId From eZDocumentLink Where itemid = @itemid And TemplateId=@TemplateId And LinkedItemId=@LinkedItemId and Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@itemid", objtemp.itemid)
            objParam(0) = param
            param = New SqlParameter("@LinkedItemId", objtemp.LinkedItemId)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZDocumentLink Code already exist!")
            End If
            strQry = "Select LinkId From eZDocumentLink Where itemid = @itemid And TemplateId=@TemplateId And LinkedItemId=@LinkedItemId and Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@itemid", objtemp.LinkedItemId)
            objParam(0) = param
            param = New SqlParameter("@LinkedItemId", objtemp.itemid)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(2) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZDocumentLink Code already exist!")
            End If
            strQry = "INSERT INTO eZDocumentLink(LinkedItemId,itemid,LinkBy,TemplateId,LinkedTemplateId,CreatedOn,CreatedBy) " +
                "VALUES(@LinkedItemId,@itemid,@LinkBy,@TemplateId,@LinkedTemplateId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@LinkedItemId", objtemp.LinkedItemId)
            objParam(0) = param
            param = New SqlParameter("@itemid", objtemp.itemid)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@LinkBy", objtemp.LinkBy)
            objParam(4) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(5) = param
            param = New SqlParameter("@LinkedTemplateId", objtemp.LinkedTemplateId)
            objParam(6) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZDocumentLink(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZDocumentLink)
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
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  " +
                "From eZDocumentLink Where Isdeleted=0 and LinkId=@LinkId"
            param = New SqlParameter("@LinkId", objRead.LinkId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZDocumentLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.LinkId = GetInteger(sqlRdr("LinkId"))
                objRead.LinkedItemId = GetInteger(sqlRdr("LinkedItemId"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.LinkedTemplateId = GetInteger(sqlRdr("LinkedTemplateId"))
                objRead.itemid = GetInteger(sqlRdr("itemid"))
                objRead.LinkBy = GetInteger(sqlRdr("LinkBy"))
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
    Public Function ReadAlleZDocumentLink() As System.Collections.Generic.List(Of IeZDocumentLink)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDocumentLink)()
        Dim objItem As IeZDocumentLink
        Try
            Dim strQry As String = ""
            strQry = "Select LinkId From eZDocumentLink where Isdeleted=0 order by itemid"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZDocumentLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZDocumentLink(GetSmallInterger(sqlRdr("LinkId")))
                objItem.LinkId = GetSmallInterger(sqlRdr("LinkId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZDocumentLink(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZDocumentLink)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDocumentLink)()
        Dim objItem As IeZDocumentLink
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LinkId From eZDocumentLink where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by itemid"
            Else
                strQry = "Select LinkId From eZDocumentLink where Isdeleted=0 order by itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZDocumentLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZDocumentLink(GetSmallInterger(sqlRdr("LinkId")))
                objItem.LinkId = GetSmallInterger(sqlRdr("LinkId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZDocumentLink(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZDocumentLink)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDocumentLink)()
        Dim objItem As IeZDocumentLink
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LinkId From eZDocumentLink where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(200)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by itemid"
            Else
                strQry = "Select LinkId From eZDocumentLink where Isdeleted=0 order by itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZDocumentLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZDocumentLink(GetSmallInterger(sqlRdr("LinkId")))
                objItem.LinkId = GetSmallInterger(sqlRdr("LinkId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZDocumentLink)
        'If Not objToUpdate.IsModified Then
        '    Return
        'End If
        'If Not objToUpdate.IsReadFromDB Then
        '    Return
        'End If
        'Dim strQry As String = ""
        'Dim objParam As SqlParameter()
        'Dim param As SqlParameter
        'strQry = "Select LinkedItemId From eZDocumentLink Where TemplateName = @TemplateName and LinkedItemId <> @LinkedItemId and Isdeleted=0"
        'objParam = New SqlParameter(1) {}
        'param = New SqlParameter("@TemplateName", objToUpdate.TemplateName)
        'objParam(0) = param
        'param = New SqlParameter("@LinkedItemId", objToUpdate.LinkedItemId)
        'objParam(1) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("eZDocumentLink Code already exist!")
        'Else
        '    strQry = "Update eZDocumentLink Set TemplateName=@TemplateName,DuplicateTypeId=@DuplicateTypeId,Description=@Description,CabinetID=@CabinetID,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where LinkedItemId=@LinkedItemId"
        '    objParam = New SqlParameter(6) {}
        '    param = New SqlParameter("@TemplateName", objToUpdate.TemplateName)
        '    objParam(0) = param
        '    param = New SqlParameter("@CabinetID", objToUpdate.CabinetID)
        '    objParam(1) = param
        '    param = New SqlParameter("@Description", objToUpdate.Description)
        '    objParam(2) = param
        '    param = New SqlParameter("@LinkedItemId", objToUpdate.LinkedItemId)
        '    objParam(3) = param
        '    param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        '    objParam(4) = param
        '    param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        '    objParam(5) = param
        '    param = New SqlParameter("@DuplicateTypeId", objToUpdate.DuplicateTypeId)
        '    objParam(6) = param
        '    If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
        '        Throw New Exception("Record Not updated due to some error")

        '    End If
        'End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZDocumentLink, ByVal loginid As Integer)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        'strQry = "Update eZDocumentLink set Isdeleted=1 where LinkId=@LinkId"
        strQry = "update d set d.isdeleted=1 from eZDocumentLink as d join eztemplate as t on d.templateid=t.templateid join ezcabinet as c on t.cabinetid=c.cabinetid join eZCabOwners as CO on CO.cabinetid=c.cabinetid where d.linkid=@Linkid and(d.createdby=@Loginid or c.createdby=@Loginid or co.createdby=@Loginid or (SELECT ECMusertypeid FROM ezECMlogin where ecmloginid=@Loginid)=1)"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@LinkId", objToDelete.LinkId)
        objParam(0) = param
        param = New SqlParameter("@Loginid", loginid.ToString)
        objParam(1) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("You cannot remove the Linked Document")
        End If
    End Sub
    Public Function ReadSelectedeZDocumentLinkWithTempleteId(Criteria As String, Value As String, Criteria1 As String, Value1 As String) As System.Collections.Generic.List(Of IeZDocumentLink)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDocumentLink)()
        Dim objItem As IeZDocumentLink
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select LinkId From eZDocumentLink where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' and "
                strQry = strQry & "Convert(varchar(20)," & Criteria1 & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value1)
                strQry = strQry & "' "
                strQry = strQry & " order by itemid"
            Else
                strQry = "Select LinkId From eZDocumentLink where Isdeleted=0 order by itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZDocumentLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZDocumentLink(GetSmallInterger(sqlRdr("LinkId")))
                objItem.LinkId = GetSmallInterger(sqlRdr("LinkId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function readselecteddocumentlinklistwithtemplatelist(criteria1 As String, value1 As String, criteria2 As String, value2 As String, criteria3 As String, value3 As String) As List(Of IeZDocumentLink)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDocumentLink)()
        Dim objItem As IeZDocumentLink
        Try
            Dim strQry As String = ""

            strQry = "Select LinkId From eZDocumentLink where Isdeleted=0 and "
            strQry = strQry & "Convert(varchar(200)," & criteria2 & ") "
            strQry = strQry & " =N'"
            strQry = strQry & Unquote(value2)
            strQry = strQry & "' and "
            strQry = strQry & "Convert(varchar(200)," & criteria1 & ") "
            strQry = strQry & " =N'"
            strQry = strQry & Unquote(value1)
            strQry = strQry & "' and "
            strQry = strQry & "Convert(varchar(200)," & criteria3 & ") "
            strQry = strQry & " in ("
            strQry = strQry & Unquote(value3)
            strQry = strQry & ") "
            strQry = strQry & "   order by itemid"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZDocumentLink.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZDocumentLink(GetSmallInterger(sqlRdr("LinkId")))
                objItem.LinkId = GetSmallInterger(sqlRdr("LinkId"))
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

