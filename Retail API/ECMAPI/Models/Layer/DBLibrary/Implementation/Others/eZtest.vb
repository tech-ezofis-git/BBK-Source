Imports System.Data
Imports System.Configuration
Imports System.Web


Public Class eZtest
    Inherits IDatabaseCommonItems
    Implements ieZtest

    Protected d_Outlooksyncid As Integer
    Protected d_Scheduleid As Integer = 0
    Protected d_Syncname As String = ""
    Protected d_Syncrule As String = ""
    Protected d_SyncMail As String = ""
    Protected d_Createdon As String
    Protected d_updatedon As String
    Protected d_Createdby As Integer = 0
    Protected d_updatedby As Integer = 0
    Private d_isdeleted As Integer = 0


    Public Sub New()

    End Sub

    Public Sub New(ByVal temoutlooksyncid As Integer)
        Me.d_Outlooksyncid = temoutlooksyncid
    End Sub


    Public Property Createdby As Integer Implements ieZtest.Createdby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return d_Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If d_Createdby = value Then
                Return
            End If
            d_Createdby = value
            IsModified = True
        End Set
    End Property


    Public Property Createdon As String Implements ieZtest.Createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return d_Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If d_Createdon = value Then
                Return
            End If
            d_Createdon = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements ieZtest.isdeleted
        Get
            Return d_isdeleted
        End Get
    End Property

    Public Property Outlooksyncid As Integer Implements ieZtest.Outlooksyncid
        Get
            If d_Outlooksyncid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return d_Outlooksyncid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If d_Outlooksyncid <> 0 And d_Outlooksyncid <> value Then
                Throw New MemberAccessException
            End If
            d_Outlooksyncid = value
        End Set
    End Property

    Public Property Scheduleid As Integer Implements ieZtest.Scheduleid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return d_Scheduleid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If d_Scheduleid = value Then
                Return
            End If
            d_Scheduleid = value
            IsModified = True
        End Set
    End Property

    Public Property SyncMail As String Implements ieZtest.SyncMail
        Get
            DBLayer.DBLInstance.Read(Me)
            Return d_SyncMail
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If d_SyncMail = value Then
                Return
            End If
            d_SyncMail = value
            IsModified = True
        End Set
    End Property

    Public Property Syncname As String Implements ieZtest.Syncname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return d_Syncname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If d_Syncname = value Then
                Return
            End If
            d_Syncname = value
            IsModified = True
        End Set
    End Property

    Public Property Syncrule As String Implements ieZtest.Syncrule
        Get
            DBLayer.DBLInstance.Read(Me)
            Return d_Syncrule
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If d_Syncrule = value Then
                Return
            End If
            d_Syncrule = value
            IsModified = True
        End Set
    End Property

    Public Property updatedby As Integer Implements ieZtest.updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return d_updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If d_updatedby = value Then
                Return
            End If
            d_updatedby = value
            IsModified = True
        End Set
    End Property

    Public Property updatedon As String Implements ieZtest.updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return d_updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If d_updatedon = value Then
                Return
            End If
            d_updatedon = value
            IsModified = True

        End Set
    End Property
End Class
