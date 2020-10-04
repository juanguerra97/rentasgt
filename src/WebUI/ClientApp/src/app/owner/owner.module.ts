import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppCommonModule } from '../app-common/app-common.module';
import { ProductsComponent } from './products/products.component';
import { NewProductComponent } from './new-product/new-product.component';
import { DetailProductComponent } from './detail-product/detail-product.component';
import { EditProductComponent } from './edit-product/edit-product.component';
import { RentRequestsComponent } from './rent-requests/rent-requests.component';
import { RentsComponent } from './rents/rents.component';
import { EndRentComponent } from './end-rent/end-rent.component';

@NgModule({
  declarations: [
    ProductsComponent, NewProductComponent, DetailProductComponent,
    EditProductComponent, RentRequestsComponent, RentsComponent, EndRentComponent
  ],
  imports: [
    AppCommonModule,
    RouterModule.forChild([
      {
        path: '',
        redirectTo: 'productos',
        pathMatch: 'full',
      },
      {
        path: 'productos',
        component: ProductsComponent
      },
      {
        path: 'productos/nuevo',
        component: NewProductComponent,
      },
      {
        path: 'productos/detalle/:id',
        component: DetailProductComponent,
      },
      {
        path: 'productos/editar/:id',
        component: EditProductComponent,
      },
      {
        path: 'solicitudes',
        component: RentRequestsComponent,
      },
      {
        path: 'rentas',
        component: RentsComponent,
      }
    ]),
  ]
})
export class OwnerModule { }
