/* global using for System namespaces here */

global using System.Net;
global using System.Net.Http.Json;
global using System.Text.Json;

/* global using for Microsoft namespaces here */

global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.DependencyInjection;

/* global using for Vinder namespaces here */

global using Comanda.Profiles.WebApi;

global using Comanda.Profiles.Domain.Collections;
global using Comanda.Profiles.Domain.Aggregates;
global using Comanda.Profiles.Domain.Concepts;
global using Comanda.Profiles.Domain.Errors;
global using Comanda.Profiles.Domain.Filtering;

global using Comanda.Profiles.Application.Payloads.Customer;
global using Comanda.Profiles.Application.Payloads.Owner;

global using Comanda.Profiles.TestSuite.Fixtures;

global using HttpsRichardy.Internal.Essentials.Patterns;
global using HttpsRichardy.Internal.Essentials.Utilities;

/* global usings for third-party namespaces here */

global using MongoDB.Driver;
global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;
global using AutoFixture;
