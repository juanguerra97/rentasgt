import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ApiAuthorizationModule } from '../../api-authorization/api-authorization.module';
import { CategoriesComponent } from './categories/categories.component';

@NgModule({
  declarations: [CategoriesComponent],
  imports: [
    CommonModule,
    FontAwesomeModule,
    FormsModule,
    ReactiveFormsModule,
    ApiAuthorizationModule,
    RouterModule.forChild([
      {
        path: '',
        redirectTo: 'categorias', // TODO: redirect to admin home
        pathMatch: 'full',
      },
      {
        path: 'categorias',
        component: CategoriesComponent
      }
    ])
  ]
})
export class AdminModule { }
