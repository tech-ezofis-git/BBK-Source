Imports System.Collections.Generic
Imports System.Text
Public Interface IeZLookupSPparameters
    Inherits IDatabaseItems
    Property LookupSPparamId() As Integer
    Property ECMField() As String
    Property VariableDataType() As String
    Property ParameterName() As String
    Property LookupId() As Integer
    Property IsOutputParameterDirection() As Boolean
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZLookupSPparameters() As Boolean
End Interface
