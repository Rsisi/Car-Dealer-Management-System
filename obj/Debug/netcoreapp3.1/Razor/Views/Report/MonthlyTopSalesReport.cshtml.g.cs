#pragma checksum "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/Report/MonthlyTopSalesReport.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2ba8cd3835ca49bdd9c55ade177e295e7880eca3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Report_MonthlyTopSalesReport), @"mvc.1.0.view", @"/Views/Report/MonthlyTopSalesReport.cshtml")]
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
#line 1 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/_ViewImports.cshtml"
using team011;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/_ViewImports.cshtml"
using team011.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2ba8cd3835ca49bdd9c55ade177e295e7880eca3", @"/Views/Report/MonthlyTopSalesReport.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b6eceadba93b855566579e663d5cd9e6ca14d499", @"/Views/_ViewImports.cshtml")]
    public class Views_Report_MonthlyTopSalesReport : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/Report/MonthlyTopSalesReport.cshtml"
  
    var monthTopReport = ViewBag.month_Top_report as List<MonthlyTopSalesReport>;
    int tempMonth = ViewBag.TempMonth;
    int tempYear = ViewBag.TempYear;

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<h1>Report - Monthly Sales</h1>

<div class=""btn-group mt-3"">

    <button type=""button"" class=""btn btn-primary dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
        Get Reports
    </button>
    <div class=""dropdown-menu"">
        <a class=""dropdown-item"" href=""/Report/ColorReport"">Sales by Color</a>
        <a class=""dropdown-item"" href=""/Report/VehicleTypeReport"">Sales by Vehicle Type</a>
        <a class=""dropdown-item"" href=""/Report/ManufacturerReport"">Sales by Manufacturer</a>
        <a class=""dropdown-item"" href=""/Report/GrossCustomerIncomeReport"">Gross Customer Income</a>
        <a class=""dropdown-item"" href=""/Report/RepairReport"">Repair Reports</a>
        <a class=""dropdown-item"" href=""/Report/BelowCostSaleReport"">Below Cost Sales</a>
        <a class=""dropdown-item"" href=""/Report/AverageInventoryTimeReport"">View Average Inventory Time</a>
        <a class=""dropdown-item"" href=""/Report/PartReport"">Parts Statistics</a>
        <a class=""dropdown-item"" href=""/Re");
            WriteLiteral(@"port/MonthlySaleReport"">Monthly Sales</a>
    </div>
</div>

<p style=""line-height:1.5em;""></p>

<a href=""/Report/MonthlySaleReport"" class=""btn btn-primary btn-sm active"" role=""button"" aria-pressed=""true"">Back to Monthly Sales Report</a>

<h1>Detail for Top Sales in ");
#nullable restore
#line 33 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/Report/MonthlyTopSalesReport.cshtml"
                       Write(tempYear);

#line default
#line hidden
#nullable disable
            WriteLiteral("/");
#nullable restore
#line 33 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/Report/MonthlyTopSalesReport.cshtml"
                                 Write(tempMonth);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\n<h2>Top Proforming Sales</h2>\n\n<div");
            BeginWriteAttribute("id", " id=\"", 1635, "\"", 1640, 0);
            EndWriteAttribute();
            WriteLiteral(@" style=""padding-top:5px"">
    <table id=""tblrepairbytype"" class=""table table-striped table-bordered"" style=""width:100%"">
        <thead>
            <tr>
                <th>
                    Saler Name
                </th>
                <th>
                    Number of Sales
                </th>
                <th>
                    Total Income
                </th>
            </tr>
        </thead>
        <tbody");
            BeginWriteAttribute("id", " id=\"", 2073, "\"", 2078, 0);
            EndWriteAttribute();
            WriteLiteral(">\n\n\n");
#nullable restore
#line 54 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/Report/MonthlyTopSalesReport.cshtml"
             foreach (var r in monthTopReport)
            {


#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\n                    <td>\n                        ");
#nullable restore
#line 59 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/Report/MonthlyTopSalesReport.cshtml"
                   Write(r.first_Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                        ");
#nullable restore
#line 60 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/Report/MonthlyTopSalesReport.cshtml"
                   Write(r.last_Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n\n                    </td>\n\n                    <th>\n                        ");
#nullable restore
#line 65 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/Report/MonthlyTopSalesReport.cshtml"
                   Write(r.totalVehicleSold);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                    </th>\n                    <td>\n                        ");
#nullable restore
#line 68 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/Report/MonthlyTopSalesReport.cshtml"
                   Write(r.Sales);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                    </td>\n\n                </tr>\n");
#nullable restore
#line 72 "/Users/christineloveyy/Gatech/6400 DB/GIT/cs6400-2021-03-Team011/team011/team011/Views/Report/MonthlyTopSalesReport.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
        </tbody>
    </table>

</div>

<script>$('#tblrepairbytype').DataTable({
    ""order"": [],
    ""pageLength"": 100,
    initComplete: function () {
        var api = this.api();

        $(api.row(':eq(0)').node()).css('background-color','yellow');
    }

})
    </script>
");
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