using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PieceOfCake.IDP.Data;
using PieceOfCake.IDP.Models;

[assembly: HostingStartup(typeof(PieceOfCake.IDP.Areas.Identity.IdentityHostingStartup))]
namespace PieceOfCake.IDP.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
        }
    }
}