import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AppCommonModule } from '../app-common/app-common.module'
import { ApiAuthorizationModule } from '../../api-authorization/api-authorization.module';
import { CategoriesComponent } from './categories/categories.component';
import { NewCategoryComponent } from './categories/new-category/new-category.component';
import { EditCategoryComponent } from './categories/edit-category/edit-category.component';

@NgModule({
  declarations: [CategoriesComponent, NewCategoryComponent, EditCategoryComponent],
  imports: [
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
    AppCommonModule,
  ]
})
export class AdminModule { }
