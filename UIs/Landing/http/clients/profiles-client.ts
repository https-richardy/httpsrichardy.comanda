import { AxiosInstance } from "axios";
import { OwnerScheme, ProfileCreationScheme } from "../contracts/profiles-schemes";
import { Result } from "@/patterns/result";

export class ProfilesClient {
    private readonly httpClient: AxiosInstance;

    public constructor(httpClient: AxiosInstance) {
        this.httpClient = httpClient;
    }

    public async createOwner(parameters: ProfileCreationScheme): Promise<Result<OwnerScheme>> {
        var response = await this.httpClient.post("/api/v1/profiles/owner", parameters);
        if (response.status < 200 || response.status >= 300) {
            return Result.failure<OwnerScheme>(response.data);
        }

        return Result.success<OwnerScheme>(response.data);
    }
}
