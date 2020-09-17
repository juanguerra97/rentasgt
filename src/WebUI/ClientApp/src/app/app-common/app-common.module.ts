import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginatorNavComponent } from './paginator-nav/paginator-nav.component';
import {
  ButtonModule, FileUploadModule, MultiSelectModule,
  OrderListModule, PaginatorModule, CarouselModule,
  InputTextModule, DropdownModule, GalleriaModule,
  DialogModule, CalendarModule, SliderModule,
  MessageModule, MessagesModule, MessageService,
  TableModule, ProgressBarModule, ProgressSpinnerModule,
} from 'primeng';
import { MatDialogModule } from '@angular/material/dialog';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ImgCropperComponent } from './img-cropper/img-cropper.component';
import { SelectLocationComponent } from './select-location/select-location.component';
import { AgmCoreModule } from '@agm/core';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { AngularCropperjsModule } from 'angular-cropperjs';

@NgModule({
  declarations: [
    PaginatorNavComponent,
    ConfirmationModalComponent,
    ImgCropperComponent,
    SelectLocationComponent,
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
    DropdownModule,
    GalleriaModule,
    DialogModule,
    CalendarModule,
    SliderModule,
    MessageModule,
    MessagesModule,
    TableModule,
    ProgressBarModule,
    ProgressSpinnerModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    AngularCropperjsModule,
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
    DropdownModule,
    GalleriaModule,
    DialogModule,
    CalendarModule,
    SliderModule,
    MessageModule,
    MessagesModule,
    TableModule,
    ProgressSpinnerModule,
    ProgressBarModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    AngularCropperjsModule,
    SelectLocationComponent,
  ],
  providers: [MessageService]
})
export class AppCommonModule { }
