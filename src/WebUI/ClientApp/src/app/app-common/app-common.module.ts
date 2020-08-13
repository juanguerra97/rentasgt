import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginatorNavComponent } from './paginator-nav/paginator-nav.component';
import { ButtonModule } from 'primeng';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  declarations: [
    PaginatorNavComponent,
    ConfirmationModalComponent
  ],
  imports: [
    CommonModule,
    ButtonModule,
    ModalModule.forRoot()
  ],
  exports: [
    PaginatorNavComponent,
    ConfirmationModalComponent,
    CommonModule,
    ModalModule,
    ButtonModule
  ]
})
export class AppCommonModule { }
