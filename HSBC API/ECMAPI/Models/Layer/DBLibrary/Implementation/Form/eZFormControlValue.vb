Imports System.Data
Imports System.Configuration
Imports System.Web
''' <summary>
''' Summary description for ControlValueGroup
''' </summary>
Public Class eZFormControlValue
    Inherits IDatabaseCommonItems
    Implements IeZFormControlValue
    Protected _ControlValueId As Integer
    Protected _ControlValue As String
    Protected _FormControlId As Integer
    Protected _RefControlId As Integer
    Protected _RefControlValueId As Integer
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

    Public Sub New(tmpControlValueId As Integer)
        Me._ControlValueId = tmpControlValueId
    End Sub
    Public Sub New(tmpControlValue As String)
        Me._ControlValue = tmpControlValue
    End Sub

    Public Sub New()
    End Sub
    Public Property ControlValueId() As Integer Implements IeZFormControlValue.ControlValueId
        Get
            If _ControlValueId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ControlValueId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ControlValueId <> 0 AndAlso _ControlValueId <> value Then
                Throw New MemberAccessException()
            End If
            _ControlValueId = value
        End Set
    End Property
    Public Property RefControlValueId() As Integer Implements IeZFormControlValue.RefControlValueId
        Get
            If _RefControlValueId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _RefControlValueId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _RefControlValueId <> 0 AndAlso _RefControlValueId <> value Then
                Throw New MemberAccessException()
            End If
            _RefControlValueId = value
        End Set
    End Property
    Public Property RefControlId() As Integer Implements IeZFormControlValue.RefControlId
        Get
            If _RefControlId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _RefControlId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _RefControlId <> 0 AndAlso _RefControlId <> value Then
                Throw New MemberAccessException()
            End If
            _RefControlId = value
        End Set
    End Property
   
    Public Property FormControlId() As Integer Implements IeZFormControlValue.FormControlId
        Get
            If _FormControlId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FormControlId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FormControlId <> 0 AndAlso _FormControlId <> value Then
                Throw New MemberAccessException()
            End If
            _FormControlId = value
        End Set
    End Property

    Public Property ControlValue() As String Implements IeZFormControlValue.ControlValue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ControlValue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ControlValue = value Then
                Return
            End If
            _ControlValue = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZFormControlValue.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZFormControlValue.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZFormControlValue.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZFormControlValue.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZFormControlValue.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZFormControlValue.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZFormControlValue.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsControlValueExist() As Boolean Implements IeZFormControlValue.IsControlValueExist
        Get
            Return (ControlValueId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
