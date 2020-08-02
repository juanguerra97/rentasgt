import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginatorNavComponent } from './paginator-nav/paginator-nav.component';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal.component';

@NgModule({
  declarations: [
    PaginatorNavComponent,
    ConfirmationModalComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    PaginatorNavComponent,
    ConfirmationModalComponent,
  ]
})
export class AppCommonModule { }
