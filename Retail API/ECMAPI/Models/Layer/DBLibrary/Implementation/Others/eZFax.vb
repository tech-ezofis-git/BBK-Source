Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZFax
    Inherits IDatabaseCommonItems
    Implements IeZFax
    Protected _FaxId As Integer
    Protected _FaxReceiverRuleId As Integer
    Protected _FaxName As String
    Protected _FaxNumber As String
    Protected _FaxType As Integer
    Protected _FaxTypeValue As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CUserName As String
    Protected _CUserCode As String
    Protected _UUserName As String
    Protected _UUserCode As String
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpFaxId As Integer)
        Me._FaxId = tmpFaxId
    End Sub
    Public Sub New(tmpFaxNumber As String)
        Me._FaxNumber = tmpFaxNumber
    End Sub

    Public Sub New()
    End Sub

    Public Property FaxReceiverRuleId() As Integer Implements IeZFax.FaxReceiverRuleId
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
    Public Property FaxId() As Integer Implements IeZFax.FaxId
        Get
            If _FaxId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FaxId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FaxId <> 0 AndAlso _FaxId <> value Then
                Throw New MemberAccessException()
            End If
            _FaxId = value
        End Set
    End Property
   
    Public Property FaxType() As Integer Implements IeZFax.FaxType
        Get
            If _FaxType = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FaxType
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FaxType <> 0 AndAlso _FaxType <> value Then
                Throw New MemberAccessException()
            End If
            _FaxType = value
        End Set
    End Property
    Public Property FaxName() As String Implements IeZFax.FaxName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FaxName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FaxName = value Then
                Return
            End If
            _FaxName = value
            IsModified = True
        End Set
    End Property

    Public Property FaxTypeValue() As String Implements IeZFax.FaxTypeValue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FaxTypeValue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FaxTypeValue = value Then
                Return
            End If
            _FaxTypeValue = value

        End Set
    End Property
    Public Property FaxNumber() As String Implements IeZFax.FaxNumber
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FaxNumber
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FaxNumber = value Then
                Return
            End If
            _FaxNumber = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZFax.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZFax.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZFax.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZFax.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZFax.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZFax.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZFax.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsFaxExist() As Boolean Implements IeZFax.IsFaxExist
        Get
            Return (FaxId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
