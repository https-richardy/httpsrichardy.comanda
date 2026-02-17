namespace Comanda.Orchestrator.CrossCutting.Constants;

public static class Permissions
{
    public const string ViewActivities = "permissions.activities.view";
    public const string ViewPayments = "permissions.payments.view";
    public const string MakePayment = "permissions.payments.make";

    public const string ViewOwners = "permissions.owners.view";
    public const string CreateOwners = "permissions.owners.create";
    public const string EditOwners = "permissions.owners.edit";
    public const string DeleteOwners = "permissions.owners.delete";

    public const string ViewCustomers = "permissions.customers.view";
    public const string CreateCustomers = "permissions.customers.create";
    public const string EditCustomers = "permissions.customers.edit";
    public const string DeleteCustomers = "permissions.customers.delete";

    public const string ViewEstablishments = "permissions.stores.establishments.view";
    public const string CreateEstablishment = "permissions.stores.establishments.create";
    public const string UpdateEstablishment = "permissions.stores.establishments.update";
    public const string DeleteEstablishment = "permissions.stores.establishments.delete";

    public const string ViewProducts = "permissions.stores.products.view";
    public const string CreateProduct = "permissions.stores.products.create";
    public const string UpdateProduct = "permissions.stores.products.update";
    public const string UploadProductImage = "permissions.stores.products.upload.image";
    public const string DeleteProduct = "permissions.stores.products.delete";

    public const string ViewCredentials = "permissions.stores.credentials.view";
    public const string CreateCredential = "permissions.stores.credentials.create";
    public const string UpdateCredential = "permissions.stores.credentials.update";

    public const string ViewSubscriptions = "permissions.subscriptions.view";
    public const string Subscribe = "permissions.subscriptions.subscribe";
    public const string CancelSubscription = "permissions.subscriptions.cancel";

    public const string ViewOrders = "permissions.orders.view";
    public const string CreateOrder = "permissions.orders.create";
    public const string UpdateOrder = "permissions.orders.update";
}
