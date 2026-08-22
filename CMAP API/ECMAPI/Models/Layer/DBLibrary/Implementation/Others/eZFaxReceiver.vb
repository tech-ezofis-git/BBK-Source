Imports System.Data
Imports System.Configuration
Imports System.Web


Public Class eZFaxReceiver
    Inherits IDatabaseCommonItems
    Implements IeZFaxReceiver
    Protected _FaxReceiverId As Integer
    Protected _ECMLoginId As Integer
    Protected _FaxReceiverRuleId As Integer
    Protected _DisplayText As String = ""
    Protected _Hours As String = ""
    Protected _RuleName As String = ""
    Protected _PrimaryUser As String = ""
    Protected _SecondaryUser As String = ""
    Protected _SenderType As String = ""
    Protected _FaxReceiverRule As String
    Protected _DisplayFrom As Integer
    Protected _IsPrimaryUser As Boolean
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(FaxReceiverId As Integer)
        Me._FaxReceiverId = FaxReceiverId
    End Sub
    Public Sub New()
    End Sub
    Public Property FaxReceiverRuleId() As Integer Implements IeZFaxReceiver.FaxReceiverRuleId
        Get
            If _FaxReceiverRuleId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FaxReceiverRuleId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FaxReceiverRuleId <> 0 AndAlso _FaxReceiverRuleId <> value Then
                Throw New MemberAccessException()
            End If
            _FaxReceiverRuleId = value
        End Set
    End Property

    Public Property IsPrimaryUser() As Boolean Implements IeZFaxReceiver.IsPrimaryUser
        Get
            If _IsPrimaryUser = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _IsPrimaryUser
        End Get
        Set(value As Boolean)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _IsPrimaryUser <> 0 AndAlso _IsPrimaryUser <> value Then
                Throw New MemberAccessException()
            End If
            _IsPrimaryUser = value
        End Set
    End Property

    Public Property FaxReceiverRule() As String Implements IeZFaxReceiver.FaxReceiverRule
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
    Public Property RuleName() As String Implements IeZFaxReceiver.RuleName
        Get

            DBLayer.DBLInstance.Read(Me)

            Return _RuleName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _RuleName = value Then
                Return
            End If
            _RuleName = value
            IsModified = True
        End Set
    End Property
    Public Property SenderType() As String Implements IeZFaxReceiver.SenderType
        Get

            DBLayer.DBLInstance.Read(Me)

            Return _SenderType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SenderType = value Then
                Return
            End If
            _SenderType = value
            IsModified = True
        End Set
    End Property
    Public Property PrimaryUser() As String Implements IeZFaxReceiver.PrimaryUser
        Get

            DBLayer.DBLInstance.Read(Me)

            Return _PrimaryUser
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _PrimaryUser = value Then
                Return
            End If
            _PrimaryUser = value
            IsModified = True
        End Set
    End Property
    Public Property SecondaryUser() As String Implements IeZFaxReceiver.SecondaryUser
        Get

            DBLayer.DBLInstance.Read(Me)

            Return _SecondaryUser
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SecondaryUser = value Then
                Return
            End If
            _SecondaryUser = value
            IsModified = True
        End Set
    End Property


    Public Property DisplayFrom() As Integer Implements IeZFaxReceiver.DisplayFrom
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
    Public Property DisplayText() As String Implements IeZFaxReceiver.DisplayText
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

    Public Property Hours() As String Implements IeZFaxReceiver.Hours
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

   
    Public Property ECMLoginId() As Integer Implements IeZFaxReceiver.ECMLoginId
        Get
            If _ECMLoginId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ECMLoginId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ECMLoginId <> 0 AndAlso _ECMLoginId <> value Then
                Throw New MemberAccessException()
            End If
            _ECMLoginId = value
        End Set
    End Property
   
   
    Public Property FaxReceiverId() As Integer Implements IeZFaxReceiver.FaxReceiverId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FaxReceiverId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FaxReceiverId = value Then
                Return
            End If
            _FaxReceiverId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZFaxReceiver.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZFaxReceiver.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZFaxReceiver.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZFaxReceiver.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZFaxReceiver.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZFaxReceiver.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZFaxReceiver.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsFaxReceiver() As Boolean Implements IeZFaxReceiver.IsFaxReceiver
        Get
            Return (_FaxReceiverId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub


End Class
