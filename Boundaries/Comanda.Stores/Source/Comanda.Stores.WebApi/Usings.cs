global using System.Diagnostics.CodeAnalysis;
global using System.Security.Claims;
global using System.Text.Json;
global using System.Web;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using Comanda.Stores.WebApi.Extensions;
global using Comanda.Stores.WebApi.Authentication;
global using Comanda.Stores.Domain.Errors;

global using Comanda.Stores.Application.Payloads.Establishment;
global using Comanda.Stores.Application.Payloads.Product;
global using Comanda.Stores.Application.Mappers;
global using Comanda.Stores.Application.Gateways;
global using Comanda.Stores.Application.Payloads;

global using Comanda.Stores.Infrastructure.IoC.Extensions;
global using Comanda.Stores.Infrastructure.Gateways;

global using Comanda.Stores.Infrastructure.Constants;
global using Comanda.Stores.Infrastructure.Options;

global using Comanda.Stores.Infrastructure.IoC.Configurations;

global using Comanda.Internal.AspNet.Middlewares;
global using Comanda.Internal.AspNet.Constants;

global using HttpsRichardy.Dispatcher.Contracts;

global using Scalar.AspNetCore;
global using Serilog;
global using FluentValidation.AspNetCore;
