global using System.Security.Claims;
global using System.Text.Encodings.Web;

global using System.Net;
global using System.Net.Http.Json;
global using System.Text.Json;
global using System.Collections.Generic;

global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;

global using Comanda.Stores.WebApi;
global using Comanda.Stores.Domain.Collections;
global using Comanda.Stores.Domain.Aggregates;
global using Comanda.Stores.Domain.Concepts;
global using Comanda.Stores.Domain.Filtering;
global using Comanda.Stores.Application.Payloads.Establishment;
global using Comanda.Stores.Application.Payloads.Product;
global using Comanda.Stores.TestSuite.Fixtures;
global using Comanda.Stores.TestSuite.Extensions;

global using HttpsRichardy.Internal.Essentials.Utilities;
global using HttpsRichardy.Internal.Essentials.Concepts;

global using MongoDB.Driver;
global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;
global using Xunit;
global using AutoFixture;

