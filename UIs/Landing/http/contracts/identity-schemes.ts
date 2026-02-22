export type AuthenticationCredentials = {
    username: string;
    password: string;
};

export type AuthenticationResult = {
    accessToken: string;
    refreshToken: string;
}

export type UserScheme = {
    id: string;
    username: string;
}
