Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.IO.Compression
Imports ECMAPI.ParaVariables
Imports ECMAPI.SharedGetFunction
Imports System.Threading

Namespace Controllers
    Public Class eZCabinetAndTemplateController
        Inherits ApiController


        <HttpPost>
        Public Function GetCabinetListByLoginId(Para As Byid) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = eZCabinetListByLoginId(Para.id)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetTemplateListWithCabinetId(Para As Byid) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = eZTemplateListWithCabinetId(Para.id)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetSelectedeZTemplateFieldList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = SelectedeZTemplateFieldList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetSelectedeZCabinetList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = SelectedeZCabinetList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetSelectedeZTemplateList(Para As Criteria) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = SelectedeZTemplateList(Para.Criteria, Para.Value)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetERSDetails(Para As ForERSPath) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = GetERSPath(Para.CabinetID, Para.ERSDirPath, Para.SettingPath)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetXMLCreation(Para As ForXMLCreation) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = XMLCreation(Para.cabid, Para.cabname, Para.tmpid, Para.tmpname, Para.fields, Para.fieldvalues, Para.filename, Para.size, Para.xmlfilename, Para.loginid, Para.ipaddress, Para.ezfrom, Para.nopages)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetItemUsers(Para As ItemtableList) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = GetItemUserList(Para.TemplateId, Para.ReportFor)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Public Function GetItemApps(Para As ItemtableList) As HttpResponseMessage
            Dim response As HttpResponseMessage
            Try
                Dim Objvalue = GetItemApplicationList(Para.TemplateId, Para.ReportFor)
                response = Request.CreateResponse(HttpStatusCode.OK, Objvalue)
            Catch ex As Exception
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message)
            End Try
            Return response
        End Function

        <HttpPost>
        Function GetItemsByCriteria(ByVal Para As SearchRegistries) As DataSet

            Dim CondtionReg As String = ""
            Dim Tablename = ""
            For Each cond In Para.Criteria


                If cond.Criteria.ToLower() = "templateid" Then
                    If cond.Value1 <> "" Then
                        Tablename = GetTableName(cond.Value1)
                    End If
                Else
                    If cond.Criteria.ToLower() = "createdby" Then
                        If cond.Value1 <> "" AndAlso Not cond.Value1.Contains(",") Then
                            cond.Value1 = GetLoginIdByUsername(cond.Value1)
                        End If
                    End If
                    If cond.DataTypeId = "2" Then
                        If cond.Value1.Contains(",") Then
                            Dim Inval = ""
                            Dim values = cond.Value1.ToString.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                            For j As Integer = 0 To values.Count - 1
                                If cond.Criteria.ToLower() = "createdby" Then
                                    If values(j) <> "" Then
                                        values(j) = GetLoginIdByUsername(values(j))
                                    End If
                                End If

                                If j = 0 Then
                                    Inval = "'" + values(j) + "'"
                                Else
                                    Inval = Inval + ",'" + values(j) + "'"
                                End If

                            Next

                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in (" + Inval + ") "
                        Else
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] = '" + cond.Value1 + "'"
                        End If


                    ElseIf cond.DataTypeId = "4" Then
                        If cond.Criteria = "ezfrom" AndAlso cond.Value1 = "ECM-Capture" Then
                            CondtionReg = CondtionReg + " and ([" + cond.Criteria + "] LIKE '%ECM-Capture%' or [" + cond.Criteria + "] LIKE '%Scanned%' or [" + cond.Criteria + "] LIKE '%Digital%')"
                        Else
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                        End If
                    ElseIf cond.DataTypeId = "5" Then
                        If cond.Value1 <> "" And cond.Value2 <> "" Then
                            If cond.Value1 = cond.Value2 Then
                                CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            Else

                                CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            End If

                        ElseIf cond.Value1 <> "" Then
                            CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and convert(datetime,[" + cond.Criteria + "],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                        ElseIf cond.Value2 <> "" Then
                            CondtionReg = CondtionReg + "  and convert(datetime,[" + cond.Criteria + "],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                        End If
                    End If
                End If

            Next
            Dim ItemListCount = GetDatasetByQuery("Select Count(1) from " + Tablename + " where isdeleted=0 " + CondtionReg)
            Dim TotalRow As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select Count(1) from " + Tablename + " where isdeleted=0 and ezfrom like 'Scanned%' " + CondtionReg)
            Dim TotalRow_Scanned As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                TotalRow_Scanned = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select Count(1) from " + Tablename + " where isdeleted=0 and ezfrom like 'Digital%' " + CondtionReg)
            Dim TotalRow_Digital As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                TotalRow_Digital = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0  " + CondtionReg)
            Dim PageCount As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0 and ezfrom like 'Scanned%' " + CondtionReg)
            Dim PageCount_Scanned As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                PageCount_Scanned = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0 and ezfrom like 'Digital%' " + CondtionReg)
            Dim PageCount_Digital As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                PageCount_Digital = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM NUMBER] from " + Tablename + " where isdeleted=0  " + CondtionReg + ") as x")
            Dim RIMCount As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                RIMCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM NUMBER] from " + Tablename + " where isdeleted=0 and ezfrom like 'Scanned%' " + CondtionReg + ") as x")
            Dim RIMCount_Scanned As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                RIMCount_Scanned = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM NUMBER] from " + Tablename + " where isdeleted=0 and ezfrom like 'Digital%' " + CondtionReg + ") as x")
            Dim RIMCount_Digital As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                RIMCount_Digital = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If

            Dim FinalQuery = ""
            If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                If Para.RowCount <> 0 Then
                    FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount,'" + PageCount_Scanned + "' as PageCount_Scanned,'" + PageCount_Digital + "' as PageCount_Digital, '" + TotalRow + "' as TotalRow,'" + TotalRow_Scanned + "' as TotalRow_Scanned,'" + TotalRow_Digital + "' as TotalRow_Digital,'" + RIMCount + "' as RIMCount,'" + RIMCount_Scanned + "' as RIMCount_Scanned, '" + RIMCount_Digital + "' as RIMCount_Digital from " + Tablename + " where isdeleted=0  " + CondtionReg & " order by itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                Else
                    FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount,'" + PageCount_Scanned + "' as PageCount_Scanned,'" + PageCount_Digital + "' as PageCount_Digital,'" + TotalRow + "' as TotalRow, '" + TotalRow_Scanned + "' as TotalRow,'" + TotalRow_Digital + "' as TotalRow_Digital,'" + RIMCount + "' as RIMCount,'" + RIMCount_Scanned + "' as RIMCount_Scanned, '" + RIMCount_Digital + "' as RIMCount_Digital from " + Tablename + " where isdeleted=0  " + CondtionReg & " order by itemid "
                End If
            Else
                If Para.RowCount <> 0 Then
                    '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    FinalQuery = "SELECT * FROM (Select *  ,dbo.udf_LoginName (CreatedBy) As LoginName ,'" + PageCount + "' as PageCount,'" + PageCount_Scanned + "' as PageCount_Scanned,'" + PageCount_Digital + "' as PageCount_Digital,'" + TotalRow + "' as TotalRow,'" + TotalRow_Scanned + "' as TotalRow_Scanned,'" + TotalRow_Digital + "' as TotalRow_Digital,'" + RIMCount + "' as RIMCount,'" + RIMCount_Scanned + "' as RIMCount_Scanned, '" + RIMCount_Digital + "' as RIMCount_Digital, ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0  " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                    ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                Else
                    FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) As LoginName ,'" + PageCount + "' as PageCount,'" + PageCount_Scanned + "' as PageCount_Scanned,'" + PageCount_Digital + "' as PageCount_Digital,'" + TotalRow + "' as TotalRow, '" + TotalRow_Scanned + "' as TotalRow_Scanned,'" + TotalRow_Digital + "' as TotalRow_Digital,'" + RIMCount + "' as RIMCount,'" + RIMCount_Scanned + "' as RIMCount_Scanned, '" + RIMCount_Digital + "' as RIMCount_Digital from " + Tablename + " where isdeleted=0  " + CondtionReg
                End If
            End If
            'If Para.RowCount <> 0 Then

            '    CondtionReg = CondtionReg & " order by itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
            'End If


            Dim ItemList = GetDatasetByQuery(FinalQuery)
            If Not IsNothing(ItemList) AndAlso ItemList.Tables.Count > 0 AndAlso ItemList.Tables(0).Rows.Count > 0 Then
                Return ItemList
            Else
                Return Nothing
            End If




            '  Return ReurnResults
        End Function
        Function GetItemsByCriteriaTest(ByVal Para As SearchRegistries) As String

            Dim CondtionReg As String = ""
            Dim Tablename = ""
            For Each cond In Para.Criteria


                If cond.Criteria.ToLower() = "templateid" Then
                    If cond.Value1 <> "" Then
                        Tablename = GetTableName(cond.Value1)
                    End If
                Else
                    If cond.Criteria.ToLower() = "createdby" Then
                        If cond.Value1 <> "" AndAlso Not cond.Value1.Contains(",") Then
                            cond.Value1 = GetLoginIdByUsername(cond.Value1)
                        End If
                    End If
                    If cond.DataTypeId = "2" Then
                        If cond.Value1.Contains(",") Then
                            Dim Inval = ""
                            Dim values = cond.Value1.ToString.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                            For j As Integer = 0 To values.Count - 1
                                If cond.Criteria.ToLower() = "createdby" Then
                                    If values(j) <> "" Then
                                        values(j) = GetLoginIdByUsername(values(j))
                                    End If
                                End If

                                If j = 0 Then
                                    Inval = "'" + values(j) + "'"
                                Else
                                    Inval = Inval + ",'" + values(j) + "'"
                                End If

                            Next

                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in (" + Inval + ") "
                        Else
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] = '" + cond.Value1 + "'"
                        End If


                    ElseIf cond.DataTypeId = "4" Then
                        If cond.Criteria = "ezfrom" AndAlso cond.Value1 = "ECM-Capture" Then
                            CondtionReg = CondtionReg + " and ([" + cond.Criteria + "] LIKE '%ECM-Capture%' or [" + cond.Criteria + "] LIKE '%Scanned%' or [" + cond.Criteria + "] LIKE '%Digital%')"
                        Else
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                        End If
                    ElseIf cond.DataTypeId = "5" Then
                        If cond.Value1 <> "" And cond.Value2 <> "" Then
                            If cond.Value1 = cond.Value2 Then
                                CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            Else

                                CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                            End If

                        ElseIf cond.Value1 <> "" Then
                            CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and convert(datetime,[" + cond.Criteria + "],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                        ElseIf cond.Value2 <> "" Then
                            CondtionReg = CondtionReg + "  and convert(datetime,[" + cond.Criteria + "],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                        End If
                    End If
                End If

            Next
            Dim ItemListCount = GetDatasetByQuery("Select Count(1) from " + Tablename + " where isdeleted=0 " + CondtionReg)
            Dim TotalRow As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select Count(1) from " + Tablename + " where isdeleted=0 and ezfrom like 'Scanned%' " + CondtionReg)
            Dim TotalRow_Scanned As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                TotalRow_Scanned = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select Count(1) from " + Tablename + " where isdeleted=0 and ezfrom like 'Digital%' " + CondtionReg)
            Dim TotalRow_Digital As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                TotalRow_Digital = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0  " + CondtionReg)
            Dim PageCount As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0 and ezfrom like 'Scanned%' " + CondtionReg)
            Dim PageCount_Scanned As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                PageCount_Scanned = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0 and ezfrom like 'Digital%' " + CondtionReg)
            Dim PageCount_Digital As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                PageCount_Digital = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM NUMBER] from " + Tablename + " where isdeleted=0  " + CondtionReg + ") as x")
            Dim RIMCount As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                RIMCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM NUMBER] from " + Tablename + " where isdeleted=0 and ezfrom like 'Scanned%' " + CondtionReg + ") as x")
            Dim RIMCount_Scanned As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                RIMCount_Scanned = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If
            ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM NUMBER] from " + Tablename + " where isdeleted=0 and ezfrom like 'Digital%' " + CondtionReg + ") as x")
            Dim RIMCount_Digital As String = "0"
            If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                RIMCount_Digital = ItemListCount.Tables(0).Rows(0)(0).ToString()
            End If

            Dim FinalQuery = ""
            If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                If Para.RowCount <> 0 Then
                    FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount,'" + PageCount_Scanned + "' as PageCount_Scanned,'" + PageCount_Digital + "' as PageCount_Digital, '" + TotalRow + "' as TotalRow,'" + TotalRow_Scanned + "' as TotalRow_Scanned,'" + TotalRow_Digital + "' as TotalRow_Digital,'" + RIMCount + "' as RIMCount,'" + RIMCount_Scanned + "' as RIMCount_Scanned, '" + RIMCount_Digital + "' as RIMCount_Digital from " + Tablename + " where isdeleted=0  " + CondtionReg & " order by itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                Else
                    FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount,'" + PageCount_Scanned + "' as PageCount_Scanned,'" + PageCount_Digital + "' as PageCount_Digital,'" + TotalRow + "' as TotalRow, '" + TotalRow_Scanned + "' as TotalRow,'" + TotalRow_Digital + "' as TotalRow_Digital,'" + RIMCount + "' as RIMCount,'" + RIMCount_Scanned + "' as RIMCount_Scanned, '" + RIMCount_Digital + "' as RIMCount_Digital from " + Tablename + " where isdeleted=0  " + CondtionReg & " order by itemid "
                End If
            Else
                If Para.RowCount <> 0 Then
                    '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    FinalQuery = "SELECT * FROM (Select *  ,dbo.udf_LoginName (CreatedBy) As LoginName ,'" + PageCount + "' as PageCount,'" + PageCount_Scanned + "' as PageCount_Scanned,'" + PageCount_Digital + "' as PageCount_Digital,'" + TotalRow + "' as TotalRow,'" + TotalRow_Scanned + "' as TotalRow_Scanned,'" + TotalRow_Digital + "' as TotalRow_Digital,'" + RIMCount + "' as RIMCount,'" + RIMCount_Scanned + "' as RIMCount_Scanned, '" + RIMCount_Digital + "' as RIMCount_Digital, ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0  " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                    ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                Else
                    FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) As LoginName ,'" + PageCount + "' as PageCount,'" + PageCount_Scanned + "' as PageCount_Scanned,'" + PageCount_Digital + "' as PageCount_Digital,'" + TotalRow + "' as TotalRow, '" + TotalRow_Scanned + "' as TotalRow_Scanned,'" + TotalRow_Digital + "' as TotalRow_Digital,'" + RIMCount + "' as RIMCount,'" + RIMCount_Scanned + "' as RIMCount_Scanned, '" + RIMCount_Digital + "' as RIMCount_Digital from " + Tablename + " where isdeleted=0  " + CondtionReg
                End If
            End If
            'If Para.RowCount <> 0 Then

            '    CondtionReg = CondtionReg & " order by itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
            'End If

            Return FinalQuery
            '  Return ReurnResults
        End Function
        Private Function FieldLevelSecurity(tmpid As String, loginid As String) As String
            Dim result As String = ""

            Try
                ' Dim sql As String = "select FieldValue from ezECMFieldLevel where TemplateId=" + tmpid.ToString + " and ECMLoginId<>" + loginid.ToString + " and FieldValue not in (Select FieldValue from ezECMFieldLevel where ECMGroupId in (select ecmprofileid from  eZECMLogin where ECMLoginId =" + loginid.ToString + "))"

                Dim sql As String = "select distinct FieldValue from ezECMFieldLevel where TemplateId=" + tmpid.ToString + " and (ECMLoginId<>" + loginid.ToString + " or ECMGroupId not in (select ECMGroupId from eZECMGroupUsers where ECMLoginId=" + loginid.ToString + ") or ECMProfileId not in (select ECMProfileId from eZECMLogin where ECMLoginId=" + loginid.ToString + ")) and FieldValue not in (Select FieldValue from ezECMFieldLevel where ECMLoginId=" + loginid.ToString + " or ecmgroupid in (select ECMGroupId from eZECMGroupUsers where ECMLoginId=" + loginid.ToString + ") or ECMProfileId in (select ECMProfileId from eZECMLogin where ECMLoginId=" + loginid.ToString + "))"
                Dim table As String = GetTableName(tmpid)
                Dim ds As DataSet = GetDatasetByQuery(sql)
                If Not ds Is Nothing Then
                    If ds.Tables.Count > 0 Then
                        For Each Row As DataRow In ds.Tables(0).Rows
                            result += " (" + Row("FieldValue").ToString + ") or "
                        Next
                        If result.Length > 0 Then
                            result = result.Substring(0, result.Length - 3)
                        End If
                        If ds.Tables(0).Rows.Count > 0 Then
                            result = " and itemid not in (select itemid from " + table + " where " + result + " and itemid not in (select itemid from ezhidefile where hidefileid in (select hidefileid from ezhidefileusers where userid=" + loginid.ToString + " and show=1) " +
                    "and GETDATE() between fromdate and todate and templateid=" + tmpid.ToString + " and isdeleted=0)) "
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try
            Return result
        End Function

        <HttpPost>
        Function GetItemsFieldSearch(ByVal Para As SearchRegistries) As DataSet()

            Dim CondtionReg As String = ""
            Dim Tablename = ""
            Dim templateid = ""
            Dim ECMLoginId = ""
            Dim Flag = 0
            For Each cond In Para.Criteria

                If cond.Criteria = "without Deposit" Then
                    Flag = 4
                    CondtionReg = CondtionReg + " and  [Document Type] <> 'Deposit' and [RIM Number] not in (select distinct [RIM Number] from eZCA_3_9_items where ifilename<>'' and [Document Type] ='Deposit') "
                ElseIf cond.Criteria = "Account Number Report" Then
                    CondtionReg = CondtionReg + " and  [Account Number] <> '' "
                ElseIf cond.Criteria = "PRODUCTIVITY REPORT" Then
                    Flag = 1
                ElseIf cond.Criteria = "ORIGINAL FILE STATUS" Then
                    Flag = 2
                ElseIf cond.Criteria = "QUALITY CHECK CHANGE REPORT" Then
                    Flag = 3
                ElseIf cond.Criteria = "PRODUCTIVITY REPORT Unique" Then
                    Flag = 5
                Else

                    If cond.Criteria.ToLower() = "templateid" Then
                        If cond.Value1 <> "" Then
                            Tablename = GetTableName(cond.Value1)
                            templateid = cond.Value1
                        End If
                    ElseIf cond.Criteria.ToLower() = "ecmloginid" Then
                        If cond.Value1 <> "" Then
                            ' Tablename = GetTableName(cond.Value1)
                            ECMLoginId = cond.Value1
                        End If
                    Else
                        If cond.Criteria.ToLower() = "createdby" Then
                            If cond.Value1 <> "" AndAlso Not cond.Value1.Contains(",") Then
                                cond.Value1 = GetLoginIdByUsername(cond.Value1)
                            End If
                        End If
                        If cond.DataTypeId = "2" Then
                            If cond.Value1.Contains(",") Then
                                Dim Inval = ""
                                Dim values = cond.Value1.ToString.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                                For j As Integer = 0 To values.Count - 1
                                    If cond.Criteria.ToLower() = "createdby" Then
                                        If values(j) <> "" Then
                                            values(j) = GetLoginIdByUsername(values(j))
                                        End If
                                    End If

                                    If j = 0 Then
                                        Inval = "'" + values(j) + "'"
                                    Else
                                        Inval = Inval + ",'" + values(j) + "'"
                                    End If

                                Next

                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in (" + Inval + ") "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] = '" + cond.Value1 + "'"
                            End If


                        ElseIf cond.DataTypeId = "4" Then
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.DataTypeId = "5" Then
                            If cond.Value1 <> "" And cond.Value2 <> "" Then
                                If cond.Value1 = cond.Value2 Then
                                    CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                Else

                                    CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                End If

                            ElseIf cond.Value1 <> "" Then
                                CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and convert(datetime,[" + cond.Criteria + "],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                            ElseIf cond.Value2 <> "" Then
                                CondtionReg = CondtionReg + "  and convert(datetime,[" + cond.Criteria + "],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                            End If
                        End If
                    End If
                End If
            Next

            Dim docseccond = ""
            docseccond = FieldLevelSecurity(templateid, ECMLoginId)
            CondtionReg += docseccond


            Dim FinalQuery = ""
            If Flag = 0 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg)
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg)
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                'ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM NUMBER] from " + Tablename + " where isdeleted=0  " + CondtionReg + ") as x")
                'Dim RIMCount As String = ""
                'If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                '    RIMCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                'End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & " order by itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg & " order by itemid "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                        FinalQuery = "SELECT * FROM (Select *  ,dbo.udf_LoginName (CreatedBy) As LoginName,dbo.udf_LoginName (UpdatedBy) As [Last UpdatedBy] ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                        ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    Else
                        FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) As LoginName ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg
                    End If
                End If
            ElseIf Flag = 4 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg)
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg)
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                'ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM NUMBER] from " + Tablename + " where isdeleted=0  " + CondtionReg + ") as x")
                'Dim RIMCount As String = ""
                'If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                '    RIMCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                'End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & " order by [RIM Number]  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg & " order by [RIM Number] "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                        FinalQuery = "SELECT * FROM (Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY [RIM Number]) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                        ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    Else
                        FinalQuery = "Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg
                    End If
                End If
            ElseIf Flag = 1 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> '' " + CondtionReg & " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy order by convert(date,max(substring(createdon,0,12)),106)  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> ''  " + CondtionReg & " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy order by convert(date,max(substring(createdon,0,12)),106) "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                        FinalQuery = "SELECT * FROM (Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString() + " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                        ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg + +" group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                    End If
                End If
            ElseIf Flag = 5 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 substring(createdon,0,12) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> '' " + CondtionReg & " group by [RIM Number],CheckOut order by [Created Date]  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 substring(createdon,0,12) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User],'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> ''  " + CondtionReg & " group by [RIM Number],CheckOut order by [Created Date] "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                        FinalQuery = "SELECT * FROM (Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 substring(createdon,0,12) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString() + " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                        ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 substring(createdon,0,12) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg + +" group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                    End If
                End If
            ElseIf Flag = 2 Then
                Dim ItemListCount = GetDatasetByQuery("select Count(1) from ezusersession us left join eZCA_3_9_items itm on us.itemid=itm.itemid where us.TemplateId='9' and  us.TemplateId=9 and ifilename<>''")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("select sum(itm.nopages) from ezusersession us left join eZCA_3_9_items itm on us.itemid=itm.itemid where us.TemplateId='9' and  us.TemplateId=9 and ifilename<>''")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "select itm.itemid,itm.templateid,dbo.udf_CabinetByTemplateId(us.TemplateId) as Cabinet,dbo.udf_Template(us.TemplateId) as Template,itm.ifilename as [File Name], [RIM Number],[TIN Number],[Account Number], case when UplaodDocument<>0 then 'Document Uploaded' when ViewDocument<>0 then 'Document Viewed' when CommentsId <>0 then 'Document Commented' when us.CheckOut<>0 then 'Document Checked Out' when linkid<>0 then 'Document Linked' when AlertDocument<>0 then 'Document Alerted' when IndexingChange<>0 then 'Document Indexing Value Changed' when Deleted<>0 then 'Document Deleted' when bookmarks<>0 then 'Document Bookmarked' when email<>0 then 'Document Sent by Email' when checkin<>0 then 'Document Checked In' when PrintDoc<>0 then 'Document Printed' else '' end as [Action],dbo.udf_LoginName(us.ECMLoginId) as [Action By],us.CreatedOn as [Acted On],'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from ezusersession us left join eZCA_3_9_items itm on us.itemid=itm.itemid where us.TemplateId='9' and  us.TemplateId=9 and ifilename<>'' order by itm.itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "select itm.itemid,itm.templateid,dbo.udf_CabinetByTemplateId(us.TemplateId) as Cabinet,dbo.udf_Template(us.TemplateId) as Template,itm.ifilename as [File Name], [RIM Number],[TIN Number],[Account Number], case when UplaodDocument<>0 then 'Document Uploaded' when ViewDocument<>0 then 'Document Viewed' when CommentsId <>0 then 'Document Commented' when us.CheckOut<>0 then 'Document Checked Out' when linkid<>0 then 'Document Linked' when AlertDocument<>0 then 'Document Alerted' when IndexingChange<>0 then 'Document Indexing Value Changed' when Deleted<>0 then 'Document Deleted' when bookmarks<>0 then 'Document Bookmarked' when email<>0 then 'Document Sent by Email' when checkin<>0 then 'Document Checked In' when PrintDoc<>0 then 'Document Printed' else '' end as [Action],dbo.udf_LoginName(us.ECMLoginId) as [Action By],us.CreatedOn as [Acted On],'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from ezusersession us left join eZCA_3_9_items itm on us.itemid=itm.itemid where us.TemplateId='9' and  us.TemplateId=9 and ifilename<>'' order by itm.itemid"
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        FinalQuery = "SELECT * FROM (select itm.itemid,itm.templateid,dbo.udf_CabinetByTemplateId(us.TemplateId) as Cabinet,dbo.udf_Template(us.TemplateId) as Template,itm.ifilename as [File Name], [RIM Number],[TIN Number],[Account Number], case when UplaodDocument<>0 then 'Document Uploaded' when ViewDocument<>0 then 'Document Viewed' when CommentsId <>0 then 'Document Commented' when us.CheckOut<>0 then 'Document Checked Out' when linkid<>0 then 'Document Linked' when AlertDocument<>0 then 'Document Alerted' when IndexingChange<>0 then 'Document Indexing Value Changed' when Deleted<>0 then 'Document Deleted' when bookmarks<>0 then 'Document Bookmarked' when email<>0 then 'Document Sent by Email' when checkin<>0 then 'Document Checked In' when PrintDoc<>0 then 'Document Printed' else '' end as [Action],dbo.udf_LoginName(us.ECMLoginId) as [Action By],us.CreatedOn as [Acted On],'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow, ROW_NUMBER() OVER (ORDER BY itm.itemid) AS Seq from ezusersession us left join eZCA_3_9_items itm on us.itemid=itm.itemid where us.TemplateId='9' and  us.TemplateId=9 and ifilename<>'')t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                    Else
                        FinalQuery = "SELECT * FROM (select itm.itemid,itm.templateid,dbo.udf_CabinetByTemplateId(us.TemplateId) as Cabinet,dbo.udf_Template(us.TemplateId) as Template,itm.ifilename as [File Name], [RIM Number],[TIN Number],[Account Number], case when UplaodDocument<>0 then 'Document Uploaded' when ViewDocument<>0 then 'Document Viewed' when CommentsId <>0 then 'Document Commented' when us.CheckOut<>0 then 'Document Checked Out' when linkid<>0 then 'Document Linked' when AlertDocument<>0 then 'Document Alerted' when IndexingChange<>0 then 'Document Indexing Value Changed' when Deleted<>0 then 'Document Deleted' when bookmarks<>0 then 'Document Bookmarked' when email<>0 then 'Document Sent by Email' when checkin<>0 then 'Document Checked In' when PrintDoc<>0 then 'Document Printed' else '' end as [Action],dbo.udf_LoginName(us.ECMLoginId) as [Action By],us.CreatedOn as [Acted On],'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow, ROW_NUMBER() OVER (ORDER BY itm.itemid) AS Seq from ezusersession us left join eZCA_3_9_items itm on us.itemid=itm.itemid where us.TemplateId='9' and  us.TemplateId=9 and ifilename<>'')t"
                    End If
                End If
            ElseIf Flag = 3 Then
                Dim ItemListCount = GetDatasetByQuery("select count(1)  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid)")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("select sum(itm.nopages)  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid)")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid) order by itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid) order by itemid"
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        FinalQuery = "SELECT * FROM (select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments],'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itm.itemid) AS Seq  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid))t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                    Else
                        FinalQuery = "SELECT * FROM (select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow, ROW_NUMBER() OVER (ORDER BY itm.itemid) AS Seq  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid))t"
                    End If
                End If
            End If

            Dim fields As DataSet
            If templateid <> "" Then
                Dim FieldQuery = "Select * from eZTemplateField where templateid=  " + templateid
                fields = GetDatasetByQuery(FieldQuery)
            End If

            Dim ItemList = GetDatasetByQuery(FinalQuery)

            Dim res = {ItemList, fields}

            Return res


        End Function

        <HttpPost>
        Function GetItemsFieldSearchReport(ByVal Para As SearchRegistries) As DataSet

            Dim CondtionReg As String = ""
            Dim Tablename = ""
            Dim templateid = ""
            Dim ECMLoginId = ""
            Dim Flag = 0
            For Each cond In Para.Criteria
                If cond.Criteria = "without Deposit" Then
                    Flag = 4
                    CondtionReg = CondtionReg + " and  [Document Type] <> 'Deposit' and [RIM Number] not in (select distinct [RIM Number] from eZCA_3_9_items where ifilename<>'' and [Document Type] ='Deposit') "
                ElseIf cond.Criteria = "Account Number Report" Then
                    CondtionReg = CondtionReg + " and  [Account Number] <> '' "
                ElseIf cond.Criteria = "PRODUCTIVITY REPORT" Then
                    Flag = 1
                ElseIf cond.Criteria = "QUALITY CHECK CHANGE REPORT" Then
                    Flag = 3
                ElseIf cond.Criteria = "PRODUCTIVITY REPORT Unique" Then
                    Flag = 5
                ElseIf cond.Criteria = "Document Status" Then
                    Flag = 2
                    CondtionReg = CondtionReg + " and  [Checkout] = '" + cond.Value1 + "' "
                Else
                    If cond.Criteria.ToLower() = "templateid" Then
                        If cond.Value1 <> "" Then
                            Tablename = GetTableName(cond.Value1)
                            templateid = cond.Value1
                        End If
                    ElseIf cond.Criteria.ToLower() = "ecmloginid" Then
                        If cond.Value1 <> "" Then
                            ' Tablename = GetTableName(cond.Value1)
                            ECMLoginId = cond.Value1
                        End If
                    Else
                        If cond.Criteria.ToLower() = "createdby" Then
                            If cond.Value1 <> "" AndAlso Not cond.Value1.Contains(",") Then
                                cond.Value1 = GetLoginIdByUsername(cond.Value1)
                            End If
                        End If
                        If cond.DataTypeId = "2" Then
                            If cond.Value1.Contains(",") Then
                                Dim Inval = ""
                                Dim values = cond.Value1.ToString.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                                For j As Integer = 0 To values.Count - 1
                                    If cond.Criteria.ToLower() = "createdby" Then
                                        If values(j) <> "" Then
                                            values(j) = GetLoginIdByUsername(values(j))
                                        End If
                                    End If

                                    If j = 0 Then
                                        Inval = "'" + values(j) + "'"
                                    Else
                                        Inval = Inval + ",'" + values(j) + "'"
                                    End If

                                Next

                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in (" + Inval + ") "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] = '" + cond.Value1 + "'"
                            End If


                        ElseIf cond.DataTypeId = "4" Then
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.DataTypeId = "5" Then
                            If cond.Criteria = "ic.createdon" Then
                                If cond.Value1 <> "" And cond.Value2 <> "" Then
                                    If cond.Value1 = cond.Value2 Then
                                        CondtionReg = CondtionReg + " and " + cond.Criteria + " <> '' and  convert(datetime," + cond.Criteria + ",101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                    Else

                                        CondtionReg = CondtionReg + " and " + cond.Criteria + " <> '' and  convert(datetime," + cond.Criteria + ",101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                    End If

                                ElseIf cond.Value1 <> "" Then
                                    CondtionReg = CondtionReg + " and " + cond.Criteria + " <> '' and convert(datetime," + cond.Criteria + ",101) >= convert(datetime,'" + cond.Value1 + "',101) "
                                ElseIf cond.Value2 <> "" Then
                                    CondtionReg = CondtionReg + "  and convert(datetime," + cond.Criteria + ",101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                                End If
                            Else
                                If cond.Value1 <> "" And cond.Value2 <> "" Then
                                    If cond.Value1 = cond.Value2 Then
                                        CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                    Else

                                        CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                    End If

                                ElseIf cond.Value1 <> "" Then
                                    CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and convert(datetime,[" + cond.Criteria + "],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                                ElseIf cond.Value2 <> "" Then
                                    CondtionReg = CondtionReg + "  and convert(datetime,[" + cond.Criteria + "],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                                End If
                            End If


                        End If
                    End If
                End If
            Next

            'Dim docseccond = ""
            'docseccond = FieldLevelSecurity(templateid, ECMLoginId)
            'CondtionReg += docseccond


            Dim FinalQuery = ""
            If Flag = 0 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg)
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg)
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                'ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM NUMBER] from " + Tablename + " where isdeleted=0  " + CondtionReg + ") as x")
                'Dim RIMCount As String = ""
                'If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                '    RIMCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                'End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & " order by itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg & " order by itemid "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                        FinalQuery = "SELECT * FROM (Select *  ,dbo.udf_LoginName (CreatedBy) As LoginName,dbo.udf_LoginName (UpdatedBy) As [Last UpdatedBy] ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                        ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    Else
                        FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) As LoginName ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg
                    End If
                End If
            ElseIf Flag = 4 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg + ") as x")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg)
                Dim PageCount As String = ""
                'If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                '    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                'End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & " order by [RIM Number]  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg & " order by [RIM Number] "
                    End If
                Else
                    If Para.RowCount <> 0 Then

                        FinalQuery = "SELECT * FROM (Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY [RIM Number]) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                    Else
                        FinalQuery = "Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg
                    End If
                End If

            ElseIf Flag = 1 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> '' " + CondtionReg & " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy order by convert(date,max(substring(createdon,0,12)),106)  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> ''  " + CondtionReg & " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy order by convert(date,max(substring(createdon,0,12)),106) "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                        FinalQuery = "SELECT * FROM (Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString() + " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                        ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg + +" group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                    End If
                End If
            ElseIf Flag = 2 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> '' " + CondtionReg & " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy order by convert(date,max(substring(createdon,0,12)),106)  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> ''  " + CondtionReg & " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy order by convert(date,max(substring(createdon,0,12)),106) "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                        FinalQuery = "SELECT * FROM (Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString() + " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                        ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg + +" group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                    End If
                End If
            ElseIf Flag = 5 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + " group by [RIM Number],CheckOut) as x")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut) as x")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 convert(date,substring(createdon,0,12),106) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> '' " + CondtionReg & " group by [RIM Number],CheckOut order by [Created Date]  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 convert(date,substring(createdon,0,12),106) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User],'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> ''  " + CondtionReg & " group by [RIM Number],CheckOut order by [Created Date] "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                        FinalQuery = "SELECT * FROM (Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 substring(createdon,0,12) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString() + " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                        ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 substring(createdon,0,12) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg + +" group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                    End If
                End If
            ElseIf Flag = 3 Then
                Dim ItemListCount = GetDatasetByQuery("select count(1)  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid and FieldId=59 order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid)")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("select sum(itm.nopages)  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid and FieldId=59 order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid)")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid and FieldId=59 order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid) order by itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid and FieldId=59 order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid) order by itemid"
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        FinalQuery = "SELECT * FROM (select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments],'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itm.itemid) AS Seq  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid))t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                    Else
                        FinalQuery = "SELECT * FROM (select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow, ROW_NUMBER() OVER (ORDER BY itm.itemid) AS Seq  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid))t"
                    End If
                End If

            End If



            Dim ItemList = GetDatasetByQuery(FinalQuery)
            If Not IsNothing(ItemList) AndAlso ItemList.Tables.Count > 0 AndAlso ItemList.Tables(0).Rows.Count > 0 Then
                Return ItemList
            Else
                Return Nothing
            End If


        End Function

        <HttpPost>
        Function SendMailOfFieldSearchReport(ByVal Para As SearchRegistries) As String

            Dim CondtionReg As String = ""
            Dim Tablename = ""
            Dim templateid = ""
            Dim ECMLoginId = ""
            Dim Flag = 0
            For Each cond In Para.Criteria
                If cond.Criteria = "without Deposit" Then
                    Flag = 4
                    CondtionReg = CondtionReg + " and  [Document Type] <> 'Deposit' and [RIM Number] not in (select distinct [RIM Number] from eZCA_3_9_items where ifilename<>'' and [Document Type] ='Deposit') "
                ElseIf cond.Criteria = "Account Number Report" Then
                    CondtionReg = CondtionReg + " and  [Account Number] <> '' "
                ElseIf cond.Criteria = "PRODUCTIVITY REPORT" Then
                    Flag = 1
                ElseIf cond.Criteria = "QUALITY CHECK CHANGE REPORT" Then
                    Flag = 3
                ElseIf cond.Criteria = "PRODUCTIVITY REPORT Unique" Then
                    Flag = 5
                ElseIf cond.Criteria = "Document Status" Then
                    Flag = 2
                    CondtionReg = CondtionReg + " and  [Checkout] = '" + cond.Value1 + "' "
                Else
                    If cond.Criteria.ToLower() = "templateid" Then
                        If cond.Value1 <> "" Then
                            Tablename = GetTableName(cond.Value1)
                            templateid = cond.Value1
                        End If
                    ElseIf cond.Criteria.ToLower() = "ecmloginid" Then
                        If cond.Value1 <> "" Then
                            ' Tablename = GetTableName(cond.Value1)
                            ECMLoginId = cond.Value1
                        End If
                    Else
                        If cond.Criteria.ToLower() = "createdby" Then
                            If cond.Value1 <> "" AndAlso Not cond.Value1.Contains(",") Then
                                cond.Value1 = GetLoginIdByUsername(cond.Value1)
                            End If
                        End If
                        If cond.DataTypeId = "2" Then
                            If cond.Value1.Contains(",") Then
                                Dim Inval = ""
                                Dim values = cond.Value1.ToString.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                                For j As Integer = 0 To values.Count - 1
                                    If cond.Criteria.ToLower() = "createdby" Then
                                        If values(j) <> "" Then
                                            values(j) = GetLoginIdByUsername(values(j))
                                        End If
                                    End If

                                    If j = 0 Then
                                        Inval = "'" + values(j) + "'"
                                    Else
                                        Inval = Inval + ",'" + values(j) + "'"
                                    End If

                                Next

                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] in (" + Inval + ") "
                            Else
                                CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] = '" + cond.Value1 + "'"
                            End If


                        ElseIf cond.DataTypeId = "4" Then
                            CondtionReg = CondtionReg + " and  [" + cond.Criteria + "] LIKE '%" + cond.Value1 + "%'"
                        ElseIf cond.DataTypeId = "5" Then
                            If cond.Criteria = "ic.createdon" Then
                                If cond.Value1 <> "" And cond.Value2 <> "" Then
                                    If cond.Value1 = cond.Value2 Then
                                        CondtionReg = CondtionReg + " and " + cond.Criteria + " <> '' and  convert(datetime," + cond.Criteria + ",101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                    Else

                                        CondtionReg = CondtionReg + " and " + cond.Criteria + " <> '' and  convert(datetime," + cond.Criteria + ",101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                    End If

                                ElseIf cond.Value1 <> "" Then
                                    CondtionReg = CondtionReg + " and " + cond.Criteria + " <> '' and convert(datetime," + cond.Criteria + ",101) >= convert(datetime,'" + cond.Value1 + "',101) "
                                ElseIf cond.Value2 <> "" Then
                                    CondtionReg = CondtionReg + "  and convert(datetime," + cond.Criteria + ",101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                                End If
                            Else
                                If cond.Value1 <> "" And cond.Value2 <> "" Then
                                    If cond.Value1 = cond.Value2 Then
                                        CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                    Else

                                        CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and  convert(datetime,[" + cond.Criteria + "],101) between convert(datetime,'" + cond.Value1 + " 00:00:00',101) and convert(datetime,'" + cond.Value2 + " 23:59:59',101)  "
                                    End If

                                ElseIf cond.Value1 <> "" Then
                                    CondtionReg = CondtionReg + " and [" + cond.Criteria + "] <> '' and convert(datetime,[" + cond.Criteria + "],101) >= convert(datetime,'" + cond.Value1 + "',101) "
                                ElseIf cond.Value2 <> "" Then
                                    CondtionReg = CondtionReg + "  and convert(datetime,[" + cond.Criteria + "],101) <= convert(datetime,'" + cond.Value2 + " 23:59:59',101) "
                                End If
                            End If


                        End If
                    End If
                End If
            Next



            Dim FinalQuery = ""
            If Flag = 0 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg)
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg)
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If

                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & " order by itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg & " order by itemid "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        FinalQuery = "SELECT * FROM (Select *  ,dbo.udf_LoginName (CreatedBy) As LoginName,dbo.udf_LoginName (UpdatedBy) As [Last UpdatedBy] ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()

                    Else
                        FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) As LoginName ,dbo.udf_LoginName (checkoutby) as checkedoutby ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg
                    End If
                End If
            ElseIf Flag = 4 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from (select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg + ") as x")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg)
                Dim PageCount As String = ""

                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & " order by [RIM Number]  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg & " order by [RIM Number] "
                    End If
                Else
                    If Para.RowCount <> 0 Then

                        FinalQuery = "SELECT * FROM (Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY [RIM Number]) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                    Else
                        FinalQuery = "Select distinct [RIM Number],[TIN Number],[RIM Type],[RIM Branch],[RIM Name] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg
                    End If
                End If

            ElseIf Flag = 1 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> '' " + CondtionReg & " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy order by convert(date,max(substring(createdon,0,12)),106)  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> ''  " + CondtionReg & " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy order by convert(date,max(substring(createdon,0,12)),106) "
                    End If
                Else
                    If Para.RowCount <> 0 Then

                        FinalQuery = "SELECT * FROM (Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString() + " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"

                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg + +" group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                    End If
                End If
            ElseIf Flag = 2 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy) as x")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> '' " + CondtionReg & " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy order by convert(date,max(substring(createdon,0,12)),106)  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> ''  " + CondtionReg & " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy order by convert(date,max(substring(createdon,0,12)),106) "
                    End If
                Else
                    If Para.RowCount <> 0 Then

                        FinalQuery = "SELECT * FROM (Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString() + " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"

                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],max(substring(createdon,0,12)) as [Created Date],dbo.udf_LoginName(itm.createdby) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg + +" group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                    End If
                End If
            ElseIf Flag = 5 Then
                Dim ItemListCount = GetDatasetByQuery("Select Count(1) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + " group by [RIM Number],CheckOut) as x")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("Select SUM(nopages) from (Select [Rim Number] from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg + "   group by [RIM Number],CheckOut) as x")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 convert(date,substring(createdon,0,12),106) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> '' " + CondtionReg & " group by [RIM Number],CheckOut order by [Created Date]  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 convert(date,substring(createdon,0,12),106) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User],'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " itm where isdeleted=0 and ifilename <> ''  " + CondtionReg & " group by [RIM Number],CheckOut order by [Created Date] "
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        '   FinalQuery = "Select *  ,dbo.udf_LoginName (CreatedBy) as LoginName ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from " + Tablename + " where isdeleted=0  " + CondtionReg & " and Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                        FinalQuery = "SELECT * FROM (Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 substring(createdon,0,12) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itemid) AS Seq from " + Tablename + " where isdeleted=0 and ifilename <> '' " + CondtionReg & ")t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString() + " group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                        ' strQry = "SELECT Transactionid FROM (" + strQry.Replace("select Transactionid", "select Transactionid, ROW_NUMBER() OVER (ORDER BY Transactionid) AS Seq") + ")t" + " WHERE Seq BETWEEN " + RowFrom.ToString() + " AND " + RowCount.ToString()
                    Else
                        FinalQuery = "Select [RIM Number],case when CheckOut='' then 'CheckIn' else checkout end as [Status],Count(1) as [File Count],sum(nopages) as [Page Count],(select top 1 substring(createdon,0,12) from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created Date],dbo.udf_LoginName((select top 1 createdby from eZCA_3_9_items where [RIM Number]=itm.[RIM Number] order by itemid)) as [Created User] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow  from " + Tablename + " where isdeleted=0 and ifilename <> ''  " + CondtionReg + +" group by [RIM Number],CheckOut,convert(date,createdon,106),CreatedBy"
                    End If
                End If
            ElseIf Flag = 3 Then
                Dim ItemListCount = GetDatasetByQuery("select count(1)  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid and FieldId=59 order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid)")
                Dim TotalRow As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    TotalRow = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                ItemListCount = GetDatasetByQuery("select sum(itm.nopages)  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid and FieldId=59 order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid)")
                Dim PageCount As String = ""
                If Not IsNothing(ItemListCount) AndAlso ItemListCount.Tables.Count > 0 AndAlso ItemListCount.Tables(0).Rows.Count > 0 Then
                    PageCount = ItemListCount.Tables(0).Rows(0)(0).ToString()
                End If
                If ConfigurationManager.AppSettings("SqlAbouve2012") = "true" Then
                    If Para.RowCount <> 0 Then
                        FinalQuery = "select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid and FieldId=59 order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid) order by itemid  OFFSET " + Para.RowFrom.ToString() + " ROWS FETCH NEXT " + Para.RowCount.ToString() + " ROWS ONLY"
                    Else
                        FinalQuery = "select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid and FieldId=59 order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid) order by itemid"
                    End If
                Else
                    If Para.RowCount <> 0 Then
                        FinalQuery = "SELECT * FROM (select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments],'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow , ROW_NUMBER() OVER (ORDER BY itm.itemid) AS Seq  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid))t" + " WHERE Seq BETWEEN " + Para.RowFrom.ToString() + " AND " + Para.RowCount.ToString()
                    Else
                        FinalQuery = "SELECT * FROM (select itm.itemid,itm.templateid,ifilename as [File Name],[RIM Number],[TIN Number],[Account Number],oldvalue as [ExistingValue_Quality Check],dbo.udf_LoginName(itm.CreatedBy) as [Archived By],itm.CreatedOn as [Archived On],newvalue as [NewValue_Quality Check],ic.[CreatedOn] as [Last Acted On],dbo.udf_LoginName(ic.Createdby) as [Action By], (select top 1 Comments from ezcomments where itemid=itm.itemid and createdon=ic.createdon order by commentsid desc) as [Comments] ,'" + PageCount + "' as PageCount, '" + TotalRow + "' as TotalRow, ROW_NUMBER() OVER (ORDER BY itm.itemid) AS Seq  from ezindexingchange ic left join ezca_3_9_items itm on itm.TemplateId=ic.Templateid and itm.itemid=ic.itemid where itm.isdeleted=0 and itm.ifilename<>'' " + CondtionReg & " and Indexingchangeid in (select (select top 1 Indexingchangeid from eZIndexingChange where itemid=ic.itemid order by Indexingchangeid desc) from eZIndexingChange ic group by Templateid,FieldId,itemid))t"
                    End If
                End If

            End If



            Dim ItemList = GetDatasetByQuery(FinalQuery)
            If Not IsNothing(ItemList) AndAlso ItemList.Tables.Count > 0 AndAlso ItemList.Tables(0).Rows.Count > 0 Then
                Dim Obj As New MailPara
                Obj.DSdata = ItemList
                Obj.Email = "arasu@ezofis.com"

                '  Dim evaluator As New Thread(Sub()
                ArchivedSendmail(Obj)
                '   End Sub)
                'Dim processthr() As Thread
                'Array.Resize(processthr, 1)
                'processthr(0) = New Thread(New ParameterizedThreadStart(AddressOf ArchivedSendmail))


                Return "Mail sent Successfully"
            Else
                Return Nothing
            End If


        End Function
        Public Function Attachdir() As String
            Dim source As String = ""
            Try
                Dim apppath As String = ""
                apppath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath()
                apppath = Path.GetDirectoryName(apppath)
                source = apppath + "\Attachment\Report"
                If Not Directory.Exists(source) Then   'Checking Directory Exist or Not
                    Directory.CreateDirectory(source)
                End If
            Catch ex As Exception

            End Try

            Return source
        End Function
        Public Function ArchivedSendmail(Para As MailPara)
            Dim res = ""
            Try
                Dim MailType As String = "0"
                Dim Dateformat As String = "dd-MMM-yyyy_hhmmsstt"
                Dim DateformatWithTime As String = "dd-MMM-yyyy hh:mm:ss tt"
                Dim Body = "Dear Team,<span style='color: #d52a16'><b >Cabinet : Accounts</b></span> <br/> This is auto generated report for the date<b style='color: #d52a16'> " + DateTime.Now.ToString("dd MMM yyyy") + "</b> with the archived document list and its attached."
                Dim AttchFilename = "1"
                Dim Subject = "Archived Document Report"
                Dim ReportFor = "Archived Document Report"

                If Not Para.DSdata Is Nothing AndAlso Para.DSdata.Tables.Count > 0 AndAlso Para.DSdata.Tables(0).Rows.Count > 0 Then
                    Dim StrDate = Today.ToString(Dateformat)
                    Dim AttDir = Attachdir()
                    Dim Attachsavepath = AttDir + "\" + "Archived Document Report_" + StrDate + ".csv" '+ ".xlsx"

                    Dim AttachFilename = "Archived Document Report_" + StrDate + ".csv"

                    Dim Exportres = csvexport.SaveAsCSV(Attachsavepath, Para.DSdata, ReportFor)
                    Dim Createdon = DateTime.Now.ToString(DateformatWithTime)
                    If Exportres = "Success" Then
                        Try
                            ZipFile.CreateFromDirectory(AttDir + "\", AttDir.Replace("\Report", "") + "\" + "Archived Document Report_" + StrDate + ".zip")
                        Catch ex As Exception
                            '  writetxtfle("ZipFile :" + ex.Message.ToString())
                        End Try


                        Attachsavepath = AttDir.Replace("\Report", "") + "\" + "Archived Document Report_" + StrDate + ".zip"

                        Dim query = "insert into ezmail (mailsettingid,toadd,subject,bodyhtmltypeid,body,attachmentspaths,mailstatus,createdon,updatedon,createdby,updatedby,isdeleted) values(1,'" + Para.Email + "','" + Subject + "',2,'" + Body.Replace("'", "''") + "','" + Attachsavepath.Replace("'", "''") + "',0,'" + Createdon + "','',1,0,0);"
                        res = InsertAndUpdateAndDeleteeZUserDefined(query)
                        'writetxtfle("Mail sent Successfully")
                    End If

                End If


            Catch ex As Exception
                ' writetxtfle("ArchivedSendmail :" + ex.Message.ToString())
            End Try
        End Function

    End Class
End Namespace