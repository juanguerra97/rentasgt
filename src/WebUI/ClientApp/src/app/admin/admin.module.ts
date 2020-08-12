import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AppCommonModule } from '../app-common/app-common.module'
import { ApiAuthorizationModule } from '../../api-authorization/api-authorization.module';
import { CategoriesComponent } from './categories/categories.component';
import { NewCategoryComponent } from './categories/new-category/new-category.component';

@NgModule({
  declarations: [CategoriesComponent, NewCategoryComponent],
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
    ]),
    AppCommonModule
  ]
})
export class AdminModule { }
