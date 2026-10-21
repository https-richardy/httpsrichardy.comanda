global using System.Net;
global using System.Net.Mime;
global using System.Text;
global using System.Text.Json;

global using Microsoft.Net.Http.Headers;

global using Comanda.Internal.Contracts.Clients;
global using Comanda.Internal.Contracts.Errors;
global using Comanda.Internal.Contracts.Transport;
global using Comanda.Internal.Contracts.Transport.Authentication;
global using Comanda.Internal.Contracts.Transport.Authorization;

global using Comanda.Internal.Contracts.Tests.Fixtures;

global using AutoFixture;
global using WireMock.Server;
global using WireMock.RequestBuilders;
global using WireMock.ResponseBuilders;

global using HttpsRichardy.Internal.Essentials.Patterns;
