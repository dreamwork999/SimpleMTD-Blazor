using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using SimplyMTD.Data;

namespace SimplyMTD.Controllers
{
    public partial class ExportMTDController : ExportController
    {
        private readonly MTDContext context;
        private readonly MTDService service;

        public ExportMTDController(MTDContext context, MTDService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/MTD/clients/csv")]
        [HttpGet("/export/MTD/clients/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClientsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetClients(), Request.Query), fileName);
        }

        [HttpGet("/export/MTD/clients/excel")]
        [HttpGet("/export/MTD/clients/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClientsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetClients(), Request.Query), fileName);
        }
    }
}
