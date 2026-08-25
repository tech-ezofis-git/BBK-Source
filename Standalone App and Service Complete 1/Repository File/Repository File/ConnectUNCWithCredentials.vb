Imports System
Imports System.Runtime.InteropServices
Imports DWORD = System.UInt32
Imports LPWSTR = System.String
Imports NET_API_STATUS = System.UInt32
Imports System.IO

Public Class ConnectUNCWithCredentials
    Implements IDisposable

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Friend Structure USE_INFO_2
        Friend ui2_local As LPWSTR
        Friend ui2_remote As LPWSTR
        Friend ui2_password As LPWSTR
        Friend ui2_status As DWORD
        Friend ui2_asg_type As DWORD
        Friend ui2_refcount As DWORD
        Friend ui2_usecount As DWORD
        Friend ui2_username As LPWSTR
        Friend ui2_domainname As LPWSTR
    End Structure

    <DllImport("NetApi32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Friend Shared Function NetUseAdd(ByVal UncServerName As LPWSTR, ByVal Level As DWORD, ByRef Buf As USE_INFO_2, <Out> ByRef ParmError As DWORD) As NET_API_STATUS
    End Function

    <DllImport("NetApi32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Friend Shared Function NetUseDel(ByVal UncServerName As LPWSTR, ByVal UseName As LPWSTR, ByVal ForceCond As DWORD) As NET_API_STATUS
    End Function

    Private disposed = False
    Private sUNCPath As String
    Private sUser As String
    Private sPassword As String
    Private sDomain As String
    Private iLastError As Integer

    Public Sub New()
    End Sub

    Public ReadOnly Property LastError As Integer
        Get
            Return iLastError
        End Get
    End Property

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not disposed Then
            NetUseDelete()
        End If

        disposed = True
        GC.SuppressFinalize(Me)
    End Sub

    Public Function NetUseWithCredentials(ByVal UNCPath As String, ByVal User As String, ByVal Domain As String, ByVal Password As String) As Boolean
        sUNCPath = UNCPath
        sUser = User
        sPassword = Password
        sDomain = Domain
        Return NetUseWithCredentials()
    End Function

    Private Function NetUseWithCredentials() As Boolean
        Dim returncode As UInteger

        Try
            Dim useinfo = New USE_INFO_2
            useinfo.ui2_remote = sUNCPath
            useinfo.ui2_username = sUser
            useinfo.ui2_domainname = sDomain
            useinfo.ui2_password = sPassword
            useinfo.ui2_asg_type = 0
            useinfo.ui2_usecount = 1
            Dim paramErrorIndex As UInteger
            returncode = NetUseAdd(Nothing, 2, useinfo, paramErrorIndex)
            iLastError = CInt(returncode)
            Return returncode = 0
        Catch ex As Exception
            writetxtfle("NetUseWithCredentials : " + ex.Message)
            iLastError = Marshal.GetLastWin32Error
            Return False
        End Try
    End Function

    Public Function NetUseDelete() As Boolean
        Dim returncode As UInteger

        Try
            returncode = NetUseDel(Nothing, sUNCPath, 2)
            iLastError = CInt(returncode)
            Return returncode = 0
        Catch ex As Exception
            writetxtfle("Impersonation : " + ex.Message)
            iLastError = Marshal.GetLastWin32Error
            Return False
        End Try
    End Function

    Protected Overrides Sub Finalize()
        Dispose()
    End Sub

    Public filelocation As String

    Public Function dir() As String
        Dim source As String = ""
        Dim apppath As String = ""
        Try
            apppath = System.Reflection.Assembly.GetEntryAssembly().Location
            apppath = Path.GetDirectoryName(apppath)
            source = apppath + "\log"
            If Not Directory.Exists(source) Then
                Directory.CreateDirectory(source)
            End If
        Catch ex As Exception
        End Try
        Return source
    End Function

    Public Sub writetxtfle(ByVal msg As String)
        Try
            Dim logfilename = DateTime.Now.ToString("yyyyMMddhhmmsstt") + "UNC"
            filelocation = Dir() & "\" & logfilename & ".txt"
            Using sw As StreamWriter = New StreamWriter(filelocation, True)
                sw.WriteLine(Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss") + " : " + msg)
            End Using
            'System.Windows.Forms.MessageBox.Show(msg)
        Catch ex As Exception
        End Try
    End Sub
End Class
