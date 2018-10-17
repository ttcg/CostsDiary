using CostsDiary.Web.Enums;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace CostsDiary.Web.Extensions
{
    public static class TempDataExtensions
    {
        public static Dictionary<string, IList<string>> GetNotifications(this ITempDataDictionary tempData)
        {
            {
                var notifications = new Dictionary<string, IList<string>>();

                LoadNotificationsByType(notifications, NotificationType.Success);
                LoadNotificationsByType(notifications, NotificationType.Error);
                LoadNotificationsByType(notifications, NotificationType.Info);
                LoadNotificationsByType(notifications, NotificationType.Warning);
                
                return notifications;
            }

            void LoadNotificationsByType(Dictionary<string, IList<string>> notifications, NotificationType notificationType)
            {
                var keyString = $"notifications.{notificationType}";
                if (tempData.TryGetValue(keyString, out var messages))
                    notifications[notificationType.ToString()] = messages as IList<string>;
            }
        }
    }
}
