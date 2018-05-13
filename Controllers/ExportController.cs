using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceAdvisor.Controllers.Resources;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;
using PriceAdvisor.Persistence;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace PriceAdvisor.Controllers
{
    [Route("/api/export")]
    public class ExportController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private readonly IMapper mapper;        
        private readonly IExportRepository repository;
        public ExportController(IUnitOfWork unitOfWork,IMapper mapper,IExportRepository repository,PriceAdvisorDbContext context)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.repository = repository;
        }
        [Route("prices")]
        [HttpGet] 
        
        public async Task<IEnumerable<PriceResource>> GetPrices()
        {
        var prices = await repository.GetPricesAsync();

        return mapper.Map<IEnumerable<Price>,IEnumerable<PriceResource>>(prices);
        }

        [HttpGet] 
        public async Task ExportToCsv()
        {
        var prices = await context.Prices.Where(p=>p.Edited==true).ToListAsync();

        var path = Environment.CurrentDirectory+@"\Exports\PriceImports.csv";
        

        using(var w = new StreamWriter(path))
        {
        if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path);
            }
            var header = "PriceListName;ManufacturerPartNo;Category;PriceCategory;Manufacturer"+
            ";Bid/Normal (B, N or BN for both);Startdate (dd-MM-yyyy);Enddate (dd-MM-yyyy);C, U or D "+
            "(for create, update or delete);ProductActive;Value type 1 M, FM, FP (margin, fixed markup, "+
            "fixed price);Interval-1 (start1-end1);Value 1;Value type 2;Interval-2 (start2-end2);Value 2";
            w.WriteLine(header);
            w.Flush();
            var date = DateTime.Now;
            var year = date.Year;
            var month = date.Month.ToString();
            if(Int32.Parse(month) < 10){
                month= "0"+month;
            }
            var day = date.Day.ToString();
            if(Int32.Parse(day) < 10){
                day= "0"+day;
            }
            foreach(var price in prices)
            {
                var priceInDb = await context.Prices.FirstOrDefaultAsync(pric=> pric.Id == price.Id);
                var priceValue = price.Value;
                var product = await context.Products.FirstOrDefaultAsync(prod=> prod.Id == price.ProductId);
                var productAtrNo = product.Code;
                var priceList = "Global_OpenWeb";
                var action = "U";
                var bidNorm = "BN";
                var startDate = day+"-"+month+"-"+year.ToString();
                var endDate= day+"-"+month+"-"+(year+2).ToString();
                var productActive = "1";
                var valueType = "FP";
                var interval = "0.0-";
                var line = string.Format("{0};{1};;;;{2};{3};{4};{5};{6};{7};{8};{9};;;;"
                ,priceList,productAtrNo,bidNorm,startDate,endDate,action,productActive,valueType,
                interval,priceValue);
                w.WriteLine(line);
                w.Flush();
                
                priceInDb.Edited = false;
            }
            await unitOfWork.CompleteAsync();
        }
            var pr = new Process();
            pr.StartInfo = new ProcessStartInfo(path)
            { 
                UseShellExecute = true 
            };
            pr.Start();
        }
    }
}