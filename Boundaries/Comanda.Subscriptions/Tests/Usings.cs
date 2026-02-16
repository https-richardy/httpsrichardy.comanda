global using System.Net;
global using System.Net.Http.Json;
global using System.Text.Json;

global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.DependencyInjection;

global using Comanda.Subscriptions.WebApi;
global using Comanda.Subscriptions.Domain.Collections;
global using Comanda.Subscriptions.Domain.Aggregates;
global using Comanda.Subscriptions.Domain.Concepts;
global using Comanda.Subscriptions.Domain.Errors;

global using Comanda.Subscriptions.Application.Payloads;
global using Comanda.Subscriptions.Application.Gateways;
global using Comanda.Subscriptions.Application.Payloads.Subscription;

global using Comanda.Subscriptions.TestSuite.Fixtures;
global using Comanda.Subscriptions.TestSuite.Mocks;

global using HttpsRichardy.Internal.Essentials.Patterns;

global using MongoDB.Driver;
global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;
global using AutoFixture;
