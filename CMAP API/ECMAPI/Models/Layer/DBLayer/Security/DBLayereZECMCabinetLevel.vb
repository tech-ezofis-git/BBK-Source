Imports System.Data.SqlClient
Imports ECMAPI.DBLibrary
Partial Public Class DBLayer
#Region "User ECMCabinetLevels"
    Public Function CreateECMCabinetLevel(objEmp As eZECMCabinetLevel) As IeZECMCabinetLevel
        Dim newObject As IeZECMCabinetLevel = Nothing

        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel Where ECMLoginId = @ECMLoginId and TemplateId = @TemplateId " +
                "and CabinetId = @CabinetId And Isdeleted=0"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@CabinetId", objEmp.CabinetId)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(2) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ECMLoginId Code already exist!")
            End If
            strQry = "INSERT INTO eZECMCabinetLevel(ECMLoginId,CabinetId,TemplateId) VALUES(@ECMLoginId,@CabinetId,@TemplateId);" +
                "Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(2) {}
            param = New SqlParameter("@ECMLoginId", objEmp.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@CabinetId", objEmp.CabinetId)
            objParam(1) = param
            param = New SqlParameter("@TemplateId", objEmp.TemplateId)
            objParam(2) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZECMCabinetLevel(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZECMCabinetLevel)
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
                strQry = "Select ezcab.*,eztmp.encrypt,dbo.udf_Cabinet(ezcab.CabinetId) as Cabinet,dbo.udf_Template(ezcab.TemplateId) " +
                    "as Template,dbo.udf_LoginName(ezcab.ECMLoginId) as LoginName From eZECMCabinetLevel as ezcab left join eztemplate " +
                    "as eztmp on ezcab.templateid=eztmp.templateid Where ezcab.ECMCabinetLevelId=@ECMCabinetLevel_ID and ezcab.Isdeleted=0 and eztmp.isdeleted=0"
                param = New SqlParameter("@ECMCabinetLevel_ID", objRead.ECMCabinetLevelId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select ezcab.*,eztmp.encrypt,dbo.udf_Cabinet(ezcab.CabinetId) as Cabinet,dbo.udf_Template(ezcab.TemplateId) " +
                    "as Template,dbo.udf_LoginName(ECMLoginId) as LoginName From eZECMCabinetLevel as ezcab left join eztemplate " +
                    "as eztmp on ezcab.templateid=eztmp.templateid Where ezcab.ECMLoginId=@ECMLoginId and ezcab.Isdeleted=0 and eztmp.isdeleted=0"
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
                objRead.ECMCabinetLevelId = GetInteger(sqlRdr("ECMCabinetLevelId"))
                objRead.ECMLoginId = GetInteger(sqlRdr("ECMLoginId"))
                objRead.CabinetId = GetInteger(sqlRdr("CabinetId"))
                objRead.Cabinet = sqlRdr("Cabinet").ToString()
                objRead.LoginName = sqlRdr("LoginName").ToString()
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.Template = sqlRdr("Template").ToString()
                objRead.Encrypt = GetInteger(sqlRdr("Encrypt"))
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
    Public Function ReadAllECMCabinetLevel() As System.Collections.Generic.List(Of IeZECMCabinetLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMCabinetLevel)()
        Dim objItem As IeZECMCabinetLevel

        Try
            Dim strQry As String = ""
            strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel where Isdeleted=0 and Cabinetid not in(select cabinetid from ezcabinet WHERE (convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) or Isdeleted=1) and cabinetid<>1) order by ECMLoginId"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMLoginId.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMCabinetLevel(GetInteger(sqlRdr("ECMCabinetLevelId")))
                objItem.ECMCabinetLevelId = GetInteger(sqlRdr("ECMCabinetLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZECMCabinetLevel)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel Where ECMLoginId = @ECMLoginId and " +
            "ECMCabinetLevelId <> @ECMCabinetLevelId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
        objParam(0) = param
        param = New SqlParameter("@ECMCabinetLevelId", objToUpdate.ECMCabinetLevelId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ECMLoginId Code already exist!")
        Else
            strQry = "Update eZECMCabinetLevel Set ECMLoginId=@ECMLoginId,TemplateId=@TemplateId,CabinetId=@CabinetId " +
                "where ECMCabinetLevelId=@ECMCabinetLevel_ID"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@ECMLoginId", objToUpdate.ECMLoginId)
            objParam(0) = param
            param = New SqlParameter("@ECMCabinetLevel_ID", objToUpdate.ECMCabinetLevelId)
            objParam(1) = param
            param = New SqlParameter("@CabinetId", objToUpdate.CabinetId)
            objParam(2) = param
            param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
            objParam(3) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMCabinetLevel)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZECMCabinetLevel set Isdeleted=1 where ECMCabinetLevelId=@ECMCabinetLevel_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ECMCabinetLevel_ID", objToDelete.ECMCabinetLevelId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Function ReadFilteredeZECMCabinetLevel(Criteria As String, Value As String) As List(Of IeZECMCabinetLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMCabinetLevel)()
        Dim objItem As IeZECMCabinetLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel where Isdeleted=0 and Cabinetid not in(select cabinetid from " +
                    "ezcabinet WHERE (convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) or Isdeleted=1) and cabinetid<>1) and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel where Isdeleted=0 and Cabinetid not in(select cabinetid from ezcabinet " +
                    "WHERE (convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) or Isdeleted=1) and cabinetid<>1) order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMCabinetLevel(GetInteger(sqlRdr("ECMCabinetLevelId")))
                objItem.ECMCabinetLevelId = GetInteger(sqlRdr("ECMCabinetLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMCabinetLevel(Criteria As String, Value As String) As List(Of IeZECMCabinetLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMCabinetLevel)()
        Dim objItem As IeZECMCabinetLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel where Isdeleted=0 and Cabinetid not in(select cabinetid from ezcabinet " +
                    "WHERE (convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) or Isdeleted=1) and cabinetid<>1) and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel where Isdeleted=0 and Cabinetid not in(select cabinetid from ezcabinet " +
                    "WHERE (convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) or Isdeleted=1) and cabinetid<>1) order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMCabinetLevel(GetInteger(sqlRdr("ECMCabinetLevelId")))
                objItem.ECMCabinetLevelId = GetInteger(sqlRdr("ECMCabinetLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMCabinetLevelwithexpirydate(Criteria As String, Value As String) As List(Of IeZECMCabinetLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMCabinetLevel)()
        Dim objItem As IeZECMCabinetLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel where Isdeleted=0  and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel where Isdeleted=0  order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMCabinetLevel(GetInteger(sqlRdr("ECMCabinetLevelId")))
                objItem.ECMCabinetLevelId = GetInteger(sqlRdr("ECMCabinetLevelId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMCabinetLevelWithProfileId(Criteria As String, Value As String, ProfileId As String) As List(Of IeZECMCabinetLevel)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMCabinetLevel)()
        Dim objItem As IeZECMCabinetLevel

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel where Isdeleted=0 and Cabinetid not in(select cabinetid from ezcabinet " +
                    "WHERE (convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) or Isdeleted=1) and cabinetid<>1 ) and " +
                    "ECMLoginId=" + ProfileId + " and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMLoginId"
            Else
                strQry = "Select ECMCabinetLevelId From eZECMCabinetLevel where Isdeleted=0 and Cabinetid not in(select cabinetid from ezcabinet " +
                    "WHERE (convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) or Isdeleted=1) and cabinetid<>1) order by ECMLoginId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMCabinetLevel(GetInteger(sqlRdr("ECMCabinetLevelId")))
                objItem.ECMCabinetLevelId = GetInteger(sqlRdr("ECMCabinetLevelId"))
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
