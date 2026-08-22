Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZDocumentAlert
    Inherits IDatabaseCommonItems
    Implements IeZDocumentAlert
    Protected _DocumentAlertId As Integer
    Protected _itemid As Integer
    Protected _TemplateId As Integer
    Protected _TableName As String
    Protected _filename As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _ToMail As String
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CUserName As String
    Protected _CUserCode As String
    Protected _UUserName As String
    Protected _UUserCode As String
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpDocumentAlertId As Integer)
        Me._DocumentAlertId = tmpDocumentAlertId
    End Sub
    Public Sub New(tmpDocumentAlert As String)
        Me._TableName = tmpDocumentAlert
    End Sub

    Public Sub New()
    End Sub
    Public Property DocumentAlertId() As Integer Implements IeZDocumentAlert.DocumentAlertId
        Get
            If _DocumentAlertId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _DocumentAlertId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _DocumentAlertId <> 0 AndAlso _DocumentAlertId <> value Then
                Throw New MemberAccessException()
            End If
            _DocumentAlertId = value
        End Set
    End Property
    Public Property ToMail() As String Implements IeZDocumentAlert.ToMail
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToMail
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ToMail = value Then
                Return
            End If
            _ToMail = value
            IsModified = True
        End Set
    End Property
    Public Property itemId() As Integer Implements IeZDocumentAlert.itemid
        Get
            If _itemid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _itemid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _itemid <> 0 AndAlso _itemid <> value Then
                Throw New MemberAccessException()
            End If
            _itemid = value
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZDocumentAlert.TemplateId
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
    Public Property filename() As String Implements IeZDocumentAlert.filename
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _filename
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _filename = value Then
                Return
            End If
            _filename = value
            IsModified = True
        End Set
    End Property
    Public Property TableName() As String Implements IeZDocumentAlert.TableName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TableName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TableName = value Then
                Return
            End If
            _TableName = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZDocumentAlert.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZDocumentAlert.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZDocumentAlert.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZDocumentAlert.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZDocumentAlert.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZDocumentAlert.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZDocumentAlert.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsDocumentAlertExist() As Boolean Implements IeZDocumentAlert.IsDocumentAlertExist
        Get
            Return (DocumentAlertId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
