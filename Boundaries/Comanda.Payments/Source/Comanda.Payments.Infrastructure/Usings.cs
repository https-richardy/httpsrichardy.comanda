global using HttpsRichardy.Internal.Essentials.Patterns;
global using HttpsRichardy.Internal.Infrastructure.Persistence;
global using HttpsRichardy.Internal.Infrastructure.Persistence.Pipelines;

global using Comanda.Payments.Domain.Aggregates;
global using Comanda.Payments.Domain.Filtering;
global using Comanda.Payments.Domain.Collections;

global using Comanda.Payments.Application.Gateways;
global using Comanda.Payments.Application.Payloads.Payment;

global using Comanda.Payments.Infrastructure.Pipelines;
global using Comanda.Payments.Infrastructure.Mappers;
global using Comanda.Payments.Infrastructure.Constants;

global using Comanda.Internal.Contracts.Transport.External.AbacatePay;
global using Comanda.Internal.Contracts.Clients.Interfaces;

global using MongoDB.Driver;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization;
