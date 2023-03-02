using Azure.Data.Tables;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DailyProduction.Models;

namespace IbasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyProductionController : ControllerBase
    {
        private List<DailyProductionDTO> _productionRepo;
        private readonly ILogger<DailyProductionController> _logger;

        public DailyProductionController(ILogger<DailyProductionController> logger)
        {
            _logger = logger;
            _productionRepo = new List<DailyProductionDTO>
            {
                //new DailyProductionDTO {Date = new DateTime(2020, 12, 31), Model = BikeModel.evIB200, ItemsProduced = 88}
            };

            TableClient client = new TableClient("DefaultEndpointsProtocol=https;AccountName=ibasproduktion2023;AccountKey=EmD7Jkw2wDlgPs+vs8WrdkeFzjDGWCtmbz+yaQXMECkZ7Tu4RBgSSCJNxdQKri/tM7d3sOtIdY/K+AStvLsu5g==;BlobEndpoint=https://ibasproduktion2023.blob.core.windows.net/;QueueEndpoint=https://ibasproduktion2023.queue.core.windows.net/;TableEndpoint=https://ibasproduktion2023.table.core.windows.net/;FileEndpoint=https://ibasproduktion2023.file.core.windows.net/;", "data");
            Pageable<TableEntity> entities = client.Query<TableEntity>();

            foreach (TableEntity entity in entities){
                DateTime entityDate = DateTime.Parse(entity.GetString("RowKey"))!;
                BikeModel entityModel = (BikeModel)Int32.Parse(entity.GetString("PartitionKey")!);;
                int entityItemsProduced = (int)entity.GetInt32("itemsProduced")!;
                _productionRepo.Add(new DailyProductionDTO {Date = entityDate, Model = entityModel, ItemsProduced = entityItemsProduced});
            }

            
        }
        
        [HttpGet]
        public IEnumerable<DailyProductionDTO> Get()
        {
            return _productionRepo;
        }
    }
}