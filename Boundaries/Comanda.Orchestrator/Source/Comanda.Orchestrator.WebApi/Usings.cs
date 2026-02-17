global using System.Diagnostics.CodeAnalysis;
global using System.Security.Claims;
global using Microsoft.Extensions.Primitives;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi.Models;

global using Comanda.Internal.Contracts.Errors;
global using Comanda.Internal.Contracts.Transport.Internal.Products;
global using Comanda.Internal.Contracts.Transport.Internal.Profiles;
global using Comanda.Internal.Contracts.Transport.Internal.Payments;
global using Comanda.Internal.Contracts.Transport.Internal.Stores;
global using Comanda.Internal.Contracts.Transport.Internal.Orders;
global using Comanda.Internal.Contracts.Transport.Internal.Subscriptions;
global using Comanda.Internal.Contracts.Transport.Internal;

global using Comanda.Internal.Contracts.Clients;
global using Comanda.Internal.Contracts.Clients.Interfaces;

global using Comanda.Orchestrator.WebApi.Extensions;
global using Comanda.Orchestrator.WebApi.Constants;
global using Comanda.Orchestrator.WebApi.Middlewares;
global using Comanda.Orchestrator.WebApi.Providers;

global using Comanda.Orchestrator.Application.Payloads.Payments;
global using Comanda.Orchestrator.Application.Providers;
global using Comanda.Orchestrator.Infrastructure.IoC.Extensions;

global using Comanda.Orchestrator.CrossCutting.Configurations;
global using Comanda.Orchestrator.CrossCutting.Constants;

global using HttpsRichardy.Dispatcher.Contracts;
global using HttpsRichardy.Internal.Essentials.Concepts;

global using HttpsRichardy.Federation.Sdk.Extensions;
global using HttpsRichardy.Federation.Sdk.Contracts.Errors;

global using Scalar.AspNetCore;
global using Serilog;
global using FluentValidation.AspNetCore;

