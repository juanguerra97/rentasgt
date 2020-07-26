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
import {Observable} from 'rxjs';
import {ApplicationPaths, QueryParameterNames} from './api-authorization.constants';
import {tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OnlyAdminGuard implements CanActivate, CanLoad, CanActivateChild {
  constructor(private authorize: AuthorizeService, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.verifyAdminRole(state);
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.verifyAdminRole(state);
  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
    return this.authorize.isAdmin();
  }

  private verifyAdminRole(state: RouterStateSnapshot) {
    return this.authorize.isAuthenticated().pipe(tap(isAuthenticated => {
      this.handleAuthorization(isAuthenticated, state);
      this.authorize.isAdmin().pipe(tap(isAdmin => this.handleIsAdmin(isAdmin)));
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

  private handleIsAdmin(isAdmin: boolean) {
    if (!isAdmin) {
      // TODO: Redirect to other route
    }
  }

}
