global using System.Text.Json;
global using System.Net.Http.Json;
global using System.ComponentModel.DataAnnotations;

global using System.Security.Claims;
global using System.IdentityModel.Tokens.Jwt;

global using Microsoft.JSInterop;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Microsoft.AspNetCore.Components.Authorization;

global using Comanda.Merchants.WebUI.Extensions;
global using Comanda.Merchants.WebUI.Contracts;
global using Comanda.Merchants.WebUI.Constants;
global using Comanda.Merchants.WebUI.Gateways;
global using Comanda.Merchants.WebUI.Authentication;

global using Comanda.Merchants.WebUI.Http.Clients;
global using Comanda.Merchants.WebUI.Http.Payloads.Identity;
global using Comanda.Merchants.WebUI.Http.Interceptors;

global using Comanda.Internal.Contracts.Clients;
global using Comanda.Internal.Contracts.Clients.Interfaces;
global using Comanda.Internal.Contracts.Transport.Internal.Stores;
global using Comanda.Internal.Contracts.Transport.Internal.Profiles;

global using HttpsRichardy.Federation.Sdk.Contracts.Clients;
global using HttpsRichardy.Federation.Sdk.Contracts.Errors;
global using HttpsRichardy.Federation.Sdk.Contracts.Payloads.Identity;
global using HttpsRichardy.Federation.Sdk.Contracts.Payloads.User;
global using HttpsRichardy.Federation.Sdk.Contracts.Payloads.Group;
global using HttpsRichardy.Federation.Sdk.Contracts.Payloads.Permission;

global using HttpsRichardy.Internal.Essentials.Patterns;
global using HttpsRichardy.Internal.Essentials.Filtering;
global using MudBlazor.Services;
