Imports System.Collections.Generic
Imports System.Text
Public Interface IeZFoldersForTemp
    Inherits IDatabaseItems
    Property CabinetID() As Integer
    Property CabinetName() As String
    Property ParentNodeId() As Integer
    Property NodeId() As Integer
    Property UserId() As Integer
    Property NodeName() As String
    Property TableName As String
    Property TemplateId() As Integer
    Property LevelId() As Integer
    Property PathId() As String
    Property TemplateName() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZFoldersForTempExist() As Boolean
End Interface

