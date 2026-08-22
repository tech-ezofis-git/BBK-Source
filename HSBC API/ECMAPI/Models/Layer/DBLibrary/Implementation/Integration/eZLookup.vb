Imports System.Data
Imports System.Configuration
Imports System.Web


Public Class eZLookup
    Inherits IDatabaseCommonItems
    Implements IeZLookup
    Protected _LookupId As Integer
    Protected _LookupTypeId As Integer
    Protected _LookupServerTypeId As Integer
    Protected _LookupConnStrId As Integer
    Protected _LookupType As String
    Protected _TemplateId As Integer
    Protected _ConnectionString As String
    Protected _Scheduletime As String = "Live Sync"
    Protected _Schedule As Integer = 0
    Protected _LookupValue As String
    Protected _lookupname As String
    Protected _Lookupconnectionname As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(LookupId As Integer)
        Me._LookupId = LookupId
    End Sub
    Public Sub New()
    End Sub
    Public Property LookupValue() As String Implements IeZLookup.LookupValue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupValue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LookupValue = value Then
                Return
            End If
            _LookupValue = value
            IsModified = True
        End Set
    End Property
    Public Property Lookupconnectionname() As String Implements IeZLookup.Lookupconnectionname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Lookupconnectionname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Lookupconnectionname = value Then
                Return
            End If
            _Lookupconnectionname = value
            IsModified = True
        End Set
    End Property

    Public Property Scheduletime() As String Implements IeZLookup.Scheduletime
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Scheduletime
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Scheduletime = value Then
                Return
            End If
            _Scheduletime = value
            IsModified = True
        End Set
    End Property
    Public Property schedule() As Integer Implements IeZLookup.schedule
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Schedule
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Schedule = value Then
                Return
            End If
            _Schedule = value
            IsModified = True
        End Set
    End Property
    Public Property lookupname() As String Implements IeZLookup.lookupname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _lookupname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _lookupname = value Then
                Return
            End If
            _lookupname = value
            IsModified = True
        End Set
    End Property

    Public Property LookupType() As String Implements IeZLookup.LookupType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LookupType = value Then
                Return
            End If
            _LookupType = value
            IsModified = True
        End Set
    End Property
    Public Property ConnectionString() As String Implements IeZLookup.ConnectionString
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ConnectionString
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ConnectionString = value Then
                Return
            End If
            _ConnectionString = value
            IsModified = True
        End Set
    End Property


    Public Property LookupConnStrId() As Integer Implements IeZLookup.LookupConnStrId
        Get
            If _LookupConnStrId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LookupConnStrId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LookupConnStrId <> 0 AndAlso _LookupConnStrId <> value Then
                Throw New MemberAccessException()
            End If
            _LookupConnStrId = value
        End Set
    End Property
    Public Property LookupTypeId() As Integer Implements IeZLookup.LookupTypeId
        Get
            If _LookupTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LookupTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LookupTypeId <> 0 AndAlso _LookupTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _LookupTypeId = value
        End Set
    End Property
    Public Property LookupServerTypeId() As Integer Implements IeZLookup.LookupServerTypeId
        Get
            If _LookupServerTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LookupServerTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LookupServerTypeId <> 0 AndAlso _LookupServerTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _LookupServerTypeId = value
        End Set
    End Property



    Public Property TemplateId() As Integer Implements IeZLookup.TemplateId
        Get
            If _TemplateId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _TemplateId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _TemplateId <> 0 AndAlso _TemplateId <> value Then
                Throw New MemberAccessException()
            End If
            _TemplateId = value
        End Set
    End Property
    Public Property LookupId() As Integer Implements IeZLookup.LookupId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LookupId = value Then
                Return
            End If
            _LookupId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZLookup.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZLookup.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZLookup.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZLookup.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZLookup.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZLookup.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZLookup.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZLookup() As Boolean Implements IeZLookup.IseZLookup
        Get
            Return (_LookupId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub




End Class
