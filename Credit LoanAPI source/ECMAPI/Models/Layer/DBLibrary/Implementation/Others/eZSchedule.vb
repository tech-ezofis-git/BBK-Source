Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZSchedule
    Inherits IDatabaseCommonItems
    Implements IeZSchedule
    Protected _ScheduleId As Integer
    Protected _ScheduleTypeId As Integer
    Protected _ForSchedule As Integer
    Protected _Id As Integer
    Protected _WeekDay As Integer
    Protected _Mont As Integer
    Protected _Day As Integer
    Protected _EachDay As Integer
    Protected _OnceDate As DateTime
    Protected _Time As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer
    Public Sub New(tempScheduleTypeId As Integer)
        Me._ScheduleId = tempScheduleTypeId
    End Sub
    
    Public Sub New()
    End Sub
    Public Property Id() As Integer Implements IeZSchedule.Id
        Get
            If _Id = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Id
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Id <> 0 AndAlso _Id <> value Then
                Throw New MemberAccessException()
            End If
            _Id = value
        End Set
    End Property

    Public Property ForSchedule() As Integer Implements IeZSchedule.ForSchedule
        Get
            If _ForSchedule = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ForSchedule
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ForSchedule <> 0 AndAlso _ForSchedule <> value Then
                Throw New MemberAccessException()
            End If
            _ForSchedule = value
        End Set
    End Property

    Public Property ScheduleTypeId() As Integer Implements IeZSchedule.ScheduleTypeId
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

    Public Property Day() As Integer Implements IeZSchedule.Day
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Day
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Day = value Then
                Return
            End If
            _Day = value
            IsModified = True
        End Set
    End Property

    Public Property EachDay() As Integer Implements IeZSchedule.EachDay
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EachDay
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _EachDay = value Then
                Return
            End If
            _EachDay = value
            IsModified = True
        End Set
    End Property

    Public Property ScheduleId() As Integer Implements IeZSchedule.ScheduleId
        Get
            If _ScheduleId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ScheduleId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ScheduleId <> 0 AndAlso _ScheduleId <> value Then
                Throw New MemberAccessException()
            End If
            _ScheduleId = value
        End Set
    End Property

    Public Property Mont() As Integer Implements IeZSchedule.Mont
        Get
            If _Mont = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Mont
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Mont <> 0 AndAlso _Mont <> value Then
                Throw New MemberAccessException()
            End If
            _Mont = value
        End Set
    End Property
    Public Property WeekDay() As Integer Implements IeZSchedule.WeekDay
        Get
            If _WeekDay = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _WeekDay
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _WeekDay <> 0 AndAlso _WeekDay <> value Then
                Throw New MemberAccessException()
            End If
            _WeekDay = value
        End Set
    End Property
    Public Property Time() As String Implements IeZSchedule.Time
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Time
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Time = value Then
                Return
            End If
            _Time = value
            IsModified = True
        End Set
    End Property
    Public Property OnceDate() As DateTime Implements IeZSchedule.OnceDate
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _OnceDate
        End Get
        Set(value As DateTime)
            DBLayer.DBLInstance.Read(Me)
            If _OnceDate = value Then
                Return
            End If
            _OnceDate = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZSchedule.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZSchedule.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZSchedule.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZSchedule.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZSchedule.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZSchedule.UpdatedOn
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
            IsModified = True
        End Set
    End Property
    Public ReadOnly Property Isdeleted() As Integer Implements IeZSchedule.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZScheduleExist() As Boolean Implements IeZSchedule.IseZScheduleExist
        Get
            Return (_ScheduleTypeId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class

