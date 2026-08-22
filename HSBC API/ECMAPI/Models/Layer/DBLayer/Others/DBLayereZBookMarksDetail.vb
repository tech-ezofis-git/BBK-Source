Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "BookMarksDetail Details"


    Public Function CreateeZBookMarksDetail(objtemp As eZBookMarksDetail) As IeZBookMarksDetail
        Dim newObject As IeZBookMarksDetail = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select BookMarksDetailID From eZBookMarksDetail Where BookMarksId=@BookMarksId and ItemId = @ItemId and TemplateId = @TemplateId And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@ItemId", objtemp.ItemId)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@BookMarksId", objtemp.BookMarksId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZBookMarksDetail Code already exist!")
            End If
            strQry = "INSERT INTO eZBookMarksDetail(BookMarksId,TemplateId,ItemId,DisplayName,DirectLink,ifiletype,HitCount,Synopsis,Dates,Size,CreatedOn,CreatedBy) VALUES(@BookMarksId,@TemplateId,@ItemId,@DisplayName,@DirectLink,@ifiletype,@HitCount,@Synopsis,@Dates,@Size,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(11) {}
            param = New SqlParameter("@BookMarksId", objtemp.BookMarksId)
            objParam(0) = param
            param = New SqlParameter("@TemplateId", objtemp.TemplateId)
            objParam(1) = param
            param = New SqlParameter("@ItemId", objtemp.ItemId)
            objParam(2) = param
            param = New SqlParameter("@DisplayName", objtemp.DisplayName)
            objParam(3) = param
            param = New SqlParameter("@DirectLink", objtemp.DirectLink)
            objParam(4) = param
            param = New SqlParameter("@ifiletype", objtemp.ifiletype)
            objParam(5) = param
            param = New SqlParameter("@HitCount", objtemp.HitCount)
            objParam(6) = param
            param = New SqlParameter("@Synopsis", objtemp.Synopsis)
            objParam(7) = param
            param = New SqlParameter("@Dates", objtemp.Dates)
            objParam(8) = param
            param = New SqlParameter("@Size", objtemp.Size)
            objParam(9) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(10) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(11) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZBookMarksDetail(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZBookMarksDetail)
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
            If objRead.BookMarksId = 0 Then
                ' If you want add this dbo.udf_BookMarks(BookMarksId) as BookMarksName,
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZBookMarksDetail where Isdeleted=0 and  BookMarksDetailid=@BookMarksDetailid"
                param = New SqlParameter("@BookMarksDetailID", objRead.BookMarksDetailid)
                objParam(0) = param
            Else
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZBookMarksDetail where Isdeleted=0 and  BookMarksID=@BookMarksID"
                param = New SqlParameter("@BookMarksId", objRead.BookMarksId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarksDetail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.BookMarksDetailid = GetInteger(sqlRdr("BookMarksDetailID"))
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.ItemId = GetInteger(sqlRdr("ItemId"))
                objRead.BookMarksId = GetInteger(sqlRdr("BookMarksId"))
                objRead.DirectLink = sqlRdr("DirectLink").ToString()
                objRead.DisplayName = sqlRdr("DisplayName").ToString()
                objRead.Synopsis = sqlRdr("Synopsis").ToString()
                objRead.HitCount = sqlRdr("HitCount").ToString()
                objRead.ifiletype = sqlRdr("ifiletype").ToString()
                objRead.Dates = sqlRdr("Dates").ToString()
                objRead.Size = sqlRdr("Size").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZBookMarksDetail.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZBookMarksDetail() As System.Collections.Generic.List(Of IeZBookMarksDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBookMarksDetail)()
        Dim objItem As IeZBookMarksDetail
        Try
            Dim strQry As String = ""
            strQry = "Select BookMarksDetailID From eZBookMarksDetail where Isdeleted=0 order by ItemId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarksDetail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBookMarksDetail(GetSmallInterger(sqlRdr("BookMarksDetailID")))
                objItem.BookMarksDetailID = GetSmallInterger(sqlRdr("BookMarksDetailID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZBookMarksDetail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZBookMarksDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBookMarksDetail)()
        Dim objItem As IeZBookMarksDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select BookMarksDetailID From eZBookMarksDetail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ItemId"
            Else
                strQry = "Select BookMarksDetailID From eZBookMarksDetail where Isdeleted=0  order by ItemId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarksDetail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBookMarksDetail(GetSmallInterger(sqlRdr("BookMarksDetailID")))
                objItem.BookMarksDetailID = GetSmallInterger(sqlRdr("BookMarksDetailID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZBookMarksDetail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZBookMarksDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZBookMarksDetail)()
        Dim objItem As IeZBookMarksDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select BookMarksDetailID From eZBookMarksDetail where Isdeleted=0 and  "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ItemId"
            Else
                strQry = "Select BookMarksDetailID From eZBookMarksDetail where Isdeleted=0 order by ItemId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZBookMarksDetail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZBookMarksDetail(GetSmallInterger(sqlRdr("BookMarksDetailID")))
                objItem.BookMarksDetailID = GetSmallInterger(sqlRdr("BookMarksDetailID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZBookMarksDetail)
        'If Not objToUpdate.IsModified Then
        '    Return
        'End If
        'If Not objToUpdate.IsReadFromDB Then
        '    Return
        'End If
        'Dim strQry As String = ""
        'Dim objParam As SqlParameter()
        'Dim param As SqlParameter
        'strQry = "Select BookMarksDetailID From eZBookMarksDetail Where ItemId = @ItemId and BookMarksDetailID <> @BookMarksDetailID and Isdeleted=0"
        'objParam = New SqlParameter(1) {}
        'param = New SqlParameter("@ItemId", objToUpdate.ItemId)
        'objParam(0) = param
        'param = New SqlParameter("@BookMarksDetailID", objToUpdate.BookMarksDetailID)
        'objParam(1) = param
        'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        'If obj IsNot Nothing Then
        '    Throw New Exception("eZBookMarksDetail Code already exist!")
        'Else
        '    strQry = "Update eZBookMarksDetail Set ItemId=@ItemId,ERSId=@ERSId,Description=@Description,CabSize=@CabSize,CabIcon=@CabIcon,CabExpiryDate=@CabExpiryDate,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where BookMarksDetailID=@BookMarksDetailID"
        '    objParam = New SqlParameter(8) {}
        '    param = New SqlParameter("@ItemId", objToUpdate.ItemId)
        '    objParam(0) = param
        '    param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        '    objParam(1) = param
        '    param = New SqlParameter("@SearchKeyWord", objToUpdate.SearchKeyWord)
        '    objParam(2) = param
        '    param = New SqlParameter("@BookMarksDetailID", objToUpdate.BookMarksDetailID)
        '    objParam(6) = param
        '    param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
        '    objParam(7) = param
        '    param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
        '    objParam(8) = param
        '    If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
        '        Throw New Exception("Record Not updated due to some error")
        '    End If
        'End If
        'objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZBookMarksDetail)
        'If objToDelete Is Nothing Then
        '    Return
        'End If
        'Dim strQry As String = ""
        'Dim objParam As SqlParameter()
        'Dim param As SqlParameter
        'strQry = "Update eZBookMarksDetail set Isdeleted=1 where BookMarksDetailID=@BookMarksDetailID"
        'objParam = New SqlParameter(0) {}
        'param = New SqlParameter("@BookMarksDetailID", objToDelete.BookMarksDetailID)
        'objParam(0) = param
        'If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
        '    Throw New Exception("Record Not deleted due to some error")
        'End If
    End Sub


#End Region
End Class

