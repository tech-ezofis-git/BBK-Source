Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI
Imports System.IO
Imports ECMAPI.ParaVariables

Public Class eZWorkflowDetails
    Inherits IDatabaseCommonItems
    Implements IeZWorkflowDetails

    Protected D_Workflowid As Integer
    Protected D_Workflowitemid As Integer
    Protected D_Status As String
    Protected D_Createdon As String
    Protected D_Updatedon As String
    Protected D_Createdby As Integer
    Protected D_Updatedby As Integer
    Private D_isdeleted As Integer
    Protected D_Createdby1 As String = ""
    Protected D_Updatedby1 As String = ""
    Protected D_MailSettingsId As Integer
    Protected D_WorkflowName As String = ""
    Protected D_TemplateId As String
    Protected D_FormId As String
    Protected D_XMLDS As String
    Protected D_ItemTableName As String
    Protected D_FormTableName As String
    Protected D_flownodes As List(Of FlowNodes)


    Public Sub New()
    End Sub
    Public Sub New(Workflowid As Integer)
        Me.D_Workflowid = Workflowid
    End Sub
    Public Property Createdby As Integer Implements IeZWorkflowDetails.Createdby
        Get
            DBLayer.DBLInstance.Read(Me)

            Return D_Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdby = value Then
                Return
            End If
            D_Createdby = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon As String Implements IeZWorkflowDetails.Createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdon = value Then
                Return
            End If
            D_Createdon = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements IeZWorkflowDetails.isdeleted
        Get
            Return D_isdeleted
        End Get
    End Property

    Public Property Status As String Implements IeZWorkflowDetails.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Status
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Status = value Then
                Return
            End If
            D_Status = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedby As Integer Implements IeZWorkflowDetails.Updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Updatedby = value Then
                Return
            End If
            D_Updatedby = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedon As String Implements IeZWorkflowDetails.Updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Updatedon = value Then
                Return
            End If
            D_Updatedon = value
            IsModified = True
        End Set
    End Property

    Public Property Workflowid As Integer Implements IeZWorkflowDetails.Workflowid
        Get
            If D_Workflowid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return D_Workflowid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_Workflowid <> 0 AndAlso D_Workflowid <> value Then
                Throw New MemberAccessException()
            End If
            D_Workflowid = value
        End Set
    End Property
    Public Property Workflowitemid As Integer Implements IeZWorkflowDetails.Workflowitemid
        Get
            If D_Workflowitemid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return D_Workflowitemid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_Workflowitemid <> 0 AndAlso D_Workflowitemid <> value Then
                Throw New MemberAccessException()
            End If
            D_Workflowitemid = value
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZWorkflowDetails.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Createdby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdby1 = value Then
                Return
            End If
            D_Createdby1 = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1 As String Implements IeZWorkflowDetails.UpdatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Updatedby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Updatedby1 = value Then
                Return
            End If
            D_Updatedby1 = value
            IsModified = True
        End Set
    End Property

    Public Property MailSettingsId As Integer Implements IeZWorkflowDetails.MailSettingsId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_MailSettingsId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_MailSettingsId = value Then
                Return
            End If
            D_MailSettingsId = value
            IsModified = True
        End Set
    End Property

    Public Property WorkflowName As String Implements IeZWorkflowDetails.WorkflowName
        Get
            DBLayer.DBLInstance.Read(Me)

            Return D_WorkflowName

        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_WorkflowName = value Then
                Return
            End If
            D_WorkflowName = value
            IsModified = True
        End Set
    End Property



    Public Property XMLDS As String Implements IeZWorkflowDetails.XMLDS
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_XMLDS
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_XMLDS = value Then
                Return
            End If
            D_XMLDS = value
            IsModified = False
        End Set
    End Property

    Public ReadOnly Property TemplateId As String Implements IeZWorkflowDetails.TemplateId
        Get
            If D_XMLDS <> "" Then
                Dim _TemplateId As String = ""
                Try
                    Dim xmlds As New DataSet
                    xmlds.ReadXml(New StringReader(D_XMLDS))
                    _TemplateId = xmlds.Tables("Activity").Rows(0)("Template").ToString()
                Catch ex As Exception
                    _TemplateId = ""
                End Try
                Return _TemplateId
            Else
                Return ""
            End If
        End Get

    End Property

    Public ReadOnly Property FormId As String Implements IeZWorkflowDetails.FormId
        Get
            If D_XMLDS <> "" Then
                Dim _FormId As String = ""
                Try
                    Dim xmlds As New DataSet
                    xmlds.ReadXml(New StringReader(D_XMLDS))
                    _FormId = xmlds.Tables("Activity").Rows(0)("formid").ToString
                Catch ex As Exception
                    _FormId = ""
                End Try
                Return _FormId
            Else
                Return ""
            End If
        End Get

    End Property

    Public ReadOnly Property ItemTableName As String Implements IeZWorkflowDetails.ItemTableName
        Get
            If D_XMLDS <> "" Then
                Dim _ItemTableName As String = ""
                Try
                    Dim xmlds As New DataSet
                    xmlds.ReadXml(New StringReader(D_XMLDS))
                    _ItemTableName = "[" + xmlds.Tables("Activity").Rows(0)("tablename").ToString() + "]"
                Catch ex As Exception
                    _ItemTableName = ""
                End Try
                Return _ItemTableName
            Else
                Return ""
            End If
        End Get

    End Property

    Public ReadOnly Property FormTableName As String Implements IeZWorkflowDetails.FormTableName
        Get
            If D_XMLDS <> "" Then
                Dim _FormTableName As String = ""
                Try
                    Dim xmlds As New DataSet
                    xmlds.ReadXml(New StringReader(D_XMLDS))
                    _FormTableName = "[" + xmlds.Tables("Activity").Rows(0)("HTMLFormTable").ToString() + "]"
                Catch ex As Exception
                    _FormTableName = ""
                End Try
                Return _FormTableName
            Else
                Return ""
            End If
        End Get

    End Property

    Public ReadOnly Property ProcessInfoColumns As String Implements IeZWorkflowDetails.ProcessInfoColumns
        Get
            If D_XMLDS <> "" Then
                Dim _ProcessInfoColumns As String = ""
                Try
                    Dim xmlds As New DataSet
                    xmlds.ReadXml(New StringReader(D_XMLDS))
                    _ProcessInfoColumns = xmlds.Tables("Activity").Rows(0)("ProcessInfo").ToString()
                Catch ex As Exception
                    _ProcessInfoColumns = ""
                End Try
                Return _ProcessInfoColumns
            Else
                Return ""
            End If
        End Get
    End Property

    Public Property FlowTreeInfo As List(Of FlowNodes)
        Get
            Return D_flownodes
        End Get
        Set(value As List(Of FlowNodes))
            D_flownodes = value
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
