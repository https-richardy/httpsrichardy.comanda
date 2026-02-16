global using System.Diagnostics.CodeAnalysis;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Comanda.Profiles.Domain.Collections;
global using Comanda.Profiles.Domain.Concepts;

global using Comanda.Profiles.CrossCutting.Configurations;
global using Comanda.Profiles.CrossCutting.Exceptions;

global using Comanda.Profiles.Application.Payloads.Customer;
global using Comanda.Profiles.Application.Payloads.Owner;

global using Comanda.Profiles.Application.Validators.Customer;
global using Comanda.Profiles.Application.Validators.Owner;

global using Comanda.Profiles.Application.Handlers.Traceability;
global using Comanda.Profiles.Infrastructure.Persistence;

global using HttpsRichardy.Internal.Essentials.Contracts;
global using HttpsRichardy.Internal.Infrastructure.Persistence;
global using HttpsRichardy.Dispatcher.Extensions;

global using MongoDB.Driver;
global using FluentValidation;
