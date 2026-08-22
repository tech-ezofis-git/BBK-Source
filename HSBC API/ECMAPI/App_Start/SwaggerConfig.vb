Imports System.Web.Http
Imports WebActivatorEx

Imports Swashbuckle.Application

<Assembly: PreApplicationStartMethod(GetType(SwaggerConfig), "Register")>
Public Class SwaggerConfig
    Public Shared Sub Register()
        Dim thisAssembly = GetType(SwaggerConfig).Assembly
        GlobalConfiguration.Configuration.EnableSwagger(Function(c)
                                                            c.SingleApiVersion("v1", "ECM")
                                                        End Function).EnableSwaggerUi(Function(c)
                                                                                      End Function)
    End Sub
End Class 'v1

