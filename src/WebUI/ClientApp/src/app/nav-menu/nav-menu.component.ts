import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;

  public submenuTarget: any = null;

  public isAuthenticated: Observable<boolean>;
  public isAdmin: Observable<boolean>;
  public isModerador: Observable<boolean>;
  public email: Observable<string>;
  public searchText = '';
  public sidebarExpanded: boolean = false;

  constructor(
    public authService: AuthService,
    public oidcSecurityService: OidcSecurityService,
    public router: Router,
  ) {
  }

  ngOnInit(): void {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.isAdmin = this.authService.isAdmin();
    this.isModerador = this.authService.isModerador();
    this.email = this.authService.user().pipe(map(u => u && u.email));
    window.addEventListener('click', (e) => {
      if (this.submenuTarget && this.submenuTarget.lastChild && e.target !== this.submenuTarget.lastChild) {
        this.submenuTarget.lastChild.classList.remove('show');
        this.submenuTarget.classList.remove('nav-list-item-active');
        this.submenuTarget = null;
      }
    });
    window.addEventListener('resize', (e => {
      if (this.submenuTarget !== null) {
        this.placeSubmenu(this.submenuTarget);
      }
    }));
  }

  private placeSubmenu(li): void {
    const submenu = li.lastChild;
    if (window.innerWidth < 768) {
      submenu.style.width =  `${window.innerWidth}px`;

      submenu.style.right = `${-(window.innerWidth - (li.offsetLeft + li.offsetWidth))}px`;
    } else {
      submenu.style.width = `230px`;
      submenu.style.right = `${this.calculateCenteredRightPosition(li, submenu)}px`;
    }
  }

  public toggleSidebar(): void {
    this.sidebarExpanded = !this.sidebarExpanded;
  }

  public async logIn(): Promise<any> {
    // await this.authService.signIn();
    this.oidcSecurityService.authorize();
  }

  public async logOut(): Promise<any> {
    // this.oidcSecurityService.logoff();
    this.oidcSecurityService.logoffLocal();
    (<any>navigator).showToast("Has cerrado sesión");
    this.router.navigate(['/articulos']);
    // this.oidcSecurityService.logoffAndRevokeTokens().subscribe(console.log, console.error);
  }

  private calculateCenteredRightPosition(li: HTMLElement, ul: HTMLElement): number {
    return (li.offsetWidth - ul.offsetWidth) / 2;
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  public onSubmenuClicked($event): void {

    let menuBtn = $event.target;
    if ($event.target.nodeName === 'A') {
      menuBtn = $event.target.parentNode;
    }

    if (this.submenuTarget !== null) {
      this.submenuTarget.lastChild.classList.remove('show');
      this.submenuTarget.classList.remove('nav-list-item-active');
    }

    if (menuBtn !== this.submenuTarget) {
      this.submenuTarget = menuBtn;
      this.submenuTarget.lastChild.classList.add('show');
      this.submenuTarget.classList.add('nav-list-item-active');
      this.placeSubmenu(this.submenuTarget);
    } else {
      this.submenuTarget = null;
    }

    $event.stopPropagation();

  }
}
