using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RubberduckWebsite.Models
{
    public class ScreenshotViewModel
    {
        public string FeatureName { get; set; }
        public IFormFile File { get; set; }

    }
}
