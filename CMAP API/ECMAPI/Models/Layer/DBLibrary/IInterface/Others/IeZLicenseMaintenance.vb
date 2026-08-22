
Imports System.Collections.Generic
Imports System.Text
Public Interface IeZLicenseMaintenance
    Inherits IDatabaseItems
    Property Maintenance_Id As Integer
    Property Client_Name As String
    Property License_Key As String
    Property Keytype As String
    Property Created_On As String
    Property Created_by As Integer
    Property createdby1 As Integer
    Property Updated_On As String
    Property Updated_by As Integer
    Property updatedby1 As Integer
    ReadOnly Property isdeleted As Integer

End Interface
