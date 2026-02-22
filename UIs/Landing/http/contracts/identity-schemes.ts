export type AuthenticationCredentials = {
    email: string;
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
