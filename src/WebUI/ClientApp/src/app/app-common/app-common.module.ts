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
  RatingModule, SidebarModule
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
    SidebarModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    MatOptionModule,
    AngularCropperjsModule,
    ChartsModule,
    ModalModule.forRoot(),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyCir67E-b2FdkVB6nbJ1pEYON_llhZPO-A',
      libraries: ['places']
    })
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
    SidebarModule,
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
