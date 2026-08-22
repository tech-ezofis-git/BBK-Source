Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZFormValidation
    Inherits IDatabaseCommonItems
    Implements IeZFormValidation
    Protected _ValidationId As Integer
    Protected _ValidationName As String
    Protected _FunctionName As String
    Protected _OnEvent As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpValidationId As Integer)
        Me._ValidationId = tmpValidationId
    End Sub
    Public Sub New()
    End Sub

    Public Property ValidationId() As Integer Implements IeZFormValidation.ValidationId
        Get
            If _ValidationId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ValidationId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ValidationId <> 0 AndAlso _ValidationId <> value Then
                Throw New MemberAccessException()
            End If
            _ValidationId = value
        End Set
    End Property

    Public Property ValidationName() As String Implements IeZFormValidation.ValidationName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ValidationName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ValidationName = value Then
                Return
            End If
            _ValidationName = value
            IsModified = True
        End Set
    End Property

    Public Property FunctionName() As String Implements IeZFormValidation.FunctionName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FunctionName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FunctionName = value Then
                Return
            End If
            _FunctionName = value
            IsModified = True
        End Set
    End Property

    Public Property OnEvent() As String Implements IeZFormValidation.OnEvent
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _OnEvent
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _OnEvent = value Then
                Return
            End If
            _OnEvent = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZFormValidation.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZFormValidation.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZFormValidation.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZFormValidation.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZFormValidation.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZFormValidation.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZFormValidation.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
