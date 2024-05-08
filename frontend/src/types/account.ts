export type Credentials = {
  emailAddress: string;
  password?: string;
};

export type CurrentUser = {
  displayName: string;
  emailAddress?: string;
  pictureUrl?: string;
};

export type SignInPayload = {
  locale: string;
  credentials?: Credentials;
  authenticationToken?: string;
  // TODO(fpion): oneTimePassword?: OneTimePasswordPayload
  // TODO(fpion): profile?: CompleteProfilePayload
};

export type SignInResponse = {
  // TODO(fpion): authenticationLinkSentTo?: SentMessage
  isPasswordRequired: boolean;
  // TODO(fpion): oneTimePasswordValidation?: OneTimePasswordValidation
  profileCompletionToken?: string;
  currentUser?: CurrentUser;
};
