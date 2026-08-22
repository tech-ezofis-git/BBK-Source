Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common

Partial Public Class DBLayer

    Public Function CreateTblsForForm(ByVal FormId As Integer) As Integer
        Try
            Dim Lst1 As New List(Of IeZFormControlDetail)()
            Dim objParam As SqlParameter()
            objParam = New SqlParameter(0) {}
            Dim Field As String = ""
            Lst1 = DBLayer.DBLInstance.ReadSelectedeZFormControlDetail("FormId", FormId.ToString())
            For i As Integer = 0 To Lst1.Count - 1
                If Lst1(i).ControlTypeId = "4" Then
                    Field = Field + "[" + Lst1(i).ControlName + "] nvarchar(150) null ,"
                ElseIf Lst1(i).ControlTypeId = "5" Then
                    Field = Field + "[" + Lst1(i).ControlName + "] int null ,"
                ElseIf Lst1(i).ControlTypeId = "6" Then
                    Field = Field + "[" + Lst1(i).ControlName + "] nvarchar(1000) null ,"
                ElseIf Lst1(i).ControlTypeId = "8" Then
                    Field = Field + "[" + Lst1(i).ControlName + "] nvarchar(1000) null ,"
                ElseIf Lst1(i).ControlTypeId = "9" Then
                    Field = Field + "[" + Lst1(i).ControlName + "] int null ,"
                ElseIf Lst1(i).ControlTypeId = "11" Then
                    Field = Field + "[" + Lst1(i).ControlName + "] [bit] NULL ,"
                End If

            Next
            Dim strQry As String = "create table eZForm_" + FormId.ToString + " (itemid int IDENTITY(1,1) NOT NULL,ProcessId Int Not Null," + Field + "Df_Comments nvarchar(100) Null,Df_Attachments nvarchar(100) Null,CreatedOn nvarchar(100) NULL, UpdatedOn nvarchar(100) NULL, CreatedBy int NOT NULL,UpdatedBy int NOT NULL, Isdeleted bit NOT NULL ) "
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString())
            Return 1
        Catch ex As Exception

            Return 0
        End Try

    End Function


    Public Function CreateeZFormControlDetail(objEmp As eZFormControlDetail) As IeZFormControlDetail
        Dim newObject As IeZFormControlDetail = Nothing
        If String.IsNullOrEmpty(objEmp.ControlName) Then
            Return Nothing
        End If
        objEmp.ControlName = objEmp.ControlName.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ControlId From eZFormControlDetail Where ControlName = @ControlName And Isdeleted=0"
            objParam = New SqlParameter(0) {}
            param = New SqlParameter("@ControlName", objEmp.ControlName)
            objParam(0) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ControlName Code already exist!")
            End If
            strQry = "INSERT INTO eZFormControlDetail(ControlName,FormId,OrderId,ControlTypeId,DataType,TabIndex,ValidationId,style,TableTagType,GridRow,GridColumn,CreatedOn,CreatedBy) VALUES(@ControlName,@FormId,@OrderId,@ControlTypeId,@DataType,@TabIndex,@ValidationId,@style,@TableTagType,@GridRow,@GridColumn,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(12) {}
            param = New SqlParameter("@ControlName", objEmp.ControlName)
            objParam(0) = param
            param = New SqlParameter("@FormId", objEmp.FormId)
            objParam(1) = param
            param = New SqlParameter("@OrderId", objEmp.OrderId)
            objParam(2) = param
            param = New SqlParameter("@ControlTypeId", objEmp.ControlTypeId)
            objParam(3) = param
            param = New SqlParameter("@DataType", objEmp.DataType)
            objParam(4) = param
            param = New SqlParameter("@GridRow", objEmp.GridRow)
            objParam(5) = param
            param = New SqlParameter("@GridColumn", objEmp.GridColumn)
            objParam(6) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(7) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(8) = param
            param = New SqlParameter("@TabIndex", objEmp.TabIndex)
            objParam(9) = param
            param = New SqlParameter("@ValidationId", objEmp.ValidationId)
            objParam(10) = param
            param = New SqlParameter("@style", objEmp.style)
            objParam(11) = param
            param = New SqlParameter("@TableTagType", objEmp.TableTagType)
            objParam(12) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZFormControlDetail(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZFormControlDetail)
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
            If objRead.ControlName Is Nothing Then

                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFormControlDetail Where ControlId=@ControlName_ID and Isdeleted=0"
                param = New SqlParameter("@ControlName_ID", objRead.ControlId)
                objParam(0) = param
            Else
                objParam = New SqlParameter(1) {}
                strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZFormControlDetail Where ControlName=@ControlName and Isdeleted=0"
                param = New SqlParameter("@ControlName", objRead.ControlName)
                objParam(0) = param
            End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ControlName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.ControlId = GetInteger(sqlRdr("ControlId"))
                objRead.ControlName = sqlRdr("ControlName").ToString()
                objRead.FormId = GetInteger(sqlRdr("FormId"))
                objRead.OrderId = GetInteger(sqlRdr("OrderId"))
                objRead.ControlTypeId = GetInteger(sqlRdr("ControlTypeId"))
                objRead.ValidationId = GetDouble(sqlRdr("ValidationId"))
                objRead.TabIndex = GetInteger(sqlRdr("TabIndex"))
                objRead.DataType = GetInteger(sqlRdr("DataType"))
                objRead.style = sqlRdr("style").ToString
                objRead.TableTagType = sqlRdr("TableTagType").ToString()
                objRead.GridRow = GetInteger(sqlRdr("GridRow"))
                objRead.GridColumn = GetInteger(sqlRdr("GridColumn"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.CreatedBy = sqlRdr("CreatedBy").ToString()
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.UpdatedBy = sqlRdr("UpdatedBy").ToString()
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
    Public Function ReadeZFormControlDetail() As System.Collections.Generic.List(Of IeZFormControlDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormControlDetail)()
        Dim objItem As IeZFormControlDetail

        Try
            Dim strQry As String = ""
            strQry = "Select ControlId From eZFormControlDetail where Isdeleted=0 order by ControlName"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ControlName.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormControlDetail(GetInteger(sqlRdr("ControlId")))
                objItem.ControlId = GetInteger(sqlRdr("ControlId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function

    Public Function ReadFilteredeZFormControlDetail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFormControlDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormControlDetail)()
        Dim objItem As IeZFormControlDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ControlId From eZFormControlDetail where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like '%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by OrderId"
            Else
                strQry = "Select ControlId From eZFormControlDetail where Isdeleted=0 order by OrderId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFormControlDetail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormControlDetail(GetInteger(sqlRdr("ControlId")))
                objItem.ControlId = GetInteger(sqlRdr("ControlId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZFormControlDetail(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZFormControlDetail)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZFormControlDetail)()
        Dim objItem As IeZFormControlDetail
        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ControlId From eZFormControlDetail where Isdeleted=0 and "
                strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " ='"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by OrderId"
            Else
                strQry = "Select ControlId From eZFormControlDetail where Isdeleted=0 order by OrderId"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid eZFormControlDetail.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZFormControlDetail(GetInteger(sqlRdr("ControlId")))
                objItem.ControlId = GetInteger(sqlRdr("ControlId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function

    Public Sub Update(objToUpdate As IeZFormControlDetail)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ControlId From eZFormControlDetail Where ControlName = @ControlName and ControlId <> @ControlId and Isdeleted=0"
        objParam = New SqlParameter(1) {}
        param = New SqlParameter("@ControlName", objToUpdate.ControlName)
        objParam(0) = param
        param = New SqlParameter("@ControlId", objToUpdate.ControlId)
        objParam(1) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ControlName Code already exist!")
        Else
            strQry = "Update eZFormControlDetail Set TableTagType=@TableTagType,style=@style,TabIndex=@TabIndex,ValidationId=@ValidationId,ControlName=@ControlName,FormId=@FormId,OrderId=@OrderId,ControlTypeId=@ControlTypeId,GridRow=@GridRow,GridColumn=@GridColumn,UpdatedBy=@UpdatedBy,DataType=@DataType,UpdatedOn=@UpdatedOn where ControlId=@ControlName_ID"
            objParam = New SqlParameter(13) {}
            param = New SqlParameter("@ControlName", objToUpdate.ControlName)
            objParam(0) = param
            param = New SqlParameter("@ControlName_ID", objToUpdate.ControlId)
            objParam(1) = param
            param = New SqlParameter("@FormId", objToUpdate.FormId)
            objParam(2) = param
            param = New SqlParameter("@OrderId", objToUpdate.OrderId)
            objParam(3) = param
            param = New SqlParameter("@ControlTypeId", objToUpdate.ControlTypeId)
            objParam(4) = param
            param = New SqlParameter("@DataType", objToUpdate.DataType)
            objParam(5) = param
            param = New SqlParameter("@TabIndex", objToUpdate.TabIndex)
            objParam(6) = param
            param = New SqlParameter("@ValidationId", objToUpdate.ValidationId)
            objParam(7) = param
            param = New SqlParameter("@GridRow", objToUpdate.GridRow)
            objParam(8) = param
            param = New SqlParameter("@GridColumn", objToUpdate.GridColumn)
            objParam(9) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(10) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(11) = param
            param = New SqlParameter("@style", objToUpdate.style)
            objParam(12) = param
            param = New SqlParameter("@TableTagType", objToUpdate.TableTagType)
            objParam(13) = param
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    'udaya
    Public Function ReadOrderid(ByVal rowcount As Integer, ByVal Columncount As Integer, ByVal row As Integer, ByVal column As Integer) As Integer
        Dim lstorderid As New List(Of Integer)
        Dim orderid As Integer
        Try
            For i As Integer = 0 To rowcount - 1 Step 1
                For j As Integer = 0 To Columncount - 1 Step 1
                    lstorderid.Add(j)
                    If i = row And j = column Then
                        orderid = lstorderid.Count + 1
                        GoTo EndFor
                    End If
                Next
            Next
EndFor:
            Return orderid
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    'udaya
    Public Function ReadTableTagIndex(ByVal rowcount As Integer, ByVal Columncount As Integer) As List(Of Integer)
        Dim lsttagindex As New List(Of Integer)
        Dim tabletagindex As Integer
        Try
            For i As Integer = 0 To rowcount - 1 Step 1
                For j As Integer = 0 To Columncount - 1 Step 1

                    If i = 0 And j = 0 Then
                        tabletagindex = 127
                    ElseIf i <> 0 And j = 0 Then
                        tabletagindex = 27
                    ElseIf i = rowcount - 1 And j = Columncount - 1 Then
                        tabletagindex = 756
                    ElseIf j <> Columncount - 1 Then
                        tabletagindex = 7
                    ElseIf j = Columncount - 1 Then
                        tabletagindex = 75
                    End If
                    lsttagindex.Add(tabletagindex)
                Next

            Next

            Return lsttagindex
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Sub Delete(objToDelete As IeZFormControlDetail)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ControlName set Isdeleted=1 where ControlId=@ControlName_ID"
        objParam = New SqlParameter(0) {}
        param = New SqlParameter("@ControlName_ID", objToDelete.ControlId)
        objParam(0) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
End Class