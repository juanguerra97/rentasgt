import { NgModule } from '@angular/core';
import { UsersValidationListComponent } from './users-validation-list/users-validation-list.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { RouterModule, Routes } from '@angular/router';
import { ConflictsComponent } from './conflicts/conflicts.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/usuarios' },
  { path: 'usuarios', component: UsersValidationListComponent },
  { path: 'conflictos', component: ConflictsComponent, }
];

@NgModule({
  declarations: [UsersValidationListComponent, ConflictsComponent],
  imports: [
    AppCommonModule,
    RouterModule.forChild(routes)
  ]
})
export class ModeradorModule { }
