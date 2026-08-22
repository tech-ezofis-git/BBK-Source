Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZERSInfo
    Inherits IDatabaseCommonItems
    Implements IeZERSInfo
    Protected _ERSId As Integer
    Protected _ERSName As String
    Protected _ERSServerName As String
    Protected _ERSDirPath As String
    Protected _SettingPath As String
    Protected _ERSIndexinpath As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _IsMain As Boolean
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpERSId As Integer)
        Me._ERSId = tmpERSId
    End Sub
    Public Sub New(tmpERSName As String)
        Me._ERSName = tmpERSName.Trim()
    End Sub
    Public Sub New()
    End Sub

    Public Property ERSId() As Integer Implements IeZERSInfo.ERSId
        Get
            If _ERSId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ERSId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ERSId <> 0 AndAlso _ERSId <> value Then
                Throw New MemberAccessException()
            End If
            _ERSId = value
        End Set
    End Property
   
    Public Property IsMain() As Boolean Implements IeZERSInfo.IsMain
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsMain
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsMain = value Then
                Return
            End If
            _IsMain = value
            IsModified = True
        End Set
    End Property
    Public Property ERSName() As String Implements IeZERSInfo.ERSName
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
    Public Property ERSServerName() As String Implements IeZERSInfo.ERSServerName
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
    Public Property ERSDirPath() As String Implements IeZERSInfo.ERSDirPath
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
    Public Property SettingPath() As String Implements IeZERSInfo.SettingPath
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
    Public Property ERSIndexinpath() As String Implements IeZERSInfo.ERSIndexinpath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ERSIndexinpath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ERSIndexinpath = value Then
                Return
            End If
            _ERSIndexinpath = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZERSInfo.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZERSInfo.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZERSInfo.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZERSInfo.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZERSInfo.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZERSInfo.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZERSInfo.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsERSInfoExist() As Boolean Implements IeZERSInfo.IsERSInfoExist
        Get
            Return (_ERSId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

End Class
