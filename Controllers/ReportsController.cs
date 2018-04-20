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
                        if (overdue != null)
                        {
                            return Json(overdue);
                        }
                        else
                        {
                            return NoContent();
                        }
                    }
                    else if (RequestInfo.reportType == "Popular")
                    {
                        var Popular = newReportUtils.RunPopularReport(RequestInfo.reportQty);
                        if (Popular != null)
                        {
                            return Json(Popular);
                        }
                        else
                        {
                            return NoContent();
                        }
                    }
                    else if (RequestInfo.reportType == "Customer")
                    {
                        var bestCust = newReportUtils.RunCustomerReport(RequestInfo.reportQty);
                        if (bestCust != null)
                        {
                            return Json(bestCust);
                        }
                        else
                        {
                            return NoContent();
                        }
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