using System;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Reports")]
    public class ReportsController : Controller
    {
        [HttpPost]
        public IActionResult GetReport([FromBody] ReportInfo RequestInfo)
        {
            try
            {
                if (RequestInfo.reportType != null)
                {
                    ReportUtils newReportUtils = new ReportUtils();

                    if (RequestInfo.reportType == "Overdue")
                    {
                        var overdue = newReportUtils.RunOverdueReport(RequestInfo.reportQty);
                        return Json(overdue);
                    }
                    else if (RequestInfo.reportType == "Popular")
                    {
                        var Popular = newReportUtils.RunPopularReport(RequestInfo.reportQty);
                        return Json(Popular);
                    }
                    else if (RequestInfo.reportType == "Customer")
                    {
                        var bestCust = newReportUtils.RunCustomerReport(RequestInfo.reportQty);
                        return Json(bestCust);
                    }
                }

                return NotFound($"Your Inputs are Bad! Report Type: {RequestInfo.reportType}, Report Quantity: {RequestInfo.reportQty}");
            }
            catch (Exception e)
            {
                return NotFound("Couldnt get A report, given the inputs! " + e);
            }
        }
    }

    public class ReportInfo
    {
        public string reportType { get; set; } // Overdue, Popular, Customer
        public int reportQty { get; set; } // -1 is ALL
    }
}