Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZApplication
    Inherits IDatabaseCommonItems
    Implements IeZApplication
    Protected _ApplicationId As Integer
    Protected _ApplicationName As String
    Protected _AppVersion As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpApplicationId As Integer)
        Me._ApplicationId = tmpApplicationId
    End Sub
    Public Sub New()
    End Sub

    Public Property ApplicationId() As Integer Implements IeZApplication.ApplicationId
        Get
            If _ApplicationId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ApplicationId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ApplicationId <> 0 AndAlso _ApplicationId <> value Then
                Throw New MemberAccessException()
            End If
            _ApplicationId = value
        End Set
    End Property

    Public Property ApplicationName() As String Implements IeZApplication.ApplicationName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ApplicationName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ApplicationName = value Then
                Return
            End If
            _ApplicationName = value
            IsModified = True
        End Set
    End Property

    Public Property AppVersion() As String Implements IeZApplication.AppVersion
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _AppVersion
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _AppVersion = value Then
                Return
            End If
            _AppVersion = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZApplication.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZApplication.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZApplication.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZApplication.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZApplication.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZApplication.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZApplication.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
