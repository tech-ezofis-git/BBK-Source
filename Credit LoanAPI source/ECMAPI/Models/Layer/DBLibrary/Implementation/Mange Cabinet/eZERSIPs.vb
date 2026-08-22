Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZERSIPs
    Inherits IDatabaseCommonItems
    Implements IeZERSIPs
    Protected _ERSIPID As Integer
    Protected _FromIP As String
    Protected _ToIP As String
    Protected _ERSId As Integer
    Protected _ERSName As String
    Protected _ERSServerName As String
    Protected _ERSDirPath As String
    Protected _SettingPath As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpERSId As Integer)
        Me._ERSIPID = tmpERSId
    End Sub
    Public Sub New()
    End Sub
    Public Property ERSIPID() As Integer Implements IeZERSIPs.ERSIPID
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ERSIPID
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ERSIPID = value Then
                Return
            End If
            _ERSIPID = value
            IsModified = True
        End Set
    End Property
    Public Property FromIP() As String Implements IeZERSIPs.FromIP
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FromIP
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FromIP = value Then
                Return
            End If
            _FromIP = value
            IsModified = True
        End Set
    End Property
    Public Property ToIP() As String Implements IeZERSIPs.ToIP
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToIP
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ToIP = value Then
                Return
            End If
            _ToIP = value
            IsModified = True
        End Set
    End Property
    Public Property SettingPath() As String Implements IeZERSIPs.SettingPath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SettingPath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SettingPath = value Then
                Return
            End If
            _SettingPath = value
            IsModified = True
        End Set
    End Property
    Public Property ERSId() As Integer Implements IeZERSIPs.ERSId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ERSId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ERSId = value Then
                Return
            End If
            _ERSId = value
            IsModified = True
        End Set
    End Property
    Public Property ERSName() As String Implements IeZERSIPs.ERSName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ERSName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ERSName = value Then
                Return
            End If
            _ERSName = value
            IsModified = True
        End Set
    End Property
    Public Property ERSServerName() As String Implements IeZERSIPs.ERSServerName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ERSServerName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ERSServerName = value Then
                Return
            End If
            _ERSServerName = value
            IsModified = True
        End Set
    End Property
    Public Property ERSDirPath() As String Implements IeZERSIPs.ERSDirPath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ERSDirPath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ERSDirPath = value Then
                Return
            End If
            _ERSDirPath = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZERSIPs.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZERSIPs.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZERSIPs.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZERSIPs.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZERSIPs.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZERSIPs.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZERSIPs.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsERSInfoExist() As Boolean Implements IeZERSIPs.IsERSInfoExist
        Get
            Return (_ERSId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub


End Class
