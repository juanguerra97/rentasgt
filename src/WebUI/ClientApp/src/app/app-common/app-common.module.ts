import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginatorNavComponent } from './paginator-nav/paginator-nav.component';
import {
  ButtonModule, FileUploadModule, MultiSelectModule,
  OrderListModule, PaginatorModule, CarouselModule,
  InputTextModule, InputText,
} from 'primeng';
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
    MultiSelectModule,
    OrderListModule,
    PaginatorModule,
    CarouselModule,
    InputTextModule,
    ModalModule.forRoot(),
    AgmCoreModule.forRoot({
      apiKey: '<API_KEY>',
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
    MultiSelectModule,
    OrderListModule,
    PaginatorModule,
    CarouselModule,
    InputTextModule,
    SelectLocationComponent,
  ]
})
export class AppCommonModule { }
