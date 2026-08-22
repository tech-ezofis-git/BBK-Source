Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI
Imports ECMAPI.ParaVariables

Public Class eZWFlowTransation
    Inherits IDatabaseCommonItems
    Implements IeZWFlowTransation



    Protected D_Transactionid As Integer
    Protected D_Processid As Integer


    Protected D_RequestNo As String
    Protected D_RaisedOn As String
    Protected D_RaisedBy As String
    Protected D_ActionBy As List(Of Userslist)
    Protected D_ActionGroupBy As String
    Protected D_Formid As Integer
    Protected D_FTemplateid As Integer
    Protected D_FormTableName As String
    '  Protected D_FormName As String
    Protected D_ItemTableName As String
    Protected D_LastActedBy As String
    Protected D_LastActedOn As String
    Protected D_DocCount As String
    Protected _DynamicProperty As Dictionary(Of String, String)

    Protected _SplUsers As List(Of Userslist)

    Protected D_ActivityId As String = ""
    Protected D_RuleId As String = ""
    Protected D_ActivityUserId As Integer
    Protected D_ActivityGroupId As Integer
    Protected D_Action As String = ""
    Protected D_Review As String = ""
    Protected D_TranPath As String = ""
    Protected D_TransactionStatus As Integer
    Protected D_Templateid As Integer
    Protected D_FileType As String = ""
    Protected D_Createdon As String = ""
    Protected D_Updatedon As String = ""
    Protected D_Createdby As Integer
    Protected D_Updatedby As Integer
    Protected D_Createdby1 As String = ""
    Protected D_Updatedby1 As String = ""
    Protected D_itemid As Integer
    Private D_isdeleted As Integer
    Protected D_Notification As Boolean
    Protected D_SkipTo As String = ""
    Protected D_FromMail As String = ""
    Protected D_RequestType As Boolean
    Protected D_UserType As String = ""
    Protected D_Attachment As Integer
    Protected D_Escalated As Integer
    Protected D_DaysOpen As String
    Protected D_Month As String
    Protected D_LastActionStage As String
    Protected D_LastActionReview As String
    Public Sub New()
    End Sub
    Public Sub New(TransactionId As Integer)
        Me.D_Transactionid = TransactionId
    End Sub
    Public Property Createdby() As Integer Implements IeZWFlowTransation.Createdby
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
    Public Property itemid() As Integer Implements IeZWFlowTransation.itemid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_itemid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_itemid = value Then
                Return
            End If
            D_itemid = value
            IsModified = True
        End Set
    End Property

    Public Property LastActionStage As String Implements IeZWFlowTransation.LastActionStage
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

    Public Property LastActionReview As String Implements IeZWFlowTransation.LastActionReview
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

    Public Property Createdon() As String Implements IeZWFlowTransation.Createdon
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

    Public Property templateid() As Integer Implements IeZWFlowTransation.Templateid
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

    Public Property FileType() As String Implements IeZWFlowTransation.FileType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_FileType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_FileType = value Then
                Return
            End If
            D_FileType = value
            IsModified = True
        End Set
    End Property
    Public ReadOnly Property isdeleted() As Integer Implements IeZWFlowTransation.isdeleted
        Get
            Return D_isdeleted
        End Get
    End Property
    Public Property notification() As Boolean Implements IeZWFlowTransation.Notification
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Notification
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If D_Notification = value Then
                Return
            End If
            D_Notification = value
            IsModified = True
        End Set
    End Property

    Public Property ProcessId() As Integer Implements IeZWFlowTransation.ProcessId
        Get
            If D_Processid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return D_Processid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_Processid <> 0 AndAlso D_Processid <> value Then
                Throw New MemberAccessException()
            End If
            D_Processid = value
        End Set

    End Property
    Public Property RuleId As String Implements IeZWFlowTransation.RuleId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_RuleId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_RuleId = value Then
                Return
            End If
            D_RuleId = value
            IsModified = True
        End Set
    End Property


    Public Property RequestNo As String Implements IeZWFlowTransation.RequestNo
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


    Public Property ActivityId As String Implements IeZWFlowTransation.ActivityId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ActivityId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_ActivityId = value Then
                Return
            End If
            D_ActivityId = value
            IsModified = True
        End Set
    End Property

    Public Property Action As String Implements IeZWFlowTransation.Action
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

    Public Property Review As String Implements IeZWFlowTransation.Review
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Review
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Review = value Then
                Return
            End If
            D_Review = value
            IsModified = True
        End Set
    End Property

    Public Property TranPath As String Implements IeZWFlowTransation.TranPath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_TranPath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_TranPath = value Then
                Return
            End If
            D_TranPath = value
            IsModified = True
        End Set
    End Property

    Public Property TransactionStatus As Integer Implements IeZWFlowTransation.TransactionStatus
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_TransactionStatus
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_TransactionStatus = value Then
                Return
            End If
            D_TransactionStatus = value
            IsModified = True
        End Set
    End Property
    Public Property ActivityGroupId As Integer Implements IeZWFlowTransation.ActivityGroupId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ActivityGroupId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_ActivityGroupId = value Then
                Return
            End If
            D_ActivityGroupId = value
            IsModified = True
        End Set
    End Property

    Public Property ActivityUserId As Integer Implements IeZWFlowTransation.ActivityUserId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ActivityUserId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_ActivityUserId = value Then
                Return
            End If
            D_ActivityUserId = value
            IsModified = True
        End Set
    End Property

    Public Property Transactionid() As Integer Implements IeZWFlowTransation.Transactionid
        Get
            If D_Transactionid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return D_Transactionid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_Transactionid <> 0 AndAlso D_Transactionid <> value Then
                Throw New MemberAccessException()
            End If
            D_Transactionid = value
        End Set
    End Property
    Public Property Updatedby As Integer Implements IeZWFlowTransation.Updatedby
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

    Public Property Updatedon As String Implements IeZWFlowTransation.Updatedon
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
    Public Property SkipTo As String Implements IeZWFlowTransation.SkipTo
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_SkipTo
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_SkipTo = value Then
                Return
            End If
            D_SkipTo = value
            IsModified = True
        End Set
    End Property
    Public Property FromMail As String Implements IeZWFlowTransation.FromMail
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_FromMail
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_FromMail = value Then
                Return
            End If
            D_FromMail = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZWFlowTransation.CreatedBy1
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

    Public Property UpdatedBy1 As String Implements IeZWFlowTransation.UpdatedBy1
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

    Public Property RequestType As Boolean Implements IeZWFlowTransation.RequestType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_RequestType
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If D_RequestType = value Then
                Return
            End If
            D_RequestType = value
            IsModified = True
        End Set
    End Property

    Public Property UserType As String Implements IeZWFlowTransation.UserType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_UserType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_UserType = value Then
                Return
            End If
            D_UserType = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

    Public Property Attachment As Integer Implements IeZWFlowTransation.Attachment
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Attachment
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Attachment = value Then
                Return
            End If
            D_Attachment = value
            IsModified = True

        End Set
    End Property

    Public Property RaisedOn As String Implements IeZWFlowTransation.RaisedOn
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

    Public Property RaisedBy As String Implements IeZWFlowTransation.RaisedBy
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



    Public Property ActionGroupBy As String Implements IeZWFlowTransation.ActionGroupBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ActionGroupBy
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_ActionGroupBy = value Then
                Return
            End If
            D_ActionGroupBy = value
            IsModified = True

        End Set
    End Property

    Public Property Formid As Integer Implements IeZWFlowTransation.Formid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Formid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Formid = value Then
                Return
            End If
            D_Formid = value
            IsModified = True

        End Set
    End Property

    Public Property FTemplateid As Integer Implements IeZWFlowTransation.FTemplateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_FTemplateid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_FTemplateid = value Then
                Return
            End If
            D_FTemplateid = value
            IsModified = True

        End Set
    End Property

    Public Property FormTableName As String Implements IeZWFlowTransation.FormTableName
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

    'Public Property FormName As String Implements IeZWFlowTransation.FormName
    '    Get
    '        DBLayer.DBLInstance.Read(Me)
    '        Return D_FormName
    '    End Get
    '    Set(value As String)
    '        DBLayer.DBLInstance.Read(Me)
    '        If D_FormName = value Then
    '            Return
    '        End If
    '        D_FormName = value
    '        IsModified = True

    '    End Set
    'End Property

    Public Property ItemTableName As String Implements IeZWFlowTransation.ItemTableName
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

    Public Property LastActedBy As String Implements IeZWFlowTransation.LastActedBy
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

    Public Property LastActedOn As String Implements IeZWFlowTransation.LastActedOn
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

    Public Property DocCount As String Implements IeZWFlowTransation.DocCount
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
    'SplUsers
    Public Property DynamicProperty As Dictionary(Of String, String) Implements IeZWFlowTransation.DynamicProperty
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

    Public Property ActionBy As List(Of Userslist) Implements IeZWFlowTransation.ActionBy
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
    Public Property SplUsers As List(Of Userslist) Implements IeZWFlowTransation.SplUsers
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

    Public Property Escalated As Integer Implements IeZWFlowTransation.Escalated
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

    Public Property DaysOpen As String Implements IeZWFlowTransation.DaysOpen
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

    Public Property Month As String Implements IeZWFlowTransation.Month
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
End Class
