global using System.Diagnostics.CodeAnalysis;
global using System.Security.Claims;
global using System.Text.Json;
global using System.Web;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using Comanda.Orders.WebApi.Extensions;
global using Comanda.Orders.WebApi.Authentication;
global using Comanda.Orders.Domain.Errors;

global using Comanda.Orders.Application.Payloads.Order;
global using Comanda.Orders.Application.Payloads;

global using Comanda.Orders.Infrastructure.IoC.Extensions;
global using Comanda.Orders.Infrastructure.IoC.Configurations;

global using Comanda.Internal.AspNet.Middlewares;
global using Comanda.Internal.AspNet.Constants;

global using HttpsRichardy.Dispatcher.Contracts;

global using Scalar.AspNetCore;
global using Serilog;
global using FluentValidation.AspNetCore;
