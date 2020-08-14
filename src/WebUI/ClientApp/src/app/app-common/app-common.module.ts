import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginatorNavComponent } from './paginator-nav/paginator-nav.component';
import { ButtonModule, FileUploadModule } from 'primeng';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ImgCropperComponent } from './img-cropper/img-cropper.component';
import { SelectLocationComponent } from './select-location/select-location.component';
import { AgmCoreModule } from '@agm/core';

@NgModule({
  declarations: [
    PaginatorNavComponent,
    ConfirmationModalComponent,
    ImgCropperComponent,
    SelectLocationComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    FileUploadModule,
    ModalModule.forRoot(),
    AgmCoreModule.forRoot({
      apiKey: '<SECRET_API_KEY>',
      libraries: ['places']
    })
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
    SelectLocationComponent,
  ]
})
export class AppCommonModule { }
