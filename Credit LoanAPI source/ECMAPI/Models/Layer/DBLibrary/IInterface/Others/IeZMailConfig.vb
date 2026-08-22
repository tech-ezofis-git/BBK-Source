Imports System.Collections.Generic
Imports System.Text

Public Interface IeZMailConfig
    Inherits IDatabaseItems

    Property MailConfigId() As Integer
    Property Host() As String
    Property Port() As Integer
    Property Mailid() As String
    Property UserName() As String
    Property Password() As String
    ReadOnly Property EnableSSL() As Boolean
    Property CreatedOn() As String
    Property UpdatedOn() As String
    Property CreatedBy() As Integer
    Property UpdatedBy() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Boolean

End Interface
