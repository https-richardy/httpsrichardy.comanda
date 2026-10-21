namespace Comanda.Internal.Contracts.Errors;

public static class AuthorizationErrors
{
    public static readonly Error UserDoesNotExist = new(
        Code: "#AUTHORIZATION-ERROR-A1D42",
        Description: "The specified user does not exist."
    );

    public static readonly Error UserAlreadyExists = new(
        Code: "#AUTHORIZATION-ERROR-B2E53",
        Description: "A user with the same username or email already exists."
    );

    public static readonly Error GroupDoesNotExist = new(
        Code: "#AUTHORIZATION-ERROR-C3F64",
        Description: "The specified group does not exist."
    );

    public static readonly Error GroupAlreadyExists = new(
        Code: "#AUTHORIZATION-ERROR-D4A75",
        Description: "A group with the same name already exists."
    );

    public static readonly Error RoleDoesNotExist = new(
        Code: "#AUTHORIZATION-ERROR-E5B86",
        Description: "The specified role does not exist."
    );

    public static readonly Error RoleAlreadyExists = new(
        Code: "#AUTHORIZATION-ERROR-F6C97",
        Description: "A role with the same name already exists."
    );

    public static readonly Error ClientDoesNotExist = new(
        Code: "#AUTHORIZATION-ERROR-C9F20",
        Description: "The specified client does not exist."
    );
}
