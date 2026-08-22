Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "User ECMDocumentLevels"
    Public Function CreateECMDocumentLevel(objEmp As eZECMDocumentLevel) As IeZECMDocumentLevel
        Dim newObject As IeZECMDocumentLevel = Nothing

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ECMDocumentLevelId From eZECMDocumentLevel Where TemplateId=@TemplateId and ECMLoginId = @ECMLoginId and itemid = @itemid And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@itemid", objEmp.itemid)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ECMLoginId Code already exist!")
            End If
            strQry = "INSERT INTO eZECMDocumentLevel(ECMLoginId,itemid,TemplateId) VALUES(@ECMLoginId,@itemid,@TemplateId);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@itemid", objEmp.itemid)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(2) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZECMDocumentLevel(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZECMDocumentLevel)
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
            If objRead.ECMLoginId = 0 Then
                strQry = "Select *,dbo.udf_LoginName(ECMLoginId) as LoginName From eZECMDocumentLevel Where ECMDocumentLevelId=@ECMDocumentLevel_ID and Isdeleted=0"
                param = New SqlParameter("@ECMDocumentLevel_ID", objRead.ECMDocumentLevelId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_LoginName(ECMLoginId) as LoginName From eZECMDocumentLevel Where ECMLoginId=@ECMLoginId and Isdeleted=0"
                param = New SqlParameter("@ECMLoginId", objRead.ECMLoginId)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMLoginId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ECMDocumentLevelId = GetInteger(sqlRdr("ECMDocumentLevelId"))
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                objRead.itemid = GetInteger(sqlRdr("itemid"))
                objRead.LoginName = sqlRdr("LoginName").ToString()
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
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
    Public Function ReadAllECMDocumentLevel() As System.Collections.Generic.List(Of IeZECMDocumentLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMDocumentLevel)()
        Dim objItem As IeZECMDocumentLevel

        Try
            Dim strQry As String = ""
            strQry = "Select ECMDocumentLevelId From eZECMDocumentLevel where Isdeleted=0 order by ECMLoginId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMLoginId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMDocumentLevel(GetInteger(sqlRdr("ECMDocumentLevelId")))
                objItem.ECMDocumentLevelId = GetInteger(sqlRdr("ECMDocumentLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZECMDocumentLevel)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ECMDocumentLevelId From eZECMDocumentLevel Where ECMLoginId = @ECMLoginId and ECMDocumentLevelId <> @ECMDocumentLevelId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        objParam(0) = param
        param = New SqlParameter("@ECMDocumentLevelId", objToUpdate.ECMDocumentLevelId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ECMLoginId Code already exist!")
        Else
            strQry = "Update eZECMDocumentLevel Set ECMLoginId=@ECMLoginId,itemid=@itemid where ECMDocumentLevelId=@ECMDocumentLevel_ID"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@ECMDocumentLevel_ID", objToUpdate.ECMDocumentLevelId)
            objParam(1) = param
            param = New SqlParameter("@itemid", objToUpdate.itemid)
            objParam(2) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMDocumentLevel)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMDocumentLevel set Isdeleted=1 where ECMDocumentLevelId=@ECMDocumentLevel_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ECMDocumentLevel_ID", objToDelete.ECMDocumentLevelId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Function ReadFilteredeZECMDocumentLevel(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMDocumentLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMDocumentLevel)()
        Dim objItem As IeZECMDocumentLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMDocumentLevelId From eZECMDocumentLevel where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select ECMDocumentLevelId From eZECMDocumentLevel where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMDocumentLevel(GetInteger(sqlRdr("ECMDocumentLevelId")))
                objItem.ECMDocumentLevelId = GetInteger(sqlRdr("ECMDocumentLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZECMDocumentLevel(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMDocumentLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMDocumentLevel)()
        Dim objItem As IeZECMDocumentLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMDocumentLevelId From eZECMDocumentLevel where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select ECMDocumentLevelId From eZECMDocumentLevel where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMDocumentLevel(GetInteger(sqlRdr("ECMDocumentLevelId")))
                objItem.ECMDocumentLevelId = GetInteger(sqlRdr("ECMDocumentLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Function ReadSelectedeZECMDocumentLevelWithProfileId(Criteria As String, Value As String, ProfileId As String) As System.Collections.Generic.List(Of IeZECMDocumentLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMDocumentLevel)()
        Dim objItem As IeZECMDocumentLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMDocumentLevelId From eZECMDocumentLevel where Isdeleted=0 and ECMLoginId=" + ProfileId + " and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select ECMDocumentLevelId From eZECMDocumentLevel where Isdeleted=0 order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMDocumentLevel(GetInteger(sqlRdr("ECMDocumentLevelId")))
                objItem.ECMDocumentLevelId = GetInteger(sqlRdr("ECMDocumentLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMDocumentLevelById(ByVal itemid As String, ByVal templateid As String, loginid As String) As System.Collections.Generic.List(Of IeZECMDocumentLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMDocumentLevel)()
        Dim objItem As IeZECMDocumentLevel

        Try
            Dim strQry As String = ""
            strQry = "Select ECMDocumentLevelId From eZECMDocumentLevel where Isdeleted=0 and ECMLoginId=" + loginid + " and TemplateId=" + templateid + " and itemid=" + itemid
          
            strQry = strQry & " order by ECMLoginId"

            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMDocumentLevel(GetInteger(sqlRdr("ECMDocumentLevelId")))
                objItem.ECMDocumentLevelId = GetInteger(sqlRdr("ECMDocumentLevelId"))
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
