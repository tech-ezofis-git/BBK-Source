Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "Template Details"

    Public Function CreateTbls(ByVal cabinetid As String, ByVal templateid As String) As Integer
        Try
            Dim Lst1 As New List(Of IeZTemplateField)()
            Dim objParam As SqlParameter()
            Dim columns As String = ""
            objParam = New SqlParameter(0) {}
            Dim Field As String = ""
            Lst1 = DBLayer.DBLInstance.ReadSelectedeZTemplateField("TemplateId", templateid)
            For i As Integer = 0 To Lst1.Count - 1
                Field = Field + "[" + Lst1(i).FieldName + "] " + Lst1(i).DT + ","
                columns = "Update([" + Lst1(i).FieldName + "]) or " + columns
            Next
            columns = columns.Substring(0, columns.LastIndexOf("or") - 1)
            Dim strQry As String = "create table eZCA_" + cabinetid + "_" + templateid + "_stage (itemid int IDENTITY(1,1) NOT NULL,ERSId int NOT NULL DEFAULT(0)," +
                "TemplateId int NOT NULL  DEFAULT(0)," + Field + "ifilepath nvarchar(512) null DEFAULT(''), ifilename nvarchar(200) null DEFAULT(''), ifiletype nvarchar(100) null DEFAULT('')," +
                " eZFrom nvarchar(100) null DEFAULT(''),version nvarchar(100) null DEFAULT(''),dtitle nvarchar(100) null DEFAULT(''), dauthor nvarchar(100) null DEFAULT(''),dsubject nvarchar(100) " +
                "null DEFAULT(''),dkeywords nvarchar(100) null DEFAULT(''), checkout nvarchar(50) null DEFAULT(''),checkoutpath nvarchar(100) null DEFAULT(''), checkoutby int NOT NULL DEFAULT(0), " +
                "dstatus nvarchar(50) null DEFAULT(''),dsize nvarchar(50) null DEFAULT(''), nopages int null DEFAULT(''),Password NVARCHAR(MAX) DEFAULT(''),Passwordby INT DEFAULT(0),PasswordRemoveby" +
                " INT DEFAULT(0),Encrypt NVARCHAR(MAX) DEFAULT(''),Encryptby INT DEFAULT(0),Decryptby INT DEFAULT(0),CreatedOn nvarchar(100) NULL DEFAULT(''), UpdatedOn nvarchar(100) NULL DEFAULT(''), " +
                "CreatedBy int NOT NULL DEFAULT(0),UpdatedBy int NOT NULL DEFAULT(0), Isdeleted bit NOT NULL DEFAULT(0)) "
            Dim strQry1 As String = "create table eZCA_" + cabinetid + "_" + templateid + "_items (itemid int IDENTITY(1,1) NOT NULL,ERSId int NOT NULL DEFAULT(0)," +
                "TemplateId int NOT NULL DEFAULT(0)," + Field + "ifilepath nvarchar(512) null DEFAULT(''), ifilename nvarchar(200) null DEFAULT(''), ifiletype nvarchar(100) null DEFAULT('')," +
                " eZFrom nvarchar(100) null DEFAULT(''),version nvarchar(100) null DEFAULT(''),dtitle nvarchar(100) null DEFAULT(''), dauthor nvarchar(100) null DEFAULT(''),dsubject nvarchar(100)" +
                " null DEFAULT(''),dkeywords nvarchar(100) null DEFAULT(''), checkout nvarchar(50) null DEFAULT(''),checkoutpath nvarchar(100) null DEFAULT(''), checkoutby int NOT NULL DEFAULT(0)," +
                " dstatus nvarchar(50) null DEFAULT(''),dsize nvarchar(50) null DEFAULT(''), nopages int null DEFAULT(''),Password NVARCHAR(MAX) DEFAULT(''),Passwordby INT DEFAULT(0),PasswordRemoveby " +
                "INT DEFAULT(0),Encrypt NVARCHAR(MAX) DEFAULT(''),Encryptby INT DEFAULT(0),Decryptby INT DEFAULT(0),CreatedOn nvarchar(100) NULL DEFAULT(''), UpdatedOn nvarchar(100) NULL DEFAULT(''), " +
                "CreatedBy int NOT NULL DEFAULT(0),UpdatedBy int NOT NULL DEFAULT(0), Isdeleted bit NOT NULL DEFAULT(0)) "
            Dim strQry2 As String = "create table eZCA_" + cabinetid + "_" + templateid + "_history (itemid int NULL  DEFAULT(0),ERSId int NOT NULL  DEFAULT(0)," +
                "TemplateId int NOT NULL DEFAULT(0)," + Field + "ifilepath nvarchar(512) null DEFAULT(''), ifilename nvarchar(200) null DEFAULT(''), ifiletype nvarchar(100) null DEFAULT('')," +
                " eZFrom nvarchar(100) null DEFAULT(''),version nvarchar(100) null DEFAULT(''),dtitle nvarchar(100) null DEFAULT(''), dauthor nvarchar(100) null DEFAULT(''),dsubject nvarchar(100) " +
                "null DEFAULT(''),dkeywords nvarchar(100) null DEFAULT(''), checkout nvarchar(50) null DEFAULT(''),checkoutpath nvarchar(100) null DEFAULT(''), checkoutby int NOT NULL  DEFAULT(0), " +
                "dstatus nvarchar(50) null  DEFAULT(''),dsize nvarchar(50) null DEFAULT(''), nopages int null DEFAULT('') ,Password NVARCHAR(MAX) DEFAULT(''),Passwordby INT DEFAULT(0),PasswordRemoveby " +
                "INT DEFAULT(0),Encrypt NVARCHAR(MAX) DEFAULT(''),Encryptby INT DEFAULT(0),Decryptby INT DEFAULT(0),CreatedOn nvarchar(100) NULL DEFAULT(''), UpdatedOn nvarchar(100) NULL DEFAULT(''), " +
                "CreatedBy int NOT NULL DEFAULT(0),UpdatedBy int NOT NULL DEFAULT(0), Isdeleted bit NOT NULL DEFAULT(0)) "
            Lst1 = DBLayer.DBLInstance.ReadSelectedeZTemplateFieldWithTemplateId("Mandatory", "1", templateid)
            Field = ""
            For i As Integer = 0 To Lst1.Count - 1
                Field = Field + "[" + Lst1(i).FieldName + "]" + ","
            Next
            Field = Field + ","
            Field = Field.Replace(",,", "")
            Dim strQry3 As String = "CREATE INDEX index_" + cabinetid + "_" + templateid + " ON eZCA_" + cabinetid + "_" + templateid + "_items (" + Field + ")"
            Dim strQry4 As String = "CREATE INDEX index_" + cabinetid + "_" + templateid + " ON eZCA_" + cabinetid + "_" + templateid + "_history (" + Field + ")"
            Dim strqry5 As String = "CREATE TRIGGER TRG_eZ_" + cabinetid + "_" + templateid + "_item ON [dbo].[eZCA_" + cabinetid + "_" + templateid + "_items] "
            strqry5 = strqry5 + "  AFTER INSERT,UPDATE AS BEGIN DECLARE @itemid integer DECLARE @ECMLoginId integer DECLARE @TemplateId integer DECLARE @CreatedOn nvarchar(1000) DECLARE @CreatedBy nvarchar(1000)" +
                " DECLARE @UpdatedOn nvarchar(1000) DECLARE @UpdatedBy nvarchar(1000) DECLARE @ezfrom nvarchar(max) DECLARE @Isdeleted integer DECLARE @ifilename nvarchar(max) SELECT @ifilename=ifilename from inserted "
            strqry5 = strqry5 + " SELECT @itemid=itemid FROM inserted SELECT @TemplateId=TemplateId FROM inserted SELECT @ezfrom=ezfrom FROM inserted "
            'Update by srini
            'strqry5 = strqry5 + " if (not UPDATE(EncryptBy) or (exists(select 1 from inserted where Encryptby=0 and Decryptby=0))) and @ifilename<>'' " +
            strqry5 = strqry5 + "  if ((UPDATE(ifilepath) or UPDATE(ifilename)) and (exists(select 1 from inserted where Encryptby=0)) and @ifilename<>'') " +
                "  begin insert  into [eZDtSearchPath] ([ERSId],[TemplateId],[iFilePath],[ifiletype],[itemid]) select ERSID,Templateid,ifilepath+ifilename as iFilePath,ifiletype,itemid from eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_items "
            strqry5 = strqry5 + " where itemid=@itemid end IF NOT EXISTS(SELECT 1 FROM DELETED) BEGIN SELECT @ECMLoginId=CreatedBy FROM inserted SELECT @CreatedOn=CreatedOn FROM inserted SELECT @CreatedBy=CreatedBy FROM inserted "
            strqry5 = strqry5 + " if @ezfrom is not null and  @ezfrom <> '' begin INSERT INTO dbo.eZUserSession(ECMLoginId,TemplateId,itemid,UplaodDocument,AlertDocument,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,Isdeleted,Loggedfrom,Loggedat)" +
                "  values(@ECMLoginId,@TemplateId,@itemid,'1',0,@CreatedOn,@CreatedBy,'','0','0',substring(@ezfrom,0,charindex('(',@ezfrom)),substring(@ezfrom,charindex('(',@ezfrom)+1,(charindex(')',@ezfrom))-(charindex('(',@ezfrom)+1))) END END  " +
                " if exists (SELECT 1 FROM DELETED)  BEGIN  SELECT @ECMLoginId=Updatedby FROM inserted  SELECT @CreatedOn=Updatedon FROM inserted  SELECT @CreatedBy=Updatedby FROM inserted  IF " + columns + "	 BEGIN 	" +
                " INSERT INTO dbo.eZUserSession(ECMLoginId,TemplateId,itemid,IndexingChange,AlertDocument,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,Isdeleted)  values(@ECMLoginId,@TemplateId,@itemid,'1',0,@CreatedOn,@CreatedBy,'','0','0') END" +
                " IF UPDATE(isdeleted) BEGIN select @IsDeleted=isdeleted from inserted  if @IsDeleted=1 begin INSERT INTO dbo.eZUserSession(ECMLoginId,TemplateId,itemid,[DELETED],AlertDocument,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,Isdeleted)" +
                " values(@ECMLoginId,@TemplateId,@itemid,'1',0,@CreatedOn,@CreatedBy,'','0','0') end else begin INSERT INTO dbo.eZUserSession(ECMLoginId,TemplateId,itemid,[DELETED],AlertDocument,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,Isdeleted) " +
                "values(@ECMLoginId,@TemplateId,@itemid,'2',0,@CreatedOn,@CreatedBy,'','0','0') end END END End"
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry)
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry1)
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry2)
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry3)
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry4)
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strqry5)
            Return 1
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Function CreateeZTemplate(objtemp As eZTemplate) As IeZTemplate
        Dim newObject As IeZTemplate = Nothing
        If String.IsNullOrEmpty(objtemp.TemplateName) Then
            Return Nothing
        End If
        objtemp.TemplateName = objtemp.TemplateName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select TemplateId From eZTemplate Where TemplateName = @TemplateName And CabinetID=@CabinetID and Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@TemplateName", objtemp.TemplateName)
            objParam(0) = param
            param = New SqlParameter("@CabinetID", objtemp.CabinetID)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZTemplate Code already exist!")
            End If
            strQry = "INSERT INTO eZTemplate(TemplateName,Description,CabinetID,DuplicateTypeId,Encrypt,CreatedOn,CreatedBy) " +
                "VALUES(@TemplateName,@Description,@CabinetID,@DuplicateTypeId,@Encrypt,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@TemplateName", objtemp.TemplateName)
            objParam(0) = param
            param = New SqlParameter("@Description", objtemp.Description)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(3) = param
            param = New SqlParameter("@CabinetID", objtemp.CabinetID)
            objParam(4) = param
            param = New SqlParameter("@DuplicateTypeId", objtemp.DuplicateTypeId)
            objParam(5) = param
            param = New SqlParameter("@Encrypt", objtemp.Encrypt)
            objParam(6) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj Is Nothing Then
                Return Nothing
            End If

            newObject = GlobalInstance.eZTemplate(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZTemplate)
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
            If objRead.TemplateName Is Nothing Then
                'strQry = "Select *,dbo.udf_TableName(TemplateId) as TableName,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_Cabinet(CabinetId) as CabinetName,+
                '"dbo.udf_DuplicateType(DuplicateTypeId) as DuplicateType,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZTemplate Where Isdeleted=0 +
                '"and TemplateId=@TemplateId"
                strQry = "Select tmp.*,dbo.udf_TableName(tmp.TemplateId) as TableName,dbo.udf_UserName(tmp.UpdatedBy) as UpdatedBy1,dbo.udf_Cabinet(tmp.CabinetId) as CabinetName," +
                    "dbo.udf_DuplicateType(tmp.DuplicateTypeId) as DuplicateType,dbo.udf_UserName(tmp.CreatedBy) as CreatedBy1,ERS.ERSName as ERSName,ERS.ERSServerName as ERSServerName," +
                    "ERS.ERSDirPath as ERSDirPath,ERS.ERSIndexinpath as ERSIndexinpath From eZTemplate tmp left outer join eZCabinet cab on tmp.cabinetid=cab.cabinetid " +
                    "left outer join eZERSInfo ERS on ERS.ERSId =cab.ERSId Where (isnull(cab.Isdeleted, 0) = 0) and isnull(ERS.Isdeleted,0)=0 " +
                    "and isnull(tmp.Isdeleted,0)=0 and tmp.TemplateId=@TemplateId"
                param = New SqlParameter("@TemplateId", objRead.TemplateId)
                objParam(0) = param
            Else
                'strQry = "Select *,dbo.udf_TableName(TemplateId) as TableName,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_Cabinet(CabinetId) as CabinetName," +
                '    "dbo.udf_DuplicateType(DuplicateTypeId) as DuplicateType,dbo.udf_UserName(CreatedBy) as CreatedBy1  From eZTemplate Where Isdeleted=0 " +
                '    "and TemplateName=@TemplateName"
                strQry = "Select tmp.*,dbo.udf_TableName(tmp.TemplateId) as TableName,dbo.udf_UserName(tmp.UpdatedBy) as UpdatedBy1,dbo.udf_Cabinet(tmp.CabinetId) as CabinetName," +
                 "dbo.udf_DuplicateType(tmp.DuplicateTypeId) as DuplicateType,dbo.udf_UserName(tmp.CreatedBy) as CreatedBy1,ERS.ERSName as ERSName,ERS.ERSServerName as ERSServerName," +
                 "ERS.ERSDirPath as ERSDirPath,ERS.ERSIndexinpath as ERSIndexinpath From eZTemplate tmp left outer join eZCabinet cab on tmp.cabinetid=cab.cabinetid " +
                 "left outer join eZERSInfo ERS on ERS.ERSId =cab.ERSId Where (isnull(cab.Isdeleted, 0) = 0) and isnull(ERS.Isdeleted,0)=0 " +
                 "and isnull(tmp.Isdeleted,0)=0 and tmp.TemplateName=@TemplateName"
                param = New SqlParameter("@TemplateName", objRead.TemplateName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry, objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.TemplateId = GetInteger(sqlRdr("TemplateId"))
                objRead.TemplateName = sqlRdr("TemplateName").ToString()
                objRead.TableName = sqlRdr("TableName").ToString()
                objRead.DuplicateType = sqlRdr("DuplicateType").ToString()
                objRead.DuplicateTypeId = sqlRdr("DuplicateTypeId").ToString()
                objRead.CabinetName = sqlRdr("CabinetName").ToString()
                objRead.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                objRead.Description = sqlRdr("Description").ToString()
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                objRead.Encrypt = Convert.ToInt32(Convert.ToBoolean(sqlRdr("Encrypt")))
                'Try
                '    Dim ERSDirPath = sqlRdr("ERSDirPath").ToString()
                '    If objRead.TemplateId <> 0 Then
                '        If ERSDirPath <> "" Then
                '            Try
                '                DBLayer.DBLInstance.TotalSize = 0
                '                Dim size = (Convert.ToDouble(GetDirSize(ERSDirPath + "\" + objRead.CabinetName + "\" + objRead.TemplateName)) / 1073741824).ToString
                '                objRead.TempCurrentSize = size + " GB"
                '            Catch ex As Exception

                '            End Try
                '        End If
                '    End If
                'Catch
                'End Try
            Else
                'throw new Exception("Attempt to read Invalid eZTemplate.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function ReadAlleZTemplatewithcabexpirydate() As System.Collections.Generic.List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            strQry = "Select TemplateId From eZTemplate where Isdeleted=0  order by TemplateName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadAlleZTemplate() As System.Collections.Generic.List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            strQry = "Select TemplateId From eZTemplate where Isdeleted=0 and cabinetid not in(select cabinetid from ezcabinet " +
                "WHERE (convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) and cabinetid<>1) or isdeleted=1 ) order by TemplateName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadAlleZTemplateForCAC() As System.Collections.Generic.List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            strQry = "Select TemplateId From eZTemplate where Isdeleted=0 and cabinetid not in(select cabinetid from ezcabinet WHERE cabinetid<>1) order by TemplateName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZTemplate(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0  and cabinetid not in(select cabinetid from ezcabinet WHERE convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) and cabinetid<>1) and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by TemplateName"
            Else
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0  and cabinetid not in(select cabinetid from ezcabinet WHERE convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) and cabinetid<>1) order by TemplateName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZTemplateForCAC(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0  and cabinetid not in(select cabinetid from ezcabinet WHERE cabinetid<>1) and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by TemplateName"
            Else
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0  and cabinetid not in(select cabinetid from ezcabinet WHERE cabinetid<>1) order by TemplateName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTemplate(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                'strQry = "Select TemplateId From eZTemplate where Isdeleted=0  and cabinetid not in"+"(select cabinetid from "+
                '"ezcabinet WHERE convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) and cabinetid<>1) and "
                'shankar
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0 and "
                strQry = strQry & "cast(" & Criteria & " as nvarchar) "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by TemplateName"
            Else
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0 order by TemplateName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTemplateForCAC(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by TemplateName"
            Else
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0 order by TemplateName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTemplatewithcabexpirydate(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0  and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by TemplateName"
            Else
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0   order by TemplateName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTemplateWithLoginId(Criteria As String, Value As String, LoginId As String) As System.Collections.Generic.List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select C.TemplateId From eZECMCabinetLevel As C Left Join eZTemplate As T On T.TemplateId=C.TemplateId Where C.ECMLoginId=" + LoginId + " or C.Createdby=" + LoginId + " and C.Cabinetid=T.Cabinetid And T.Isdeleted=0 And C.Isdeleted=0 and C.Cabinetid not in(select cabinetid from ezcabinet WHERE convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106)  and isdeleted=0 and cabinetid<>1) "
                '  strQry = strQry & "Convert(varchar(20),T." & Criteria & ") "
                ' strQry = strQry & " ='"
                ' strQry = strQry & Unquote(Value)
                ' strQry = strQry & "' "
                ' strQry = strQry & " order by TemplateName"
            Else
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0  and cabinetid not in(select cabinetid from ezcabinet WHERE convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) and cabinetid<>1) order by TemplateName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTemplate1(Criteria As String, Value As String, cabid As Integer) As List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0 and CabinetID=" + cabid.ToString() + " and "
                strQry = strQry & "cast(" & Criteria & " as nvarchar) "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & "  and cabinetid not in(select cabinetid from ezcabinet WHERE " +
                    "convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) and cabinetid<>1) order by TemplateName"
            Else
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0  and cabinetid not in(select cabinetid from ezcabinet " +
                    "WHERE convert(datetime,dateadd(dd,1,cabexpirydate),106)<=convert(datetime,getdate(),106) " +
                    "and cabinetid<>1) order by TemplateName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZTemplate1withexpirydate(Criteria As String, Value As String, cabid As Integer) As System.Collections.Generic.List(Of IeZTemplate)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZTemplate)()
        Dim objItem As IeZTemplate
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0 and CabinetID=" + cabid.ToString() + " and "
                strQry = strQry & "Convert(Nvarchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & "  and  order by TemplateName"
            Else
                strQry = "Select TemplateId From eZTemplate where Isdeleted=0   order by TemplateName"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZTemplate.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZTemplate(GetSmallInterger(sqlRdr("TemplateId")))
                objItem.TemplateId = GetSmallInterger(sqlRdr("TemplateId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZTemplate)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select TemplateId From eZTemplate Where TemplateName = @TemplateName and TemplateId <> @TemplateId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@TemplateName", objToUpdate.TemplateName)
        objParam(0) = param
        param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry, objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZTemplate Code already exist!")
        Else
            strQry = "Update eZTemplate Set TemplateName=@TemplateName,DuplicateTypeId=@DuplicateTypeId,Description=@Description," +
                "CabinetID=@CabinetID,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy,Encrypt=@Encrypt where TemplateId=@TemplateId"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@TemplateName", objToUpdate.TemplateName)
            objParam(0) = param
            param = New SqlParameter("@CabinetID", objToUpdate.CabinetID)
            objParam(1) = param
            param = New SqlParameter("@Description", objToUpdate.Description)
            objParam(2) = param
            param = New SqlParameter("@TemplateId", objToUpdate.TemplateId)
            objParam(3) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(4) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(5) = param
            param = New SqlParameter("@DuplicateTypeId", objToUpdate.DuplicateTypeId)
            objParam(6) = param
            param = New SqlParameter("@Encrypt", objToUpdate.Encrypt)
            objParam(7) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry, objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")

            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZTemplate)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZTemplate set Isdeleted=1 where TemplateId=@TemplateId"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@TemplateId", objToDelete.TemplateId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry, objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
#End Region

End Class

