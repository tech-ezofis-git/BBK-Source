Imports System.Collections.Generic
Imports System.Text
Public Interface IeZMail
    Inherits IDatabaseItems
    Property MailId() As Integer
    Property MailSettingId() As Integer
    'Property FromAdd() As String
    Property ToAdd() As String
    Property MailStatus() As Integer
    Property Subject() As String
    Property Body() As String
    Property BodyHtmlTypeId() As Integer
    Property AttachmentsPaths() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZMailExist() As Boolean
End Interface
