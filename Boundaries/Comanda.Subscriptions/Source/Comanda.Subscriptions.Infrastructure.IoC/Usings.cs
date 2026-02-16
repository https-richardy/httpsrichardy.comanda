global using System.Diagnostics.CodeAnalysis;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Comanda.Subscriptions.Domain.Collections;
global using Comanda.Subscriptions.CrossCutting.Configurations;
global using Comanda.Subscriptions.CrossCutting.Exceptions;

global using Comanda.Subscriptions.Application.Gateways;
global using Comanda.Subscriptions.Application.Handlers.Traceability;
global using Comanda.Subscriptions.Application.Payloads.Subscription;
global using Comanda.Subscriptions.Application.Validators.Subscription;

global using Comanda.Subscriptions.Infrastructure.Gateways;
global using Comanda.Subscriptions.Infrastructure.Persistence;

global using HttpsRichardy.Internal.Essentials.Contracts;
global using HttpsRichardy.Internal.Infrastructure.Persistence;

global using HttpsRichardy.Dispatcher.Extensions;

global using Stripe;
global using MongoDB.Driver;
global using FluentValidation;
