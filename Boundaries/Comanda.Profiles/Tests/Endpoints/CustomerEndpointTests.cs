namespace Comanda.Profiles.TestSuite.Endpoints;

public sealed class CustomerEndpointTests(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>,
    IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact(DisplayName = "[e2e] - when GET /api/v1/customers is called and there are customers, 200 OK is returned with the customers")]
    public async Task When_GetCustomers_IsCalled_AndThereAreCustomers_Then_200OkIsReturnedWithTheCustomers()
    {
        /* arrange: resolves http client and collection instances from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: create 3 non-deleted customers to be inserted in the database */
        var customers = _fixture.Build<Customer>()
            .With(customer => customer.IsDeleted, false)
            .CreateMany(3)
            .ToList();

        await collection.InsertManyAsync(customers, cancellation: TestContext.Current.CancellationToken);

        /* act: send GET request to the customers endpoint */
        var response = await httpClient.GetAsync("/api/v1/customers", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response and validate data structure */
        var result = JsonSerializer.Deserialize<IEnumerable<CustomerScheme>>(content, _serializerOptions);

        /* assert: ensure response contains customers */
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        /* assert: check if the number of returned customers matches the inserted ones */
        Assert.Equal(customers.Count, result.Count());
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/customers is called with valid data, 201 Created is returned with the created customer")]
    public async Task When_PostCustomers_IsCalled_WithValidData_Then_201CreatedIsReturnedWithTheCreatedCustomer()
    {
        /* arrange: resolves http client from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: create a valid customer payload according to validation rules */
        var request = _fixture.Build<CustomerCreationScheme>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Email, "richard.garcia@example.com")
            .With(customer => customer.PhoneNumber, "11999999999")
            .With(customer => customer.Username, "richardgarcia")
            .With(customer => customer.UserId, Identifier.Generate<Customer>())
            .Create();

        /* act: send POST request to create the new customer */
        var response = await httpClient.PostAsJsonAsync("/api/v1/customers", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status code and content body */
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response to validate returned structure */
        var result = JsonSerializer.Deserialize<CustomerScheme>(content, _serializerOptions);

        /* assert: validate returned data matches input fields */
        Assert.NotNull(result);

        Assert.Equal(request.FirstName, result.FirstName);
        Assert.Equal(request.LastName, result.LastName);

        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.PhoneNumber, result.PhoneNumber);

        Assert.Equal(request.Username, result.Username);
        Assert.Equal(request.UserId, result.UserId);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/customers is called with an existing email, 409 Conflict is returned [#COMANDA-ERROR-76A71]")]
    public async Task When_PostCustomers_IsCalled_WithExistingEmail_Then_409ConflictIsReturned()
    {
        /* arrange: resolve http client and customer collection */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: insert an existing customer with specific email */
        var existingCustomer = _fixture.Build<Customer>()
            .With(customer => customer.Contact, new Contact("richard.garcia@example.com", "11988888888"))
            .With(customer => customer.IsDeleted, false)
            .Create();

        await collection.InsertAsync(existingCustomer, cancellation: TestContext.Current.CancellationToken);

        /* arrange: create a new customer payload with the same email */
        var request = _fixture.Build<CustomerCreationScheme>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Email, "richard.garcia@example.com")
            .With(customer => customer.PhoneNumber, "11999999999")
            .With(customer => customer.Username, "richardgarcia2")
            .With(customer => customer.UserId, Identifier.Generate<Customer>())
            .Create();

        /* act: send POST request to create the duplicate customer */
        var response = await httpClient.PostAsJsonAsync("/api/v1/customers", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status and content body */
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-76A71 */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(ProfileErrors.ProfileAlreadyExists, error);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/customers/{id} is called with valid data, 200 OK is returned with the updated customer")]
    public async Task When_PutCustomers_IsCalled_WithValidData_Then_200OkIsReturnedWithTheUpdatedCustomer()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: insert an existing customer in the database */
        var existingCustomer = _fixture.Build<Customer>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(customer => customer.IsDeleted, false)
            .Create();

        await collection.InsertAsync(existingCustomer, cancellation: TestContext.Current.CancellationToken);

        /* arrange: create a valid edit payload with new data */
        var request = _fixture.Build<EditCustomerScheme>()
            .With(customer => customer.FirstName, "Ricardo")
            .With(customer => customer.LastName, "Gonçalves")
            .With(customer => customer.Email, "ricardo.goncalves@example.com")
            .With(customer => customer.PhoneNumber, "11988887777")
            .Create();

        /* act: send PUT request to update the existing customer */
        var response = await httpClient.PutAsJsonAsync($"/api/v1/customers/{existingCustomer.Id}", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status and content body */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response to validate returned structure */
        var result = JsonSerializer.Deserialize<CustomerScheme>(content, _serializerOptions);

        Assert.NotNull(result);

        Assert.Equal(existingCustomer.Id, result.Identifier);
        Assert.Equal(request.FirstName, result.FirstName);

        Assert.Equal(request.LastName, result.LastName);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.PhoneNumber, result.PhoneNumber);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/customers/{id} is called with a non-existent customer, 404 NotFound is returned [#COMANDA-ERROR-AF04C]")]
    public async Task When_PutCustomers_IsCalled_WithNonExistentCustomer_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: define a non-existent customer identifier */
        var nonExistentCustomerId = Identifier.Generate<Customer>();

        /* arrange: create a valid edit payload */
        var request = _fixture.Build<EditCustomerScheme>()
            .With(customer => customer.FirstName, "Ricardo")
            .With(customer => customer.LastName, "Gonçalves")
            .With(customer => customer.Email, "ricardo.goncalves@example.com")
            .With(customer => customer.PhoneNumber, "11988887777")
            .Create();

        /* act: send PUT request for a non-existent customer */
        var response = await httpClient.PutAsJsonAsync($"/api/v1/customers/{nonExistentCustomerId}", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status and content body */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-AF04C */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(CustomerErrors.CustomerDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/customers/{id} is called with an existing customer, 204 NoContent is returned and the customer is marked as deleted")]
    public async Task When_DeleteCustomers_IsCalled_WithExistingCustomer_Then_204NoContentIsReturned_AndCustomerIsMarkedAsDeleted()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: insert an existing customer in the database */
        var existingCustomer = _fixture.Build<Customer>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(customer => customer.IsDeleted, false)
            .Create();

        await collection.InsertAsync(existingCustomer, cancellation: TestContext.Current.CancellationToken);

        /* act: send DELETE request to remove the existing customer */
        var response = await httpClient.DeleteAsync($"/api/v1/customers/{existingCustomer.Id}", TestContext.Current.CancellationToken);

        /* assert: verify http response status */
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        /* assert: fetch the deleted customer from the collection */
        var filters = CustomerFilters.WithSpecifications()
            .WithIdentifier(existingCustomer.Id)
            .WithIsDeleted(true)
            .Build();

        var deletedCustomers = await collection.GetCustomersAsync(filters, TestContext.Current.CancellationToken);
        var deletedCustomer = deletedCustomers.FirstOrDefault();

        /* assert: ensure customer exists and is marked as deleted */
        Assert.NotNull(deletedCustomer);
        Assert.True(deletedCustomer.IsDeleted);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/customers/{id} is called with a non-existing customer, 404 NotFound is returned [#COMANDA-ERROR-AF04C]")]
    public async Task When_DeleteCustomers_IsCalled_WithNonExistingCustomer_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: create a random non-existing customer id */
        var nonExistingCustomerId = Identifier.Generate<Customer>();

        /* act: send DELETE request for a non-existing customer */
        var response = await httpClient.DeleteAsync($"/api/v1/customers/{nonExistingCustomerId}", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and body */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-AF04C */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(CustomerErrors.CustomerDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when GET /api/v1/customers/{id}/addresses is called for an existing customer, 200 OK is returned with the customer's addresses")]
    public async Task When_GetCustomerAddresses_IsCalled_ForExistingCustomer_Then_200OkIsReturnedWithAddresses()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: create a customer with 3 valid addresses */
        var addresses = _fixture.Build<Address>()
            .With(address => address.Street, "Rua das Flores")
            .With(address => address.Number, "123")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01000000")
            .With(address => address.Neighborhood, "Centro")
            .CreateMany(3)
            .ToList();

        var customer = _fixture.Build<Customer>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(customer => customer.Addresses, addresses)
            .With(customer => customer.IsDeleted, false)
            .Create();

        await collection.InsertAsync(customer, cancellation: TestContext.Current.CancellationToken);

        /* act: send GET request to retrieve the customer's addresses */
        var response = await httpClient.GetAsync($"/api/v1/customers/{customer.Id}/addresses", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response and content body */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and validate address list */
        var result = JsonSerializer.Deserialize<List<Address>>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        Assert.Equal(addresses.Count, result.Count);

        /* assert: ensure addresses match persisted data */
        foreach (var address in addresses)
        {
            Assert.Contains(result, result =>
                result.Street == address.Street &&
                result.Number == address.Number &&
                result.City == address.City &&
                result.State == address.State &&
                result.ZipCode == address.ZipCode
            );
        }
    }

    [Fact(DisplayName = "[e2e] - when GET /api/v1/customers/{id}/addresses is called for a non-existing customer, 404 NotFound is returned [#COMANDA-ERROR-AF04C]")]
    public async Task When_GetCustomerAddresses_IsCalled_ForNonExistingCustomer_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: generate a random non-existing customer id */
        var nonExistingCustomerId = Identifier.Generate<Customer>();

        /* act: send GET request for non-existing customer addresses */
        var response = await httpClient.GetAsync($"/api/v1/customers/{nonExistingCustomerId}/addresses", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status and content body */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and validate returned error */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(CustomerErrors.CustomerDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/customers/{id}/addresses is called with a valid request, 201 Created is returned and the address is persisted")]
    public async Task When_PostCustomersAddresses_IsCalled_WithValidRequest_Then_201CreatedIsReturned_AndAddressIsPersisted()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: insert an existing customer in the database */
        var existingCustomer = _fixture.Build<Customer>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(customer => customer.IsDeleted, false)
            .Without(customer => customer.Addresses)
            .Create();

        await collection.InsertAsync(existingCustomer, cancellation: TestContext.Current.CancellationToken);

        /* arrange: prepare a valid address assignment request */
        var request = _fixture.Build<AssignCustomerAddressScheme>()
            .With(address => address.Street, "Rua das Palmeiras")
            .With(address => address.Number, "100")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01001-000")
            .With(address => address.Neighborhood, "Centro")
            .With(address => address.Complement, "Apto 21")
            .With(address => address.Reference, "Próximo à praça")
            .Create();

        /* act: send POST request to assign address to existing customer */
        var response = await httpClient.PostAsJsonAsync($"/api/v1/customers/{existingCustomer.Id}/addresses", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and body */
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize address returned by API */
        var result = JsonSerializer.Deserialize<Address>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.Equal("Rua das Palmeiras", result.Street);

        /* assert: fetch customer from database and verify address persisted */
        var filters = CustomerFilters.WithSpecifications()
            .WithIdentifier(existingCustomer.Id)
            .Build();

        var customers = await collection.GetCustomersAsync(filters, TestContext.Current.CancellationToken);
        var customer = customers.FirstOrDefault();

        Assert.NotNull(customer);
        Assert.Single(customer.Addresses);
        Assert.Equal("Rua das Palmeiras", customer.Addresses.First().Street);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/customers/{id}/addresses is called with a non-existing customer, 404 NotFound is returned [#COMANDA-ERROR-AF04C]")]
    public async Task When_PostCustomersAddresses_IsCalled_WithNonExistingCustomer_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: create a random non-existing customer id */
        var nonExistingCustomerId = Identifier.Generate<Customer>();

        /* arrange: prepare a valid address assignment request using fixture */
        var request = _fixture.Build<AssignCustomerAddressScheme>()
            .With(customer => customer.Street, "Rua das Palmeiras")
            .With(customer => customer.Number, "100")
            .With(customer => customer.City, "São Paulo")
            .With(customer => customer.State, "SP")
            .With(customer => customer.ZipCode, "01001-000")
            .With(customer => customer.Neighborhood, "Centro")
            .With(customer => customer.Complement, "Apto 21")
            .With(customer => customer.Reference, "Próximo à praça")
            .Create();

        /* act: send POST request for a non-existing customer */
        var response = await httpClient.PostAsJsonAsync($"/api/v1/customers/{nonExistingCustomerId}/addresses", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and body */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-AF04C */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(CustomerErrors.CustomerDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/customers/{id}/addresses is called with valid data, 200 OK is returned and the address is updated")]
    public async Task When_PutCustomersAddresses_IsCalled_WithValidData_Then_200OkIsReturned_AndAddressIsUpdated()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: insert an existing customer with one address in the database */
        var existingAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Rua das Palmeiras")
            .With(address => address.Number, "100")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01001-000")
            .With(address => address.Neighborhood, "Centro")
            .With(address => address.Complement, "Apto 21")
            .With(address => address.Reference, "Próximo à praça")
            .Create();

        var existingCustomer = _fixture.Build<Customer>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(customer => customer.IsDeleted, false)
            .With(customer => customer.Addresses, [existingAddress])
            .Create();

        await collection.InsertAsync(existingCustomer, cancellation: TestContext.Current.CancellationToken);

        /* arrange: create edit request replacing the existing address */
        var replacementAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Avenida Paulista")
            .With(address => address.Number, "200")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01310-200")
            .With(address => address.Neighborhood, "Bela Vista")
            .With(address => address.Complement, "Apto 101")
            .With(address => address.Reference, "Próximo ao MASP")
            .Create();

        var request = _fixture.Build<EditCustomerAddressScheme>()
            .With(customer => customer.Target, existingAddress)
            .With(customer => customer.Replacement, replacementAddress)
            .Create();

        /* act: send PUT request to update the address */
        var response = await httpClient.PutAsJsonAsync($"/api/v1/customers/{existingCustomer.Id}/addresses", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and body */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize the updated address returned by API */
        var result = JsonSerializer.Deserialize<Address>(content, _serializerOptions);

        Assert.NotNull(result);

        Assert.Equal("Avenida Paulista", result.Street);
        Assert.Equal("200", result.Number);

        /* assert: fetch customer from database and verify the address was updated */
        var filters = CustomerFilters.WithSpecifications()
            .WithIdentifier(existingCustomer.Id)
            .Build();

        var customers = await collection.GetCustomersAsync(filters, TestContext.Current.CancellationToken);
        var customer = customers.FirstOrDefault();

        Assert.NotNull(customer);
        Assert.Single(customer.Addresses);

        Assert.Equal("Avenida Paulista", customer.Addresses.First().Street);
        Assert.Equal("200", customer.Addresses.First().Number);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/customers/{id}/addresses is called with a non-existing customer, 404 NotFound is returned [#COMANDA-ERROR-AF04C]")]
    public async Task When_PutCustomersAddresses_IsCalled_WithNonExistingCustomer_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: create a random non-existing customer id */
        var nonExistingCustomerId = Identifier.Generate<Customer>();

        /* arrange: prepare edit request using fixture */
        var targetAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Rua das Palmeiras")
            .With(address => address.Number, "100")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01001-000")
            .With(address => address.Neighborhood, "Centro")
            .With(address => address.Complement, "Apto 21")
            .With(address => address.Reference, "Próximo à praça")
            .Create();

        var replacementAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Avenida Paulista")
            .With(address => address.Number, "200")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01310-200")
            .With(address => address.Neighborhood, "Bela Vista")
            .With(address => address.Complement, "Apto 101")
            .With(address => address.Reference, "Próximo ao MASP")
            .Create();

        var request = _fixture.Build<EditCustomerAddressScheme>()
            .With(customer => customer.Target, targetAddress)
            .With(customer => customer.Replacement, replacementAddress)
            .Create();

        /* act: send PUT request for a non-existing customer */
        var response = await httpClient.PutAsJsonAsync($"/api/v1/customers/{nonExistingCustomerId}/addresses", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and body */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-AF04C */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(CustomerErrors.CustomerDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/customers/{id}/addresses is called with the same target and replacement address, 409 Conflict is returned [#COMANDA-ERROR-4901F]")]
    public async Task When_PutCustomersAddresses_IsCalled_WithSameTargetAndReplacement_Then_409ConflictIsReturned()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: create an existing address using fixture */
        var existingAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Rua das Palmeiras")
            .With(address => address.Number, "100")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01001-000")
            .With(address => address.Neighborhood, "Centro")
            .With(address => address.Complement, "Apto 21")
            .With(address => address.Reference, "Próximo à praça")
            .Create();

        /* arrange: create an existing customer with this address */
        var existingCustomer = _fixture.Build<Customer>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(customer => customer.IsDeleted, false)
            .With(customer => customer.Addresses, [existingAddress])
            .Create();

        await collection.InsertAsync(existingCustomer, cancellation: TestContext.Current.CancellationToken);

        /* arrange: prepare edit request where target and replacement are the same */
        var request = _fixture.Build<EditCustomerAddressScheme>()
            .With(customer => customer.Target, existingAddress)
            .With(customer => customer.Replacement, existingAddress)
            .Create();

        /* act: send PUT request to update the address */
        var response = await httpClient.PutAsJsonAsync($"/api/v1/customers/{existingCustomer.Id}/addresses", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and body */
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/customers/{id}/addresses is called with a non-existing target address, 404 NotFound is returned [#COMANDA-ERROR-2616B]")]
    public async Task When_PutCustomersAddresses_IsCalled_WithNonExistingTargetAddress_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: insert an existing customer with one address in the database */
        var existingAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Rua das Palmeiras")
            .With(address => address.Number, "100")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01001-000")
            .With(address => address.Neighborhood, "Centro")
            .With(address => address.Complement, "Apto 21")
            .With(address => address.Reference, "Próximo à praça")
            .Create();

        var existingCustomer = _fixture.Build<Customer>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(customer => customer.IsDeleted, false)
            .With(customer => customer.Addresses, [existingAddress])
            .Create();

        await collection.InsertAsync(existingCustomer, cancellation: TestContext.Current.CancellationToken);

        /* arrange: prepare edit request with a target address that does not exist */
        var nonExistingTargetAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Rua Inexistente")
            .With(address => address.Number, "999")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "99999-999")
            .With(address => address.Neighborhood, "Centro")
            .With(address => address.Complement, "Apto 999")
            .With(address => address.Reference, "Nenhum")
            .Create();

        var replacementAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Avenida Paulista")
            .With(address => address.Number, "200")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01310-200")
            .With(address => address.Neighborhood, "Bela Vista")
            .With(address => address.Complement, "Apto 101")
            .With(address => address.Reference, "Próximo ao MASP")
            .Create();

        var request = _fixture.Build<EditCustomerAddressScheme>()
            .With(customer => customer.Target, nonExistingTargetAddress)
            .With(customer => customer.Replacement, replacementAddress)
            .Create();

        /* act: send PUT request to update a non-existing target address */
        var response = await httpClient.PutAsJsonAsync($"/api/v1/customers/{existingCustomer.Id}/addresses", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and body */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-2616B */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(CustomerErrors.AddressDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/customers/{id}/addresses is called with an existing customer and address, 204 NoContent is returned and the address is removed")]
    public async Task When_DeleteCustomersAddresses_IsCalled_WithExistingCustomerAndAddress_Then_204NoContentIsReturned_AndAddressIsRemoved()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: insert an existing customer with one address in the database */
        var existingAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Rua das Palmeiras")
            .With(address => address.Number, "100")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01001-000")
            .With(address => address.Neighborhood, "Centro")
            .With(address => address.Complement, "Apto 21")
            .With(address => address.Reference, "Próximo à praça")
            .Create();

        var existingCustomer = _fixture.Build<Customer>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(customer => customer.IsDeleted, false)
            .With(customer => customer.Addresses, [existingAddress])
            .Create();

        await collection.InsertAsync(existingCustomer, cancellation: TestContext.Current.CancellationToken);

        /* arrange: prepare delete request using fixture */
        var request = _fixture.Build<DeleteCustomerAddressScheme>()
            .With(customer => customer.Target, existingAddress)
            .Create();

        /* act: send DELETE request to remove the address */
        var httpMessage = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/customers/{existingCustomer.Id}/addresses")
        {
            Content = JsonContent.Create(request, options: _serializerOptions)
        };

        var response = await httpClient.SendAsync(httpMessage, TestContext.Current.CancellationToken);

        /* assert: verify http response status */
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        /* assert: fetch customer from database and verify address was removed */
        var filters = CustomerFilters.WithSpecifications()
            .WithIdentifier(existingCustomer.Id)
            .Build();

        var customers = await collection.GetCustomersAsync(filters, TestContext.Current.CancellationToken);
        var customer = customers.FirstOrDefault();

        Assert.NotNull(customer);
        Assert.Empty(customer.Addresses);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/customers/{id}/addresses is called with a non-existing address, 404 NotFound is returned [#COMANDA-ERROR-2616B]")]
    public async Task When_DeleteCustomersAddresses_IsCalled_WithNonExistingAddress_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ICustomerCollection>();

        /* arrange: insert an existing customer with no addresses */
        var existingCustomer = _fixture.Build<Customer>()
            .With(customer => customer.FirstName, "Richard")
            .With(customer => customer.LastName, "Garcia")
            .With(customer => customer.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(customer => customer.IsDeleted, false)
            .Without(customer => customer.Addresses)
            .Create();

        await collection.InsertAsync(existingCustomer, cancellation: TestContext.Current.CancellationToken);

        /* arrange: prepare delete request with a non-existing address */
        var nonExistingAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Rua Inexistente")
            .With(address => address.Number, "999")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "00000-000")
            .With(address => address.Neighborhood, "Desconhecido")
            .With(address => address.Complement, "Sem complemento")
            .With(address => address.Reference, "Sem referência")
            .Create();

        var request = _fixture.Build<DeleteCustomerAddressScheme>()
            .With(customer => customer.Target, nonExistingAddress)
            .Create();

        /* act: send DELETE request */
        var httpMessage = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/customers/{existingCustomer.Id}/addresses")
        {
            Content = JsonContent.Create(request, options: _serializerOptions)
        };

        var response = await httpClient.SendAsync(httpMessage, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and body */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-2616B */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(CustomerErrors.AddressDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/customers/{id}/addresses is called with a non-existing customer, 404 NotFound is returned [#COMANDA-ERROR-AF04C]")]
    public async Task When_DeleteCustomersAddresses_IsCalled_WithNonExistingCustomer_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: create a random non-existing customer id */
        var nonExistingCustomerId = Identifier.Generate<Customer>();

        /* arrange: prepare delete request using fixture */
        var targetAddress = _fixture.Build<Address>()
            .With(address => address.Street, "Rua das Palmeiras")
            .With(address => address.Number, "100")
            .With(address => address.City, "São Paulo")
            .With(address => address.State, "SP")
            .With(address => address.ZipCode, "01001-000")
            .With(address => address.Neighborhood, "Centro")
            .With(address => address.Complement, "Apto 21")
            .With(address => address.Reference, "Próximo à praça")
            .Create();

        var request = _fixture.Build<DeleteCustomerAddressScheme>()
            .With(customer => customer.Target, targetAddress)
            .Create();

        /* act: send DELETE request for a non-existing customer */
        var httpMessage = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/customers/{nonExistingCustomerId}/addresses")
        {
            Content = JsonContent.Create(request, options: _serializerOptions)
        };

        var response = await httpClient.SendAsync(httpMessage, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and body */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-AF04C */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(CustomerErrors.CustomerDoesNotExist, error);
    }

    public ValueTask InitializeAsync() => factory.InitializeAsync();
    public ValueTask DisposeAsync() => factory.DisposeAsync();
}
