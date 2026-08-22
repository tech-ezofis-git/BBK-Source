Imports System.Collections.Generic
Imports System.Text
Public Interface IeZClientAppproval
    Inherits IDatabaseItems

    Property ClientApprovalId() As Integer
    Property ConfigPrimeId() As Integer
    Property Approval() As String
    Property Appprime() As String
    Property ApprovalCode() As String
    Property ISA() As Integer
    Property PrimeOn() As String
    Property PrimeCount() As String
    Property PrimeDepart() As String
    Property Active() As Integer
    Property CreatedOn() As String
    Property UpdatedOn() As String
    Property CreatedBy() As String
    Property UpdatedBy() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property IsDeleted() As Integer
    Property UserId() As Integer


End Interface
