using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Gluwa.Error
{
    public sealed class PublicError : Error<HttpStatusCode>
    {
    }
}
