
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports ECMAPI.DBLibrary
Imports System.IO
Imports System.Collections.Generic
Imports System.Data.Common
Partial Public Class DBlayer
    Public Sub Read(objRead As IeZLicenseMaintenance)
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
            'If objRead.AlertConditionId = 0 Then

            strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZLicenseMaintenance Where Maintenance_Id=@Maintenance_Id and Isdeleted=0"
            param = New SqlParameter("@Maintenance_Id", objRead.Maintenance_Id)
            objParam(0) = param
            'Else
            '    objParam = New SqlParameter(1) {}
            '    strQry = "Select *,dbo.udf_UserName(UpdatedBy) as UpdatedBy1,dbo.udf_UserName(CreatedBy) as CreatedBy1 From eZAlert Where AlertConditionId=@AlertConditionId and Isdeleted=0"
            '    param = New SqlParameter("@AlertConditionId", objRead.AlertConditionId)
            '    objParam(0) = param
            'End If
            Dim obj As Object = ""
            obj = SqlHelper.ExecuteReader(ConnectionStr, CommandType.Text, strQry.ToString(), objParam)
            If obj Is Nothing Then
                Throw New Exception("Attempt to read Invalid Maintenance_Id.")
            End If
            sqlRdr = DirectCast(obj, SqlDataReader)

            If sqlRdr.Read() Then
                objRead.Maintenance_Id = GetInteger(sqlRdr("Maintenance_Id"))
                objRead.Client_Name = sqlRdr("Client_Name").ToString()
                objRead.License_Key = sqlRdr("License_Key").ToString()
                objRead.Keytype = sqlRdr("Keytype").ToString()
                objRead.Created_On = sqlRdr("Created_On").ToString
                objRead.CreatedBy1 = sqlRdr("CreatedBy1").ToString()
                objRead.Created_by = sqlRdr("Created_by").ToString()
                objRead.Updated_On = sqlRdr("Updated_On").ToString()
                objRead.UpdatedBy1 = sqlRdr("UpdatedBy1").ToString()
                objRead.Updated_by = sqlRdr("Updated_by").ToString()
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

End Class
