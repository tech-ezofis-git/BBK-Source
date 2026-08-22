Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "Cabinet Details"
    Public Function CreateeZCabinet(objtemp As eZCabinet, userid As String) As IeZCabinet
        Dim newObject As IeZCabinet = Nothing
        Dim cabowner() As String
        If userid <> "" Then
            cabowner = userid.Split(",")
        End If
        If String.IsNullOrEmpty(objtemp.CabinetName) Then
            Return Nothing
        End If
        objtemp.CabinetName = objtemp.CabinetName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select CabinetID From eZCabinet Where CabinetName = @CabinetName And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@CabinetName", objtemp.CabinetName)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("eZCabinet Code already exist!")
            End If
            strQry = "INSERT INTO eZCabinet(CabinetName,Description,CabSize,CabExpiryDate,ERSId,CreatedOn,CreatedBy) " +
                "VALUES(@CabinetName,@Description,@CabSize,@CabExpiryDate,@ERSId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(6) {}
            param = New SqlParameter("@CabinetName", objtemp.CabinetName)
            objParam(0) = param
            param = New SqlParameter("@Description", objtemp.Description)
            objParam(1) = param
            param = New SqlParameter("@CabSize", objtemp.CabSize)
            objParam(2) = param
            'param = New SqlParameter("@CabIcon", objtemp.CabIcon)
            'objParam(3) = param
            param = New SqlParameter("@CabExpiryDate", objtemp.CabExpiryDate)
            objParam(3) = param
            param = New SqlParameter("@ERSId", objtemp.ERSId)
            objParam(4) = param
            param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
            objParam(5) = param
            param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
            objParam(6) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZCabinet(Convert.ToInt32(obj))
            Read(newObject)
            If newObject.CabinetID <> 0 Then
                If cabowner.Count > 0 Then
                    For i As Integer = 0 To cabowner.Count - 1
                        strQry = "INSERT INTO eZCabOwners(CabinetID,UserId,CreatedOn,CreatedBy) " +
                            "VALUES(@CabinetID,@UserId,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
                        objParam = New SqlParameter(3) {}
                        param = New SqlParameter("@CabinetID", newObject.CabinetID)
                        objParam(0) = param
                        param = New SqlParameter("@UserId", cabowner(i).ToString())
                        objParam(1) = param
                        param = New SqlParameter("@CreatedOn", objtemp.CreatedOn)
                        objParam(2) = param
                        param = New SqlParameter("@CreatedBy", objtemp.CreatedBy)
                        objParam(3) = param
                        obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
                    Next
                    If obj Is Nothing Then
                        Return Nothing
                    End If
                    If obj Is Nothing Then
                        Throw New Exception("Cabinet Created and Cabinet owner Problem occuer")
                        Return newObject
                    Else
                        Return newObject
                    End If
                Else
                    Return newObject
                End If
            End If
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZCabinet)
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
            If objRead.CabinetName Is Nothing Then
                strQry = "Select cab.*,dbo.udf_UserName(cab.UpdatedBy) as UpdatedBy1,dbo.udf_UserName(cab.CreatedBy) as CreatedBy1," +
                    "own.UserId as UserId,own.CabOwnerID as CabOwnerID,dbo.udf_UserName(own.UserId) as CabOwnerName," +
                    "ERS.ERSName as ERSName,ERS.ERSServerName as ERSServerName,ERS.ERSDirPath as ERSDirPath,ERS.ERSIndexinpath as " +
                    "ERSIndexinpath From eZCabinet cab left outer join eZERSInfo ERS on ERS.ERSId =cab.ERSId left outer join " +
                    "eZCabOwners own on own.CabinetID =cab.CabinetID Where(isnull(cab.Isdeleted, 0) = 0) and isnull(own.Isdeleted,0)=0 " +
                    "and isnull(ERS.Isdeleted,0)=0 and  cab.CabinetID=@CabinetID"
                param = New SqlParameter("@CabinetID", objRead.CabinetID)
                objParam(0) = param
            Else
                strQry = "Select cab.*,dbo.udf_UserName(cab.UpdatedBy) as UpdatedBy1,dbo.udf_UserName(cab.CreatedBy) as CreatedBy1," +
                    "own.UserId as UserId,own.CabOwnerID as CabOwnerID,dbo.udf_UserName(own.UserId) as CabOwnerName,ERS.ERSName as " +
                    "ERSName,ERS.ERSServerName as ERSServerName,ERS.ERSDirPath as ERSDirPath,ERS.ERSIndexinpath as ERSIndexinpath " +
                    "From eZCabinet cab left outer join eZERSInfo ERS on ERS.ERSId =cab.ERSId left outer join eZCabOwners own on " +
                    "own.CabinetID =cab.CabinetID Where(isnull(cab.Isdeleted, 0) = 0) and isnull(own.Isdeleted,0)=0 and " +
                    "isnull(ERS.Isdeleted,0)=0 and cab.CabinetName=@CabinetName "
                param = New SqlParameter("@CabinetName", objRead.CabinetName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.CabinetID = GetInteger(sqlRdr("CabinetID"))
                objRead.CabOwnerID = GetInteger(sqlRdr("CabOwnerID"))
                objRead.CabinetName = sqlRdr("CabinetName").ToString()
                objRead.UserId = GetSmallInterger(sqlRdr("UserId"))
                objRead.Description = sqlRdr("Description").ToString()
                objRead.CabOwnerName = sqlRdr("CabOwnerName").ToString()
                objRead.CabSize = sqlRdr("CabSize").ToString()
                objRead.CabExpiryDate = GetDate(sqlRdr("CabExpiryDate").ToString())
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                'objRead.CabIcon = DirectCast(sqlRdr("CabIcon"), Byte())
                objRead.ERSId = GetInteger(sqlRdr("ERSId"))
                objRead.ERSName = sqlRdr("ERSName").ToString()
                objRead.ERSServerName = sqlRdr("ERSServerName").ToString()
                objRead.ERSDirPath = sqlRdr("ERSDirPath").ToString()
                objRead.ERSIndexinpath = sqlRdr("ERSIndexinpath").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
                'Try
                '    If objRead.CabinetID <> 0 Then
                '        If objRead.ERSDirPath IsNot Nothing Then
                '            Try
                '                TotalSize = 0
                '                objRead.CabCurrentSize = GetDirSize(objRead.ERSDirPath + "\" + objRead.CabinetName)
                '            Catch ex As Exception

                '            End Try
                '        End If
                '    End If
                'Catch
                'End Try
            Else
                'throw new Exception("Attempt to read Invalid eZCabinet.");
                Return
            End If
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
            objRead.IsModified = False
        End Try
    End Sub
    Public Function GetDirSize(RootFolder As String) As Long
        Dim TotalSize As Long = 0
        Dim FolderInfo = New IO.DirectoryInfo(RootFolder)
        For Each File In FolderInfo.GetFiles : TotalSize += File.Length
        Next
        For Each SubFolderInfo In FolderInfo.GetDirectories : GetDirSize(SubFolderInfo.FullName)
        Next
        Return TotalSize
    End Function
    Public Function ReadAlleZCabinet() As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            strQry = "Select CabinetID From eZCabinet where Isdeleted=0 and convert(datetime,dateadd(dd,1,cabexpirydate),106)" +
                ">=convert(datetime,getdate(),106) or cabinetid=1 order by CabinetName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            'objItem = GlobalInstance.eZCabinet(GetSmallInterger("1"))
            'objItem.CabinetID = GetSmallInterger("1")
            'lstItems.Add(objItem)
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadezCabinetListForCAC() As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            strQry = "Select CabinetID From eZCabinet where Isdeleted=0 order by CabinetName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            'objItem = GlobalInstance.eZCabinet(GetSmallInterger("1"))
            'objItem.CabinetID = GetSmallInterger("1")
            'lstItems.Add(objItem)
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZCabinet(Criteria As String, Value As String) As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            If (Criteria.ToUpper = "CABINETID" And Value = "1") Or (Criteria.ToUpper = "CABINETNAME" And Value.ToUpper = "EZDEFAULTCABINET") Then
                strQry = "Select CabinetID From eZCabinet where Isdeleted=0 and " + Criteria + " = '" + Value.ToString() + "'  order by CabinetName"
            Else
                If Criteria <> "All" Then
                    strQry = "Select CabinetID From eZCabinet where Isdeleted=0  and "
                    strQry = strQry & Criteria
                    strQry = strQry & " like N'%"
                    strQry = strQry & Unquote(Value)
                    strQry = strQry & "%' "
                    strQry = strQry & " and convert(datetime,dateadd(dd,1,cabexpirydate),106)>=convert(datetime,getdate(),106) order by CabinetName"
                Else
                    strQry = "Select CabinetID From eZCabinet where Isdeleted=0 and convert(datetime,dateadd(dd,1,cabexpirydate),106)" +
                        ">=convert(datetime,getdate(),106)  order by CabinetName"
                End If
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            '     If Not ((Criteria.ToUpper = "CABINETID" And Value = "1") Or (Criteria.ToUpper = "CABINETNAME" And Value.ToUpper = "EZDEFAULTCABINET")) Then
            '    'If Criteria.ToUpper <> "CABINETID" Then
            '    objItem = GlobalInstance.eZCabinet(GetSmallInterger("1"))
            '    objItem.CabinetID = GetSmallInterger("1")
            '    lstItems.Add(objItem)
            'End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadFilteredeZCabinetForCAC(Criteria As String, Value As String) As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            If (Criteria.ToUpper = "CABINETID" And Value = "1") Or (Criteria.ToUpper = "CABINETNAME" And Value.ToUpper = "EZDEFAULTCABINET") Then
                strQry = "Select CabinetID From eZCabinet where Isdeleted=0 and " + Criteria + " = '" + Value.ToString() + "'  order by CabinetName"
            Else
                If Criteria <> "All" Then
                    strQry = "Select CabinetID From eZCabinet where Isdeleted=0  and "
                    strQry = strQry & Criteria
                    strQry = strQry & " like N'%"
                    strQry = strQry & Unquote(Value)
                    strQry = strQry & "%' "
                    strQry = strQry & " order by CabinetName"
                Else
                    strQry = "Select CabinetID From eZCabinet where Isdeleted=0 order by CabinetName"
                End If
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            'If Not ((Criteria.ToUpper = "CABINETID" And Value = "1") Or (Criteria.ToUpper = "CABINETNAME" And Value.ToUpper = "EZDEFAULTCABINET")) Then
            '    'If Criteria.ToUpper <> "CABINETID" Then
            '    objItem = GlobalInstance.eZCabinet(GetSmallInterger("1"))
            '    objItem.CabinetID = GetSmallInterger("1")
            '    lstItems.Add(objItem)
            'End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZCabinet(Criteria As String, Value As String) As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            If (Criteria.ToUpper = "CABINETID" And Value = "1") Or (Criteria.ToUpper = "CABINETNAME" And Value.ToUpper = "EZDEFAULTCABINET") Then
                strQry = "Select CabinetID From eZCabinet where Isdeleted=0 and " + Criteria + " = '" + Value.ToString() + "'  order by CabinetName"
            Else
                If Criteria <> "All" Then
                    strQry = "Select CabinetID From eZCabinet where Isdeleted=0  and  "
                    strQry = strQry & "Convert(nvarchar(max)," & Criteria & ") "
                    strQry = strQry & " =N'"
                    strQry = strQry & Unquote(Value)
                    strQry = strQry & "' "
                    strQry = strQry & " and convert(datetime,dateadd(dd,1,cabexpirydate),106)>=convert(datetime,getdate(),106) order by CabinetName"
                Else
                    strQry = "Select CabinetID From eZCabinet where Isdeleted=0 and convert(datetime,dateadd(dd,1,cabexpirydate),106)" +
                        ">=convert(datetime,getdate(),106)  order by CabinetName"
                End If
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            '   If Not ((Criteria.ToUpper = "CABINETID" And Value = "1") Or (Criteria.ToUpper = "CABINETNAME" And Value.ToUpper = "EZDEFAULTCABINET")) Then
            '    'If Criteria.ToUpper <> "CABINETID" Then
            '    objItem = GlobalInstance.eZCabinet(GetSmallInterger("1"))
            '    objItem.CabinetID = GetSmallInterger("1")
            '    lstItems.Add(objItem)
            'End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZCabinetForCAC(Criteria As String, Value As String) As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            If (Criteria.ToUpper = "CABINETID" And Value = "1") Or (Criteria.ToUpper = "CABINETNAME" And Value.ToUpper = "EZDEFAULTCABINET") Then
                strQry = "Select CabinetID From eZCabinet where Isdeleted=0 and " + Criteria + " = '" + Value.ToString() + "'  order by CabinetName"
            Else
                If Criteria <> "All" Then
                    strQry = "Select CabinetID From eZCabinet where Isdeleted=0  and  "
                    strQry = strQry & "Convert(Nvarchar(20)," & Criteria & ") "
                    strQry = strQry & " =N'"
                    strQry = strQry & Unquote(Value)
                    strQry = strQry & "' "
                    strQry = strQry & " order by CabinetName"
                Else
                    strQry = "Select CabinetID From eZCabinet where Isdeleted=0 order by CabinetName"
                End If
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            '   If Not ((Criteria.ToUpper = "CABINETID" And Value = "1") Or (Criteria.ToUpper = "CABINETNAME" And Value.ToUpper = "EZDEFAULTCABINET")) Then
            '    'If Criteria.ToUpper <> "CABINETID" Then
            '    objItem = GlobalInstance.eZCabinet(GetSmallInterger("1"))
            '    objItem.CabinetID = GetSmallInterger("1")
            '    lstItems.Add(objItem)
            'End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZCabinetByuserid(ByVal UserId As String) As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            strQry = "Select C.CabinetID as CabinetID From eZCabinet As C Left Join eZCabOwners As O On O.CabinetId=C.CabinetId " +
                "where C.Isdeleted=0 and O.UserId='" + UserId + "' and convert(datetime,dateadd(dd,1,c.cabexpirydate),106)" +
                ">=convert(datetime,getdate(),106) "
            'strQry = strQry & "UserId "
            'strQry = strQry & " ='"
            'strQry = strQry & Unquote(UserId)
            'strQry = strQry & "' "
            'strQry = strQry & " order by CabinetID"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            objItem = GlobalInstance.eZCabinet(GetSmallInterger("1"))
            objItem.CabinetID = GetSmallInterger("1")
            lstItems.Add(objItem)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZCabinetByuseridForCAC(ByVal UserId As String) As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            strQry = "Select C.CabinetID as CabinetID From eZCabinet As C Left Join eZCabOwners As O On O.CabinetId=C.CabinetId " +
                "where C.Isdeleted=0 and O.UserId='" + UserId + "' "
            'strQry = strQry & "UserId "
            'strQry = strQry & " ='"
            'strQry = strQry & Unquote(UserId)
            'strQry = strQry & "' "
            'strQry = strQry & " order by CabinetID"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            objItem = GlobalInstance.eZCabinet(GetSmallInterger("1"))
            objItem.CabinetID = GetSmallInterger("1")
            lstItems.Add(objItem)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZCabinetByuserid1(ByVal UserId As String) As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            strQry = "Select C.CabinetID as CabinetID From eZCabinet As C Left Join eZCabOwners As O On O.CabinetId=C.CabinetId " +
                "where C.CabinetID=1 or C.Isdeleted=0 and O.UserId='" + UserId + "' and convert(datetime,dateadd(dd,1,c.cabexpirydate),106)" +
                ">=convert(datetime,getdate(),106) "
            'strQry = strQry & "UserId "
            'strQry = strQry & " ='"
            'strQry = strQry & Unquote(UserId)
            'strQry = strQry & "' "
            'strQry = strQry & " order by CabinetID"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            'objItem = GlobalInstance.eZCabinet(GetSmallInterger("1"))
            'objItem.CabinetID = GetSmallInterger("1")
            'lstItems.Add(objItem)
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZCabinetByuserid1ForCAC(ByVal UserId As String) As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            strQry = "Select C.CabinetID as CabinetID From eZCabinet As C Left Join eZCabOwners As O On O.CabinetId=C.CabinetId " +
                "where C.CabinetID=1 or C.Isdeleted=0 and O.UserId='" + UserId + "' "
            'strQry = strQry & "UserId "
            'strQry = strQry & " ='"
            'strQry = strQry & Unquote(UserId)
            'strQry = strQry & "' "
            'strQry = strQry & " order by CabinetID"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            'objItem = GlobalInstance.eZCabinet(GetSmallInterger("1"))
            'objItem.CabinetID = GetSmallInterger("1")
            'lstItems.Add(objItem)
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZCabinetByuseridwithcabexpirydate(ByVal UserId As String) As List(Of IeZCabinet)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZCabinet)()
        Dim objItem As IeZCabinet
        Try
            Dim strQry As String = ""
            strQry = "Select C.CabinetID as CabinetID From eZCabinet As C Left Join eZCabOwners As O On O.CabinetId=C.CabinetId where " +
                "C.CabinetID=1 or C.Isdeleted=0 and O.UserId='" + UserId + "' "
            'strQry = strQry & "UserId "
            'strQry = strQry & " ='"
            'strQry = strQry & Unquote(UserId)
            'strQry = strQry & "' "
            'strQry = strQry & " order by CabinetID"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZCabinet.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZCabinet(GetSmallInterger(sqlRdr("CabinetID")))
                objItem.CabinetID = GetSmallInterger(sqlRdr("CabinetID"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    'Public Function updateandAddandRemoveowner(cabinetid As String, adduserid As String, removeuserid As String, updatedby As String) As String
    '    Dim strQry As String = ""
    '    Dim objParam As SqlParameter()
    '    Dim param As SqlParameter


    '    If removeuserid <> "" Then
    '        strQry = "Update eZCabOwners Set isdeleted=1,UpdatedOn='" + DateDateTimeToString(Date.Now, True) + "',UpdatedBy='" + updatedby.ToString() + "' where cabinetid='" + cabinetid.ToString() + "' and userid in(" + removeuserid.ToString() + ")"
    '        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString()) = 0 Then
    '            Throw New Exception("Record Not updated due to some error")
    '        End If

    '    End If
    '    If adduserid <> "" Then
    '        Dim obj2 As Object
    '        Dim obj1 As Object
    '        Dim add() As String
    '        add = adduserid.Split(",")
    '        If add.Count > 0 Then
    '            For i As Integer = 0 To add.Count - 1
    '                strQry = "SELECT userid from ezcabowners WHERE isdeleted=0 and cabinetid='" + cabinetid.ToString() + "' and userid not in (" + adduserid.ToString() + ")"
    '                Dim ds = GetDatasetByQuery(strQry)
    '                If ds.Tables(0).Rows.Count <> 0 Then
    '                    For i As Integer = 0 To ds.Tables(0).
    '                    adduserid = adduserid.Replace("," + ds.Tables(0).Rows(0).Item(0).ToString(), "").Replace(ds.Tables(0).Rows(0).Item(0).ToString(), "")
    '                End If
    '                obj1 = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString())
    '                If obj1 Is Nothing Then
    '                    strQry = "INSERT INTO eZCabOwners(CabinetID,UserId,CreatedOn,CreatedBy) VALUES('" + cabinetid.ToString() + "','" + add(i).ToString() + "','" + DateDateTimeToString(Date.Now, True) + "','" + updatedby.ToString() + "');Select SCOPE_IDENTITY();"
    '                    obj2 = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString())
    '                End If
    '            Next
    '            If obj2 Is Nothing And obj1 Is Nothing Then
    '                Throw New Exception("Cabowner Not added due to some error")
    '            Else
    '                Return "user added!"
    '            End If
    '        End If
    '    End If



    'End Function
    Public Sub Update(objToUpdate As IeZCabinet)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select CabinetID From eZCabinet Where CabinetName = @CabinetName and CabinetID <> @CabinetID and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@CabinetName", objToUpdate.CabinetName)
        objParam(0) = param
        param = New SqlParameter("@CabinetID", objToUpdate.CabinetID)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("eZCabinet Code already exist!")
        Else
            strQry = "Update eZCabinet Set CabinetName=@CabinetName,ERSId=@ERSId,Description=@Description,CabSize=@CabSize," +
                "CabExpiryDate=@CabExpiryDate,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where CabinetID=@CabinetID"
            objParam = New SqlParameter(7) {}
            param = New SqlParameter("@CabinetName", objToUpdate.CabinetName)
            objParam(0) = param
            param = New SqlParameter("@ERSId", objToUpdate.ERSId)
            objParam(1) = param
            param = New SqlParameter("@Description", objToUpdate.Description)
            objParam(2) = param
            param = New SqlParameter("@CabSize", objToUpdate.CabSize)
            objParam(3) = param
            'param = New SqlParameter("@CabIcon", objToUpdate.CabIcon)
            'objParam(4) = param
            param = New SqlParameter("@CabExpiryDate", objToUpdate.CabExpiryDate)
            objParam(4) = param
            param = New SqlParameter("@CabinetID", objToUpdate.CabinetID)
            objParam(5) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(6) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(7) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            Else

                'strQry = "Update eZCabOwners Set UserId=@UserId,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy where CabOwnerID=@CabOwnerID"
                'objParam = New SqlParameter(3) {}
                'param = New SqlParameter("@UserId", objToUpdate.UserId)
                'objParam(0) = param
                'param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
                'objParam(1) = param
                'param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
                'objParam(2) = param
                'param = New SqlParameter("@CabOwnerID", objToUpdate.CabOwnerID)
                'objParam(3) = param
                'If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                '    Throw New Exception("Record Not updated due to some error")
                'End If
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZCabinet)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update eZCabinet set Isdeleted=1 where CabinetID=@CabinetID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@CabinetID", objToDelete.CabinetID)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            strQry = "Update eZCabOwners set Isdeleted=1 where CabinetID=@CabinetID"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@CabinetID", objToDelete.CabinetID)
            objParam(0) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not deleted due to some error")
            End If
        End If
    End Sub
#End Region
End Class

