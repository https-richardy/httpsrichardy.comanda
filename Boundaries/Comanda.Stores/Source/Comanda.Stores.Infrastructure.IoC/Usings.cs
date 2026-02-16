global using System.Diagnostics.CodeAnalysis;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Comanda.Stores.Domain.Collections;
global using Comanda.Stores.CrossCutting.Configurations;
global using Comanda.Stores.CrossCutting.Exceptions;

global using Comanda.Stores.Infrastructure.Persistence;

global using Comanda.Stores.Application.Payloads.Product;
global using Comanda.Stores.Application.Payloads.Establishment;
global using Comanda.Stores.Application.Validators.Establishment;

global using Comanda.Stores.Application.Validators.Product;
global using Comanda.Stores.Application.Handlers.Establishment;

global using HttpsRichardy.Internal.Essentials.Contracts;
global using HttpsRichardy.Internal.Infrastructure.Persistence;

global using HttpsRichardy.Dispatcher.Extensions;

global using MongoDB.Driver;
global using FluentValidation;
