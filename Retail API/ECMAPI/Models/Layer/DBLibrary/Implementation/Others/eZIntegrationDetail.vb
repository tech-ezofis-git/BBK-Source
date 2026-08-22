Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZIntegrationDetail
    Inherits IDatabaseCommonItems
    Implements IeZIntegrationDetail
    Protected _IntegrationId As Integer
    Protected _IntegrationName As String
    Protected _IGServerType As String
    Protected _IGDataSource As String
    Protected _IGUserId As String
    Protected _IGPassword As String
    Protected _IGeZURL As String
    Protected _IGStatus As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpIntegrationId As Integer)
        Me._IntegrationId = tmpIntegrationId
    End Sub
    Public Sub New()
    End Sub

    Public Property IntegrationId() As Integer Implements IeZIntegrationDetail.IntegrationId
        Get
            If _IntegrationId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _IntegrationId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _IntegrationId <> 0 AndAlso _IntegrationId <> value Then
                Throw New MemberAccessException()
            End If
            _IntegrationId = value
        End Set
    End Property

    Public Property IntegrationName() As String Implements IeZIntegrationDetail.IntegrationName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IntegrationName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _IntegrationName = value Then
                Return
            End If
            _IntegrationName = value
            IsModified = True
        End Set
    End Property

    Public Property IGServerType() As String Implements IeZIntegrationDetail.IGServerType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IGServerType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _IGServerType = value Then
                Return
            End If
            _IGServerType = value
            IsModified = True
        End Set
    End Property

    Public Property IGDataSource() As String Implements IeZIntegrationDetail.IGDataSource
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IGDataSource
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _IGDataSource = value Then
                Return
            End If
            _IGDataSource = value
            IsModified = True
        End Set
    End Property

    Public Property IGUserId() As String Implements IeZIntegrationDetail.IGUserId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IGUserId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _IGUserId = value Then
                Return
            End If
            _IGUserId = value
            IsModified = True
        End Set
    End Property

    Public Property IGPassword() As String Implements IeZIntegrationDetail.IGPassword
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IGPassword
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _IGPassword = value Then
                Return
            End If
            _IGPassword = value
            IsModified = True
        End Set
    End Property

    Public Property IGeZURL() As String Implements IeZIntegrationDetail.IGeZURL
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IGeZURL
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _IGeZURL = value Then
                Return
            End If
            _IGeZURL = value
            IsModified = True
        End Set
    End Property

    Public Property IGStatus() As Integer Implements IeZIntegrationDetail.IGStatus
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IGStatus
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _IGStatus = value Then
                Return
            End If

            _IGStatus = value
            IsModified = True
        End Set
    End Property
    

    Public Property UpdatedBy1() As String Implements IeZIntegrationDetail.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZIntegrationDetail.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZIntegrationDetail.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZIntegrationDetail.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZIntegrationDetail.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZIntegrationDetail.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZIntegrationDetail.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
