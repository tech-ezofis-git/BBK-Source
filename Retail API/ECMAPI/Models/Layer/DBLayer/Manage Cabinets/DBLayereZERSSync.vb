
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common


Partial Public Class DBLayer


    Public Function CreateeZERSsync(objtemp As eZERSSync) As IeZERSSync
        Dim newObject As IeZERSSync = Nothing
        If String.IsNullOrEmpty(objtemp.eZERSSyncname) Then
            Return Nothing
        End If

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            Dim obj As Object

            'strQry = "Select eZERSSyncid From eZERSSync Where FromERS = @FromERS And ToERS = @ToERS And Isdeleted=0"
            'objParam = New SqlParameter(1) {}
            'param = New SqlParameter("@FromERS", objtemp.FromERS)
            'objParam(0) = param
            'param = New SqlParameter("@ToERS", objtemp.ToERS)
            'objParam(1) = param
            'Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            'If obj IsNot Nothing Then
            '    Throw New Exception("Code already exist!")
            'End If
            strQry = "INSERT INTO eZERSSync(eZERSSyncname,FromERS,ToERS,Status,CreatedOn,CreatedBy) VALUES(@eZERSSyncname,@FromERS,@ToERS,@Status,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(5) {}
            param = New SqlParameter("@eZERSSyncname", objtemp.eZERSSyncname)
            objParam(0) = param
            param = New SqlParameter("@FromERS", objtemp.FromERS)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.Createdon)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.Createdby)
            objParam(3) = param
            param = New SqlParameter("@ToERS", objtemp.ToERS)
            objParam(4) = param
            param = New SqlParameter("@Status", objtemp.Status)
            objParam(5) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZERSSync(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function


    Public Sub Read(objRead As IeZERSSync)
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
          
            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZERSSync Where Isdeleted=0 and eZERSSyncid=@eZERSSyncid"
            param = New SqlParameter("@eZERSSyncid", objRead.eZERSSyncid)
                objParam(0) = param

            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSSync.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.eZERSSyncid = GetInteger(sqlRdr("eZERSSyncid"))
                objRead.eZERSSyncname = sqlRdr("eZERSSyncname").ToString()
                objRead.FromERS = sqlRdr("FromERS").ToString()
                objRead.ToERS = sqlRdr("ToERS").ToString()
                objRead.Status = sqlRdr("Status").ToString()
                objRead.Createdon = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.updatedby1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZERSInfo.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub

    Public Function ReadAlleZERSSync() As System.Collections.Generic.List(Of IeZERSSync)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSSync)()
        Dim objItem As IeZERSSync
        Try
            Dim strQry As String = ""
            strQry = "Select eZERSSyncid From eZERSSync where Isdeleted=0 order by eZERSSyncid"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSSync.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZERSSync(GetSmallInterger(sqlRdr("eZERSSyncid")))
                objItem.eZERSSyncid = GetSmallInterger(sqlRdr("eZERSSyncid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZERSSync(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZERSSync)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSSync)()
        Dim objItem As IeZERSSync
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select eZERSSyncid From eZERSSync where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by eZERSSyncid"
            Else
                strQry = "Select eZERSSyncid From eZERSSync where Isdeleted=0 order by eZERSSyncid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSSync.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZERSSync(GetSmallInterger(sqlRdr("eZERSSyncid")))
                objItem.eZERSSyncid = GetSmallInterger(sqlRdr("eZERSSyncid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZERSSync(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZERSSync)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZERSSync)()
        Dim objItem As IeZERSSync
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select eZERSSyncid From eZERSSync where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by eZERSSyncid"
            Else
                strQry = "Select eZERSSyncid From eZERSSync where Isdeleted=0 order by eZERSSyncid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZERSSync.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZERSSync(GetSmallInterger(sqlRdr("eZERSSyncid")))
                objItem.eZERSSyncid = GetSmallInterger(sqlRdr("eZERSSyncid"))
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
