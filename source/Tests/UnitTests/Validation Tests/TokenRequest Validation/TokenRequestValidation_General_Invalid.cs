﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.Services;
using UnitTests.Plumbing;

namespace UnitTests.TokenRequest_Validation
{
    [TestClass]
    public class TokenRequestValidation_General_Invalid
    {
        ILogger _logger = new DebugLogger();
        ICoreSettings _settings = new TestSettings();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory("TokenRequest Validation - General - Invalid")]
        public void Parameters_Null()
        {
            var store = new TestCodeStore();
            var validator = ValidatorFactory.CreateTokenValidator(_settings, _logger,
                authorizationCodeStore: store);

            var result = validator.ValidateRequest(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory("TokenRequest Validation - General - Invalid")]
        public void Client_Null()
        {
            var store = new TestCodeStore();
            var validator = ValidatorFactory.CreateTokenValidator(_settings, _logger,
                authorizationCodeStore: store);

            var parameters = new NameValueCollection();
            parameters.Add(Constants.TokenRequest.GrantType, Constants.GrantTypes.AuthorizationCode);
            parameters.Add(Constants.TokenRequest.Code, "valid");
            parameters.Add(Constants.TokenRequest.RedirectUri, "https://server/cb");

            var result = validator.ValidateRequest(parameters, null);
        }

        [TestMethod]
        [TestCategory("TokenRequest Validation - General - Invalid")]
        public void Unknown_Grant_Type()
        {
            var client = _settings.FindClientById("codeclient");
            var store = new TestCodeStore();

            var code = new AuthorizationCode
            {
                Client = client,
                IsOpenId = true,
                RedirectUri = new Uri("https://server/cb"),
            };

            store.Store("valid", code);

            var validator = ValidatorFactory.CreateTokenValidator(_settings, _logger,
                authorizationCodeStore: store);

            var parameters = new NameValueCollection();
            parameters.Add(Constants.TokenRequest.GrantType, "unknown");
            parameters.Add(Constants.TokenRequest.Code, "valid");
            parameters.Add(Constants.TokenRequest.RedirectUri, "https://server/cb");

            var result = validator.ValidateRequest(parameters, client);

            Assert.IsTrue(result.IsError);
            Assert.AreEqual(Constants.TokenErrors.UnsupportedGrantType, result.Error);
        }

        [TestMethod]
        [TestCategory("TokenRequest Validation - General - Invalid")]
        public void Missing_Grant_Type()
        {
            var client = _settings.FindClientById("codeclient");
            var store = new TestCodeStore();

            var code = new AuthorizationCode
            {
                Client = client,
                IsOpenId = true,
                RedirectUri = new Uri("https://server/cb"),
            };

            store.Store("valid", code);

            var validator = ValidatorFactory.CreateTokenValidator(_settings, _logger,
                authorizationCodeStore: store);

            var parameters = new NameValueCollection();
            parameters.Add(Constants.TokenRequest.Code, "valid");
            parameters.Add(Constants.TokenRequest.RedirectUri, "https://server/cb");

            var result = validator.ValidateRequest(parameters, client);

            Assert.IsTrue(result.IsError);
            Assert.AreEqual(Constants.TokenErrors.UnsupportedGrantType, result.Error);
        }
    }
}