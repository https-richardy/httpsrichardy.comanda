global using System.Diagnostics.CodeAnalysis;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Comanda.Orders.Domain.Collections;
global using Comanda.Orders.Domain.Contracts;
global using Comanda.Orders.Domain.Services;

global using Comanda.Orders.CrossCutting.Configurations;
global using Comanda.Orders.CrossCutting.Exceptions;
global using Comanda.Orders.Infrastructure.Persistence;

global using Comanda.Orders.Application.Payloads.Order;

global using Comanda.Orders.Application.Handlers.Order;
global using Comanda.Orders.Application.Validators;

global using HttpsRichardy.Internal.Essentials.Contracts;
global using HttpsRichardy.Internal.Infrastructure.Persistence;

global using HttpsRichardy.Dispatcher.Extensions;

global using MongoDB.Driver;
global using FluentValidation;
