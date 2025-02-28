/** Interface for outgoing Authentication Request */
export interface AuthRequest {
  email: string;
  password: string;
}

/** Interface for incoming Authentication Response */
export interface AuthResponse {
  access_token: string;
  token_type: string;
  expires_in: number;
  refresh_token: string;
}

/** Type for incoming Registration success message */
export type RegisterResponse = string;
