Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer
    Public Function CreateDocumentAlert(objEmp As eZDocumentAlert) As IeZDocumentAlert
        Dim newObject As IeZDocumentAlert = Nothing
        If objEmp.itemId = 0 Then
            Return Nothing
        End If
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select DocumentAlertId From eZDocumentAlert Where TemplateId = @TemplateId And itemid = @itemid And CreatedBy = @CreatedBy And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@itemid", objEmp.itemId)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("itemid Code already exist!")
            End If
            strQry = "INSERT INTO eZDocumentAlert(itemid,TemplateId,ToMail,CreatedOn,CreatedBy) VALUES(@itemid,@TemplateId,@ToMail,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@itemid", objEmp.itemId)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(2) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(3) = param
            param = New SqlParameter("@ToMail", objEmp.ToMail)
            objParam(4) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZDocumentAlert(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZDocumentAlert)
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
            If objRead.itemid = 0 Then

                strQry = "Select *,dbo.udf_AlertCondition(al.AlertConditionId) as BYCondition,dbo.udf_TableName(da.TemplateId) as TableName,dbo.udf_UserName(da.UpdatedBy) as UpdatedBy1,dbo.udf_UserName(da.CreatedBy) as CreatedBy1 From eZDocumentAlert da left outer join eZAlert Al on da.DocumentAlertId=al.DocumentAlertId and da.CreatedBy=al.CreatedBy  Where da.DocumentAlertId=@DocumentAlert_ID and da.Isdeleted=0 and al.Isdeleted=0"
                param = New SqlParameter("@DocumentAlert_ID", objRead.DocumentAlertId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_TableName(TemplateId) as TableName,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZDocumentAlert Where itemid=@itemid and Isdeleted=0"
                param = New SqlParameter("@itemid", objRead.itemid)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid itemid.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.DocumentAlertId = GetInteger(sqlRdr("DocumentAlertId"))
                objRead.itemid = GetInteger(sqlRdr("itemid"))
                objRead.TableName = sqlRdr("TableName").ToString()
                objRead.ToMail = sqlRdr("ToMail").ToString()
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("BYCondition").ToString()
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
    Public Function ReadAllDocumentAlert() As System.Collections.Generic.List(Of IeZDocumentAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDocumentAlert)()
        Dim objItem As IeZDocumentAlert

        Try
            Dim strQry As String = ""
            strQry = "Select DocumentAlertId From eZDocumentAlert where Isdeleted=0 order by itemid"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid itemid.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZDocumentAlert(GetInteger(sqlRdr("DocumentAlertId")))
                objItem.DocumentAlertId = GetInteger(sqlRdr("DocumentAlertId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Function ReadSelectedDocumentAlert(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZDocumentAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDocumentAlert)()
        Dim objItem As IeZDocumentAlert
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select DocumentAlertId From eZDocumentAlert where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by itemid"
            Else
                strQry = "Select DocumentAlertId From eZDocumentAlert where Isdeleted=0 order by itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZDocumentAlert.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZDocumentAlert(GetSmallInterger(sqlRdr("DocumentAlertId")))
                objItem.DocumentAlertId = GetSmallInterger(sqlRdr("DocumentAlertId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedDocumentAlertByCreatedby(ByVal Value As String) As System.Collections.Generic.List(Of IeZDocumentAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim sqlRdr1 As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDocumentAlert)()

        Try
            Dim strQry As String = ""
            
            strQry = "Select * From eZDocumentAlert where Isdeleted=0 and Convert(varchar(20),CreatedBy)  =N'" + Value + "'  order by itemid"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZDocumentAlert.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                Dim documeid As String = sqlRdr("DocumentAlertId")
                strQry = "Select *,dbo.udf_AlertCondition(al.AlertConditionId) as BYCondition,dbo.udf_TableName(da.TemplateId) as TableName,dbo.udf_UserName(da.UpdatedBy) as UpdatedBy1,dbo.udf_UserName(da.CreatedBy) as CreatedBy1 From eZDocumentAlert da left outer join eZAlert Al on da.DocumentAlertId=al.DocumentAlertId and da.CreatedBy=al.CreatedBy  Where da.DocumentAlertId=" + documeid + " and da.Isdeleted=0 and al.Isdeleted=0"
                obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
                If obj Is Nothing Then
                    Throw New Exception("Attempt to read Invalid eZDocumentAlert.")
                End If
                sqlRdr1 = DirectCast(obj, SqlDataReader)
                While sqlRdr1.Read()
                    Dim objItem As New eZDocumentAlert
                    objItem.DocumentAlertId = GetInteger(sqlRdr1("DocumentAlertId"))
                    objItem.itemId = GetInteger(sqlRdr1("itemid"))
                    objItem.UpdatedBy1 = sqlRdr1("BYCondition").ToString()
                    objItem.TableName = sqlRdr1("TableName").ToString()
                    objItem.ToMail = sqlRdr1("ToMail").ToString()
                    objItem.TemplateId = GetInteger(sqlRdr1("TemplateId"))
                    objItem.CreatedOn = sqlRdr1("CreatedOn").ToString
                    objItem.CreatedBy1 = sqlRdr1("CreatedBy1").ToString()
                    objItem.CreatedBy = sqlRdr1("CreatedBy").ToString()
                    objItem.UpdatedOn = sqlRdr1("UpdatedOn").ToString()
                    objItem.UpdatedBy = sqlRdr1("UpdatedBy").ToString()
                    lstItems.Add(objItem)
                End While
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    'udaya
    Public Function ReadSelectedDocumentAlertWithidTemplateidItemid(Loginid As String, Templateid As String, Itemid As String) As DataSet

        Dim ds As DataSet = Nothing
        Try
            Dim strqry As String = ""
            strqry = "SELECT DA.DocumentAlertId,DA.TemplateId,DA.itemid,A.AlertId,A.AlertConditionId,DA.ToMail,DA.CreatedOn,DA.UpdatedOn,DA.CreatedBy,DA.UpdatedBy,DA.Isdeleted FROM eZDocumentAlert as DA join eZAlert as A on DA.DocumentAlertId=A.DocumentAlertId WHERE A.Isdeleted=0 and DA.CreatedBy=N'" + Loginid + "' and  DA.TemplateId=N'" + Templateid + "' and DA.itemid=N'" + Itemid + "'"
            ds = DBLayer.DBLInstance.GetDatasetByQuery(strqry)
            Return ds
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ReadSelectedDocumentAlertWithid(Criteria As String, Value As String, Loginid As String) As System.Collections.Generic.List(Of IeZDocumentAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDocumentAlert)()
        Dim objItem As IeZDocumentAlert
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select DocumentAlertId From eZDocumentAlert where Isdeleted=0 and CreatedBy=N'" + Loginid + "' and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by itemid"
            Else
                strQry = "Select DocumentAlertId From eZDocumentAlert where Isdeleted=0 order by itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZDocumentAlert.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZDocumentAlert(GetSmallInterger(sqlRdr("DocumentAlertId")))
                objItem.DocumentAlertId = GetSmallInterger(sqlRdr("DocumentAlertId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedDocumentAlertWithTemplateid(Criteria As String, Value As String, TemplateId As String) As System.Collections.Generic.List(Of IeZDocumentAlert)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZDocumentAlert)()
        Dim objItem As IeZDocumentAlert
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select DocumentAlertId From eZDocumentAlert where Isdeleted=0 and TemplateId=N'" + TemplateId + "' and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by itemid"
            Else
                strQry = "Select DocumentAlertId From eZDocumentAlert where Isdeleted=0 order by itemid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZDocumentAlert.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZDocumentAlert(GetSmallInterger(sqlRdr("DocumentAlertId")))
                objItem.DocumentAlertId = GetSmallInterger(sqlRdr("DocumentAlertId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZDocumentAlert)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select DocumentAlertId From eZDocumentAlert Where TemplateId = @TemplateId And itemid = @itemid and DocumentAlertId <> @DocumentAlertId and Isdeleted=0"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@itemid", objToUpdate.itemid)
        objParam(0) = param
        param = New SqlParameter("@DocumentAlertId", objToUpdate.DocumentAlertId)
        objParam(1) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(2) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("itemid Code already exist!")
        Else
            strQry = "Update eZDocumentAlert Set ToMail=@ToMail,itemid=@itemid,TemplateId=@TemplateId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where DocumentAlertId=@DocumentAlertId"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@itemid", objToUpdate.itemid)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(2) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(3) = param
            param = New SqlParameter("@DocumentAlertId", objToUpdate.DocumentAlertId)
            objParam(4) = param
            param = New SqlParameter("@ToMail", objToUpdate.ToMail)
            objParam(5) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZDocumentAlert)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZDocumentAlert set Isdeleted=1 where DocumentAlertId=@DocumentAlert_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@DocumentAlert_ID", objToDelete.DocumentAlertId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class