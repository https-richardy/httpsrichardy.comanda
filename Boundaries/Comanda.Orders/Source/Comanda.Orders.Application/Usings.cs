global using System.Text.Json.Serialization;

global using Comanda.Orders.Domain.Aggregates;
global using Comanda.Orders.Domain.Errors;
global using Comanda.Orders.Domain.Contracts;
global using Comanda.Orders.Domain.Concepts;
global using Comanda.Orders.Domain.Collections;
global using Comanda.Orders.Domain.Filtering;

global using Comanda.Orders.Application.Payloads;
global using Comanda.Orders.Application.Payloads.Order;
global using Comanda.Orders.Application.Mappers;

global using HttpsRichardy.Internal.Essentials.Utilities;
global using HttpsRichardy.Internal.Essentials.Patterns;
global using HttpsRichardy.Internal.Essentials.Filtering;
global using HttpsRichardy.Internal.Essentials.Concepts;

global using HttpsRichardy.Dispatcher.Contracts;
global using FluentValidation;
