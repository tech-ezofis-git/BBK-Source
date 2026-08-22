Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZFieldAlertDetail
    Inherits IDatabaseCommonItems
    Implements IeZFieldAlertDetail
    Protected _FieldAlertDetailId As Integer
    Protected _ToMail As String
    Protected _FieldAlertName As String
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

    Public Sub New(tmpFieldAlertDetailId As Integer)
        Me._FieldAlertDetailId = tmpFieldAlertDetailId
    End Sub
    Public Sub New(tmpFieldAlertDetail As String)
        Me._FieldAlertName = tmpFieldAlertDetail
    End Sub

    Public Sub New()
    End Sub
    Public Property FieldAlertDetailId() As Integer Implements IeZFieldAlertDetail.FieldAlertDetailId
        Get
            If _FieldAlertDetailId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FieldAlertDetailId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FieldAlertDetailId <> 0 AndAlso _FieldAlertDetailId <> value Then
                Throw New MemberAccessException()
            End If
            _FieldAlertDetailId = value
        End Set
    End Property
   
    Public Property ToMail() As String Implements IeZFieldAlertDetail.ToMail
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToMail
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ToMail = value Then
                Return
            End If
            _ToMail = value
            IsModified = True
        End Set
    End Property
    Public Property FieldAlertName() As String Implements IeZFieldAlertDetail.FieldAlertName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FieldAlertName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FieldAlertName = value Then
                Return
            End If
            _FieldAlertName = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZFieldAlertDetail.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZFieldAlertDetail.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZFieldAlertDetail.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZFieldAlertDetail.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZFieldAlertDetail.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZFieldAlertDetail.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZFieldAlertDetail.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsFieldAlertDetailExist() As Boolean Implements IeZFieldAlertDetail.IsFieldAlertDetailExist
        Get
            Return (FieldAlertDetailId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
