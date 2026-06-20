export interface LoginRequestDTO {
  username: string;
  password: string;
}

export interface LoginResponseDTO {
  accessToken: string;
}

export interface JwtPayloadDTO {
  sub: string;
  exp: number;
  iat: number;
  email: string;
  name: string;
  groups: string | string[];
}
