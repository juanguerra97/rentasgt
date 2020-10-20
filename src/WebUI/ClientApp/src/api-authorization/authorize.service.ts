import { Injectable } from '@angular/core';
import { User, UserManager } from 'oidc-client';
import { BehaviorSubject, concat, from, Observable } from 'rxjs';
import { filter, map, mergeMap, take, tap } from 'rxjs/operators';

export type IAuthenticationResult =
  SuccessAuthenticationResult |
  FailureAuthenticationResult |
  RedirectAuthenticationResult;

export interface SuccessAuthenticationResult {
  status: AuthenticationResultStatus.Success;
  state: any;
}

export interface FailureAuthenticationResult {
  status: AuthenticationResultStatus.Fail;
  message: string;
}

export interface RedirectAuthenticationResult {
  status: AuthenticationResultStatus.Redirect;
}

export enum AuthenticationResultStatus {
  Success,
  Redirect,
  Fail
}

export interface IUser {
  sub: string;
  userId: string;
  email: string;
  firstName: string;
  lastName: string,
  profileStatus: number | string;
  name: string;
  role: string | string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {
  // By default pop ups are disabled because they don't work properly on Edge.
  // If you want to enable pop up authentication simply set this flag to false.

  private popUpDisabled = true;
  private userManager: UserManager;
  private userSubject: BehaviorSubject<IUser | null> = new BehaviorSubject(null);
  private user: IUser = null;

  public loggedIn(): boolean {
    return this.user != null;
  }

  public loadUser(): void {
    this.getUser().subscribe(u => {
      this.user = u;
    }, console.error);
  }

  public isAuthenticated(): Observable<boolean> {
    return this.getUser().pipe(map(u => !!u));
  }

  public getUser(): Observable<IUser | null> {
    return concat(
      this.userSubject.pipe(take(1), filter(u => !!u)),
      this.getUserFromStorage().pipe(filter(u => !!u), tap(u => this.userSubject.next(u))),
      this.userSubject.asObservable());
  }

  public getAccessToken(): Observable<string> {
    return from(this.ensureUserManagerInitialized())
      .pipe(mergeMap(() => from(this.userManager.getUser())),
        map(user => user && user.access_token));
  }

  public isAdmin(): Observable<boolean> {
    return this.hasRole('Admin');
  }

  public isModerador(): Observable<boolean> {
    return this.hasRole('Moderador');
  }

  public hasRole(role: string): Observable<boolean> {
    return this.getUser().pipe(map(u => {
      if (u == null || !u.role) {
        return false;
      }
      return u.role.includes(role);
    }));
  }
  
  public async signIn(state?: any): Promise<void> {
    await this.ensureUserManagerInitialized();
    await this.userManager.signinRedirect(this.createArguments(state));
  }

  public async completeSignIn(url: string): Promise<void> {
      await this.ensureUserManagerInitialized();
      const user = await this.userManager.signinRedirectCallback(url);
      this.user = user.profile;
      this.userSubject.next(user && user.profile);
  }

  public async signOut(state?: any): Promise<IAuthenticationResult> {
    try {
      if (this.popUpDisabled) {
        throw new Error('Popup disabled. Change \'authorize.service.ts:AuthorizeService.popupDisabled\' to false to enable it.');
      }

      await this.ensureUserManagerInitialized();
      await this.userManager.signoutPopup(this.createArguments());
      this.userSubject.next(null);
      return this.success(state);
    } catch (popupSignOutError) {
      console.log('Popup signout error: ', popupSignOutError);
      try {
        await this.userManager.signoutRedirect(this.createArguments(state));
        return this.redirect();
      } catch (redirectSignOutError) {
        console.log('Redirect signout error: ', popupSignOutError);
        return this.error(redirectSignOutError);
      }
    }
  }

  public async completeSignOut(url: string): Promise<IAuthenticationResult> {
    await this.ensureUserManagerInitialized();
    try {
      const state = await this.userManager.signoutCallback(url);
      this.userSubject.next(null);
      return this.success(state && state.data);
    } catch (error) {
      console.log(`There was an error trying to log out '${error}'.`);
      return this.error(error);
    }
  }

  private createArguments(state?: any): any {
    return { useReplaceToNavigate: true, data: state };
  }

  private error(message: string): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Fail, message };
  }

  private success(state: any): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Success, state };
  }

  private redirect(): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Redirect };
  }

  private async ensureUserManagerInitialized(): Promise<void> {
    if (this.userManager !== undefined) {
      return;
    }

    // const response = await fetch(ApplicationPaths.ApiAuthorizationClientConfigurationUrl);
    // if (!response.ok) {
    //   throw new Error(`Could not load settings for '${ApplicationName}'`);
    // }

    // const settings: any = await response.json();
    // settings.automaticSilentRenew = true;
    // settings.includeIdTokenInSilentRenew = true;

    this.userManager = new UserManager({
      authority: 'https://rentasguatemala.com',
      client_id: 'rentasgt.MobileApp',
      scope: 'openid profile rentasgt.WebUIAPI',
      redirect_uri: 'rentasgt://callback',
      response_type: 'code',
      response_mode: 'query',
      post_logout_redirect_uri: '/articulos',
      automaticSilentRenew: true,
      includeIdTokenInSilentRenew: true
    });

    this.userManager.events.addUserSignedOut(async () => {
      await this.userManager.removeUser();
      this.userSubject.next(null);
    });
  }

  private getUserFromStorage(): Observable<IUser> {
    return from(this.ensureUserManagerInitialized())
      .pipe(
        mergeMap(() => this.userManager.getUser()),
        map(u => u && u.profile));
  }
}
