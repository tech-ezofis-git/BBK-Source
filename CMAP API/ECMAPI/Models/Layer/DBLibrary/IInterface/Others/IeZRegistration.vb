Imports System.Collections.Generic
Imports System.Text

Public Interface IeZRegistration
    Inherits IDatabaseItems
    Property CompanyId() As Integer
    Property CompanyName() As String
    Property StateName() As String
    Property Country() As String
    Property TypeOfIndustry() As String
    Property NoOfEmployees() As Integer
    Property EmpName() As String
    Property Phone() As String
    Property Designation() As String
    Property Email() As String
    Property Subscribe() As Integer
    Property AllowTeamToContact() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
