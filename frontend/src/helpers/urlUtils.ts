import { stringUtils } from "logitar-js";

const { cleanTrim, isNullOrWhiteSpace, trimStart } = stringUtils;

/**
 * Represents credentials to be used in an authentication system.
 */
export class Credentials implements ICredentials {
  private identifier: string;
  /**
   * Returns the identifier of the credentials, typically an username.
   * @returns The identifier of the credentials.
   */
  getIdentifier(): string {
    return this.identifier;
  }
  /**
   * Sets the identifier of the credentials, typically an username.
   * @param identifier The identifier to set.
   */
  setIdentifier(identifier: string): void {
    this.identifier = identifier;
  }

  private secret: string;
  /**
   * Returns the secret of the credentials, typically a password.
   * @returns The secret of the credentials.
   */
  getSecret(): string {
    return this.secret;
  }
  /**
   * Sets the secret of the credentials, typically a password.
   * @param secret The secret to set.
   */
  setSecret(secret: string): void {
    this.secret = secret;
  }

  /**
   * Creates a new instance of the `Credentials` class.
   * @param identifier The identifier of the credentials, typically an username.
   * @param secret The secret of the credentials, typically a password.
   */
  constructor(identifier: string = "", secret: string = "") {
    this.identifier = identifier;
    this.secret = secret;
  }

  /**
   * Parses the specified credentials using the `{identifier}:{secret}` format into a new instance of the `Credentials` class.
   * @param credentials
   * @returns The created instance.
   */
  static parse(credentials?: string): Credentials | undefined {
    credentials = cleanTrim(credentials);
    if (typeof credentials !== "string") {
      return undefined;
    }

    const index: number = credentials.indexOf(":");
    if (index < 0) {
      return new Credentials(credentials);
    }
    return new Credentials(credentials.substring(0, index), credentials.substring(index + 1));
  }
}

/**
 * Defines credentials to be used in an authentication system.
 */
export interface ICredentials {
  /**
   * Returns the identifier of the credentials, typically an username.
   * @returns The identifier of the credentials.
   */
  getIdentifier(): string;
  /**
   * Returns the secret of the credentials, typically a password.
   * @returns The secret of the credentials.
   */
  getSecret(): string;
}

/**
 * Defines a builder class used to build URLs.
 */
export interface IUrlBuilder {
  /**
   * Returns the scheme of the URL.
   * @returns The scheme of the URL.
   */
  getScheme(): string;
  /**
   * Sets the scheme of the URL.
   * @param scheme The scheme to set.
   * @param inferPort If true, the port will be inferred from the scheme.
   * @returns The instance of the builder.
   */
  setScheme(scheme: string, inferPort?: boolean): IUrlBuilder;

  /**
   * Returns the credentials of the URL.
   * @returns The credentials of the URL.
   */
  getCredentials(): ICredentials | undefined;
  /**
   * Sets the credentials of the URL.
   * @param credentials The credentials to set.
   * @returns The instance of the builder.
   */
  setCredentials(credentials?: ICredentials): IUrlBuilder;
  /**
   * Returns the host of the URL.
   * @returns The host of the URL.
   */
  getHost(): string;
  /**
   * Sets the host of the URL. The default host will be set if the specified host is empty.
   * @param host The host of the URL.
   * @returns The instance of the builder.
   */
  setHost(host: string): IUrlBuilder;
  /**
   * Returns the port of the URL.
   * @returns The port of the URL.
   */
  getPort(): number;
  /**
   * Sets the portal of the URL.
   * @param port The port of the URL.
   * @returns The instance of the builder.
   */
  setPort(port: number): IUrlBuilder;
  /**
   * Returns the authority of the URL.
   * @returns The authority of the URL.
   */
  getAuthority(): string;
  /**
   * Sets the authority of the URL.
   * @param authority The authority of the URL.
   * @returns The instance of the builder.
   */
  setAuthority(authority: string): IUrlBuilder;

  /**
   * Returns the list of segments of the URL path.
   * @returns The segments of the URL.
   */
  getSegments(): string[];
  /**
   * Sets the segments of the URL path. Empty segments will be discarded.
   * @param segments The segments of the URL.
   * @returns The instance of the builder.
   */
  setSegments(segments: string[]): IUrlBuilder;
  /**
   * Returns the path of the URL.
   * @returns The path of the URL.
   */
  getPath(): string | undefined;
  /**
   * Sets the path of the URL.
   * @param path The path of the URL.
   * @returns The instance of the builder.
   */
  setPath(path?: string): IUrlBuilder;

  /**
   * Returns the query parameters of the URL.
   * @returns The query parameters of the URL.
   */
  getQuery(): Map<string, string[]>;
  /**
   * Returns the query string of the URL.
   * @returns The query string of the URL.
   */
  getQueryString(): string | undefined;
  /**
   * Adds a query parameter to the URL. The specified value will be appended to existing values associated to this key. Empty keys and values will be discarded.
   * @param key The key of the parameter.
   * @param values The value of the parameter.
   * @returns The instance of the builder.
   */
  addQuery(key: string, value: string): IUrlBuilder;
  /**
   * Adds a query parameter to the URL. The specified values will be appended to existing values associated to this key. Empty keys and values will be discarded.
   * @param key The key of the parameter.
   * @param values The values of the parameter.
   * @returns The instance of the builder.
   */
  addQuery(key: string, values: string[]): IUrlBuilder;
  /**
   * Sets a query parameter to the URL. The specified value will replace the existing values associated to this key. Empty keys and values will be discarded.
   * @param key The key of the parameter.
   * @param values The value of the parameter.
   * @returns The instance of the builder.
   */
  setQuery(key: string, value: string): IUrlBuilder;
  /**
   * Sets a query parameter to the URL. The specified values will replace the existing values associated to this key. Empty keys and values will be discarded.
   * @param key The key of the parameter.
   * @param values The values of the parameter.
   * @returns The instance of the builder.
   */
  setQuery(key: string, values: string[]): IUrlBuilder;
  /**
   * Sets the query string of the URL. Empty keys and values will be discarded.
   * @param queryString The query string of the URL.
   * @returns The instance of the builder.
   */
  setQueryString(queryString?: string): IUrlBuilder;

  /**
   * Returns the fragment of the URL.
   * @returns The fragment of the URL.
   */
  getFragment(): string | undefined;
  /**
   * Sets the fragment of the URL.
   * @param fragment The fragment of the URL.
   * @returns The instance of the builder.
   */
  setFragment(fragment?: string): IUrlBuilder;

  /**
   * Returns the parameters of the URL. Parameters are tokens that replace values in built URLs.
   * @returns The parameters of the URL.
   */
  getParameters(): Map<string, string>;
  /**
   * Sets a parameter of the URL. Parameters are tokens that replace values in built URLs.
   * @param key The key of the parameter.
   * @param value The value of the parameter.
   * @returns The instance of the builder.
   * @throws {Error} The key is empty.
   */
  setParameter(key: string, value?: string): IUrlBuilder;

  /**
   * Builds an URL of the specified kind.
   * @param kind The URL kind (defaults to `Absolute`).
   * @returns The built URL.
   */
  build(kind?: UriKind): string;
  /**
   * Builds an absolute URL.
   * @returns The built URL.
   */
  buildAbsolute(): string;
  /**
   * Builds a relative URL.
   * @returns The built URL.
   */
  buildRelative(): string;
}

/**
 * Defines the supported URL kinds.
 */
export type UriKind = "Absolute" | "Relative";

/**
 * Represents a builder class used to build URLs.
 */
export class UrlBuilder implements IUrlBuilder {
  /**
   * The default URL scheme.
   */
  static readonly DEFAULT_SCHEME = "http";
  /**
   * The default URL host.
   */
  static readonly DEFAULT_HOST = "localhost";

  private static supportedSchemes: Set<string> = new Set<string>(["http", "https"]);
  /**
   * Returns the list of supported URL schemes.
   * @returns The supported URL schemes.
   */
  static getSupportedSchemes(): string[] {
    return [...UrlBuilder.supportedSchemes];
  }
  /**
   * Returns a value indicating whether or not the specified scheme is supported.
   * @param scheme The scheme to check.
   * @returns True if the scheme is supported, or false otherwise.
   */
  static isSchemeSupported(scheme: string): boolean {
    return UrlBuilder.supportedSchemes.has(scheme.trim().toLowerCase());
  }

  private scheme: string = UrlBuilder.DEFAULT_SCHEME;
  /**
   * Returns the scheme of the URL.
   * @returns The scheme of the URL.
   */
  getScheme(): string {
    return this.scheme;
  }
  /**
   * Sets the scheme of the URL.
   * @param scheme The scheme to set.
   * @param inferPort If true, the port will be inferred from the scheme.
   * @returns The instance of the builder.
   * @throws {Error} The scheme is not supported.
   */
  setScheme(scheme: string, inferPort?: boolean): IUrlBuilder {
    if (!UrlBuilder.isSchemeSupported(scheme)) {
      throw new Error(`The scheme '${scheme}' is not supported.`);
    }
    this.scheme = scheme.trim().toLowerCase();
    if (inferPort) {
      this.port = UrlBuilder.inferPort(scheme);
    }
    return this;
  }

  private credentials?: ICredentials;
  /**
   * Returns the credentials of the URL.
   * @returns The credentials of the URL.
   */
  getCredentials(): ICredentials | undefined {
    return this.credentials;
  }
  /**
   * Sets the credentials of the URL.
   * @param credentials The credentials to set.
   * @returns The instance of the builder.
   */
  setCredentials(credentials?: ICredentials): IUrlBuilder {
    this.credentials = credentials;
    return this;
  }
  private host: string = UrlBuilder.DEFAULT_HOST;
  /**
   * Returns the host of the URL.
   * @returns The host of the URL.
   */
  getHost(): string {
    return this.host;
  }
  /**
   * Sets the host of the URL. The default host will be set if the specified host is empty.
   * @param host The host of the URL.
   * @returns The instance of the builder.
   */
  setHost(host: string): IUrlBuilder {
    this.host = cleanTrim(host) ?? UrlBuilder.DEFAULT_HOST;
    return this;
  }
  private port: number = 80;
  /**
   * Returns the port of the URL.
   * @returns The port of the URL.
   */
  getPort(): number {
    return this.port;
  }
  /**
   * Sets the portal of the URL.
   * @param port The port of the URL.
   * @returns The instance of the builder.
   * @throws {Error} The port is not a number or not within port ranges (0-65535).
   */
  setPort(port: number): IUrlBuilder {
    if (isNaN(port) || port < 0 || port > 65535) {
      throw new Error(`The port '${port}' must be a value between 0 and 65535.`);
    }
    this.port = port;
    return this;
  }
  /**
   * Returns the authority of the URL.
   * @returns The authority of the URL.
   */
  getAuthority(): string {
    let authority = "";
    if (this.credentials) {
      authority += `${this.credentials.getIdentifier()}:${this.credentials.getSecret()}@`;
    }
    authority += `${this.host}:${this.port}`;
    return authority;
  }
  /**
   * Sets the authority of the URL.
   * @param authority The authority of the URL.
   * @returns The instance of the builder.
   * @throws {Error} The value is not a valid URL authority.
   */
  setAuthority(authority: string): IUrlBuilder {
    const parts: string[] = authority.split("@");
    if (parts.length > 2) {
      throw new Error(`The value '${authority}' is not a valid URL authority.`);
    } else if (parts.length === 2) {
      this.setCredentials(Credentials.parse(parts[0]));
    }

    const endPoint: string = parts[parts.length - 1];
    const index: number = endPoint.indexOf(":");
    if (index < 0) {
      this.setHost(endPoint);
    } else {
      this.setHost(endPoint.substring(0, index));
      this.setPort(Number(endPoint.substring(index + 1)));
    }

    return this;
  }

  private segments: string[] = [];
  /**
   * Returns the list of segments of the URL path.
   * @returns The segments of the URL.
   */
  getSegments(): string[] {
    return [...this.segments];
  }
  /**
   * Sets the segments of the URL path. Empty segments will be discarded.
   * @param segments The segments of the URL.
   * @returns The instance of the builder.
   */
  setSegments(segments: string[]): IUrlBuilder {
    this.segments.length = 0;
    segments.forEach((segment) => {
      if (!isNullOrWhiteSpace(segment)) {
        this.segments.push(segment.trim());
      }
    });
    return this;
  }
  /**
   * Returns the path of the URL.
   * @returns The path of the URL.
   */
  getPath(): string | undefined {
    return this.segments.length === 0 ? undefined : `/${this.segments.join("/")}`;
  }
  /**
   * Sets the path of the URL.
   * @param path The path of the URL.
   * @returns The instance of the builder.
   */
  setPath(path?: string): IUrlBuilder {
    this.setSegments(path?.split("/") ?? []);
    return this;
  }

  private query: Map<string, string[]> = new Map<string, string[]>();
  /**
   * Returns the query parameters of the URL.
   * @returns The query parameters of the URL.
   */
  getQuery(): Map<string, string[]> {
    return new Map<string, string[]>(this.query);
  }
  /**
   * Returns the query string of the URL.
   * @returns The query string of the URL.
   */
  getQueryString(): string | undefined {
    if (this.query.size === 0) {
      return undefined;
    }
    const parameters: string[] = [];
    this.query.forEach((values, key) => values.forEach((value) => parameters.push([key, value].join("="))));
    return `?${parameters.join("&")}`;
  }
  /**
   * Adds a query parameter to the URL. The specified values will be appended to existing values associated to this key. Empty keys and values will be discarded.
   * @param key The key of the parameter.
   * @param values The value or the values of the parameter.
   * @returns The instance of the builder.
   */
  addQuery(key: string, values: string | string[]): IUrlBuilder {
    if (typeof values === "string") {
      return this.addQuery(key, [values]);
    }
    if (!isNullOrWhiteSpace(key)) {
      key = key.trim();
      const existingValues: string[] = this.query.get(key) ?? [];
      values.forEach((value) => {
        if (!isNullOrWhiteSpace(value)) {
          existingValues.push(value.trim());
        }
      });
      this.setQuery(key, existingValues);
    }
    return this;
  }
  /**
   * Sets a query parameter to the URL. The specified values will replace the existing values associated to this key. Empty keys and values will be discarded.
   * @param key The key of the parameter.
   * @param values The value or the values of the parameter.
   * @returns The instance of the builder.
   */
  setQuery(key: string, values: string | string[]): IUrlBuilder {
    if (typeof values === "string") {
      return this.setQuery(key, [values]);
    }
    if (!isNullOrWhiteSpace(key)) {
      key = key.trim();
      const newValues: string[] = [];
      values.forEach((value) => {
        if (!isNullOrWhiteSpace(value)) {
          newValues.push(value.trim());
        }
      });
      if (newValues.length > 0) {
        this.query.set(key, newValues);
      } else {
        this.query.delete(key);
      }
    }
    return this;
  }
  /**
   * Sets the query string of the URL. Empty keys and values will be discarded.
   * @param queryString The query string of the URL.
   * @returns The instance of the builder.
   */
  setQueryString(queryString?: string): IUrlBuilder {
    queryString = cleanTrim(trimStart(queryString?.trim() ?? "", "?"));
    this.query.clear();
    if (typeof queryString === "string") {
      const parameters: string[] = queryString.split("&");
      parameters.forEach((parameter) => {
        const index: number = parameter.indexOf("=");
        if (index >= 0) {
          this.addQuery(parameter.substring(0, index), parameter.substring(index + 1));
        }
      });
    }
    return this;
  }

  private fragment?: string;
  /**
   * Returns the fragment of the URL.
   * @returns The fragment of the URL.
   */
  getFragment(): string | undefined {
    return this.fragment;
  }
  /**
   * Sets the fragment of the URL.
   * @param fragment The fragment of the URL.
   * @returns The instance of the builder.
   */
  setFragment(fragment?: string): IUrlBuilder {
    fragment = cleanTrim(trimStart(fragment?.trim() ?? "", "#"));
    this.fragment = typeof fragment !== "string" ? undefined : `#${fragment}`;
    return this;
  }

  private parameters: Map<string, string> = new Map<string, string>();
  /**
   * Returns the parameters of the URL. Parameters are tokens that replace values in built URLs.
   * @returns The parameters of the URL.
   */
  getParameters(): Map<string, string> {
    return new Map<string, string>(this.parameters);
  }
  /**
   * Sets a parameter of the URL. Parameters are tokens that replace values in built URLs.
   * @param key The key of the parameter.
   * @param value The value of the parameter.
   * @returns The instance of the builder.
   * @throws {Error} The key is empty.
   */
  setParameter(key: string, value?: string): IUrlBuilder {
    if (isNullOrWhiteSpace(key)) {
      throw new Error("The parameter key is required.");
    }
    key = key.trim();
    if (isNullOrWhiteSpace(value)) {
      this.parameters.delete(key);
    } else if (value) {
      this.parameters.set(key, value.trim());
    }
    return this;
  }

  /**
   * Creates a new instance of the `UrlBuilder` class.
   * @param options The initialization options.
   */
  constructor(options?: UrlOptions) {
    options = options ?? {};
    if (options.scheme) {
      this.setScheme(options.scheme, true);
    }
    if (options.host) {
      this.setHost(options.host);
    }
    if (options.port) {
      this.setPort(options.port);
    }
    if (options.path) {
      this.setPath(options.path);
    }
    if (options.queryString) {
      this.setQueryString(options.queryString);
    }
    if (options.fragment) {
      this.setFragment(options.fragment);
    }
    if (options.credentials) {
      this.setCredentials(options.credentials);
    }
  }

  /**
   * Builds an URL of the specified kind.
   * @param kind The URL kind (defaults to `Absolute`).
   * @returns The built URL.
   */
  build(kind: UriKind = "Absolute"): string {
    let url: string = "";
    if (kind === "Absolute") {
      url += `${this.scheme}://${this.getAuthority()}`;
    }
    const path: string | undefined = this.getPath();
    if (typeof path === "string") {
      url += path;
    }
    const queryString: string | undefined = this.getQueryString();
    if (typeof queryString === "string") {
      url += queryString;
    }
    const fragment: string | undefined = this.getFragment();
    if (typeof fragment === "string") {
      url += fragment;
    }
    this.parameters.forEach((value, key) => {
      const pattern: string = `\\{${key}\\}`;
      url = url.replace(new RegExp(pattern, "g"), value);
    });
    return url;
  }
  /**
   * Builds an absolute URL.
   * @returns The built URL.
   */
  buildAbsolute(): string {
    return this.build("Absolute");
  }
  /**
   * Builds a relative URL.
   * @returns The built URL.
   */
  buildRelative(): string {
    return this.build("Relative");
  }

  private static inferPort(scheme: string): number {
    switch (scheme.trim().toLowerCase()) {
      case "https":
        return 443;
      default:
        return 80;
    }
  }
}

/**
 * The initialization options of the `UrlBuilder` class.
 */
export type UrlOptions = {
  /**
   * The scheme of the URL.
   */
  scheme?: string;
  /**
   * The host of the URL.
   */
  host?: string;
  /**
   * The port of the URL.
   */
  port?: number;
  /**
   * The path of the URL.
   */
  path?: string;
  /**
   * The query string of the URL.
   */
  queryString?: string;
  /**
   * The fragment of the URL.
   */
  fragment?: string;
  /**
   * The credentials of the URL.
   */
  credentials?: ICredentials;
};

// TODO(fpion): use logitar-js
