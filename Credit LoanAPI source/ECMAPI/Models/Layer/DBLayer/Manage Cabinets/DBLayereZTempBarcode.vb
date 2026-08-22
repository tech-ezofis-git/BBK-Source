Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "Pdf Properties Details"


    Public Function CreateeZTempBarcode(objtemp As eZTempBarcode) As IeZTempBarcode
        Dim newObject As IeZTempBarcode = Nothing
        If objtemp.TemplateId = 0 Then
            Return Nothing
        End If
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select BarcodeId From eZTempBarcode Where TemplateId = @TemplateId And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("TemplateId already exist!")
            End If
            strQry = "INSERT INTO eZTempBarcode(StartsWith,prefix,EndWith,TemplateId,suffix,BarcodeTypeId,BarcodeField,Length,CreatedOn,CreatedBy) VALUES(@StartsWith,@prefix,@EndWith,@TemplateId,@suffix,@BarcodeTypeId,@BarcodeField,@Length,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(9) {}
            param = New SqlParameter("@StartsWith", objtemp.StartsWith)
            objParam(0) = param
            param = New SqlParameter("@prefix", objtemp.prefix)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(4) = param
            param = New SqlParameter("@suffix", objtemp.suffix)
            objParam(5) = param
            param = New SqlParameter("@EndWith", objtemp.EndWith)
            objParam(6) = param
            param = New SqlParameter("@BarcodeTypeId", objtemp.BarcodeTypeId)
            objParam(7) = param
            param = New SqlParameter("@BarcodeField", objtemp.BarcodeField)
            objParam(8) = param
            param = New SqlParameter("@Length", objtemp.Length)
            objParam(9) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZTempBarcode(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZTempBarcode)
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
            'If objRead.StartsWith Is Nothing Then
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_BarcodeType(BarcodeTypeId) as BarcodeType,dbo.udf_Template(TemplateId) as TemplateName,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZTempBarcode Where Isdeleted=0 and BarcodeId=@BarcodeId"
            param = New SqlParameter("@BarcodeId", objRead.BarcodeId)
            objParam(0) = param
            'Else
            '    strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_Template(TemplateId) as TemplateName,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZTempBarcode Where Isdeleted=0 and StartsWith=@StartsWith"
            '    param = New SqlParameter("@StartsWith", objRead.StartsWith)
            '    objParam(0) = param
            'End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTempBarcode.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.BarcodeId = GetInteger(sqlRdr("BarcodeId"))
                objRead.StartsWith = sqlRdr("StartsWith").ToString()
                objRead.EndWith = sqlRdr("EndWith").ToString()
                objRead.TemplateName = sqlRdr("TemplateName").ToString()
                objRead.BarcodeType = sqlRdr("BarcodeType").ToString()
                objRead.TemplateID = GetSmallInterger(sqlRdr("TemplateId"))
                objRead.BarcodeTypeId = GetInteger(sqlRdr("BarcodeTypeId"))
                objRead.prefix = sqlRdr("prefix").ToString()
                objRead.Length = sqlRdr("Length").ToString()
                objRead.suffix = sqlRdr("suffix").ToString()
                objRead.BarcodeField = sqlRdr("BarcodeField").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZTempBarcode.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZTempBarcode() As System.Collections.Generic.List(Of IeZTempBarcode)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTempBarcode)()
        Dim objItem As IeZTempBarcode
        Try
            Dim strQry As String = ""
            strQry = "Select BarcodeId From eZTempBarcode where Isdeleted=0 order by StartsWith"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTempBarcode.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTempBarcode(GetSmallInterger(sqlRdr("BarcodeId")))
                objItem.BarcodeId = GetSmallInterger(sqlRdr("BarcodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZTempBarcode(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTempBarcode)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTempBarcode)()
        Dim objItem As IeZTempBarcode
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select BarcodeId From eZTempBarcode where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by StartsWith"
            Else
                strQry = "Select BarcodeId From eZTempBarcode where Isdeleted=0 order by StartsWith"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTempBarcode.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTempBarcode(GetSmallInterger(sqlRdr("BarcodeId")))
                objItem.BarcodeId = GetSmallInterger(sqlRdr("BarcodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTempBarcode(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTempBarcode)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTempBarcode)()
        Dim objItem As IeZTempBarcode
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select BarcodeId From eZTempBarcode where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by StartsWith"
            Else
                strQry = "Select BarcodeId From eZTempBarcode where Isdeleted=0 order by StartsWith"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTempBarcode.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTempBarcode(GetSmallInterger(sqlRdr("BarcodeId")))
                objItem.BarcodeId = GetSmallInterger(sqlRdr("BarcodeId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZTempBarcode)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter

        strQry = "Update eZTempBarcode Set StartsWith=@StartsWith,Length=@Length,EndWith=@EndWith,BarcodeField=@BarcodeField,BarcodeTypeId=@BarcodeTypeId,suffix=@suffix,prefix=@prefix,TemplateId=@TemplateId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where BarcodeId=@BarcodeId"
        objParam = New SqlParameter(10) {}
        param = New SqlParameter("@StartsWith", objToUpdate.StartsWith)
        objParam(0) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateID)
        objParam(1) = param
        param = New SqlParameter("@prefix", objToUpdate.prefix)
        objParam(2) = param
        param = New SqlParameter("@BarcodeId", objToUpdate.BarcodeId)
        objParam(3) = param
        param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        objParam(4) = param
        param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        objParam(5) = param
        param = New SqlParameter("@suffix", objToUpdate.suffix)
        objParam(6) = param
        param = New SqlParameter("@EndWith", objToUpdate.EndWith)
        objParam(7) = param
        param = New SqlParameter("@BarcodeTypeId", objToUpdate.BarcodeTypeId)
        objParam(8) = param
        param = New SqlParameter("@BarcodeField", objToUpdate.BarcodeField)
        objParam(9) = param
        param = New SqlParameter("@Length", objToUpdate.Length)
        objParam(10) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not updated due to some error")

        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZTempBarcode)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZTempBarcode set Isdeleted=1 where BarcodeId=@BarcodeId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@BarcodeId", objToDelete.BarcodeId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub


#End Region

End Class

