global using System.Text.Json.Serialization;

global using Comanda.Payments.Domain.Aggregates;
global using Comanda.Payments.Domain.Errors;
global using Comanda.Payments.Domain.Concepts;
global using Comanda.Payments.Domain.Collections;
global using Comanda.Payments.Domain.Filtering;

global using Comanda.Payments.Application.Payloads;
global using Comanda.Payments.Application.Payloads.Payment;
global using Comanda.Payments.Application.Payloads.Events.Billing;

global using Comanda.Payments.Application.Gateways;
global using Comanda.Payments.Application.Mappers;

global using HttpsRichardy.Internal.Essentials.Contracts;
global using HttpsRichardy.Internal.Essentials.Patterns;
global using HttpsRichardy.Internal.Essentials.Aggregates;
global using HttpsRichardy.Internal.Essentials.Filtering;
global using HttpsRichardy.Internal.Essentials.Concepts;
global using HttpsRichardy.Internal.Essentials.Utilities;

global using HttpsRichardy.Dispatcher.Contracts;
global using FluentValidation;
