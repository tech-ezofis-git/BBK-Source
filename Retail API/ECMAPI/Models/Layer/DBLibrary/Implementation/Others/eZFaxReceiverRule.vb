Imports System.Data
Imports System.Configuration
Imports System.Web


Public Class eZFaxReceiverRule
    Inherits IDatabaseCommonItems
    Implements IeZFaxReceiverRule
    Protected _FaxReceiverRuleId As Integer
    Protected _DisplayText As String = ""
    Protected _Hours As String = ""
    Protected _FaxReceiverRule As String
    Protected _DisplayFrom As Integer
    Protected _ValidityOfFax As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(FaxReceiverRuleId As Integer)
        Me._FaxReceiverRuleId = FaxReceiverRuleId
    End Sub
    Public Sub New()
    End Sub

    

    Public Property FaxReceiverRule() As String Implements IeZFaxReceiverRule.FaxReceiverRule
        Get

            DBLayer.DBLInstance.Read(Me)

            Return _FaxReceiverRule
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FaxReceiverRule = value Then
                Return
            End If
            _FaxReceiverRule = value
            IsModified = True
        End Set
    End Property

    Public Property DisplayFrom() As Integer Implements IeZFaxReceiverRule.DisplayFrom
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DisplayFrom
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _DisplayFrom = value Then
                Return
            End If
            _DisplayFrom = value
            IsModified = True
        End Set
    End Property
    Public Property DisplayText() As String Implements IeZFaxReceiverRule.DisplayText
        Get

            DBLayer.DBLInstance.Read(Me)
            Return _DisplayText
        End Get
        Set(value As String)
           DBLayer.DBLInstance.Read(Me)
            If _DisplayText = value Then
                Return
            End If
            _DisplayText = value
            IsModified = True
        End Set
    End Property
   
    Public Property Hours() As String Implements IeZFaxReceiverRule.Hours
        Get

            DBLayer.DBLInstance.Read(Me)
            Return _Hours
        End Get
        Set(value As String)
             DBLayer.DBLInstance.Read(Me)
            If _Hours = value Then
                Return
            End If
            _Hours = value
            IsModified = True
        End Set
    End Property
    Public Property ValidityOfFax() As String Implements IeZFaxReceiverRule.ValidityOfFax
        Get

            DBLayer.DBLInstance.Read(Me)
            Return _ValidityOfFax
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ValidityOfFax = value Then
                Return
            End If
            _ValidityOfFax = value
            IsModified = True
        End Set
    End Property
    Public Property FaxReceiverRuleId() As Integer Implements IeZFaxReceiverRule.FaxReceiverRuleId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FaxReceiverRuleId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FaxReceiverRuleId = value Then
                Return
            End If
            _FaxReceiverRuleId = value
            IsModified = True
        End Set
    End Property

   
    Public Property UpdatedBy1() As String Implements IeZFaxReceiverRule.UpdatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy1 = value Then
                Return
            End If
            _UpdatedBy1 = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedBy1() As String Implements IeZFaxReceiverRule.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy1 = value Then
                Return
            End If
            _CreatedBy1 = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedBy() As Integer Implements IeZFaxReceiverRule.CreatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy = value Then
                Return
            End If

            _CreatedBy = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedOn() As String Implements IeZFaxReceiverRule.CreatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedOn = value Then
                Return
            End If

            _CreatedOn = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy() As Integer Implements IeZFaxReceiverRule.UpdatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy = value Then
                Return
            End If

            _UpdatedBy = value
        End Set
    End Property
    Public Property UpdatedOn() As String Implements IeZFaxReceiverRule.UpdatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedOn = value Then
                Return
            End If

            _UpdatedOn = value
        End Set
    End Property
    Public ReadOnly Property Isdeleted() As Integer Implements IeZFaxReceiverRule.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsFaxReceiverRule() As Boolean Implements IeZFaxReceiverRule.IsFaxReceiverRule
        Get
            Return (_FaxReceiverRuleId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub




End Class
