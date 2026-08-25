Imports System.Xml
Imports System.Configuration
Imports System.Reflection

Public Class ConfigSettings
    Private Sub New()
    End Sub

    Public Shared Function ReadSetting(ByVal key As String) As String
        Return ConfigurationSettings.AppSettings(key)
    End Function

    Public Shared Sub WriteSetting(ByVal key As String, ByVal value As String)
        ' load config document for current assembly
        Dim doc As XmlDocument = loadConfigDocument()

        ' retrieve appSettings node
        Dim node As XmlNode = doc.SelectSingleNode("//Database")

        If node Is Nothing Then
            Throw New InvalidOperationException("appSettings section not found in config file.")
        End If

        Try
            ' select the 'add' element that contains the key
            Dim elem As XmlElement = DirectCast(node.SelectSingleNode(String.Format("//add[@key='{0}']", key)), XmlElement)

            If elem IsNot Nothing Then
                ' add value for key
                elem.SetAttribute("value", value)
            Else
                ' key was not found so create the 'add' element 
                ' and set it's key/value attributes 
                elem = doc.CreateElement("add")
                elem.SetAttribute("key", key)
                elem.SetAttribute("value", value)
                node.AppendChild(elem)
            End If
            doc.Save(getConfigFilePath())
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub RemoveSetting(ByVal key As String)
        ' load config document for current assembly
        Dim doc As XmlDocument = loadConfigDocument()

        ' retrieve appSettings node
        Dim node As XmlNode = doc.SelectSingleNode("//appSettings")

        Try
            If node Is Nothing Then
                Throw New InvalidOperationException("appSettings section not found in config file.")
            Else
                ' remove 'add' element with coresponding key
                node.RemoveChild(node.SelectSingleNode(String.Format("//add[@key='{0}']", key)))
                doc.Save(getConfigFilePath())
            End If
        Catch e As NullReferenceException
            Throw New Exception(String.Format("The key {0} does not exist.", key), e)
        End Try
    End Sub

    Public Shared Function loadConfigDocument() As XmlDocument
        Dim doc As XmlDocument = Nothing
        Try
            doc = New XmlDocument()
            doc.Load(getConfigFilePath())
            Return doc
        Catch e As System.IO.FileNotFoundException
            Throw New Exception("No configuration file found.", e)
        End Try
    End Function
   
    Private Shared Function getConfigFilePath() As String
        Dim loc As String = Assembly.GetExecutingAssembly().Location + ".config"
        Return Assembly.GetExecutingAssembly().Location + ".config"
    End Function

    Public Shared Sub SaveEndpointAddress(ByVal endpointAddress As String)
        ' load config document for current assembly
        Dim doc As XmlDocument = loadConfigDocument()

        ' retrieve appSettings node
        Dim node As XmlNode = doc.SelectSingleNode("//system.serviceModel//client//endpoint")

        If node Is Nothing Then
            Throw New InvalidOperationException("Error. Could not find endpoint node in config file.")
        End If

        Try
            ' select the 'add' element that contains the key
            'XmlElement elem = (XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));
            Dim Http_String As String = "http://localhost/ezofis-Service/CACservice.svc"
            If My.Computer.Network.Ping(endpointAddress) Then
                endpointAddress = Replace(Http_String, "localhost", endpointAddress)
                If IsConnectionAvailable(endpointAddress) = True Then
                    node.Attributes("address").Value = endpointAddress
                    MsgBox("Connection Succeed", vbInformation)
                Else
                    MsgBox("Service Is Not Available", vbInformation)
                End If
            Else
                MsgBox("Given IP Is Not Pinging", vbInformation)
            End If
            doc.Save(getConfigFilePath())
        Catch e As Exception
            Throw e
        End Try
    End Sub

    Public Shared Function IsConnectionAvailable(ByVal StrUrl As String) As Boolean
        Dim objUrl As New System.Uri(StrUrl)
        ' Setup WebRequest
        Dim objWebReq As System.Net.WebRequest
        objWebReq = System.Net.WebRequest.Create(objUrl)
        Dim objResp As System.Net.WebResponse
        Try
            ' Attempt to get response and return True
            objResp = objWebReq.GetResponse
            objResp.Close()
            objWebReq = Nothing
            Return True
        Catch ex As Exception
            ' Error, exit and return False
            Return False
            objResp.Close()
            objWebReq = Nothing
        End Try
    End Function

    Public Shared Sub SaveZonalFilePath(ByVal ZonalFilePath As String)
        ' load config document for current assembly
        Dim doc As XmlDocument = loadConfigDocument()

        ' retrieve appSettings node
        Dim node As XmlNode = doc.SelectSingleNode("//configuration//ZonalSettings//ZonalFilePath")

        If node Is Nothing Then
            Throw New InvalidOperationException("Error. Could not find endpoint node in config file.")
        End If

        Try
            ' select the 'add' element that contains the key
            'XmlElement elem = (XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));
            Dim FilePath As String = ZonalFilePath
            If FileIO.FileSystem.FileExists(FilePath) = True Then
                node.Attributes("Value").Value = FilePath
                MsgBox("Zonal File Path Saved Successfully", vbInformation)
            Else
                MsgBox("Zonal File Does Not Exists", vbInformation)
            End If
            doc.Save(getConfigFilePath())
        Catch e As Exception
            Throw e
        End Try
    End Sub
End Class
