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

        [HttpGet("/export/MTD/planings/csv")]
        [HttpGet("/export/MTD/planings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPlaningsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPlanings(), Request.Query), fileName);
        }

        [HttpGet("/export/MTD/planings/excel")]
        [HttpGet("/export/MTD/planings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPlaningsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPlanings(), Request.Query), fileName);
        }
    }
}
