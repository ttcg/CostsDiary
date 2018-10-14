using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsDiary.Web.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CostsDiary.Web.Controllers
{
    public class BaseController : Controller
    {
        protected virtual void AddNotification(NotificationType notificationType, string message)
        {
            string dataKey = $"notifications.{notificationType}";

            if (TempData[dataKey] == null)
                TempData[dataKey] = new List<string>();

            ((List<string>)TempData[dataKey]).Add(message);
        }

        protected virtual void AddSuccessMessage(string message) => AddNotification(NotificationType.Success, message);
        protected virtual void AddErrorMessage(string message) => AddNotification(NotificationType.Error, message);
        protected virtual void AddWarningMessage(string message) => AddNotification(NotificationType.Warning, message);
        protected virtual void AddInfoMessage(string message) => AddNotification(NotificationType.Info, message);
    }
}