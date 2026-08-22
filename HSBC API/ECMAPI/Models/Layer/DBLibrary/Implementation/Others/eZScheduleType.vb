Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZScheduleType
    Inherits IDatabaseCommonItems
    Implements IeZScheduleType
    Protected _ScheduleTypeId As Integer
    Protected _ScheduleType As String
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

    Public Sub New(tmpScheduleTypeId As Integer)
        Me._ScheduleTypeId = tmpScheduleTypeId
    End Sub
    Public Sub New(tmpScheduleType As String)
        Me._ScheduleType = tmpScheduleType
    End Sub

    Public Sub New()
    End Sub
    Public Property ScheduleTypeId() As Integer Implements IeZScheduleType.ScheduleTypeId
        Get
            If _ScheduleTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ScheduleTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ScheduleTypeId <> 0 AndAlso _ScheduleTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _ScheduleTypeId = value
        End Set
    End Property

    Public Property ScheduleType() As String Implements IeZScheduleType.ScheduleType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ScheduleType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ScheduleType = value Then
                Return
            End If
            _ScheduleType = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZScheduleType.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZScheduleType.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZScheduleType.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZScheduleType.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZScheduleType.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZScheduleType.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZScheduleType.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsScheduleTypeExist() As Boolean Implements IeZScheduleType.IsScheduleTypeExist
        Get
            Return (ScheduleTypeId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
