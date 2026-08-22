Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZDuplicateType
    Inherits IDatabaseCommonItems
    Implements IeZDuplicateType
    Protected _DuplicateTypeId As Integer
    Protected _DuplicateType As String
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

    Public Sub New(tmpDuplicateTypeId As Integer)
        Me._DuplicateTypeId = tmpDuplicateTypeId
    End Sub
    Public Sub New(tmpDuplicateType As String)
        Me._DuplicateType = tmpDuplicateType
    End Sub

    Public Sub New()
    End Sub
    Public Property DuplicateTypeId() As Integer Implements IeZDuplicateType.DuplicateTypeId
        Get
            If _DuplicateTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _DuplicateTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _DuplicateTypeId <> 0 AndAlso _DuplicateTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _DuplicateTypeId = value
        End Set
    End Property

    Public Property DuplicateType() As String Implements IeZDuplicateType.DuplicateType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DuplicateType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _DuplicateType = value Then
                Return
            End If
            _DuplicateType = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZDuplicateType.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZDuplicateType.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZDuplicateType.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZDuplicateType.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZDuplicateType.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZDuplicateType.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZDuplicateType.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsDuplicateTypeExist() As Boolean Implements IeZDuplicateType.IsDuplicateTypeExist
        Get
            Return (DuplicateTypeId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
