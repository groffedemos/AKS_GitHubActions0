using System;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace APIContagem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContadorController : ControllerBase
    {
        private static Contador _CONTADOR = new Contador();

        [HttpGet]
        public object Get([FromServices]IConfiguration configuration)
        {  
            lock (_CONTADOR)
            {
                _CONTADOR.Incrementar();

                if (_CONTADOR.ValorAtual % 3 == 0)
                {
                    throw new Exception("Testando erro");
                }

                return new
                {
                    _CONTADOR.ValorAtual,
                    Environment.MachineName,
                    Local = "Teste",
                    Sistema = Environment.OSVersion.VersionString,
                    Variavel = configuration["TesteAmbiente"],
                    TargetFramework = Assembly
                        .GetEntryAssembly()?
                        .GetCustomAttribute<TargetFrameworkAttribute>()?
                        .FrameworkName
                };
            }
        }
    }
}