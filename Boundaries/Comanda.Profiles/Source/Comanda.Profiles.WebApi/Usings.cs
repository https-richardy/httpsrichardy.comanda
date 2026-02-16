global using System.Diagnostics.CodeAnalysis;
global using System.Text.Json;
global using System.Web;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi.Models;

global using Comanda.Profiles.WebApi.Extensions;
global using Comanda.Profiles.WebApi.Constants;
global using Comanda.Profiles.Domain.Errors;

global using Comanda.Profiles.Application.Payloads.Traceability;
global using Comanda.Profiles.Application.Payloads.Customer;
global using Comanda.Profiles.Application.Payloads.Owner;
global using Comanda.Profiles.Application.Payloads;

global using Comanda.Profiles.Infrastructure.IoC.Extensions;
global using Comanda.Profiles.CrossCutting.Configurations;

global using HttpsRichardy.Dispatcher.Contracts;
global using HttpsRichardy.Federation.Sdk.Extensions;

global using Scalar.AspNetCore;
global using Serilog;
global using FluentValidation.AspNetCore;
