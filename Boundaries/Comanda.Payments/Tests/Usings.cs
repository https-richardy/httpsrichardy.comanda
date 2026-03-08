global using System.Text.Json;
global using System.Net;
global using System.Net.Http.Json;
global using System.Net.Mime;

global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.DependencyInjection;

global using Comanda.Payments.Domain.Aggregates;
global using Comanda.Payments.Domain.Filtering;
global using Comanda.Payments.Domain.Concepts;
global using Comanda.Payments.Domain.Collections;

global using Comanda.Payments.Application.Payloads.Payment;
global using Comanda.Payments.Application.Payloads;
global using Comanda.Payments.Application.Gateways;

global using Comanda.Payments.Infrastructure.Constants;
global using Comanda.Payments.Infrastructure.Pipelines;

global using Comanda.Payments.TestSuite.Fixtures;
global using Comanda.Payments.TestSuite.Mocks;
global using Comanda.Payments.WebApi;
global using Comanda.Payments.WebApi.Interceptors;

global using HttpsRichardy.Internal.Essentials.Concepts;
global using HttpsRichardy.Internal.Essentials.Utilities;
global using HttpsRichardy.Internal.Essentials.Patterns;

global using AutoFixture;

global using MongoDB.Driver;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization;

global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;
