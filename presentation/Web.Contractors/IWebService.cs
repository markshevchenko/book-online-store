using System;

namespace Web.Contractors
{
    public interface IWebService
    {
        Guid Uid { get; }

        string PostUri { get; }
    }
}
