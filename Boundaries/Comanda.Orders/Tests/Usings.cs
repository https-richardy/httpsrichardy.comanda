global using System.Net;
global using System.Net.Http.Json;
global using System.Text.Json;

global using System.Security.Claims;
global using System.Text.Encodings.Web;

global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;

global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.DependencyInjection;

global using Comanda.Orders.WebApi;

global using Comanda.Orders.Domain.Collections;
global using Comanda.Orders.Domain.Aggregates;
global using Comanda.Orders.Domain.Errors;
global using Comanda.Orders.Domain.Concepts;

global using Comanda.Orders.Application.Payloads.Order;
global using Comanda.Orders.TestSuite.Fixtures;

global using HttpsRichardy.Internal.Essentials.Patterns;
global using HttpsRichardy.Internal.Essentials.Utilities;

global using MongoDB.Driver;
global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;
global using AutoFixture;
