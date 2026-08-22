''' <summary>
''' Summary description for Instance
''' </summary>
Partial Public Class Instance
    Implements IInstance

#Region "Instance Defintion for Login"
    Public Function eZTrialLicense(TrialId As Integer) As IeZTrialLicense Implements IInstance.eZTrialLicense
        Return New eZTrialLicense(TrialId)
    End Function
    Public Function eZRegistration(CompanyId As Integer) As IeZRegistration Implements IInstance.eZRegistration
        Return New eZRegistration(CompanyId)
    End Function
    'Public Function eZWorkflowDetails(WorkFlowId As Integer) As IeZWorkflowDetails Implements IInstance.eZWorkflowDetails
    '    Return New eZWorkflowDetails(WorkFlowId)
    'End Function
    Public Function eZLicenseClients(LicenseClientId As Integer) As IeZLicenseClients Implements IInstance.eZLicenseClients
        Return New eZLicenseClients(LicenseClientId)
    End Function
    Public Function eZWorkFlow(WorkFlowId As Integer) As IeZWorkFlow Implements IInstance.eZWorkFlow
        Return New eZWorkFlow(WorkFlowId)
    End Function
    Public Function eZWorkFlowRelation(RelationId As Integer) As IeZWorkFlowRelation Implements IInstance.eZWorkFlowRelation
        Return New eZWorkFlowRelation(RelationId)
    End Function
    Public Function eZWorkFlowProcess(ProcessId As Integer) As IeZWorkFlowProcess Implements IInstance.eZWorkFlowProcess
        Return New eZWorkFlowProcess(ProcessId)
    End Function
    Public Function eZWorkFlowType(WorkFlowTypeId As Integer) As IeZWorkFlowType Implements IInstance.eZWorkFlowType
        Return New eZWorkFlowType(WorkFlowTypeId)
    End Function
    Public Function eZApplication(ApplicationId As Integer) As IeZApplication Implements IInstance.eZApplication
        Return New eZApplication(ApplicationId)
    End Function
    Public Function eZLicense(LicenseId As Integer) As IeZLicense Implements IInstance.eZLicense
        Return New eZLicense(LicenseId)
    End Function
    Public Function eZAlternateField(eZAlternateFieldId As Integer) As IeZAlternateField Implements IInstance.eZAlternateField
        Return New eZAlternateField(eZAlternateFieldId)
    End Function
    Public Function eZFaxReceiverRule(eZFaxReceiverRuleId As Integer) As IeZFaxReceiverRule Implements IInstance.eZFaxReceiverRule
        Return New eZFaxReceiverRule(eZFaxReceiverRuleId)
    End Function
    Public Function eZFaxReceiver(eZFaxReceiverId As Integer) As IeZFaxReceiver Implements IInstance.eZFaxReceiver
        Return New eZFaxReceiver(eZFaxReceiverId)
    End Function
    Public Function eZFax(eZFaxId As Integer) As IeZFax Implements IInstance.eZFax
        Return New eZFax(eZFaxId)
    End Function
    Public Function eZFaxTransaction(eZFaxTransactionId As Integer) As IeZFaxTransaction Implements IInstance.eZFaxTransaction
        Return New eZFaxTransaction(eZFaxTransactionId)
    End Function
    Public Function eZInboxTransaction(eZInboxTransactionId As Integer) As IeZInboxTransaction Implements IInstance.eZInboxTransaction
        Return New eZInboxTransaction(eZInboxTransactionId)
    End Function
    Public Function eZBookMarksDetail(eZBookMarksDetailId As Integer) As IeZBookMarksDetail Implements IInstance.eZBookMarksDetail
        Return New eZBookMarksDetail(eZBookMarksDetailId)
    End Function
    Public Function eZBookMarks(eZBookMarksId As Integer) As IeZBookMarks Implements IInstance.eZBookMarks
        Return New eZBookMarks(eZBookMarksId)
    End Function
    Public Function eZMail(eZMailId As Integer) As IeZMail Implements IInstance.eZMail
        Return New eZMail(eZMailId)
    End Function

    Public Function eZMailArchive(MailArchiveId As Integer) As IeZMailArchive Implements IInstance.eZMailArchive
        Return New eZMailArchive(MailArchiveId)
    End Function
    Public Function eZTask(TaskId As Integer) As IeZTask Implements IInstance.eZTask
        Return New eZTask(TaskId)
    End Function
    Public Function eZTaskType(Typeid As Integer) As IeZTaskType Implements IInstance.eZTaskType
        Return New eZTaskType(Typeid)
    End Function
    Public Function eZDtSearchPath(indexpathid As Integer) As IeZDtSearchPath Implements IInstance.eZDtSearchPath
        Return New eZDtSearchPath(indexpathid)
    End Function
    Public Function eZOutlookContact(OutlookContactId As Integer) As IeZOutlookContact Implements IInstance.eZOutlookContact
        Return New eZOutlookContact(OutlookContactId)
    End Function
    Public Function eZECMUserInfo(UserId As Integer) As IeZECMUserInfo Implements IInstance.eZECMUserInfo
        Return New eZECMUserInfo(UserId)
    End Function
    Public Function eZTaskUsers(TaskUsersId As Integer) As IeZTaskUsers Implements IInstance.eZTaskUsers
        Return New eZTaskUsers(TaskUsersId)
    End Function
    Public Function eZAllottedTask(AllottedTaskId As Integer) As IeZAllottedTask Implements IInstance.eZAllottedTask
        Return New eZAllottedTask(AllottedTaskId)
    End Function
    Public Function eZECMLogin(ECMLoginId As Integer) As IOldeZECMLogin Implements IInstance.eZECMLogin
        Return New OldeZECMLogin(ECMLoginId)
    End Function
    Public Function eZProfile(Profileid As Integer) As IeZProfile Implements IInstance.eZProfile
        Return New eZProfile(Profileid)
    End Function
    Public Function eZECMGroup(ECMGroupId As Integer) As IeZECMGroup Implements IInstance.eZECMGroup
        Return New eZECMGroup(ECMGroupId)
    End Function
    Public Function eZECMGroupusers(ECMGroupuserId As Integer) As IeZECMGroupusers Implements IInstance.eZECMGroupusers
        Return New eZECMGroupusers(ECMGroupuserId)
    End Function
    Public Function eZECMFieldLevel(ECMFieldLevelId As Integer) As IeZECMFieldLevel Implements IInstance.eZECMFieldLevel
        Return New eZECMFieldLevel(ECMFieldLevelId)
    End Function
    Public Function eZECMCabinetLevel(ECMCabinetLevelId As Integer) As IeZECMCabinetLevel Implements IInstance.eZECMCabinetLevel
        Return New eZECMCabinetLevel(ECMCabinetLevelId)
    End Function

    Public Function eZECMDocumentLevel(ECMDocumentLevelId As Integer) As IeZECMDocumentLevel Implements IInstance.eZECMDocumentLevel
        Return New eZECMDocumentLevel(ECMDocumentLevelId)
    End Function
    Public Function eZECMControlLevel(ECMControlLevelId As Integer) As IeZECMControlLevel Implements IInstance.eZECMControlLevel
        Return New eZECMControlLevel(ECMControlLevelId)
    End Function
    Public Function eZECMControl(ECMControlId As Integer) As IeZECMControl Implements IInstance.eZECMControl
        Return New eZECMControl(ECMControlId)
    End Function
    Public Function ezImpersonation(ImpersonateId As Integer) As IezImpersonation Implements IInstance.ezImpersonation
        Return New ezImpersonation(ImpersonateId)
    End Function
    Public Function eZECMProfile(ECMProfileId As Integer) As IeZECMProfile Implements IInstance.eZECMProfile
        Return New eZECMProfile(ECMProfileId)
    End Function
    Public Function eZCabOwners(CabOwnerId As Integer) As IeZCabOwners Implements IInstance.eZCabOwners
        Return New eZCabOwners(CabOwnerId)
    End Function
    Public Function eZERSInfo(ERSId As Integer) As IeZERSInfo Implements IInstance.eZERSInfo
        Return New eZERSInfo(ERSId)
    End Function
    Public Function eZERSSync(eZERSSyncId As Integer) As IeZERSSync Implements IInstance.eZERSSync
        Return New eZERSSync(eZERSSyncId)
    End Function
    Public Function ezerssync_History(eZERSSyncId As Integer) As IeZERSSync_History Implements IInstance.ezerssync_History
        Return New eZERSSync_History(eZERSSyncId)
    End Function

    Public Function eZCabinet(eZCabinetId As Integer) As IeZCabinet Implements IInstance.eZCabinet
        Return New eZCabinet(eZCabinetId)
    End Function
    Public Function eZTemplate(eZTemplateId As Integer) As IeZTemplate Implements IInstance.eZTemplate
        Return New eZTemplate(eZTemplateId)
    End Function
    Public Function eZDuplicateType(eZDuplicateTypeId As Integer) As IeZDuplicateType Implements IInstance.eZDuplicateType
        Return New eZDuplicateType(eZDuplicateTypeId)
    End Function
    Public Function eZECMUserType(eZECMUserTypeId As Integer) As IeZECMUserType Implements IInstance.eZECMUserType
        Return New eZECMUserType(eZECMUserTypeId)
    End Function
    Public Function eZLanguage(eZLanguageId As Integer) As IeZLanguage Implements IInstance.eZLanguage
        Return New eZLanguage(eZLanguageId)
    End Function
    Public Function eZCondition(eZConditionId As Integer) As IeZCondition Implements IInstance.eZCondition
        Return New eZCondition(eZConditionId)
    End Function
    Public Function eZAlertCondition(eZAlertConditionId As Integer) As IeZAlertCondition Implements IInstance.eZAlertCondition
        Return New eZAlertCondition(eZAlertConditionId)
    End Function
    Public Function eZBodyHtmlType(eZBodyHtmlTypeId As Integer) As IeZBodyHtmlType Implements IInstance.eZBodyHtmlType
        Return New eZBodyHtmlType(eZBodyHtmlTypeId)
    End Function
    Public Function eZChart(eZChartId As Integer) As IeZChart Implements IInstance.eZChart
        Return New eZChart(eZChartId)
    End Function
    Public Function eZDocumentAlert(eZDocumentAlertId As Integer) As IeZDocumentAlert Implements IInstance.eZDocumentAlert
        Return New eZDocumentAlert(eZDocumentAlertId)
    End Function
    Public Function eZAlert(eZAlertId As Integer) As IeZAlert Implements IInstance.eZAlert
        Return New eZAlert(eZAlertId)
    End Function
    Public Function eZFieldAlert(eZFieldAlertId As Integer) As IeZFieldAlert Implements IInstance.eZFieldAlert
        Return New eZFieldAlert(eZFieldAlertId)
    End Function
    Public Function eZFieldAlertDetail(eZFieldAlertDetailId As Integer) As IeZFieldAlertDetail Implements IInstance.eZFieldAlertDetail
        Return New eZFieldAlertDetail(eZFieldAlertDetailId)
    End Function
    Public Function eZFieldAlertDoc(FieldAlertDocId As Integer) As IeZFieldAlertDoc Implements IInstance.eZFieldAlertDoc
        Return New eZFieldAlertDoc(FieldAlertDocId)
    End Function
    Public Function eZFieldAlertTemp(Id As Integer) As IeZFieldAlertTemp Implements IInstance.eZFieldAlertTemp
        Return New eZFieldAlertTemp(Id)
    End Function
    Public Function eZScheduleType(eZScheduleTypeId As Integer) As IeZScheduleType Implements IInstance.eZScheduleType
        Return New eZScheduleType(eZScheduleTypeId)
    End Function
    Public Function eZMailArchiveType(eZMailArchiveTypeId As Integer) As IeZMailArchiveType Implements IInstance.eZMailArchiveType
        Return New eZMailArchiveType(eZMailArchiveTypeId)
    End Function
    Public Function eZSchedule(eZScheduleId As Integer) As IeZSchedule Implements IInstance.eZSchedule
        Return New eZSchedule(eZScheduleId)
    End Function
    Public Function eZReminder(eZReminderId As Integer) As IeZReminder Implements IInstance.eZReminder
        Return New eZReminder(eZReminderId)
    End Function
    Public Function eZLookupServerType(eZLookupServerTypeId As Integer) As IeZLookupServerType Implements IInstance.eZLookupServerType
        Return New eZLookupServerType(eZLookupServerTypeId)
    End Function
    Public Function eZLookupConnection(LookupType As Integer) As IeZLookupConnection Implements IInstance.eZLookupConnection
        Return New eZLookupConnection(LookupType)
    End Function
    Public Function eZLookupType(eZLookupTypeId As Integer) As IeZLookupType Implements IInstance.eZLookupType
        Return New eZLookupType(eZLookupTypeId)
    End Function
    Public Function eZTempDatatype(eZTempDatatypeId As Integer) As IeZTempDatatype Implements IInstance.eZTempDatatype
        Return New eZTempDatatype(eZTempDatatypeId)
    End Function
    Public Function eZBarcodeType(eZBarcodeTypeId As Integer) As IeZBarcodeType Implements IInstance.eZBarcodeType
        Return New eZBarcodeType(eZBarcodeTypeId)
    End Function
    Public Function eZChartType(eZChartTypeId As Integer) As IeZChartType Implements IInstance.eZChartType
        Return New eZChartType(eZChartTypeId)
    End Function
    Public Function eZTemplateField(eZTemplateFieldId As Integer) As IeZTemplateField Implements IInstance.eZTemplateField
        Return New eZTemplateField(eZTemplateFieldId)
    End Function
    Public Function eZTemplateUserFields(UserFieldId As Integer) As IeZTemplateUserFields Implements IInstance.eZTemplateUserFields
        Return New eZTemplateUserFields(UserFieldId)
    End Function
    Public Function eZPdfProperties(eZPdfPropertiesId As Integer) As IeZPdfProperties Implements IInstance.eZPdfProperties
        Return New eZPdfProperties(eZPdfPropertiesId)
    End Function
    Public Function eZTempBarcode(eZTempBarcodeId As Integer) As IeZTempBarcode Implements IInstance.eZTempBarcode
        Return New eZTempBarcode(eZTempBarcodeId)
    End Function
    Public Function eZERSIPs(eZERSIPsId As Integer) As IeZERSIPs Implements IInstance.eZERSIPs
        Return New eZERSIPs(eZERSIPsId)
    End Function

    Public Function eZInbox(eZInboxId As Integer) As IeZInbox Implements IInstance.eZInbox
        Return New eZInbox(eZInboxId)
    End Function
    Public Function eZFolders(eZFoldersId As Integer) As IeZFolders Implements IInstance.eZFolders
        Return New eZFolders(eZFoldersId)
    End Function
    Public Function ezFoldersByUser(NodeId As Integer) As IezFoldersByUser Implements IInstance.ezFoldersByUser
        Return New ezFoldersByUser(NodeId)
    End Function
    Public Function eZPrivateFolders(Privatefolderid As Integer) As IeZPrivateFolders Implements IInstance.eZPrivateFolders
        Return New eZPrivateFolders(Privatefolderid)
    End Function
    Public Function eZFoldersForTemp(eZFoldersForTempId As Integer) As IeZFoldersForTemp Implements IInstance.eZFoldersForTemp
        Return New eZFoldersForTemp(eZFoldersForTempId)
    End Function
    Public Function eZHierarchy(Hierarchy_id As Integer) As IeZHierarchy Implements IInstance.eZHierarchy
        Return New eZHierarchy(Hierarchy_id)
    End Function
    Public Function eZIndexingChange(Indexingchangeid As Integer) As IeZIndexingChange Implements IInstance.eZIndexingChange
        Return New eZIndexingChange(Indexingchangeid)
    End Function
    Public Function eZDocumentLink(LinkId As Integer) As IeZDocumentLink Implements IInstance.eZDocumentLink
        Return New eZDocumentLink(LinkId)
    End Function
    Public Function eZLookup(LookupId As Integer) As IeZLookup Implements IInstance.eZLookup
        Return New eZLookup(LookupId)
    End Function
    Public Function ezlookupsynchistory(synchistoryid As Integer) As Iezlookupsynchistory Implements IInstance.ezlookupsynchistory
        Return New ezlookupsynchistory(synchistoryid)
    End Function
    Public Function eZLookupFields(LookupFieldId As Integer) As IeZLookupFields Implements IInstance.eZLookupFields
        Return New eZLookupFields(LookupFieldId)
    End Function
    Public Function eZLookupClientField(LookupFieldId As Integer) As IeZLookupClientField Implements IInstance.eZLookupClientField
        Return New eZLookupClientField(LookupFieldId)
    End Function
    Public Function eZLookupSPparameters(LookupSPparamId As Integer) As IeZLookupSPparameters Implements IInstance.eZLookupSPparameters
        Return New eZLookupSPparameters(LookupSPparamId)
    End Function
    Public Function eZComments(CommentsId As Integer) As IeZComments Implements IInstance.eZComments
        Return New eZComments(CommentsId)
    End Function
    Public Function eZFilesCopyLink(CopyLinkId As Integer) As IeZFilesCopyLink Implements IInstance.eZFilesCopyLink
        Return New eZFilesCopyLink(CopyLinkId)
    End Function
    Public Function eZZonal(eZZonalId As Integer) As IeZZonal Implements IInstance.eZZonal 'Arunachalam
        Return New eZZonal(eZZonalId)
    End Function
    Public Function eZIntegrationDetail(IntegrationId As Integer) As IeZIntegrationDetail Implements IInstance.eZIntegrationDetail
        Return New eZIntegrationDetail(IntegrationId)
    End Function
    Public Function eZOutlooksync(outlooksyncid As Integer) As IeZOutlooksync Implements IInstance.eZOutlooksync
        Return New eZOutlooksync(outlooksyncid)
    End Function
    Public Function eZOutlooksync_histroy(Outlooksync_historyid As Integer) As IeZOutlookSync_History Implements IInstance.eZOutlooksync_histroy
        Return New eZOutlookSync_History(Outlooksync_historyid)
    End Function
    Public Function eZtest(outlooksyncid As Integer) As ieZtest Implements IInstance.eZtest
        Return New eZtest(outlooksyncid)
    End Function
    Public Function eZOutlookDetail(Outlookdetailid As Integer) As IeZOutlookDetail Implements IInstance.eZOutlookDetail
        Return New eZOutlookDetail(Outlookdetailid)
    End Function
#End Region

#Region "Form Details"
    Public Function eZFormControlDetail(eZFormControlDetailId As Integer) As IeZFormControlDetail Implements IInstance.eZFormControlDetail
        Return New eZFormControlDetail(eZFormControlDetailId)
    End Function
    Public Function eZFormControlValue(eZFormControlValueId As Integer) As IeZFormControlValue Implements IInstance.eZFormControlValue
        Return New eZFormControlValue(eZFormControlValueId)
    End Function
    Public Function eZFormDetails(eZFormDetailsId As Integer) As IeZFormDetails Implements IInstance.eZFormDetails
        Return New eZFormDetails(eZFormDetailsId)
    End Function
    Public Function eZFrmControlDataType(eZFrmControlDataTypeId As Integer) As IeZFrmControlDataType Implements IInstance.eZFrmControlDataType
        Return New eZFrmControlDataType(eZFrmControlDataTypeId)
    End Function
    Public Function eZFrmControlType(eZFrmControlTypeId As Integer) As IeZFrmControlType Implements IInstance.eZFrmControlType
        Return New eZFrmControlType(eZFrmControlTypeId)
    End Function
    Public Function eZFormValidation(ValidationId As Integer) As IeZFormValidation Implements IInstance.eZFormValidation
        Return New eZFormValidation(ValidationId)
    End Function
    Public Function eZlinkeditems(Linkedid As Integer) As IeZLinkedItems Implements IInstance.ezlinkeditems
        Return New eZLinkedItems(Linkedid)
    End Function
    Public Function SyncTable(Syncid As Integer) As ISyncTable Implements IInstance.SyncTable
        Return New SyncTable(Syncid)
    End Function
#End Region
#Region "License"
    Public Function eZClient(clientid As Integer) As IeZClient Implements IInstance.eZClient
        Return New eZClient(clientid)
    End Function
    Public Function eZClientApproval(clientapprovalid As Integer) As IeZClientAppproval Implements IInstance.ezclientapproval
        Return New eZClientApproval(clientapprovalid)
    End Function
#End Region
#Region "Files"
    Public Function eZHideFile(hidefileid As Integer) As IeZHideFile Implements IInstance.eZHideFile
        Return New eZHideFile(hidefileid)
    End Function
    Public Function eZHideFileUsers(hidefileuserid As Integer) As IeZHideFileUsers Implements IInstance.eZHideFileUsers
        Return New eZHideFileUsers(hidefileuserid)
    End Function
    Public Function eZHidePages(hideid As Integer) As IeZHidePages Implements IInstance.eZHidePages
        Return New eZHidePages(hideid)
    End Function
    Public Function ezSupportFiles(Attachmentid As Integer) As IezSupportFiles Implements IInstance.ezSupportFiles
        Return New ezSupportFiles(Attachmentid)
    End Function
#End Region
#Region "Vault"
    Public Function eZVault(ezvaultid As Integer) As IeZVault Implements IInstance.eZVault
        Return New eZVault(ezvaultid)
    End Function
#End Region
#Region "WorkFlow"
    Public Function eZProcessItems(processitemsid As Integer) As IeZProcessItems Implements IInstance.eZProcessItems
        Return New eZProcessItems(processitemsid)
    End Function
    Public Function eZWorkflowUsers(workflowusersid As Integer) As IeZWorkflowUsers Implements IInstance.eZWorkflowUsers
        Return New eZWorkflowUsers(workflowusersid)
    End Function
    Public Function eZFormUsers(formusersid As Integer) As IeZFormUsers Implements IInstance.eZFormUsers
        Return New eZFormUsers(formusersid)
    End Function
    Public Function eZWFlowFormDetails(formdetailsid As Integer) As IeZWFlowFormDetails Implements IInstance.eZWFlowFormDetails
        Return New eZWFlowFormDetails(formdetailsid)
    End Function
    Public Function eZWFlowTransation(TransactionId As Integer) As IeZWFlowTransation Implements IInstance.eZWFlowTransation
        Return New eZWFlowTransation(TransactionId)
    End Function
    Public Function eZWFProcess(ProcessId As Integer) As IeZWFProcess Implements IInstance.eZWFProcess
        Return New eZWFProcess(ProcessId)
    End Function
    Public Function eZWorkflowDetails(Workflowid As Integer) As IeZWorkflowDetails Implements IInstance.eZWorkflowDetails
        Return New eZWorkflowDetails(Workflowid)
    End Function
#End Region
#Region "eZMailSettings"
    Public Function eZMailSettings(SettingId As Integer) As IeZMailSettings Implements IInstance.eZMailSettings
        Return New eZMailSettings(SettingId)
    End Function
    Public Function eZMailArchiveValue(MailArchiveValueId As Integer) As IeZMailArchiveValue Implements IInstance.eZMailArchiveValue
        Return New eZMailArchiveValue(MailArchiveValueId)
    End Function
    Public Function eZMailTriggering(MailTriggerid As Integer) As IeZMailTriggering Implements IInstance.eZMailTriggering
        Return New eZMailTriggering(MailTriggerid)
    End Function
    Public Function ezMailTriggerTypes(TriggerTypeid As Integer) As IezMailTriggerTypes Implements IInstance.ezMailTriggerTypes
        Return New ezMailTriggerTypes(TriggerTypeid)
    End Function
    Public Function eZMailType(MailTypeId As Integer) As IeZMailType Implements IInstance.eZMailType
        Return New eZMailType(MailTypeId)
    End Function
    Public Function eZMailWatching(mailwatchingid As Integer) As IeZMailWatching Implements IInstance.eZMailWatching
        Return New eZMailWatching(mailwatchingid)
    End Function
    Public Function eZMailWatchingCondition(conditionid As Integer) As IeZMailWatchingCondition Implements IInstance.eZMailWatchingCondition
        Return New eZMailWatchingCondition(conditionid)
    End Function
    Public Function eZMailWatchingDetails(sendid As Integer) As IeZMailWatchingDetails Implements IInstance.eZMailWatchingDetails
        Return New eZMailWatchingDetails(sendid)
    End Function
    Public Function eZMailWatchingStatus(MailWatchingId As Integer) As IeZMailWatchingStatus Implements IInstance.eZMailWatchingStatus
        Return New eZMailWatchingStatus(MailWatchingId)
    End Function
    Public Function eZUnAllocatedMail(MailRequestId As Integer) As IeZUnAllocatedMail Implements IInstance.eZUnAllocatedMail
        Return New eZUnAllocatedMail(MailRequestId)
    End Function
#End Region
#Region "Active Directory"
    Public Function eZLdapConnection(LdapConnId As Integer) As IeZLdapConnection Implements IInstance.eZLdapConnection
        Return New eZLdapConnection(LdapConnId)
    End Function
    Public Function eZADUsers(LdapUserId As Integer) As IeZADUsers Implements IInstance.eZADUsers
        Return New eZADUsers(LdapUserId)
    End Function
    Public Function eZRCContacts(eZContactId As Integer) As IeZRCContacts Implements IInstance.eZRCContacts
        Return New eZRCContacts(eZContactId)
    End Function
#End Region
#Region "FolderMonitor"
    Public Function eZFolderMonitor(Monitorid As Integer) As IeZFolderMonitor Implements IInstance.eZFolderMonitor
        Return New eZFolderMonitor(Monitorid)
    End Function
    Public Function eZUnProcessedFiles(UnprocessId As Integer) As IeZUnProcessedFiles Implements IInstance.eZUnProcessedFiles
        Return New eZUnProcessedFiles(UnprocessId)
    End Function
#End Region
#Region "Profile Security"
    Public Function eZECMProfileTemplate(ProfileTemplateId As Integer) As IeZECMProfileTemplate Implements IInstance.eZECMProfileTemplate
        Return New eZECMProfileTemplate(ProfileTemplateId)
    End Function
    Public Function eZECMProfileUsers(ECMProfileUsersId As Integer) As IeZECMProfileUsers Implements IInstance.eZECMProfileUsers
        Return New eZECMProfileUsers(ECMProfileUsersId)
    End Function
#End Region
#Region "Error"
    Public Function ErrorMessage() As IErrorMessage Implements IInstance.ErrorMessage
        Return New ErrorMessage()
    End Function
#End Region
#Region "Collaboration"
    Public Function eZCollaboration(CollId As Integer) As IeZCollaboration Implements IInstance.eZCollaboration
        Return New eZCollaboration(CollId)
    End Function
    Public Function eZCollaborationUserDetails(Id As Integer) As IeZCollaborationUserDetails Implements IInstance.eZCollaborationUserDetails
        Return New eZCollaborationUserDetails(Id)
    End Function
    Public Function eZTaskComments(CommentsId As Integer) As IeZTaskComments Implements IInstance.eZTaskComments
        Return New eZTaskComments(CommentsId)
    End Function
#End Region
#Region "Map"
    Public Function eZMapFields(Mapfieldsid As Integer) As IeZMapFields Implements IInstance.eZMapFields
        Return New eZMapFields(Mapfieldsid)
    End Function
    Public Function eZMapLocation(LocationId As Integer) As IeZMapLocation Implements IInstance.eZMapLocation
        Return New eZMapLocation(LocationId)
    End Function
    Public Function eZMapTemplate(MapTemplateId As Integer) As IeZMapTemplate Implements IInstance.eZMapTemplate
        Return New eZMapTemplate(MapTemplateId)
    End Function
#End Region
#Region "Notification"
    Public Function ezNotification(NotificationId As Integer) As IezNotification Implements IInstance.ezNotification
        Return New ezNotification(NotificationId)
    End Function
#End Region
#Region "Scan"
    Public Function eZScanBatch(BatchId As Integer) As IeZScanBatch Implements IInstance.eZScanBatch
        Return New eZScanBatch(BatchId)
    End Function
    Public Function ezScannedImg(ScannedImgId As Integer) As IezScannedImg Implements IInstance.ezScannedImg
        Return New ezScannedImg(ScannedImgId)
    End Function
    Public Function eZScanSettings(SettingId As Integer) As IeZScanSettings Implements IInstance.eZScanSettings
        Return New eZScanSettings(SettingId)
    End Function
#End Region
#Region "Schedule"
    Public Function eZScheduleDetail(Detailid As Integer) As IeZScheduleDetail Implements IInstance.eZScheduleDetail
        Return New eZScheduleDetail(Detailid)
    End Function
    Public Function eZScheduleFor(ForScheduleId As Integer) As IeZScheduleFor Implements IInstance.eZScheduleFor
        Return New eZScheduleFor(ForScheduleId)
    End Function
    Public Function ezEscalation(EscalationId As Integer) As IezEscalation Implements IInstance.ezEscalation
        Return New ezEscalation(EscalationId)
    End Function
    Public Function ezEscalationHistory(EscalationHistoryId As Integer) As IezEscalationHistory Implements IInstance.ezEscalationHistory
        Return New ezEscalationHistory(EscalationHistoryId)
    End Function
    Public Function ezEscalationuser(EscalationUserId As Integer) As IezEscalationUser Implements IInstance.ezEscalationUser
        Return New ezEscalationUser(EscalationUserId)
    End Function
#End Region
#Region "Retention"
    Public Function ezRetentionRule(RetentionId As Integer) As IezRetentionRule Implements IInstance.ezRetentionRule
        Return New ezRetentionRule(RetentionId)
    End Function
    Public Function ezRetentionMail(RetMailId As Integer) As IezRetentionMail Implements IInstance.ezRetentionMail
        Return New ezRetentionMail(RetMailId)
    End Function
#End Region
End Class
