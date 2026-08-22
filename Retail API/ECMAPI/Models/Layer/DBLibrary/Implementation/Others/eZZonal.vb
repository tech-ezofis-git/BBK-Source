Public Class eZZonal
    Inherits IDatabaseCommonItems
    Implements IeZZonal

    Protected _ZonalId As Integer
    Protected _CabinetId As Integer
    Protected _TemplateId As Integer
    Protected _CabinetName As String = ""
    Protected _TemplateName As String = ""
    Protected _ProcessName As String = ""
    Protected _CreatedFrom As String = ""
    Protected _ZonalName As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Private _Isdeleted As Integer

    Public Sub New(tmpZonalId As Integer)
        Me._ZonalId = tmpZonalId
    End Sub
    Public Sub New()
    End Sub
    Public Property ZonalId() As Integer Implements IeZZonal.ZonalId
        Get
            If _ZonalId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ZonalId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ZonalId <> 0 AndAlso _ZonalId <> value Then
                Throw New MemberAccessException()
            End If
            _ZonalId = value
        End Set
    End Property
    Public Property CabinetId() As Integer Implements IeZZonal.CabinetId
        Get
            If _CabinetId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _CabinetId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _CabinetId <> 0 AndAlso _CabinetId <> value Then
                Throw New MemberAccessException()
            End If
            _CabinetId = value
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZZonal.TemplateId
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
    Public Property ZonalName() As String Implements IeZZonal.ZonalName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ZonalName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ZonalName = value Then
                Return
            End If
            _ZonalName = value
            IsModified = True
        End Set
    End Property
    Public Property CabinetName() As String Implements IeZZonal.CabinetName
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
    Public Property TemaplateName() As String Implements IeZZonal.TemplateName
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
    Public Property ProcessName() As String Implements IeZZonal.ProcessName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ProcessName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ProcessName = value Then
                Return
            End If
            _ProcessName = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedBy() As Integer Implements IeZZonal.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZZonal.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZZonal.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZZonal.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZZonal.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property CreatedFrom As String Implements IeZZonal.CreatedFrom
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedFrom
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedFrom = value Then
                Return
            End If
            _CreatedFrom = value
            IsModified = True
        End Set
    End Property
    '---------------------------------------------------------------------------
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
