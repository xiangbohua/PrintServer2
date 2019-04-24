using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HTTPServerLib
{
    public class ServiceModule
    {
        public bool SearchRoute(ServiceRoute route)
        {
            return true;
        }

        public ActionResult ExecuteRoute(ServiceRoute route)
        {
            var type = this.GetType();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            methods = methods.Where(m => m.ReturnType == typeof(ActionResult)).ToArray();
            if (methods == null || methods.Length <= 0) return null;
            var method = methods.FirstOrDefault(m =>
            {
                var attributes = m.GetCustomAttributes(typeof(RouteAttribute), true);
                
                foreach (object attribute in attributes)
                {
                    if (attribute == null || attribute.GetType() == typeof(RouteAttribute))
                        return false;
                    RouteAttribute actualAttribute = (RouteAttribute)attribute;
                    if (actualAttribute.Method == route.Method && actualAttribute.RoutePath == route.RoutePath)
                        return true;
                }
               
                return false;
            });

            if (method == null) return null;
            return (ActionResult)method.Invoke(this, new object[] { });

        }
    }
}
