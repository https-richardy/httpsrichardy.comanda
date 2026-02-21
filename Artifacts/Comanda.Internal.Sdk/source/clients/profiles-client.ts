import { AxiosInstance } from "axios";
import { Result } from "../patterns/result";
import { Error } from "../patterns/error";

import { ProfileCreationScheme } from "../transport/profiles/profile-creation-scheme";
import { ProfileScheme } from "../transport/profiles/profile-scheme";

export class ProfilesClient {
    private readonly httpClient: AxiosInstance;

    public constructor(httpClient: AxiosInstance) {
        this.httpClient = httpClient;
    }

    public async createOwner(parameters: ProfileCreationScheme): Promise<Result<ProfileScheme>>
    {
        const response = await this.httpClient.post("/api/v1/profiles/owner", parameters);
        if (response.status < 200 || response.status >= 300) {
            return Result.failure(Error.from(response.data.code, response.data.description));
        }

        return Result.success(response.data);
    }

    public async createCustomer(parameters: ProfileCreationScheme): Promise<Result<void>>
    {
        const response = await this.httpClient.post("/api/v1/profiles/owner", parameters);
        if (response.status < 200 || response.status >= 300) {
            return Result.failure(Error.from(response.data.code, response.data.description));
        }

        return Result.success(response.data);
    }
}
