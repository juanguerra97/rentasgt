import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  CanLoad, Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree
} from '@angular/router';
import {Injectable} from '@angular/core';
import {AuthorizeService} from './authorize.service';
import {Observable, of} from 'rxjs';
import {ApplicationPaths, QueryParameterNames} from './api-authorization.constants';
import {tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OnlyModeradorGuard implements CanActivate, CanLoad, CanActivateChild {
  constructor(private authorize: AuthorizeService, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.verifyModeradorRole(state);
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.verifyModeradorRole(state);
  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
    return this.authorize.isModerador();
  }

  private verifyModeradorRole(state: RouterStateSnapshot) {
    return this.authorize.isAuthenticated().pipe(tap(isAuthenticated => {
      this.handleAuthorization(isAuthenticated, state);
      return isAuthenticated && this.authorize.isModerador().pipe(tap(isModerador => this.handleIsModerador(isModerador)));
    }));
  }

  private handleAuthorization(isAuthenticated: boolean, state: RouterStateSnapshot) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents, {
        queryParams: {
          [QueryParameterNames.ReturnUrl]: state.url
        }
      });
    }
  }

  private handleIsModerador(isModerador: boolean) {
    if (!isModerador) {
      // TODO: Redirect to other route
    }
    return isModerador;
  }

}
