using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class RouteItem
    {
        public int Id { get; set; }
        public string FriendlyUrl { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int ContentId { get; set; }

        public Common.TypeEntityFromRouteEnum Type { get; set; }
        public string Alias { get; set; }

        public RouteItem() { }

        public RouteItem(int id,string friendlyUrl, string controllerName,string actionName, int contentId) {
            Id = id;
            FriendlyUrl = friendlyUrl;
            ContentId = contentId;
            ControllerName = controllerName;
            ActionName = actionName;
        }

    }
}
