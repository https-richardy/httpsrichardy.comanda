global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.DependencyInjection;

global using Comanda.Payments.WebApi;
global using Comanda.Payments.Domain.Aggregates;
global using Comanda.Payments.Domain.Filtering;
global using Comanda.Payments.Domain.Concepts;

global using Comanda.Payments.Infrastructure.Constants;
global using Comanda.Payments.Infrastructure.Pipelines;

global using HttpsRichardy.Internal.Essentials.Concepts;
global using HttpsRichardy.Internal.Essentials.Utilities;

global using MongoDB.Driver;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization;

global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;
