import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppCommonModule } from '../app-common/app-common.module';

@NgModule({
  declarations: [],
  imports: [
    AppCommonModule,
    RouterModule.forChild([
      {
        path: '',
        redirectTo: 'products',
        pathMatch: 'full',
      },
    ]),
  ]
})
export class OwnerModule { }
