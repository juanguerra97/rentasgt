import { Component } from '@angular/core';
import {AuthorizeService} from '../../api-authorization/authorize.service';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-home',
  styleUrls: ['home.component.css'],
  templateUrl: './home.component.html',
})
export class HomeComponent {

  public isAuthenticated: Observable<boolean>;
  public serchText: string = '';

  constructor(
    private authorize: AuthorizeService
  ) {
    this.isAuthenticated = authorize.isAuthenticated();
  }

}
