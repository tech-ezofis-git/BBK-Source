Imports System.Collections.Generic
Imports System.Text
Public Interface IeZERSInfo
    Inherits IDatabaseItems
    Property ERSId() As Integer
    Property ERSName() As String
    Property ERSServerName() As String
    Property ERSDirPath() As String
    Property SettingPath() As String
    Property ERSIndexinpath() As String
    Property IsMain() As Boolean
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsERSInfoExist() As Boolean
End Interface
