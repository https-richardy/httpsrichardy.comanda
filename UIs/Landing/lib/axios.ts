import LocalStorage from "@/constants/localstorage-constants";
import axios from "axios";

export const httpClient = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_URL,
    headers: {
        "Content-Type": "application/json"
    },
    validateStatus: () => true
});

export const federationClient = axios.create({
    baseURL: process.env.NEXT_PUBLIC_FEDERATION_API_URL,
    headers: {
        "Content-Type": "application/json",
        "Realm": process.env.NEXT_PUBLIC_REALM
    },
    validateStatus: () => true
});

httpClient.interceptors.request.use((configuration) => {
    const token = typeof window !== "undefined"
        ? localStorage.getItem(LocalStorage.AccessToken)
        : null;

    if (token) {
        configuration.headers.Authorization = `Bearer ${token}`;
    }

    return configuration;
});
