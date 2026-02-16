global using System.Diagnostics.CodeAnalysis;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi.Models;

global using Comanda.Subscriptions.WebApi.Extensions;
global using Comanda.Subscriptions.WebApi.Constants;
global using Comanda.Subscriptions.Domain.Errors;

global using Comanda.Subscriptions.Application.Payloads.Traceability;
global using Comanda.Subscriptions.Application.Payloads.Subscription;

global using Comanda.Subscriptions.Infrastructure.IoC.Extensions;
global using Comanda.Subscriptions.CrossCutting.Configurations;

global using HttpsRichardy.Dispatcher.Contracts;
global using HttpsRichardy.Federation.Sdk.Extensions;

global using Scalar.AspNetCore;
global using Serilog;
global using FluentValidation.AspNetCore;
