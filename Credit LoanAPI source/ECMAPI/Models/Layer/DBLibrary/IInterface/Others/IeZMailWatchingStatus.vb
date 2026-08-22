Imports System.Collections.Generic
Imports System.Text
Public Interface IeZMailWatchingStatus
    Inherits IDatabaseItems
    Property Mailwatchingid() As Integer
    Property receivedtime() As DateTime
    Property ReceivedFrom() As String
    Property Keyword() As String
    Property sendid() As Integer
    Property MailsendStatus() As String
    Property MailsendTime() As DateTime
    Property CreatedOn() As String
    Property UpdatedOn() As String
    Property CreatedBy() As Integer
    Property UpdatedBy() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer
End Interface
