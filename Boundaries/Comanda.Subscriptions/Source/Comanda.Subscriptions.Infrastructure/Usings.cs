global using HttpsRichardy.Internal.Essentials.Patterns;

global using HttpsRichardy.Internal.Infrastructure.Persistence;
global using HttpsRichardy.Internal.Infrastructure.Persistence.Pipelines;

global using Comanda.Subscriptions.Domain.Aggregates;
global using Comanda.Subscriptions.Domain.Filtering;
global using Comanda.Subscriptions.Domain.Collections;
global using Comanda.Subscriptions.Domain.Concepts;
global using Comanda.Subscriptions.Domain.Errors;


global using Comanda.Subscriptions.CrossCutting.Extensions;
global using Comanda.Subscriptions.Application.Gateways;
global using Comanda.Subscriptions.Application.Payloads.Subscription;

global using Comanda.Subscriptions.Infrastructure.Constants;
global using Comanda.Subscriptions.Infrastructure.Pipelines;
global using Comanda.Subscriptions.Infrastructure.Gateways.Mappers;

global using MongoDB.Driver;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization;
global using Stripe.Checkout;
