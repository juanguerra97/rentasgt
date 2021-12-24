import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginatorNavComponent } from './paginator-nav/paginator-nav.component';

import { ButtonModule } from 'primeng/button';
import { FileUploadModule } from 'primeng/fileupload';
import { MultiSelectModule } from 'primeng/multiselect';
import { OrderListModule } from 'primeng/orderlist';
import { PaginatorModule } from 'primeng/paginator';
import { CarouselModule } from 'primeng/carousel';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { GalleriaModule } from 'primeng/galleria';
import { DialogModule } from 'primeng/dialog';
import { CalendarModule } from 'primeng/calendar';
import { SliderModule } from 'primeng/slider';
import { MessageModule } from 'primeng/message';
import { TableModule } from 'primeng/table';
import { ProgressBarModule } from 'primeng/progressbar';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { RatingModule } from 'primeng/rating';
import { MessagesModule } from 'primeng/messages';
import { MessageService } from 'primeng/api';

import { MatDialogModule } from '@angular/material/dialog';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ImgCropperComponent } from './img-cropper/img-cropper.component';
import { SelectLocationComponent } from './select-location/select-location.component';
import { GoogleMapsModule } from '@angular/google-maps'
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { AngularCropperjsModule } from 'angular-cropperjs';
import { CreateConflictComponent } from './create-conflict/create-conflict.component';
import { MatOptionModule } from '@angular/material/core';
import { RateProductComponent } from './rate-product/rate-product.component';
import { ChartsModule } from 'ng2-charts';

@NgModule({
  declarations: [
    PaginatorNavComponent,
    ConfirmationModalComponent,
    ImgCropperComponent,
    SelectLocationComponent,
    CreateConflictComponent,
    RateProductComponent,
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
    RatingModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    MatOptionModule,
    AngularCropperjsModule,
    ChartsModule,
    ModalModule.forRoot(),
    GoogleMapsModule
  ],
  exports: [
    PaginatorNavComponent,
    ConfirmationModalComponent,
    ImgCropperComponent,
    CreateConflictComponent,
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
    RatingModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    MatOptionModule,
    AngularCropperjsModule,
    SelectLocationComponent,
    ChartsModule,
  ],
  providers: [MessageService]
})
export class AppCommonModule { }
