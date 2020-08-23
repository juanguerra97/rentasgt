import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

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
  public userName: Observable<string>;
  public searchText = '';

  constructor(
    private authorizeService: AuthorizeService,
  ) {
  }

  ngOnInit(): void {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.isAdmin = this.authorizeService.isAdmin();
    this.isModerador = this.authorizeService.isModerador();
    this.userName = this.authorizeService.getUser().pipe(map(u => u && u.name));
    window.addEventListener('click', (e) => {
      if (this.submenuTarget !== null && e.target !== this.submenuTarget.lastChild) {
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
      console.log(li.offsetLeft);
      console.log(li.offsetWidth);

      submenu.style.right = `${-(window.innerWidth - (li.offsetLeft + li.offsetWidth))}px`;
    } else {
      submenu.style.width = `230px`;
      submenu.style.right = `${this.calculateCenteredRightPosition(li, submenu)}px`;
    }
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
