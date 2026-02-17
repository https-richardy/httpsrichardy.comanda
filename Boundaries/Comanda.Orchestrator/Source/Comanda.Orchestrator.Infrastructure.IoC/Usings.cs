global using System.Diagnostics.CodeAnalysis;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Comanda.Orchestrator.CrossCutting.Configurations;
global using Comanda.Orchestrator.CrossCutting.Exceptions;

global using Comanda.Orchestrator.Application.Validators.Establishments;
global using Comanda.Orchestrator.Application.Validators.Products;
global using Comanda.Orchestrator.Application.Validators.Customer;
global using Comanda.Orchestrator.Application.Validators.Payments;
global using Comanda.Orchestrator.Application.Validators.Orders;
global using Comanda.Orchestrator.Application.Validators.Subscriptions;
global using Comanda.Orchestrator.Application.Validators.Checkout;

global using Comanda.Orchestrator.Application.Gateways;
global using Comanda.Orchestrator.Application.Payloads.Checkout;
global using Comanda.Orchestrator.Application.Handlers.Profiles;

global using Comanda.Orchestrator.Infrastructure.Gateways;
global using Comanda.Orchestrator.Application.Validators.Owner;

global using Comanda.Internal.Contracts.Transport.Internal.Stores;
global using Comanda.Internal.Contracts.Transport.Internal.Products;
global using Comanda.Internal.Contracts.Transport.Internal.Profiles;
global using Comanda.Internal.Contracts.Transport.Internal.Payments;
global using Comanda.Internal.Contracts.Transport.Internal.Orders;
global using Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

global using HttpsRichardy.Dispatcher.Extensions;
global using FluentValidation;
