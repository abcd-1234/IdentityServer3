﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Thinktecture.IdentityServer.Core.Connect.Models;

namespace Thinktecture.IdentityServer.Core.Services
{
    public interface ICoreSettings
    {
        IEnumerable<Scope> GetScopes();
        Client FindClientById(string clientId);
        X509Certificate2 GetSigningCertificate();
        string GetIssuerUri();
        string GetSiteName();
        string GetPublicHost();
        InternalProtectionSettings GetInternalProtectionSettings();
    }
}
