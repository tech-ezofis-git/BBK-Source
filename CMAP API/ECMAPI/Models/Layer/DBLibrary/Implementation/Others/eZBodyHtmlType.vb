Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZBodyHtmlType
    Inherits IDatabaseCommonItems
    Implements IeZBodyHtmlType
    Protected _BodyHtmlTypeId As Integer
    Protected _BodyHtmlType As String
    Protected _CreatedBy As Integer
    Protected _HtmlNamewithPath As String
    Protected _NoOfParameter As Integer
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

    Public Sub New(tmpBodyHtmlTypeId As Integer)
        Me._BodyHtmlTypeId = tmpBodyHtmlTypeId
    End Sub
    Public Sub New(tmpBodyHtmlType As String)
        Me._BodyHtmlType = tmpBodyHtmlType
    End Sub

    Public Sub New()
    End Sub
    Public Property BodyHtmlTypeId() As Integer Implements IeZBodyHtmlType.BodyHtmlTypeId
        Get
            If _BodyHtmlTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _BodyHtmlTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _BodyHtmlTypeId <> 0 AndAlso _BodyHtmlTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _BodyHtmlTypeId = value
        End Set
    End Property

    Public Property BodyHtmlType() As String Implements IeZBodyHtmlType.BodyHtmlType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _BodyHtmlType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _BodyHtmlType = value Then
                Return
            End If
            _BodyHtmlType = value
            IsModified = True
        End Set
    End Property
    Public Property HtmlNamewithPath() As String Implements IeZBodyHtmlType.HtmlNamewithPath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _HtmlNamewithPath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _HtmlNamewithPath = value Then
                Return
            End If
            _HtmlNamewithPath = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZBodyHtmlType.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZBodyHtmlType.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZBodyHtmlType.CreatedBy
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
    Public Property NoOfParameter() As Integer Implements IeZBodyHtmlType.NoOfParameter
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _NoOfParameter
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _NoOfParameter = value Then
                Return
            End If

            _NoOfParameter = value
            IsModified = True
        End Set
    End Property


    Public Property CreatedOn() As String Implements IeZBodyHtmlType.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZBodyHtmlType.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZBodyHtmlType.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZBodyHtmlType.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsBodyHtmlTypeExist() As Boolean Implements IeZBodyHtmlType.IsBodyHtmlTypeExist
        Get
            Return (BodyHtmlTypeId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
