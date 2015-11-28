using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Web.Models.Logs;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        private readonly IRequestClient<IListLogs, IListLogsResult> _listLogsRequest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LogController" /> class.
        /// </summary>
        /// <param name="listLogsRequest">The list logs request.</param>
        public LogController(IRequestClient<IListLogs, IListLogsResult> listLogsRequest)
        {
            Contract.Requires(listLogsRequest != null);
            _listLogsRequest = listLogsRequest;
        }

        public ActionResult Index()
        {
            var now = DateTime.Now;
            var model = new IndexViewModel
            {
                FromDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0).ToString(CultureInfo.InvariantCulture),
                ToDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59).ToString(CultureInfo.InvariantCulture)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            DateTime start;
            DateTime end;

            if (!DateTime.TryParse(model.FromDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal, out start))
            {
                ModelState.AddModelError("FromDate", "Invalid From date");
            }

            if (!DateTime.TryParse(model.ToDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal, out end))
            {
                ModelState.AddModelError("ToDate", "Invalid To date");
            }

            if (start >= end)
            {
                ModelState.AddModelError("FromDate", "From date should less than To date");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = ListCommand.ListLogs(start, end);
            var logsResult = await _listLogsRequest.Request(command);
            model.Logs = logsResult.Logs;
            return View(model);
        }
    }
}