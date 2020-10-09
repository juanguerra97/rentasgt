import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AppCommonModule } from '../app-common/app-common.module'
import { ApiAuthorizationModule } from '../../api-authorization/api-authorization.module';
import { CategoriesComponent } from './categories/categories.component';
import { NewCategoryComponent } from './categories/new-category/new-category.component';
import { EditCategoryComponent } from './categories/edit-category/edit-category.component';
import { ReportsComponent } from './reports/reports.component';

@NgModule({
  declarations: [CategoriesComponent, NewCategoryComponent, EditCategoryComponent, ReportsComponent],
  imports: [
    FontAwesomeModule,
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
      },
      {
        path: 'reportes',
        component: ReportsComponent
      }
    ]),
    AppCommonModule,
  ]
})
export class AdminModule { }
