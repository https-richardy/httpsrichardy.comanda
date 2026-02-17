global using Microsoft.Extensions.Logging;

global using Comanda.Internal.Contracts.Errors;
global using Comanda.Internal.Contracts.Clients.Interfaces;

global using Comanda.Internal.Contracts.Transport.Internal.Profiles;
global using Comanda.Internal.Contracts.Transport.Internal.Payments;
global using Comanda.Internal.Contracts.Transport.Internal.Products;
global using Comanda.Internal.Contracts.Transport.Internal.Stores;
global using Comanda.Internal.Contracts.Transport.Internal.Orders;
global using Comanda.Internal.Contracts.Transport.Internal.Subscriptions;
global using Comanda.Internal.Contracts.Transport.Internal;

global using Comanda.Orchestrator.Application.Gateways;
global using Comanda.Orchestrator.Infrastructure.Policies;
global using HttpsRichardy.Internal.Essentials.Patterns;

global using Polly;
global using Polly.RateLimit;
global using Polly.Wrap;
