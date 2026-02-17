global using System.Diagnostics.CodeAnalysis;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Comanda.Payments.Domain.Collections;
global using Comanda.Payments.Domain.Concepts;

global using Comanda.Payments.CrossCutting.Configurations;
global using Comanda.Payments.CrossCutting.Exceptions;

global using Comanda.Payments.Application.Handlers.Payment;
global using Comanda.Payments.Application.Gateways;
global using Comanda.Payments.Application.Payloads.Payment;
global using Comanda.Payments.Application.Validators;

global using Comanda.Payments.Infrastructure.Gateways;
global using Comanda.Payments.Infrastructure.Persistence;

global using HttpsRichardy.Internal.Essentials.Contracts;
global using HttpsRichardy.Internal.Infrastructure.Persistence;

global using HttpsRichardy.Dispatcher.Extensions;

global using MongoDB.Driver;
global using FluentValidation;
