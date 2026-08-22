Public Interface IInstance
#Region "Login Details"
    Function eZRegistration(CompanyId As Integer) As IeZRegistration
    Function eZLicenseClients(LicenseClientId As Integer) As IeZLicenseClients
    Function eZLicense(LicenseId As Integer) As IeZLicense
    Function eZTrialLicense(TrialId As Integer) As IeZTrialLicense
    Function eZApplication(ApplicationId As Integer) As IeZApplication
    Function eZWorkFlow(WorkFlowId As Integer) As IeZWorkFlow
    Function eZWorkFlowType(WorkFlowTypeId As Integer) As IeZWorkFlowType
    'Function eZWorkflowDetails(WorkFlowId As Integer) As IeZWorkflowDetails
    Function eZWorkFlowRelation(RelationId As Integer) As IeZWorkFlowRelation
    Function eZWorkFlowProcess(ProcessId As Integer) As IeZWorkFlowProcess
    Function eZIntegrationDetail(IntegrationId As Integer) As IeZIntegrationDetail
    Function eZAlternateField(eZAlternateFieldId As Integer) As IeZAlternateField
    Function eZFaxReceiverRule(eZFaxReceiverRuleId As Integer) As IeZFaxReceiverRule
    Function eZFaxReceiver(eZFaxReceiverId As Integer) As IeZFaxReceiver
    Function eZFax(eZFaxId As Integer) As IeZFax
    Function eZZonal(eZZonalId As Integer) As IeZZonal
    Function eZBookMarksDetail(eZBookMarksDetailId As Integer) As IeZBookMarksDetail
    Function eZFaxTransaction(eZFaxTransactionId As Integer) As IeZFaxTransaction
    Function eZInboxTransaction(eZInboxTransactionId As Integer) As IeZInboxTransaction
    Function eZBookMarks(eZBookMarksId As Integer) As IeZBookMarks
    Function eZMail(eZMailId As Integer) As IeZMail

    Function eZTask(eZTaskId As Integer) As IeZTask
    Function eZTaskType(Typeid As Integer) As IeZTaskType
    Function eZMailArchive(eZMailArchiveId As Integer) As IeZMailArchive
    Function eZOutlookContact(eZOutlookContactId As Integer) As IeZOutlookContact
    Function eZAllottedTask(eZAllottedTaskId As Integer) As IeZAllottedTask
    Function eZTaskUsers(eZTaskUsersId As Integer) As IeZTaskUsers
    Function eZECMUserInfo(UserId As Integer) As IeZECMUserInfo
    Function eZECMLogin(Login_ID As Integer) As IOldeZECMLogin
    Function eZProfile(ProfileId As Integer) As IeZProfile
    Function eZECMGroup(ECMGroupId As Integer) As IeZECMGroup
    Function eZECMGroupusers(ECMGroupUserId As Integer) As IeZECMGroupusers
    Function eZSchedule(ScheduleId As Integer) As IeZSchedule
    Function eZReminder(ReminderId As Integer) As IeZReminder
    Function eZECMCabinetLevel(ECMCabinetLevelId As Integer) As IeZECMCabinetLevel
    Function eZECMDocumentLevel(ECMDocumentLevelId As Integer) As IeZECMDocumentLevel
    Function eZECMFieldLevel(ECMFieldLevelId As Integer) As IeZECMFieldLevel
    Function eZECMControlLevel(ECMControlLevelId As Integer) As IeZECMControlLevel
    Function ezImpersonation(ImpersonateId As Integer) As IezImpersonation
    Function eZECMControl(ECMControlId As Integer) As IeZECMControl
    Function eZECMProfile(ECMProfileId As Integer) As IeZECMProfile
    Function eZCabOwners(CabOwnerId As Integer) As IeZCabOwners
    Function eZERSInfo(ERSId As Integer) As IeZERSInfo
    Function eZERSSync(eZERSSyncid As Integer) As IeZERSSync
    Function ezerssync_History(eZERSSyncid As Integer) As IeZERSSync_History
    Function eZCabinet(eZCabinetId As Integer) As IeZCabinet
    Function eZTemplate(eZTemplateId As Integer) As IeZTemplate
    Function eZDuplicateType(eZDuplicateTypeId As Integer) As IeZDuplicateType
    Function eZECMUserType(eZECMUserTypeId As Integer) As IeZECMUserType
    Function eZLanguage(eZLanguageId As Integer) As IeZLanguage
    Function eZCondition(eZConditionId As Integer) As IeZCondition
    Function eZAlertCondition(eZAlertConditionId As Integer) As IeZAlertCondition
    Function eZBodyHtmlType(eZBodyHtmlTypeId As Integer) As IeZBodyHtmlType
    Function eZChart(eZChartId As Integer) As IeZChart
    Function eZDocumentAlert(eZDocumentAlertId As Integer) As IeZDocumentAlert
    Function eZAlert(eZAlertId As Integer) As IeZAlert
    Function eZFieldAlert(eZFieldAlertId As Integer) As IeZFieldAlert
    Function eZFieldAlertDetail(eZFieldAlertDetailId As Integer) As IeZFieldAlertDetail
    Function eZFieldAlertDoc(FieldAlertDocId As Integer) As IeZFieldAlertDoc
    Function eZFieldAlertTemp(Id As Integer) As IeZFieldAlertTemp
    Function eZScheduleType(eZScheduleTypeId As Integer) As IeZScheduleType
    Function eZMailArchiveType(eZMailArchiveTypeId As Integer) As IeZMailArchiveType
    Function eZLookupServerType(eZLookupServerTypeId As Integer) As IeZLookupServerType
    Function eZLookupType(eZLookupTypeId As Integer) As IeZLookupType
    Function eZLookupConnection(LookupType As Integer) As IeZLookupConnection
    Function eZTempDatatype(eZTempDatatypeId As Integer) As IeZTempDatatype
    Function eZBarcodeType(eZBarcodeTypeId As Integer) As IeZBarcodeType
    Function eZChartType(eZChartTypeId As Integer) As IeZChartType
    Function eZTemplateField(eZTemplateFieldId As Integer) As IeZTemplateField
    Function eZTemplateUserFields(UserFieldId As Integer) As IeZTemplateUserFields
    Function eZTempBarcode(eZTempBarcodeId As Integer) As IeZTempBarcode
    Function eZPdfProperties(eZPdfPropertiesId As Integer) As IeZPdfProperties
    Function eZERSIPs(eZERSIPsId As Integer) As IeZERSIPs
    Function eZFolders(eZFoldersId As Integer) As IeZFolders
    Function ezFoldersByUser(NodeId As Integer) As IezFoldersByUser
    Function eZPrivateFolders(Privatefolderid As Integer) As IeZPrivateFolders
    Function eZFoldersForTemp(eZFoldersForTempId As Integer) As IeZFoldersForTemp
    Function eZHierarchy(Hierarchy_id As Integer) As IeZHierarchy
    Function eZIndexingChange(Indexingchangeid As Integer) As IeZIndexingChange
    Function eZInbox(eZInboxId As Integer) As IeZInbox
    Function eZLookup(LookupId As Integer) As IeZLookup
    Function ezlookupsynchistory(synchistoryid As Integer) As Iezlookupsynchistory
    Function eZLookupFields(LookupFieldId As Integer) As IeZLookupFields
    Function eZLookupClientField(LookupFieldId As Integer) As IeZLookupClientField
    Function eZLookupSPparameters(LookupSPparamId As Integer) As IeZLookupSPparameters
    Function eZDocumentLink(LinkId As Integer) As IeZDocumentLink
    Function eZFilesCopyLink(CopyLinkId As Integer) As IeZFilesCopyLink
    Function eZComments(CommentsId As Integer) As IeZComments
    Function eZOutlooksync(outlooksyncid As Integer) As IeZOutlooksync
    Function eZOutlooksync_histroy(Outlooksync_historyid As Integer) As IeZOutlookSync_History
    Function eZtest(outlooksyncid As Integer) As ieZtest
    Function eZDtSearchPath(indexpathid As Integer) As IeZDtSearchPath
    Function eZOutlookDetail(Outlookdetailid As Integer) As IeZOutlookDetail
#End Region
#Region "Form Details"
    Function eZFormControlDetail(eZFormControlDetailId As Integer) As IeZFormControlDetail
    Function eZFormControlValue(eZFormControlValueId As Integer) As IeZFormControlValue
    Function eZFormDetails(eZFormDetailsId As Integer) As IeZFormDetails
    Function eZFrmControlDataType(eZFrmControlDataTypeId As Integer) As IeZFrmControlDataType
    Function eZFrmControlType(eZFrmControlTypeId As Integer) As IeZFrmControlType
    Function eZFormValidation(ValidationId As Integer) As IeZFormValidation
#End Region
    Function SyncTable(Syncid As Integer) As ISyncTable
    Function ezlinkeditems(p1 As Integer) As IeZLinkedItems
#Region "License"
    Function eZClient(clientid As Integer) As IeZClient
    Function ezclientapproval(clientapprovalid As Integer) As IeZClientAppproval
#End Region
#Region "Files"
    Function eZHideFile(hidefileid As Integer) As IeZHideFile
    Function eZHideFileUsers(hidefileuserid As Integer) As IeZHideFileUsers
    Function eZHidePages(hideid As Integer) As IeZHidePages
    Function ezSupportFiles(Attachmentid As Integer) As IezSupportFiles
#End Region
#Region "Vault"
    Function eZVault(ezvaultid As Integer) As IeZVault
#End Region
#Region "WorkFlow"
    Function eZProcessItems(processitemsid As Integer) As IeZProcessItems
    Function eZWorkflowUsers(workflowusersid As Integer) As IeZWorkflowUsers
    Function eZFormUsers(formusersid As Integer) As IeZFormUsers
    Function eZWFlowFormDetails(formdetailsid As Integer) As IeZWFlowFormDetails
    Function eZWFlowTransation(TransactionId As Integer) As IeZWFlowTransation
    Function eZWFProcess(ProcessId As Integer) As IeZWFProcess
    Function eZWorkflowDetails(WorkflowId As Integer) As IeZWorkflowDetails
#End Region
#Region "eZMailSettings"
    Function eZMailSettings(SettingId As Integer) As IeZMailSettings
    Function eZMailArchiveValue(MailArchiveValueId As Integer) As IeZMailArchiveValue
    Function eZMailTriggering(MailTriggerid As Integer) As IeZMailTriggering
    Function ezMailTriggerTypes(TriggerTypeid As Integer) As IezMailTriggerTypes
    Function eZMailType(MailTypeId As Integer) As IeZMailType
    Function eZMailWatching(mailwatchingid As Integer) As IeZMailWatching
    Function eZMailWatchingCondition(conditionid As Integer) As IeZMailWatchingCondition
    Function eZMailWatchingDetails(sendid As Integer) As IeZMailWatchingDetails
    Function eZMailWatchingStatus(MailWatchingId As Integer) As IeZMailWatchingStatus
    Function eZUnAllocatedMail(MailRequestId As Integer) As IeZUnAllocatedMail
#End Region
#Region "Active Directory"
    Function eZLdapConnection(LdapConnId As Integer) As IeZLdapConnection
    Function eZADUsers(LdapUserId As Integer) As IeZADUsers
    Function eZRCContacts(eZContactId As Integer) As IeZRCContacts
#End Region
#Region "FolderMonitor"
    Function eZFolderMonitor(Monitorid As Integer) As IeZFolderMonitor
    Function eZUnProcessedFiles(UnprocessId As Integer) As IeZUnProcessedFiles
#End Region
#Region "Profile Security"
    Function eZECMProfileTemplate(ProfileTemplateId As Integer) As IeZECMProfileTemplate
    Function eZECMProfileUsers(ECMProfileUsersId As Integer) As IeZECMProfileUsers
#End Region
#Region "Error"
    Function ErrorMessage() As IErrorMessage
#End Region
#Region "Collaboration"
    Function eZCollaboration(CollId As Integer) As IeZCollaboration
    Function eZCollaborationUserDetails(Id As Integer) As IeZCollaborationUserDetails
    Function eZTaskComments(CommentsId As Integer) As IeZTaskComments
#End Region
#Region "Map"
    Function eZMapFields(Mapfieldsid As Integer) As IeZMapFields
    Function eZMapLocation(LocationId As Integer) As IeZMapLocation
    Function eZMapTemplate(MapTemplateId As Integer) As IeZMapTemplate
#End Region
#Region "Notification"
    Function ezNotification(NotificationId As Integer) As IezNotification
#End Region
#Region "Scan"
    Function eZScanBatch(BatchId As Integer) As IeZScanBatch
    Function ezScannedImg(ScannedImgId As Integer) As IezScannedImg
    Function eZScanSettings(SettingId As Integer) As IeZScanSettings
#End Region
#Region "Schedule"
    Function eZScheduleDetail(Detailid As Integer) As IeZScheduleDetail
    Function eZScheduleFor(ForScheduleId As Integer) As IeZScheduleFor
    Function ezEscalation(EscalationId As Integer) As IezEscalation
    Function ezEscalationHistory(EscalationHistoryId As Integer) As IezEscalationHistory
    Function ezEscalationUser(EscalationUserId As Integer) As IezEscalationUser
#End Region
#Region "Retention"
    Function ezRetentionRule(RetentionId As Integer) As IezRetentionRule
    Function ezRetentionMail(RetMailId As Integer) As IezRetentionMail
#End Region
End Interface
