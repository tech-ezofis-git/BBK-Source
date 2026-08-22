Imports System.Data
Imports System.Configuration
Imports System.Web
''' <summary>
''' Summary description for ControlNameGroup
''' </summary>
''' 
Public Class eZFormControlDetail
    Inherits IDatabaseCommonItems
    Implements IeZFormControlDetail
    Protected _ControlId As Integer
    Protected _ControlName As String
    Protected _FormId As Integer
    Protected _OrderId As Double
    Protected _ControlTypeId As Integer
    Protected _DataType As Integer
    Protected _ValidationId As Integer
    Protected _TabIndex As Integer
    Protected _style As String
    Protected _TableTagType As String
    Protected _GridRow As Integer
    Protected _GridColumn As Integer
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

    Public Sub New(tmpControlId As Integer)
        Me._ControlId = tmpControlId
    End Sub
    Public Sub New(tmpControlName As String)
        Me._ControlName = tmpControlName
    End Sub

    Public Sub New()
    End Sub
    Public Property ControlId() As Integer Implements IeZFormControlDetail.ControlId
        Get
            If _ControlId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ControlId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ControlId <> 0 AndAlso _ControlId <> value Then
                Throw New MemberAccessException()
            End If
            _ControlId = value
        End Set
    End Property
    Public Property DataType() As Integer Implements IeZFormControlDetail.DataType
        Get
            If _DataType = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _DataType
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _DataType <> 0 AndAlso _DataType <> value Then
                Throw New MemberAccessException()
            End If
            _DataType = value
        End Set
    End Property
    Public Property ValidationId() As Integer Implements IeZFormControlDetail.ValidationId
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
    Public Property TabIndex() As Integer Implements IeZFormControlDetail.TabIndex
        Get
            If _TabIndex = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _TabIndex
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _TabIndex <> 0 AndAlso _TabIndex <> value Then
                Throw New MemberAccessException()
            End If
            _TabIndex = value
        End Set
    End Property
    Public Property GridRow() As Integer Implements IeZFormControlDetail.GridRow
        Get
            If _GridRow = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _GridRow
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _GridRow <> 0 AndAlso _GridRow <> value Then
                Throw New MemberAccessException()
            End If
            _GridRow = value
        End Set
    End Property
    Public Property GridColumn() As Integer Implements IeZFormControlDetail.GridColumn
        Get
            If _GridColumn = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _GridColumn
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _GridColumn <> 0 AndAlso _GridColumn <> value Then
                Throw New MemberAccessException()
            End If
            _GridColumn = value
        End Set
    End Property
    Public Property ControlTypeId() As Integer Implements IeZFormControlDetail.ControlTypeId
        Get
            If _ControlTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ControlTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ControlTypeId <> 0 AndAlso _ControlTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _ControlTypeId = value
        End Set
    End Property
    Public Property OrderId() As Double Implements IeZFormControlDetail.OrderId
        Get
            If _OrderId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _OrderId
        End Get
        Set(value As Double)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _OrderId <> 0 AndAlso _OrderId <> value Then
                Throw New MemberAccessException()
            End If
            _OrderId = value
        End Set
    End Property
    Public Property FormId() As Integer Implements IeZFormControlDetail.FormId
        Get
            If _FormId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FormId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FormId <> 0 AndAlso _FormId <> value Then
                Throw New MemberAccessException()
            End If
            _FormId = value
        End Set
    End Property

    Public Property ControlName() As String Implements IeZFormControlDetail.ControlName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ControlName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ControlName = value Then
                Return
            End If
            _ControlName = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZFormControlDetail.UpdatedBy1
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
    Public Property style() As String Implements IeZFormControlDetail.style
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _style
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _style = value Then
                Return
            End If
            _style = value
            IsModified = True
        End Set
    End Property
    Public Property TableTagType() As String Implements IeZFormControlDetail.TableTagType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TableTagType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TableTagType = value Then
                Return
            End If
            _TableTagType = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedBy1() As String Implements IeZFormControlDetail.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZFormControlDetail.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZFormControlDetail.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZFormControlDetail.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZFormControlDetail.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZFormControlDetail.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsControlNameExist() As Boolean Implements IeZFormControlDetail.IsControlNameExist
        Get
            Return (ControlId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
