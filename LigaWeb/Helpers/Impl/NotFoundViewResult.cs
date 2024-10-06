using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LigaWeb.Helpers.Impl
{
    public class NotFoundViewResult : ViewResult
    {
        public NotFoundViewResult(string viewName, object model = null)
        {
            ViewName = viewName;
            StatusCode = (int)HttpStatusCode.NotFound;

            if (model != null)
            {
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    Model = model
                };
            }
        }


    }
}
