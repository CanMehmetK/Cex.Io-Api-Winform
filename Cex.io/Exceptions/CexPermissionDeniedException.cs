using System;
using System.Linq;
using System.Net.Http;

namespace Kanpinar.Cex
{
    public class CexPermissionDeniedException : CexApiException
    {

        public CexPermissionDeniedException(HttpResponseMessage response, string message) : base(response, message) {}

    }
}
