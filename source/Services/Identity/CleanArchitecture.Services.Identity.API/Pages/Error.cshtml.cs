using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Services.Identity.API.Pages
{
    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        private readonly IIdentityServerInteractionService _interaction;

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

               public ErrorModel(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        //public void OnGet()
        //{
        //    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        //}
        public void OnGet(string errorId)
        {           

            // retrieve error details from identityserver
            var message = _interaction.GetErrorContextAsync(errorId).Result;
            
        }
    }
}
