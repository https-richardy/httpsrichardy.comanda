global using System.Diagnostics.CodeAnalysis;
global using System.Security.Claims;
global using System.Text.Json;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using Comanda.Subscriptions.WebApi.Extensions;
global using Comanda.Subscriptions.WebApi.Authentication;
global using Comanda.Subscriptions.Domain.Errors;

global using Comanda.Subscriptions.Application.Payloads.Traceability;
global using Comanda.Subscriptions.Application.Payloads.Subscription;

global using Comanda.Subscriptions.Infrastructure.IoC.Extensions;
global using Comanda.Subscriptions.Infrastructure.IoC.Configurations;

global using Comanda.Internal.AspNet.Middlewares;
global using Comanda.Internal.AspNet.Constants;

global using HttpsRichardy.Dispatcher.Contracts;

global using Scalar.AspNetCore;
global using Serilog;
global using FluentValidation.AspNetCore;
