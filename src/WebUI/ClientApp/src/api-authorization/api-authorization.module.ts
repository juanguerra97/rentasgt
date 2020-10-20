import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { AppCommonModule } from '../app/app-common/app-common.module';

@NgModule({
  imports: [
    AppCommonModule,
    HttpClientModule,
    RouterModule.forChild(
      []
    )
  ],
  declarations: [],
  exports: []
})
export class ApiAuthorizationModule { }
