Imports System.Data
Imports System.Configuration
Imports System.Web
''' <summary>
''' Summary description for BarcodeTypeGroup
''' </summary>
Public Class eZBarcodeType
    Inherits IDatabaseCommonItems
    Implements IeZBarcodeType
    Protected _BarcodeTypeId As Integer
    Protected _BarcodeType As String
    
    Private _Isdeleted As Integer

    Public Sub New(tmpBarcodeTypeId As Integer)
        Me._BarcodeTypeId = tmpBarcodeTypeId
    End Sub
    Public Sub New(tmpBarcodeType As String)
        Me._BarcodeType = tmpBarcodeType
    End Sub

    Public Sub New()
    End Sub
    Public Property BarcodeTypeId() As Integer Implements IeZBarcodeType.BarcodeTypeId
        Get
            If _BarcodeTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _BarcodeTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _BarcodeTypeId <> 0 AndAlso _BarcodeTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _BarcodeTypeId = value
        End Set
    End Property

    Public Property BarcodeType() As String Implements IeZBarcodeType.BarcodeType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _BarcodeType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _BarcodeType = value Then
                Return
            End If
            _BarcodeType = value
            IsModified = True
        End Set
    End Property
   
    Public ReadOnly Property Isdeleted() As Integer Implements IeZBarcodeType.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsBarcodeTypeExist() As Boolean Implements IeZBarcodeType.IsBarcodeTypeExist
        Get
            Return (BarcodeTypeId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
