import { Injectable } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    public oidcSecurityService: OidcSecurityService,
  ) { }

  public isAuthenticated(): Observable<boolean> {
    return this.oidcSecurityService.isAuthenticated$;
  }

  public isNotAuthenticated(): Observable<boolean> {
    return this.oidcSecurityService.isAuthenticated$.pipe(map(auth => !auth));
  }

  public user(): Observable<any> {
    return this.oidcSecurityService.userData$;
  }

  public isAdmin(): Observable<boolean> {
    return this.hasRole('Admin');
  }

  public isModerador(): Observable<boolean> {
    return this.hasRole('Moderador');
  }

  public hasRole(role: string): Observable<boolean> {
    return this.user().pipe(map(u => {
      return !!u && !!u.role && u.role.includes(role);
    }));
  }

}
