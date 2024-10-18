using Microsoft.AspNetCore.Mvc;
using System.IO;
using st10209886_PROG_POE1.Controllers;



namespace st10209886_PROG_POE1.Models

{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
