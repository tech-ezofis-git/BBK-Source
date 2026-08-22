
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI
Imports ECMAPI.ParaVariables


Public Class eZWFProcess
    Inherits IDatabaseCommonItems
    Implements IeZWFProcess


    Protected D_ProcessId As Integer
    Protected D_WorkflowId As Integer
    Protected D_FlowStatus As String
    Protected D_Workflowtypeid As Integer
    Protected D_Itemid As Integer
    Protected D_Templateid As Integer
    Protected D_Createdon As String
    Protected D_Updatedon As String
    Protected D_Createdby As Integer
    Protected D_Updatedby As Integer
    Private D_isdeleted As Integer
    Protected D_Action As String = ""
    Protected D_Createdby1 As String = ""
    Protected D_Updatedby1 As String = ""
    Protected D_RequestNo As String = ""
    Protected D_DocCount As String = "0"
    Protected D_RaisedBy As String = ""
    Protected D_RaisedOn As String = ""
    Protected D_Formid As String = ""
    Protected D_FormTableName As String = ""
    Protected D_FTemplateid As String = ""
    Protected D_ItemTableName As String = ""
    Protected D_ifilepath As String = ""
    Protected D_LastActedBy As String = ""
    Protected D_LastActedOn As String = ""
    Protected _DynamicProperty As Dictionary(Of String, String)
    Protected D_Escalated As Integer
    Protected D_DaysOpen As String
    Protected D_Month As String
    Protected _SplUsers As List(Of Userslist)
    Protected D_ActionBy As List(Of Userslist)
    Protected D_LastActionStage As String
    Protected D_LastActionReview As String
    Public Sub New()
    End Sub
    Public Sub New(ProcessId As Integer)
        Me.D_ProcessId = ProcessId

    End Sub
    Public Property Createdby() As Integer Implements IeZWFProcess.Createdby
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

    Public Property Action As String Implements IeZWFProcess.Action
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Action
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Action = value Then
                Return
            End If
            D_Action = value
            IsModified = True
        End Set
    End Property
    Public Property Createdon() As String Implements IeZWFProcess.Createdon
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

    Public Property FlowStatus As String Implements IeZWFProcess.FlowStatus
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_FlowStatus
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_FlowStatus = value Then
                Return
            End If
            D_FlowStatus = value
            IsModified = True
        End Set
    End Property

    Public Property LastActionStage As String Implements IeZWFProcess.LastActionStage
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_LastActionStage
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_LastActionStage = value Then
                Return
            End If
            D_LastActionStage = value
            IsModified = True

        End Set
    End Property

    Public Property LastActionReview As String Implements IeZWFProcess.LastActionReview
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_LastActionReview
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_LastActionReview = value Then
                Return
            End If
            D_LastActionReview = value
            IsModified = True

        End Set
    End Property
    Public Property Workflowtypeid As Integer Implements IeZWFProcess.Workflowtypeid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Workflowtypeid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Workflowtypeid = value Then
                Return
            End If
            D_Workflowtypeid = value
            IsModified = True
        End Set
    End Property
    Public Property Itemid As Integer Implements IeZWFProcess.Itemid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Itemid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Itemid = value Then
                Return
            End If
            D_Itemid = value
            IsModified = True
        End Set
    End Property
    Public Property Templateid As Integer Implements IeZWFProcess.Templateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Templateid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Templateid = value Then
                Return
            End If
            D_Templateid = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements IeZWFProcess.isdeleted
        Get
            Return D_isdeleted
        End Get
    End Property

    Public Property ProcessId As Integer Implements IeZWFProcess.ProcessId
        Get
            If D_ProcessId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return D_ProcessId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_ProcessId <> 0 AndAlso D_ProcessId <> value Then
                Throw New MemberAccessException()
            End If
            D_ProcessId = value
        End Set
    End Property

    Public Property Updatedby As Integer Implements IeZWFProcess.Updatedby
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

    Public Property Updatedon As String Implements IeZWFProcess.Updatedon
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

    Public Property WorkflowId As Integer Implements IeZWFProcess.WorkflowId
        Get
            If D_WorkflowId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return D_WorkflowId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_WorkflowId <> 0 AndAlso D_WorkflowId <> value Then
                Throw New MemberAccessException()
            End If
            D_WorkflowId = value
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZWFProcess.CreatedBy1
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

    Public Property UpdatedBy1 As String Implements IeZWFProcess.UpdatedBy1
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

    Public Property RequestNo As String Implements IeZWFProcess.RequestNo
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_RequestNo
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_RequestNo = value Then
                Return
            End If
            D_RequestNo = value
            IsModified = True
        End Set
    End Property


    Public Property DocCount As String Implements IeZWFProcess.DocCount
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_DocCount
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_DocCount = value Then
                Return
            End If
            D_DocCount = value
            IsModified = True
        End Set
    End Property

    Public Property RaisedOn As String Implements IeZWFProcess.RaisedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_RaisedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_RaisedOn = value Then
                Return
            End If
            D_RaisedOn = value
            IsModified = True
        End Set
    End Property

    Public Property RaisedBy As String Implements IeZWFProcess.RaisedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_RaisedBy
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_RaisedBy = value Then
                Return
            End If
            D_RaisedBy = value
            IsModified = True
        End Set
    End Property

    Public Property Formid As String Implements IeZWFProcess.Formid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Formid
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Formid = value Then
                Return
            End If
            D_Formid = value
            IsModified = True
        End Set
    End Property

    Public Property FormTableName As String Implements IeZWFProcess.FormTableName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_FormTableName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_FormTableName = value Then
                Return
            End If
            D_FormTableName = value
            IsModified = True
        End Set
    End Property

    Public Property FTemplateid As String Implements IeZWFProcess.FTemplateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_FTemplateid
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_FTemplateid = value Then
                Return
            End If
            D_FTemplateid = value
            IsModified = True
        End Set
    End Property

    Public Property ItemTableName As String Implements IeZWFProcess.ItemTableName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ItemTableName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_ItemTableName = value Then
                Return
            End If
            D_ItemTableName = value
            IsModified = True
        End Set
    End Property

    Public Property ifilepath As String Implements IeZWFProcess.ifilepath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ifilepath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_ifilepath = value Then
                Return
            End If
            D_ifilepath = value
            IsModified = True
        End Set
    End Property

    Public Property LastActedBy As String Implements IeZWFProcess.LastActedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_LastActedBy
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_LastActedBy = value Then
                Return
            End If
            D_LastActedBy = value
            IsModified = True
        End Set
    End Property

    Public Property LastActedOn As String Implements IeZWFProcess.LastActedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_LastActedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_LastActedOn = value Then
                Return
            End If
            D_LastActedOn = value
            IsModified = True
        End Set
    End Property

    Public Property DynamicProperty As Dictionary(Of String, String) Implements IeZWFProcess.DynamicProperty
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DynamicProperty
        End Get
        Set(value As Dictionary(Of String, String))
            DBLayer.DBLInstance.Read(Me)
            _DynamicProperty = value
            IsModified = True
        End Set
    End Property

    Public Property DaysOpen As String Implements IeZWFProcess.DaysOpen
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_DaysOpen
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_DaysOpen = value Then
                Return
            End If
            D_DaysOpen = value
            IsModified = True

        End Set
    End Property

    Public Property Month As String Implements IeZWFProcess.Month
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Month
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Month = value Then
                Return
            End If
            D_Month = value
            IsModified = True

        End Set
    End Property

    Public Property Escalated As Integer Implements IeZWFProcess.Escalated
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Escalated
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Escalated = value Then
                Return
            End If
            D_Escalated = value
            IsModified = True

        End Set
    End Property

    Public Property ActionBy As List(Of ParaVariables.Userslist) Implements IeZWFProcess.ActionBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ActionBy
        End Get
        Set(value As List(Of Userslist))
            DBLayer.DBLInstance.Read(Me)
            D_ActionBy = value
            IsModified = True

        End Set
    End Property

    Public Property SplUsers As List(Of ParaVariables.Userslist) Implements IeZWFProcess.SplUsers
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SplUsers
        End Get
        Set(value As List(Of Userslist))
            DBLayer.DBLInstance.Read(Me)
            _SplUsers = value
            IsModified = True

        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
