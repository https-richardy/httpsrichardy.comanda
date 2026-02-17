global using System.Diagnostics.CodeAnalysis;
global using System.Text.Json;
global using System.Web;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi.Models;

global using Comanda.Orders.WebApi.Extensions;
global using Comanda.Orders.WebApi.Constants;
global using Comanda.Orders.Domain.Errors;

global using Comanda.Orders.Application.Payloads.Order;
global using Comanda.Orders.Application.Payloads;

global using Comanda.Orders.Infrastructure.IoC.Extensions;
global using Comanda.Orders.CrossCutting.Configurations;

global using HttpsRichardy.Dispatcher.Contracts;
global using HttpsRichardy.Federation.Sdk.Extensions;

global using Scalar.AspNetCore;
global using Serilog;
global using FluentValidation.AspNetCore;
