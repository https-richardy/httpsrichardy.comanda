import { ProfilesClient } from "@/http/clients/profiles-client";
import { federationClient, httpClient } from "./axios";
import { IdentityClient } from "@/http/clients/identity-client";

export const profilesClient = new ProfilesClient(httpClient);
export const identityClient = new IdentityClient(federationClient);
