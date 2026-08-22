Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZAlternateField
    Inherits IDatabaseCommonItems
    Implements IeZAlternateField
    Protected _AlternateId As Integer
    Protected _FieldId As Integer
    Protected _FieldName As String
    Protected _AlternateFieldId As Integer
    Protected _AlternateFieldName As String
    Protected _FieldValue As String
    Protected _AlternateValue As String
    Protected _TemplateId As Integer
    Protected _LastNo As Integer
    Protected _TemplateName As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Private _Isdeleted As Integer

    Public Sub New(tmpAlternateId As Integer)
        Me._AlternateId = tmpAlternateId
    End Sub
    Public Sub New()
    End Sub
    Public Property AlternateId() As Integer Implements IeZAlternateField.AlternateId
        Get
            If _AlternateId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _AlternateId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _AlternateId <> 0 AndAlso _AlternateId <> value Then
                Throw New MemberAccessException()
            End If
            _AlternateId = value
        End Set
    End Property
    Public Property LastNo() As Integer Implements IeZAlternateField.LastNo
        Get
            If _LastNo = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LastNo
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LastNo <> 0 AndAlso _LastNo <> value Then
                Throw New MemberAccessException()
            End If
            _LastNo = value
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZAlternateField.TemplateID
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
    Public Property FieldId() As Integer Implements IeZAlternateField.FieldId
        Get
            If _FieldId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FieldId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FieldId <> 0 AndAlso _FieldId <> value Then
                Throw New MemberAccessException()
            End If
            _FieldId = value
        End Set
    End Property
    Public Property AlternateFieldId() As Integer Implements IeZAlternateField.AlternateFieldId
        Get
            If _AlternateFieldId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _AlternateFieldId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _AlternateFieldId <> 0 AndAlso _AlternateFieldId <> value Then
                Throw New MemberAccessException()
            End If
            _AlternateFieldId = value
        End Set
    End Property
    Public Property AlternateFieldName() As String Implements IeZAlternateField.AlternateFieldName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _AlternateFieldName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _AlternateFieldName = value Then
                Return
            End If
            _AlternateFieldName = value
            IsModified = True
        End Set
    End Property
    Public Property FieldName() As String Implements IeZAlternateField.FieldName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FieldName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FieldName = value Then
                Return
            End If
            _FieldName = value
            IsModified = True
        End Set
    End Property
    Public Property TemplateName() As String Implements IeZAlternateField.TemplateName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TemplateName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TemplateName = value Then
                Return
            End If
            _TemplateName = value
            IsModified = True
        End Set
    End Property
    Public Property FieldValue() As String Implements IeZAlternateField.FieldValue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FieldValue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FieldValue = value Then
                Return
            End If
            _FieldValue = value
            IsModified = True
        End Set
    End Property
    Public Property AlternateValue() As String Implements IeZAlternateField.AlternateValue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _AlternateValue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _AlternateValue = value Then
                Return
            End If
            _AlternateValue = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedBy() As Integer Implements IeZAlternateField.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZAlternateField.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZAlternateField.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZAlternateField.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZAlternateField.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class

