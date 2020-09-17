import { NgModule } from '@angular/core';
import { UsersValidationListComponent } from './users-validation-list/users-validation-list.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/usuarios' },
  { path: 'usuarios', component: UsersValidationListComponent }
];

@NgModule({
  declarations: [UsersValidationListComponent],
  imports: [
    AppCommonModule,
    RouterModule.forChild(routes)
  ]
})
export class ModeradorModule { }
