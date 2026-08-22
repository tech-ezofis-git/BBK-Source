Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBLayer

#Region "User ECMControls"
    Public Function CreateECMControl(objEmp As eZECMControl) As IeZECMControl
        Dim newObject As IeZECMControl = Nothing
        If String.IsNullOrEmpty(objEmp.ECMControl) Then
            Return Nothing
        End If
        objEmp.ECMControl = objEmp.ECMControl.Trim()
        Try
            Dim strQry As String = ""
            Dim objParam As SqlParameter()
            Dim param As SqlParameter
            strQry = "Select ECMControlId From eZECMControl Where ECMControl = @ECMControl and ECMControlType=@ECMControlType And Isdeleted=0"
            objParam = New SqlParameter(1) {}
            param = New SqlParameter("@ECMControl", objEmp.ECMControl)
            objParam(0) = param
            param = New SqlParameter("@ECMControlType", objEmp.ECMControlType)
            objParam(1) = param
            Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj IsNot Nothing Then
                Throw New Exception("ECMControl Code already exist!")
            End If
            strQry = "INSERT INTO eZECMControl(ECMControl,ECMControlType,CreatedOn,CreatedBy) VALUES(@ECMControl,@ECMControlType,@CreatedOn,@CreatedBy);Select SCOPE_IDENTITY();"
            objParam = New SqlParameter(3) {}
            param = New SqlParameter("@ECMControl", objEmp.ECMControl)
            objParam(0) = param
            param = New SqlParameter("@ECMControlType", objEmp.ECMControlType)
            objParam(1) = param
            param = New SqlParameter("@CreatedOn", objEmp.CreatedOn)
            objParam(2) = param
            param = New SqlParameter("@CreatedBy", objEmp.CreatedBy)
            objParam(3) = param
            obj = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Return Nothing
            End If
            ' here need to set a robust process where we can extract integer value from object.
            newObject = GlobalInstance.eZECMControl(Convert.ToInt32(obj))
            Read(newObject)
            Return newObject
        Catch e As Exception
            Throw New Exception(e.Message)
            Return Nothing
        End Try
    End Function
    Public Sub Read(objRead As IeZECMControl)
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
            strQry = "Select ez.*,ezlg.loginname as UpdatedBy1,ezl.loginname as CreatedBy1 From eZECMControl ez " +
                "left join ezecmlogin ezl on ez.createdby=ezl.ecmloginid left join ezecmlogin ezlg on ez.updatedby=ezlg.ecmloginid " +
                " Where ez.ECMControlId=@ECMControl_ID and ez.Isdeleted=0"
            param = New SqlParameter("@ECMControl_ID", objRead.ECMControlId)
            objParam(0) = param
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMControl.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            If sqlRdr.Read() Then
                objRead.ECMControlId = GetInteger(sqlRdr("ECMControlId"))
                objRead.ECMControlType = GetInteger(sqlRdr("ECMControlType"))
                objRead.ECMControl = sqlRdr("ECMControl").ToString()
                objRead.CreatedBy = GetInteger(sqlRdr("CreatedBy"))
                objRead.CreatedOn = sqlRdr("CreatedOn").ToString
                objRead.UpdatedBy = GetInteger(sqlRdr("UpdatedBy"))
                objRead.UpdatedOn = sqlRdr("UpdatedOn").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
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
    Public Function ReadAllECMControl() As System.Collections.Generic.List(Of IeZECMControl)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMControl)()
        Dim objItem As IeZECMControl
        Try
            Dim strQry As String = ""
            strQry = "Select ECMControlId From eZECMControl where Isdeleted=0 order by ECMControl"
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid ECMControl.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMControl(GetInteger(sqlRdr("ECMControlId")))
                objItem.ECMControlId = GetInteger(sqlRdr("ECMControlId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()

            End If
        End Try
    End Function
    Public Sub Update(objToUpdate As IeZECMControl)
        If Not objToUpdate.IsModified Then
            Return
        End If
        If Not objToUpdate.IsReadFromDB Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Select ECMControlId From eZECMControl Where ECMControl = @ECMControl and ECMControlType=@ECMControlType and ECMControlId <> @ECMControlId and Isdeleted=0"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@ECMControl", objToUpdate.ECMControl)
        objParam(0) = param
        param = New SqlParameter("@ECMControlType", objToUpdate.ECMControlType)
        objParam(1) = param
        param = New SqlParameter("@ECMControlId", objToUpdate.ECMControlId)
        objParam(2) = param
        Dim obj As Object = SqlHelper.ExecuteScalar(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
        If obj IsNot Nothing Then
            Throw New Exception("ECMControl Code already exist!")
        Else
            strQry = "Update eZECMControl Set ECMControl=@ECMControl,ECMControlType=@ECMControlType,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where ECMControlId=@ECMControl_ID"
            objParam = New SqlParameter(4) {}
            param = New SqlParameter("@ECMControl", objToUpdate.ECMControl)
            objParam(0) = param
            param = New SqlParameter("@ECMControlType", objToUpdate.ECMControlType)
            objParam(1) = param
            param = New SqlParameter("@UpdatedBy", objToUpdate.UpdatedBy)
            objParam(2) = param
            param = New SqlParameter("@UpdatedOn", objToUpdate.UpdatedOn)
            objParam(3) = param
            param = New SqlParameter("@ECMControl_ID", objToUpdate.ECMControlId)
            objParam(4) = param
           
            If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
                Throw New Exception("Record Not updated due to some error")
            End If
        End If
        objToUpdate.IsModified = False
    End Sub
    Public Sub Delete(objToDelete As IeZECMControl)
        If objToDelete Is Nothing Then
            Return
        End If
        Dim strQry As String = ""
        Dim objParam As SqlParameter()
        Dim param As SqlParameter
        strQry = "Update ECMControl set Isdeleted=1,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn where ECMControlId=@ECMControl_ID"
        objParam = New SqlParameter(2) {}
        param = New SqlParameter("@UpdatedBy", objToDelete.UpdatedBy)
        objParam(0) = param
        param = New SqlParameter("@UpdatedOn", objToDelete.UpdatedOn)
        objParam(1) = param
        param = New SqlParameter("@ECMControl_ID", objToDelete.ECMControlId)
        objParam(2) = param
        If SqlHelper.ExecuteNonQuery(ConnectionStr, CommandType.Text, strQry.ToString(), objParam) = 0 Then
            Throw New Exception("Record Not deleted due to some error")
        End If
    End Sub
    Public Function ReadFilteredeZECMControl(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMControl)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMControl)()
        Dim objItem As IeZECMControl

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMControlId From eZECMControl where Isdeleted=0 and "
                strQry = strQry & Criteria
                strQry = strQry & " like N'%"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "%' "
                strQry = strQry & " order by ECMControl"
            Else
                strQry = "Select ECMControlId From eZECMControl where Isdeleted=0 order by ECMControl"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMControl(GetInteger(sqlRdr("ECMControlId")))
                objItem.ECMControlId = GetInteger(sqlRdr("ECMControlId"))
                lstItems.Add(objItem)
            End While
            Return lstItems
        Finally
            If sqlRdr IsNot Nothing Then
                sqlRdr.Close()
            End If
        End Try
    End Function
    Public Function ReadSelectedeZECMControl(Criteria As String, Value As String) As System.Collections.Generic.List(Of IeZECMControl)
        Dim sqlRdr As SqlDataReader = Nothing
        Dim lstItems As New System.Collections.Generic.List(Of IeZECMControl)()
        Dim objItem As IeZECMControl

        Try
            Dim strQry As String = ""
            If Criteria <> "All" Then
                strQry = "Select ECMControlId From eZECMControl where Isdeleted=0 and " + Criteria
                'strQry = strQry & "Convert(varchar(20)," & Criteria & ") "
                strQry = strQry & " =N'"
                strQry = strQry & Unquote(Value)
                strQry = strQry & "' "
                strQry = strQry & " order by ECMControl"
            Else
                strQry = "Select ECMControlId From eZECMControl where Isdeleted=0 order by ECMControl"
            End If
            Dim obj As Object = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString())

            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Profile.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)
            While sqlRdr.Read()
                objItem = GlobalInstance.eZECMControl(GetInteger(sqlRdr("ECMControlId")))
                objItem.ECMControlId = GetInteger(sqlRdr("ECMControlId"))
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
