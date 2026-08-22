Imports System.Text
Imports System.Data
Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common



Partial Public Class DBLayer

    Public Function CreateeZtest(ByVal objemp As eZtest) As ieZtest
        Dim newobject As ieZtest = Nothing
        Try
            Dim strqry As String = ""
            Dim objparam As SqlParameter()
            Dim param As SqlParameter
            strqry = "INSERT INTO eZOutlooksync(Scheduleid,Syncname,Syncrule,SyncMail,Createdon,Createdby) values(@Scheduleid,@Syncname,@Syncrule,@SyncMail,@Createdon,@Createdby)"
            objparam = New SqlParameter(5) {}
            param = New SqlParameter("@Scheduleid", objemp.Scheduleid)
            objparam(0) = param
            param = New SqlParameter("@Syncname", objemp.Syncname)
            objparam(1) = param
            param = New SqlParameter("@Syncrule", objemp.Syncrule)
            objparam(2) = param
            param = New SqlParameter("@SyncMail", objemp.SyncMail)
            objparam(3) = param
            param = New SqlParameter("@Createdon", objemp.Createdon)
            objparam(4) = param
            param = New SqlParameter("@Createdby", objemp.Createdby)
            objparam(5) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strqry.ToString(), objparam)
            If obj Is Nothing Then
                Return Nothing
            End If
            newobject = GlobalInstance.eZtest(Convert.ToInt32(obj))
            Read(newobject)
            Return newobject
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
            Return Nothing
        End Try

    End Function

    Public Sub read(ByVal objread As ieZtest)
        If objread.IsReadFromDB Then
            Return
        End If
        If objread.IsModified Then
            Throw New InvalidOperationException
        End If
        Dim sqlrdr As SqlDataReader = Nothing
        objread.IsReadFromDB = True

        Try
            Dim strqry As String = ""
            Dim objparam As SqlParameter()
            Dim param As SqlParameter

            strqry = "SELECT * FROM ezoutlooksync WHERE outlooksyncid=@outlooksyncid and isdeleted=0"
            objparam = New SqlParameter(0) {}
            param = New SqlParameter("@Outlooksyncid", objread.Outlooksyncid)
            objparam(0) = param

            Dim obj As Object = ""

            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strqry.ToString(), objparam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read invalid outlooksync")
            End If
            sqlrdr = DirectCast(obj, SqlDataReader)
            If sqlrdr.Read() Then
                objread.Outlooksyncid = GetInteger(sqlrdr("Outlooksyncid").ToString())
                objread.Scheduleid = GetInteger(sqlrdr("Scheduleid").ToString())
                objread.SyncMail = sqlrdr("SyncMail").ToString()
                objread.Syncname = sqlrdr("Syncname").ToString()
                objread.Syncrule = sqlrdr("Syncrule").ToString()
                objread.Createdon = sqlrdr("Createdon").ToString()
                objread.updatedon = sqlrdr("updatedon").ToString()
                objread.Createdby = GetInteger(sqlrdr("Createdby").ToString())
                objread.updatedby = GetInteger(sqlrdr("updatedby").ToString())
            Else
                Return
            End If


        Finally
            If sqlrdr IsNot Nothing Then
                sqlrdr.Close()
            End If
            objread.IsModified = False
        End Try
    End Sub

End Class
