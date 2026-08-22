
Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZERSSync
    Inherits IDatabaseCommonItems
    Implements IeZERSSync


    Protected D_eZERSSyncid As Integer
    Protected D_eZERSSyncname As String
    Protected D_FromERS As String
    Protected D_ToERS As String
    Protected D_Status As String
    Protected D_Createdon As String
    Protected D_Updatedon As String
    Protected D_Createdby As Integer
    Protected D_updatedby As Integer
    Protected D_Createdby1 As String
    Protected D_updatedby1 As String
    Private D_isdeleted As Integer

    Public Sub New()

    End Sub
    Public Sub New(tmperssyncid As Integer)
        Me.D_eZERSSyncid = tmperssyncid
    End Sub

    Public Property Createdby As Integer Implements IeZERSSync.Createdby
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

    Public Property Createdby1 As String Implements IeZERSSync.Createdby1
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

    Public Property Createdon As String Implements IeZERSSync.Createdon
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

    Public Property eZERSSyncid As Integer Implements IeZERSSync.eZERSSyncid
        Get
            If D_eZERSSyncid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return D_eZERSSyncid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_eZERSSyncid <> 0 AndAlso D_eZERSSyncid <> value Then
                Throw New MemberAccessException()
            End If
            D_eZERSSyncid = value
        End Set
    End Property
    Public Property eZERSSyncname As String Implements IeZERSSync.eZERSSyncname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_eZERSSyncname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_eZERSSyncname = value Then
                Return
            End If
            D_eZERSSyncname = value
            IsModified = True
        End Set
    End Property

    Public Property FromERS As String Implements IeZERSSync.FromERS
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_FromERS
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_FromERS = value Then
                Return
            End If
            D_FromERS = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements IeZERSSync.isdeleted
        Get
            Return D_isdeleted
        End Get
    End Property

    Public Property Status As String Implements IeZERSSync.Status
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

    Public Property ToERS As String Implements IeZERSSync.ToERS
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ToERS
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_ToERS = value Then
                Return
            End If
            D_ToERS = value
            IsModified = True
        End Set
    End Property

    Public Property updatedby As Integer Implements IeZERSSync.updatedby
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
        End Set
    End Property

    Public Property updatedby1 As String Implements IeZERSSync.updatedby1
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

    Public Property Updatedon As String Implements IeZERSSync.Updatedon
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
        End Set
    End Property
End Class
