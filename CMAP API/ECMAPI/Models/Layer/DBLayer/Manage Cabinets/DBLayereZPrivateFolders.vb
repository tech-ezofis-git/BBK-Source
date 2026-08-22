
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer


    Public Function CreateeZPrivateFolders(objtemp As eZPrivateFolders) As IeZPrivateFolders
        Dim newObject As IeZPrivateFolders = Nothing
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select Privatefolderid From eZPrivateFolders Where Nodeid = @Nodeid and userid=@userid And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@Nodeid", objtemp.Nodeid)
            objParam(0) = param
            param = New SqlParameter("@userid", objtemp.userid)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZPrivateFolders Code already exist!")
            End If
            strQry = "INSERT INTO eZPrivateFolders(Nodeid,userid,CreatedOn,CreatedBy) VALUES(@Nodeid,@userid,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@Nodeid", objtemp.Nodeid)
            objParam(0) = param
            param = New SqlParameter("@userid", objtemp.userid)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param

            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newObject = GlobalInstance.eZPrivateFolders(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub read(objread As IeZPrivateFolders)
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

            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZPrivateFolders Where  isdeleted=0 and Privatefolderid=@Privatefolderid"
            param = New SqlParameter("@Privatefolderid", objread.Privatefolderid)
            objParam(0) = param

            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZPrivateFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objread.Privatefolderid = GetInteger(sqlRdr("Privatefolderid"))
                objread.Nodeid = GetInteger(sqlRdr("Nodeid"))
                objread.userid = GetInteger(sqlRdr("userid"))
                objread.Createdon = sqlRdr("CreatedOn").ToString
                objread.Createdby1 = sqlRdr("CreatedBy1").ToString()
                objread.Createdby = sqlRdr("CreatedBy").ToString()
                objread.Updatedon = sqlRdr("UpdatedOn").ToString()
                objread.Updatedby1 = sqlRdr("UpdatedBy1").ToString()
                objread.Updatedby = sqlRdr("UpdatedBy").ToString()
            Else
                'throw new Exception("Attempt to read Invalid eZFolders.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objread.IsModified = False
        End Try
    End Sub

    Public Function ReadAlleZPrivatefolders() As System.Collections.Generic.List(Of IeZPrivateFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZPrivateFolders)()
        Dim objItem As IeZPrivateFolders
        Try
            Dim strQry As String = ""
            strQry = "Select Privatefolderid From eZPrivateFolders where Isdeleted=0 order by Privatefolderid"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZPrivateFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZPrivateFolders(GetSmallInterger(sqlRdr("Privatefolderid")))
                objItem.Privatefolderid = GetSmallInterger(sqlRdr("Privatefolderid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZprivatefolders(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZPrivateFolders)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZPrivateFolders)()
        Dim objItem As IeZPrivateFolders
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select Privatefolderid From eZPrivateFolders where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(100)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                'strQry = strQry & " order by FieldLevel"
                strQry = strQry & "ORDER BY Privatefolderid"
            Else
                strQry = "Select Privatefolderid From eZPrivateFolders where Isdeleted=0 ORDER BY Privatefolderid"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZPrivateFolders.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZPrivateFolders(GetSmallInterger(sqlRdr("Privatefolderid")))
                objItem.Privatefolderid = GetSmallInterger(sqlRdr("Privatefolderid"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZPrivateFolders)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select Privatefolderid From eZPrivateFolders Where Nodeid = @Nodeid and userid=@userid and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@Nodeid", objToUpdate.Nodeid)
        objParam(0) = param
        param = New SqlParameter("@userid", objToUpdate.userid)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("Private folders Code already exist!")
        Else
            strQry = "Update eZPrivateFolders Set Nodeid=@Nodeid,userid=@userid,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where Privatefolderid=@Privatefolderid"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@Nodeid", objToUpdate.Nodeid)
            objParam(0) = param
            param = New SqlParameter("@userid", objToUpdate.userid)
            objParam(1) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.Updatedon)
            objParam(2) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(3) = param
            param = New SqlParameter("@Privatefolderid", objToUpdate.Privatefolderid)
            objParam(4) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error1")

            End If
        End If
        objToUpdate.IsModified = False
    End Sub


    Public Sub Delete(objToDelete As IeZPrivateFolders)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZPrivateFolders set Isdeleted=1 where Privatefolderid=@Privatefolderid"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@Privatefolderid", objToDelete.Privatefolderid)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub

End Class
