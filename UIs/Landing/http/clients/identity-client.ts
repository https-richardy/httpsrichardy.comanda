import { Result } from "@/patterns/result";
import { AxiosInstance } from "axios";
import { AuthenticationCredentials, AuthenticationResult, UserScheme } from "../contracts/identity-schemes";

export class IdentityClient {
    private readonly httpClient: AxiosInstance;

    public constructor(httpClient: AxiosInstance) {
        this.httpClient = httpClient;
    }

    public async register(parameters: AuthenticationCredentials): Promise<Result<UserScheme>> {
        var response = await this.httpClient.post("/api/v1/identity", parameters);
        if (response.status < 200 || response.status >= 300) {
            return Result.failure<UserScheme>(response.data);
        }

        return Result.success<UserScheme>(response.data);
    }

    public async authenticate(parameters: AuthenticationCredentials): Promise<Result<AuthenticationResult>> {
        var response = await this.httpClient.post("/api/v1/identity/authenticate", parameters);
        if (response.status < 200 || response.status >= 300) {
            return Result.failure<AuthenticationResult>(response.data);
        }

        return Result.success<AuthenticationResult>(response.data);
    }
}
