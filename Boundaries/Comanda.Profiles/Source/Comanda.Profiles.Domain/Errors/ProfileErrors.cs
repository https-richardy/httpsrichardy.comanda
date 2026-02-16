namespace Comanda.Profiles.Domain.Errors;

public static class ProfileErrors
{
    public static readonly Error ProfileAlreadyExists = new(
        Code: "#COMANDA-ERROR-76A71",
        Description: "A profile with this email already exists."
    );
}
