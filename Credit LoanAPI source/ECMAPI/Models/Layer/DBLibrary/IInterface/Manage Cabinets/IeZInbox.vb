Imports System.Collections.Generic
Imports System.Text
Public Interface IeZInbox
    Inherits IDatabaseItems
   
    Property ParentNodeId() As Integer
    Property NodeId() As Integer
    Property NodeName() As String
    Property LoginId() As Integer
    Property LevelId() As Integer
    Property PathId() As String
    Property LoginName() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZInboxExist() As Boolean
End Interface

