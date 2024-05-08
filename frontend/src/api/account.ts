import type { SignInPayload, SignInResponse, UserProfile } from "@/types/account";
import { get, post } from ".";

export async function getProfile(): Promise<UserProfile> {
  return (await get<UserProfile>("/profile")).data;
}

export async function signIn(payload: SignInPayload): Promise<SignInResponse> {
  return (await post<SignInPayload, SignInResponse>("/auth/sign/in", payload)).data;
}

export async function signOut(): Promise<void> {
  await post("/auth/sign/out");
}
