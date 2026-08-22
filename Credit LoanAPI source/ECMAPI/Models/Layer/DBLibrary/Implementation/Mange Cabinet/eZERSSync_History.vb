
Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZERSSync_History
    Inherits IDatabaseCommonItems
    Implements IeZERSSync_History
    Protected D_ezerssync_historyid As Integer
    Protected D_eZERSSyncid As Integer = 0
    Protected D_Scheduleid As Integer = 0
    Protected D_NO_OF_Files_Copied As Integer = 0
    Protected D_Status As String = ""
    Protected D_Createdon As String = ""
    Protected D_Updatedon As String = ""
    Protected D_Createdby As Integer = 0
    Protected D_updatedby As Integer = 0
    Protected D_Createdby1 As String = 0
    Protected D_updatedby1 As String = 0
    Private D_isdeleted As Integer = 0


    Public Sub New(ByVal tmpezerssync_historyid As Integer)
        Me.ezerssync_historyid = tmpezerssync_historyid
    End Sub
    Public Sub New()
    End Sub
    Public Property Createdby As Integer Implements IeZERSSync_History.Createdby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdby = value Then
                Return
            End If
            D_Createdby = value
            IsModified = True
        End Set
    End Property
    Public Property NO_OF_Files_Copied As Integer Implements IeZERSSync_History.NO_OF_Files_Copied
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_NO_OF_Files_Copied
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_NO_OF_Files_Copied = value Then
                Return
            End If
            D_NO_OF_Files_Copied = value
            IsModified = True
        End Set
    End Property

    Public Property Createdby1 As String Implements IeZERSSync_History.Createdby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Createdby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdby1 = value Then
                Return
            End If
            D_Createdby1 = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon As String Implements IeZERSSync_History.Createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdon = value Then
                Return
            End If

            D_Createdon = value
            IsModified = True
        End Set
    End Property
    Public Property ezerssync_historyid As Integer Implements IeZERSSync_History.ezerssync_historyid
        Get
            If D_ezerssync_historyid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return D_ezerssync_historyid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_ezerssync_historyid <> 0 AndAlso D_ezerssync_historyid <> value Then
                Throw New MemberAccessException()
            End If
            D_ezerssync_historyid = value
        End Set
    End Property

    Public Property eZERSSyncid As Integer Implements IeZERSSync_History.eZERSSyncid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_eZERSSyncid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_eZERSSyncid = value Then
                Return
            End If
            D_eZERSSyncid = value
            IsModified = True
        End Set
    End Property
    Public Property Scheduleid As Integer Implements IeZERSSync_History.Scheduleid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Scheduleid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Scheduleid Then
            End If
            D_Scheduleid = value
            IsModified = True
        End Set
    End Property


    Public Property Status As String Implements IeZERSSync_History.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Status
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Status = value Then
                Return
            End If
            D_Status = value
            IsModified = True
        End Set
    End Property


    Public Property updatedby As Integer Implements IeZERSSync_History.updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_updatedby = value Then
                Return
            End If
            D_updatedby = value
            IsModified = True
        End Set
    End Property

    Public Property updatedby1 As String Implements IeZERSSync_History.updatedby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_updatedby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_updatedby1 = value Then
                Return
            End If
            D_updatedby1 = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedon As String Implements IeZERSSync_History.Updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Updatedon = value Then
                Return
            End If
            D_Updatedon = value
            IsModified = True
        End Set
    End Property
    Public ReadOnly Property Isdeleted() As Integer Implements IeZERSSync_History.isdeleted
        Get
            Return D_isdeleted
        End Get
    End Property


End Class
