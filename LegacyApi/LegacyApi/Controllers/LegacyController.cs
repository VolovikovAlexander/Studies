using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LegacyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LegacyController : ControllerBase
    {
       
        private readonly ILogger<LegacyController> _logger;

        public LegacyController(ILogger<LegacyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Проект подготовки к занятиям в ИГУ. Инструменты: \nC#, EF, MS SQL Server 2014\n\nДокументация: ../Docs\nЛогика проекта: ../LegacyCore\nНабор автотестов: ../LegacyTest\n\nhttps://github.com/VolovikovAlexander/Legacy";
        }
    }
}
