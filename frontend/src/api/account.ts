import type { SignInPayload, SignInResponse } from "@/types/account";
import { post } from ".";

export async function signIn(payload: SignInPayload): Promise<SignInResponse> {
  return (await post<SignInPayload, SignInResponse>("/auth/sign/in", payload)).data;
}

export async function signOut(): Promise<void> {
  await post("/auth/sign/out");
}
