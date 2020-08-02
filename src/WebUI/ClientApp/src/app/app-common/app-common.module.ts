import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginatorNavComponent } from './paginator-nav/paginator-nav.component';

@NgModule({
  declarations: [
    PaginatorNavComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    PaginatorNavComponent
  ]
})
export class AppCommonModule { }
