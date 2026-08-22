Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZCabinet
    Inherits IDatabaseCommonItems
    Implements IeZCabinet
    Protected _CabinetID As Integer
    Protected _CabinetName As String
    Protected _Description As String = ""
    Protected _CabSize As Integer
    Protected _CabCurrentSize As String
    Protected _CabExpiryDate As Date
    Protected _DocumentCount As Integer
    Protected _UserId As Integer
    Protected _CabOwnerID As Integer
    Protected _Profile As String
    Protected _CabOwnerName As String
    Protected _ERSName As String
    Protected _ERSServerName As String
    Protected _ERSDirPath As String
    Protected _ERSIndexinpath As String
    Protected _ProfileId As Integer
    Protected _ERSId As Integer
    Protected _CabIcon() As Byte
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._CabinetID = DeptId
    End Sub
    Public Sub New(tmpCabinetName As String)
        Me._CabinetName = tmpCabinetName.Trim()
    End Sub
    Public Sub New()
    End Sub

    Public Property CabinetID() As Integer Implements IeZCabinet.CabinetID
        Get
            If _CabinetID = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _CabinetID
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _CabinetID <> 0 AndAlso _CabinetID <> value Then
                Throw New MemberAccessException()
            End If
            _CabinetID = value
        End Set
    End Property
    Public Property DocumentCount() As Integer Implements IeZCabinet.DocumentCount
        Get
            If _DocumentCount = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _DocumentCount
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _DocumentCount <> 0 AndAlso _DocumentCount <> value Then
                Throw New MemberAccessException()
            End If
            _DocumentCount = value
        End Set
    End Property
    Public Property CabCurrentSize() As String Implements IeZCabinet.CabCurrentSize
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabCurrentSize
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CabCurrentSize = value Then
                Return
            End If
            _CabCurrentSize = value
            IsModified = True
        End Set
    End Property
    Public Property CabinetName() As String Implements IeZCabinet.CabinetName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabinetName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CabinetName = value Then
                Return
            End If
            _CabinetName = value
            IsModified = True
        End Set
    End Property
    Public Property Description() As String Implements IeZCabinet.Description
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Description
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Description = value Then
                Return
            End If
            _Description = value
            IsModified = True
        End Set
    End Property
    Public Property Profile() As String Implements IeZCabinet.Profile
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Profile
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Profile = value Then
                Return
            End If
            _Profile = value
            IsModified = True
        End Set
    End Property
    Public Property CabOwnerName() As String Implements IeZCabinet.CabOwnerName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabOwnerName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CabOwnerName = value Then
                Return
            End If
            _CabOwnerName = value
            IsModified = True
        End Set
    End Property
    Public Property ERSName() As String Implements IeZCabinet.ERSName
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
    Public Property ERSServerName() As String Implements IeZCabinet.ERSServerName
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
    Public Property ERSDirPath() As String Implements IeZCabinet.ERSDirPath
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
    Public Property ERSIndexinpath() As String Implements IeZCabinet.ERSIndexinpath
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
    Public Property CabOwnerID() As Integer Implements IeZCabinet.CabOwnerID
        Get
            If _CabOwnerID = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _CabOwnerID
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _CabOwnerID <> 0 AndAlso _CabOwnerID <> value Then
                Throw New MemberAccessException()
            End If
            _CabOwnerID = value
        End Set
    End Property
    Public Property CabSize() As Integer Implements IeZCabinet.CabSize
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabSize
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CabSize = value Then
                Return
            End If
            _CabSize = value
            IsModified = True
        End Set
    End Property
    Public Property CabExpiryDate() As Date Implements IeZCabinet.CabExpiryDate
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabExpiryDate
        End Get
        Set(value As Date)
            DBLayer.DBLInstance.Read(Me)
            If _CabExpiryDate = value Then
                Return
            End If
            _CabExpiryDate = value
            IsModified = True
        End Set
    End Property
    Public Property UserId() As Integer Implements IeZCabinet.UserId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UserId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UserId = value Then
                Return
            End If
            _UserId = value
            IsModified = True
        End Set
    End Property
    Public Property ERSId() As Integer Implements IeZCabinet.ERSId
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
    Public Property ProfileId() As Integer Implements IeZCabinet.ProfileId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ProfileId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ProfileId = value Then
                Return
            End If
            _ProfileId = value
            IsModified = True
        End Set
    End Property
    Public Property CabIcon() As Byte() Implements IeZCabinet.CabIcon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabIcon
        End Get
        Set(value As Byte())
            DBLayer.DBLInstance.Read(Me)
            'If _CabIcon = value Then
            '    Return
            'End If
            _CabIcon = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZCabinet.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZCabinet.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZCabinet.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZCabinet.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZCabinet.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZCabinet.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZCabinet.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsCabinetExist() As Boolean Implements IeZCabinet.IsCabinetExist
        Get
            Return (_CabinetID > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

End Class
