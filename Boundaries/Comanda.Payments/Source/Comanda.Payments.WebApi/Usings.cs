global using System.Diagnostics.CodeAnalysis;
global using System.Security.Claims;
global using System.Text.Json;
global using System.Net.Http.Headers;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using Comanda.Payments.WebApi.Interceptors;
global using Comanda.Payments.WebApi.Extensions;
global using Comanda.Payments.WebApi.Authentication;
global using Comanda.Payments.Domain.Errors;

global using Comanda.Payments.Application.Payloads.Traceability;
global using Comanda.Payments.Application.Payloads.Payment;
global using Comanda.Payments.Application.Payloads.Events.Billing;

global using Comanda.Payments.Infrastructure.IoC.Extensions;
global using Comanda.Payments.Infrastructure.IoC.Configurations;

global using Comanda.Internal.Contracts.Clients;
global using Comanda.Internal.Contracts.Clients.Interfaces;

global using Comanda.Internal.AspNet.Middlewares;
global using Comanda.Internal.AspNet.Constants;
global using Comanda.Internal.AspNet.Interceptors;

global using HttpsRichardy.Dispatcher.Contracts;

global using Scalar.AspNetCore;
global using Serilog;
global using FluentValidation.AspNetCore;
