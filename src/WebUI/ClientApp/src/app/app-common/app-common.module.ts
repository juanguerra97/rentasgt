import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginatorNavComponent } from './paginator-nav/paginator-nav.component';
import { ButtonModule, FileUploadModule } from 'primeng';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ImgCropperComponent } from './img-cropper/img-cropper.component';

@NgModule({
  declarations: [
    PaginatorNavComponent,
    ConfirmationModalComponent,
    ImgCropperComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    FileUploadModule,
    ModalModule.forRoot(),
  ],
  exports: [
    PaginatorNavComponent,
    ConfirmationModalComponent,
    ImgCropperComponent,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule,
    ButtonModule,
    FileUploadModule,
  ]
})
export class AppCommonModule { }
