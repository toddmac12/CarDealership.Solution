#pragma checksum "C:\Users\toddm\Desktop\Epicodus\Week 9 Web Apps\CarDealership.Solution\CarDealership\Views\Cars\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d2c9384fa3931587ff2db3c9487b642cf095d8e7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Cars_Index), @"mvc.1.0.view", @"/Views/Cars/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\toddm\Desktop\Epicodus\Week 9 Web Apps\CarDealership.Solution\CarDealership\Views\Cars\Index.cshtml"
using CarDealership.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d2c9384fa3931587ff2db3c9487b642cf095d8e7", @"/Views/Cars/Index.cshtml")]
    public class Views_Cars_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<h1>Car List</h1>\r\n<ul>\r\n");
#nullable restore
#line 5 "C:\Users\toddm\Desktop\Epicodus\Week 9 Web Apps\CarDealership.Solution\CarDealership\Views\Cars\Index.cshtml"
 if (Model.Count == 0)
  {

#line default
#line hidden
#nullable disable
            WriteLiteral("  <p>No cars available.</p>\r\n");
#nullable restore
#line 8 "C:\Users\toddm\Desktop\Epicodus\Week 9 Web Apps\CarDealership.Solution\CarDealership\Views\Cars\Index.cshtml"
  }

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\toddm\Desktop\Epicodus\Week 9 Web Apps\CarDealership.Solution\CarDealership\Views\Cars\Index.cshtml"
 foreach (Car car in Model)
  {

#line default
#line hidden
#nullable disable
            WriteLiteral("      <li>");
#nullable restore
#line 11 "C:\Users\toddm\Desktop\Epicodus\Week 9 Web Apps\CarDealership.Solution\CarDealership\Views\Cars\Index.cshtml"
     Write(car.CarModel);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n      <li>");
#nullable restore
#line 12 "C:\Users\toddm\Desktop\Epicodus\Week 9 Web Apps\CarDealership.Solution\CarDealership\Views\Cars\Index.cshtml"
     Write(car.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n      <li>");
#nullable restore
#line 13 "C:\Users\toddm\Desktop\Epicodus\Week 9 Web Apps\CarDealership.Solution\CarDealership\Views\Cars\Index.cshtml"
     Write(car.Miles);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n");
#nullable restore
#line 14 "C:\Users\toddm\Desktop\Epicodus\Week 9 Web Apps\CarDealership.Solution\CarDealership\Views\Cars\Index.cshtml"
      
  }

#line default
#line hidden
#nullable disable
            WriteLiteral("</ul>\r\n<a href=\"/cars/new\">Add a new car.</a>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
