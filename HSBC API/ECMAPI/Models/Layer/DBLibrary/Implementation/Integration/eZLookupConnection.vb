Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZLookupConnection
    Inherits IDatabaseCommonItems
    Implements IeZLookupConnection

    Protected _LookupConnStrId As Integer
    Protected _LookupConnName As String
    Protected _LookupServerType As String
    Protected _ConnectionString As String
    Protected _DataSource As String
    Protected _UserId As String
    Protected _Pasword As String
    Protected _provider As String
    Protected _connectedlookup As String
    Protected _databasename As String = ""
    Protected _LookupServerTypeId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer
    Protected _conn As String = ""

    Public Sub New(DeptId As Integer)
        Me._LookupConnStrId = DeptId
    End Sub
    Public Sub New(tmpLookupConnName As String)
        Me._LookupConnName = tmpLookupConnName.Trim()
    End Sub
    Public Sub New()
    End Sub

    Public Property ConnectionString() As String Implements IeZLookupConnection.ConnectionString
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
    Public Property connectedlookup() As String Implements IeZLookupConnection.connectedlookup
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _connectedlookup
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _connectedlookup = value Then
                Return
            End If
            _connectedlookup = value
            IsModified = True
        End Set
    End Property
    Public Property databasename() As String Implements IeZLookupConnection.Databasename
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _databasename
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _databasename = value Then
                Return
            End If
            _databasename = value
            IsModified = True
        End Set
    End Property
    Public Property provider() As String Implements IeZLookupConnection.provider
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _provider
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _provider = value Then
                Return
            End If
            _provider = value
            IsModified = True
        End Set
    End Property

    Public Property DataSource() As String Implements IeZLookupConnection.DataSource
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DataSource
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _DataSource = value Then
                Return
            End If
            _DataSource = value
            IsModified = True
        End Set
    End Property
    Public Property UserId() As String Implements IeZLookupConnection.UserId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UserId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UserId = value Then
                Return
            End If
            _UserId = value
            IsModified = True
        End Set
    End Property
    Public Property LookupServerType() As String Implements IeZLookupConnection.LookupServerType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupServerType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LookupServerType = value Then
                Return
            End If
            _LookupServerType = value
            IsModified = True
        End Set
    End Property
    Public Property LookupConnStrId() As Integer Implements IeZLookupConnection.LookupConnStrId
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
    Public Property LookupConnName() As String Implements IeZLookupConnection.LookupConnName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupConnName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LookupConnName = value Then
                Return
            End If
            _LookupConnName = value
            IsModified = True
        End Set
    End Property
    Public Property Pasword() As String Implements IeZLookupConnection.Pasword
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Pasword
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Pasword = value Then
                Return
            End If
            _Pasword = value
            IsModified = True
        End Set
    End Property
    Public Property LookupServerTypeId() As Integer Implements IeZLookupConnection.LookupServerTypeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupServerTypeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LookupServerTypeId = value Then
                Return
            End If
            _LookupServerTypeId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZLookupConnection.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZLookupConnection.CreatedBy1
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
    Public Property conn() As String Implements IeZLookupConnection.conn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _conn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _conn = value Then
                Return
            End If
            _conn = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedBy() As Integer Implements IeZLookupConnection.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZLookupConnection.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZLookupConnection.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZLookupConnection.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZLookupConnection.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZLookupConnectionExist() As Boolean Implements IeZLookupConnection.IseZLookupConnectionExist
        Get
            Return (_LookupConnStrId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub


End Class
