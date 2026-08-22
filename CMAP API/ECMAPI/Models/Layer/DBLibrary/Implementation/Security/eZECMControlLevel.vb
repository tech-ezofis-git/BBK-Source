Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI

Public Class eZECMControlLevel
    Inherits IDatabaseCommonItems
    Implements IeZECMControlLevel

    Protected _ECMControlLevelId As Integer

    Protected _ECMControlId As Integer
    Protected _ECMProfileId As Integer
    Protected _ECMControl As String = ""
    Protected _ECMControlType As Integer
    Protected _templateid As Integer
    Protected _templatename As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._ECMControlLevelId = DeptId
    End Sub

    Public Sub New()
    End Sub

    Public Property ECMControlLevelId() As Integer Implements IeZECMControlLevel.ECMControlLevelId
        Get
            If _ECMControlLevelId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ECMControlLevelId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ECMControlLevelId <> 0 AndAlso _ECMControlLevelId <> value Then
                Throw New MemberAccessException()
            End If
            _ECMControlLevelId = value
        End Set
    End Property
    Public Property ECMControlId() As Integer Implements IeZECMControlLevel.ECMControlId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMControlId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMControlId = value Then
                Return
            End If
            _ECMControlId = value
            IsModified = True
        End Set
    End Property

    'udaya
    Public Property ECMProfileId() As Integer Implements IeZECMControlLevel.ECMProfileId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMProfileId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMProfileId = value Then
                Return
            End If
            _ECMProfileId = value
            IsModified = True
        End Set
    End Property

    Public Property ECMControl() As String Implements IeZECMControlLevel.ECMControl
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMControl
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ECMControl = value Then
                Return
            End If
            _ECMControl = value
            IsModified = True
        End Set
    End Property

    Public Property ECMControlType() As Integer Implements IeZECMControlLevel.ECMControlType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMControlType
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMControlType = value Then
                Return
            End If
            _ECMControlType = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZECMControlLevel.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZECMControlLevel.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZECMControlLevel.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZECMControlLevel.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZECMControlLevel.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZECMControlLevel.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZECMControlLevel.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZECMControlLeveltExist() As Boolean Implements IeZECMControlLevel.IseZECMControlLevelExist
        Get
            Return (_ECMControlLevelId > 0)
        End Get
    End Property

    Public Property templatename As String Implements IeZECMControlLevel.templatename
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _templatename
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _templatename = value Then
                Return
            End If

            _templatename = value
        End Set
    End Property

    Public Property templateid As Integer Implements IeZECMControlLevel.templateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _templateid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _templateid = value Then
                Return
            End If

            _templateid = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
