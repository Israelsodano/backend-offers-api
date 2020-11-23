using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace Offers.UnitTests
{
    class HostingEnvironmentStub : Microsoft.AspNetCore.Hosting.IHostingEnvironment
    {
        public string ApplicationName { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
        public string ContentRootPath { get; set; }
        public string EnvironmentName { get => "Test"; set => WebRootPath = value; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string WebRootPath { get; set; }
    }
}
